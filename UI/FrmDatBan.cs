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
using UI.Controls;
using Guna.UI2.WinForms;
using Sunny.UI;
using Reservation = UI.Controls.Reservation;
using ReservationEventArgs = UI.Controls.ReservationEventArgs;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmDatBan : Form
    {
        private BanBLL _banBLL;
        private int _chiNhanhId;
        private System.Windows.Forms.Timer _autoCheckTimer;

        public FrmDatBan()
        {
            InitializeComponent();
            _banBLL = new BanBLL(new DatabaseHelper());
            _chiNhanhId = Session.ChiNhanhId;
            InitializeAutoCheckTimer();
        }

        private void FrmDatBan_Load(object sender, EventArgs e)
        {
            // Kiểm tra ChiNhanhId trước khi load dữ liệu
            if (_chiNhanhId <= 0)
            {
                MessageBox.Show("Chưa chọn chi nhánh! Vui lòng chọn chi nhánh trước.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và cập nhật trạng thái quá hạn ngay khi load form
            KiemTraVaCapNhatDatBanQuaHan();

            LoadDanhSachDatBan();
            LoadThongKe();
            
            panelDanhSachDatBan.Resize += PanelDanhSachDatBan_Resize;
            
            _autoCheckTimer.Start();
        }

        private void KiemTraVaCapNhatDatBanQuaHan()
        {
            try
            {
                // Cập nhật các đặt bàn đã xác nhận nhưng quá giờ đặt sang "TRỄ GIỜ ĐẶT"
                int soDatBanTreGio = _banBLL.CapNhatTrangThaiTreGio();
                if (soDatBanTreGio > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Đã cập nhật {soDatBanTreGio} đặt bàn trễ giờ");
                }

                // Tự động hủy các đặt bàn trễ giờ quá 2 tiếng
                int soDatBanTuDongHuy = _banBLL.TuDongHuyDatBanTreGio();
                if (soDatBanTuDongHuy > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Đã tự động hủy {soDatBanTuDongHuy} đặt bàn quá hạn");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra đặt bàn quá hạn: {ex.Message}", 
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitializeAutoCheckTimer()
        {
            _autoCheckTimer = new System.Windows.Forms.Timer
            {
                Interval = 30000 // Kiểm tra mỗi 30 giây để phản ứng nhanh hơn
            };
            _autoCheckTimer.Tick += AutoCheckTimer_Tick;
        }

        private void AutoCheckTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra và cập nhật trạng thái trễ giờ
                int updated = _banBLL.CapNhatTrangThaiTreGio();
                
                // Tự động hủy các đặt bàn trễ giờ quá 2 tiếng
                int cancelled = _banBLL.TuDongHuyDatBanTreGio();
                
                if (updated > 0 || cancelled > 0)
                {
                    LoadDanhSachDatBan();
                    LoadThongKe();
                }
            }
            catch (Exception ex)
            {
                // Log lỗi
                System.Diagnostics.Debug.WriteLine($"Lỗi auto check timer: {ex.Message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Dừng timer khi đóng form
            if (_autoCheckTimer != null)
            {
                _autoCheckTimer.Stop();
                _autoCheckTimer.Dispose();
            }
            base.OnFormClosing(e);
        }

        private void PanelDanhSachDatBan_Resize(object sender, EventArgs e)
        {
            // Khi container resize, cập nhật width của tất cả các Frm_DatBan controls
            foreach (Control control in panelDanhSachDatBan.Controls)
            {
                if (control is Frm_DatBan frmDatBan)
                {
                    frmDatBan.Width = panelDanhSachDatBan.Width - 40;
                }
            }
        }

        private void LoadDanhSachDatBan()
        {
            try
            {
                panelDanhSachDatBan.Controls.Clear();

                // Trước khi load, kiểm tra lại các đặt bàn quá hạn để đảm bảo dữ liệu cập nhật
                try
                {
                    _banBLL.CapNhatTrangThaiTreGio();
                    _banBLL.TuDongHuyDatBanTreGio();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi kiểm tra quá hạn trong LoadDanhSachDatBan: {ex.Message}");
                }

                var dtDatBan = _banBLL.LayDanhSachDatBan(_chiNhanhId);
                
                if (dtDatBan == null || dtDatBan.Rows.Count == 0)
                {
                    var lblEmpty = new Label
                    {
                        Text = "Chưa có đặt bàn nào",
                        Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        Size = new Size(panelDanhSachDatBan.Width - 20, 50)
                    };
                    panelDanhSachDatBan.Controls.Add(lblEmpty);
                    return;
                }

                var reservations = ConvertDataTableToReservations(dtDatBan);

                foreach (var reservation in reservations)
                {
                    var frmDatBan = new Frm_DatBan
                    {
                        Reservation = reservation,
                        Width = panelDanhSachDatBan.Width - 40
                    };

                    frmDatBan.ReservationClicked += Frm_DatBan_ReservationClicked;
                    frmDatBan.EditClicked += Panel_EditClicked;
                    frmDatBan.ArrivedClicked += Panel_ArrivedClicked;
                    frmDatBan.CancelClicked += Panel_CancelClicked;

                    // thêm vào panel
                    panelDanhSachDatBan.Controls.Add(frmDatBan);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách đặt bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Frm_DatBan_ReservationClicked(object sender, ReservationEventArgs e)
        {
            ShowChiTietDatBan(e.Reservation);
        }

        private void ShowChiTietDatBan(Reservation reservation)
        {
            try
            {
                panelChiTietDatBan.Controls.Clear();

                if (reservation == null)
                {
                    txtThongTinDatBan.Text = "Thông tin đặt bàn . . .";
                    panelChiTietDatBan.Controls.Add(txtThongTinDatBan);
                    return;
                }

                var detailPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(20)
                };

                int yPos = 20;

                var lblTitle = new Label
                {
                    Text = $"Chi tiết đặt bàn {reservation.Code}",
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    Location = new Point(20, yPos),
                    AutoSize = true,
                    ForeColor = Color.Black
                };
                detailPanel.Controls.Add(lblTitle);
                yPos += 40;

                var pnlStatus = new Sunny.UI.UIPanel
                {
                    Location = new Point(20, yPos),
                    Size = new Size(200, 29),
                    Text = reservation.Status,
                    TextAlignment = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                    Radius = 25
                };
                SetStatusStyle(pnlStatus, reservation.Status);
                detailPanel.Controls.Add(pnlStatus);
                yPos += 50;

                var lblKhachHangTitle = new Label
                {
                    Text = "Thông tin khách hàng",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Location = new Point(20, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblKhachHangTitle);
                yPos += 35;

                var lblTenKH = new Label
                {
                    Text = $"Tên khách hàng: {reservation.CustomerName}",
                    Font = new Font("Segoe UI", 11F),
                    Location = new Point(30, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblTenKH);
                yPos += 25;

                var lblSDT = new Label
                {
                    Text = $"Số điện thoại: {reservation.Phone}",
                    Font = new Font("Segoe UI", 11F),
                    Location = new Point(30, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblSDT);
                yPos += 40;

                var lblDatBanTitle = new Label
                {
                    Text = "Thông tin đặt bàn",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    Location = new Point(20, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblDatBanTitle);
                yPos += 35;

                var lblNgayDat = new Label
                {
                    Text = $"Ngày đặt: {reservation.Date:dd/MM/yyyy}",
                    Font = new Font("Segoe UI", 11F),
                    Location = new Point(30, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblNgayDat);
                yPos += 25;

                var lblGioDat = new Label
                {
                    Text = $"Giờ đặt: {reservation.Date:HH:mm}",
                    Font = new Font("Segoe UI", 11F),
                    Location = new Point(30, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblGioDat);
                yPos += 25;

                var lblBan = new Label
                {
                    Text = $"Bàn: {reservation.TableName}",
                    Font = new Font("Segoe UI", 11F),
                    Location = new Point(30, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblBan);
                yPos += 25;

                var lblKhuVuc = new Label
                {
                    Text = $"Khu vực: {(!string.IsNullOrWhiteSpace(reservation.Area) ? reservation.Area : "Chưa có")}",
                    Font = new Font("Segoe UI", 11F),
                    Location = new Point(30, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblKhuVuc);
                yPos += 25;

                var lblSoKhach = new Label
                {
                    Text = $"Số khách: {reservation.Guests} người",
                    Font = new Font("Segoe UI", 11F),
                    Location = new Point(30, yPos),
                    AutoSize = true
                };
                detailPanel.Controls.Add(lblSoKhach);
                yPos += 25;

                // Note section
                if (!string.IsNullOrWhiteSpace(reservation.Note))
                {
                    var lblGhiChuTitle = new Label
                    {
                        Text = "Ghi chú",
                        Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                        Location = new Point(20, yPos),
                        AutoSize = true
                    };
                    detailPanel.Controls.Add(lblGhiChuTitle);
                    yPos += 35;

                    var txtGhiChu = new TextBox
                    {
                        Text = reservation.Note,
                        Font = new Font("Segoe UI", 11F),
                        Location = new Point(30, yPos),
                        Size = new Size(290, 80),
                        Multiline = true,
                        ReadOnly = true,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.FromArgb(253, 249, 241)
                    };
                    detailPanel.Controls.Add(txtGhiChu);
                    yPos += 100;
                }

                panelChiTietDatBan.Controls.Add(detailPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStatusStyle(Sunny.UI.UIPanel panel, string status)
        {
            string statusUpper = status.ToUpper();

            if (statusUpper.Contains("HỦY") || statusUpper.Contains("ĐÃ HỦY"))
            {
                panel.Style = UIStyle.Red;
            }
            else if (statusUpper.Contains("TRỄ") || statusUpper.Contains("TRỄ GIỜ"))
            {
                panel.Style = UIStyle.LayuiOrange; // Màu cam để cảnh báo
            }
            else if (statusUpper.Contains("XÁC NHẬN") || statusUpper.Contains("ĐÃ XÁC NHẬN"))
            {
                panel.Style = UIStyle.Green;
            }
            else if (statusUpper.Contains("CHỜ") || statusUpper.Contains("CHỜ XÁC NHẬN"))
            {
                panel.Style = UIStyle.Orange;
            }
            else if (statusUpper.Contains("ĐÃ ĐẾN") || statusUpper.Contains("ĐẾN"))
            {
                panel.Style = UIStyle.Blue;
            }
            else
            {
                panel.Style = UIStyle.Gray;
            }
        }

        private void LoadThongKe()
        {
            try
            {
                var dtThongKe = _banBLL.LayThongKeDatBan(_chiNhanhId);
                if (dtThongKe != null && dtThongKe.Rows.Count > 0)
                {
                    var row = dtThongKe.Rows[0];
                    label4.Text = row["TongDatBan"].ToString();
                    label7.Text = row["ChoXacNhan"].ToString();
                    label10.Text = row["DaXacNhan"].ToString();
                    label13.Text = row["DaDen"].ToString();
                    label16.Text = row["DaHuy"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<Reservation> ConvertDataTableToReservations(DataTable dt)
        {
            var reservations = new List<Reservation>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string trangThai = row["trang_thai"].ToString();
                    string ghiChu = row["ghi_chu"]?.ToString() ?? "";
                    DateTime ngayGio = Convert.ToDateTime(row["ngay_gio"]);
                    
                    reservations.Add(new Reservation
                    {
                        Code = row["ma_dat_ban"].ToString(),
                        CustomerName = row["ho_ten"].ToString(),
                        Phone = row["sdt"].ToString(),
                        Date = ngayGio,
                        TableName = row["so_ban"].ToString(),
                        Area = row["ten_khu_vuc"]?.ToString() ?? "",
                        Guests = Convert.ToInt32(row["so_khach"]),
                        Status = GetStatusDisplay(trangThai, ghiChu, ngayGio),
                        Note = ghiChu
                    });
                }

                // Sắp xếp: ưu tiên trong ngày (giờ gần nhất), rồi đến các ngày gần nhất
                DateTime today = DateTime.Now.Date;
                reservations = reservations
                    .OrderBy(r => r.Date.Date == today ? 0 : 1) // Trong ngày trước
                    .ThenBy(r => r.Date.Date) // Các ngày gần nhất trước
                    .ThenBy(r => r.Date) // Trong cùng ngày, sắp xếp theo giờ tăng dần
                    .ToList();
            }

            return reservations;
        }

        private string GetStatusDisplay(string status, string ghiChu = "", DateTime? ngayGio = null)
        {
            // Tính toán "Trễ giờ đặt" dựa trên logic thời gian, không dựa vào ghi chú trong database
            if (status.ToUpper() == "ĐÃ XÁC NHẬN" && ngayGio.HasValue)
            {
                DateTime now = DateTime.Now;
                DateTime reservationTime = ngayGio.Value;
                TimeSpan timeDiff = now - reservationTime;
                
                // Nếu đã quá giờ đặt nhưng chưa quá 2 tiếng -> hiển thị "Trễ giờ đặt"
                if (timeDiff.TotalMinutes > 0 && timeDiff.TotalMinutes < 120)
                {
                    return "Trễ giờ đặt";
                }
            }

            switch (status.ToUpper())
            {
                case "CHỜ XÁC NHẬN": return "Chờ xác nhận";
                case "ĐÃ XÁC NHẬN": return "Đã xác nhận";
                case "TRỄ GIỜ ĐẶT": return "Trễ giờ đặt";
                case "ĐÃ HỦY": return "Đã hủy";
                case "ĐÃ PHỤC VỤ": return "Hoàn thành";
                default: return status;
            }
        }

        private void btnTaoDatBanMoi_Click(object sender, EventArgs e)
        {
            Frm_TaoDatBan frm = new Frm_TaoDatBan();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachDatBan();
                LoadThongKe();
            }
        }

        private void Panel_ArrivedClicked(object sender, ReservationEventArgs e)
        {
            try
            {
                // Xác nhận khách hàng đã đến
                var result = MessageBox.Show(
                    $"Xác nhận khách hàng {e.Reservation.CustomerName} đã đến?\n" +
                    $"Bàn: {e.Reservation.TableName} - {e.Reservation.Area}\n" +
                    $"Số khách: {e.Reservation.Guests}\n\n" +
                    $"Sau khi xác nhận sẽ chuyển sang form bán hàng để gọi món.",
                    "Xác nhận đã đến",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Thực hiện xác nhận đã đến
                    bool success = _banBLL.XacNhanDaDen(e.Reservation.Code);

                    if (success)
                    {
                        MessageBox.Show("Đã xác nhận khách hàng đến thành công!\nChuyển sang form bán hàng...", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh dữ liệu
                        LoadDanhSachDatBan();
                        LoadThongKe();

                        // Chuyển sang form bán hàng trong cùng cửa sổ chính
                        var frmTrangChu = this.ParentForm as FrmTrangChu;
                        if (frmTrangChu != null)
                        {
                            frmTrangChu.ShowBanHangWithTable(e.Reservation.TableName, e.Reservation.CustomerName, e.Reservation.Guests);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể xác nhận khách hàng đến. Vui lòng thử lại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xác nhận đã đến: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Panel_CancelClicked(object sender, ReservationEventArgs e)
        {
            try
            {
                if (MessageBox.Show($"Bạn có chắc chắn muốn hủy đặt bàn {e.Reservation.Code}?\n\n" +
                                  $"Khách hàng: {e.Reservation.CustomerName}\n" +
                                  $"SĐT: {e.Reservation.Phone}\n" +
                                  $"Ngày giờ: {e.Reservation.Date:dd/MM/yyyy HH:mm}\n" +
                                  $"Bàn: {e.Reservation.TableName}",
                                  "Xác nhận hủy đặt bàn",
                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Thực hiện hủy đặt bàn
                    bool result = _banBLL.HuyDatBan(e.Reservation.Code);

                    if (result)
                    {
                        MessageBox.Show("Đã hủy đặt bàn thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh dữ liệu
                        LoadDanhSachDatBan();
                        LoadThongKe();
                    }
                    else
                    {
                        MessageBox.Show("Không thể hủy đặt bàn. Vui lòng thử lại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hủy đặt bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Panel_EditClicked(object sender, ReservationEventArgs e)
        {
            try
            {
                using (var frmChinhSua = new Frm_ChinhSuaDatBan(e.Reservation))
                {
                    if (frmChinhSua.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSachDatBan();
                        LoadThongKe();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở form chỉnh sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
