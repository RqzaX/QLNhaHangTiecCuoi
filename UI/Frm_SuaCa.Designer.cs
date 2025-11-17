using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class Frm_SuaCa
    {
   
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

      
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.ResumeLayout(false);
            BuildUI();
        }

        #endregion

     
        /// </summary>
        private void BuildUI()
        {
            // Form properties
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(1000, 650);
            this.MinimumSize = new Size(900, 550);
            this.BackColor = Color.FromArgb(248, 250, 252);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 1
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            this.Controls.Add(root);

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

            // Label "Nhân viên chưa có trong ca"
            var lblAll = new Label
            {
                Text = "Nhân viên chưa có trong ca",
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

            // Label "Nhân viên trong ca"
            var lblSelected = new Label
            {
                Text = "Nhân viên trong ca",
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

            // DataGridView nhân viên chưa có trong ca
            this.dgvNhanVienChuaTrongCa = new DataGridView
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
            this.dgvNhanVienChuaTrongCa.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = Color.FromArgb(17, 24, 39),
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                Padding = new Padding(12, 12, 12, 12),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            this.dgvNhanVienChuaTrongCa.ColumnHeadersHeight = 45;

            // Styling rows
            this.dgvNhanVienChuaTrongCa.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(31, 41, 55),
                BackColor = Color.White,
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(17, 24, 39),
                Padding = new Padding(12, 10, 12, 10)
            };
            this.dgvNhanVienChuaTrongCa.RowTemplate.Height = 40;

            this.dgvNhanVienChuaTrongCa.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251)
            };

            this.dgvNhanVienChuaTrongCa.DataBindingComplete += (s, e) =>
            {
                try
                {
                    if (this.dgvNhanVienChuaTrongCa == null || this.dgvNhanVienChuaTrongCa.Columns == null) return;

                    if (this.dgvNhanVienChuaTrongCa.Columns.Contains("nguoi_dung_id"))
                        this.dgvNhanVienChuaTrongCa.Columns["nguoi_dung_id"].Visible = false;
                    if (this.dgvNhanVienChuaTrongCa.Columns.Contains("tai_khoan"))
                    {
                        this.dgvNhanVienChuaTrongCa.Columns["tai_khoan"].HeaderText = "Tài khoản";
                        this.dgvNhanVienChuaTrongCa.Columns["tai_khoan"].FillWeight = 25;
                        this.dgvNhanVienChuaTrongCa.Columns["tai_khoan"].MinimumWidth = 120;
                    }
                    if (this.dgvNhanVienChuaTrongCa.Columns.Contains("ho_ten"))
                    {
                        this.dgvNhanVienChuaTrongCa.Columns["ho_ten"].HeaderText = "Họ tên";
                        this.dgvNhanVienChuaTrongCa.Columns["ho_ten"].FillWeight = 50;
                        this.dgvNhanVienChuaTrongCa.Columns["ho_ten"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    if (this.dgvNhanVienChuaTrongCa.Columns.Contains("chuc_vu"))
                    {
                        this.dgvNhanVienChuaTrongCa.Columns["chuc_vu"].HeaderText = "Chức vụ";
                        this.dgvNhanVienChuaTrongCa.Columns["chuc_vu"].FillWeight = 25;
                        this.dgvNhanVienChuaTrongCa.Columns["chuc_vu"].MinimumWidth = 120;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in DataBindingComplete: {ex.Message}");
                }
            };
            pnlLeftContent.Controls.Add(this.dgvNhanVienChuaTrongCa, 0, 1);

            // DataGridView nhân viên trong ca
            this.dgvNhanVienTrongCa = new DataGridView
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
            this.dgvNhanVienTrongCa.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251),
                ForeColor = Color.FromArgb(17, 24, 39),
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                Padding = new Padding(12, 12, 12, 12),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            this.dgvNhanVienTrongCa.ColumnHeadersHeight = 45;

            // Styling rows
            this.dgvNhanVienTrongCa.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(31, 41, 55),
                BackColor = Color.White,
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(17, 24, 39),
                Padding = new Padding(12, 10, 12, 10)
            };
            this.dgvNhanVienTrongCa.RowTemplate.Height = 40;

            this.dgvNhanVienTrongCa.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(249, 250, 251)
            };

            this.dgvNhanVienTrongCa.DataBindingComplete += (s, e) =>
            {
                try
                {
                    if (this.dgvNhanVienTrongCa == null || this.dgvNhanVienTrongCa.Columns == null) return;

                    if (this.dgvNhanVienTrongCa.Columns.Contains("nguoi_dung_ca_id"))
                        this.dgvNhanVienTrongCa.Columns["nguoi_dung_ca_id"].Visible = false;
                    if (this.dgvNhanVienTrongCa.Columns.Contains("nguoi_dung_id"))
                        this.dgvNhanVienTrongCa.Columns["nguoi_dung_id"].Visible = false;
                    if (this.dgvNhanVienTrongCa.Columns.Contains("tai_khoan"))
                    {
                        this.dgvNhanVienTrongCa.Columns["tai_khoan"].HeaderText = "Tài khoản";
                        this.dgvNhanVienTrongCa.Columns["tai_khoan"].FillWeight = 25;
                        this.dgvNhanVienTrongCa.Columns["tai_khoan"].MinimumWidth = 120;
                    }
                    if (this.dgvNhanVienTrongCa.Columns.Contains("ho_ten"))
                    {
                        this.dgvNhanVienTrongCa.Columns["ho_ten"].HeaderText = "Họ tên";
                        this.dgvNhanVienTrongCa.Columns["ho_ten"].FillWeight = 50;
                        this.dgvNhanVienTrongCa.Columns["ho_ten"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    if (this.dgvNhanVienTrongCa.Columns.Contains("chuc_vu"))
                    {
                        this.dgvNhanVienTrongCa.Columns["chuc_vu"].HeaderText = "Chức vụ";
                        this.dgvNhanVienTrongCa.Columns["chuc_vu"].FillWeight = 25;
                        this.dgvNhanVienTrongCa.Columns["chuc_vu"].MinimumWidth = 120;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in DataBindingComplete: {ex.Message}");
                }
            };
            pnlRightContent.Controls.Add(this.dgvNhanVienTrongCa, 0, 1);

            // Panel nút bên trái
            var pnlLeftButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.Transparent,
                Padding = new Padding(12, 0, 12, 0)
            };
            this.btnThem = new Button
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
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            this.btnThem.Click += BtnThem_Click;
            pnlLeftButtons.Controls.Add(this.btnThem);
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

            this.btnDong = new Button
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
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.FlatAppearance.MouseOverBackColor = Color.FromArgb(75, 85, 99);

            this.btnLuu = new Button
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
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 163, 74);

            this.btnXoa = new Button
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
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);

            this.btnLuu.Click += BtnLuu_Click;
            this.btnXoa.Click += BtnXoa_Click;
            pnlRightButtons.Controls.AddRange(new Control[] { this.btnDong, this.btnLuu, this.btnXoa });
            pnlRight.Controls.Add(pnlRightButtons);

            // Thêm spacing giữa 2 panel
            pnlLeftWrapper.Margin = new Padding(0, 0, 10, 0);
            pnlRightWrapper.Margin = new Padding(10, 0, 0, 0);
        }
    }
}