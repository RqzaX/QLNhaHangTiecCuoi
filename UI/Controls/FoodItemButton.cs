using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [DefaultEvent(nameof(Click))]
    [SupportedOSPlatform("windows")]
    public class FoodItemButton : Control
    {
        private bool _hover, _pressed;

        private string _title = "Tên món";
        private string _priceText = "45.000 đ";
        private Image _icon;
        private Size _iconSize = new Size(38, 38);

        private int _cornerRadius = 18;

        private Color _cardColor = Color.White;
        private Color _borderColor = Color.FromArgb(220, 225, 230);
        private Color _borderHoverColor = Color.FromArgb(40, 120, 255);
        private Color _priceColor = Color.FromArgb(31, 111, 235);
        private Color _titleColor = Color.FromArgb(28, 28, 30);

        private Padding _cardPadding = new Padding(14);

        // >>> NEW: chừa khoảng để không cắt bóng
        private Padding _shadowPadding = new Padding(8, 6, 8, 16);
        private int _shadowBlur = 3;          // độ mềm
        private int _shadowOffsetY = 6;        // lệch xuống
        private int _shadowMaxAlpha = 60;      // 0..255

        public FoodItemButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = LicenseManager.UsageMode == LicenseUsageMode.Designtime
                        ? SystemColors.Control
                        : Color.Transparent;

            Font = new Font("Segoe UI Semibold", 11f);
            Cursor = Cursors.Hand;
            Size = new Size(200, 170);
        }

        #region Props
        [Category("Food")]
        [Localizable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("Tên món")]
        public string Title
        {
            get => _title;
            set { _title = value ?? string.Empty; Invalidate(); }
        }
        public bool ShouldSerializeTitle() => _title != "Tên món";
        public void ResetTitle() => Title = "Tên món";

        [Category("Food")]
        [Localizable(true)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue("45.000 đ")]
        public string PriceText
        {
            get => _priceText;
            set { _priceText = value ?? string.Empty; Invalidate(); }
        }
        public bool ShouldSerializePriceText() => _priceText != "45.000 đ";
        public void ResetPriceText() => PriceText = "45.000 đ";

        [Category("Food")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(null)]
        // (tuỳ chọn) giúp mở hộp thoại chọn ảnh trong Designer
        [System.ComponentModel.Editor(typeof(System.Drawing.Design.ImageEditor),
                                      typeof(System.Drawing.Design.UITypeEditor))]
        public Image Icon
        {
            get => _icon;
            set { _icon = value; Invalidate(); }
        }
        public bool ShouldSerializeIcon() => _icon != null;
        public void ResetIcon() => Icon = null;

        [Category("Appearance")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(4, value); Invalidate(); } }
        [Category("Appearance")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public Color CardColor { get => _cardColor; set { _cardColor = value; Invalidate(); } }
        [Category("Appearance")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public Color BorderColor { get => _borderColor; set { _borderColor = value; Invalidate(); } }
        [Category("Appearance")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public Color BorderHoverColor { get => _borderHoverColor; set { _borderHoverColor = value; Invalidate(); } }
        [Category("Appearance")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public Color TitleColor { get => _titleColor; set { _titleColor = value; Invalidate(); } }
        [Category("Appearance")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public Color PriceColor { get => _priceColor; set { _priceColor = value; Invalidate(); } }
        [Category("Layout")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public Padding CardPadding { get => _cardPadding; set { _cardPadding = value; Invalidate(); } }

        // NEW
        [Category("Shadow")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public Padding ShadowPadding { get => _shadowPadding; set { _shadowPadding = value; Invalidate(); } }
        [Category("Shadow")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public int ShadowBlur { get => _shadowBlur; set { _shadowBlur = Math.Max(0, value); Invalidate(); } }
        [Category("Shadow")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public int ShadowOffsetY { get => _shadowOffsetY; set { _shadowOffsetY = value; Invalidate(); } }
        [Category("Shadow")][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] public int ShadowMaxAlpha { get => _shadowMaxAlpha; set { _shadowMaxAlpha = Math.Max(0, Math.Min(180, value)); Invalidate(); } }
        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                    cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; _pressed = false; Invalidate(); }
        protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); } }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _pressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Vùng hiển thị card, đã trừ khoảng shadow nên sẽ không bị cắt
            var visualRect = Rectangle.Inflate(ClientRectangle, -_shadowPadding.Left, -_shadowPadding.Top);
            visualRect = new Rectangle(visualRect.X,
                                       visualRect.Y,
                                       ClientRectangle.Width - _shadowPadding.Horizontal,
                                       ClientRectangle.Height - _shadowPadding.Vertical);

            // Nhấn -> dịch nhẹ
            int pressOffset = _pressed ? 1 : 0;

            // Bóng: chỉ khi hover
            if (_hover && _shadowBlur > 0)
                DrawSoftShadow(g, visualRect, _cornerRadius, _shadowBlur, _shadowOffsetY + pressOffset, _shadowMaxAlpha);

            // Card
            var cardRect = visualRect; cardRect.Offset(0, pressOffset);
            using (var path = RoundedRect(cardRect, _cornerRadius))
            {
                using (var br = new SolidBrush(_cardColor)) g.FillPath(br, path);
                var borderCol = _hover ? Mix(_borderColor, _borderHoverColor, 0.6f) : _borderColor;
                using (var pen = new Pen(borderCol, 1f)) g.DrawPath(pen, path);
            }

            // Nội dung
            var content = Rectangle.Inflate(cardRect, -_cardPadding.Left, -_cardPadding.Top);
            content = new Rectangle(content.X, content.Y, cardRect.Width - _cardPadding.Horizontal, cardRect.Height - _cardPadding.Vertical);

            int topY = content.Y;
            if (_icon != null)
            {
                var r = new Rectangle(content.X + (content.Width - _iconSize.Width) / 2, topY, _iconSize.Width, _iconSize.Height);
                // nền nhạt cho icon
                using (var br = new SolidBrush(Color.FromArgb(245, 247, 250)))
                    g.FillEllipse(br, new Rectangle(r.X - 8, r.Y - 8, r.Width + 16, r.Height + 16));
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(_icon, r);
                topY = r.Bottom + 10;
            }

            var titleRect = new Rectangle(content.X + 6, topY, content.Width - 12, 44);
            TextRenderer.DrawText(g, _title, Font, titleRect, _titleColor,
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl |
                TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis |
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);

            using var priceFont = new Font(Font.FontFamily, Math.Max(10f, Font.Size - 1f), FontStyle.Bold);
            var priceSize = TextRenderer.MeasureText(_priceText, priceFont);
            var priceRect = new Rectangle(
                content.X + (content.Width - priceSize.Width) / 2,
                cardRect.Bottom - _cardPadding.Bottom - priceSize.Height - 2,
                priceSize.Width, priceSize.Height);
            TextRenderer.DrawText(g, _priceText, priceFont, priceRect, _priceColor,
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.HorizontalCenter);
        }

        // Bóng mềm bằng nhiều lớp fill phình dần (không bị viền cứng)
        private void DrawSoftShadow(Graphics g, Rectangle baseRect, int radius, int blur, int offsetY, int maxAlpha)
        {
            var rect = baseRect; rect.Offset(0, offsetY);
            for (int i = blur; i >= 1; i--)
            {
                // phình ra mỗi bước 1px
                var grow = blur - i;
                var r = Inflate(rect, grow);
                using (var p = RoundedRect(r, radius + grow))
                using (var br = new SolidBrush(Color.FromArgb((int)(maxAlpha * Math.Pow(i / (float)blur, 1.6)), 0, 0, 0)))
                {
                    g.FillPath(br, p);
                }
            }
        }

        private static Rectangle Inflate(Rectangle r, int d) =>
            new Rectangle(r.X - d, r.Y - d, r.Width + d * 2, r.Height + d * 2);

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = Math.Max(0, radius) * 2;
            var path = new GraphicsPath();
            if (d == 0) { path.AddRectangle(bounds); path.CloseFigure(); return path; }
            Rectangle arc = new Rectangle(bounds.Location, new Size(d, d));
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - d; path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - d; path.AddArc(arc, 0, 90);
            arc.X = bounds.Left; path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Color Mix(Color a, Color b, float t)
        {
            t = Math.Max(0, Math.Min(1, t));
            return Color.FromArgb(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }
    }
}