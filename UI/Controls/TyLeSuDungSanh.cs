// UsageRateCard.cs
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [ToolboxItem(true)]
    [DefaultProperty(nameof(BookedCount))]
    [SupportedOSPlatform("windows")]
    public class UsageRateCard : Control
    {
        // ===== Data =====
        private int _totalCount = 20;
        private int _bookedCount = 15;
        private string _title = "Tỷ lệ sử dụng sảnh";
        private string _subtitle = "Tháng này";

        [Category("Data"), DefaultValue(20)]
        public int TotalCount
        {
            get => _totalCount;
            set { _totalCount = Math.Max(0, value); Invalidate(); }
        }

        [Category("Data"), DefaultValue(15)]
        public int BookedCount
        {
            get => _bookedCount;
            set { _bookedCount = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance"), DefaultValue("Tỷ lệ sử dụng sảnh")]
        public string Title { get => _title; set { _title = value ?? ""; Invalidate(); } }

        [Category("Appearance"), DefaultValue("Tháng này")]
        public string Subtitle { get => _subtitle; set { _subtitle = value ?? ""; Invalidate(); } }

        // ===== Style =====
        [Category("Appearance"), DefaultValue(18)]
        public int CornerRadius { get; set; } = 18;

        [Category("Appearance"), DefaultValue(typeof(Color), "White")]
        public Color CardBackColor { get; set; } = Color.White;

        [Category("Appearance"), DefaultValue(typeof(Color), "230,232,236")]
        public Color BorderColor { get; set; } = Color.FromArgb(230, 232, 236);

        [Category("Appearance"), DefaultValue(typeof(Color), "26,188,156")]
        public Color BookedColor { get; set; } = Color.FromArgb(26, 188, 156);

        [Category("Appearance"), DefaultValue(typeof(Color), "210,216,222")]
        public Color EmptyColor { get; set; } = Color.FromArgb(210, 216, 222);

        [Category("Appearance"), DefaultValue(true)]
        public bool ShowShadow { get; set; } = true;

        public UsageRateCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI", 10f);
            Size = new Size(560, 420);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var card = ClientRectangle;
            card.Inflate(-2, -2);

            // Shadow
            if (ShowShadow)
            {
                var sh = card; sh.Offset(0, 2);
                using var sb = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
                using var gpS = Round(sh, CornerRadius + 2);
                g.FillPath(sb, gpS);
            }

            // Body
            using (var bg = new SolidBrush(CardBackColor))
            using (var pen = new Pen(BorderColor))
            using (var gp = Round(card, CornerRadius))
            {
                g.FillPath(bg, gp);
                g.DrawPath(pen, gp);
            }

            var pad = 18;
            int x = card.X + pad, y = card.Y + pad;

            // Title + subtitle
            TextRenderer.DrawText(g, Title, new Font(Font, FontStyle.Bold), new Point(x, y), Color.Black);
            y += 24;
            TextRenderer.DrawText(g, Subtitle, new Font(Font, FontStyle.Regular), new Point(x, y),
                Color.FromArgb(120, 120, 120));

            // Chart area
            int chartTop = y + 24;
            var chartRect = new Rectangle(x + 60, chartTop + 10, 200, 200);
            DrawPie(g, chartRect);

            // Labels (legend around chart)
            var bookedPct = PercentBooked();
            var emptyPct = 100 - bookedPct;

            TextRenderer.DrawText(g, $"Đã đặt: {bookedPct}%", new Font(Font, FontStyle.Bold),
                new Point(chartRect.Left - 54, chartRect.Top + chartRect.Height / 3),
                BookedColor);

            TextRenderer.DrawText(g, $"Còn trống: {emptyPct}%", new Font(Font, FontStyle.Regular),
                new Point(chartRect.Right + 12, chartRect.Top + chartRect.Height / 2),
                Color.FromArgb(150, 150, 150));

            // Bottom stats (two columns)
            int statsTop = chartRect.Bottom + 30;
            var colLeft = new Rectangle(x, statsTop, Width / 2 - 2 * pad, 60);
            var colRight = new Rectangle(Width - colLeft.Width - pad, statsTop, colLeft.Width, 60);

            DrawStat(g, colLeft, "Tổng sảnh:", TotalCount.ToString("0", CultureInfo.InvariantCulture), false);
            DrawStat(g, colRight, "Đã đặt:", $"{BookedCount:0} sảnh ({bookedPct}%)", true);
        }

        private void DrawPie(Graphics g, Rectangle r)
        {
            int bookedPct = PercentBooked();
            float sweepBooked = 360f * bookedPct / 100f;

            using var brBooked = new SolidBrush(BookedColor);
            using var brEmpty = new SolidBrush(EmptyColor);

            // full empty
            g.FillEllipse(brEmpty, r);
            // booked slice
            using (var gp = new GraphicsPath())
            {
                gp.AddPie(r, -90, sweepBooked);
                g.FillPath(brBooked, gp);
            }

            // inner hole (donut)
            var hole = r;
            int holeMargin = (int)(r.Width * 0.23); // bề dày vòng
            hole.Inflate(-holeMargin, -holeMargin);
            using var bWhite = new SolidBrush(CardBackColor);
            g.FillEllipse(bWhite, hole);
        }

        private void DrawStat(Graphics g, Rectangle area, string label, string value, bool highlight)
        {
            var labelColor = Color.FromArgb(120, 120, 120);
            var valueColor = highlight ? BookedColor : Color.Black;

            // Fix: Use Font instead of FontFamily for TextRenderer.DrawText
            TextRenderer.DrawText(g, label, new Font(Font.FontFamily, 10f, FontStyle.Regular),
                new Point(area.X, area.Y), labelColor);

            var fVal = new Font(Font.FontFamily, 10.5f, highlight ? FontStyle.Bold : FontStyle.Bold);
            TextRenderer.DrawText(g, value, fVal,
                new Rectangle(area.Right - 10 - 220, area.Y, 220, 22),
                valueColor, TextFormatFlags.Right);
        }

        private int PercentBooked()
        {
            if (TotalCount <= 0) return 0;
            var pct = (int)Math.Round(100.0 * Math.Min(BookedCount, TotalCount) / TotalCount, MidpointRounding.AwayFromZero);
            return Math.Max(0, Math.Min(100, pct));
        }

        private static GraphicsPath Round(Rectangle r, int radius)
        {
            var gp = new GraphicsPath();
            int d = radius * 2;
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }
}
