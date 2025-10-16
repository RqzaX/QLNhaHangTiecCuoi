using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    [DefaultEvent("ValueChanged")]
    public class TextBox_ChonNgayThangNam : UserControl
    {
        private TextBox txtDisplay;
        private PictureBox picCalendar;
        private ToolStripDropDown dropDown;
        private MonthCalendar monthCalendar;
        private ContextMenuStrip ctx;
        private string _displayFormat = "dd/MM/yyyy";
        private DateTime? _selectedDate = null;

        [Category("Appearance")]
        [Description("Format hiển thị ngày (e.g. dd/MM/yyyy)")]
        public string DisplayFormat
        {
            get => _displayFormat;
            set
            {
                _displayFormat = value ?? "dd/MM/yyyy";
                UpdateText();
            }
        }

        [Category("Behavior")]
        [Description("Ngày được chọn, null nếu không chọn")]
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                UpdateText();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Category("Behavior")]
        [Description("Min selectable date")]
        public DateTime MinDate
        {
            get => monthCalendar.MinDate;
            set => monthCalendar.MinDate = value;
        }

        [Category("Behavior")]
        [Description("Max selectable date")]
        public DateTime MaxDate
        {
            get => monthCalendar.MaxDate;
            set => monthCalendar.MaxDate = value;
        }

        [Browsable(true)]
        public override string Text
        {
            get => SelectedDate?.ToString(DisplayFormat) ?? string.Empty;
            set
            {
                if (DateTime.TryParseExact(value, DisplayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                    SelectedDate = dt;
                else if (string.IsNullOrWhiteSpace(value))
                    SelectedDate = null;
                else
                {
                    // attempt fallback
                    if (DateTime.TryParse(value, out var d2)) SelectedDate = d2;
                }
            }
        }

        public event EventHandler ValueChanged;

        public TextBox_ChonNgayThangNam()
        {
            InitializeComponents();
            Size = new Size(180, 30);
            BackColor = Color.Transparent;
        }

        private void InitializeComponents()
        {
            // TextBox (display)
            txtDisplay = new TextBox
            {
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 6, 30, 6),
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.White,
            };

            // Calendar icon
            picCalendar = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.CenterImage,
                Dock = DockStyle.Right,
                Width = 30,
                Cursor = Cursors.Hand,
                Image = DrawCalendarIcon(Color.FromArgb(80, 80, 80), 16) // small icon
            };
            picCalendar.Click += PicCalendar_Click;

            // Panel border (rounded-ish by padding)
            var outer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 6, 6, 6),
                BackColor = Color.White,
            };
            outer.Controls.Add(txtDisplay);
            outer.Controls.Add(picCalendar);
            outer.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var pen = new Pen(Color.FromArgb(200, 200, 200)))
                using (var brush = new SolidBrush(outer.BackColor))
                {
                    var r = new Rectangle(0, 0, outer.Width - 1, outer.Height - 1);
                    var path = RoundedRect(r, 10);
                    g.FillPath(brush, path);
                    g.DrawPath(pen, path);
                }
            };

            Controls.Add(outer);

            // MonthCalendar hosted in ToolStripDropDown
            monthCalendar = new MonthCalendar
            {
                MaxSelectionCount = 1,
                ShowToday = true,
                ShowTodayCircle = true,
                BackColor = Color.White,
            };
            monthCalendar.DateSelected += MonthCalendar_DateSelected;

            var host = new ToolStripControlHost(monthCalendar) { Margin = Padding.Empty, Padding = Padding.Empty };
            dropDown = new ToolStripDropDown { Padding = Padding.Empty, AutoClose = true };
            dropDown.Items.Add(host);

            // Context menu for clearing
            ctx = new ContextMenuStrip();
            var clearItem = new ToolStripMenuItem("Xóa ngày");
            clearItem.Click += (s, e) => { SelectedDate = null; };
            ctx.Items.Add(clearItem);

            // Click on textbox to open calendar
            txtDisplay.Click += (s, e) => ShowCalendar();
            txtDisplay.ContextMenuStrip = ctx;

            // keyboard: Backspace clears, Delete clears
            txtDisplay.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    SelectedDate = null;
                    e.Handled = true;
                }
            };

            // initial text
            UpdateText();
        }

        private void PicCalendar_Click(object sender, EventArgs e)
        {
            ShowCalendar();
        }

        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            SelectedDate = e.Start.Date;
            dropDown.Close();
        }

        private void ShowCalendar()
        {
            if (SelectedDate.HasValue)
            {
                monthCalendar.SetDate(SelectedDate.Value);
            }
            var p = PointToScreen(new Point(0, Height));
            dropDown.Show(p);
        }

        private void UpdateText()
        {
            if (_selectedDate.HasValue)
            {
                try
                {
                    txtDisplay.ForeColor = Color.Black;
                    txtDisplay.Text = _selectedDate.Value.ToString(DisplayFormat, CultureInfo.InvariantCulture);
                }
                catch
                {
                    txtDisplay.Text = _selectedDate.Value.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                txtDisplay.ForeColor = Color.Gray;
                txtDisplay.Text = DisplayFormat; // placeholder
            }
            Invalidate();
        }

        // helper to draw a simple calendar icon (vector-ish)
        private static Bitmap DrawCalendarIcon(Color color, int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var pen = new Pen(color, 1.5f))
                {
                    g.DrawRectangle(pen, 2, 3, size - 5, size - 6);
                    g.DrawLine(pen, 2, 7, size - 3, 7);
                    g.FillRectangle(new SolidBrush(color), 4, 5, 2, 2);
                    g.FillRectangle(new SolidBrush(color), size - 7, 5, 2, 2);
                }
            }
            return bmp;
        }

        // rounded rectangle path
        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // public helper to clear
        public void Clear() => SelectedDate = null;

        // optional: set by code
        public void SetDate(DateTime dt) => SelectedDate = dt.Date;
    }
}
