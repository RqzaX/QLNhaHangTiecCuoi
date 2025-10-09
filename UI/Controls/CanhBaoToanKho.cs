// InventoryAlertItem.cs
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    public enum AlertLevel { Danger, Warning }

    [ToolboxItem(true)]
    [DefaultProperty(nameof(ItemName))]
    [DefaultEvent(nameof(ImportClicked))]
    [SupportedOSPlatform("windows")]
    public class CanhBaoToanKho : Control
    {
        // ===== Data =====
        private string _itemName = "Tôm sú";
        private double _stock = 2.5;       // tồn kho hiện tại
        private double _minStock = 10;     // mức tối thiểu
        private string _unit = "kg";
        private AlertLevel _level = AlertLevel.Danger;

        [Category("Data"), DefaultValue("Tôm sú")]
        public string ItemName { get => _itemName; set { _itemName = value ?? ""; Invalidate(); } }

        [Category("Data"), DefaultValue(2.5)]
        public double Stock { get => _stock; set { _stock = Math.Max(0, value); Invalidate(); } }

        [Category("Data"), DefaultValue(10d)]
        public double MinStock { get => _minStock; set { _minStock = Math.Max(0, value); Invalidate(); } }

        [Category("Data"), DefaultValue("kg")]
        public string Unit { get => _unit; set { _unit = value ?? ""; Invalidate(); } }

        [Category("Data"), DefaultValue(typeof(AlertLevel), nameof(AlertLevel.Danger))]
        public AlertLevel Level { get => _level; set { _level = value; Invalidate(); } }

        // ===== Style =====
        [Category("Appearance"), DefaultValue(18)]
        public int CornerRadius { get; set; } = 18;

        [Category("Appearance"), DefaultValue(typeof(Color), "255,241,242")]
        public Color DangerBack { get; set; } = Color.FromArgb(255, 241, 242);    // đỏ nhạt

        [Category("Appearance"), DefaultValue(typeof(Color), "254,251,235")]
        public Color WarningBack { get; set; } = Color.FromArgb(254, 251, 235);   // vàng nhạt

        [Category("Appearance"), DefaultValue(typeof(Color), "244,63,94")]
        public Color DangerAccent { get; set; } = Color.FromArgb(244, 63, 94);    // đỏ

        [Category("Appearance"), DefaultValue(typeof(Color), "234,179,8")]
        public Color WarningAccent { get; set; } = Color.FromArgb(234, 179, 8);   // vàng

        [Category("Appearance"), DefaultValue(typeof(Color), "230,232,236")]
        public Color BorderColor { get; set; } = Color.FromArgb(230, 232, 236);

        [Category("Appearance"), DefaultValue(typeof(Color), "247,247,247")]
        public Color ButtonBack { get; set; } = Color.FromArgb(247, 247, 247);

        // ===== Events =====
        [Category("Action")] public event EventHandler ImportClicked;

        // ===== Runtime =====
        private Rectangle _rcButton = Rectangle.Empty;
        private bool _hover, _hoverBtn, _pressBtn;

        public CanhBaoToanKho()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI", 10f);
            Height = 80;
            Size = new Size(1120, 80);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _hover = true;
            bool hb = _rcButton.Contains(e.Location);
            if (hb != _hoverBtn) { _hoverBtn = hb; Invalidate(); }
            base.OnMouseMove(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = _hoverBtn = _pressBtn = false; Invalidate();
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _pressBtn = _rcButton.Contains(e.Location);
            Invalidate();
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool fire = _pressBtn && _rcButton.Contains(e.Location);
            _pressBtn = false; Invalidate();
            if (fire) ImportClicked?.Invoke(this, EventArgs.Empty);
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;

            var card = ClientRectangle; card.Inflate(-1, -1);
            var accent = Level == AlertLevel.Danger ? DangerAccent : WarningAccent;
            var fill = Level == AlertLevel.Danger ? DangerBack : WarningBack;

            // background + viền
            using (var bg = new SolidBrush(fill))
            using (var pen = new Pen(_hover ? ControlPaint.Dark(BorderColor, .05f) : BorderColor))
            using (var gp = Round(card, CornerRadius))
            {
                g.FillPath(bg, gp);
                g.DrawPath(pen, gp);
            }

            int padX = 18, padY = 14;
            int x = card.X + padX, y = card.Y + padY;

            // icon hộp trong vòng tròn mềm
            var icCircle = new Rectangle(x, y, 34, 34);
            using (var soft = new SolidBrush(Color.FromArgb(36, accent.R, accent.G, accent.B)))
                g.FillEllipse(soft, icCircle);
            DrawBoxIcon(g, new Rectangle(icCircle.X + 7, icCircle.Y + 7, 20, 20), accent);

            // text
            int textLeft = icCircle.Right + 12;
            int textWidth = card.Width - textLeft - 160;
            TextRenderer.DrawText(g, ItemName, new Font(Font, FontStyle.Bold),
                new Rectangle(textLeft, y - 2, textWidth, 24), Color.Black,
                TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);

            string sub = $"Tồn kho: {FmtNum(Stock)} {Unit} (Mức tối thiểu: {FmtNum(MinStock)} {Unit})";
            TextRenderer.DrawText(g, sub, new Font(Font.FontFamily, 9f), new Rectangle(textLeft, y + 22, textWidth, 22),
                Color.FromArgb(105, 112, 119), TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);

            // nút Nhập kho
            int btnW = 120, btnH = 36;
            _rcButton = new Rectangle(card.Right - padX - btnW, card.Y + (card.Height - btnH) / 2, btnW, btnH);
            using (var b = new SolidBrush(_hoverBtn ? Color.White : ButtonBack))
            using (var p = new Pen(_hoverBtn ? accent : BorderColor, 2) { Alignment = PenAlignment.Inset })
            using (var gpBtn = Round(_rcButton, btnH / 2))
            {
                g.FillPath(b, gpBtn);
                g.DrawPath(p, gpBtn);
            }
            // chữ
            var textCol = _hoverBtn ? accent : Color.Black;
            if (_pressBtn) { _rcButton.Offset(0, 1); }
            TextRenderer.DrawText(g, "Nhập kho", new Font(Font, FontStyle.Bold),
                _rcButton, textCol, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        // ===== Helpers =====
        private static string FmtNum(double n)
            => n % 1 == 0 ? n.ToString("0", CultureInfo.InvariantCulture)
                          : n.ToString("#,0.##", CultureInfo.InvariantCulture);

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

        private static void DrawBoxIcon(Graphics g, Rectangle r, Color c)
        {
            using var p = new Pen(c, 2) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };
            // lập phương đơn giản
            var top = new Point(r.X + r.Width / 2, r.Y);
            var left = new Point(r.X, r.Y + r.Height / 3);
            var right = new Point(r.Right, r.Y + r.Height / 3);
            var bottomLeft = new Point(r.X, r.Bottom);
            var bottomRight = new Point(r.Right, r.Bottom);
            var mid = new Point(r.X + r.Width / 2, r.Y + r.Height / 3);

            g.DrawPolygon(p, new[] { top, right, bottomRight, bottomLeft, left, top });
            g.DrawLine(p, left, bottomLeft);
            g.DrawLine(p, right, bottomRight);
            g.DrawLine(p, top, mid);
        }
    }
}
