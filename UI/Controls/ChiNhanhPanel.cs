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

namespace UI.Controls
{
    public partial class ChiNhanhPanel : UserControl
    {
        private ChiNhanhBLL _chiNhanhBLL;
        private int _chiNhanhId;

        public event EventHandler ChiNhanhUpdated;

        public ChiNhanhPanel()
        {
            InitializeComponent();
            var dbHelper = new DatabaseHelper();
            _chiNhanhBLL = new ChiNhanhBLL(dbHelper);
            
            // Đăng ký event cho nút xóa
            btnXoa.Click += BtnXoa_Click;
        }

        public void LoadChiNhanh(int chiNhanhId, string ten, string diaChi, string sdt, int trangThai)
        {
            _chiNhanhId = chiNhanhId;
            label1.Text = ten;
            lbldiaChi.Text = diaChi ?? "";
            lblSDT.Text = sdt ?? "";

            // Cập nhật trạng thái
            if (trangThai == 1)
            {
                uiPanel1.Text = "Hoạt động";
                uiPanel1.FillColor = Color.FromArgb(245, 251, 241);
                uiPanel1.FillColor2 = Color.FromArgb(245, 251, 241);
                uiPanel1.RectColor = Color.FromArgb(110, 190, 40);
            }
            else
            {
                uiPanel1.Text = "Ngừng hoạt động";
                uiPanel1.FillColor = Color.FromArgb(255, 240, 240);
                uiPanel1.FillColor2 = Color.FromArgb(255, 240, 240);
                uiPanel1.RectColor = Color.Red;
            }

            // Cập nhật mã chi nhánh
            uiPanel2.Text = $"CN-{chiNhanhId}";

            // Load thống kê
            LoadThongKe();
        }

        private void LoadThongKe()
        {
            try
            {
                int soBan = _chiNhanhBLL.DemSoBan(_chiNhanhId);
                int soSanh = _chiNhanhBLL.DemSoSanh(_chiNhanhId);
                int soNhanVien = _chiNhanhBLL.DemSoNhanVien(_chiNhanhId);

                lblSoBan.Text = soBan.ToString();
                lblSanh.Text = soSanh.ToString();
                lblNhanVien.Text = soNhanVien.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_ChiTietChiNhanh(_chiNhanhId))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ChiNhanhUpdated += (s, args) =>
                {
                    // Trigger event để form cha reload
                    ChiNhanhUpdated?.Invoke(this, EventArgs.Empty);
                };
                f.ShowDialog(this);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Xác nhận trước khi xóa
                string tenChiNhanh = label1.Text;
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa chi nhánh \"{tenChiNhanh}\"?\n\n" +
                    "CẢNH BÁO: Tất cả dữ liệu liên quan đến chi nhánh này sẽ bị xóa vĩnh viễn!\n" +
                    "(Bao gồm: Khu vực, Bàn, Sảnh, Đặt bàn, Hóa đơn, Tồn kho, v.v.)\n\n" +
                    "Hành động này không thể hoàn tác!",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    // Thực hiện xóa (hard delete - xóa vĩnh viễn khỏi database)
                    bool success = _chiNhanhBLL.XoaChiNhanh(_chiNhanhId);

                    if (success)
                    {
                        MessageBox.Show("Xóa chi nhánh thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Trigger event để form cha reload
                        ChiNhanhUpdated?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa chi nhánh. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public int ChiNhanhId => _chiNhanhId;
    }
}
