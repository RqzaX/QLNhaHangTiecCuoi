namespace UI
{
    partial class FrmKho
    {
        /// <summary>
        /// Required designer variable.
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            VanThuan.UI.PillItem pillItem1 = new VanThuan.UI.PillItem();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            label1 = new Label();
            label2 = new Label();
            btnNhapKho = new UI.Controls.RoundedButton();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            label9 = new Label();
            label5 = new Label();
            roundedPanel3 = new UI.Controls.RoundedPanel();
            label8 = new Label();
            label4 = new Label();
            roundedPanel4 = new UI.Controls.RoundedPanel();
            label10 = new Label();
            label6 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            cbbTinhTrang = new UiControls.BorderComboBox();
            dgvKho = new DataGridView();
            dgvtxtTenNguyenLieu = new DataGridViewTextBoxColumn();
            dgvtxtDonVi = new DataGridViewTextBoxColumn();
            dgvtxtTonKho = new DataGridViewTextBoxColumn();
            dgvtxtTonToiThieu = new DataGridViewTextBoxColumn();
            dgvtxtDungTb = new DataGridViewTextBoxColumn();
            DgvtxtGiaTri = new DataGridViewTextBoxColumn();
            dgvtxtTrangThai = new DataGridViewTextBoxColumn();
            dgvtxtThaoTac = new DataGridViewTextBoxColumn();
            roundedButton1 = new UI.Controls.RoundedButton();
            btnChuyenKho = new UI.Controls.RoundedButton();
            txtSearch = new UI.Controls.RoundedTextBox();
            roundedPanel2.SuspendLayout();
            roundedPanel3.SuspendLayout();
            roundedPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKho).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(13, 9);
            label1.Name = "label1";
            label1.Size = new Size(159, 35);
            label1.TabIndex = 11;
            label1.Text = "Quản Lý Kho";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 45);
            label2.Name = "label2";
            label2.Size = new Size(250, 20);
            label2.TabIndex = 12;
            label2.Text = "Nhập - Xuất - Kiểm Kê - Chuyển Kho";
            // 
            // btnNhapKho
            // 
            btnNhapKho.BackColor = Color.White;
            btnNhapKho.BorderColor = Color.White;
            btnNhapKho.BorderThickness = 0;
            btnNhapKho.FlatStyle = FlatStyle.Flat;
            btnNhapKho.Font = new Font("Segoe UI Semibold", 10.5F);
            btnNhapKho.ForeColor = Color.Black;
            btnNhapKho.HoverBackColor = Color.Silver;
            btnNhapKho.Location = new Point(571, 45);
            btnNhapKho.Name = "btnNhapKho";
            btnNhapKho.Padding = new Padding(10, 6, 10, 6);
            btnNhapKho.PressedBackColor = Color.Silver;
            btnNhapKho.Size = new Size(142, 37);
            btnNhapKho.TabIndex = 13;
            btnNhapKho.Text = "Nhập Kho ";
            btnNhapKho.UseVisualStyleBackColor = false;
            btnNhapKho.Click += roundedButton1_Click;
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.FromArgb(192, 255, 192);
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(label9);
            roundedPanel2.Controls.Add(label5);
            roundedPanel2.Location = new Point(474, 126);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(12);
            roundedPanel2.Size = new Size(262, 141);
            roundedPanel2.TabIndex = 14;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(15, 81);
            label9.Name = "label9";
            label9.Size = new Size(17, 20);
            label9.TabIndex = 17;
            label9.Text = "0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 31);
            label5.Name = "label5";
            label5.Size = new Size(96, 20);
            label5.TabIndex = 17;
            label5.Text = "Sắp hết hàng";
            // 
            // roundedPanel3
            // 
            roundedPanel3.BackColor = Color.FromArgb(192, 255, 192);
            roundedPanel3.BorderThickness = 5;
            roundedPanel3.Controls.Add(label8);
            roundedPanel3.Controls.Add(label4);
            roundedPanel3.Location = new Point(87, 126);
            roundedPanel3.Name = "roundedPanel3";
            roundedPanel3.Padding = new Padding(12);
            roundedPanel3.Size = new Size(265, 141);
            roundedPanel3.TabIndex = 15;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 81);
            label8.Name = "label8";
            label8.Size = new Size(17, 20);
            label8.TabIndex = 17;
            label8.Text = "7";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 31);
            label4.Name = "label4";
            label4.Size = new Size(110, 20);
            label4.TabIndex = 17;
            label4.Text = "Tổng mặt hàng";
            // 
            // roundedPanel4
            // 
            roundedPanel4.BackColor = Color.FromArgb(192, 255, 192);
            roundedPanel4.BorderThickness = 5;
            roundedPanel4.Controls.Add(label10);
            roundedPanel4.Controls.Add(label6);
            roundedPanel4.Location = new Point(867, 126);
            roundedPanel4.Name = "roundedPanel4";
            roundedPanel4.Padding = new Padding(12);
            roundedPanel4.Size = new Size(267, 141);
            roundedPanel4.TabIndex = 16;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(15, 81);
            label10.Name = "label10";
            label10.Size = new Size(17, 20);
            label10.TabIndex = 17;
            label10.Text = "0";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(15, 31);
            label6.Name = "label6";
            label6.Size = new Size(70, 20);
            label6.TabIndex = 17;
            label6.Text = "Hết hàng";
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Nhập Kho";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Location = new Point(38, 273);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(412, 55);
            segmentedPill1.TabIndex = 17;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // cbbTinhTrang
            // 
            cbbTinhTrang.DrawMode = DrawMode.OwnerDrawFixed;
            cbbTinhTrang.FormattingEnabled = true;
            cbbTinhTrang.IntegralHeight = false;
            cbbTinhTrang.ItemHeight = 26;
            cbbTinhTrang.Items.AddRange(new object[] { "Tất cả", "Đủ Hàng", "Sắp Hết", "Hết Hàng" });
            cbbTinhTrang.Location = new Point(611, 330);
            cbbTinhTrang.Name = "cbbTinhTrang";
            cbbTinhTrang.Size = new Size(180, 32);
            cbbTinhTrang.TabIndex = 18;
            // 
            // dgvKho
            // 
            dgvKho.AllowUserToAddRows = false;
            dgvKho.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvKho.BackgroundColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvKho.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvKho.ColumnHeadersHeight = 60;
            dgvKho.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvKho.Columns.AddRange(new DataGridViewColumn[] { dgvtxtTenNguyenLieu, dgvtxtDonVi, dgvtxtTonKho, dgvtxtTonToiThieu, dgvtxtDungTb, DgvtxtGiaTri, dgvtxtTrangThai, dgvtxtThaoTac });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(255, 255, 192);
            dataGridViewCellStyle2.SelectionForeColor = Color.Olive;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvKho.DefaultCellStyle = dataGridViewCellStyle2;
            dgvKho.Location = new Point(12, 378);
            dgvKho.Name = "dgvKho";
            dgvKho.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 255, 192);
            dataGridViewCellStyle3.SelectionForeColor = Color.Olive;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvKho.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvKho.RowHeadersVisible = false;
            dgvKho.RowHeadersWidth = 51;
            dgvKho.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvKho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKho.Size = new Size(1148, 473);
            dgvKho.TabIndex = 19;
            dgvKho.CellClick += dgvKho_CellClick;
            dgvKho.CellPainting += dgvKho_CellPainting;
            // 
            // dgvtxtTenNguyenLieu
            // 
            dgvtxtTenNguyenLieu.HeaderText = "Tên Nguyên Liệu";
            dgvtxtTenNguyenLieu.MinimumWidth = 6;
            dgvtxtTenNguyenLieu.Name = "dgvtxtTenNguyenLieu";
            dgvtxtTenNguyenLieu.ReadOnly = true;
            dgvtxtTenNguyenLieu.Width = 143;
            // 
            // dgvtxtDonVi
            // 
            dgvtxtDonVi.HeaderText = "Đơn Vị";
            dgvtxtDonVi.MinimumWidth = 6;
            dgvtxtDonVi.Name = "dgvtxtDonVi";
            dgvtxtDonVi.ReadOnly = true;
            dgvtxtDonVi.Width = 143;
            // 
            // dgvtxtTonKho
            // 
            dgvtxtTonKho.HeaderText = "Tồn Kho";
            dgvtxtTonKho.MinimumWidth = 6;
            dgvtxtTonKho.Name = "dgvtxtTonKho";
            dgvtxtTonKho.ReadOnly = true;
            dgvtxtTonKho.Width = 143;
            // 
            // dgvtxtTonToiThieu
            // 
            dgvtxtTonToiThieu.HeaderText = "Tồn Tối Thiểu";
            dgvtxtTonToiThieu.MinimumWidth = 6;
            dgvtxtTonToiThieu.Name = "dgvtxtTonToiThieu";
            dgvtxtTonToiThieu.ReadOnly = true;
            dgvtxtTonToiThieu.Width = 144;
            // 
            // dgvtxtDungTb
            // 
            dgvtxtDungTb.HeaderText = "Dùng TB/Ngày";
            dgvtxtDungTb.MinimumWidth = 6;
            dgvtxtDungTb.Name = "dgvtxtDungTb";
            dgvtxtDungTb.ReadOnly = true;
            dgvtxtDungTb.Width = 143;
            // 
            // DgvtxtGiaTri
            // 
            DgvtxtGiaTri.HeaderText = "Giá Trị";
            DgvtxtGiaTri.MinimumWidth = 6;
            DgvtxtGiaTri.Name = "DgvtxtGiaTri";
            DgvtxtGiaTri.ReadOnly = true;
            DgvtxtGiaTri.Width = 143;
            // 
            // dgvtxtTrangThai
            // 
            dgvtxtTrangThai.HeaderText = "Trạng Thái";
            dgvtxtTrangThai.MinimumWidth = 6;
            dgvtxtTrangThai.Name = "dgvtxtTrangThai";
            dgvtxtTrangThai.ReadOnly = true;
            dgvtxtTrangThai.Width = 143;
            // 
            // dgvtxtThaoTac
            // 
            dgvtxtThaoTac.HeaderText = "Thao Tác";
            dgvtxtThaoTac.MinimumWidth = 6;
            dgvtxtThaoTac.Name = "dgvtxtThaoTac";
            dgvtxtThaoTac.ReadOnly = true;
            dgvtxtThaoTac.Width = 143;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.White;
            roundedButton1.BorderColor = Color.FromArgb(224, 224, 224);
            roundedButton1.BorderThickness = 0;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.HoverBackColor = Color.FromArgb(224, 224, 224);
            roundedButton1.Location = new Point(719, 45);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.PressedBackColor = Color.Silver;
            roundedButton1.Size = new Size(142, 37);
            roundedButton1.TabIndex = 20;
            roundedButton1.Text = "Xuất Kho";
            roundedButton1.UseVisualStyleBackColor = false;
            roundedButton1.Click += roundedButton1_Click_1;
            // 
            // btnChuyenKho
            // 
            btnChuyenKho.BackColor = Color.White;
            btnChuyenKho.BorderColor = Color.White;
            btnChuyenKho.BorderThickness = 0;
            btnChuyenKho.FlatAppearance.BorderSize = 0;
            btnChuyenKho.FlatStyle = FlatStyle.Flat;
            btnChuyenKho.Font = new Font("Segoe UI Semibold", 10.5F);
            btnChuyenKho.ForeColor = Color.Black;
            btnChuyenKho.HoverBackColor = Color.FromArgb(224, 224, 224);
            btnChuyenKho.Location = new Point(867, 45);
            btnChuyenKho.Name = "btnChuyenKho";
            btnChuyenKho.Padding = new Padding(10, 6, 10, 6);
            btnChuyenKho.PressedBackColor = Color.Silver;
            btnChuyenKho.Size = new Size(145, 37);
            btnChuyenKho.TabIndex = 21;
            btnChuyenKho.Text = "Chuyển Kho";
            btnChuyenKho.UseVisualStyleBackColor = false;
            btnChuyenKho.Click += btnChuyenKho_Click;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.White;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.ForeColor = Color.Black;
            txtSearch.Location = new Point(38, 330);
            txtSearch.Name = "txtSearch";
            txtSearch.Padding = new Padding(10, 8, 10, 8);
            txtSearch.Size = new Size(518, 42);
            txtSearch.TabIndex = 1;
            // 
            // FrmKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1172, 853);
            Controls.Add(btnChuyenKho);
            Controls.Add(roundedButton1);
            Controls.Add(dgvKho);
            Controls.Add(cbbTinhTrang);
            Controls.Add(segmentedPill1);
            Controls.Add(roundedPanel4);
            Controls.Add(roundedPanel3);
            Controls.Add(roundedPanel2);
            Controls.Add(btnNhapKho);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtSearch);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmKho";
            Text = "Tồn Kho";
            Load += FrmKho_Load;
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            roundedPanel3.ResumeLayout(false);
            roundedPanel3.PerformLayout();
            roundedPanel4.ResumeLayout(false);
            roundedPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKho).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private Controls.RoundedButton btnNhapKho;
        private Controls.RoundedPanel roundedPanel2;
        private Controls.RoundedPanel roundedPanel3;
        private Controls.RoundedPanel roundedPanel4;
        private Label label5;
        private Label label4;
        private Label label8;
        private Label label6;
        private Label label9;
        private Label label10;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private UiControls.BorderComboBox cbbTinhTrang;
        private DataGridView dgvKho;
        private DataGridViewTextBoxColumn dgvtxtTenNguyenLieu;
        private DataGridViewTextBoxColumn dgvtxtDonVi;
        private DataGridViewTextBoxColumn dgvtxtTonKho;
        private DataGridViewTextBoxColumn dgvtxtTonToiThieu;
        private DataGridViewTextBoxColumn dgvtxtDungTb;
        private DataGridViewTextBoxColumn DgvtxtGiaTri;
        private DataGridViewTextBoxColumn dgvtxtTrangThai;
        private DataGridViewTextBoxColumn dgvtxtThaoTac;
        private Controls.RoundedButton roundedButton1;
        private Controls.RoundedButton btnChuyenKho;
        private Controls.RoundedTextBox txtSearch;
    }
}