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
using UI.Common;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmNhanSuVaCa : Form
    {
        private NguoiDungBLL _nguoiDungBLL;
        private DatabaseHelper _dbHelper;

        // Panel phân ca
        private DataGridView _dgvPhanCaNhanVien;
        private Panel _panelCaLamViec;
        private int _selectedNguoiDungId = -1;
        private List<CheckBox> _caCheckBoxes = new List<CheckBox>();

        public FrmNhanSuVaCa()
        {
            InitializeComponent();
            try
            {
                _dbHelper = new DatabaseHelper();

                if (!_dbHelper.TestConnection())
                {
                    MessageBox.Show(
                        "Không thể kết nối đến database!",
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
        private const string NS_ID = "NguoiDungId"; // Cột ẩn để lưu ID
        private void InitDgvNhanSu()
        {
            var dgv = dgvNhanSu;

            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (dgv.Columns.Count == 0)
            {
                // Cột ẩn để lưu ID
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = NS_ID, HeaderText = "ID", Visible = false });
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


                string searchKeyword = roundedTextBox1?.Text?.Trim() ?? "";
                int currentChiNhanhId = Session.ChiNhanhId;

                DataTable dt = _nguoiDungBLL.LayDanhSachNhanVien();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string ten = row["TenNV"]?.ToString() ?? "";
                        string chucVu = row["ChucVu"]?.ToString() ?? "";
                        string chiNhanh = row["ChiNhanh"]?.ToString() ?? "";
                        int chiNhanhId = 0;
                        if (row.Table.Columns.Contains("ChiNhanhId") && row["ChiNhanhId"] != DBNull.Value)
                        {
                            chiNhanhId = Convert.ToInt32(row["ChiNhanhId"]);
                        }

                        // Lọc theo chi nhánh: nếu đã chọn chi nhánh -> chỉ hiển thị nhân viên thuộc chi nhánh đó
                        // hoặc nhân viên chưa thuộc chi nhánh nào (chiNhanhId = 0)
                        bool matchChiNhanh = true;
                        if (currentChiNhanhId > 0)
                        {
                            matchChiNhanh = chiNhanhId == currentChiNhanhId || chiNhanhId == 0;
                        }

                        if (!matchChiNhanh)
                            continue;

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

                        // Chỉ hiển thị nếu thỏa cả 3 điều kiện
                        if (matchChiNhanh && matchChucVu && matchSearch)
                        {
                            int nguoiDungId = Convert.ToInt32(row["NguoiDungId"]);
                            dgvNhanSu.Rows.Add(nguoiDungId, ten, chucVu, chiNhanh);
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

                // Đăng ký sự kiện CellDoubleClick
                dgvNhanSu.CellDoubleClick += DgvNhanSu_CellDoubleClick;

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

                // Lấy danh sách nhân viên và đếm số lượng theo chi nhánh hiện tại
                DataTable dt = _nguoiDungBLL.LayDanhSachNhanVien();
                int currentChiNhanhId = Session.ChiNhanhId;
                int tongSo = 0;
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int chiNhanhId = 0;
                        if (row.Table.Columns.Contains("ChiNhanhId") && row["ChiNhanhId"] != DBNull.Value)
                        {
                            chiNhanhId = Convert.ToInt32(row["ChiNhanhId"]);
                        }

                        if (currentChiNhanhId <= 0 || chiNhanhId == currentChiNhanhId || chiNhanhId == 0)
                        {
                            tongSo++;
                        }
                    }
                }

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
                    if (segmentedPill1.SelectedIndex == 1 && _dgvPhanCaNhanVien != null)
                    {
                        LoadPhanCaNhanVien();
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
                btnTimNVCa.Visible = false;
            }
            else if (segmentedPill1.SelectedIndex == 1)
            {
                panelNhanSu.Visible = false;
                panelPhanCa.Visible = true;
                btnTimNVCa.Visible = true;

                // Khởi tạo controls nếu chưa có
                if (_dgvPhanCaNhanVien == null)
                {
                    InitializePhanCaControls();
                }

                // Reload dữ liệu khi chuyển tab
                LoadPhanCaNhanVien();
            }
        }

        private void InitializePhanCaControls()
        {
            try
            {
                // Xóa controls cũ nếu có
                panelPhanCa.Controls.Clear();

                // Tạo SplitContainer để chia đôi màn hình
                var tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    Padding = new Padding(5)
                };
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));


                // === Panel bên trái: DataGridView nhân viên ===
                var leftPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };

                var lblTitleLeft = new Label
                {
                    Text = "Danh sách nhân viên",
                    Font = new Font("Segoe UI Semibold", 12f),
                    Dock = DockStyle.Top,
                    Height = 35,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                leftPanel.Controls.Add(lblTitleLeft);

                _dgvPhanCaNhanVien = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoGenerateColumns = false,
                    AllowUserToAddRows = false,
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    ReadOnly = true,
                    BackgroundColor = SystemColors.Window,
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Thêm các cột
                _dgvPhanCaNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "NguoiDungId", HeaderText = "ID", Visible = false });
                _dgvPhanCaNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenNV", HeaderText = "Tên nhân viên", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                _dgvPhanCaNhanVien.Columns.Add(new DataGridViewTextBoxColumn { Name = "ChucVu", HeaderText = "Chức vụ", Width = 150 });

                // Style
                _dgvPhanCaNhanVien.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
                _dgvPhanCaNhanVien.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10f);
                _dgvPhanCaNhanVien.RowTemplate.Height = 45;

                // Event khi chọn nhân viên
                _dgvPhanCaNhanVien.SelectionChanged += DgvPhanCaNhanVien_SelectionChanged;

                leftPanel.Controls.Add(_dgvPhanCaNhanVien);
                _dgvPhanCaNhanVien.BringToFront();

                tableLayout.Controls.Add(leftPanel, 0, 0);

                // === Panel bên phải: Danh sách ca làm việc ===
                _panelCaLamViec = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15), AutoScroll = true };

                var lblTitleRight = new Label
                {
                    Text = "Đăng ký ca làm việc",
                    Font = new Font("Segoe UI Semibold", 12f),
                    Dock = DockStyle.Top,
                    Height = 35,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                _panelCaLamViec.Controls.Add(lblTitleRight);

                var lblSelectEmployee = new Label
                {
                    Name = "lblSelectEmployee",
                    Text = "Vui lòng chọn nhân viên để xem/đăng ký ca",
                    Font = new Font("Segoe UI", 10f),
                    ForeColor = Color.Gray,
                    Dock = DockStyle.Top,
                    Height = 30,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                _panelCaLamViec.Controls.Add(lblSelectEmployee);
                lblSelectEmployee.BringToFront();

                lblSelectEmployee.BringToFront();

                tableLayout.Controls.Add(_panelCaLamViec, 1, 0);

                // Thêm tableLayout vào panelPhanCa
                panelPhanCa.Controls.Add(tableLayout);

                // Load dữ liệu nhân viên
                LoadPhanCaNhanVien();

                // Load danh sách ca
                LoadDanhSachCa();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo panel phân ca: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPhanCaNhanVien(string searchKeyword = "")
        {
            try
            {
                if (_dgvPhanCaNhanVien == null) return;

                _dgvPhanCaNhanVien.Rows.Clear();

                DataTable dt = _nguoiDungBLL.LayDanhSachNhanVien();
                int currentChiNhanhId = Session.ChiNhanhId;

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string ten = row["TenNV"]?.ToString() ?? "";
                        string chucVu = row["ChucVu"]?.ToString() ?? "";
                        int chiNhanhId = 0;
                        if (row.Table.Columns.Contains("ChiNhanhId") && row["ChiNhanhId"] != DBNull.Value)
                        {
                            chiNhanhId = Convert.ToInt32(row["ChiNhanhId"]);
                        }

                        // Lọc theo chi nhánh hiện tại
                        if (currentChiNhanhId > 0 && chiNhanhId != currentChiNhanhId && chiNhanhId != 0)
                            continue;

                        // Lọc theo từ khóa tìm kiếm
                        if (!string.IsNullOrEmpty(searchKeyword))
                        {
                            string searchLower = searchKeyword.ToLower();
                            if (!ten.ToLower().Contains(searchLower) && !chucVu.ToLower().Contains(searchLower))
                                continue;
                        }

                        int nguoiDungId = Convert.ToInt32(row["NguoiDungId"]);
                        _dgvPhanCaNhanVien.Rows.Add(nguoiDungId, ten, chucVu);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách nhân viên phân ca: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachCa()
        {
            try
            {
                // Load danh sách ca từ database
                string query = "SELECT ca_id, ten_ca, gio_bd, gio_kt FROM dbo.ca ORDER BY gio_bd";
                DataTable dtCa = _dbHelper.GetDataTable(query);

                if (dtCa == null || dtCa.Rows.Count == 0)
                {
                    var lblNoCa = new Label
                    {
                        Text = "Chưa có dữ liệu ca làm việc",
                        Font = new Font("Segoe UI", 10f),
                        ForeColor = Color.Red,
                        AutoSize = true,
                        Location = new Point(15, 80)
                    };
                    _panelCaLamViec.Controls.Add(lblNoCa);
                    return;
                }

                _caCheckBoxes.Clear();
                int yPos = 80;

                foreach (DataRow row in dtCa.Rows)
                {
                    int caId = Convert.ToInt32(row["ca_id"]);
                    string tenCa = row["ten_ca"]?.ToString() ?? "";
                    TimeSpan gioBd = (TimeSpan)row["gio_bd"];
                    TimeSpan gioKt = (TimeSpan)row["gio_kt"];

                    // Tạo panel cho mỗi ca
                    var caPanel = new Panel
                    {
                        Location = new Point(0, yPos),
                        Size = new Size(_panelCaLamViec.Width - 30, 60),
                        BackColor = Color.FromArgb(248, 249, 250),
                        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
                    };
                    caPanel.Paint += (s, e) =>
                    {
                        using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
                        {
                            e.Graphics.DrawRectangle(pen, 0, 0, caPanel.Width - 1, caPanel.Height - 1);
                        }
                    };

                    var chk = new CheckBox
                    {
                        Name = $"chkCa_{caId}",
                        Tag = caId,
                        Text = $"{tenCa}",
                        Font = new Font("Segoe UI Semibold", 11f),
                        Location = new Point(15, 8),
                        AutoSize = true,
                        Enabled = false // Mặc định disable cho đến khi chọn nhân viên
                    };
                    chk.CheckedChanged += ChkCa_CheckedChanged;
                    _caCheckBoxes.Add(chk);
                    caPanel.Controls.Add(chk);

                    var lblTime = new Label
                    {
                        Text = $"⏰ {gioBd:hh\\:mm} - {gioKt:hh\\:mm}",
                        Font = new Font("Segoe UI", 9f),
                        ForeColor = Color.FromArgb(100, 100, 100),
                        Location = new Point(35, 32),
                        AutoSize = true
                    };
                    caPanel.Controls.Add(lblTime);

                    _panelCaLamViec.Controls.Add(caPanel);
                    yPos += 70;
                }

                // Nút Lưu
                var btnLuu = new Button
                {
                    Name = "btnLuuPhanCa",
                    Text = "💾 Lưu phân ca",
                    Font = new Font("Segoe UI Semibold", 10f),
                    Size = new Size(150, 40),
                    Location = new Point(15, yPos + 10),
                    BackColor = Color.FromArgb(34, 139, 34),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Enabled = false
                };
                btnLuu.FlatAppearance.BorderSize = 0;
                btnLuu.Click += BtnLuuPhanCa_Click;
                _panelCaLamViec.Controls.Add(btnLuu);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách ca: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvPhanCaNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (_dgvPhanCaNhanVien.SelectedRows.Count == 0)
                {
                    _selectedNguoiDungId = -1;
                    ResetCaCheckboxes();
                    return;
                }

                var row = _dgvPhanCaNhanVien.SelectedRows[0];
                _selectedNguoiDungId = Convert.ToInt32(row.Cells["NguoiDungId"].Value);
                string tenNV = row.Cells["TenNV"].Value?.ToString() ?? "";

                // Cập nhật label
                var lblSelect = _panelCaLamViec.Controls.Find("lblSelectEmployee", true).FirstOrDefault() as Label;
                if (lblSelect != null)
                {
                    lblSelect.Text = $"Đang chọn: {tenNV}";
                    lblSelect.ForeColor = Color.FromArgb(0, 100, 0);
                }

                // Enable các checkbox và nút lưu
                foreach (var chk in _caCheckBoxes)
                {
                    chk.Enabled = true;
                }
                var btnLuu = _panelCaLamViec.Controls.Find("btnLuuPhanCa", true).FirstOrDefault() as Button;
                if (btnLuu != null) btnLuu.Enabled = true;

                // Load ca đã đăng ký của nhân viên này
                LoadCaDaDangKy(_selectedNguoiDungId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi SelectionChanged: {ex.Message}");
            }
        }

        private void LoadCaDaDangKy(int nguoiDungId)
        {
            try
            {
                // Uncheck tất cả trước
                foreach (var chk in _caCheckBoxes)
                {
                    chk.CheckedChanged -= ChkCa_CheckedChanged;
                    chk.Checked = false;
                    chk.CheckedChanged += ChkCa_CheckedChanged;
                }

                // Lấy danh sách ca đã đăng ký
                int chiNhanhId = Session.ChiNhanhId;
                string query = @"SELECT ca_id FROM dbo.nguoi_dung_ca 
                                WHERE nguoi_dung_id = @nguoiDungId 
                                AND chi_nhanh_id = @chiNhanhId 
                                AND trang_thai = 1";

                var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@nguoiDungId", nguoiDungId),
                    new Microsoft.Data.SqlClient.SqlParameter("@chiNhanhId", chiNhanhId)
                };

                DataTable dt = _dbHelper.GetDataTable(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int caId = Convert.ToInt32(row["ca_id"]);
                        var chk = _caCheckBoxes.FirstOrDefault(c => (int)c.Tag == caId);
                        if (chk != null)
                        {
                            chk.CheckedChanged -= ChkCa_CheckedChanged;
                            chk.Checked = true;
                            chk.CheckedChanged += ChkCa_CheckedChanged;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi LoadCaDaDangKy: {ex.Message}");
            }
        }

        private void ResetCaCheckboxes()
        {
            foreach (var chk in _caCheckBoxes)
            {
                chk.Enabled = false;
                chk.CheckedChanged -= ChkCa_CheckedChanged;
                chk.Checked = false;
                chk.CheckedChanged += ChkCa_CheckedChanged;
            }

            var btnLuu = _panelCaLamViec?.Controls.Find("btnLuuPhanCa", true).FirstOrDefault() as Button;
            if (btnLuu != null) btnLuu.Enabled = false;

            var lblSelect = _panelCaLamViec?.Controls.Find("lblSelectEmployee", true).FirstOrDefault() as Label;
            if (lblSelect != null)
            {
                lblSelect.Text = "Vui lòng chọn nhân viên để xem/đăng ký ca";
                lblSelect.ForeColor = Color.Gray;
            }
        }

        private void ChkCa_CheckedChanged(object sender, EventArgs e)
        {
            // Có thể thêm logic preview ở đây nếu cần
        }

        private void BtnLuuPhanCa_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedNguoiDungId <= 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Vui lòng chọn chi nhánh!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xóa tất cả ca cũ của nhân viên này tại chi nhánh
                string deleteQuery = @"DELETE FROM dbo.nguoi_dung_ca 
                                      WHERE nguoi_dung_id = @nguoiDungId 
                                      AND chi_nhanh_id = @chiNhanhId";
                var deleteParams = new Microsoft.Data.SqlClient.SqlParameter[]
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@nguoiDungId", _selectedNguoiDungId),
                    new Microsoft.Data.SqlClient.SqlParameter("@chiNhanhId", chiNhanhId)
                };
                _dbHelper.ExecuteNonQuery(deleteQuery, deleteParams);

                // Thêm các ca được chọn
                foreach (var chk in _caCheckBoxes)
                {
                    if (chk.Checked)
                    {
                        int caId = (int)chk.Tag;
                        string insertQuery = @"INSERT INTO dbo.nguoi_dung_ca (nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
                                              VALUES (@nguoiDungId, @chiNhanhId, @caId, 1)";
                        var insertParams = new Microsoft.Data.SqlClient.SqlParameter[]
                        {
                            new Microsoft.Data.SqlClient.SqlParameter("@nguoiDungId", _selectedNguoiDungId),
                            new Microsoft.Data.SqlClient.SqlParameter("@chiNhanhId", chiNhanhId),
                            new Microsoft.Data.SqlClient.SqlParameter("@caId", caId)
                        };
                        _dbHelper.ExecuteNonQuery(insertQuery, insertParams);
                    }
                }

                MessageBox.Show("Lưu phân ca thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu phân ca: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvNhanSu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Kiểm tra xem có click vào row hợp lệ không
                if (e.RowIndex < 0 || e.RowIndex >= dgvNhanSu.Rows.Count)
                    return;

                DataGridViewRow row = dgvNhanSu.Rows[e.RowIndex];

                // Lấy nguoi_dung_id từ cột ẩn
                if (row.Cells[NS_ID].Value == null)
                    return;

                int nguoiDungId = Convert.ToInt32(row.Cells[NS_ID].Value);

                // Mở form sửa/xóa nhân viên
                using (var f = new Frm_SuaXoaNV(nguoiDungId, _nguoiDungBLL))
                {
                    f.StartPosition = FormStartPosition.CenterParent;
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        // Reload dữ liệu nhân viên sau khi sửa/xóa
                        LoadDataNhanSu();
                        LoadTongSoNhanVien();

                        // Reload dữ liệu phân ca nếu đang ở tab phân ca
                        if (segmentedPill1.SelectedIndex == 1 && _dgvPhanCaNhanVien != null)
                        {
                            LoadPhanCaNhanVien();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form sửa/xóa nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panelNhanSu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTimNVCa_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_dgvPhanCaNhanVien == null) return;

                // Lấy từ khóa tìm kiếm từ TextBox
                string searchKeyword = "";
                if (sender is TextBox textBox)
                {
                    searchKeyword = textBox.Text?.Trim() ?? "";
                }
                else if (sender is Guna.UI2.WinForms.Guna2TextBox guna2TextBox)
                {
                    searchKeyword = guna2TextBox.Text?.Trim() ?? "";
                }

                // Load lại danh sách với filter
                LoadPhanCaNhanVien(searchKeyword);

                // Reset selection khi tìm kiếm
                _selectedNguoiDungId = -1;
                ResetCaCheckboxes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi tìm kiếm nhân viên phân ca: {ex.Message}");
            }
        }
    }
}