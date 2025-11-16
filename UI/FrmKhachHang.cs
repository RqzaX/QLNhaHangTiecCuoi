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
using QLNhaHangTiecCuoi.BLL;
using UI.Common;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmKhachHang : Form
    {
        private const string KH_TEN = "TenKH";
        private const string KH_LH = "LienHe";
        private const string KH_HANG = "Hang";
        private const string KH_TONG = "TongChiTieu";
        private const string KH_CUOI = "LanCuoi";
        private const string KH_GHICHU = "GhiChu";

        private KhachHangBLL _khachHangBLL;
        private System.Windows.Forms.Timer _searchTimer;
        private QLNhaHangTiecCuoi.Share.DatabaseHelper _dbHelper;
        private BLL.HoaDonBLL _hoaDonBLL;
        private QLNhaHangTiecCuoi.BLL.DatSanhBLL _datSanhBLL;

        public FrmKhachHang()
        {
            InitializeComponent();
            _khachHangBLL = new KhachHangBLL();
            _dbHelper = new QLNhaHangTiecCuoi.Share.DatabaseHelper();
            _hoaDonBLL = new BLL.HoaDonBLL(_dbHelper);
            _datSanhBLL = new QLNhaHangTiecCuoi.BLL.DatSanhBLL();

            // Khởi tạo timer cho tìm kiếm real-time (delay 300ms)
            _searchTimer = new System.Windows.Forms.Timer();
            _searchTimer.Interval = 300;
            _searchTimer.Tick += SearchTimer_Tick;

            // Đăng ký event handlers
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
            cbbLocHang.SelectedIndexChanged += CbbLocHang_SelectedIndexChanged;

            // Load danh sách hạng vào combobox
            LoadComboBoxHang();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void dgvKhachHang_MouseEnter(object sender, EventArgs e)
        {

        }

        private void dgvKhachHang_MouseLeave(object sender, EventArgs e)
        {

        }
        private void dgvKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void InitDgvKhachHang()
        {
            var dgv = dgvKhachHang;
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Xóa các cột cũ nếu có (từ Designer)
            dgv.Columns.Clear();

            // Tất cả cột dùng Fill để tràn đều
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_TEN, HeaderText = "Khách hàng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 20 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_LH, HeaderText = "Liên hệ", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 20 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_HANG, HeaderText = "Hạng", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 12 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_TONG, HeaderText = "Chi tiêu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 20 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_GHICHU, HeaderText = "Ghi chú", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 28 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_CUOI, HeaderText = "Lần cuối", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 4, Visible = false });

            // Style
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgv.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.RowTemplate.Height = 56;

            // 2 cột hiển thị 2 dòng
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.Columns[KH_TEN].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.Columns[KH_LH].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.Columns[KH_TONG].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Sự kiện custom + click + hover

        }


        private void dgvKhachHang_CellFormatting_ColorHang(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvKhachHang.Columns[e.ColumnIndex].Name != "Hang") return;

            string val = (e.Value ?? "").ToString().Trim();

            // Màu chữ theo hạng
            if (val.Equals("VIP", StringComparison.OrdinalIgnoreCase))
                e.CellStyle.ForeColor = Color.FromArgb(126, 34, 206);      // tím
            else if (val.Equals("Vàng", StringComparison.OrdinalIgnoreCase))
                e.CellStyle.ForeColor = Color.FromArgb(161, 98, 7);        // vàng đậm
            else if (val.Equals("Bạc", StringComparison.OrdinalIgnoreCase))
                e.CellStyle.ForeColor = Color.FromArgb(71, 85, 105);       // xám
            else
                e.CellStyle.ForeColor = Color.FromArgb(23, 23, 23);        // mặc định

            e.CellStyle.Font = new Font("Segoe UI Semibold", 10f);         // cho chữ “Hạng” nổi hơn
        }
        private void LoadDataKhachHang(string keyword = null, string hangCode = null)
        {
            try
            {
                dgvKhachHang.Rows.Clear();

                DataTable dt = _khachHangBLL.LayDanhSachKhachHangChiTiet(keyword, hangCode);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string hoTen = row["ho_ten"]?.ToString() ?? "";
                        DateTime? ngaySinh = row["ngay_sinh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ngay_sinh"]);
                        string sdt = row["sdt"]?.ToString() ?? "";
                        string email = row["email"]?.ToString() ?? "";
                        string tenHang = row["ten_hang"]?.ToString() ?? "Thành viên";
                        decimal tongChiTieu = row["tong_chi_tieu"] == DBNull.Value ? 0 : Convert.ToDecimal(row["tong_chi_tieu"]);
                        int soLanDen = row["so_lan_den"] == DBNull.Value ? 0 : Convert.ToInt32(row["so_lan_den"]);
                        int diem = row["diem"] == DBNull.Value ? 0 : Convert.ToInt32(row["diem"]);
                        DateTime? lanCuoiDen = row["lan_cuoi_den"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["lan_cuoi_den"]);
                        decimal? conLaiLenHang = row["con_lai_len_hang"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(row["con_lai_len_hang"]);
                        string ghiChu = row["ghi_chu"]?.ToString() ?? "";

                        // Format tên + ngày sinh
                        string tenCell = hoTen;
                        if (ngaySinh.HasValue)
                        {
                            tenCell += $"\nSN: {ngaySinh.Value:dd/M/yyyy}";
                        }

                        // Format liên hệ với icon (sẽ vẽ bằng CellPainting)
                        string lhCell = $"{sdt}\n{email}";

                        // Format chi tiêu: tổng chi tiêu + "Còn X ₫ lên hạng"
                        string chiTieuCell = Money(tongChiTieu);
                        if (conLaiLenHang.HasValue && conLaiLenHang.Value > 0)
                        {
                            chiTieuCell += $"\nCòn {Money(conLaiLenHang.Value)} lên hạng";
                        }

                        int khachHangId = Convert.ToInt32(row["khach_hang_id"]);
                        var newRow = dgvKhachHang.Rows.Add(
                            tenCell,
                            lhCell,
                            tenHang,
                            chiTieuCell,
                            ghiChu,
                            lanCuoiDen.HasValue ? lanCuoiDen.Value.ToString("d/M/yyyy") : ""
                        );
                        // Lưu khach_hang_id vào Tag để có thể lấy lại khi double click
                        dgvKhachHang.Rows[newRow].Tag = khachHangId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Money(decimal v) => string.Format("{0:#,0} ₫", v).Replace(",", ".");

        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            InitDgvKhachHang();
            LoadThongKe();
            LoadDataKhachHang();
            LoadChiTietHangThanhVien();
            LoadHoaDonGiaoDich();
        }

        private void LoadThongKe()
        {
            try
            {
                // Tổng khách hàng
                int tongKhachHang = _khachHangBLL.DemTongSoKhachHang();
                label4.Text = tongKhachHang.ToString();

                // Khách hạng VIP
                int khachVip = _khachHangBLL.DemKhachHangTheoHang("VIP");
                label7.Text = khachVip.ToString();

                // Khách hạng Vàng
                int khachVang = _khachHangBLL.DemKhachHangTheoHang("VANG");
                label10.Text = khachVang.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhachHang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvKhachHang.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Cột Thao tác đã được bỏ
        }

        private void dgvKhachHang_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            string col = dgv.Columns[e.ColumnIndex].Name;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Tên KH: 2 dòng (tên + ngày sinh)
            if (col == KH_TEN)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                var parts = (e.FormattedValue?.ToString() ?? "").Split('\n');
                string top = parts.ElementAtOrDefault(0) ?? "";
                string sub = parts.ElementAtOrDefault(1) ?? "";

                var r = Rectangle.Inflate(e.CellBounds, -8, -6);
                using var fTop = new Font(e.CellStyle.Font, FontStyle.Regular);
                using var fSub = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size - 1f);
                using var brTop = new SolidBrush(e.CellStyle.ForeColor);
                using var brSub = new SolidBrush(Color.FromArgb(110, 119, 135));

                g.DrawString(top, fTop, brTop, new RectangleF(r.X, r.Y + 2, r.Width, r.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                if (!string.IsNullOrEmpty(sub))
                {
                    g.DrawString(sub, fSub, brSub, new RectangleF(r.X, r.Y + r.Height / 2f - 2, r.Width, r.Height / 2f),
                        new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                }

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Liên hệ: 2 dòng với icon phone và email
            if (col == KH_LH)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                var parts = (e.FormattedValue?.ToString() ?? "").Split('\n');
                string phone = parts.ElementAtOrDefault(0) ?? "";
                string email = parts.ElementAtOrDefault(1) ?? "";

                var r = Rectangle.Inflate(e.CellBounds, -8, -6);
                using var f = new Font(e.CellStyle.Font, FontStyle.Regular);
                using var fSub = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size - 1f);
                using var br = new SolidBrush(e.CellStyle.ForeColor);
                using var brSub = new SolidBrush(Color.FromArgb(110, 119, 135));

                // Vẽ icon phone (📞) và số điện thoại
                if (!string.IsNullOrEmpty(phone))
                {
                    float iconSize = 14f;
                    float yPhone = r.Y + 2;
                    g.DrawString("📞", f, br, new PointF(r.X, yPhone));
                    g.DrawString(phone, f, br, new RectangleF(r.X + iconSize + 4, yPhone, r.Width - iconSize - 4, r.Height / 2f),
                        new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                }

                // Vẽ icon email (✉️) và email
                if (!string.IsNullOrEmpty(email))
                {
                    float iconSize = 14f;
                    float yEmail = r.Y + r.Height / 2f - 2;
                    g.DrawString("✉️", fSub, brSub, new PointF(r.X, yEmail));
                    g.DrawString(email, fSub, brSub, new RectangleF(r.X + iconSize + 4, yEmail, r.Width - iconSize - 4, r.Height / 2f),
                        new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                }

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Chi tiêu: 2 dòng (tổng chi tiêu + "Còn X ₫ lên hạng")
            if (col == KH_TONG)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                var parts = (e.FormattedValue?.ToString() ?? "").Split('\n');
                string tongChi = parts.ElementAtOrDefault(0) ?? "";
                string conLai = parts.ElementAtOrDefault(1) ?? "";

                var r = Rectangle.Inflate(e.CellBounds, -8, -6);
                using var fTop = new Font(e.CellStyle.Font, FontStyle.Regular);
                using var fSub = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size - 1f);
                using var brTop = new SolidBrush(e.CellStyle.ForeColor);
                using var brSub = new SolidBrush(Color.FromArgb(110, 119, 135));

                g.DrawString(tongChi, fTop, brTop, new RectangleF(r.X, r.Y + 2, r.Width, r.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                if (!string.IsNullOrEmpty(conLai))
                {
                    g.DrawString(conLai, fSub, brSub, new RectangleF(r.X, r.Y + r.Height / 2f - 2, r.Width, r.Height / 2f),
                        new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                }

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Hạng: chip màu
            if (col == KH_HANG)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string hang = Convert.ToString(e.FormattedValue) ?? "";
                var (bg, fg) = HangColors(hang);

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 26) / 2, 96, 26);
                using var path = Rounded(chip, 13);
                using var fill = new SolidBrush(bg);
                using var br = new SolidBrush(fg);

                g.FillPath(fill, path);
                g.DrawString(hang, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

        }
        private (Color bg, Color fg) HangColors(string hang)
        {
            switch (hang.Trim().ToLowerInvariant())
            {
                case "vàng": return (Color.FromArgb(255, 246, 204), Color.FromArgb(161, 98, 7));
                case "bạc": return (Color.FromArgb(229, 231, 235), Color.FromArgb(75, 85, 99));
                case "vip": return (Color.FromArgb(237, 233, 254), Color.FromArgb(91, 33, 182));
                default: return (Color.FromArgb(229, 231, 235), Color.FromArgb(55, 65, 81)); // Thành viên
            }
        }

        private static GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private void DrawStar(Graphics g, Rectangle r, Color c)
        {
            using var sb = new SolidBrush(c);
            PointF[] pts = new PointF[10];
            double cx = r.Left + r.Width / 2.0, cy = r.Top + r.Height / 2.0;
            double R = r.Width / 2.0, r2 = R * 0.5, a = -Math.PI / 2;
            for (int i = 0; i < 10; i++)
            {
                double rad = (i % 2 == 0) ? R : r2;
                pts[i] = new PointF((float)(cx + rad * Math.Cos(a)), (float)(cy + rad * Math.Sin(a)));
                a += Math.PI / 5.0;
            }
            g.FillPolygon(sb, pts);
        }

        private void dgvKhachHang_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvKhachHang.Cursor = Cursors.Default;
        }

        private void dgvKhachHang_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvKhachHang.Cursor = Cursors.Default;
        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_ThemKHMoi())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    // Reload dữ liệu sau khi thêm khách hàng mới
                    LoadThongKe();
                    PerformSearch(); // Reload danh sách với filter hiện tại
                }
            }
        }

        private void LoadComboBoxHang()
        {
            try
            {
                cbbLocHang.Items.Clear();
                cbbLocHang.Items.Add("Tất Cả Hạng");
                cbbLocHang.Items.Add("VIP");
                cbbLocHang.Items.Add("Vàng");
                cbbLocHang.Items.Add("Bạc");
                cbbLocHang.Items.Add("Thành viên");
                cbbLocHang.SelectedIndex = 0; // Mặc định chọn "Tất Cả Hạng"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load combo box hạng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            // Dừng timer cũ nếu có
            _searchTimer.Stop();

            // Bắt đầu timer mới - sẽ trigger search sau 300ms khi user ngừng gõ
            _searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            // Dừng timer
            _searchTimer.Stop();

            // Thực hiện tìm kiếm
            PerformSearch();
        }

        private void CbbLocHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            try
            {
                string keyword = txtTimKiem.Text?.Trim() ?? "";
                string hangCode = GetSelectedHangCode();

                // Load danh sách với filter
                LoadDataKhachHang(keyword, hangCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetSelectedHangCode()
        {
            if (cbbLocHang.SelectedIndex < 0) return null;

            string selectedText = cbbLocHang.SelectedItem?.ToString();

            switch (selectedText)
            {
                case "Tất Cả Hạng":
                    return "ALL";
                case "VIP":
                    return "VIP";
                case "Vàng":
                    return "VANG";
                case "Bạc":
                    return "BAC";
                case "Thành viên":
                    return "MEM";
                default:
                    return "ALL";
            }
        }

        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Chỉ xử lý khi click vào cell (không phải header)
            if (e.RowIndex < 0) return;

            try
            {
                // Lấy khach_hang_id từ Tag của row
                if (dgvKhachHang.Rows[e.RowIndex].Tag is int khachHangId)
                {
                    // Mở form chi tiết khách hàng
                    using (var frm = new Frm_ChiTietKhachHang(khachHangId))
                    {
                        frm.StartPosition = FormStartPosition.CenterParent;
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            // Reload dữ liệu sau khi sửa/xóa
                            LoadDataKhachHang();
                            LoadThongKe();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở form chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietHangThanhVien()
        {
            try
            {
                panelCTTT.Controls.Clear();
                panelCTTT.AutoScroll = false;
                panelCTTT.Padding = new Padding(20);

                // Tạo TableLayoutPanel để sắp xếp 2x2
                TableLayoutPanel tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 2,
                    AutoSize = false,
                    Padding = new Padding(10)
                };
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

                // Hạng VIP
                CreateHangPanel(tableLayout, "VIP", "Hạng VIP",
                    Color.FromArgb(237, 233, 254), Color.FromArgb(221, 214, 254),
                    Color.FromArgb(91, 33, 182), "🏆",
                    new string[] {
                        "Chi tiêu tích lũy: ≥ 30 triệu",
                        "Giảm 20% mọi đơn hàng",
                        "Tặng voucher sinh nhật 2 triệu",
                        "Ưu tiên đặt chỗ & phục vụ",
                        "Tích điểm x2"
                    }, 0, 0);

                // Hạng Vàng
                CreateHangPanel(tableLayout, "VANG", "Hạng Vàng",
                    Color.FromArgb(255, 246, 204), Color.FromArgb(254, 240, 138),
                    Color.FromArgb(161, 98, 7), "🥇",
                    new string[] {
                        "Chi tiêu tích lũy: 15-30 triệu",
                        "Giảm 15% mọi đơn hàng",
                        "Tặng voucher sinh nhật 1 triệu",
                        "Tích điểm x1.5"
                    }, 0, 1);

                // Hạng Bạc
                CreateHangPanel(tableLayout, "BAC", "Hạng Bạc",
                    Color.FromArgb(243, 244, 246), Color.FromArgb(229, 231, 235),
                    Color.FromArgb(75, 85, 99), "🥈",
                    new string[] {
                        "Chi tiêu tích lũy: 5-15 triệu",
                        "Giảm 10% mọi đơn hàng",
                        "Tặng voucher sinh nhật 500K",
                        "Tích điểm x1.2"
                    }, 1, 0);

                // Thành viên
                CreateHangPanel(tableLayout, "MEM", "Thành viên",
                    Color.FromArgb(219, 234, 254), Color.FromArgb(191, 219, 254),
                    Color.FromArgb(30, 64, 175), "💙",
                    new string[] {
                        "Chi tiêu tích lũy: < 5 triệu",
                        "Giảm 5% mọi đơn hàng",
                        "Tích điểm cơ bản",
                        "Nhận tin khuyến mãi"
                    }, 1, 1);

                panelCTTT.Controls.Add(tableLayout);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load chi tiết hạng thành viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateHangPanel(TableLayoutPanel parent, string hangCode, string title,
            Color bgColor1, Color bgColor2, Color textColor, string icon,
            string[] benefits, int row, int col)
        {
            // Panel chính
            Guna.UI2.WinForms.Guna2GradientPanel panel = new Guna.UI2.WinForms.Guna2GradientPanel
            {
                Margin = new Padding(12),
                BorderRadius = 20,
                FillColor = bgColor1,
                FillColor2 = bgColor2,
                BorderThickness = 2,
                BorderColor = Color.FromArgb(180, 180, 180),
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 30, 30, 30),
                Cursor = Cursors.Hand
            };

            // Thêm event click để mở form chi tiết
            EventHandler clickHandler = (s, e) => OpenChiTietHang(hangCode, title, bgColor1, bgColor2, textColor, icon, benefits);
            panel.Click += clickHandler;
            panel.MouseEnter += (s, e) => panel.BorderColor = Color.FromArgb(150, 150, 150);
            panel.MouseLeave += (s, e) => panel.BorderColor = Color.FromArgb(180, 180, 180);

            // Title - không có icon, chỉ hiển thị text
            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = textColor,
                Location = new Point(30, 30),
                AutoSize = true,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            lblTitle.Click += clickHandler;
            panel.Controls.Add(lblTitle);

            // Benefits container - sử dụng Panel với vị trí cố định
            Panel benefitsContainer = new Panel
            {
                Location = new Point(30, 75),
                BackColor = Color.Transparent,
                AutoScroll = false,
                Cursor = Cursors.Hand
            };
            benefitsContainer.Click += clickHandler;

            // Set kích thước ban đầu
            void UpdateBenefitsContainerSize()
            {
                if (panel.Width > 0 && panel.Height > 0)
                {
                    benefitsContainer.Left = 30;
                    benefitsContainer.Top = 75;
                    benefitsContainer.Width = panel.Width - 60;
                    benefitsContainer.Height = panel.Height - 105;
                }
            }

            panel.SizeChanged += (s, e) => UpdateBenefitsContainerSize();
            panel.Controls.Add(benefitsContainer);

            // Benefits - thêm trực tiếp vào container với vị trí cố định
            int yPos = 0;
            foreach (string benefit in benefits)
            {
                // Tạo panel cho mỗi benefit
                Panel benefitItem = new Panel
                {
                    Location = new Point(0, yPos),
                    Height = 32,
                    Width = benefitsContainer.Width,
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand
                };
                benefitItem.Click += clickHandler;

                // Vẽ icon sao
                Panel starIcon = new Panel
                {
                    Size = new Size(18, 18),
                    Location = new Point(0, 7),
                    BackColor = Color.Transparent
                };
                starIcon.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // Vẽ sao 5 cánh rỗng
                    var points = new PointF[10];
                    float centerX = starIcon.Width / 2f;
                    float centerY = starIcon.Height / 2f;
                    float outerRadius = 7f;
                    float innerRadius = 3.5f;

                    for (int i = 0; i < 10; i++)
                    {
                        float angle = (float)(i * Math.PI / 5 - Math.PI / 2);
                        float radius = i % 2 == 0 ? outerRadius : innerRadius;
                        points[i] = new PointF(
                            centerX + radius * (float)Math.Cos(angle),
                            centerY + radius * (float)Math.Sin(angle)
                        );
                    }

                    using (var pen = new Pen(textColor, 1.5f))
                    {
                        g.DrawPolygon(pen, points);
                    }
                };
                benefitItem.Controls.Add(starIcon);

                // Label text
                Label lblBenefit = new Label
                {
                    Text = benefit,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    ForeColor = Color.FromArgb(70, 70, 70),
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    Location = new Point(25, 6),
                    UseCompatibleTextRendering = true,
                    Cursor = Cursors.Hand
                };
                lblBenefit.Click += clickHandler;
                benefitItem.Controls.Add(lblBenefit);

                benefitsContainer.Controls.Add(benefitItem);
                yPos += 40; // Khoảng cách giữa các benefit
            }

            // Cập nhật width của các benefitItem khi container resize
            benefitsContainer.SizeChanged += (s, e) =>
            {
                foreach (Control ctrl in benefitsContainer.Controls)
                {
                    if (ctrl is Panel benefitItem)
                    {
                        benefitItem.Width = benefitsContainer.Width;
                    }
                }
            };

            parent.Controls.Add(panel, col, row);

            // Update kích thước sau khi panel được thêm vào parent
            panel.HandleCreated += (s, e) =>
            {
                UpdateBenefitsContainerSize();
            };

            // Cũng gọi ngay nếu panel đã có kích thước
            UpdateBenefitsContainerSize();

            // Force update sau khi thêm vào parent
            panel.ParentChanged += (s, e) =>
            {
                if (panel.Parent != null)
                {
                    panel.Parent.Resize += (s2, e2) => UpdateBenefitsContainerSize();
                    UpdateBenefitsContainerSize();
                }
            };
        }

        private void OpenChiTietHang(string hangCode, string title, Color bgColor1, Color bgColor2,
            Color textColor, string icon, string[] benefits)
        {
            try
            {
                using (var frm = new Frm_ChiTietHangThanhVien(hangCode, title, bgColor1, bgColor2, textColor, icon, benefits))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở form chi tiết hạng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void segmentedPill1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (segmentedPill1.SelectedIndex == 0)
            {
                // Hiển thị danh sách hóa đơn và thanh toán
                panelHDGD.Visible = false;
                panelCTTT.Visible = false;
                btnThem.Visible = true;
                cbbLocHang.Visible = true;
            }
            else if (segmentedPill1.SelectedIndex == 1)
            {
                panelHDGD.Visible = true;
                panelCTTT.Visible = false;
                cbbLocHang.Visible = false;
                btnThem.Visible = false;
                panelHDGD.Location = new Point(10, 305);
                // Load lại khi chuyển sang tab này
                if (panelHDGD.Controls.Count == 0)
                {
                    LoadHoaDonGiaoDich();
                }
            }
            else if (segmentedPill1.SelectedIndex == 2)
            {
                panelCTTT.Visible = true;
                panelHDGD.Visible = false;
                cbbLocHang.Visible = false;
                btnThem.Visible= false;
                panelCTTT.Location = new Point(10, 305);
                // Load lại khi chuyển sang tab này
                if (panelCTTT.Controls.Count == 0)
                {
                    LoadChiTietHangThanhVien();
                }
            }
        }

        private void LoadHoaDonGiaoDich()
        {
            try
            {
                panelHDGD.Controls.Clear();
                panelHDGD.AutoScroll = true;
                panelHDGD.Padding = new Padding(20);

                // Lấy chi_nhanh_id từ Session
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    Label lblNoData = new Label
                    {
                        Text = "Vui lòng chọn chi nhánh để xem đặt sảnh",
                        Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                        ForeColor = Color.FromArgb(150, 150, 150),
                        Location = new Point(15, 15),
                        AutoSize = true
                    };
                    panelHDGD.Controls.Add(lblNoData);
                    return;
                }

                // Load dữ liệu từ dat_sanh
                DataTable dt = _datSanhBLL.LayDanhSachDatSanhTheoChiNhanh(chiNhanhId, 100);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int yPos = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        string tenKhachHang = row["ten_khach_hang"]?.ToString() ?? "";
                        string tenSanh = row["ten_sanh"]?.ToString() ?? "";
                        decimal giaGoiTiec = row["gia_goi_tiec"] == DBNull.Value ? 0 : Convert.ToDecimal(row["gia_goi_tiec"]);
                        DateTime ngayToChuc = row["ngay_to_chuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["ngay_to_chuc"]);
                        string trangThai = row["trang_thai"]?.ToString() ?? "";

                        // Tạo panel cho mỗi đặt sảnh
                        Panel itemPanel = new Panel
                        {
                            Location = new Point(0, yPos),
                            Size = new Size(panelHDGD.Width - 40, 130),
                            BackColor = Color.White,
                            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                        };

                        // Tên khách hàng
                        Label lblTenKH = new Label
                        {
                            Text = tenKhachHang,
                            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                            ForeColor = Color.FromArgb(31, 41, 55),
                            Location = new Point(20, 18),
                            AutoSize = true,
                            BackColor = Color.Transparent
                        };
                        itemPanel.Controls.Add(lblTenKH);

                        // Tên sảnh
                        Label lblTenSanh = new Label
                        {
                            Text = $"Sảnh: {tenSanh}",
                            Font = new Font("Segoe UI", 13F, FontStyle.Regular),
                            ForeColor = Color.FromArgb(107, 114, 128),
                            Location = new Point(20, 48),
                            AutoSize = true,
                            BackColor = Color.Transparent
                        };
                        itemPanel.Controls.Add(lblTenSanh);

                        // Giá gói tiệc
                        Label lblGiaGoi = new Label
                        {
                            Text = FormatMoneyWithoutPrefix(giaGoiTiec),
                            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                            ForeColor = Color.FromArgb(34, 197, 94),
                            Location = new Point(20, 74),
                            AutoSize = true,
                            BackColor = Color.Transparent
                        };
                        itemPanel.Controls.Add(lblGiaGoi);

                        // Ngày tổ chức
                        Label lblNgay = new Label
                        {
                            Text = $"Ngày: {ngayToChuc:dd/M/yyyy}",
                            Font = new Font("Segoe UI", 12.5F, FontStyle.Regular),
                            ForeColor = Color.FromArgb(120, 120, 120),
                            Location = new Point(20, 98),
                            AutoSize = true,
                            BackColor = Color.Transparent
                        };
                        itemPanel.Controls.Add(lblNgay);

                        // Trạng thái (bên phải)
                        Label lblTrangThai = new Label
                        {
                            Text = trangThai,
                            Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                            ForeColor = GetTrangThaiColor(trangThai),
                            Location = new Point(itemPanel.Width - 200, 24),
                            AutoSize = true,
                            BackColor = Color.Transparent,
                            Anchor = AnchorStyles.Top | AnchorStyles.Right
                        };
                        itemPanel.Controls.Add(lblTrangThai);

                        // Đường phân cách
                        Panel separator = new Panel
                        {
                            Location = new Point(0, itemPanel.Height - 1),
                            Size = new Size(itemPanel.Width, 1),
                            BackColor = Color.FromArgb(230, 230, 230),
                            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
                        };
                        itemPanel.Controls.Add(separator);

                        panelHDGD.Controls.Add(itemPanel);
                        yPos += 130;
                    }
                }
                else
                {
                    // Hiển thị thông báo không có dữ liệu
                    Label lblNoData = new Label
                    {
                        Text = "Không có dữ liệu đặt sảnh",
                        Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                        ForeColor = Color.FromArgb(150, 150, 150),
                        Location = new Point(15, 15),
                        AutoSize = true
                    };
                    panelHDGD.Controls.Add(lblNoData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load đặt sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Color GetTrangThaiColor(string trangThai)
        {
            switch (trangThai.ToUpper())
            {
                case "CHỜ XÁC NHẬN":
                    return Color.FromArgb(245, 158, 11); // Cam/Vàng đậm - đang chờ xác nhận
                case "ĐÃ XÁC NHẬN":
                    return Color.FromArgb(59, 130, 246); // Xanh dương - đã xác nhận
                case "ĐÃ CỌC":
                    return Color.FromArgb(99, 102, 241); // Tím/Xanh dương đậm - đã cọc
                case "ĐÃ THANH TOÁN":
                    return Color.FromArgb(16, 185, 129); // Xanh lá đậm - đã thanh toán
                case "HOÀN TẤT":
                    return Color.FromArgb(34, 197, 94); // Xanh lá sáng - hoàn tất
                case "ĐÃ HỦY":
                    return Color.FromArgb(239, 68, 68); // Đỏ - đã hủy
                default:
                    return Color.FromArgb(107, 114, 128); // Xám - mặc định
            }
        }

        private string FormatMoney(decimal amount)
        {
            string prefix = amount >= 0 ? "+" : "";
            return $"{prefix}{string.Format("{0:#,0}", amount).Replace(",", ".")} ₫";
        }

        private string FormatMoneyWithoutPrefix(decimal amount)
        {
            return $"{string.Format("{0:#,0}", amount).Replace(",", ".")} ₫";
        }
    }
}


