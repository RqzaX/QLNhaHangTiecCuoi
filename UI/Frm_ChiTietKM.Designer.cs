namespace UI
{
    partial class Frm_ChiTietKM
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
            dateNgayKetThuc = new DateTimePicker();
            dateNgayBatDau = new DateTimePicker();
            txtGiamTD = new UI.Controls.RoundedTextBox();
            label10 = new Label();
            label6 = new Label();
            txtMaKM = new UI.Controls.RoundedTextBox();
            txtTenCT = new UI.Controls.RoundedTextBox();
            label9 = new Label();
            CBBLoaiKM = new UiControls.BorderComboBox();
            label4 = new Label();
            label7 = new Label();
            label8 = new Label();
            label2 = new Label();
            label1 = new Label();
            btnLuu = new UI.Controls.RoundedButton();
            btnXoa = new UI.Controls.RoundedButton();
            checkSuDung = new CheckBox();
            cbbLoaiApDung = new UiControls.BorderComboBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // dateNgayKetThuc
            // 
            dateNgayKetThuc.Location = new Point(354, 359);
            dateNgayKetThuc.Name = "dateNgayKetThuc";
            dateNgayKetThuc.Size = new Size(237, 27);
            dateNgayKetThuc.TabIndex = 31;
            // 
            // dateNgayBatDau
            // 
            dateNgayBatDau.Location = new Point(15, 359);
            dateNgayBatDau.Name = "dateNgayBatDau";
            dateNgayBatDau.Size = new Size(237, 27);
            dateNgayBatDau.TabIndex = 30;
            // 
            // txtGiamTD
            // 
            txtGiamTD.BackColor = Color.White;
            txtGiamTD.Font = new Font("Segoe UI", 10F);
            txtGiamTD.ForeColor = Color.Black;
            txtGiamTD.Location = new Point(354, 282);
            txtGiamTD.Name = "txtGiamTD";
            txtGiamTD.Padding = new Padding(10, 8, 10, 8);
            txtGiamTD.Size = new Size(237, 51);
            txtGiamTD.TabIndex = 29;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(15, 336);
            label10.Name = "label10";
            label10.Size = new Size(103, 20);
            label10.TabIndex = 22;
            label10.Text = "Ngày bắt đầu";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(354, 259);
            label6.Name = "label6";
            label6.Size = new Size(90, 20);
            label6.TabIndex = 23;
            label6.Text = "Giảm tối đa";
            // 
            // txtMaKM
            // 
            txtMaKM.BackColor = Color.White;
            txtMaKM.Font = new Font("Segoe UI", 10F);
            txtMaKM.ForeColor = Color.Black;
            txtMaKM.Location = new Point(14, 205);
            txtMaKM.Name = "txtMaKM";
            txtMaKM.Padding = new Padding(10, 8, 10, 8);
            txtMaKM.Size = new Size(577, 51);
            txtMaKM.TabIndex = 27;
            // 
            // txtTenCT
            // 
            txtTenCT.BackColor = Color.White;
            txtTenCT.Font = new Font("Segoe UI", 10F);
            txtTenCT.ForeColor = Color.Black;
            txtTenCT.Location = new Point(14, 116);
            txtTenCT.Name = "txtTenCT";
            txtTenCT.Padding = new Padding(10, 8, 10, 8);
            txtTenCT.Size = new Size(577, 51);
            txtTenCT.TabIndex = 28;
        
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(354, 336);
            label9.Name = "label9";
            label9.Size = new Size(107, 20);
            label9.TabIndex = 24;
            label9.Text = "Ngày kết thúc";
            // 
            // CBBLoaiKM
            // 
            CBBLoaiKM.DrawMode = DrawMode.OwnerDrawFixed;
            CBBLoaiKM.FormattingEnabled = true;
            CBBLoaiKM.IntegralHeight = false;
            CBBLoaiKM.ItemHeight = 26;
            CBBLoaiKM.Items.AddRange(new object[] { "Giảm Theo %", "GIảm Theo Số Tiền", "Tặng quà" });
            CBBLoaiKM.Location = new Point(15, 282);
            CBBLoaiKM.Name = "CBBLoaiKM";
            CBBLoaiKM.Size = new Size(207, 32);
            CBBLoaiKM.TabIndex = 26;
            CBBLoaiKM.Text = "Chọn Loại";
        
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(14, 259);
            label4.Name = "label4";
            label4.Size = new Size(125, 20);
            label4.TabIndex = 25;
            label4.Text = "Loại Khuyến Mãi";
          
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(14, 182);
            label7.Name = "label7";
            label7.Size = new Size(118, 20);
            label7.TabIndex = 20;
            label7.Text = "Mã Khuyến Mãi";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(14, 93);
            label8.Name = "label8";
            label8.Size = new Size(131, 20);
            label8.TabIndex = 21;
            label8.Text = "Tên Chương trình";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 43);
            label2.Name = "label2";
            label2.Size = new Size(140, 20);
            label2.TabIndex = 19;
            label2.Text = "Thiết lập CTKM mới";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(14, 15);
            label1.Name = "label1";
            label1.Size = new Size(208, 28);
            label1.TabIndex = 18;
            label1.Text = "Chỉnh Sửa Khuyến Mãi";
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(31, 111, 235);
            btnLuu.BorderThickness = 0;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI Semibold", 10.5F);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(482, 466);
            btnLuu.Name = "btnLuu";
            btnLuu.Padding = new Padding(10, 6, 10, 6);
            btnLuu.Size = new Size(109, 49);
            btnLuu.TabIndex = 32;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = false;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.Red;
            btnXoa.BorderThickness = 0;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI Semibold", 10.5F);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(367, 466);
            btnXoa.Name = "btnXoa";
            btnXoa.Padding = new Padding(10, 6, 10, 6);
            btnXoa.Size = new Size(109, 49);
            btnXoa.TabIndex = 32;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            // 
            // checkSuDung
            // 
            checkSuDung.AutoSize = true;
            checkSuDung.Location = new Point(358, 392);
            checkSuDung.Name = "checkSuDung";
            checkSuDung.Size = new Size(86, 24);
            checkSuDung.TabIndex = 33;
            checkSuDung.Text = "Sử dụng";
            checkSuDung.UseVisualStyleBackColor = true;
            // 
            // cbbLoaiApDung
            // 
            cbbLoaiApDung.DrawMode = DrawMode.OwnerDrawFixed;
            cbbLoaiApDung.FormattingEnabled = true;
            cbbLoaiApDung.IntegralHeight = false;
            cbbLoaiApDung.ItemHeight = 26;
            cbbLoaiApDung.Location = new Point(15, 416);
            cbbLoaiApDung.Name = "cbbLoaiApDung";
            cbbLoaiApDung.Size = new Size(207, 32);
            cbbLoaiApDung.TabIndex = 26;
            cbbLoaiApDung.Text = "Chọn Loại";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(15, 393);
            label3.Name = "label3";
            label3.Size = new Size(99, 20);
            label3.TabIndex = 25;
            label3.Text = "Loại áp dụng";
            // 
            // Frm_ChiTietKM
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(608, 544);
            Controls.Add(checkSuDung);
            Controls.Add(btnXoa);
            Controls.Add(btnLuu);
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
            Name = "Frm_ChiTietKM";
            Text = "Frm_ChiTietKM";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dateNgayKetThuc;
        private DateTimePicker dateNgayBatDau;
        private Controls.RoundedTextBox txtGiamTD;
        private Label label10;
        private Label label6;
        private Controls.RoundedTextBox txtMaKM;
        private Controls.RoundedTextBox txtTenCT;
        private Label label9;
        private UiControls.BorderComboBox CBBLoaiKM;
        private Label label4;
        private Label label7;
        private Label label8;
        private Label label2;
        private Label label1;
        private Controls.RoundedButton btnLuu;
        private Controls.RoundedButton btnXoa;
        private CheckBox checkSuDung;
        private UiControls.BorderComboBox cbbLoaiApDung;
        private Label label3;
    }
}