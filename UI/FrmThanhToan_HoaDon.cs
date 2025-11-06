using BLL;
using QLNhaHangTiecCuoi.Share;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Common;
using UI.Controls;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmThanhToan_HoaDon : Form
    {
        private FlowLayoutPanel? invoicesFlowPanel;
        private readonly DatabaseHelper _db = new DatabaseHelper();
        private readonly HoaDonBLL _hoaDonBLL;

        public FrmThanhToan_HoaDon()
        {
            InitializeComponent();
            _hoaDonBLL = new HoaDonBLL(_db);
            Activated += FrmThanhToan_HoaDon_Activated; // tự làm mới khi trở lại màn hình
            segmentedPill1.SelectedIndexChanged += SegmentedPill1_SelectedIndexChanged;
            // Event handlers cho filter lịch sử thanh toán
            dateTuNgay.ValueChanged += (s, e) => { if (segmentedPill1.SelectedIndex == 1) LoadPaymentHistoryToDataGridView(); };
            dateDenNgay.ValueChanged += (s, e) => { if (segmentedPill1.SelectedIndex == 1) LoadPaymentHistoryToDataGridView(); };
            guna2ComboBox1.SelectedIndexChanged += (s, e) => { if (segmentedPill1.SelectedIndex == 1) LoadPaymentHistoryToDataGridView(); };
            guna2ComboBox2.SelectedIndexChanged += (s, e) => { if (segmentedPill1.SelectedIndex == 1) LoadPaymentHistoryToDataGridView(); };
            // Event handlers cho DataGridView
            if (dgvHoaDon != null)
            {
                dgvHoaDon.CellClick += DgvHoaDon_CellClick;
                dgvHoaDon.CellPainting += DgvHoaDon_CellPainting;
            }
        }

        private void FrmThanhToan_HoaDon_Load(object sender, EventArgs e)
        {
            lbSoHD.Text = "0";
            lbSoGiaoDich.Text = "0";
            lbTongThuHomNay.Text = "0 đ";
            lbSoTienTrungBinh.Text = "0 đ";
            lbSoSanhPhanTram.Text = "0 % vs hôm qua";
            SetupInvoiceListContainer();
            LoadInvoicesFromDb();
            RefreshTopStats();
            // Khởi tạo visibility ban đầu
            SegmentedPill1_SelectedIndexChanged(null, EventArgs.Empty);
            // Mặc định không filter theo ngày để hiển thị tất cả hóa đơn
            dateTuNgay.Checked = false;
            dateDenNgay.Checked = false;
        }
        // Thiết lập container cho danh sách hóa đơn
        private void SetupInvoiceListContainer()
        {
            invoicesFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(8),
            };
            panelDanhSachHoaDon.Controls.Clear();
            panelDanhSachHoaDon.Controls.Add(invoicesFlowPanel);
        }
        // Load hóa đơn từ database
        private void LoadInvoicesFromDb()
        {
            if (invoicesFlowPanel == null)
            {
                SetupInvoiceListContainer();
            }

            invoicesFlowPanel.Controls.Clear();

            var dt = _hoaDonBLL.GetHoaDonList(Session.ChiNhanhId, "CHỜ TT", 100);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                var invoiceItem = new Controls.HoaDonPanel
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.None,
                };

                int id = System.Convert.ToInt32(row["hoa_don_id"]);
                decimal sub = System.Convert.ToDecimal(row["tong_truoc_thue"]);
                decimal vatPercent = System.Convert.ToDecimal(row["vat"]);
                decimal vatValue = System.Math.Round(sub * vatPercent / 100m, 0);
                decimal total = System.Convert.ToDecimal(row["tong_sau_thue"]);
                DateTime ngayLap = System.Convert.ToDateTime(row["ngay_lap"]);

                invoiceItem.TableName = row["loai"].ToString() == "NHAHANG" ? "Nhà hàng" : "Tiệc cưới";
                invoiceItem.GuestsAndDishes = $"HD#{id}";
                invoiceItem.InvoiceCode = $"HD{id}";
                invoiceItem.Subtotal = FormatCurrency(sub);
                invoiceItem.Vat = FormatCurrency(vatValue);
                invoiceItem.Total = FormatCurrency(total);
                try { invoiceItem.SetStartTime(ngayLap.ToLocalTime()); } catch { }

                invoiceItem.Width = invoicesFlowPanel.ClientSize.Width - 40;
                invoiceItem.Height = 189;
                invoiceItem.Margin = new Padding(0, 0, 5, 8);
                invoiceItem.Show();
                invoiceItem.Selected += (_, __) => OnInvoiceSelected(invoiceItem);
                invoicesFlowPanel.Controls.Add(invoiceItem);
            }
        }
        
        // Khi quay lại form (từ màn hình bán hàng), tự reload danh sách hóa đơn
        private void FrmThanhToan_HoaDon_Activated(object? sender, EventArgs e)
        {
            LoadInvoicesFromDb();
            RefreshTopStats();
        }
        // Khi chọn hóa đơn, hiển thị chi tiết thanh toán
        private void OnInvoiceSelected(Controls.HoaDonPanel selected)
        {
            panelHoaDonThanhToan.Controls.Clear();

            var payPanel = new Controls.HoaDonThanhToanPanel
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill,
            };

            panelHoaDonThanhToan.Controls.Add(payPanel);

            // Lấy hoa_don_id từ InvoiceCode (format: "HD{id}")
            int hoaDonId = 0;
            if (!string.IsNullOrWhiteSpace(selected.InvoiceCode))
            {
                string idStr = selected.InvoiceCode.Replace("HD", "").Trim();
                int.TryParse(idStr, out hoaDonId);
            }
            payPanel.HoaDonId = hoaDonId;

            payPanel.PaymentCompleted += (s, e) =>
            {
                LoadInvoicesFromDb();
                RefreshTopStats();
                panelHoaDonThanhToan.Controls.Clear(); // Xóa panel thanh toán sau khi thanh toán thành công
            };

            // tính tổng tiền hóa đơn
            decimal subtotal = ParseCurrency(selected.Total) - ParseCurrency(selected.Vat);
            decimal vatValue = ParseCurrency(selected.Vat);
            decimal total = ParseCurrency(selected.Total);
            decimal vatPercent = 0;
            if (subtotal > 0) vatPercent = System.Math.Round(vatValue * 100m / subtotal, 0);

            payPanel.SetTitle($"Thanh toán - {selected.InvoiceCode}");
            payPanel.BindAmounts(subtotal, vatPercent, total);
            payPanel.Show();
        }

        // Tải và hiển thị thống kê trên các thẻ đầu trang
        private void RefreshTopStats()
        {
            try
            {
                int cn = Session.ChiNhanhId;
                int soHdWaiting = _hoaDonBLL.GetWaitingInvoicesCount(cn);
                var todayStats = _hoaDonBLL.GetPaidStatsOnDateUtc(cn, DateTime.UtcNow.Date);
                var yesterdayStats = _hoaDonBLL.GetPaidStatsOnDateUtc(cn, DateTime.UtcNow.Date.AddDays(-1));

                int soHdToday = todayStats.SoHd;
                decimal tongToday = todayStats.Tong;
                int soHdY = yesterdayStats.SoHd;
                decimal tongY = yesterdayStats.Tong;

                // Số giao dịch = số hóa đơn hôm nay
                int soGiaoDich = soHdToday;
                // Giá trị TB mỗi hóa đơn hôm nay
                decimal giaTriTb = soHdToday > 0 ? Math.Round(tongToday / soHdToday, 0) : 0m;
                // So sánh % với hôm qua (theo tổng doanh thu)
                decimal percent = tongY <= 0 ? 0 : Math.Round((tongToday - tongY) * 100m / tongY, 1);

                // Gán trực tiếp vào các label trên UI
                if (lbSoHD != null) lbSoHD.Text = soHdWaiting.ToString();
                if (lbTongThuHomNay != null) lbTongThuHomNay.Text = FormatShortMoney(tongToday);
                if (lbSoSanhPhanTram != null) lbSoSanhPhanTram.Text = (percent >= 0 ? "+" : "") + percent.ToString("0.0") + "% vs hôm qua";
                if (lbSoGiaoDich != null) lbSoGiaoDich.Text = soGiaoDich.ToString();
                if (lbSoTienTrungBinh != null) lbSoTienTrungBinh.Text = FormatCurrency(giaTriTb);
            }
            catch { /* errors */ }
        }
        // Chuyển đổi text thành số tiền
        private static decimal ParseCurrency(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            var digits = new string(text.Where(ch => char.IsDigit(ch)).ToArray());
            if (decimal.TryParse(digits, out var v)) return v;
            return 0;
        }
        // Chuyển đổi số tiền thành text
        private static string FormatCurrency(decimal value)
        {
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0} đ", value);
        }

        private static string FormatShortMoney(decimal value)
        {
            // Hiển thị 25.5 Triệu, 1.2 Tỷ, hoặc số dạng VNĐ nếu nhỏ
            if (value >= 1_000_000_000m)
            {
                return (value / 1_000_000_000m).ToString("0.#") + " Tỷ";
            }
            if (value >= 1_000_000m)
            {
                return (value / 1_000_000m).ToString("0.#") + " Triệu";
            }
            return FormatCurrency(value);
        }

        // Chuyển đổi giữa danh sách hóa đơn/thanh toán và lịch sử giao dịch
        private void SegmentedPill1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (segmentedPill1.SelectedIndex == 0)
            {
                // Hiển thị danh sách hóa đơn và thanh toán
                panelDanhSachHoaDon.Visible = true;
                panelHoaDonThanhToan.Visible = true;
                panelLichSuGiaoDich.Visible = false;
            }
            else if (segmentedPill1.SelectedIndex == 1)
            {
                // Hiển thị lịch sử giao dịch, ẩn danh sách hóa đơn và thanh toán
                panelDanhSachHoaDon.Visible = false;
                panelHoaDonThanhToan.Visible = false;
                panelLichSuGiaoDich.Visible = true;
                // Load dữ liệu vào DataGridView
                LoadPaymentHistoryToDataGridView();
            }
        }

        // Load lịch sử thanh toán vào dgvHoaDon
        private void LoadPaymentHistoryToDataGridView()
        {
            try
            {
                if (dgvHoaDon == null)
                {
                    MessageBox.Show("dgvHoaDon is null!");
                    return;
                }

                // Xóa dữ liệu cũ
                dgvHoaDon.Rows.Clear();
                
                // Tăng row height để hiển thị 2 buttons dọc
                dgvHoaDon.RowTemplate.Height = 70;

                // Lấy filter từ UI
                DateTime? fromDate = dateTuNgay.Checked ? dateTuNgay.Value.Date : null;
                DateTime? toDate = dateDenNgay.Checked ? dateDenNgay.Value.Date : null;
                string? phuongThuc = guna2ComboBox1.SelectedItem?.ToString();
                if (phuongThuc == "Tất cả phương thức") phuongThuc = null;
                // Lấy dữ liệu từ database
                var dt = _hoaDonBLL.GetPaidInvoicesHistory(
                    Session.ChiNhanhId,
                    fromDate,
                    toDate,
                    phuongThuc,
                    100
                );
                // Thêm dữ liệu vào DataGridView
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        int id = Convert.ToInt32(row["hoa_don_id"]);
                        decimal total = Convert.ToDecimal(row["tong_sau_thue"]);
                        DateTime ngayLap = Convert.ToDateTime(row["ngay_lap"]);
                        string? banSanh = row["ban_sanh"] != DBNull.Value ? row["ban_sanh"].ToString() : "-";
                        string? tenKm = row["ten_km"] != DBNull.Value ? row["ten_km"].ToString() : null;
                        string? maKm = row["ma_km"] != DBNull.Value ? row["ma_km"].ToString() : null;
                        decimal? soTienKm = row["so_tien_km"] != DBNull.Value ? Convert.ToDecimal(row["so_tien_km"]) : null;
                        string? phuongThucTT = row["phuong_thuc_tt"] != DBNull.Value ? row["phuong_thuc_tt"].ToString() : null;
                        string? thuNgan = row["thu_ngan"] != DBNull.Value ? row["thu_ngan"].ToString() : null;
                        string? trangThai = row["trang_thai"] != DBNull.Value ? row["trang_thai"].ToString() : "";

                        // Format khuyến mãi
                        string kmText = "-";
                        if (!string.IsNullOrWhiteSpace(tenKm) || !string.IsNullOrWhiteSpace(maKm))
                        {
                            string kmName = !string.IsNullOrWhiteSpace(maKm) ? maKm : tenKm ?? "";
                            if (soTienKm.HasValue && soTienKm.Value > 0)
                            {
                                kmText = $"{kmName}\n-{FormatCurrency(soTienKm.Value)}";
                            }
                            else
                            {
                                kmText = kmName;
                            }
                        }

                        // Format phương thức thanh toán
                        string phuongThucText = phuongThucTT ?? (trangThai == "CHỜ TT" ? "-" : "Tiền mặt");

                        // Format trạng thái
                        string trangThaiText;
                        if (trangThai == "ĐÃ THANH TOÁN")
                        {
                            trangThaiText = "Hoàn thành";
                        }
                        else if (trangThai == "CHỜ TT")
                        {
                            trangThaiText = "Chờ thanh toán";
                        }
                        else
                        {
                            trangThaiText = trangThai ?? "";
                        }

                        // Thêm row vào DataGridView - map vào đúng các cột theo tên
                        int rowIndex = dgvHoaDon.Rows.Add();
                        dgvHoaDon.Rows[rowIndex].Cells["Column1"].Value = id.ToString("D2"); // Mã HĐ
                        dgvHoaDon.Rows[rowIndex].Cells["Column2"].Value = banSanh ?? "-"; // Bàn/Sảnh
                        dgvHoaDon.Rows[rowIndex].Cells["Column3"].Value = FormatCurrency(total); // Số tiền
                        dgvHoaDon.Rows[rowIndex].Cells["Column4"].Value = kmText; // Khuyến mãi
                        dgvHoaDon.Rows[rowIndex].Cells["Column5"].Value = phuongThucText; // Phương thức
                        dgvHoaDon.Rows[rowIndex].Cells["Column6"].Value = ngayLap.ToLocalTime().ToString("yyyy-MM-dd"); // Ngày
                        dgvHoaDon.Rows[rowIndex].Cells["Column7"].Value = ngayLap.ToLocalTime().ToString("HH:mm"); // Thời gian
                        dgvHoaDon.Rows[rowIndex].Cells["Column8"].Value = thuNgan ?? "-"; // Thu ngân
                        dgvHoaDon.Rows[rowIndex].Cells["Column9"].Value = trangThaiText; // Trạng thái
                        dgvHoaDon.Rows[rowIndex].Cells["Column10"].Value = ""; // Thao tác - để trống, sẽ vẽ buttons
                        dgvHoaDon.Rows[rowIndex].Tag = id; // Lưu HoaDonId vào Tag để dùng cho buttons
                    }
                    catch (Exception rowEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error processing row: {rowEx.Message}");
                        // Tiếp tục với row tiếp theo
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadPaymentHistoryToDataGridView Exception: {ex}");
                GunaToast.Show(this, $"Lỗi tải lịch sử thanh toán: {ex.Message}", UI.Controls.ToastType.Error);
            }
        }

        // Xử lý click vào cột Thao tác
        private void DgvHoaDon_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvHoaDon == null) return;

            if (dgvHoaDon.Columns[e.ColumnIndex].Name == "Column10")
            {
                int hoaDonId = dgvHoaDon.Rows[e.RowIndex].Tag != null ? (int)dgvHoaDon.Rows[e.RowIndex].Tag : 0;
                string trangThai = dgvHoaDon.Rows[e.RowIndex].Cells["Column9"].Value?.ToString() ?? "";

                var cellRect = dgvHoaDon.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                Point mousePos = dgvHoaDon.PointToClient(Cursor.Position);
                int yInCell = mousePos.Y - cellRect.Y;

                int buttonHeight = 30;
                int spacing = 8;
                int totalHeight = buttonHeight * 2 + spacing;
                int startY = Math.Max(4, (cellRect.Height - totalHeight) / 2);

                // Kiểm tra click vào nút In (nút trên)
                if (yInCell >= startY && yInCell < startY + buttonHeight)
                {
                    PrintInvoice(hoaDonId);
                }
                // Kiểm tra click vào nút Hoàn tiền (nút dưới, chỉ nếu trạng thái là "Hoàn thành")
                else if (yInCell >= startY + buttonHeight + spacing && 
                         yInCell < startY + buttonHeight + spacing + buttonHeight && 
                         trangThai == "Hoàn thành")
                {
                    RefundPayment(hoaDonId);
                }
            }
        }

        // Vẽ buttons In và Hoàn tiền trong cột Thao tác (style Guna2Button bo tròn, 2 nút nằm dọc)
        private void DgvHoaDon_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvHoaDon == null) return;

            if (dgvHoaDon.Columns[e.ColumnIndex].Name == "Column10")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                e.Handled = true;

                if (e.CellBounds.Width <= 0 || e.CellBounds.Height <= 0) return;

                string trangThai = dgvHoaDon.Rows[e.RowIndex].Cells["Column9"].Value?.ToString() ?? "";
                bool canRefund = trangThai == "Hoàn thành";

                int buttonWidth = 75;
                int buttonHeight = 30;
                int spacing = 8; // Khoảng cách giữa 2 nút
                int totalHeight = canRefund ? (buttonHeight * 2 + spacing) : buttonHeight;
                int startY = e.CellBounds.Y + Math.Max(4, (e.CellBounds.Height - totalHeight) / 2);
                int centerX = e.CellBounds.X + (e.CellBounds.Width - buttonWidth) / 2;
                
                // Vẽ nút In (nút trên) - luôn hiển thị
                Rectangle printRect = new Rectangle(centerX, startY, buttonWidth, buttonHeight);
                DrawGuna2Button(e.Graphics, printRect, "In", Color.FromArgb(100, 100, 100));

                // Vẽ nút Hoàn tiền (nút dưới, chỉ nếu trạng thái là "Hoàn thành")
                if (canRefund)
                {
                    Rectangle refundRect = new Rectangle(centerX, startY + buttonHeight + spacing, buttonWidth, buttonHeight);
                    DrawGuna2Button(e.Graphics, refundRect, "Hoàn tiền", Color.FromArgb(244, 67, 54));
                }
            }
        }

        // Vẽ button với style Guna2Button bo tròn
        private void DrawGuna2Button(Graphics g, Rectangle rect, string text, Color fillColor)
        {
            int radius = 12; 
            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90); // Top-left
                path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90); // Top-right
                path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90); // Bottom-right
                path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90); // Bottom-left
                path.CloseFigure();

                // Fill background với màu
                using (var brush = new SolidBrush(fillColor))
                {
                    g.FillPath(brush, path);
                }

                // Draw border
                using (var pen = new Pen(fillColor, 1))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Vẽ text
            using (var font = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.White))
            {
                var textSize = g.MeasureString(text, font);
                g.DrawString(text, font, textBrush,
                    rect.X + (rect.Width - textSize.Width) / 2,
                    rect.Y + (rect.Height - textSize.Height) / 2);
            }
        }

        // In hóa đơn
        private void PrintInvoice(int hoaDonId)
        {
            try
            {
                GunaToast.Show(this, $"Đang in hóa đơn HD{hoaDonId:D2}...", UI.Controls.ToastType.Info);
                // TODO: Implement print functionality
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi khi in hóa đơn: {ex.Message}", UI.Controls.ToastType.Error);
            }
        }

        // Hoàn tiền
        private void RefundPayment(int hoaDonId)
        {
            try
            {
                var result = MessageBox.Show(
                    $"Xác nhận hoàn tiền cho hóa đơn HD{hoaDonId:D2}?\n\nThao tác này không thể hoàn tác!",
                    "Xác nhận hoàn tiền",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // TODO: Implement refund functionality
                    GunaToast.Show(this, $"Đã hoàn tiền cho hóa đơn HD{hoaDonId:D2}", UI.Controls.ToastType.Success);
                    LoadPaymentHistoryToDataGridView(); // Refresh danh sách
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi khi hoàn tiền: {ex.Message}", UI.Controls.ToastType.Error);
            }
        }

    }
}
