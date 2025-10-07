using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace VanThuan.Controls
{
    [DefaultProperty(nameof(ShowDayOfWeek))]
    [SupportedOSPlatform("windows")]
    public class DateVNControl : Control
    {
        private readonly Timer _timer;
        private bool _showDayOfWeek = true;
        private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;
        private CultureInfo _culture = new CultureInfo("vi-VN");
        private bool _smooth = true;

        public DateVNControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            Font = new Font("Segoe UI", 11f, FontStyle.Regular);
            ForeColor = Color.FromArgb(40, 40, 40);
            BackColor = IsDesignMode ? SystemColors.Control : Color.Transparent;

            _timer = new Timer { Interval = 1000, Enabled = true };
            _timer.Tick += (_, __) => Invalidate();
        }

        private static bool IsDesignMode =>
            LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
            Application.RenderWithVisualStyles == false && !Environment.UserInteractive;

        [Category("Behavior")]
        [Description("Hiển thị 'Thứ ...' ở đầu chuỗi.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowDayOfWeek
        {
            get => _showDayOfWeek;
            set { _showDayOfWeek = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ContentAlignment TextAlign
        {
            get => _textAlign;
            set { _textAlign = value; Invalidate(); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CultureInfo Culture
        {
            get => _culture;
            set { _culture = value ?? new CultureInfo("vi-VN"); Invalidate(); }
        }

        [Category("Behavior")]
        [Description("Bật vẽ mượt (double buffer).")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
                if (_smooth && !IsDesignMode) cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        private string BuildDateText(DateTime dt)
        {
            string dow = dt.ToString("dddd", _culture);
            string rest = dt.ToString("dd 'tháng' MM ', ' yyyy", _culture);
            if (_showDayOfWeek)
                return $"{char.ToUpper(dow[0], _culture)}{dow.Substring(1)}, {rest}";
            return rest;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var bg = (BackColor.A < 255 && Parent != null) ? Parent.BackColor : BackColor;
            using (var br = new SolidBrush(bg)) e.Graphics.FillRectangle(br, ClientRectangle);

            string text = BuildDateText(DateTime.Now);

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

            TextRenderer.DrawText(e.Graphics, text, Font, ClientRectangle, ForeColor, flags);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _timer?.Dispose();
            base.Dispose(disposing);
        }
    }
}
