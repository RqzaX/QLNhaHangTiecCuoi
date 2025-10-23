using System;
using System.Data;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;
using UI.Controls;

namespace UI
{
    public partial class Frm_ChinhSuaDatBan : Form
    {
        private BanBLL _banBLL;
        private KhachHangBLL _khachHangBLL;
        private KhuVucBLL _khuVucBLL;
        private DataGripView_DatBan.Reservation _reservation;

        public Frm_ChinhSuaDatBan(DataGripView_DatBan.Reservation reservation)
        {
            InitializeComponent();
            _banBLL = new BanBLL(new DatabaseHelper());
            _khachHangBLL = new KhachHangBLL();
            _khuVucBLL = new KhuVucBLL();
            _reservation = reservation;
            
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var dtKhuVuc = _khuVucBLL.LayDanhSachKhuVuc(1); // Sử dụng chi nhánh ID = 1
                if (dtKhuVuc != null && dtKhuVuc.Rows.Count > 0)
                {
                    cboKhuVuc.DataSource = dtKhuVuc;
                    cboKhuVuc.DisplayMember = "ten_khu_vuc";
                    cboKhuVuc.ValueMember = "khu_vuc_id";
                }

                LoadGioDat();
                LoadBanTheoKhuVuc();
                DisplayCurrentReservation();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGioDat()
        {
            try
            {
                cboGioDat.Items.Clear();
                
                for (int hour = 8; hour <= 20; hour++)
                {
                    for (int minute = 0; minute < 60; minute += 30)
                    {
                        if (hour == 20 && minute > 0) break;
                        
                        string timeString = $"{hour:D2}:{minute:D2}";
                        cboGioDat.Items.Add(timeString);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load giờ đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayCurrentReservation()
        {
            try
            {
                txtMaDatBan.Text = _reservation.Code;
                txtTenKhachHang.Text = _reservation.CustomerName;
                txtSoDienThoai.Text = _reservation.Phone;
                txtSoKhach.Text = _reservation.Guests.ToString();
                dtpNgayDat.Value = _reservation.Date;
                
                // Set giờ đặt từ ComboBox
                string timeString = _reservation.Date.ToString("HH:mm");
                int index = cboGioDat.Items.IndexOf(timeString);
                if (index >= 0)
                {
                    cboGioDat.SelectedIndex = index;
                }
                
                txtGhiChu.Text = _reservation.Note;

                // Load bàn theo khu vực và chọn bàn hiện tại
                LoadBanTheoKhuVuc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBanTheoKhuVuc()
        {
            try
            {
                if (cboKhuVuc.SelectedValue != null && cboKhuVuc.SelectedIndex >= 0)
                {
                    // Lấy khu_vuc_id từ DataTable
                    DataTable dtKhuVuc = (DataTable)cboKhuVuc.DataSource;
                    int khuVucId = Convert.ToInt32(dtKhuVuc.Rows[cboKhuVuc.SelectedIndex]["khu_vuc_id"]);
                    
                    var dtBan = _banBLL.LayDanhSachBanTheoKhuVuc(khuVucId);
                    
                    if (dtBan != null && dtBan.Rows.Count > 0)
                    {
                        cboBan.DataSource = dtBan;
                        cboBan.DisplayMember = "so_ban";
                        cboBan.ValueMember = "ban_id";

                        // Tìm và chọn bàn hiện tại
                        for (int i = 0; i < dtBan.Rows.Count; i++)
                        {
                            if (dtBan.Rows[i]["so_ban"].ToString() == _reservation.TableName)
                            {
                                cboBan.SelectedIndex = i;
                                // Hiển thị sức chứa bàn
                                txtSucChuaBan.Text = dtBan.Rows[i]["suc_chua"].ToString();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtTenKhachHang.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhachHang.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSoKhach.Text) || !int.TryParse(txtSoKhach.Text, out int soKhach) || soKhach <= 0)
            {
                MessageBox.Show("Vui lòng nhập số khách hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoKhach.Focus();
                return false;
            }

            if (cboKhuVuc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khu vực!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboKhuVuc.Focus();
                return false;
            }

            if (cboBan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bàn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboBan.Focus();
                return false;
            }

            // Kiểm tra sức chứa bàn
            try
            {
                DataTable dtBan = (DataTable)cboBan.DataSource;
                int banId = Convert.ToInt32(cboBan.SelectedValue);
                int sucChua = 0;

                for (int i = 0; i < dtBan.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dtBan.Rows[i]["ban_id"]) == banId)
                    {
                        sucChua = Convert.ToInt32(dtBan.Rows[i]["suc_chua"]);
                        break;
                    }
                }

                if (soKhach > sucChua)
                {
                    MessageBox.Show($"Số khách ({soKhach}) không được vượt quá sức chứa của bàn ({sucChua})!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoKhach.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra sức chứa bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool UpdateReservation()
        {
            try
            {
                // Lấy thông tin bàn
                DataTable dtBan = (DataTable)cboBan.DataSource;
                int banId = Convert.ToInt32(cboBan.SelectedValue);
                int sucChua = 0;
                string soBan = "";

                for (int i = 0; i < dtBan.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dtBan.Rows[i]["ban_id"]) == banId)
                    {
                        sucChua = Convert.ToInt32(dtBan.Rows[i]["suc_chua"]);
                        soBan = dtBan.Rows[i]["so_ban"].ToString();
                        break;
                    }
                }

                // Kết hợp ngày và giờ đặt
                string selectedTime = cboGioDat.SelectedItem?.ToString() ?? "08:00";
                string[] timeParts = selectedTime.Split(':');
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);
                DateTime ngayGioDat = dtpNgayDat.Value.Date.Add(new TimeSpan(hour, minute, 0));
                
                return _banBLL.CapNhatDatBan(
                    _reservation.Code,
                    1, // Khách hàng ID mặc định (có thể cần thay đổi logic này)
                    banId,
                    Convert.ToInt32(txtSoKhach.Text),
                    ngayGioDat,
                    txtGhiChu.Text.Trim()
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật đặt bàn: {ex.Message}");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    if (UpdateReservation())
                    {
                        MessageBox.Show("Cập nhật đặt bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật đặt bàn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void cboKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBanTheoKhuVuc();
        }

        private void cboBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBan.SelectedIndex >= 0)
                {
                    // Lấy sức chứa của bàn được chọn
                    DataTable dtBan = (DataTable)cboBan.DataSource;
                    txtSucChuaBan.Text = dtBan.Rows[cboBan.SelectedIndex]["suc_chua"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật sức chứa bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
