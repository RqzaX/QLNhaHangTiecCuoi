using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    // UserControl "vỏ" bo góc + ComboBox flat bên trong (owner draw)
    [DefaultEvent(nameof(SelectedIndexChanged))]
    public class ComboBox_Border : UserControl
    {
        private readonly ComboBox _combo;
        private bool _hover, _focus;

        // ==== Appearance ====
        private int _cornerRadius = 12;
        private Color _borderColor = Color.FromArgb(226, 232, 240);   // xám nhạt
        private Color _borderHover = Color.FromArgb(203, 213, 225);
        private Color _borderFocus = Color.FromArgb(99, 102, 241);    // indigo
        private Color _bg = Color.White;
        private string _placeholder = "";

        public ComboBox_Border()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Padding = new Padding(10, 6, 30, 6); // chừa chỗ cho mũi tên

            _combo = new ComboBox
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DrawMode = DrawMode.OwnerDrawFixed,
                IntegralHeight = false,
                ItemHeight = 30, // cao vừa giống hình
                TabStop = true
            };
            _combo.DrawItem += Combo_DrawItem;
            _combo.DropDown += (s, e) => Invalidate();
            _combo.SelectedIndexChanged += (s, e) => Invalidate();
            _combo.GotFocus += (s, e) => { _focus = true; Invalidate(); };
            _combo.LostFocus += (s, e) => { _focus = false; Invalidate(); };

            // loại bỏ border mặc định của ComboBox
            _combo.Region = new Region(new Rectangle(1, 1, Width - 2, Height - 2));
            _combo.MouseEnter += (s, e) => { _hover = true; Invalidate(); };
            _combo.MouseLeave += (s, e) => { _hover = false; Invalidate(); };

            Controls.Add(_combo);
            Size = new Size(200, 40);
            MinimumSize = new Size(80, 36);
        }

        // ===== Public API (proxy) =====
        [Browsable(true)]
        [Category("Appearance")]
        public new Font Font { get => base.Font; set { base.Font = value; _combo.Font = value; Invalidate(); } }

        [Category("Data")]
        public ComboBox.ObjectCollection Items => _combo.Items;

        [Category("Data")]
        public int SelectedIndex { get => _combo.SelectedIndex; set => _combo.SelectedIndex = value; }

        [Category("Data")]
        public object SelectedItem { get => _combo.SelectedItem; set => _combo.SelectedItem = value; }

        [Category("Behavior")]
        public event EventHandler SelectedIndexChanged
        {
            add { _combo.SelectedIndexChanged += value; }
            remove { _combo.SelectedIndexChanged -= value; }
        }

        [Category("Appearance")]
        public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(6, value); Invalidate(); } }

        [Category("Appearance")]
        public Color BorderColor { get => _borderColor; set { _borderColor = value; Invalidate(); } }

        [Category("Appearance")]
        public Color BorderHoverColor { get => _borderHover; set { _borderHover = value; Invalidate(); } }

        [Category("Appearance")]
        public Color BorderFocusColor { get => _borderFocus; set { _borderFocus = value; Invalidate(); } }

        [Category("Appearance")]
        public Color CardBackColor { get => _bg; set { _bg = value; Invalidate(); } }

        [Category("Appearance")]
        public string Placeholder { get => _placeholder; set { _placeholder = value ?? ""; Invalidate(); } }

        [Browsable(false)]
        public ComboBox InnerCombo => _combo; // nếu cần truy cập sâu

        public void SetItems(string[] items)
        {
            _combo.Items.Clear();
            if (items != null) _combo.Items.AddRange(items);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _combo.Region = new Region(new Rectangle(2, 2, Width - 4, Height - 4));
            Invalidate();
        }

        // ===== Paint "vỏ" bo góc + icon mũi tên =====
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            using (var path = RoundRect(ClientRectangle, _cornerRadius))
            using (var bg = new SolidBrush(_bg))
            {
                g.FillPath(bg, path);
            }

            // Border
            Color border = !_combo.Enabled ? _borderColor
                         : _focus ? _borderFocus
                         : _hover ? _borderHover
                         : _borderColor;

            using (var pen = new Pen(border, 1.6f))
            using (var path = RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), _cornerRadius))
                g.DrawPath(pen, path);

            // Placeholder (khi chưa chọn)
            if (_combo.SelectedIndex < 0 && !string.IsNullOrEmpty(_placeholder))
            {
                var rect = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal - 24, Height - Padding.Vertical);
                TextRenderer.DrawText(g, _placeholder, Font, rect,
                    Color.FromArgb(160, 160, 170), TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }

            // Icon mũi tên
            var cx = Width - 18; var cy = Height / 2;
            using (var p = new Pen(Color.FromArgb(120, 120, 130), 2f))
            {
                g.DrawLines(p, new[]
                {
                    new Point(cx-6, cy-2),
                    new Point(cx,   cy+4),
                    new Point(cx+6, cy-2)
                });
            }
        }

        // ===== Owner draw item (dropdown) =====
        private void Combo_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            bool focused = (e.State & DrawItemState.Focus) == DrawItemState.Focus;

            var bounds = e.Bounds;
            bounds.Inflate(-6, -2);

            // màu giống ảnh: selected có nền xám nhạt + viền mỏng
            if (selected)
            {
                using (var b = new SolidBrush(Color.FromArgb(245, 247, 250)))
                    g.FillRectangle(b, e.Bounds);

                using (var p = new Pen(Color.FromArgb(226, 232, 240)))
                    g.DrawRectangle(p, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            }

            // text
            string text = _combo.Items[e.Index]?.ToString() ?? "";
            var textRect = new Rectangle(bounds.X + 28, bounds.Y, bounds.Width - 28, bounds.Height);
            TextRenderer.DrawText(g, text, Font, textRect,
                Color.FromArgb(17, 24, 39), // gần #111827
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            // tick cho item đang được chọn
            if (_combo.SelectedIndex == e.Index)
            {
                DrawCheck(g, new Rectangle(bounds.X + 4, bounds.Y + (bounds.Height - 16) / 2, 16, 16));
            }

            // focus cue
            if (focused) e.DrawFocusRectangle();
        }

        private static void DrawCheck(Graphics g, Rectangle r)
        {
            using (var p = new Pen(Color.FromArgb(59, 130, 246), 2.2f)) // xanh dương
            {
                p.StartCap = LineCap.Round; p.EndCap = LineCap.Round;
                g.DrawLines(p, new[]
                {
                    new Point(r.Left+2, r.Top+8),
                    new Point(r.Left+7, r.Bottom-3),
                    new Point(r.Right-2, r.Top+3)
                });
            }
        }

        private static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var gp = new GraphicsPath();
            if (radius <= 0) { gp.AddRectangle(r); gp.CloseFigure(); return gp; }

            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        // loại bỏ focus rectangle xấu trên ComboBox (Windows message)
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            const int WM_PAINT = 0x000F;
            if (m.Msg == WM_PAINT) Invalidate(); // đồng bộ border khi nội dung đổi
        }
    }
}
