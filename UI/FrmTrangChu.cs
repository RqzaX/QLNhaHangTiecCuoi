using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiControls;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;
using UI.Common;
using UI.Controls;
using Sunny.UI;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmTrangChu : Form
    {
        private readonly Dictionary<Type, Form> _cache = new();
        private ImageList _icons;
        private NguoiDungBLL _bll;
        private DatabaseHelper _dbHelper;
        private bool _isLoggingOut = false;
        public FrmTrangChu()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bll = new NguoiDungBLL(_dbHelper);
            ShowChild<FrmDashboard>();
        }

        private void FrmTrangChu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_isLoggingOut)
            {
                Application.Exit();
            }
        }

        private async void FrmTrangChu_Load(object sender, EventArgs e)
        {
            WireNavButtons();
            var first = panel3.Controls.OfType<NavButton>().FirstOrDefault();
            if (first != null) SetSelected(first);

            // Load dữ liệu chi nhánh
            LoadChiNhanhComboBox();

            await IconPack.EnsureDownloadedAsync();
            _icons = IconPack.BuildImageListColored();
            this.components?.Add(_icons);
            void Bind(NavButton btn, string key)
            {
                btn.IconImage = _icons.Images[key];
            }
            Bind(btnDashboard, "dashboard");
            Bind(btnBanHang, "pos");
            Bind(btnDatBan, "table");
            Bind(btnDatSanh_TiecCuoi, "wedding");
            Bind(btnHopDong_Coc, "contract");
            Bind(btnKOT, "kitchen");
            Bind(btnThanhToan_HoaDon, "invoice");
            Bind(btnThucDon_Goi, "menu");
            Bind(btnKho, "warehouse");
            Bind(btnKhuyenMai_Voucher, "discount");
            Bind(btnChiNhanh_Ban_Sanh, "branch");
            Bind(btnKhachHang, "customer");
            Bind(btnNhanSu_Ca, "staff");
            Bind(btnBaoCao, "report");
            Bind(btnCauHinh, "settings");
            Bind(btnPhanQuyen, "shield");
        }
        private void WireNavButtons()
        {
            foreach (var btn in panel3.Controls.OfType<NavButton>())
            {
                btn.Click -= Nav_Click;
                btn.Click += Nav_Click;
            }
        }
        private void Nav_Click(object sender, EventArgs e)
        {
            if (sender is NavButton nb) SetSelected(nb);
        }

        private void SetSelected(NavButton selected)
        {
            foreach (var btn in panel3.Controls.OfType<NavButton>())
                btn.IsSelected = (btn == selected);
        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {
            ShowChild<FrmBanHang>();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowChild<FrmDashboard>();
        }

        private void btnDatBan_Click(object sender, EventArgs e)
        {
            ShowChild<FrmDatBan>();
        }

        private void btnThucDon_Goi_Click(object sender, EventArgs e)
        {
            ShowChild<FrmThucDonvaGoi>();
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            ShowChild<FrmKho>();
        }

        private void btnKhuyenMai_Voucher_Click(object sender, EventArgs e)
        {
            ShowChild<FrmVoucher>();

        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            ShowChild<FrmKhachHang>();
        }

        private void btnChiNhanh_Ban_Sanh_Click(object sender, EventArgs e)
        {
            ShowChild<FrmChiNhanh>();

        }

        private void btnNhanSu_Ca_Click(object sender, EventArgs e)
        {
            ShowChild<FrmNhanSuVaCa>();
        }
        private void btnDatSanh_TiecCuoi_Click(object sender, EventArgs e)
        {
            ShowChild<FrmDatSanh_TiecCuoi>();
        }

        private void btnHopDong_Coc_Click(object sender, EventArgs e)
        {
            ShowChild<FrmHopDong_Coc>();
        }
        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            ShowChild<FrmBaoCao>();
        }

        private void btnKOT_Click(object sender, EventArgs e)
        {
            ShowChild<FrmBep_Bar>();
        }

        private void btnThanhToan_HoaDon_Click(object sender, EventArgs e)
        {
            ShowChild<FrmThanhToan_HoaDon>();
        }

        private void btnPhanQuyen_Click(object sender, EventArgs e)
        {
            ShowChild<FrmPhanQuyen>();
        }

        private void FrmTrangChu_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_isLoggingOut)
            {
                Environment.Exit(0);
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn đăng xuất?",
                    "Xác nhận đăng xuất",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2
                );

                if (result != DialogResult.Yes)
                {
                    return;
                }

                _isLoggingOut = true;

                Session.NguoiDungId = 0;
                Session.TaiKhoan = "";
                Session.HoTen = "";
                Session.ChiNhanhId = 0;
                Session.TenChiNhanh = "";

                FrmLogin frmLogin = new FrmLogin();
                frmLogin.Show();

                this.Hide();
                this.Close();
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, "Lỗi khi đăng xuất: " + ex.Message, UI.Controls.ToastType.Error, 2500, UI.Controls.ToastPos.TopRight);
            }
        }

        private void ShowChild<T>() where T : Form, new()
        {
            if (!_cache.TryGetValue(typeof(T), out var form) || form.IsDisposed)
            {
                form = new T
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };
                _cache[typeof(T)] = form;
            }
            // Dọn panel và gắn form
            foreach (Control c in panelChinh.Controls) c.Hide();
            panelChinh.Controls.Clear();
            panelChinh.Controls.Add(form);
            form.BringToFront();
            form.Show();

            // Nếu form implement IFormRefreshable thì refresh dữ liệu
            if (form is IFormRefreshable refreshable)
            {
                refreshable.RefreshData();
            }
        }

        public void ShowBanHangWithTable(string soBan, string tenKhachHang, int soKhach)
        {
            try
            {
                // Tạo form bán hàng mới với thông tin bàn
                var frmBanHang = new FrmBanHang(soBan, tenKhachHang, soKhach);

                // Set properties để hiển thị trong panel chính
                frmBanHang.TopLevel = false;
                frmBanHang.FormBorderStyle = FormBorderStyle.None;
                frmBanHang.Dock = DockStyle.Fill;

                // Clear panel và add form mới
                foreach (Control c in panelChinh.Controls) c.Hide();
                panelChinh.Controls.Clear();
                panelChinh.Controls.Add(frmBanHang);
                frmBanHang.BringToFront();
                frmBanHang.Show();

                // Cập nhật cache
                _cache[typeof(FrmBanHang)] = frmBanHang;

                // Set selected button
                SetSelected(btnBanHang);
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi chuyển sang form bán hàng: {ex.Message}", UI.Controls.ToastType.Error, 2500, UI.Controls.ToastPos.TopRight);
            }
        }

        private void ResetAllChildForms()
        {
            try
            {
                foreach (var kvp in _cache.ToList())
                {
                    if (kvp.Value != null && !kvp.Value.IsDisposed)
                    {
                        kvp.Value.Dispose();
                    }
                }
                _cache.Clear();

                ResetFrmBanHang();
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi reset form: {ex.Message}", UI.Controls.ToastType.Error, 2500, UI.Controls.ToastPos.TopRight);
            }
        }

        private void ResetFrmBanHang()
        {
            try
            {
                var frmBanHang = new FrmBanHang
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };
                _cache[typeof(FrmBanHang)] = frmBanHang;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi reset FrmBanHang: {ex.Message}");
            }
        }

        private void LoadChiNhanhComboBox()
        {
            try
            {
                if (Session.NguoiDungId <= 0)
                {
                    GunaToast.Show(this, "Phiên đăng nhập không hợp lệ! Vui lòng đăng nhập lại.", UI.Controls.ToastType.Error, 2500, UI.Controls.ToastPos.TopRight);
                    cbbChonChiNhanh.Enabled = false;
                    return;
                }

                DataTable dt = _bll.LayChiNhanhTheoNguoiDung(Session.NguoiDungId);

                if (dt == null || dt.Rows.Count == 0)
                {
                    GunaToast.Show(this, "Bạn không được phân quyền truy cập chi nhánh nào!\nVui lòng liên hệ quản trị viên.", UI.Controls.ToastType.Info, 2500, UI.Controls.ToastPos.TopRight);
                    cbbChonChiNhanh.Enabled = false;
                    return;
                }

                if (cbbChonChiNhanh.DataSource != null)
                {
                    cbbChonChiNhanh.DataSource = null;
                }

                cbbChonChiNhanh.DataSource = dt;
                cbbChonChiNhanh.DisplayMember = "ten";           // Hiển thị tên chi nhánh
                cbbChonChiNhanh.ValueMember = "chi_nhanh_id";    // Lưu ID chi nhánh

                if (Session.ChiNhanhId > 0)
                {
                    cbbChonChiNhanh.SelectedValue = Session.ChiNhanhId;
                }

                cbbChonChiNhanh.SelectedIndexChanged -= ComboBox1_SelectedIndexChanged;
                cbbChonChiNhanh.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

                cbbChonChiNhanh.Enabled = true;
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, "Lỗi load chi nhánh: " + ex.Message, UI.Controls.ToastType.Error, 2500, UI.Controls.ToastPos.TopRight);
                cbbChonChiNhanh.Enabled = false;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbChonChiNhanh.SelectedValue != null)
                {
                    int chiNhanhId = (int)cbbChonChiNhanh.SelectedValue;
                    string tenChiNhanh = cbbChonChiNhanh.Text;

                    Session.ChiNhanhId = chiNhanhId;
                    Session.TenChiNhanh = tenChiNhanh;

                    ResetAllChildForms();

                    ShowChild<FrmDashboard>();

                    SetSelected(btnDashboard);

                    GunaToast.Show(this, $"Đã chuyển sang chi nhánh: {tenChiNhanh}", UI.Controls.ToastType.Success, 2500, UI.Controls.ToastPos.TopRight);
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, "Lỗi khi chuyển chi nhánh: " + ex.Message, UI.Controls.ToastType.Error, 2500, UI.Controls.ToastPos.TopRight);
            }
        }

        private void btnFormTest_Click(object sender, EventArgs e)
        {
            ShowChild<test>();
        }
    }
}
