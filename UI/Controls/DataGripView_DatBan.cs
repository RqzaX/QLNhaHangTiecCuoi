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
    [ToolboxItem(true)]
    [SupportedOSPlatform("windows")]
    public class DataGripView_DatBan : Control
    {
        #region Events
        public event EventHandler<ReservationEventArgs> ViewClicked;
        public event EventHandler<ReservationEventArgs> ConfirmClicked;
        public event EventHandler<ReservationEventArgs> ArrivedClicked;
        public event EventHandler<ReservationEventArgs> EditClicked;
        public event EventHandler<ReservationEventArgs> CancelClicked;
        #endregion

        #region Data Classes
        public class Reservation
        {
            public string Code { get; set; } = "";
            public string CustomerName { get; set; } = "";
            public string Phone { get; set; } = "";
            public DateTime Date { get; set; }
            public string TableName { get; set; } = "";
            public string Area { get; set; } = "";
            public int Guests { get; set; }
            public string Status { get; set; } = "";
            public decimal Deposit { get; set; }
            public string Note { get; set; } = "";
        }

        public class ReservationEventArgs : EventArgs
        {
            public Reservation Reservation { get; set; }
            public ReservationEventArgs(Reservation reservation)
            {
                Reservation = reservation;
            }
        }
        #endregion

        #region Private Fields
        private List<Reservation> _reservations = new List<Reservation>();
        private VScrollBar _vScrollBar;
        private int _itemHeight = 100;
        private int _scrollOffset = 0;
        private int _hoveredIndex = -1;
        private int _selectedIndex = -1;
        private string _iconFolder = "";
        private bool _virtualModeEnabled = false;
        #endregion

        #region Properties
        [Category("Data")]
        public string IconFolder
        {
            get => _iconFolder;
            set => _iconFolder = value ?? "";
        }

        [Category("Behavior")]
        public bool VirtualModeEnabled
        {
            get => _virtualModeEnabled;
            set => _virtualModeEnabled = value;
        }

        [Category("Appearance")]
        public int ItemHeight
        {
            get => _itemHeight;
            set
            {
                _itemHeight = Math.Max(60, value);
                Invalidate();
            }
        }
        #endregion

        #region Constructor
        public DataGripView_DatBan()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            
            Font = new Font("Segoe UI", 10f);
            BackColor = Color.White;
            
            // Initialize scrollbar first
            _vScrollBar = new VScrollBar
            {
                Dock = DockStyle.Right,
                Width = 17,
                Visible = false
            };
            _vScrollBar.Scroll += VScrollBar_Scroll;
            Controls.Add(_vScrollBar);
            
            // Set size after scrollbar is initialized
            Size = new Size(400, 300);
        }
        #endregion

        #region Public Methods
        public void SetData(List<Reservation> reservations)
        {
            _reservations = reservations ?? new List<Reservation>();
            UpdateScrollBar();
            Invalidate();
        }

        public void AddReservation(Reservation reservation)
        {
            if (reservation != null)
            {
                _reservations.Add(reservation);
                UpdateScrollBar();
                Invalidate();
            }
        }

        public void RemoveReservation(string code)
        {
            _reservations.RemoveAll(r => r.Code == code);
            UpdateScrollBar();
            Invalidate();
        }

        public void ClearData()
        {
            _reservations.Clear();
            _scrollOffset = 0;
            _hoveredIndex = -1;
            _selectedIndex = -1;
            UpdateScrollBar();
            Invalidate();
        }
        #endregion

        #region Private Methods
        private void UpdateScrollBar()
        {
            if (_vScrollBar == null) return;
            
            int totalHeight = _reservations.Count * _itemHeight;
            int visibleHeight = ClientSize.Height;
            
            if (totalHeight > visibleHeight)
            {
                _vScrollBar.Visible = true;
                _vScrollBar.Maximum = totalHeight - visibleHeight + _itemHeight;
                _vScrollBar.LargeChange = visibleHeight;
                _vScrollBar.SmallChange = _itemHeight;
            }
            else
            {
                _vScrollBar.Visible = false;
                _scrollOffset = 0;
            }
        }

        private int GetItemIndexFromPoint(Point point)
        {
            int y = point.Y + _scrollOffset;
            int index = y / _itemHeight;
            
            if (index >= 0 && index < _reservations.Count)
                return index;
            
            return -1;
        }

        private Rectangle GetItemRectangle(int index)
        {
            int y = index * _itemHeight - _scrollOffset;
            return new Rectangle(0, y, ClientSize.Width - (_vScrollBar.Visible ? _vScrollBar.Width : 0), _itemHeight);
        }
        #endregion

        #region Event Handlers
        private void VScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            _scrollOffset = e.NewValue;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            int newHoveredIndex = GetItemIndexFromPoint(e.Location);
            if (newHoveredIndex != _hoveredIndex)
            {
                _hoveredIndex = newHoveredIndex;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoveredIndex = -1;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            
            int index = GetItemIndexFromPoint(e.Location);
            if (index >= 0 && index < _reservations.Count)
            {
                _selectedIndex = index;
                var reservation = _reservations[index];
                
                // Determine which button was clicked based on mouse position
                var itemRect = GetItemRectangle(index);
                int totalButtonWidth = (45 + 6) * 5 - 6; // 5 buttons with spacing
                var buttonArea = new Rectangle(itemRect.Right - totalButtonWidth - 15, itemRect.Bottom - 35, totalButtonWidth, 30);
                
                if (buttonArea.Contains(e.Location))
                {
                    // Calculate which button was clicked
                    int buttonWidth = 45;
                    int buttonSpacing = 6;
                    int totalWidth = (buttonWidth + buttonSpacing) * 5 - buttonSpacing;
                    int startX = itemRect.Right - totalWidth - 15;
                    
                    for (int i = 0; i < 5; i++)
                    {
                        var buttonRect = new Rectangle(startX + i * (buttonWidth + buttonSpacing), 
                                                     buttonArea.Y, buttonWidth, buttonArea.Height);
                        
                        if (buttonRect.Contains(e.Location))
                        {
                            switch (i)
                            {
                                case 0: // Xem
                                    ViewClicked?.Invoke(this, new ReservationEventArgs(reservation));
                                    break;
                                case 1: // Xác nhận
                                    ConfirmClicked?.Invoke(this, new ReservationEventArgs(reservation));
                                    break;
                                case 2: // Đã đến
                                    ArrivedClicked?.Invoke(this, new ReservationEventArgs(reservation));
                                    break;
                                case 3: // Sửa
                                    EditClicked?.Invoke(this, new ReservationEventArgs(reservation));
                                    break;
                                case 4: // Hủy
                                    CancelClicked?.Invoke(this, new ReservationEventArgs(reservation));
                                    break;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    // Click on item itself - show details
                    ViewClicked?.Invoke(this, new ReservationEventArgs(reservation));
                }
                
                Invalidate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateScrollBar();
        }
        #endregion

        #region Painting
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            
            // Clear background
            g.Clear(BackColor);
            
            if (_reservations.Count == 0)
            {
                // Draw empty state
                var emptyText = "Không có dữ liệu đặt bàn";
                var font = new Font(Font, FontStyle.Italic);
                var textSize = g.MeasureString(emptyText, font);
                var textRect = new RectangleF(
                    (ClientSize.Width - textSize.Width) / 2,
                    (ClientSize.Height - textSize.Height) / 2,
                    textSize.Width,
                    textSize.Height);
                
                g.DrawString(emptyText, font, Brushes.Gray, textRect);
                return;
            }
            
            // Draw items
            int startIndex = Math.Max(0, _scrollOffset / _itemHeight - 1);
            int endIndex = Math.Min(_reservations.Count - 1, 
                (ClientSize.Height + _scrollOffset) / _itemHeight + 1);
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i >= 0 && i < _reservations.Count)
                {
                    DrawReservationItem(g, _reservations[i], i);
                }
            }
        }

        private void DrawReservationItem(Graphics g, Reservation reservation, int index)
        {
            var itemRect = GetItemRectangle(index);
            if (itemRect.Bottom < 0 || itemRect.Top > ClientSize.Height)
                return;
            
            // Background
            Color bgColor = _hoveredIndex == index ? Color.FromArgb(240, 248, 255) : Color.White;
            if (_selectedIndex == index)
                bgColor = Color.FromArgb(230, 240, 255);
            
            using (var bgBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(bgBrush, itemRect);
            }
            
            // Border
            using (var borderPen = new Pen(_hoveredIndex == index ? Color.FromArgb(99, 102, 241) : Color.FromArgb(230, 232, 236)))
            {
                g.DrawRectangle(borderPen, itemRect);
            }
            
            // Content
            int padding = 15;
            int x = itemRect.X + padding;
            int y = itemRect.Y + padding;
            int contentWidth = itemRect.Width - padding * 2 - 200; // Reserve space for buttons
            
            // Time
            var timeFont = new Font(Font, FontStyle.Bold);
            var timeText = reservation.Date.ToString("HH:mm");
            var timeColor = Color.FromArgb(99, 102, 241);
            g.DrawString(timeText, timeFont, new SolidBrush(timeColor), x, y);
            
            // Customer name and status
            int nameX = x + 60;
            var nameFont = new Font(Font, FontStyle.Bold);
            var nameText = reservation.CustomerName;
            g.DrawString(nameText, nameFont, Brushes.Black, nameX, y);
            
            // Status pill - positioned higher up
            int statusY = itemRect.Bottom - 80; // Position higher up
            DrawStatusPill(g, reservation.Status, new Rectangle(itemRect.Right - 120, statusY, 100, 24));
            
            // Details
            y += 25;
            var detailsFont = new Font(Font, FontStyle.Regular);
            var detailsText = $"Đặt bàn • {reservation.TableName} • {reservation.Guests} khách";
            var detailsColor = Color.FromArgb(110, 119, 129);
            g.DrawString(detailsText, detailsFont, new SolidBrush(detailsColor), nameX, y);
            
            // Phone
            y += 20;
            var phoneText = $"SĐT: {reservation.Phone}";
            g.DrawString(phoneText, detailsFont, new SolidBrush(detailsColor), nameX, y);
            
            // Action buttons
            DrawActionButtons(g, itemRect, reservation);
        }

        private void DrawStatusPill(Graphics g, string status, Rectangle rect)
        {
            Color backColor, foreColor;
            
            switch (status.ToUpper())
            {
                case "CHỜ XÁC NHẬN":
                    backColor = Color.FromArgb(240, 185, 0);
                    foreColor = Color.Black;
                    break;
                case "ĐÃ XÁC NHẬN":
                    backColor = Color.FromArgb(24, 24, 27);
                    foreColor = Color.White;
                    break;
                case "ĐÃ HỦY":
                    backColor = Color.FromArgb(220, 38, 38);
                    foreColor = Color.White;
                    break;
                default:
                    backColor = Color.FromArgb(200, 200, 200);
                    foreColor = Color.Black;
                    break;
            }
            
            using (var brush = new SolidBrush(backColor))
            {
                var pillRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                var path = GetRoundedRectanglePath(pillRect, rect.Height / 2);
                g.FillPath(brush, path);
            }
            
            var font = new Font(Font, FontStyle.Bold);
            var textSize = g.MeasureString(status, font);
            var textRect = new RectangleF(
                rect.X + (rect.Width - textSize.Width) / 2,
                rect.Y + (rect.Height - textSize.Height) / 2,
                textSize.Width,
                textSize.Height);
            
            g.DrawString(status, font, new SolidBrush(foreColor), textRect);
        }

        private void DrawActionButtons(Graphics g, Rectangle itemRect, Reservation reservation)
        {
            int buttonWidth = 45;
            int buttonHeight = 30;
            int buttonSpacing = 6;
            int totalButtonWidth = (buttonWidth + buttonSpacing) * 5 - buttonSpacing;
            int startX = itemRect.Right - totalButtonWidth - 15;
            int startY = itemRect.Bottom - 35;
            
            string[] buttonTexts = { "Xem", "Xác nhận", "Đã đến", "Sửa", "Hủy" };
            Color[] buttonColors = { 
                Color.FromArgb(99, 102, 241),   // View - Blue
                Color.FromArgb(34, 197, 94),    // Confirm - Green
                Color.FromArgb(245, 158, 11),   // Arrived - Orange
                Color.FromArgb(59, 130, 246),   // Edit - Blue
                Color.FromArgb(239, 68, 68)     // Cancel - Red
            };
            
            for (int i = 0; i < buttonTexts.Length; i++)
            {
                var buttonRect = new Rectangle(
                    startX + i * (buttonWidth + buttonSpacing),
                    startY,
                    buttonWidth,
                    buttonHeight);
                
                // Button background
                using (var brush = new SolidBrush(buttonColors[i]))
                {
                    var path = GetRoundedRectanglePath(buttonRect, 6);
                    g.FillPath(brush, path);
                }
                
                // Button text
                var font = new Font("Segoe UI", 8f, FontStyle.Bold);
                var textSize = g.MeasureString(buttonTexts[i], font);
                var textRect = new RectangleF(
                    buttonRect.X + (buttonRect.Width - textSize.Width) / 2,
                    buttonRect.Y + (buttonRect.Height - textSize.Height) / 2,
                    textSize.Width,
                    textSize.Height);
                
                g.DrawString(buttonTexts[i], font, Brushes.White, textRect);
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            
            return path;
        }
        #endregion
    }
}
