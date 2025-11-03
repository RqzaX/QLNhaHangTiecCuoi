using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using UI.Controls;
using Sunny.UI;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_DatBan : UserControl
    {
        private Reservation _reservation;
        private Color _originalBackColor = Color.White;
        private Color _hoverBackColor = Color.FromArgb(247, 247, 247);
        private System.Windows.Forms.Timer _countdownTimer;
        public event EventHandler<ReservationEventArgs> ReservationClicked;
        public event EventHandler<ReservationEventArgs> EditClicked;
        public event EventHandler<ReservationEventArgs> ArrivedClicked;
        public event EventHandler<ReservationEventArgs> CancelClicked;

        public Frm_DatBan()
        {
            InitializeComponent();
            this.Size = new Size(659, 144);
            this.Margin = new Padding(10);
            this.BackColor = _originalBackColor;
            InitializeCountdownTimer();
            HookEvents();
        }

        public Reservation Reservation
        {
            get { return _reservation; }
            set
            {
                _reservation = value;
                LoadData();
            }
        }

        private void HookEvents()
        {
            // Make the entire control clickable (except buttons)
            this.Click += Frm_DatBan_Click;
            
            // Hook mouse events for hover effect
            this.MouseEnter += Frm_DatBan_MouseEnter;
            this.MouseLeave += Frm_DatBan_MouseLeave;
            
            // Hook click events for all controls except buttons
            foreach (Control control in this.Controls)
            {
                if (control != btnChinhSua && control != btnDaDen && control != btnHuy)
                {
                    control.Click += Frm_DatBan_Click;
                    // Hook mouse events for child controls to maintain hover effect
                    control.MouseEnter += Frm_DatBan_MouseEnter;
                    control.MouseLeave += Frm_DatBan_MouseLeave;
                }
            }

            // Hook button events
            btnChinhSua.Click += BtnChinhSua_Click;
            btnDaDen.Click += BtnDaDen_Click;
            btnHuy.Click += BtnHuy_Click;
        }

        private void Frm_DatBan_MouseEnter(object sender, EventArgs e)
        {
            // Change background color on hover
            this.BackColor = _hoverBackColor;
        }

        private void Frm_DatBan_MouseLeave(object sender, EventArgs e)
        {
            // Restore original background color when mouse leaves
            // Check if mouse is still within the control bounds
            Point mousePos = this.PointToClient(Control.MousePosition);
            if (!this.ClientRectangle.Contains(mousePos))
            {
                this.BackColor = _originalBackColor;
            }
        }

        private void Frm_DatBan_Click(object sender, EventArgs e)
        {
            if (_reservation != null)
            {
                ReservationClicked?.Invoke(this, new ReservationEventArgs(_reservation));
            }
        }

        private void BtnChinhSua_Click(object sender, EventArgs e)
        {
            if (_reservation != null)
            {
                EditClicked?.Invoke(this, new ReservationEventArgs(_reservation));
            }
        }

        private void BtnDaDen_Click(object sender, EventArgs e)
        {
            if (_reservation != null)
            {
                ArrivedClicked?.Invoke(this, new ReservationEventArgs(_reservation));
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            if (_reservation != null)
            {
                CancelClicked?.Invoke(this, new ReservationEventArgs(_reservation));
            }
        }

        private void LoadData()
        {
            if (_reservation == null) return;

            try
            {
                lbGio.Text = _reservation.Date.ToString("HH:mm");
                lbTenKhach_SoBan.Text = $"{_reservation.CustomerName} - Bàn {_reservation.TableName}";
                panelTrangThaiBan.Text = _reservation.Status;
                SetStatusColor(_reservation.Status);
                lbNgay.Text = _reservation.Date.ToString("dd/MM/yyyy");
                lbSoKhach.Text = $"{_reservation.Guests} khách";
                lbKhu.Text = !string.IsNullOrWhiteSpace(_reservation.Area) ? _reservation.Area : "Chưa có khu vực";

                DateTime now = DateTime.Now;
                bool isPassedReservationTime = _reservation.Date < now;
                bool isTreGioStatus = _reservation.Status.Contains("Trễ giờ đặt");
                TimeSpan timeDiff = now - _reservation.Date;
                
                // Hiển thị ghi chú cảnh báo khi đã quá giờ đặt nhưng chưa quá 2 tiếng
                if (isTreGioStatus && isPassedReservationTime && timeDiff.TotalMinutes >= 0 && timeDiff.TotalMinutes < 120)
                {
                    panelGhiChu.Text = "⚠️ TRỄ GIỜ ĐẶT - Bàn này sẽ tự động Hủy sau 2 tiếng nếu khách chưa tới!";
                    if (!string.IsNullOrWhiteSpace(_reservation.Note) && !_reservation.Note.Contains("⚠️ TRỄ GIỜ ĐẶT"))
                    {
                        panelGhiChu.Text += $"\n{_reservation.Note}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(_reservation.Note))
                {
                    // Hiển thị ghi chú gốc nếu không phải trạng thái trễ giờ
                    panelGhiChu.Text = $"Ghi chú: {_reservation.Note}";
                }
                else
                {
                    panelGhiChu.Text = "Ghi chú: không có";
                }

                UpdateButtonStates();
                
                // Cập nhật bộ đếm ngược thời gian
                UpdateCountdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeCountdownTimer()
        {
            _countdownTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // Cập nhật mỗi 1 giây
            };
            _countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            UpdateCountdown();
        }

        private void UpdateCountdown()
        {
            if (_reservation == null)
            {
                panelBoDemThoiGian.Visible = false;
                _countdownTimer.Stop();
                return;
            }

            string statusUpper = _reservation.Status.ToUpper();
            DateTime reservationTime = _reservation.Date;
            DateTime now = DateTime.Now;
            
            // Chỉ hiển thị đếm ngược khi trạng thái hiển thị là "Trễ giờ đặt"
            // (được tính toán dựa trên logic thời gian, không dựa vào ghi chú trong database)
            bool isTreGioStatus = statusUpper.Contains("TRỄ") || statusUpper.Contains("TRỄ GIỜ");
            bool isPassedReservationTime = reservationTime < now;
            TimeSpan timeDiff = now - reservationTime;
            
            // Chỉ hiển thị nếu đã quá giờ đặt và trạng thái là "Trễ giờ đặt"
            // (tức là đã quá giờ nhưng chưa quá 2 tiếng)
            if (isTreGioStatus && isPassedReservationTime && timeDiff.TotalMinutes >= 0 && timeDiff.TotalMinutes < 120)
            {
                DateTime cancelTime = reservationTime.AddHours(2); // Thời điểm sẽ hủy (2 tiếng sau giờ đặt)

                if (now < cancelTime)
                {
                    // Tính thời gian còn lại
                    TimeSpan remaining = cancelTime - now;
                    
                    if (remaining.TotalSeconds > 0)
                    {
                        // Hiển thị panel và cập nhật thời gian
                        panelBoDemThoiGian.Visible = true;
                        panelBoDemThoiGian.Text = $"{remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
                        
                        // Đổi màu khi còn ít thời gian (dưới 30 phút)
                        if (remaining.TotalMinutes < 30)
                        {
                            panelBoDemThoiGian.RectColor = Color.FromArgb(239, 68, 68); // Đỏ
                            panelBoDemThoiGian.ForeColor = Color.FromArgb(239, 68, 68);
                        }
                        else
                        {
                            panelBoDemThoiGian.RectColor = Color.FromArgb(245, 158, 11); // Cam
                            panelBoDemThoiGian.ForeColor = Color.FromArgb(245, 158, 11);
                        }

                        if (!_countdownTimer.Enabled)
                        {
                            _countdownTimer.Start();
                        }
                    }
                    else
                    {
                        // Đã hết thời gian
                        panelBoDemThoiGian.Visible = false;
                        _countdownTimer.Stop();
                    }
                }
                else
                {
                    // Đã quá thời gian hủy
                    panelBoDemThoiGian.Visible = false;
                    _countdownTimer.Stop();
                }
            }
            else
            {
                // Ẩn panel khi chưa quá giờ đặt hoặc không phải trạng thái trễ giờ
                panelBoDemThoiGian.Visible = false;
                _countdownTimer.Stop();
            }
        }

        private void SetStatusColor(string status)
        {
            string statusUpper = status.ToUpper();
            
            // Kiểm tra nếu trạng thái là "Trễ giờ đặt" và đã quá giờ đặt
            bool isTreGio = statusUpper.Contains("TRỄ") || statusUpper.Contains("TRỄ GIỜ");
            bool isPassedReservationTime = _reservation != null && _reservation.Date < DateTime.Now;
            
            // Chỉ hiển thị màu cam "trễ giờ" khi trạng thái hiển thị là "Trễ giờ đặt"
            // (được tính toán dựa trên logic thời gian)
            if (statusUpper.Contains("HỦY") || statusUpper.Contains("ĐÃ HỦY"))
            {
                panelTrangThaiBan.Style = UIStyle.Red;
            }
            else if (isTreGio && isPassedReservationTime)
            {
                panelTrangThaiBan.Style = UIStyle.LayuiOrange; // Màu cam để cảnh báo
            }
            else if (statusUpper.Contains("XÁC NHẬN") || statusUpper.Contains("ĐÃ XÁC NHẬN"))
            {
                panelTrangThaiBan.Style = UIStyle.Green;
            }
            else if (statusUpper.Contains("CHỜ") || statusUpper.Contains("CHỜ XÁC NHẬN"))
            {
                panelTrangThaiBan.Style = UIStyle.Orange;
            }
            else if (statusUpper.Contains("ĐÃ ĐẾN") || statusUpper.Contains("ĐẾN"))
            {
                panelTrangThaiBan.Style = UIStyle.Blue;
            }
            else
            {
                panelTrangThaiBan.Style = UIStyle.Gray;
            }
        }

        private void UpdateButtonStates()
        {
            string statusUpper = _reservation?.Status.ToUpper() ?? "";
            
            // Disable buttons if cancelled
            if (statusUpper.Contains("HỦY") || statusUpper.Contains("ĐÃ HỦY"))
            {
                btnChinhSua.Enabled = true;
                btnDaDen.Enabled = false;
                btnHuy.Enabled = false;
            }
            else if (statusUpper.Contains("TRỄ") || statusUpper.Contains("TRỄ GIỜ"))
            {
                // Trễ giờ đặt: vẫn có thể chỉnh sửa và khách vẫn có thể đến
                btnChinhSua.Enabled = true;
                btnDaDen.Enabled = true; // Cho phép xác nhận đã đến ngay cả khi trễ
                btnHuy.Enabled = true;
            }
            else
            {
                btnChinhSua.Enabled = true;
                
                // Đã đến button only enabled for confirmed reservations
                if (statusUpper.Contains("XÁC NHẬN") || statusUpper.Contains("ĐÃ XÁC NHẬN"))
                {
                    btnDaDen.Enabled = true;
                }
                else
                {
                    btnDaDen.Enabled = false;
                }
                
                btnHuy.Enabled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose countdown timer
                if (_countdownTimer != null)
                {
                    _countdownTimer.Stop();
                    _countdownTimer.Dispose();
                    _countdownTimer = null;
                }

                // Dispose components (if any)
                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}

