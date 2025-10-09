using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmChiNhanh : Form
    {
        public FrmChiNhanh()
        {
            InitializeComponent();
        }
        private const string COL_TEN = "TenCN";
        private const string COL_DC = "DiaChi";
        private const string COL_DT = "DienThoai";
        private const string COL_TT = "TrangThai";
        private const string COL_TTAC = "ThaoTac";

        private void FrmChiNhanh_Load(object sender, EventArgs e)
        {
            InitDgvChiNhanh();
            LoadDataChiNhanh();
           
            

        }
        private void InitDgvChiNhanh()
        {
            var dgv = dgvChiNhanh;

            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = COL_TEN, HeaderText = "Tên chi nhánh", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 200 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = COL_DC, HeaderText = "Địa chỉ", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 320 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = COL_DT, HeaderText = "Số điện thoại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = COL_TT, HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            // cột thao tác để vẽ custom
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = COL_TTAC, HeaderText = "Thao tác", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ReadOnly = true, SortMode = DataGridViewColumnSortMode.NotSortable });

            // style giống mockup
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgv.DefaultCellStyle.Padding = new Padding(14, 10, 14, 10);
            dgv.RowTemplate.Height = 60;

            // giảm giật khi vẽ
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgv, new object[] { true });
        }
        private void LoadDataChiNhanh()
        {
            dgvChiNhanh.Rows.Clear();
            AddBranch("Chi nhánh Quận 1", "123 Nguyễn Huệ, Q1, TP.HCM", "028 3821 1234", true);
            AddBranch("Chi nhánh Quận 3", "456 Võ Văn Tần, Q3, TP.HCM", "028 3930 2345", true);
            AddBranch("Chi nhánh Thủ Đức", "789 Võ Văn Ngân, Thủ Đức, TP.HCM", "028 3897 3456", true);
        }

        private void AddBranch(string ten, string diaChi, string soDt, bool hoatDong)
        {
            int rowIndex = dgvChiNhanh.Rows.Add(ten, diaChi, soDt, hoatDong ? "Hoạt động" : "Ngừng", null);
            dgvChiNhanh.Rows[rowIndex].Cells["ThaoTac"].Value = "edit|delete";
        }

        private void dgvChiNhanh_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            string col = dgv.Columns[e.ColumnIndex].Name;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // CHIP TRẠNG THÁI
            if (col == COL_TT)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                bool active = string.Equals(Convert.ToString(e.FormattedValue), "Hoạt động", StringComparison.OrdinalIgnoreCase);
                string text = active ? "Hoạt động" : "Ngừng";

                // chip 92x28 – màu xanh nhạt như hình
                var chip = new Rectangle(e.CellBounds.X + 10, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2, 98, 28);
                using var path = Rounded(chip, 14);
                using var fill = new SolidBrush(active ? Color.FromArgb(209, 250, 229) : Color.FromArgb(254, 226, 226));
                using var br = new SolidBrush(active ? Color.FromArgb(16, 128, 67) : Color.FromArgb(153, 27, 27));
                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // NÚT THAO TÁC (bút chì + thùng rác)
            if (col == COL_TTAC)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                // layout 2 nút tròn 32px, lệch phải
                int size = 32;
                int gap = 12;
                var rCell = Rectangle.Inflate(e.CellBounds, -10, -10);

                var btnEdit = new Rectangle(rCell.Right - (size * 2 + gap), rCell.Y + (rCell.Height - size) / 2, size, size);
                var btnDel = new Rectangle(rCell.Right - size, rCell.Y + (rCell.Height - size) / 2, size, size);

                DrawCircleButton(g, btnEdit, Color.White, Color.FromArgb(223, 229, 241));
                DrawPencil(g, new Rectangle(btnEdit.X + 7, btnEdit.Y + 7, size - 14, size - 14), Color.FromArgb(23, 23, 23));

                DrawCircleButton(g, btnDel, Color.White, Color.FromArgb(223, 229, 241));
                DrawTrash(g, new Rectangle(btnDel.X + 7, btnDel.Y + 7, size - 14, size - 14), Color.FromArgb(220, 38, 38));

                // lưu để hit-test khi click
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = (btnEdit, btnDel);

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }
        }
        private static System.Drawing.Drawing2D.GraphicsPath Rounded(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            p.AddArc(rect.X, rect.Y, d, d, 180, 90);
            p.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            p.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            p.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            p.CloseFigure(); return p;
        }

        // Vẽ nền nút tròn
        private void DrawCircleButton(Graphics g, Rectangle r, Color fill, Color border)
        {
            using var sb = new SolidBrush(fill);
            using var pen = new Pen(border);
            g.FillEllipse(sb, r);
            g.DrawEllipse(pen, r);
        }

        // Bút chì (đơn giản, nét 1.7)
        private void DrawPencil(Graphics g, Rectangle r, Color c)
        {
            using var pen = new Pen(c, 1.7f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round, StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round };
            // thân bút
            g.DrawLine(pen, r.Left + 2, r.Bottom - 4, r.Right - 4, r.Top + 4);
            // đầu bút (tam giác nhỏ)
            PointF p1 = new(r.Right - 4, r.Top + 4);
            PointF p2 = new(r.Right - 1, r.Top + 7);
            PointF p3 = new(r.Right - 7, r.Top + 1);
            using var sb = new SolidBrush(c);
            g.FillPolygon(sb, new[] { p1, p2, p3 });
            // nắp bút
            g.DrawLine(pen, r.Left + 4, r.Bottom - 2, r.Left + 7, r.Bottom + 1);
        }

        // Thùng rác
        private void DrawTrash(Graphics g, Rectangle r, Color c)
        {
            using var pen = new Pen(c, 1.7f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            // nắp
            g.DrawLine(pen, r.Left + 3, r.Top + 4, r.Right - 3, r.Top + 4);
            // thân
            g.DrawRectangle(pen, r.Left + 4, r.Top + 6, r.Width - 8, r.Height - 10);
            // 2 vạch
            g.DrawLine(pen, r.Left + r.Width / 2 - 4, r.Top + 8, r.Left + r.Width / 2 - 4, r.Bottom - 6);
            g.DrawLine(pen, r.Left + r.Width / 2 + 4, r.Top + 8, r.Left + r.Width / 2 + 4, r.Bottom - 6);
        }
        private void dgvChiNhanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvChiNhanh.Columns[e.ColumnIndex].Name != COL_TTAC) return;

            var cell = dgvChiNhanh.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Tag is ValueTuple<Rectangle, Rectangle> pair)
            {
                var (editBox, delBox) = pair;
                var cellRect = dgvChiNhanh.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                var local = new Point(Cursor.Position.X - dgvChiNhanh.PointToScreen(cellRect.Location).X,
                                      Cursor.Position.Y - dgvChiNhanh.PointToScreen(cellRect.Location).Y);

                string ten = dgvChiNhanh.Rows[e.RowIndex].Cells[COL_TEN].Value?.ToString();

                if (editBox.Contains(local))
                {
                    MessageBox.Show($"Sửa chi nhánh: {ten}");
                }
                else if (delBox.Contains(local))
                {
                    if (MessageBox.Show($"Xoá \"{ten}\"?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        dgvChiNhanh.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void dgvChiNhanh_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = dgvChiNhanh.HitTest(e.X, e.Y);
            if (hit.RowIndex >= 0 && hit.ColumnIndex >= 0 &&
                dgvChiNhanh.Columns[hit.ColumnIndex].Name == COL_TTAC)
            {
                var cell = dgvChiNhanh.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                if (cell.Tag is ValueTuple<Rectangle, Rectangle> pair)
                {
                    var (editBox, delBox) = pair;
                    var cellRect = dgvChiNhanh.GetCellDisplayRectangle(hit.ColumnIndex, hit.RowIndex, true);
                    var local = new Point(e.X - cellRect.X, e.Y - cellRect.Y);
                    dgvChiNhanh.Cursor = (editBox.Contains(local) || delBox.Contains(local)) ? Cursors.Hand : Cursors.Default;
                    return;
                }
            }
            dgvChiNhanh.Cursor = Cursors.Default;
        }
    }
}
