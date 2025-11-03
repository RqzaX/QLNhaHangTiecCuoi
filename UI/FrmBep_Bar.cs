using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;
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
using System.Drawing.Printing;
using UI.Common;
using UI.Controls;
using Sunny.UI;
using Timer = System.Windows.Forms.Timer;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmBep_Bar : Form, IFormRefreshable
    {
        private DatabaseHelper _dbHelper;
        private KOTBLL _kotBLL;
        private List<KOTTicket> _kotTickets = new List<KOTTicket>();
        private KOTStatus _currentStatus = KOTStatus.Pending;
        private Timer _autoRefreshTimer;

        public enum KOTStatus
        {
            Pending,    // Chờ làm
            InProgress, // Đang làm
            Ready       // Sẵn sàng
        }

        public class KOTTicket
        {
            public int KOTId { get; set; }
            public string TicketCode { get; set; }
            public string TableName { get; set; }
            public DateTime OrderTime { get; set; }
            public KOTStatus Status { get; set; }
            public bool IsPriority { get; set; }
            public List<KOTItem> Items { get; set; } = new List<KOTItem>();
            public string Notes { get; set; }
        }

        public class KOTItem
        {
            public int ItemId { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public string Notes { get; set; }
        }

        public FrmBep_Bar()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _kotBLL = new KOTBLL(_dbHelper);
            this.Load += FrmBep_Bar_Load;
            this.FormClosing += (s, e) =>
            {
                _autoRefreshTimer?.Stop();
                _autoRefreshTimer?.Dispose();
            };
        }

        private void FrmBep_Bar_Load(object sender, EventArgs e)
        {
            SetupUI();
            SetupEventHandlers();
            LoadKOTTickets(); // LoadKOTTickets() sẽ gọi DisplayKOTTickets() và UpdateStatistics()
        }

        private void SetupUI()
        {
            lbChoLam.Text = "0";
            lbDangLam.Text = "0";
            lbSanSang.Text = "0";
            lbThoiGianTB.Text = "0 phút";

            segmentedPill1.Items.Clear();
            segmentedPill1.Items.Add(new VanThuan.UI.PillItem { Text = "Chờ làm (0)" });
            segmentedPill1.Items.Add(new VanThuan.UI.PillItem { Text = "Đang làm (0)" });
            segmentedPill1.Items.Add(new VanThuan.UI.PillItem { Text = "Sẵn sàng (0)" });
            segmentedPill1.SelectedIndex = 0;
        }

        private void SetupEventHandlers()
        {
            segmentedPill1.SelectedIndexChanged += SegmentedPill1_SelectedIndexChanged;
            
            // Auto-refresh timer: refresh mỗi 5 giây khi form đang visible
            _autoRefreshTimer = new Timer { Interval = 5000 }; // 5 giây
            _autoRefreshTimer.Tick += (s, e) =>
            {
                if (this.Visible && this.Parent != null && this.Parent.Visible)
                {
                    RefreshData();
                }
            };
            _autoRefreshTimer.Start(); // Bắt đầu timer ngay sau khi load
        }
        
        /// <summary>
        /// Implement IFormRefreshable - Refresh dữ liệu khi form được chọn lại
        /// </summary>
        public void RefreshData()
        {
            try
            {
                // Chỉ refresh nếu form đang visible để tránh refresh không cần thiết
                if (this.Visible && this.Parent != null)
                {
                    LoadKOTTickets();
                }
            }
            catch (Exception ex)
            {
                // Silent fail để tránh spam error
                System.Diagnostics.Debug.WriteLine($"FrmBep_Bar RefreshData error: {ex.Message}");
            }
        }

        private void LoadKOTTickets()
        {
            try
            {
                if (!TestDatabaseConnection())
                {
                    DisplayKOTTickets();
                    UpdateStatistics();
                    return;
                }
                
                if (Session.ChiNhanhId <= 0)
                {
                    DisplayKOTTickets();
                    UpdateStatistics();
                    return;
                }
                
                var dt = _kotBLL.LayDanhSachKOT(Session.ChiNhanhId, null, null);
                
                ProcessKOTData(dt);
                
                DisplayKOTTickets();
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                DisplayKOTTickets();
                UpdateStatistics();
            }
        }

        private void ProcessKOTData(DataTable dt)
        {
            _kotTickets.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var trangThai = row["trang_thai"]?.ToString() ?? "";
                    
                    var trangThaiUpper = trangThai.ToUpper().Trim();
                    if (trangThaiUpper == "ĐÃ ĐÓNG" || trangThaiUpper == "DA DONG" || trangThaiUpper == "ĐÃĐÓNG")
                    {
                        continue; // Không thêm vào danh sách
                    }
                    
                    var status = GetStatusFromString(trangThai);
                    var soBan = row["so_ban"].ToString();
                    var tableName = soBan == "TIỆC" ? "Tiệc cưới" : $"Bàn {soBan}";
                    
                    var kot = new KOTTicket
                    {
                        KOTId = Convert.ToInt32(row["kot_id"]),
                        TicketCode = row["ma_kot"].ToString(),
                        TableName = tableName,
                        OrderTime = Convert.ToDateTime(row["thoi_gian_dat"]),
                        Status = status,
                        IsPriority = Convert.ToBoolean(row["uu_tien"]),
                        Notes = row["ghi_chu"]?.ToString()
                    };
                    LoadKOTItems(kot);
                    _kotTickets.Add(kot);
                }
            }
        }

        private bool TestDatabaseConnection()
        {
            try
            {
                // Test connection bằng cách lấy danh sách chi nhánh
                string testQuery = "SELECT COUNT(*) FROM chi_nhanh WHERE trang_thai = 1";
                var result = _dbHelper.ExecuteScalar(testQuery);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestDatabaseConnection: Failed - {ex.Message}");
                return false;
            }
        }

        private KOTStatus GetStatusFromString(string status)
        {
            switch (status?.ToUpper())
            {
                case "ĐANG PHỤC VỤ": return KOTStatus.Pending;
                case "CHỜ THANH TOÁN": return KOTStatus.InProgress;
                case "ĐÃ ĐÓNG": return KOTStatus.Ready;
                default: 
                    Console.WriteLine($"Unknown status: '{status}' - defaulting to Pending");
                    return KOTStatus.Pending;
            }
        }


        private void LoadKOTItems(KOTTicket kot)
        {
            try
            {
                var dtItems = _kotBLL.LayChiTietKOT(kot.KOTId);
                if (dtItems != null && dtItems.Rows.Count > 0)
                {
                    foreach (DataRow row in dtItems.Rows)
                    {
                        kot.Items.Add(new KOTItem
                        {
                            ItemId = Convert.ToInt32(row["mon_id"]),
                            Name = row["ten_mon"].ToString(),
                            Quantity = Convert.ToInt32(row["so_luong"]),
                            Notes = row["ghi_chu_bep"]?.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Silent fail
            }
        }

        private void SegmentedPill1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentStatus = (KOTStatus)segmentedPill1.SelectedIndex;
            DisplayKOTTickets();
        }


        private void DisplayKOTTickets()
        {
            panelDanhSach.Controls.Clear();

            var filteredTickets = _kotTickets.Where(kot => kot.Status == _currentStatus).ToList();
            if (filteredTickets.Count == 0)
            {
                var lblNoData = new Label
                {
                    Text = "Không có đơn hàng nào",
                    Font = new Font("Segoe UI", 14F, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(50, 50)
                };
                panelDanhSach.Controls.Add(lblNoData);
                return;
            }

            int cardWidth = 450;
            int spacing = 20;
            int cardsPerRow = (panelDanhSach.Width - spacing) / (cardWidth + spacing);
            int currentY = spacing;
            int currentRowMaxHeight = 0;

            for (int i = 0; i < filteredTickets.Count; i++)
            {
                var kot = filteredTickets[i];
                int col = i % cardsPerRow;
                
                // Tính chiều cao dựa trên số lượng items
                int cardHeight = CalculateCardHeight(kot);
                
                if (col == 0 && i > 0)
                {
                    currentY += currentRowMaxHeight + spacing;
                    currentRowMaxHeight = 0;
                }
                
                // Cập nhật chiều cao tối đa của hàng hiện tại
                currentRowMaxHeight = Math.Max(currentRowMaxHeight, cardHeight);
                
                int x = spacing + col * (cardWidth + spacing);
                int y = currentY;
                
                var card = CreateKOTCard(kot);
                card.Location = new Point(x, y);
                card.Size = new Size(cardWidth, cardHeight);
                panelDanhSach.Controls.Add(card);
            }
        }

        private int CalculateCardHeight(KOTTicket kot)
        {
            // Dựa trên layout của KOTTicketCard
            const int paddingTop = 16;
            const int paddingBottom = 16;
            const int titleHeight = 30;
            const int chipRowHeight = 36;
            const int itemRowHeight = 24;
            const int buttonHeight = 40;
            const int shadowMargin = 8;
            const int minHeight = 200;

            int baseHeight = paddingTop + titleHeight + chipRowHeight + paddingBottom + buttonHeight + shadowMargin;
            int itemsHeight = kot.Items.Count * itemRowHeight;
            int notesHeight = !string.IsNullOrWhiteSpace(kot.Notes) ? 31 : 0;

            int totalHeight = baseHeight + itemsHeight + notesHeight;
            return Math.Max(minHeight, totalHeight);
        }

        private KOTTicketCard CreateKOTCard(KOTTicket kot)
        {
            var card = new KOTTicketCard
            {
                TicketCode = kot.TicketCode,
                TableName = kot.TableName,
                OrderTime = kot.OrderTime,
                Notes = kot.Notes,
                CardBackColor = Color.White,
                CornerRadius = 18,
                CardPadding = new Padding(18, 16, 18, 16),
                BorderColor = kot.IsPriority ? Color.Red : Color.FromArgb(225, 229, 234),
                ButtonColor = Color.FromArgb(12, 15, 28),
                ButtonHoverColor = Color.FromArgb(20, 24, 45),
                ButtonTextColor = Color.White,
                ButtonRadius = 12,
                ButtonHeight = 40
            };

            // Set items
            card.Items.Clear();
            foreach (var item in kot.Items)
            {
                var kotItem = new KOTTicketCard.KotItem
                {
                    Name = item.Name,
                    Qty = item.Quantity
                };
                card.Items.Add(kotItem);
            }

            switch (kot.Status)
            {
                case KOTStatus.Pending:
                    card.ActionText = "Bắt đầu làm";
                    card.StartClicked += (s, e) => StartCooking(kot.KOTId);
                    card.SecondaryText = "In món";
                    card.SecondaryVisible = true;
                    card.SecondaryClicked += (s, e) => PrintKOT(kot);
                    break;
                case KOTStatus.InProgress:
                    card.ActionText = "Đã xong";
                    card.StartClicked += (s, e) => MarkAsReady(kot.KOTId);
                    card.SecondaryText = "In món";
                    card.SecondaryVisible = true;
                    card.SecondaryClicked += (s, e) => PrintKOT(kot);
                    break;
                case KOTStatus.Ready:
                    card.ActionText = "Đã phục vụ";
                    card.StartClicked += (s, e) => MarkAsServed(kot.KOTId);
                    card.SecondaryText = "In món";
                    card.SecondaryVisible = true;
                    card.SecondaryClicked += (s, e) => PrintKOT(kot);
                    break;
            }

            return card;
        }

        private void PrintKOT(KOTTicket kot)
        {
            try
            {
                var lines = new List<string>();
                lines.Add($"KOT: {kot.TicketCode}");
                lines.Add($"Bàn: {kot.TableName}");
                lines.Add($"Thời gian: {kot.OrderTime:HH:mm dd/MM/yyyy}");
                if (!string.IsNullOrWhiteSpace(kot.Notes))
                {
                    lines.Add($"Ghi chú: {kot.Notes}");
                }
                lines.Add("------------------------------");
                foreach (var it in kot.Items)
                {
                    var note = string.IsNullOrWhiteSpace(it.Notes) ? "" : $" – {it.Notes}";
                    lines.Add($"{it.Quantity}x {it.Name}{note}");
                }

                string content = string.Join("\n", lines);

                using (var doc = new PrintDocument())
                {
                    doc.DocumentName = $"KOT_{kot.TicketCode}";
                    doc.PrintPage += (s, e) =>
                    {
                        var font = new Font("Segoe UI", 10f);
                        e.Graphics.DrawString(content, font, Brushes.Black, new RectangleF(40, 40, e.PageBounds.Width - 80, e.PageBounds.Height - 80));
                        e.HasMorePages = false;
                    };

                    using (var dlg = new PrintDialog())
                    {
                        dlg.Document = doc;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            doc.Print();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi in: {ex.Message}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
            }
        }

        private void StartCooking(int kotId)
        {
            try
            {
                var kot = _kotTickets.FirstOrDefault(k => k.KOTId == kotId);
                if (kot != null)
                {
                    // Cập nhật database
                    bool success = _kotBLL.CapNhatTrangThaiKOT(kotId, "CHỜ THANH TOÁN");
                    
                    if (success)
                    {
                        kot.Status = KOTStatus.InProgress;
                        UpdateStatistics();
                        DisplayKOTTickets();
                        GunaToast.Show(this, $"Đã bắt đầu làm {kot.TicketCode}", UI.Controls.ToastType.Success, 2000, UI.Controls.ToastPos.TopRight);
                    }
                    else
                    {
                        GunaToast.Show(this, $"Không thể cập nhật trạng thái {kot.TicketCode}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
                    }
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi: {ex.Message}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
            }
        }

        private void MarkAsReady(int kotId)
        {
            try
            {
                var kot = _kotTickets.FirstOrDefault(k => k.KOTId == kotId);
                if (kot != null)
                {
                    bool success = _kotBLL.CapNhatTrangThaiKOT(kotId, "ĐÃ ĐÓNG");
                    
                    if (success)
                    {
                        kot.Status = KOTStatus.Ready;
                        UpdateStatistics();
                        DisplayKOTTickets();
                        GunaToast.Show(this, $"Đã hoàn thành {kot.TicketCode}", UI.Controls.ToastType.Success, 2000, UI.Controls.ToastPos.TopRight);
                    }
                    else
                    {
                        GunaToast.Show(this, $"Không thể cập nhật trạng thái {kot.TicketCode}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
                    }
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi: {ex.Message}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
            }
        }

        private void MarkAsServed(int kotId)
        {
            try
            {
                var kot = _kotTickets.FirstOrDefault(k => k.KOTId == kotId);
                if (kot != null)
                {
                    // Cập nhật database
                    bool success = _kotBLL.CapNhatTrangThaiKOT(kotId, "ĐÃ ĐÓNG");
                    
                    if (success)
                    {
                        _kotTickets.Remove(kot);
                        UpdateStatistics();
                        DisplayKOTTickets();
                        GunaToast.Show(this, $"Đã phục vụ {kot.TicketCode}", UI.Controls.ToastType.Success, 2000, UI.Controls.ToastPos.TopRight);
                    }
                    else
                    {
                        GunaToast.Show(this, $"Không thể cập nhật trạng thái {kot.TicketCode}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
                    }
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi: {ex.Message}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
            }
        }

        private void UpdateStatistics()
        {
            var pendingCount = _kotTickets.Count(k => k.Status == KOTStatus.Pending);
            var inProgressCount = _kotTickets.Count(k => k.Status == KOTStatus.InProgress);
            var readyCount = _kotTickets.Count(k => k.Status == KOTStatus.Ready);

            lbChoLam.Text = pendingCount.ToString();
            lbDangLam.Text = inProgressCount.ToString();
            lbSanSang.Text = readyCount.ToString();

            segmentedPill1.Items[0].Text = $"Chờ làm ({pendingCount})";
            segmentedPill1.Items[1].Text = $"Đang làm ({inProgressCount})";
            segmentedPill1.Items[2].Text = $"Sẵn sàng ({readyCount})";

            var avgTime = CalculateAverageTime();
            lbThoiGianTB.Text = $"{avgTime} phút";
        }

        private int CalculateAverageTime()
        {
            var completedTickets = _kotTickets.Where(k => k.Status == KOTStatus.Ready).ToList();
            if (completedTickets.Count == 0) return 0;

            var totalMinutes = completedTickets.Sum(k => (DateTime.Now - k.OrderTime).TotalMinutes);
            return (int)(totalMinutes / completedTickets.Count);
        }
    }
}
