using BLL;
using NLog.Filters;
using QLNhaHangTiecCuoi.BLL;
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
using Timer = System.Windows.Forms.Timer;


namespace UI
{

    [SupportedOSPlatform("windows")]
    public partial class FrmKho : Form
    {

        
        private readonly NguyenLieuBLL _bll;
        private readonly DatabaseHelper _dbHelper;
        private readonly Timer _debounceTimer = new Timer();
        private const int DebounceMs = 300;
        private const decimal NguongCanhBaoMacDinh = 30;
        List<string> filters = new List<string>();
        private int _selectedBranchId = Session.ChiNhanhId;
        private string _currentSearchText = string.Empty;

        public FrmKho(DatabaseHelper dbHelper)
        {
            InitializeComponent();
            _dbHelper = dbHelper;
            _bll = new NguyenLieuBLL(dbHelper);
            dgvKho.CellContentClick += dgvKho_CellContentClick;
            InitGrid();
            InitSearchRealtime();
            WireEvents();
        }


        private void roundedButton1_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_NhapKho())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                var result = f.ShowDialog(this);
                
                if (result == DialogResult.OK)
                {
                    ReloadGrid();
                    UpdateSummaryPanels();
                }
            }
        }



        public FrmKho() : this(new DatabaseHelper()) { }
        private const string ColActionName = "colChiTiet";
        private bool _gridColumnsBuilt = false;
        private void FrmKho_Load(object sender, EventArgs e)
        {
            LoadBranchCombo();
            LoadTinhTrangCombo();
            ReloadGrid();
            UpdateSummaryPanels();
        }
        private void InitGrid()
        {
            if (_gridColumnsBuilt) return;
            _gridColumnsBuilt = true;

            dgvKho.AutoGenerateColumns = false;
            dgvKho.MultiSelect = false;
            dgvKho.RowHeadersVisible = false;

            dgvKho.Columns.Clear();
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "nl_id", Name = "nl_id", Visible = false });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ma_nl", Name = "ma_nl", HeaderText = "Mã NL", Width = 110 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ten_nl", Name = "ten_nl", HeaderText = "Tên nguyên liệu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "don_vi", Name = "don_vi", HeaderText = "Đơn vị", Width = 90 });
            dgvKho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "sl_ton",
                Name = "sl_ton",
                HeaderText = "SL tồn",
                Width = 90,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" }
            });


           

            if (dgvKho.Columns["colChiTiet"] == null)
            {
                dgvKho.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "colChiTiet",
                    HeaderText = "Thao tác",
                    Text = "Chi tiết",
                    UseColumnTextForButtonValue = true,
                    Width = 90
                });
            }
        }
        private void InitSearchRealtime()
        {
            _debounceTimer.Interval = DebounceMs;
            _debounceTimer.Tick += DebounceTimer_Tick;
        }
        private void WireEvents()
        {
           this.Load += FrmKho_Load;
    cbbTinhTrang.SelectionChangeCommitted += cbbTinhTrang_SelectionChangeCommitted;
    txtSearch.TextChanged += txtSearch_TextChanged;


        }
        private void LoadBranchCombo()
        {
            try
            {
                if (_dbHelper == null)
                {
                    MessageBox.Show("Database helper chưa được khởi tạo!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chi nhánh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _selectedBranchId = 1;
            }
        }
        private List<(int BranchId, string BranchName)> GetBranchesWithInventoryData()
        {
            try
            {
                var result = new List<(int BranchId, string BranchName)>();
                
                var branchesData = _bll.LayChiNhanhCoDuLieuTonKho();
                
                if (branchesData != null)
                {
                    foreach (System.Data.DataRow row in branchesData.Rows)
                    {
                        bool hasData = Convert.ToBoolean(row["co_du_lieu"]);
                        if (hasData)
                        {
                            result.Add((Convert.ToInt32(row["chi_nhanh_id"]), row["ten"].ToString()));
                        }
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách chi nhánh có dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<(int BranchId, string BranchName)>();
            }
        }

        private List<(int BranchId, string BranchName)> GetAllBranches()
        {
            try
            {
                var result = new List<(int BranchId, string BranchName)>();
                var allBranches = _bll.LayTatCaChiNhanh();
                
                if (allBranches != null)
                {
                    foreach (System.Data.DataRow row in allBranches.Rows)
                    {
                        result.Add((Convert.ToInt32(row["chi_nhanh_id"]), row["ten"].ToString()));
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách chi nhánh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<(int BranchId, string BranchName)>();
            }
        }

        private void LoadTinhTrangCombo()
        {
            var tb = new DataTable();
            tb.Columns.Add("Value", typeof(int));
            tb.Columns.Add("Text", typeof(string));
            tb.Rows.Add(0, "Tất cả");
            tb.Rows.Add(1, "Còn hàng (>0)");
            tb.Rows.Add(2, "Hết hàng (=0)");
            tb.Rows.Add(3, $"Sắp hết (≤ {NguongCanhBaoMacDinh})");

            cbbTinhTrang.DisplayMember = "Text";
            cbbTinhTrang.ValueMember = "Value";
            cbbTinhTrang.DataSource = tb;
            cbbTinhTrang.SelectedValue = 0;
        }
        private int GetTinhTrang()
        {
            return int.TryParse(cbbTinhTrang?.SelectedValue?.ToString(), out var v) ? v : 0;
        }
        private static decimal SafeGetDecimal(DataRow r, string col)
        {
            return r.Table.Columns.Contains(col) && r[col] != DBNull.Value
                ? Convert.ToDecimal(r[col])
                : 0m;
        }
        private static string BuildRowFilter(int tinhTrang, string searchText, decimal nguongSapHet)
        {
            var parts = new List<string>();


          
            switch (tinhTrang)
            {
                case 1: parts.Add("sl_ton > 0"); break;
                case 2: parts.Add("sl_ton = 0"); break;
                case 3: parts.Add($"sl_ton > 0 AND sl_ton <= {nguongSapHet.ToString(System.Globalization.CultureInfo.InvariantCulture)}"); break;
                default: break; // tất cả
            }

           
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string esc = searchText.Trim().Replace("'", "''");
                // tìm theo mã hoặc tên nguyên liệu
                parts.Add($"(ma_nl LIKE '%{esc}%' OR ten_nl LIKE '%{esc}%')");
            }

            return string.Join(" AND ", parts);
        }

        public void ReloadGrid()
        {
            DataTable tb = _bll.LayTonKhoTheoTinhTrang(0, _selectedBranchId);
            if (tb == null) { dgvKho.DataSource = null; return; }


            var dv = tb.DefaultView;

            // lấy trạng thái hiện tại + text đang gõ
            int stt = GetTinhTrang();
            _currentSearchText = txtSearch?.Text?.Trim() ?? string.Empty;

            // gộp filter
            dv.RowFilter = BuildRowFilter(stt, _currentSearchText, NguongCanhBaoMacDinh);


            string canhBao = NguongCanhBaoMacDinh.ToString(CultureInfo.InvariantCulture);
            
            switch (stt)
            {
                case 1: 
                    dv.RowFilter = "sl_ton > 0";
                    break;
                case 2: 
                    dv.RowFilter = "sl_ton = 0";
                    break;
                case 3:
                    dv.RowFilter = $"sl_ton > 0 AND sl_ton <= {canhBao}";
                    break;
                default:
                    dv.RowFilter = string.Empty;
                    break;
            }

        

            if (!tb.Columns.Contains("GiaTri")) tb.Columns.Add("GiaTri", typeof(decimal));
            foreach (DataRow r in tb.Rows)
            {
                decimal sl = r.Table.Columns.Contains("sl_ton") && r["sl_ton"] != DBNull.Value ? Convert.ToDecimal(r["sl_ton"]) : 0m;
                decimal giaTri = 0m;
                if (tb.Columns.Contains("gia_tri")) giaTri = r["gia_tri"] == DBNull.Value ? 0m : Convert.ToDecimal(r["gia_tri"]);
                else
                {
                    decimal donGia = 0m;
                    if (tb.Columns.Contains("gia_nhap")) donGia = r["gia_nhap"] == DBNull.Value ? 0m : Convert.ToDecimal(r["gia_nhap"]);
                    else if (tb.Columns.Contains("don_gia")) donGia = r["don_gia"] == DBNull.Value ? 0m : Convert.ToDecimal(r["don_gia"]);
                    giaTri = sl * donGia;
                }
                r["GiaTri"] = giaTri;
            }


            dgvKho.DataSource = null;
            dgvKho.DataSource = dv.ToTable();

          
            dgvKho.DataSource = null;
            dgvKho.DataSource = dv.ToTable();  

        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            ReloadGrid();
        }

        private void cbbTinhTrang_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ReloadGrid();
        }



        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");

        private void dgvKho_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }
       

        private void dgvKho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvKho.Columns[e.ColumnIndex].Name != ColActionName) return;

            if (dgvKho.Rows[e.RowIndex].DataBoundItem is DataRowView drv)
            {
                var r = drv.Row;
                int nlId = r.Table.Columns.Contains("nl_id") ? Convert.ToInt32(r["nl_id"]) : -1;
                if (nlId <= 0)
                {
                    MessageBox.Show("Không tìm thấy nl_id của dòng này.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                OpenNguyenLieuChiTiet(nlId);
            }
        }

        private void dgvKho_CellContentClick(object sender, DataGridViewCellEventArgs e, DatabaseHelper _dbHelper)
        {
            

        }


        private void dgvKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private int CurrentBranchId
        {
            get
            {
                return _selectedBranchId;
            }
        }
        private void OpenNguyenLieuChiTiet(int nlId)
        {
            using (var f = new UI.FrmNguyenLieuChiTiet(
        nlId,

        CurrentBranchId,                           
        _bll                                        

    ))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    ReloadGrid();
                    UpdateSummaryPanels();
                }
            }
        }

        private void roundedButton1_Click_1(object sender, EventArgs e)
        {
            using (var f = new Frm_XuatKho())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                var result = f.ShowDialog(this);
                
                if (result == DialogResult.OK)
                {
                    ReloadGrid();
                    UpdateSummaryPanels();
                }
            }
        }

        private void btnChuyenKho_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_ChuyenKho())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                var result = f.ShowDialog(this);
                
                if (result == DialogResult.OK)
                {
                    ReloadGrid();
                    UpdateSummaryPanels();
                }
            }
        }
        private void UpdateSummaryPanels()
        {
            try
            {
                var all = _bll.LayTonKhoTheoTinhTrang(0, _selectedBranchId);

                int tongMatHang = all?.Rows.Count ?? 0;

                int hetHang = 0, sapHet = 0;
                decimal tongGiaTri = 0m;

                if (all != null)
                {
                    foreach (DataRow r in all.Rows)
                    {
                        decimal slTon = 0m;
                        decimal giaTri = 0m;
                        decimal giaNhap = 0m;

                        if (all.Columns.Contains("sl_ton"))
                            slTon = Convert.ToDecimal(r["sl_ton"]);

                      
                        if (all.Columns.Contains("gia_tri"))
                            giaTri = Convert.ToDecimal(r["gia_tri"]);
                        else if (all.Columns.Contains("gia_nhap"))
                            giaTri = slTon * Convert.ToDecimal(r["gia_nhap"]);
                        else if (all.Columns.Contains("don_gia"))
                            giaTri = slTon * Convert.ToDecimal(r["don_gia"]);

                        tongGiaTri += giaTri;

                        if (slTon <= 0) hetHang++;
                        else if (slTon <= NguongCanhBaoMacDinh) sapHet++;
                    }
                }
                _currentSearchText = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(_currentSearchText))
                {
                    // Escape single quotes để tránh lỗi với RowFilter
                    string searchEscaped = _currentSearchText.Replace("'", "''");

                    // Tìm kiếm trong cả mã NL và tên NL
                    string searchFilter = $"(ma_nl LIKE '%{searchEscaped}%' OR ten_nl LIKE '%{searchEscaped}%')";
                    filters.Add(searchFilter);
                }


                label8.Text = tongMatHang.ToString();     
                label9.Text = sapHet.ToString();         
                label10.Text = hetHang.ToString();        
            }
            catch
            {
                
              
                label8.Text = "0";
                label9.Text = "0";
                label10.Text = "0";
            }
        }

        private void btnKiemKe_Click(object sender, EventArgs e)
        {

        }

        public void SetBranchId(int branchId)
        {
            _selectedBranchId = branchId;
            ReloadGrid();
            UpdateSummaryPanels();
        }
        public void ClearSearch()
        {
            txtSearch.Text = string.Empty;
            _currentSearchText = string.Empty;
            ReloadGrid();
        }


        public int GetCurrentBranchId()
        {
            return _selectedBranchId;
        }

        public void ShowBranchSelection()
        {
            try
            {
                var branchesWithData = GetBranchesWithInventoryData();
                
                if (branchesWithData != null && branchesWithData.Count > 0)
                {
                    string message = "Chi nhánh có dữ liệu tồn kho:\n\n";
                    foreach (var branch in branchesWithData)
                    {
                        message += $"• {branch.BranchName} (ID: {branch.BranchId})\n";
                    }
                    
                    MessageBox.Show(message, "Danh Sách Chi Nhánh", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có chi nhánh nào có dữ liệu tồn kho!", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị danh sách chi nhánh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
