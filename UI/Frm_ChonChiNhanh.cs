using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;
using UI.Common;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_ChonChiNhanh : Form
    {
        private NguoiDungBLL _bll;
        private DatabaseHelper _dbHelper;

        public Frm_ChonChiNhanh()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bll = new NguoiDungBLL(_dbHelper);
        }
        private void Frm_ChonChiNhanh_Load(object sender, EventArgs e)
        {
            try
            {
                LoadChiNhanhComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load form: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiNhanhComboBox()
        {
            try
            {

                // Lấy dữ liệu từ database
                DataTable dt = _bll.LayDanhSachChiNhanh();


                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có chi nhánh nào trong hệ thống!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnTiepTuc.Enabled = false;
                    return;
                }

                // Xóa DataSource cũ nếu có
                if (cbbChonChiNhanh.DataSource != null)
                {
                    cbbChonChiNhanh.DataSource = null;
                }


                cbbChonChiNhanh.DataSource = dt;
                cbbChonChiNhanh.DisplayMember = "ten";           // Hiển thị tên chi nhánh
                cbbChonChiNhanh.ValueMember = "chi_nhanh_id";    // Lưu ID chi nhánh

                // Chọn mục đầu tiên
                if (cbbChonChiNhanh.Items.Count > 0)
                {
                    cbbChonChiNhanh.SelectedIndex = 0;
                }

                btnTiepTuc.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load chi nhánh: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnTiepTuc.Enabled = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnTiepTuc_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbbChonChiNhanh.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn một chi nhánh!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int chiNhanhId = (int)cbbChonChiNhanh.SelectedValue;
                string tenChiNhanh = cbbChonChiNhanh.Text;

                // Lưu vào Session
                Session.ChiNhanhId = chiNhanhId;
                Session.TenChiNhanh = tenChiNhanh;

                // Đóng form và mở Dashboard
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}