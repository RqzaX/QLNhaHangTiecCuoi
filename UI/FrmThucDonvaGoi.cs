using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Forms;
using UiControls;
using BLL;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmThucDonvaGoi : Form
    {
        private ThucDonGoiBLL _bll;
        private string _loaiHienTai = "MONAN";
        private Rectangle _hoverEditButton = Rectangle.Empty;
        private Rectangle _hoverDeleteButton = Rectangle.Empty;

        private const string COL_ID = "ID";
        private const string COL_TEN = "TenMon";
        private const string COL_DM = "DanhMuc";
        private const string COL_GB = "GiaBan";
        private const string COL_GV = "GiaVon";
        private const string COL_LN = "LoiNhuan";
        private const string COL_TT = "TrangThai";
        private const string COL_TTAC = "ThaoTac";

        public FrmThucDonvaGoi()
        {
            InitializeComponent();
           
            try
            {

                _bll = new ThucDonGoiBLL();


                if (!_bll.TestConnection())
                {
                    MessageBox.Show(
                        "Không thể kết nối đến database!\n\n" +
                        "Kiểm tra lại:\n" +
                        "1. SQL Server đang chạy\n" +
                        "2. Server name: LAPTOP-2L5G5GIH\\SQLEXPRESS03\n" +
                        "3. Database: QL_NhaHangTiecCuoi_V3\n\n" +
                        "Mở SQL Server Management Studio để kiểm tra.",
                        "Lỗi Kết Nối Database",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khởi tạo BLL:\n{ex.Message}\n\n{ex.StackTrace}",
                    "Lỗi Khởi Tạo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            roundedTextBox1.TextChanged += roundedTextBox1_TextChanged;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void menuGrid1_Load(object sender, EventArgs e)
        {

        }

        private void FrmThucDonvaGoi_Load(object sender, EventArgs e)
        {
            try
            {
                if (_bll == null)
                {
                    MessageBox.Show("BLL chưa được khởi tạo!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (!dgvThucDonVaGoi.Columns.Contains(COL_ID))
                {
                    DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                    colId.Name = COL_ID;
                    colId.HeaderText = "ID";
                    colId.Visible = false;
                    dgvThucDonVaGoi.Columns.Insert(0, colId);
                }

                ConfigureDataGridView();
                LoadDataThucDonVaGoi();
                EnsureActionColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load form:\n{ex.Message}\n\n{ex.StackTrace}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            dgvThucDonVaGoi.AutoGenerateColumns = false;
            dgvThucDonVaGoi.AllowUserToAddRows = false;
            dgvThucDonVaGoi.ReadOnly = true;
            dgvThucDonVaGoi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThucDonVaGoi.MultiSelect = false;
            dgvThucDonVaGoi.RowHeadersVisible = false;
            dgvThucDonVaGoi.RowTemplate.Height = 60;
        }

        private void LoadDataThucDonVaGoi()
        {
            try
            {
                DataTable dt;

                if (_loaiHienTai == "MONAN")
                {
                    dt = _bll.GetDanhSachMonAn();
                }
                else
                {
                    dt = _bll.GetDanhSachGoiTiec();
                }

                dgvThucDonVaGoi.Rows.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["ID"]);
                        string ten = row["TenMon"].ToString();
                        string dm = row["DanhMuc"].ToString();
                        decimal gb = Convert.ToDecimal(row["GiaBan"]);
                        decimal gv = Convert.ToDecimal(row["GiaVon"]);
                        bool conHang = row["TrangThai"].ToString() == "Còn hàng";

                        AddTDRow(id, ten, dm, gb, gv, conHang);
                    }
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu:\n{ex.Message}\n\n{ex.StackTrace}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private const string COL_EDIT = "colSua";
        private const string COL_DEL = "colXoa";

        private void EnsureActionColumns()
        {
            if (!dgvThucDonVaGoi.Columns.Contains(COL_EDIT))
            {
                var colSua = new DataGridViewButtonColumn
                {
                    Name = COL_EDIT,
                    HeaderText = "Thao tác",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true,
                    Width = 100
                };
                dgvThucDonVaGoi.Columns.Add(colSua);
            }

            if (!dgvThucDonVaGoi.Columns.Contains(COL_DEL))
            {
                var colXoa = new DataGridViewButtonColumn
                {
                    Name = COL_DEL,
                    HeaderText = "",
                    Text = "Xóa",
                    UseColumnTextForButtonValue = true,
                    Width = 70
                };
                dgvThucDonVaGoi.Columns.Add(colXoa);
            }
        }

        private void AddTDRow(int id, string ten, string dm, decimal gb, decimal gv, bool conHang)
        {
            int idx = dgvThucDonVaGoi.Rows.Add();
            var row = dgvThucDonVaGoi.Rows[idx];

            row.Cells[COL_ID].Value = id;
            row.Cells[COL_TEN].Value = ten;
            row.Cells[COL_DM].Value = dm;
            row.Cells[COL_GB].Value = Money(gb);
            row.Cells[COL_GV].Value = Money(gv);
            row.Cells[COL_LN].Value = Money(gb - gv) + "\n" + ProfitPercent(gb, gv);
            row.Cells[COL_TT].Value = conHang ? "Còn hàng" : "Hết hàng";
            row.Cells[COL_TTAC].Value = "";
        }

        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");

        private static string ProfitPercent(decimal gb, decimal gv)
        {
            if (gb <= 0) return "0%";
            var p = (gb - gv) / gb * 100m;
            return Math.Round(p, 1).ToString("0.0") + "%";
        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {
            using (var f = new FrmThemMonMoi())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK)
                {

                    _loaiHienTai = "MONAN";
                    LoadDataThucDonVaGoi();
                    EnsureActionColumns();


                    if (f.CreatedMonId.HasValue)
                    {
                        foreach (DataGridViewRow row in dgvThucDonVaGoi.Rows)
                        {
                            if (row.Cells["ID"].Value is int id && id == f.CreatedMonId.Value)
                            {
                                row.Selected = true;
                                dgvThucDonVaGoi.FirstDisplayedScrollingRowIndex =
                                    Math.Max(row.Index - 3, 0);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnGoiTiecCuoi_Click(object sender, EventArgs e)
        {
            using (var f = new FrmGoiTiec())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
        }
        bool HasCol(string name) => dgvThucDonVaGoi.Columns.Contains(name);
        object CellVal(DataGridViewRow r, string name) => HasCol(name) ? r.Cells[name].Value : null;

        private void dgvThucDonVaGoi_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var colName = dgvThucDonVaGoi.Columns[e.ColumnIndex].Name;
            if (colName != COL_EDIT && colName != COL_DEL) return;

            var row = dgvThucDonVaGoi.Rows[e.RowIndex];
            var vId = CellVal(row, COL_ID);
            if (vId == null || vId == DBNull.Value)
            {
                MessageBox.Show("Không xác định được ID món.");
                return;
            }
            int monId = Convert.ToInt32(vId);

            if (colName == COL_EDIT)
            {
                var info = _bll.GetMonAnById(monId);
                if (info == null)
                {
                    MessageBox.Show("Không tìm thấy món trong cơ sở dữ liệu."); return;
                }

                using (var f = new FrmThemMonMoi(
                    monId,
                    info.Value.MaMon,
                    info.Value.TenMon,
                    info.Value.Nhom,
                    info.Value.DonViTinh,
                    info.Value.DonGia,
                    info.Value.DangBan))
                {
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadDataThucDonVaGoi();
                        EnsureActionColumns();
                        SelectRowById(monId);
                    }
                }
            }
            else if (colName == COL_DEL)
            {
                var ten = Convert.ToString(CellVal(row, COL_TEN));
                if (MessageBox.Show($"Bạn có chắc muốn xóa món: {ten}?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        _bll.XoaMonAn(monId);
                        LoadDataThucDonVaGoi();
                        EnsureActionColumns();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể xóa món.\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SelectRowById(int monId)
        {
            foreach (DataGridViewRow r in dgvThucDonVaGoi.Rows)
            {
                var v = CellVal(r, COL_ID);
                if (v is int id && id == monId)
                {
                    r.Selected = true;
                    dgvThucDonVaGoi.FirstDisplayedScrollingRowIndex = Math.Max(r.Index - 3, 0);
                    break;
                }
            }
        }
        private void roundedTextBox1_TextChanged(object? sender, EventArgs e)
        {
            ApplyFilter(roundedTextBox1.Text);
        }
        private void ApplyFilter(string? query)
        {
            string q = (query ?? "").Trim();
            string normQ = NormalizeNoDiacritics(q);

            // nếu ô tìm kiếm rỗng -> hiện tất cả
            if (string.IsNullOrEmpty(normQ))
            {
                foreach (DataGridViewRow r in dgvThucDonVaGoi.Rows) r.Visible = true;
                return;
            }

            foreach (DataGridViewRow r in dgvThucDonVaGoi.Rows)
            {
                bool match =
                    MatchCell(r, COL_TEN, normQ) ||
                    MatchCell(r, COL_DM, normQ) ||
                    MatchCell(r, COL_GB, normQ) ||
                    MatchCell(r, COL_GV, normQ) ||
                    MatchCell(r, COL_LN, normQ) ||
                    MatchCell(r, COL_TT, normQ);

                r.Visible = match;
            }
        }

        // So khớp 1 ô theo logic bỏ dấu + lowercase
        private bool MatchCell(DataGridViewRow r, string colName, string normQ)
        {
            if (!dgvThucDonVaGoi.Columns.Contains(colName)) return false;
            var val = r.Cells[colName].Value?.ToString() ?? "";
            var normVal = NormalizeNoDiacritics(val);
            return normVal.Contains(normQ);
        }

        // Bỏ dấu tiếng Việt và lower-case
        private string NormalizeNoDiacritics(string input)
        {
            input = input?.Trim().ToLowerInvariant() ?? "";
            string stFormD = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(capacity: stFormD.Length);

            foreach (var ch in stFormD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void roundedTextBox1_Load(object sender, EventArgs e)
        {

        }
    }
}