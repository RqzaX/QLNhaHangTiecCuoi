using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace UI
{
    public partial class FrmGoiTiec : Form
    {
        private GoiTiecBLL _bll;
        private int _goiIdDangChon = 0;
        private bool _isEditing = false;
        private static bool HasCol(DataRowView v, string col)
            => v?.Row?.Table?.Columns.Contains(col) == true;

        public FrmGoiTiec()
        {

            InitializeComponent();
            _bll = new GoiTiecBLL();
            this.Load += FrmGoiTiec_Load;


        }


        private void FrmGoiTiec_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigureDataGridView();
                LoadDanhSachGoiTiec();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load form: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            dgvGoiTiec.AutoGenerateColumns = false;
            dgvGoiTiec.AllowUserToAddRows = false;
            dgvGoiTiec.ReadOnly = true;
            dgvGoiTiec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGoiTiec.MultiSelect = false;
            dgvGoiTiec.RowHeadersVisible = false;
            dgvGoiTiec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvGoiTiec.Columns.Clear();


            dgvGoiTiec.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "goi_id",
                HeaderText = "ID",
                DataPropertyName = "goi_id",
                Visible = false
            });

            dgvGoiTiec.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ma_goi",
                HeaderText = "Mã gói",
                DataPropertyName = "ma_goi",
                Width = 150
            });

            dgvGoiTiec.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ten_goi",
                HeaderText = "Tên gói",
                DataPropertyName = "ten_goi",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvGoiTiec.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "gia_co_ban",
                HeaderText = "Giá cơ bản",
                DataPropertyName = "gia_co_ban",
                Width = 200,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "#,##0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            dgvGoiTiec.RowTemplate.Height = 35;
        }

        private void LoadDanhSachGoiTiec()
        {
            try
            {
                DataTable dt = _bll.GetAllGoiTiec();
                dgvGoiTiec.DataSource = dt;
           


                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Chưa có gói tiệc nào trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtMaGoi.Clear();
            txtTenGoi.Clear();
            txtGiaGoi.Clear();

            _goiIdDangChon = 0;
            _isEditing = false;

            // Trạng thái nút
            btnThemGoi.Enabled = true;
            btnCapNhat.Enabled = false;

            // Nhập mã khi thêm mới
            txtMaGoi.Enabled = true;
            txtMaGoi.Focus();
        }

        private void dgvGoiTiec_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvGoiTiec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dgvGoiTiec.Rows[e.RowIndex];

                _goiIdDangChon = Convert.ToInt32(row.Cells["goi_id"].Value);
                txtMaGoi.Text = row.Cells["ma_goi"].Value?.ToString();
                txtTenGoi.Text = row.Cells["ten_goi"].Value?.ToString();
                
                // Lấy giá từ database và format đúng cách
                object giaValue = row.Cells["gia_co_ban"].Value;
                if (giaValue != null && giaValue != DBNull.Value)
                {
                    decimal gia = Convert.ToDecimal(giaValue);
                    txtGiaGoi.Text = gia.ToString("#,##0");
                }
                else
                {
                    txtGiaGoi.Text = "";
                }

                _isEditing = true;


                // Chỉ cho phép cập nhật, không cho bấm thêm
                btnThemGoi.Enabled = false;
                btnCapNhat.Enabled = true;

                // Không cho sửa mã gói khi cập nhật

                btnCapNhat.Text = "Cập Nhật";
                btnCapNhat.BackColor = System.Drawing.Color.DodgerBlue;

                txtMaGoi.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn gói: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                string maGoi = txtMaGoi.Text.Trim();
                string tenGoi = txtTenGoi.Text.Trim();
                decimal giaGoi = _bll.ParseTien(txtGiaGoi.Text);

                if (_isEditing)
                {
                    bool success = _bll.CapNhatGoiTiec(_goiIdDangChon, maGoi, tenGoi, giaGoi, out string errorMessage);

                    if (success)
                    {
                        MessageBox.Show("Cập nhật gói tiệc thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachGoiTiec();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show(errorMessage, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    bool success = _bll.ThemGoiTiec(maGoi, tenGoi, giaGoi, out string errorMessage);

                    if (success)
                    {
                        MessageBox.Show("Thêm gói tiệc thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachGoiTiec();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show(errorMessage, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_goiIdDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn gói tiệc!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string tenGoi = txtTenGoi.Text;

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa gói tiệc:\n'{tenGoi}'?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = _bll.XoaGoiTiec(_goiIdDangChon, out string errorMessage);

                    if (success)
                    {
                        MessageBox.Show("Xóa gói tiệc thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachGoiTiec();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show(errorMessage, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void roundedButton1_Click(object sender, EventArgs e)
        {
            if (_goiIdDangChon == 0)
            {
                MessageBox.Show("Vui lòng chọn một gói tiệc!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string tenGoi = txtTenGoi.Text;
                decimal giaGoi = _bll.ParseTien(txtGiaGoi.Text);

                DialogResult result = MessageBox.Show(
                    $"Bạn có muốn chọn gói:\n\n" +
                    $"Tên gói: {tenGoi}\n" +
                    $"Giá: {_bll.FormatTien(giaGoi)}\n\n" +
                    $"Xác nhận chọn gói này?",
                    "Xác nhận chọn gói",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show(
                        $"Đã chọn gói '{tenGoi}' thành công!\n\n" +
                        $"Giá: {_bll.FormatTien(giaGoi)} \" \n Chuyển Khoản tới số tài khoản:" +
                        $"2777777777704  \n \"Ngân hàng MBBank",
                        "Chọn gói thành công \n",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtGiaGoi_KeyPress(object sender, KeyPressEventArgs e)
        {
          
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtGiaGoi_Leave(object sender, EventArgs e)
        {

            // Format hiển thị tiền khi rời khỏi textbox
            if (string.IsNullOrWhiteSpace(txtGiaGoi.Text))
            {
                txtGiaGoi.Text = "";
                return;
            }
            

            decimal gia = _bll.ParseTien(txtGiaGoi.Text);
            if (gia > 0)
            {
                txtGiaGoi.Text = gia.ToString("#,##0");
            }
            else
            {
                txtGiaGoi.Text = "";
            }
        }
        private void EnsureGoiIdColumnForGrid()
        {
            if (dgvGoiTiec.DataSource is DataTable dt)
            {
                if (!dt.Columns.Contains("goi_id"))
                    dt.Columns.Add("goi_id", typeof(int));

                foreach (DataRow r in dt.Rows)
                {
                    if (r["goi_id"] != DBNull.Value && Convert.ToInt32(r["goi_id"]) > 0)
                        continue;

                    // ưu tiên tra theo ma_goi nếu có
                    string ma = dt.Columns.Contains("ma_goi") ? Convert.ToString(r["ma_goi"]) : null;
                    int id = 0;
                    if (!string.IsNullOrWhiteSpace(ma))
                        id = _bll.GetGoiIdByMaGoi(ma.Trim());

                    // fallback: tra theo tên gói (nếu unique)
                    if (id <= 0 && dt.Columns.Contains("ten_goi"))
                    {
                        string ten = Convert.ToString(r["ten_goi"]);
                        if (!string.IsNullOrWhiteSpace(ten))
                            id = _bll.GetGoiIdByTenGoi(ten.Trim()); // thêm ở mục B bên dưới
                    }

                    if (id > 0) r["goi_id"] = id;
                }

                // thêm cột ẩn vào grid nếu chưa có
                if (!dgvGoiTiec.Columns.Contains("goi_id"))
                {
                    var col = new DataGridViewTextBoxColumn
                    {
                        Name = "goi_id",
                        DataPropertyName = "goi_id",
                        HeaderText = "ID",
                        Visible = false
                    };
                    dgvGoiTiec.Columns.Add(col);
                }
            }
        }
        private void btnThemGoi_Click(object sender, EventArgs e)
        {
            try
            {
                string maGoi = txtMaGoi.Text.Trim();
                string tenGoi = txtTenGoi.Text.Trim();
                decimal giaGoi = _bll.ParseTien(txtGiaGoi.Text);

                if (string.IsNullOrWhiteSpace(maGoi) || string.IsNullOrWhiteSpace(tenGoi))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã gói và Tên gói!", "Thiếu dữ liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool success = _bll.ThemGoiTiec(maGoi, tenGoi, giaGoi, out string errorMessage);

                if (success)
                {
                    MessageBox.Show("Thêm gói tiệc thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachGoiTiec();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show(errorMessage, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnChiTietGoi_Click(object sender, EventArgs e)
        {

            try
            {
                // đảm bảo BLL không null
                _bll ??= new GoiTiecBLL();

                // lưới phải có dòng đang chọn
                if (dgvGoiTiec == null || dgvGoiTiec.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn một gói tiệc.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int goiId = 0;
                string maGoi = "";
                string tenGoi = "";
                decimal giaCoBan = 0m;

                // Nguồn là DataTable
                if (dgvGoiTiec.CurrentRow.DataBoundItem is DataRowView drv)
                {
                    var tbl = drv.Row?.Table;

                    if (tbl?.Columns.Contains("goi_id") == true)
                        goiId = drv["goi_id"] == DBNull.Value ? 0 : Convert.ToInt32(drv["goi_id"]);

                    if (tbl?.Columns.Contains("ma_goi") == true)
                        maGoi = Convert.ToString(drv["ma_goi"]);

                    if (tbl?.Columns.Contains("ten_goi") == true)
                        tenGoi = Convert.ToString(drv["ten_goi"]);

                    if (tbl?.Columns.Contains("gia_co_ban") == true)
                        giaCoBan = drv["gia_co_ban"] == DBNull.Value ? 0m : Convert.ToDecimal(drv["gia_co_ban"]);
                }
                // Nguồn là List<T>…: lấy theo tên cột của grid
                else
                {
                    object Cell(string col) => dgvGoiTiec.Columns.Contains(col) ? dgvGoiTiec.CurrentRow.Cells[col]?.Value : null;

                    goiId = TryInt(Cell("goi_id"));
                    maGoi = Convert.ToString(Cell("ma_goi") ?? "");
                    tenGoi = Convert.ToString(Cell("ten_goi") ?? "");
                    giaCoBan = TryDec(Cell("gia_co_ban"));
                }

                // Fallback: nếu thiếu goi_id nhưng có ma_goi thì tra từ DB
                if (goiId <= 0 && !string.IsNullOrWhiteSpace(maGoi))
                    goiId = _bll.GetGoiIdByMaGoi(maGoi.Trim());

                if (goiId <= 0)
                {
                    MessageBox.Show("Không xác định được ID gói tiệc (cột goi_id / hoặc tra theo ma_goi thất bại).",
                        "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var f = new Frm_ChiTietGoiTiec(goiId, maGoi, tenGoi, giaCoBan))
                    f.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không mở được chi tiết: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // --- local helpers ---
            int TryInt(object v) { if (v == null || v == DBNull.Value) return 0; return int.TryParse(v.ToString(), out var n) ? n : 0; }
            decimal TryDec(object v) { if (v == null || v == DBNull.Value) return 0m; return decimal.TryParse(v.ToString(), out var d) ? d : 0m; }
        }
        private object GetCellValue(DataGridViewRow row, string col)
        {
            if (row == null || string.IsNullOrEmpty(col)) return null;
            return dgvGoiTiec.Columns.Contains(col) ? row.Cells[col]?.Value : null;
        }
        private static int ToIntSafe(object v)
        {
            if (v == null || v == DBNull.Value) return 0;
            int n; return int.TryParse(v.ToString(), out n) ? n : 0;
        }
        private static decimal ToDecSafe(object v)
        {
            if (v == null || v == DBNull.Value) return 0m;
            decimal d; return decimal.TryParse(v.ToString(), out d) ? d : 0m;
        }

        private void dgvGoiTiec_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnChiTietGoi_Click(sender, EventArgs.Empty);
            }
        }
    }
}
