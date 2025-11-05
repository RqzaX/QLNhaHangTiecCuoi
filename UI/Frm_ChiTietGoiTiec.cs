using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    public class Frm_ChiTietGoiTiec : Form
    {
        private readonly int _goiId;
        private readonly string _maGoi;
        private readonly string _tenGoi;
        private readonly decimal _giaCoBan;
        private readonly GoiTiecBLL _bll;

        private Label lblTitle, lblGia, lblSucChua;
        private DataGridView dgvChiTiet;

        public Frm_ChiTietGoiTiec(int goiId, string maGoi, string tenGoi, decimal giaCoBan)
        {
            _goiId = goiId;
            _maGoi = maGoi ?? "";
            _tenGoi = tenGoi ?? "";
            _giaCoBan = giaCoBan;
            _bll = new GoiTiecBLL();

            BuildUI();
            this.Load += (s, e) => { RenderHeader(); LoadChiTiet(); LoadSucChua(); };
        }

        private void BuildUI()
        {
           Text = "Chi tiết gói tiệc";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(860, 560);
            BackColor = Color.White;

            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), ColumnCount = 1, RowCount = 5 };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(root);

            var header = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.LeftToRight };
            lblTitle = new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 16f, FontStyle.Bold) };
            header.Controls.Add(lblTitle);
            root.Controls.Add(header);

            var pricePanel = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.FromArgb(236, 245, 255), Padding = new Padding(12), Margin = new Padding(0, 8, 0, 8) };
            var lblGiaTitle = new Label { Text = "Giá mỗi bàn", AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) };
            lblGia = new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 18f, FontStyle.Bold), Top = 28 };
            pricePanel.Controls.Add(lblGiaTitle);
            pricePanel.Controls.Add(lblGia);
            root.Controls.Add(pricePanel);

            var capacityPanel = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.FromArgb(255, 247, 237), Padding = new Padding(12), Margin = new Padding(0, 0, 0, 8) };
            var lblSucChuaTitle = new Label { Text = "Sức chứa tối đa", AutoSize = true, ForeColor = Color.FromArgb(71, 85, 105) };
            lblSucChua = new Label { AutoSize = true, Font = new Font("Segoe UI Semibold", 18f, FontStyle.Bold), Top = 28 };
            capacityPanel.Controls.Add(lblSucChuaTitle);
            capacityPanel.Controls.Add(lblSucChua);
            root.Controls.Add(capacityPanel);

            var pnlActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Padding = new Padding(0),
                Margin = new Padding(0, 0, 0, 8)
            };
            var btnThem = new Button { Text = "Thêm món", AutoSize = true, Padding = new Padding(12, 6, 12, 6) };
            var btnSua = new Button { Text = "Sửa món", AutoSize = true, Padding = new Padding(12, 6, 12, 6) };
            var btnXoa = new Button { Text = "Xóa món", AutoSize = true, Padding = new Padding(12, 6, 12, 6) };

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;

            pnlActions.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa });
            root.Controls.Add(pnlActions);

            dgvChiTiet = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ma_mon", HeaderText = "Mã món", Visible = false , FillWeight = 25 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ten_mon", HeaderText = "Tên món", FillWeight = 55 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "so_luong",
                HeaderText = "Số lượng",
                FillWeight = 20,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0.###" }
            });
            root.Controls.Add(dgvChiTiet);


        }

        private void RenderHeader()
        {
            lblTitle.Text = string.IsNullOrWhiteSpace(_tenGoi) ? $"Gói: {_maGoi}" : _tenGoi;
            
            // Tính giá mỗi bàn = tổng giá các món + 10% phí dịch vụ
            try
            {
                decimal giaMoiBan = _bll.TinhGiaMoiBan(_goiId);
                lblGia.Text = $"{giaMoiBan:#,##0} đ";
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, hiển thị giá cơ bản từ database
                lblGia.Text = $"{_giaCoBan:#,##0} đ";
                MessageBox.Show("Lỗi tính giá mỗi bàn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadSucChua()
        {
            try
            {
                int sucChua = _bll.GetSucChuaGoiTiec(_goiId);
                if (sucChua > 0)
                {
                    lblSucChua.Text = $"{sucChua:#,##0} người";
                }
                else
                {
                    lblSucChua.Text = "Chưa cập nhật";
                }
            }
            catch (Exception ex)
            {
                lblSucChua.Text = "N/A";
                MessageBox.Show("Lỗi tải sức chứa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadChiTiet()
        {
            try
            {
                DataTable dt = _bll.GetChiTietGoiTiec(_goiId);
                dgvChiTiet.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết gói: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ShowMonDialog(string title, string maMonDefault, decimal soLuongDefault,
                           out string maMon, out decimal soLuong)
        {
            maMon = maMonDefault; soLuong = soLuongDefault;

            var f = new Form
            {
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(360, 140)
            };

            var lblMa = new Label { Text = "Mã món:", Left = 12, Top = 16, AutoSize = true };
            var txtMa = new TextBox { Left = 100, Top = 12, Width = 230, Text = maMonDefault ?? "" };
            var lblSL = new Label { Text = "Số lượng:", Left = 12, Top = 56, AutoSize = true };
            var txtSL = new TextBox { Left = 100, Top = 52, Width = 230, Text = soLuongDefault.ToString("#,##0.###") };

            var btnOK = new Button { Text = "OK", DialogResult = DialogResult.OK, Left = 170, Top = 92, Width = 75 };
            var btnCan = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Left = 255, Top = 92, Width = 75 };

            f.Controls.AddRange(new Control[] { lblMa, txtMa, lblSL, txtSL, btnOK, btnCan });
            f.AcceptButton = btnOK; f.CancelButton = btnCan;

            if (f.ShowDialog(this) == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã món.", "Thiếu dữ liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ShowMonDialog(title, txtMa.Text, soLuongDefault, out maMon, out soLuong);
                }
                if (!decimal.TryParse(txtSL.Text.Replace(",", "").Trim(), out var sl) || sl <= 0)
                {
                    MessageBox.Show("Số lượng phải là số > 0.", "Thiếu dữ liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ShowMonDialog(title, txtMa.Text, soLuongDefault, out maMon, out soLuong);
                }

                maMon = txtMa.Text.Trim();
                soLuong = sl;
                return true;
            }
            return false;
        }
        private string GetStrFromCurrent(string col)
        {
            if (dgvChiTiet?.CurrentRow?.DataBoundItem is System.Data.DataRowView v &&
                v.Row?.Table?.Columns.Contains(col) == true)
                return Convert.ToString(v[col]);

            // fallback nếu không phải DataTable
            if (dgvChiTiet?.Columns.Contains(col) == true)
                return Convert.ToString(dgvChiTiet.CurrentRow.Cells[col]?.Value);

            return "";
        }

        private decimal GetDecFromCurrent(string col)
        {
            var s = GetStrFromCurrent(col);
            return decimal.TryParse(s, out var d) ? d : 0m;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowMonDialog("Thêm món vào gói", "", 1, out var maMon, out var soLuong))
                {
                    _bll.ThemMonVaoGoi(_goiId, maMon, soLuong);
                    LoadChiTiet();
                    RenderHeader(); // Cập nhật lại giá mỗi bàn
                    MessageBox.Show("Đã thêm/ cập nhật món trong gói.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm món: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChiTiet.CurrentRow == null)
                {
                    MessageBox.Show("Chọn một dòng để sửa.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // LẤY TRỰC TIẾP TỪ DataRowView
                string oldMaMon = GetStrFromCurrent("ma_mon");
                string tenMon = GetStrFromCurrent("ten_mon");
                decimal soLuongCu = GetDecFromCurrent("so_luong");

                if (string.IsNullOrWhiteSpace(oldMaMon))
                {
                    MessageBox.Show("Không tìm thấy mã món (ma_mon) trong dữ liệu nguồn.",
                        "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (ShowMonDialog($"Sửa món ({tenMon})", oldMaMon, soLuongCu,
                                  out var newMaMon, out var newSoLuong))
                {
                    _bll.SuaMonTrongGoi(_goiId, oldMaMon, newMaMon, newSoLuong);
                    LoadChiTiet();
                    RenderHeader(); // Cập nhật lại giá mỗi bàn
                    MessageBox.Show("Đã cập nhật món trong gói.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa món: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChiTiet.CurrentRow == null)
                {
                    MessageBox.Show("Chọn một dòng để xóa.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string maMon = GetStrFromCurrent("ma_mon");
                string tenMon = GetStrFromCurrent("ten_mon");

                if (string.IsNullOrWhiteSpace(maMon))
                {
                    MessageBox.Show("Không tìm thấy mã món (ma_mon) trong dữ liệu nguồn.",
                        "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show($"Xóa món '{tenMon}' ({maMon}) khỏi gói?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _bll.XoaMonKhoiGoi(_goiId, maMon);
                    LoadChiTiet();
                    RenderHeader(); // Cập nhật lại giá mỗi bàn
                    MessageBox.Show("Đã xóa món khỏi gói.", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa món: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
