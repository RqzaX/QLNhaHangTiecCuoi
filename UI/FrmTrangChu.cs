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

            // Áp dụng phân quyền
            ApplyPermissions();

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
            CheckPermissionBeforeOpen<FrmBanHang>(btnBanHang, new[] { "LETAN_THUNGAN", "QLCN", "ADMIN" });
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ShowChild<FrmDashboard>();
        }

        private void btnDatBan_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmDatBan>(btnDatBan, new[] { "LETAN_THUNGAN", "QLCN", "ADMIN" });
        }

        private void btnThucDon_Goi_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmThucDonvaGoi>(btnThucDon_Goi, new[] { "QLCN", "ADMIN" });
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmKho>(btnKho, new[] { "QLKHO", "ADMIN" });
        }

        private void btnKhuyenMai_Voucher_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmChuongTrinhKM>(btnKhuyenMai_Voucher, new[] { "QLCN", "ADMIN" });
        }

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            ShowChild<FrmKhachHang>(); // Tất cả đều được xem
        }

        private void btnChiNhanh_Ban_Sanh_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmChiNhanh>(btnChiNhanh_Ban_Sanh, new[] { "QLCN", "ADMIN" });
        }

        private void btnNhanSu_Ca_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmNhanSuVaCa>(btnNhanSu_Ca, new[] { "QLCN", "ADMIN" });
        }
        private void btnDatSanh_TiecCuoi_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmDatSanh_TiecCuoi>(btnDatSanh_TiecCuoi, new[] { "LETAN_THUNGAN", "QLCN", "ADMIN" });
        }

        private void btnHopDong_Coc_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmHopDong_Coc>(btnHopDong_Coc, new[] { "LETAN_THUNGAN", "QLCN", "ADMIN" });
        }
        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmBaoCao>(btnBaoCao, new[] { "QLCN", "ADMIN" });
        }

        private void btnKOT_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmBep_Bar>(btnKOT, new[] { "QLBEP", "ADMIN" });
        }

        private void btnThanhToan_HoaDon_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmThanhToan_HoaDon>(btnThanhToan_HoaDon, new[] { "LETAN_THUNGAN", "QLCN", "ADMIN" });
        }

        private void btnPhanQuyen_Click(object sender, EventArgs e)
        {
            CheckPermissionBeforeOpen<FrmPhanQuyen>(btnPhanQuyen, new[] { "ADMIN" });
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

        public void ShowChild<T>() where T : Form, new()
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

            // Dọn panel và gắn form - dispose controls properly to avoid ObjectDisposedException
            try
            {
                // Hide and dispose existing controls properly
                var controlsToRemove = new List<Control>();
                foreach (Control c in panelChinh.Controls)
                {
                    try
                    {
                        if (!c.IsDisposed)
                        {
                            c.Hide();
                            controlsToRemove.Add(c);
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // Control already disposed, skip it
                        continue;
                    }
                }

                // Remove controls from panel first
                foreach (var c in controlsToRemove)
                {
                    try
                    {
                        if (!c.IsDisposed && panelChinh.Controls.Contains(c))
                        {
                            panelChinh.Controls.Remove(c);
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // Already disposed, continue
                        continue;
                    }
                }

                // Dispose removed controls
                foreach (var c in controlsToRemove)
                {
                    try
                    {
                        if (!c.IsDisposed)
                        {
                            c.Dispose();
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // Already disposed, continue
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but continue
                System.Diagnostics.Debug.WriteLine($"Error clearing panel: {ex.Message}");
            }

            // Add new form
            try
            {
                if (!form.IsDisposed)
                {
                    panelChinh.Controls.Add(form);
                    form.BringToFront();
                    form.Show();
                }
            }
            catch (ObjectDisposedException)
            {
                // Form was disposed, create a new one
                form = new T
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };
                _cache[typeof(T)] = form;
                panelChinh.Controls.Add(form);
                form.BringToFront();
                form.Show();
            }

            // Nếu form implement IFormRefreshable thì refresh dữ liệu
            if (form is IFormRefreshable refreshable && !form.IsDisposed)
            {
                try
                {
                    refreshable.RefreshData();
                }
                catch (ObjectDisposedException)
                {
                    // Form or its controls were disposed, skip refresh
                }
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

                // Clear panel và add form mới - dispose controls properly
                try
                {
                    var controlsToRemove = new List<Control>();
                    foreach (Control c in panelChinh.Controls)
                    {
                        try
                        {
                            if (!c.IsDisposed)
                            {
                                c.Hide();
                                controlsToRemove.Add(c);
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            continue;
                        }
                    }

                    foreach (var c in controlsToRemove)
                    {
                        try
                        {
                            if (!c.IsDisposed && panelChinh.Controls.Contains(c))
                            {
                                panelChinh.Controls.Remove(c);
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            continue;
                        }
                    }

                    foreach (var c in controlsToRemove)
                    {
                        try
                        {
                            if (!c.IsDisposed)
                            {
                                c.Dispose();
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error clearing panel: {ex.Message}");
                }

                // Add new form
                if (!frmBanHang.IsDisposed)
                {
                    panelChinh.Controls.Add(frmBanHang);
                    frmBanHang.BringToFront();
                    frmBanHang.Show();
                }

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
                // Clear panel controls first
                try
                {
                    var controlsToRemove = new List<Control>();
                    foreach (Control c in panelChinh.Controls)
                    {
                        try
                        {
                            if (!c.IsDisposed)
                            {
                                controlsToRemove.Add(c);
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            continue;
                        }
                    }

                    foreach (var c in controlsToRemove)
                    {
                        try
                        {
                            if (!c.IsDisposed && panelChinh.Controls.Contains(c))
                            {
                                panelChinh.Controls.Remove(c);
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            continue;
                        }
                    }

                    foreach (var c in controlsToRemove)
                    {
                        try
                        {
                            if (!c.IsDisposed)
                            {
                                c.Dispose();
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error clearing panel in ResetAllChildForms: {ex.Message}");
                }

                // Dispose cached forms
                foreach (var kvp in _cache.ToList())
                {
                    try
                    {
                        if (kvp.Value != null && !kvp.Value.IsDisposed)
                        {
                            kvp.Value.Dispose();
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // Already disposed, continue
                        continue;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error disposing form {kvp.Key}: {ex.Message}");
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

        private void ApplyPermissions()
        {
            // ADMIN có toàn quyền, không cần kiểm tra
            if (Session.HasRole("ADMIN"))
            {
                return;
            }

            // Phân quyền cho từng nút dựa trên vai trò
            // Dashboard: Tất cả đều được xem

            // Bán hàng (POS): LETAN_THUNGAN, QLCN
            btnBanHang.Enabled = Session.HasAnyRole("LETAN_THUNGAN", "QLCN");

            // Đặt bàn: LETAN_THUNGAN, QLCN
            btnDatBan.Enabled = Session.HasAnyRole("LETAN_THUNGAN", "QLCN");

            // Đặt sảnh/Tiệc cưới: LETAN_THUNGAN, QLCN
            btnDatSanh_TiecCuoi.Enabled = Session.HasAnyRole("LETAN_THUNGAN", "QLCN");

            // KOT (Bếp/Bar): QLBEP
            btnKOT.Enabled = Session.HasRole("QLBEP");

            // Thanh toán và Hóa đơn: LETAN_THUNGAN, QLCN
            btnThanhToan_HoaDon.Enabled = Session.HasAnyRole("LETAN_THUNGAN", "QLCN");

            // Thực đơn và Gói: QLCN
            btnThucDon_Goi.Enabled = Session.HasRole("QLCN");

            // Kho: QLKHO
            btnKho.Enabled = Session.HasRole("QLKHO");

            // Khuyến mãi và Voucher: QLCN
            btnKhuyenMai_Voucher.Enabled = Session.HasRole("QLCN");

            // Chi nhánh/Bàn/Sảnh: QLCN
            btnChiNhanh_Ban_Sanh.Enabled = Session.HasRole("QLCN");

            // Khách hàng: Tất cả (có thể xem)

            // Nhân sự và Ca: QLCN
            btnNhanSu_Ca.Enabled = Session.HasRole("QLCN");

            // Báo cáo: QLCN
            btnBaoCao.Enabled = Session.HasRole("QLCN");

            // Cấu hình: ADMIN
            btnCauHinh.Enabled = Session.HasRole("ADMIN");

            // Phân quyền: ADMIN
            btnPhanQuyen.Enabled = Session.HasRole("ADMIN");

            // Form Test
            btnFormTest.Enabled = Session.HasRole("ADMIN");
        }

        private void CheckPermissionBeforeOpen<T>(NavButton button, string[] allowedRoles) where T : Form, new()
        {
            if (!Session.HasAnyRole(allowedRoles))
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ShowChild<T>();
        }
    }
}
