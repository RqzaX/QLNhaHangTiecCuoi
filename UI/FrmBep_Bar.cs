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
using UI.Common;
using UI.Controls;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmBep_Bar : Form
    {
        private DatabaseHelper _dbHelper;
        private KOTBLL _kotBLL;
        private List<KOTTicket> _kotTickets = new List<KOTTicket>();
        private KOTStatus _currentStatus = KOTStatus.Pending;

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
        }

        private void LoadKOTTickets()
        {
            try
            {
                if (!TestDatabaseConnection())
                {
                    CreateSampleData();
                    DisplayKOTTickets();
                    UpdateStatistics();
                    return;
                }
                
                if (Session.ChiNhanhId <= 0)
                {
                    CreateSampleData();
                    DisplayKOTTickets();
                    UpdateStatistics();
                    return;
                }
                
                var dt = _kotBLL.LayDanhSachKOT(Session.ChiNhanhId, null, null);
                
                ProcessKOTData(dt);
                
                if (_kotTickets.Count == 0)
                {
                    CreateSampleData();
                }
                
                DisplayKOTTickets();
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                CreateSampleData();
                DisplayKOTTickets();
                UpdateStatistics();
            }
        }

        private string GetStatusString(KOTStatus status)
        {
            switch (status)
            {
                case KOTStatus.Pending: return "ĐANG PHỤC VỤ";
                case KOTStatus.InProgress: return "CHỜ THANH TOÁN";
                case KOTStatus.Ready: return "ĐÃ ĐÓNG";
                default: return "ĐANG PHỤC VỤ"; // Default to Pending status
            }
        }


        private void ProcessKOTData(DataTable dt)
        {
            _kotTickets.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var soBan = row["so_ban"].ToString();
                    var tableName = soBan == "TIỆC" ? "Tiệc cưới" : $"Bàn {soBan}";
                    
                    var kot = new KOTTicket
                    {
                        KOTId = Convert.ToInt32(row["kot_id"]),
                        TicketCode = row["ma_kot"].ToString(),
                        TableName = tableName,
                        OrderTime = Convert.ToDateTime(row["thoi_gian_dat"]),
                        Status = GetStatusFromString(row["trang_thai"].ToString()),
                        IsPriority = Convert.ToBoolean(row["uu_tien"]),
                        Notes = row["ghi_chu"]?.ToString()
                    };
                    LoadKOTItems(kot);
                    _kotTickets.Add(kot);
                }
            }
            else
            {
            }
        }

        private void CreateSampleData()
        {            
            var sampleKOT1 = new KOTTicket
            {
                KOTId = 1,
                TicketCode = "KOT001",
                TableName = "Bàn 01",
                OrderTime = DateTime.Now.AddMinutes(-30),
                Status = KOTStatus.Pending,
                IsPriority = false,
                Notes = "Không cay"
            };
            sampleKOT1.Items.Add(new KOTItem { ItemId = 1, Name = "Phở bò", Quantity = 2 });
            sampleKOT1.Items.Add(new KOTItem { ItemId = 2, Name = "Bún bò Huế", Quantity = 1 });
            _kotTickets.Add(sampleKOT1);

            var sampleKOT2 = new KOTTicket
            {
                KOTId = 2,
                TicketCode = "KOT002",
                TableName = "Bàn 05",
                OrderTime = DateTime.Now.AddMinutes(-15),
                Status = KOTStatus.InProgress,
                IsPriority = true,
                Notes = "Ưu tiên"
            };
            sampleKOT2.Items.Add(new KOTItem { ItemId = 3, Name = "Cơm tấm", Quantity = 3 });
            sampleKOT2.Items.Add(new KOTItem { ItemId = 4, Name = "Canh chua", Quantity = 2 });
            _kotTickets.Add(sampleKOT2);

            var sampleKOT3 = new KOTTicket
            {
                KOTId = 3,
                TicketCode = "KOT003",
                TableName = "Tiệc cưới",
                OrderTime = DateTime.Now.AddMinutes(-45),
                Status = KOTStatus.Ready,
                IsPriority = false,
                Notes = ""
            };
            sampleKOT3.Items.Add(new KOTItem { ItemId = 5, Name = "Bánh mì", Quantity = 4 });
            _kotTickets.Add(sampleKOT3);
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
            int cardHeight = 300;
            int spacing = 20;
            int cardsPerRow = (panelDanhSach.Width - spacing) / (cardWidth + spacing);

            for (int i = 0; i < filteredTickets.Count; i++)
            {
                var kot = filteredTickets[i];
                var card = CreateKOTCard(kot);
                int row = i / cardsPerRow;
                int col = i % cardsPerRow;
                int x = spacing + col * (cardWidth + spacing);
                int y = spacing + row * (cardHeight + spacing);

                card.Location = new Point(x, y);
                card.Size = new Size(cardWidth, cardHeight);
                panelDanhSach.Controls.Add(card);
            }
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
                    break;
                case KOTStatus.InProgress:
                    card.ActionText = "Đã xong";
                    card.StartClicked += (s, e) => MarkAsReady(kot.KOTId);
                    break;
                case KOTStatus.Ready:
                    card.ActionText = "Đã phục vụ";
                    card.StartClicked += (s, e) => MarkAsServed(kot.KOTId);
                    break;
            }

            return card;
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
                        MessageBox.Show($"Đã bắt đầu làm {kot.TicketCode}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Không thể cập nhật trạng thái {kot.TicketCode}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show($"Đã hoàn thành {kot.TicketCode}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Không thể cập nhật trạng thái {kot.TicketCode}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show($"Đã phục vụ {kot.TicketCode}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Không thể cập nhật trạng thái {kot.TicketCode}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
