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
    public partial class Frm_ThemChiNhanh : Form
    {
        private ChiNhanhBLL _chiNhanhBLL;

        public event EventHandler ChiNhanhAdded;

        public Frm_ThemChiNhanh()
        {
            InitializeComponent();
            var dbHelper = new DatabaseHelper();
            _chiNhanhBLL = new ChiNhanhBLL(dbHelper);
            
            // Load dữ liệu cho combobox trạng thái
            LoadComboBoxTrangThai();
            
            // Thiết lập ràng buộc cho số điện thoại
            SetupPhoneNumberValidation();
            
            // Đăng ký event cho nút Lưu
            roundedButton2.Click += BtnLuu_Click;
            // Đăng ký event cho nút Hủy
            roundedButton1.Click += (s, e) => this.Close();
        }

        private void SetupPhoneNumberValidation()
        {
            // Giới hạn tối đa 10 ký tự
            roundedTextBox3.MaxLength = 10;
            
            // Đăng ký event sau khi control được load
            this.Load += (s, e) =>
            {
                // Lấy TextBox bên trong RoundedTextBox
                var textBox = roundedTextBox3.Controls.OfType<TextBox>().FirstOrDefault();
                if (textBox != null)
                {
                    // Chặn KeyPress để chỉ cho phép số
                    textBox.KeyPress += TextBox_KeyPress;
                }
            };
            
            // Validate khi text thay đổi để chỉ cho phép số và giới hạn 10 ký tự
            roundedTextBox3.TextChanged += RoundedTextBox3_TextChanged;
        }

        private void RoundedTextBox3_TextChanged(object sender, EventArgs e)
        {
            // Lấy TextBox bên trong RoundedTextBox
            var textBox = roundedTextBox3.Controls.OfType<TextBox>().FirstOrDefault();
            if (textBox != null)
            {
                // Loại bỏ các ký tự không phải số và giới hạn 10 ký tự
                string text = textBox.Text;
                string numbersOnly = new string(text.Where(char.IsDigit).Take(10).ToArray());
                
                if (text != numbersOnly)
                {
                    int selectionStart = textBox.SelectionStart;
                    textBox.Text = numbersOnly;
                    textBox.SelectionStart = Math.Min(selectionStart, textBox.Text.Length);
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số (0-9) và các phím điều khiển (Backspace, Delete, etc.)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void LoadComboBoxTrangThai()
        {
            try
            {
                // Sử dụng DataTable để bind dữ liệu vào combobox
                DataTable dt = new DataTable();
                dt.Columns.Add("Text", typeof(string));
                dt.Columns.Add("Value", typeof(int));
                
                dt.Rows.Add("Đang hoạt động", 1);
                dt.Rows.Add("Bảo trì", 0);
                
                cbbTrangThai.DataSource = dt;
                cbbTrangThai.DisplayMember = "Text";
                cbbTrangThai.ValueMember = "Value";
                
                // Mặc định chọn "Đang hoạt động"
                cbbTrangThai.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load combo box trạng thái: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                string ten = roundedTextBox2.Text.Trim();
                string diaChi = roundedTextBox1.Text.Trim();
                string sdt = roundedTextBox3.Text.Trim();

                // Validation
                if (string.IsNullOrWhiteSpace(ten))
                {
                    MessageBox.Show("Vui lòng nhập tên chi nhánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    roundedTextBox2.Focus();
                    return;
                }

                // Validation số điện thoại - BẮT BUỘC
                if (string.IsNullOrWhiteSpace(sdt))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    roundedTextBox3.Focus();
                    return;
                }

                // Loại bỏ khoảng trắng và kiểm tra
                string sdtClean = sdt.Replace(" ", "").Replace("-", "").Trim();
                
                // Kiểm tra chỉ chứa số
                if (!sdtClean.All(char.IsDigit))
                {
                    MessageBox.Show("Số điện thoại chỉ được chứa số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    roundedTextBox3.Focus();
                    return;
                }
                
                // Kiểm tra độ dài chính xác 10 số
                if (sdtClean.Length != 10)
                {
                    MessageBox.Show("Số điện thoại phải có đúng 10 số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    roundedTextBox3.Focus();
                    return;
                }
                
                // Cập nhật lại giá trị đã làm sạch
                sdt = sdtClean;

                // Lấy trạng thái từ combobox
                int trangThai = 1; // Mặc định là đang hoạt động
                if (cbbTrangThai.SelectedValue != null)
                {
                    trangThai = Convert.ToInt32(cbbTrangThai.SelectedValue);
                }
                
                // Thêm chi nhánh vào database
                int chiNhanhId = _chiNhanhBLL.ThemChiNhanh(ten, diaChi, sdt, trangThai);

                string message = $"Thêm chi nhánh thành công!\nMã chi nhánh ID: {chiNhanhId}";
                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger event để form cha reload
                ChiNhanhAdded?.Invoke(this, EventArgs.Empty);

                // Đóng form
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
