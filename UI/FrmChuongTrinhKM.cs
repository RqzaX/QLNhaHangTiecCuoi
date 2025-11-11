using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BLL;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmChuongTrinhKM : Form
    {
        private ChuongTrinhKMBLL _bll;
        private DataTable _allData; // Lưu dữ liệu gốc để filter

        public FrmChuongTrinhKM()
        {
            InitializeComponent();
            _bll = new ChuongTrinhKMBLL();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            // Event tìm kiếm theo thời gian thực
            roundedTextBox1.TextChanged += TxtTimKiem_TextChanged;
        }
        private const string KM_TEN = "TenCT";
        private const string KM_MA = "MaKM";
        private const string KM_LOAI = "Loai";
        private const string KM_GIA = "GiaTri";
        private const string KM_AP_DUNG = "LoaiApDung";
        private const string KM_TG = "ThoiGian";
        private const string KM_TT = "TrangThai";

        private void FrmVoucher_Load(object sender, EventArgs e)
        {
            InitDgvKhuyenMai();
            LoadDataKhuyenMai();
        }
        private void InitDgvKhuyenMai()
        {
            var dgv = dgvKhuyenMai;

            dgv.DataSource = null;
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Thêm cột ID ẩn để lấy ID khi double click
            var colId = new DataGridViewTextBoxColumn { Name = "ID", HeaderText = "ID", Visible = false };
            dgv.Columns.Add(colId);

            // Thêm cột Tên chương trình - đảm bảo hiển thị rõ ràng
            var colTen = new DataGridViewTextBoxColumn
            {
                Name = KM_TEN,
                HeaderText = "Tên chương trình",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 300,
                Visible = true,
                MinimumWidth = 200
            };
            dgv.Columns.Add(colTen);

            var colMa = new DataGridViewTextBoxColumn { Name = KM_MA, HeaderText = "Mã KM", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Visible = true };
            dgv.Columns.Add(colMa);

            var colLoai = new DataGridViewTextBoxColumn { Name = KM_LOAI, HeaderText = "Loại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Visible = true };
            dgv.Columns.Add(colLoai);

            var colGia = new DataGridViewTextBoxColumn { Name = KM_GIA, HeaderText = "Giá trị", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Visible = true };
            dgv.Columns.Add(colGia);

            var colApDung = new DataGridViewTextBoxColumn { Name = KM_AP_DUNG, HeaderText = "Loại áp dụng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Visible = true };
            dgv.Columns.Add(colApDung);

            var colTG = new DataGridViewTextBoxColumn { Name = KM_TG, HeaderText = "Thời gian", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Visible = true };
            dgv.Columns.Add(colTG);

            var colTT = new DataGridViewTextBoxColumn { Name = KM_TT, HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, Visible = true };
            dgv.Columns.Add(colTT);

            // Đảm bảo DisplayIndex đúng thứ tự: ID (ẩn), Tên chương trình, Mã KM, Loại, Giá trị, Loại áp dụng, Thời gian, Trạng thái
            colId.DisplayIndex = 0;
            colTen.DisplayIndex = 1;
            colMa.DisplayIndex = 2;
            colLoai.DisplayIndex = 3;
            colGia.DisplayIndex = 4;
            colApDung.DisplayIndex = 5;
            colTG.DisplayIndex = 6;
            colTT.DisplayIndex = 7;

            // style tổng
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgv.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.RowTemplate.Height = 56;


        }

        private void LoadDataKhuyenMai()
        {
            try
            {
                dgvKhuyenMai.Rows.Clear();

                // Lưu dữ liệu gốc để filter
                _allData = _bll.LoadData();

                if (_allData != null && _allData.Rows.Count > 0)
                {
                    foreach (DataRow row in _allData.Rows)
                    {
                        ProcessAndAddRow(row);
                    }
                }

                // Cập nhật số liệu cho các panel
                UpdatePanelStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePanelStatistics()
        {
            try
            {
                if (_allData == null || _allData.Rows.Count == 0)
                {
                    label7.Text = "0"; // Tất cả Khuyến Mãi
                    label4.Text = "0"; // Đang Sử Dụng
                    label9.Text = "0"; // Đã Hết Hạn
                    return;
                }

                DateTime now = DateTime.Now.Date;
                int totalKM = _allData.Rows.Count;
                int dangSuDung = 0;
                int daHetHan = 0;

                foreach (DataRow row in _allData.Rows)
                {
                    DateTime tgBatDau = row["TgBatDau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgBatDau"]).Date;
                    DateTime tgKetThuc = row["TgKetThuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgKetThuc"]).Date;

                    // Đang sử dụng: trong thời gian hiệu lực
                    if (tgBatDau <= now && tgKetThuc >= now)
                    {
                        dangSuDung++;
                    }
                    // Đã hết hạn: đã qua ngày kết thúc
                    else if (tgKetThuc < now)
                    {
                        daHetHan++;
                    }
                }

                // Cập nhật label
                label7.Text = totalKM.ToString(); // Tất cả Khuyến Mãi
                label4.Text = dangSuDung.ToString(); // Đang Sử Dụng
                label9.Text = daHetHan.ToString(); // Đã Hết Hạn
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật thống kê: {ex.Message}");
            }
        }

        private void ProcessAndAddRow(DataRow row)
        {
            int kmId = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"]);
            string ten = row["TenCT"]?.ToString() ?? "";
            string ma = row["MaKM"]?.ToString() ?? "";
            string hinhThuc = row["HinhThuc"]?.ToString() ?? "";
            decimal giaTri = row["GiaTri"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaTri"]);
            DateTime tgBatDau = row["TgBatDau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgBatDau"]);
            DateTime tgKetThuc = row["TgKetThuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgKetThuc"]);

            // Format loại
            string loai = hinhThuc == "PERCENT" ? "Giảm %" :
                         hinhThuc == "AMOUNT" ? "Giảm tiền" :
                         hinhThuc == "GIFT" ? "Tặng quà" : "Khác";

            // Format giá trị
            string giatri = hinhThuc == "PERCENT" ? $"{giaTri:0}%" :
                           hinhThuc == "AMOUNT" ? Money(giaTri) : "-";

            // Format loại áp dụng
            string apDungLoai = row["ApDungLoai"]?.ToString() ?? "ALL";
            string loaiApDung = apDungLoai == "ALL" ? "Tất cả" :
                               apDungLoai == "NHAHANG" ? "Nhà hàng" :
                               apDungLoai == "TIECCUOI" ? "Tiệc cưới" : apDungLoai;

            // Format thời gian
            string thoigian = $"{tgBatDau:dd/M/yyyy} - {tgKetThuc:dd/M/yyyy}";

            // Trạng thái
            DateTime now = DateTime.Now;
            string trangthai = (now >= tgBatDau && now <= tgKetThuc) ? "Đang áp dụng" : "Đã hết hạn";

            AddKM(kmId, ten, ma, loai, giatri, loaiApDung, thoigian, trangthai);
        }

        private void AddKM(int kmId, string ten, string ma, string loai, string giatri,
                           string loaiApDung, string thoigian, string trangthai)
        {
            int r = dgvKhuyenMai.Rows.Add();
            var row = dgvKhuyenMai.Rows[r];
            row.Cells["ID"].Value = kmId;
            row.Cells[KM_TEN].Value = ten;
            row.Cells[KM_MA].Value = ma;
            row.Cells[KM_LOAI].Value = loai;
            row.Cells[KM_GIA].Value = giatri;
            row.Cells[KM_AP_DUNG].Value = loaiApDung;
            row.Cells[KM_TG].Value = thoigian;
            row.Cells[KM_TT].Value = trangthai;
        }

        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");

        // =============== CUSTOM DRAW ===============
        private static System.Drawing.Drawing2D.GraphicsPath Round(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure(); return p;
        }

        private void dgvKhuyenMai_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            string col = dgv.Columns[e.ColumnIndex].Name;

            // Mã KM -> chip xám nhạt
            if (col == KM_MA)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string text = Convert.ToString(e.FormattedValue) ?? "";
                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2,
                                         Math.Max(110, TextRenderer.MeasureText(text, new Font("Segoe UI Semibold", 9f)).Width + 22),
                                         28);
                using var path = Round(chip, 14);
                using var fill = new SolidBrush(Color.FromArgb(243, 244, 246));
                using var br = new SolidBrush(Color.FromArgb(55, 65, 81));
                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Trạng thái -> chip xanh/ xám
            if (col == KM_TT)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string text = Convert.ToString(e.FormattedValue) ?? "";
                bool active = text.Equals("Đang áp dụng", StringComparison.OrdinalIgnoreCase);

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2, 120, 28);
                using var path = Round(chip, 14);
                using var fill = new SolidBrush(active ? Color.FromArgb(209, 250, 229) : Color.FromArgb(243, 244, 246));
                using var br = new SolidBrush(active ? Color.FromArgb(16, 128, 67) : Color.FromArgb(55, 65, 81));
                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_TaoCTKM())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadDataKhuyenMai(); // Reload dữ liệu sau khi thêm mới
                }
            }
        }

        private void dgvKhuyenMai_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dgvKhuyenMai.Rows[e.RowIndex];
                object idValue = row.Cells["ID"].Value;

                if (idValue == null || idValue == DBNull.Value)
                {
                    MessageBox.Show("Không tìm thấy ID chương trình khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int kmId = Convert.ToInt32(idValue);

                using (var f = new Frm_ChiTietKM(kmId, this))
                {
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở chi tiết: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ReloadData()
        {
            LoadDataKhuyenMai();
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = roundedTextBox1.Text?.Trim() ?? "";

                // Nếu không có dữ liệu gốc, load lại
                if (_allData == null || _allData.Rows.Count == 0)
                {
                    LoadDataKhuyenMai();
                    return;
                }

                // Xóa dữ liệu hiện tại
                dgvKhuyenMai.Rows.Clear();

                // Nếu search text rỗng, hiển thị tất cả
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (DataRow row in _allData.Rows)
                    {
                        ProcessAndAddRow(row);
                    }
                    // Cập nhật lại thống kê từ toàn bộ dữ liệu
                    UpdatePanelStatistics();
                    return;
                }

                // Chuẩn hóa text tìm kiếm (loại bỏ dấu, chuyển thành chữ thường)
                string normalizedSearch = NormalizeText(searchText);

                // Filter dữ liệu
                foreach (DataRow row in _allData.Rows)
                {
                    // Lấy các giá trị để so sánh
                    string ten = row["TenCT"]?.ToString() ?? "";
                    string ma = row["MaKM"]?.ToString() ?? "";
                    string hinhThuc = row["HinhThuc"]?.ToString() ?? "";
                    decimal giaTri = row["GiaTri"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaTri"]);
                    DateTime tgBatDau = row["TgBatDau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgBatDau"]);
                    DateTime tgKetThuc = row["TgKetThuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgKetThuc"]);
                    string apDungLoai = row["ApDungLoai"]?.ToString() ?? "ALL";

                    // Format các giá trị để so sánh
                    string loai = hinhThuc == "PERCENT" ? "Giảm %" :
                                 hinhThuc == "AMOUNT" ? "Giảm tiền" :
                                 hinhThuc == "GIFT" ? "Tặng quà" : "Khác";

                    string giatri = hinhThuc == "PERCENT" ? $"{giaTri:0}%" :
                                   hinhThuc == "AMOUNT" ? Money(giaTri) : "-";

                    string loaiApDung = apDungLoai == "ALL" ? "Tất cả" :
                                       apDungLoai == "NHAHANG" ? "Nhà hàng" :
                                       apDungLoai == "TIECCUOI" ? "Tiệc cưới" : apDungLoai;

                    string thoigian = $"{tgBatDau:dd/M/yyyy} - {tgKetThuc:dd/M/yyyy}";

                    DateTime now = DateTime.Now;
                    string trangthai = (now >= tgBatDau && now <= tgKetThuc) ? "Đang áp dụng" : "Đã hết hạn";

                    // Kiểm tra nếu search text có trong bất kỳ cột nào
                    bool match = NormalizeText(ten).Contains(normalizedSearch) ||
                                NormalizeText(ma).Contains(normalizedSearch) ||
                                NormalizeText(loai).Contains(normalizedSearch) ||
                                NormalizeText(giatri).Contains(normalizedSearch) ||
                                NormalizeText(loaiApDung).Contains(normalizedSearch) ||
                                NormalizeText(thoigian).Contains(normalizedSearch) ||
                                NormalizeText(trangthai).Contains(normalizedSearch);

                    if (match)
                    {
                        int kmId = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"]);

                        AddKM(kmId, ten, ma, loai, giatri, loaiApDung, thoigian, trangthai);
                    }
                }

                // Cập nhật lại thống kê dựa trên kết quả tìm kiếm
                UpdatePanelStatisticsFromDgv();
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi khi search để tránh làm gián đoạn người dùng
                System.Diagnostics.Debug.WriteLine($"Lỗi khi tìm kiếm: {ex.Message}");
            }
        }

        private void UpdatePanelStatisticsFromDgv()
        {
            try
            {
                if (dgvKhuyenMai.Rows.Count == 0)
                {
                    label7.Text = "0";
                    label4.Text = "0";
                    label9.Text = "0";
                    return;
                }

                DateTime now = DateTime.Now.Date;
                int totalKM = dgvKhuyenMai.Rows.Count;
                int dangSuDung = 0;
                int daHetHan = 0;

                foreach (DataGridViewRow dgvRow in dgvKhuyenMai.Rows)
                {
                    if (dgvRow.IsNewRow) continue;

                    string trangThai = dgvRow.Cells[KM_TT].Value?.ToString() ?? "";

                    if (trangThai == "Đang áp dụng")
                    {
                        dangSuDung++;
                    }
                    else if (trangThai == "Đã hết hạn")
                    {
                        daHetHan++;
                    }
                }

                // Cập nhật label (chỉ hiển thị số lượng từ kết quả tìm kiếm)
                label7.Text = totalKM.ToString();
                label4.Text = dangSuDung.ToString();
                label9.Text = daHetHan.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật thống kê từ DGV: {ex.Message}");
            }
        }

        // Hàm chuẩn hóa text để tìm kiếm (loại bỏ dấu, chuyển thành chữ thường)
        private string NormalizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            // Chuyển thành chữ thường
            text = text.ToLower();

            // Loại bỏ dấu tiếng Việt
            string normalized = text.Normalize(NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void btnVoucher_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_Voucher())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }
    }
}
