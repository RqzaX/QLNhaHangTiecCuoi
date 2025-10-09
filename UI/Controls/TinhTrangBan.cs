using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace UI.Controls
{
    [ToolboxItem(true)]
    [SupportedOSPlatform("windows")]
    public class TinhTrangBan : UserControl
    {
        public enum TableState { Available, InUse, Reserved }

        // ====== Data ======
        private string _tableCode = "A01";
        private TableState _status = TableState.Available;
        private int _capacity = 4;
        private int _minutesUsed = 0;           // dùng khi InUse
        private string _reservedTime = "13:00"; // dùng khi Reserved
        private decimal _price = 0m;

        // ====== Appearance ======
        private int _radius = 20;

        // Hover animation
        private bool _hover;
        private float _hoverT = 0f; // 0..1
        private readonly Timer _anim;

        // UI pieces (chỉ để trang trí/hiển thị)
        private readonly Button _btnCode;     // chip mã bàn (trái trên)
        private readonly Button _btnStatus;   // chip trạng thái (phải trên)
        private readonly Label _lblCapacity;
        private readonly Label _lblTime;
        private readonly Label _lblPrice;

        public TinhTrangBan()
        {
            // DoubleBuffer chuẩn
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            UpdateStyles();

            Size = new Size(220, 140);
            Padding = new Padding(14, 14, 14, 12);
            Margin = new Padding(10);
            Font = new Font("Segoe UI", 10f);
            ForeColor = Color.FromArgb(17, 24, 39);
            BackColor = Color.Transparent;

            // Buttons "chip"
            _btnCode = MakeChip("A01", Color.White, Color.FromArgb(51, 65, 85));
            _btnStatus = MakeChip("Trống", Color.FromArgb(16, 185, 129), Color.White);
            Controls.Add(_btnCode);
            Controls.Add(_btnStatus);

            // Labels nội dung
            _lblCapacity = MakeLabel();
            _lblTime = MakeLabel();
            _lblPrice = MakeLabel(true);
            Controls.AddRange(new Control[] { _lblCapacity, _lblTime, _lblPrice });

            // Layout lần đầu
            LayoutContent();

            // Anim
            _anim = new Timer { Interval = 16 };
            _anim.Tick += (s, e) =>
            {
                float target = _hover ? 1f : 0f;
                _hoverT = Lerp(_hoverT, target, 0.20f);
                if (Math.Abs(_hoverT - target) < 0.02f) { _hoverT = target; _anim.Stop(); }
                Invalidate();
            };

            MouseEnter += (s, e) => { _hover = true; if (!_anim.Enabled) _anim.Start(); };
            MouseLeave += (s, e) => { _hover = false; if (!_anim.Enabled) _anim.Start(); };
            Resize += (s, e) => LayoutContent();

            // Không cần Click – chỉ hiển thị
            UpdateTexts();
        }

        // ====== Public API ======
        [Category("Data")]
        public string TableCode
        {
            get => _tableCode; set { _tableCode = value ?? ""; UpdateTexts(); Invalidate(); }
        }

        [Category("Data")]
        public TableState Status
        {
            get => _status; set { _status = value; UpdateTexts(); Invalidate(); }
        }

        [Category("Data")]
        public int Capacity
        {
            get => _capacity; set { _capacity = value; UpdateTexts(); Invalidate(); }
        }

        [Category("Data")]
        public int MinutesUsed
        {
            get => _minutesUsed; set { _minutesUsed = value; UpdateTexts(); Invalidate(); }
        }

        [Category("Data")]
        public string ReservedTime
        {
            get => _reservedTime; set { _reservedTime = value ?? ""; UpdateTexts(); Invalidate(); }
        }

        [Category("Data")]
        public decimal Price
        {
            get => _price; set { _price = value; UpdateTexts(); Invalidate(); }
        }

        [Category("Appearance")]
        public int CornerRadius
        {
            get => _radius; set { _radius = Math.Max(10, value); Invalidate(); }
        }

        // ====== Paint (vẽ thẻ + hover grow & darken) ======
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // vẽ vào buffer nội bộ để mượt
            using var buffer = new Bitmap(Width, Height);
            using var g = Graphics.FromImage(buffer);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            // theme theo trạng thái
            var (border, fill, accent) = ThemeByState(_status);

            // hover: phồng to & đậm màu
            int grow = (int)(4 * _hoverT); // phồng ~4px
            var cardRect = new Rectangle(-grow, -grow, Width - 1 + grow * 2, Height - 1 + grow * 2);

            Color fillDark = Blend(fill, border, 0.12 * _hoverT);

            using (var path = RoundRect(cardRect, _radius + grow))
            {
                using var sb = new SolidBrush(fillDark);
                g.FillPath(sb, path);

                using var pen = new Pen(border, 2.0f) { Alignment = PenAlignment.Inset, LineJoin = LineJoin.Round };
                g.DrawPath(pen, path);
            }

            // đổ ra màn hình
            e.Graphics.DrawImageUnscaled(buffer, 0, 0);
        }

        // ====== Layout & Texts ======
        private void LayoutContent()
        {
            // chip mã bàn (trái trên)
            _btnCode.Location = new Point(Padding.Left, Padding.Top);
            _btnCode.AutoSize = true;

            // chip trạng thái (phải trên)
            _btnStatus.AutoSize = true;
            _btnStatus.Location = new Point(
                Width - Padding.Right - _btnStatus.Width,
                Padding.Top
            );

            int startY = _btnCode.Bottom + 10;
            int leftX = Padding.Left;

            _lblCapacity.SetBounds(leftX, startY, Width - Padding.Horizontal, 24);
            _lblTime.SetBounds(leftX, _lblCapacity.Bottom, Width - Padding.Horizontal, 24);
            _lblPrice.SetBounds(leftX, _lblTime.Bottom, Width - Padding.Horizontal, 26);
        }

        private void UpdateTexts()
        {
            _btnCode.Text = _tableCode;

            var (border, fill, accent) = ThemeByState(_status);
            _btnStatus.Text = _status == TableState.Available ? "Trống" :
                              _status == TableState.InUse ? "Đang dùng" : "Đã đặt";
            _btnStatus.BackColor = accent;
            _btnStatus.ForeColor = Color.White;

            _lblCapacity.Text = $"Sức  {_capacity}  chứa người";

            if (_status == TableState.InUse)
                _lblTime.Text = $"Thời  {_minutesUsed}  gian phút";
            else if (_status == TableState.Reserved)
                _lblTime.Text = $"Giờ  đặt  {_reservedTime}";
            else
                _lblTime.Text = " ";

            _lblPrice.Text = $"$  Giá trị   {(_price <= 0 ? "0 đ" : string.Format("{0:n0} đ", _price))}";
            _lblPrice.ForeColor = accent;

            LayoutContent();
        }

        // ====== Helpers ======
        private (Color border, Color fill, Color accent) ThemeByState(TableState st)
        {
            if (st == TableState.Available)
                return (Color.FromArgb(34, 197, 94), Color.FromArgb(229, 250, 238), Color.FromArgb(16, 185, 129));
            if (st == TableState.InUse)
                return (Color.FromArgb(239, 68, 68), Color.FromArgb(255, 235, 238), Color.FromArgb(220, 38, 38));
            return (Color.FromArgb(245, 158, 11), Color.FromArgb(255, 247, 214), Color.FromArgb(245, 158, 11));
        }

        private static Button MakeChip(string text, Color bg, Color fg)
        {
            var b = new Button
            {
                Text = text,
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                BackColor = bg,
                ForeColor = fg,
                Padding = new Padding(10, 2, 10, 2),
                TabStop = false
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = bg;
            b.FlatAppearance.MouseDownBackColor = bg;

            // bo tròn pill
            b.Resize += (s, e) =>
            {
                var btn = (Button)s!;
                var path = new GraphicsPath();
                int r = btn.Height;
                path.AddArc(0, 0, r, r, 90, 180);
                path.AddArc(btn.Width - r, 0, r, r, 270, 180);
                path.CloseFigure();
                btn.Region = new Region(path);
            };
            return b;
        }

        private static Label MakeLabel(bool strong = false)
        {
            return new Label
            {
                AutoSize = false,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = strong ? Color.FromArgb(55, 65, 81) : Color.FromArgb(107, 114, 128),
                Font = new Font("Segoe UI", strong ? 10f : 9f, strong ? FontStyle.Bold : FontStyle.Regular),
                BackColor = Color.Transparent
            };
        }

        private static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var gp = new GraphicsPath();
            if (radius <= 0) { gp.AddRectangle(r); gp.CloseFigure(); return gp; }
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private static Color Blend(Color a, Color b, double t)
        {
            int r = (int)(a.R + (b.R - a.R) * t);
            int g = (int)(a.G + (b.G - a.G) * t);
            int bl = (int)(a.B + (b.B - a.B) * t);
            return Color.FromArgb(Clamp(r), Clamp(g), Clamp(bl));
        }
        private static int Clamp(int v) => v < 0 ? 0 : (v > 255 ? 255 : v);
        private static float Lerp(float a, float b, float t) => a + (b - a) * t;
    }
}
