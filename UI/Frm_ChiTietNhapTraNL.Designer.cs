namespace UI
{
    partial class Frm_ChiTietNhapTraNL
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlBorder = new Guna.UI2.WinForms.Guna2Panel();
            pnlMain = new Guna.UI2.WinForms.Guna2Panel();
            pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            lblTieuDe = new Label();
            lblMaPhieu = new Label();
            pnlThongTin = new Guna.UI2.WinForms.Guna2Panel();
            lblNgayLabel = new Label();
            lblNgay = new Label();
            lblGioLabel = new Label();
            lblGio = new Label();
            lblNhanVienLabel = new Label();
            lblNhanVien = new Label();
            lblTrangThaiLabel = new Label();
            lblTrangThai = new Label();
            pnlGhiChu = new Guna.UI2.WinForms.Guna2Panel();
            lblGhiChuLabel = new Label();
            lblGhiChu = new Label();
            dgvChiTiet = new DataGridView();
            colSTT = new DataGridViewTextBoxColumn();
            colMaNL = new DataGridViewTextBoxColumn();
            colTenNL = new DataGridViewTextBoxColumn();
            colTon = new DataGridViewTextBoxColumn();
            colSoLuong = new DataGridViewTextBoxColumn();
            colConLai = new DataGridViewTextBoxColumn();
            colDVT = new DataGridViewTextBoxColumn();
            colGhiChu = new DataGridViewTextBoxColumn();
            pnlFooter = new Guna.UI2.WinForms.Guna2Panel();
            btnHuy = new Guna.UI2.WinForms.Guna2Button();
            btnDong = new Guna.UI2.WinForms.Guna2Button();
            pnlBorder.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlThongTin.SuspendLayout();
            pnlGhiChu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBorder
            // 
            pnlBorder.BorderColor = Color.Black;
            pnlBorder.BorderThickness = 2;
            pnlBorder.Controls.Add(pnlMain);
            pnlBorder.CustomizableEdges = customizableEdges11;
            pnlBorder.Dock = DockStyle.Fill;
            pnlBorder.Location = new Point(10, 10);
            pnlBorder.Name = "pnlBorder";
            pnlBorder.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnlBorder.Size = new Size(906, 645);
            pnlBorder.TabIndex = 5;
            // 
            // pnlMain
            // 
            pnlMain.AutoScroll = true;
            pnlMain.Controls.Add(pnlHeader);
            pnlMain.Controls.Add(pnlThongTin);
            pnlMain.Controls.Add(pnlGhiChu);
            pnlMain.Controls.Add(dgvChiTiet);
            pnlMain.Controls.Add(pnlFooter);
            pnlMain.CustomizableEdges = customizableEdges9;
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.ShadowDecoration.CustomizableEdges = customizableEdges10;
            pnlMain.Size = new Size(906, 645);
            pnlMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblTieuDe);
            pnlHeader.Controls.Add(lblMaPhieu);
            pnlHeader.CustomizableEdges = customizableEdges1;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.FillColor = Color.White;
            pnlHeader.Location = new Point(0, 180);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges2;
            pnlHeader.Size = new Size(906, 80);
            pnlHeader.TabIndex = 0;
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoSize = true;
            lblTieuDe.BackColor = Color.Transparent;
            lblTieuDe.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            lblTieuDe.Location = new Point(20, 15);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(181, 37);
            lblTieuDe.TabIndex = 0;
            lblTieuDe.Text = "Chi tiết phiếu";
            // 
            // lblMaPhieu
            // 
            lblMaPhieu.AutoSize = true;
            lblMaPhieu.BackColor = Color.Transparent;
            lblMaPhieu.Font = new Font("Segoe UI", 10F);
            lblMaPhieu.ForeColor = Color.FromArgb(107, 114, 128);
            lblMaPhieu.Location = new Point(20, 52);
            lblMaPhieu.Name = "lblMaPhieu";
            lblMaPhieu.Size = new Size(98, 23);
            lblMaPhieu.TabIndex = 1;
            lblMaPhieu.Text = "Mã phiếu: -";
            // 
            // pnlThongTin
            // 
            pnlThongTin.Controls.Add(lblNgayLabel);
            pnlThongTin.Controls.Add(lblNgay);
            pnlThongTin.Controls.Add(lblGioLabel);
            pnlThongTin.Controls.Add(lblGio);
            pnlThongTin.Controls.Add(lblNhanVienLabel);
            pnlThongTin.Controls.Add(lblNhanVien);
            pnlThongTin.Controls.Add(lblTrangThaiLabel);
            pnlThongTin.Controls.Add(lblTrangThai);
            pnlThongTin.CustomizableEdges = customizableEdges3;
            pnlThongTin.Dock = DockStyle.Top;
            pnlThongTin.FillColor = Color.White;
            pnlThongTin.Location = new Point(0, 60);
            pnlThongTin.Name = "pnlThongTin";
            pnlThongTin.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlThongTin.Size = new Size(906, 120);
            pnlThongTin.TabIndex = 1;
            // 
            // lblNgayLabel
            // 
            lblNgayLabel.AutoSize = true;
            lblNgayLabel.BackColor = Color.Transparent;
            lblNgayLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblNgayLabel.Location = new Point(20, 15);
            lblNgayLabel.Name = "lblNgayLabel";
            lblNgayLabel.Size = new Size(55, 23);
            lblNgayLabel.TabIndex = 0;
            lblNgayLabel.Text = "Ngày:";
            // 
            // lblNgay
            // 
            lblNgay.AutoSize = true;
            lblNgay.BackColor = Color.Transparent;
            lblNgay.Font = new Font("Segoe UI", 10F);
            lblNgay.Location = new Point(80, 15);
            lblNgay.Name = "lblNgay";
            lblNgay.Size = new Size(17, 23);
            lblNgay.TabIndex = 1;
            lblNgay.Text = "-";
            // 
            // lblGioLabel
            // 
            lblGioLabel.AutoSize = true;
            lblGioLabel.BackColor = Color.Transparent;
            lblGioLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblGioLabel.Location = new Point(200, 15);
            lblGioLabel.Name = "lblGioLabel";
            lblGioLabel.Size = new Size(40, 23);
            lblGioLabel.TabIndex = 2;
            lblGioLabel.Text = "Giờ:";
            // 
            // lblGio
            // 
            lblGio.AutoSize = true;
            lblGio.BackColor = Color.Transparent;
            lblGio.Font = new Font("Segoe UI", 10F);
            lblGio.Location = new Point(250, 15);
            lblGio.Name = "lblGio";
            lblGio.Size = new Size(17, 23);
            lblGio.TabIndex = 3;
            lblGio.Text = "-";
            // 
            // lblNhanVienLabel
            // 
            lblNhanVienLabel.AutoSize = true;
            lblNhanVienLabel.BackColor = Color.Transparent;
            lblNhanVienLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblNhanVienLabel.Location = new Point(20, 50);
            lblNhanVienLabel.Name = "lblNhanVienLabel";
            lblNhanVienLabel.Size = new Size(93, 23);
            lblNhanVienLabel.TabIndex = 4;
            lblNhanVienLabel.Text = "Nhân viên:";
            // 
            // lblNhanVien
            // 
            lblNhanVien.AutoSize = true;
            lblNhanVien.BackColor = Color.Transparent;
            lblNhanVien.Font = new Font("Segoe UI", 10F);
            lblNhanVien.Location = new Point(125, 50);
            lblNhanVien.Name = "lblNhanVien";
            lblNhanVien.Size = new Size(17, 23);
            lblNhanVien.TabIndex = 5;
            lblNhanVien.Text = "-";
            // 
            // lblTrangThaiLabel
            // 
            lblTrangThaiLabel.AutoSize = true;
            lblTrangThaiLabel.BackColor = Color.Transparent;
            lblTrangThaiLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblTrangThaiLabel.Location = new Point(20, 85);
            lblTrangThaiLabel.Name = "lblTrangThaiLabel";
            lblTrangThaiLabel.Size = new Size(91, 23);
            lblTrangThaiLabel.TabIndex = 6;
            lblTrangThaiLabel.Text = "Trạng thái:";
            // 
            // lblTrangThai
            // 
            lblTrangThai.AutoSize = true;
            lblTrangThai.BackColor = Color.Transparent;
            lblTrangThai.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblTrangThai.Location = new Point(125, 85);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(17, 23);
            lblTrangThai.TabIndex = 7;
            lblTrangThai.Text = "-";
            // 
            // pnlGhiChu
            // 
            pnlGhiChu.Controls.Add(lblGhiChuLabel);
            pnlGhiChu.Controls.Add(lblGhiChu);
            pnlGhiChu.CustomizableEdges = customizableEdges5;
            pnlGhiChu.Dock = DockStyle.Top;
            pnlGhiChu.FillColor = Color.White;
            pnlGhiChu.Location = new Point(0, 0);
            pnlGhiChu.Name = "pnlGhiChu";
            pnlGhiChu.ShadowDecoration.CustomizableEdges = customizableEdges6;
            pnlGhiChu.Size = new Size(906, 60);
            pnlGhiChu.TabIndex = 2;
            pnlGhiChu.Visible = false;
            // 
            // lblGhiChuLabel
            // 
            lblGhiChuLabel.AutoSize = true;
            lblGhiChuLabel.BackColor = Color.Transparent;
            lblGhiChuLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblGhiChuLabel.Location = new Point(20, 15);
            lblGhiChuLabel.Name = "lblGhiChuLabel";
            lblGhiChuLabel.Size = new Size(73, 23);
            lblGhiChuLabel.TabIndex = 0;
            lblGhiChuLabel.Text = "Ghi chú:";
            // 
            // lblGhiChu
            // 
            lblGhiChu.AutoSize = true;
            lblGhiChu.BackColor = Color.Transparent;
            lblGhiChu.Font = new Font("Segoe UI", 10F);
            lblGhiChu.Location = new Point(100, 15);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(17, 23);
            lblGhiChu.TabIndex = 1;
            lblGhiChu.Text = "-";
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            dgvChiTiet.BackgroundColor = Color.White;
            dgvChiTiet.BorderStyle = BorderStyle.None;
            dgvChiTiet.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Padding(10, 0, 0, 0);
            dataGridViewCellStyle1.SelectionBackColor = Color.White;
            dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvChiTiet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvChiTiet.ColumnHeadersHeight = 45;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiTiet.Columns.AddRange(new DataGridViewColumn[] { colSTT, colMaNL, colTenNL, colTon, colSoLuong, colConLai, colDVT, colGhiChu });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 11.25F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new Padding(10, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvChiTiet.DefaultCellStyle = dataGridViewCellStyle2;
            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.GridColor = Color.FromArgb(240, 240, 240);
            dgvChiTiet.Location = new Point(0, 262);
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11.25F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.Black;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvChiTiet.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.RowTemplate.Height = 40;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Size = new Size(903, 321);
            dgvChiTiet.TabIndex = 3;
            // 
            // colSTT
            // 
            colSTT.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSTT.FillWeight = 50F;
            colSTT.HeaderText = "STT";
            colSTT.MinimumWidth = 50;
            colSTT.Name = "colSTT";
            colSTT.ReadOnly = true;
            colSTT.Width = 50;
            // 
            // colMaNL
            // 
            colMaNL.FillWeight = 120F;
            colMaNL.HeaderText = "Mã NL";
            colMaNL.MinimumWidth = 100;
            colMaNL.Name = "colMaNL";
            colMaNL.ReadOnly = true;
            // 
            // colTenNL
            // 
            colTenNL.FillWeight = 250F;
            colTenNL.HeaderText = "Tên nguyên liệu";
            colTenNL.MinimumWidth = 200;
            colTenNL.Name = "colTenNL";
            colTenNL.ReadOnly = true;
            colTenNL.Width = 200;
            // 
            // colTon
            // 
            colTon.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colTon.FillWeight = 120F;
            colTon.HeaderText = "Tồn";
            colTon.MinimumWidth = 100;
            colTon.Name = "colTon";
            colTon.ReadOnly = true;
            colTon.Visible = false;
            colTon.Width = 120;
            // 
            // colSoLuong
            // 
            colSoLuong.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colSoLuong.FillWeight = 120F;
            colSoLuong.HeaderText = "Số lượng";
            colSoLuong.MinimumWidth = 100;
            colSoLuong.Name = "colSoLuong";
            colSoLuong.ReadOnly = true;
            colSoLuong.Width = 120;
            // 
            // colConLai
            // 
            colConLai.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colConLai.FillWeight = 120F;
            colConLai.HeaderText = "Còn lại";
            colConLai.MinimumWidth = 100;
            colConLai.Name = "colConLai";
            colConLai.ReadOnly = true;
            colConLai.Visible = false;
            colConLai.Width = 120;
            // 
            // colDVT
            // 
            colDVT.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colDVT.FillWeight = 80F;
            colDVT.HeaderText = "ĐVT";
            colDVT.MinimumWidth = 60;
            colDVT.Name = "colDVT";
            colDVT.ReadOnly = true;
            colDVT.Width = 80;
            // 
            // colGhiChu
            // 
            colGhiChu.FillWeight = 200F;
            colGhiChu.HeaderText = "Ghi chú";
            colGhiChu.MinimumWidth = 150;
            colGhiChu.Name = "colGhiChu";
            colGhiChu.ReadOnly = true;
            colGhiChu.Width = 150;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(btnHuy);
            pnlFooter.Controls.Add(btnDong);
            pnlFooter.CustomizableEdges = customizableEdges7;
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.FillColor = Color.White;
            pnlFooter.Location = new Point(0, 588);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.ShadowDecoration.CustomizableEdges = customizableEdges8;
            pnlFooter.Size = new Size(906, 57);
            pnlFooter.TabIndex = 4;
            // 
            // btnHuy
            // 
            btnHuy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnHuy.Animated = true;
            btnHuy.BorderColor = Color.FromArgb(239, 68, 68);
            btnHuy.BorderRadius = 8;
            btnHuy.BorderThickness = 1;
            btnHuy.CustomizableEdges = customizableEdges1;
            btnHuy.DisabledState.BorderColor = Color.DarkGray;
            btnHuy.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHuy.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHuy.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHuy.FillColor = Color.White;
            btnHuy.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            btnHuy.ForeColor = Color.FromArgb(239, 68, 68);
            btnHuy.Location = new Point(727, 14);
            btnHuy.Name = "btnHuy";
            btnHuy.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnHuy.Size = new Size(85, 40);
            btnHuy.TabIndex = 1;
            btnHuy.Text = "Xóa";
            btnHuy.Visible = false;
            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDong.Animated = true;
            btnDong.BorderRadius = 8;
            btnDong.CustomizableEdges = customizableEdges3;
            btnDong.DisabledState.BorderColor = Color.DarkGray;
            btnDong.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDong.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDong.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDong.FillColor = Color.FromArgb(33, 42, 57);
            btnDong.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold);
            btnDong.ForeColor = Color.White;
            btnDong.Location = new Point(818, 14);
            btnDong.Name = "btnDong";
            btnDong.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnDong.Size = new Size(85, 40);
            btnDong.TabIndex = 0;
            btnDong.Text = "Đóng";
            // 
            // Frm_ChiTietNhapTraNL
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(926, 665);
            Controls.Add(pnlBorder);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Frm_ChiTietNhapTraNL";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết phiếu nhập/trả nguyên liệu";
            pnlBorder.ResumeLayout(false);
            pnlMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlThongTin.ResumeLayout(false);
            pnlThongTin.PerformLayout();
            pnlGhiChu.ResumeLayout(false);
            pnlGhiChu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlMain;
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private Label lblTieuDe;
        private Label lblMaPhieu;
        private Guna.UI2.WinForms.Guna2Panel pnlThongTin;
        private Label lblNgayLabel;
        private Label lblNgay;
        private Label lblGioLabel;
        private Label lblGio;
        private Label lblNhanVienLabel;
        private Label lblNhanVien;
        private Label lblTrangThaiLabel;
        private Label lblTrangThai;
        private Guna.UI2.WinForms.Guna2Panel pnlGhiChu;
        private Label lblGhiChuLabel;
        private Label lblGhiChu;
        private DataGridView dgvChiTiet;
        private DataGridViewTextBoxColumn colSTT;
        private DataGridViewTextBoxColumn colMaNL;
        private DataGridViewTextBoxColumn colTenNL;
        private DataGridViewTextBoxColumn colTon;
        private DataGridViewTextBoxColumn colSoLuong;
        private DataGridViewTextBoxColumn colConLai;
        private DataGridViewTextBoxColumn colDVT;
        private DataGridViewTextBoxColumn colGhiChu;
        private Guna.UI2.WinForms.Guna2Panel pnlFooter;
        private Guna.UI2.WinForms.Guna2Button btnDong;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private Guna.UI2.WinForms.Guna2Panel pnlBorder;
    }
}

