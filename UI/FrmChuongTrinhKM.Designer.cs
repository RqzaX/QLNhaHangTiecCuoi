namespace UI
{
    partial class FrmChuongTrinhKM
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            VanThuan.UI.PillItem pillItem1 = new VanThuan.UI.PillItem();
            dgvKhuyenMai = new DataGridView();
            TenCT = new DataGridViewTextBoxColumn();
            MaKM = new DataGridViewTextBoxColumn();
            Loai = new DataGridViewTextBoxColumn();
            GiaTri = new DataGridViewTextBoxColumn();
            DieuKien = new DataGridViewTextBoxColumn();
            ThoiGian = new DataGridViewTextBoxColumn();
            DaDung = new DataGridViewTextBoxColumn();
            TrangThai = new DataGridViewTextBoxColumn();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            label1 = new Label();
            label2 = new Label();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label7 = new Label();
            label3 = new Label();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            label4 = new Label();
            label5 = new Label();
            roundedPanel4 = new UI.Controls.RoundedPanel();
            label9 = new Label();
            label10 = new Label();
            btnTaoCTKM = new UI.Controls.RoundedButton();
            btnVoucher = new UI.Controls.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)dgvKhuyenMai).BeginInit();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
            roundedPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // dgvKhuyenMai
            // 
            dgvKhuyenMai.AllowUserToAddRows = false;
            dgvKhuyenMai.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvKhuyenMai.BackgroundColor = SystemColors.ControlLightLight;
            dgvKhuyenMai.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvKhuyenMai.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvKhuyenMai.ColumnHeadersHeight = 60;
            dgvKhuyenMai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new Padding(12, 6, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(192, 255, 255);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(0, 192, 192);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvKhuyenMai.DefaultCellStyle = dataGridViewCellStyle2;
            dgvKhuyenMai.Location = new Point(12, 355);
            dgvKhuyenMai.Name = "dgvKhuyenMai";
            dgvKhuyenMai.ReadOnly = true;
            dgvKhuyenMai.RowHeadersVisible = false;
            dgvKhuyenMai.RowHeadersWidth = 51;
            dgvKhuyenMai.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvKhuyenMai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhuyenMai.Size = new Size(1176, 497);
            dgvKhuyenMai.TabIndex = 0;
            dgvKhuyenMai.CellDoubleClick += dgvKhuyenMai_CellDoubleClick;
            dgvKhuyenMai.CellPainting += dgvKhuyenMai_CellPainting_1;
            // 
            // TenCT
            // 
            TenCT.HeaderText = "Tên Chương Trình";
            TenCT.MinimumWidth = 6;
            TenCT.Name = "TenCT";
            TenCT.ReadOnly = true;
            TenCT.Width = 180;
            // 
            // MaKM
            // 
            MaKM.HeaderText = "Mã KM";
            MaKM.MinimumWidth = 6;
            MaKM.Name = "MaKM";
            MaKM.ReadOnly = true;
            MaKM.Width = 146;
            // 
            // Loai
            // 
            Loai.HeaderText = "Loại";
            Loai.MinimumWidth = 6;
            Loai.Name = "Loai";
            Loai.ReadOnly = true;
            Loai.Width = 147;
            // 
            // GiaTri
            // 
            GiaTri.HeaderText = "Giá Trị";
            GiaTri.MinimumWidth = 6;
            GiaTri.Name = "GiaTri";
            GiaTri.ReadOnly = true;
            GiaTri.Width = 147;
            // 
            // DieuKien
            // 
            DieuKien.HeaderText = "Điều Kiện";
            DieuKien.MinimumWidth = 6;
            DieuKien.Name = "DieuKien";
            DieuKien.ReadOnly = true;
            DieuKien.Width = 146;
            // 
            // ThoiGian
            // 
            ThoiGian.HeaderText = "Thời Gian";
            ThoiGian.MinimumWidth = 6;
            ThoiGian.Name = "ThoiGian";
            ThoiGian.ReadOnly = true;
            ThoiGian.Width = 147;
            // 
            // DaDung
            // 
            DaDung.HeaderText = "Đã Dùng";
            DaDung.MinimumWidth = 6;
            DaDung.Name = "DaDung";
            DaDung.ReadOnly = true;
            DaDung.Width = 146;
            // 
            // TrangThai
            // 
            TrangThai.HeaderText = "Trạng Thái";
            TrangThai.MinimumWidth = 6;
            TrangThai.Name = "TrangThai";
            TrangThai.ReadOnly = true;
            TrangThai.Width = 147;
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Khuyễn Mãi";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Location = new Point(12, 237);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(140, 55);
            segmentedPill1.TabIndex = 1;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.AccessibleDescription = "";
            roundedTextBox1.AccessibleName = "";
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(12, 298);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.PlaceholderText = "Tìm Kiếm Chương Trình";
            roundedTextBox1.Size = new Size(509, 51);
            roundedTextBox1.TabIndex = 2;
            roundedTextBox1.Tag = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(289, 35);
            label1.TabIndex = 12;
            label1.Text = "Khuyến Mãi và Voucher";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(24, 55);
            label2.Name = "label2";
            label2.Size = new Size(441, 24);
            label2.TabIndex = 13;
            label2.Text = "Quản lý chương trình khuyến mãi và phiếu giảm giá";
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.FromArgb(255, 255, 192);
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(label7);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Location = new Point(12, 90);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(268, 141);
            roundedPanel1.TabIndex = 14;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(20, 81);
            label7.Name = "label7";
            label7.Size = new Size(17, 20);
            label7.TabIndex = 17;
            label7.Text = "2";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 31);
            label3.Name = "label3";
            label3.Size = new Size(130, 20);
            label3.TabIndex = 17;
            label3.Text = "Tất cả Khuyến Mãi";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.FromArgb(255, 255, 192);
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(label4);
            roundedPanel2.Controls.Add(label5);
            roundedPanel2.Location = new Point(402, 90);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(12);
            roundedPanel2.Size = new Size(268, 141);
            roundedPanel2.TabIndex = 15;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 81);
            label4.Name = "label4";
            label4.Size = new Size(17, 20);
            label4.TabIndex = 17;
            label4.Text = "0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 31);
            label5.Name = "label5";
            label5.Size = new Size(110, 20);
            label5.TabIndex = 17;
            label5.Text = "Đang Sử Dụng ";
            // 
            // roundedPanel4
            // 
            roundedPanel4.BackColor = Color.FromArgb(255, 255, 192);
            roundedPanel4.BorderThickness = 5;
            roundedPanel4.Controls.Add(label9);
            roundedPanel4.Controls.Add(label10);
            roundedPanel4.Location = new Point(829, 90);
            roundedPanel4.Name = "roundedPanel4";
            roundedPanel4.Padding = new Padding(12);
            roundedPanel4.Size = new Size(268, 141);
            roundedPanel4.TabIndex = 17;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(20, 81);
            label9.Name = "label9";
            label9.Size = new Size(17, 20);
            label9.TabIndex = 17;
            label9.Text = "0";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(20, 31);
            label10.Name = "label10";
            label10.Size = new Size(87, 20);
            label10.TabIndex = 17;
            label10.Text = "Đã Hết Hạn";
            // 
            // btnTaoCTKM
            // 
            btnTaoCTKM.BackColor = Color.Black;
            btnTaoCTKM.BorderThickness = 0;
            btnTaoCTKM.FlatStyle = FlatStyle.Flat;
            btnTaoCTKM.Font = new Font("Segoe UI Semibold", 10.5F);
            btnTaoCTKM.ForeColor = Color.White;
            btnTaoCTKM.Location = new Point(980, 309);
            btnTaoCTKM.Name = "btnTaoCTKM";
            btnTaoCTKM.Padding = new Padding(10, 6, 10, 6);
            btnTaoCTKM.Size = new Size(180, 40);
            btnTaoCTKM.TabIndex = 18;
            btnTaoCTKM.Text = "+ Tạo mới CTKM";
            btnTaoCTKM.UseVisualStyleBackColor = false;
            btnTaoCTKM.Click += roundedButton2_Click;
            // 
            // btnVoucher
            // 
            btnVoucher.BackColor = Color.Silver;
            btnVoucher.BorderThickness = 0;
            btnVoucher.FlatStyle = FlatStyle.Flat;
            btnVoucher.Font = new Font("Segoe UI Semibold", 10.5F);
            btnVoucher.ForeColor = Color.Black;
            btnVoucher.HoverBackColor = Color.WhiteSmoke;
            btnVoucher.Location = new Point(158, 237);
            btnVoucher.Name = "btnVoucher";
            btnVoucher.Padding = new Padding(10, 6, 10, 6);
            btnVoucher.Size = new Size(122, 55);
            btnVoucher.TabIndex = 19;
            btnVoucher.Text = "Voucher";
            btnVoucher.UseVisualStyleBackColor = false;
            btnVoucher.Click += btnVoucher_Click;
            // 
            // FrmChuongTrinhKM
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1190, 900);
            Controls.Add(btnVoucher);
            Controls.Add(btnTaoCTKM);
            Controls.Add(roundedPanel4);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(roundedTextBox1);
            Controls.Add(segmentedPill1);
            Controls.Add(dgvKhuyenMai);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmChuongTrinhKM";
            Text = "FrmVoucher";
            Load += FrmVoucher_Load;
            ((System.ComponentModel.ISupportInitialize)dgvKhuyenMai).EndInit();
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            roundedPanel4.ResumeLayout(false);
            roundedPanel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvKhuyenMai;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Controls.RoundedTextBox roundedTextBox1;
        private Label label1;
        private Label label2;
        private Controls.RoundedPanel roundedPanel1;
        private Label label7;
        private Label label3;
        private Controls.RoundedPanel roundedPanel2;
        private Label label4;
        private Label label5;
        private Controls.RoundedPanel roundedPanel4;
        private Label label9;
        private Label label10;
        private Controls.RoundedButton btnTaoCTKM;
        private DataGridViewTextBoxColumn TenCT;
        private DataGridViewTextBoxColumn MaKM;
        private DataGridViewTextBoxColumn Loai;
        private DataGridViewTextBoxColumn GiaTri;
        private DataGridViewTextBoxColumn DieuKien;
        private DataGridViewTextBoxColumn ThoiGian;
        private DataGridViewTextBoxColumn DaDung;
        private DataGridViewTextBoxColumn TrangThai;
        private Controls.RoundedButton btnVoucher;
    }
}