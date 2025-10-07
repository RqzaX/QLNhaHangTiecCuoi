// RainbowTitle.cs
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class RainbowTitle : Control
    {
        private readonly System.Windows.Forms.Timer _timer;
        private float _phase;                 // 0..1, dịch chuyển màu
        private int _fps = 30;                // khung hình/giây
        private float _cyclesPerSecond = 0.25f; // số vòng rainbow/giây
        private float _saturation = 1.0f;     // 0..1
        private float _value = 1.0f;          // 0..1 (brightness)
        private bool _drawShadow = true;
        private bool _drawOutline = true;
        private Color _outlineColor = Color.FromArgb(60, 0, 0, 0);

        public RainbowTitle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            Text = "Hệ thống quản lý Nhà Hàng – Tiệc Cưới";
            Font = new Font("Segoe UI Semibold", 26f, FontStyle.Bold);
            ForeColor = Color.White;

            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += OnTick;
            ApplyFps(); // set Interval theo _fps
            if (!DesignMode) _timer.Start();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!DesignMode) _timer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _timer?.Dispose();
            base.Dispose(disposing);
        }

        private void OnTick(object sender, EventArgs e)
        {
            // tiến pha dựa trên cycles/second và fps
            float delta = _cyclesPerSecond / Math.Max(1, _fps);
            _phase += delta;
            while (_phase > 1f) _phase -= 1f;
            Invalidate();
        }

        // ========== Public Properties ==========
        [Category("Behavior"), Description("Khung hình/giây của hiệu ứng."), DefaultValue(30)]
        public int FPS
        {
            get { return _fps; }
            set
            {
                if (value < 1) value = 1;
                _fps = value;
                ApplyFps();
            }
        }

        [Category("Appearance"), Description("Số vòng rainbow/giây."), DefaultValue(0.25f)]
        public float CyclesPerSecond
        {
            get { return _cyclesPerSecond; }
            set { _cyclesPerSecond = Math.Max(0f, value); }
        }

        [Category("Appearance"), Description("Độ bão hòa màu (0..1)."), DefaultValue(1.0f)]
        public float Saturation
        {
            get { return _saturation; }
            set { _saturation = Clamp01(value); }
        }

        [Category("Appearance"), Description("Độ sáng (0..1)."), DefaultValue(1.0f)]
        public float Brightness
        {
            get { return _value; }
            set { _value = Clamp01(value); }
        }

        [Category("Appearance"), Description("Hiển thị bóng chữ."), DefaultValue(true)]
        public bool DrawShadow
        {
            get { return _drawShadow; }
            set { _drawShadow = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Hiển thị viền chữ mảnh."), DefaultValue(true)]
        public bool DrawOutline
        {
            get { return _drawOutline; }
            set { _drawOutline = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Màu viền chữ."), DefaultValue(typeof(Color), "60,0,0,0")]
        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; Invalidate(); }
        }

        // ========== Rendering ==========
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            RectangleF layout = ClientRectangle;
            layout.Inflate(-4, -4);

            // Tạo GraphicsPath từ text để fill gradient và vẽ outline
            using (GraphicsPath path = BuildTextPath(g, Text, Font, layout))
            {
                // Shadow nhẹ
                if (_drawShadow)
                {
                    using (Matrix m = new Matrix())
                    {
                        m.Translate(2f, 2f);
                        using (GraphicsPath shadow = (GraphicsPath)path.Clone())
                        {
                            shadow.Transform(m);
                            using (SolidBrush sb = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
                            {
                                g.FillPath(sb, shadow);
                            }
                        }
                    }
                }

                // Fill rainbow
                using (Brush br = CreateRainbowBrush(path.GetBounds(), _phase, _saturation, _value))
                {
                    g.FillPath(br, path);
                }

                // Outline
                if (_drawOutline)
                {
                    using (Pen pen = new Pen(_outlineColor, 2f)
                    { LineJoin = LineJoin.Round })
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        private static GraphicsPath BuildTextPath(Graphics g, string text, Font font, RectangleF bounds)
        {
            var path = new GraphicsPath();
            // Tính emSize theo DPI để AddString hiển thị đúng kích thước
            float emSize = font.SizeInPoints * g.DpiY / 72f;

            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.NoClip
            };

            path.AddString(text,
                           font.FontFamily,
                           (int)font.Style,
                           emSize,
                           bounds,
                           sf);
            return path;
        }

        private static Brush CreateRainbowBrush(RectangleF r, float phase, float sat, float val)
        {
            // Linear gradient ngang với ColorBlend 8-10 stop
            LinearGradientBrush lgb = new LinearGradientBrush(r, Color.Red, Color.Blue, 0f);
            int n = 8; // số stop -> mượt nhưng nhẹ
            ColorBlend cb = new ColorBlend(n);
            cb.Positions = new float[n];

            for (int i = 0; i < n; i++)
            {
                float t = (float)i / (n - 1);          // 0..1
                float hue = (phase + t) * 360f;        // độ
                cb.Colors[i] = HsvToColor(hue, sat, val);
                cb.Positions[i] = t;
            }
            lgb.InterpolationColors = cb;
            return lgb;
        }

        private static float Clamp01(float v) => v < 0 ? 0 : (v > 1 ? 1 : v);

        // HSV → Color (h: độ 0..360, s:0..1, v:0..1)
        private static Color HsvToColor(float h, float s, float v)
        {
            while (h < 0) h += 360f;
            while (h >= 360f) h -= 360f;

            if (s <= 0f) { int vi = (int)(v * 255); return Color.FromArgb(vi, vi, vi); }

            float c = v * s;
            float x = c * (1 - Math.Abs((h / 60f) % 2 - 1));
            float m = v - c;

            float r1 = 0, g1 = 0, b1 = 0;
            if (h < 60) { r1 = c; g1 = x; }
            else if (h < 120) { r1 = x; g1 = c; }
            else if (h < 180) { g1 = c; b1 = x; }
            else if (h < 240) { g1 = x; b1 = c; }
            else if (h < 300) { r1 = x; b1 = c; }
            else { r1 = c; b1 = x; }

            int r = (int)((r1 + m) * 255);
            int g = (int)((g1 + m) * 255);
            int b = (int)((b1 + m) * 255);
            return Color.FromArgb(r, g, b);
        }

        private void ApplyFps()
        {
            _timer.Interval = (int)Math.Round(1000.0 / Math.Max(1, _fps));
        }
    }
}
