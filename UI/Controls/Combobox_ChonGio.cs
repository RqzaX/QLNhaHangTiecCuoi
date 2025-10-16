using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    // Styled TimePicker with rounded popup list
    [SupportedOSPlatform("windows")]
    public class TimePickerExStyled : UserControl
    {
        private Label lblText;
        private PictureBox picArrow;
        private PopupListForm popup;
        private TimeSpan? _selected;
        private List<TimeSpan> items = new List<TimeSpan>();

        [Category("Appearance")] public string Placeholder { get; set; } = "Chọn giờ";
        [Category("Behavior")] public TimeSpan StartTime { get; set; } = new TimeSpan(10, 0, 0);
        [Category("Behavior")] public TimeSpan EndTime { get; set; } = new TimeSpan(22, 0, 0);
        [Category("Behavior")] public int IntervalMinutes { get; set; } = 30;

        [Browsable(false)]
        public TimeSpan? SelectedTime
        {
            get => _selected;
            set
            {
                _selected = value;
                UpdateText();
                TimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler TimeChanged;

        public TimePickerExStyled()
        {
            InitializeComponents();
            BuildItems();
            UpdateText();
        }

        private void InitializeComponents()
        {
            this.Height = 38;
            this.MinimumSize = new Size(120, 34);
            this.BackColor = Color.Transparent;

            var outer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 6, 6, 6),
                BackColor = Color.White
            };
            outer.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, outer.Width - 1, outer.Height - 1);
                using (var path = RoundedRect(r, 10))
                using (var br = new SolidBrush(outer.BackColor))
                using (var p = new Pen(Color.FromArgb(200, 200, 200)))
                {
                    e.Graphics.FillPath(br, path);
                    e.Graphics.DrawPath(p, path);
                }
            };

            lblText = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Font = new Font("Segoe UI", 9.25F),
                ForeColor = Color.Gray,
                Cursor = Cursors.Hand
            };
            lblText.Click += (s, e) => TogglePopup();
            outer.Controls.Add(lblText);

            picArrow = new PictureBox
            {
                Dock = DockStyle.Right,
                Width = 28,
                Cursor = Cursors.Hand,
                SizeMode = PictureBoxSizeMode.CenterImage,
                Image = DrawChevron(12, Color.FromArgb(110, 110, 110))
            };
            picArrow.Click += (s, e) => TogglePopup();
            outer.Controls.Add(picArrow);

            this.Controls.Add(outer);

            // keyboard
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Down) ShowPopup();
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) SelectedTime = null;
            };
        }

        private void BuildItems()
        {
            items.Clear();
            if (IntervalMinutes <= 0) IntervalMinutes = 30;
            var s = StartTime;
            var e = EndTime;
            if (e < s) e = s;
            for (var t = s; t <= e; t = t.Add(TimeSpan.FromMinutes(IntervalMinutes)))
                items.Add(t);
        }

        private void TogglePopup()
        {
            if (popup != null && !popup.IsDisposed && popup.Visible)
                popup.Close();
            else
                ShowPopup();
        }

        private void ShowPopup()
        {
            BuildItems();
            popup = new PopupListForm(items, SelectedTime)
            {
                Owner = this.FindForm(),
                StartPosition = FormStartPosition.Manual
            };

            // position below control
            var screen = this.PointToScreen(new Point(0, this.Height));
            // adjust to keep inside screen horizontally if needed
            var scr = Screen.FromControl(this).WorkingArea;
            popup.Width = Math.Max(this.Width, 140);
            // popup max height ~ 10 items
            int itemH = popup.ItemHeight;
            int maxVisible = 10;
            popup.Height = Math.Min(Math.Max(5, items.Count) * itemH + popup.BottomChevronHeight, itemH * maxVisible + popup.BottomChevronHeight);

            // if not enough space below, show above
            if (screen.Y + popup.Height > scr.Bottom && screen.Y - popup.Height - this.Height > scr.Top)
            {
                // show above
                var ptAbove = this.PointToScreen(new Point(0, -popup.Height));
                popup.Location = new Point(ptAbove.X, ptAbove.Y);
            }
            else
            {
                popup.Location = screen;
            }

            popup.ItemSelected += (s, e) =>
            {
                SelectedTime = e;
                popup.Close();
            };

            popup.Show();
        }

        private void UpdateText()
        {
            if (SelectedTime.HasValue)
            {
                lblText.Text = SelectedTime.Value.ToString(@"hh\:mm");
                lblText.ForeColor = Color.Black;
            }
            else
            {
                lblText.Text = Placeholder;
                lblText.ForeColor = Color.Gray;
            }
        }

        // small chevron image
        private static Bitmap DrawChevron(int size, Color c)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(c, 1.6f))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.DrawLines(pen, new PointF[] { new PointF(2, 5), new PointF(size / 2f, size - 3), new PointF(size - 2, 5) });
            }
            return bmp;
        }

        // rounded rectangle path
        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // usage helper in code: SetTime("10:30")
        public void SetTime(string hhmm)
        {
            if (TimeSpan.TryParse(hhmm, out var ts)) SelectedTime = ts;
        }
    }

    // Popup top-level form with owner-drawn list (rounded, shadow)
    [SupportedOSPlatform("windows")]
    internal class PopupListForm : Form
    {
        private readonly List<TimeSpan> _items;
        private readonly ListBox _list;
        private readonly PictureBox _chev;
        public int ItemHeight => 34;
        public int BottomChevronHeight => 14;

        public event Action<object, TimeSpan> ItemSelected;

        public PopupListForm(List<TimeSpan> items, TimeSpan? selected)
        {
            _items = items ?? new List<TimeSpan>();
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            //BackColor = Color.Transparent;
            DoubleBuffered = true;

            // allow shadow (CS_DROPSHADOW)
            // note: CS_DROPSHADOW works on Windows > XP and when composition enabled
            // override CreateParams below adds the style

            // ListBox (owner draw)
            _list = new ListBox
            {
                BorderStyle = BorderStyle.None,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = ItemHeight,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(35, 35, 35),
                Location = new Point(8, 8),
                Width = 240,
                HorizontalScrollbar = false
            };
            _list.DrawItem += List_DrawItem;
            _list.MouseMove += List_MouseMove;
            _list.MouseLeave += (s, e) => { _hoverIndex = -1; _list.Invalidate(); };
            _list.Click += (s, e) => AcceptSelection();
            _list.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) AcceptSelection(); else if (e.KeyCode == Keys.Escape) this.Close(); };
            foreach (var t in _items) _list.Items.Add(t.ToString(@"hh\:mm"));
            this.Controls.Add(_list);

            // bottom chevron
            _chev = new PictureBox { Size = new Size(24, BottomChevronHeight), BackColor = Color.Transparent };
            this.Controls.Add(_chev);
            _chev.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(110, 110, 110), 1.5f))
                {
                    var cx = _chev.Width / 2f;
                    var cy = 6f;
                    g.DrawLines(pen, new[] { new PointF(4, 4), new PointF(cx, cy + 6), new PointF(_chev.Width - 4, 4) });
                }
            };

            // layout will be set by caller (Width/Height), fix controls
            this.Load += (s, e) => LayoutControls(selected);
            Deactivate += (s, e) => this.Close(); // close when clicking outside
        }

        // enable drop shadow (class style)
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x00020000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private int _hoverIndex = -1;
        private void List_MouseMove(object sender, MouseEventArgs e)
        {
            int idx = _list.IndexFromPoint(e.Location);
            if (idx != _hoverIndex)
            {
                _hoverIndex = idx;
                _list.Invalidate();
            }
        }

        private void LayoutControls(TimeSpan? selected)
        {
            int w = this.Width;
            int h = this.Height;
            _list.Width = Math.Max(120, w - 16);
            _list.Left = 8;
            _list.Top = 8;
            _list.Height = Math.Max(40, h - (8 + BottomChevronHeight + 4));
            _chev.Width = 24;
            _chev.Left = (w - _chev.Width) / 2;
            _chev.Top = _list.Bottom + 2;
            // rounded region
            using (var path = RoundedRect(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 12))
            {
                this.Region = new Region(path);
            }
            // preselect
            if (selected.HasValue)
            {
                string s = selected.Value.ToString(@"hh\:mm");
                int ix = _list.Items.IndexOf(s);
                if (ix >= 0) _list.SelectedIndex = ix;
            }
        }

        private void List_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0 || e.Index >= _list.Items.Count) return;

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            bool hovered = e.Index == _hoverIndex;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = e.Bounds;
            rect.Inflate(-6, -4);

            // background for item (round highlight for selected or hover)
            if (selected || hovered)
            {
                Color fill = selected ? Color.FromArgb(24, 103, 255) : Color.FromArgb(245, 246, 247);
                Color text = selected ? Color.White : Color.FromArgb(35, 35, 35);
                using (var br = new SolidBrush(fill))
                using (var gp = RoundedRect(rect, 8))
                {
                    g.FillPath(br, gp);
                }
                var txt = _list.Items[e.Index].ToString();
                using (var sf = new StringFormat { LineAlignment = StringAlignment.Center })
                using (var f = new Font("Segoe UI", 10F))
                {
                    g.DrawString(txt, f, new SolidBrush(text), rect.Left + 10, rect.Top + (rect.Height - f.Height) / 2);
                }
            }
            else
            {
                // normal item
                var txt = _list.Items[e.Index].ToString();
                using (var f = new Font("Segoe UI", 10F))
                using (var br = new SolidBrush(Color.FromArgb(40, 40, 40)))
                {
                    g.DrawString(txt, f, br, rect.Left + 10, rect.Top + (rect.Height - f.Height) / 2);
                }
            }
        }

        private void AcceptSelection()
        {
            if (_list.SelectedIndex >= 0)
            {
                var s = _list.SelectedItem.ToString();
                if (TimeSpan.TryParse(s, out var ts))
                {
                    ItemSelected?.Invoke(this, ts);
                }
            }
        }

        // helper rounded path
        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ensure popup shows without stealing parent activation
        public new void Show()
        {
            // set topmost so it appears over owner but not steal focus
            this.TopMost = true;
            base.Show();
            this.Activate();
            _list.Focus();
        }
    }
}
