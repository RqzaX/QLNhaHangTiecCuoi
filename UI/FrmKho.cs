using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FrmKho : Form
    {
        public FrmKho()
        {
            InitializeComponent();
        }
        private const string K_TEN = "TenNL";
        private const string K_DONVI = "DonVi";
        private const string K_TON = "TonKho";
        private const string K_MIN = "TonToiThieu";
        private const string K_TB = "DungTB";
        private const string K_GIA = "GiaTri";
        private const string K_TT = "TrangThai";
        private const string K_TTAC = "ThaoTac";

        private void roundedButton1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FrmKho_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            dgvKho.Rows.Clear();

            AddRow("Thịt bò Úc", "kg", 45, 20, 15, 13500000m);
            AddRow("Tôm sú", "kg", 12, 15, 8, 7200000m);
            AddRow("Cá hồi Na Uy", "kg", 8, 10, 6, 6400000m);
            AddRow("Rau xà lách", "kg", 25, 10, 12, 500000m);
            AddRow("Bia Tiger", "thùng", 45, 30, 20, 13500000m);
            AddRow("Coca Cola", "thùng", 18, 20, 15, 3600000m);
        }
        private void AddRow(string ten, string donvi, int ton, int toiThieu, int dungTBNgay, decimal giaTri)
        {
            bool duHang = ton >= toiThieu;
            dgvKho.Rows.Add(ten, donvi, ton, toiThieu, dungTBNgay, Money(giaTri),
                            duHang ? "Đủ hàng" : "Sắp hết", "Chi tiết");
        }

        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");

        private void dgvKho_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvKho.Columns[e.ColumnIndex].Name != K_TT) return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            string text = Convert.ToString(e.FormattedValue) ?? "";
            bool duHang = text.Equals("Đủ hàng", StringComparison.OrdinalIgnoreCase);

            var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2, 90, 28);
            using var path = Rounded(chip, 14);
            using var fill = new SolidBrush(duHang ? Color.FromArgb(208, 247, 225) : Color.FromArgb(255, 239, 185));
            using var br = new SolidBrush(duHang ? Color.FromArgb(16, 128, 67) : Color.FromArgb(159, 108, 0));

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
        }
        private static System.Drawing.Drawing2D.GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure(); return p;
        }

        private void dgvKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvKho.Columns[e.ColumnIndex].Name != K_TTAC) return;

            string ten = dgvKho.Rows[e.RowIndex].Cells[K_TEN].Value?.ToString();
            string ton = dgvKho.Rows[e.RowIndex].Cells[K_TON].Value?.ToString();
            string donvi = dgvKho.Rows[e.RowIndex].Cells[K_DONVI].Value?.ToString();

            MessageBox.Show($"Chi tiết nguyên liệu:\n- Tên: {ten}\n- Tồn kho: {ton} {donvi}",
                            "Kho", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
