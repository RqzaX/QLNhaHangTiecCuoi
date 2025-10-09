// BookingSidebar.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    public enum TableState { Trong, DaDat, DangDung }

    public class TableItem
    {
        public string Name { get; set; } = "Bàn A01";
        public int Seats { get; set; } = 4;
        public TableState State { get; set; } = TableState.Trong;
    }

    public class Area
    {
        public string Name { get; set; } = "Khu A";
        public List<TableItem> Tables { get; set; } = new List<TableItem>();
    }

    [ToolboxItem(true)]
    [SupportedOSPlatform("windows")]
    public class DatBan_Sidebar : UserControl
    {
        // ======= calendar (custom-draw) =======
        private CalendarLite _calendar;

        // ======= layout dưới (legend + khu) =======
        private Panel _legend;
        private FlowLayoutPanel _stack; // danh sách khu (một khu = panel dọc)

        private List<Area> _areas = new();

        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Area> Areas
        {
            get => _areas;
            set { _areas = value ?? new(); RenderAreas(); }
        }

        [Category("Calendar")]
        public DateTime SelectedDate
        {
            get => _calendar.SelectedDate;
            set => _calendar.SelectedDate = value;
        }

        public event EventHandler SelectedDateChanged;

        public event EventHandler<TableItem> TableClicked;

        public DatBan_Sidebar()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            Size = new Size(360, 820); // Set default size here

            // Block "Lịch đặt bàn"
            var lbHeader = SectionHeader("Lịch đặt bàn", null);
            lbHeader.Dock = DockStyle.Top;
            Controls.Add(lbHeader);

            _calendar = new CalendarLite { Dock = DockStyle.Top, Height = 300 };
            _calendar.SelectedDateChanged += (s, e) => SelectedDateChanged?.Invoke(this, EventArgs.Empty);
            Controls.Add(_calendar);

            var div = new Panel { Height = 8, Dock = DockStyle.Top, BackColor = Color.White };
            Controls.Add(div);

            // Legend
            _legend = BuildLegend();
            _legend.Dock = DockStyle.Top;
            Controls.Add(_legend);

            // Block "Sơ đồ bàn"
            var lbHeader2 = SectionHeader("Sơ đồ bàn", null);
            lbHeader2.Dock = DockStyle.Top;
            Controls.Add(lbHeader2);

            // danh sách khu
            _stack = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Padding = new Padding(12, 6, 12, 12)
            };
            Controls.Add(_stack);

            // mẫu dữ liệu demo
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                Areas = DemoAreas();
        }

        private Control SectionHeader(string text, Image? icon)
        {
            var pnl = new Panel { Height = 38, Padding = new Padding(10, 10, 10, 6) };
            var lbl = new Label { AutoSize = true, Text = text, Font = new Font("Segoe UI Semibold", 10.5f) };
            pnl.Controls.Add(lbl);
            return pnl;
        }

        private Panel BuildLegend()
        {
            Panel p = new Panel { Height = 28, Padding = new Padding(12, 0, 12, 0) };
            int x = 12;

            void Item(string title, Color c)
            {
                var dot = new Panel { Width = 14, Height = 14, Left = x, Top = 6, BackColor = c, BorderStyle = BorderStyle.None };
                dot.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using var b = new SolidBrush(c);
                    e.Graphics.FillEllipse(b, 0, 0, dot.Width - 1, dot.Height - 1);
                    using var p2 = new Pen(Color.FromArgb(210, c), 1);
                    e.Graphics.DrawEllipse(p2, 0, 0, dot.Width - 1, dot.Height - 1);
                };
                p.Controls.Add(dot);
                x += 18;

                var lb = new Label { Text = title, Left = x, Top = 6, AutoSize = true, ForeColor = Color.FromArgb(100, 100, 110) };
                p.Controls.Add(lb);
                x += lb.PreferredWidth + 14;
            }

            Item("Trống", Color.FromArgb(46, 204, 113));
            Item("Đã đặt", Color.FromArgb(93, 95, 239));
            Item("Đang dùng", Color.FromArgb(244, 67, 54));

            return p;
        }

        private void RenderAreas()
        {
            _stack.SuspendLayout();
            _stack.Controls.Clear();

            foreach (var area in _areas)
            {
                var wrap = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = true,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0, 4, 0, 12),
                    Padding = new Padding(0)
                };
                // tiêu đề khu
                var lbl = new Label
                {
                    Text = area.Name,
                    Font = new Font("Segoe UI Semibold", 10f),
                    ForeColor = Color.FromArgb(60, 62, 64),
                    AutoSize = true,
                    Margin = new Padding(0, 8, 0, 6)
                };
                var regionPanel = new Panel { Width = _stack.ClientSize.Width - 30, Height = 24, Margin = new Padding(0) };
                regionPanel.Controls.Add(lbl);
                _stack.Controls.Add(regionPanel);

                foreach (var t in area.Tables)
                {
                    var chip = new TableChip { Margin = new Padding(10, 8, 10, 8) };
                    chip.SetData(t.Name, t.Seats, t.State);
                    chip.Click += (s, e) => TableClicked?.Invoke(this, t);
                    wrap.Controls.Add(chip);
                }
                _stack.Controls.Add(wrap);
            }

            _stack.ResumeLayout();
        }

        // ====== Small sub controls ======
        private class TableChip : Control
        {
            public TableChip()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
                Font = new Font("Segoe UI", 10f);
                Size = new Size(148, 64);
                Cursor = Cursors.Hand;
            }

            private string _name = "Bàn A01";
            private int _seats = 4;
            private TableState _state = TableState.Trong;

            public void SetData(string name, int seats, TableState st)
            {
                _name = name; _seats = seats; _state = st; Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                var r = ClientRectangle; r.Inflate(-2, -2);

                (Color border, Color back) = _state switch
                {
                    TableState.Trong => (Color.FromArgb(46, 204, 113), Color.FromArgb(230, 250, 238)),
                    TableState.DaDat => (Color.FromArgb(93, 95, 239), Color.FromArgb(233, 238, 255)),
                    TableState.DangDung => (Color.FromArgb(244, 67, 54), Color.FromArgb(255, 236, 236)),
                    _ => (Color.Gray, Color.White)
                };

                using var bg = new SolidBrush(back);
                using var pen = new Pen(border, 2) { Alignment = PenAlignment.Inset };
                using var path = Round(r, 18);
                g.FillPath(bg, path);
                g.DrawPath(pen, path);

                var title = _name;
                var seats = $"{_seats} chỗ";
                TextRenderer.DrawText(g, title, new Font(Font, FontStyle.Bold),
                    new Point(r.Left + 12, r.Top + 10), Color.FromArgb(40, 40, 45));
                TextRenderer.DrawText(g, seats, new Font(Font.FontFamily, 9f),
                    new Point(r.Left + 12, r.Top + 34), Color.FromArgb(110, 119, 129));
            }
        }

        // ===== Calendar Lite =====
        private class CalendarLite : Control
        {
            public DateTime CurrentMonth { get; private set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            private DateTime _selected = DateTime.Today;

            public DateTime SelectedDate
            {
                get => _selected;
                set { _selected = value.Date; CurrentMonth = new DateTime(_selected.Year, _selected.Month, 1); Invalidate(); SelectedDateChanged?.Invoke(this, EventArgs.Empty); }
            }

            public event EventHandler SelectedDateChanged;

            public CalendarLite()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
                Font = new Font("Segoe UI", 10f);
                Height = 300;
                BackColor = Color.White;
            }

            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                Focus();
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                var hit = HitTest(e.Location);
                if (hit.type == "prev") { CurrentMonth = CurrentMonth.AddMonths(-1); Invalidate(); }
                else if (hit.type == "next") { CurrentMonth = CurrentMonth.AddMonths(1); Invalidate(); }
                else if (hit.type == "day")
                {
                    SelectedDate = hit.day;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;

                var box = new Rectangle(12, 6, Width - 24, Height - 12);
                using (var bg = new SolidBrush(Color.White))
                using (var pen = new Pen(Color.FromArgb(230, 232, 236)))
                using (var path = Round(box, 18))
                {
                    g.FillPath(bg, path);
                    g.DrawPath(pen, path);
                }

                // header
                var monthText = CurrentMonth.ToString("MMMM yyyy", CultureInfo.GetCultureInfo("en-US"));
                var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(monthText);
                var fTitle = new Font("Segoe UI Semibold", 11f);
                var titRect = new Rectangle(box.X, box.Y + 10, box.Width, 24);
                TextRenderer.DrawText(g, title, fTitle, titRect, Color.Black, TextFormatFlags.HorizontalCenter);

                // arrows
                _rcPrev = new Rectangle(box.X + 12, box.Y + 8, 28, 28);
                _rcNext = new Rectangle(box.Right - 40, box.Y + 8, 28, 28);
                DrawArrow(g, _rcPrev, true);
                DrawArrow(g, _rcNext, false);

                // day headers
                string[] days = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
                int gridTop = titRect.Bottom + 8;
                int colW = (box.Width - 24) / 7;
                for (int i = 0; i < 7; i++)
                {
                    var r = new Rectangle(box.X + 12 + i * colW, gridTop, colW, 20);
                    TextRenderer.DrawText(g, days[i], new Font(Font.FontFamily, 9f), r, Color.FromArgb(120, 120, 120), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }

                // dates
                int firstDay = (int)CurrentMonth.DayOfWeek; // 0..6
                int daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);
                int rowH = 34;
                int startY = gridTop + 24;
                int d = 1;

                for (int week = 0; week < 6; week++)
                {
                    for (int wd = 0; wd < 7; wd++)
                    {
                        int index = week * 7 + wd;
                        if (index < firstDay || d > daysInMonth) continue;

                        var cell = new Rectangle(box.X + 12 + wd * colW, startY + week * rowH, colW, rowH);

                        var date = new DateTime(CurrentMonth.Year, CurrentMonth.Month, d);
                        bool isToday = date == DateTime.Today;
                        bool isSelected = date == SelectedDate;

                        if (isSelected)
                        {
                            var sel = new Rectangle(cell.X + (cell.Width - 28) / 2, cell.Y + 3, 28, 28);
                            using var b = new SolidBrush(Color.FromArgb(16, 16, 28));
                            using var pth = Round(sel, 14);
                            g.FillPath(b, pth);
                            TextRenderer.DrawText(g, d.ToString(), new Font(Font, FontStyle.Bold), sel, Color.White,
                                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                        }
                        else
                        {
                            var col = isToday ? Color.FromArgb(73, 92, 245) : Color.FromArgb(60, 62, 64);
                            TextRenderer.DrawText(g, d.ToString(), Font, cell, col,
                                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                        }

                        _dateCells[date] = cell;
                        d++;
                    }
                }
            }

            private Rectangle _rcPrev, _rcNext;
            private readonly Dictionary<DateTime, Rectangle> _dateCells = new();

            private (string type, DateTime day) HitTest(Point p)
            {
                if (_rcPrev.Contains(p)) return ("prev", DateTime.MinValue);
                if (_rcNext.Contains(p)) return ("next", DateTime.MinValue);
                foreach (var kv in _dateCells.ToArray())
                    if (kv.Value.Contains(p)) return ("day", kv.Key);
                return ("", DateTime.MinValue);
            }
        }

        // ===== helpers =====
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

        private static void DrawArrow(Graphics g, Rectangle r, bool left)
        {
            using var b = new SolidBrush(Color.FromArgb(245, 246, 248));
            using var p = new Pen(Color.FromArgb(170, 172, 176), 2) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillEllipse(b, r);
            var cx = r.X + r.Width / 2; var cy = r.Y + r.Height / 2;
            if (left)
            {
                g.DrawLines(p, new[] { new Point(cx + 3, cy - 6), new Point(cx - 3, cy), new Point(cx + 3, cy + 6) });
            }
            else
            {
                g.DrawLines(p, new[] { new Point(cx - 3, cy - 6), new Point(cx + 3, cy), new Point(cx - 3, cy + 6) });
            }
        }

        private static List<Area> DemoAreas() => new()
        {
            new Area
            {
                Name = "Khu A",
                Tables = new()
                {
                    new TableItem{ Name = "Bàn A01", Seats = 4, State = TableState.Trong },
                    new TableItem{ Name = "Bàn A02", Seats = 6, State = TableState.DaDat },
                    new TableItem{ Name = "Bàn A03", Seats = 6, State = TableState.Trong },
                    new TableItem{ Name = "Bàn A04", Seats = 4, State = TableState.Trong },
                    new TableItem{ Name = "Bàn A05", Seats = 6, State = TableState.DaDat },
                }
            },
            new Area
            {
                Name = "Khu B",
                Tables = new()
                {
                    new TableItem{ Name = "Bàn B01", Seats = 8, State = TableState.DangDung },
                    new TableItem{ Name = "Bàn B02", Seats = 6, State = TableState.Trong },
                    new TableItem{ Name = "Bàn B03", Seats = 8, State = TableState.DaDat },
                }
            },
            new Area
            {
                Name = "VIP",
                Tables = new()
                {
                    new TableItem{ Name = "VIP 01", Seats = 10, State = TableState.DaDat },
                    new TableItem{ Name = "VIP 02", Seats = 12, State = TableState.Trong },
                }
            },
        };
    }
}
