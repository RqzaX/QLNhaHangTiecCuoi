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

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmDatBan : Form
    {
        private BanBLL _banBLL;
        private int _chiNhanhId;
        
        // Simple Reservation class
        public class Reservation
        {
            public string Code { get; set; }
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public DateTime Date { get; set; }
            public string TableName { get; set; }
            public string Area { get; set; }
            public int Guests { get; set; }
            public string Status { get; set; }
        }

        public FrmDatBan()
        {
            InitializeComponent();
            _banBLL = new BanBLL(new DatabaseHelper());
            _chiNhanhId = Session.ChiNhanhId;
        }

        private void FrmDatBan_Load(object sender, EventArgs e)
        {
            // Kiểm tra ChiNhanhId trước khi load dữ liệu
            if (_chiNhanhId <= 0)
            {
                MessageBox.Show("Chưa chọn chi nhánh! Vui lòng chọn chi nhánh trước.", 
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // Test với dữ liệu mẫu để kiểm tra UI
                TestWithSampleData();
                return;
            }
            
            LoadDanhSachDatBan();
            LoadThongKe();
            HookEvents();
        }

        private void HookEvents()
        {
            panelDanhSachDatBan.ViewClicked += PanelDanhSachDatBan_ViewClicked;
            panelDanhSachDatBan.ConfirmClicked += PanelDanhSachDatBan_ConfirmClicked;
            panelDanhSachDatBan.ArrivedClicked += PanelDanhSachDatBan_ArrivedClicked;
            panelDanhSachDatBan.EditClicked += PanelDanhSachDatBan_EditClicked;
            panelDanhSachDatBan.CancelClicked += PanelDanhSachDatBan_CancelClicked;
        }

        private void LoadDanhSachDatBan()
        {
            try
            {
                var dtDatBan = _banBLL.LayDanhSachDatBan(_chiNhanhId);
                var reservations = ConvertDataTableToReservations(dtDatBan);
                panelDanhSachDatBan.SetData(reservations);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách đặt bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private List<DataGripView_DatBan.Reservation> ConvertDataTableToReservations(DataTable dt)
        {
            var reservations = new List<DataGripView_DatBan.Reservation>();
            
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    reservations.Add(new DataGripView_DatBan.Reservation
                    {
                        Code = row["ma_dat_ban"].ToString(),
                        CustomerName = row["ho_ten"].ToString(),
                        Phone = row["sdt"].ToString(),
                        Date = Convert.ToDateTime(row["ngay_gio"]),
                        TableName = row["so_ban"].ToString(),
                        Area = row["ten_khu_vuc"].ToString(),
                        Guests = Convert.ToInt32(row["so_khach"]),
                        Status = GetStatusDisplay(row["trang_thai"].ToString()),
                        Deposit = 0 // Bỏ tiền cọc vì không có trong database
                    });
                }
            }
            
            return reservations;
        }

        private string GetStatusDisplay(string status)
        {
            switch (status.ToUpper())
            {
                case "CHỜ XÁC NHẬN": return "Chờ xác nhận";
                case "ĐÃ XÁC NHẬN": return "Đã xác nhận";
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

        // Method test để debug
        private void TestWithSampleData()
        {
            try
            {
                // Tạo dữ liệu test từ hình ảnh bạn cung cấp
                var testReservations = new List<DataGripView_DatBan.Reservation>
                {
                    new DataGripView_DatBan.Reservation
                    {
                        Code = "DB001",
                        CustomerName = "Nguyễn Văn A",
                        Phone = "0123456789",
                        Date = new DateTime(2025, 1, 15, 10, 0, 0),
                        TableName = "Bàn 12",
                        Area = "Tầng 1",
                        Guests = 4,
                        Status = "Chờ xác nhận",
                        Deposit = 0
                    },
                    new DataGripView_DatBan.Reservation
                    {
                        Code = "DB002",
                        CustomerName = "Trần Thị B",
                        Phone = "0987654321",
                        Date = new DateTime(2025, 1, 15, 14, 30, 0),
                        TableName = "Bàn 8",
                        Area = "Tầng 2",
                        Guests = 6,
                        Status = "Đã xác nhận",
                        Deposit = 500000
                    },
                    new DataGripView_DatBan.Reservation
                    {
                        Code = "DB003",
                        CustomerName = "Lê Văn C",
                        Phone = "0369258147",
                        Date = new DateTime(2025, 1, 15, 18, 0, 0),
                        TableName = "Bàn 15",
                        Area = "Tầng 1",
                        Guests = 2,
                        Status = "Đã hủy",
                        Deposit = 0
                    }
                };
                
                System.Diagnostics.Debug.WriteLine("DEBUG: Test với dữ liệu mẫu");
                panelDanhSachDatBan.SetData(testReservations);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi test: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PanelDanhSachDatBan_ViewClicked(object sender, DataGripView_DatBan.ReservationEventArgs e)
        {
            try
            {
                MessageBox.Show($"Mã: {e.Reservation.Code}\n" +
                              $"Khách hàng: {e.Reservation.CustomerName}\n" +
                              $"SĐT: {e.Reservation.Phone}\n" +
                              $"Ngày giờ: {e.Reservation.Date:dd/MM/yyyy HH:mm}\n" +
                              $"Bàn: {e.Reservation.TableName}\n" +
                              $"Khu vực: {e.Reservation.Area}\n" +
                              $"Số khách: {e.Reservation.Guests}\n" +
                              $"Trạng thái: {e.Reservation.Status}\n",
                              "Chi tiết đặt bàn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xem chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PanelDanhSachDatBan_ConfirmClicked(object sender, DataGripView_DatBan.ReservationEventArgs e)
        {
            try
            {
                // Xác nhận đặt bàn
                var result = MessageBox.Show(
                    $"Xác nhận đặt bàn {e.Reservation.Code}?\n" +
                    $"Khách hàng: {e.Reservation.CustomerName}\n" +
                    $"Bàn: {e.Reservation.TableName} - {e.Reservation.Area}\n" +
                    $"Số khách: {e.Reservation.Guests}\n" +
                    $"Ngày: {e.Reservation.Date:dd/MM/yyyy HH:mm}",
                    "Xác nhận đặt bàn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Thực hiện xác nhận đặt bàn
                    bool success = _banBLL.XacNhanDatBan(e.Reservation.Code);

                    if (success)
                    {
                        MessageBox.Show("Đã xác nhận đặt bàn thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh dữ liệu
                        LoadDanhSachDatBan();
                        LoadThongKe();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xác nhận đặt bàn. Vui lòng thử lại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xác nhận đặt bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PanelDanhSachDatBan_ArrivedClicked(object sender, DataGripView_DatBan.ReservationEventArgs e)
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


        private void PanelDanhSachDatBan_CancelClicked(object sender, DataGripView_DatBan.ReservationEventArgs e)
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

        private void PanelDanhSachDatBan_EditClicked(object sender, DataGripView_DatBan.ReservationEventArgs e)
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
