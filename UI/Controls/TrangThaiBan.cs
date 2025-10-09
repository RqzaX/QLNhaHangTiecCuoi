// TrangThaiBan.cs
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    public enum TableStatus { Trong, DangDung, DaDat }

    [ToolboxItem(true)]
    [DefaultProperty(nameof(Status))]
    [DefaultEvent(nameof(CardClicked))]
    [SupportedOSPlatform("windows")]
    public class TrangThaiBan : Control
    {
        // ===== Data =====
        private string _maBan = "A01";
        private int _sucChua = 4;
        private int _soKhach = 0;
        private TableStatus _status = TableStatus.Trong;
        private int _soPhut = 0;          // khi đang dùng
        private string _gioDat = "";       // khi đã đặt
        private decimal _tien = 0m;

        // ===== Appearance =====
        private int _cornerRadius = 18;
        private bool _showShadow = true;

        [Category("Data"), DefaultValue("A01")] public string MaBan { get => _maBan; set { _maBan = value ?? ""; Invalidate(); } }
        [Category("Data"), DefaultValue(4)] public int SucChua { get => _sucChua; set { _sucChua = Math.Max(0, value); Invalidate(); } }
        [Category("Data"), DefaultValue(0)] public int SoKhach { get => _soKhach; set { _soKhach = Math.Max(0, value); Invalidate(); } }
        [Category("Data"), DefaultValue(typeof(TableStatus), nameof(TableStatus.Trong))] public TableStatus Status { get => _status; set { _status = value; Invalidate(); } }
        [Category("Data"), DefaultValue(0), Description("Số phút đã dùng (chỉ hiển thị khi Đang dùng).")] public int SoPhut { get => _soPhut; set { _soPhut = Math.Max(0, value); Invalidate(); } }
        [Category("Data"), DefaultValue(""), Description("Giờ đặt (vd 13:00) – chỉ hiển thị khi Đã đặt.")] public string GioDat { get => _gioDat; set { _gioDat = value ?? ""; Invalidate(); } }
        [Category("Data"), DefaultValue(typeof(decimal), "0")] public decimal Tien { get => _tien; set { _tien = Math.Max(0, value); Invalidate(); } }

        [Category("Appearance"), DefaultValue(18)] public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(8, value); Invalidate(); } }
        [Category("Appearance"), DefaultValue(true)] public bool ShowShadow { get => _showShadow; set { _showShadow = value; Invalidate(); } }

        // ===== Events =====
        [Category("Action")] public event EventHandler CardClicked;
        [Category("Action")] public event EventHandler TransferClicked;

        // ===== States & hit box =====
        private bool _hoverCard, _pressedCard;
        private bool _hoverTransfer, _pressedTransfer;
        private Rectangle _rcTransfer = Rectangle.Empty;

        public TrangThaiBan()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                     ControlStyles.Selectable, true);

            Font = new Font("Segoe UI", 10f);
            Size = new Size(170, 215);
            TabStop = true;
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var card = ClientRectangle;
            card.Inflate(-2, -2);

            // palette theo trạng thái
            GetPalette(Status, out var border, out var light, out var badgeBack, out var badgeText);

            if (_hoverCard) border = ControlPaint.Dark(border, .08f);
            if (_pressedCard) card.Offset(0, 1);

            // Shadow
            if (ShowShadow)
            {
                var sh = card; sh.Offset(0, 2);
                using var sb = new SolidBrush(Color.FromArgb(40, 0, 0, 0));
                using var gpS = Round(sh, CornerRadius + 2);
                g.FillPath(sb, gpS);
            }

            // Card body
            using (var bg = new SolidBrush(ControlPaint.LightLight(light)))
            using (var pen = new Pen(border, 2))
            using (var gp = Round(card, CornerRadius))
            {
                g.FillPath(bg, gp);
                g.DrawPath(pen, gp);
            }

            // Viền focus (dotted)
            if (Focused)
            {
                var focus = card; focus.Inflate(-3, -3);
                using var fp = new Pen(Color.FromArgb(90, 0, 120, 215), 2) { DashStyle = DashStyle.Dot };
                g.DrawPath(fp, Round(focus, CornerRadius - 3));
            }

            int pad = 12;
            int x = card.X + pad;
            int y = card.Y + pad;

            // Header: mã bàn
            TextRenderer.DrawText(g, MaBan, new Font(Font, FontStyle.Bold), new Point(x, y), Color.Black);

            // Badge trạng thái
            string badgeStr = Status switch
            {
                TableStatus.Trong => "Trống",
                TableStatus.DangDung => "Đang dùng",
                _ => "Đã đặt"
            };
            var szBadge = TextRenderer.MeasureText(badgeStr, new Font(Font, FontStyle.Bold));
            var rcBadge = new Rectangle(card.Right - pad - szBadge.Width - 16, y - 2, szBadge.Width + 12, 24);
            using (var b = new SolidBrush(badgeBack))
            using (var gpB = Round(rcBadge, 12))
            {
                g.FillPath(b, gpB);
                TextRenderer.DrawText(g, badgeStr, new Font(Font, FontStyle.Bold),
                    rcBadge, badgeText,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            y += 30;

            // dòng info
            DrawLine(g, ref y, x, "👥", $"{SoKhach}/{SucChua} khách", border);
            if (Status == TableStatus.DangDung)
                DrawLine(g, ref y, x, "⏲", $"{SoPhut} phút", border);
            else if (Status == TableStatus.DaDat)
                DrawLine(g, ref y, x, "🕒", string.IsNullOrWhiteSpace(GioDat) ? "Đặt" : $"Đặt lúc {GioDat}", border);
            else
                DrawLine(g, ref y, x, "🕒", "0 phút", Color.FromArgb(120, 120, 120));

            DrawLine(g, ref y, x, "💲", MoneyVN(Tien), border);

            // Nút "Chuyển" (chỉ khi đang dùng)
            _rcTransfer = Rectangle.Empty;
            if (Status == TableStatus.DangDung)
            {
                y += 8;
                var w = Width - pad * 2;
                var h = 34;
                _rcTransfer = new Rectangle(x, y, w, h);

                var btnBorder = _hoverTransfer ? ControlPaint.Dark(border, .1f) : border;
                using var b = new SolidBrush(Color.White);
                using var p = new Pen(btnBorder, 2) { Alignment = PenAlignment.Inset };
                using var gp = Round(_rcTransfer, 16);
                g.FillPath(b, gp);
                g.DrawPath(p, gp);

                // icon swap
                var ic = new Rectangle(_rcTransfer.X + 10, _rcTransfer.Y + 7, 20, 20);
                DrawSwapIcon(g, ic, btnBorder);

                var txtRect = new Rectangle(ic.Right + 6, _rcTransfer.Y, _rcTransfer.Width - (ic.Right + 12 - _rcTransfer.X), _rcTransfer.Height);
                TextRenderer.DrawText(g, "Chuyển", Font, txtRect, Color.Black,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
        }

        // ===== Interaction (button-like) =====
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool hCard = ClientRectangle.Contains(e.Location);
            bool hTransfer = !_rcTransfer.IsEmpty && _rcTransfer.Contains(e.Location);

            if (hCard != _hoverCard || hTransfer != _hoverTransfer)
            {
                _hoverCard = hCard;
                _hoverTransfer = hTransfer;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hoverCard = _hoverTransfer = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus(); // để nhận phím
            _pressedCard = ClientRectangle.Contains(e.Location);
            _pressedTransfer = !_rcTransfer.IsEmpty && _rcTransfer.Contains(e.Location);
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            bool upInCard = ClientRectangle.Contains(e.Location);
            bool upInTransfer = !_rcTransfer.IsEmpty && _rcTransfer.Contains(e.Location);

            if (_pressedTransfer && upInTransfer && Status == TableStatus.DangDung)
                TransferClicked?.Invoke(this, EventArgs.Empty);
            else if (_pressedCard && upInCard)
            {
                CardClicked?.Invoke(this, EventArgs.Empty);
                OnClick(EventArgs.Empty); // phát sinh Click chuẩn
            }

            _pressedCard = _pressedTransfer = false;
            Invalidate();
        }

        protected override bool IsInputKey(Keys keyData) => true;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Space || keyData == Keys.Enter)
            {
                CardClicked?.Invoke(this, EventArgs.Empty);
                OnClick(EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ===== Helpers =====
        private static void GetPalette(TableStatus st, out Color border, out Color light, out Color badgeBack, out Color badgeText)
        {
            switch (st)
            {
                case TableStatus.Trong:
                    border = Color.FromArgb(46, 125, 50);
                    light = Color.FromArgb(225, 245, 233);
                    badgeBack = border;
                    badgeText = Color.White;
                    break;
                case TableStatus.DangDung:
                    border = Color.FromArgb(211, 47, 47);
                    light = Color.FromArgb(255, 235, 238);
                    badgeBack = border;
                    badgeText = Color.White;
                    break;
                default: // DaDat
                    border = Color.FromArgb(251, 140, 0);
                    light = Color.FromArgb(255, 249, 196);
                    badgeBack = Color.FromArgb(204, 141, 0);
                    badgeText = Color.White;
                    break;
            }
        }

        private static void DrawLine(Graphics g, ref int y, int x, string emoji, string text, Color tone)
        {
            var icRect = new Rectangle(x, y, 22, 22);
            TextRenderer.DrawText(g, emoji, new Font("Segoe UI Emoji", 10f), icRect, tone,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            var txtRect = new Rectangle(x + 24, y, 130, 22);
            TextRenderer.DrawText(g, text, new Font("Segoe UI", 9f), txtRect, Color.Black,
                TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            y += 26;
        }

        private static string MoneyVN(decimal money)
        {
            var ci = new CultureInfo("vi-VN");
            return string.Format(ci, "{0:#,0} đ", money);
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

        private static void DrawSwapIcon(Graphics g, Rectangle r, Color c)
        {
            using var p = new Pen(c, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            // trái -> phải
            g.DrawArc(p, r.X + 2, r.Y + 4, r.Width - 8, r.Height - 12, 200, 160);
            g.DrawLine(p, r.Right - 7, r.Y + r.Height / 2 - 5, r.Right - 3, r.Y + r.Height / 2 - 5);
            // phải -> trái
            g.DrawArc(p, r.X + 2, r.Y + 6, r.Width - 8, r.Height - 12, 20, 160);
            g.DrawLine(p, r.X + 3, r.Y + r.Height / 2 + 5, r.X + 7, r.Y + r.Height / 2 + 5);
        }
    }
}
