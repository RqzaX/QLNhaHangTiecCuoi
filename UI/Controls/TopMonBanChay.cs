// TopDishRow.cs
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
    [DefaultProperty(nameof(Title))]
    [SupportedOSPlatform("windows")]
    public class TopMonBanChay : Control
    {
        // ===== Data =====
        private int _rank = 1;
        private string _title = "Tên món";
        private int _orders = 0;
        private decimal _revenue = 0m;

        [Category("Data"), DefaultValue(1)]
        public int Rank { get => _rank; set { _rank = Math.Max(1, value); Invalidate(); } }

        [Category("Data"), DefaultValue("Tên món")]
        public string Title { get => _title; set { _title = value ?? ""; Invalidate(); } }

        [Category("Data"), DefaultValue(0)]
        public int Orders { get => _orders; set { _orders = Math.Max(0, value); Invalidate(); } }

        [Category("Data"), DefaultValue(typeof(decimal), "0")]
        public decimal Revenue { get => _revenue; set { _revenue = Math.Max(0, value); Invalidate(); } }

        // ===== Appearance =====
        [Category("Appearance"), DefaultValue(typeof(Color), "234, 240, 255")]
        public Color BubbleBackColor { get; set; } = Color.FromArgb(234, 240, 255);

        [Category("Appearance"), DefaultValue(typeof(Color), "93, 95, 239")]
        public Color BubbleTextColor { get; set; } = Color.FromArgb(93, 95, 239);

        [Category("Appearance"), DefaultValue(typeof(Color), "105, 112, 119")]
        public Color SubTextColor { get; set; } = Color.FromArgb(105, 112, 119);

        [Category("Appearance"), DefaultValue(false)]
        public bool ShowSeparator { get; set; } = false;

        private bool _hover;

        public TopMonBanChay()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI", 10f);
            Height = 66;
            Size = new Size(560, 66);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = ClientRectangle;
            rect.Inflate(-2, -2);

            // hover very light highlight
            if (_hover)
            {
                using var hl = new SolidBrush(Color.FromArgb(12, 0, 0, 0));
                g.FillRectangle(hl, rect);
            }

            int padX = 8, padY = 8;
            int x = rect.X + padX, y = rect.Y + padY;

            // Bubble thứ hạng
            var bubbleRect = new Rectangle(x, y, 36, 36);
            using (var br = new SolidBrush(BubbleBackColor))
                g.FillEllipse(br, bubbleRect);
            TextRenderer.DrawText(g, Rank.ToString(), new Font(Font, FontStyle.Bold),
                bubbleRect, BubbleTextColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            // Text bên trái (2 dòng)
            int textLeft = bubbleRect.Right + 12;
            int textWidth = rect.Width - textLeft - 160; // chừa cột doanh thu
            var titleRect = new Rectangle(textLeft, y, textWidth, 22);
            TextRenderer.DrawText(g, Title, new Font(Font, FontStyle.Bold), titleRect, Color.Black,
                TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);

            var subRect = new Rectangle(textLeft, y + 22, textWidth, 20);
            TextRenderer.DrawText(g, $"{Orders} đơn", new Font(Font.FontFamily, 9f, FontStyle.Regular), subRect, SubTextColor,
                TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);

            // Cột doanh thu (bên phải)
            var money = string.Format(new CultureInfo("vi-VN"), "{0:#,0} đ", Revenue);
            var moneyRect = new Rectangle(rect.Right - 140, y + 10, 132, 24);
            TextRenderer.DrawText(g, money, new Font(Font, FontStyle.Bold), moneyRect, Color.Black,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter);

            // Separator (tuỳ chọn)
            if (ShowSeparator)
            {
                using var p = new Pen(Color.FromArgb(230, 232, 236));
                g.DrawLine(p, textLeft, rect.Bottom - 1, rect.Right - 8, rect.Bottom - 1);
            }
        }
    }
}
