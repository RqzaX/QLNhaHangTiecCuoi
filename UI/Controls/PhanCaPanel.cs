using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using UI;

namespace UI.Controls
{
    public partial class PhanCaPanel : UserControl
    {
        private NguoiDungBLL _nguoiDungBLL;
        private DatabaseHelper _dbHelper;
        private FlowLayoutPanel _flowLayoutCa;

        public PhanCaPanel()
        {
            InitializeComponent();
            InitializeDataAccess();
            InitializeFlowLayout();
        }

        private void InitializeDataAccess()
        {
            try
            {
                _dbHelper = new DatabaseHelper();
                _nguoiDungBLL = new NguoiDungBLL(_dbHelper);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo kết nối: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeFlowLayout()
        {
            // Tạo FlowLayoutPanel để chứa các panel ca
            _flowLayoutCa = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(20, 20, 20, 20),
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            // Xóa các control cũ và thêm FlowLayoutPanel
            this.Controls.Clear();
            this.Controls.Add(_flowLayoutCa);
        }

        public void LoadDataPhanCa()
        {
            try
            {
                if (_nguoiDungBLL == null)
                {
                    InitializeDataAccess();
                }

                // Xóa các panel ca cũ
                _flowLayoutCa.Controls.Clear();

                // Load dữ liệu phân ca từ database
                DataTable dt = _nguoiDungBLL.LayDanhSachPhanCa();

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Nhóm theo ca
                    var caGroups = dt.AsEnumerable()
                        .GroupBy(row => new
                        {
                            CaId = Convert.ToInt32(row["ca_id"]),
                            TenCa = row["ten_ca"]?.ToString() ?? "",
                            GioBd = GetTimeSpanFromRow(row, "gio_bd"),
                            GioKt = GetTimeSpanFromRow(row, "gio_kt")
                        });

                    foreach (var caGroup in caGroups)
                    {
                        // Lấy chi_nhanh_id từ dữ liệu (lấy từ row đầu tiên trong group)
                        int chiNhanhId = 0;
                        if (caGroup.Any())
                        {
                            var firstRow = caGroup.First();
                            chiNhanhId = Convert.ToInt32(firstRow["chi_nhanh_id"]);
                        }

                        // Tạo panel cho mỗi ca
                        var caPanel = CreateCaPanel(
                            caGroup.Key.CaId,
                            chiNhanhId,
                            caGroup.Key.TenCa,
                            caGroup.Key.GioBd,
                            caGroup.Key.GioKt,
                            caGroup.ToList()
                        );

                        _flowLayoutCa.Controls.Add(caPanel);
                    }
                }
                else
                {
                    // Hiển thị thông báo không có dữ liệu
                    var lblNoData = new Label
                    {
                        Text = "Chưa có dữ liệu phân ca",
                        Font = new Font("Segoe UI", 12F),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    _flowLayoutCa.Controls.Add(lblNoData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu phân ca: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateCaPanel(int caId, int chiNhanhId, string tenCa, TimeSpan gioBd, TimeSpan gioKt, List<DataRow> nhanVienList)
        {
            var panel = new Panel
            {
                Size = new Size(350, 450),
                Margin = new Padding(15),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            // Label tên ca
            var lblTenCa = new Guna.UI2.WinForms.Guna2HtmlLabel
            {
                Text = tenCa,
                Font = new Font("Segoe UI", 13.8F, FontStyle.Regular),
                Location = new Point(12, 21),
                Size = new Size(200, 33),
                AutoSize = false
            };
            panel.Controls.Add(lblTenCa);

            // Panel giờ ca
            var panelGio = new Sunny.UI.UIPanel
            {
                Font = new Font("Microsoft Sans Serif", 12F),
                Location = new Point(155, 12),
                Size = new Size(191, 42),
                Radius = 18,
                Text = $"{gioBd:hh\\:mm}-{gioKt:hh\\:mm}",
                TextAlignment = ContentAlignment.MiddleCenter
            };
            panel.Controls.Add(panelGio);

            // Label nhân viên
            var lblNhanVien = new Guna.UI2.WinForms.Guna2HtmlLabel
            {
                Text = "Nhân Viên",
                Font = new Font("Segoe UI", 12F),
                Location = new Point(27, 74),
                Size = new Size(93, 30)
            };
            panel.Controls.Add(lblNhanVien);

            // FlowLayoutPanel cho danh sách nhân viên (điều chỉnh để có chỗ cho button)
            var flowNhanVien = new FlowLayoutPanel
            {
                Location = new Point(27, 110),
                Size = new Size(296, 250),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            // Thêm các panel nhân viên
            foreach (var row in nhanVienList)
            {
                string hoTen = row["ho_ten"]?.ToString() ?? "";
                var nvPanel = new Sunny.UI.UIPanel
                {
                    Font = new Font("Microsoft Sans Serif", 12F),
                    Size = new Size(269, 42),
                    Text = hoTen,
                    TextAlignment = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 0, 0, 10)
                };
                flowNhanVien.Controls.Add(nvPanel);
            }

            panel.Controls.Add(flowNhanVien);

            // Button chỉnh sửa (đặt ở dưới cùng, không bị che)
            var btnChinhSua = new Guna.UI2.WinForms.Guna2Button
            {
                FillColor = Color.FromArgb(192, 255, 192),
                Font = new Font("Segoe UI", 13.8F),
                ForeColor = Color.Black,
                Location = new Point(62, 375),
                Size = new Size(225, 56),
                Text = "Chỉnh Sửa",
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            
            // Điều chỉnh lại vị trí FlowLayoutPanel để không che button
            flowNhanVien.Height = btnChinhSua.Top - flowNhanVien.Top - 10;
            btnChinhSua.Click += (s, e) => {
                try
                {
                    if (_nguoiDungBLL == null)
                    {
                        InitializeDataAccess();
                    }

                    // Mở form chỉnh sửa phân ca
                    var frmSuaCa = new Frm_SuaCa(caId, chiNhanhId, tenCa, _nguoiDungBLL);
                    DialogResult result = frmSuaCa.ShowDialog();
                    
                    // Luôn reload dữ liệu sau khi đóng form (dù có thay đổi hay không)
                    // để đảm bảo dữ liệu luôn được cập nhật
                    LoadDataPhanCa();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi mở form chỉnh sửa: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel.Controls.Add(btnChinhSua);
            
            // Đảm bảo button luôn ở trên cùng (z-order)
            btnChinhSua.BringToFront();

            return panel;
        }

        private TimeSpan GetTimeSpanFromRow(DataRow row, string columnName)
        {
            if (row[columnName] == DBNull.Value || row[columnName] == null)
                return TimeSpan.Zero;

            if (row[columnName] is TimeSpan ts)
                return ts;

            if (TimeSpan.TryParse(row[columnName].ToString(), out TimeSpan parsed))
                return parsed;

            return TimeSpan.Zero;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadDataPhanCa();
        }
    }
}
