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
using QLNhaHangTiecCuoi.Share;

namespace UI
{
    public partial class Frm_SuaXoaNV : Form
    {
        private NguoiDungBLL _nguoiDungBLL;
        private DatabaseHelper _dbHelper;
        private int _nguoiDungId;

        public Frm_SuaXoaNV(int nguoiDungId, NguoiDungBLL nguoiDungBLL)
        {
            InitializeComponent();
            _nguoiDungId = nguoiDungId;
            _nguoiDungBLL = nguoiDungBLL;
            _dbHelper = new DatabaseHelper();

            // Load thông tin nhân viên
            LoadThongTinNhanVien();

            // Load danh sách chức vụ vào ComboBox
            LoadChucVu();

             // Load danh sách chi nhánh vào ComboBox
            LoadChiNhanh();

            // Đăng ký sự kiện cho các nút
            roundedButton2.Click += BtnLuu_Click;
            roundedButton3.Click += BtnXoa_Click;
            roundedButton1.Click += BtnHuy_Click;
        }

        /// <summary>
        /// Load thông tin nhân viên vào form
        /// </summary>
        private void LoadThongTinNhanVien()
        {
            try
            {
                if (_nguoiDungBLL == null)
                    return;

                DataTable dt = _nguoiDungBLL.LayThongTinNhanVien(_nguoiDungId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    roundedTextBox1.Text = row["ho_ten"]?.ToString() ?? "";
                    label5.Text = $"Tài khoản: {row["tai_khoan"]?.ToString() ?? ""}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load thông tin nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách chức vụ vào ComboBox
        /// </summary>
        private void LoadChucVu()
        {
            try
            {
                if (_nguoiDungBLL == null)
                    return;

                roundedComboBox1.Items.Clear();

                DataTable dt = _nguoiDungBLL.LayDanhSachChucVu();
                if (dt != null && dt.Rows.Count > 0)
                {
                    // Tạo DataTable để bind vào ComboBox với DisplayMember và ValueMember
                    DataTable comboDt = new DataTable();
                    comboDt.Columns.Add("Display", typeof(string));
                    comboDt.Columns.Add("Value", typeof(int));

                    int currentVaiTroId = 0;

                    // Lấy vai trò hiện tại của nhân viên
                    DataTable dtNhanVien = _nguoiDungBLL.LayThongTinNhanVien(_nguoiDungId);
                    if (dtNhanVien != null && dtNhanVien.Rows.Count > 0)
                    {
                        object vaiTroIdObj = dtNhanVien.Rows[0]["vai_tro_id"];
                        if (vaiTroIdObj != null && vaiTroIdObj != DBNull.Value)
                        {
                            currentVaiTroId = Convert.ToInt32(vaiTroIdObj);
                        }
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        string tenChucVu = row["ten_chuc_vu"]?.ToString() ?? "";
                        int vaiTroId = Convert.ToInt32(row["vai_tro_id"]);
                        if (!string.IsNullOrEmpty(tenChucVu))
                        {
                            comboDt.Rows.Add(tenChucVu, vaiTroId);
                        }
                    }

                    roundedComboBox1.DataSource = comboDt;
                    roundedComboBox1.DisplayMember = "Display";
                    roundedComboBox1.ValueMember = "Value";

                    // Chọn chức vụ hiện tại
                    if (currentVaiTroId > 0)
                    {
                        for (int i = 0; i < roundedComboBox1.Items.Count; i++)
                        {
                            DataRowView drv = (DataRowView)roundedComboBox1.Items[i];
                            if (Convert.ToInt32(drv["Value"]) == currentVaiTroId)
                            {
                                roundedComboBox1.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách chức vụ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiNhanh()
        {
            try
            {
                if (_nguoiDungBLL == null)
                    return;

                roundedComboBoxChiNhanh.Items.Clear();

                DataTable dt = _nguoiDungBLL.LayDanhSachChiNhanh();

                int currentChiNhanhId = 0;
                var dtChiNhanhNguoiDung = _nguoiDungBLL.LayChiNhanhTheoNguoiDung(_nguoiDungId);
                if (dtChiNhanhNguoiDung != null && dtChiNhanhNguoiDung.Rows.Count > 0)
                {
                    object chiNhanhIdObj = dtChiNhanhNguoiDung.Rows[0]["chi_nhanh_id"];
                    if (chiNhanhIdObj != null && chiNhanhIdObj != DBNull.Value)
                    {
                        currentChiNhanhId = Convert.ToInt32(chiNhanhIdObj);
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    roundedComboBoxChiNhanh.DataSource = dt;
                    roundedComboBoxChiNhanh.DisplayMember = "ten";
                    roundedComboBoxChiNhanh.ValueMember = "chi_nhanh_id";

                    if (currentChiNhanhId > 0)
                    {
                        roundedComboBoxChiNhanh.SelectedValue = currentChiNhanhId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách chi nhánh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ form
                string hoTen = roundedTextBox1.Text?.Trim() ?? "";

                // Validation
                if (string.IsNullOrWhiteSpace(hoTen))
                {
                    MessageBox.Show("Vui lòng nhập họ và tên nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    roundedTextBox1.Focus();
                    return;
                }

                if (roundedComboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn chức vụ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    roundedComboBox1.Focus();
                    return;
                }

                int vaiTroId = Convert.ToInt32(roundedComboBox1.SelectedValue);

                // Cập nhật nhân viên
                bool success = _nguoiDungBLL.CapNhatNhanVien(_nguoiDungId, hoTen, vaiTroId);

                if (success)
                {
                    // Gán chi nhánh mới nếu có chọn
                    if (roundedComboBoxChiNhanh.SelectedValue != null)
                    {
                        int chiNhanhId = Convert.ToInt32(roundedComboBoxChiNhanh.SelectedValue);
                        if (chiNhanhId > 0)
                        {
                            _nguoiDungBLL.GanChiNhanhChoNguoiDung(_nguoiDungId, chiNhanhId);
                        }
                    }

                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Đặt DialogResult và đóng form
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật nhân viên. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy tên nhân viên để hiển thị trong xác nhận
                string hoTen = roundedTextBox1.Text?.Trim() ?? "nhân viên này";

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa nhân viên \"{hoTen}\"?\n\n" +
                    "Lưu ý: Nhân viên sẽ bị vô hiệu hóa (không thể đăng nhập) nhưng dữ liệu vẫn được lưu trữ.",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Xóa nhân viên
                    bool success = _nguoiDungBLL.XoaNhanVien(_nguoiDungId);

                    if (success)
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Đặt DialogResult và đóng form
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa nhân viên. Vui lòng thử lại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

