namespace UI
{
    partial class Frm_ChiTietKhachHang
    {
       
        private System.ComponentModel.IContainer components = null;

        
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

       
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            txtHoTen = new UI.Controls.RoundedTextBox();
            label3 = new Label();
            label4 = new Label();
            txtSDT = new UI.Controls.RoundedTextBox();
            label5 = new Label();
            txtEmail = new UI.Controls.RoundedTextBox();
            label7 = new Label();
            cbbHang = new UI.Controls.RoundedComboBox();
            label9 = new Label();
            txtGhiChu = new UI.Controls.RoundedTextBox();
            label10 = new Label();
            txtChiTieu = new UI.Controls.RoundedTextBox();
            btnHuy = new UI.Controls.RoundedButton();
            btnSua = new UI.Controls.RoundedButton();
            btnLuu = new UI.Controls.RoundedButton();
            btnXoa = new UI.Controls.RoundedButton();
            SuspendLayout();
            
            // label1
            
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(200, 28);
            label1.TabIndex = 6;
            label1.Text = "Chi Tiết Khách Hàng";
            
            // label2
            
            label2.AutoSize = true;
            label2.Location = new Point(12, 37);
            label2.Name = "label2";
            label2.Size = new Size(237, 20);
            label2.TabIndex = 7;
            label2.Text = "Xem và chỉnh sửa thông tin khách hàng";
            
            // txtHoTen
            
            txtHoTen.BackColor = Color.White;
            txtHoTen.Font = new Font("Segoe UI", 10F);
            txtHoTen.ForeColor = Color.Black;
            txtHoTen.Location = new Point(12, 93);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Padding = new Padding(10, 8, 10, 8);
            txtHoTen.Size = new Size(577, 51);
            txtHoTen.TabIndex = 18;
             
            //abel3
            
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 70);
            label3.Name = "label3";
            label3.Size = new Size(78, 20);
            label3.TabIndex = 17;
            label3.Text = "Họ và Tên";
            
            // label4
            
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 158);
            label4.Name = "label4";
            label4.Size = new Size(100, 20);
            label4.TabIndex = 17;
            label4.Text = "Số điện thoại";
            
            // txtSDT
            
            txtSDT.BackColor = Color.White;
            txtSDT.Font = new Font("Segoe UI", 10F);
            txtSDT.ForeColor = Color.Black;
            txtSDT.Location = new Point(12, 181);
            txtSDT.Name = "txtSDT";
            txtSDT.Padding = new Padding(10, 8, 10, 8);
            txtSDT.Size = new Size(257, 51);
            txtSDT.TabIndex = 18;
            
            // label5
            
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(332, 158);
            label5.Name = "label5";
            label5.Size = new Size(47, 20);
            label5.TabIndex = 17;
            label5.Text = "Email";
            
            // txtEmail
            
            txtEmail.BackColor = Color.White;
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.ForeColor = Color.Black;
            txtEmail.Location = new Point(332, 181);
            txtEmail.Name = "txtEmail";
            txtEmail.Padding = new Padding(10, 8, 10, 8);
            txtEmail.Size = new Size(257, 51);
            txtEmail.TabIndex = 18;
            
            // label7
            
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(12, 245);
            label7.Name = "label7";
            label7.Size = new Size(124, 20);
            label7.TabIndex = 17;
            label7.Text = "Hạng thành viên";
            
            // cbbHang
            
            cbbHang.BackColor = Color.FromArgb(248, 248, 250);
            cbbHang.FlatStyle = FlatStyle.Flat;
            cbbHang.ForeColor = Color.FromArgb(70, 70, 70);
            cbbHang.FormattingEnabled = true;
            cbbHang.Location = new Point(12, 268);
            cbbHang.Name = "cbbHang";
            cbbHang.Size = new Size(255, 28);
            cbbHang.TabIndex = 22;
            
            // label9
            
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(12, 397);
            label9.Name = "label9";
            label9.Size = new Size(64, 20);
            label9.TabIndex = 17;
            label9.Text = "Ghi Chú";
            
            // txtGhiChu
            
            txtGhiChu.BackColor = Color.White;
            txtGhiChu.Font = new Font("Segoe UI", 10F);
            txtGhiChu.ForeColor = Color.Black;
            txtGhiChu.Location = new Point(12, 420);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Padding = new Padding(10, 8, 10, 8);
            txtGhiChu.Size = new Size(577, 95);
            txtGhiChu.TabIndex = 18;
            
            // label10
            
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(12, 309);
            label10.Name = "label10";
            label10.Size = new Size(65, 20);
            label10.TabIndex = 17;
            label10.Text = "Chi Tiêu";
            
            // txtChiTieu
            
            txtChiTieu.BackColor = Color.White;
            txtChiTieu.Font = new Font("Segoe UI", 10F);
            txtChiTieu.ForeColor = Color.Black;
            txtChiTieu.Location = new Point(12, 332);
            txtChiTieu.Name = "txtChiTieu";
            txtChiTieu.Padding = new Padding(10, 8, 10, 8);
            txtChiTieu.Size = new Size(255, 51);
            txtChiTieu.TabIndex = 18;
             
            // btnHuy
            
            btnHuy.BackColor = Color.White;
            btnHuy.BorderThickness = 0;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI Semibold", 10.5F);
            btnHuy.ForeColor = Color.Black;
            btnHuy.Location = new Point(399, 520);
            btnHuy.Name = "btnHuy";
            btnHuy.Padding = new Padding(10, 6, 10, 6);
            btnHuy.Size = new Size(94, 61);
            btnHuy.TabIndex = 20;
            btnHuy.Text = "Đóng";
            btnHuy.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            btnSua.BackColor = Color.Black;
            btnSua.BorderThickness = 0;
            btnSua.FlatAppearance.BorderSize = 0;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI Semibold", 10.5F);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(12, 520);
            btnSua.Name = "btnSua";
            btnSua.Padding = new Padding(10, 6, 10, 6);
            btnSua.Size = new Size(120, 61);
            btnSua.TabIndex = 21;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = false;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.Black;
            btnLuu.BorderThickness = 0;
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI Semibold", 10.5F);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(138, 520);
            btnLuu.Name = "btnLuu";
            btnLuu.Padding = new Padding(10, 6, 10, 6);
            btnLuu.Size = new Size(120, 61);
            btnLuu.TabIndex = 21;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = false;
            btnLuu.Visible = false;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(220, 38, 38);
            btnXoa.BorderThickness = 0;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI Semibold", 10.5F);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(264, 520);
            btnXoa.Name = "btnXoa";
            btnXoa.Padding = new Padding(10, 6, 10, 6);
            btnXoa.Size = new Size(120, 61);
            btnXoa.TabIndex = 21;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            // 
            // Frm_ChiTietKhachHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(599, 612);
            Controls.Add(btnXoa);
            Controls.Add(btnLuu);
            Controls.Add(btnSua);
            Controls.Add(btnHuy);
            Controls.Add(cbbHang);
            Controls.Add(txtEmail);
            Controls.Add(label5);
            Controls.Add(txtSDT);
            Controls.Add(label7);
            Controls.Add(label4);
            Controls.Add(txtGhiChu);
            Controls.Add(label9);
            Controls.Add(txtChiTieu);
            Controls.Add(label10);
            Controls.Add(txtHoTen);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Frm_ChiTietKhachHang";
            Text = "Chi Tiết Khách Hàng";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Controls.RoundedTextBox txtHoTen;
        private Label label3;
        private Label label4;
        private Controls.RoundedTextBox txtSDT;
        private Label label5;
        private Controls.RoundedTextBox txtEmail;
        private Label label7;
        private Controls.RoundedComboBox cbbHang;
        private Label label9;
        private Controls.RoundedTextBox txtGhiChu;
        private Label label10;
        private Controls.RoundedTextBox txtChiTieu;
        private Controls.RoundedButton btnHuy;
        private Controls.RoundedButton btnSua;
        private Controls.RoundedButton btnLuu;
        private Controls.RoundedButton btnXoa;
    }
}

