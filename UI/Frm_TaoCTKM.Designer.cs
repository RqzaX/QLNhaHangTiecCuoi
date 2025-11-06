namespace UI
{
    partial class Frm_TaoCTKM
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            label2 = new Label();
            label8 = new Label();
            CBBLoaiKM = new UiControls.BorderComboBox();
            label4 = new Label();
            txtTenCT = new UI.Controls.RoundedTextBox();
            label6 = new Label();
            txtGiamTD = new UI.Controls.RoundedTextBox();
            label7 = new Label();
            txtMaKM = new UI.Controls.RoundedTextBox();
            label9 = new Label();
            label10 = new Label();
            dateNgayBatDau = new DateTimePicker();
            dateNgayKetThuc = new DateTimePicker();
            btnHuy = new UI.Controls.RoundedButton();
            btnTao = new UI.Controls.RoundedButton();
            cbbLoaiApDung = new UiControls.BorderComboBox();
            label3 = new Label();
            btnDong = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(224, 28);
            label1.TabIndex = 4;
            label1.Text = "Chương trìn Khuyến mãi";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 37);
            label2.Name = "label2";
            label2.Size = new Size(140, 20);
            label2.TabIndex = 5;
            label2.Text = "Thiết lập CTKM mới";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(12, 87);
            label8.Name = "label8";
            label8.Size = new Size(131, 20);
            label8.TabIndex = 10;
            label8.Text = "Tên Chương trình";
            // 
            // CBBLoaiKM
            // 
            CBBLoaiKM.DrawMode = DrawMode.OwnerDrawFixed;
            CBBLoaiKM.FormattingEnabled = true;
            CBBLoaiKM.IntegralHeight = false;
            CBBLoaiKM.ItemHeight = 26;
            CBBLoaiKM.Items.AddRange(new object[] { "Giảm Theo %", "GIảm Theo Số Tiền" });
            CBBLoaiKM.Location = new Point(12, 288);
            CBBLoaiKM.Name = "CBBLoaiKM";
            CBBLoaiKM.Size = new Size(207, 32);
            CBBLoaiKM.TabIndex = 13;
            CBBLoaiKM.Text = "Chọn Loại";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 265);
            label4.Name = "label4";
            label4.Size = new Size(125, 20);
            label4.TabIndex = 12;
            label4.Text = "Loại Khuyến Mãi";
            // 
            // txtTenCT
            // 
            txtTenCT.BackColor = Color.White;
            txtTenCT.Font = new Font("Segoe UI", 10F);
            txtTenCT.ForeColor = Color.Black;
            txtTenCT.Location = new Point(12, 110);
            txtTenCT.Name = "txtTenCT";
            txtTenCT.Padding = new Padding(10, 8, 10, 8);
            txtTenCT.Size = new Size(577, 51);
            txtTenCT.TabIndex = 14;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(352, 265);
            label6.Name = "label6";
            label6.Size = new Size(90, 20);
            label6.TabIndex = 11;
            label6.Text = "Giảm tối đa";
            // 
            // txtGiamTD
            // 
            txtGiamTD.BackColor = Color.White;
            txtGiamTD.Font = new Font("Segoe UI", 10F);
            txtGiamTD.ForeColor = Color.Black;
            txtGiamTD.Location = new Point(352, 288);
            txtGiamTD.Name = "txtGiamTD";
            txtGiamTD.Padding = new Padding(10, 8, 10, 8);
            txtGiamTD.Size = new Size(237, 51);
            txtGiamTD.TabIndex = 15;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(12, 176);
            label7.Name = "label7";
            label7.Size = new Size(118, 20);
            label7.TabIndex = 10;
            label7.Text = "Mã Khuyến Mãi";
            // 
            // txtMaKM
            // 
            txtMaKM.BackColor = Color.White;
            txtMaKM.Font = new Font("Segoe UI", 10F);
            txtMaKM.ForeColor = Color.Black;
            txtMaKM.Location = new Point(12, 199);
            txtMaKM.Name = "txtMaKM";
            txtMaKM.Padding = new Padding(10, 8, 10, 8);
            txtMaKM.Size = new Size(577, 51);
            txtMaKM.TabIndex = 14;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(352, 342);
            label9.Name = "label9";
            label9.Size = new Size(107, 20);
            label9.TabIndex = 11;
            label9.Text = "Ngày kết thúc";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(12, 342);
            label10.Name = "label10";
            label10.Size = new Size(103, 20);
            label10.TabIndex = 11;
            label10.Text = "Ngày bắt đầu";
            // 
            // dateNgayBatDau
            // 
            dateNgayBatDau.Location = new Point(12, 378);
            dateNgayBatDau.Name = "dateNgayBatDau";
            dateNgayBatDau.Size = new Size(237, 27);
            dateNgayBatDau.TabIndex = 16;
            // 
            // dateNgayKetThuc
            // 
            dateNgayKetThuc.Location = new Point(352, 378);
            dateNgayKetThuc.Name = "dateNgayKetThuc";
            dateNgayKetThuc.Size = new Size(237, 27);
            dateNgayKetThuc.TabIndex = 17;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.White;
            btnHuy.BackgroundImageLayout = ImageLayout.Stretch;
            btnHuy.BorderThickness = 0;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI Semibold", 10.5F);
            btnHuy.ForeColor = Color.Black;
            btnHuy.Location = new Point(332, 510);
            btnHuy.Name = "btnHuy";
            btnHuy.Padding = new Padding(10, 6, 10, 6);
            btnHuy.Size = new Size(94, 49);
            btnHuy.TabIndex = 18;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            // 
            // btnTao
            // 
            btnTao.BackColor = Color.FromArgb(31, 111, 235);
            btnTao.BorderThickness = 0;
            btnTao.FlatAppearance.BorderSize = 0;
            btnTao.FlatStyle = FlatStyle.Flat;
            btnTao.Font = new Font("Segoe UI Semibold", 10.5F);
            btnTao.ForeColor = Color.White;
            btnTao.Location = new Point(432, 510);
            btnTao.Name = "btnTao";
            btnTao.Padding = new Padding(10, 6, 10, 6);
            btnTao.Size = new Size(157, 49);
            btnTao.TabIndex = 19;
            btnTao.Text = "Tạo CTKM";
            btnTao.UseVisualStyleBackColor = false;
            // 
            // cbbLoaiApDung
            // 
            cbbLoaiApDung.DrawMode = DrawMode.OwnerDrawFixed;
            cbbLoaiApDung.FormattingEnabled = true;
            cbbLoaiApDung.IntegralHeight = false;
            cbbLoaiApDung.ItemHeight = 26;
            cbbLoaiApDung.Location = new Point(12, 443);
            cbbLoaiApDung.Name = "cbbLoaiApDung";
            cbbLoaiApDung.Size = new Size(207, 32);
            cbbLoaiApDung.TabIndex = 13;
            cbbLoaiApDung.Text = "Chọn Loại";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 420);
            label3.Name = "label3";
            label3.Size = new Size(104, 20);
            label3.TabIndex = 12;
            label3.Text = "Loại Áp Dụng";
            // 
            // btnDong
            // 
            btnDong.BorderRadius = 20;
            btnDong.CustomizableEdges = customizableEdges1;
            btnDong.DisabledState.BorderColor = Color.DarkGray;
            btnDong.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDong.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDong.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDong.FillColor = Color.Silver;
            btnDong.Font = new Font("Segoe UI", 9F);
            btnDong.ForeColor = Color.White;
            btnDong.Location = new Point(208, 510);
            btnDong.Name = "btnDong";
            btnDong.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnDong.Size = new Size(106, 49);
            btnDong.TabIndex = 20;
            btnDong.Text = "Đóng";
            btnDong.Click += btnDong_Click;
            // 
            // Frm_TaoCTKM
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(619, 600);
            Controls.Add(btnDong);
            Controls.Add(btnTao);
            Controls.Add(btnHuy);
            Controls.Add(dateNgayKetThuc);
            Controls.Add(dateNgayBatDau);
            Controls.Add(txtGiamTD);
            Controls.Add(label10);
            Controls.Add(label6);
            Controls.Add(txtMaKM);
            Controls.Add(txtTenCT);
            Controls.Add(label9);
            Controls.Add(cbbLoaiApDung);
            Controls.Add(CBBLoaiKM);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Frm_TaoCTKM";
            Text = "Frm_TaoCTKM";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label8;
        private UiControls.BorderComboBox CBBLoaiKM;
        private Label label4;
        private Controls.RoundedTextBox txtTenCT;
        private Label label6;
        private Controls.RoundedTextBox txtGiamTD;
        private Label label7;
        private Controls.RoundedTextBox txtMaKM;
        private Label label9;
        private Label label10;
        private DateTimePicker dateNgayBatDau;
        private DateTimePicker dateNgayKetThuc;
        private Controls.RoundedButton btnHuy;
        private Controls.RoundedButton btnTao;
        private UiControls.BorderComboBox cbbLoaiApDung;
        private Label label3;
        private Guna.UI2.WinForms.Guna2Button btnDong;
    }
}