using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace UI.Controls
{
    [DefaultProperty(nameof(TimeFormat))]
    [SupportedOSPlatform("windows")]
    public class DigitalClockControl : Control
    {
        private readonly Timer _timer;
        private string _timeFormat = "HH:mm:ss";
        private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;
        private bool _smooth = true;

        public DigitalClockControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Font = new Font("Segoe UI Semibold", 20f, FontStyle.Bold);
            ForeColor = Color.LimeGreen;

            // Tránh crash Designer khi đặt Transparent
            BackColor = IsDesignMode ? SystemColors.Control : Color.Transparent;

            _timer = new Timer { Interval = 1000, Enabled = true };
            _timer.Tick += (_, __) => Invalidate();
        }

        private static bool IsDesignMode =>
            LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
            Application.RenderWithVisualStyles == false && !Environment.UserInteractive; // phòng hờ

        [Category("Behavior")]
        [Description("Định dạng thời gian .NET, ví dụ HH:mm:ss, HH:mm, etc.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TimeFormat
        {
            get => _timeFormat;
            set { _timeFormat = string.IsNullOrWhiteSpace(value) ? "HH:mm:ss" : value; Invalidate(); }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ContentAlignment TextAlign
        {
            get => _textAlign;
            set { _textAlign = value; Invalidate(); }
        }

        [Category("Behavior")]
        [Description("Bật vẽ mượt (double buffer).")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Smooth
        {
            get => _smooth;
            set { _smooth = value; Invalidate(); }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                // Chỉ bật composited khi RUN-TIME để tránh Designer lỗi
                if (_smooth && !IsDesignMode) cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Giả lập trong suốt: nếu Transparent thì tô màu của Parent
            var bg = (BackColor.A < 255 && Parent != null) ? Parent.BackColor : BackColor;
            using (var br = new SolidBrush(bg)) e.Graphics.FillRectangle(br, ClientRectangle);

            string time = DateTime.Now.ToString(_timeFormat);

            var flags = TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis;
            switch (_textAlign)
            {
                case ContentAlignment.TopLeft: flags |= TextFormatFlags.Top | TextFormatFlags.Left; break;
                case ContentAlignment.TopCenter: flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter; break;
                case ContentAlignment.TopRight: flags |= TextFormatFlags.Top | TextFormatFlags.Right; break;
                case ContentAlignment.MiddleLeft: flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left; break;
                case ContentAlignment.MiddleCenter: flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter; break;
                case ContentAlignment.MiddleRight: flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right; break;
                case ContentAlignment.BottomLeft: flags |= TextFormatFlags.Bottom | TextFormatFlags.Left; break;
                case ContentAlignment.BottomCenter: flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter; break;
                case ContentAlignment.BottomRight: flags |= TextFormatFlags.Bottom | TextFormatFlags.Right; break;
            }

            TextRenderer.DrawText(e.Graphics, time, Font, ClientRectangle, ForeColor, flags);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _timer?.Dispose();
            base.Dispose(disposing);
        }
    }
}
