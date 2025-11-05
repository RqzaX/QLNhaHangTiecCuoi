using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Timer = System.Windows.Forms.Timer;

namespace UI.Controls
{
    public enum ToastType { Success, Info, Error }
    public enum ToastPos { TopRight, TopLeft, BottomRight, BottomLeft }

    /// <summary>
    /// Borderless form dùng để có Opacity (fade) + stack nhiều toast
    /// </summary>
    public sealed class GunaToast : Form
    {
        private readonly Guna2ShadowPanel _card;
        private readonly Label _lblIcon;
        private readonly Label _lblText;

        private Timer _fadeIn, _life, _fadeOut;
        private const int CARD_H = 66;
        private const int CARD_W = 380;
        private const int MARGIN = 12;
        private const int GAP    = 10;

        // === Manager (stack & reflow) ===
        private static readonly Dictionary<Form, List<GunaToast>> _stacks = new();
        private static readonly object _lock = new();

        private readonly Form _owner;
        private readonly ToastPos _pos;

        private GunaToast(Form owner, string text, ToastType type, int ms, ToastPos pos)
        {
            _owner = owner;
            _pos = pos;

            // Base form
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            Opacity = 0; // start invisible
            BackColor = Color.Black; // WinForms doesn't support Transparent, use black instead
            TransparencyKey = Color.Black; // Make black color transparent
            Size = new Size(CARD_W, CARD_H);

            // Card (bo góc + shadow)
            _card = new Guna2ShadowPanel
            {
                Parent = this,
                Dock = DockStyle.Fill,
                Radius = 14,
                FillColor = GetFill(type),
                ShadowColor = Color.FromArgb(100, 0, 0, 0),
                ShadowDepth = 6,
                ShadowShift = 2,
                BackColor = Color.Transparent
            };

            // Icon (dùng glyph ✓/i/! để không cần resource)
            _lblIcon = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = GetFore(type),
                Size = new Size(30, 30),
                Location = new Point(14, 18),
                BackColor = Color.Transparent
            };
            _lblIcon.Text = type switch
            {
                ToastType.Success => "✓",
                ToastType.Info    => "ℹ",
                _                  => "!"
            };

            // Text
            _lblText = new Label
            {
                AutoSize = false,
                Font = new Font("Inter", 10, FontStyle.Bold),
                ForeColor = GetFore(type),
                Location = new Point(52, 14),
                Size = new Size(CARD_W - 70, 38),
                BackColor = Color.Transparent,
                Text = text
            };

            _card.Controls.Add(_lblIcon);
            _card.Controls.Add(_lblText);

            // Timers
            _fadeIn  = new Timer { Interval = 15 };
            _life    = new Timer { Interval = ms };
            _fadeOut = new Timer { Interval = 15 };

            _fadeIn.Tick += (_, __) => { Opacity += 0.08; if (Opacity >= 1) _fadeIn.Stop(); };
            _life.Tick   += (_, __) => { _life.Stop(); _fadeOut.Start(); };
            _fadeOut.Tick+= (_, __) => { Opacity -= 0.08; if (Opacity <= 0) { _fadeOut.Stop(); Close(); } };

            Shown += (_, __) => _fadeIn.Start();
            FormClosed += (_, __) =>
            {
                lock (_lock)
                {
                    if (_stacks.TryGetValue(_owner, out var list))
                    {
                        list.Remove(this);
                        Reflow(_owner, _pos, list);
                        if (list.Count == 0) _stacks.Remove(_owner);
                    }
                }
                DisposeTimers();
            };
        }

        private void DisposeTimers()
        {
            _fadeIn?.Dispose(); _life?.Dispose(); _fadeOut?.Dispose();
            _fadeIn = _life = _fadeOut = null;
        }

        private static Color GetFill(ToastType t) => t switch
        {
            ToastType.Success => Color.FromArgb(6, 64, 35),   // xanh đậm
            ToastType.Info    => Color.FromArgb(25, 48, 82),  // xanh navy
            _                 => Color.FromArgb(96, 21, 32)   // đỏ sậm
        };

        private static Color GetFore(ToastType t) => Color.White;

        private static Point CalcLocation(Form owner, ToastPos pos, int index, Size size)
        {
            if (owner == null || owner.IsDisposed)
            {
                // Fallback to screen if owner is invalid
                var scr = Screen.PrimaryScreen.WorkingArea;
                return new Point(scr.Right - size.Width - MARGIN, scr.Top + MARGIN + index * (size.Height + GAP));
            }

            int x = 0, y = 0;
            var ownerRect = owner.RectangleToScreen(owner.ClientRectangle);

            switch (pos)
            {
                case ToastPos.TopRight:
                    x = ownerRect.Right - size.Width - MARGIN;
                    y = ownerRect.Top + MARGIN + index * (size.Height + GAP);
                    break;
                case ToastPos.TopLeft:
                    x = ownerRect.Left + MARGIN;
                    y = ownerRect.Top + MARGIN + index * (size.Height + GAP);
                    break;
                case ToastPos.BottomRight:
                    x = ownerRect.Right - size.Width - MARGIN;
                    y = ownerRect.Bottom - size.Height - MARGIN - index * (size.Height + GAP);
                    break;
                case ToastPos.BottomLeft:
                    x = ownerRect.Left + MARGIN;
                    y = ownerRect.Bottom - size.Height - MARGIN - index * (size.Height + GAP);
                    break;
            }

            // Ensure toast stays on screen
            var screen = Screen.FromControl(owner);
            var screenRect = screen.WorkingArea;
            
            if (x + size.Width > screenRect.Right)
                x = screenRect.Right - size.Width - MARGIN;
            if (x < screenRect.Left)
                x = screenRect.Left + MARGIN;
            if (y + size.Height > screenRect.Bottom)
                y = screenRect.Bottom - size.Height - MARGIN;
            if (y < screenRect.Top)
                y = screenRect.Top + MARGIN;

            return new Point(x, y);
        }

        private static void Reflow(Form owner, ToastPos pos, List<GunaToast> list)
        {
            for (int i = 0; i < list.Count; i++)
                list[i].Location = CalcLocation(owner, pos, i, list[i].Size);
        }

        /// <summary>
        /// API public: gọi 1 dòng để show toast
        /// </summary>
        public static void Show(Form owner, string text,
                                ToastType type = ToastType.Success,
                                int durationMs = 2600,
                                ToastPos pos = ToastPos.TopRight)
        {
            if (owner == null || owner.IsDisposed) return;

            var toast = new GunaToast(owner, text, type, durationMs, pos);

            lock (_lock)
            {
                if (!_stacks.TryGetValue(owner, out var list))
                {
                    list = new List<GunaToast>();
                    _stacks[owner] = list;

                    // Reflow khi owner thay đổi kích thước/di chuyển
                    owner.ResizeEnd += (_, __) => Reflow(owner, pos, list);
                    owner.Move      += (_, __) => Reflow(owner, pos, list);
                }
                list.Insert(0, toast); // mới nhất trên cùng
                Reflow(owner, pos, list);
            }

            toast.Show();
            toast._life.Start();
        }
    }
}

