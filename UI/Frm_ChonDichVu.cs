using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public class Frm_ChonDichVu : Form
    {
        private readonly int _goiId;
        private readonly GoiTiecBLL _bll;
        private DataGridView dgvAllDichVu;
        private DataGridView dgvSelectedDichVu;
        private Button btnThem, btnXoa, btnLuu, btnDong;

        public Frm_ChonDichVu(int goiId, GoiTiecBLL bll)
        {
            _goiId = goiId;
            _bll = bll;
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            Text = "Chọn dịch vụ cho gói tiệc";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1000, 650);
            MinimumSize = new Size(900, 550);
            BackColor = Color.FromArgb(248, 250, 252);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 1
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            Controls.Add(root);

            // Panel bên trái với border
            var pnlLeftWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(229, 231, 235),
                Padding = new Padding(1)
            };
            var pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            var pnlLeftContent = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(12)
            };
            pnlLeftContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pnlLeftContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            
            // Label "Tất cả dịch vụ"
            var lblAll = new Label
            {
                Text = "Tất cả dịch vụ",
                Font = new Font("Segoe UI Semibold", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            pnlLeftContent.Controls.Add(lblAll, 0, 0);
            pnlLeft.Controls.Add(pnlLeftContent);
            pnlLeftWrapper.Controls.Add(pnlLeft);
            root.Controls.Add(pnlLeftWrapper, 0, 0);

            // Panel bên phải với border
            var pnlRightWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(229, 231, 235),
                Padding = new Padding(1)
            };
            var pnlRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            var pnlRightContent = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(12)
            };
            pnlRightContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pnlRightContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            
            // Label "Dịch vụ đã chọn"
            var lblSelected = new Label
            {
                Text = "Dịch vụ đã chọn",
                Font = new Font("Segoe UI Semibold", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(17, 24, 39),
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 12),
                Dock = DockStyle.Top
            };
            pnlRightContent.Controls.Add(lblSelected, 0, 0);
            pnlRight.Controls.Add(pnlRightContent);
            pnlRightWrapper.Controls.Add(pnlRight);
            root.Controls.Add(pnlRightWrapper, 1, 0);

            // DataGridView tất cả dịch vụ
            dgvAllDichVu = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(229, 231, 235),
                EnableHeadersVisualStyles = false
            };
            
            // Styling header
            dgvAllDichVu.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = Color.FromArgb(17, 24, 39),
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                Padding = new Padding(12, 12, 12, 12),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgvAllDichVu.ColumnHeadersHeight = 45;
            
            // Styling rows
            dgvAllDichVu.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(31, 41, 55),
                BackColor = Color.White,
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(17, 24, 39),
                Padding = new Padding(12, 10, 12, 10)
            };
            dgvAllDichVu.RowTemplate.Height = 40;
            
            dgvAllDichVu.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251)
            };
            
            dgvAllDichVu.DataBindingComplete += (s, e) =>
            {
                try
                {
                    if (dgvAllDichVu == null || dgvAllDichVu.Columns == null) return;
                    
                    if (dgvAllDichVu.Columns.Contains("dv_id"))
                        dgvAllDichVu.Columns["dv_id"].Visible = false;
                    if (dgvAllDichVu.Columns.Contains("dang_ban"))
                        dgvAllDichVu.Columns["dang_ban"].Visible = false;
                    if (dgvAllDichVu.Columns.Contains("ma_dv"))
                    {
                        dgvAllDichVu.Columns["ma_dv"].HeaderText = "Mã DV";
                        dgvAllDichVu.Columns["ma_dv"].FillWeight = 15;
                        dgvAllDichVu.Columns["ma_dv"].MinimumWidth = 100;
                        dgvAllDichVu.Columns["ma_dv"].Width = 100;
                    }
                    if (dgvAllDichVu.Columns.Contains("ten_dv"))
                    {
                        dgvAllDichVu.Columns["ten_dv"].HeaderText = "Tên dịch vụ";
                        dgvAllDichVu.Columns["ten_dv"].FillWeight = 55;
                        dgvAllDichVu.Columns["ten_dv"].MinimumWidth = 300;
                        dgvAllDichVu.Columns["ten_dv"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvAllDichVu.Columns["ten_dv"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    }
                    if (dgvAllDichVu.Columns.Contains("don_vi_tinh"))
                    {
                        dgvAllDichVu.Columns["don_vi_tinh"].HeaderText = "ĐVT";
                        dgvAllDichVu.Columns["don_vi_tinh"].FillWeight = 12;
                        dgvAllDichVu.Columns["don_vi_tinh"].MinimumWidth = 80;
                        dgvAllDichVu.Columns["don_vi_tinh"].Width = 80;
                    }
                    if (dgvAllDichVu.Columns.Contains("don_gia"))
                    {
                        dgvAllDichVu.Columns["don_gia"].HeaderText = "Đơn giá";
                        dgvAllDichVu.Columns["don_gia"].FillWeight = 18;
                        dgvAllDichVu.Columns["don_gia"].MinimumWidth = 130;
                        dgvAllDichVu.Columns["don_gia"].Width = 130;
                        dgvAllDichVu.Columns["don_gia"].DefaultCellStyle = new DataGridViewCellStyle 
                        { 
                            Alignment = DataGridViewContentAlignment.MiddleRight, 
                            Format = "#,##0 đ",
                            Font = new Font("Segoe UI", 10f),
                            Padding = new Padding(12, 10, 12, 10)
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Silently handle errors in column configuration
                    System.Diagnostics.Debug.WriteLine($"Error in DataBindingComplete: {ex.Message}");
                }
            };
            pnlLeftContent.Controls.Add(dgvAllDichVu, 0, 1);

            // DataGridView dịch vụ đã chọn
            dgvSelectedDichVu = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(229, 231, 235),
                EnableHeadersVisualStyles = false
            };
            
            // Styling header
            dgvSelectedDichVu.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = Color.FromArgb(17, 24, 39),
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                Padding = new Padding(12, 12, 12, 12),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            dgvSelectedDichVu.ColumnHeadersHeight = 45;
            
            // Styling rows
            dgvSelectedDichVu.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(31, 41, 55),
                BackColor = Color.White,
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(17, 24, 39),
                Padding = new Padding(12, 10, 12, 10)
            };
            dgvSelectedDichVu.RowTemplate.Height = 40;
            
            dgvSelectedDichVu.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251)
            };
            
            dgvSelectedDichVu.DataBindingComplete += (s, e) =>
            {
                try
                {
                    if (dgvSelectedDichVu == null || dgvSelectedDichVu.Columns == null) return;
                    
                    if (dgvSelectedDichVu.Columns.Contains("dv_id"))
                        dgvSelectedDichVu.Columns["dv_id"].Visible = false;
                    if (dgvSelectedDichVu.Columns.Contains("ma_dv"))
                    {
                        dgvSelectedDichVu.Columns["ma_dv"].HeaderText = "Mã DV";
                        dgvSelectedDichVu.Columns["ma_dv"].FillWeight = 15;
                        dgvSelectedDichVu.Columns["ma_dv"].MinimumWidth = 100;
                        dgvSelectedDichVu.Columns["ma_dv"].Width = 100;
                    }
                    if (dgvSelectedDichVu.Columns.Contains("ten_dv"))
                    {
                        dgvSelectedDichVu.Columns["ten_dv"].HeaderText = "Tên dịch vụ";
                        dgvSelectedDichVu.Columns["ten_dv"].FillWeight = 55;
                        dgvSelectedDichVu.Columns["ten_dv"].MinimumWidth = 300;
                        dgvSelectedDichVu.Columns["ten_dv"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvSelectedDichVu.Columns["ten_dv"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    }
                    if (dgvSelectedDichVu.Columns.Contains("don_vi_tinh"))
                    {
                        dgvSelectedDichVu.Columns["don_vi_tinh"].HeaderText = "ĐVT";
                        dgvSelectedDichVu.Columns["don_vi_tinh"].FillWeight = 12;
                        dgvSelectedDichVu.Columns["don_vi_tinh"].MinimumWidth = 80;
                        dgvSelectedDichVu.Columns["don_vi_tinh"].Width = 80;
                    }
                    if (dgvSelectedDichVu.Columns.Contains("don_gia"))
                    {
                        dgvSelectedDichVu.Columns["don_gia"].HeaderText = "Đơn giá";
                        dgvSelectedDichVu.Columns["don_gia"].FillWeight = 18;
                        dgvSelectedDichVu.Columns["don_gia"].MinimumWidth = 130;
                        dgvSelectedDichVu.Columns["don_gia"].Width = 130;
                        dgvSelectedDichVu.Columns["don_gia"].DefaultCellStyle = new DataGridViewCellStyle 
                        { 
                            Alignment = DataGridViewContentAlignment.MiddleRight, 
                            Format = "#,##0 đ",
                            Font = new Font("Segoe UI", 10f),
                            Padding = new Padding(12, 10, 12, 10)
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Silently handle errors in column configuration
                    System.Diagnostics.Debug.WriteLine($"Error in DataBindingComplete: {ex.Message}");
                }
            };
            pnlRightContent.Controls.Add(dgvSelectedDichVu, 0, 1);

            // Panel nút bên trái
            var pnlLeftButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.Transparent,
                Padding = new Padding(12, 0, 12, 0)
            };
            btnThem = new Button 
            { 
                Text = "Thêm >>", 
                Size = new Size(120, 36),
                Location = new Point(0, 7),
                Font = new Font("Segoe UI Semibold", 10f),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnThem.Click += BtnThem_Click;
            pnlLeftButtons.Controls.Add(btnThem);
            pnlLeft.Controls.Add(pnlLeftButtons);

            // Panel nút bên phải
            var pnlRightButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(12, 7, 12, 7),
                AutoSize = false
            };
            
            btnDong = new Button 
            { 
                Text = "Đóng", 
                Size = new Size(100, 36),
                Font = new Font("Segoe UI Semibold", 10f),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel,
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 0, 0)
            };
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.FlatAppearance.MouseOverBackColor = Color.FromArgb(75, 85, 99);
            
            btnLuu = new Button 
            { 
                Text = "Lưu", 
                Size = new Size(100, 36),
                Font = new Font("Segoe UI Semibold", 10f),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 0, 0)
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 163, 74);
            
            btnXoa = new Button 
            { 
                Text = "Xóa", 
                Size = new Size(100, 36),
                Font = new Font("Segoe UI Semibold", 10f),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 0, 0)
            };
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            
            btnLuu.Click += BtnLuu_Click;
            btnXoa.Click += BtnXoa_Click;
            pnlRightButtons.Controls.AddRange(new Control[] { btnDong, btnLuu, btnXoa });
            pnlRight.Controls.Add(pnlRightButtons);
            
            // Thêm spacing giữa 2 panel bằng cách thêm margin
            pnlLeftWrapper.Margin = new Padding(0, 0, 10, 0);
            pnlRightWrapper.Margin = new Padding(10, 0, 0, 0);
        }

        private void LoadData()
        {
            try
            {
                // Load tất cả dịch vụ
                DataTable dtAll = _bll.GetAllDichVu();
                if (dtAll == null)
                {
                    dtAll = new DataTable();
                }
                dgvAllDichVu.DataSource = dtAll;

                // Load dịch vụ đã chọn
                DataTable dtSelected = _bll.GetDichVuTrongGoi(_goiId);
                if (dtSelected == null)
                {
                    dtSelected = new DataTable();
                }
                dgvSelectedDichVu.DataSource = dtSelected;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAllDichVu.CurrentRow == null)
                {
                    MessageBox.Show("Chọn một dịch vụ để thêm.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRowView row = (DataRowView)dgvAllDichVu.CurrentRow.DataBoundItem;
                string maDv = row["ma_dv"].ToString();
                string tenDv = row["ten_dv"].ToString();

                // Kiểm tra xem dịch vụ đã có trong danh sách chọn chưa
                DataTable dtSelected = (DataTable)dgvSelectedDichVu.DataSource;
                if (dtSelected != null && dtSelected.AsEnumerable().Any(r => r["ma_dv"].ToString() == maDv))
                {
                    MessageBox.Show($"Dịch vụ '{tenDv}' đã có trong danh sách.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Thêm vào DataTable
                if (dtSelected == null)
                {
                    dtSelected = new DataTable();
                    dtSelected.Columns.Add("dv_id", typeof(int));
                    dtSelected.Columns.Add("ma_dv", typeof(string));
                    dtSelected.Columns.Add("ten_dv", typeof(string));
                    dtSelected.Columns.Add("don_vi_tinh", typeof(string));
                    dtSelected.Columns.Add("don_gia", typeof(decimal));
                    dgvSelectedDichVu.DataSource = dtSelected;
                }

                DataRow newRow = dtSelected.NewRow();
                newRow["dv_id"] = row["dv_id"];
                newRow["ma_dv"] = row["ma_dv"];
                newRow["ten_dv"] = row["ten_dv"];
                newRow["don_vi_tinh"] = row["don_vi_tinh"];
                newRow["don_gia"] = row["don_gia"];
                dtSelected.Rows.Add(newRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dịch vụ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSelectedDichVu.CurrentRow == null)
                {
                    MessageBox.Show("Chọn một dịch vụ để xóa.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRowView row = (DataRowView)dgvSelectedDichVu.CurrentRow.DataBoundItem;
                string tenDv = row["ten_dv"].ToString();

                if (MessageBox.Show($"Xóa dịch vụ '{tenDv}' khỏi danh sách?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataTable dtSelected = (DataTable)dgvSelectedDichVu.DataSource;
                    if (dtSelected != null)
                    {
                        dtSelected.Rows.RemoveAt(dgvSelectedDichVu.CurrentRow.Index);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa dịch vụ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtSelected = (DataTable)dgvSelectedDichVu.DataSource;
                if (dtSelected == null || dtSelected.Rows.Count == 0)
                {
                    // Xóa tất cả dịch vụ khỏi gói
                    DataTable dtOld = _bll.GetDichVuTrongGoi(_goiId);
                    if (dtOld != null && dtOld.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtOld.Rows)
                        {
                            if (row["ma_dv"] != null && row["ma_dv"] != DBNull.Value)
                            {
                                _bll.XoaDichVuKhoiGoi(_goiId, row["ma_dv"].ToString());
                            }
                        }
                    }
                }
                else
                {
                    // Lấy danh sách dịch vụ cũ
                    DataTable dtOld = _bll.GetDichVuTrongGoi(_goiId);
                    var oldMaDvList = new List<string>();
                    if (dtOld != null && dtOld.Rows.Count > 0)
                    {
                        oldMaDvList = dtOld.AsEnumerable()
                            .Where(r => r["ma_dv"] != null && r["ma_dv"] != DBNull.Value)
                            .Select(r => r["ma_dv"].ToString())
                            .ToList();
                    }
                    
                    var newMaDvList = dtSelected.AsEnumerable()
                        .Where(r => r["ma_dv"] != null && r["ma_dv"] != DBNull.Value)
                        .Select(r => r["ma_dv"].ToString())
                        .ToList();

                    // Xóa các dịch vụ không còn trong danh sách mới
                    foreach (string maDv in oldMaDvList)
                    {
                        if (!newMaDvList.Contains(maDv))
                        {
                            _bll.XoaDichVuKhoiGoi(_goiId, maDv);
                        }
                    }

                    // Thêm các dịch vụ mới (chỉ thêm những dịch vụ chưa có trong danh sách cũ)
                    foreach (DataRow row in dtSelected.Rows)
                    {
                        if (row["ma_dv"] != null && row["ma_dv"] != DBNull.Value)
                        {
                            string maDv = row["ma_dv"].ToString();
                            if (!oldMaDvList.Contains(maDv))
                            {
                                _bll.ThemDichVuVaoGoi(_goiId, maDv);
                            }
                        }
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dịch vụ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

