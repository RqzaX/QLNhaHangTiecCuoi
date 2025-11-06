using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using UI.Common;

namespace UI
{
    public partial class Frm_ChiTietSanh : Form
    {
        private readonly SanhBLL _sanhBLL;
        private readonly int _sanhId;
        private bool _isEditMode = false;
        private bool _allowEdit = false;

        public Frm_ChiTietSanh(int sanhId, SanhBLL sanhBLL, bool allowEdit = false)
        {
            InitializeComponent();
            _sanhId = sanhId;
            _sanhBLL = sanhBLL;
            _allowEdit = allowEdit;
            
            this.Text = allowEdit ? "Sửa sảnh" : "Chi tiết sảnh";
            lblTitle.Text = allowEdit ? "Sửa sảnh" : "Chi tiết sảnh";
            lblSubtitle.Text = allowEdit ? "Chỉnh sửa thông tin sảnh" : "Thông tin chi tiết về sảnh";
            
            LoadData();
            
            // Ẩn nút Sửa nếu không cho phép chỉnh sửa
            if (!_allowEdit)
            {
                btnSua.Visible = false;
                SetEditMode(false);
                // Điều chỉnh vị trí nút Đóng
                btnDong.Location = new Point((this.Width - btnDong.Width) / 2, btnDong.Location.Y);
            }
            else
            {
                // Nếu cho phép chỉnh sửa, tự động vào chế độ chỉnh sửa ngay
                SetEditMode(true);
            }
        }

        private void LoadData()
        {
            try
            {
                // Load thông tin sảnh
                var dtSanh = _sanhBLL.LayThongTinSanh(_sanhId);
                if (dtSanh != null && dtSanh.Rows.Count > 0)
                {
                    var sanh = dtSanh.Rows[0];
                    
                    txtTenSanh.Text = sanh["ten_sanh"]?.ToString() ?? "";
                    txtSucChua.Text = sanh["suc_chua"]?.ToString() ?? "0";
                    txtPhiThueCb.Text = sanh["phi_thue_cb"] != DBNull.Value ? 
                        Convert.ToDecimal(sanh["phi_thue_cb"]).ToString("N0") : "0";
                    
                    // Load chi nhánh
                    LoadChiNhanh();
                    int? chiNhanhId = sanh["chi_nhanh_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(sanh["chi_nhanh_id"]);
                    if (chiNhanhId.HasValue)
                    {
                        for (int i = 0; i < cbbChiNhanh.Items.Count; i++)
                        {
                            DataRowView drv = (DataRowView)cbbChiNhanh.Items[i];
                            if (Convert.ToInt32(drv["chi_nhanh_id"]) == chiNhanhId.Value)
                            {
                                cbbChiNhanh.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiNhanh()
        {
            try
            {
                var dbHelper = new DatabaseHelper();
                var chiNhanhBLL = new BLL.ChiNhanhBLL(dbHelper);
                var dtChiNhanh = chiNhanhBLL.LayTatCaChiNhanh();
                cbbChiNhanh.DisplayMember = "ten";
                cbbChiNhanh.ValueMember = "chi_nhanh_id";
                cbbChiNhanh.DataSource = dtChiNhanh;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEditMode(bool isEdit)
        {
            _isEditMode = isEdit;
            
            txtTenSanh.ReadOnly = !isEdit;
            txtSucChua.ReadOnly = !isEdit;
            txtPhiThueCb.ReadOnly = !isEdit;
            cbbChiNhanh.Enabled = isEdit;
            
            if (isEdit)
            {
                btnSua.Text = "Lưu";
                btnSua.BackColor = Color.FromArgb(34, 197, 94);
                btnSua.HoverBackColor = Color.FromArgb(22, 163, 74);
                btnSua.PressedBackColor = Color.FromArgb(21, 128, 61);
            }
            else
            {
                btnSua.Text = "Sửa";
                btnSua.BackColor = Color.FromArgb(59, 130, 246);
                btnSua.HoverBackColor = Color.FromArgb(37, 99, 235);
                btnSua.PressedBackColor = Color.FromArgb(29, 78, 216);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!_allowEdit)
            {
                return; // Không cho phép sửa
            }

            if (!_isEditMode)
            {
                // Chuyển sang chế độ sửa
                SetEditMode(true);
            }
            else
            {
                // Lưu dữ liệu
                SaveData();
            }
        }

        private void SaveData()
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(txtTenSanh.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên sảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenSanh.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSucChua.Text) || !int.TryParse(txtSucChua.Text, out int sucChua) || sucChua <= 0)
                {
                    MessageBox.Show("Vui lòng nhập sức chứa hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSucChua.Focus();
                    return;
                }

                // Parse phí thuê cơ bản (loại bỏ dấu phẩy, dấu chấm, và ký tự không phải số)
                string phiThueText = txtPhiThueCb.Text.Trim().Replace(",", "").Replace(".", "").Replace("₫", "").Replace(" ", "");
                if (string.IsNullOrWhiteSpace(phiThueText) || !decimal.TryParse(phiThueText, out decimal phiThueCb) || phiThueCb < 0)
                {
                    MessageBox.Show("Vui lòng nhập phí thuê cơ bản hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhiThueCb.Focus();
                    return;
                }

                // Lưu dữ liệu
                string tenSanh = txtTenSanh.Text.Trim();
                
                // Cập nhật sảnh
                bool result = _sanhBLL.CapNhatSanh(_sanhId, tenSanh, sucChua, phiThueCb);
                
                if (result)
                {
                    MessageBox.Show("Cập nhật sảnh thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetEditMode(false);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật sảnh thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật sảnh: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

