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
    public partial class MenuGrid : UserControl
    {
        private DataGridView dgv;

        public event EventHandler<int> EditClicked;   // rowIndex
        public event EventHandler<int> DeleteClicked; // rowIndex

        public MenuGrid()
        {
         
            Build();
        }

        private void Build()
        {
            dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            DoubleBuffered(dgv, true);

            // Header style
            dgv.ColumnHeadersHeight = 42;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(23, 23, 23),
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 12, 0)
            };
            // Row style
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(38, 38, 38),
                Font = new Font("Segoe UI", 10f),
                SelectionBackColor = Color.FromArgb(243, 246, 255),
                SelectionForeColor = Color.FromArgb(38, 38, 38),
                Padding = new Padding(12, 8, 12, 8)
            };
            dgv.GridColor = Color.FromArgb(234, 238, 243);

            // Columns
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenMon",
                HeaderText = "Tên món",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 180
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DanhMuc",
                HeaderText = "Danh mục",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaBan",
                HeaderText = "Giá bán",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaVon",
                HeaderText = "Giá vốn",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LoiNhuan",
                HeaderText = "Lợi nhuận",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThaoTac",
                HeaderText = "Thao tác",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            // paint custom cells
            dgv.CellPainting += Dgv_CellPainting;
            dgv.CellClick += Dgv_CellClick;
            dgv.RowPrePaint += (s, e) =>
            {
                // zebra
                if (e.RowIndex % 2 == 1)
                    dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(251, 252, 255);
            };

            Controls.Add(dgv);
            Padding = new Padding(8);
            BackColor = Color.White;

            // bo góc container
            this.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = ClientRectangle;
                rect.Inflate(-1, -1);
                using var path = Rounded(rect, 14);
                using var pen = new Pen(Color.FromArgb(229, 234, 242));
                using var bg = new SolidBrush(Color.White);
                g.FillPath(bg, path);
                g.DrawPath(pen, path);
            };
        }

        // Public API: nạp dữ liệu
        public void SetData(IEnumerable<MenuRow> rows)
        {
            dgv.Rows.Clear();
            foreach (var r in rows)
            {
                int idx = dgv.Rows.Add(
                    r.TenMon,
                    r.DanhMuc,
                    FormatMoney(r.GiaBan),
                    FormatMoney(r.GiaVon),
                    $"{FormatMoney(r.GiaBan - r.GiaVon)}\n({Percent(r.GiaBan, r.GiaVon)})",
                    r.ConHang ? "Còn hàng" : "Hết hàng",
                    "edit|delete");
                dgv.Rows[idx].Tag = r; // attach model
            }
        }

        // ==== Rendering helpers ====
        private void Dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // standard background & grid
            e.Handled = true;
            using var back = new SolidBrush(e.CellStyle.BackColor);
            g.FillRectangle(back, e.CellBounds);
            using var grid = new Pen(dgv.GridColor);
            g.DrawLine(grid, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);

            // padding
            var content = Rectangle.Inflate(e.CellBounds, -e.CellStyle.Padding.Left, -e.CellStyle.Padding.Top);
            content.Width -= e.CellStyle.Padding.Right - e.CellStyle.Padding.Left;
            content.Height -= e.CellStyle.Padding.Bottom - e.CellStyle.Padding.Top;

            // draw each column custom
            var text = e.FormattedValue?.ToString() ?? "";

            if (dgv.Columns[e.ColumnIndex].Name == "LoiNhuan")
            {
                // 2 dòng: số đậm & % nhỏ xám
                var parts = text.Split('\n');
                string line1 = parts.Length > 0 ? parts[0] : "";
                string line2 = parts.Length > 1 ? parts[1] : "";

                using var f1 = new Font(e.CellStyle.Font, FontStyle.Bold);
                using var f2 = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size - 1f);
                using var br1 = new SolidBrush(e.CellStyle.ForeColor);
                using var br2 = new SolidBrush(Color.FromArgb(110, 119, 135));
                g.DrawString(line1, f1, br1, new RectangleF(content.X, content.Y + 2, content.Width, content.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                g.DrawString(line2, f2, br2, new RectangleF(content.X, content.Y + content.Height / 2f - 2, content.Width, content.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "TrangThai")
            {
                bool conHang = text == "Còn hàng";
                var chipRect = new Rectangle(content.X, content.Y + (content.Height - 26) / 2, 90, 26);
                var fill = conHang ? Color.FromArgb(208, 247, 225) : Color.FromArgb(255, 228, 232);
                var fore = conHang ? Color.FromArgb(16, 128, 67) : Color.FromArgb(176, 16, 48);
                using var path = Rounded(chipRect, 13);
                using var sb = new SolidBrush(fill);
                using var br = new SolidBrush(fore);
                g.FillPath(sb, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chipRect,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "ThaoTac")
            {
                // vẽ icon bút & thùng rác
                var r = content;
                var editRect = new Rectangle(r.Right - 56, r.Y + (r.Height - 22) / 2, 22, 22);
                var delRect = new Rectangle(r.Right - 26, r.Y + (r.Height - 22) / 2, 22, 22);
                DrawEditIcon(g, editRect, Color.FromArgb(23, 23, 23));
                DrawTrashIcon(g, delRect, Color.FromArgb(23, 23, 23));
                // gợi ý hover: nếu đang chọn thì tô nền nhẹ
                if (dgv.CurrentCell != null && dgv.CurrentCell.RowIndex == e.RowIndex &&
                    dgv.CurrentCell.ColumnIndex == e.ColumnIndex)
                {
                    using var sb = new SolidBrush(Color.FromArgb(15, 31, 111, 235));
                    g.FillRectangle(sb, e.CellBounds);
                }
            }
            else
            {
                // text thường
                using var br = new SolidBrush(e.CellStyle.ForeColor);
                g.DrawString(text, e.CellStyle.Font, br, content,
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgv.Columns[e.ColumnIndex].Name != "ThaoTac") return;

            var cellRect = dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            var r = Rectangle.Inflate(cellRect, -dgv.DefaultCellStyle.Padding.Left, -dgv.DefaultCellStyle.Padding.Top);
            var editRect = new Rectangle(r.Right - 56, r.Y + (r.Height - 22) / 2, 22, 22);
            var delRect = new Rectangle(r.Right - 26, r.Y + (r.Height - 22) / 2, 22, 22);

            var mouse = dgv.PointToClient(Cursor.Position);
            if (editRect.Contains(mouse)) { EditClicked?.Invoke(this, e.RowIndex); return; }
            if (delRect.Contains(mouse)) { DeleteClicked?.Invoke(this, e.RowIndex); return; }
        }

        // ==== Models & utils ====
        public class MenuRow
        {
            public string TenMon { get; set; }
            public string DanhMuc { get; set; }
            public decimal GiaBan { get; set; }
            public decimal GiaVon { get; set; }
            public bool ConHang { get; set; }
        }

        private static string FormatMoney(decimal v)
        {
            // 120000 -> 120.000 đ
            return string.Format("{0:#,0} đ", v).Replace(",", ".");
        }
        private static string Percent(decimal giaBan, decimal giaVon)
        {
            if (giaBan <= 0) return "0%";
            var p = (giaBan - giaVon) / giaBan * 100m;
            return $"{Math.Round(p, 1)}%";
        }

        private static void DoubleBuffered(DataGridView dgv, bool setting)
        {
            var pi = typeof(DataGridView).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi?.SetValue(dgv, setting, null);
        }
        private static GraphicsPath Rounded(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void DrawEditIcon(Graphics g, Rectangle r, Color c)
        {
            using var pen = new Pen(c, 1.7f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
            // cây bút đơn giản
            g.DrawLine(pen, r.Left + 4, r.Bottom - 6, r.Right - 6, r.Top + 6);
            g.DrawLine(pen, r.Left + 6, r.Bottom - 6, r.Left + 4, r.Bottom - 4);
            g.DrawLine(pen, r.Right - 6, r.Top + 6, r.Right - 4, r.Top + 8);
            g.DrawRectangle(pen, r.Left + 3, r.Top + 3, r.Width - 6, r.Height - 6);
        }

        private static void DrawTrashIcon(Graphics g, Rectangle r, Color c)
        {
            using var pen = new Pen(c, 1.7f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
            // nắp
            g.DrawLine(pen, r.Left + 6, r.Top + 8, r.Right - 6, r.Top + 8);
            g.DrawLine(pen, r.Left + 10, r.Top + 6, r.Right - 10, r.Top + 6);
            // thân
            g.DrawRectangle(pen, r.Left + 6, r.Top + 9, r.Width - 12, r.Height - 14);
            // 2 vạch
            g.DrawLine(pen, r.Left + r.Width / 2 - 4, r.Top + 12, r.Left + r.Width / 2 - 4, r.Bottom - 7);
            g.DrawLine(pen, r.Left + r.Width / 2 + 4, r.Top + 12, r.Left + r.Width / 2 + 4, r.Bottom - 7);
        }
    }
}
