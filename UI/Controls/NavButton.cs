using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UiControls
{
    [SupportedOSPlatform("windows")]
    [ToolboxItem(true)]
    public class NavButton : Control
    {
        // ---- State ----
        private bool _hover;
        private bool _pressed;
        private bool _isSelected;

        // ---- Appearance ----
        private Image _iconImage;
        private Size _iconSize = new Size(18, 18);
        private int _cornerRadius = 12;
        private int _paddingX = 14;
        private int _gap = 10;

        private Color _textColor = Color.FromArgb(31, 41, 55);        // #1F2937
        private Color _textColorSelected = Color.FromArgb(17, 24, 39); // #111827
        private Color _textColorDisabled = Color.FromArgb(156, 163, 175); // #9CA3AF

        private Color _bgHover = Color.FromArgb(245, 247, 250);        // rất nhạt
        private Color _bgSelected = Color.FromArgb(235, 240, 255);     // xanh nhạt
        private Color _bgPressed = Color.FromArgb(225, 232, 255);
        private Color _bgTransparent = Color.Transparent;

        public NavButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            ForeColor = _textColor;
            Height = 36;
            Cursor = Cursors.Hand;
            TabStop = true; // focus bằng bàn phím
            Padding = new Padding(_paddingX, 8, _paddingX, 8);
            AccessibleRole = AccessibleRole.PushButton;
        }

        // -------- Properties (Designer-friendly) --------
        [Category("Appearance"), Description("Icon hiển thị bên trái.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image IconImage
        {
            get => _iconImage;
            set { _iconImage = value; Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(typeof(Size), "18, 18")]
        public Size IconSize
        {
            get => _iconSize;
            set { _iconSize = value; Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(12)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("Layout"), DefaultValue(14)]
        public int PaddingX
        {
            get => _paddingX;
            set { _paddingX = Math.Max(0, value); Padding = new Padding(_paddingX, Padding.Top, _paddingX, Padding.Bottom); Invalidate(); }
        }

        [Category("Layout"), DefaultValue(10)]
        public int Gap
        {
            get => _gap;
            set { _gap = Math.Max(0, value); Invalidate(); }
        }

        [Category("Behavior"), DefaultValue(false)]
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color TextColor { get => _textColor; set { _textColor = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color TextColorSelected { get => _textColorSelected; set { _textColorSelected = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color TextColorDisabled { get => _textColorDisabled; set { _textColorDisabled = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackgroundHover { get => _bgHover; set { _bgHover = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackgroundSelected { get => _bgSelected; set { _bgSelected = value; Invalidate(); } }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackgroundPressed { get => _bgPressed; set { _bgPressed = value; Invalidate(); } }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            // --- Background theo trạng thái ---
            Color bg = _bgTransparent;
            if (!Enabled) bg = _bgTransparent;
            else if (_pressed) bg = _bgPressed;
            else if (_isSelected) bg = _bgSelected;
            else if (_hover) bg = _bgHover;

            // Hơi co vào 1px cho đỡ răng cưa ở viền
            Rectangle rect = ClientRectangle;
            rect.Inflate(-1, -1);

            if (bg.A > 0)
            {
                using (var path = RoundRect(rect, _cornerRadius))
                using (var br = new SolidBrush(bg))
                    g.FillPath(br, path);
            }

            // --- Layout icon + text ---
            int x = Padding.Left;
            int centerY = rect.Top + rect.Height / 2;

            if (_iconImage != null)
            {
                var iconRect = new Rectangle(x, centerY - _iconSize.Height / 2, _iconSize.Width, _iconSize.Height);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(_iconImage, iconRect);
                x += _iconSize.Width + _gap;
            }

            // --- Text (Bold khi IsSelected) ---
            var textRect = new Rectangle(x, rect.Top, rect.Right - x - Padding.Right, rect.Height);
            var txtColor = Enabled ? (_isSelected ? _textColorSelected : _textColor) : _textColorDisabled;

            Font drawFont = Font;
            bool created = false;
            if (_isSelected && Font.Style != FontStyle.Bold)
            {
                drawFont = new Font(Font, FontStyle.Bold);
                created = true;
            }

            TextRenderer.DrawText(
                g, Text, drawFont, textRect, txtColor,
                TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix
            );

            if (created) drawFont.Dispose();

            // --- Focus cue (tab vào nút) ---
            if (Focused && ShowFocusCues)
            {
                var focusRect = Rectangle.Inflate(rect, -3, -3);
                ControlPaint.DrawFocusRectangle(g, focusRect);
            }
        }

        // ---- Mouse/Keyboard states ----
        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); } base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        protected override bool IsInputKey(Keys keyData) => keyData == Keys.Space || keyData == Keys.Enter || base.IsInputKey(keyData);
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                _pressed = true; Invalidate();
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                _pressed = false; Invalidate();
                // kích hoạt Click khi nhả phím
                OnClick(EventArgs.Empty);
                e.Handled = true;
            }
            base.OnKeyUp(e);
        }

        // ---- Helpers ----
        private static GraphicsPath RoundRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            if (radius <= 0) { path.AddRectangle(rect); path.CloseFigure(); return path; }

            var arc = new Rectangle(rect.X, rect.Y, d, d);
            path.AddArc(arc, 180, 90);
            arc.X = rect.Right - d; path.AddArc(arc, 270, 90);
            arc.Y = rect.Bottom - d; path.AddArc(arc, 0, 90);
            arc.X = rect.X; path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
