using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using UI.Controls;
using Guna.UI2.WinForms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_ChiTietDatBan : Form
    {
        private Reservation _reservation;

        public Frm_ChiTietDatBan(Reservation reservation)
        {
            InitializeComponent();
            _reservation = reservation;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (_reservation == null) return;

                // Header
                lblTitle.Text = $"Chi tiết đặt bàn {_reservation.Code}";
                lblSubtitle.Text = "Xem và quản lý thông tin đặt bàn";

                // Status badge
                lblStatus.Text = _reservation.Status;
                var colors = GetStatusColors(_reservation.Status);
                pnlStatusBadge.BackColor = colors.BadgeColor;
                lblStatus.ForeColor = colors.TextColor;
                
                // Đổi icon theo trạng thái
                string statusUpper = _reservation.Status.ToUpper();
                if (statusUpper.Contains("HỦY") || statusUpper.Contains("ĐÃ HỦY"))
                {
                    lblStatusIcon.Text = "✕";
                }
                else if (statusUpper.Contains("XÁC NHẬN") || statusUpper.Contains("ĐÃ XÁC NHẬN"))
                {
                    lblStatusIcon.Text = "✓";
                }
                else
                {
                    lblStatusIcon.Text = "";
                }

                // Thông tin khách hàng
                lblTenKhachHangValue.Text = _reservation.CustomerName;
                lblSoDienThoaiValue.Text = _reservation.Phone;
                lblEmailValue.Text = ""; // Có thể lấy từ database sau, nếu không có thì ẩn
                if (string.IsNullOrWhiteSpace(lblEmailValue.Text))
                {
                    lblEmail.Visible = false;
                    lblEmailIcon.Visible = false;
                    lblEmailValue.Visible = false;
                }

                // Thông tin đặt bàn
                lblNgayDatValue.Text = _reservation.Date.ToString("dd/MM/yyyy");
                lblBanValue.Text = _reservation.TableName;
                lblSoKhachValue.Text = $"{_reservation.Guests} người";
                lblGioValue.Text = _reservation.Date.ToString("HH:mm");
                lblKhuVucValue.Text = _reservation.Area;
                lblTienCocValue.Text = _reservation.Deposit > 0 ? $"{_reservation.Deposit:N0} đ" : "Chưa cọc";
                
                if (_reservation.Deposit == 0)
                {
                    lblTienCocValue.ForeColor = Color.FromArgb(34, 197, 94); // Màu xanh lá
                }

                // Ghi chú
                if (!string.IsNullOrWhiteSpace(_reservation.Note))
                {
                    pnlGhiChu.Visible = true;
                    lblGhiChuValue.Text = _reservation.Note;
                }
                else
                {
                    pnlGhiChu.Visible = false;
                }

                // Timestamp (giả sử dùng Date của reservation làm ngày tạo)
                lblTimestamp.Text = $"Tạo lúc: {_reservation.Date:HH:mm:ss dd/MM/yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private (Color BadgeColor, Color TextColor) GetStatusColors(string status)
        {
            string statusUpper = status.ToUpper();

            if (statusUpper.Contains("HỦY") || statusUpper.Contains("ĐÃ HỦY"))
            {
                return (Color.FromArgb(239, 68, 68), Color.White); // Đỏ
            }
            else if (statusUpper.Contains("XÁC NHẬN") || statusUpper.Contains("ĐÃ XÁC NHẬN"))
            {
                return (Color.FromArgb(34, 197, 94), Color.White); // Xanh lá
            }
            else if (statusUpper.Contains("CHỜ") || statusUpper.Contains("CHỜ XÁC NHẬN"))
            {
                return (Color.FromArgb(245, 158, 11), Color.White); // Cam/vàng
            }
            else if (statusUpper.Contains("ĐÃ ĐẾN") || statusUpper.Contains("ĐẾN"))
            {
                return (Color.FromArgb(59, 130, 246), Color.White); // Xanh dương
            }
            else
            {
                return (Color.FromArgb(107, 114, 128), Color.White); // Xám
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuiTinNhan_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_reservation.Phone))
                {
                    // Có thể mở form gửi tin nhắn hoặc thực hiện hành động gửi tin nhắn
                    MessageBox.Show($"Gửi tin nhắn đến {_reservation.Phone}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có số điện thoại để gửi tin nhắn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi tin nhắn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

