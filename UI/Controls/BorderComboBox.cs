using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UiControls
{
    [SupportedOSPlatform("windows")]
    [ToolboxItem(true)]
    public class BorderComboBox : ComboBox
    {
        private bool _hover;

        private int _cornerRadius = 8;
        private int _borderThickness = 1;
        private Color _borderColor = Color.FromArgb(209, 213, 219);        // #D1D5DB
        private Color _borderHoverColor = Color.FromArgb(156, 163, 175);   // #9CA3AF
        private Color _borderFocusColor = Color.FromArgb(31, 111, 235);    // #1F6FEB
        private Color _borderDisabledColor = Color.FromArgb(229, 231, 235);// #E5E7EB

        public BorderComboBox()
        {
            // Hệ thống vẽ phần nền; ta chỉ "đè" viền sau cùng bằng WndProc
            DrawMode = DrawMode.OwnerDrawFixed;     // để vẽ item mượt
            ItemHeight = Math.Max(20, Font.Height + 6);
            IntegralHeight = false;
        }

        [Category("Appearance"), Description("Bo góc (px)."), DefaultValue(8)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance"), Description("Độ dày viền (px)."), DefaultValue(1)]
        public int BorderThickness
        {
            get => _borderThickness;
            set { _borderThickness = Math.Max(1, value); Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "209,213,219")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "156,163,175")]
        public Color BorderHoverColor
        {
            get => _borderHoverColor;
            set { _borderHoverColor = value; Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "31,111,235")]
        public Color BorderFocusColor
        {
            get => _borderFocusColor;
            set { _borderFocusColor = value; Invalidate(); }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "229,231,235")]
        public Color BorderDisabledColor
        {
            get => _borderDisabledColor;
            set { _borderDisabledColor = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

        // Vẽ item (chỉ để màu chữ/selected mượt hơn)
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index >= 0 && e.Index < Items.Count)
            {
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                Color fore = selected ? SystemColors.HighlightText : ForeColor;
                using var br = new SolidBrush(fore);
                string text = GetItemText(Items[e.Index]);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                e.Graphics.DrawString(text, Font, br,
                    e.Bounds.X + 6, e.Bounds.Y + (e.Bounds.Height - Font.Height) / 2f);
            }
            e.DrawFocusRectangle();
            base.OnDrawItem(e);
        }

        // Vẽ viền sau khi hệ thống vẽ control
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            const int WM_PAINT = 0x000F;
            const int WM_NCPAINT = 0x0085;
            if (m.Msg == WM_PAINT || m.Msg == WM_NCPAINT)
            {
                using Graphics g = Graphics.FromHwnd(Handle);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
                Color use =
                    !Enabled ? _borderDisabledColor :
                    Focused ? _borderFocusColor :
                    _hover ? _borderHoverColor :
                               _borderColor;

                using GraphicsPath path = CreateRoundRect(rect, _cornerRadius);
                using Pen pen = new Pen(use, _borderThickness);
                g.DrawPath(pen, path);
            }
        }

        private static GraphicsPath CreateRoundRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            if (radius <= 0) { path.AddRectangle(r); return path; }
            int d = radius * 2;
            var arc = new Rectangle(r.X, r.Y, d, d);
            path.AddArc(arc, 180, 90);                 // TL
            arc.X = r.Right - d; path.AddArc(arc, 270, 90); // TR
            arc.Y = r.Bottom - d; path.AddArc(arc, 0, 90);  // BR
            arc.X = r.X; path.AddArc(arc, 90, 90);          // BL
            path.CloseFigure();
            return path;
        }
    }
}
