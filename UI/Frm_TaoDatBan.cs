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
using QLNhaHangTiecCuoi.Share;
using UI.Common;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_TaoDatBan : RoundedBorderForm
    {
        private BanBLL _banBLL;
        private KhachHangBLL _khachHangBLL;
        private KhuVucBLL _khuVucBLL;
        private int _chiNhanhId;

        public Frm_TaoDatBan()
        {
            InitializeComponent();
            this.CornerRadius = 15;
            this.BorderColor = Color.Black;
            this.BorderThickness = 2;
            this.BackColor = Color.White;
            
            _banBLL = new BanBLL(new DatabaseHelper());
            _khachHangBLL = new KhachHangBLL();
            _khuVucBLL = new KhuVucBLL();
            _chiNhanhId = Session.ChiNhanhId;
            
            InitializeForm();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void roundedTextBox4_Load(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeForm()
        {
            try
            {
                LoadKhuVuc();
                dateNgay.Value = DateTime.Now;
                timeGio.SelectedTime = DateTime.Now.TimeOfDay;
                btnTaoDatBan.Click += BtnTaoDatBan_Click;
                cbbKhuVuc.SelectedIndexChanged += CbbKhuVuc_SelectedIndexChanged;
                txtSoDienThoai.TextChanged += TxtSoDienThoai_TextChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadKhuVuc()
        {
            try
            {
                var dtKhuVuc = _khuVucBLL.LayDanhSachKhuVuc(_chiNhanhId);
                cbbKhuVuc.Items.Clear();
                cbbKhuVuc.Items.Add("-- Chọn khu vực --");
                
                if (dtKhuVuc != null && dtKhuVuc.Rows.Count > 0)
                {
                    foreach (DataRow row in dtKhuVuc.Rows)
                    {
                        cbbKhuVuc.Items.Add(new ComboBoxItem(
                            row["ten_khu_vuc"].ToString(),
                            Convert.ToInt32(row["khu_vuc_id"])
                        ));
                    }
                }

                cbbKhuVuc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBanTheoKhuVuc(int khuVucId)
        {
            try
            {
                var dtBan = _banBLL.LayDanhSachBan(_chiNhanhId, khuVucId);
                cbbSoBan.Items.Clear();
                cbbSoBan.Items.Add("-- Chọn bàn --");
                
                if (dtBan != null && dtBan.Rows.Count > 0)
                {
                    foreach (DataRow row in dtBan.Rows)
                    {
                        string trangThai = row["trang_thai"].ToString();
                        if (trangThai == "TRỐNG")
                        {
                            cbbSoBan.Items.Add(new ComboBoxItem(
                                $"Bàn {row["so_ban"]} (Sức chứa: {row["suc_chua"]})",
                                Convert.ToInt32(row["ban_id"])
                            ));
                        }
                    }
                }

                cbbSoBan.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CbbKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbKhuVuc.SelectedItem is ComboBoxItem selectedItem && selectedItem.Value > 0)
                {
                    LoadBanTheoKhuVuc(selectedItem.Value);
                }
                else
                {
                    cbbSoBan.Items.Clear();
                    cbbSoBan.Items.Add("-- Chọn bàn --");
                    cbbSoBan.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSoDienThoai_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string sdt = txtSoDienThoai.Text.Trim();
                if (sdt.Length >= 10)
                {
                    var dtKhachHang = _khachHangBLL.TimKhachHangTheoSdt(sdt);
                    if (dtKhachHang != null && dtKhachHang.Rows.Count > 0)
                    {
                        var khachHang = dtKhachHang.Rows[0];
                        txtTenKhachHang.Text = khachHang["ho_ten"].ToString();
                        txtEmail.Text = khachHang["email"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi khi tìm kiếm
            }
        }

        private void BtnTaoDatBan_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;
                string sdt = txtSoDienThoai.Text.Trim();
                string tenKhachHang = txtTenKhachHang.Text.Trim();
                string email = txtEmail.Text.Trim();
                DateTime ngayDat = dateNgay.Value.Date;
                TimeSpan gioDat = timeGio.SelectedTime ?? DateTime.Now.TimeOfDay;
                int soKhach = Convert.ToInt32(txtSoKhach.Text);
                int khuVucId = ((ComboBoxItem)cbbKhuVuc.SelectedItem).Value;
                int banId = ((ComboBoxItem)cbbSoBan.SelectedItem).Value;
                string ghiChu = txtGhiChu.Text.Trim();

                // Tạo datetime đầy đủ
                DateTime ngayGioDat = ngayDat.Add(gioDat);

                // Tìm hoặc tạo khách hàng
                int khachHangId = GetOrCreateKhachHang(sdt, tenKhachHang, email);

                bool success = _banBLL.TaoDatBan(_chiNhanhId, banId, khachHangId, ngayGioDat, soKhach, ghiChu);
                if (success)
                {
                    MessageBox.Show("Tạo đặt bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể tạo đặt bàn. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo đặt bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Kiểm tra SĐT
            if (string.IsNullOrEmpty(txtSoDienThoai.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            // Kiểm tra tên khách hàng
            if (string.IsNullOrEmpty(txtTenKhachHang.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhachHang.Focus();
                return false;
            }

            // Kiểm tra ngày
            if (dateNgay.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Ngày đặt bàn không được là ngày trong quá khứ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateNgay.Focus();
                return false;
            }

            // Kiểm tra giờ
            if (timeGio.SelectedTime == null)
            {
                MessageBox.Show("Vui lòng chọn giờ đặt bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                timeGio.Focus();
                return false;
            }

            // Kiểm tra số khách
            if (string.IsNullOrEmpty(txtSoKhach.Text) || !int.TryParse(txtSoKhach.Text, out int soKhach) || soKhach <= 0)
            {
                MessageBox.Show("Vui lòng nhập số khách hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoKhach.Focus();
                return false;
            }

            // Kiểm tra khu vực
            if (cbbKhuVuc.SelectedItem == null || !(cbbKhuVuc.SelectedItem is ComboBoxItem) || ((ComboBoxItem)cbbKhuVuc.SelectedItem).Value <= 0)
            {
                MessageBox.Show("Vui lòng chọn khu vực!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbKhuVuc.Focus();
                return false;
            }

            // Kiểm tra bàn
            if (cbbSoBan.SelectedItem == null || !(cbbSoBan.SelectedItem is ComboBoxItem) || ((ComboBoxItem)cbbSoBan.SelectedItem).Value <= 0)
            {
                MessageBox.Show("Vui lòng chọn bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbSoBan.Focus();
                return false;
            }

            if (!ValidateSoKhachVsSucChua(soKhach, ((ComboBoxItem)cbbSoBan.SelectedItem).Value))
            {
                return false;
            }

            return true;
        }

        private bool ValidateSoKhachVsSucChua(int soKhach, int banId)
        {
            try
            {
                var dtBan = _banBLL.LayThongTinBan(banId);
                if (dtBan != null && dtBan.Rows.Count > 0)
                {
                    int sucChua = Convert.ToInt32(dtBan.Rows[0]["suc_chua"]);
                    string soBan = dtBan.Rows[0]["so_ban"].ToString();
                    
                    
                    if (soKhach > sucChua)
                    {
                        MessageBox.Show($"Số khách ({soKhach}) vượt quá sức chứa của bàn {soBan} ({sucChua} người)!\nVui lòng chọn bàn khác hoặc giảm số khách.", 
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbSoBan.Focus();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra sức chứa bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private int GetOrCreateKhachHang(string sdt, string tenKhachHang, string email)
        {
            try
            {
                // Tìm khách hàng theo SĐT
                var dtKhachHang = _khachHangBLL.TimKhachHangTheoSdt(sdt);
                if (dtKhachHang != null && dtKhachHang.Rows.Count > 0)
                {
                    return Convert.ToInt32(dtKhachHang.Rows[0]["khach_hang_id"]);
                }
                else
                {
                    // Tạo khách hàng mới
                    return _khachHangBLL.TaoKhachHang(tenKhachHang, sdt, email, "");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xử lý khách hàng: {ex.Message}");
            }
        }
    }

}
