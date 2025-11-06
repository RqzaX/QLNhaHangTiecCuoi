using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using BLL;
using Guna.UI2.WinForms;

namespace UI
{
    public partial class FrmChiTietGoiTiec : Form
    {
        private int _goiId;
        private GoiTiecBLL _goiTiecBLL;
        private DataRow _goiTiecRow;
        private DataTable _monAnTable;
        private DataTable _dichVuTable;

        // Constants
        private const int SO_KHACH_MOI_BAN = 10;

        public FrmChiTietGoiTiec(int goiId)
        {
            InitializeComponent();
            _goiId = goiId;
            _goiTiecBLL = new GoiTiecBLL();
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Paint += FrmChiTietGoiTiec_Paint;
            this.Resize += FrmChiTietGoiTiec_Resize;
        }

        // Cập nhật lại Region khi form resize
        private void FrmChiTietGoiTiec_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        // Bo tròn form và vẽ viền đen
        private void FrmChiTietGoiTiec_Paint(object sender, PaintEventArgs e)
        {
            int borderRadius = 15;
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            GraphicsPath path = new GraphicsPath();
            
            path.AddArc(rect.X, rect.Y, borderRadius * 2, borderRadius * 2, 180, 90);
            path.AddArc(rect.Right - borderRadius * 2, rect.Y, borderRadius * 2, borderRadius * 2, 270, 90);
            path.AddArc(rect.Right - borderRadius * 2, rect.Bottom - borderRadius * 2, borderRadius * 2, borderRadius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - borderRadius * 2, borderRadius * 2, borderRadius * 2, 90, 90);
            path.CloseAllFigures();
            
            this.Region = new Region(path);
            
            // Vẽ viền đen
            using (Pen pen = new Pen(Color.Black, 2))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void FrmChiTietGoiTiec_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
                BuildUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            // Load thông tin gói tiệc
            _goiTiecRow = _goiTiecBLL.GetGoiTiecById(_goiId);
            if (_goiTiecRow == null)
            {
                throw new Exception("Không tìm thấy thông tin gói tiệc!");
            }

            // Load danh sách món ăn
            _monAnTable = _goiTiecBLL.GetMonAnByGoiId(_goiId);

            // Load danh sách dịch vụ
            _dichVuTable = _goiTiecBLL.GetDichVuByGoiId(_goiId);
        }

        private void BuildUI()
        {
            // Header
            lblTenGoi.Text = _goiTiecRow["ten_goi"].ToString();
            lblMoTa.Text = "Gói tiệc cao cấp với thực đơn đa dạng, nguyên liệu nhập khẩu";

            // Package Summary
            decimal giaCoBan = Convert.ToDecimal(_goiTiecRow["gia_co_ban"]);
            lblGiaMoiBan.Text = _goiTiecBLL.FormatTien(giaCoBan);
            lblSoKhachBan.Text = $"{SO_KHACH_MOI_BAN} khách";

            // Load món ăn theo nhóm
            LoadMonAnTheoNhom();

            // Load dịch vụ
            LoadDichVu();
        }

        private void LoadMonAnTheoNhom()
        {
            if (_monAnTable == null || _monAnTable.Rows.Count == 0)
                return;

            // Nhóm món ăn theo nhom
            var nhomGroups = _monAnTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("nhom") ?? "Khác")
                .OrderBy(g => GetNhomOrder(g.Key));

            int yPos = 10;
            foreach (var nhom in nhomGroups)
            {
                // Tạo section cho mỗi nhóm
                var section = CreateMonAnSection(nhom.Key, nhom.ToList(), yPos);
                panelContent.Controls.Add(section);
                yPos = section.Bottom + 15;
            }
        }

        private int GetNhomOrder(string nhom)
        {
            // Sắp xếp thứ tự: Món khai vị, Món chính, Canh & Súp, Tráng miệng
            if (nhom.Contains("khai vị") || nhom.Contains("Khai vị"))
                return 1;
            if (nhom.Contains("chính") || nhom.Contains("Chính"))
                return 2;
            if (nhom.Contains("canh") || nhom.Contains("súp") || nhom.Contains("Canh") || nhom.Contains("Súp"))
                return 3;
            if (nhom.Contains("tráng miệng") || nhom.Contains("Tráng miệng"))
                return 4;
            return 5;
        }

        private Panel CreateMonAnSection(string nhomTen, List<DataRow> monAnList, int yPos)
        {
            var sectionPanel = new Panel
            {
                Location = new Point(0, yPos),
                Width = panelContent.Width - 20,
                Height = 0,
                AutoSize = true,
                BackColor = Color.White
            };

            // Icon và tiêu đề
            var iconLabel = new Label
            {
                Text = GetIconForNhom(nhomTen),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(0, 0),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            sectionPanel.Controls.Add(iconLabel);

            var titleLabel = new Label
            {
                Text = nhomTen,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(iconLabel.Right + 10, 0),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            sectionPanel.Controls.Add(titleLabel);

            // Danh sách món ăn
            int itemY = titleLabel.Bottom + 10;
            foreach (var row in monAnList)
            {
                var itemLabel = new Label
                {
                    Text = $"• {row["ten_mon"].ToString()}",
                    Font = new Font("Segoe UI", 11),
                    Location = new Point(20, itemY),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(85, 85, 85)
                };
                sectionPanel.Controls.Add(itemLabel);
                itemY += itemLabel.Height + 5;
            }

            sectionPanel.Height = itemY + 5;
            return sectionPanel;
        }

        private string GetIconForNhom(string nhom)
        {
            if (nhom.Contains("khai vị") || nhom.Contains("Khai vị"))
                return "🍴";
            if (nhom.Contains("chính") || nhom.Contains("Chính"))
                return "👨‍🍳";
            if (nhom.Contains("canh") || nhom.Contains("súp") || nhom.Contains("Canh") || nhom.Contains("Súp"))
                return "🍲";
            if (nhom.Contains("tráng miệng") || nhom.Contains("Tráng miệng"))
                return "🍰";
            return "•";
        }

        private void LoadDichVu()
        {
            int yPos = panelContent.Controls.Count > 0 
                ? panelContent.Controls.Cast<Control>().Max(c => c.Bottom) + 30 
                : 10;

            // Tiêu đề Đồ uống (nếu có dịch vụ đồ uống)
            bool hasDrinks = false;
            if (_dichVuTable != null && _dichVuTable.Rows.Count > 0)
            {
                var drinkRows = _dichVuTable.AsEnumerable()
                    .Where(r => r.Field<string>("ten_dv")?.ToLower().Contains("đồ uống") == true ||
                               r.Field<string>("ten_dv")?.ToLower().Contains("bia") == true ||
                               r.Field<string>("ten_dv")?.ToLower().Contains("rượu") == true ||
                               r.Field<string>("ten_dv")?.ToLower().Contains("nước") == true)
                    .ToList();

                if (drinkRows.Count > 0)
                {
                    hasDrinks = true;
                    var titleLabel = new Label
                    {
                        Text = "🍷 Đồ uống",
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        Location = new Point(0, yPos),
                        AutoSize = true,
                        ForeColor = Color.FromArgb(52, 73, 94)
                    };
                    panelContent.Controls.Add(titleLabel);

                    yPos = titleLabel.Bottom + 10;
                    foreach (var row in drinkRows)
                    {
                        var itemLabel = new Label
                        {
                            Text = $"• {row["ten_dv"].ToString()}",
                            Font = new Font("Segoe UI", 11),
                            Location = new Point(20, yPos),
                            AutoSize = true,
                            ForeColor = Color.FromArgb(85, 85, 85)
                        };
                        panelContent.Controls.Add(itemLabel);
                        yPos += itemLabel.Height + 5;
                    }
                    yPos += 10;
                }
            }

            // Dịch vụ kèm theo
            if (_dichVuTable != null && _dichVuTable.Rows.Count > 0)
            {
                var serviceRows = _dichVuTable.AsEnumerable()
                    .Where(r => !hasDrinks || 
                               (r.Field<string>("ten_dv")?.ToLower().Contains("đồ uống") != true &&
                                r.Field<string>("ten_dv")?.ToLower().Contains("bia") != true &&
                                r.Field<string>("ten_dv")?.ToLower().Contains("rượu") != true &&
                                r.Field<string>("ten_dv")?.ToLower().Contains("nước") != true))
                    .ToList();

                if (serviceRows.Count > 0)
                {
                    var serviceLabel = new Label
                    {
                        Text = "⭐ Dịch vụ kèm theo",
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        Location = new Point(0, yPos),
                        AutoSize = true,
                        ForeColor = Color.FromArgb(52, 73, 94)
                    };
                    panelContent.Controls.Add(serviceLabel);

                    yPos = serviceLabel.Bottom + 10;
                    foreach (var row in serviceRows)
                    {
                        var itemLabel = new Label
                        {
                            Text = $"• {row["ten_dv"].ToString()}",
                            Font = new Font("Segoe UI", 11),
                            Location = new Point(20, yPos),
                            AutoSize = true,
                            ForeColor = Color.FromArgb(85, 85, 85)
                        };
                        panelContent.Controls.Add(itemLabel);
                        yPos += itemLabel.Height + 5;
                    }
                }
            }

            // Thêm thông tin bổ sung
            yPos += 30;
            AddAdditionalInfo(yPos);
        }

        // Thêm thông tin bổ sung vào panelContent
        private void AddAdditionalInfo(int startY)
        {
            int yPos = startY;
            
            // Thông tin về gói tiệc
            var infoPanel = new Guna2Panel
            {
                Location = new Point(0, yPos),
                Width = panelContent.Width - 20,
                Height = 0,
                AutoSize = true,
                FillColor = Color.FromArgb(245, 245, 250),
                BorderRadius = 8,
                Padding = new Padding(15, 15, 15, 15)
            };

            var infoTitle = new Label
            {
                Text = "📋 Thông tin chi tiết",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Location = new Point(0, 0),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            infoPanel.Controls.Add(infoTitle);

            yPos = infoTitle.Bottom + 15;
            
            // Thông tin về số lượng món
            if (_monAnTable != null && _monAnTable.Rows.Count > 0)
            {
                var monInfo = new Label
                {
                    Text = $"• Số lượng món ăn: {_monAnTable.Rows.Count} món",
                    Font = new Font("Segoe UI", 11),
                    Location = new Point(0, yPos),
                    AutoSize = true,
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent
                };
                infoPanel.Controls.Add(monInfo);
                yPos = monInfo.Bottom + 8;
            }

            // Thông tin về số lượng dịch vụ
            if (_dichVuTable != null && _dichVuTable.Rows.Count > 0)
            {
                var dvInfo = new Label
                {
                    Text = $"• Số lượng dịch vụ: {_dichVuTable.Rows.Count} dịch vụ",
                    Font = new Font("Segoe UI", 11),
                    Location = new Point(0, yPos),
                    AutoSize = true,
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent
                };
                infoPanel.Controls.Add(dvInfo);
                yPos = dvInfo.Bottom + 8;
            }

            // Thông tin về giá
            if (_goiTiecRow != null)
            {
                decimal giaCoBan = Convert.ToDecimal(_goiTiecRow["gia_co_ban"]);
                var giaInfo = new Label
                {
                    Text = $"• Giá cơ bản: {_goiTiecBLL.FormatTien(giaCoBan)}",
                    Font = new Font("Segoe UI", 11),
                    Location = new Point(0, yPos),
                    AutoSize = true,
                    ForeColor = Color.Black,
                    BackColor = Color.Transparent
                };
                infoPanel.Controls.Add(giaInfo);
                yPos = giaInfo.Bottom + 8;
            }

            // Thông tin về số khách
            var khachInfo = new Label
            {
                Text = $"• Số khách mỗi bàn: {SO_KHACH_MOI_BAN} khách",
                Font = new Font("Segoe UI", 11),
                Location = new Point(0, yPos),
                AutoSize = true,
                ForeColor = Color.Black,
                BackColor = Color.Transparent
            };
            infoPanel.Controls.Add(khachInfo);

            infoPanel.Height = khachInfo.Bottom + 15;
            panelContent.Controls.Add(infoPanel);

            // Cập nhật lại AutoScrollMinSize
            int maxBottom = panelContent.Controls.Cast<Control>().Max(c => c.Bottom);
            panelContent.AutoScrollMinSize = new Size(0, maxBottom + 20);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

