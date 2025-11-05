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
    [SupportedOSPlatform("windows")]
    public class KOTTicketCard : UserControl
    {
        // ====== Data model ======
        public class KotItem
        {
            public int Qty { get; set; }
            public string Name { get; set; } = "";
            public override string ToString() => $"{Qty}x {Name}";
        }
        private bool _mergeSameNames = true;

        [Category("Behavior")]
        [Description("Gộp các item trùng tên (không phân biệt hoa thường) và cộng Qty")]
        public bool MergeSameNames
        {
            get => _mergeSameNames;
            set { _mergeSameNames = value; NormalizeItems(); Invalidate(); }
        }

        // Chuẩn hoá danh sách items (gộp trùng)
        private void NormalizeItems()
        {
            if (!_mergeSameNames) return;

            var merged = _items
                .GroupBy(i => (i.Name ?? "").Trim(), StringComparer.CurrentCultureIgnoreCase)
                .Select(g => new KotItem
                {
                    Name = g.Key,
                    Qty = g.Sum(x => Math.Max(0, x.Qty))
                })
                .Where(i => i.Qty > 0 && !string.IsNullOrWhiteSpace(i.Name))
                .ToList();

            _items.Clear();
            _items.AddRange(merged);
        }
        private string _tableName = "Bàn A03";
        private string _ticketCode = "KOT003";
        private DateTime _time = DateTime.Now;
        private string _notes = "";
        private readonly List<KotItem> _items = new List<KotItem>
        {
            new KotItem{ Qty = 4, Name = "Gỏi cuốn tôm thịt"},
            new KotItem{ Qty = 2, Name = "Salad hải sản"},
        };

        // ====== Appearance (editable in Designer) ======
        private int _cornerRadius = 18;
        private Color _cardBackColor = Color.White;
        private Color _borderColor = Color.FromArgb(225, 229, 234);
        private Color _shadowColor = Color.FromArgb(32, 0, 0, 0);
        private Padding _cardPadding = new Padding(18, 16, 18, 16);

        // Button style
        private string _actionText = "Bắt đầu làm";
        private Color _btnColor = Color.FromArgb(12, 15, 28);      // dark navy
        private Color _btnHover = Color.FromArgb(20, 24, 45);
        private Color _btnText = Color.White;
        private int _btnHeight = 40;
        private int _btnRadius = 12;

        // Secondary button (e.g., In món)
        private string _secondaryText = "In món";
        private bool _secondaryVisible = true;
        private RectangleF _secBtnRect;
        private bool _secHovering;
        private bool _secPressed;

        // State
        private bool _btnHovering;
        private bool _btnPressed;
        private RectangleF _btnRect;

        // Fonts
        private Font _fontTitle;
        private Font _fontMeta;
        private Font _fontItem;
        private Font _fontButton;

        public KOTTicketCard()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.Transparent;
            Size = new Size(400, 220);

            _fontTitle = new Font("Segoe UI", 12.0f, FontStyle.Bold);
            _fontMeta = new Font("Segoe UI", 10.0f, FontStyle.Regular);
            _fontItem = new Font("Segoe UI", 10.5f, FontStyle.Regular);
            _fontButton = new Font("Segoe UI", 10.5f, FontStyle.Bold);

            // Mouse delegate for button
            MouseMove += (s, e) =>
            {
                bool h = _btnRect.Contains(e.Location);
                bool hs = _secBtnRect.Contains(e.Location);
                if (h != _btnHovering || hs != _secHovering) { _btnHovering = h; _secHovering = hs; Invalidate(); }
            };
            MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left && _btnRect.Contains(e.Location))
                {
                    _btnPressed = true; Invalidate();
                }
                if (e.Button == MouseButtons.Left && _secBtnRect.Contains(e.Location))
                {
                    _secPressed = true; Invalidate();
                }
            };
            MouseUp += (s, e) =>
            {
                if (_btnPressed)
                {
                    _btnPressed = false; Invalidate();
                    if (_btnRect.Contains(e.Location)) OnStartClicked(EventArgs.Empty);
                }
                if (_secPressed)
                {
                    _secPressed = false; Invalidate();
                    if (_secBtnRect.Contains(e.Location)) OnSecondaryClicked(EventArgs.Empty);
                }
            };
            MouseLeave += (s, e) =>
            {
                if (_btnHovering || _btnPressed || _secHovering || _secPressed) { _btnHovering = _btnPressed = _secHovering = _secPressed = false; Invalidate(); }
            };
        }

        // ====== Designer properties ======
        [Category("Data")]
        public string TableName { get => _tableName; set { _tableName = value; Invalidate(); } }

        [Category("Data")]
        public string TicketCode { get => _ticketCode; set { _ticketCode = value; Invalidate(); } }

        [Category("Data")]
        public DateTime OrderTime { get => _time; set { _time = value; Invalidate(); } }

        [Category("Data")]
        public string Notes { get => _notes; set { _notes = value; Invalidate(); } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Data")]
        public List<KotItem> Items => _items;

        [Category("Appearance")]
        public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(6, value); Invalidate(); } }

        [Category("Appearance")]
        public Padding CardPadding { get => _cardPadding; set { _cardPadding = value; Invalidate(); } }

        [Category("Appearance")]
        public Color CardBackColor { get => _cardBackColor; set { _cardBackColor = value; Invalidate(); } }

        [Category("Appearance")]
        public Color BorderColor { get => _borderColor; set { _borderColor = value; Invalidate(); } }

        [Category("Appearance")]
        public string ActionText { get => _actionText; set { _actionText = value; Invalidate(); } }

        [Category("Appearance")]
        public string SecondaryText { get => _secondaryText; set { _secondaryText = value; Invalidate(); } }

        [Category("Appearance")]
        public bool SecondaryVisible { get => _secondaryVisible; set { _secondaryVisible = value; Invalidate(); } }

        [Category("Appearance")]
        public Color ButtonColor { get => _btnColor; set { _btnColor = value; Invalidate(); } }

        [Category("Appearance")]
        public Color ButtonHoverColor { get => _btnHover; set { _btnHover = value; Invalidate(); } }

        [Category("Appearance")]
        public Color ButtonTextColor { get => _btnText; set { _btnText = value; Invalidate(); } }

        [Category("Appearance")]
        public int ButtonHeight { get => _btnHeight; set { _btnHeight = Math.Max(28, value); Invalidate(); } }

        [Category("Appearance")]
        public int ButtonRadius { get => _btnRadius; set { _btnRadius = Math.Max(6, value); Invalidate(); } }

        // ====== Event ======
        public event EventHandler StartClicked;
        protected virtual void OnStartClicked(EventArgs e) => StartClicked?.Invoke(this, e);

        public event EventHandler SecondaryClicked;
        protected virtual void OnSecondaryClicked(EventArgs e) => SecondaryClicked?.Invoke(this, e);

        // ====== Layout helpers ======
        private static GraphicsPath RoundedRect(RectangleF bounds, float radius)
        {
            var path = new GraphicsPath();
            float d = radius * 2f;
            if (radius <= 0f) { path.AddRectangle(bounds); path.CloseFigure(); return path; }
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Parent?.BackColor ?? SystemColors.Control);

            var card = RectangleF.Inflate(ClientRectangle, -4, -4); // leave room for shadow
            // Shadow
            using (var shadowPath = RoundedRect(new RectangleF(card.X + 2, card.Y + 4, card.Width, card.Height), _cornerRadius + 2))
            using (var shadow = new SolidBrush(_shadowColor))
                g.FillPath(shadow, shadowPath);

            // Card
            using (var path = RoundedRect(card, _cornerRadius))
            using (var back = new SolidBrush(_cardBackColor))
            using (var pen = new Pen(_borderColor, 1))
            {
                g.FillPath(back, path);
                g.DrawPath(pen, path);
            }

            // Content layout
            var x = card.X + _cardPadding.Left;
            var y = card.Y + _cardPadding.Top;
            float right = card.Right - _cardPadding.Right;

            // Row 1: Title (TableName) + time with clock icon
            using (var titleBrush = new SolidBrush(Color.FromArgb(30, 33, 40)))
            using (var metaBrush = new SolidBrush(Color.FromArgb(90, 95, 110)))
            {
                // Title
                g.DrawString(_tableName, _fontTitle, titleBrush, x, y);
                // Time (🕒)
                string timeStr = _time.ToString("HH:mm");
                var timeText = "Thời gian: " + timeStr;
                var sz = g.MeasureString(timeText, _fontMeta);
                g.DrawString(timeText, _fontMeta, metaBrush, right - sz.Width, y + 2);
            }
            y += _fontTitle.Height + 10;

            // Row 2: Chip "KOT003" + chef icon
            float chipH = 24f;
            string chipText = _ticketCode;
            var chipPad = new SizeF(12, 2);
            var chipSize = TextRenderer.MeasureText(chipText, _fontMeta);
            var chipRect = new RectangleF(x, y, chipSize.Width + chipPad.Width * 2, chipH);

            using (var chipPath = RoundedRect(chipRect, chipH / 2f))
            using (var chipBack = new SolidBrush(Color.FromArgb(244, 246, 249)))
            using (var chipBorder = new Pen(Color.FromArgb(220, 224, 230)))
            using (var chipTextBrush = new SolidBrush(Color.FromArgb(70, 75, 90)))
            {
                g.FillPath(chipBack, chipPath);
                g.DrawPath(chipBorder, chipPath);
                g.DrawString(chipText, _fontMeta, chipTextBrush, chipRect.X + chipPad.Width - 2, chipRect.Y + 3);
            }

            using (var metaBrush = new SolidBrush(Color.FromArgb(90, 95, 110)))
            {
                string chef = "Nhà bếp";
                var szChef = g.MeasureString(chef, _fontMeta);
                g.DrawString(chef, _fontMeta, metaBrush, chipRect.Right + 8, y + (chipH - szChef.Height) / 2f);
            }
            y += chipH + 12;

            // Items
            using (var itemBrush = new SolidBrush(Color.FromArgb(40, 44, 52)))
            {
                foreach (var it in _items)
                {
                    g.DrawString(it.ToString(), _fontItem, itemBrush, x, y);
                    y += _fontItem.Height + 6;
                }
            }

            // Notes (if any)
            if (!string.IsNullOrWhiteSpace(_notes))
            {
                y += 8; // Extra spacing before notes
                using (var notesBrush = new SolidBrush(Color.FromArgb(120, 120, 120)))
                using (var notesFont = new Font("Segoe UI", 9.0f, FontStyle.Italic))
                {
                    string notesText = "Ghi chú: " + _notes;
                    g.DrawString(notesText, notesFont, notesBrush, x, y);
                    y += notesFont.Height + 8;
                }
            }

            // Buttons
            y = Math.Max(y, card.Bottom - _cardPadding.Bottom - _btnHeight);
            float secWidth = _secondaryVisible ? 110f : 0f;
            float gap = _secondaryVisible ? 8f : 0f;
            if (_secondaryVisible)
            {
                _secBtnRect = new RectangleF(right - secWidth, y, secWidth, _btnHeight);
            }
            else
            {
                _secBtnRect = RectangleF.Empty;
            }
            _btnRect = new RectangleF(x, y, (right - x) - (secWidth + gap), _btnHeight);
            using (var btnPath = RoundedRect(_btnRect, _btnRadius))
            using (var btnBack = new SolidBrush(_btnPressed
                       ? ControlPaint.Dark(_btnHovering ? _btnHover : _btnColor)
                       : (_btnHovering ? _btnHover : _btnColor)))
            using (var btnTextBrush = new SolidBrush(_btnText))
            {
                g.FillPath(btnBack, btnPath);
                // center text
                var tSize = g.MeasureString(_actionText, _fontButton);
                var tx = _btnRect.X + (_btnRect.Width - tSize.Width) / 2f;
                var ty = _btnRect.Y + (_btnRect.Height - tSize.Height) / 2f;
                g.DrawString(_actionText, _fontButton, btnTextBrush, tx, ty + 1);
            }

            // Secondary button (outline style)
            if (_secondaryVisible)
            {
                using (var secPath = RoundedRect(_secBtnRect, _btnRadius))
                using (var secBack = new SolidBrush(Color.White))
                using (var secBorder = new Pen(_btnColor, 1.2f))
                using (var secTextBrush = new SolidBrush(_btnColor))
                {
                    if (_secPressed)
                    {
                        g.FillPath(new SolidBrush(Color.FromArgb(245, 245, 245)), secPath);
                    }
                    else if (_secHovering)
                    {
                        g.FillPath(new SolidBrush(Color.FromArgb(250, 250, 250)), secPath);
                    }
                    else
                    {
                        g.FillPath(secBack, secPath);
                    }
                    g.DrawPath(secBorder, secPath);
                    var tSize2 = g.MeasureString(_secondaryText, _fontButton);
                    var tx2 = _secBtnRect.X + (_secBtnRect.Width - tSize2.Width) / 2f;
                    var ty2 = _secBtnRect.Y + (_secBtnRect.Height - tSize2.Height) / 2f;
                    g.DrawString(_secondaryText, _fontButton, secTextBrush, tx2, ty2 + 1);
                }
            }
        }

        // ====== Public helpers ======
        public void SetItems(IEnumerable<KotItem> items)
        {
            _items.Clear();
            if (items != null) _items.AddRange(items);
            NormalizeItems();
            Invalidate();
        }

        public void AddItem(int qty, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || qty <= 0) return;
            _items.Add(new KotItem { Qty = qty, Name = name });
            NormalizeItems();
            Invalidate();
        }
    }
}
