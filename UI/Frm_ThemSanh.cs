using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using Guna.UI2.WinForms;
using System.ComponentModel;
using System.ComponentModel;

namespace UI
{
    public partial class Frm_ThemSanh : Form
    {
        private readonly int _chiNhanhId;
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
            // var db = new DatabaseHelper(/* ... */);
            // _sanhBLL = new SanhBLL(db);
        }

        /// <summary>Nạp dữ liệu combobox, v.v…</summary>
        private void LoadData()
        {
            // Ví dụ: nạp danh sách chi nhánh
            // var list = _sanhBLL.GetChiNhanh();
            // cboChiNhanh.DataSource = list;
            // cboChiNhanh.DisplayMember = "TenChiNhanh";
            // cboChiNhanh.ValueMember = "ChiNhanhId";
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

            // decimal phi;
            // if (!decimal.TryParse(txtPhiThue.Text.Trim(), out phi) || phi < 0)
            // {
            //     MessageBox.Show("Phí thuê cơ bản không hợp lệ.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //     txtPhiThue.Focus();
            //     return;
            // }

            // var chiNhanhId = (int)(cboChiNhanh.SelectedValue ?? 0);

            // TODO: Gọi BLL lưu
            // _sanhBLL.ThemSanh(new SanhCreateDto {
            //     ChiNhanhId = chiNhanhId,
            //     TenSanh = txtTenSanh.Text.Trim(),
            //     SucChua = (int)numSucChua.Value,
            //     PhiThueCoBan = phi
            // });

            MessageBox.Show("Đã lưu sảnh (demo). Hãy nối BLL/DB thực tế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
