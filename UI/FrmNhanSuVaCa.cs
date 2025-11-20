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
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmNhanSuVaCa : Form
    {
        private NguoiDungBLL _nguoiDungBLL;
        private DatabaseHelper _dbHelper;
        private Controls.PhanCaPanel _phanCaPanel;

        public FrmNhanSuVaCa()
        {
            InitializeComponent();
            try
            {
                _dbHelper = new DatabaseHelper();

                if (!_dbHelper.TestConnection())
                {
                    MessageBox.Show(
                        "Không thể kết nối đến database!\n\n" +
                        "Kiểm tra lại:\n" +
                        "1. SQL Server đang chạy\n" +
                        "2. Server name trong DatabaseHelper.cs\n" +
                        "3. Database: QL_NhaHangTiecCuoi_V3\n\n" +
                        "Mở SQL Server Management Studio để kiểm tra.",
                        "Lỗi Kết Nối Database",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                _nguoiDungBLL = new NguoiDungBLL(_dbHelper);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void cbbNhanSu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ load lại dữ liệu nếu form đã được load hoàn toàn
            // Tránh trigger khi đang khởi tạo ComboBox
            if (this.IsHandleCreated && _nguoiDungBLL != null)
            {
                LoadDataNhanSu();
            }
        }

        /// <summary>
        /// Load danh sách chức vụ vào ComboBox cbbNhanSu
        /// </summary>
        private void LoadChucVu()
        {
            try
            {
                if (_nguoiDungBLL == null)
                    return;

                cbbNhanSu.Items.Clear();
                cbbNhanSu.Items.Add("Tất cả"); // Thêm option "Tất cả"

                DataTable dt = _nguoiDungBLL.LayDanhSachChucVu();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string tenChucVu = row["ten_chuc_vu"]?.ToString() ?? "";
                        if (!string.IsNullOrEmpty(tenChucVu))
                        {
                            cbbNhanSu.Items.Add(tenChucVu);
                        }
                    }
                }

                // Chọn "Tất cả" mặc định (tạm thời tắt event để tránh trigger khi load)
                if (cbbNhanSu.Items.Count > 0)
                {
                    cbbNhanSu.SelectedIndexChanged -= cbbNhanSu_SelectedIndexChanged;
                    cbbNhanSu.SelectedIndex = 0;
                    cbbNhanSu.SelectedIndexChanged += cbbNhanSu_SelectedIndexChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách chức vụ: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private const string NS_TEN = "TenNV";
        private const string NS_CV = "ChucVu";
        private const string NS_CN = "ChiNhanh";
        private void InitDgvNhanSu()
        {
            var dgv = dgvNhanSu;

            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (dgv.Columns.Count == 0)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = NS_TEN, HeaderText = "Tên NV", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 210 });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = NS_CV, HeaderText = "Chức vụ", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = NS_CN, HeaderText = "Chi nhánh", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 250 });
            }
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgv.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.RowTemplate.Height = 56;
        }
        private void LoadDataNhanSu()
        {
            try
            {
                dgvNhanSu.Rows.Clear();

                // Lấy chức vụ được chọn từ ComboBox
                string chucVuFilter = null;
                if (cbbNhanSu.SelectedItem != null && cbbNhanSu.SelectedItem.ToString() != "Tất cả")
                {
                    chucVuFilter = cbbNhanSu.SelectedItem.ToString();
                }

                // Lấy từ khóa tìm kiếm từ TextBox
                string searchKeyword = roundedTextBox1?.Text?.Trim() ?? "";

                DataTable dt = _nguoiDungBLL.LayDanhSachNhanVien();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string ten = row["TenNV"]?.ToString() ?? "";
                        string chucVu = row["ChucVu"]?.ToString() ?? "";
                        string chiNhanh = row["ChiNhanh"]?.ToString() ?? "";

                        // Lọc theo chức vụ nếu có chọn
                        bool matchChucVu = (chucVuFilter == null || chucVu == chucVuFilter);

                        // Tìm kiếm theo tên hoặc chức vụ (không phân biệt hoa thường)
                        bool matchSearch = string.IsNullOrEmpty(searchKeyword);
                        if (!matchSearch)
                        {
                            string searchLower = searchKeyword.ToLower();
                            matchSearch = ten.ToLower().Contains(searchLower) || 
                                         chucVu.ToLower().Contains(searchLower);
                        }

                        // Chỉ hiển thị nếu thỏa cả 2 điều kiện: lọc chức vụ và tìm kiếm
                        if (matchChucVu && matchSearch)
                        {
                            dgvNhanSu.Rows.Add(ten, chucVu, chiNhanh);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
        private void roundedTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                if (_nguoiDungBLL != null)
                {
                    LoadDataNhanSu();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi trong TextChanged: {ex.Message}");
            }
        }


        private void FrmNhanSuVaCa_Load(object sender, EventArgs e)
        {
            try
            {
                if (_nguoiDungBLL == null)
                {
                    MessageBox.Show("Không thể khởi tạo kết nối database. Vui lòng kiểm tra lại cấu hình.", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                InitDgvNhanSu();
                LoadDataNhanSu();
                LoadTongSoNhanVien();
                LoadChucVu(); // Load chức vụ vào ComboBox
                
                // Khởi tạo PhanCaPanel nhưng chưa hiển thị
                InitializePhanCaPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load form: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load tổng số nhân viên lên RpanelTongNV
        /// </summary>
        private void LoadTongSoNhanVien()
        {
            try
            {
                if (_nguoiDungBLL == null)
                    return;

                // Lấy danh sách nhân viên và đếm số lượng
                DataTable dt = _nguoiDungBLL.LayDanhSachNhanVien();
                int tongSo = dt != null ? dt.Rows.Count : 0;

                // Cập nhật label8 với tổng số nhân viên
                if (label8 != null)
                {
                    label8.Text = tongSo.ToString();
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, hiển thị "0" hoặc giữ nguyên giá trị cũ
                if (label8 != null)
                {
                    label8.Text = "0";
                }
                System.Diagnostics.Debug.WriteLine($"Lỗi load tổng số nhân viên: {ex.Message}");
            }
        }

        private void btnThemNV_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_ThemNV())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    // Reload dữ liệu nhân viên sau khi thêm mới
                    LoadDataNhanSu();
                    LoadTongSoNhanVien();
                    
                    // Reload dữ liệu phân ca nếu đang ở tab phân ca
                    if (segmentedPill1.SelectedIndex == 1 && _phanCaPanel != null && !_phanCaPanel.IsDisposed)
                    {
                        _phanCaPanel.LoadDataPhanCa();
                    }
                }
            }
        }

        private void segmentedPill1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (segmentedPill1.SelectedIndex == 0)
            {
                panelNhanSu.Visible = true;
                panelPhanCa.Visible = false;
                panelNhanSu.Location= new Point(2, 341);
            }
            else if (segmentedPill1.SelectedIndex == 1)
            {
                panelNhanSu.Visible = false;
                panelPhanCa.Visible = true;
               

                // Khởi tạo và load PhanCaPanel nếu chưa có
                if (_phanCaPanel == null || _phanCaPanel.IsDisposed)
                {
                    InitializePhanCaPanel();
                }
                else
                {
                    // Reload dữ liệu khi chuyển tab
                    _phanCaPanel.LoadDataPhanCa();
                }
            }
        }

        private void InitializePhanCaPanel()
        {
            try
            {
                // Xóa panel cũ nếu có
                panelPhanCa.Controls.Clear();
                if (_phanCaPanel != null && !_phanCaPanel.IsDisposed)
                {
                    _phanCaPanel.Dispose();
                }

                // Tạo PhanCaPanel mới
                _phanCaPanel = new Controls.PhanCaPanel
                {
                    Dock = DockStyle.Fill
                };

                // Thêm vào panelPhanCa
                panelPhanCa.Controls.Add(_phanCaPanel);
                
                // Load dữ liệu
                _phanCaPanel.LoadDataPhanCa();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo panel phân ca: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
