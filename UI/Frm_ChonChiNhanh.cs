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
using Microsoft.Win32;

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
                if (Session.NguoiDungId <= 0)
                {
                    MessageBox.Show("Phiên đăng nhập không hợp lệ! Vui lòng đăng nhập lại.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnTiepTuc.Enabled = false;
                    return;
                }

                DataTable dt = _bll.LayChiNhanhTheoNguoiDung(Session.NguoiDungId);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Bạn không được phân quyền truy cập chi nhánh nào!\nVui lòng liên hệ quản trị viên.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnTiepTuc.Enabled = false;
                    return;
                }

                // Sắp xếp theo chi_nhanh_id tăng dần
                var dv = dt.DefaultView;
                dv.Sort = "chi_nhanh_id ASC";
                var sorted = dv.ToTable();

                if (cbbChonChiNhanh.DataSource != null)
                {
                    cbbChonChiNhanh.DataSource = null;
                }

                cbbChonChiNhanh.DataSource = sorted;
                cbbChonChiNhanh.DisplayMember = "ten";           // Hiển thị tên chi nhánh
                cbbChonChiNhanh.ValueMember = "chi_nhanh_id";    // Lưu ID chi nhánh

                // Chọn lại chi nhánh gần nhất nếu có
                int lastId = LoadLastBranchId();
                if (lastId > 0)
                {
                    var found = sorted.AsEnumerable().Any(r => r.Field<int>("chi_nhanh_id") == lastId);
                    if (found)
                    {
                        cbbChonChiNhanh.SelectedValue = lastId;
                    }
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
                this.DialogResult = DialogResult.Cancel;
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

                Session.ChiNhanhId = chiNhanhId;
                Session.TenChiNhanh = tenChiNhanh;

                // Lưu lại lựa chọn gần nhất
                SaveLastBranchId(chiNhanhId);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private const string RegistryKeyPath = @"Software\\QLNhaHangTiecCuoi";
        private const string RegistryBranchValue = "LastBranchId";
        // Lấy ID chi nhánh gần nhất
        private static int LoadLastBranchId()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
                {
                    if (key == null) return 0;
                    object v = key.GetValue(RegistryBranchValue);
                    return v == null ? 0 : Convert.ToInt32(v);
                }
            }
            catch { return 0; }
        }
        // Lưu ID chi nhánh gần nhất
        private static void SaveLastBranchId(int chiNhanhId)
        {
            try
            {
                using (var key = Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
                {
                    key?.SetValue(RegistryBranchValue, chiNhanhId, RegistryValueKind.DWord);
                }
            }
            catch { /* ignore */ }
        }
    }
}