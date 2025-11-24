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
using UI.Controls;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmBaoCao : Form
    {
        private NguyenLieuBLL _nguyenLieuBLL;
        private DatabaseHelper _dbHelper;
        private BLL.HoaDonBLL _hoaDonBLL;

        public FrmBaoCao()
        {
            InitializeComponent();
            try
            {
                _dbHelper = new DatabaseHelper();
                if (!_dbHelper.TestConnection())
                {
                    MessageBox.Show("Không thể kết nối đến database!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _nguyenLieuBLL = new NguyenLieuBLL(_dbHelper);
                _hoaDonBLL = new BLL.HoaDonBLL(_dbHelper);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmBaoCao_Load(object sender, EventArgs e)
        {
           
            LoadPieChartHoaDon();
            
            LoadTop5MonBanChay();
        }

        private void FrmBaoCao_Activated(object sender, EventArgs e)
        {
            
            if (segmentedPill1.SelectedIndex == 0)
            {
                LoadPieChartHoaDon();
                LoadTop5MonBanChay();
            }
        }

        /// <summary>
        /// Load biểu đồ tròn hiển thị phân bổ hóa đơn theo loại (Nhà hàng và Tiệc cưới) vào PanelBieuDo
        /// </summary>
        private void LoadPieChartHoaDon()
        {
            try
            {
                if (_hoaDonBLL == null)
                {
                    System.Diagnostics.Debug.WriteLine("HoaDonBLL chưa được khởi tạo");
                    return;
                }

                // Xóa các control cũ trong PanelBieuDo
                PanelBieuDo.Controls.Clear();

                // Lấy số lượng hóa đơn theo loại (null = tất cả chi nhánh)
                var (nhaHang, tiecCuoi) = _hoaDonBLL.GetHoaDonCountByLoai(null);

                // Tính tổng để tính phần trăm
                int tong = nhaHang + tiecCuoi;
                
                float phanTramNhaHang = 0f;
                float phanTramTiecCuoi = 0f;

                if (tong == 0)
                {
                    // Nếu không có dữ liệu, hiển thị 50-50
                    phanTramNhaHang = 50f;
                    phanTramTiecCuoi = 50f;
                }
                else
                {
                    // Tính phần trăm
                    phanTramNhaHang = (float)(nhaHang * 100.0 / tong);
                    phanTramTiecCuoi = (float)(tiecCuoi * 100.0 / tong);
                }

                // Tạo biểu đồ tròn
                var pieChart = new UI.Controls.MiniPieChart
                {
                    Dock = DockStyle.Fill,
                    Title = "Phân bổ hóa đơn theo loại (%)",
                    Labels = new string[] { "Nhà hàng", "Tiệc cưới" },
                    Values = new float[] { phanTramNhaHang, phanTramTiecCuoi },
                    Colors = new Color[]
                    {
                        Color.FromArgb(66, 133, 244),  // Xanh dương cho Nhà hàng
                        Color.FromArgb(245, 158, 11)    // Vàng cam cho Tiệc cưới
                    },
                    CornerRadius = 18,
                    Font = new Font("Segoe UI", 10F)
                };

                PanelBieuDo.Controls.Add(pieChart);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load biểu đồ tròn: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void segmentedPill1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (segmentedPill1.SelectedIndex == 0)
            {
                PanelCanhBao.Visible = false;
                PanelBanChay.Visible = true;
                PanelBieuDo.Visible = true;
                // Reload top 5 món bán chạy khi chuyển về tab báo cáo
                LoadTop5MonBanChay();
            }
            else if (segmentedPill1.SelectedIndex == 1)
            {
                PanelCanhBao.Visible = true;
                PanelBanChay.Visible = false;
                PanelBieuDo.Visible= false;
               
                LoadCanhBaoNguyenLieu();
            }
        }

        /// <summary>
        /// Load danh sách nguyên liệu sắp hết và hết hàng vào PanelCanhBao
        /// </summary>
        private void LoadCanhBaoNguyenLieu()
        {
            try
            {
                if (_nguyenLieuBLL == null)
                {
                    MessageBox.Show("Chưa khởi tạo BLL!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Xóa các panel cũ
                PanelCanhBao.Controls.Clear();

                const decimal NGUONG_CANH_BAO = 30m; // Ngưỡng cảnh báo: số lượng <= 30

                // Load nguyên liệu số lượng = 0 (hết hàng)
                DataTable dtHetHang = _nguyenLieuBLL.LayTonKhoTheoTinhTrang(2, null); // tinhTrang = 2: số lượng = 0
                
                // Load nguyên liệu sắp hết (số lượng > 0 và <= 30)
                DataTable dtSapHet = _nguyenLieuBLL.LayTonKhoTheoTinhTrang(3, null, NGUONG_CANH_BAO); // tinhTrang = 3: sắp hết, canhBao = 30

                int yPosition = 10; // Vị trí Y ban đầu
                const int spacing = 10; // Khoảng cách giữa các panel
                const int panelHeight = 64; // Chiều cao của mỗi panel

                // Hiển thị nguyên liệu hết hàng trước (màu đỏ)
                if (dtHetHang != null && dtHetHang.Rows.Count > 0)
                {
                    foreach (DataRow row in dtHetHang.Rows)
                    {
                        string tenNL = row["ten_nl"]?.ToString() ?? "Chưa có tên";
                        decimal slTon = Convert.ToDecimal(row["sl_ton"] ?? 0);

                        var panel = new CanhBaoNLPanel
                        {
                            Location = new Point(10, yPosition),
                            Width = PanelCanhBao.Width - 30, // Trừ margin
                            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                        };
                        panel.SetData(tenNL, slTon, hetHang: true);
                        
                        // Đăng ký event click để mở FrmKho
                        panel.PanelClicked += (sender, e) =>
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
                                    // Nếu không tìm thấy FrmTrangChu, mở dialog như cũ
                                    FrmKho frm = new FrmKho();
                                    frm.ShowDialog(this);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi mở form kho: {ex.Message}", "Lỗi", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        
                        PanelCanhBao.Controls.Add(panel);
                        yPosition += panelHeight + spacing;
                    }
                }

                if (dtSapHet != null && dtSapHet.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSapHet.Rows)
                    {
                        string tenNL = row["ten_nl"]?.ToString() ?? "Chưa có tên";
                        decimal slTon = Convert.ToDecimal(row["sl_ton"] ?? 0);

                        var panel = new CanhBaoNLPanel
                        {
                            Location = new Point(10, yPosition),
                            Width = PanelCanhBao.Width - 30, // Trừ margin
                            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                        };
                        panel.SetData(tenNL, slTon, hetHang: false);
                        
                        // Đăng ký event click để mở FrmKho
                        panel.PanelClicked += (sender, e) =>
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
                                    // Nếu không tìm thấy FrmTrangChu, mở dialog như cũ
                                    FrmKho frm = new FrmKho();
                                    frm.ShowDialog(this);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi mở form kho: {ex.Message}", "Lỗi", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };
                        
                        PanelCanhBao.Controls.Add(panel);
                        yPosition += panelHeight + spacing;
                    }
                }

            
                if ((dtHetHang == null || dtHetHang.Rows.Count == 0) && 
                    (dtSapHet == null || dtSapHet.Rows.Count == 0))
                {
                    var lblNoData = new Label
                    {
                        Text = "Không có nguyên liệu nào cần cảnh báo",
                        Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                        ForeColor = Color.FromArgb(107, 114, 128),
                        AutoSize = true,
                        Location = new Point(20, 20),
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    };
                    PanelCanhBao.Controls.Add(lblNoData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu cảnh báo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
      
        /// </summary>
        private void LoadTop5MonBanChay()
        {
            try
            {
                if (_hoaDonBLL == null)
                {
                    System.Diagnostics.Debug.WriteLine("HoaDonBLL chưa được khởi tạo");
                    return;
                }

               
                var controlsToRemove = new List<Control>();
                foreach (Control ctrl in PanelBanChay.Controls)
                {
                   
                    if (ctrl.Name != "label17")
                    {
                        controlsToRemove.Add(ctrl);
                    }
                }
                foreach (var ctrl in controlsToRemove)
                {
                    PanelBanChay.Controls.Remove(ctrl);
                    ctrl.Dispose();
                }

               
                DataTable dt = _hoaDonBLL.GetTop5MonBanChay(null);

                if (dt == null || dt.Rows.Count == 0)
                {
                   
                    var lblNoData = new Label
                    {
                        Text = "Chưa có dữ liệu món bán chạy",
                        Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                        ForeColor = Color.FromArgb(107, 114, 128),
                        AutoSize = true,
                        Location = new Point(20, 60),
                        Anchor = AnchorStyles.Top | AnchorStyles.Left
                    };
                    PanelBanChay.Controls.Add(lblNoData);
                    return;
                }

                // Tạo panel container với FlowLayoutPanel để tự động sắp xếp
                // Đặt flowPanel ở dưới label17 (tiêu đề) để không che mất tiêu đề
                int topPosition = label17 != null ? label17.Bottom + 10 : 50;
                var flowPanel = new FlowLayoutPanel
                {
                    Location = new Point(0, topPosition),
                    Size = new Size(PanelBanChay.Width, PanelBanChay.Height - topPosition),
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                    FlowDirection = FlowDirection.TopDown,
                    AutoScroll = true,
                    Padding = new Padding(15, 0, 15, 15),
                    WrapContents = false
                };
                PanelBanChay.Controls.Add(flowPanel);

                int stt = 1;
                foreach (DataRow row in dt.Rows)
                {
                    string tenMon = row["ten_hang"]?.ToString() ?? "Chưa có tên";
                    decimal tongSoLuong = Convert.ToDecimal(row["tong_so_luong"] ?? 0);
                    decimal tongTien = Convert.ToDecimal(row["tong_tien"] ?? 0);
                    
                    // Capture giá trị stt vào biến local để tránh closure issue
                    int currentStt = stt;

                    // Tạo panel cho mỗi món
                    var itemPanel = new Panel
                    {
                        Size = new Size(flowPanel.Width - 30, 70),
                        Margin = new Padding(0, 0, 0, 12),
                        BackColor = Color.FromArgb(249, 250, 251),
                        BorderStyle = BorderStyle.None
                    };

                   
                    itemPanel.Paint += (s, e) =>
                    {
                        using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                        {
                            e.Graphics.DrawRectangle(pen, 0, 0, itemPanel.Width - 1, itemPanel.Height - 1);
                        }
                    };

                    // Badge số thứ tự
                    var badgePanel = new Panel
                    {
                        Size = new Size(50, 50),
                        Location = new Point(15, 10),
                        BackColor = Color.FromArgb(219, 234, 254),
                        BorderStyle = BorderStyle.None
                    };
                    badgePanel.Paint += (s, e) =>
                    {
                        using (var pen = new Pen(Color.FromArgb(59, 130, 246), 2))
                        {
                            e.Graphics.DrawRectangle(pen, 1, 1, badgePanel.Width - 3, badgePanel.Height - 3);
                        }
                        using (var brush = new SolidBrush(Color.FromArgb(17, 24, 39)))
                        using (var font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold))
                        {
                            var text = $"#{currentStt}";
                            var sf = new StringFormat
                            {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center
                            };
                            e.Graphics.DrawString(text, font, brush, badgePanel.ClientRectangle, sf);
                        }
                    };
                    itemPanel.Controls.Add(badgePanel);

                    // Label tên món
                    var lblTenMon = new Label
                    {
                        Text = tenMon,
                        Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                        ForeColor = Color.FromArgb(17, 24, 39),
                        AutoSize = false,
                        Size = new Size(itemPanel.Width - 250, 30),
                        Location = new Point(80, 20),
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    itemPanel.Controls.Add(lblTenMon);

                    // Label tổng số tiền
                    var lblTongTien = new Label
                    {
                        Text = tongTien.ToString("N0") + " đ",
                        Font = new Font("Segoe UI", 11f, FontStyle.Regular),
                        ForeColor = Color.FromArgb(59, 130, 246),
                        AutoSize = false,
                        Size = new Size(150, 30),
                        Location = new Point(itemPanel.Width - 170, 20),
                        TextAlign = ContentAlignment.MiddleRight
                    };
                    itemPanel.Controls.Add(lblTongTien);

                    flowPanel.Controls.Add(itemPanel);
                    stt++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load top 5 món bán chạy: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tìm parent form theo kiểu T bằng cách đi lên cây control
        /// </summary>
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
    }

}
