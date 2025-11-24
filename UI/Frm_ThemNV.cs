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
using UI.Common;

namespace UI
{
    public partial class Frm_ThemNV : Form
    {
        private NguoiDungBLL _nguoiDungBLL;
        private DatabaseHelper _dbHelper;

        public Frm_ThemNV()
        {
            InitializeComponent();
            try
            {
                _dbHelper = new DatabaseHelper();
                _nguoiDungBLL = new NguoiDungBLL(_dbHelper);

                // Load danh sách chức vụ vào ComboBox
                LoadChucVu();

                // Đăng ký sự kiện cho các nút
                roundedButton2.Click += BtnThemNV_Click;
                roundedButton1.Click += BtnHuy_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách chức vụ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThemNV_Click(object sender, EventArgs e)
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

                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Vui lòng chọn chi nhánh làm việc trước khi thêm nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Thêm nhân viên
                int nguoiDungId = _nguoiDungBLL.ThemNhanVien(hoTen, vaiTroId, chiNhanhId);

                if (nguoiDungId > 0)
                {
                    MessageBox.Show($"Thêm nhân viên thành công!\nMã nhân viên ID: {nguoiDungId}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Đặt DialogResult và đóng form
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể thêm nhân viên. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
