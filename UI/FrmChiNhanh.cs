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
using BLL;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using UI.Common;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmChiNhanh : Form
    {
        private ChiNhanhBLL _chiNhanhBLL;
        private KhuVucBLL _khuVucBLL;
        private QLNhaHangTiecCuoi.BLL.BanBLL _banBLL;
        private QLNhaHangTiecCuoi.BLL.SanhBLL _sanhBLL;
        private List<ChiNhanhPanel> _listChiNhanhPanels = new List<ChiNhanhPanel>();
        private List<TinhTrangBan> _listBanPanels = new List<TinhTrangBan>();
        private List<SanhPanel> _listSanhPanels = new List<SanhPanel>();
        private System.Windows.Forms.Timer _searchTimer;
        private FlowLayoutPanel _flowLayoutSanh;

        public FrmChiNhanh()
        {
            InitializeComponent();
            var dbHelper = new DatabaseHelper();
            _chiNhanhBLL = new ChiNhanhBLL(dbHelper);
            _khuVucBLL = new KhuVucBLL();
            _banBLL = new QLNhaHangTiecCuoi.BLL.BanBLL(dbHelper);
            _sanhBLL = new QLNhaHangTiecCuoi.BLL.SanhBLL(dbHelper);
            btnThemChiNhanh.Click += btnThemChiNhanh_Click;
            InitializeComboBoxChiNhanh();

            // Khởi tạo timer cho tìm kiếm real-time (delay 300ms)
            _searchTimer = new System.Windows.Forms.Timer();
            _searchTimer.Interval = 300;
            _searchTimer.Tick += SearchTimer_Tick;

            // Đăng ký event handlers
            txtTimKiemChiNhanh.TextChanged += TxtTimKiemChiNhanh_TextChanged;
            cbbLocCN.SelectedIndexChanged += CbbLocCN_SelectedIndexChanged;

            // Đăng ký event handlers cho PanelTimKiemBan
            txtTimBan.TextChanged += TxtTimBan_TextChanged;
            cbbTrangThai.SelectedIndexChanged += CbbTrangThai_SelectedIndexChanged;
            cbbKhuVuc.SelectedIndexChanged += CbbKhuVuc_SelectedIndexChanged;
            btnThemBan.Click += BtnThemBan_Click;

            // Đăng ký event handler cho nút thêm khu vực
            btnThem.Click += BtnThemKhuVuc_Click;

            // Đăng ký event handler cho nút thêm sảnh
            btnThemSanh.Click += BtnThemSanh_Click;
        }
        private const string COL_TEN = "TenCN";
        private const string COL_DC = "DiaChi";
        private const string COL_DT = "DienThoai";
        private const string COL_TT = "TrangThai";
        private const string COL_TTAC = "ThaoTac";

        private void FrmChiNhanh_Load(object sender, EventArgs e)
        {
            LoadComboBoxLoc();
            LoadDanhSachChiNhanh();
        }

        private void LoadComboBoxLoc()
        {
            try
            {
                cbbLocCN.Items.Clear();
                cbbLocCN.Items.Add("Tất cả");
                cbbLocCN.Items.Add("Đang hoạt động");
                cbbLocCN.Items.Add("Bảo trì");
                cbbLocCN.SelectedIndex = 0; // Mặc định chọn "Tất cả"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load combo box: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadDanhSachChiNhanh()
        {
            LoadDanhSachChiNhanh(null, null);
        }

        private void LoadDanhSachChiNhanh(string searchKeyword, int? trangThai)
        {
            try
            {
                // Xóa các panel cũ
                foreach (var panel in _listChiNhanhPanels)
                {
                    panelChiNhanh.Controls.Remove(panel);
                    panel.Dispose();
                }
                _listChiNhanhPanels.Clear();
                panelChiNhanh.Controls.Clear();

                // Load dữ liệu từ database
                DataTable dt;

                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    // Tìm kiếm với keyword
                    dt = _chiNhanhBLL.TimKiemChiNhanh(searchKeyword, trangThai);
                }
                else if (trangThai.HasValue)
                {
                    // Chỉ lọc theo trạng thái
                    dt = _chiNhanhBLL.LayChiNhanhTheoTrangThai(trangThai);
                }
                else
                {
                    // Lấy tất cả
                    dt = _chiNhanhBLL.LayTatCaChiNhanh();
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Kích thước thực của ChiNhanhPanel
                    const int panelWidth = 600;
                    const int panelHeight = 454;
                    const int spacing = 15; // Giảm spacing để sát nhau hơn
                    const int margin = 10; // Margin nhỏ hơn

                    int x = margin;
                    int y = margin;
                    int availableWidth = panelChiNhanh.Width - (margin * 2);

                    // Tính số cột có thể chứa (có thể chứa bao nhiêu panel ngang)
                    int colsPerRow = (availableWidth + spacing) / (panelWidth + spacing);
                    if (colsPerRow < 1) colsPerRow = 1;

                    foreach (DataRow row in dt.Rows)
                    {
                        var chiNhanhPanel = new ChiNhanhPanel();
                        chiNhanhPanel.Location = new Point(x, y);
                        chiNhanhPanel.Size = new Size(panelWidth, panelHeight);

                        int chiNhanhId = Convert.ToInt32(row["chi_nhanh_id"]);
                        string ten = row["ten"].ToString();
                        string diaChi = row["dia_chi"]?.ToString() ?? "";
                        string sdt = row["sdt"]?.ToString() ?? "";
                        int trangThaiValue = Convert.ToInt32(row["trang_thai"]);

                        chiNhanhPanel.LoadChiNhanh(chiNhanhId, ten, diaChi, sdt, trangThaiValue);

                        // Đăng ký event để reload khi chi nhánh được cập nhật
                        chiNhanhPanel.ChiNhanhUpdated += (s, args) =>
                        {
                            // Reload danh sách chi nhánh với filter hiện tại
                            string currentSearch = txtTimKiemChiNhanh.Text.Trim();
                            int? currentTrangThai = GetCurrentTrangThai();
                            LoadDanhSachChiNhanh(currentSearch, currentTrangThai);
                        };

                        panelChiNhanh.Controls.Add(chiNhanhPanel);
                        _listChiNhanhPanels.Add(chiNhanhPanel);

                        // Tính toán vị trí cho panel tiếp theo (layout grid)
                        x += panelWidth + spacing;

                        // Nếu hết chỗ ngang, xuống dòng mới
                        if (x + panelWidth > panelChiNhanh.Width - margin)
                        {
                            x = margin;
                            y += panelHeight + spacing;
                        }
                    }
                }

                // Bật AutoScroll cho panel
                panelChiNhanh.AutoScroll = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtTimKiemChiNhanh_TextChanged(object sender, EventArgs e)
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

        private void CbbLocCN_SelectedIndexChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private int? GetCurrentTrangThai()
        {
            int? trangThai = null;

            // Lấy trạng thái từ combo box
            if (cbbLocCN.SelectedIndex >= 0)
            {
                string selectedText = cbbLocCN.SelectedItem?.ToString();
                if (selectedText == "Đang hoạt động")
                {
                    trangThai = 1;
                }
                else if (selectedText == "Bảo trì")
                {
                    trangThai = 0; // Giả sử 0 là bảo trì, nếu khác thì cần điều chỉnh
                }
                // "Tất cả" thì trangThai = null
            }

            return trangThai;
        }

        private void PerformSearch()
        {
            try
            {
                string searchKeyword = txtTimKiemChiNhanh.Text.Trim();
                int? trangThai = GetCurrentTrangThai();

                // Load danh sách với filter
                LoadDanhSachChiNhanh(searchKeyword, trangThai);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnThemChiNhanh_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_ThemChiNhanh())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ChiNhanhAdded += (s, args) =>
                {
                    // Reload danh sách khi thêm chi nhánh mới, giữ lại filter hiện tại
                    string currentSearch = txtTimKiemChiNhanh.Text.Trim();
                    int? currentTrangThai = GetCurrentTrangThai();
                    LoadDanhSachChiNhanh(currentSearch, currentTrangThai);
                };
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    // Đảm bảo reload ngay cả khi form đóng bằng DialogResult.OK
                    string currentSearch = txtTimKiemChiNhanh.Text.Trim();
                    int? currentTrangThai = GetCurrentTrangThai();
                    LoadDanhSachChiNhanh(currentSearch, currentTrangThai);
                }
            }
        }

        private void InitializeComboBoxChiNhanh()
        {
            // Tạo Label để hiển thị chi nhánh hiện tại (từ session)
            Label lblChiNhanh = new Label();
            lblChiNhanh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblChiNhanh.Location = new Point(12, 50);
            lblChiNhanh.Size = new Size(400, 30);
            lblChiNhanh.Text = $"Chi nhánh: {Session.TenChiNhanh}";
            lblChiNhanh.AutoSize = true;
            panelKhuVuc.Controls.Add(lblChiNhanh);
        }

        private void LoadDanhSachKhuVuc()
        {
            // Lấy chi_nhanh_id từ session đăng nhập
            int? chiNhanhId = null;
            if (Session.ChiNhanhId > 0)
            {
                chiNhanhId = Session.ChiNhanhId;
            }
            LoadDanhSachKhuVuc(chiNhanhId);
        }

        private void LoadDanhSachKhuVuc(int? chiNhanhId)
        {
            try
            {
                // Lấy khu vực theo chi nhánh (null = tất cả)
                DataTable dt = _khuVucBLL.LayDanhSachKhuVucVoiSoBan(chiNhanhId);

                // Clear và setup DataGridView
                dgvKhuVuc.AutoGenerateColumns = false;
                dgvKhuVuc.DataSource = null;
                dgvKhuVuc.Columns.Clear();

                // Tắt cột thừa đầu tiên (RowHeadersVisible)
                dgvKhuVuc.RowHeadersVisible = false;

                // Thêm cột ID nhưng ẩn đi (để có thể lấy ID nếu cần)
                dgvKhuVuc.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "khu_vuc_id",
                    HeaderText = "ID",
                    DataPropertyName = "khu_vuc_id",
                    Visible = false, // Ẩn cột ID
                    ReadOnly = true
                });

                // Cột Tên khu vực
                dgvKhuVuc.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "ten_khu_vuc",
                    HeaderText = "Tên khu vực",
                    DataPropertyName = "ten_khu_vuc",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 150, // Tăng trọng số để cột này rộng hơn
                    ReadOnly = true
                });

                // Cột Số bàn - giữ độ rộng cố định
                dgvKhuVuc.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "so_ban_text",
                    HeaderText = "Số bàn",
                    DataPropertyName = "so_ban_text",
                    Width = 150,
                    MinimumWidth = 120,
                    ReadOnly = true
                });

                // Cột Mô tả - mở rộng để hiển thị đầy đủ
                dgvKhuVuc.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "mo_ta",
                    HeaderText = "Mô tả",
                    DataPropertyName = "mo_ta",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    FillWeight = 250, // Tăng trọng số để cột Mô tả rộng hơn
                    ReadOnly = true
                });

                // Tăng chiều cao dòng để có thêm không gian
                dgvKhuVuc.RowTemplate.Height = 50;
                dgvKhuVuc.DefaultCellStyle.Padding = new Padding(8, 8, 8, 8);

                // Cho phép wrap text trong các ô để hiển thị đầy đủ mô tả
                dgvKhuVuc.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // Đăng ký event double click
                dgvKhuVuc.CellDoubleClick -= dgvKhuVuc_CellDoubleClick;
                dgvKhuVuc.CellDoubleClick += dgvKhuVuc_CellDoubleClick;

                // Bind data
                dgvKhuVuc.DataSource = dt;
                dgvKhuVuc.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void segmentedPill1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (segmentedPill1.SelectedIndex == 0)
            {
                // Hiển thị danh sách hóa đơn và thanh toán
                panelChiNhanh.Visible = true;
                PanelTimKiemChiNhanh.Visible = true;
                panelKhuVuc.Visible = false;
                panelBan.Visible = false;
                panelSanh.Visible = false;
            }
            else if (segmentedPill1.SelectedIndex == 1)
            {
                // Hiển thị danh sách khu vực
                panelChiNhanh.Visible = false;
                PanelTimKiemChiNhanh.Visible = false;
                panelKhuVuc.Visible = true;
                panelBan.Visible = false;
                panelSanh.Visible = false;
                // Load dữ liệu khu vực theo chi_nhanh_id từ session đăng nhập
                LoadDanhSachKhuVuc();
            }
            else if (segmentedPill1.SelectedIndex == 2)
            {
                // Hiển thị danh sách bàn, ẩn các panel khác
                panelChiNhanh.Visible = false;
                PanelTimKiemChiNhanh.Visible = false;
                panelKhuVuc.Visible = false;
                panelBan.Visible = true;
                panelSanh.Visible = false;
                // Load combo box và danh sách bàn
                LoadComboBoxBan();
                FilterDanhSachBan();
                // Load sơ đồ bàn vào PanelSoDoBan (bên phải)
                LoadSoDoBan();
            }
            else if (segmentedPill1.SelectedIndex == 3)
            {
                // Hiển thị danh sách sảnh, ẩn các panel khác
                panelChiNhanh.Visible = false;
                PanelTimKiemChiNhanh.Visible = false;
                panelKhuVuc.Visible = false;
                panelBan.Visible = false;
                panelSanh.Visible = true;
                panelTimKiemSanh.Visible = true;

                // Load danh sách sảnh
                LoadDanhSachSanh();
            }
        }

        private void dgvKhuVuc_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return; // Không click vào header

                DataGridViewRow row = dgvKhuVuc.Rows[e.RowIndex];

                // Lấy khu_vuc_id từ cột ẩn
                int khuVucId = Convert.ToInt32(row.Cells["khu_vuc_id"].Value);

                // Mở form chi tiết khu vực
                using (var f = new Frm_ChiTietKhuVuc(khuVucId))
                {
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.KhuVucUpdated += (s, args) =>
                    {
                        // Reload danh sách khu vực khi có thay đổi
                        LoadDanhSachKhuVuc();
                    };
                    f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở chi tiết khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSoDoBan()
        {
            try
            {
                // Xóa tất cả controls cũ (bao gồm cả panels và labels)
                PanelSoDoBan.Controls.Clear();
                _listBanPanels.Clear();

                // Reset label phân chia khu vực
                lblPhanChiaKhuVuc.Text = "";
                lblPhanChiaKhuVuc.Visible = false;

                // Lấy chi_nhanh_id từ session
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin chi nhánh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy khu vực được chọn từ combo box (nếu có)
                int? khuVucIdFilter = null;
                if (cbbKhuVuc.SelectedIndex > 0)
                {
                    string tenKhuVuc = cbbKhuVuc.SelectedItem.ToString();
                    DataTable dtKhuVuc = _banBLL.LayDanhSachKhuVucTheoChiNhanh(chiNhanhId);
                    if (dtKhuVuc != null)
                    {
                        var row = dtKhuVuc.AsEnumerable()
                            .FirstOrDefault(r => r["ten_khu_vuc"].ToString() == tenKhuVuc);
                        if (row != null)
                        {
                            khuVucIdFilter = Convert.ToInt32(row["khu_vuc_id"]);
                        }
                    }
                }

                // Load dữ liệu bàn từ database - filter theo khu vực nếu có
                DataTable dtBan = _banBLL.LayDanhSachBanTheoChiNhanh(chiNhanhId, khuVucIdFilter);
                if (dtBan == null || dtBan.Rows.Count == 0)
                {
                    return;
                }

                // Đảm bảo chỉ hiển thị bàn của chi_nhanh_id hiện tại (double check)
                var filteredRows = dtBan.AsEnumerable()
                    .Where(r => r["chi_nhanh_id"] != DBNull.Value && Convert.ToInt32(r["chi_nhanh_id"]) == chiNhanhId);

                if (!filteredRows.Any())
                {
                    return;
                }

                dtBan = filteredRows.CopyToDataTable();

                // Kích thước và spacing
                const int cardWidth = 200;
                const int cardHeight = 140;
                const int margin = 10;
                const int spacing = 15;
                const int labelHeight = 30;
                const int labelMargin = 20;

                int maxWidth = PanelSoDoBan.Width - (margin * 2);
                int columnsPerRow = (maxWidth + spacing) / (cardWidth + spacing);
                if (columnsPerRow < 1) columnsPerRow = 1;

                // Tính toán spacing đều giữa các cột
                int totalCardWidth = columnsPerRow * cardWidth;
                int totalSpacing = maxWidth - totalCardWidth;
                int spacingBetweenCards = columnsPerRow > 1 ? totalSpacing / (columnsPerRow - 1) : 0;

                int currentX = margin;
                int currentY = margin; // Bắt đầu từ đầu
                int columnIndex = 0; // Index của cột hiện tại trong hàng

                int? currentKhuVucId = null;
                string currentKhuVucTen = "";
                bool isFirstKhuVuc = true;

                foreach (DataRow row in dtBan.Rows)
                {
                    int? khuVucId = row["khu_vuc_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["khu_vuc_id"]);
                    string khuVucTen = row["ten_khu_vuc"]?.ToString() ?? "Chưa phân khu vực";

                    // Nếu qua khu vực mới, tạo label và xuống dòng mới
                    if (khuVucId != currentKhuVucId)
                    {
                        currentKhuVucId = khuVucId;
                        currentKhuVucTen = khuVucTen;

                        // Sử dụng label có sẵn cho khu vực đầu tiên, tạo mới cho các khu vực tiếp theo
                        if (isFirstKhuVuc)
                        {
                            lblPhanChiaKhuVuc.Text = currentKhuVucTen;
                            lblPhanChiaKhuVuc.Location = new Point(margin, margin);
                            lblPhanChiaKhuVuc.Visible = true;
                            PanelSoDoBan.Controls.Add(lblPhanChiaKhuVuc);
                            isFirstKhuVuc = false;
                            currentY = margin + labelHeight + labelMargin;
                        }
                        else
                        {
                            // Tạo label khu vực mới
                            Label newKhuVucLabel = new Label
                            {
                                Text = currentKhuVucTen,
                                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                                AutoSize = true,
                                Location = new Point(margin, currentY),
                                ForeColor = Color.FromArgb(17, 24, 39),
                                Name = $"lblKhuVuc_{khuVucId ?? 0}"
                            };
                            PanelSoDoBan.Controls.Add(newKhuVucLabel);
                            currentY += labelHeight + spacing;
                        }

                        // Reset về đầu hàng khi qua khu vực mới
                        currentX = margin;
                        columnIndex = 0;
                    }

                    // Tính toán vị trí X chính xác dựa trên cột
                    currentX = margin + columnIndex * (cardWidth + spacingBetweenCards);

                    // Tạo BanPanel
                    TinhTrangBan banPanel = CreateBanPanel(row);
                    banPanel.Size = new Size(cardWidth, cardHeight);
                    banPanel.Location = new Point(currentX, currentY);

                    PanelSoDoBan.Controls.Add(banPanel);
                    _listBanPanels.Add(banPanel);

                    // Tăng chỉ số cột
                    columnIndex++;

                    // Nếu hết chỗ ngang, xuống dòng mới
                    if (columnIndex >= columnsPerRow)
                    {
                        columnIndex = 0;
                        currentX = margin;
                        currentY += cardHeight + spacing;
                    }
                }

                // Refresh panel để đảm bảo hiển thị đúng
                PanelSoDoBan.Refresh();
                PanelSoDoBan.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load sơ đồ bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TinhTrangBan CreateBanPanel(DataRow row)
        {
            try
            {
                int banId = row["ban_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["ban_id"]);
                string soBan = row["so_ban"]?.ToString() ?? "";
                string trangThai = row["trang_thai"]?.ToString() ?? "";

                TinhTrangBan banPanel = new TinhTrangBan
                {
                    TableCode = soBan,
                    Capacity = row["suc_chua"] == DBNull.Value ? 0 : Convert.ToInt32(row["suc_chua"]),
                    Status = GetTableStateFromString(trangThai),
                    CornerRadius = 20,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(17, 24, 39),
                };

                // Lưu thông tin bàn vào Tag để sử dụng khi click (lưu banId, soBan, trangThai)
                banPanel.Tag = new { BanId = banId, SoBan = soBan, TrangThai = trangThai };

                // Thêm event handler cho click
                banPanel.Click += BanPanel_Click;
                banPanel.Cursor = Cursors.Hand;

                return banPanel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo panel bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new TinhTrangBan();
            }
        }

        private void BanPanel_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is TinhTrangBan banPanel && banPanel.Tag != null)
                {
                    dynamic banInfo = banPanel.Tag;
                    int banId = banInfo.BanId;
                    string soBan = banInfo.SoBan;
                    string trangThai = banInfo.TrangThai;

                    if (banId > 0)
                    {
                        // Mở form ở chế độ chỉ xem (không cho sửa) - từ PanelSoDoBan
                        using (var frm = new Frm_ThongTinBan(banId, soBan, trangThai, _banBLL, allowEdit: false))
                        {
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.ShowDialog(this);
                            // Không cần reload vì không có thay đổi
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở thông tin bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachBan()
        {
            try
            {
                // Xóa các control cũ
                PanelDanhSachBan.Controls.Clear();

                // Lấy chi_nhanh_id từ session
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin chi nhánh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Load dữ liệu bàn từ database
                DataTable dtBan = _banBLL.LayDanhSachBanTheoChiNhanh(chiNhanhId, null);
                if (dtBan == null || dtBan.Rows.Count == 0)
                {
                    Label lblNoData = new Label
                    {
                        Text = "Chưa có bàn nào",
                        Font = new Font("Segoe UI", 12F),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };
                    PanelDanhSachBan.Controls.Add(lblNoData);
                    return;
                }

                // Đảm bảo chỉ hiển thị bàn của chi_nhanh_id hiện tại
                var filteredRows = dtBan.AsEnumerable()
                    .Where(r => r["chi_nhanh_id"] != DBNull.Value && Convert.ToInt32(r["chi_nhanh_id"]) == chiNhanhId);

                if (!filteredRows.Any())
                {
                    return;
                }

                dtBan = filteredRows.CopyToDataTable();

                // Tạo các card bàn
                int yPos = 10;
                const int cardHeight = 80;
                const int spacing = 10;

                foreach (DataRow row in dtBan.Rows)
                {
                    BanCard banCard = CreateBanCard(row);
                    banCard.Location = new Point(10, yPos);
                    banCard.Width = PanelDanhSachBan.Width - 30;
                    banCard.Height = cardHeight;

                    PanelDanhSachBan.Controls.Add(banCard);
                    yPos += cardHeight + spacing;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private BanCard CreateBanCard(DataRow row)
        {
            try
            {
                int banId = row["ban_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["ban_id"]);
                string soBan = row["so_ban"]?.ToString() ?? "";
                int sucChua = row["suc_chua"] == DBNull.Value ? 0 : Convert.ToInt32(row["suc_chua"]);
                string trangThai = row["trang_thai"]?.ToString() ?? "TRỐNG";
                string tenKhuVuc = row["ten_khu_vuc"]?.ToString() ?? "Chưa phân khu vực";

                BanCard banCard = new BanCard
                {
                    SoBan = soBan,
                    KhuVuc = tenKhuVuc,
                    SucChua = sucChua,
                    TrangThai = trangThai
                };

                // Lưu thông tin bàn vào Tag
                banCard.Tag = new { BanId = banId, SoBan = soBan, TrangThai = trangThai };

                // Thêm event handler cho click
                banCard.Click += BanCard_Click;

                return banCard;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo card bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new BanCard();
            }
        }

        private void BanCard_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is BanCard banCard && banCard.Tag != null)
                {
                    dynamic banInfo = banCard.Tag;
                    int banId = banInfo.BanId;
                    string soBan = banInfo.SoBan;
                    string trangThai = banInfo.TrangThai;

                    if (banId > 0)
                    {
                        // Mở form ở chế độ cho phép sửa - từ PanelDanhSachBan
                        using (var frm = new Frm_ThongTinBan(banId, soBan, trangThai, _banBLL, allowEdit: true))
                        {
                            frm.StartPosition = FormStartPosition.CenterParent;
                            if (frm.ShowDialog(this) == DialogResult.OK)
                            {
                                // Reload lại cả danh sách bàn và sơ đồ bàn sau khi có thay đổi
                                FilterDanhSachBan();
                                LoadSoDoBan();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở thông tin bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TinhTrangBan.TableState GetTableStateFromString(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
                return TinhTrangBan.TableState.Available;

            switch (trangThai.ToUpper())
            {
                case "TRỐNG":
                    return TinhTrangBan.TableState.Available;
                case "PHỤC VỤ":
                    return TinhTrangBan.TableState.InUse;
                case "ĐÃ ĐẶT":
                    return TinhTrangBan.TableState.Reserved;
                case "VỆ SINH":
                    return TinhTrangBan.TableState.Cleaning;
                default:
                    return TinhTrangBan.TableState.Available;
            }
        }

        private void lblPhanChiaKhuVuc_Click(object sender, EventArgs e)
        {

        }

        private void LoadComboBoxBan()
        {
            try
            {
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    return;
                }

                // Load cbbTrangThai
                cbbTrangThai.Items.Clear();
                cbbTrangThai.Items.Add("Tất cả");
                cbbTrangThai.Items.Add("TRỐNG");
                cbbTrangThai.Items.Add("PHỤC VỤ");
                cbbTrangThai.Items.Add("ĐÃ ĐẶT");
                cbbTrangThai.Items.Add("VỆ SINH");
                cbbTrangThai.SelectedIndex = 0;

                // Load cbbKhuVuc
                cbbKhuVuc.Items.Clear();
                cbbKhuVuc.Items.Add("Tất cả");
                DataTable dtKhuVuc = _banBLL.LayDanhSachKhuVucTheoChiNhanh(chiNhanhId);
                if (dtKhuVuc != null && dtKhuVuc.Rows.Count > 0)
                {
                    foreach (DataRow row in dtKhuVuc.Rows)
                    {
                        cbbKhuVuc.Items.Add(row["ten_khu_vuc"].ToString());
                    }
                }
                cbbKhuVuc.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load combo box: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterDanhSachBan()
        {
            try
            {
                // Xóa các control cũ
                PanelDanhSachBan.Controls.Clear();

                // Lấy chi_nhanh_id từ session
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin chi nhánh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy điều kiện lọc
                string searchKeyword = txtTimBan.Text.Trim();
                string trangThaiFilter = null;
                int? khuVucIdFilter = null;

                // Lọc theo trạng thái
                if (cbbTrangThai.SelectedIndex > 0)
                {
                    trangThaiFilter = cbbTrangThai.SelectedItem.ToString();
                }

                // Lọc theo khu vực
                if (cbbKhuVuc.SelectedIndex > 0)
                {
                    string tenKhuVuc = cbbKhuVuc.SelectedItem.ToString();
                    DataTable dtKhuVuc = _banBLL.LayDanhSachKhuVucTheoChiNhanh(chiNhanhId);
                    if (dtKhuVuc != null)
                    {
                        var row = dtKhuVuc.AsEnumerable()
                            .FirstOrDefault(r => r["ten_khu_vuc"].ToString() == tenKhuVuc);
                        if (row != null)
                        {
                            khuVucIdFilter = Convert.ToInt32(row["khu_vuc_id"]);
                        }
                    }
                }

                // Load dữ liệu bàn từ database
                DataTable dtBan = _banBLL.LayDanhSachBanTheoChiNhanh(chiNhanhId, khuVucIdFilter);
                if (dtBan == null || dtBan.Rows.Count == 0)
                {
                    Label lblNoData = new Label
                    {
                        Text = "Chưa có bàn nào",
                        Font = new Font("Segoe UI", 12F),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };
                    PanelDanhSachBan.Controls.Add(lblNoData);
                    return;
                }

                // Đảm bảo chỉ hiển thị bàn của chi_nhanh_id hiện tại
                var filteredRows = dtBan.AsEnumerable()
                    .Where(r => r["chi_nhanh_id"] != DBNull.Value && Convert.ToInt32(r["chi_nhanh_id"]) == chiNhanhId);

                // Lọc theo số bàn (tìm kiếm)
                if (!string.IsNullOrWhiteSpace(searchKeyword))
                {
                    filteredRows = filteredRows.Where(r =>
                        r["so_ban"] != DBNull.Value &&
                        r["so_ban"].ToString().ToLower().Contains(searchKeyword.ToLower()));
                }

                // Lọc theo trạng thái
                if (!string.IsNullOrWhiteSpace(trangThaiFilter))
                {
                    filteredRows = filteredRows.Where(r =>
                        r["trang_thai"] != DBNull.Value &&
                        r["trang_thai"].ToString() == trangThaiFilter);
                }

                if (!filteredRows.Any())
                {
                    Label lblNoData = new Label
                    {
                        Text = "Không tìm thấy bàn nào",
                        Font = new Font("Segoe UI", 12F),
                        ForeColor = Color.Gray,
                        AutoSize = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };
                    PanelDanhSachBan.Controls.Add(lblNoData);
                    return;
                }

                dtBan = filteredRows.CopyToDataTable();

                // Tạo các card bàn
                int yPos = 10;
                const int cardHeight = 80;
                const int spacing = 10;

                foreach (DataRow row in dtBan.Rows)
                {
                    BanCard banCard = CreateBanCard(row);
                    banCard.Location = new Point(10, yPos);
                    banCard.Width = PanelDanhSachBan.Width - 30;
                    banCard.Height = cardHeight;

                    PanelDanhSachBan.Controls.Add(banCard);
                    yPos += cardHeight + spacing;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lọc danh sách bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtTimBan_TextChanged(object sender, EventArgs e)
        {
            FilterDanhSachBan();
        }

        private void CbbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDanhSachBan();
        }

        private void CbbKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterDanhSachBan();
            LoadSoDoBan(); // Reload sơ đồ bàn khi đổi khu vực
        }

        private void BtnThemBan_Click(object sender, EventArgs e)
        {
            try
            {
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin chi nhánh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở form thêm bàn mới
                using (var frm = new Frm_ThemBan(chiNhanhId, _banBLL))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        // Reload lại danh sách bàn và sơ đồ bàn sau khi thêm
                        LoadComboBoxBan();
                        FilterDanhSachBan();
                        LoadSoDoBan();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThemKhuVuc_Click(object sender, EventArgs e)
        {
            try
            {
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin chi nhánh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở form thêm khu vực mới
                using (var frm = new Frm_ThemKhuVuc(chiNhanhId))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.KhuVucAdded += (s, args) =>
                    {
                        // Reload danh sách khu vực khi có thay đổi
                        LoadDanhSachKhuVuc();
                    };

                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        // Đảm bảo reload ngay cả khi form đóng bằng DialogResult.OK
                        LoadDanhSachKhuVuc();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThemSanh_Click(object sender, EventArgs e)
        {
            try
            {
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin chi nhánh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở form thêm sảnh mới
                using (var frm = new Frm_ThemSanh(chiNhanhId))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.SanhAdded += (s, args) =>
                    {
                        // Reload danh sách sảnh khi có thay đổi
                        LoadDanhSachSanh();
                    };

                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        // Đảm bảo reload ngay cả khi form đóng bằng DialogResult.OK
                        LoadDanhSachSanh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachSanh()
        {
            try
            {
                // Xóa các panel cũ
                foreach (var panel in _listSanhPanels)
                {
                    if (panel.Parent != null)
                        panel.Parent.Controls.Remove(panel);
                    panel.Dispose();
                }
                _listSanhPanels.Clear();

                // Xóa các panel sảnh cũ và FlowLayoutPanel
                if (_flowLayoutSanh != null)
                {
                    if (_flowLayoutSanh.Parent != null)
                        _flowLayoutSanh.Parent.Controls.Remove(_flowLayoutSanh);
                    _flowLayoutSanh.Dispose();
                    _flowLayoutSanh = null;
                }

                // Xóa chỉ FlowLayoutPanel và các SanhPanel, giữ lại panelTimKiemSanh
                var controlsToRemove = new List<Control>();
                foreach (Control ctrl in panelSanh.Controls)
                {
                    // Giữ lại panelTimKiemSanh, xóa các control khác
                    if (ctrl != panelTimKiemSanh && ctrl.Name != "panelTimKiemSanh")
                    {
                        controlsToRemove.Add(ctrl);
                    }
                }
                foreach (var ctrl in controlsToRemove)
                {
                    panelSanh.Controls.Remove(ctrl);
                    if (ctrl != _flowLayoutSanh) // Đã dispose ở trên
                        ctrl.Dispose();
                }

                // Đảm bảo panelSanh có kích thước và hiển thị
                if (panelSanh.Width <= 0 || panelSanh.Height <= 0)
                {
                    System.Diagnostics.Debug.WriteLine($"panelSanh size: {panelSanh.Width}x{panelSanh.Height}");
                }

                // Đảm bảo panelTimKiemSanh có Dock = Top
                if (panelTimKiemSanh != null)
                {
                    panelTimKiemSanh.Dock = DockStyle.Top;
                    panelTimKiemSanh.BringToFront();
                }

                // Khởi tạo FlowLayoutPanel mới với Dock = Fill để fill phần còn lại
                _flowLayoutSanh = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = true,
                    Padding = new Padding(20, 20, 20, 20),
                    BackColor = Color.FromArgb(249, 250, 251),
                    Margin = new Padding(0)
                };
                panelSanh.Controls.Add(_flowLayoutSanh);

                // Đảm bảo FlowLayoutPanel được hiển thị
                _flowLayoutSanh.Visible = true;
                _flowLayoutSanh.BringToFront();
                if (panelTimKiemSanh != null)
                    panelTimKiemSanh.BringToFront();

                // Lấy chi_nhanh_id từ session
                int chiNhanhId = Session.ChiNhanhId;
                if (chiNhanhId <= 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin chi nhánh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Load dữ liệu sảnh từ database
                DataTable dtSanh = null;
                try
                {
                    dtSanh = _sanhBLL.LayDanhSachSanhTheoChiNhanh(chiNhanhId);
                    System.Diagnostics.Debug.WriteLine($"LoadDanhSachSanh: chiNhanhId = {chiNhanhId}, dtSanh = {(dtSanh == null ? "null" : dtSanh.Rows.Count.ToString())}");
                }
                catch (Exception exLoad)
                {
                    MessageBox.Show($"Lỗi khi load dữ liệu sảnh: {exLoad.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (dtSanh == null || dtSanh.Rows.Count == 0)
                {
                    // Hiển thị thông báo không có sảnh
                    Label lblNoData = new Label
                    {
                        Text = "Chưa có sảnh nào trong chi nhánh này.",
                        Font = new Font("Segoe UI", 12f),
                        ForeColor = Color.Gray,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        AutoSize = false
                    };
                    _flowLayoutSanh.Controls.Add(lblNoData);
                    System.Diagnostics.Debug.WriteLine("LoadDanhSachSanh: Không có dữ liệu sảnh");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"LoadDanhSachSanh: Tìm thấy {dtSanh.Rows.Count} sảnh");

                // Tạo SanhPanel cho mỗi sảnh
                foreach (DataRow row in dtSanh.Rows)
                {
                    try
                    {
                        int sanhId = Convert.ToInt32(row["sanh_id"]);
                        string tenSanh = row["ten_sanh"].ToString();
                        string tenChiNhanh = row["ten_chi_nhanh"] != DBNull.Value ? row["ten_chi_nhanh"].ToString() : "";
                        int sucChua = Convert.ToInt32(row["suc_chua"]);
                        decimal phiThueCb = Convert.ToDecimal(row["phi_thue_cb"]);

                        // Tạo SanhPanel
                        SanhPanel sanhPanel = new SanhPanel
                        {
                            Size = new Size(370, 240),
                            Margin = new Padding(12, 12, 12, 12)
                        };

                        // Load dữ liệu vào panel
                        sanhPanel.LoadData(sanhId, tenSanh, tenChiNhanh, sucChua, phiThueCb);

                        // Đăng ký event handlers
                        int sanhIdCopy = sanhId; // Copy để tránh closure issue
                        sanhPanel.ChiTietClicked += (s, e) =>
                        {
                            // Mở form chi tiết sảnh (chỉ xem, không cho phép sửa)
                            try
                            {
                                using (var frm = new Frm_ChiTietSanh(sanhIdCopy, _sanhBLL, allowEdit: false))
                                {
                                    frm.StartPosition = FormStartPosition.CenterParent;
                                    frm.ShowDialog(this);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi mở form chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };

                        sanhPanel.SuaClicked += (s, e) =>
                        {
                            // Mở form sửa sảnh (cho phép chỉnh sửa)
                            try
                            {
                                using (var frm = new Frm_ChiTietSanh(sanhIdCopy, _sanhBLL, allowEdit: true))
                                {
                                    frm.StartPosition = FormStartPosition.CenterParent;
                                    frm.SanhDeleted += (s2, e2) =>
                                    {
                                        // Reload danh sách sảnh sau khi xóa thành công
                                        LoadDanhSachSanh();
                                    };

                                    if (frm.ShowDialog(this) == DialogResult.OK)
                                    {
                                        // Reload danh sách sảnh sau khi sửa hoặc xóa thành công
                                        LoadDanhSachSanh();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi mở form sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };

                        // Thêm vào danh sách và FlowLayoutPanel
                        _listSanhPanels.Add(sanhPanel);
                        _flowLayoutSanh.Controls.Add(sanhPanel);
                    }
                    catch (Exception exRow)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo SanhPanel: {exRow.Message}");
                        continue;
                    }
                }

                // Refresh để đảm bảo hiển thị
                _flowLayoutSanh.Refresh();
                panelSanh.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách sảnh: {ex.Message}\n\nChi tiết: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
