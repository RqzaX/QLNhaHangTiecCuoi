namespace UI
{
    partial class FrmVoucher
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
            VanThuan.UI.PillItem pillItem2 = new VanThuan.UI.PillItem();
            dgvKhuyenMai = new DataGridView();
            dgvTenChuongTrinh = new DataGridViewTextBoxColumn();
            dgvtxtMaKM = new DataGridViewTextBoxColumn();
            dgvtxtLoai = new DataGridViewTextBoxColumn();
            dgvtxtGiaTri = new DataGridViewTextBoxColumn();
            dgvtxtDieuKien = new DataGridViewTextBoxColumn();
            dgvtxtThoiGian = new DataGridViewTextBoxColumn();
            dgvtxtDaDung = new DataGridViewTextBoxColumn();
            dgvtxtTrangThai = new DataGridViewTextBoxColumn();
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
            roundedPanel3 = new UI.Controls.RoundedPanel();
            label6 = new Label();
            label8 = new Label();
            roundedPanel4 = new UI.Controls.RoundedPanel();
            label9 = new Label();
            label10 = new Label();
            roundedButton2 = new UI.Controls.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)dgvKhuyenMai).BeginInit();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
            roundedPanel3.SuspendLayout();
            roundedPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // dgvKhuyenMai
            // 
            dgvKhuyenMai.AllowUserToAddRows = false;
            dgvKhuyenMai.BorderStyle = BorderStyle.Fixed3D;
            dgvKhuyenMai.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKhuyenMai.Columns.AddRange(new DataGridViewColumn[] { dgvTenChuongTrinh, dgvtxtMaKM, dgvtxtLoai, dgvtxtGiaTri, dgvtxtDieuKien, dgvtxtThoiGian, dgvtxtDaDung, dgvtxtTrangThai });
            dgvKhuyenMai.Location = new Point(3, 355);
            dgvKhuyenMai.Name = "dgvKhuyenMai";
            dgvKhuyenMai.RowHeadersVisible = false;
            dgvKhuyenMai.RowHeadersWidth = 51;
            dgvKhuyenMai.Size = new Size(1168, 497);
            dgvKhuyenMai.TabIndex = 0;
            // 
            // dgvTenChuongTrinh
            // 
            dgvTenChuongTrinh.HeaderText = "Tên Chương Trình";
            dgvTenChuongTrinh.MinimumWidth = 6;
            dgvTenChuongTrinh.Name = "dgvTenChuongTrinh";
            dgvTenChuongTrinh.Resizable = DataGridViewTriState.True;
            dgvTenChuongTrinh.Width = 146;
            // 
            // dgvtxtMaKM
            // 
            dgvtxtMaKM.HeaderText = "Mã Khuyến Mãi";
            dgvtxtMaKM.MinimumWidth = 6;
            dgvtxtMaKM.Name = "dgvtxtMaKM";
            dgvtxtMaKM.Width = 145;
            // 
            // dgvtxtLoai
            // 
            dgvtxtLoai.HeaderText = "Loại";
            dgvtxtLoai.MinimumWidth = 6;
            dgvtxtLoai.Name = "dgvtxtLoai";
            dgvtxtLoai.Width = 146;
            // 
            // dgvtxtGiaTri
            // 
            dgvtxtGiaTri.HeaderText = "Giá Trị";
            dgvtxtGiaTri.MinimumWidth = 6;
            dgvtxtGiaTri.Name = "dgvtxtGiaTri";
            dgvtxtGiaTri.Width = 146;
            // 
            // dgvtxtDieuKien
            // 
            dgvtxtDieuKien.HeaderText = "Điều Kiện";
            dgvtxtDieuKien.MinimumWidth = 6;
            dgvtxtDieuKien.Name = "dgvtxtDieuKien";
            dgvtxtDieuKien.Width = 145;
            // 
            // dgvtxtThoiGian
            // 
            dgvtxtThoiGian.HeaderText = "Thời Gian";
            dgvtxtThoiGian.MinimumWidth = 6;
            dgvtxtThoiGian.Name = "dgvtxtThoiGian";
            dgvtxtThoiGian.Width = 146;
            // 
            // dgvtxtDaDung
            // 
            dgvtxtDaDung.HeaderText = "Đã dùng/Giới Hạn";
            dgvtxtDaDung.MinimumWidth = 6;
            dgvtxtDaDung.Name = "dgvtxtDaDung";
            dgvtxtDaDung.Width = 145;
            // 
            // dgvtxtTrangThai
            // 
            dgvtxtTrangThai.HeaderText = "Trạng Thái";
            dgvtxtTrangThai.MinimumWidth = 6;
            dgvtxtTrangThai.Name = "dgvtxtTrangThai";
            dgvtxtTrangThai.Width = 146;
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Khuyễn Mãi";
            pillItem2.Text = "Voucher";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Location = new Point(12, 237);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(243, 55);
            segmentedPill1.TabIndex = 1;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(12, 298);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(509, 51);
            roundedTextBox1.TabIndex = 2;
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
            roundedPanel1.BackColor = Color.White;
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
            label3.Size = new Size(120, 20);
            label3.TabIndex = 17;
            label3.Text = "CTKM đang chạy";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.White;
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(label4);
            roundedPanel2.Controls.Add(label5);
            roundedPanel2.Location = new Point(307, 90);
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
            label4.Size = new Size(33, 20);
            label4.TabIndex = 17;
            label4.Text = "193";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 31);
            label5.Name = "label5";
            label5.Size = new Size(96, 20);
            label5.TabIndex = 17;
            label5.Text = "Lượt sử dụng";
            // 
            // roundedPanel3
            // 
            roundedPanel3.BackColor = Color.White;
            roundedPanel3.BorderThickness = 5;
            roundedPanel3.Controls.Add(label6);
            roundedPanel3.Controls.Add(label8);
            roundedPanel3.Location = new Point(601, 90);
            roundedPanel3.Name = "roundedPanel3";
            roundedPanel3.Padding = new Padding(12);
            roundedPanel3.Size = new Size(268, 141);
            roundedPanel3.TabIndex = 16;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 81);
            label6.Name = "label6";
            label6.Size = new Size(17, 20);
            label6.TabIndex = 17;
            label6.Text = "3";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(20, 31);
            label8.Name = "label8";
            label8.Size = new Size(132, 20);
            label8.TabIndex = 17;
            label8.Text = "Voucher phát hành";
            // 
            // roundedPanel4
            // 
            roundedPanel4.BackColor = Color.White;
            roundedPanel4.BorderThickness = 5;
            roundedPanel4.Controls.Add(label9);
            roundedPanel4.Controls.Add(label10);
            roundedPanel4.Location = new Point(892, 90);
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
            label9.Size = new Size(49, 20);
            label9.TabIndex = 17;
            label9.Text = "15.8M";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(20, 31);
            label10.Name = "label10";
            label10.Size = new Size(106, 20);
            label10.TabIndex = 17;
            label10.Text = "Tổng giảm giá";
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.BorderColor = Color.Black;
            roundedButton2.BorderThickness = 2;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.HoverBackColor = Color.LightGray;
            roundedButton2.Location = new Point(980, 309);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.PressedBackColor = Color.FromArgb(192, 255, 255);
            roundedButton2.Size = new Size(180, 40);
            roundedButton2.TabIndex = 18;
            roundedButton2.Text = "+ Tạo mới CTKM";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // FrmVoucher
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 900);
            Controls.Add(roundedButton2);
            Controls.Add(roundedPanel4);
            Controls.Add(roundedPanel3);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(roundedTextBox1);
            Controls.Add(segmentedPill1);
            Controls.Add(dgvKhuyenMai);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmVoucher";
            Text = "FrmVoucher";
            Load += FrmVoucher_Load;
            ((System.ComponentModel.ISupportInitialize)dgvKhuyenMai).EndInit();
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            roundedPanel3.ResumeLayout(false);
            roundedPanel3.PerformLayout();
            roundedPanel4.ResumeLayout(false);
            roundedPanel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvKhuyenMai;
        private DataGridViewTextBoxColumn dgvTenChuongTrinh;
        private DataGridViewTextBoxColumn dgvtxtMaKM;
        private DataGridViewTextBoxColumn dgvtxtLoai;
        private DataGridViewTextBoxColumn dgvtxtGiaTri;
        private DataGridViewTextBoxColumn dgvtxtDieuKien;
        private DataGridViewTextBoxColumn dgvtxtThoiGian;
        private DataGridViewTextBoxColumn dgvtxtDaDung;
        private DataGridViewTextBoxColumn dgvtxtTrangThai;
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
        private Controls.RoundedPanel roundedPanel3;
        private Label label6;
        private Label label8;
        private Controls.RoundedPanel roundedPanel4;
        private Label label9;
        private Label label10;
        private Controls.RoundedButton roundedButton2;
    }
}