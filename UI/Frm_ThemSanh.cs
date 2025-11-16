using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using Guna.UI2.WinForms;
using System.ComponentModel;

namespace UI
{
    public partial class Frm_ThemSanh : Form
    {
        private readonly int _chiNhanhId;
        private QLNhaHangTiecCuoi.BLL.SanhBLL _sanhBLL;
        public event EventHandler SanhAdded;
        public Frm_ThemSanh() : this(-1) { }

         public Frm_ThemSanh(int chiNhanhId)
        {
            InitializeComponent();              

            _chiNhanhId = chiNhanhId;

          
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                BuildUI();      
                WireEvents();   
                InitBusiness(); // khởi tạo BLL/DB (nếu có)
                LoadData();     // nạp dữ liệu ban đầu
            }
        }

        
        private void BuildUI()
        {
            // Ví dụ tinh chỉnh thêm:
            this.StartPosition = FormStartPosition.CenterParent;

            txtTenSanh.PlaceholderText = "Nhập tên sảnh";
            numSucChua.UpDownButtonFillColor = System.Drawing.Color.Silver;

            txtPhiThue.PlaceholderText = "0";
            txtPhiThue.Text = string.Empty;

           
            if (_chiNhanhId > 0)
            {
                
            }
        }

    
        private void WireEvents()
        {
            btnLuu.Click += BtnLuu_Click;
            btnThoat.Click += (s, e) => this.Close();
        }

       
        private void InitBusiness()
        {
            var dbHelper = new DatabaseHelper();
            _sanhBLL = new QLNhaHangTiecCuoi.BLL.SanhBLL(dbHelper);
        }

        /// <summary>Nạp dữ liệu combobox, v.v…</summary>
        private void LoadData()
        {
            // Không cần nạp dữ liệu vì chi nhánh đã được truyền vào constructor
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Validate cơ bản
            if (string.IsNullOrWhiteSpace(txtTenSanh.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên sảnh.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSanh.Focus();
                return;
            }

            if (numSucChua.Value <= 0)
            {
                MessageBox.Show("Sức chứa phải > 0.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numSucChua.Focus();
                return;
            }

            decimal phiThue = 0;
            if (!string.IsNullOrWhiteSpace(txtPhiThue.Text))
            {
                // Xử lý định dạng số Việt Nam (dấu chấm là phân cách hàng nghìn)
                string phiThueText = txtPhiThue.Text.Trim();
                // Loại bỏ dấu chấm (phân cách hàng nghìn) nhưng giữ dấu phẩy (phân cách thập phân)
                phiThueText = phiThueText.Replace(".", "");
                // Thay dấu phẩy thành dấu chấm cho định dạng số quốc tế
                phiThueText = phiThueText.Replace(",", ".");
                
                if (!decimal.TryParse(phiThueText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out phiThue) || phiThue < 0)
                {
                    MessageBox.Show("Phí thuê cơ bản không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhiThue.Focus();
                    return;
                }
            }

            if (_chiNhanhId <= 0)
            {
                MessageBox.Show("Chi nhánh không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Gọi BLL lưu sảnh
                int sanhId = _sanhBLL.ThemSanh(
                    _chiNhanhId,
                    txtTenSanh.Text.Trim(),
                    (int)numSucChua.Value,
                    phiThue
                );

                if (sanhId > 0)
                {
                    MessageBox.Show("Đã lưu sảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SanhAdded?.Invoke(this, EventArgs.Empty);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể lưu sảnh. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
