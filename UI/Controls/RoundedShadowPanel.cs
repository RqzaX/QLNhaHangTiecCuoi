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
    public class RoundedShadowPanel : Panel
    {
        private int _cornerRadius = 16;
        private int _shadowBlur = 12;
        private float _shadowOpacity = 0.35f;
        private Point _shadowOffset = new Point(0, 4);
        private Color _shadowColor = Color.Black;
        private Color _borderColor = Color.FromArgb(220, 220, 220);
        private int _borderThickness = 1;
        private bool _shadowEnabled = true;

        public RoundedShadowPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.White;
            ForeColor = Color.Black;
            Padding = new Padding(8);
        }

        [Category("Appearance")]
        [Description("Bán kính bo góc (px).")]
        [DefaultValue(16)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); Invalidate(); UpdateRegionSafe(); }
        }

        [Category("Appearance")]
        [Description("Bật/Tắt đổ bóng.")]
        [DefaultValue(true)]
        public bool ShadowEnabled
        {
            get => _shadowEnabled;
            set { _shadowEnabled = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Độ mờ của bóng (0..1).")]
        [DefaultValue(0.35f)]
        public float ShadowOpacity
        {
            get => _shadowOpacity;
            set { _shadowOpacity = Math.Min(1f, Math.Max(0f, value)); Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Mức độ làm mờ của bóng (px).")]
        [DefaultValue(12)]
        public int ShadowBlur
        {
            get => _shadowBlur;
            set { _shadowBlur = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Độ lệch bóng (px).")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(Point), "0, 4")]
        public Point ShadowOffset
        {
            get => _shadowOffset;
            set { _shadowOffset = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Màu bóng.")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ShadowColor
        {
            get => _shadowColor;
            set { _shadowColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Màu viền panel.")]
        [DefaultValue(typeof(Color), "220, 220, 220")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("Độ dày viền (px). 0 là không viền.")]
        [DefaultValue(1)]
        public int BorderThickness
        {
            get => _borderThickness;
            set { _borderThickness = Math.Max(0, value); Invalidate(); }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegionSafe();
        }

        private void UpdateRegionSafe()
        {
            // Tránh lỗi khi đang ở DesignMode/đang dispose
            if (!IsHandleCreated && !DesignMode) return;
            using (var path = CreateRoundRectPath(ClientRectangle, _cornerRadius))
            {
                Region?.Dispose();
                Region = new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int blur = _shadowEnabled ? _shadowBlur : 0;

            var contentRect = new Rectangle(
                Math.Max(0, blur - _shadowOffset.X),
                Math.Max(0, blur - _shadowOffset.Y),
                Math.Max(0, Width - 1 - Math.Max(0, blur + _shadowOffset.X) - Math.Max(0, blur - _shadowOffset.X)),
                Math.Max(0, Height - 1 - Math.Max(0, blur + _shadowOffset.Y) - Math.Max(0, blur - _shadowOffset.Y))
            );

            if (_shadowEnabled && _shadowOpacity > 0 && _shadowBlur > 0)
                DrawSoftShadow(g, contentRect, _cornerRadius, _shadowColor, _shadowOpacity, _shadowBlur, _shadowOffset);

            using (var path = CreateRoundRectPath(contentRect, _cornerRadius))
            using (var br = new SolidBrush(BackColor))
            {
                g.FillPath(br, path);

                if (_borderThickness > 0)
                {
                    using (var pen = new Pen(_borderColor, _borderThickness))
                        g.DrawPath(pen, path);
                }
            }
        }

        private static GraphicsPath CreateRoundRectPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;

            if (radius <= 0 || d > rect.Width || d > rect.Height)
            {
                path.AddRectangle(rect);
                path.CloseFigure();
                return path;
            }

            var arc = new Rectangle(rect.X, rect.Y, d, d);

            // Top left arc
            path.AddArc(arc, 180, 90);

            // Top right arc
            arc.X = rect.Right - d;
            path.AddArc(arc, 270, 90);

            // Bottom right arc
            arc.Y = rect.Bottom - d;
            path.AddArc(arc, 0, 90);

            // Bottom left arc
            arc.X = rect.X;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void DrawSoftShadow(Graphics g, Rectangle rect, int radius, Color color, float opacity, int blur, Point offset)
        {
            // Create a temporary bitmap for the shadow
            using (var shadowBmp = new Bitmap(rect.Width + blur * 2, rect.Height + blur * 2))
            using (var shadowG = Graphics.FromImage(shadowBmp))
            {
                shadowG.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CreateRoundRectPath(
                    new Rectangle(blur, blur, rect.Width, rect.Height),
                    radius))
                using (var brush = new SolidBrush(Color.FromArgb((int)(opacity * 255), color)))
                {
                    shadowG.FillPath(brush, path);
                }

                // Apply a simple blur by drawing the shadow bitmap multiple times with transparency
                for (int i = 1; i <= blur; i++)
                {
                    using (var attr = new System.Drawing.Imaging.ImageAttributes())
                    {
                        float alpha = opacity * (1f - (float)i / (blur + 1));
                        var cm = new System.Drawing.Imaging.ColorMatrix
                        {
                            Matrix33 = alpha
                        };
                        attr.SetColorMatrix(cm);
                        g.DrawImage(
                            shadowBmp,
                            new Rectangle(rect.X + offset.X - blur / 2, rect.Y + offset.Y - blur / 2, shadowBmp.Width, shadowBmp.Height),
                            0, 0, shadowBmp.Width, shadowBmp.Height,
                            GraphicsUnit.Pixel,
                            attr);
                    }
                }
            }
        }
    }
}
