using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using QLNhaHangTiecCuoi.Share;

namespace UI
{
    public partial class Frm_ChiTietChiNhanh : Form
    {
        private ChiNhanhBLL _chiNhanhBLL;
        private int _chiNhanhId;
        private bool _isEditMode = false;

        public event EventHandler ChiNhanhUpdated;

        public Frm_ChiTietChiNhanh(int chiNhanhId)
        {
            InitializeComponent();
            _chiNhanhId = chiNhanhId;
            var dbHelper = new DatabaseHelper();
            _chiNhanhBLL = new ChiNhanhBLL(dbHelper);
            
            LoadComboBoxTrangThai();
            LoadChiNhanhData();
            SetEditMode(false);
            SetupPhoneNumberValidation();
            
            // Đăng ký events
            btnSua.Click += BtnSua_Click;
            btnDong.Click += (s, e) => this.Close();
        }

        private void SetupPhoneNumberValidation()
        {
            // Giới hạn tối đa 10 ký tự
            guna2TextBox1.MaxLength = 10;
            
            // Đăng ký event sau khi control được load
            this.Load += (s, e) =>
            {
                // Lấy TextBox bên trong Guna2TextBox (nếu có)
                var textBox = guna2TextBox1.Controls.OfType<TextBox>().FirstOrDefault();
                if (textBox != null)
                {
                    textBox.KeyPress += TextBox_KeyPress;
                }
            };
            
            // Validate khi text thay đổi
            guna2TextBox1.TextChanged += Guna2TextBox1_TextChanged;
        }

        private void Guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                // Chỉ validate khi ở chế độ edit
                string text = guna2TextBox1.Text;
                string numbersOnly = new string(text.Where(char.IsDigit).Take(10).ToArray());
                
                if (text != numbersOnly)
                {
                    int selectionStart = guna2TextBox1.SelectionStart;
                    guna2TextBox1.Text = numbersOnly;
                    guna2TextBox1.SelectionStart = Math.Min(selectionStart, guna2TextBox1.Text.Length);
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_isEditMode)
            {
                // Chỉ chặn khi ở chế độ edit
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void LoadComboBoxTrangThai()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Text", typeof(string));
                dt.Columns.Add("Value", typeof(int));
                
                dt.Rows.Add("Đang hoạt động", 1);
                dt.Rows.Add("Bảo trì", 0);
                
                guna2ComboBox1.DataSource = dt;
                guna2ComboBox1.DisplayMember = "Text";
                guna2ComboBox1.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load combo box trạng thái: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiNhanhData()
        {
            try
            {
                DataTable dt = _chiNhanhBLL.LayChiNhanhById(_chiNhanhId);
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    
                    txtTenChiNhanh.Text = row["ten"]?.ToString() ?? "";
                    txtMaChiNhanh.Text = $"CN-{_chiNhanhId}"; // Mã chi nhánh từ ID
                    txtDiaChi.Text = row["dia_chi"]?.ToString() ?? "";
                    guna2TextBox1.Text = row["sdt"]?.ToString() ?? "";
                    
                    int trangThai = Convert.ToInt32(row["trang_thai"]);
                    guna2ComboBox1.SelectedValue = trangThai;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEditMode(bool isEdit)
        {
            _isEditMode = isEdit;
            
            // Enable/Disable các control
            txtTenChiNhanh.ReadOnly = !isEdit;
            txtMaChiNhanh.ReadOnly = true; // Mã chi nhánh không cho sửa
            txtDiaChi.ReadOnly = !isEdit;
            guna2TextBox1.ReadOnly = !isEdit;
            guna2ComboBox1.Enabled = isEdit;
            
            // Thay đổi nút Sửa/Lưu
            if (isEdit)
            {
                btnSua.Text = "Lưu";
                btnSua.FillColor = Color.FromArgb(0, 192, 0); // Màu xanh lá
            }
            else
            {
                btnSua.Text = "Sửa";
                btnSua.FillColor = Color.FromArgb(31, 111, 235); // Màu xanh dương
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (!_isEditMode)
            {
                // Chuyển sang chế độ sửa
                SetEditMode(true);
            }
            else
            {
                // Lưu dữ liệu
                SaveChiNhanhData();
            }
        }

        private void SaveChiNhanhData()
        {
            try
            {
                string ten = txtTenChiNhanh.Text.Trim();
                string diaChi = txtDiaChi.Text.Trim();
                string sdt = guna2TextBox1.Text.Trim();

                // Validation
                if (string.IsNullOrWhiteSpace(ten))
                {
                    MessageBox.Show("Vui lòng nhập tên chi nhánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenChiNhanh.Focus();
                    return;
                }

                // Validation số điện thoại - BẮT BUỘC
                if (string.IsNullOrWhiteSpace(sdt))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    guna2TextBox1.Focus();
                    return;
                }

                // Loại bỏ khoảng trắng và kiểm tra
                string sdtClean = sdt.Replace(" ", "").Replace("-", "").Trim();
                
                // Kiểm tra chỉ chứa số
                if (!sdtClean.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại chỉ được chứa số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    guna2TextBox1.Focus();
                    return;
                }
                
                // Kiểm tra độ dài chính xác 10 số
                if (sdtClean.Length != 10)
                {
                    MessageBox.Show("Số điện thoại phải có đúng 10 số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    guna2TextBox1.Focus();
                    return;
                }
                
                // Cập nhật lại giá trị đã làm sạch
                sdt = sdtClean;

                // Lấy trạng thái
                int trangThai = 1;
                if (guna2ComboBox1.SelectedValue != null)
                {
                    trangThai = Convert.ToInt32(guna2ComboBox1.SelectedValue);
                }

                // Cập nhật vào database
                _chiNhanhBLL.CapNhatChiNhanh(_chiNhanhId, ten, diaChi, sdt, trangThai);

                MessageBox.Show("Cập nhật chi nhánh thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Chuyển về chế độ xem
                SetEditMode(false);
                
                // Reload dữ liệu để đảm bảo đồng bộ
                LoadChiNhanhData();
                
                // Trigger event để form cha reload
                ChiNhanhUpdated?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
