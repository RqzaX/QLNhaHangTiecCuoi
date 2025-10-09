using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UiControls
{
    [SupportedOSPlatform("windows")]
    public class MiniBarChart : UserControl
    {
        // Data
        private string[] _labels = Array.Empty<string>();
        private float[] _seriesA = Array.Empty<float>(); // Doanh thu
        private float[] _seriesB = Array.Empty<float>(); // Lợi nhuận

        // Style
        private string _title = "Doanh thu & Lợi nhuận";
        private Color _bgCard = Color.White;
        private Color _bdCard = Color.FromArgb(226, 232, 240);
        private Color _axColor = Color.FromArgb(203, 213, 225);
        private Color _seriesAColor = Color.FromArgb(66, 133, 244);
        private Color _seriesBColor = Color.FromArgb(16, 185, 129);
        private int _cornerRadius = 18;
        private Padding _inner = new Padding(16, 40, 16, 40); // chừa chỗ cho trục/legend

        public MiniBarChart()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            Size = new Size(720, 420);
            Font = new Font("Segoe UI", 10f);
        }

        #region Public API
        [Category("Data")] public string[] Labels { get => _labels; set { _labels = value ?? Array.Empty<string>(); Invalidate(); } }
        [Category("Data")] public float[] SeriesA { get => _seriesA; set { _seriesA = value ?? Array.Empty<float>(); Invalidate(); } }
        [Category("Data")] public float[] SeriesB { get => _seriesB; set { _seriesB = value ?? Array.Empty<float>(); Invalidate(); } }
        [Category("Appearance")] public string Title { get => _title; set { _title = value ?? ""; Invalidate(); } }
        [Category("Appearance")] public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(8, value); Invalidate(); } }
        [Category("Appearance")] public Color CardColor { get => _bgCard; set { _bgCard = value; Invalidate(); } }
        [Category("Appearance")] public Color BorderColor { get => _bdCard; set { _bdCard = value; Invalidate(); } }
        [Category("Appearance")] public Color AxisColor { get => _axColor; set { _axColor = value; Invalidate(); } }
        [Category("Appearance")] public Color SeriesAColor { get => _seriesAColor; set { _seriesAColor = value; Invalidate(); } }
        [Category("Appearance")] public Color SeriesBColor { get => _seriesBColor; set { _seriesBColor = value; Invalidate(); } }
        [Category("Layout")] public Padding InnerPadding { get => _inner; set { _inner = value; Invalidate(); } }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            // --- Card ---
            var card = ClientRectangle;
            using (var path = Round(card, _cornerRadius))
            using (var sb = new SolidBrush(_bgCard))
            using (var pen = new Pen(_bdCard))
            { g.FillPath(sb, path); g.DrawPath(pen, path); }

            // ---- Fonts & khoảng trống động ----
            using var fTitle = new Font("Segoe UI Semibold", 12f);
            using var fAxis = new Font(Font.FontFamily, Font.Size - 1f);
            using var fLegend = new Font("Segoe UI", 10f);

            var titleSize = g.MeasureString(_title, fTitle);
            float titleH = titleSize.Height + 12;                       // khoảng trống cho tiêu đề
            float xLabelH = g.MeasureString("T10", Font).Height + 10;    // chiều cao nhãn X
            float legendH = Math.Max(20, g.MeasureString("Lợi nhuận", fLegend).Height) + 16;

            // viền trong card
            int margin = 14;
            var inner = new Rectangle(
                card.Left + margin,
                card.Top + margin,
                card.Width - margin * 2,
                card.Height - margin * 2
            );

            // ===== DỮ LIỆU =====
            int n = new[] { _labels.Length, _seriesA.Length, _seriesB.Length }.DefaultIfEmpty(0).Min();
            if (n <= 0) return;

            float maxVal = Math.Max(_seriesA.Take(n).DefaultIfEmpty(0).Max(),
                                    _seriesB.Take(n).DefaultIfEmpty(0).Max());
            if (maxVal <= 0) maxVal = 1;
            float yMax = (float)Math.Ceiling(maxVal * 1.1f / 50f) * 50f;

            // --- đo bề rộng lớn nhất của nhãn Y để chừa lề trái ---
            int yTicks = 5;
            float maxYLabelW = 0;
            for (int i = 0; i <= yTicks; i++)
            {
                float t = i / (float)yTicks;
                string lab = ((int)(yMax * t)).ToString();
                maxYLabelW = Math.Max(maxYLabelW, g.MeasureString(lab, fAxis).Width);
            }
            int leftYPadding = (int)Math.Ceiling(maxYLabelW) + 12; // 12px cách trục

            // --- Title (vẽ trước) ---
            using (var brTitle = new SolidBrush(Color.FromArgb(23, 23, 23)))
                g.DrawString(_title, fTitle, brTitle, new PointF(inner.Left, inner.Top - 4));

            // --- Plot: đã trừ Title + X labels + Legend + lề trái cho nhãn Y ---
            var plot = new Rectangle(
                inner.Left + leftYPadding,
                inner.Top + (int)titleH,
                inner.Width - leftYPadding,
                inner.Height - (int)titleH - (int)xLabelH - (int)legendH
            );
            if (plot.Width <= 0 || plot.Height <= 0) return;

            // --- Lưới, nhãn Y & trục ---
            using (var gridPen = new Pen(_axColor) { DashStyle = DashStyle.Dot })
            using (var axPen = new Pen(_axColor))
            using (var brAxis = new SolidBrush(Color.FromArgb(80, 80, 80)))
            {
                for (int i = 0; i <= yTicks; i++)
                {
                    float t = i / (float)yTicks;
                    int y = (int)(plot.Bottom - t * plot.Height);
                    g.DrawLine(gridPen, plot.Left, y, plot.Right, y);

                    string lab = ((int)(yMax * t)).ToString();
                    var sz = g.MeasureString(lab, fAxis);
                    g.DrawString(lab, fAxis, brAxis, plot.Left - 8 - sz.Width, y - sz.Height / 2f);
                }
                // trục X & trục Y
                g.DrawLine(axPen, plot.Left, plot.Bottom, plot.Right, plot.Bottom);
                g.DrawLine(axPen, plot.Left, plot.Top, plot.Left, plot.Bottom);
            }

            // --- CỘT ---
            float groupW = plot.Width / (float)n;
            float barGap = 6f;
            float barWidth = (groupW - 16) / 2f; // chừa 8px hai bên
            if (barWidth < 4) barWidth = 4;

            using (var sA = new SolidBrush(_seriesAColor))
            using (var sB = new SolidBrush(_seriesBColor))
            using (var brX = new SolidBrush(Color.FromArgb(60, 60, 60)))
            {
                for (int i = 0; i < n; i++)
                {
                    float x0 = plot.Left + i * groupW + 8;

                    float hA = _seriesA[i] / yMax * plot.Height;
                    var rA = new RectangleF(x0, plot.Bottom - hA, barWidth, hA);
                    FillRoundRect(g, sA, rA, 4);

                    float hB = _seriesB[i] / yMax * plot.Height;
                    var rB = new RectangleF(x0 + barWidth + barGap, plot.Bottom - hB, barWidth, hB);
                    FillRoundRect(g, sB, rB, 4);

                    // Nhãn X — nằm dưới plot, không bị đè
                    string xl = _labels[i];
                    var sz = g.MeasureString(xl, Font);
                    float tx = x0 + (barWidth * 2 + barGap - sz.Width) / 2f;
                    g.DrawString(xl, Font, brX, tx, plot.Bottom + 4);
                }
            }

            // --- Legend dưới cùng ---
            int legendY = plot.Bottom + (int)xLabelH + 4;
            DrawLegend(g, "Doanh thu", _seriesAColor, inner.Left, legendY,
                       inner.Width / 2 - 8, ContentAlignment.MiddleRight, fLegend);
            DrawLegend(g, "Lợi nhuận", _seriesBColor, inner.Left + inner.Width / 2 + 8, legendY,
                       inner.Width / 2 - 8, ContentAlignment.MiddleLeft, fLegend);
        }


        // sửa hàm legend để nhận font và trả lời căn vị trí đẹp
        private void DrawLegend(Graphics g, string text, Color color, int x, int y, int width,
                                ContentAlignment align, Font font)
        {
            using var br = new SolidBrush(color);
            SizeF tsize = g.MeasureString(text, font);
            int box = 14, gap = 6;
            float tx, bx;
            if (align == ContentAlignment.MiddleRight)
            {
                tx = x + width - tsize.Width;
                bx = tx - gap - box;
            }
            else
            {
                bx = x;
                tx = bx + box + gap;
            }
            g.FillRectangle(br, bx, y, box, box);
            g.DrawString(text, font, Brushes.Black, tx, y - 2);
        }


        // Helpers
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

        private static void FillRoundRect(Graphics g, Brush b, RectangleF r, int radius)
        {
            int d = radius * 2;
            using var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            g.FillPath(b, path);
        }

        
    }
}
