using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;

namespace UI
{
    public partial class Frm_ThongTinBan : Form
    {
        private readonly BanBLL _banBLL;
        private readonly int _banId;
        private readonly string _soBan;
        private readonly string _trangThai;

        public Frm_ThongTinBan(int banId, string soBan, string trangThai, BanBLL banBLL)
        {
            InitializeComponent();
            _banId = banId;
            _soBan = soBan;
            _trangThai = trangThai;
            _banBLL = banBLL;
            
            this.Text = $"Thông tin bàn {soBan}";
            
            // Hiển thị nút theo trạng thái bàn
            btnTiepNhanKhach.Visible = (trangThai.ToUpper() == "ĐÃ ĐẶT");
            btnOrderThemMon.Visible = (trangThai.ToUpper() == "PHỤC VỤ");
            
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Load thông tin đặt bàn
                var dtDatBan = _banBLL.LayThongTinDatBan(_banId);
                if (dtDatBan != null && dtDatBan.Rows.Count > 0)
                {
                    var datBan = dtDatBan.Rows[0];
                    lblKhachHang.Text = datBan["ho_ten"].ToString();
                    lblSDT.Text = datBan["sdt"].ToString();
                    lblSoKhach.Text = datBan["so_khach"].ToString();
                    lblGhiChu.Text = datBan["ghi_chu"].ToString();
                    lblThoiGianDat.Text = Convert.ToDateTime(datBan["ngay_gio"]).ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    lblKhachHang.Text = "Không có thông tin";
                    lblSDT.Text = "Không có thông tin";
                    lblSoKhach.Text = "0";
                    lblGhiChu.Text = "Không có ghi chú";
                    lblThoiGianDat.Text = "Không có thông tin";
                }

                // Load order hiện tại
                var dtOrder = _banBLL.LayOrderHienTai(_banId);
                if (dtOrder != null && dtOrder.Rows.Count > 0)
                {
                    dgvOrder.DataSource = dtOrder;
                    dgvOrder.Columns["mon_id"].Visible = false;
                    dgvOrder.Columns["phieu_order_id"].Visible = false;
                    dgvOrder.Columns["ngay_gio"].Visible = false;
                    dgvOrder.Columns["trang_thai"].Visible = false;
                    dgvOrder.Columns["don_gia"].Visible = false;
                    
                    // Format columns
                    dgvOrder.Columns["ten_mon"].HeaderText = "Tên món";
                    dgvOrder.Columns["so_luong"].HeaderText = "Số lượng";
                    dgvOrder.Columns["thanh_tien"].HeaderText = "Thành tiền";
                    dgvOrder.Columns["ghi_chu_bep"].HeaderText = "Ghi chú bếp";
                    
                    dgvOrder.Columns["so_luong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvOrder.Columns["thanh_tien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvOrder.Columns["thanh_tien"].DefaultCellStyle.Format = "N0";
                    
                    // Styling DataGridView
                    dgvOrder.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                    dgvOrder.DefaultCellStyle.BackColor = Color.White;
                    dgvOrder.DefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);
                    dgvOrder.DefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 246, 255);
                    dgvOrder.DefaultCellStyle.SelectionForeColor = Color.FromArgb(31, 41, 55);
                    
                    // Header styling
                    dgvOrder.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
                    dgvOrder.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);
                    dgvOrder.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    dgvOrder.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    
                    // Alternating rows
                    dgvOrder.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);
                    
                    // Disable editing
                    dgvOrder.ReadOnly = true;
                    dgvOrder.AllowUserToAddRows = false;
                    dgvOrder.AllowUserToDeleteRows = false;
                }
                else
                {
                    dgvOrder.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTiepNhanKhach_Click(object sender, EventArgs e)
        {
            try
            {
                if (_trangThai == "ĐÃ ĐẶT")
                {
                    var result = MessageBox.Show(
                        $"Xác nhận tiếp nhận khách cho bàn {_soBan}?\nBàn sẽ chuyển sang trạng thái 'Đang sử dụng' và có thể order món.",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        bool success = _banBLL.CapNhatTrangThaiBan(_banId, "PHỤC VỤ");
                        if (success)
                        {
                            MessageBox.Show("Đã tiếp nhận khách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật trạng thái bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOrderThemMon_Click(object sender, EventArgs e)
        {
            try
            {
                // Đóng form thông tin bàn và trả về kết quả để mở form order
                this.DialogResult = DialogResult.Yes; // Sử dụng DialogResult.Yes để phân biệt với OK
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
