using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    [ToolboxItem(true)]
    public class RoundedTextBox : UserControl
    {
        private readonly TextBox _tb = new TextBox();
        private readonly Label _ph = new Label(); // placeholder
        private bool _hover, _focused;

        private int _cornerRadius = 10;
        private int _borderThickness = 1;

        private Color _borderColor = Color.FromArgb(209, 213, 219); // #D1D5DB
        private Color _borderHoverColor = Color.FromArgb(156, 163, 175); // #9CA3AF
        private Color _borderFocusColor = Color.FromArgb(31, 111, 235);  // #1F6FEB
        private Color _borderDisabledColor = Color.FromArgb(229, 231, 235); // #E5E7EB
        private Color _placeholderColor = Color.FromArgb(156, 163, 175); // #9CA3AF
        private string _placeholderText = "";

        public RoundedTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            // mặc định
            base.BackColor = Color.White;
            ForeColor = Color.Black;
            Font = new Font("Segoe UI", 10f);
            Padding = new Padding(10, 8, 10, 8);
            Height = Math.Max(32, Font.Height + Padding.Vertical + 2);

            // TextBox con
            _tb.BorderStyle = BorderStyle.None;
            _tb.BackColor = BackColor;
            _tb.ForeColor = ForeColor;
            _tb.Font = Font;
            _tb.Location = new Point(Padding.Left, Padding.Top);
            _tb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            _tb.TextChanged += (s, e) => { TogglePlaceholder(); OnTextChanged(e); };
            _tb.GotFocus += (s, e) => { _focused = true; Invalidate(); TogglePlaceholder(); };
            _tb.LostFocus += (s, e) => { _focused = false; Invalidate(); TogglePlaceholder(); };
            _tb.MouseEnter += (s, e) => { _hover = true; Invalidate(); };
            _tb.MouseLeave += (s, e) => { _hover = false; Invalidate(); };

            // Placeholder
            _ph.AutoSize = false;
            _ph.BackColor = Color.Transparent;
            _ph.ForeColor = _placeholderColor;
            _ph.Font = Font;
            _ph.Enabled = false; // không nhận focus
            _ph.TextAlign = ContentAlignment.MiddleLeft;
            _ph.MouseDown += (s, e) => _tb.Focus();

            Controls.Add(_tb);
            Controls.Add(_ph);

            SizeChanged += (s, e) => LayoutInner();
            BackColorChanged += (s, e) => { _tb.BackColor = BackColor; Invalidate(); };
            ForeColorChanged += (s, e) => { _tb.ForeColor = ForeColor; Invalidate(); };
            FontChanged += (s, e) => { _tb.Font = Font; _ph.Font = Font; LayoutInner(); };

            MouseEnter += (s, e) => { _hover = true; Invalidate(); };
            MouseLeave += (s, e) => { _hover = false; Invalidate(); };

            LayoutInner();
            TogglePlaceholder();
            UpdateRegion();
        }

        // ---------------- Properties ----------------
        [Category("Appearance"), DefaultValue(10)]
        public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(0, value); UpdateRegion(); Invalidate(); } }

        [Category("Appearance"), DefaultValue(1)]
        public int BorderThickness { get => _borderThickness; set { _borderThickness = Math.Max(1, value); Invalidate(); } }

        [Category("Appearance"), DefaultValue(typeof(Color), "209,213,219")]
        public Color BorderColor { get => _borderColor; set { _borderColor = value; Invalidate(); } }

        [Category("Appearance"), DefaultValue(typeof(Color), "156,163,175")]
        public Color BorderHoverColor { get => _borderHoverColor; set { _borderHoverColor = value; Invalidate(); } }

        [Category("Appearance"), DefaultValue(typeof(Color), "31,111,235")]
        public Color BorderFocusColor { get => _borderFocusColor; set { _borderFocusColor = value; Invalidate(); } }

        [Category("Appearance"), DefaultValue(typeof(Color), "229,231,235")]
        public Color BorderDisabledColor { get => _borderDisabledColor; set { _borderDisabledColor = value; Invalidate(); } }

        [Category("Appearance"), DefaultValue(typeof(Color), "156,163,175")]
        public Color PlaceholderColor { get => _placeholderColor; set { _placeholderColor = value; _ph.ForeColor = value; Invalidate(); } }

        [Category("Appearance"), DefaultValue(""), Description("Văn bản gợi ý khi ô trống.")]
        public string PlaceholderText { get => _placeholderText; set { _placeholderText = value; _ph.Text = value; TogglePlaceholder(); Invalidate(); } }

        [Category("Behavior"), DefaultValue(false)]
        public bool UseSystemPasswordChar { get => _tb.UseSystemPasswordChar; set => _tb.UseSystemPasswordChar = value; }

        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public char PasswordChar { get => _tb.PasswordChar; set => _tb.PasswordChar = value; }

        [Category("Behavior"), DefaultValue(false)]
        public bool Multiline { get => _tb.Multiline; set { _tb.Multiline = value; LayoutInner(); } }

        [Category("Behavior"), DefaultValue(32767)]
        public int MaxLength { get => _tb.MaxLength; set => _tb.MaxLength = value; }

        [Category("Appearance"), DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign { get => _tb.TextAlign; set { _tb.TextAlign = value; LayoutInner(); } }

        // expose Text/ReadOnly như TextBox
        [Browsable(true)]
        public override string Text { get => _tb.Text; set { _tb.Text = value; TogglePlaceholder(); } }

        [Category("Behavior"), DefaultValue(false)]
        public bool ReadOnly { get => _tb.ReadOnly; set => _tb.ReadOnly = value; }

        // ---------------- Layout / Painting ----------------
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = RoundRect(rect, _cornerRadius))
            using (var bg = new SolidBrush(BackColor))
            {
                g.FillPath(bg, path);
            }

            // chọn màu viền theo trạng thái
            Color use =
                !Enabled ? _borderDisabledColor :
                 _focused ? _borderFocusColor :
                   _hover ? _borderHoverColor : _borderColor;

            using (var path = RoundRect(rect, _cornerRadius))
            using (var pen = new Pen(use, _borderThickness))
            {
                g.DrawPath(pen, path);
            }

            base.OnPaint(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _tb.Enabled = Enabled;
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            _tb.Focus();
        }

        private void LayoutInner()
        {
            var inner = Rectangle.Inflate(ClientRectangle, -_borderThickness - 1, -_borderThickness - 1);
            var tbRect = new Rectangle(
                inner.X + Padding.Left,
                inner.Y + Padding.Top,
                Math.Max(10, inner.Width - Padding.Horizontal),
                Math.Max(10, inner.Height - Padding.Vertical)
            );
            _tb.Bounds = tbRect;
            _ph.Bounds = tbRect;
            _ph.TextAlign = Multiline ? ContentAlignment.TopLeft : ContentAlignment.MiddleLeft;
            UpdateRegion();
            Invalidate();
        }

        private void TogglePlaceholder()
        {
            _ph.Text = _placeholderText;
            _ph.ForeColor = _placeholderColor;
            _ph.Visible = string.IsNullOrEmpty(_tb.Text) && !_tb.Focused && !DesignMode && !UseSystemPasswordChar;
        }

        private void UpdateRegion()
        {
            using var gp = RoundRect(new Rectangle(0, 0, Width, Height), _cornerRadius);
            Region = new Region(gp);
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
