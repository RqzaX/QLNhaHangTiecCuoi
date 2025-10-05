// RoundedButton.cs
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class RoundedButton : Button
    {
        private int _radius = 14;
        private int _border = 1;
        private Color _borderColor = Color.FromArgb(0x1F, 0x6F, 0xEB); // Primary 500
        private Color _hoverBack = Color.FromArgb(0x1A, 0x5C, 0xD6);   // Primary 600
        private Color _pressedBack = Color.FromArgb(0x18, 0x4F, 0xB8); // darker
        private bool _hovered;
        private bool _pressed;

        public RoundedButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;

            BackColor = Color.FromArgb(0x1F, 0x6F, 0xEB); // default fill
            ForeColor = Color.White;
            Font = new Font("Segoe UI Semibold", 10.5f);
            Padding = new Padding(10, 6, 10, 6);
            Cursor = Cursors.Hand;
        }

        // ====== Properties ======
        [Category("Appearance"), Description("Bo góc theo pixel."), DefaultValue(14)]
        public int CornerRadius
        {
            get { return _radius; }
            set { _radius = value < 0 ? 0 : value; UpdateRegion(); Invalidate(); }
        }
        public bool ShouldSerializeCornerRadius() { return _radius != 14; }
        public void ResetCornerRadius() { CornerRadius = 14; }

        [Category("Appearance"), Description("Độ dày viền."), DefaultValue(1)]
        public int BorderThickness
        {
            get { return _border; }
            set { _border = value < 0 ? 0 : value; Invalidate(); }
        }
        public bool ShouldSerializeBorderThickness() { return _border != 1; }
        public void ResetBorderThickness() { BorderThickness = 1; }

        [Category("Appearance"), Description("Màu viền."), DefaultValue(typeof(Color), "31,111,235")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }
        public bool ShouldSerializeBorderColor() { return _borderColor != Color.FromArgb(31, 111, 235); }
        public void ResetBorderColor() { BorderColor = Color.FromArgb(31, 111, 235); }

        [Category("Appearance"), Description("Màu khi hover."), DefaultValue(typeof(Color), "26,92,214")]
        public Color HoverBackColor
        {
            get { return _hoverBack; }
            set { _hoverBack = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Màu khi nhấn giữ."), DefaultValue(typeof(Color), "24,79,184")]
        public Color PressedBackColor
        {
            get { return _pressedBack; }
            set { _pressedBack = value; Invalidate(); }
        }

        // ====== Events for hover/press ======
        protected override void OnMouseEnter(EventArgs e) { _hovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hovered = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs mevent) { if (mevent.Button == MouseButtons.Left) _pressed = true; Invalidate(); base.OnMouseDown(mevent); }
        protected override void OnMouseUp(MouseEventArgs mevent) { _pressed = false; Invalidate(); base.OnMouseUp(mevent); }

        protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e); Invalidate(); }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            using (GraphicsPath path = BuildPath(ClientRectangle, _radius))
            {
                if (Region != null) Region.Dispose();
                Region = new Region(path);
            }
        }

        // ====== Painting ======
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = ClientRectangle;
            rect.Width -= 1; rect.Height -= 1;

            Color fill = BackColor;
            if (!Enabled)
                fill = ControlPaint.Light(BackColor, 0.6f);
            else if (_pressed)
                fill = _pressedBack;
            else if (_hovered)
                fill = _hoverBack;

            using (GraphicsPath path = BuildPath(rect, _radius))
            using (SolidBrush br = new SolidBrush(fill))
            {
                e.Graphics.FillPath(br, path);
                if (_border > 0)
                {
                    using (Pen pen = new Pen(_borderColor, _border))
                        e.Graphics.DrawPath(pen, path);
                }
            }

            // Vẽ Text/Image (giữa nút)
            TextRenderer.DrawText(
                e.Graphics, Text, Font, rect,
                Enabled ? ForeColor : SystemColors.GrayText,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private static GraphicsPath BuildPath(Rectangle r, int radius)
        {
            GraphicsPath p = new GraphicsPath();
            if (radius <= 0) { p.AddRectangle(r); return p; }
            int d = radius * 2;
            p.AddArc(r.Left, r.Top, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
