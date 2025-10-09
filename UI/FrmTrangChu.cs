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

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmTrangChu : Form
    {
        private readonly Dictionary<Type, Form> _cache = new();
        private ImageList _icons;
        public FrmTrangChu()
        {
            InitializeComponent();
            ShowChild<FrmDashboard>();
        }

        private void FrmTrangChu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private async void FrmTrangChu_Load(object sender, EventArgs e)
        {
            WireNavButtons();
            var first = panel3.Controls.OfType<NavButton>().FirstOrDefault();
            if (first != null) SetSelected(first);

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
        }
    }
}
