namespace UI
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            pictureBox1 = new PictureBox();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            rainbowTitle1 = new UI.Controls.RainbowTitle();
            btnDangNhap = new UI.Controls.RoundedButton();
            cbLuuThongTin = new CheckBox();
            label3 = new Label();
            txtMatKhau = new TextBox();
            label2 = new Label();
            txtTaiKhoan = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            roundedPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(22, 114);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(502, 417);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(rainbowTitle1);
            roundedPanel1.Controls.Add(btnDangNhap);
            roundedPanel1.Controls.Add(cbLuuThongTin);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Controls.Add(txtMatKhau);
            roundedPanel1.Controls.Add(label2);
            roundedPanel1.Controls.Add(txtTaiKhoan);
            roundedPanel1.Location = new Point(568, 29);
            roundedPanel1.Margin = new Padding(3, 2, 3, 2);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(10, 9, 10, 9);
            roundedPanel1.Size = new Size(606, 579);
            roundedPanel1.TabIndex = 1;
            // 
            // rainbowTitle1
            // 
            rainbowTitle1.BackColor = Color.White;
            rainbowTitle1.Font = new Font("Calibri", 40.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rainbowTitle1.ForeColor = Color.White;
            rainbowTitle1.Location = new Point(28, 34);
            rainbowTitle1.Margin = new Padding(3, 2, 3, 2);
            rainbowTitle1.Name = "rainbowTitle1";
            rainbowTitle1.Size = new Size(556, 190);
            rainbowTitle1.TabIndex = 25;
            rainbowTitle1.Text = "Hệ thống Quản lý Nhà hàng Tiệc cưới";
            // 
            // btnDangNhap
            // 
            btnDangNhap.BackColor = Color.FromArgb(31, 111, 235);
            btnDangNhap.CornerRadius = 27;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.Font = new Font("Calibri", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(90, 466);
            btnDangNhap.Margin = new Padding(3, 2, 3, 2);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Padding = new Padding(9, 4, 9, 4);
            btnDangNhap.Size = new Size(438, 49);
            btnDangNhap.TabIndex = 24;
            btnDangNhap.Text = "Đăng Nhập";
            btnDangNhap.UseVisualStyleBackColor = false;
            btnDangNhap.Click += btnDangNhap_Click_1;
            // 
            // cbLuuThongTin
            // 
            cbLuuThongTin.AutoSize = true;
            cbLuuThongTin.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbLuuThongTin.Location = new Point(43, 433);
            cbLuuThongTin.Margin = new Padding(3, 2, 3, 2);
            cbLuuThongTin.Name = "cbLuuThongTin";
            cbLuuThongTin.Size = new Size(203, 23);
            cbLuuThongTin.TabIndex = 23;
            cbLuuThongTin.Text = "Lưu thông tin đăng nhập!";
            cbLuuThongTin.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(43, 335);
            label3.Name = "label3";
            label3.Size = new Size(102, 27);
            label3.TabIndex = 22;
            label3.Text = "Mật Khẩu";
            // 
            // txtMatKhau
            // 
            txtMatKhau.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtMatKhau.ForeColor = Color.DodgerBlue;
            txtMatKhau.Location = new Point(57, 364);
            txtMatKhau.Margin = new Padding(3, 2, 3, 2);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(513, 34);
            txtMatKhau.TabIndex = 21;
            txtMatKhau.Text = "123456";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(43, 262);
            label2.Name = "label2";
            label2.Size = new Size(101, 27);
            label2.TabIndex = 20;
            label2.Text = "Tài Khoản";
            // 
            // txtTaiKhoan
            // 
            txtTaiKhoan.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTaiKhoan.ForeColor = Color.DodgerBlue;
            txtTaiKhoan.Location = new Point(57, 290);
            txtTaiKhoan.Margin = new Padding(3, 2, 3, 2);
            txtTaiKhoan.Name = "txtTaiKhoan";
            txtTaiKhoan.Size = new Size(513, 34);
            txtTaiKhoan.TabIndex = 19;
            txtTaiKhoan.Text = "admin";
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1209, 640);
            Controls.Add(roundedPanel1);
            Controls.Add(pictureBox1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmLogin";
            FormClosing += FrmLogin_FormClosing;
            Load += FrmLogin_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Controls.RoundedPanel roundedPanel1;
        private Controls.RainbowTitle rainbowTitle1;
        private Controls.RoundedButton btnDangNhap;
        private CheckBox cbLuuThongTin;
        private Label label3;
        private TextBox txtMatKhau;
        private Label label2;
        private TextBox txtTaiKhoan;
    }
}