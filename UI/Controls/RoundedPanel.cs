using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class RoundedPanel : Panel
    {
        private int _radius = 12;
        private int _border = 5;
        private Color _borderColor = Color.FromArgb(0xE5, 0xE7, 0xEB);

        public RoundedPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            BackColor = Color.White;
            Padding = new Padding(12);
        }

        // ----- CornerRadius -----
        [Category("Appearance")]
        [Description("Bo góc theo pixel.")]
        [DefaultValue(12)] // giúp Designer biết giá trị mặc định để khỏi serialize
        public int CornerRadius
        {
            get { return _radius; }
            set
            {
                if (value < 0) value = 0;
                if (_radius == value) return;
                _radius = value;
                UpdateRegion();
                Invalidate();
            }
        }

        // Designer sẽ gọi để quyết định có cần sinh code hay không
        public bool ShouldSerializeCornerRadius() => _radius != 12;
        public void ResetCornerRadius() { CornerRadius = 12; }

        // ----- BorderThickness -----
        [Category("Appearance")]
        [Description("Độ dày viền.")]
        [DefaultValue(1)]
        public int BorderThickness
        {
            get { return _border; }
            set
            {
                if (value < 0) value = 0;
                if (_border == value) return;
                _border = value;
                Invalidate();
            }
        }
        public bool ShouldSerializeBorderThickness() => _border != 1;
        public void ResetBorderThickness() { BorderThickness = 1; }

        // ----- BorderColor -----
        [Category("Appearance")]
        [Description("Màu viền.")]
        [DefaultValue(typeof(Color), "229, 231, 235")] // #E5E7EB
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor == value) return;
                _borderColor = value;
                Invalidate();
            }
        }
        public bool ShouldSerializeBorderColor() => _borderColor != Color.FromArgb(0xE5, 0xE7, 0xEB);
        public void ResetBorderColor() { BorderColor = Color.FromArgb(0xE5, 0xE7, 0xEB); }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            using (GraphicsPath path = BuildPath(ClientRectangle, _radius))
            {
                if (Region != null) Region.Dispose();
                Region = new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;
            rect.Width -= 1; rect.Height -= 1;

            using (GraphicsPath path = BuildPath(rect, _radius))
            using (SolidBrush fill = new SolidBrush(BackColor))
            {
                e.Graphics.FillPath(fill, path);
                if (_border > 0)
                {
                    using (Pen pen = new Pen(_borderColor, _border))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            }
        }

        private static GraphicsPath BuildPath(Rectangle r, int radius)
        {
            GraphicsPath p = new GraphicsPath();
            if (radius <= 0) { p.AddRectangle(r); return p; }
            int d = radius * 2;
            p.AddArc(r.Left, r.Top, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
