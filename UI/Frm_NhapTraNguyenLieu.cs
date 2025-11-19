using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Common;
using UI.Controls;
using Guna.UI2.WinForms;
using System.ComponentModel.Design;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_NhapTraNguyenLieu : Form
    {
        private DatabaseHelper _dbHelper;
        private NguyenLieuBLL _nguyenLieuBLL;
        private int _chiNhanhId;

        public Frm_NhapTraNguyenLieu()
        {
            InitializeComponent();
            
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
            
            _dbHelper = new DatabaseHelper();
            _nguyenLieuBLL = new NguyenLieuBLL(_dbHelper);
            _chiNhanhId = Session.ChiNhanhId > 0 ? Session.ChiNhanhId : 1;

            this.Load += Frm_NhapTraNguyenLieu_Load;
            segmentedPill1.SelectedIndexChanged += SegmentedPill1_SelectedIndexChanged;
            
            // Tắt separator/dấu gạch ngang giữa các tab
            try
            {
                var showSeparatorProp = segmentedPill1.GetType().GetProperty("ShowSeparator");
                if (showSeparatorProp != null)
                {
                    showSeparatorProp.SetValue(segmentedPill1, false);
                }
            }
            catch { }
            btnThemDongNhap.Click += btnThemDongNhap_Click;
            btnThemDongTra.Click += btnThemDongTra_Click;
            btnLuuPhieuNhap.Click += btnLuuPhieuNhap_Click;
            btnLuuPhieuTra.Click += btnLuuPhieuTra_Click;
            dgvChiTietNhap.CellClick += dgvChiTietNhap_CellClick;
            dgvChiTietNhap.CellBeginEdit += dgvChiTietNhap_CellBeginEdit;
            dgvChiTietNhap.CellEndEdit += dgvChiTietNhap_CellEndEdit;
            dgvChiTietNhap.CellValueChanged += dgvChiTietNhap_CellValueChanged;
            dgvChiTietNhap.DataError += dgvChiTietNhap_DataError;
            dgvChiTietNhap.EditingControlShowing += dgvChiTietNhap_EditingControlShowing;
            dgvChiTietNhap.CellValidating += dgvChiTietNhap_CellValidating;
            dgvChiTietTra.CellClick += dgvChiTietTra_CellClick;
            dgvChiTietTra.CellBeginEdit += dgvChiTietTra_CellBeginEdit;
            dgvChiTietTra.CellEndEdit += dgvChiTietTra_CellEndEdit;
            dgvChiTietTra.CellValueChanged += dgvChiTietTra_CellValueChanged;
            dgvChiTietTra.DataError += dgvChiTietTra_DataError;
            txtSearchTonKho.TextChanged += txtSearchTonKho_TextChanged;
            dgvTonKho.CellEndEdit += dgvTonKho_CellEndEdit;
            txtSearchLichSu.TextChanged += txtSearchLichSu_TextChanged;
            dgvLichSu.CellDoubleClick += dgvLichSu_CellDoubleClick;
            dgvLichSu.CellFormatting += dgvLichSu_CellFormatting;
            cmbLoaiPhieu.SelectedIndexChanged += cmbLoaiPhieu_SelectedIndexChanged;
            dtpTuNgay.ValueChanged += dtpTuNgay_ValueChanged;
            dtpDenNgay.ValueChanged += dtpDenNgay_ValueChanged;
        }

        private void Frm_NhapTraNguyenLieu_Load(object sender, EventArgs e)
        {
            InitializeForm();
            // Hiển thị tab đầu tiên (Nhập kho)
            segmentedPill1.SelectedIndex = 0;
            panelNhapKho.Visible = true;
            panelTraKho.Visible = false;
            panelTonKho.Visible = false;
            panelLichSu.Visible = false;
            
            LoadNguyenLieu();
            
            cmbLoaiPhieu.SelectedIndex = 0;
            
            // Refresh lại để đảm bảo combobox hiển thị dữ liệu
            this.BeginInvoke(new Action(() =>
            {
                PopulateComboBoxColumns();
            }));
            SetupSegmentedPill();
        }

        private void InitializeForm()
        {
            dtpNgayNhap.Value = DateTime.Now;
            dtpGioNhap.Value = DateTime.Now;
            dtpNgayTra.Value = DateTime.Now;
            dtpGioTra.Value = DateTime.Now;

            SetupDataGridViewNhap();
            SetupDataGridViewTra();
            SetupDataGridViewTonKho();
        }

        private void SetupSegmentedPill()
        {
            try
            {
                var type = segmentedPill1.GetType();
                
                var showSeparatorProp = type.GetProperty("ShowSeparator");
                if (showSeparatorProp != null && showSeparatorProp.CanWrite)
                {
                    showSeparatorProp.SetValue(segmentedPill1, false);
                    return;
                }
                
                var separatorVisibleProp = type.GetProperty("SeparatorVisible");
                if (separatorVisibleProp != null && separatorVisibleProp.CanWrite)
                {
                    separatorVisibleProp.SetValue(segmentedPill1, false);
                    return;
                }
                
                var showDividerProp = type.GetProperty("ShowDivider");
                if (showDividerProp != null && showDividerProp.CanWrite)
                {
                    showDividerProp.SetValue(segmentedPill1, false);
                    return;
                }
                
                var itemSeparatorProp = type.GetProperty("ItemSeparator");
                if (itemSeparatorProp != null && itemSeparatorProp.CanWrite)
                {
                    itemSeparatorProp.SetValue(segmentedPill1, false);
                    return;
                }
            }
            catch
            {
            }
        }

        private void SetupDataGridViewNhap()
        {
            dgvChiTietNhap.Rows.Clear();
            AddRowNhap();
        }

        private void SetupDataGridViewTra()
        {
            dgvChiTietTra.Rows.Clear();
            AddRowTra();
        }

        private void SetupDataGridViewTonKho()
        {
            dgvTonKho.Rows.Clear();
        }

        private DataTable _nguyenLieuData;
        private Dictionary<int, string> _nguyenLieuDict = new Dictionary<int, string>();

        private void LoadNguyenLieu()
        {
            try
            {
                _nguyenLieuData = _nguyenLieuBLL.LayDanhMuc();
                if (_nguyenLieuData != null && _nguyenLieuData.Rows.Count > 0)
                {
                    _nguyenLieuDict.Clear();
                    foreach (DataRow row in _nguyenLieuData.Rows)
                    {
                        int nlId = Convert.ToInt32(row["nl_id"]);
                        string tenNL = row["ten_nl"].ToString();
                        _nguyenLieuDict[nlId] = tenNL;
                    }

                    // Populate combobox columns
                    PopulateComboBoxColumns();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nguyên liệu trong database!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách nguyên liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateComboBoxColumns()
        {
            if (_nguyenLieuData == null || _nguyenLieuData.Rows.Count == 0) return;

            try
            {
                var nhapData = _nguyenLieuData.Copy();
                colNguyenLieu_Nhap.DataSource = null; // Clear trước
                colNguyenLieu_Nhap.DataSource = nhapData;
                colNguyenLieu_Nhap.DisplayMember = "ten_nl";
                colNguyenLieu_Nhap.ValueMember = "nl_id";
                colNguyenLieu_Nhap.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                colNguyenLieu_Nhap.FlatStyle = FlatStyle.Flat;

                var traData = _nguyenLieuData.Copy();
                colNguyenLieu_Tra.DataSource = null; // Clear trước
                colNguyenLieu_Tra.DataSource = traData;
                colNguyenLieu_Tra.DisplayMember = "ten_nl";
                colNguyenLieu_Tra.ValueMember = "nl_id";
                colNguyenLieu_Tra.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                colNguyenLieu_Tra.FlatStyle = FlatStyle.Flat;

                // Refresh DataGridViews để combobox hiển thị dữ liệu
                if (dgvChiTietNhap.Columns.Contains(colNguyenLieu_Nhap))
                {
                    dgvChiTietNhap.InvalidateColumn(colNguyenLieu_Nhap.Index);
                }
                if (dgvChiTietTra.Columns.Contains(colNguyenLieu_Tra))
                {
                    dgvChiTietTra.InvalidateColumn(colNguyenLieu_Tra.Index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi populate combobox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable _tonKhoData;

        private void LoadTonKho()
        {
            try
            {
                _tonKhoData = _nguyenLieuBLL.LayDanhSachTonKho(_chiNhanhId);
                
                // Load vào DataGridView
                dgvTonKho.Rows.Clear();
                if (_tonKhoData != null && _tonKhoData.Rows.Count > 0)
                {
                    foreach (DataRow row in _tonKhoData.Rows)
                    {
                        int rowIndex = dgvTonKho.Rows.Add();
                        var dgvRow = dgvTonKho.Rows[rowIndex];
                        dgvRow.Cells[colSTT_Ton.Index].Value = row["stt"];
                        dgvRow.Cells[colMaNL_Ton.Index].Value = row["ma_nl"];
                        dgvRow.Cells[colTenNL_Ton.Index].Value = row["ten_nl"];
                        dgvRow.Cells[colTonKho_Ton.Index].Value = FormatSoLuong(Convert.ToDecimal(row["sl_ton"]));
                        dgvRow.Cells[colTonToiThieu_Ton.Index].Value = FormatSoLuong(Convert.ToDecimal(row["ton_toi_thieu"]));
                        dgvRow.Cells[colTrangThai_Ton.Index].Value = row["trang_thai"];
                        // Lưu nl_id vào Tag để dùng khi cập nhật
                        dgvRow.Tag = row["nl_id"];
                    }
                }

                UpdateSummaryCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải tồn kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummaryCards()
        {
            try
            {
                if (_tonKhoData == null || _tonKhoData.Rows.Count == 0)
                {
                    labelTongNL.Text = "0";
                    labelDuTon.Text = "0";
                    labelTonThap.Text = "0";
                    return;
                }

                int tongNL = _tonKhoData.Rows.Count;
                int duTon = 0;
                int tonThap = 0;

                foreach (DataRow row in _tonKhoData.Rows)
                {
                    string trangThai = row["trang_thai"]?.ToString() ?? "";
                    if (trangThai == "Đủ tồn")
                    {
                        duTon++;
                    }
                    else if (trangThai == "Tồn thấp" || trangThai == "Hết hàng")
                    {
                        tonThap++;
                    }
                }

                labelTongNL.Text = tongNL.ToString();
                labelDuTon.Text = duTon.ToString();
                labelTonThap.Text = tonThap.ToString();
            }
            catch (Exception ex)
            {
                labelTongNL.Text = "0";
                labelDuTon.Text = "0";
                labelTonThap.Text = "0";
            }
        }

        private void AddRowNhap()
        {
            int stt = dgvChiTietNhap.Rows.Count + 1;
            int newRowIndex = dgvChiTietNhap.Rows.Add();
            var newRow = dgvChiTietNhap.Rows[newRowIndex];
            newRow.Cells[colSTT_Nhap.Index].Value = stt;
            newRow.Cells[colNguyenLieu_Nhap.Index].Value = DBNull.Value;
            newRow.Cells[colTon_Nhap.Index].Value = "";
            newRow.Cells[colSoLuong_Nhap.Index].Value = "";
            newRow.Cells[colDVT_Nhap.Index].Value = "";
            newRow.Cells[colGhiChu_Nhap.Index].Value = "";
        }

        private void AddRowTra()
        {
            int stt = dgvChiTietTra.Rows.Count + 1;
            int newRowIndex = dgvChiTietTra.Rows.Add();
            var newRow = dgvChiTietTra.Rows[newRowIndex];
            newRow.Cells[colSTT_Tra.Index].Value = stt;
            newRow.Cells[colNguyenLieu_Tra.Index].Value = DBNull.Value;
            newRow.Cells[colTon_Tra.Index].Value = FormatSoLuong(0);
            newRow.Cells[colSoLuongTra.Index].Value = "";
            newRow.Cells[colDVT_Tra.Index].Value = "";
            newRow.Cells[colConLai_Tra.Index].Value = FormatSoLuong(0);
            newRow.Cells[colGhiChu_Tra.Index].Value = "";
        }

        private void btnThemDongNhap_Click(object sender, EventArgs e)
        {
            AddRowNhap();
        }

        private void btnThemDongTra_Click(object sender, EventArgs e)
        {
            AddRowTra();
        }

        private void ResetFormNhap()
        {
            dtpNgayNhap.Value = DateTime.Now;
            dtpGioNhap.Value = DateTime.Now;
            txtNhanVienNhap.Text = "";
            if (txtGhiChuPhieuNhap != null)
            {
                txtGhiChuPhieuNhap.Text = "";
            }
            dgvChiTietNhap.Rows.Clear();
        }

        private void ResetFormTra()
        {
            dtpNgayTra.Value = DateTime.Now;
            dtpGioTra.Value = DateTime.Now;
            txtNhanVienTra.Text = "";
            if (txtGhiChuPhieuTra != null)
            {
                txtGhiChuPhieuTra.Text = "";
            }
            dgvChiTietTra.Rows.Clear();
        }

        private void btnLuuPhieuNhap_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidatePhieuNhap())
                {
                    // Save phiếu nhập kho
                    SavePhieuNhapKho();
                    MessageBox.Show("Lưu phiếu nhập kho thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetFormNhap();
                    LoadTonKho();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu phiếu nhập kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuuPhieuTra_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidatePhieuTra())
                {
                    // Save phiếu trả kho
                    SavePhieuTraKho();
                    MessageBox.Show("Lưu phiếu trả kho thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetFormTra();
                    LoadTonKho();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu phiếu trả kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidatePhieuNhap()
        {
            if (string.IsNullOrWhiteSpace(txtNhanVienNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên nhập kho!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNhanVienNhap.Focus();
                return false;
            }

            if (dgvChiTietNhap.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một nguyên liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (DataGridViewRow row in dgvChiTietNhap.Rows)
            {
                if (row.IsNewRow) continue;

                var nguyenLieu = row.Cells[1].Value?.ToString();
                var soLuong = row.Cells[2].Value?.ToString();

                if (string.IsNullOrWhiteSpace(nguyenLieu))
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu cho tất cả các dòng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(soLuong) || !TryParseSoLuong(soLuong, out _))
                {
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private bool ValidatePhieuTra()
        {
            if (string.IsNullOrWhiteSpace(txtNhanVienTra.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên trả kho!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNhanVienTra.Focus();
                return false;
            }

            if (dgvChiTietTra.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một nguyên liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (DataGridViewRow row in dgvChiTietTra.Rows)
            {
                if (row.IsNewRow) continue;

                var nguyenLieu = row.Cells[1].Value?.ToString();
                var soLuongTra = row.Cells[3].Value?.ToString();

                if (string.IsNullOrWhiteSpace(nguyenLieu))
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu cho tất cả các dòng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(soLuongTra) || !TryParseSoLuong(soLuongTra, out _))
                {
                    MessageBox.Show("Vui lòng nhập số lượng trả hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void SavePhieuNhapKho()
        {
            var chiTietList = new List<PhieuNhapKhoChiTiet>();

            foreach (DataGridViewRow row in dgvChiTietNhap.Rows)
            {
                if (row.IsNewRow) continue;

                var nlValue = row.Cells[colNguyenLieu_Nhap.Index].Value;
                var soLuongStr = row.Cells[colSoLuong_Nhap.Index].Value?.ToString();
                var dvt = row.Cells[colDVT_Nhap.Index].Value?.ToString() ?? "";
                var ghiChu = row.Cells[colGhiChu_Nhap.Index].Value?.ToString() ?? "";

                if (nlValue == null || nlValue == DBNull.Value) continue;
                if (string.IsNullOrWhiteSpace(soLuongStr) || !TryParseSoLuong(soLuongStr, out decimal soLuong)) continue;

                int nlId = Convert.ToInt32(nlValue);
                
                // Kiểm tra tồn kho trước khi lưu
                decimal tonHienTai = _nguyenLieuBLL.LayTonKhoTaiChiNhanh(_chiNhanhId, nlId);
                if (tonHienTai < soLuong)
                {
                    string tenNL = _nguyenLieuDict.ContainsKey(nlId) ? _nguyenLieuDict[nlId] : $"ID: {nlId}";
                    throw new Exception($"Nguyên liệu '{tenNL}' không đủ tồn kho!\n\n" +
                                      $"Số lượng yêu cầu: {FormatSoLuong(soLuong)}\n" +
                                      $"Tồn kho hiện tại: {FormatSoLuong(tonHienTai)}\n" +
                                      $"Thiếu: {FormatSoLuong(soLuong - tonHienTai)}");
                }
                
                chiTietList.Add(new PhieuNhapKhoChiTiet
                {
                    NlId = nlId,
                    SoLuong = soLuong,
                    DonVi = dvt,
                    GhiChu = ghiChu
                });
            }

            if (chiTietList.Count == 0)
            {
                throw new Exception("Vui lòng thêm ít nhất một nguyên liệu!");
            }

            DateTime ngayNhap = dtpNgayNhap.Value.Date;
            TimeSpan gioNhap = dtpGioNhap.Value.TimeOfDay;
            string nhanVienNhap = txtNhanVienNhap.Text.Trim();
            string ghiChuPhieu = txtGhiChuPhieuNhap != null ? txtGhiChuPhieuNhap.Text.Trim() : "";

            _nguyenLieuBLL.LuuPhieuNhapKho(_chiNhanhId, ngayNhap, gioNhap, nhanVienNhap, ghiChuPhieu, chiTietList);
        }

        private void SavePhieuTraKho()
        {
            var chiTietList = new List<PhieuTraKhoChiTiet>();

            foreach (DataGridViewRow row in dgvChiTietTra.Rows)
            {
                if (row.IsNewRow) continue;

                var nlValue = row.Cells[colNguyenLieu_Tra.Index].Value;
                var soLuongTraStr = row.Cells[colSoLuongTra.Index].Value?.ToString();
                var tonStr = row.Cells[colTon_Tra.Index].Value?.ToString();
                var conLaiStr = row.Cells[colConLai_Tra.Index].Value?.ToString();
                var dvt = row.Cells[colDVT_Tra.Index].Value?.ToString() ?? "";
                var ghiChu = row.Cells[colGhiChu_Tra.Index].Value?.ToString() ?? "";

                if (nlValue == null || nlValue == DBNull.Value) continue;
                if (string.IsNullOrWhiteSpace(soLuongTraStr) || !TryParseSoLuong(soLuongTraStr, out decimal soLuongTra)) continue;
                if (string.IsNullOrWhiteSpace(tonStr) || !TryParseSoLuong(tonStr, out decimal ton)) continue;
                if (string.IsNullOrWhiteSpace(conLaiStr) || !TryParseSoLuong(conLaiStr, out decimal conLai)) continue;

                int nlId = Convert.ToInt32(nlValue);
                
                // Kiểm tra số lượng trả không vượt quá tồn kho
                if (soLuongTra > ton)
                {
                    string tenNL = _nguyenLieuDict.ContainsKey(nlId) ? _nguyenLieuDict[nlId] : $"ID: {nlId}";
                    throw new Exception($"Nguyên liệu '{tenNL}' không đủ tồn kho để trả!\n\n" +
                                      $"Số lượng muốn trả: {FormatSoLuong(soLuongTra)}\n" +
                                      $"Tồn kho hiện tại: {FormatSoLuong(ton)}\n" +
                                      $"Thiếu: {FormatSoLuong(soLuongTra - ton)}");
                }
                chiTietList.Add(new PhieuTraKhoChiTiet
                {
                    NlId = nlId,
                    SoLuongTra = soLuongTra,
                    SoLuongTon = ton,
                    SoLuongConLai = conLai,
                    DonVi = dvt,
                    GhiChu = ghiChu
                });
            }

            if (chiTietList.Count == 0)
            {
                throw new Exception("Vui lòng thêm ít nhất một nguyên liệu!");
            }

            DateTime ngayTra = dtpNgayTra.Value.Date;
            TimeSpan gioTra = dtpGioTra.Value.TimeOfDay;
            string nhanVienTra = txtNhanVienTra.Text.Trim();
            string ghiChuPhieu = txtGhiChuPhieuTra != null ? txtGhiChuPhieuTra.Text.Trim() : "";

            _nguyenLieuBLL.LuuPhieuTraKho(_chiNhanhId, ngayTra, gioTra, nhanVienTra, ghiChuPhieu, chiTietList);
        }

        private void dgvChiTietNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colXoa_Nhap.Index && e.RowIndex >= 0)
            {
                 dgvChiTietNhap.Rows.RemoveAt(e.RowIndex);
                 UpdateSTTNhap();
            }
        }

        private void dgvChiTietTra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colXoa_Tra.Index && e.RowIndex >= 0)
            {
                 dgvChiTietTra.Rows.RemoveAt(e.RowIndex);
                 UpdateSTTTra();
            }
        }

        private void UpdateSTTNhap()
        {
            for (int i = 0; i < dgvChiTietNhap.Rows.Count; i++)
            {
                dgvChiTietNhap.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void UpdateSTTTra()
        {
            for (int i = 0; i < dgvChiTietTra.Rows.Count; i++)
            {
                dgvChiTietTra.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void txtSearchTonKho_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearchTonKho.Text.Trim().ToLower();
                
                if (string.IsNullOrEmpty(keyword))
                {
                    // Hiển thị tất cả
                    LoadTonKho();
                    return;
                }

                // Filter dữ liệu
                dgvTonKho.Rows.Clear();
                if (_tonKhoData != null && _tonKhoData.Rows.Count > 0)
                {
                    var filteredRows = _tonKhoData.AsEnumerable()
                        .Where(row => 
                            (row["ma_nl"]?.ToString() ?? "").ToLower().Contains(keyword) ||
                            (row["ten_nl"]?.ToString() ?? "").ToLower().Contains(keyword));

                    foreach (DataRow row in filteredRows)
                    {
                        int rowIndex = dgvTonKho.Rows.Add();
                        var dgvRow = dgvTonKho.Rows[rowIndex];
                        dgvRow.Cells[colSTT_Ton.Index].Value = row["stt"];
                        dgvRow.Cells[colMaNL_Ton.Index].Value = row["ma_nl"];
                        dgvRow.Cells[colTenNL_Ton.Index].Value = row["ten_nl"];
                        dgvRow.Cells[colTonKho_Ton.Index].Value = FormatSoLuong(Convert.ToDecimal(row["sl_ton"]));
                        dgvRow.Cells[colTonToiThieu_Ton.Index].Value = FormatSoLuong(Convert.ToDecimal(row["ton_toi_thieu"]));
                        dgvRow.Cells[colTrangThai_Ton.Index].Value = row["trang_thai"];
                        // Lưu nl_id vào Tag
                        dgvRow.Tag = row["nl_id"];
                    }
                }
            }
            catch (Exception ex)
            {
                // Ignore search errors
            }
        }

        private void SegmentedPill1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelNhapKho.Visible = false;
            panelTraKho.Visible = false;
            panelTonKho.Visible = false;
            panelLichSu.Visible = false;

            switch (segmentedPill1.SelectedIndex)
            {
                case 0:
                    panelNhapKho.Visible = true;
                    break;
                case 1:
                    panelTraKho.Visible = true;
                    break;
                case 2:
                    panelTonKho.Visible = true;
                    LoadTonKho();
                    break;
                case 3:
                    panelLichSu.Visible = true;
                    LoadLichSu();
                    break;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvChiTietNhap_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == colSTT_Nhap.Index || e.ColumnIndex == colXoa_Nhap.Index || 
                e.ColumnIndex == colDVT_Nhap.Index || e.ColumnIndex == colTon_Nhap.Index)
            {
                e.Cancel = true;
            }
        }

        private void dgvChiTietNhap_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvChiTietNhap.Rows[e.RowIndex];

            // Khi chọn nguyên liệu, tự động điền ĐVT và Tồn kho
            if (e.ColumnIndex == colNguyenLieu_Nhap.Index)
            {
                var selectedValue = row.Cells[colNguyenLieu_Nhap.Index].Value;
                
                if (selectedValue != null && selectedValue != DBNull.Value && _nguyenLieuData != null)
                {
                    try
                    {
                        int nlId = Convert.ToInt32(selectedValue);
                        var nlRow = _nguyenLieuData.AsEnumerable()
                            .FirstOrDefault(r => Convert.ToInt32(r["nl_id"]) == nlId);
                        
                        if (nlRow != null)
                        {
                            row.Cells[colDVT_Nhap.Index].Value = nlRow["don_vi"].ToString();
                            
                            // Lấy tồn kho hiện tại
                            decimal tonKho = _nguyenLieuBLL.LayTonKhoTaiChiNhanh(_chiNhanhId, nlId);
                            row.Cells[colTon_Nhap.Index].Value = FormatSoLuong(tonKho);
                        }
                    }
                    catch
                    {
                        // Ignore conversion errors
                    }
                }
                else
                {
                    row.Cells[colTon_Nhap.Index].Value = "";
                }
                
                CheckSoLuongNhap(row);
            }
            // Khi nhập số lượng, kiểm tra tồn kho
            else if (e.ColumnIndex == colSoLuong_Nhap.Index)
            {
                CheckSoLuongNhap(row);
            }
        }

        private void CheckSoLuongNhap(DataGridViewRow row)
        {
            try
            {
                var nlValue = row.Cells[colNguyenLieu_Nhap.Index].Value;
                var soLuongStr = row.Cells[colSoLuong_Nhap.Index].Value?.ToString() ?? "";

                // Nếu chưa chọn nguyên liệu hoặc chưa nhập số lượng, xóa cảnh báo
                if (nlValue == null || nlValue == DBNull.Value || string.IsNullOrWhiteSpace(soLuongStr))
                {
                    row.Cells[colGhiChu_Nhap.Index].Value = "";
                    row.Cells[colGhiChu_Nhap.Index].Style.ForeColor = Color.Black;
                    return;
                }

                if (!TryParseSoLuong(soLuongStr, out decimal soLuong))
                {
                    row.Cells[colGhiChu_Nhap.Index].Value = "";
                    row.Cells[colGhiChu_Nhap.Index].Style.ForeColor = Color.Black;
                    return;
                }

                int nlId = Convert.ToInt32(nlValue);
                decimal tonHienTai = _nguyenLieuBLL.LayTonKhoTaiChiNhanh(_chiNhanhId, nlId);

                // Kiểm tra nếu số lượng vượt quá tồn kho
                if (soLuong > tonHienTai)
                {
                    string tenNL = _nguyenLieuDict.ContainsKey(nlId) ? _nguyenLieuDict[nlId] : $"ID: {nlId}";
                    row.Cells[colGhiChu_Nhap.Index].Value = $"⚠ Không đủ tồn kho! Tồn: {FormatSoLuong(tonHienTai)}, Thiếu: {FormatSoLuong(soLuong - tonHienTai)}";
                    row.Cells[colGhiChu_Nhap.Index].Style.ForeColor = Color.Red;
                    row.Cells[colGhiChu_Nhap.Index].Style.BackColor = Color.FromArgb(255, 240, 240); // Light red background
                }
                else
                {
                    string currentGhiChu = row.Cells[colGhiChu_Nhap.Index].Value?.ToString() ?? "";
                    if (currentGhiChu.StartsWith("⚠"))
                    {
                        row.Cells[colGhiChu_Nhap.Index].Value = "";
                        row.Cells[colGhiChu_Nhap.Index].Style.ForeColor = Color.Black;
                        row.Cells[colGhiChu_Nhap.Index].Style.BackColor = Color.White;
                    }
                }
            }
            catch
            {
                // Ignore errors
            }
        }

        private void dgvChiTietNhap_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvChiTietNhap.Rows[e.RowIndex];

            // Khi chọn nguyên liệu, tự động điền ĐVT và Tồn kho
            if (e.ColumnIndex == colNguyenLieu_Nhap.Index)
            {
                var selectedValue = row.Cells[colNguyenLieu_Nhap.Index].Value;
                
                if (selectedValue != null && selectedValue != DBNull.Value && _nguyenLieuData != null)
                {
                    try
                    {
                        int nlId = Convert.ToInt32(selectedValue);
                        var nlRow = _nguyenLieuData.AsEnumerable()
                            .FirstOrDefault(r => Convert.ToInt32(r["nl_id"]) == nlId);
                        
                        if (nlRow != null)
                        {
                            row.Cells[colDVT_Nhap.Index].Value = nlRow["don_vi"].ToString();
                            
                            // Lấy tồn kho hiện tại
                            decimal tonKho = _nguyenLieuBLL.LayTonKhoTaiChiNhanh(_chiNhanhId, nlId);
                            row.Cells[colTon_Nhap.Index].Value = FormatSoLuong(tonKho);
                        }
                    }
                    catch
                    {
                        // Ignore conversion errors
                    }
                }
                else
                {
                    // Xóa tồn kho nếu chưa chọn nguyên liệu
                    row.Cells[colTon_Nhap.Index].Value = "";
                }
                
                // Kiểm tra lại số lượng sau khi chọn nguyên liệu
                CheckSoLuongNhap(row);
            }
            // Khi nhập số lượng, kiểm tra tồn kho
            else if (e.ColumnIndex == colSoLuong_Nhap.Index)
            {
                CheckSoLuongNhap(row);
            }
        }

        private void dgvChiTietTra_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Cho phép edit tất cả các cell trừ STT, Tồn, ĐVT, Còn lại và Xóa
            if (e.ColumnIndex == colSTT_Tra.Index || 
                e.ColumnIndex == colTon_Tra.Index || 
                e.ColumnIndex == colDVT_Tra.Index || 
                e.ColumnIndex == colConLai_Tra.Index || 
                e.ColumnIndex == colXoa_Tra.Index)
            {
                e.Cancel = true;
            }
        }

        private void dgvChiTietTra_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvChiTietTra.Rows[e.RowIndex];

            // Khi chọn nguyên liệu, tự động điền Tồn, ĐVT và tính Còn lại
            if (e.ColumnIndex == colNguyenLieu_Tra.Index)
            {
                var selectedValue = row.Cells[colNguyenLieu_Tra.Index].Value;
                
                if (selectedValue != null && selectedValue != DBNull.Value && _nguyenLieuData != null)
                {
                    try
                    {
                        int nlId = Convert.ToInt32(selectedValue);
                        var nlRow = _nguyenLieuData.AsEnumerable()
                            .FirstOrDefault(r => Convert.ToInt32(r["nl_id"]) == nlId);
                        
                        if (nlRow != null)
                        {
                            row.Cells[colDVT_Tra.Index].Value = nlRow["don_vi"].ToString();
                            
                            // Lấy tồn kho hiện tại
                            decimal tonKho = _nguyenLieuBLL.LayTonKhoTaiChiNhanh(_chiNhanhId, nlId);
                            row.Cells[colTon_Tra.Index].Value = FormatSoLuong(tonKho);
                            row.Cells[colConLai_Tra.Index].Value = FormatSoLuong(tonKho);
                        }
                    }
                    catch
                    {
                        // Ignore conversion errors
                    }
                }
            }
            // Khi nhập số lượng trả, tính lại Còn lại
            else if (e.ColumnIndex == colSoLuongTra.Index)
            {
                decimal tonKho = 0;
                decimal soLuongTra = 0;
                
                if (TryParseSoLuong(row.Cells[colTon_Tra.Index].Value?.ToString() ?? "", out tonKho) &&
                    TryParseSoLuong(row.Cells[colSoLuongTra.Index].Value?.ToString() ?? "", out soLuongTra))
                {
                    decimal conLai = tonKho - soLuongTra;
                    row.Cells[colConLai_Tra.Index].Value = FormatSoLuong(conLai >= 0 ? conLai : 0);
                }
            }
        }

        private void dgvChiTietTra_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvChiTietTra.Rows[e.RowIndex];

            // Khi chọn nguyên liệu, tự động điền Tồn, ĐVT và tính Còn lại
            if (e.ColumnIndex == colNguyenLieu_Tra.Index)
            {
                var selectedValue = row.Cells[colNguyenLieu_Tra.Index].Value;
                
                if (selectedValue != null && selectedValue != DBNull.Value && _nguyenLieuData != null)
                {
                    int nlId = Convert.ToInt32(selectedValue);
                    var nlRow = _nguyenLieuData.AsEnumerable()
                        .FirstOrDefault(r => Convert.ToInt32(r["nl_id"]) == nlId);
                    
                    if (nlRow != null)
                    {
                        row.Cells[colDVT_Tra.Index].Value = nlRow["don_vi"].ToString();
                        
                        // Lấy tồn kho hiện tại
                        decimal tonKho = _nguyenLieuBLL.LayTonKhoTaiChiNhanh(_chiNhanhId, nlId);
                        row.Cells[colTon_Tra.Index].Value = tonKho.ToString("N3");
                        row.Cells[colConLai_Tra.Index].Value = tonKho.ToString("N3");
                    }
                }
            }
            // Khi nhập số lượng trả, tính lại Còn lại
            else if (e.ColumnIndex == colSoLuongTra.Index)
            {
                decimal tonKho = 0;
                decimal soLuongTra = 0;
                
                if (TryParseSoLuong(row.Cells[colTon_Tra.Index].Value?.ToString() ?? "", out tonKho) &&
                    TryParseSoLuong(row.Cells[colSoLuongTra.Index].Value?.ToString() ?? "", out soLuongTra))
                {
                    decimal conLai = tonKho - soLuongTra;
                    row.Cells[colConLai_Tra.Index].Value = FormatSoLuong(conLai >= 0 ? conLai : 0);
                }
            }
        }

        private void dgvChiTietNhap_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == colNguyenLieu_Nhap.Index)
            {
                e.ThrowException = false;
                if (e.RowIndex >= 0 && e.RowIndex < dgvChiTietNhap.Rows.Count)
                {
                    dgvChiTietNhap.Rows[e.RowIndex].Cells[colNguyenLieu_Nhap.Index].Value = DBNull.Value;
                }
            }
        }

        private void dgvChiTietNhap_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Lắng nghe TextChanged và KeyPress khi đang edit cột số lượng
            if (dgvChiTietNhap.CurrentCell.ColumnIndex == colSoLuong_Nhap.Index)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.TextChanged -= TextBox_TextChanged;
                    textBox.KeyPress -= TextBox_KeyPress;
                    textBox.TextChanged += TextBox_TextChanged;
                    textBox.KeyPress += TextBox_KeyPress;
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép số, dấu phẩy, dấu chấm và phím điều khiển
            if (dgvChiTietNhap.CurrentCell != null && 
                dgvChiTietNhap.CurrentCell.ColumnIndex == colSoLuong_Nhap.Index)
            {
                if (char.IsControl(e.KeyChar))
                {
                    return;
                }

                if (char.IsDigit(e.KeyChar))
                {
                    return;
                }

                if (e.KeyChar == ',' || e.KeyChar == '.')
                {
                    TextBox textBox = sender as TextBox;
                    if (textBox != null)
                    {
                        string currentText = textBox.Text;
                        if (currentText.Contains(',') || currentText.Contains('.'))
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                    return;
                }

                e.Handled = true;
            }
        }

        private void dgvChiTietNhap_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Validate cột số lượng
            if (e.ColumnIndex == colSoLuong_Nhap.Index)
            {
                string value = e.FormattedValue?.ToString() ?? "";
                
                if (string.IsNullOrWhiteSpace(value))
                {
                    dgvChiTietNhap.Rows[e.RowIndex].ErrorText = "";
                    return;
                }

                if (TryParseSoLuong(value, out decimal soLuong))
                {
                    if (soLuong <= 0)
                    {
                        e.Cancel = true;
                        dgvChiTietNhap.Rows[e.RowIndex].ErrorText = "Số lượng phải lớn hơn 0";
                        MessageBox.Show("Số lượng phải là số dương lớn hơn 0!", "Cảnh báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        dgvChiTietNhap.Rows[e.RowIndex].ErrorText = "";
                    }
                }
                else
                {
                    e.Cancel = true;
                    dgvChiTietNhap.Rows[e.RowIndex].ErrorText = "Số lượng không hợp lệ";
                    MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Cảnh báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            // Kiểm tra và hiển thị cảnh báo ngay khi đang nhập
            if (dgvChiTietNhap.CurrentCell != null && 
                dgvChiTietNhap.CurrentCell.ColumnIndex == colSoLuong_Nhap.Index &&
                dgvChiTietNhap.CurrentCell.RowIndex >= 0)
            {
                var row = dgvChiTietNhap.Rows[dgvChiTietNhap.CurrentCell.RowIndex];
                TextBox textBox = sender as TextBox;
                
                string soLuongStr = textBox?.Text ?? "";
                CheckSoLuongNhapWithValue(row, soLuongStr);
            }
        }

        private void CheckSoLuongNhapWithValue(DataGridViewRow row, string soLuongStr)
        {
            try
            {
                var nlValue = row.Cells[colNguyenLieu_Nhap.Index].Value;

                // Nếu chưa chọn nguyên liệu hoặc chưa nhập số lượng, xóa cảnh báo
                if (nlValue == null || nlValue == DBNull.Value || string.IsNullOrWhiteSpace(soLuongStr))
                {
                    string currentGhiChu = row.Cells[colGhiChu_Nhap.Index].Value?.ToString() ?? "";
                    if (currentGhiChu.StartsWith("⚠"))
                    {
                        row.Cells[colGhiChu_Nhap.Index].Value = "";
                        row.Cells[colGhiChu_Nhap.Index].Style.ForeColor = Color.Black;
                        row.Cells[colGhiChu_Nhap.Index].Style.BackColor = Color.White;
                    }
                    return;
                }

                // Parse số lượng
                if (!TryParseSoLuong(soLuongStr, out decimal soLuong))
                {
                    string currentGhiChu = row.Cells[colGhiChu_Nhap.Index].Value?.ToString() ?? "";
                    if (currentGhiChu.StartsWith("⚠"))
                    {
                        row.Cells[colGhiChu_Nhap.Index].Value = "";
                        row.Cells[colGhiChu_Nhap.Index].Style.ForeColor = Color.Black;
                        row.Cells[colGhiChu_Nhap.Index].Style.BackColor = Color.White;
                    }
                    return;
                }

                int nlId = Convert.ToInt32(nlValue);
                decimal tonHienTai = _nguyenLieuBLL.LayTonKhoTaiChiNhanh(_chiNhanhId, nlId);

                // Kiểm tra nếu số lượng vượt quá tồn kho
                if (soLuong > tonHienTai)
                {
                    string tenNL = _nguyenLieuDict.ContainsKey(nlId) ? _nguyenLieuDict[nlId] : $"ID: {nlId}";
                    row.Cells[colGhiChu_Nhap.Index].Value = $"⚠ Không đủ tồn kho! Tồn: {FormatSoLuong(tonHienTai)}, Thiếu: {FormatSoLuong(soLuong - tonHienTai)}";
                    row.Cells[colGhiChu_Nhap.Index].Style.ForeColor = Color.Red;
                    row.Cells[colGhiChu_Nhap.Index].Style.BackColor = Color.FromArgb(255, 240, 240); // Light red background
                }
                else
                {
                    // Xóa cảnh báo nếu đủ tồn kho
                    string currentGhiChu = row.Cells[colGhiChu_Nhap.Index].Value?.ToString() ?? "";
                    if (currentGhiChu.StartsWith("⚠"))
                    {
                        row.Cells[colGhiChu_Nhap.Index].Value = "";
                        row.Cells[colGhiChu_Nhap.Index].Style.ForeColor = Color.Black;
                        row.Cells[colGhiChu_Nhap.Index].Style.BackColor = Color.White;
                    }
                }
            }
            catch { }
        }

        private void dgvChiTietTra_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == colNguyenLieu_Tra.Index)
            {
                e.ThrowException = false;
                if (e.RowIndex >= 0 && e.RowIndex < dgvChiTietTra.Rows.Count)
                {
                    dgvChiTietTra.Rows[e.RowIndex].Cells[colNguyenLieu_Tra.Index].Value = DBNull.Value;
                }
            }
        }

        private void dgvTonKho_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Chỉ xử lý khi chỉnh sửa cột Tồn tối thiểu
            if (e.ColumnIndex == colTonToiThieu_Ton.Index)
            {
                try
                {
                    var row = dgvTonKho.Rows[e.RowIndex];
                    var nlId = row.Tag;
                    var tonToiThieuStr = row.Cells[colTonToiThieu_Ton.Index].Value?.ToString();

                    if (nlId == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin nguyên liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(tonToiThieuStr) || 
                        !TryParseSoLuong(tonToiThieuStr, out decimal tonToiThieu))
                    {
                        MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Khôi phục giá trị cũ
                        LoadTonKho();
                        return;
                    }

                    if (tonToiThieu < 0)
                    {
                        MessageBox.Show("Tồn tối thiểu không được âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadTonKho();
                        return;
                    }

                    int nlIdInt = Convert.ToInt32(nlId);
                    
                    // Cập nhật vào database
                    _nguyenLieuBLL.CapNhatTonToiThieu(_chiNhanhId, nlIdInt, tonToiThieu);

                    // Cập nhật lại trạng thái
                    var slTonStr = row.Cells[colTonKho_Ton.Index].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(slTonStr) && 
                        TryParseSoLuong(slTonStr, out decimal slTon))
                    {
                        string trangThai;
                        if (slTon == 0)
                            trangThai = "Hết hàng";
                        else if (slTon <= tonToiThieu)
                            trangThai = "Tồn thấp";
                        else
                            trangThai = "Đủ tồn";

                        row.Cells[colTrangThai_Ton.Index].Value = trangThai;
                    }

                    UpdateSummaryCards();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi cập nhật tồn tối thiểu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadTonKho();
                }
            }
        }

        private string FormatSoLuong(decimal value)
        {
            return value.ToString("#,##0.###", CultureInfo.CurrentCulture);
        }

        private bool TryParseSoLuong(string value, out decimal result)
        {
            result = 0;
            if (string.IsNullOrWhiteSpace(value))
                return false;
            
            string cleanedValue = value.Replace(",", "").Trim();
            return decimal.TryParse(cleanedValue, NumberStyles.Number, CultureInfo.CurrentCulture, out result);
        }

        private DataTable _lichSuData;

        private void LoadLichSu()
        {
            try
            {
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1); // Đến cuối ngày
                string loaiPhieu = cmbLoaiPhieu.SelectedIndex == 0 ? null : (cmbLoaiPhieu.SelectedIndex == 1 ? "NHAP" : "TRA");
                string keyword = txtSearchLichSu.Text.Trim();

                _lichSuData = _nguyenLieuBLL.LayLichSuNhapTra(_chiNhanhId, tuNgay, denNgay, loaiPhieu, keyword);
                
                dgvLichSu.Rows.Clear();
                if (_lichSuData != null && _lichSuData.Rows.Count > 0)
                {
                    int stt = 1;
                    foreach (DataRow row in _lichSuData.Rows)
                    {
                        int rowIndex = dgvLichSu.Rows.Add();
                        var dgvRow = dgvLichSu.Rows[rowIndex];
                        dgvRow.Cells[colSTT_LichSu.Index].Value = stt++;
                        dgvRow.Cells[colLoaiPhieu_LichSu.Index].Value = row["loai_phieu"]?.ToString() ?? "";
                        dgvRow.Cells[colNgay_LichSu.Index].Value = row["ngay"] != DBNull.Value 
                            ? Convert.ToDateTime(row["ngay"]).ToString("dd/MM/yyyy") : "";
                        
                        // Xử lý TimeSpan cho cột giờ
                        if (row["gio"] != DBNull.Value)
                        {
                            if (row["gio"] is TimeSpan timeSpan)
                            {
                                dgvRow.Cells[colGio_LichSu.Index].Value = timeSpan.ToString(@"hh\:mm");
                            }
                            else if (row["gio"] is DateTime dateTime)
                            {
                                dgvRow.Cells[colGio_LichSu.Index].Value = dateTime.ToString("HH:mm");
                            }
                            else
                            {
                                dgvRow.Cells[colGio_LichSu.Index].Value = row["gio"]?.ToString() ?? "";
                            }
                        }
                        else
                        {
                            dgvRow.Cells[colGio_LichSu.Index].Value = "";
                        }
                        
                        dgvRow.Cells[colNhanVien_LichSu.Index].Value = row["nhan_vien"]?.ToString() ?? "";
                        dgvRow.Cells[colTrangThai_LichSu.Index].Value = row["trang_thai"]?.ToString() ?? "";
                        dgvRow.Cells[colGhiChu_LichSu.Index].Value = row["ghi_chu"]?.ToString() ?? "";
                        
                        // Lưu phieu_id vào Tag để dùng khi hủy
                        if (row["phieu_id"] != DBNull.Value)
                        {
                            dgvRow.Tag = Convert.ToInt32(row["phieu_id"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải lịch sử: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvLichSu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var row = dgvLichSu.Rows[e.RowIndex];
                
                if (row.Tag == null)
                {
                    return;
                }

                int phieuId = Convert.ToInt32(row.Tag);
                string loaiPhieu = row.Cells[colLoaiPhieu_LichSu.Index].Value?.ToString() ?? "";

                if (phieuId > 0 && !string.IsNullOrWhiteSpace(loaiPhieu))
                {
                    // Mở form chi tiết
                    var frmChiTiet = new Frm_ChiTietNhapTraNL(phieuId, loaiPhieu);
                    if (frmChiTiet.ShowDialog() == DialogResult.OK)
                    {
                        LoadLichSu();
                        LoadTonKho();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở chi tiết phiếu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearchLichSu_TextChanged(object sender, EventArgs e)
        {
            if (panelLichSu.Visible)
            {
                LoadLichSu();
            }
        }

        private void dgvLichSu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colLoaiPhieu_LichSu.Index && e.RowIndex >= 0)
            {
                var row = dgvLichSu.Rows[e.RowIndex];
                string loaiPhieu = row.Cells[colLoaiPhieu_LichSu.Index].Value?.ToString() ?? "";
                
                if (loaiPhieu == "Nhập kho")
                {
                    e.CellStyle.BackColor = Color.FromArgb(220, 255, 220); // Light green
                    e.CellStyle.ForeColor = Color.FromArgb(34, 197, 94); // Green text
                }
                else if (loaiPhieu == "Trả kho")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 235, 238); // Light red
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68); // Red text
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void cmbLoaiPhieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (panelLichSu.Visible)
            {
                LoadLichSu();
            }
        }

        private void dtpTuNgay_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTuNgay.Value > dtpDenNgay.Value)
            {
                dtpDenNgay.Value = dtpTuNgay.Value;
            }
            
            if (panelLichSu.Visible)
            {
                LoadLichSu();
            }
        }

        private void dtpDenNgay_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDenNgay.Value < dtpTuNgay.Value)
            {
                dtpTuNgay.Value = dtpDenNgay.Value;
            }
            
            if (panelLichSu.Visible)
            {
                LoadLichSu();
            }
        }
    }
}

