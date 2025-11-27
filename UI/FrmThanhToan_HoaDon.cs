using BLL;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
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
using UI.Reporting;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmThanhToan_HoaDon : Form
    {
        private FlowLayoutPanel? invoicesFlowPanel;
        private readonly DatabaseHelper _db = new DatabaseHelper();
        private readonly HoaDonBLL _hoaDonBLL;
        private System.Windows.Forms.Timer? _autoRefreshTimer;
        private bool _isRefreshing;
        private DateTime _lastInvoicesInteractionUtc;
        private string? _lastInvoicesHash;
        private string? _currentInvoiceLoai = "NHAHANG";

        public FrmThanhToan_HoaDon()
        {
            InitializeComponent();
            _hoaDonBLL = new HoaDonBLL(_db);
            Activated += FrmThanhToan_HoaDon_Activated; // tự làm mới khi trở lại màn hình
            segmentedPill1.SelectedIndexChanged += SegmentedPill1_SelectedIndexChanged;
            // Event handlers lịch sử thanh toán
            dateTuNgay.ValueChanged += (s, e) => { if (segmentedPill1.SelectedIndex == 2) LoadPaymentHistoryToDataGridView(); };
            dateDenNgay.ValueChanged += (s, e) => { if (segmentedPill1.SelectedIndex == 2) LoadPaymentHistoryToDataGridView(); };
            guna2ComboBox1.SelectedIndexChanged += (s, e) => { if (segmentedPill1.SelectedIndex == 2) LoadPaymentHistoryToDataGridView(); };
            guna2ComboBox2.SelectedIndexChanged += (s, e) => { if (segmentedPill1.SelectedIndex == 2) LoadPaymentHistoryToDataGridView(); };
            if (dgvLichSuHoaDon != null)
            {
                dgvLichSuHoaDon.CellClick += DgvHoaDon_CellClick;
                dgvLichSuHoaDon.CellPainting += DgvTrangThai_CellPainting;
                dgvLichSuHoaDon.CellFormatting += DgvLichSuHoaDon_CellFormatting;
            }
            KeyDown += FrmThanhToan_HoaDon_KeyDown;
            KeyPreview = true;
        }

        private void FrmThanhToan_HoaDon_Load(object sender, EventArgs e)
        {
            lbSoHD.Text = "0";
            lbSoGiaoDich.Text = "0";
            lbTongThuHomNay.Text = "0 đ";
            lbSoTienTrungBinh.Text = "0 đ";
            lbSoSanhPhanTram.Text = "0 % vs hôm qua";
            SetupHistoryGrid();
            SetupInvoiceListContainer();
            RefreshTopStats();
            SegmentedPill1_SelectedIndexChanged(segmentedPill1, EventArgs.Empty);
            dateTuNgay.Checked = false;
            dateDenNgay.Checked = false;

            SetupAutoRefresh();
        }

        private void SetupAutoRefresh()
        {
            _autoRefreshTimer = new System.Windows.Forms.Timer();
            _autoRefreshTimer.Interval = 3000; // 3s
            _autoRefreshTimer.Tick += (s, e) =>
            {
                // Kiểm tra form và controls có bị dispose không
                if (this.IsDisposed || this.Disposing || _isRefreshing) return;
                if (invoicesFlowPanel != null && invoicesFlowPanel.IsDisposed) return;
                
                try
                {
                    _isRefreshing = true;
                    if (segmentedPill1 != null && !segmentedPill1.IsDisposed)
                    {
                        if (segmentedPill1.SelectedIndex == 0 || segmentedPill1.SelectedIndex == 1)
                        {
                            if ((DateTime.UtcNow - _lastInvoicesInteractionUtc) < TimeSpan.FromSeconds(4)) return;
                            LoadInvoicesFromDb();
                            RefreshTopStats();
                        }
                        else if (segmentedPill1.SelectedIndex == 2)
                        {
                            LoadPaymentHistoryToDataGridView();
                        }
                    }
                }
                catch (ObjectDisposedException)
                {
                    // Form hoặc controls đã bị dispose, dừng timer
                    if (_autoRefreshTimer != null)
                    {
                        _autoRefreshTimer.Stop();
                        _autoRefreshTimer.Dispose();
                        _autoRefreshTimer = null;
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không crash
                    System.Diagnostics.Debug.WriteLine($"Lỗi trong timer callback: {ex.Message}");
                }
                finally
                {
                    _isRefreshing = false;
                }
            };
            _autoRefreshTimer.Start();
        }

        private void SetupHistoryGrid()
        {
            if (dgvLichSuHoaDon == null) return;

            // Bỏ đóng băng nếu có để dùng Fill an toàn
            foreach (DataGridViewColumn col in dgvLichSuHoaDon.Columns)
            {
                col.Frozen = false;
            }

            dgvLichSuHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            dgvLichSuHoaDon.AutoGenerateColumns = false;
            dgvLichSuHoaDon.AllowUserToAddRows = false;
            dgvLichSuHoaDon.ReadOnly = true;
            dgvLichSuHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichSuHoaDon.MultiSelect = false;
            dgvLichSuHoaDon.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvLichSuHoaDon.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvLichSuHoaDon.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dgvLichSuHoaDon.EnableHeadersVisualStyles = false;
            dgvLichSuHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 160, 255);
            dgvLichSuHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLichSuHoaDon.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvLichSuHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLichSuHoaDon.GridColor = Color.FromArgb(80, 160, 255);
            EnsureHistoryColumns();
            SetHistoryGridHeaders();
            if (dgvLichSuHoaDon.Columns.Contains("Ngay") && dgvLichSuHoaDon.Columns.Contains("ThoiGian"))
            {
                int dateIdx = dgvLichSuHoaDon.Columns["Ngay"].DisplayIndex;
                dgvLichSuHoaDon.Columns["ThoiGian"].DisplayIndex = dateIdx;
                dgvLichSuHoaDon.Columns["Ngay"].DisplayIndex = dateIdx + 1;
            }
        }

        // Đặt tiêu đề cột
        private void SetHistoryGridHeaders()
        {
            if (dgvLichSuHoaDon == null) return;
            var headers = new Dictionary<string, string>
            {
                ["MaHD"] = "Mã HĐ",
                ["BanSanh"] = "Bàn/Sảnh",
                ["SoTien"] = "Số tiền",
                ["KhuyenMai"] = "Khuyến mãi",
                ["PhuongThuc"] = "Phương thức",
                ["Ngay"] = "Ngày",
                ["ThoiGian"] = "Thời gian",
                ["ThuNgan"] = "Thu ngân",
                ["TrangThai"] = "Trạng thái",
                ["ColPrint"] = "In hóa đơn",
                ["ColRefund"] = "Hoàn tiền"
            };

            foreach (var kv in headers)
            {
                if (dgvLichSuHoaDon.Columns.Contains(kv.Key))
                {
                    dgvLichSuHoaDon.Columns[kv.Key].HeaderText = kv.Value;
                }
            }
        }
        private void EnsureHistoryColumns()
        {
            if (dgvLichSuHoaDon == null) return;
            string[] names = new[] { "MaHD","BanSanh","SoTien","KhuyenMai","PhuongThuc","Ngay","ThoiGian","ThuNgan","TrangThai" };
            foreach (var n in names)
            {
                if (!dgvLichSuHoaDon.Columns.Contains(n))
                {
                    var col = new DataGridViewTextBoxColumn
                    {
                        Name = n,
                        ReadOnly = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    };
                    dgvLichSuHoaDon.Columns.Add(col);
                }
            }

            // Ẩn cột thao tác cũ nếu có
            if (dgvLichSuHoaDon.Columns.Contains("Column10"))
            {
                dgvLichSuHoaDon.Columns["Column10"].Visible = false;
            }

            // Thêm 2 cột nút hành động nếu chưa có
            if (!dgvLichSuHoaDon.Columns.Contains("ColPrint"))
            {
                var btnPrint = new DataGridViewButtonColumn
                {
                    Name = "ColPrint",
                    HeaderText = "In hóa đơn",
                    Text = "In hóa đơn",
                    UseColumnTextForButtonValue = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    ReadOnly = true
                };
                dgvLichSuHoaDon.Columns.Add(btnPrint);
            }

            if (!dgvLichSuHoaDon.Columns.Contains("ColRefund"))
            {
                var btnRefund = new DataGridViewButtonColumn
                {
                    Name = "ColRefund",
                    HeaderText = "Hoàn tiền",
                    Text = "Hoàn tiền",
                    UseColumnTextForButtonValue = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    ReadOnly = true
                };
                dgvLichSuHoaDon.Columns.Add(btnRefund);
            }
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
            EnableDoubleBuffer(invoicesFlowPanel);

            invoicesFlowPanel.Scroll += (s, e) => { _lastInvoicesInteractionUtc = DateTime.UtcNow; };
            invoicesFlowPanel.MouseWheel += (s, e) => { _lastInvoicesInteractionUtc = DateTime.UtcNow; };
            invoicesFlowPanel.MouseMove += (s, e) =>
            {
                if (System.Windows.Forms.Control.MouseButtons != MouseButtons.None)
                {
                    _lastInvoicesInteractionUtc = DateTime.UtcNow;
                }
            };
        }

        private static void EnableDoubleBuffer(Control c)
        {
            try
            {
                var prop = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                prop?.SetValue(c, true, null);
            }
            catch
            {
            }
        }

        private static string ComputeInvoicesHash(DataTable? dt)
        {
            if (dt == null || dt.Rows.Count == 0) return "empty";
            var sb = new StringBuilder();
            sb.Append("c=").Append(dt.Rows.Count).Append("|");
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    int id = Convert.ToInt32(row["hoa_don_id"]);
                    decimal total = Convert.ToDecimal(row["tong_sau_thue"]);
                    string stt = row.Table.Columns.Contains("trang_thai") && row["trang_thai"] != DBNull.Value
                        ? row["trang_thai"]?.ToString() ?? string.Empty
                        : string.Empty;
                    sb.Append(id).Append(':').Append(total).Append(':').Append(stt).Append('|');
                }
                catch
                {
                }
            }
            return sb.ToString();
        }

        // Load hóa đơn từ database theo loại
        private void LoadInvoicesFromDb(string? loai = null)
        {
            // Kiểm tra form có bị dispose không
            if (this.IsDisposed || this.Disposing) return;

            if (invoicesFlowPanel == null)
            {
                SetupInvoiceListContainer();
            }

            // Kiểm tra panel có bị dispose không
            if (invoicesFlowPanel == null || invoicesFlowPanel.IsDisposed)
            {
                return;
            }

            loai ??= _currentInvoiceLoai;
            if (loai == null)
            {
                _lastInvoicesHash = null;
                try
                {
                    if (!invoicesFlowPanel.IsDisposed)
                    {
                        invoicesFlowPanel.Controls.Clear();
                        invoicesFlowPanel.PerformLayout();
                    }
                }
                catch (ObjectDisposedException)
                {
                    // Panel đã bị dispose, bỏ qua
                }
                return;
            }

            try
            {
                int prevScroll = invoicesFlowPanel.VerticalScroll.Value;
                invoicesFlowPanel.SuspendLayout();

                var dt = _hoaDonBLL.GetHoaDonList(Session.ChiNhanhId, "CHỜ TT", 100, loai);
                if (dt == null)
                {
                    _lastInvoicesHash = null;
                    if (!invoicesFlowPanel.IsDisposed)
                    {
                        invoicesFlowPanel.Controls.Clear();
                        invoicesFlowPanel.ResumeLayout(false);
                        invoicesFlowPanel.PerformLayout();
                    }
                    return;
                }
                string currHash = $"{loai}|{ComputeInvoicesHash(dt)}";
                if (string.Equals(currHash, _lastInvoicesHash, StringComparison.Ordinal))
                {
                    if (!invoicesFlowPanel.IsDisposed)
                    {
                        invoicesFlowPanel.ResumeLayout(false);
                        invoicesFlowPanel.PerformLayout();
                    }
                    return;
                }
                _lastInvoicesHash = currHash;

                var existing = new Dictionary<int, Controls.HoaDonPanel>();
                if (!invoicesFlowPanel.IsDisposed)
                {
                    foreach (Control ctl in invoicesFlowPanel.Controls)
                    {
                        if (ctl is Controls.HoaDonPanel hp && hp.Name != null && hp.Name.StartsWith("INV_", StringComparison.Ordinal))
                        {
                            if (int.TryParse(hp.Name.Substring(4), out int exId))
                            {
                                existing[exId] = hp;
                            }
                        }
                    }

                    var newOrderIds = new List<int>();
                    var newIdSet = new HashSet<int>();

                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["hoa_don_id"]);
                        decimal sub = Convert.ToDecimal(row["tong_truoc_thue"]);
                        decimal vatPercent = Convert.ToDecimal(row["vat"]);
                        decimal vatValue = Math.Round(sub * vatPercent / 100m, 0);
                        decimal total = Convert.ToDecimal(row["tong_sau_thue"]);
                        DateTime ngayLap = Convert.ToDateTime(row["ngay_lap"]);

                        newOrderIds.Add(id);
                        newIdSet.Add(id);

                        Controls.HoaDonPanel invoiceItem;
                        if (!existing.TryGetValue(id, out invoiceItem!))
                        {
                            invoiceItem = new Controls.HoaDonPanel
                            {
                                TopLevel = false,
                                FormBorderStyle = FormBorderStyle.None,
                                Dock = DockStyle.None,
                                Name = $"INV_{id}"
                            };
                            invoiceItem.Width = invoicesFlowPanel.ClientSize.Width - 40;
                            invoiceItem.Height = 189;
                            invoiceItem.Margin = new Padding(0, 0, 5, 8);
                            invoiceItem.Selected += (_, __) => OnInvoiceSelected(invoiceItem);
                            invoiceItem.Show();
                            invoicesFlowPanel.Controls.Add(invoiceItem);
                        }

                        string invoiceLoai = row["loai"]?.ToString() ?? "";
                        invoiceItem.TableName = invoiceLoai == "NHAHANG" ? "Nhà hàng" : "Tiệc cưới";
                        invoiceItem.GuestsAndDishes = $"HD#{id}";
                        invoiceItem.InvoiceCode = $"HD{id}";
                        invoiceItem.Subtotal = FormatCurrency(sub);
                        invoiceItem.Vat = FormatCurrency(vatValue);
                        invoiceItem.Total = FormatCurrency(total);
                        invoiceItem.Tag = new { HoaDonId = id, VatPercent = vatPercent };

                        decimal displayVatPercent = invoiceLoai == "NHAHANG" ? 8m : 10m;
                        invoiceItem.SetVatPercent(displayVatPercent);

                        if (invoiceLoai == "TIECCUOI")
                        {
                            // Lấy thông tin ngày tổ chức cho tiệc cưới
                            try
                            {
                                if (row["tham_chieu_id"] != DBNull.Value)
                                {
                                    int hopDongId = Convert.ToInt32(row["tham_chieu_id"]);
                                    var datSanhBLL = new QLNhaHangTiecCuoi.BLL.DatSanhBLL();
                                    int? datSanhId = datSanhBLL.LayDatSanhIdByHopDongId(hopDongId);
                                    if (datSanhId.HasValue)
                                    {
                                        DataRow datSanhInfo = datSanhBLL.LayThongTinDatSanh(datSanhId.Value);
                                        if (datSanhInfo != null)
                                        {
                                            DateTime ngayToChuc = Convert.ToDateTime(datSanhInfo["ngay_to_chuc"]);
                                            TimeSpan gioToChuc = datSanhInfo["gio_to_chuc"] != DBNull.Value
                                                ? (TimeSpan)datSanhInfo["gio_to_chuc"]
                                                : new TimeSpan(10, 30, 0);
                                            invoiceItem.SetNgayToChuc(gioToChuc, ngayToChuc);
                                        }
                                    }
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            try { invoiceItem.SetStartTime(ngayLap.ToLocalTime()); } catch { }
                        }
                    }

                    var toRemove = new List<Control>();
                    if (!invoicesFlowPanel.IsDisposed)
                    {
                        foreach (Control ctl in invoicesFlowPanel.Controls)
                        {
                            if (ctl is Controls.HoaDonPanel hp && hp.Name != null && hp.Name.StartsWith("INV_", StringComparison.Ordinal))
                            {
                                if (int.TryParse(hp.Name.Substring(4), out int exId) && !newIdSet.Contains(exId))
                                {
                                    toRemove.Add(ctl);
                                }
                            }
                        }
                        foreach (var ctl in toRemove)
                        {
                            try
                            {
                                if (!invoicesFlowPanel.IsDisposed)
                                {
                                    invoicesFlowPanel.Controls.Remove(ctl);
                                }
                                try { ctl.Dispose(); } catch { }
                            }
                            catch (ObjectDisposedException) { }
                        }

                        for (int i = newOrderIds.Count - 1; i >= 0; i--)
                        {
                            string name = $"INV_{newOrderIds[i]}";
                            var ctl = invoicesFlowPanel.Controls.Cast<Control>().FirstOrDefault(c => c.Name == name);
                            if (ctl != null)
                            {
                                try
                                {
                                    if (!invoicesFlowPanel.IsDisposed)
                                    {
                                        invoicesFlowPanel.Controls.SetChildIndex(ctl, 0);
                                    }
                                }
                                catch (ObjectDisposedException) { }
                            }
                        }

                        if (!invoicesFlowPanel.IsDisposed)
                        {
                            invoicesFlowPanel.ResumeLayout(false);
                            invoicesFlowPanel.PerformLayout();
                        }

                        try
                        {
                            if (!invoicesFlowPanel.IsDisposed)
                            {
                                prevScroll = Math.Max(0, Math.Min(prevScroll, invoicesFlowPanel.VerticalScroll.Maximum));
                                invoicesFlowPanel.VerticalScroll.Value = prevScroll;
                                invoicesFlowPanel.PerformLayout();
                            }
                        }
                        catch (ObjectDisposedException) { }
                        catch { }
                    }
                }
            }
            
            catch (ObjectDisposedException)
            {
                // Panel hoặc form đã bị dispose, dừng timer
                if (_autoRefreshTimer != null)
                {
                    _autoRefreshTimer.Stop();
                    _autoRefreshTimer.Dispose();
                    _autoRefreshTimer = null;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không crash
                System.Diagnostics.Debug.WriteLine($"Lỗi trong LoadInvoicesFromDb: {ex.Message}");
            }
        }

        // Khi quay lại form (từ màn hình bán hàng), tự reload danh sách hóa đơn
        private void FrmThanhToan_HoaDon_Activated(object? sender, EventArgs e)
        {
            RefreshData();
        }

        // Xử lý phím tắt F5 để refresh
        private void FrmThanhToan_HoaDon_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                RefreshData();
            }
        }

        // Method public để refresh/reload dữ liệu
        public void RefreshData()
        {
            try
            {
                if (segmentedPill1.SelectedIndex == 0 || segmentedPill1.SelectedIndex == 1)
                {
                    _lastInvoicesHash = null;
                    LoadInvoicesFromDb();
                    RefreshTopStats();
                }
                else if (segmentedPill1.SelectedIndex == 2)
                {
                    LoadPaymentHistoryToDataGridView();
                    RefreshTopStats();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi làm mới dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                if (_autoRefreshTimer != null)
                {
                    _autoRefreshTimer.Stop();
                    _autoRefreshTimer.Dispose();
                    _autoRefreshTimer = null;
                }
            }
            finally
            {
                base.OnFormClosed(e);
            }
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
                if (segmentedPill1.SelectedIndex == 0 || segmentedPill1.SelectedIndex == 1)
                {
                    LoadInvoicesFromDb();
                    RefreshTopStats();
                }
                panelHoaDonThanhToan.Controls.Clear(); // Xóa panel thanh toán sau khi thanh toán thành công

                // Hỏi in hóa đơn
                var result = MessageBox.Show("Thanh toán thành công! Bạn có muốn in hóa đơn không?", 
                    "In hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    PrintInvoice(hoaDonId);
                }
            };

            decimal vatPercent = 8; // Mặc định 8%
            if (selected.Tag != null)
            {
                var tagData = selected.Tag as dynamic;
                if (tagData != null && tagData.VatPercent != null)
                {
                    vatPercent = Convert.ToDecimal(tagData.VatPercent);
                }
            }
            
            if (hoaDonId > 0)
            {
                var hoaDon = _hoaDonBLL.GetHoaDonById(hoaDonId);
                if (hoaDon != null && hoaDon["vat"] != DBNull.Value)
                {
                    vatPercent = Convert.ToDecimal(hoaDon["vat"]);
                }
            }

            decimal subtotal = ParseCurrency(selected.Total) - ParseCurrency(selected.Vat);
            decimal vatValue = ParseCurrency(selected.Vat);
            decimal total = ParseCurrency(selected.Total);

            // tính số tiền còn lại (trừ số tiền đã cọc và đã thanh toán) cho hóa đơn tiệc cưới
            decimal soTienConLai = total;
            if (hoaDonId > 0)
            {
                soTienConLai = _hoaDonBLL.LaySoTienConLai(hoaDonId, out string error);
                if (!string.IsNullOrEmpty(error))
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi tính số tiền còn lại: {error}");
                }
            }

            payPanel.SetTitle($"Thanh toán - {selected.InvoiceCode}");
            payPanel.BindAmounts(subtotal, vatPercent, soTienConLai);
            
            if (hoaDonId > 0)
            {
                var hoaDon = _hoaDonBLL.GetHoaDonById(hoaDonId);
                if (hoaDon != null)
                {
                    string loaiHd = hoaDon["loai"]?.ToString() ?? "";
                    if (loaiHd == "TIECCUOI")
                    {
                        try
                        {
                            decimal tongHoaDon = hoaDon["tong_sau_thue"] != DBNull.Value ? Convert.ToDecimal(hoaDon["tong_sau_thue"]) : 0m;
                            if (hoaDon["tham_chieu_id"] != DBNull.Value)
                            {
                                int hopDongId = Convert.ToInt32(hoaDon["tham_chieu_id"]);
                                var datSanhBLL = new QLNhaHangTiecCuoi.BLL.DatSanhBLL();
                                var dtCoc = datSanhBLL.LayDanhSachCoc(hopDongId);
                                decimal tongCoc = 0;
                                if (dtCoc != null && dtCoc.Rows.Count > 0)
                                {
                                    foreach (DataRow r in dtCoc.Rows)
                                    {
                                        if (r["so_tien"] != DBNull.Value)
                                            tongCoc += Convert.ToDecimal(r["so_tien"]);
                                    }
                                }
                                // Hiển thị cọc sau khi BindAmounts để đảm bảo hiển thị đúng
                                if (tongCoc > 0)
                                {
                                    payPanel.SetDeposit(tongCoc);
                                }
                                payPanel.SetBaseDiscountTotal(tongHoaDon);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy tiền cọc: {ex.Message}");
                        }
                    }
                }
            }
            
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

                int soGiaoDich = soHdToday;
                decimal giaTriTb = soHdToday > 0 ? Math.Round(tongToday / soHdToday, 0) : 0m;
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
            if (segmentedPill1.SelectedIndex == 0 || segmentedPill1.SelectedIndex == 1)
            {
                lbDanhSachHoaDon.Visible = true;
                panelDanhSachHoaDon.Visible = true;
                panelHoaDonThanhToan.Visible = true;
                panelLichSuGiaoDich.Visible = false;

                string? newLoai = segmentedPill1.SelectedIndex == 0 ? "NHAHANG" : "TIECCUOI";
                if (_currentInvoiceLoai != newLoai)
                {
                    _currentInvoiceLoai = newLoai;
                    _lastInvoicesHash = null;
                }

                if (lbDanhSachHoaDon != null)
                {
                    lbDanhSachHoaDon.Text = segmentedPill1.SelectedIndex == 0
                        ? "Danh sách hóa đơn nhà hàng"
                        : "Danh sách hóa đơn tiệc cưới";
                }

                panelHoaDonThanhToan.Controls.Clear();
                LoadInvoicesFromDb();
            }
            else if (segmentedPill1.SelectedIndex == 2)
            {
                lbDanhSachHoaDon.Visible = false;
                panelDanhSachHoaDon.Visible = false;
                panelHoaDonThanhToan.Visible = false;
                panelLichSuGiaoDich.Visible = true;
                panelHoaDonThanhToan.Controls.Clear();

                _currentInvoiceLoai = null;
                _lastInvoicesHash = null;
                if (lbDanhSachHoaDon != null)
                {
                    lbDanhSachHoaDon.Text = "Danh sách hóa đơn";
                }

                LoadPaymentHistoryToDataGridView();
            }
        }

        // Load lịch sử thanh toán vào dgvHoaDon
        private void LoadPaymentHistoryToDataGridView()
        {
            try
            {
                if (dgvLichSuHoaDon == null)
                {
                    MessageBox.Show("dgvHoaDon is null!");
                    return;
                }

                dgvLichSuHoaDon.Rows.Clear();
                dgvLichSuHoaDon.RowTemplate.Height = 70;
                // Cập nhật lại tiêu đề cột nếu dùng grid lịch sử
                SetHistoryGridHeaders();

                DateTime? fromDate = dateTuNgay.Checked ? dateTuNgay.Value.Date : null;
                DateTime? toDate = dateDenNgay.Checked ? dateDenNgay.Value.Date : null;
                string? phuongThuc = guna2ComboBox1.SelectedItem?.ToString();
                if (phuongThuc == "Tất cả phương thức") phuongThuc = null;
                // lấy dữ liệu từ database
                var dt = _hoaDonBLL.GetPaidInvoicesHistory(
                    Session.ChiNhanhId,
                    fromDate,
                    toDate,
                    phuongThuc,
                    100
                );
                // thêm dữ liệu vào DataGridView
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
                        else if (trangThai == "HOÀN TIỀN")
                        {
                            trangThaiText = "Hoàn tiền";
                        }
                        else
                        {
                            trangThaiText = trangThai ?? "";
                        }

                        // Thêm row vào DataGridView - map vào đúng các cột theo tên
                        int rowIndex = dgvLichSuHoaDon.Rows.Add();
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["MaHD"].Value = id.ToString("D2"); // Mã HĐ
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["BanSanh"].Value = banSanh ?? "-"; // Bàn/Sảnh
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["SoTien"].Value = FormatCurrency(total); // Số tiền
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["KhuyenMai"].Value = kmText; // Khuyến mãi
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["PhuongThuc"].Value = phuongThucText; // Phương thức
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["Ngay"].Value = ngayLap.ToLocalTime().ToString("dd/MM/yyyy"); // Ngày
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["ThoiGian"].Value = ngayLap.ToLocalTime().ToString("HH:mm"); // Thời gian
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["ThuNgan"].Value = thuNgan ?? "-"; // Thu ngân
                        dgvLichSuHoaDon.Rows[rowIndex].Cells["TrangThai"].Value = trangThaiText; // Trạng thái
                        string? loai = row["loai"] != DBNull.Value ? row["loai"].ToString() : null;
                        // Lưu HoaDonId và loại hóa đơn vào Tag
                        dgvLichSuHoaDon.Rows[rowIndex].Tag = new { HoaDonId = id, Loai = loai ?? "" };
                    }
                    catch (Exception rowEx)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi tải lịch sử thanh toán: {ex.Message}", UI.Controls.ToastType.Error);
            }
        }

        // Xử lý click vào cột Thao tác
        private void DgvHoaDon_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvLichSuHoaDon == null) return;

            var colName = dgvLichSuHoaDon.Columns[e.ColumnIndex].Name;
            if (colName == "ColPrint" || colName == "ColRefund")
            {
                int hoaDonId = 0;
                if (dgvLichSuHoaDon.Rows[e.RowIndex].Tag != null)
                {
                    var tagData = dgvLichSuHoaDon.Rows[e.RowIndex].Tag as dynamic;
                    if (tagData != null && tagData.HoaDonId != null)
                    {
                        hoaDonId = Convert.ToInt32(tagData.HoaDonId);
                    }
                }
                
                string trangThai = dgvLichSuHoaDon.Rows[e.RowIndex].Cells["TrangThai"].Value?.ToString() ?? "";
                if (colName == "ColPrint")
                {
                    PrintInvoice(hoaDonId);
                }
                else if (colName == "ColRefund")
                {
                    // Chỉ cho phép hoàn tiền hóa đơn nhà hàng đã thanh toán
                    if (trangThai == "Hoàn thành")
                    {
                        var hoaDon = _hoaDonBLL.GetHoaDonById(hoaDonId);
                        if (hoaDon != null)
                        {
                            string loai = hoaDon["loai"]?.ToString() ?? "";
                            if (loai == "NHAHANG")
                            {
                                RefundPayment(hoaDonId);
                            }
                            else
                            {
                                GunaToast.Show(this, "Chức năng hoàn tiền chỉ áp dụng cho hóa đơn nhà hàng!", UI.Controls.ToastType.Info);
                            }
                        }
                    }
                }
            }
        }

        // Ẩn/hiện nút hoàn tiền dựa trên loại hóa đơn và trạng thái
        private void DgvLichSuHoaDon_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvLichSuHoaDon == null) return;

            var colName = dgvLichSuHoaDon.Columns[e.ColumnIndex].Name;
            if (colName == "ColRefund")
            {
                string trangThai = dgvLichSuHoaDon.Rows[e.RowIndex].Cells["TrangThai"].Value?.ToString() ?? "";
                string loai = "";
                
                if (dgvLichSuHoaDon.Rows[e.RowIndex].Tag != null)
                {
                    var tagData = dgvLichSuHoaDon.Rows[e.RowIndex].Tag as dynamic;
                    if (tagData != null && tagData.Loai != null)
                    {
                        loai = tagData.Loai.ToString() ?? "";
                    }
                }

                // Chỉ hiển thị nút hoàn tiền cho hóa đơn nhà hàng đã thanh toán
                if (trangThai == "Hoàn thành" && loai == "NHAHANG")
                {
                    dgvLichSuHoaDon.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = dgvLichSuHoaDon.DefaultCellStyle.ForeColor;
                    dgvLichSuHoaDon.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                }
                else
                {
                    // Ẩn nút bằng cách đặt màu chữ giống màu nền
                    dgvLichSuHoaDon.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = dgvLichSuHoaDon.DefaultCellStyle.BackColor;
                    dgvLichSuHoaDon.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                }
            }
        }

        // Vẽ trạng thái bằng kiểu badge bo tròn (mô phỏng UIPanel SunnyUI)
        private void DgvTrangThai_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvLichSuHoaDon == null) return;

            if (dgvLichSuHoaDon.Columns[e.ColumnIndex].Name != "TrangThai") return;

            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
            e.Handled = true;

            string text = dgvLichSuHoaDon.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;

            // Chọn màu theo trạng thái
            Color backColor;      // màu nền nhạt
            Color borderColor;    // viền & text đậm
            Color foreColor;      // màu chữ
            switch (text)
            {
                case "Hoàn thành":
                    backColor = Color.FromArgb(214, 245, 227);    // xanh lá nhạt
                    borderColor = Color.FromArgb(46, 204, 113);   // xanh lá đậm
                    foreColor = borderColor;
                    break;
                case "Chờ thanh toán":
                    backColor = Color.FromArgb(214, 234, 248);  // xanh dương nhạt
                    borderColor = Color.FromArgb(52, 152, 219); // xanh dương đậm
                    foreColor = borderColor;
                    break;
                case "Hoàn tiền":
                    backColor = Color.FromArgb(252, 233, 231);  // đỏ nhạt
                    borderColor = Color.FromArgb(231, 76, 60);  // đỏ đậm
                    foreColor = borderColor;
                    break;
                case "Nháp":
                    backColor = Color.FromArgb(236, 240, 241);  // xám nhạt
                    borderColor = Color.FromArgb(127, 140, 141); // xám đậm
                    foreColor = Color.FromArgb(96, 106, 108);
                    break;
                default:
                    backColor = Color.FromArgb(236, 240, 241);
                    borderColor = Color.FromArgb(127, 140, 141);
                    foreColor = Color.FromArgb(96, 106, 108);
                    break;
            }

            // Tính khung vẽ badge
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            var paddingH = 8;
            var paddingV = 6;
            var font = new Font("Segoe UI", 9F, FontStyle.Bold);
            var size = g.MeasureString(text, font);
            int w = (int)Math.Min(size.Width + paddingH * 2, Math.Max(28, e.CellBounds.Width - 8));
            int h = (int)Math.Min(size.Height + paddingV, e.CellBounds.Height - 8);
            int x = e.CellBounds.X + (e.CellBounds.Width - w) / 2;
            int y = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;

            DrawRoundedBadge(g, new Rectangle(x, y, w, h), backColor, borderColor);
            using (var tb = new SolidBrush(foreColor))
            {
                g.DrawString(text, font, tb, x + (w - size.Width) / 2, y + (h - size.Height) / 2);
            }
            font.Dispose();
        }

        private static void DrawRoundedBadge(Graphics g, Rectangle rect, Color fill, Color border)
        {
            int radius = 10;
            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                path.CloseFigure();
                using (var brush = new SolidBrush(fill)) g.FillPath(brush, path);
                using (var pen = new Pen(border, 1F)) g.DrawPath(pen, path);
            }
        }

        // In hóa đơn
        private void PrintInvoice(int hoaDonId)
        {
            try
            {
                var report = new rptHoaDon();
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["QLNhaHangOnline"]?.ConnectionString;
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();

                    var sqlDs = report.DataSource as SqlDataSource;
                    if (sqlDs != null)
                    {
                        var connParams = sqlDs.ConnectionParameters as DevExpress.DataAccess.ConnectionParameters.MsSqlConnectionParameters;
                        if (connParams != null)
                        {
                            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

                            sqlDs.ConnectionParameters = new DevExpress.DataAccess.ConnectionParameters.MsSqlConnectionParameters(
                                builder.DataSource,
                                builder.InitialCatalog,
                                builder.UserID,
                                builder.Password,
                                DevExpress.DataAccess.ConnectionParameters.MsSqlAuthorizationType.SqlServer
                            );
                        }

                        var query = sqlDs.Queries[0];
                        query.Parameters[0].Value = hoaDonId;
                        query.Parameters[1].Value = Session.ChiNhanhId;

                        sqlDs.RebuildResultSchema();
                        sqlDs.Fill();
                    }
                }

                ReportPrintTool printTool = new ReportPrintTool(report);
                printTool.ShowPreviewDialog();
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
                // Kiểm tra hóa đơn có tồn tại và là loại nhà hàng
                var hoaDon = _hoaDonBLL.GetHoaDonById(hoaDonId);
                if (hoaDon == null)
                {
                    GunaToast.Show(this, "Không tìm thấy hóa đơn!", UI.Controls.ToastType.Error);
                    return;
                }

                string loai = hoaDon["loai"]?.ToString() ?? "";
                if (loai != "NHAHANG")
                {
                    GunaToast.Show(this, "Chức năng hoàn tiền chỉ áp dụng cho hóa đơn nhà hàng!", UI.Controls.ToastType.Info);
                    return;
                }

                string trangThai = hoaDon["trang_thai"]?.ToString() ?? "";
                if (trangThai != "ĐÃ THANH TOÁN")
                {
                    GunaToast.Show(this, "Chỉ có thể hoàn tiền cho hóa đơn đã thanh toán!", UI.Controls.ToastType.Info);
                    return;
                }

                decimal tongTien = hoaDon["tong_sau_thue"] != DBNull.Value 
                    ? Convert.ToDecimal(hoaDon["tong_sau_thue"]) 
                    : 0m;

                var result = MessageBox.Show(
                    $"Xác nhận hoàn tiền cho hóa đơn HD{hoaDonId:D2}?\n\n" +
                    $"Tổng tiền: {FormatCurrency(tongTien)}\n\n" +
                    $"Thao tác này không thể hoàn tác!",
                    "Xác nhận hoàn tiền",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string errorMessage;
                    bool success = _hoaDonBLL.ProcessRefund(hoaDonId, out errorMessage);
                    
                    if (success)
                    {
                        GunaToast.Show(this, $"Đã hoàn tiền cho hóa đơn HD{hoaDonId:D2}", UI.Controls.ToastType.Success);
                        LoadPaymentHistoryToDataGridView();
                        RefreshTopStats();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi hoàn tiền: {errorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hoàn tiền: {ex.Message}");
            }
        }

    }
}
