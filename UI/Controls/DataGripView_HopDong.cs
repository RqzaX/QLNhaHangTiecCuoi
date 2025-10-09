using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class DataGripView_HopDong : UserControl
    {
        public DataGridView Grid { get; } = new DataGridView();

        public DataGripView_HopDong()
        {
            DoubleBuffered = true;
            BackColor = Color.White;
            Dock = DockStyle.Fill;

            Grid.Dock = DockStyle.Fill;
            Grid.BorderStyle = BorderStyle.None;
            Grid.BackgroundColor = Color.White;
            Grid.EnableHeadersVisualStyles = false;
            Grid.AutoGenerateColumns = false;
            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.AllowUserToResizeRows = false;
            Grid.MultiSelect = false;
            Grid.ReadOnly = true;
            Grid.RowHeadersVisible = false;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.ColumnHeadersHeight = 36;
            Grid.RowTemplate.Height = 44;
            Grid.GridColor = Color.FromArgb(231, 235, 240);

            Grid.DefaultCellStyle.NullValue = string.Empty;
            // Không bật popup lỗi mặc định
            Grid.DataError += (s, e) => { e.ThrowException = false; };

            // Header style
            Grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(71, 85, 105),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Padding = new Padding(12, 0, 12, 0)
            };

            // Row style
            Grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 41, 59),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                SelectionBackColor = Color.FromArgb(239, 246, 255),
                SelectionForeColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(12, 0, 12, 0)
            };

            Grid.CellPainting += Grid_CellPainting;
            Grid.CellMouseEnter += (s, e) => Grid.Cursor = (IsActionCell(e)) ? Cursors.Hand : Cursors.Arrow;
            Grid.CellMouseLeave += (s, e) => Grid.Cursor = Cursors.Arrow;

            BuildColumns();
            Controls.Add(Grid);

            // Demo data
            var demo = new List<ContractRow>
            {
                new("HD001","Nguyễn Văn A & Trần Thị B","0901234567","Tiệc cưới","Sảnh Diamond",DateTime.Parse("2025-11-15"),400,250_000_000,75_000_000,"Đã ký"),
                new("HD002","Lê Minh C & Phạm Thu D","0912345678","Tiệc cưới","Sảnh Ruby",DateTime.Parse("2025-12-05"),250,180_000_000,54_000_000,"Chờ ký"),
                new("HD003","Công ty ABC","0287654321","Hội nghị khách hàng","Sảnh Emerald",DateTime.Parse("2025-10-25"),150,85_000_000,25_500_000,"Hoàn thành"),
                new("HD004","Hoàng Văn E","0923456789","Sinh nhật 60 tuổi","Sảnh Pearl",DateTime.Parse("2025-11-20"),100,60_000_000,18_000_000,"Đã ký"),
                new("HD005","Võ Thị F","0934567890","Tiệc cưới","Sảnh Sapphire",DateTime.Parse("2025-12-20"),300,220_000_000,0,"Nháp"),
            };
            SetData(demo);
        }

        public void SetData(IEnumerable<ContractRow> items)
        {
            Grid.Rows.Clear();
            foreach (var it in items)
            {
                var remain = it.Total - it.Paid;
                Grid.Rows.Add(
                    it.Code,
                    new IconText("file", it.Customer, it.Phone),
                    it.EventName,
                    new IconText("lock", it.Hall, null),
                    it.Date?.ToString("dd/MM/yyyy"),
                    new IconText("people", it.Guests.ToString(), null),
                    new Currency(it.Total),
                    new Currency(it.Paid, success: true),
                    new Currency(remain, danger: remain > 0),
                    new Badge(it.Status),
                    new ActionIcons(true, true, true)
                );
            }
        }

        private void BuildColumns()
        {
            Grid.Columns.Clear();

            Grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCode",
                HeaderText = "Mã HĐ",
                DataPropertyName = "Code",
                Width = 110
            });

            Grid.Columns.Add(new DataGridViewIconTextColumn
            {
                Name = "colCustomer",
                HeaderText = "Khách hàng",
                Width = 260
            });

            Grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEvent",
                HeaderText = "Sự kiện",
                Width = 180
            });

            Grid.Columns.Add(new DataGridViewIconTextColumn
            {
                Name = "colHall",
                HeaderText = "Sảnh",
                Width = 180
            });

            Grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDate",
                HeaderText = "Ngày tổ chức",
                Width = 120
            });

            Grid.Columns.Add(new DataGridViewIconTextColumn
            {
                Name = "colGuests",
                HeaderText = "Khách",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            Grid.Columns.Add(new DataGridViewCurrencyColumn
            {
                Name = "colTotal",
                HeaderText = "Tổng giá trị",
                Width = 140
            });

            Grid.Columns.Add(new DataGridViewCurrencyColumn
            {
                Name = "colPaid",
                HeaderText = "Đã thu",
                Width = 120
            });

            Grid.Columns.Add(new DataGridViewCurrencyColumn
            {
                Name = "colRemain",
                HeaderText = "Còn lại",
                Width = 130
            });

            Grid.Columns.Add(new DataGridViewBadgeColumn
            {
                Name = "colStatus",
                HeaderText = "Trạng thái",
                Width = 110
            });

            Grid.Columns.Add(new DataGridViewActionColumn
            {
                Name = "colActions",
                HeaderText = "Thao tác",
                Width = 120
            });
        }

        private bool IsActionCell(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return false;
            return Grid.Columns[e.ColumnIndex] is DataGridViewActionColumn;
        }

        // Row hover highlight with rounded separator look
        private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                using var bg = new SolidBrush(e.State.HasFlag(DataGridViewElementStates.Selected)
                    ? Color.FromArgb(239, 246, 255) : Color.White);
                e.Graphics.FillRectangle(bg, e.CellBounds);

                // draw bottom line
                using var pen = new Pen(Color.FromArgb(231, 235, 240));
                e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                e.Handled = false; // allow default text draw for normal cells
            }

            // Header bottom line
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);
                using var pen = new Pen(Color.FromArgb(231, 235, 240));
                e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                e.Handled = true;
            }
        }
    }

    #region Models
    public record ContractRow(
        string Code,
        string Customer,
        string Phone,
        string EventName,
        string Hall,
        DateTime? Date,
        int Guests,
        decimal Total,
        decimal Paid,
        string Status);
    #endregion

    #region Currency cell
    public class Currency
    {
        public decimal Value { get; }
        public bool Success { get; }
        public bool Danger { get; }
        public Currency(decimal v, bool success = false, bool danger = false)
        { Value = v; Success = success; Danger = danger; }
        public override string ToString() => Value.ToString("#,0 đ");
    }
    [SupportedOSPlatform("windows")]
    public class DataGridViewCurrencyColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewCurrencyColumn()
        {
            CellTemplate = new DataGridViewCurrencyCell();
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                ForeColor = Color.FromArgb(30, 41, 59)
            };
        }
    }
    [SupportedOSPlatform("windows")]
    public class DataGridViewCurrencyCell : DataGridViewTextBoxCell
    {
        public override Type ValueType => typeof(Currency);
        public override Type FormattedValueType => typeof(string);
        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            var cur = value as Currency;
            string text = cur?.ToString() ?? Convert.ToString(value);
            var color = cellStyle.ForeColor;

            if (cur != null)
            {
                if (cur.Success) color = Color.FromArgb(16, 185, 129);  // green
                if (cur.Danger && cur.Value > 0) color = Color.FromArgb(239, 68, 68); // red
            }

            // background
            using var bg = new SolidBrush(cellState.HasFlag(DataGridViewElementStates.Selected) ? Color.FromArgb(239, 246, 255) : Color.White);
            g.FillRectangle(bg, cellBounds);

            // text
            TextRenderer.DrawText(g, text, cellStyle.Font,
                new Rectangle(cellBounds.X + 6, cellBounds.Y + (cellBounds.Height - cellStyle.Font.Height) / 2, cellBounds.Width - 12, cellBounds.Height),
                color, TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
    #endregion

    #region Icon+Text cell (file/lock/people + text + subtext)
    public class IconText
    {
        public string Icon { get; } // "file","lock","people"
        public string Text { get; }
        public string SubText { get; } // phone
        public IconText(string icon, string text, string sub = null) { Icon = icon; Text = text; SubText = sub; }
    }
    [SupportedOSPlatform("windows")]
    public class DataGridViewIconTextColumn : DataGridViewColumn
    {
        public DataGridViewIconTextColumn() : base(new DataGridViewIconTextCell())
        {
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft };
        }
    }
    [SupportedOSPlatform("windows")]
    public class DataGridViewIconTextCell : DataGridViewCell
    {
        public override Type ValueType => typeof(IconText);
        public override Type FormattedValueType => typeof(string);

        protected override object GetFormattedValue(object value,
            int rowIndex, ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
        {
            // Trả chuỗi chính để DataGridView không coi là null
            if (value is IconText it) return it.Text ?? string.Empty;
            return value?.ToString() ?? string.Empty;
        }
        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle style, DataGridViewAdvancedBorderStyle adv, DataGridViewPaintParts parts)
        {
            using var bg = new SolidBrush(cellState.HasFlag(DataGridViewElementStates.Selected) ? Color.FromArgb(239, 246, 255) : Color.White);
            g.FillRectangle(bg, cellBounds);

            var it = value as IconText;
            string main = it?.Text ?? Convert.ToString(value);
            string sub = it?.SubText;

            int x = cellBounds.X + 6;
            int y = cellBounds.Y + 6;

            // draw icon glyph (simple vector)
            if (it != null)
            {
                var ic = new Rectangle(x, cellBounds.Y + (cellBounds.Height - 18) / 2, 18, 18);
                var color = Color.FromArgb(100, 116, 139);
                DrawGlyph(g, ic, it.Icon, color);
                x = ic.Right + 8;
            }

            // main
            TextRenderer.DrawText(g, main, style.Font, new Point(x, cellBounds.Y + 6),
                Color.FromArgb(30, 41, 59), TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);

            // sub (phone)
            if (!string.IsNullOrEmpty(sub))
            {
                var small = new Font(style.Font, FontStyle.Regular);
                TextRenderer.DrawText(g, " " + sub, small,
                    new Point(x, cellBounds.Y + 22),
                    Color.FromArgb(100, 116, 139),
                    TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
            }
        }

        private void DrawGlyph(Graphics g, Rectangle r, string kind, Color c)
        {
            using var p = new Pen(c, 1.8f) { LineJoin = LineJoin.Round };
            using var b = new SolidBrush(Color.Transparent);
            switch (kind)
            {
                case "file":
                    using (var gp = new GraphicsPath())
                    {
                        gp.AddRectangle(new Rectangle(r.X + 3, r.Y + 2, r.Width - 6, r.Height - 4));
                        g.DrawPath(p, gp);
                        g.DrawLine(p, r.X + 5, r.Y + 7, r.Right - 5, r.Y + 7);
                    }
                    break;
                case "lock":
                    g.DrawRectangle(p, new Rectangle(r.X + 4, r.Y + 8, r.Width - 8, r.Height - 10));
                    g.DrawArc(p, r.X + 5, r.Y + 1, r.Width - 10, r.Height - 10, 200, 140);
                    break;
                case "people":
                    g.DrawEllipse(p, new Rectangle(r.X + 5, r.Y + 2, 8, 8));
                    g.DrawArc(p, r.X + 2, r.Y + 9, r.Width - 4, r.Height - 6, 20, 140);
                    break;
            }
        }
    }
    #endregion

    #region Badge cell
    public class Badge
    {
        public string Text { get; }
        public Badge(string t) { Text = t; }
    }
    [SupportedOSPlatform("windows")]
    public class DataGridViewBadgeColumn : DataGridViewColumn
    {
        public DataGridViewBadgeColumn() : base(new DataGridViewBadgeCell()) { }
    }
    [SupportedOSPlatform("windows")]
    public class DataGridViewBadgeCell : DataGridViewCell
    {
        public override Type ValueType => typeof(Badge);
        public override Type FormattedValueType => typeof(string);
        protected override object GetFormattedValue(object value,
            int rowIndex, ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
        {
            return (value as Badge)?.Text ?? value?.ToString() ?? string.Empty;
        }
        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle style, DataGridViewAdvancedBorderStyle adv, DataGridViewPaintParts parts)
        {
            using var bg = new SolidBrush(cellState.HasFlag(DataGridViewElementStates.Selected) ? Color.FromArgb(239, 246, 255) : Color.White);
            g.FillRectangle(bg, cellBounds);

            var badge = value as Badge;
            string text = badge?.Text ?? Convert.ToString(value) ?? "";

            Color back, border, fore;
            switch (text)
            {
                case "Đã ký": back = Color.FromArgb(220, 252, 231); border = Color.FromArgb(134, 239, 172); fore = Color.FromArgb(22, 163, 74); break;
                case "Chờ ký": back = Color.FromArgb(254, 249, 195); border = Color.FromArgb(253, 224, 71); fore = Color.FromArgb(180, 83, 9); break;
                case "Hoàn thành": back = Color.FromArgb(219, 234, 254); border = Color.FromArgb(147, 197, 253); fore = Color.FromArgb(37, 99, 235); break;
                default: back = Color.FromArgb(229, 231, 235); border = Color.FromArgb(209, 213, 219); fore = Color.FromArgb(75, 85, 99); break; // Nháp/khác
            }

            var sz = TextRenderer.MeasureText(text, style.Font);
            var w = Math.Min(cellBounds.Width - 12, sz.Width + 18);
            var h = Math.Min(cellBounds.Height - 12, sz.Height + 8);
            var rect = new Rectangle(cellBounds.X + 6, cellBounds.Y + (cellBounds.Height - h) / 2, w, h);

            using var gp = Rounded(rect, 12);
            using var bb = new SolidBrush(back);
            using var pb = new Pen(border, 1.5f);
            g.FillPath(bb, gp);
            g.DrawPath(pb, gp);

            TextRenderer.DrawText(g, text, style.Font, rect, fore,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = radius * 2;
            var gp = new GraphicsPath();
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }
    #endregion

    #region Actions column (eye / download / message)
    public class ActionIcons
    {
        public bool View { get; }
        public bool Download { get; }
        public bool Note { get; }
        public ActionIcons(bool v, bool d, bool n) { View = v; Download = d; Note = n; }
    }
    [SupportedOSPlatform("windows")]
    public class DataGridViewActionColumn : DataGridViewColumn
    {
        public DataGridViewActionColumn() : base(new DataGridViewActionCell()) { }
    }
    [SupportedOSPlatform("windows")]
    public class DataGridViewActionCell : DataGridViewCell
    {
        public override Type ValueType => typeof(ActionIcons);
        public override Type FormattedValueType => typeof(string);

        protected override object GetFormattedValue(object value,
            int rowIndex, ref DataGridViewCellStyle cellStyle,
            TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter,
            DataGridViewDataErrorContexts context)
        {
            // Không hiển thị văn bản, trả rỗng để hợp lệ
            return string.Empty;
        }
        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle style, DataGridViewAdvancedBorderStyle adv, DataGridViewPaintParts parts)
        {
            using var bg = new SolidBrush(cellState.HasFlag(DataGridViewElementStates.Selected) ? Color.FromArgb(239, 246, 255) : Color.White);
            g.FillRectangle(bg, cellBounds);

            var act = value as ActionIcons ?? new ActionIcons(true, true, true);
            int cx = cellBounds.X + 8;
            int cy = cellBounds.Y + (cellBounds.Height - 18) / 2;
            int gap = 28;

            if (act.View) DrawEye(g, new Rectangle(cx, cy, 18, 18), Color.FromArgb(30, 41, 59));
            if (act.Download) DrawDownload(g, new Rectangle(cx + gap, cy, 18, 18), Color.FromArgb(30, 41, 59));
            if (act.Note) DrawMessage(g, new Rectangle(cx + gap * 2, cy, 18, 18), Color.FromArgb(30, 41, 59));
        }

        private void DrawEye(Graphics g, Rectangle r, Color c)
        {
            using var p = new Pen(c, 1.8f) { LineJoin = LineJoin.Round };
            g.DrawArc(p, r.X + 1, r.Y + 5, r.Width - 2, r.Height - 10, 0, 180);
            g.DrawArc(p, r.X + 1, r.Y + 5, r.Width - 2, r.Height - 10, 180, 180);
            g.DrawEllipse(p, new Rectangle(r.X + 7, r.Y + 7, 4, 4));
        }
        private void DrawDownload(Graphics g, Rectangle r, Color c)
        {
            using var p = new Pen(c, 1.8f) { LineJoin = LineJoin.Round, EndCap = LineCap.Round, StartCap = LineCap.Round };
            g.DrawLine(p, r.X + r.Width / 2, r.Y + 3, r.X + r.Width / 2, r.Bottom - 6);
            g.DrawLine(p, r.X + 5, r.Bottom - 6, r.Right - 5, r.Bottom - 6);
            g.DrawLines(p, new[] { new Point(r.X + r.Width / 2, r.Bottom - 6), new Point(r.X + r.Width / 2 - 4, r.Bottom - 10), new Point(r.X + r.Width / 2 + 4, r.Bottom - 10), new Point(r.X + r.Width / 2, r.Bottom - 6) });
        }
        private void DrawMessage(Graphics g, Rectangle r, Color c)
        {
            using var p = new Pen(c, 1.8f) { LineJoin = LineJoin.Round };
            g.DrawRectangle(p, new Rectangle(r.X + 2, r.Y + 4, r.Width - 4, r.Height - 8));
            g.DrawLine(p, r.X + 4, r.Y + 9, r.Right - 4, r.Y + 9);
            g.DrawLine(p, r.X + 4, r.Y + 12, r.Right - 12, r.Y + 12);
        }
    }
    #endregion
}
