using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class HoaDon_Button : Button
    {
        // ====== Data props ======
        [Category("Data")] public string TableTitle { get; set; } = "Bàn A01";
        [Category("Data")] public string BadgeCode { get; set; } = "B001";
        [Category("Data")] public int Guests { get; set; } = 4;
        [Category("Data")] public int Dishes { get; set; } = 8;

        [Category("Data")] public decimal Subtotal { get; set; } = 850000m;
        [Category("Data")] public decimal VatRate { get; set; } = 0.10m;
        [Browsable(false)]
        public decimal VatAmount => Math.Round(Subtotal * VatRate, 0, MidpointRounding.AwayFromZero);

        [Category("Data")] public decimal Total { get; set; } = 935000m;
        [Category("Data")] public string StartTimeText { get; set; } = "14:30";

        // ====== Style props ======
        [Category("Appearance")] public int CornerRadius { get; set; } = 16;
        [Category("Appearance")] public int BorderThickness { get; set; } = 2;
        [Category("Appearance")] public Color BorderColor { get; set; } = Color.FromArgb(40, 40, 60);
        [Category("Appearance")] public Color BackCardColor { get; set; } = Color.White;
        [Category("Appearance")] public Color ForeMain { get; set; } = Color.Black;
        [Category("Appearance")] public Color ForeMuted { get; set; } = Color.FromArgb(90, 90, 110);
        [Category("Appearance")] public Color DividerColor { get; set; } = Color.FromArgb(230, 230, 235);
        [Category("Appearance")] public Padding CardPadding { get; set; } = new Padding(14, 12, 14, 12);
        [Category("Appearance")] public bool ShowShadow { get; set; } = false;

        private readonly CultureInfo _viVN = new CultureInfo("vi-VN");

        public HoaDon_Button()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;

            MinimumSize = new Size(260, 150);
            AutoSize = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle rc = ClientRectangle;
            rc.Width -= 1; rc.Height -= 1;

            // Card area
            Rectangle card = Rectangle.Inflate(rc, -1, -1);

            using (GraphicsPath path = RoundRect(card, CornerRadius))
            {
                // Shadow
                if (ShowShadow)
                {
                    using (var shadow = new SolidBrush(Color.FromArgb(30, Color.Black)))
                    {
                        var shadowRect = new Rectangle(card.X + 2, card.Y + 3, card.Width, card.Height);
                        using (var spath = RoundRect(shadowRect, CornerRadius))
                            g.FillPath(shadow, spath);
                    }
                }

                // Fill
                using (var b = new SolidBrush(BackCardColor))
                    g.FillPath(b, path);

                // Border
                using (var pen = new Pen(BorderColor, BorderThickness))
                    g.DrawPath(pen, path);
            }

            // Layout
            var pad = CardPadding;
            var content = Rectangle.Inflate(card, -pad.Left, -pad.Top);
            content.Width -= pad.Right - pad.Left;
            content.Height -= pad.Bottom - pad.Top;

            // Fonts
            using var fTitle = new Font(Font.FontFamily, 10.5f, FontStyle.Bold);
            using var fSmall = new Font(Font.FontFamily, 9f, FontStyle.Regular);
            using var fMoney = new Font(Font.FontFamily, 10f, FontStyle.Regular);
            using var fTotal = new Font(Font.FontFamily, 11f, FontStyle.Bold);

            // 1) Title (left) + Badge (right)
            var y = content.Y;
            SizeF titleSize = g.MeasureString(TableTitle, fTitle);
            var titleRect = new RectangleF(content.X, y, content.Width - 80, titleSize.Height + 2);
            using (var sb = new SolidBrush(ForeMain))
                g.DrawString(TableTitle, fTitle, sb, titleRect, new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });

            // Badge
            string badge = BadgeCode;
            SizeF badgeSize = g.MeasureString(badge, fSmall);
            var badgePadding = new Size(10, 4);
            var badgeRect = new Rectangle((int)(content.Right - badgeSize.Width) - badgePadding.Width - 4,
                                          (int)y - 1,
                                          (int)badgeSize.Width + badgePadding.Width * 2,
                                          (int)badgeSize.Height + badgePadding.Height);

            using (var pathBadge = RoundRect(badgeRect, 12))
            using (var bBadge = new SolidBrush(Color.White))
            using (var pBadge = new Pen(BorderColor, 1))
            using (var sbBadge = new SolidBrush(ForeMuted))
            {
                g.FillPath(bBadge, pathBadge);
                g.DrawPath(pBadge, pathBadge);
                g.DrawString(badge, fSmall, sbBadge,
                    new RectangleF(badgeRect.X, badgeRect.Y + 1, badgeRect.Width, badgeRect.Height),
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }

            // 2) Sub line: "4 khách • 8 món"
            y += (int)titleSize.Height + 6;
            string sub = $"{Guests} khách • {Dishes} món";
            using (var sb = new SolidBrush(ForeMuted))
                g.DrawString(sub, fSmall, sb, new PointF(content.X, y));

            // Money formatting
            string sSubtotal = FormatMoney(Subtotal);
            string sVat = FormatMoney(VatAmount);
            string sTotal = FormatMoney(Total);

            // 3) Rows: tạm tính / VAT / Tổng cộng
            y += 20;
            DrawRow(g, "Tạm tính", sSubtotal, ref y, fSmall, fMoney, ForeMuted, ForeMain, content);
            DrawRow(g, $"VAT ({(int)(VatRate * 100)}%)", sVat, ref y, fSmall, fMoney, ForeMuted, ForeMain, content);

            // Divider
            y += 6;
            using (var p = new Pen(DividerColor, 1))
                g.DrawLine(p, content.X, y, content.Right, y);
            y += 8;

            DrawRow(g, "Tổng cộng", sTotal, ref y, fSmall, fTotal, ForeMain, ForeMain, content);

            // 4) Start time
            y += 8;
            using (var sb = new SolidBrush(ForeMuted))
                g.DrawString($"Bắt đầu: {StartTimeText}", fSmall, sb, new PointF(content.X, y));
        }

        private void DrawRow(Graphics g, string left, string right, ref int y,
                             Font fLeft, Font fRight, Color cLeft, Color cRight, Rectangle content)
        {
            int rowH = Math.Max(fLeft.Height, fRight.Height) + 2;

            using var sbLeft = new SolidBrush(cLeft);
            using var sbRight = new SolidBrush(cRight);

            var leftRect = new Rectangle(content.X, y, content.Width / 2, rowH);
            var rightRect = new Rectangle(content.X, y, content.Width, rowH);

            g.DrawString(left, fLeft, sbLeft, leftRect,
                new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });

            g.DrawString(right, fRight, sbRight, rightRect,
                new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near });

            y += rowH + 2;
        }

        private string FormatMoney(decimal v)
        {
            // “935.000 đ”
            return v.ToString("#,0 đ", _viVN);
        }

        private static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(r); return path; }

            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Disable default button painting
        protected override void OnPaintBackground(PaintEventArgs pevent) { /* no base */ }
    }
}
