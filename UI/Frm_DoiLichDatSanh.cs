using BLL;
using QLNhaHangTiecCuoi.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_DoiLichDatSanh : Form
    {
        private int _datSanhId;
        private DatSanhBLL _datSanhBLL;
        private DataRow? _datSanhInfo;

        public Frm_DoiLichDatSanh(int datSanhId)
        {
            InitializeComponent();
            _datSanhId = datSanhId;
            _datSanhBLL = new DatSanhBLL();

            StartPosition = FormStartPosition.CenterParent;

            LoadThongTinHienTai();
            LoadChiNhanh();
        }

        private void LoadThongTinHienTai()
        {
            try
            {
                _datSanhInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                if (_datSanhInfo == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin đặt sảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Kiểm tra trạng thái và vô hiệu hóa form nếu cần
                if (_datSanhInfo["trang_thai"] != DBNull.Value)
                {
                    string trangThai = _datSanhInfo["trang_thai"].ToString() ?? "";
                    if (trangThai.ToUpper() == "ĐÃ HỦY" || trangThai.ToUpper() == "HOÀN TẤT")
                    {
                        // Vô hiệu hóa các control
                        cbChiNhanh.Enabled = false;
                        cbSanh.Enabled = false;
                        dtpNgayToChucMoi.Enabled = false;
                        cbCaToChuc.Enabled = false;
                        txtLyDoDoiLich.Enabled = false;
                        txtGhiChuThem.Enabled = false;
                        btnXacNhan.Enabled = false;
                        
                        string message = trangThai.ToUpper() == "ĐÃ HỦY" 
                            ? "Đơn đặt sảnh đã bị hủy. Không thể đổi lịch!" 
                            : "Đơn đặt sảnh đã hoàn tất. Không thể đổi lịch!";
                        
                        MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Hiển thị thông tin hiện tại
                if (_datSanhInfo != null && _datSanhInfo["ngay_to_chuc"] != DBNull.Value)
                {
                    DateTime ngayToChuc = Convert.ToDateTime(_datSanhInfo["ngay_to_chuc"]).ToLocalTime();
                    lbNgayHienTai.Text = $"Ngày: {ngayToChuc:dd/MM/yyyy}";
                }

                if (_datSanhInfo != null && _datSanhInfo["ten_ca"] != DBNull.Value && _datSanhInfo["gio_bd"] != DBNull.Value && _datSanhInfo["gio_kt"] != DBNull.Value)
                {
                    string tenCa = _datSanhInfo["ten_ca"].ToString() ?? "";
                    TimeSpan gioBd = (TimeSpan)_datSanhInfo["gio_bd"];
                    TimeSpan gioKt = (TimeSpan)_datSanhInfo["gio_kt"];
                    lbCaHienTai.Text = $"Ca: {tenCa} ({gioBd:hh\\:mm} - {gioKt:hh\\:mm})";
                }

                if (_datSanhInfo != null && _datSanhInfo["ten_chi_nhanh"] != DBNull.Value)
                {
                    lbChiNhanhHienTai.Text = $"Chi nhánh: {_datSanhInfo["ten_chi_nhanh"]}";
                }

                if (_datSanhInfo != null && _datSanhInfo["ten_sanh"] != DBNull.Value)
                {
                    lbSanhHienTai.Text = $"Sảnh: {_datSanhInfo["ten_sanh"]}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin hiện tại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiNhanh()
        {
            try
            {
                cbChiNhanh.Items.Clear();
                DataTable dtChiNhanh = _datSanhBLL.LayDanhSachChiNhanh();
                
                if (dtChiNhanh != null && dtChiNhanh.Rows.Count > 0)
                {
                    foreach (DataRow row in dtChiNhanh.Rows)
                    {
                        string tenChiNhanh = row["ten"].ToString();
                        int chiNhanhId = Convert.ToInt32(row["chi_nhanh_id"]);
                        cbChiNhanh.Items.Add(new ComboBoxItem(tenChiNhanh, chiNhanhId));
                    }

                    // Chọn chi nhánh hiện tại
                    if (_datSanhInfo != null && _datSanhInfo["chi_nhanh_id"] != DBNull.Value)
                    {
                        int chiNhanhIdHienTai = Convert.ToInt32(_datSanhInfo["chi_nhanh_id"]);
                        for (int i = 0; i < cbChiNhanh.Items.Count; i++)
                        {
                            var item = (ComboBoxItem)cbChiNhanh.Items[i];
                            if (item.Value is int val && val == chiNhanhIdHienTai)
                            {
                                cbChiNhanh.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CbChiNhanh_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbChiNhanh.SelectedItem == null)
            {
                UpdateButtonState();
                return;
            }

            try
            {
                var item = (ComboBoxItem)cbChiNhanh.SelectedItem;
                int chiNhanhId = (int)item.Value;
                LoadSanh(chiNhanhId);
                UpdateButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateButtonState();
            }
        }

        private void LoadSanh(int chiNhanhId)
        {
            try
            {
                cbSanh.Items.Clear();
                DataTable dtSanh = _datSanhBLL.LayDanhSachSanh(chiNhanhId);

                if (dtSanh != null && dtSanh.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSanh.Rows)
                    {
                        string tenSanh = row["ten_sanh"].ToString();
                        int sucChua = row["suc_chua"] != DBNull.Value ? Convert.ToInt32(row["suc_chua"]) : 0;
                        int sanhId = Convert.ToInt32(row["sanh_id"]);
                        string displayText = $"{tenSanh} (Sức chứa: {sucChua:N0} k)";
                        cbSanh.Items.Add(new ComboBoxItem(displayText, sanhId));
                    }

                    // Chọn sảnh hiện tại nếu cùng chi nhánh
                    if (_datSanhInfo != null && _datSanhInfo["sanh_id"] != DBNull.Value)
                    {
                        int sanhIdHienTai = Convert.ToInt32(_datSanhInfo["sanh_id"]);
                        for (int i = 0; i < cbSanh.Items.Count; i++)
                        {
                            var item = (ComboBoxItem)cbSanh.Items[i];
                            if (item.Value is int val && val == sanhIdHienTai)
                            {
                                cbSanh.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                
                UpdateButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateButtonState();
            }
        }

        private void LoadCa()
        {
            try
            {
                cbCaToChuc.Items.Clear();
                
                // Ca cố định 1: 10h30 - 13h30
                TimeSpan gioBd1 = new TimeSpan(10, 30, 0);
                TimeSpan gioKt1 = new TimeSpan(13, 30, 0);
                string displayText1 = "10h30 - 13h30";
                cbCaToChuc.Items.Add(new ComboBoxItem(displayText1, gioBd1));

                // Ca cố định 2: 17h30 - 20h30
                TimeSpan gioBd2 = new TimeSpan(17, 30, 0);
                TimeSpan gioKt2 = new TimeSpan(20, 30, 0);
                string displayText2 = "17h30 - 20h30";
                cbCaToChuc.Items.Add(new ComboBoxItem(displayText2, gioBd2));

                // Chọn ca hiện tại dựa trên giờ tổ chức
                if (_datSanhInfo != null && _datSanhInfo["gio_to_chuc"] != DBNull.Value && _datSanhInfo["gio_to_chuc"] != null)
                {
                    TimeSpan gioToChucHienTai = (TimeSpan)_datSanhInfo["gio_to_chuc"];
                    for (int i = 0; i < cbCaToChuc.Items.Count; i++)
                    {
                        var item = (ComboBoxItem)cbCaToChuc.Items[i];
                        if (item.Value is TimeSpan gioBd)
                        {
                            if (gioBd == gioToChucHienTai)
                            {
                                cbCaToChuc.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                
                UpdateButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách ca: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateButtonState();
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnXacNhan_Click(object? sender, EventArgs e)
        {
            try
            {
                // Kiểm tra trạng thái đơn đặt sảnh
                if (_datSanhInfo != null && _datSanhInfo["trang_thai"] != DBNull.Value)
                {
                    string trangThai = _datSanhInfo["trang_thai"].ToString() ?? "";
                    if (trangThai.ToUpper() == "ĐÃ HỦY")
                    {
                        MessageBox.Show("Không thể đổi lịch cho đơn đặt sảnh đã bị hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    if (trangThai.ToUpper() == "HOÀN TẤT")
                    {
                        MessageBox.Show("Không thể đổi lịch cho đơn đặt sảnh đã hoàn tất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Validation
                if (string.IsNullOrWhiteSpace(txtLyDoDoiLich.Text))
                {
                    MessageBox.Show("Vui lòng nhập lý do đổi lịch!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLyDoDoiLich.Focus();
                    return;
                }

                if (cbChiNhanh.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn chi nhánh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbSanh.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn sảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbCaToChuc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn ca tổ chức!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var chiNhanhItem = (ComboBoxItem)cbChiNhanh.SelectedItem;
                var sanhItem = (ComboBoxItem)cbSanh.SelectedItem;
                var caItem = (ComboBoxItem)cbCaToChuc.SelectedItem;

                int chiNhanhId = (int)chiNhanhItem.Value;
                int sanhId = (int)sanhItem.Value;
                TimeSpan gioToChuc = (TimeSpan)caItem.Value;
                DateTime ngayToChucMoi = dtpNgayToChucMoi.Value.Date;
                string lyDo = txtLyDoDoiLich.Text.Trim();
                string ghiChu = txtGhiChuThem.Text.Trim();

                // Kiểm tra ngày tổ chức mới
                if (ngayToChucMoi < DateTime.Now.Date)
                {
                    MessageBox.Show("Ngày tổ chức mới không được ở quá khứ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpNgayToChucMoi.Focus();
                    return;
                }

                // Kiểm tra nếu thông tin mới giống thông tin cũ
                if (_datSanhInfo != null)
                {
                    bool thongTinGiongNhau = true;
                    
                    if (_datSanhInfo["chi_nhanh_id"] != DBNull.Value)
                    {
                        int chiNhanhIdCu = Convert.ToInt32(_datSanhInfo["chi_nhanh_id"]);
                        if (chiNhanhIdCu != chiNhanhId)
                            thongTinGiongNhau = false;
                    }
                    else
                    {
                        thongTinGiongNhau = false;
                    }

                    if (thongTinGiongNhau && _datSanhInfo["sanh_id"] != DBNull.Value)
                    {
                        int sanhIdCu = Convert.ToInt32(_datSanhInfo["sanh_id"]);
                        if (sanhIdCu != sanhId)
                            thongTinGiongNhau = false;
                    }
                    else
                    {
                        thongTinGiongNhau = false;
                    }

                    if (thongTinGiongNhau && _datSanhInfo["ngay_to_chuc"] != DBNull.Value)
                    {
                        DateTime ngayToChucCu = Convert.ToDateTime(_datSanhInfo["ngay_to_chuc"]).Date;
                        if (ngayToChucCu != ngayToChucMoi)
                            thongTinGiongNhau = false;
                    }
                    else
                    {
                        thongTinGiongNhau = false;
                    }

                    if (thongTinGiongNhau && _datSanhInfo["gio_to_chuc"] != DBNull.Value)
                    {
                        TimeSpan gioToChucCu = (TimeSpan)_datSanhInfo["gio_to_chuc"];
                        if (gioToChucCu != gioToChuc)
                            thongTinGiongNhau = false;
                    }
                    else
                    {
                        thongTinGiongNhau = false;
                    }

                    if (thongTinGiongNhau)
                    {
                        MessageBox.Show("Thông tin mới giống với thông tin hiện tại. Vui lòng thay đổi ít nhất một thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Xác nhận
                var confirmResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn đổi lịch tổ chức?\n\nThao tác này sẽ cập nhật thông tin đặt sảnh.",
                    "Xác nhận đổi lịch",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes)
                    return;

                bool result = _datSanhBLL.DoiLichDatSanh(_datSanhId, chiNhanhId, sanhId, gioToChuc, ngayToChucMoi, lyDo, 
                    string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu, out string errorMessage);

                if (result)
                {
                    MessageBox.Show("Đổi lịch tổ chức thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Lỗi khi đổi lịch: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xác nhận đổi lịch: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            dtpNgayToChucMoi.MinDate = DateTime.Now.Date;
            dtpNgayToChucMoi.Value = DateTime.Now.Date;
            
            LoadCa();
            if (cbChiNhanh.SelectedItem != null)
            {
                var item = (ComboBoxItem)cbChiNhanh.SelectedItem;
                if (item.Value is int chiNhanhId)
                {
                    LoadSanh(chiNhanhId);
                }
            }
            
            dtpNgayToChucMoi.ValueChanged += CheckSanhTrong;
            cbCaToChuc.SelectedIndexChanged += CheckSanhTrong;
            cbSanh.SelectedIndexChanged += CheckSanhTrong;
            cbChiNhanh.SelectedIndexChanged += (s, e) => UpdateButtonState();
            txtLyDoDoiLich.TextChanged += (s, e) => UpdateButtonState();
            
            UpdateButtonState();
        }

        private void CheckSanhTrong(object? sender, EventArgs e)
        {
            try
            {
                if (cbSanh.SelectedItem == null || cbCaToChuc.SelectedItem == null)
                {
                    panelAlertSuccess.Visible = false;
                    UpdateButtonState();
                    return;
                }

                var sanhItem = (ComboBoxItem)cbSanh.SelectedItem;
                var caItem = (ComboBoxItem)cbCaToChuc.SelectedItem;
                DateTime ngayToChuc = dtpNgayToChucMoi.Value.Date;
                TimeSpan gioToChuc = caItem.Value is TimeSpan ts ? ts : TimeSpan.Zero;

                int sanhId = (int)sanhItem.Value;
                bool sanhTrong = _datSanhBLL.KiemTraSanhTrong(sanhId, gioToChuc, ngayToChuc, out string errorMessage, _datSanhId);
                
                if (sanhTrong)
                {
                    panelAlertSuccess.Visible = true;
                }
                else
                {
                    panelAlertSuccess.Visible = false;
                }
            }
            catch
            {
                panelAlertSuccess.Visible = false;
            }
            
            UpdateButtonState();
        }

        // Kiểm tra và cập nhật trạng thái button
        private void UpdateButtonState()
        {
            bool isValid = IsFormValid();
            
            if (isValid)
            {
                btnXacNhan.FillColor = Color.FromArgb(76, 175, 80);
                btnXacNhan.ForeColor = Color.White;
            }
            else
            {
                btnXacNhan.FillColor = Color.Gray;
                btnXacNhan.ForeColor = Color.White;
            }
        }

        // Kiểm tra form có hợp lệ không
        private bool IsFormValid()
        {
            if (cbChiNhanh.SelectedItem == null)
                return false;
            
            if (cbSanh.SelectedItem == null)
                return false;
            
            if (cbCaToChuc.SelectedItem == null)
                return false;
            
            if (string.IsNullOrWhiteSpace(txtLyDoDoiLich.Text))
                return false;
            // Kiểm tra sảnh còn trống
            if (!panelAlertSuccess.Visible)
                return false;
            
            return true;
        }

        private void CbCaToChuc_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CheckSanhTrong(sender, e);
        }

        private void CbCaToChuc_SelectedValueChanged(object? sender, EventArgs e)
        {
            CheckSanhTrong(sender, e);
        }

        private void DtpNgayToChucMoi_ValueChanged(object? sender, EventArgs e)
        {
            CheckSanhTrong(sender, e);
        }

        private void CbSanh_SelectedIndexChanged(object? sender, EventArgs e)
        {
            CheckSanhTrong(sender, e);
        }

        // Helper class cho ComboBox items
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public ComboBoxItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}

