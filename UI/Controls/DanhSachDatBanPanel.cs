// ReservationItemPanel.cs
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    public enum ReservationStatus
    {
        DaXacNhan,
        ChoXacNhan,
        DaHuy
    }

    [ToolboxItem(true)]
    [DefaultProperty(nameof(CustomerName))]
    [SupportedOSPlatform("windows")]    
    public class DanhSachDatBanPanel : Control
    {
        // ====== Data ======
        private string _timeText = "08:00";
        private string _customerName = "Nguyễn Văn A";
        private string _typeText = "Đặt bàn";
        private string _tableText = "Bàn 12";
        private int _guestCount = 4;
        private ReservationStatus _status = ReservationStatus.DaXacNhan;

        // ====== Style ======
        [Category("Appearance"), DefaultValue(18)]
        public int CornerRadius { get; set; } = 18;

        [Category("Appearance"), DefaultValue(typeof(Color), "247,249,252")]
        public Color CardBackColor { get; set; } = Color.FromArgb(247, 249, 252);

        [Category("Appearance"), DefaultValue(typeof(Color), "230,232,236")]
        public Color BorderColor { get; set; } = Color.FromArgb(230, 232, 236);

        [Category("Appearance"), DefaultValue(typeof(Color), "99,102,241")]
        public Color TimeColor { get; set; } = Color.FromArgb(99, 102, 241); // tím nhạt

        [Category("Data"), DefaultValue("08:00")]
        public string TimeText { get => _timeText; set { _timeText = value ?? ""; Invalidate(); } }

        [Category("Data"), DefaultValue("Nguyễn Văn A")]
        public string CustomerName { get => _customerName; set { _customerName = value ?? ""; Invalidate(); } }

        [Category("Data"), DefaultValue("Đặt bàn")]
        public string TypeText { get => _typeText; set { _typeText = value ?? ""; Invalidate(); } }

        [Category("Data"), DefaultValue("Bàn 12")]
        public string TableText { get => _tableText; set { _tableText = value ?? ""; Invalidate(); } }

        [Category("Data"), DefaultValue(4)]
        public int GuestCount { get => _guestCount; set { _guestCount = Math.Max(0, value); Invalidate(); } }

        [Category("Data"), DefaultValue(typeof(ReservationStatus), nameof(ReservationStatus.DaXacNhan))]
        public ReservationStatus Status { get => _status; set { _status = value; Invalidate(); } }

        // Hover feedback (nhẹ)
        private bool _hover;
        public DanhSachDatBanPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI", 10f);
            Size = new Size(560, 92);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var card = ClientRectangle; card.Inflate(-1, -1);

            // Nền bo tròn
            using (var bg = new SolidBrush(CardBackColor))
            using (var pen = new Pen(_hover ? ControlPaint.Dark(BorderColor, .08f) : BorderColor))
            using (var gp = Round(card, CornerRadius))
            {
                g.FillPath(bg, gp);
                g.DrawPath(pen, gp);
            }

            int padX = 16, padY = 14;
            int x = card.X + padX;
            int y = card.Y + padY;

            // Cột giờ (màu tím)
            var fTime = new Font(Font, FontStyle.Bold);
            var timeSize = TextRenderer.MeasureText(TimeText, fTime);
            TextRenderer.DrawText(g, TimeText, fTime, new Point(x, y), TimeColor);
            int leftColWidth = Math.Max(52, timeSize.Width + 8);

            // Cột nội dung
            int cx = x + leftColWidth;
            int w = card.Right - padX - cx;

            // Dòng tên + pill trạng thái
            var nameFont = new Font(Font, FontStyle.Bold);
            var nameRect = new Rectangle(cx, y - 2, w, 24);
            // chừa chỗ cho pill: vẽ tên trước, pill canh phải
            int pillWidth = DrawStatusPill(g, new Rectangle(card.Right - padX - 120, y - 4, 120, 28)); // trả về kích thước thực bên trong
            var nameRectReal = new Rectangle(cx, y - 2, w - pillWidth - 8, 24);
            TextRenderer.DrawText(g, CustomerName, nameFont, nameRectReal, Color.Black,
                TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);

            // Dòng mô tả
            y += 26;
            string desc = $"{TypeText} • {TableText} • {GuestCount} khách";
            var descColor = Color.FromArgb(110, 119, 129);
            TextRenderer.DrawText(g, desc, new Font(Font, FontStyle.Regular), new Rectangle(cx, y, w, 22),
                descColor, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
        }

        private int DrawStatusPill(Graphics g, Rectangle areaMax)
        {
            string text = Status switch
            {
                ReservationStatus.DaXacNhan => "Đã xác nhận",
                ReservationStatus.ChoXacNhan => "Chờ xác nhận",
                _ => "Đã hủy"
            };

            // màu nền & chữ
            Color back, fore;
            switch (Status)
            {
                case ReservationStatus.DaXacNhan:
                    back = Color.FromArgb(24, 24, 27); // gần đen
                    fore = Color.White;
                    break;
                case ReservationStatus.ChoXacNhan:
                    back = Color.FromArgb(240, 185, 0);
                    fore = Color.Black;
                    break;
                default: // DaHuy
                    back = Color.FromArgb(220, 38, 38);
                    fore = Color.White;
                    break;
            }

            var f = new Font(Font, FontStyle.Bold);
            var sz = TextRenderer.MeasureText(text, f, Size.Empty, TextFormatFlags.NoPadding);
            int padH = 10, padV = 4;
            int width = Math.Min(areaMax.Width, sz.Width + padH * 2);
            int height = Math.Min(areaMax.Height, sz.Height + padV * 2);

            var r = new Rectangle(areaMax.Right - width, areaMax.Top, width, height);
            using var b = new SolidBrush(back);
            using var gp = Round(r, r.Height / 2);
            g.FillPath(b, gp);

            TextRenderer.DrawText(g, text, f, r, fore,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            return width;
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
