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
    public partial class Frm_ThemKhuVuc : Form
    {
        private readonly int _chiNhanhId;
        private KhuVucBLL _khuVucBLL;

        public event EventHandler KhuVucAdded;

    
        public Frm_ThemKhuVuc() : this(-1) { }

   
        public Frm_ThemKhuVuc(int chiNhanhId)
        {
            InitializeComponent();

            _chiNhanhId = chiNhanhId;

          
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                _khuVucBLL = new KhuVucBLL();
                WireEvents();
                // LoadData(); // nếu sau này cần nạp dữ liệu
            }
        }

        private void WireEvents()
        {
            btnLuu.Click += BtnLuu_Click;
            btnDong.Click += (s, e) => this.Close();
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenKhuVuc.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khu vực!", "Thiếu thông tin",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenKhuVuc.Focus();
                    return;
                }

                // Tránh chạy khi mở bằng Designer
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

                int id = _khuVucBLL.ThemKhuVuc(
                    _chiNhanhId,
                    txtTenKhuVuc.Text.Trim(),
                    txtMoTa.Text?.Trim()
                );

                if (id > 0)
                {
                    MessageBox.Show("Thêm khu vực thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    KhuVucAdded?.Invoke(this, EventArgs.Empty);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể thêm khu vực!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm khu vực: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
