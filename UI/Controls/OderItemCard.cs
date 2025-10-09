using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms.Design;

namespace UI.Controls
{
    [ToolboxItem(true)]
    [DefaultProperty("ItemName")]
    [DefaultEvent("QuantityChanged")]
    [SupportedOSPlatform("windows")]
    public class OrderItemCard : Control
    {
        // ====== Data ======
        private string _itemName = "Tôm hùm nướng phô mai";
        private decimal _unitPrice = 850000m;
        private int _quantity = 1;
        private string _note = "";

        // ====== Style (có DefaultValue + ShouldSerialize/Reset) ======
        private int _cornerRadius = 16;
        [Category("Appearance"), DefaultValue(16), Description("Bán kính bo góc của thẻ.")]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(4, value); Invalidate(); }
        }
        public bool ShouldSerializeCornerRadius() => _cornerRadius != 16;
        public void ResetCornerRadius() => CornerRadius = 16;

        private Color _cardBackColor = Color.White;
        [Category("Appearance"), DefaultValue(typeof(Color), "White")]
        public Color CardBackColor
        {
            get => _cardBackColor;
            set { _cardBackColor = value; Invalidate(); }
        }
        public bool ShouldSerializeCardBackColor() => _cardBackColor != Color.White;
        public void ResetCardBackColor() => CardBackColor = Color.White;

        private Color _borderColor = Color.FromArgb(230, 232, 236);
        [Category("Appearance"), DefaultValue(typeof(Color), "230,232,236")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        private Color _shadowColor = Color.FromArgb(40, 0, 0, 0);
        [Category("Appearance"), DefaultValue(typeof(Color), "40,0,0,0")]
        public Color ShadowColor
        {
            get => _shadowColor;
            set { _shadowColor = value; Invalidate(); }
        }

        private Color _titleColor = Color.Black;
        [Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        public Color TitleColor { get => _titleColor; set { _titleColor = value; Invalidate(); } }

        private Color _subTextColor = Color.FromArgb(120, 120, 120);
        [Category("Appearance"), DefaultValue(typeof(Color), "120,120,120")]
        public Color SubTextColor { get => _subTextColor; set { _subTextColor = value; Invalidate(); } }

        private Color _accentColor = Color.FromArgb(31, 111, 235);
        [Category("Appearance"), DefaultValue(typeof(Color), "31,111,235")]
        public Color AccentColor { get => _accentColor; set { _accentColor = value; Invalidate(); } }

        private Color _dangerColor = Color.FromArgb(214, 45, 32);
        [Category("Appearance"), DefaultValue(typeof(Color), "214,45,32")]
        public Color DangerColor { get => _dangerColor; set { _dangerColor = value; Invalidate(); } }

        private bool _showShadow = true;
        [Category("Appearance"), DefaultValue(true)]
        public bool ShowShadow { get => _showShadow; set { _showShadow = value; Invalidate(); } }

        // ====== API (để Designer lưu, KHÔNG Hidden) ======
        [Category("Data"), DefaultValue("Tôm hùm nướng phô mai")]
        public string ItemName { get => _itemName; set { _itemName = value ?? ""; Invalidate(); } }

        [Category("Data"), DefaultValue(typeof(decimal), "850000")]
        public decimal UnitPrice { get => _unitPrice; set { _unitPrice = Math.Max(0, value); Invalidate(); } }

        [Category("Data"), DefaultValue(1)]
        public int Quantity
        {
            get => _quantity;
            set
            {
                int v = Math.Max(0, value);
                if (_quantity != v)
                {
                    _quantity = v; Invalidate();
                    QuantityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Category("Data"), DefaultValue(""), Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Note { get => _note; set { _note = value ?? ""; Invalidate(); } }

        [Browsable(false)]
        public decimal LineTotal => UnitPrice * Quantity;

        // ====== Events ======
        public event EventHandler QuantityChanged;
        public event EventHandler NoteClicked;
        public event EventHandler DeleteClicked;

        // hit-boxes
        Rectangle _rcMinus, _rcPlus, _rcNote, _rcDelete;

        bool _hoverMinus, _hoverPlus, _hoverNote, _hoverDelete;

        public OrderItemCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI", 10f);
            Size = new Size(400, 120);
            Cursor = Cursors.Arrow;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = ClientRectangle;

            // shadow
            if (ShowShadow)
            {
                var shadow = rect; shadow.Inflate(-2, -2); shadow.Offset(0, 2);
                using var sb = new SolidBrush(ShadowColor);
                using var gpS = Round(shadow, CornerRadius + 4);
                g.FillPath(sb, gpS);
            }

            // card
            var card = rect; card.Inflate(-2, -4);
            using (var bg = new SolidBrush(CardBackColor))
            using (var border = new Pen(BorderColor))
            using (var gp = Round(card, CornerRadius))
            {
                g.FillPath(bg, gp);
                g.DrawPath(border, gp);
            }

            // layout
            var pad = 14;
            var y = card.Y + pad;
            var x = card.X + pad;

            // Header: name + delete icon
            var titleRect = new Rectangle(x, y, card.Width - 2 * pad - 24, 24);
            TextRenderer.DrawText(g, ItemName, new Font(Font, FontStyle.Bold),
                titleRect, TitleColor, TextFormatFlags.EndEllipsis);

            // Delete icon (trash) – nhỏ màu đỏ
            _rcDelete = new Rectangle(card.Right - pad - 18, y + 2, 18, 18);
            DrawTrash(g, _rcDelete, _hoverDelete ? ControlPaint.Light(DangerColor) : DangerColor);

            // Unit price (sub text)
            y += 24;
            var priceStr = ToVnCurrency(UnitPrice);
            TextRenderer.DrawText(g, priceStr, new Font(Font, FontStyle.Regular),
                new Rectangle(x, y, 160, 20), SubTextColor);

            // Right: line total
            var totalStr = ToVnCurrency(LineTotal);
            TextRenderer.DrawText(g, totalStr, new Font(Font, FontStyle.Bold),
                new Rectangle(card.Right - pad - 160, y, 160, 22),
                TitleColor, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);

            // Quantity group ( -  qty  + )
            y += 26;
            int btnSize = 32;
            int spacing = 10;
            _rcMinus = new Rectangle(x, y, btnSize, btnSize);
            var rcQtyText = new Rectangle(_rcMinus.Right + spacing, y, 40, btnSize);
            _rcPlus = new Rectangle(rcQtyText.Right + spacing, y, btnSize, btnSize);

            DrawCircleButton(g, _rcMinus, "–", _hoverMinus);
            DrawQtyText(g, rcQtyText);
            DrawCircleButton(g, _rcPlus, "+", _hoverPlus);

            // Note pill button
            y += btnSize + 12;
            _rcNote = new Rectangle(x, y, 120, 34);
            DrawNotePill(g, _rcNote, _hoverNote, Note);

            // bottom total (right) again for bố cục giống mẫu
            // (đã vẽ phía trên; nếu muốn rõ hơn, uncomment khối dưới)
            // TextRenderer.DrawText(g, totalStr, new Font(Font, FontStyle.SemiBold),
            //     new Rectangle(card.Right - pad - 160, y - (btnSize + 12), 160, 22),
            //     TitleColor, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        }

        // ====== Drawing helpers ======
        private void DrawCircleButton(Graphics g, Rectangle r, string text, bool hover)
        {
            using var b = new SolidBrush(hover ? Color.FromArgb(245, 247, 252) : Color.White);
            using var p = new Pen(hover ? AccentColor : BorderColor);
            using var gp = new GraphicsPath();
            gp.AddEllipse(r);
            g.FillPath(b, gp);
            g.DrawPath(p, gp);
            TextRenderer.DrawText(g, text, new Font(Font.FontFamily, 12f, FontStyle.Bold),
                r, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawQtyText(Graphics g, Rectangle r)
        {
            TextRenderer.DrawText(g, Quantity.ToString(), new Font(Font, FontStyle.Regular),
                r, TitleColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawNotePill(Graphics g, Rectangle r, bool hover, string note)
        {
            using var b = new SolidBrush(hover ? Color.FromArgb(245, 247, 252) : Color.White);
            using var p = new Pen(hover ? AccentColor : BorderColor);
            using var gp = Round(r, 16);
            g.FillPath(b, gp);
            g.DrawPath(p, gp);

            // icon giấy
            var ic = new Rectangle(r.X + 10, r.Y + 8, 18, 18);
            DrawNoteIcon(g, ic, SubTextColor);

            var text = string.IsNullOrWhiteSpace(note) ? "Ghi chú" : note;
            TextRenderer.DrawText(g, text, Font,
                new Rectangle(ic.Right + 6, r.Y, r.Width - (ic.Right + 10 - r.X), r.Height),
                SubTextColor, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private static void DrawNoteIcon(Graphics g, Rectangle r, Color color)
        {
            using var pen = new Pen(color, 1.8f) { LineJoin = LineJoin.Round };
            var gp = new GraphicsPath();
            gp.AddRectangle(new Rectangle(r.X + 3, r.Y + 3, r.Width - 6, r.Height - 6));
            g.DrawPath(pen, gp);
            // gáy sổ
            g.DrawLine(pen, r.X + 7, r.Y + 3, r.X + 7, r.Bottom - 3);
        }

        private static void DrawTrash(Graphics g, Rectangle r, Color color)
        {
            using var pen = new Pen(color, 1.8f) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };
            // nắp
            g.DrawLine(pen, r.X + 3, r.Y + 6, r.Right - 3, r.Y + 6);
            g.DrawLine(pen, r.X + 7, r.Y + 4, r.Right - 7, r.Y + 4);
            // thân
            var body = new Rectangle(r.X + 4, r.Y + 7, r.Width - 8, r.Height - 8);
            g.DrawRectangle(pen, body);
            // 2 gạch
            g.DrawLine(pen, body.X + 5, body.Y + 3, body.X + 5, body.Bottom - 3);
            g.DrawLine(pen, body.Right - 5, body.Y + 3, body.Right - 5, body.Bottom - 3);
        }

        private static GraphicsPath Round(Rectangle r, int radius)
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

        private static string ToVnCurrency(decimal money)
        {
            var ci = new CultureInfo("vi-VN");
            return string.Format(ci, "{0:#,0} đ", money);
        }

        // ====== Interaction ======
        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool hvMinus = _rcMinus.Contains(e.Location);
            bool hvPlus = _rcPlus.Contains(e.Location);
            bool hvNote = _rcNote.Contains(e.Location);
            bool hvDelete = _rcDelete.Contains(e.Location);

            if (hvMinus != _hoverMinus || hvPlus != _hoverPlus ||
                hvNote != _hoverNote || hvDelete != _hoverDelete)
            {
                _hoverMinus = hvMinus; _hoverPlus = hvPlus; _hoverNote = hvNote; _hoverDelete = hvDelete;
                Cursor = (hvMinus || hvPlus || hvNote || hvDelete) ? Cursors.Hand : Cursors.Arrow;
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hoverMinus = _hoverPlus = _hoverNote = _hoverDelete = false;
            Cursor = Cursors.Arrow;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_rcMinus.Contains(e.Location))
            {
                if (Quantity > 0) Quantity -= 1;
            }
            else if (_rcPlus.Contains(e.Location))
            {
                Quantity += 1;
            }
            else if (_rcNote.Contains(e.Location))
            {
                NoteClicked?.Invoke(this, EventArgs.Empty);
            }
            else if (_rcDelete.Contains(e.Location))
            {
                DeleteClicked?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
