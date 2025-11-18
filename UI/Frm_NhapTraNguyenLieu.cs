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
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Common;
using UI.Controls;
using Guna.UI2.WinForms;

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
            _dbHelper = new DatabaseHelper();
            _nguyenLieuBLL = new NguyenLieuBLL(_dbHelper);
            _chiNhanhId = Session.ChiNhanhId > 0 ? Session.ChiNhanhId : 1;

            this.Load += Frm_NhapTraNguyenLieu_Load;
            segmentedPill1.SelectedIndexChanged += SegmentedPill1_SelectedIndexChanged;
            btnThemDongNhap.Click += btnThemDongNhap_Click;
            btnThemDongTra.Click += btnThemDongTra_Click;
            btnHuyNhap.Click += btnHuyNhap_Click;
            btnHuyTra.Click += btnHuyTra_Click;
            btnLuuPhieuNhap.Click += btnLuuPhieuNhap_Click;
            btnLuuPhieuTra.Click += btnLuuPhieuTra_Click;
            dgvChiTietNhap.CellClick += dgvChiTietNhap_CellClick;
            dgvChiTietNhap.CellBeginEdit += dgvChiTietNhap_CellBeginEdit;
            dgvChiTietNhap.CellEndEdit += dgvChiTietNhap_CellEndEdit;
            dgvChiTietNhap.CellValueChanged += dgvChiTietNhap_CellValueChanged;
            dgvChiTietNhap.DataError += dgvChiTietNhap_DataError;
            dgvChiTietTra.CellClick += dgvChiTietTra_CellClick;
            dgvChiTietTra.CellBeginEdit += dgvChiTietTra_CellBeginEdit;
            dgvChiTietTra.CellEndEdit += dgvChiTietTra_CellEndEdit;
            dgvChiTietTra.CellValueChanged += dgvChiTietTra_CellValueChanged;
            dgvChiTietTra.DataError += dgvChiTietTra_DataError;
            txtSearchTonKho.TextChanged += txtSearchTonKho_TextChanged;
            dgvTonKho.CellEndEdit += dgvTonKho_CellEndEdit;
        }

        private void Frm_NhapTraNguyenLieu_Load(object sender, EventArgs e)
        {
            InitializeForm();
            // Hiển thị tab đầu tiên (Nhập kho)
            segmentedPill1.SelectedIndex = 0;
            panelNhapKho.Visible = true;
            panelTraKho.Visible = false;
            panelTonKho.Visible = false;
            
            LoadNguyenLieu();
            
            // Refresh lại để đảm bảo combobox hiển thị dữ liệu
            this.BeginInvoke(new Action(() =>
            {
                PopulateComboBoxColumns();
            }));
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
            newRow.Cells[colTon_Tra.Index].Value = "0";
            newRow.Cells[colSoLuongTra.Index].Value = "";
            newRow.Cells[colDVT_Tra.Index].Value = "";
            newRow.Cells[colConLai_Tra.Index].Value = "0";
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

        private void btnHuyNhap_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy phiếu nhập kho?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResetFormNhap();
            }
        }

        private void btnHuyTra_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy phiếu trả kho?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResetFormTra();
            }
        }

        private void ResetFormNhap()
        {
            dtpNgayNhap.Value = DateTime.Now;
            dtpGioNhap.Value = DateTime.Now;
            txtNhanVienNhap.Text = "";
            dgvChiTietNhap.Rows.Clear();
            AddRowNhap();
        }

        private void ResetFormTra()
        {
            dtpNgayTra.Value = DateTime.Now;
            dtpGioTra.Value = DateTime.Now;
            txtNhanVienTra.Text = "";
            dgvChiTietTra.Rows.Clear();
            AddRowTra();
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

                if (string.IsNullOrWhiteSpace(soLuong) || !decimal.TryParse(soLuong, out _))
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

                if (string.IsNullOrWhiteSpace(soLuongTra) || !decimal.TryParse(soLuongTra, out _))
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
                if (string.IsNullOrWhiteSpace(soLuongStr) || !decimal.TryParse(soLuongStr, out decimal soLuong)) continue;

                int nlId = Convert.ToInt32(nlValue);
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

            _nguyenLieuBLL.LuuPhieuNhapKho(_chiNhanhId, ngayNhap, gioNhap, nhanVienNhap, "", chiTietList);
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
                if (string.IsNullOrWhiteSpace(soLuongTraStr) || !decimal.TryParse(soLuongTraStr, out decimal soLuongTra)) continue;
                if (string.IsNullOrWhiteSpace(tonStr) || !decimal.TryParse(tonStr, out decimal ton)) continue;
                if (string.IsNullOrWhiteSpace(conLaiStr) || !decimal.TryParse(conLaiStr, out decimal conLai)) continue;

                // Kiểm tra số lượng trả không vượt quá tồn kho
                if (soLuongTra > ton)
                {
                    throw new Exception($"Số lượng trả không được vượt quá tồn kho hiện tại!");
                }

                int nlId = Convert.ToInt32(nlValue);
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

            _nguyenLieuBLL.LuuPhieuTraKho(_chiNhanhId, ngayTra, gioTra, nhanVienTra, "", chiTietList);
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
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvChiTietNhap_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == colSTT_Nhap.Index || e.ColumnIndex == colXoa_Nhap.Index || e.ColumnIndex == colDVT_Nhap.Index)
            {
                e.Cancel = true;
            }
        }

        private void dgvChiTietNhap_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Khi chọn nguyên liệu, tự động điền ĐVT
            if (e.ColumnIndex == colNguyenLieu_Nhap.Index)
            {
                var row = dgvChiTietNhap.Rows[e.RowIndex];
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
                        }
                    }
                    catch
                    {
                        // Ignore conversion errors
                    }
                }
            }
        }

        private void dgvChiTietNhap_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Khi chọn nguyên liệu, tự động điền ĐVT
            if (e.ColumnIndex == colNguyenLieu_Nhap.Index)
            {
                var row = dgvChiTietNhap.Rows[e.RowIndex];
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
                        }
                    }
                    catch
                    {
                        // Ignore conversion errors
                    }
                }
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
                            row.Cells[colTon_Tra.Index].Value = tonKho.ToString("N3");
                            row.Cells[colConLai_Tra.Index].Value = tonKho.ToString("N3");
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
                
                if (decimal.TryParse(row.Cells[colTon_Tra.Index].Value?.ToString(), out tonKho) &&
                    decimal.TryParse(row.Cells[colSoLuongTra.Index].Value?.ToString(), out soLuongTra))
                {
                    decimal conLai = tonKho - soLuongTra;
                    row.Cells[colConLai_Tra.Index].Value = conLai >= 0 ? conLai.ToString("N3") : "0";
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
                
                if (decimal.TryParse(row.Cells[colTon_Tra.Index].Value?.ToString(), out tonKho) &&
                    decimal.TryParse(row.Cells[colSoLuongTra.Index].Value?.ToString(), out soLuongTra))
                {
                    decimal conLai = tonKho - soLuongTra;
                    row.Cells[colConLai_Tra.Index].Value = conLai >= 0 ? conLai.ToString("N3") : "0";
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
                        !decimal.TryParse(tonToiThieuStr, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal tonToiThieu))
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
                        decimal.TryParse(slTonStr, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal slTon))
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
    }
}

