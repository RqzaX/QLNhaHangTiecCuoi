// SegmentedPill.cs
// WinForms single-select segmented control (pill style)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace VanThuan.UI
{
    [DefaultEvent(nameof(SelectedIndexChanged))]
    [SupportedOSPlatform("windows")]
    public class SegmentedPill : Control
    {
        // ========= Data =========
        private readonly BindingList<string> _items = new BindingList<string>
        {
            "Tất cả", "Chờ xác nhận", "Đã xác nhận", "Đã phục vụ"
        };

        private int _selectedIndex = 0;
        private int _hoverIndex = -1;
        private readonly List<Rectangle> _itemRects = new List<Rectangle>();

        // ========= Appearance =========
        private int _cornerRadius = 18;
        private Padding _containerPadding = new Padding(8, 6, 8, 6);
        private Padding _itemPadding = new Padding(14, 6, 14, 6);
        private int _spacing = 8;

        private Color _containerBack = Color.FromArgb(245, 247, 250);  // nền xám nhạt
        private Color _containerBorder = Color.FromArgb(228, 232, 238);
        private Color _selectedBack = Color.White;
        private Color _selectedBorder = Color.FromArgb(228, 232, 238);
        private Color _textColor = Color.FromArgb(20, 20, 20);

        public event EventHandler? SelectedIndexChanged;

        public SegmentedPill()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.UserPaint
                     | ControlStyles.SupportsTransparentBackColor, true);

            Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            Size = new Size(460, 44);
            BackColor = Color.Transparent; // hỗ trợ trong suốt (đã bật ở SetStyle)
            TabStop = true;

            _items.ListChanged += (_, __) => { BuildItemRects(); Invalidate(); };
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (Parent != null)
                Parent.BackColorChanged += (_, __) => Invalidate();
        }

        // Vẽ nền trong suốt bằng cách “mượn” nền của parent
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (BackColor.A == 255 || Parent == null)
            {
                base.OnPaintBackground(e);
                return;
            }

            var g = e.Graphics;
            var state = g.Save();
            try
            {
                g.TranslateTransform(-Left, -Top);
                var pe = new PaintEventArgs(g, Parent.ClientRectangle);
                InvokePaintBackground(Parent, pe);
                InvokePaint(Parent, pe);
            }
            finally
            {
                g.Restore(state);
            }
        }

        // ========= Public API / Properties =========

        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public BindingList<string> Items => _items;

        [Category("Behavior")]
        [DefaultValue(0)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                int v = Math.Max(-1, Math.Min(value, _items.Count - 1));
                if (_selectedIndex == v) return;
                _selectedIndex = v;
                Invalidate();
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public string? SelectedText =>
            (_selectedIndex >= 0 && _selectedIndex < _items.Count) ? _items[_selectedIndex] : null;

        [Category("Appearance")]
        [DefaultValue(18)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(8, value); Invalidate(); }
        }

        [Category("Layout")]
        [DefaultValue(typeof(Padding), "8, 6, 8, 6")]
        public Padding ContainerPadding
        {
            get => _containerPadding;
            set { _containerPadding = value; BuildItemRects(); Invalidate(); }
        }

        [Category("Layout")]
        [DefaultValue(typeof(Padding), "14, 6, 14, 6")]
        public Padding ItemPadding
        {
            get => _itemPadding;
            set { _itemPadding = value; BuildItemRects(); Invalidate(); }
        }

        [Category("Layout")]
        [DefaultValue(8)]
        public int Spacing
        {
            get => _spacing;
            set { _spacing = Math.Max(0, value); BuildItemRects(); Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "245, 247, 250")]
        public Color ContainerBackColor
        {
            get => _containerBack;
            set { _containerBack = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "228, 232, 238")]
        public Color ContainerBorderColor
        {
            get => _containerBorder;
            set { _containerBorder = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public Color SelectedBackColor
        {
            get => _selectedBack;
            set { _selectedBack = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "228, 232, 238")]
        public Color SelectedBorderColor
        {
            get => _selectedBorder;
            set { _selectedBorder = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "20, 20, 20")]
        public Color TextColor
        {
            get => _textColor;
            set { _textColor = value; Invalidate(); }
        }

        // ========= Layout =========

        public override Size GetPreferredSize(Size proposedSize)
        {
            using var g = CreateGraphics();
            int totalW = _containerPadding.Horizontal;
            int textH = TextRenderer.MeasureText(g, "A", Font, new Size(int.MaxValue, int.MaxValue),
                            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Height;

            int innerH = textH + _itemPadding.Vertical;
            for (int i = 0; i < _items.Count; i++)
            {
                int w = TextRenderer.MeasureText(g, _items[i], Font, new Size(int.MaxValue, int.MaxValue),
                            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Width + _itemPadding.Horizontal;
                if (i > 0) totalW += _spacing;
                totalW += w;
            }
            int h = Math.Max(innerH + _containerPadding.Vertical, 36);
            return new Size(totalW + 2, h + 2);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            BuildItemRects();
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            BuildItemRects();
        }

        private void BuildItemRects()
        {
            _itemRects.Clear();
            var r = ClientRectInner();
            int x = r.Left;
            int h = r.Height;

            using var g = CreateGraphics();
            for (int i = 0; i < _items.Count; i++)
            {
                int w = TextRenderer.MeasureText(g, _items[i], Font, new Size(int.MaxValue, int.MaxValue),
                            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Width + _itemPadding.Horizontal;

                var rr = new Rectangle(x, r.Top, w, h);
                _itemRects.Add(rr);
                x += w + _spacing;
            }
        }

        private Rectangle ClientRectOuter()
        {
            var rect = ClientRectangle;
            rect.Inflate(-1, -1);
            return rect;
        }

        private Rectangle ClientRectInner()
        {
            var r = ClientRectOuter();
            return new Rectangle(
                r.Left + _containerPadding.Left,
                r.Top + _containerPadding.Top,
                Math.Max(4, r.Width - _containerPadding.Horizontal),
                Math.Max(4, r.Height - _containerPadding.Vertical)
            );
        }

        // ========= Painting =========

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var outer = ClientRectOuter();
            using (var path = RoundRect(outer, _cornerRadius))
            using (var br = new SolidBrush(_containerBack))
            using (var pen = new Pen(_containerBorder))
            {
                g.FillPath(br, path);
                g.DrawPath(pen, path);
            }

            if (_itemRects.Count != _items.Count) BuildItemRects();

            // Selected segment
            if (_selectedIndex >= 0 && _selectedIndex < _itemRects.Count)
            {
                var rr = _itemRects[_selectedIndex];
                int rad = Math.Min(_cornerRadius - 4, rr.Height / 2);
                using var path = RoundRect(rr, Math.Max(10, rad));
                using var br = new SolidBrush(_selectedBack);
                using var pen = new Pen(_selectedBorder);
                g.FillPath(br, path);
                g.DrawPath(pen, path);
            }

            // Hover (nhẹ)
            if (_hoverIndex >= 0 && _hoverIndex < _itemRects.Count && _hoverIndex != _selectedIndex)
            {
                var rr = _itemRects[_hoverIndex];
                int rad = Math.Min(_cornerRadius - 4, rr.Height / 2);
                using var path = RoundRect(rr, Math.Max(10, rad));
                using var br = new SolidBrush(Color.FromArgb(20, Color.White));
                g.FillPath(br, path);
            }

            // Texts
            for (int i = 0; i < _items.Count; i++)
            {
                var rr = _itemRects[i];
                TextRenderer.DrawText(g, _items[i], Font, rr, _textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            }

            // Focus cue
            if (Focused)
            {
                var focus = ClientRectOuter();
                focus.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(g, focus);
            }
        }

        // ========= Interaction =========
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int idx = HitTest(e.Location);
            if (idx != _hoverIndex)
            {
                _hoverIndex = idx;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoverIndex = -1;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            int idx = HitTest(e.Location);
            if (idx >= 0) SelectedIndex = idx;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right) return true;
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (_items.Count == 0) return;

            if (e.KeyCode == Keys.Right)
            {
                SelectedIndex = Math.Min(_items.Count - 1, _selectedIndex + 1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                SelectedIndex = Math.Max(0, _selectedIndex - 1);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Home)
            {
                SelectedIndex = 0; e.Handled = true;
            }
            else if (e.KeyCode == Keys.End)
            {
                SelectedIndex = _items.Count - 1; e.Handled = true;
            }
        }

        private int HitTest(Point p)
        {
            for (int i = 0; i < _itemRects.Count; i++)
                if (_itemRects[i].Contains(p)) return i;
            return -1;
        }

        // ========= Utils =========
        private static GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            if (radius <= 0)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
