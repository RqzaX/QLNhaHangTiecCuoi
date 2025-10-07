// SlidingClock.cs (FIXED for Designer)
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    [ToolboxItem(true)]
    public class SlidingClock : Control
    {
        private static readonly Color DefaultTimeColor = Color.FromArgb(0x0F, 0x17, 0x2A);
        private static readonly Color DefaultDateColor = Color.FromArgb(0x47, 0x55, 0x69);
        private static readonly Color DefaultBackColor = Color.Transparent;
        private const int DefaultFps = 30;

        private System.Windows.Forms.Timer _timer;   // tạo khi runtime
        private readonly SlidingDigit _sec10;
        private readonly SlidingDigit _sec01;

        private string _lastTime = "";
        private int _fps = DefaultFps;
        private Color _timeColor = DefaultTimeColor;
        private Color _dateColor = DefaultDateColor;
        private Color _background = DefaultBackColor;

        public SlidingClock()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            Font = new Font("Segoe UI Semibold", 32f, FontStyle.Bold);
            ForeColor = _timeColor;
            BackColor = _background;

            // 2 digit giây – an toàn ở Designer
            _sec10 = new SlidingDigit { ForeColor = _timeColor };
            _sec01 = new SlidingDigit { ForeColor = _timeColor };
            Controls.Add(_sec10);
            Controls.Add(_sec01);

            // KHÔNG khởi tạo timer ở đây để tránh Designer crash
            // Cập nhật text ban đầu
            _lastTime = DateTime.Now.ToString("HH:mm:ss");
        }

        // Helper chuẩn để nhận biết design-time
        private bool IsDesignMode =>
            (Site?.DesignMode ?? false) || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // Chỉ tạo/khởi động timer khi runtime
            if (!IsDesignMode)
            {
                _timer = new System.Windows.Forms.Timer();
                _timer.Tick += OnTick;
                ApplyFps();
                _timer.Start();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _timer != null)
            {
                _timer.Stop();
                _timer.Tick -= OnTick;
                _timer.Dispose();
                _timer = null;
            }
            base.Dispose(disposing);
        }

        // ========== Properties with Designer serialization ==========
        [Category("Behavior"), Description("Khung hình/giây của hiệu ứng."), DefaultValue(DefaultFps)]
        public int FPS
        {
            get => _fps;
            set { _fps = Math.Max(1, value); ApplyFps(); }
        }
        public bool ShouldSerializeFPS() => _fps != DefaultFps;
        public void ResetFPS() => FPS = DefaultFps;

        [Category("Appearance"), Description("Màu phần giờ:phút:giây."), DefaultValue(typeof(Color), "15,23,42")]
        public Color TimeColor
        {
            get => _timeColor;
            set { _timeColor = value; _sec10.ForeColor = value; _sec01.ForeColor = value; Invalidate(); }
        }
        public bool ShouldSerializeTimeColor() => _timeColor != DefaultTimeColor;
        public void ResetTimeColor() => TimeColor = DefaultTimeColor;

        [Category("Appearance"), Description("Màu phần ngày/tháng/năm."), DefaultValue(typeof(Color), "71,85,105")]
        public Color DateColor
        {
            get => _dateColor;
            set { _dateColor = value; Invalidate(); }
        }
        public bool ShouldSerializeDateColor() => _dateColor != DefaultDateColor;
        public void ResetDateColor() => DateColor = DefaultDateColor;

        [Category("Appearance"), Description("Màu nền control."), DefaultValue(typeof(Color), "Transparent")]
        public override Color BackColor
        {
            get => _background;
            set { _background = value; Invalidate(); }
        }
        public bool ShouldSerializeBackColor() => _background != DefaultBackColor;
        public void ResetBackColor() => BackColor = DefaultBackColor;

        // ========== Layout ==========
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutDigits();
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            _sec10.SetBaseFont(Font);
            _sec01.SetBaseFont(Font);
            LayoutDigits();
        }

        private void LayoutDigits()
        {
            // đo bằng TextRenderer (không cần Graphics context – an toàn Designer)
            string hhmm = DateTime.Now.ToString("HH:mm:");
            Size hhmmSize = TextRenderer.MeasureText(hhmm, Font, Size.Empty, TextFormatFlags.NoPadding);

            int padding = 4;
            Size digitSize = TextRenderer.MeasureText("8", Font, Size.Empty, TextFormatFlags.NoPadding);
            int digitW = (int)(digitSize.Width * 0.9f);
            int digitH = digitSize.Height;

            int y = Padding.Top + 2;

            _sec10.Bounds = new Rectangle(Padding.Left + hhmmSize.Width + padding, y, digitW, digitH);
            _sec01.Bounds = new Rectangle(_sec10.Right, y, digitW, digitH);

            Invalidate();
        }

        // ========== Painting ==========
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using (var bg = new SolidBrush(BackColor))
                g.FillRectangle(bg, ClientRectangle);

            var now = IsDesignMode ? new DateTime(2025, 1, 1, 12, 34, 56) : DateTime.Now;

            using (var br = new SolidBrush(TimeColor))
            {
                var pt = new PointF(Padding.Left, Padding.Top + 2);
                g.DrawString(now.ToString("HH:mm:"), Font, br, pt);
            }

            int timeHeight = TextRenderer.MeasureText("Mg", Font).Height;
            using (var dateFont = new Font(Font.FontFamily, Math.Max(10f, Font.Size * 0.45f), FontStyle.Regular))
            using (var br2 = new SolidBrush(DateColor))
            {
                float y = Padding.Top + timeHeight + 6;
                g.DrawString(now.ToString("dd / MM yyyy"), dateFont, br2, Padding.Left, y);
            }
        }

        // ========== Timer ==========
        private void OnTick(object sender, EventArgs e)
        {
            var t = DateTime.Now;
            var sec = t.ToString("ss");
            _sec10.SetDigit(sec[0]);
            _sec01.SetDigit(sec[1]);

            // chỉ invalidate khi giây đổi để nhẹ
            string current = t.ToString("HH:mm:ss");
            if (!string.Equals(_lastTime, current))
            {
                _lastTime = current;
                Invalidate();
            }
        }

        private void ApplyFps()
        {
            if (_timer != null)
                _timer.Interval = (int)Math.Round(1000.0 / Math.Max(1, _fps));
        }

        // ========== Nested: một chữ số trượt ==========
        private class SlidingDigit : Control
        {
            private char _current = '0';
            private char _next = '0';
            private bool _animating;
            private float _progress; // 0..1
            private readonly System.Windows.Forms.Timer _animTimer;
            private int _fps = 30;
            private int _durationMs = 220;
            private Font _baseFont;

            public SlidingDigit()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);
                TabStop = false;

                _baseFont = new Font("Segoe UI Semibold", 32f, FontStyle.Bold);

                _animTimer = new System.Windows.Forms.Timer();
                _animTimer.Interval = 1000 / _fps;
                _animTimer.Tick += (s, e) =>
                {
                    float step = (float)_animTimer.Interval / _durationMs;
                    _progress += step;
                    if (_progress >= 1f)
                    {
                        _progress = 1f;
                        _animTimer.Stop();
                        _current = _next;
                        _animating = false;
                    }
                    Invalidate();
                };
            }

            public void SetBaseFont(Font f)
            {
                if (_baseFont != null) _baseFont.Dispose();
                _baseFont = new Font(f.FontFamily, f.Size, f.Style);
                Invalidate();
            }

            public void SetDigit(char c)
            {
                if (c == _current && !_animating) return;
                if (c == _current && _animating) return;
                _next = c;
                _progress = 0f;
                _animating = true;
                _animTimer.Start();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                var rect = ClientRectangle;
                using (var bg = new SolidBrush(Color.Transparent))
                    g.FillRectangle(bg, rect);

                if (!_animating)
                {
                    DrawDigit(g, _current, 0, 255);
                    return;
                }

                int h = rect.Height;
                float yOld = -_progress * h;         // số cũ đi lên
                float yNew = (1f - _progress) * h;   // số mới từ dưới lên
                int alphaOld = (int)(255 * (1f - _progress));
                int alphaNew = (int)(255 * (_progress));

                DrawDigit(g, _current, yOld, alphaOld);
                DrawDigit(g, _next, yNew, alphaNew);
            }

            private void DrawDigit(Graphics g, char c, float offsetY, int alpha)
            {
                var rect = ClientRectangle;
                rect.Offset(0, (int)offsetY);

                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (var br = new SolidBrush(Color.FromArgb(Math.Max(0, Math.Min(255, alpha)), ForeColor)))
                {
                    float em = _baseFont.SizeInPoints * g.DpiY / 72f;
                    path.AddString(c.ToString(), _baseFont.FontFamily, (int)_baseFont.Style, em, rect, sf);
                    g.FillPath(br, path);
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (_baseFont != null) _baseFont.Dispose();
                    if (_animTimer != null) _animTimer.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}
