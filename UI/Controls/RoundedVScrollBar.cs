using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UiControls
{
    [SupportedOSPlatform("windows")]
    [DefaultEvent("ValueChanged")]
    public class RoundedVScrollBar : Control
    {
        // ========= Public API (na ná VScrollBar) =========
        private int _minimum = 0;
        private int _maximum = 100;
        private int _largeChange = 10;
        private int _smallChange = 1;
        private int _value = 0;

        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Minimum { get => _minimum; set { _minimum = value; Coerce(); Invalidate(); } }

        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Maximum { get => _maximum; set { _maximum = Math.Max(value, _minimum + 1); Coerce(); Invalidate(); } }

        /// <summary>
        /// Lưu ý: Max hiệu dụng = Maximum - LargeChange + 1 (giống ScrollBar chuẩn)
        /// </summary>
        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int LargeChange { get => _largeChange; set { _largeChange = Math.Max(1, value); Coerce(); Invalidate(); } }

        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int SmallChange { get => _smallChange; set { _smallChange = Math.Max(1, value); } }

        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Value
        {
            get => _value;
            set { SetValueInternal(value, true, true); }
        }

        public event EventHandler ValueChanged;

        // ========== Style ========== 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")] public Color TrackColor { get; set; } = Color.FromArgb(240, 244, 252);
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")] public Color TrackBorderColor { get; set; } = Color.FromArgb(220, 226, 240);
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")] public Color ThumbColor { get; set; } = Color.FromArgb(31, 110, 235);
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")] public Color ThumbHoverColor { get; set; } = Color.FromArgb(26, 95, 205);
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")] public Color ThumbDragColor { get; set; } = Color.FromArgb(21, 78, 170);
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")] public int CornerRadius { get; set; } = 10;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")] public int TrackPadding { get; set; } = 6;  // khoảng hở 2 bên

        // ========== Smooth animation ==========
        private readonly System.Windows.Forms.Timer _animTimer;
        private double _animValue;      // giá trị thực đang hiển thị
        private double _animVelocity;   // vận tốc (spring physics nhẹ)
        private int _animTarget;        // đích Value
        private bool _draggingThumb;
        private int _dragStartY;
        private int _dragStartValue;
        private bool _hover;

        // ======== Bind Panel (tuỳ chọn) ========
        private Panel _viewport, _content;
        [Browsable(false)] public Panel Viewport => _viewport;
        [Browsable(false)] public Panel Content => _content;

        public RoundedVScrollBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Width = 16;

            _animTimer = new System.Windows.Forms.Timer { Interval = 16 }; // ~60 FPS
            _animTimer.Tick += (s, e) => AnimateStep();

            // Wheel
            this.MouseWheel += (s, e) =>
            {
                var delta = -Math.Sign(e.Delta) * SmallChange;
                Nudge(delta, smooth: true);
            };

            // Keyboard
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Up) Nudge(-SmallChange, true);
                else if (e.KeyCode == Keys.Down) Nudge(SmallChange, true);
                else if (e.KeyCode == Keys.PageUp) Nudge(-LargeChange, true);
                else if (e.KeyCode == Keys.PageDown) Nudge(LargeChange, true);
                else if (e.KeyCode == Keys.Home) SetValueInternal(Minimum, true, true);
                else if (e.KeyCode == Keys.End) SetValueInternal(MaxEffective(), true, true);
            };

            this.TabStop = true;
        }

        // ======== Public helpers ========
        /// <summary>Gắn thanh cuộn vào viewport + content. Tự tính range và đồng bộ khi Resize.</summary>
        public void BindToPanels(Panel viewport, Panel content, int? largeChangeOverride = null)
        {
            _viewport = viewport; _content = content;
            if (_viewport == null || _content == null) return;

            void Recalc(object s, EventArgs e)
            {
                int contentH = _content.Height;
                int viewH = Math.Max(1, _viewport.ClientSize.Height);
                int max = Math.Max(0, contentH - viewH);

                Minimum = 0;
                Maximum = max + 1; // +1 tránh max==0
                LargeChange = largeChangeOverride ?? Math.Max(10, viewH / 3);
                SmallChange = Math.Max(10, viewH / 10);

                // Đảm bảo Value hợp lệ + cập nhật vị trí content
                Value = Math.Min(Value, MaxEffective());
                ApplyToPanels();
                Invalidate();
            }

            _viewport.Resize += Recalc;
            _content.Resize += Recalc;
            Recalc(this, EventArgs.Empty);

            // đồng bộ khi cuộn bằng bàn phím trong content (nếu có)
            _viewport.MouseWheel += (s, e) =>
            {
                var delta = -Math.Sign(e.Delta) * SmallChange;
                Nudge(delta, smooth: true);
            };
        }

        // ======== Painting ========
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var trackRect = new Rectangle(0, 0, Width - 1, Height - 1);
            int r = Math.Min(CornerRadius, trackRect.Width / 2);

            // Track
            using (var path = Round(trackRect, r))
            using (var br = new SolidBrush(TrackColor))
            using (var pen = new Pen(TrackBorderColor))
            {
                g.FillPath(br, path);
                g.DrawPath(pen, path);
            }

            // Thumb
            var thumbRect = GetThumbRect();
            var col = _draggingThumb ? ThumbDragColor : (_hover ? ThumbHoverColor : ThumbColor);
            using (var path = Round(thumbRect, r))
            using (var br = new SolidBrush(col))
            {
                g.FillPath(br, path);
            }
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; Invalidate(); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();

            if (e.Button != MouseButtons.Left) return;

            var thumb = GetThumbRect();
            if (thumb.Contains(e.Location))
            {
                _draggingThumb = true;
                _dragStartY = e.Y;
                _dragStartValue = Value;
                Capture = true;
            }
            else
            {
                // click trên track → nhảy theo LargeChange tới vị trí bấm
                int dir = e.Y < thumb.Top ? -1 : 1;
                Nudge(dir * LargeChange, smooth: true);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_draggingThumb)
            {
                var track = InnerTrackRect();
                var thumb = GetThumbRect();
                int rangePx = Math.Max(1, track.Height - thumb.Height);
                int dy = e.Y - _dragStartY;

                double unit = (double)rangePx == 0 ? 0 : (double)(MaxEffective() - Minimum) / rangePx;
                int newVal = _dragStartValue + (int)Math.Round(dy * unit);
                SetValueInternal(newVal, raiseEvent: true, smooth: false);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_draggingThumb)
            {
                _draggingThumb = false;
                Capture = false;
                Invalidate();
            }
        }

        // ======== Core ========
        private void Nudge(int delta, bool smooth)
        {
            SetValueInternal(Value + delta, true, smooth);
        }

        private void SetValueInternal(int newVal, bool raiseEvent, bool smooth)
        {
            int effMax = MaxEffective();
            int clamped = Math.Max(Minimum, Math.Min(effMax, newVal));
            if (clamped == _value && !smooth) return;

            if (smooth)
            {
                _animTarget = clamped;
                if (!_animTimer.Enabled) { _animValue = _value; _animVelocity = 0; _animTimer.Start(); }
            }
            else
            {
                _value = clamped;
                _animTarget = clamped;
                _animValue = clamped;
                ApplyToPanels();
                Invalidate();
                if (raiseEvent) ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AnimateStep()
        {
            // Spring-damper mượt: v'' = k*(target-x)-c*v
            double k = 0.25; // stiffness
            double c = 0.35; // damping
            double x = _animValue;
            double v = _animVelocity;
            double a = k * (_animTarget - x) - c * v;

            v += a;
            x += v;

            bool done = Math.Abs(_animTarget - x) < 0.1 && Math.Abs(v) < 0.1;
            if (done)
            {
                x = _animTarget;
                v = 0;
                _animTimer.Stop();
            }

            _animValue = x;
            int newInt = (int)Math.Round(x);
            if (newInt != _value)
            {
                _value = newInt;
                ApplyToPanels();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
            Invalidate();
        }

        private int MaxEffective() => Math.Max(Minimum, Maximum - LargeChange);

        private void Coerce()
        {
            _value = Math.Max(Minimum, Math.Min(MaxEffective(), _value));
            _animTarget = _value;
            _animValue = _value;
        }

        private Rectangle InnerTrackRect()
        {
            int pad = Math.Max(0, TrackPadding);
            return new Rectangle(
                pad,
                pad,
                Width - pad * 2,
                Height - pad * 2
            );
        }

        private Rectangle GetThumbRect()
        {
            var track = InnerTrackRect();
            int effRange = Math.Max(1, MaxEffective() - Minimum);
            // tỷ lệ chiều cao thumb theo LargeChange/Range, tối thiểu 24px
            int thumbH = Math.Max(24, (int)Math.Round((double)LargeChange / (Maximum - Minimum + 1) * track.Height));
            thumbH = Math.Min(track.Height, thumbH);

            int rangePx = Math.Max(1, track.Height - thumbH);
            double t = effRange == 0 ? 0 : (_animValue - Minimum) / effRange;
            int y = track.Top + (int)Math.Round(rangePx * t);

            return new Rectangle(track.X, y, track.Width, thumbH);
        }

        private static GraphicsPath Round(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            if (radius <= 0) { path.AddRectangle(rect); return path; }
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ApplyToPanels()
        {
            if (_viewport == null || _content == null) return;
            _content.Top = -_value;
            _content.Left = 0;
            _content.Width = _viewport.ClientSize.Width - this.Width;
        }

        // tăng hit-test để dễ kéo
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.Cursor = Cursors.Hand;
        }
    }
}
