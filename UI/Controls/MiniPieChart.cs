using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class MiniPieChart : UserControl
    {
        // Data
        private string[] _labels = { "Tiệc cưới", "Đặt bàn", "Hội nghị", "Sinh nhật" };
        private float[]  _values = { 65, 20, 10, 5 };

        // Colors (xanh dương, xanh lá, cam, tím)
        private Color[] _colors = {
            Color.FromArgb(66,133,244),
            Color.FromArgb(16,185,129),
            Color.FromArgb(245,158,11),
            Color.FromArgb(124,58,237)
        };

        // Card style
        private int _cornerRadius = 18;
        private Color _card = Color.White;
        private Color _border = Color.FromArgb(226,232,240);

        // Title
        private string _title = "Phân bổ doanh thu theo loại hình (%)";

        public MiniPieChart()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI", 10f);
            Size = new Size(680, 440);
            BackColor = Color.Transparent;
        }

        #region Public API
        [Category("Data")] public string[] Labels { get => _labels; set { _labels = value ?? Array.Empty<string>(); Invalidate(); } }
        [Category("Data")] public float[]  Values { get => _values; set { _values = value ?? Array.Empty<float>(); Invalidate(); } }
        [Category("Appearance")] public Color[] Colors { get => _colors; set { _colors = value ?? Array.Empty<Color>(); Invalidate(); } }
        [Category("Appearance")] public string Title { get => _title; set { _title = value ?? ""; Invalidate(); } }
        [Category("Appearance")] public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(8, value); Invalidate(); } }
        [Category("Appearance")] public Color CardColor { get => _card; set { _card = value; Invalidate(); } }
        [Category("Appearance")] public Color BorderColor { get => _border; set { _border = value; Invalidate(); } }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            // Card
            var card = ClientRectangle;
            using (var path = Round(card, _cornerRadius))
            using (var sb = new SolidBrush(_card))
            using (var pen = new Pen(_border))
            { g.FillPath(sb, path); g.DrawPath(pen, path); }

            // Title (căn GIỮA)
            using var titleFont = new Font("Segoe UI Semibold", 12f);
            using var titleBrush = new SolidBrush(Color.FromArgb(23, 23, 23));
            var titleSize = g.MeasureString(_title, titleFont);
            g.DrawString(_title, titleFont, titleBrush,
                new PointF((Width - titleSize.Width) / 2f, 14));

            // Data guard
            if (_labels == null || _values == null || _labels.Length == 0 || _values.Length == 0) return;
            int n = Math.Min(_labels.Length, _values.Length);

            // ---- TÍNH VÙNG BÁNH Ở CHÍNH GIỮA ----
            int margin = 16;                                 // lề tổng quanh card
            int topSpace = (int)titleSize.Height + 28;       // chừa chỗ cho title
            int availableW = Math.Max(0, Width - margin * 2);
            int availableH = Math.Max(0, Height - topSpace - margin);

            // đường kính tối đa để vừa cả 2 chiều
            int diameter = Math.Min(availableW, availableH);
            if (diameter <= 0) return;

            var bounds = new Rectangle(
                x: (Width - diameter) / 2,                  // căn GIỮA NGANG
                y: topSpace + (availableH - diameter) / 2,   // căn GIỮA DỌC (sau khi trừ title)
                width: diameter,
                height: diameter
            );

            // Vẽ lát
            float total = Math.Max(1f, _values.Take(n).Sum());
            float start = -90f;

            for (int i = 0; i < n; i++)
            {
                float val = _values[i];
                float sweep = val / total * 360f;
                var color = _colors[i % _colors.Length];

                using var sb = new SolidBrush(color);
                g.FillPie(sb, bounds, start, sweep);

                using var penSep = new Pen(Color.White, 2f);
                g.DrawPie(penSep, bounds, start, sweep);

                DrawLabel(g, bounds, start + sweep / 2f, _labels[i], val / total, color);

                start += sweep;
            }
        }


        private void DrawLabel(Graphics g, Rectangle pie, float angleDeg, string label, float percent, Color color)
        {
            // điểm giữa cung
            double ang = angleDeg * Math.PI / 180.0;
            float r = pie.Width / 2f;
            var center = new PointF(pie.Left + r, pie.Top + r);

            // điểm trên biên hình tròn (ra ngoài một chút)
            float x = center.X + (float)Math.Cos(ang) * (r + 10);
            float y = center.Y + (float)Math.Sin(ang) * (r + 10);

            // vị trí text (bên phải/ trái)
            bool right = x < Width / 2 ? false : true;
            float tx = right ? x + 10 : x - 10;
            float ty = y - 10;

            using var f = new Font("Segoe UI Semibold", 11f);
            string text = $"{label}: {Math.Round(percent * 100)}%";
            SizeF size = g.MeasureString(text, f);

            if (!right) tx -= size.Width; // căn phải nếu nằm bên trái bánh

            // màu chữ theo lát
            using var br = new SolidBrush(color);
            g.DrawString(text, f, br, new PointF(tx, ty));
        }

        private static GraphicsPath Round(Rectangle r, int radius)
        {
            r = Rectangle.Inflate(r, -1, -1);
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
