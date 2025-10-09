using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace UI.Controls
{
    public enum TableStatus { Available, Reserved, InUse }
    [SupportedOSPlatform("windows")]
    public class SoDoBan_KhuVuc : Control
    {
        // ===== Data =====
        [Category("Data")] public string TableText { get; set; } = "Bàn A01";
        [Category("Data")] public string SubText { get; set; } = "4 chỗ";
        [Category("Data")] public string Zone { get; set; } = "Khu A";

        private TableStatus _status = TableStatus.Available;
        [Category("Data")]
        public TableStatus Status
        {
            get => _status;
            set { _status = value; Invalidate(); }
        }

        // ===== Appearance =====
        [Category("Appearance")] public int CornerRadius { get; set; } = 16;
        [Category("Appearance")] public int BorderThickness { get; set; } = 1;
        [Category("Appearance")] public Padding CardPadding { get; set; } = new Padding(12, 10, 12, 10);

        // Palette
        public Color BgAvailable { get; set; } = Color.FromArgb(220, 248, 235);   // xanh nhạt
        public Color BdAvailable { get; set; } = Color.FromArgb(144, 210, 185);

        public Color BgReserved { get; set; } = Color.FromArgb(225, 235, 255);    // xanh dương nhạt
        public Color BdReserved { get; set; } = Color.FromArgb(160, 185, 245);

        public Color BgInUse { get; set; } = Color.FromArgb(255, 226, 226);       // đỏ hồng nhạt
        public Color BdInUse { get; set; } = Color.FromArgb(240, 170, 170);

        public Color TextMain { get; set; } = Color.FromArgb(30, 30, 40);
        public Color TextSub { get; set; } = Color.FromArgb(90, 90, 110);

        private bool _hover, _pressed;
        private readonly Timer _animTimer;
        private float _hoverPct = 0f; // 0 -> 1

        public event EventHandler? TableClicked;

        public SoDoBan_KhuVuc()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            Cursor = Cursors.Hand;
            Size = new Size(140, 72);

            _animTimer = new Timer { Interval = 16 }; // ~60fps
            _animTimer.Tick += (s, e) =>
            {
                float target = _hover ? 1f : 0f;
                float step = 0.15f;
                if (Math.Abs(_hoverPct - target) <= step) { _hoverPct = target; _animTimer.Stop(); }
                else _hoverPct += (_hoverPct < target ? step : -step);
                Invalidate();
            };
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; _animTimer.Start(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; _animTimer.Start(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _pressed = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnClick(EventArgs e) { TableClicked?.Invoke(this, EventArgs.Empty); base.OnClick(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // base rect + hover scale
            int grow = (int)Math.Round(3 * _hoverPct); // phóng to vài px
            Rectangle rc = Rectangle.Inflate(ClientRectangle, grow, grow);
            rc.Width -= 1; rc.Height -= 1;

            // colors by status
            Color bg, bd;
            switch (Status)
            {
                case TableStatus.Reserved: bg = BgReserved; bd = BdReserved; break;
                case TableStatus.InUse: bg = BgInUse; bd = BdInUse; break;
                default: bg = BgAvailable; bd = BdAvailable; break;
            }

            // darken little on hover/press
            float darken = _pressed ? 0.12f : (_hoverPct * 0.06f);
            bg = Darken(bg, darken);
            bd = Darken(bd, darken);

            using (GraphicsPath path = RoundRect(rc, CornerRadius))
            using (var br = new SolidBrush(bg))
            using (var pen = new Pen(bd, BorderThickness))
            {
                g.FillPath(br, path);
                g.DrawPath(pen, path);
            }

            // text
            using var fTitle = new Font(Font.FontFamily, 10f, FontStyle.Bold);
            using var fSub = new Font(Font.FontFamily, 9f, FontStyle.Regular);
            using var sbMain = new SolidBrush(TextMain);
            using var sbSub = new SolidBrush(TextSub);

            var pad = CardPadding;
            var content = Rectangle.Inflate(rc, -pad.Left, -pad.Top);
            content.Width -= pad.Right - pad.Left;
            content.Height -= pad.Bottom - pad.Top;

            var titleRect = new Rectangle(content.X, content.Y, content.Width, fTitle.Height + 2);
            var subRect = new Rectangle(content.X, content.Y + fTitle.Height + 4, content.Width, fSub.Height + 2);

            g.DrawString(TableText, fTitle, sbMain, titleRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });
            g.DrawString(SubText, fSub, sbSub, subRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });
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
        private static Color Darken(Color c, float amount) // amount 0..0.3
        {
            int r = (int)(c.R * (1 - amount));
            int g = (int)(c.G * (1 - amount));
            int b = (int)(c.B * (1 - amount));
            return Color.FromArgb(c.A, Math.Max(0, r), Math.Max(0, g), Math.Max(0, b));
        }
    }
}
