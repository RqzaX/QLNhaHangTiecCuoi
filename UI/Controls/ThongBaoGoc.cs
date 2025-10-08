// InAppNotifierStacked.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System.ComponentModel;

[SupportedOSPlatform("windows")]
public static class ThongBaoGoc
{
    public static int MarginX = 16;      // cách mép phải form/panel
    public static int MarginY = 16;      // cách mép trên
    public static int GapExpanded = 10;  // khoảng cách dọc khi bung
    public static int OffsetCollapsed = 8; // khoảng chồng khi thu gọn
    public static int MaxWidth = 520;

    // Style xanh lá theo ảnh
    public static Color GreenBg = Color.FromArgb(9, 41, 30);
    public static Color GreenText = Color.FromArgb(74, 222, 128);
    public static Color GreenIconBg = Color.FromArgb(12, 84, 53);
    public static Color ShadowColor = Color.FromArgb(40, 0, 0, 0);

    // 1 host/container cho mỗi owner
    private static readonly Dictionary<Control, ToastContainer> _hosts = new();

    public static void ShowSuccess(Control owner, string message, bool autoHide = false, int durationMs = 2500)
    {
        if (owner == null) throw new ArgumentNullException(nameof(owner));
        var host = GetOrCreateHost(owner);
        host.AddToast(new ToastCard(message, autoHide, durationMs));
    }

    private static ToastContainer GetOrCreateHost(Control owner)
    {
        var top = owner;
        if (owner is not Form) top = owner.FindForm() ?? owner;

        if (!_hosts.TryGetValue(top, out var host) || host.IsDisposed)
        {
            host = new ToastContainer(top);
            _hosts[top] = host;
        }
        return host;
    }

    // ================= Container quản lý layout/animation =================
    private class ToastContainer : Panel
    {
        private readonly List<ToastCard> _toasts = new();
        private readonly Timer _anim = new() { Interval = 15 };
        private bool _expanded; // hover -> true

        public ToastContainer(Control owner)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            Parent = owner;
            Owner = owner;
            BackColor = Color.Transparent;
            AutoSize = false;
            Width = MaxWidth; // tạm thời
            Height = 1;
            Anchor = AnchorStyles.Top | AnchorStyles.Right;

            Owner.Controls.Add(this);
            BringToFront();

            Owner.Resize += (s, e) => RepositionToTopRight();

            _anim.Tick += (s, e) => Animate();
            _anim.Start();

            // Hover expand/collapse (cả container và các con)
            MouseEnter += (s, e) => { _expanded = true; };
            MouseLeave += (s, e) => { if (!ClientRectangle.Contains(PointToClient(Control.MousePosition))) _expanded = false; };
        }

        public Control Owner { get; }

        public void AddToast(ToastCard card)
        {
            card.Width = Math.Min(MaxWidth, card.Width);
            Controls.Add(card);
            _toasts.Insert(0, card); // mới nhất ở trên
            card.BringToFront();

            // Khởi tạo vị trí/alpha để slide-in
            card.CurY = -card.Height - 10;
            card.TargetY = 0;
            card.Alpha = 0f;

            // Close -> bỏ khỏi list và tiếp tục reflow
            card.Closed += (s, e) =>
            {
                Controls.Remove(card);
                _toasts.Remove(card);
                ReflowTargets();
            };

            // Forward hover từ card lên container để không bị collapse
            card.MouseEnter += (s, e) => _expanded = true;
            card.MouseLeave += (s, e) =>
            {
                if (!ClientRectangle.Contains(PointToClient(Control.MousePosition))) _expanded = false;
            };

            ReflowTargets();
            RepositionToTopRight();
        }

        private void RepositionToTopRight()
        {
            // cập nhật vị trí container theo kích thước thực của các toast
            int width = 0;
            foreach (var t in _toasts)
                width = Math.Max(width, t.Width);

            Width = Math.Min(MaxWidth, Math.Max(280, width));
            Left = Owner.ClientSize.Width - Width - MarginX;
            Top = MarginY;
        }

        private void ReflowTargets()
        {
            // Tính TargetY theo trạng thái expand/collapse
            int y = 0;
            for (int i = 0; i < _toasts.Count; i++)
            {
                var t = _toasts[i];
                t.TargetY = y;
                if (_expanded)
                    y += t.Height + GapExpanded;
                else
                    y += OffsetCollapsed; // chồng lên
            }
            // cập nhật Height container
            Height = (int)(_expanded
                ? (_toasts.Count == 0 ? 1 : (_toasts[_toasts.Count - 1].TargetY + _toasts[_toasts.Count - 1].Height))
                : (_toasts.Count == 0 ? 1 : (_toasts[0].Height + Math.Max(0, (_toasts.Count - 1) * OffsetCollapsed))));
        }

        private void Animate()
        {
            // Nếu mouse đang ở trong container => expand, ngược lại collapse
            bool inside = ClientRectangle.Contains(PointToClient(Control.MousePosition));
            _expanded = inside;

            // Lerp từng toast tới vị trí đích + fade-in
            float posLerp = 0.2f; // tốc độ trượt
            float alphaLerp = 0.2f;

            // Cập nhật targets theo trạng thái hiện tại
            ReflowTargets();

            foreach (var t in _toasts)
            {
                t.CurY = Lerp(t.CurY, t.TargetY, posLerp);
                t.Alpha = Lerp(t.Alpha, 1f, alphaLerp);

                int x = Width - t.Width; // canh phải
                t.Location = new Point(x, (int)t.CurY);
                t.Invalidate(); // để vẽ alpha
            }

            RepositionToTopRight();
        }

        private static float Lerp(float a, float b, float k) => a + (b - a) * k;
    }

    // ================= Toast item =================
    private class ToastCard : UserControl
    {
        public event EventHandler Closed;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float CurY { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float TargetY { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Alpha { get; set; } = 1f;

        private readonly Timer _life;
        private readonly bool _autoHide;
        private readonly string _message;

        private const int Corner = 12;
        private const int Shadow = 8;

        public ToastCard(string message, bool autoHide, int durationMs)
        {
            _message = message ?? "";
            _autoHide = autoHide;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;

            // Tính kích thước theo text
            var font = new Font("Segoe UI", 10f, FontStyle.Bold);
            var bounds = TextRenderer.MeasureText(_message, font, new Size(MaxWidth - 24 - 32 - 24, 0),
                                                  TextFormatFlags.WordBreak);
            int hContent = Math.Max(48, bounds.Height + 16);
            Height = hContent + 16; // + vùng shadow
            Width = 24 + 32 + 12 + bounds.Width + 24;

            // Nút close (tuỳ chọn – click vào thẻ cũng đóng)
            var btnX = new Button
            {
                FlatStyle = FlatStyle.Flat,
                Text = "✕",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(210, 255, 233),
                BackColor = Color.Transparent,
                TabStop = false,
                Size = new Size(26, 26),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Width - 26 - 14, (Height - 26) / 2)
            };
            btnX.FlatAppearance.BorderSize = 0;
            btnX.FlatAppearance.MouseOverBackColor = Color.FromArgb(25, 255, 255, 255);
            btnX.Click += (s, e) => Close();
            Controls.Add(btnX);

            // Auto-hide (nếu bật)
            _life = new Timer();
            if (_autoHide)
            {
                _life.Interval = Math.Max(800, durationMs);
                _life.Tick += (s, e) => Close();
                _life.Start();
            }

            // Click vào thẻ cũng đóng
            Click += (s, e) => Close();
            foreach (Control c in Controls) c.Click += (s, e) => Close();
        }

        public void Close()
        {
            _life.Stop();
            Closed?.Invoke(this, EventArgs.Empty);
            Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Áp alpha lên màu
            Color A(Color c, float a) => Color.FromArgb((int)(c.A * a), c);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1 - 8);

            // Shadow mềm
            for (int i = Shadow; i >= 1; i--)
            {
                using var pthS = RoundRect(new Rectangle(rect.X + 2, rect.Y + 2 + i, rect.Width, rect.Height), Corner + i);
                using var brS = new SolidBrush(Color.FromArgb((int)(35f * (i / (float)Shadow) * Alpha), ShadowColor));
                g.FillPath(brS, pthS);
            }

            // Nền card
            using (var path = RoundRect(rect, Corner))
            using (var br = new SolidBrush(A(GreenBg, Alpha)))
                g.FillPath(br, path);

            // Icon (tròn + check)
            int cx = rect.Left + 18;
            int cy = rect.Top + rect.Height / 2;
            int r = 11;
            using (var brIcon = new SolidBrush(A(GreenIconBg, Alpha)))
                g.FillEllipse(brIcon, cx - r, cy - r, r * 2, r * 2);

            using (var penCheck = new Pen(A(GreenText, Alpha), 2.3f))
            {
                penCheck.StartCap = LineCap.Round; penCheck.EndCap = LineCap.Round;
                using var gp = new GraphicsPath();
                gp.AddLines(new[]
                {
                    new Point(cx - 5, cy),
                    new Point(cx - 1, cy + 4),
                    new Point(cx + 6, cy - 5)
                });
                g.DrawPath(penCheck, gp);
            }

            // Text
            var textRect = new Rectangle(cx + r + 10, rect.Top, rect.Right - (cx + r + 10) - 40, rect.Height);
            TextRenderer.DrawText(g, _message, new Font("Segoe UI", 10f, FontStyle.Bold),
                                  textRect, A(GreenText, Alpha),
                                  TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
        }

        private static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(r); return path; }
            int d = radius * 2;
            var arc = new Rectangle(r.X, r.Y, d, d);
            path.AddArc(arc, 180, 90);
            arc.X = r.Right - d; path.AddArc(arc, 270, 90);
            arc.Y = r.Bottom - d; path.AddArc(arc, 0, 90);
            arc.X = r.X; path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
