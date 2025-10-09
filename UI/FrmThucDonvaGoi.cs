using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiControls;

namespace UI
{
    public partial class FrmThucDonvaGoi : Form
    {
        public FrmThucDonvaGoi()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void menuGrid1_Load(object sender, EventArgs e)
        {

        }
        private const string COL_TEN = "TenMon";
        private const string COL_DM = "DanhMuc";
        private const string COL_GB = "GiaBan";
        private const string COL_GV = "GiaVon";
        private const string COL_LN = "LoiNhuan";
        private const string COL_TT = "TrangThai";
        private const string COL_TTAC = "ThaoTac";
        private void FrmThucDonvaGoi_Load(object sender, EventArgs e)
        {

            LoadDataThucDonVaGoi();

        }
        private void LoadDataThucDonVaGoi()
        {
            var dgv = dgvThucDonVaGoi;
            dgv.Rows.Clear();

            AddTDRow("Gỏi cuốn tôm thịt", "Khai vị", 45000m, 25000m, true);
            AddTDRow("Salad hải sản", "Khai vị", 85000m, 45000m, true);
            AddTDRow("Bò nướng lá lốt", "Món chính", 120000m, 60000m, true);
            AddTDRow("Gà quay bơ tỏi", "Món chính", 150000m, 70000m, true);
            AddTDRow("Cá hấp xì dầu", "Hải sản", 280000m, 150000m, true);
            AddTDRow("Tôm hùm nướng phô mai", "Hải sản", 850000m, 450000m, false);
        }
        private void AddTDRow(string ten, string dm, decimal gb, decimal gv, bool conHang)
        {
            decimal ln = gb - gv;
            string loiNhuanCell = $"{Money(ln)}\n({ProfitPercent(gb, gv)})";
            int row = dgvThucDonVaGoi.Rows.Add(
                ten, dm, Money(gb), Money(gv), loiNhuanCell,
                conHang ? "Còn hàng" : "Hết hàng", "edit|delete"
            );
        }
        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");
        private static string ProfitPercent(decimal gb, decimal gv)
        {
            if (gb <= 0) return "0%";
            var p = (gb - gv) / gb * 100m;
            return Math.Round(p, 1).ToString("0.0") + "%";
        }

        private void dgvThucDonVaGoi_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var col = dgv.Columns[e.ColumnIndex].Name;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Lợi nhuận: 2 dòng
            if (col == COL_LN)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);
                var parts = (e.FormattedValue?.ToString() ?? "").Split('\n');
                string top = parts.ElementAtOrDefault(0) ?? "";
                string sub = parts.ElementAtOrDefault(1) ?? "";

                var rect = Rectangle.Inflate(e.CellBounds, -8, -4);
                using var fTop = new Font(e.CellStyle.Font, FontStyle.Bold);
                using var fSub = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size - 1f);
                using var brTop = new SolidBrush(e.CellStyle.ForeColor);
                using var brSub = new SolidBrush(Color.FromArgb(110, 119, 135));

                g.DrawString(top, fTop, brTop, new RectangleF(rect.X, rect.Y + 2, rect.Width, rect.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                g.DrawString(sub, fSub, brSub, new RectangleF(rect.X, rect.Y + rect.Height / 2f - 2, rect.Width, rect.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Trạng thái: chip
            if (col == COL_TT)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                bool conHang = string.Equals(Convert.ToString(e.FormattedValue), "Còn hàng", StringComparison.OrdinalIgnoreCase);
                string text = conHang ? "Còn hàng" : "Hết hàng";

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 26) / 2, 86, 26);
                using var path = Rounded(chip, 13);
                using var fill = new SolidBrush(conHang ? Color.FromArgb(208, 247, 225) : Color.FromArgb(255, 216, 222));
                using var br = new SolidBrush(conHang ? Color.FromArgb(16, 128, 67) : Color.FromArgb(176, 16, 48));

                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Thao tác: hai icon như button (bút & thùng rác)
            if (col == COL_TTAC)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                var r = Rectangle.Inflate(e.CellBounds, -8, -8);
                // hình tròn
                var btnEdit = new Rectangle(r.Right - 88, r.Y + (r.Height - 30) / 2, 30, 30);
                var btnDel = new Rectangle(r.Right - 48, r.Y + (r.Height - 30) / 2, 30, 30);

                DrawCircleButton(g, btnEdit, Color.White, Color.FromArgb(223, 229, 241));
                DrawEditIcon(g, new Rectangle(btnEdit.X + 6, btnEdit.Y + 6, 18, 18), Color.FromArgb(23, 23, 23));

                DrawCircleButton(g, btnDel, Color.White, Color.FromArgb(223, 229, 241));
                DrawTrashIcon(g, new Rectangle(btnDel.X + 6, btnDel.Y + 6, 18, 18), Color.FromArgb(220, 38, 38));

                // Lưu bounds vào Tag để hit-test (tránh tính lại nhiều lần)
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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.FillEllipse(sb, r); g.DrawEllipse(pen, r);
        }
        private void DrawEditIcon(Graphics g, Rectangle r, Color c)
        {
            using var pen = new Pen(c, 1.7f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            g.DrawRectangle(pen, r.Left + 2, r.Top + 2, r.Width - 4, r.Height - 4);
            g.DrawLine(pen, r.Left + 4, r.Bottom - 4, r.Right - 4, r.Top + 4);
        }
        private void DrawTrashIcon(Graphics g, Rectangle r, Color c)
        {
            using var pen = new Pen(c, 1.7f) { LineJoin = System.Drawing.Drawing2D.LineJoin.Round };
            g.DrawLine(pen, r.Left + 3, r.Top + 5, r.Right - 3, r.Top + 5);
            g.DrawRectangle(pen, r.Left + 4, r.Top + 6, r.Width - 8, r.Height - 10);
            g.DrawLine(pen, r.Left + r.Width / 2 - 3, r.Top + 8, r.Left + r.Width / 2 - 3, r.Bottom - 5);
            g.DrawLine(pen, r.Left + r.Width / 2 + 3, r.Top + 8, r.Left + r.Width / 2 + 3, r.Bottom - 5);
        }

        private void dgvThucDonVaGoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvThucDonVaGoi.Columns[e.ColumnIndex].Name != COL_TTAC) return;

            var cell = dgvThucDonVaGoi.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Tag is Tuple<Rectangle, Rectangle> boxes)
            {
                var pt = dgvThucDonVaGoi.PointToClient(Cursor.Position);
                var cellRect = dgvThucDonVaGoi.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                var localPt = new Point(pt.X - cellRect.X, pt.Y - cellRect.Y);

                if (boxes.Item1.Contains(localPt))
                {
                    string ten = dgvThucDonVaGoi.Rows[e.RowIndex].Cells[COL_TEN].Value?.ToString();
                    MessageBox.Show($"Sửa món: {ten}");
                }
                else if (boxes.Item2.Contains(localPt))
                {
                    string ten = dgvThucDonVaGoi.Rows[e.RowIndex].Cells[COL_TEN].Value?.ToString();
                    if (MessageBox.Show($"Xoá món \"{ten}\"?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        dgvThucDonVaGoi.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void dgvThucDonVaGoi_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = dgvThucDonVaGoi.HitTest(e.X, e.Y);
            if (hit.RowIndex >= 0 && hit.ColumnIndex >= 0 &&
                dgvThucDonVaGoi.Columns[hit.ColumnIndex].Name == COL_TTAC)
            {
                var cell = dgvThucDonVaGoi.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                if (cell.Tag is Tuple<Rectangle, Rectangle> boxes)
                {
                    var cellRect = dgvThucDonVaGoi.GetCellDisplayRectangle(hit.ColumnIndex, hit.RowIndex, true);
                    var local = new Point(e.X - cellRect.X, e.Y - cellRect.Y);
                    dgvThucDonVaGoi.Cursor = (boxes.Item1.Contains(local) || boxes.Item2.Contains(local))
                        ? Cursors.Hand : Cursors.Default;
                    return;
                }
            }
            dgvThucDonVaGoi.Cursor = Cursors.Default;
        }
    }
}
