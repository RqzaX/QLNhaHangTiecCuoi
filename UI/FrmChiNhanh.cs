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

            LoadDataChiNhanh();


        }
        private void LoadDataChiNhanh()
        {
            dgvChiNhanh.AutoGenerateColumns = false;
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

            // Chip trạng thái
            if (col == COL_TT)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                bool active = string.Equals(Convert.ToString(e.FormattedValue), "Hoạt động", StringComparison.OrdinalIgnoreCase);
                string text = active ? "Hoạt động" : "Ngừng";
                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2, 100, 28);

                using var path = Rounded(chip, 14);
                using var fill = new SolidBrush(active ? Color.FromArgb(208, 247, 225) : Color.FromArgb(255, 216, 222));
                using var br = new SolidBrush(active ? Color.FromArgb(16, 128, 67) : Color.FromArgb(176, 16, 48));

                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Hai icon thao tác (Sửa / Xoá)
            if (col == COL_TTAC)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                var r = Rectangle.Inflate(e.CellBounds, -8, -8);
                var btnEdit = new Rectangle(r.Right - 88, r.Y + (r.Height - 30) / 2, 30, 30);
                var btnDel = new Rectangle(r.Right - 48, r.Y + (r.Height - 30) / 2, 30, 30);

                DrawCircleButton(g, btnEdit, Color.White, Color.FromArgb(223, 229, 241));
                DrawPencil(g, new Rectangle(btnEdit.X + 6, btnEdit.Y + 6, 18, 18), Color.FromArgb(23, 23, 23));

                DrawCircleButton(g, btnDel, Color.White, Color.FromArgb(223, 229, 241));
                DrawTrash(g, new Rectangle(btnDel.X + 6, btnDel.Y + 6, 18, 18), Color.FromArgb(220, 38, 38));

                // lưu để hit-test khi click
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = new Tuple<Rectangle, Rectangle>(btnEdit, btnDel);

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
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
        private void DrawCircleButton(Graphics g, Rectangle r, Color fill, Color border)
        {
            using var sb = new SolidBrush(fill);
            using var pen = new Pen(border);
            g.FillEllipse(sb, r); g.DrawEllipse(pen, r);
        }
        private void DrawPencil(Graphics g, Rectangle r, Color c)
        {
            using var pen = new Pen(c, 1.7f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            g.DrawLine(pen, r.Left + 2, r.Bottom - 4, r.Right - 6, r.Top + 4);
            g.DrawRectangle(pen, r.Left + 4, r.Top + 4, r.Width - 8, r.Height - 8);
        }
        private void DrawTrash(Graphics g, Rectangle r, Color c)
        {
            using var pen = new Pen(c, 1.7f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            g.DrawLine(pen, r.Left + 3, r.Top + 5, r.Right - 3, r.Top + 5);
            g.DrawRectangle(pen, r.Left + 4, r.Top + 6, r.Width - 8, r.Height - 10);
            g.DrawLine(pen, r.Left + r.Width / 2 - 3, r.Top + 8, r.Left + r.Width / 2 - 3, r.Bottom - 5);
            g.DrawLine(pen, r.Left + r.Width / 2 + 3, r.Top + 8, r.Left + r.Width / 2 + 3, r.Bottom - 5);
        }

        private void dgvChiNhanh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvChiNhanh.Columns[e.ColumnIndex].Name != COL_TTAC) return;

            var cell = dgvChiNhanh.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Tag is Tuple<Rectangle, Rectangle> boxes)
            {
                var pt = dgvChiNhanh.PointToClient(Cursor.Position);
                var cellRect = dgvChiNhanh.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                var local = new Point(pt.X - cellRect.X, pt.Y - cellRect.Y);

                string ten = dgvChiNhanh.Rows[e.RowIndex].Cells[COL_TEN].Value?.ToString();

                if (boxes.Item1.Contains(local))
                {
                    MessageBox.Show($"Sửa chi nhánh: {ten}");
                }
                else if (boxes.Item2.Contains(local))
                {
                    if (MessageBox.Show($"Xoá \"{ten}\"?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                if (cell.Tag is Tuple<Rectangle, Rectangle> boxes)
                {
                    var cellRect = dgvChiNhanh.GetCellDisplayRectangle(hit.ColumnIndex, hit.RowIndex, true);
                    var local = new Point(e.X - cellRect.X, e.Y - cellRect.Y);
                    dgvChiNhanh.Cursor = (boxes.Item1.Contains(local) || boxes.Item2.Contains(local))
                        ? Cursors.Hand : Cursors.Default;
                    return;
                }
            }
            dgvChiNhanh.Cursor = Cursors.Default;
        }
    }
}
