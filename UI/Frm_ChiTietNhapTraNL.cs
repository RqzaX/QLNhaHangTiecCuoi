using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;
using System.ComponentModel;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;
using Guna.UI2.WinForms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_ChiTietNhapTraNL : Form
    {
        private DatabaseHelper _dbHelper;
        private NguyenLieuBLL _nguyenLieuBLL;
        private int _phieuId;
        private string _loaiPhieu;
        private DataRow _thongTinPhieu;

        public Frm_ChiTietNhapTraNL(int phieuId, string loaiPhieu)
        {
            InitializeComponent();
            
            // Tránh khởi tạo database khi đang ở design mode
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
            
            _dbHelper = new DatabaseHelper();
            _nguyenLieuBLL = new NguyenLieuBLL(_dbHelper);
            _phieuId = phieuId;
            _loaiPhieu = loaiPhieu;
            
            this.Load += Frm_ChiTietNhapTraNL_Load;
            btnDong.Click += btnDong_Click;
            btnHuy.Click += btnHuy_Click;
        }

        private void Frm_ChiTietNhapTraNL_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Lấy thông tin phiếu
                if (_loaiPhieu == "Nhập kho")
                {
                    _thongTinPhieu = _nguyenLieuBLL.LayThongTinPhieuNhap(_phieuId);
                    var chiTiet = _nguyenLieuBLL.LayChiTietPhieuNhap(_phieuId);
                    LoadChiTietNhap(chiTiet);
                }
                else if (_loaiPhieu == "Trả kho")
                {
                    _thongTinPhieu = _nguyenLieuBLL.LayThongTinPhieuTra(_phieuId);
                    var chiTiet = _nguyenLieuBLL.LayChiTietPhieuTra(_phieuId);
                    LoadChiTietTra(chiTiet);
                }

                if (_thongTinPhieu == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin phiếu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Hiển thị thông tin phiếu
                lblTieuDe.Text = $"Chi tiết {_loaiPhieu}";
                lblMaPhieu.Text = $"Mã phiếu: {_phieuId}";

                if (_loaiPhieu == "Nhập kho")
                {
                    lblNgay.Text = _thongTinPhieu["ngay_nhap"] != DBNull.Value
                        ? Convert.ToDateTime(_thongTinPhieu["ngay_nhap"]).ToString("dd/MM/yyyy")
                        : "";
                    
                    if (_thongTinPhieu["gio_nhap"] != DBNull.Value)
                    {
                        if (_thongTinPhieu["gio_nhap"] is TimeSpan timeSpan)
                        {
                            lblGio.Text = timeSpan.ToString(@"hh\:mm");
                        }
                        else
                        {
                            lblGio.Text = _thongTinPhieu["gio_nhap"].ToString();
                        }
                    }
                    
                    lblNhanVien.Text = _thongTinPhieu["nhan_vien_nhap"]?.ToString() ?? "";
                }
                else
                {
                    lblNgay.Text = _thongTinPhieu["ngay_tra"] != DBNull.Value
                        ? Convert.ToDateTime(_thongTinPhieu["ngay_tra"]).ToString("dd/MM/yyyy")
                        : "";
                    
                    if (_thongTinPhieu["gio_tra"] != DBNull.Value)
                    {
                        if (_thongTinPhieu["gio_tra"] is TimeSpan timeSpan)
                        {
                            lblGio.Text = timeSpan.ToString(@"hh\:mm");
                        }
                        else
                        {
                            lblGio.Text = _thongTinPhieu["gio_tra"].ToString();
                        }
                    }
                    
                    lblNhanVien.Text = _thongTinPhieu["nhan_vien_tra"]?.ToString() ?? "";
                }

                string trangThai = _thongTinPhieu["trang_thai"]?.ToString() ?? "";
                lblTrangThai.Text = trangThai;
                
                // Đổi màu trạng thái
                if (trangThai == "ĐÃ LƯU")
                {
                    lblTrangThai.ForeColor = Color.FromArgb(34, 197, 94); // Xanh lá
                }
                else if (trangThai == "HỦY")
                {
                    lblTrangThai.ForeColor = Color.FromArgb(239, 68, 68); // Đỏ
                }
                else
                {
                    lblTrangThai.ForeColor = Color.FromArgb(107, 114, 128); // Xám
                }

                string ghiChu = _thongTinPhieu["ghi_chu"]?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(ghiChu))
                {
                    lblGhiChu.Text = ghiChu;
                    pnlGhiChu.Visible = true;
                }
                else
                {
                    pnlGhiChu.Visible = false;
                }

                btnHuy.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietNhap(DataTable chiTiet)
        {
            // Ẩn các cột không dùng cho phiếu nhập
            colTon.Visible = false;
            colConLai.Visible = false;
            colSoLuong.HeaderText = "Số lượng";
            
            dgvChiTiet.Rows.Clear();
            if (chiTiet != null && chiTiet.Rows.Count > 0)
            {
                foreach (DataRow row in chiTiet.Rows)
                {
                    int rowIndex = dgvChiTiet.Rows.Add();
                    var dgvRow = dgvChiTiet.Rows[rowIndex];
                    dgvRow.Cells[colSTT.Index].Value = row["stt"];
                    dgvRow.Cells[colMaNL.Index].Value = row["ma_nl"];
                    dgvRow.Cells[colTenNL.Index].Value = row["ten_nl"];
                    dgvRow.Cells[colSoLuong.Index].Value = FormatSoLuong(Convert.ToDecimal(row["so_luong"]));
                    dgvRow.Cells[colDVT.Index].Value = row["don_vi"];
                    dgvRow.Cells[colGhiChu.Index].Value = row["ghi_chu"]?.ToString() ?? "";
                }
            }
        }

        private void LoadChiTietTra(DataTable chiTiet)
        {
            // Hiển thị các cột cho phiếu trả
            colTon.Visible = true;
            colConLai.Visible = true;
            colSoLuong.HeaderText = "SL trả";
            
            dgvChiTiet.Rows.Clear();
            if (chiTiet != null && chiTiet.Rows.Count > 0)
            {
                foreach (DataRow row in chiTiet.Rows)
                {
                    int rowIndex = dgvChiTiet.Rows.Add();
                    var dgvRow = dgvChiTiet.Rows[rowIndex];
                    dgvRow.Cells[colSTT.Index].Value = row["stt"];
                    dgvRow.Cells[colMaNL.Index].Value = row["ma_nl"];
                    dgvRow.Cells[colTenNL.Index].Value = row["ten_nl"];
                    dgvRow.Cells[colTon.Index].Value = FormatSoLuong(Convert.ToDecimal(row["so_luong_ton"]));
                    dgvRow.Cells[colSoLuong.Index].Value = FormatSoLuong(Convert.ToDecimal(row["so_luong_tra"]));
                    dgvRow.Cells[colConLai.Index].Value = FormatSoLuong(Convert.ToDecimal(row["so_luong_con_lai"]));
                    dgvRow.Cells[colDVT.Index].Value = row["don_vi"];
                    dgvRow.Cells[colGhiChu.Index].Value = row["ghi_chu"]?.ToString() ?? "";
                }
            }
        }

        private string FormatSoLuong(decimal value)
        {
            return value.ToString("#,##0.###", CultureInfo.CurrentCulture);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            string message = _loaiPhieu == "Nhập kho" 
                ? "Bạn có chắc muốn XÓA VĨNH VIỄN phiếu nhập kho này?\n\nHành động này không thể hoàn tác!"
                : "Bạn có chắc muốn XÓA VĨNH VIỄN phiếu trả kho này?\n\nHành động này không thể hoàn tác!";

            if (MessageBox.Show(message, "Xác nhận xóa vĩnh viễn",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    if (_loaiPhieu == "Nhập kho")
                    {
                        _nguyenLieuBLL.XoaPhieuNhapKho(_phieuId);
                        MessageBox.Show("Xóa phiếu nhập kho thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (_loaiPhieu == "Trả kho")
                    {
                        _nguyenLieuBLL.XoaPhieuTraKho(_phieuId);
                        MessageBox.Show("Xóa phiếu trả kho thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                    // Trigger event để form cha refresh và đóng form
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa phiếu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

