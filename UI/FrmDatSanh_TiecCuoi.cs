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
using QLNhaHangTiecCuoi.BLL;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmDatSanh_TiecCuoi : Form
    {
        private DatSanhBLL _datSanhBLL;

        public FrmDatSanh_TiecCuoi()
        {
            InitializeComponent();
            _datSanhBLL = new DatSanhBLL();
            
            // Gắn sự kiện resize để tự động điều chỉnh độ rộng cột
            this.Resize += (s, e) => DieuChinhDoRongCot();
            if (dgvDatSanh != null)
            {
                dgvDatSanh.Resize += (s, e) => DieuChinhDoRongCot();
            }
        }

        private void FrmDatSanh_TiecCuoi_Load(object sender, EventArgs e)
        {
            // Tạo cột nếu chưa có
            if (dgvDatSanh.Columns.Count == 0)
            {
                dgvDatSanh.AutoGenerateColumns = false;
                dgvDatSanh.AllowUserToAddRows = false;
                dgvDatSanh.RowHeadersVisible = false;
                dgvDatSanh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDatSanh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                var colMaDon = new DataGridViewTextBoxColumn { Name = "MaDon", HeaderText = "Mã đơn", Width = 100, MinimumWidth = 90 };
                var colNgayTiec = new DataGridViewTextBoxColumn { Name = "NgayTiec", HeaderText = "Ngày tiệc", Width = 100, MinimumWidth = 90 };
                var colKhachHang = new DataGridViewTextBoxColumn { Name = "KhachHang", HeaderText = "Khách hàng", Width = 200, MinimumWidth = 150 };
                var colSanh = new DataGridViewTextBoxColumn { Name = "Sanh", HeaderText = "Sảnh", Width = 120, MinimumWidth = 100 };
                var colSoBan = new DataGridViewTextBoxColumn { Name = "SoBan", HeaderText = "Số bàn/khách", Width = 150, MinimumWidth = 130 };
                var colTongTien = new DataGridViewTextBoxColumn
                {
                    Name = "TongTien",
                    HeaderText = "Tổng tiền",
                    Width = 120,
                    MinimumWidth = 110,
                    DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
                };
                var colTienCoc = new DataGridViewTextBoxColumn
                {
                    Name = "TienCoc",
                    HeaderText = "Tiền cọc",
                    Width = 110,
                    MinimumWidth = 100,
                    DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
                };
                var colTrangThai = new DataGridViewTextBoxColumn { Name = "TrangThai", HeaderText = "Trạng thái", Width = 130, MinimumWidth = 120 };
                var colThaoTac = new DataGridViewTextBoxColumn { Name = "ThaoTac", HeaderText = "Thao tác", Width = 120, MinimumWidth = 100 };

                dgvDatSanh.Columns.Add(colMaDon);
                dgvDatSanh.Columns.Add(colNgayTiec);
                dgvDatSanh.Columns.Add(colKhachHang);
                dgvDatSanh.Columns.Add(colSanh);
                dgvDatSanh.Columns.Add(colSoBan);
                dgvDatSanh.Columns.Add(colTongTien);
                dgvDatSanh.Columns.Add(colTienCoc);
                dgvDatSanh.Columns.Add(colTrangThai);
                dgvDatSanh.Columns.Add(colThaoTac);

                // Điều chỉnh cột ThaoTac để fill phần còn lại
                colThaoTac.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDatSanh.ColMaDonIndex = dgvDatSanh.Columns["MaDon"].Index;
                dgvDatSanh.ColTrangThaiIndex = dgvDatSanh.Columns["TrangThai"].Index;
                dgvDatSanh.ColThaoTacIndex = dgvDatSanh.Columns["ThaoTac"].Index;
            }

            // Load dữ liệu từ database
            LoadDanhSachDatSanh();

            // Gắn sự kiện click nút
            dgvDatSanh.DetailClicked += DgvDatSanh_DetailClicked;
            dgvDatSanh.ConfirmClicked += DgvDatSanh_ConfirmClicked;
        }

        private void btnTaoDonDatSanh_Click(object sender, EventArgs e)
        {
            var formCon = new Frm_DatSanh
            {
                Text = "Tạo đơn đặt sảnh",
                StartPosition = FormStartPosition.CenterParent
            };

            // Thêm nút đóng
            // var btnClose = new Button
            // {
            //     Text = "✕",
            //     Size = new Size(35, 35),
            //     Location = new Point(formCon.ClientSize.Width - 45, 10),
            //     FlatStyle = FlatStyle.Flat,
            //     Font = new Font("Segoe UI", 14f),
            //     ForeColor = Color.Black,
            //     BackColor = Color.Transparent,
            //     Cursor = Cursors.Hand
            // };
            // btnClose.FlatAppearance.BorderSize = 0;
            // btnClose.Click += (s, ev) => formCon.Close();
            // formCon.Controls.Add(btnClose);

            // formCon.Resize += (s, ev) =>
            // {
            //     btnClose.Location = new Point(formCon.ClientSize.Width - 45, 10);
            // };

            formCon.ShowDialog(this);
            
            // Reload dữ liệu sau khi tạo đơn mới
            if (formCon.DialogResult == DialogResult.OK)
            {
                LoadDanhSachDatSanh();
            }
        }

        // Load danh sách đơn đặt sảnh từ database
        private void LoadDanhSachDatSanh()
        {
            try
            {
                dgvDatSanh.Rows.Clear();
                // Lấy dữ liệu từ database
                DataTable dt = _datSanhBLL.LayDanhSachDatSanh();

                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }
                // Thêm dữ liệu vào DataGridView
                foreach (DataRow row in dt.Rows)
                {
                    int datSanhId = Convert.ToInt32(row["dat_sanh_id"]);
                    DateTime ngayToChuc = Convert.ToDateTime(row["ngay_to_chuc"]);
                    string tenKhachHang = row["ten_khach_hang"].ToString();
                    string sdt = row["sdt"]?.ToString() ?? "";
                    string tenSanh = row["ten_sanh"].ToString();
                    int? soBanDuKien = row["so_ban_du_kien"] != DBNull.Value ? (int?)Convert.ToInt32(row["so_ban_du_kien"]) : null;
                    int? soKhachDuKien = row["so_khach_du_kien"] != DBNull.Value ? (int?)Convert.ToInt32(row["so_khach_du_kien"]) : null;
                    string trangThai = row["trang_thai"].ToString();
                    decimal tongTien = Convert.ToDecimal(row["tong_tien"]);
                    decimal tienCoc = Convert.ToDecimal(row["tien_coc"]);

                    // Format mã đơn: DS + 6 số (VD: DS000001)
                    string maDon = $"DS{datSanhId:D6}";
                    // Format ngày tiệc
                    string ngayTiec = ngayToChuc.ToString("dd/MM/yyyy");
                    // Format thông tin khách hàng
                    string khachHang = $"{tenKhachHang}\n{sdt}";
                    // Format số bàn/khách
                    string soBanKhach = "";
                    if (soBanDuKien.HasValue && soKhachDuKien.HasValue)
                    {
                        soBanKhach = $"👥 {soBanDuKien.Value} bàn ({soKhachDuKien.Value:N0} khách)";
                    }
                    else if (soBanDuKien.HasValue)
                    {
                        soBanKhach = $"👥 {soBanDuKien.Value} bàn";
                    }
                    else
                    {
                        soBanKhach = "Chưa xác định";
                    }

                    // Format trạng thái (chuyển từ database sang hiển thị)
                    string trangThaiHienThi = trangThai;
                    switch (trangThai.ToUpper())
                    {
                        case "CHỜ XÁC NHẬN":
                            trangThaiHienThi = "Chờ xác nhận";
                            break;
                        case "ĐÃ XÁC NHẬN":
                            trangThaiHienThi = "Đã xác nhận";
                            break;
                        case "ĐÃ HỦY":
                            trangThaiHienThi = "Đã hủy";
                            break;
                        case "HOÀN TẤT":
                            trangThaiHienThi = "Hoàn tất";
                            break;
                    }

                    // Format tiền với dấu phẩy ngăn cách hàng nghìn
                    string tongTienFormatted = tongTien > 0 ? tongTien.ToString("#,##0") + " ₫" : "0 ₫";
                    string tienCocFormatted = tienCoc > 0 ? tienCoc.ToString("#,##0") + " ₫" : "0 ₫";

                    // Thêm dòng vào DataGridView
                    dgvDatSanh.Rows.Add(maDon, ngayTiec, khachHang, tenSanh, soBanKhach, 
                                       tongTienFormatted, tienCocFormatted, trangThaiHienThi, "");
                }

                // Điều chỉnh độ rộng cột sau khi load dữ liệu
                DieuChinhDoRongCot();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn đặt sảnh: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện click nút Chi tiết
        private void DgvDatSanh_DetailClicked(object sender, Controls.RowActionEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvDatSanh.Rows.Count)
                    return;

                // Lấy mã đơn từ cột đầu tiên
                string maDon = dgvDatSanh.Rows[e.RowIndex].Cells["MaDon"].Value?.ToString() ?? "";
                MessageBox.Show($"Chi tiết đơn: {maDon}", "Chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện click nút Xác nhận
        private void DgvDatSanh_ConfirmClicked(object sender, Controls.RowActionEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvDatSanh.Rows.Count)
                    return;

                // Lấy mã đơn
                string maDon = dgvDatSanh.Rows[e.RowIndex].Cells["MaDon"].Value?.ToString() ?? "";
                
                // Lấy dat_sanh_id từ mã đơn (DS000001 -> 1)
                if (maDon.StartsWith("DS") && int.TryParse(maDon.Substring(2), out int datSanhId))
                {
                    // Xác nhận đơn đặt sảnh
                    string errorMessage;
                    bool success = _datSanhBLL.CapNhatTrangThaiDatSanh(datSanhId, "ĐÃ XÁC NHẬN", out errorMessage);
                    
                    if (success)
                    {
                        MessageBox.Show($"Đã xác nhận đơn {maDon}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Reload dữ liệu để đảm bảo đồng bộ
                        LoadDanhSachDatSanh();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi xác nhận đơn: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không thể lấy mã đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xác nhận: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Điều chỉnh độ rộng cột để hiển thị đầy đủ
        private void DieuChinhDoRongCot()
        {
            try
            {
                if (dgvDatSanh == null || dgvDatSanh.Columns.Count == 0) return;

                // Tính tổng độ rộng cố định của các cột (trừ cột ThaoTac)
                int totalFixedWidth = 0;
                foreach (DataGridViewColumn col in dgvDatSanh.Columns)
                {
                    if (col.Name != "ThaoTac" && col.AutoSizeMode != DataGridViewAutoSizeColumnMode.Fill)
                    {
                        totalFixedWidth += col.Width;
                    }
                }

                // Điều chỉnh cột ThaoTac để fill phần còn lại
                var colThaoTac = dgvDatSanh.Columns["ThaoTac"];
                if (colThaoTac != null && dgvDatSanh.ClientSize.Width > 0)
                {
                    int availableWidth = dgvDatSanh.ClientSize.Width - totalFixedWidth - 20; // Trừ padding và scrollbar
                    if (availableWidth > colThaoTac.MinimumWidth)
                    {
                        colThaoTac.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        colThaoTac.Width = availableWidth;
                    }
                    else
                    {
                        colThaoTac.Width = colThaoTac.MinimumWidth;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi điều chỉnh độ rộng cột: {ex.Message}");
            }
        }
    }
}
