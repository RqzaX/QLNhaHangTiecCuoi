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
    public partial class FrmKho : Form
    {
        public FrmKho()
        {
            InitializeComponent();
        }
        private const string K_TEN = "TenNL";
        private const string K_DONVI = "DonVi";
        private const string K_TON = "TonKho";
        private const string K_TOITH = "ToiThieu";
        private const string K_TB = "DungTB";
        private const string K_GIA = "GiaTri";
        private const string K_TT = "TrangThai";
        private const string K_TAC = "ThaoTac";

        private void roundedButton1_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_NhapKho())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FrmKho_Load(object sender, EventArgs e)
        {
            InitDgvKho();
            LoadData();
        }
        private void InitDgvKho()
        {
            var dgv = dgvKho;

            dgv.DataSource = null;
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = K_TEN, HeaderText = "Tên nguyên liệu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 260 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = K_DONVI, HeaderText = "Đơn vị", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = K_TON, HeaderText = "Tồn kho", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = K_TOITH, HeaderText = "Tồn tối thiểu", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = K_TB, HeaderText = "Dùng TB/ngày", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = K_GIA, HeaderText = "Giá trị", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = K_TT, HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            dgv.Columns.Add(new DataGridViewLinkColumn
            {
                Name = K_TAC,
                HeaderText = "Thao tác",
                Text = "Chi tiết",
                UseColumnTextForLinkValue = true,
                LinkBehavior = LinkBehavior.HoverUnderline,
                LinkColor = Color.FromArgb(23, 82, 255),
                ActiveLinkColor = Color.FromArgb(23, 82, 255),
                VisitedLinkColor = Color.FromArgb(23, 82, 255)
            });

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgv.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.RowTemplate.Height = 56;


            dgv.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                    dgv.Columns[e.ColumnIndex].Name == K_TAC) dgv.Cursor = Cursors.Hand;
            };
            dgv.CellMouseLeave += (s, e) => dgv.Cursor = Cursors.Default;
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
            var dgv = (DataGridView)sender;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (dgv.Columns[e.ColumnIndex].Name == K_TT)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string text = Convert.ToString(e.FormattedValue) ?? "";
                bool ok = text.Equals("Đủ hàng", StringComparison.OrdinalIgnoreCase);

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2, 90, 28);
                using var path = Rounded(chip, 14);
                using var fill = new SolidBrush(ok ? Color.FromArgb(209, 250, 229) : Color.FromArgb(254, 243, 199));
                using var br = new SolidBrush(ok ? Color.FromArgb(16, 128, 67) : Color.FromArgb(146, 64, 14));

                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }
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
        }

        private void dgvKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvKho.Columns[e.ColumnIndex].Name != K_TAC) return;

            string ten = dgvKho.Rows[e.RowIndex].Cells[K_TEN].Value?.ToString();
            MessageBox.Show($"Xem chi tiết nguyên liệu: {ten}", "Kho");
        }

        private void roundedButton1_Click_1(object sender, EventArgs e)
        {
            using (var f = new Frm_XuatKho())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }

        private void btnChuyenKho_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_ChuyenKho())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }
    }
}
