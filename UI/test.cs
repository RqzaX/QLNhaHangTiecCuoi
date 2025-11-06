using Sunny.UI;
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
    public partial class test : Form
    {
        //private UIDataGridView dgvTest;
        private DataGridViewButtonColumn colPrint;
        private DataGridViewButtonColumn colRefund;

        public test()
        {
            InitializeComponent();
            this.Load += test_Load;
            dgvTest = this.dgvTest; // gán sẵn nếu đã kéo sẵn control
            dgvTest.CellContentClick += dgvTest_CellContentClick;
        }
        private class InvoiceRow
        {
            public string MaHD { get; set; }
            public string BanSanh { get; set; }
            public decimal SoTien { get; set; }
            public string KhuyenMai { get; set; }
            public string PhuongThuc { get; set; }
            public DateTime Ngay { get; set; }
            public string ThoiGian => Ngay.ToString("HH:mm");
            public string ThuNgan { get; set; }
            public string TrangThai { get; set; }
        }
        private void test_Load(object sender, EventArgs e)
        {
            SetupGrid();
            SeedData();
        }
        private void SetupGrid()
        {
            // Clear designer-added columns first to avoid Frozen columns inheriting Fill
            dgvTest.Columns.Clear();
            dgvTest.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvTest.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTest.AutoGenerateColumns = false;
            dgvTest.AllowUserToAddRows = false;
            dgvTest.ReadOnly = true;
            dgvTest.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTest.MultiSelect = false;
            dgvTest.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTest.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTest.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            dgvTest.EnableHeadersVisualStyles = false;
            dgvTest.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(80, 160, 255);
            dgvTest.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgvTest.GridColor = System.Drawing.Color.FromArgb(80, 160, 255);

            dgvTest.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaHD", HeaderText = "Mã HĐ" });
            dgvTest.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "BanSanh", HeaderText = "Bàn/Sảnh" });
            dgvTest.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoTien",
                HeaderText = "Số tiền",
                DefaultCellStyle = { Format = "n0" }
            });
            dgvTest.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "KhuyenMai", HeaderText = "Khuyến mãi" });
            dgvTest.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PhuongThuc", HeaderText = "Phương thức" });
            dgvTest.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Ngay",
                HeaderText = "Ngày",
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });
            dgvTest.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ThoiGian", HeaderText = "Thời gian" });
            dgvTest.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ThuNgan", HeaderText = "Thu ngân" });
            dgvTest.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TrangThai", HeaderText = "Trạng thái" });

            // Hai cột nút thao tác
            colPrint = new DataGridViewButtonColumn
            {
                HeaderText = "In hóa đơn",
                Text = "In hóa đơn",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            colRefund = new DataGridViewButtonColumn
            {
                HeaderText = "Hoàn tiền",
                Text = "Hoàn tiền",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            };

            dgvTest.Columns.Add(colPrint);
            dgvTest.Columns.Add(colRefund);
        }

        private void SeedData()
        {
            var rnd = new Random();
            string[] thuNgan = { "Hà My", "Minh Khang", "Trúc Vy", "Anh Tuấn" };
            string[] pm = { "Tiền mặt", "Chuyển khoản", "QR" };
            string[] trangThai = { "NHÁP", "CHỜ TT", "ĐÃ THANH TOÁN" };

            var list = Enumerable.Range(1, 10).Select(i => new InvoiceRow
            {
                MaHD = $"HD{i:000}",
                BanSanh = (i % 2 == 0) ? $"Bàn T{i:00}" : $"Sảnh Ruby {(i % 3) + 1}",
                SoTien = 200000 + i * 80000,
                KhuyenMai = (i % 3 == 0) ? "5%" : "-",
                PhuongThuc = pm[rnd.Next(pm.Length)],
                Ngay = DateTime.Today.AddDays(-rnd.Next(0, 5)).AddHours(10 + rnd.Next(0, 8)),
                ThuNgan = thuNgan[rnd.Next(thuNgan.Length)],
                TrangThai = trangThai[rnd.Next(trangThai.Length)]
            }).ToList();

            dgvTest.DataSource = new BindingList<InvoiceRow>(list);
        }

        private void dgvTest_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = ((BindingList<InvoiceRow>)dgvTest.DataSource)[e.RowIndex];

            if (e.ColumnIndex == colPrint.Index)
            {
                UIMessageBox.Show($"🧾 Đang in hóa đơn: {row.MaHD}", "In hóa đơn", UIStyle.Blue);
            }
            else if (e.ColumnIndex == colRefund.Index)
            {
                if (UIMessageBox.ShowAsk($"Xác nhận hoàn tiền cho {row.MaHD}?"))
                {
                    UIMessageBox.Show($"✅ Đã hoàn tiền cho hóa đơn {row.MaHD}", "Thành công", UIStyle.Green);
                }
            }
        }
    }
}
