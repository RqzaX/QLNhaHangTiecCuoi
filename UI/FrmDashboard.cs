using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using UI.Common;
using UI.Controls;

namespace UI
{
    public partial class FrmDashboard : Form
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly HoaDonBLL _hoaDonBLL;
        private readonly DatSanhBLL _datSanhBLL;
        private readonly BanBLL _banBLL;
        private readonly NguyenLieuBLL _nguyenLieuBLL;
        private ToolTip _tooltip;
        private int _hoveredBarIndex = -1;
        private List<BarInfo> _barInfoList = new List<BarInfo>();

        private class BarInfo
        {
            public Rectangle Rect { get; set; }
            public decimal Value { get; set; }
            public string DayName { get; set; }
            public DateTime Date { get; set; }
        }

        public FrmDashboard()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _hoaDonBLL = new HoaDonBLL(_dbHelper);
            _datSanhBLL = new DatSanhBLL();
            _banBLL = new BanBLL(_dbHelper);
            _nguyenLieuBLL = new NguyenLieuBLL(_dbHelper);
            
            _tooltip = new ToolTip();
            _tooltip.IsBalloon = false;
            _tooltip.AutoPopDelay = 5000;
            _tooltip.InitialDelay = 200;
            _tooltip.ReshowDelay = 100;
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    return;
                }

                LoadDoanhThuHomNay(chiNhanhId);
                LoadSuKienHomNay(chiNhanhId);
                LoadBanDangPhucVu(chiNhanhId);
                LoadHoaDonCho(chiNhanhId);
                // Vẽ biểu đồ doanh thu 7 ngày
                DrawRevenueChart(chiNhanhId);
                // Vẽ biểu đồ tỷ lệ sử dụng sảnh
                DrawHallUsageChart(chiNhanhId);
                // Load top món bán chạy trong tháng
                LoadTopMonBanChay(chiNhanhId);
                // Load danh sách đặt bàn/sảnh hôm nay
                LoadDanhSachDatBanHomNay(chiNhanhId);
                // Load cảnh báo tồn kho
                LoadCanhBaoTonKho(chiNhanhId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu dashboard: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDoanhThuHomNay(int chiNhanhId)
        {
            try
            {
                var (homNay, homQua) = _hoaDonBLL.GetRevenueTodayAndYesterday(chiNhanhId);
                
                lblSoTienDanhThuHomNay.Text = FormatMoney(homNay);
                
                // Tính phần trăm so với hôm qua
                if (homQua > 0)
                {
                    decimal tyLe = ((homNay - homQua) / homQua) * 100;
                    lblTyLeSoVoiHomQua.Text = $"{tyLe:+#0.0;-#0.0;0}%";
                    lblTyLeSoVoiHomQua.ForeColor = tyLe >= 0 ? Color.MediumSeaGreen : Color.IndianRed;
                }
                else
                {
                    lblTyLeSoVoiHomQua.Text = homNay > 0 ? "+100%" : "0%";
                    lblTyLeSoVoiHomQua.ForeColor = Color.MediumSeaGreen;
                }
            }
            catch (Exception ex)
            {
                lblSoTienDanhThuHomNay.Text = "0 ₫";
                lblTyLeSoVoiHomQua.Text = "0%";
            }
        }

        private void LoadSuKienHomNay(int chiNhanhId)
        {
            try
            {
                int soSuKien = _datSanhBLL.LaySoSuKienHomNay(chiNhanhId);
                lblSuKienDatSanh.Text = $"{soSuKien} sự kiện";
            }
            catch (Exception ex)
            {
                lblSuKienDatSanh.Text = "0 sự kiện";
            }
        }

        private void LoadBanDangPhucVu(int chiNhanhId)
        {
            try
            {
                var (soBan, tongKhach, tongBan) = _banBLL.GetBanDangPhucVuInfo(chiNhanhId);
                lblSoBan_TongSoBan.Text = $"{soBan}/{tongBan} bàn";
                lblKhachDangPhucVu.Text = $"{tongKhach} khách";
            }
            catch (Exception ex)
            {
                lblSoBan_TongSoBan.Text = "0/0 bàn";
                lblKhachDangPhucVu.Text = "0 khách";
            }
        }

        private void LoadHoaDonCho(int chiNhanhId)
        {
            try
            {
                int soHoaDon = _hoaDonBLL.GetWaitingInvoicesCount(chiNhanhId);
                lblSoHoaDonChoXuLy.Text = $"{soHoaDon} hóa đơn";
            }
            catch (Exception ex)
            {
                lblSoHoaDonChoXuLy.Text = "0 hóa đơn";
            }
        }

        private string FormatMoney(decimal amount)
        {
            return $"{amount:N0}".Replace(",", ".") + " ₫";
        }

        private void DrawRevenueChart(int chiNhanhId)
        {
            try
            {
                panelBieuDoDoanhThu.Paint -= PanelBieuDoDoanhThu_Paint;
                panelBieuDoDoanhThu.Paint += PanelBieuDoDoanhThu_Paint;
                panelBieuDoDoanhThu.MouseMove -= PanelBieuDoDoanhThu_MouseMove;
                panelBieuDoDoanhThu.MouseMove += PanelBieuDoDoanhThu_MouseMove;
                panelBieuDoDoanhThu.MouseLeave -= PanelBieuDoDoanhThu_MouseLeave;
                panelBieuDoDoanhThu.MouseLeave += PanelBieuDoDoanhThu_MouseLeave;
                
                DataTable dt = _hoaDonBLL.GetRevenue7Days(chiNhanhId);
                panelBieuDoDoanhThu.Tag = dt;
                panelBieuDoDoanhThu.Invalidate();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
            }
        }

        private void PanelBieuDoDoanhThu_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            var dt = panel.Tag as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                // Vẽ text "Không có dữ liệu"
                using (var font = new Font("Segoe UI", 12f))
                using (var brush = new SolidBrush(Color.Gray))
                {
                    var text = "Không có dữ liệu";
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, 
                        (panel.Width - size.Width) / 2, 
                        (panel.Height - size.Height) / 2);
                }
                _barInfoList.Clear();
                return;
            }

            // Tính toán kích thước
            int padding = 40;
            int chartWidth = panel.Width - padding * 2;
            int chartHeight = panel.Height - padding * 2;
            int barWidth = chartWidth / (dt.Rows.Count + 1);
            int maxBarHeight = chartHeight - 30;

            // Tìm giá trị max để scale
            decimal maxValue = 0;
            foreach (DataRow row in dt.Rows)
            {
                decimal value = Convert.ToDecimal(row["doanh_thu"] ?? 0);
                if (value > maxValue) maxValue = value;
            }
            if (maxValue == 0) maxValue = 1000000; // Default scale

            // Vẽ grid và labels
            DrawChartGrid(g, panel, padding, chartHeight, maxValue);

            // Lưu thông tin các cột để detect hover
            _barInfoList.Clear();

            // Vẽ các cột
            int x = padding;
            int dayIndex = 0;
            
            foreach (DataRow row in dt.Rows)
            {
                decimal value = Convert.ToDecimal(row["doanh_thu"] ?? 0);
                int barHeight = maxValue > 0 ? (int)((value / maxValue) * maxBarHeight) : 0;
                
                int xPos = x + dayIndex * barWidth;
                int yPos = padding + maxBarHeight - barHeight;
                
                // Lưu thông tin cột
                DateTime date = row["ngay"] != DBNull.Value ? Convert.ToDateTime(row["ngay"]) : DateTime.Now;
                string dayName = FormatDayLabel(date);
                
                _barInfoList.Add(new BarInfo
                {
                    Rect = new Rectangle(xPos + 10, yPos, barWidth - 20, barHeight),
                    Value = value,
                    DayName = dayName,
                    Date = date
                });
                
                // Vẽ cột với màu khác nếu đang hover
                Color barColor = (_hoveredBarIndex == dayIndex) 
                    ? Color.FromArgb(37, 99, 235) // Màu sáng hơn khi hover
                    : Color.FromArgb(59, 130, 246);
                
                using (var brush = new SolidBrush(barColor))
                {
                    g.FillRectangle(brush, xPos + 10, yPos, barWidth - 20, barHeight);
                }

                // Vẽ viền khi hover
                if (_hoveredBarIndex == dayIndex)
                {
                    using (var pen = new Pen(Color.FromArgb(29, 78, 216), 2f))
                    {
                        g.DrawRectangle(pen, xPos + 10, yPos, barWidth - 20, barHeight);
                    }
                }

                // Vẽ label ngày (hiển thị dd/MM)
                using (var font = new Font("Segoe UI", 9f))
                using (var brush = new SolidBrush(Color.Black))
                {
                    var size = g.MeasureString(dayName, font);
                    g.DrawString(dayName, font, brush, 
                        xPos + (barWidth - size.Width) / 2, 
                        padding + maxBarHeight + 5);
                }

                // Vẽ giá trị trên cột (nếu đủ chỗ)
                if (barHeight > 20)
                {
                    using (var font = new Font("Segoe UI", 8f))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        string valueText = FormatMoneyShort(value);
                        var size = g.MeasureString(valueText, font);
                        g.DrawString(valueText, font, brush, 
                            xPos + (barWidth - size.Width) / 2, 
                            yPos - size.Height - 2);
                    }
                }

                dayIndex++;
            }
        }

        private void PanelBieuDoDoanhThu_MouseMove(object sender, MouseEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            // Tìm cột đang được hover
            int hoveredIndex = -1;
            for (int i = 0; i < _barInfoList.Count; i++)
            {
                if (_barInfoList[i].Rect.Contains(e.Location))
                {
                    hoveredIndex = i;
                    break;
                }
            }

            // Nếu thay đổi cột hover, vẽ lại
            if (hoveredIndex != _hoveredBarIndex)
            {
                _hoveredBarIndex = hoveredIndex;
                panel.Invalidate();

                // Hiển thị tooltip
                if (hoveredIndex >= 0 && hoveredIndex < _barInfoList.Count)
                {
                    var barInfo = _barInfoList[hoveredIndex];
                    string dayOfWeek = GetDayOfWeekVietnamese(barInfo.Date);
                    string tooltipText = $"{dayOfWeek} ({barInfo.DayName}): {FormatMoney(barInfo.Value)}";
                    _tooltip.SetToolTip(panel, tooltipText);
                }
                else
                {
                    _tooltip.SetToolTip(panel, "");
                }
            }
        }

        private void PanelBieuDoDoanhThu_MouseLeave(object sender, EventArgs e)
        {
            _hoveredBarIndex = -1;
            var panel = sender as Panel;
            if (panel != null)
            {
                panel.Invalidate();
                _tooltip.SetToolTip(panel, "");
            }
        }

        private void DrawChartGrid(Graphics g, Panel panel, int padding, int chartHeight, decimal maxValue)
        {
            // Vẽ trục Y với các mốc
            int numLines = 6;
            using (var pen = new Pen(Color.FromArgb(230, 230, 230), 1f))
            using (var font = new Font("Segoe UI", 8f))
            using (var brush = new SolidBrush(Color.Gray))
            {
                for (int i = 0; i <= numLines; i++)
                {
                    int y = padding + (int)((chartHeight - padding) * (numLines - i) / numLines);
                    decimal value = maxValue * i / numLines;
                    
                    // Vẽ đường grid
                    g.DrawLine(pen, padding, y, panel.Width - padding, y);
                    
                    // Vẽ label giá trị
                    string valueText = FormatMoneyShort(value);
                    var size = g.MeasureString(valueText, font);
                    g.DrawString(valueText, font, brush, padding - size.Width - 5, y - size.Height / 2);
                }
            }
        }

        private string FormatMoneyShort(decimal amount)
        {
            if (amount >= 1000000)
                return $"{amount / 1000000:F1}M";
            else if (amount >= 1000)
                return $"{amount / 1000:F0}K";
            else
                return amount.ToString("F0");
        }

        private string FormatDayLabel(DateTime date)
        {
            // Hiển thị dd/MM (ví dụ: 15/12)
            return date.ToString("dd/MM");
        }

        private string GetDayOfWeekVietnamese(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday: return "Thứ 2";
                case DayOfWeek.Tuesday: return "Thứ 3";
                case DayOfWeek.Wednesday: return "Thứ 4";
                case DayOfWeek.Thursday: return "Thứ 5";
                case DayOfWeek.Friday: return "Thứ 6";
                case DayOfWeek.Saturday: return "Thứ 7";
                case DayOfWeek.Sunday: return "Chủ nhật";
                default: return "";
            }
        }

        private void DrawHallUsageChart(int chiNhanhId)
        {
            try
            {
                panelThongKeTyLeSuDungSanh.Paint -= PanelThongKeTyLeSuDungSanh_Paint;
                panelThongKeTyLeSuDungSanh.Paint += PanelThongKeTyLeSuDungSanh_Paint;
                
                int tongSanh = _datSanhBLL.LayTongSoSanhTheoChiNhanh(chiNhanhId);
                int daDat = _datSanhBLL.LaySoSanhDaDatThangNay(chiNhanhId);
                
                panelThongKeTyLeSuDungSanh.Tag = new { TongSanh = tongSanh, DaDat = daDat };
                panelThongKeTyLeSuDungSanh.Invalidate();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
            }
        }

        private void PanelThongKeTyLeSuDungSanh_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            var data = panel.Tag as dynamic;
            if (data == null)
            {
                return;
            }

            int tongSanh = data.TongSanh;
            int daDat = data.DaDat;
            int conTrong = tongSanh - daDat;

            if (tongSanh == 0)
            {
                using (var font = new Font("Segoe UI", 12f))
                using (var brush = new SolidBrush(Color.Gray))
                {
                    var text = "Không có dữ liệu";
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, 
                        (panel.Width - size.Width) / 2, 
                        (panel.Height - size.Height) / 2);
                }
                return;
            }

            // Tính toán vị trí và kích thước biểu đồ tròn
            int centerX = panel.Width / 2;
            int centerY = panel.Height / 2 - 20;
            int radius = Math.Min(panel.Width, panel.Height) / 3;
            var rect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);

            // Tính góc
            float daDatAngle = (float)(daDat * 360.0 / tongSanh);
            float conTrongAngle = 360f - daDatAngle;

            // Vẽ phần "Đã đặt"
            if (daDatAngle > 0)
            {
                using (var brush = new SolidBrush(Color.FromArgb(34, 197, 94)))
                {
                    g.FillPie(brush, rect, 0, daDatAngle);
                }
            }

            // Vẽ phần "Còn trống"
            if (conTrongAngle > 0)
            {
                using (var brush = new SolidBrush(Color.FromArgb(229, 231, 235)))
                {
                    g.FillPie(brush, rect, daDatAngle, conTrongAngle);
                }
            }

            // Vẽ viền
            using (var pen = new Pen(Color.White, 3f))
            {
                g.DrawPie(pen, rect, 0, daDatAngle);
                g.DrawPie(pen, rect, daDatAngle, conTrongAngle);
            }

            // Vẽ text ở giữa
            float percent = tongSanh > 0 ? (float)(daDat * 100.0 / tongSanh) : 0;
            using (var font = new Font("Segoe UI", 20f, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Black))
            {
                string text = $"{percent:F0}%";
                var size = g.MeasureString(text, font);
                g.DrawString(text, font, brush, 
                    centerX - size.Width / 2, 
                    centerY - size.Height / 2);
            }

            // Vẽ thông tin bên dưới
            int yPos = centerY + radius + 30;
            using (var font = new Font("Segoe UI", 11f))
            {
                // Tổng sảnh
                using (var brush = new SolidBrush(Color.Black))
                {
                    string text = $"Tổng sảnh: {tongSanh} sảnh";
                    g.DrawString(text, font, brush, 20, yPos);
                }

                // Đã đặt
                using (var brush = new SolidBrush(Color.FromArgb(34, 197, 94)))
                {
                    string text = $"Đã đặt: {daDat} sảnh ({percent:F0}%)";
                    g.DrawString(text, font, brush, 20, yPos + 25);
                }

                // Còn trống
                if (conTrong > 0)
                {
                    float percentTrong = (float)(conTrong * 100.0 / tongSanh);
                    using (var brush = new SolidBrush(Color.FromArgb(107, 114, 128)))
                    {
                        string text = $"Còn trống: {conTrong} sảnh ({percentTrong:F0}%)";
                        g.DrawString(text, font, brush, 20, yPos + 50);
                    }
                }
            }
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        public void RefreshData()
        {
            LoadDashboardData();
        }

        // Tìm parent form theo kiểu T bằng cách đi lên cây control
        private T? FindParentForm<T>(Control control) where T : Form
        {
            Control? parent = control.Parent;
            while (parent != null)
            {
                if (parent is T form)
                {
                    return form;
                }
                parent = parent.Parent;
            }

            Form? topLevel = control.FindForm();
            if (topLevel is T topLevelForm)
            {
                return topLevelForm;
            }

            if (control is Form formControl && formControl.MdiParent is T mdiParent)
            {
                return mdiParent;
            }

            return null;
        }

        private void LoadTopMonBanChay(int chiNhanhId)
        {
            try
            {
                DataTable dt = _hoaDonBLL.GetTop5MonBanChayTrongThang(chiNhanhId);
                
                // Danh sách các control TopMonBanChay
                var controls = new[] { topMonBanChay1, topMonBanChay2, topMonBanChay3, topMonBanChay4, topMonBanChay5 };
                
                foreach (var ctrl in controls)
                {
                    ctrl.Visible = false;
                }
                
                for (int i = 0; i < Math.Min(dt.Rows.Count, controls.Length); i++)
                {
                    DataRow row = dt.Rows[i];
                    var ctrl = controls[i];
                    
                    ctrl.Rank = i + 1;
                    ctrl.Title = row["ten_hang"]?.ToString() ?? "";
                    ctrl.Orders = Convert.ToInt32(row["so_lan_goi"] ?? 0);
                    ctrl.Revenue = Convert.ToDecimal(row["tong_tien"] ?? 0);
                    ctrl.Visible = true;
                }
            }
            catch (Exception ex)
            {
                var controls = new[] { topMonBanChay1, topMonBanChay2, topMonBanChay3, topMonBanChay4, topMonBanChay5 };
                foreach (var ctrl in controls)
                {
                    ctrl.Visible = false;
                }
            }
        }

        private void LoadDanhSachDatBanHomNay(int chiNhanhId)
        {
            try
            {
                // Xóa tất cả control cũ trong panel
                panelDanhSachDatBanHomNay.Controls.Clear();
                
                // Lấy danh sách đặt bàn hôm nay
                DataTable dtBan = _banBLL.LayDanhSachDatBanHomNay(chiNhanhId);
                // Lấy danh sách đặt sảnh hôm nay
                DataTable dtSanh = _datSanhBLL.LayDanhSachDatSanhHomNay(chiNhanhId);
                
                if ((dtBan == null || dtBan.Rows.Count == 0) && 
                    (dtSanh == null || dtSanh.Rows.Count == 0))
                {
                    return;
                }
                
                // Tạo DataTable để gộp dữ liệu
                DataTable dtAll = null;
                if (dtBan != null && dtBan.Rows.Count > 0)
                {
                    dtAll = dtBan.Clone();
                    foreach (DataRow row in dtBan.Rows)
                    {
                        dtAll.ImportRow(row);
                    }
                }
                
                if (dtSanh != null && dtSanh.Rows.Count > 0)
                {
                    if (dtAll == null)
                    {
                        dtAll = dtSanh.Clone();
                    }
                    foreach (DataRow row in dtSanh.Rows)
                    {
                        dtAll.ImportRow(row);
                    }
                }
                
                if (dtAll == null || dtAll.Rows.Count == 0)
                {
                    return; // Không có dữ liệu sau khi gộp
                }
                
                // Sắp xếp theo thời gian
                DataView dv = dtAll.DefaultView;
                dv.Sort = "ngay_gio ASC";
                dtAll = dv.ToTable();
                
                int yPos = 3;
                int spacing = 83; // 77 height + 6 margin
                
                for (int i = 0; i < dtAll.Rows.Count; i++)
                {
                    DataRow row = dtAll.Rows[i];
                    
                    var ctrl = new UI.Controls.DanhSachDatBanPanel();
                    ctrl.Location = new Point(13, yPos);
                    ctrl.Size = new Size(440, 77);
                    ctrl.Name = $"danhSachDatBanPanel_{i}";
                    
                    DateTime ngayGio = Convert.ToDateTime(row["ngay_gio"]);
                    ctrl.TimeText = ngayGio.ToString("HH:mm");
                    ctrl.CustomerName = row["ho_ten"]?.ToString() ?? "";
                    ctrl.TypeText = row["loai_su_kien"]?.ToString() ?? "";
                    ctrl.TableText = row["thong_tin"]?.ToString() ?? "";
                    ctrl.GuestCount = Convert.ToInt32(row["so_khach"] ?? 0);
                    
                    // Map trạng thái
                    string trangThai = row["trang_thai"]?.ToString() ?? "";
                    if (trangThai == "ĐÃ XÁC NHẬN")
                    {
                        ctrl.Status = UI.Controls.ReservationStatus.DaXacNhan;
                    }
                    else if (trangThai == "CHỜ XÁC NHẬN")
                    {
                        ctrl.Status = UI.Controls.ReservationStatus.ChoXacNhan;
                    }
                    else if (trangThai == "ĐÃ HỦY")
                    {
                        ctrl.Status = UI.Controls.ReservationStatus.DaHuy;
                    }
                    else
                    {
                        ctrl.Status = UI.Controls.ReservationStatus.ChoXacNhan;
                    }
                    
                    panelDanhSachDatBanHomNay.Controls.Add(ctrl);
                    
                    yPos += spacing;
                }
            }
            catch (Exception ex)
            {
                panelDanhSachDatBanHomNay.Controls.Clear();
            }
        }

        private void LoadCanhBaoTonKho(int chiNhanhId)
        {
            try
            {
                // Xóa tất cả control cũ trong panel
                panelDanhSachCanhBaoTonKho.Controls.Clear();

                // Lấy danh sách tồn kho
                DataTable dtTonKho = _nguyenLieuBLL.LayDanhSachTonKho(chiNhanhId);
                
                if (dtTonKho == null || dtTonKho.Rows.Count == 0)
                {
                    return;
                }

                // Lọc các nguyên liệu cần cảnh báo
                var canhBaoList = new List<DataRow>();
                
                foreach (DataRow row in dtTonKho.Rows)
                {
                    decimal slTon = Convert.ToDecimal(row["sl_ton"] ?? 0);
                    decimal tonToiThieu = Convert.ToDecimal(row["ton_toi_thieu"] ?? 0);
                    
                    if (tonToiThieu > 0)
                    {
                        if (slTon < tonToiThieu)
                        {
                            canhBaoList.Add(row);
                        }
                        else if (slTon >= tonToiThieu && slTon <= tonToiThieu * 1.5m)
                        {
                            canhBaoList.Add(row);
                        }
                    }
                }

                // Sắp xếp: Danger trước, Warning sau
                canhBaoList = canhBaoList.OrderBy(r =>
                {
                    decimal slTon = Convert.ToDecimal(r["sl_ton"] ?? 0);
                    decimal tonToiThieu = Convert.ToDecimal(r["ton_toi_thieu"] ?? 0);
                    return slTon < tonToiThieu ? 0 : 1; // 0 = Danger, 1 = Warning
                }).ThenBy(r => Convert.ToDecimal(r["sl_ton"] ?? 0)).ToList();

                if (canhBaoList.Count == 0)
                {
                    return;
                }

                // Tạo các control cảnh báo
                int yPos = 3;
                int spacing = 86; // 80 height + 6 margin
                int panelWidth = panelDanhSachCanhBaoTonKho.Width - 20; // Trừ padding

                foreach (DataRow row in canhBaoList)
                {
                    decimal slTon = Convert.ToDecimal(row["sl_ton"] ?? 0);
                    decimal tonToiThieu = Convert.ToDecimal(row["ton_toi_thieu"] ?? 0);
                    string tenNL = row["ten_nl"]?.ToString() ?? "";
                    string donVi = row["don_vi"]?.ToString() ?? "";
                    
                    // Xác định mức cảnh báo
                    AlertLevel level = slTon < tonToiThieu ? AlertLevel.Danger : AlertLevel.Warning;

                    // Tạo control
                    var canhBao = new CanhBaoToanKho
                    {
                        Location = new Point(10, yPos),
                        Size = new Size(panelWidth, 80),
                        ItemName = tenNL,
                        Stock = (double)slTon,
                        MinStock = (double)tonToiThieu,
                        Unit = donVi,
                        Level = level
                    };

                    // Đăng ký sự kiện click nút "Nhập kho"
                    int nlId = Convert.ToInt32(row["nl_id"]);
                    canhBao.ImportClicked += (sender, e) =>
                    {
                        try
                        {
                            // Tìm FrmTrangChu và mở FrmKho trong panelChinh
                            FrmTrangChu? trangChu = FindParentForm<FrmTrangChu>(this);
                            if (trangChu != null)
                            {
                                trangChu.ShowChild<FrmKho>();
                            }
                            else
                            {
                                FrmKho frm = new FrmKho();
                                frm.ShowDialog(this);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi mở form nhập kho: {ex.Message}", "Lỗi", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    };

                    panelDanhSachCanhBaoTonKho.Controls.Add(canhBao);
                    yPos += spacing;
                }
            }
            catch (Exception ex)
            {
                panelDanhSachCanhBaoTonKho.Controls.Clear();
                // Log lỗi để debug
                System.Diagnostics.Debug.WriteLine($"Lỗi load cảnh báo tồn kho: {ex.Message}");
            }
        }
    }
}
