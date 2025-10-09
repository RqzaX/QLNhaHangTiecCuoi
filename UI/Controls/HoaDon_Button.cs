// PillButton.cs
// WinForms .NET Framework 4.8+ hoặc .NET 6/7/8
// Nút dạng "pill/chip" với preset chữ, AutoSize, DPI-aware.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class HoaDon_Button : Control
    {
        // ====== Preset text ======
        public enum PillPreset
        {
            Custom = 0, // Dùng Text hiện tại
            B001,
            KOT003,
            PAID,
            HUY,          // "HỦY"
            DANG_LAM,     // "Đang làm"
            COMPLETED
        }

        private PillPreset _preset = PillPreset.B001;
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool UsePresetText { get; set; } = true;

        [Category("Appearance")]
        [DefaultValue(PillPreset.B001)]
        public PillPreset Preset
        {
            get => _preset;
            set
            {
                _preset = value;
                if (UsePresetText)
                {
                    switch (_preset)
                    {
                        case PillPreset.B001: Text = "B001"; break;
                        case PillPreset.KOT003: Text = "KOT003"; break;
                        case PillPreset.PAID: Text = "PAID"; break;
                        case PillPreset.HUY: Text = "HỦY"; break;
                        case PillPreset.DANG_LAM: Text = "Đang làm"; break;
                        case PillPreset.COMPLETED: Text = "Completed"; break;
                        default: /* Custom */ break;
                    }
                }
            }
        }

        // ====== Visual states ======
        private bool _hover, _pressed, _focused;

        // ====== Appearance (mặc định theo ảnh) ======
        private Color _bgNormal = Color.FromArgb(0xF4, 0xF6, 0xF9); // #F4F6F9
        private Color _bgHover = Color.FromArgb(0xEC, 0xF0, 0xF4);
        private Color _bgPressed = Color.FromArgb(0xE4, 0xE9, 0xEE);
        private Color _border = Color.FromArgb(0xDD, 0xE3, 0xEA); // #DDE3EA
        private Color _textCol = Color.FromArgb(0x46, 0x4B, 0x5A); // #464B5A
        private Color _focusRing = Color.FromArgb(64, 31, 111, 235); // ring Sapphire mờ

        private int _fixedHeight = 24;     // đúng mẫu
        private int _cornerRadius = 12;    // pill
        private Padding _contentPadding = new Padding(12, 3, 12, 3);

        // ====== .ctor ======
        public HoaDon_Button()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            TabStop = true;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            ForeColor = _textCol;
            AutoSize = true;
            Margin = Padding.Empty;

            // Thả lần đầu có sẵn chữ mẫu, nhưng không ghi đè nếu Designer đã lưu giá trị khác
            if (string.IsNullOrEmpty(Text))
                Preset = PillPreset.B001;
        }

        // ====== Exposed properties ======
        [Category("Appearance")]
        public Color BackNormal { get => _bgNormal; set { _bgNormal = value; Invalidate(); } }
        [Category("Appearance")]
        public Color BackHover { get => _bgHover; set { _bgHover = value; Invalidate(); } }
        [Category("Appearance")]
        public Color BackPressed { get => _bgPressed; set { _bgPressed = value; Invalidate(); } }
        [Category("Appearance")]
        public Color BorderColor { get => _border; set { _border = value; Invalidate(); } }
        [Category("Appearance")]
        public Color TextColor { get => _textCol; set { _textCol = value; Invalidate(); } }
        [Category("Appearance")]
        public Color FocusRing { get => _focusRing; set { _focusRing = value; Invalidate(); } }

        [Category("Appearance")]
        public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(6, value); Invalidate(); } }

        [Category("Layout")]
        public Padding ContentPadding { get => _contentPadding; set { _contentPadding = value; Invalidate(); } }

        [Category("Layout")]
        [DefaultValue(24)]
        public int FixedHeight { get => _fixedHeight; set { _fixedHeight = Math.Max(16, value); Invalidate(); } }

        // Text mặc định + giữ nguyên khi chạy (không reset)
        [DefaultValue("B001")]
        public override string Text
        {
            get => base.Text;
            set { base.Text = value; Invalidate(); }
        }
        public bool ShouldSerializeText() => !string.Equals(Text, "B001", StringComparison.Ordinal);
        public void ResetText() => Text = "B001";

        // ====== Input handling ======
        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = _pressed = false; Invalidate(); }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
            Focus();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            bool fire = _pressed && ClientRectangle.Contains(e.Location);
            _pressed = false; Invalidate();
            if (fire) OnClick(EventArgs.Empty);
        }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); _focused = true; Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); _focused = false; Invalidate(); }
        protected override bool IsInputKey(Keys keyData) => keyData == Keys.Space || base.IsInputKey(keyData);
        protected override void OnKeyDown(KeyEventArgs e) { base.OnKeyDown(e); if (e.KeyCode == Keys.Space) { _pressed = true; Invalidate(); } }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Space)
            {
                _pressed = false; Invalidate();
                OnClick(EventArgs.Empty);
            }
        }

        // ====== Layout & Paint ======
        public override Size GetPreferredSize(Size proposedSize)
        {
            float scale = DeviceDpi / 96f;
            int h = (int)Math.Round(_fixedHeight * scale, MidpointRounding.AwayFromZero);
            var szText = TextRenderer.MeasureText(Text, Font, Size.Empty, TextFormatFlags.NoPadding);
            int w = _contentPadding.Horizontal + szText.Width;
            w = Math.Max(w, h); // tối thiểu hình tròn nếu text ngắn
            return new Size(w, h);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            // Màu theo state
            Color bg = _bgNormal;
            if (_pressed) bg = _bgPressed;
            else if (_hover) bg = _bgHover;

            // Viền 1px sắc nét (dịch 0.5f)
            var rect = ClientRectangle;
            var rF = new RectangleF(rect.X + 0.5f, rect.Y + 0.5f, rect.Width - 1f, rect.Height - 1f);

            using var path = RoundedRect(rF, _cornerRadius);
            using var br = new SolidBrush(bg);
            using var pen = new Pen(_border, 1f);

            g.FillPath(br, path);
            g.DrawPath(pen, path);

            // Focus ring
            if (_focused)
            {
                using var penF = new Pen(_focusRing, 2f);
                var fr = RectangleF.Inflate(rF, 2, 2);
                using var fp = RoundedRect(fr, _cornerRadius + 2);
                g.DrawPath(penF, fp);
            }

            // Text center (ClearType, DPI-aware)
            var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;
            TextRenderer.DrawText(g, Text, Font, rect, _textCol, flags);
        }

        // ====== Helpers ======
        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            float d = radius * 2f;
            var p = new GraphicsPath();
            if (radius <= 0) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}
