using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;

namespace UI
{
    public partial class Frm_ChiTietKhachHang : Form
    {
        private KhachHangBLL _khachHangBLL;
        private DataTable _dtHang;
        private int _khachHangId;
        private bool _isEditMode = false;
        private bool _isUpdatingHang = false;

        public Frm_ChiTietKhachHang(int khachHangId)
        {
            InitializeComponent();
            _khachHangBLL = new KhachHangBLL();
            _khachHangId = khachHangId;

            // Load danh sách hạng từ database
            LoadDanhSachHang();

            // Load thông tin khách hàng
            LoadThongTinKhachHang();

            // Đăng ký event handlers
            this.Load += Frm_ChiTietKhachHang_Load;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLuu.Click += BtnLuu_Click;
            btnHuy.Click += (s, e) => this.Close();
            txtChiTieu.TextChanged += TxtChiTieu_TextChanged;

            // Mặc định ở chế độ xem
            SetEditMode(false);
        }

        private void Frm_ChiTietKhachHang_Load(object sender, EventArgs e)
        {
            // Ràng buộc số điện thoại: chỉ cho phép nhập số, tối đa 10 ký tự
            txtSDT.MaxLength = 10;
            txtSDT.KeyPress += TxtSDT_KeyPress;
        }

        private void TxtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số (0-9) và phím điều khiển (Backspace, Delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void LoadDanhSachHang()
        {
            try
            {
                _dtHang = _khachHangBLL.LayDanhSachHang();

                if (_dtHang != null && _dtHang.Rows.Count > 0)
                {
                    cbbHang.DataSource = _dtHang;
                    cbbHang.DisplayMember = "ten_hang";
                    cbbHang.ValueMember = "hang_code";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách hạng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThongTinKhachHang()
        {
            try
            {
                DataTable dt = _khachHangBLL.LayThongTinKhachHang(_khachHangId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    txtHoTen.Text = row["ho_ten"]?.ToString() ?? "";
                    txtSDT.Text = row["sdt"]?.ToString() ?? "";
                    txtEmail.Text = row["email"]?.ToString() ?? "";
                    txtGhiChu.Text = row["ghi_chu"]?.ToString() ?? "";

                    decimal tongChiTieu = row["tong_chi_tieu"] == DBNull.Value ? 0 : Convert.ToDecimal(row["tong_chi_tieu"]);
                    txtChiTieu.Text = tongChiTieu.ToString("N0");

                    string hangCode = row["hang_code"]?.ToString() ?? "MEM";
                    if (cbbHang.DataSource != null)
                    {
                        cbbHang.SelectedValue = hangCode;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load thông tin khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEditMode(bool isEdit)
        {
            _isEditMode = isEdit;

            txtHoTen.ReadOnly = !isEdit;
            txtSDT.ReadOnly = !isEdit;
            txtEmail.ReadOnly = !isEdit;
            txtGhiChu.ReadOnly = !isEdit;
            txtChiTieu.ReadOnly = !isEdit;
            cbbHang.Enabled = isEdit;

            btnSua.Visible = !isEdit;
            btnLuu.Visible = isEdit;
            btnXoa.Visible = !isEdit;
        }

        private void TxtChiTieu_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingHang || !_isEditMode) return;

            try
            {
                string chiTieuText = txtChiTieu.Text.Trim().Replace(",", "").Replace(".", "").Replace("₫", "").Replace(" ", "");

                if (string.IsNullOrWhiteSpace(chiTieuText))
                {
                    chiTieuText = "0";
                }

                if (decimal.TryParse(chiTieuText, out decimal chiTieu))
                {
                    string hangCode = TinhHangTheoChiTieu(chiTieu);
                    SetHangByCode(hangCode);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi tính hạng: {ex.Message}");
            }
        }

        private string TinhHangTheoChiTieu(decimal chiTieu)
        {
            if (_dtHang == null || _dtHang.Rows.Count == 0)
                return "MEM";

            var sortedHang = _dtHang.AsEnumerable()
                .OrderByDescending(r => Convert.ToInt32(r["thu_tu"]))
                .ToList();

            foreach (var row in sortedHang)
            {
                decimal minTichLuy = Convert.ToDecimal(row["min_tich_luy"]);
                if (chiTieu >= minTichLuy)
                {
                    return row["hang_code"].ToString();
                }
            }

            return "MEM";
        }

        private void SetHangByCode(string hangCode)
        {
            if (_dtHang == null || _dtHang.Rows.Count == 0)
                return;

            _isUpdatingHang = true;
            try
            {
                if (cbbHang.DataSource != null)
                {
                    cbbHang.SelectedValue = hangCode;
                }
            }
            finally
            {
                _isUpdatingHang = false;
            }
        }

        private string GetHangCodeFromSelected()
        {
            if (cbbHang.SelectedIndex < 0 || _dtHang == null)
                return "MEM";

            if (cbbHang.DataSource != null && !string.IsNullOrEmpty(cbbHang.ValueMember))
            {
                object value = cbbHang.SelectedValue;
                if (value != null)
                {
                    return value.ToString();
                }
            }

            return "MEM";
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            SetEditMode(true);
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHoTen.Focus();
                    return;
                }

                // Parse các giá trị
                string hoTen = txtHoTen.Text.Trim();
                string sdt = txtSDT.Text?.Trim() ?? "";
                string email = txtEmail.Text?.Trim() ?? "";
                string ghiChu = txtGhiChu.Text?.Trim() ?? "";
                DateTime? ngaySinh = null; // Không yêu cầu ngày sinh

                // Validation số điện thoại
                if (!string.IsNullOrWhiteSpace(sdt))
                {
                    sdt = new string(sdt.Where(char.IsDigit).ToArray());

                    if (sdt.Length != 10)
                    {
                        MessageBox.Show("Số điện thoại phải có đúng 10 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSDT.Focus();
                        return;
                    }
                }

                // Parse chi tiêu
                string chiTieuText = txtChiTieu.Text.Trim().Replace(",", "").Replace(".", "").Replace("₫", "").Replace(" ", "");
                if (string.IsNullOrWhiteSpace(chiTieuText)) chiTieuText = "0";
                if (!decimal.TryParse(chiTieuText, out decimal tongChiTieu))
                {
                    MessageBox.Show("Chi tiêu không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtChiTieu.Focus();
                    return;
                }

                // Lấy hang_code từ combobox
                string hangCode = GetHangCodeFromSelected();

                // Cập nhật khách hàng (giữ nguyên so_lan_den và diem)
                DataTable dt = _khachHangBLL.LayThongTinKhachHang(_khachHangId);
                int soLanDen = 0;
                int diem = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    soLanDen = dt.Rows[0]["so_lan_den"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["so_lan_den"]);
                    diem = dt.Rows[0]["diem"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["diem"]);
                }

                bool success = _khachHangBLL.CapNhatKhachHang(_khachHangId, hoTen, sdt, email, ghiChu, ngaySinh, hangCode, tongChiTieu, soLanDen, diem);

                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thông tin khách hàng thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa khách hàng này?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    bool success = _khachHangBLL.XoaKhachHang(_khachHangId);

                    if (success)
                    {
                        MessageBox.Show("Xóa khách hàng thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Xóa khách hàng thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

