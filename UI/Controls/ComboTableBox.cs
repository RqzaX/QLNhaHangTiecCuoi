using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;


namespace UI.Controls
{
    // ====== Model dữ liệu cho từng dòng ======
    public class TableOption
    {
        public string MaBan { get; set; } = "A01";
        public int SoCho { get; set; } = 4;
        public string TrangThai { get; set; } = "TRỐNG";
        public bool IsVip { get; set; } = false;

        public override string ToString()
            => $"Bàn {MaBan} ({SoCho} chỗ) - {FormatTrangThai(TrangThai)}";

        public static string FormatTrangThai(string s)
        {
            s = (s ?? "").Trim().ToUpperInvariant();
            if (s == "TRỐNG") return "Trống";
            if (s == "ĐÃ ĐẶT" || s == "DA DAT") return "Đã đặt";
            if (s == "PHỤC VỤ" || s == "PHUC VU") return "Đang phục vụ";
            if (s == "VỆ SINH" || s == "VE SINH") return "Đang vệ sinh";
            return s;
        }
    }
    [SupportedOSPlatform("windows")]
    // ====== ListBox owner-draw để hiển thị dropdown ======
    internal class DropDownListBox : ListBox
    {
        public Func<TableOption, Color> GetStateColor;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HoverBackColor { get; set; } = Color.FromArgb(245, 247, 252);
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ItemPadX { get; set; } = 10;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ItemPadY { get; set; } = 6;

        private int _hoverIndex = -1;

        public DropDownListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            IntegralHeight = false;
            BorderStyle = BorderStyle.None;
            ItemHeight = 32;
            DoubleBuffered = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int idx = IndexFromPoint(e.Location);
            if (idx != _hoverIndex)
            {
                _hoverIndex = idx;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hoverIndex = -1;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0 || e.Index >= Items.Count) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = e.Bounds;
            rect.Inflate(-2, -2);

            // Hover
            if (e.Index == _hoverIndex)
            {
                using var b = new SolidBrush(HoverBackColor);
                var r = rect; r.Inflate(-2, -2);
                using var gp = CreateRoundRect(r, 8);
                g.FillPath(b, gp);
            }

            var item = Items[e.Index] as TableOption;
            string left = $"Bàn {item.MaBan}";
            string mid = $"({item.SoCho} chỗ)";
            string right = $" - {TableOption.FormatTrangThai(item.TrangThai)}";

            // Vẽ text
            using var fLeft = new Font(Font, FontStyle.Bold);
            using var fMid = new Font(Font, FontStyle.Regular);
            using var fRight = new Font(Font, FontStyle.Regular);

            int x = rect.X + ItemPadX;
            int y = rect.Y + ItemPadY;

            // dấu VIP nho nhỏ
            if (item.IsVip)
            {
                var vipRect = new Rectangle(x, y + 2, 28, 18);
                using var brVip = new SolidBrush(Color.FromArgb(255, 230, 0));
                using var gpVip = CreateRoundRect(vipRect, 6);
                g.FillPath(brVip, gpVip);
                TextRenderer.DrawText(g, "VIP", new Font(Font, FontStyle.Bold),
                    vipRect, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                x += vipRect.Width + 6;
            }

            var colLeft = Color.Black;
            var colMid = Color.FromArgb(120, 120, 120);
            var colState = GetStateColor?.Invoke(item) ?? Color.Gray;

            TextRenderer.DrawText(g, left, fLeft, new Point(x, y), colLeft);
            x += TextRenderer.MeasureText(g, left, fLeft, Size, TextFormatFlags.NoPadding).Width;

            TextRenderer.DrawText(g, " " + mid + " ", fMid, new Point(x, y), colMid);
            x += TextRenderer.MeasureText(g, " " + mid + " ", fMid, Size, TextFormatFlags.NoPadding).Width;

            TextRenderer.DrawText(g, right, fRight, new Point(x, y), colState);

            // Tick nếu đang chọn
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                var checkRect = new Rectangle(e.Bounds.Right - 28, e.Bounds.Top + 8, 18, 18);
                DrawCheck(g, checkRect);
            }

            // gạch phân cách
            using var pen = new Pen(Color.FromArgb(235, 238, 245));
            g.DrawLine(pen, e.Bounds.Left + 8, e.Bounds.Bottom - 1, e.Bounds.Right - 8, e.Bounds.Bottom - 1);
        }

        private static void DrawCheck(Graphics g, Rectangle r)
        {
            using var p = new Pen(Color.FromArgb(31, 111, 235), 2.2f);
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var pts = new[]
            {
                new Point(r.Left+2, r.Top + r.Height/2),
                new Point(r.Left + r.Width/2 - 2, r.Bottom - 3),
                new Point(r.Right - 2, r.Top + 3)
            };
            g.DrawLines(p, pts);
        }

        private static GraphicsPath CreateRoundRect(Rectangle r, int radius)
        {
            var gp = new GraphicsPath();
            int d = radius * 2;
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }

    // ====== ToolStrip host để có bóng đổ đẹp ======
    [SupportedOSPlatform("windows")]
    internal class RoundedDropDown : ToolStripDropDown
    {
        public RoundedDropDown(Control content)
        {
            DoubleBuffered = true;
            AutoSize = false;
            Padding = Margin = Padding.Empty;
            BackColor = Color.White;
            RenderMode = ToolStripRenderMode.System;

            var host = new ToolStripControlHost(content)
            {
                Margin = Padding = Padding.Empty,
                AutoSize = false
            };
            base.Items.Add(host);
        }

        public void Show(Control owner, Rectangle align, int width, int height)
        {
            if (Items[0] is ToolStripControlHost host)
            {
                host.Size = new Size(width, height);
                Size = new Size(width, height);
            }

            // canh theo ô hiển thị
            var screen = owner.RectangleToScreen(align);
            base.Show(screen.Left, screen.Bottom + 2);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(220, 224, 231));
            using var gp = new GraphicsPath();
            gp.AddRectangle(new Rectangle(0, 0, Width - 1, Height - 1));
            e.Graphics.DrawPath(pen, gp);
        }
    }

    // ====== Control chính xuất hiện như ComboBox ======
    [DefaultEvent("SelectedIndexChanged")]
    [SupportedOSPlatform("windows")]
    public class ComboTableBox : Control
    {
        private readonly DropDownListBox _list;
        private readonly RoundedDropDown _drop;
        private bool _opened;
        private int _selectedIndex = -1;
        private List<TableOption> _items = new();

        // Style
        private int _radius = 10;
        private Color _border = Color.FromArgb(213, 217, 224);
        private Color _hoverBorder = Color.FromArgb(31, 111, 235);
        private Color _back = Color.White;
        private Color _arrow = Color.FromArgb(90, 98, 110);

        public event EventHandler SelectedIndexChanged;

        public ComboTableBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI", 10f);
            Cursor = Cursors.Hand;
            Height = 40;

            _list = new DropDownListBox();
            _list.GetStateColor = StateColor;
            _list.Click += (s, e) => CommitSelection();
            _list.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) CommitSelection(); };

            _drop = new RoundedDropDown(_list);
            _drop.Closed += (s, e) => { _opened = false; Invalidate(); };

            // demo default
            Items = new List<TableOption>()
            {
                new TableOption{ MaBan="B02", SoCho=6, TrangThai="TRỐNG"},
            };
            SelectedIndex = 0;
        }

        // ====== Public API ======
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TableOption> Items
        {
            get => _items;
            set
            {
                _items = value ?? new List<TableOption>();
                _list.Items.Clear();
                _list.Items.AddRange(_items.Cast<object>().ToArray());
                if (_items.Count == 0) SelectedIndex = -1;
                Invalidate();
            }
        }

        [Browsable(false)]
        public TableOption SelectedItem => (_selectedIndex >= 0 && _selectedIndex < _items.Count) ? _items[_selectedIndex] : null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                if (_list.Items.Count > 0 && value >= 0 && value < _list.Items.Count)
                    _list.SelectedIndex = value;
                Invalidate();
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CornerRadius { get => _radius; set { _radius = Math.Max(4, value); Invalidate(); } }

        // ====== Interaction ======
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ToggleDropDown();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        // ====== Render ======
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = ClientRectangle;
            rect.Inflate(-1, -1);

            using (var bg = new SolidBrush(_back))
            using (var gp = CreateRoundRect(rect, _radius))
            using (var pen = new Pen(Focused || _opened ? _hoverBorder : _border))
            {
                g.FillPath(bg, gp);
                g.DrawPath(pen, gp);
            }

            // text hiện tại
            string text = SelectedItem?.ToString() ?? "Chọn bàn...";
            var textRect = new Rectangle(12, 0, Width - 44, Height);
            TextRenderer.DrawText(g, text, Font, textRect, Color.Black,
                TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            // mũi tên
            DrawArrow(g, new Rectangle(Width - 28, (Height - 12) / 2, 16, 12), _arrow, _opened);
        }

        private void ToggleDropDown()
        {
            if (_opened)
            {
                _drop.Close(ToolStripDropDownCloseReason.AppFocusChange);
                return;
            }

            _list.Width = Width - 2;
            _list.Height = Math.Min(Math.Max(Items.Count, 1) * _list.ItemHeight + 6, 260);
            _list.SelectedIndex = Math.Max(0, SelectedIndex);

            _opened = true;
            _drop.Show(this, ClientRectangle, _list.Width, _list.Height);
            _list.Focus();
        }

        private void CommitSelection()
        {
            SelectedIndex = _list.SelectedIndex;
            _drop.Close();
        }

        // ====== Helpers ======
        private static void DrawArrow(Graphics g, Rectangle r, Color c, bool up)
        {
            using var p = new Pen(c, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            var midX = r.Left + r.Width / 2;
            if (up)
            {
                g.DrawLines(p, new[]
                {
                    new Point(r.Left+2, r.Bottom-2),
                    new Point(midX, r.Top+2),
                    new Point(r.Right-2, r.Bottom-2),
                });
            }
            else
            {
                g.DrawLines(p, new[]
                {
                    new Point(r.Left+2, r.Top+2),
                    new Point(midX, r.Bottom-2),
                    new Point(r.Right-2, r.Top+2),
                });
            }
        }

        private static GraphicsPath CreateRoundRect(Rectangle r, int radius)
        {
            var gp = new GraphicsPath();
            int d = radius * 2;
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private Color StateColor(TableOption t)
        {
            var s = (t.TrangThai ?? "").ToUpperInvariant();
            if (s == "TRỐNG") return Color.FromArgb(46, 125, 50);          // xanh lá
            if (s == "ĐÃ ĐẶT" || s == "DA DAT") return Color.FromArgb(183, 28, 28); // đỏ
            if (s == "PHỤC VỤ" || s == "PHUC VU") return Color.FromArgb(230, 81, 0); // cam
            if (s == "VỆ SINH" || s == "VE SINH") return Color.FromArgb(81, 81, 81);  // xám
            return Color.FromArgb(56, 68, 77);
        }
    }

    // ====== extension nhỏ cho FontStyle SemiBold ======
    internal static class FontExt
    {
        // There is no FontStyle.SemiBold in System.Drawing.FontStyle.
        // Use FontStyle.Bold as the closest available style.
        public static FontStyle SemiBold() => FontStyle.Bold;
    }
}
