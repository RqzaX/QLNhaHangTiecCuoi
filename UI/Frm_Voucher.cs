using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace UI
{
    public partial class Frm_Voucher : Form
    {
        private VoucherBLL _bll;
        private DataTable _allData;

        private const string VC_CODE = "Code";
        private const string VC_KH = "KhachHang";
        private const string VC_GIA = "GiaTri";
        private const string VC_DON_MIN = "DonToiThieu";
        private const string VC_NGAY_PH = "NgayPhatHanh";
        private const string VC_NGAY_HH = "NgayHetHan";
        private const string VC_SO_LAN = "SoLan";
        private const string VC_DA_DUNG = "DaDung";
        private const string VC_TT = "TrangThai";

        public Frm_Voucher()
        {
            InitializeComponent();
            _bll = new VoucherBLL();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            this.Load += Frm_Voucher_Load;
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
            dgvVoucher.CellPainting += DgvVoucher_CellPainting;
            cbbLoc.SelectedIndexChanged += CbbLoc_SelectedIndexChanged;
            dgvVoucher.CellDoubleClick += DgvVoucher_CellDoubleClick;
            btnThemVoucher.Click += BtnThemVoucher_Click;
        }

        private void BtnThemVoucher_Click(object sender, EventArgs e)
        {
            using (var frm = new Frm_ThemVoucher())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadDataVoucher();
                }
            }
        }

        private void Frm_Voucher_Load(object sender, EventArgs e)
        {
            InitDgvVoucher();
            InitCbbLoc();
            LoadDataVoucher();
        }

        private void InitCbbLoc()
        {
            cbbLoc.Items.Clear();
            cbbLoc.Items.Add("Tất cả"); // Option để hiển thị tất cả
            cbbLoc.Items.Add("Đang áp dụng");
            cbbLoc.Items.Add("Đã hết hạn");
            cbbLoc.SelectedIndex = 0; // Mặc định chọn "Tất cả"
        }

        private void InitDgvVoucher()
        {
            var dgv = dgvVoucher;

            dgv.DataSource = null;
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Thêm cột ID ẩn
            var colId = new DataGridViewTextBoxColumn { Name = "ID", HeaderText = "ID", Visible = false };
            dgv.Columns.Add(colId);

            // Các cột hiển thị với HeaderText rõ ràng
            var colCode = new DataGridViewTextBoxColumn
            {
                Name = VC_CODE,
                HeaderText = "voucher",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = true,
                MinimumWidth = 150
            };
            dgv.Columns.Add(colCode);

            var colGia = new DataGridViewTextBoxColumn
            {
                Name = VC_GIA,
                HeaderText = "Giá trị",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = true,
                MinimumWidth = 120
            };
            dgv.Columns.Add(colGia);

            var colDonMin = new DataGridViewTextBoxColumn
            {
                Name = VC_DON_MIN,
                HeaderText = "Đơn tối thiểu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = true,
                MinimumWidth = 150
            };
            dgv.Columns.Add(colDonMin);

            var colNgayPH = new DataGridViewTextBoxColumn
            {
                Name = VC_NGAY_PH,
                HeaderText = "Ngày phát hành",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = true,
                MinimumWidth = 130
            };
            dgv.Columns.Add(colNgayPH);

            var colNgayHH = new DataGridViewTextBoxColumn
            {
                Name = VC_NGAY_HH,
                HeaderText = "Ngày hết hạn",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = true,
                MinimumWidth = 130
            };
            dgv.Columns.Add(colNgayHH);

            var colSoLan = new DataGridViewTextBoxColumn
            {
                Name = VC_SO_LAN,
                HeaderText = "Lượt dùng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = true,
                MinimumWidth = 100
            };
            dgv.Columns.Add(colSoLan);

            var colDaDung = new DataGridViewTextBoxColumn
            {
                Name = VC_DA_DUNG,
                HeaderText = "Đã dùng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = true,
                MinimumWidth = 100
            };
            dgv.Columns.Add(colDaDung);

            var colTT = new DataGridViewTextBoxColumn
            {
                Name = VC_TT,
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                Visible = true,
                MinimumWidth = 140
            };
            dgv.Columns.Add(colTT);

            // Đảm bảo DisplayIndex đúng thứ tự
            colId.DisplayIndex = 0;
            colCode.DisplayIndex = 1;
            colGia.DisplayIndex = 2;
            colDonMin.DisplayIndex = 3;
            colNgayPH.DisplayIndex = 4;
            colNgayHH.DisplayIndex = 5;
            colSoLan.DisplayIndex = 6;
            colDaDung.DisplayIndex = 7;
            colTT.DisplayIndex = 8;

            // Style
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.RowTemplate.Height = 56;
            dgv.ColumnHeadersHeight = 50;
        }

        private void LoadDataVoucher()
        {
            try
            {
                _allData = _bll.LoadData();

                // Áp dụng filter và search sau khi load dữ liệu
                ApplyFilterAndSearch();
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
                    label5.Text = "0"; // Tất cả Voucher
                    label6.Text = "0"; // Đang áp dụng
                    label7.Text = "0"; // Đã hết hạn
                    return;
                }

                DateTime now = DateTime.Now.Date;
                int totalVouchers = _allData.Rows.Count;
                int dangApDung = 0;
                int daHetHan = 0;

                foreach (DataRow row in _allData.Rows)
                {
                    DateTime tgBatDau = row["TgBatDau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgBatDau"]).Date;
                    DateTime tgKetThuc = row["TgKetThuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgKetThuc"]).Date;

                    // Tính trạng thái dựa trên CTKM (tg_bat_dau và tg_ket_thuc)
                    if (tgKetThuc < now)
                    {
                        daHetHan++;
                    }
                    else if (tgBatDau <= now && tgKetThuc >= now)
                    {
                        dangApDung++;
                    }
                    else
                    {
                        daHetHan++;
                    }
                }

                // Cập nhật label
                label5.Text = totalVouchers.ToString(); // Tất cả Voucher
                label6.Text = dangApDung.ToString(); // Đang áp dụng
                label7.Text = daHetHan.ToString(); // Đã hết hạn
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật thống kê: {ex.Message}");
            }
        }

        private void ProcessAndAddRow(DataRow row)
        {
            int voucherId = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"]);
            string code = row["Code"]?.ToString() ?? "";
            string khachHang = row["KhachHang"]?.ToString() ?? "Chưa sử dụng";
            decimal giaTri = row["GiaTri"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaTri"]);
            string hinhThuc = row["HinhThuc"]?.ToString() ?? "";
            decimal donToiThieu = row["DonToiThieu"] == DBNull.Value ? 0 : Convert.ToDecimal(row["DonToiThieu"]);

            DateTime ngayPhatHanh = row["NgayPhatHanh"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["NgayPhatHanh"]);
            DateTime ngayHetHan = row["NgayHetHan"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["NgayHetHan"]);
            DateTime tgBatDau = row["TgBatDau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgBatDau"]);
            DateTime tgKetThuc = row["TgKetThuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgKetThuc"]);

            int soLan = row["SoLan"] == DBNull.Value ? 0 : Convert.ToInt32(row["SoLan"]);
            int daDung = row["DaDung"] == DBNull.Value ? 0 : Convert.ToInt32(row["DaDung"]);

            // Format giá trị dựa trên hình thức
            string giaTriStr = "";
            if (hinhThuc == "PERCENT")
            {
                giaTriStr = $"{giaTri:0}%";
            }
            else if (hinhThuc == "AMOUNT")
            {
                giaTriStr = Money(giaTri);
            }
            else if (hinhThuc == "GIFT")
            {
                giaTriStr = "-";
            }
            else
            {
                giaTriStr = Money(giaTri);
            }

            // Format đơn tối thiểu (mặc định 5 triệu nếu chưa có)
            if (donToiThieu == 0)
            {
                donToiThieu = giaTri * 10; // Đơn tối thiểu = 10 lần giá trị voucher
                if (donToiThieu < 1000000) donToiThieu = 1000000; // Tối thiểu 1 triệu
            }
            string donToiThieuStr = Money(donToiThieu);

            // Format ngày
            string ngayPHStr = ngayPhatHanh.ToString("dd/M/yyyy");
            string ngayHHStr = ngayHetHan.ToString("dd/M/yyyy");

            // Tính trạng thái dựa trên CTKM (tg_bat_dau và tg_ket_thuc)
            DateTime now = DateTime.Now.Date;
            string trangThai = "";

            if (tgKetThuc < now)
            {
                trangThai = "Đã hết hạn";
            }
            else if (tgBatDau <= now && tgKetThuc >= now)
            {
                trangThai = "Đang áp dụng";
            }
            else
            {
                trangThai = "Đã hết hạn";
            }

            AddVoucher(voucherId, code, khachHang, giaTriStr, donToiThieuStr, ngayPHStr, ngayHHStr, soLan, daDung, trangThai);
        }

        private void AddVoucher(int voucherId, string code, string khachHang, string giaTri,
                                string donToiThieu, string ngayPH, string ngayHH, int soLan, int daDung, string trangThai)
        {
            int r = dgvVoucher.Rows.Add();
            var row = dgvVoucher.Rows[r];
            row.Cells["ID"].Value = voucherId;
            row.Cells[VC_CODE].Value = code;
            row.Cells[VC_GIA].Value = giaTri;
            row.Cells[VC_DON_MIN].Value = donToiThieu;
            row.Cells[VC_NGAY_PH].Value = ngayPH;
            row.Cells[VC_NGAY_HH].Value = ngayHH;
            row.Cells[VC_SO_LAN].Value = soLan;
            row.Cells[VC_DA_DUNG].Value = daDung;
            row.Cells[VC_TT].Value = trangThai;
        }

        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");

        private void DgvVoucher_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            string col = dgv.Columns[e.ColumnIndex].Name;

            // Trạng thái -> chip màu
            if (col == VC_TT)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string text = Convert.ToString(e.FormattedValue) ?? "";
                bool isDangApDung = text.Equals("Đang áp dụng", StringComparison.OrdinalIgnoreCase);
                bool isHetHan = text.Equals("Đã hết hạn", StringComparison.OrdinalIgnoreCase);

                Color bgColor;
                Color textColor;

                if (isDangApDung)
                {
                    bgColor = Color.FromArgb(209, 250, 229); // Xanh lá nhạt
                    textColor = Color.FromArgb(16, 128, 67); // Xanh lá đậm
                }
                else if (isHetHan)
                {
                    bgColor = Color.FromArgb(243, 244, 246); // Xám nhạt
                    textColor = Color.FromArgb(55, 65, 81); // Xám đậm
                }
                else
                {
                    bgColor = Color.FromArgb(243, 244, 246); // Xám nhạt
                    textColor = Color.FromArgb(55, 65, 81); // Xám đậm
                }

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2, 120, 28);
                using var path = Round(chip, 14);
                using var fill = new SolidBrush(bgColor);
                using var br = new SolidBrush(textColor);
                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath Round(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            ApplyFilterAndSearch();
        }

        private void ApplyFilterAndSearch()
        {
            try
            {
                if (_allData == null || _allData.Rows.Count == 0)
                {
                    if (dgvVoucher.Rows.Count == 0)
                    {
                        LoadDataVoucher();
                    }
                    return;
                }

                string searchText = txtTimKiem.Text?.Trim() ?? "";
                string selectedStatus = cbbLoc.SelectedItem?.ToString() ?? "Tất cả";

                dgvVoucher.Rows.Clear();

                string normalizedSearch = string.IsNullOrWhiteSpace(searchText) ? "" : NormalizeText(searchText);

                foreach (DataRow row in _allData.Rows)
                {
                    // Kiểm tra tìm kiếm
                    bool matchSearch = true;
                    if (!string.IsNullOrWhiteSpace(normalizedSearch))
                    {
                        string code = row["Code"]?.ToString() ?? "";
                        matchSearch = NormalizeText(code).Contains(normalizedSearch);
                    }

                    if (!matchSearch)
                        continue;

                    // Kiểm tra lọc theo trạng thái
                    bool matchStatus = true;
                    if (selectedStatus != "Tất cả")
                    {
                        // Tính trạng thái của voucher
                        DateTime ngayPhatHanh = row["NgayPhatHanh"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["NgayPhatHanh"]).Date;
                        DateTime ngayHetHan = row["NgayHetHan"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["NgayHetHan"]).Date;
                        int soLan = row["SoLan"] == DBNull.Value ? 0 : Convert.ToInt32(row["SoLan"]);
                        int daDung = row["DaDung"] == DBNull.Value ? 0 : Convert.ToInt32(row["DaDung"]);

                        DateTime now = DateTime.Now.Date;
                        string trangThai = "";

                        if (ngayHetHan < now)
                        {
                            trangThai = "Đã hết hạn";
                        }
                        else if (ngayPhatHanh <= now && ngayHetHan >= now)
                        {
                            trangThai = "Đang áp dụng";
                        }
                        else
                        {
                            trangThai = "Đã hết hạn";
                        }

                        matchStatus = trangThai == selectedStatus;
                    }

                    if (matchSearch && matchStatus)
                    {
                        ProcessAndAddRow(row);
                    }
                }

                // Cập nhật thống kê
                if (string.IsNullOrWhiteSpace(searchText) && selectedStatus == "Tất cả")
                {
                    UpdatePanelStatistics();
                }
                else
                {
                    UpdatePanelStatisticsFromDgv();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi áp dụng lọc và tìm kiếm: {ex.Message}");
            }
        }

        private void UpdatePanelStatisticsFromDgv()
        {
            try
            {
                if (dgvVoucher.Rows.Count == 0)
                {
                    label5.Text = "0";
                    label6.Text = "0";
                    label7.Text = "0";
                    return;
                }

                int totalVouchers = dgvVoucher.Rows.Count;
                int dangApDung = 0;
                int daHetHan = 0;

                foreach (DataGridViewRow dgvRow in dgvVoucher.Rows)
                {
                    if (dgvRow.IsNewRow) continue;

                    string trangThai = dgvRow.Cells[VC_TT].Value?.ToString() ?? "";

                    if (trangThai == "Đang áp dụng")
                    {
                        dangApDung++;
                    }
                    else if (trangThai == "Đã hết hạn")
                    {
                        daHetHan++;
                    }
                }

                // Cập nhật label (chỉ hiển thị số lượng từ kết quả tìm kiếm)
                label5.Text = totalVouchers.ToString();
                label6.Text = dangApDung.ToString();
                label7.Text = daHetHan.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật thống kê từ DGV: {ex.Message}");
            }
        }

        private string NormalizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            text = text.ToLower();
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

        private void BtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkHanDung_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cboKhuyenMai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CbbLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilterAndSearch();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void DgvVoucher_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var row = dgvVoucher.Rows[e.RowIndex];
                if (row.Cells["ID"].Value == null) return;

                int voucherId = Convert.ToInt32(row.Cells["ID"].Value);
                
                using (var frm = new Frm_ChiTietVoucher(voucherId, this))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadDataVoucher();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở chi tiết voucher: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ReloadData()
        {
            LoadDataVoucher();
        }
    }
}
