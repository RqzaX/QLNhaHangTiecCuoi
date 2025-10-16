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

        /// <summary>
        /// Form Load - Tự động load dữ liệu chi nhánh
        /// </summary>
        private void Frm_ChonChiNhanh_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("=== Frm_ChonChiNhanh_Load gọi ===");
            try
            {
                LoadChiNhanhComboBox();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in Load: {ex.Message}");
                MessageBox.Show("Lỗi khi load form: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách chi nhánh vào ComboBox
        /// </summary>
        private void LoadChiNhanhComboBox()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("1. Bắt đầu LoadChiNhanhComboBox");

                // Lấy dữ liệu từ database
                DataTable dt = _bll.LayDanhSachChiNhanh();

                System.Diagnostics.Debug.WriteLine($"2. DataTable có {dt?.Rows.Count ?? 0} rows");

                if (dt == null || dt.Rows.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("3. DataTable null hoặc rỗng");
                    MessageBox.Show("Không có chi nhánh nào trong hệ thống!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnTiepTuc.Enabled = false;
                    return;
                }

                // In ra dữ liệu để kiểm tra
                System.Diagnostics.Debug.WriteLine("4. Dữ liệu từ DB:");
                foreach (DataRow row in dt.Rows)
                {
                    System.Diagnostics.Debug.WriteLine($"   - ID: {row["chi_nhanh_id"]}, Tên: {row["ten"]}");
                }

                // Xóa DataSource cũ nếu có
                if (cbbChonChiNhanh.DataSource != null)
                {
                    cbbChonChiNhanh.DataSource = null;
                }

                System.Diagnostics.Debug.WriteLine("5. Bind dữ liệu vào ComboBox");

                // Bind dữ liệu vào ComboBox
                cbbChonChiNhanh.DataSource = dt;
                cbbChonChiNhanh.DisplayMember = "ten";           // Hiển thị tên chi nhánh
                cbbChonChiNhanh.ValueMember = "chi_nhanh_id";    // Lưu ID chi nhánh

                System.Diagnostics.Debug.WriteLine($"6. ComboBox Items: {cbbChonChiNhanh.Items.Count}");

                // Chọn mục đầu tiên
                if (cbbChonChiNhanh.Items.Count > 0)
                {
                    cbbChonChiNhanh.SelectedIndex = 0;
                    System.Diagnostics.Debug.WriteLine($"7. Chọn SelectedIndex = 0, giá trị: {cbbChonChiNhanh.Text}");
                }

                btnTiepTuc.Enabled = true;
                System.Diagnostics.Debug.WriteLine("8. ✓ Load thành công");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.GetType().Name} - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
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
                FrmLogin frmLogin = new FrmLogin();
                frmLogin.Show();
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

                // Lấy ID và tên chi nhánh được chọn
                int chiNhanhId = (int)cbbChonChiNhanh.SelectedValue;
                string tenChiNhanh = cbbChonChiNhanh.Text;

                System.Diagnostics.Debug.WriteLine($"Chọn Chi Nhánh: ID={chiNhanhId}, Tên={tenChiNhanh}");

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