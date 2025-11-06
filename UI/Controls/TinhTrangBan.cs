using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace UI.Controls
{
    [ToolboxItem(true)]
    [SupportedOSPlatform("windows")]
    public class TinhTrangBan : UserControl
    {
        public enum TableState { Available, InUse, Reserved, Cleaning }

        // ====== Data ======
        private string _tableCode = "A01";
        private TableState _status = TableState.Available;
        private int _capacity = 4;
        private int _minutesUsed = 0;  
        private string _reservedTime = "13:00";
        private decimal _price = 0m;

        // ====== Appearance ======
        private int _radius = 20;

        private bool _hover;
        private float _hoverT = 0f;
        private readonly Timer _anim;

        public TinhTrangBan()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            UpdateStyles();

            Size = new Size(280, 140);
            Padding = new Padding(10, 10, 10, 10);
            Margin = new Padding(8);
            Font = new Font("Segoe UI", 11f);
            ForeColor = Color.FromArgb(17, 24, 39);
            BackColor = Color.Transparent;
            _anim = new Timer { Interval = 20 };
            _anim.Tick += (s, e) =>
            {
                float target = _hover ? 1f : 0f;
                _hoverT = Lerp(_hoverT, target, 0.15f);
                if (Math.Abs(_hoverT - target) < 0.01f) { _hoverT = target; _anim.Stop(); }
                Invalidate();
            };

            MouseEnter += (s, e) => { _hover = true; if (!_anim.Enabled) _anim.Start(); };
            MouseLeave += (s, e) => { _hover = false; if (!_anim.Enabled) _anim.Start(); };
            
            // Đảm bảo control có thể nhận click events
            SetStyle(ControlStyles.Selectable, true);
        }

        // ====== Public API ======
        [Category("Data")]
        public string TableCode
        {
            get => _tableCode; set { _tableCode = value ?? ""; Invalidate(); }
        }

        [Category("Data")]
        public TableState Status
        {
            get => _status; set { _status = value; Invalidate(); }
        }

        [Category("Data")]
        public int Capacity
        {
            get => _capacity; set { _capacity = value; Invalidate(); }
        }

        [Category("Data")]
        public int MinutesUsed
        {
            get => _minutesUsed; set { _minutesUsed = value; Invalidate(); }
        }

        [Category("Data")]
        public string ReservedTime
        {
            get => _reservedTime; set { _reservedTime = value ?? ""; Invalidate(); }
        }

        [Category("Data")]
        public decimal Price
        {
            get => _price; set { _price = value; Invalidate(); }
        }

        [Category("Appearance")]
        public int CornerRadius
        {
            get => _radius; set { _radius = Math.Max(10, value); Invalidate(); }
        }

        // ====== Paint (vẽ thẻ + hover grow & darken + nội dung) ======
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using var buffer = new Bitmap(Width, Height);
            using var g = Graphics.FromImage(buffer);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            var (primaryColor, backgroundColor, accentColor, textColor) = GetModernTheme(_status);

            // Hover effect nhẹ nhàng hơn
            float scale = 1.0f + (0.04f * _hoverT); // Giảm scale effect
            int grow = (int)(4 * _hoverT); // Giảm grow effect
            var cardRect = new Rectangle(grow, grow, Width - 1 - grow * 2, Height - 1 - grow * 2);

            // Shadow effect nhẹ nhàng hơn khi hover
            if (_hoverT > 0)
            {
                int shadowOffset = (int)(3 * _hoverT);
                int shadowBlur = (int)(4 * _hoverT);
                using var shadowPath = RoundRect(new Rectangle(cardRect.X + shadowOffset, cardRect.Y + shadowOffset, cardRect.Width, cardRect.Height), _radius);
                using var shadowBrush = new SolidBrush(Color.FromArgb((int)(20 * _hoverT), Color.Black));
                g.FillPath(shadowBrush, shadowPath);
            }

            using (var path = RoundRect(cardRect, _radius))
            {
                // Gradient background với hover effect
                Color bgStart = backgroundColor;
                Color bgEnd = _hoverT > 0 ? 
                    Color.FromArgb(Math.Max(0, backgroundColor.A - 30), backgroundColor) : 
                    Color.FromArgb(Math.Max(0, backgroundColor.A - 10), backgroundColor);
                
                using var gradientBrush = new LinearGradientBrush(
                    cardRect, 
                    bgStart, 
                    bgEnd, 
                    135f);
                g.FillPath(gradientBrush, path);

                // Border với hover effect
                Color borderStart = primaryColor;
                Color borderEnd = _hoverT > 0 ? 
                    Color.FromArgb(255, primaryColor) : 
                    Color.FromArgb(200, primaryColor);
                
                using var borderBrush = new LinearGradientBrush(
                    cardRect,
                    borderStart,
                    borderEnd,
                    45f);
                
                float borderWidth = 2.0f + (0.5f * _hoverT); // Tăng độ dày border nhẹ khi hover
                using var pen = new Pen(borderBrush, borderWidth) { 
                    Alignment = PenAlignment.Inset, 
                    LineJoin = LineJoin.Round 
                };
                g.DrawPath(pen, path);
            }

            if (scale != 1.0f)
            {
                g.ScaleTransform(scale, scale);
                g.TranslateTransform((1 - scale) * Width / 2, (1 - scale) * Height / 2);
            }

            DrawModernContent(g, primaryColor, backgroundColor, accentColor, textColor);

            e.Graphics.DrawImageUnscaled(buffer, 0, 0);
        }

        // ====== Vẽ nội dung hiện đại ======
        private void DrawModernContent(Graphics g, Color primaryColor, Color backgroundColor, Color accentColor, Color textColor)
        {
            // Tính toán scale dựa trên kích thước thực tế
            float scaleX = Width / 280f;
            float scaleY = Height / 140f;
            float scale = Math.Min(scaleX, scaleY);
            if (scale > 1.0f) scale = 1.0f; // Không scale lên, chỉ scale xuống
            
            // Điều chỉnh font size dựa trên scale - tăng cỡ chữ lên một chút
            var fontHeader = new Font("Segoe UI", Math.Max(14f, 18f * scale), FontStyle.Bold, GraphicsUnit.Pixel);
            var fontSubheader = new Font("Segoe UI", Math.Max(12f, 15f * scale), FontStyle.Regular, GraphicsUnit.Pixel);
            var fontBody = new Font("Segoe UI", Math.Max(11f, 14f * scale), FontStyle.Regular, GraphicsUnit.Pixel);
            var fontCaption = new Font("Segoe UI", Math.Max(10f, 13f * scale), FontStyle.Regular, GraphicsUnit.Pixel);
            
            var colorTextPrimary = Color.FromArgb(31, 41, 55);
            var colorTextSecondary = Color.FromArgb(75, 85, 99);
            var colorTextMuted = Color.FromArgb(156, 163, 175);
            
            // Tính toán padding và spacing động
            int dynamicPadding = (int)(15 * scale);
            int headerHeight = (int)(35 * scale);
            int contentSpacing = (int)(20 * scale);
            
            // Header section với table code - đảm bảo chiều cao cố định
            var headerRect = new Rectangle(dynamicPadding, dynamicPadding, Width - dynamicPadding * 2, headerHeight);
            
            // Vẽ table code với background
            using (var headerPath = RoundRect(headerRect, (int)(8 * scale)))
            {
                using var headerBrush = new SolidBrush(Color.FromArgb(240, 248, 255));
                g.FillPath(headerBrush, headerPath);
                
                using var headerPen = new Pen(Color.FromArgb(219, 234, 254), 1f);
                g.DrawPath(headerPen, headerPath);
            }
            
            // Status indicator (góc phải trên) - vẽ trước để không ảnh hưởng layout
            string statusText = _status == TableState.Available ? "Trống" :
                              _status == TableState.InUse ? "Đang dùng" :
                              _status == TableState.Reserved ? "Đã đặt" : "Vệ sinh";
            
            var statusSize = g.MeasureString(statusText, fontCaption);
            int statusBadgeHeight = (int)(24 * scale); // Chiều cao cố định cho badge
            var statusRect = new RectangleF(
                Width - dynamicPadding - statusSize.Width - (int)(16 * scale), 
                dynamicPadding + (int)((headerHeight - statusBadgeHeight) / 2), 
                statusSize.Width + (int)(12 * scale), 
                statusBadgeHeight
            );
            
            // Vẽ status chip hiện đại
            using (var statusPath = RoundRect(Rectangle.Round(statusRect), (int)(10 * scale)))
            {
                using var statusBrush = new SolidBrush(accentColor);
                g.FillPath(statusBrush, statusPath);
            }
            
            g.DrawString(statusText, fontCaption, new SolidBrush(Color.White), statusRect, 
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            
            // Table code text - căn giữa trong header, không bị ảnh hưởng bởi badge
            var tableCodeRect = new RectangleF(headerRect.X + 10, headerRect.Y + (int)(8 * scale), headerRect.Width - 20, headerHeight - (int)(16 * scale));
            var tableCodeFormat = new StringFormat 
            { 
                Alignment = StringAlignment.Center, 
                LineAlignment = StringAlignment.Center 
            };
            g.DrawString(_tableCode, fontHeader, new SolidBrush(primaryColor), tableCodeRect, tableCodeFormat);
            
            // Content section - luôn bắt đầu từ cùng một vị trí Y cố định
            int contentY = dynamicPadding + headerHeight + (int)(12 * scale);
            int contentX = dynamicPadding + (int)(12 * scale);
            int availableHeight = Height - contentY - dynamicPadding;
            
            // Capacity info với icon
            string capacityText = $"👥 {_capacity} người";
            g.DrawString(capacityText, fontBody, new SolidBrush(colorTextPrimary), contentX, contentY);
            
            // Time info với icon
            string timeText = "";
            if (_status == TableState.InUse)
                timeText = $"⏱️ {_minutesUsed} phút";
            else if (_status == TableState.Reserved)
                timeText = $"📅 {_reservedTime}";
            else if (_status == TableState.Cleaning)
                timeText = "🧹 Đang vệ sinh";
            else
                timeText = "✨ Sẵn sàng";
                
            int timeY = contentY + contentSpacing;
            if (timeY + (int)(20 * scale) <= Height - dynamicPadding)
            {
                g.DrawString(timeText, fontBody, new SolidBrush(colorTextSecondary), contentX, timeY);
            }
            
            // Decorative elements - chỉ vẽ nếu có đủ không gian
            if (Height >= 100)
            {
                DrawDecorativeElements(g, primaryColor, accentColor, scale);
            }
        }
        
        private void DrawDecorativeElements(Graphics g, Color primaryColor, Color accentColor, float scale = 1.0f)
        {
            int dynamicPadding = (int)(15 * scale);
            // Vẽ các đường trang trí nhẹ
            using var pen = new Pen(Color.FromArgb(30, primaryColor), 1f);
            
            // Đường ngang nhẹ
            int lineY = dynamicPadding + (int)(40 * scale);
            if (lineY < Height - dynamicPadding)
            {
                g.DrawLine(pen, dynamicPadding + (int)(12 * scale), lineY, Width - dynamicPadding - (int)(12 * scale), lineY);
            }
            
            // Điểm nhấn góc
            var cornerSize = (int)(8 * scale);
            using var cornerBrush = new SolidBrush(accentColor);
            g.FillEllipse(cornerBrush, Width - dynamicPadding - (int)(20 * scale), Height - dynamicPadding - (int)(20 * scale), cornerSize, cornerSize);
        }
        

        // ====== Modern Theme System ======
        private (Color primary, Color background, Color accent, Color text) GetModernTheme(TableState st)
        {
            switch (st)
            {
                case TableState.Available:
                    return (
                        Color.FromArgb(34, 197, 94),        // Green-500 - Màu xanh lá tươi
                        Color.FromArgb(240, 253, 244),       // Green-50 - Nền xanh nhạt
                        Color.FromArgb(22, 163, 74),         // Green-600 - Xanh đậm hơn
                        Color.FromArgb(20, 83, 45)           // Green-800 - Chữ xanh đậm
                    );
                case TableState.InUse:
                    return (
                        Color.FromArgb(239, 68, 68),         // Red-500 - Màu đỏ
                        Color.FromArgb(254, 226, 226),       // Red-50 - Nền đỏ nhạt
                        Color.FromArgb(220, 38, 38),         // Red-600 - Đỏ đậm hơn
                        Color.FromArgb(153, 27, 27)          // Red-800 - Chữ đỏ đậm
                    );
                case TableState.Reserved:
                    return (
                        Color.FromArgb(251, 146, 60),        // Orange-500 - Màu cam
                        Color.FromArgb(255, 247, 237),       // Orange-50 - Nền cam nhạt
                        Color.FromArgb(234, 88, 12),         // Orange-600 - Cam đậm
                        Color.FromArgb(154, 52, 18)          // Orange-800 - Chữ cam đậm
                    );
                case TableState.Cleaning:
                    return (
                        Color.FromArgb(107, 114, 128),       // Gray-500 - Màu xám
                        Color.FromArgb(243, 244, 246),       // Gray-100 - Nền xám nhạt
                        Color.FromArgb(75, 85, 99),          // Gray-600 - Xám đậm hơn
                        Color.FromArgb(55, 65, 81)           // Gray-700 - Chữ xám đậm
                    );
                default:
                    return (
                        Color.FromArgb(107, 114, 128),      // Gray-500
                        Color.FromArgb(249, 250, 251),      // Gray-50
                        Color.FromArgb(75, 85, 99),         // Gray-600
                        Color.FromArgb(31, 41, 55)          // Gray-800
                    );
            }
        }

        // Đã loại bỏ MakeChip và MakeLabel vì không cần control con nữa

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
