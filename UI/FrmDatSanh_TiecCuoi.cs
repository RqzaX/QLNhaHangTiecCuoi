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
    public partial class FrmDatSanh_TiecCuoi : Form
    {
        public FrmDatSanh_TiecCuoi()
        {
            InitializeComponent();
        }

        private void FrmDatSanh_TiecCuoi_Load(object sender, EventArgs e)
        {
            // 1) Tạo cột nếu chưa có
            if (dgvDatSanh.Columns.Count == 0)
            {
                dgvDatSanh.AutoGenerateColumns = false;
                dgvDatSanh.AllowUserToAddRows = false;
                dgvDatSanh.RowHeadersVisible = false;
                dgvDatSanh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaDon", HeaderText = "Mã đơn", Width = 110 });
                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn { Name = "NgayTiec", HeaderText = "Ngày tiệc", Width = 110 });
                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn { Name = "KhachHang", HeaderText = "Khách hàng", Width = 220 });
                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn { Name = "Sanh", HeaderText = "Sảnh", Width = 120 });
                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn { Name = "SoBan", HeaderText = "Số bàn/khách", Width = 150 });
                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TongTien",
                    HeaderText = "Tổng tiền",
                    Width = 120,
                    DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "c0" }
                });
                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TienCoc",
                    HeaderText = "Tiền cọc",
                    Width = 110,
                    DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "c0" }
                });
                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng thái", Width = 140 });
                // Cột thao tác để vẽ nút
                dgvDatSanh.Columns.Add(new DataGridViewTextBoxColumn { Name = "ThaoTac", HeaderText = "Thao tác", Width = 200 });

                // Gán index cho control custom (rất quan trọng để vẽ/hit-test nút)
                dgvDatSanh.ColMaDonIndex = dgvDatSanh.Columns["MaDon"].Index;
                dgvDatSanh.ColTrangThaiIndex = dgvDatSanh.Columns["TrangThai"].Index;
                dgvDatSanh.ColThaoTacIndex = dgvDatSanh.Columns["ThaoTac"].Index;
            }

            // 2) Thêm dữ liệu sau khi có cột
            dgvDatSanh.Rows.Add("WD2025001", "2025-11-15", "Trần Thị Hoa & Nguyễn Văn Nam\n0901234567",
                                "Sảnh Diamond", "👥 50 bàn (500 khách)", 250_000_000m, 50_000_000m, "Đã xác nhận", "");

            dgvDatSanh.Rows.Add("WD2025002", "2025-11-20", "Lê Thị Mai & Hoàng Văn Minh\n0912345678",
                                "Sảnh Ruby", "👥 30 bàn (300 khách)", 150_000_000m, 30_000_000m, "Chờ xác nhận", "");

            dgvDatSanh.Rows.Add("BD2025001", "2025-10-25", "Phạm Thị Lan - Sinh nhật\n0923456789",
                                "Sảnh Emerald", "👥 10 bàn (100 khách)", 50_000_000m, 10_000_000m, "Đã xác nhận", "");

            // 3) Gắn sự kiện click nút
            dgvDatSanh.DetailClicked += (s, ev) => MessageBox.Show($"Chi tiết hàng {ev.RowIndex + 1}");
            dgvDatSanh.ConfirmClicked += (s, ev) =>
            {
                dgvDatSanh.Rows[ev.RowIndex].Cells["TrangThai"].Value = "Đã xác nhận";
                dgvDatSanh.InvalidateRow(ev.RowIndex);
            };
        }

        private void btnTaoDonDatSanh_Click(object sender, EventArgs e)
        {
            using (var f = new FrmDatSanhWizard())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }
    }
}
