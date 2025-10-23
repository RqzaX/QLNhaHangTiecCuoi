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
            // Cấu hình DataGridView
            dgvGoiTiec.AutoGenerateColumns = false;
            dgvGoiTiec.AllowUserToAddRows = false;
            dgvGoiTiec.ReadOnly = true;
            dgvGoiTiec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGoiTiec.MultiSelect = false;
            dgvGoiTiec.RowHeadersVisible = false;
            dgvGoiTiec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Clear các cột cũ (nếu có)
            dgvGoiTiec.Columns.Clear();

            // Thêm các cột
            dgvGoiTiec.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                DataPropertyName = "ID",
                Visible = false
            });

            dgvGoiTiec.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaGoi",
                HeaderText = "Mã Gói",
                DataPropertyName = "Mã Gói",
                Width = 150
            });

            dgvGoiTiec.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenGoi",
                HeaderText = "Tên Gói",
                DataPropertyName = "Tên Gói",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvGoiTiec.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GiaCoBan",
                HeaderText = "Giá Cơ Bản",
                DataPropertyName = "Giá Cơ Bản",
                Width = 200,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "#,##0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // Đặt chiều cao dòng
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

            btnCapNhat.Text = "Thêm Mới";
            btnCapNhat.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);

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

                _goiIdDangChon = Convert.ToInt32(row.Cells["ID"].Value);
                txtMaGoi.Text = row.Cells["MaGoi"].Value.ToString();
                txtTenGoi.Text = row.Cells["TenGoi"].Value.ToString();
                txtGiaGoi.Text = row.Cells["GiaCoBan"].Value.ToString();

                _isEditing = true;
                btnCapNhat.Text = "Cập Nhật";
                btnCapNhat.BackColor = System.Drawing.Color.DodgerBlue;
                txtMaGoi.Enabled = false; // Không cho sửa mã gói
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
                    // Cập nhật
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
                    // Thêm mới
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
                MessageBox.Show("Vui lòng chọn gói tiệc cần xóa!", "Thông báo",
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
            // Nút "Chọn Gói"
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

        // Thêm các event handlers khác
        private void txtGiaGoi_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtGiaGoi_Leave(object sender, EventArgs e)
        {
            // Format hiển thị tiền khi rời khỏi textbox
            decimal gia = _bll.ParseTien(txtGiaGoi.Text);
            if (gia > 0)
            {
                txtGiaGoi.Text = gia.ToString("#,##0");
            }
        }
    }
}
