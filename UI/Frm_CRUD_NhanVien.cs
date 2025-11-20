using System;
using System.Data;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_CRUD_NhanVien : Form
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly NguoiDungBLL _nguoiDungBLL;
        private DataTable _nhanVienSource;
        private DataTable _chucVuSource;
        private DataTable _chiNhanhSource;

        public Frm_CRUD_NhanVien()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _nguoiDungBLL = new NguoiDungBLL(_dbHelper);

            dgvNhanVien.AutoGenerateColumns = false;
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToDeleteRows = false;
            dgvNhanVien.RowHeadersVisible = false;

            Shown += Frm_CRUD_NhanVien_Shown;
        }

        private void Frm_CRUD_NhanVien_Shown(object sender, EventArgs e)
        {
            LoadComboboxSources();
            LoadNhanVienData();
        }

        private void LoadComboboxSources()
        {
            try
            {
                _chucVuSource = _nguoiDungBLL.LayDanhSachChucVu();
                cboChucVu.DataSource = _chucVuSource;
                cboChucVu.DisplayMember = "ten_chuc_vu";
                cboChucVu.ValueMember = "vai_tro_id";

                _chiNhanhSource = _nguoiDungBLL.LayDanhSachChiNhanh();
                cboChiNhanh.DataSource = _chiNhanhSource;
                cboChiNhanh.DisplayMember = "ten";
                cboChiNhanh.ValueMember = "chi_nhanh_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNhanVienData()
        {
            try
            {
                _nhanVienSource = _nguoiDungBLL.LayDanhSachNhanVien();
                dgvNhanVien.DataSource = _nhanVienSource;
                lblTongNhanVien.Text = $"{_nhanVienSource?.Rows.Count ?? 0} nhân viên";
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            if (_nhanVienSource == null) return;

            string text = txtSearch.Text.Trim().Replace("'", "''");
            var view = _nhanVienSource.DefaultView;
            if (string.IsNullOrWhiteSpace(text))
            {
                view.RowFilter = "";
            }
            else
            {
                view.RowFilter = $"TenNV LIKE '%{text}%' OR TaiKhoan LIKE '%{text}%'";
            }

            lblTongNhanVien.Text = $"{view.Count} nhân viên";
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                PopulateDetailFromRow(dgvNhanVien.Rows[e.RowIndex]);
            }
        }

        private void PopulateDetailFromRow(DataGridViewRow row)
        {
            if (row?.DataBoundItem is DataRowView view)
            {
                var data = view.Row;
                txtHoTen.Text = data["TenNV"]?.ToString();
                txtTaiKhoan.Text = data["TaiKhoan"]?.ToString();
                txtMatKhau.Text = data["MatKhau"]?.ToString() ?? string.Empty;
                toggleHoatDong.Checked = data.Field<bool>("HoatDong");

                int vaiTroId = data.Field<int>("VaiTroId");
                int chiNhanhId = data.Field<int>("ChiNhanhId");

                if (vaiTroId > 0)
                {
                    cboChucVu.SelectedValue = vaiTroId;
                }

                if (chiNhanhId > 0)
                {
                    cboChiNhanh.SelectedValue = chiNhanhId;
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearDetailForm();
            dgvNhanVien.ClearSelection();
        }

        private void ClearDetailForm()
        {
            txtHoTen.Clear();
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            toggleHoatDong.Checked = true;
            if (_chucVuSource != null && _chucVuSource.Rows.Count > 0)
            {
                cboChucVu.SelectedIndex = 0;
            }
            if (_chiNhanhSource != null && _chiNhanhSource.Rows.Count > 0)
            {
                cboChiNhanh.SelectedIndex = 0;
            }
            txtHoTen.Focus();
        }

        private bool ValidateInputs(bool isCreate, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                message = "Vui lòng nhập họ tên.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTaiKhoan.Text))
            {
                message = "Vui lòng nhập tài khoản.";
                return false;
            }
            if (txtTaiKhoan.Text.Length < 3)
            {
                message = "Tài khoản phải có ít nhất 3 ký tự.";
                return false;
            }
            if (isCreate && (string.IsNullOrWhiteSpace(txtMatKhau.Text) || txtMatKhau.Text.Length < 6))
            {
                message = "Mật khẩu phải có ít nhất 6 ký tự.";
                return false;
            }
            if (cboChucVu.SelectedValue == null)
            {
                message = "Vui lòng chọn chức vụ.";
                return false;
            }
            if (cboChiNhanh.SelectedValue == null)
            {
                message = "Vui lòng chọn chi nhánh.";
                return false;
            }
            return true;
        }

        private int? GetSelectedNhanVienId()
        {
            if (dgvNhanVien.SelectedRows.Count == 0)
                return null;

            var rowView = dgvNhanVien.SelectedRows[0].DataBoundItem as DataRowView;
            return rowView?.Row.Field<int>("NguoiDungId");
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(true, out string message))
            {
                MessageBox.Show(message, "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int vaiTroId = Convert.ToInt32(cboChucVu.SelectedValue);
                int chiNhanhId = Convert.ToInt32(cboChiNhanh.SelectedValue);

                int newId = _nguoiDungBLL.ThemNhanVien(
                    txtHoTen.Text.Trim(),
                    txtTaiKhoan.Text.Trim(),
                    txtMatKhau.Text.Trim(),
                    vaiTroId,
                    chiNhanhId,
                    toggleHoatDong.Checked);

                MessageBox.Show("Thêm nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhanVienData();
                SelectRowById(newId);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (errorMessage.Contains("Lỗi thêm nhân viên:"))
                {
                    errorMessage = errorMessage.Replace("Lỗi thêm nhân viên: ", "");
                }
                MessageBox.Show(errorMessage, "Lỗi thêm nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            var selectedId = GetSelectedNhanVienId();
            if (selectedId == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!ValidateInputs(false, out string message))
            {
                MessageBox.Show(message, "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int vaiTroId = Convert.ToInt32(cboChucVu.SelectedValue);
                int chiNhanhId = Convert.ToInt32(cboChiNhanh.SelectedValue);

                string matKhau = string.IsNullOrWhiteSpace(txtMatKhau.Text) ? null : txtMatKhau.Text.Trim();

                _nguoiDungBLL.CapNhatNhanVien(
                    selectedId.Value,
                    txtHoTen.Text.Trim(),
                    txtTaiKhoan.Text.Trim(),
                    toggleHoatDong.Checked,
                    vaiTroId,
                    chiNhanhId,
                    matKhau);

                MessageBox.Show("Cập nhật nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhanVienData();
                SelectRowById(selectedId.Value);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (errorMessage.Contains("Lỗi cập nhật nhân viên:"))
                {
                    errorMessage = errorMessage.Replace("Lỗi cập nhật nhân viên: ", "");
                }
                MessageBox.Show(errorMessage, "Lỗi cập nhật nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var selectedId = GetSelectedNhanVienId();
            if (selectedId == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn vô hiệu hóa nhân viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                _nguoiDungBLL.XoaNhanVien(selectedId.Value);
                MessageBox.Show("Đã vô hiệu hóa nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhanVienData();
                ClearDetailForm();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (errorMessage.Contains("Lỗi xóa nhân viên:"))
                {
                    errorMessage = errorMessage.Replace("Lỗi xóa nhân viên: ", "");
                }
                MessageBox.Show(errorMessage, "Lỗi xóa nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectRowById(int nguoiDungId)
        {
            foreach (DataGridViewRow row in dgvNhanVien.Rows)
            {
                if (row.DataBoundItem is DataRowView view && view.Row.Field<int>("NguoiDungId") == nguoiDungId)
                {
                    row.Selected = true;
                    dgvNhanVien.FirstDisplayedScrollingRowIndex = row.Index;
                    PopulateDetailFromRow(row);
                    break;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void panelDetail_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

