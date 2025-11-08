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
            
            this.Resize += (s, e) => DieuChinhDoRongCot();
            if (dgvDatSanh != null)
            {
                dgvDatSanh.Resize += (s, e) => DieuChinhDoRongCot();
            }
        }

        private void FrmDatSanh_TiecCuoi_Load(object sender, EventArgs e)
        {
            CapNhatThongKe();
            
            if (dgvDatSanh.Columns.Count == 0)
            {
                dgvDatSanh.AutoGenerateColumns = false;
                dgvDatSanh.AllowUserToAddRows = false;
                dgvDatSanh.RowHeadersVisible = false;
                dgvDatSanh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDatSanh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                var colMaDon = new DataGridViewTextBoxColumn { Name = "MaDon", HeaderText = "Mã đơn", Width = 100, MinimumWidth = 90 };
                var colNgayTiec = new DataGridViewTextBoxColumn { Name = "NgayTiec", HeaderText = "Ngày tiệc", Width = 150, MinimumWidth = 130 };
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

                colThaoTac.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDatSanh.ColMaDonIndex = dgvDatSanh.Columns["MaDon"].Index;
                dgvDatSanh.ColTrangThaiIndex = dgvDatSanh.Columns["TrangThai"].Index;
                dgvDatSanh.ColThaoTacIndex = dgvDatSanh.Columns["ThaoTac"].Index;
            }

            LoadDanhSachDatSanh();
            dgvDatSanh.HuyDatClicked += DgvDatSanh_HuyDatClicked;
            dgvDatSanh.OrderCodeClicked += DgvDatSanh_OrderCodeClicked;
            dgvDatSanh.CellDoubleClick += DgvDatSanh_CellDoubleClick;
        }

        private void CapNhatThongKe()
        {
            try
            {
                int tongSoDon = _datSanhBLL.LayTongSoDon();
                lbSoDon.Text = $"{tongSoDon:N0} đơn";

                int soDonXacNhan = _datSanhBLL.LaySoDonXacNhan();
                lbSoDonXacNhan.Text = $"{soDonXacNhan:N0} đơn";

                int tongSoSanh = _datSanhBLL.LayTongSoSanh();
                lbTongSanh.Text = $"{tongSoSanh:N0} sảnh";

                decimal doanhThuThang = _datSanhBLL.LayDoanhThuThang();
                lbDoanhThuThang.Text = doanhThuThang > 0 ? doanhThuThang.ToString("#,##0") + " ₫" : "0 ₫";
            }
            catch (Exception ex)
            {
                lbSoDon.Text = "0 đơn";
                lbSoDonXacNhan.Text = "0 đơn";
                lbTongSanh.Text = "0 sảnh";
                lbDoanhThuThang.Text = "0 ₫";
                System.Diagnostics.Debug.WriteLine($"Lỗi cập nhật thống kê: {ex.Message}");
            }
        }

        private void btnTaoDonDatSanh_Click(object sender, EventArgs e)
        {
            var formCon = new Frm_DatSanh
            {
                Text = "Tạo đơn đặt sảnh",
                StartPosition = FormStartPosition.CenterParent
            };

            formCon.ShowDialog(this);
            
            if (formCon.DialogResult == DialogResult.OK)
            {
                LoadDanhSachDatSanh();
                CapNhatThongKe();
            }
        }

        private void LoadDanhSachDatSanh()
        {
            try
            {
                dgvDatSanh.Rows.Clear();
                DataTable dt = _datSanhBLL.LayDanhSachDatSanh();

                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }
                foreach (DataRow row in dt.Rows)
                {
                    int datSanhId = Convert.ToInt32(row["dat_sanh_id"]);
                    DateTime ngayToChuc = Convert.ToDateTime(row["ngay_to_chuc"]);
                    
                    TimeSpan? gioToChuc = null;
                    if (row["gio_to_chuc"] != DBNull.Value && row["gio_to_chuc"] != null)
                    {
                        if (row["gio_to_chuc"] is TimeSpan ts)
                        {
                            gioToChuc = ts;
                        }
                        else if (TimeSpan.TryParse(row["gio_to_chuc"].ToString(), out TimeSpan parsedTime))
                        {
                            gioToChuc = parsedTime;
                        }
                    }
                    
                    string tenKhachHang = row["ten_khach_hang"].ToString();
                    string sdt = row["sdt"]?.ToString() ?? "";
                    string tenSanh = row["ten_sanh"].ToString();
                    int? soBanDuKien = row["so_ban_du_kien"] != DBNull.Value ? (int?)Convert.ToInt32(row["so_ban_du_kien"]) : null;
                    int? soKhachDuKien = row["so_khach_du_kien"] != DBNull.Value ? (int?)Convert.ToInt32(row["so_khach_du_kien"]) : null;
                    string trangThai = row["trang_thai"].ToString();
                    decimal tongTien = Convert.ToDecimal(row["tong_tien"]);
                    decimal tienCoc = Convert.ToDecimal(row["tien_coc"]);

                    string maDon = $"DS{datSanhId:D6}";
                    string ngayTiec = ngayToChuc.ToString("dd/MM/yyyy");
                    if (gioToChuc.HasValue)
                    {
                        ngayTiec += $"\n{gioToChuc.Value:hh\\:mm}";
                    }
                    string khachHang = $"{tenKhachHang}\n{sdt}";
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

                    string trangThaiHienThi = trangThai;
                    switch (trangThai.ToUpper())
                    {
                        case "CHỜ XÁC NHẬN":
                            trangThaiHienThi = "Chờ xác nhận";
                            break;
                        case "ĐÃ XÁC NHẬN":
                            trangThaiHienThi = "Đã xác nhận";
                            break;
                        case "ĐÃ CỌC":
                            trangThaiHienThi = "Đã cọc";
                            break;
                        case "ĐÃ THANH TOÁN":
                            trangThaiHienThi = "Đã thanh toán";
                            break;
                        case "ĐÃ HỦY":
                            trangThaiHienThi = "Đã hủy";
                            break;
                        case "HOÀN TẤT":
                            trangThaiHienThi = "Hoàn tất";
                            break;
                    }

                    string tongTienFormatted = tongTien > 0 ? tongTien.ToString("#,##0") + " ₫" : "0 ₫";
                    string tienCocFormatted = tienCoc > 0 ? tienCoc.ToString("#,##0") + " ₫" : "0 ₫";

                    dgvDatSanh.Rows.Add(maDon, ngayTiec, khachHang, tenSanh, soBanKhach, 
                                       tongTienFormatted, tienCocFormatted, trangThaiHienThi, "");
                }

                DieuChinhDoRongCot();
                CapNhatThongKe();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn đặt sảnh: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDatSanh_HuyDatClicked(object sender, Controls.RowActionEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex >= dgvDatSanh.Rows.Count)
                    return;

                string maDon = dgvDatSanh.Rows[e.RowIndex].Cells["MaDon"].Value?.ToString() ?? "";
                
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn hủy đặt sảnh cho đơn {maDon}?",
                    "Xác nhận hủy đặt",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                if (maDon.StartsWith("DS") && int.TryParse(maDon.Substring(2), out int datSanhId))
                {
                    string errorMessage;
                    bool success = _datSanhBLL.HuyDatSanh(datSanhId, out errorMessage);
                    
                    if (success)
                    {
                        MessageBox.Show($"Đã hủy đặt sảnh cho đơn {maDon}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachDatSanh();
                        CapNhatThongKe();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi hủy đặt sảnh: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không thể lấy mã đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hủy đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDatSanh_OrderCodeClicked(object sender, Controls.RowActionEventArgs e)
        {
            MoFormChiTietDatSanh(e.RowIndex);
        }

        private void DgvDatSanh_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MoFormChiTietDatSanh(e.RowIndex);
            }
        }

        private void MoFormChiTietDatSanh(int rowIndex)
        {
            try
            {
                if (rowIndex < 0 || rowIndex >= dgvDatSanh.Rows.Count)
                    return;

                string maDon = dgvDatSanh.Rows[rowIndex].Cells["MaDon"].Value?.ToString() ?? "";
                
                if (maDon.StartsWith("DS") && int.TryParse(maDon.Substring(2), out int datSanhId))
                {
                    var formChiTiet = new Frm_ChiTietDatSanh(datSanhId)
                    {
                        StartPosition = FormStartPosition.CenterParent
                    };
                    formChiTiet.ShowDialog(this);
                    
                    // Reload danh sách sau khi đóng form chi tiết (nếu có thay đổi)
                    LoadDanhSachDatSanh();
                    CapNhatThongKe();
                }
                else
                {
                    MessageBox.Show("Không thể lấy mã đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DieuChinhDoRongCot()
        {
            try
            {
                if (dgvDatSanh == null || dgvDatSanh.Columns.Count == 0) return;

                int totalFixedWidth = 0;
                foreach (DataGridViewColumn col in dgvDatSanh.Columns)
                {
                    if (col.Name != "ThaoTac" && col.AutoSizeMode != DataGridViewAutoSizeColumnMode.Fill)
                    {
                        totalFixedWidth += col.Width;
                    }
                }

                var colThaoTac = dgvDatSanh.Columns["ThaoTac"];
                if (colThaoTac != null && dgvDatSanh.ClientSize.Width > 0)
                {
                    int availableWidth = dgvDatSanh.ClientSize.Width - totalFixedWidth - 20;
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
