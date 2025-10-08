using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI
{
    [DefaultEvent("SelectedIndexChanged")]
    [SupportedOSPlatform("windows")]
    public class OderItemCard : Control
    {
        // ====== Data ======
        private readonly List<string> _items = new()
        {
            "Tất cả", "Chờ xác nhận", "Đã xác nhận", "Đã phục vụ"
        };
        private int _selectedIndex = 0;
        private int _hoverIndex = -1;
        private readonly List<Rectangle> _itemRects = new();

        // ====== Appearance ======
        private int _cornerRadius = 18;
        private Padding _containerPadding = new Padding(8, 6, 8, 6);
        private Padding _itemPadding = new Padding(14, 6, 14, 6);
        private int _spacing = 8;

        private Color _containerBack = Color.FromArgb(245, 247, 250);   // nền xám nhạt
        private Color _containerBorder = Color.FromArgb(228, 232, 238);
        private Color _selectedBack = Color.White;
        private Color _selectedBorder = Color.FromArgb(228, 232, 238);
        private Color _textColor = Color.FromArgb(20, 20, 20);

        public event EventHandler? SelectedIndexChanged;

        public OderItemCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            Size = new Size(460, 44);
            BackColor = Color.Transparent;
            TabStop = true;
        }

        // ====== Public API ======
        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<string> Items => _items;

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

        [Category("Appearance")]
        [DefaultValue(18)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(8, value); Invalidate(); }
        }

        [Category("Layout")]
        [DefaultValue(typeof(Padding), "8,6,8,6")]
        public Padding ContainerPadding
        {
            get => _containerPadding;
            set { _containerPadding = value; Invalidate(); }
        }

        [Category("Layout")]
        [DefaultValue(typeof(Padding), "14,6,14,6")]
        public Padding ItemPadding
        {
            get => _itemPadding;
            set { _itemPadding = value; Invalidate(); }
        }

        [Category("Layout")]
        [DefaultValue(8)]
        public int Spacing
        {
            get => _spacing;
            set { _spacing = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "245,247,250")]
        public Color ContainerBackColor
        {
            get => _containerBack;
            set { _containerBack = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "228,232,238")]
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
        [DefaultValue(typeof(Color), "228,232,238")]
        public Color SelectedBorderColor
        {
            get => _selectedBorder;
            set { _selectedBorder = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "20,20,20")]
        public Color TextColor
        {
            get => _textColor;
            set { _textColor = value; Invalidate(); }
        }

        [Browsable(false)]
        public string? SelectedText => (_selectedIndex >= 0 && _selectedIndex < _items.Count) ? _items[_selectedIndex] : null;

        // ====== Layout helpers ======
        public override Size GetPreferredSize(Size proposedSize)
        {
            using var g = CreateGraphics();
            var totalW = _containerPadding.Horizontal;
            var h = TextRenderer.MeasureText(g, "A", Font, new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Height + _itemPadding.Vertical;

            for (int i = 0; i < _items.Count; i++)
            {
                var w = TextRenderer.MeasureText(g, _items[i], Font, new Size(int.MaxValue, int.MaxValue),
                        TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Width + _itemPadding.Horizontal;
                if (i > 0) totalW += _spacing;
                totalW += w;
            }
            return new Size(totalW + 2, Math.Max(h + _containerPadding.Vertical, 36));
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

                var rr = new Rectangle(x, r.Top + (h - (h - 0)) / 2, w, Math.Min(h, 9999));
                rr.Height = h; // mọi item cùng chiều cao
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

        // ====== Painting ======
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

            // Selected background first (để viền ngoài không bị đè)
            for (int i = 0; i < _items.Count; i++)
            {
                if (i == _selectedIndex)
                {
                    var rr = _itemRects[i];
                    int rad = Math.Min(_cornerRadius - 4, rr.Height / 2);
                    using var path = RoundRect(rr, Math.Max(10, rad));
                    using var br = new SolidBrush(_selectedBack);
                    using var pen = new Pen(_selectedBorder);
                    g.FillPath(br, path);
                    g.DrawPath(pen, path);
                }
            }

            // Hover effect (nhẹ)
            if (_hoverIndex >= 0 && _hoverIndex < _items.Count && _hoverIndex != _selectedIndex)
            {
                var rr = _itemRects[_hoverIndex];
                int rad = Math.Min(_cornerRadius - 4, rr.Height / 2);
                using var path = RoundRect(rr, Math.Max(10, rad));
                using var br = new SolidBrush(Color.FromArgb(25, Color.White));
                g.FillPath(br, path);
            }

            // Draw texts
            for (int i = 0; i < _items.Count; i++)
            {
                var rr = _itemRects[i];
                var color = _textColor;
                TextRenderer.DrawText(g, _items[i], Font, rr, color,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            }
        }

        // ====== Interaction ======
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
            if (idx >= 0)
                SelectedIndex = idx;
        }

        private int HitTest(Point p)
        {
            for (int i = 0; i < _itemRects.Count; i++)
                if (_itemRects[i].Contains(p)) return i;
            return -1;
        }

        // ====== Utils ======
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
