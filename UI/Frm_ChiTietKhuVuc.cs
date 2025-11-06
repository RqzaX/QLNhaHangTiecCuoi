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
    public partial class Frm_ChiTietKhuVuc : Form
    {
        private KhuVucBLL _khuVucBLL;
        private int _khuVucId;
        private bool _isEditMode = false;

        public event EventHandler KhuVucUpdated;

        public Frm_ChiTietKhuVuc(int khuVucId)
        {
            InitializeComponent();
            _khuVucId = khuVucId;
            _khuVucBLL = new KhuVucBLL();

            LoadKhuVucData();
            SetEditMode(false);

            // Đăng ký events
            btnSua.Click += BtnSua_Click;
            btnLuu.Click += BtnLuu_Click;
            btnXoa.Click += BtnXoa_Click;
            btnDong.Click += (s, e) => this.Close();
        }

        private void LoadKhuVucData()
        {
            try
            {
                DataRow row = _khuVucBLL.LayKhuVucById(_khuVucId);
                if (row == null)
                {
                    MessageBox.Show("Không tìm thấy khu vực!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtTenKhuVuc.Text = row["ten_khu_vuc"]?.ToString() ?? "";
                txtMoTa.Text = row["mo_ta"]?.ToString() ?? "";
                txtSoBan.Text = row["so_ban"]?.ToString() ?? "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void SetEditMode(bool isEdit)
        {
            _isEditMode = isEdit;

            txtTenKhuVuc.ReadOnly = !isEdit;
            txtMoTa.ReadOnly = !isEdit;
            // Số bàn luôn readonly
            txtSoBan.ReadOnly = true;

            if (isEdit)
            {
                btnSua.Text = "Hủy";
                btnSua.FillColor = Color.FromArgb(220, 53, 69); // Đỏ
                btnLuu.Visible = true;
            }
            else
            {
                btnSua.Text = "Sửa";
                btnSua.FillColor = Color.FromArgb(13, 110, 253); // Xanh
                btnLuu.Visible = false;
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                // Hủy - quay về chế độ xem
                LoadKhuVucData(); // Reload dữ liệu gốc
                SetEditMode(false);
            }
            else
            {
                // Chuyển sang chế độ sửa
                SetEditMode(true);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtTenKhuVuc.Text))
                {
                    MessageBox.Show("Tên khu vực không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenKhuVuc.Focus();
                    return;
                }

                // Lưu dữ liệu
                bool success = _khuVucBLL.CapNhatKhuVuc(
                    _khuVucId,
                    txtTenKhuVuc.Text.Trim(),
                    txtMoTa.Text?.Trim()
                );

                if (success)
                {
                    MessageBox.Show("Cập nhật khu vực thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetEditMode(false);
                    LoadKhuVucData();

                    // Trigger event để form cha reload
                    KhuVucUpdated?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật khu vực. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Xác nhận trước khi xóa
                string tenKhuVuc = txtTenKhuVuc.Text;
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa khu vực \"{tenKhuVuc}\"?\n\n" +
                    "CẢNH BÁO: Tất cả bàn trong khu vực này cũng sẽ bị xóa vĩnh viễn!\n\n" +
                    "Hành động này không thể hoàn tác!",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    // Thực hiện xóa
                    bool success = _khuVucBLL.XoaKhuVuc(_khuVucId);

                    if (success)
                    {
                        MessageBox.Show("Xóa khu vực thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Trigger event để form cha reload
                        KhuVucUpdated?.Invoke(this, EventArgs.Empty);

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa khu vực. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSoBan_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
