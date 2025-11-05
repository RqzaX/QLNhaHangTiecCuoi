using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Common;
using UiControls;
using static System.Collections.Specialized.BitVector32;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmLogin : Form
    {
        private NguoiDungBLL _bll;
        private DatabaseHelper _dbHelper;

        public FrmLogin()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bll = new NguoiDungBLL(_dbHelper);
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            _bll = null;
            _dbHelper = null;
            Application.Exit();
            Environment.Exit(0);
        }

        private void btnDangNhap_Click_1(object sender, EventArgs e)
        {
            string taiKhoan = txtTaiKhoan.Text.Trim();
            string matKhau = txtMatKhau.Text;

            var (success, message, nguoiDungId, hoTen) = _bll.XacThucDangNhap(taiKhoan, matKhau);

            if (!success)
            {
                MessageBox.Show(message, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Clear();
                txtMatKhau.Focus();
                return;
            }

            if (cbLuuThongTin.Checked)
            {
                SaveCredentials(taiKhoan, matKhau);
            }
            else
            {
                ClearSavedCredentials();
            }

            // Lưu session
            Session.NguoiDungId = nguoiDungId;
            Session.TaiKhoan = taiKhoan;
            Session.HoTen = hoTen;

            // Mở form chọn chi nhánh
            Frm_ChonChiNhanh frmChonChiNhanh = new Frm_ChonChiNhanh();
            if (frmChonChiNhanh.ShowDialog() == DialogResult.OK)
            {
                FrmTrangChu frmTrangChu = new FrmTrangChu();
                frmTrangChu.Show();
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            if (!_dbHelper.TestConnection())
            {
                MessageBox.Show("Không thể kết nối đến database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            txtTaiKhoan.Focus();

            LoadSavedCredentials();
        }
        private void SaveCredentials(string taiKhoan, string matKhau)
        {
            CredentialsHelper.SaveCredentials(taiKhoan, matKhau, true);
        }
        private void ClearSavedCredentials()
        {
            CredentialsHelper.ClearCredentials();
        }
        private void LoadSavedCredentials()
        {
            var (found, taiKhoan, matKhau) = CredentialsHelper.LoadCredentials();

            if (found)
            {
                txtTaiKhoan.Text = taiKhoan;
                txtMatKhau.Text = matKhau;
                cbLuuThongTin.Checked = true;
            }
        }
        // Nhấn enter để đăng nhập
        private void txtMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnDangNhap_Click_1(null, null);
                e.Handled = true;
            }
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {
            // text changed
        }

        private void parrotButton1_Click(object sender, EventArgs e)
        {

        }
    }
}