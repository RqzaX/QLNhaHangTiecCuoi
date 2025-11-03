using Guna.UI2.WinForms;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pictureBox1 = new PictureBox();
            roundedPanel1 = new Guna2Panel();
            rainbowTitle1 = new UI.Controls.RainbowTitle();
            btnDangNhap = new Guna2Button();
            cbLuuThongTin = new Guna2CheckBox();
            label3 = new Label();
            txtMatKhau = new Guna2TextBox();
            label2 = new Label();
            txtTaiKhoan = new Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            roundedPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(25, 152);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(574, 556);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderColor = Color.White;
            roundedPanel1.BorderRadius = 20;
            roundedPanel1.BorderThickness = 2;
            roundedPanel1.Controls.Add(rainbowTitle1);
            roundedPanel1.Controls.Add(btnDangNhap);
            roundedPanel1.Controls.Add(cbLuuThongTin);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Controls.Add(txtMatKhau);
            roundedPanel1.Controls.Add(label2);
            roundedPanel1.Controls.Add(txtTaiKhoan);
            roundedPanel1.CustomizableEdges = customizableEdges7;
            roundedPanel1.Location = new Point(649, 39);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(11, 12, 11, 12);
            roundedPanel1.ShadowDecoration.CustomizableEdges = customizableEdges8;
            roundedPanel1.Size = new Size(693, 772);
            roundedPanel1.TabIndex = 1;
            // 
            // rainbowTitle1
            // 
            rainbowTitle1.BackColor = Color.White;
            rainbowTitle1.Font = new Font("Calibri", 40.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rainbowTitle1.ForeColor = Color.White;
            rainbowTitle1.Location = new Point(32, 45);
            rainbowTitle1.Name = "rainbowTitle1";
            rainbowTitle1.Size = new Size(635, 253);
            rainbowTitle1.TabIndex = 25;
            rainbowTitle1.Text = "Hệ thống Quản lý Nhà hàng Tiệc cưới";
            // 
            // btnDangNhap
            // 
            btnDangNhap.Animated = true;
            btnDangNhap.BackColor = Color.Transparent;
            btnDangNhap.BorderRadius = 20;
            btnDangNhap.CustomizableEdges = customizableEdges1;
            btnDangNhap.DisabledState.BorderColor = Color.DarkGray;
            btnDangNhap.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDangNhap.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDangNhap.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDangNhap.FillColor = Color.FromArgb(31, 111, 235);
            btnDangNhap.Font = new Font("Calibri", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(103, 621);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.ShadowDecoration.BorderRadius = 27;
            btnDangNhap.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnDangNhap.ShadowDecoration.Depth = 5;
            btnDangNhap.ShadowDecoration.Enabled = true;
            btnDangNhap.Size = new Size(501, 65);
            btnDangNhap.TabIndex = 24;
            btnDangNhap.Text = "Đăng Nhập";
            btnDangNhap.Click += btnDangNhap_Click_1;
            // 
            // cbLuuThongTin
            // 
            cbLuuThongTin.AutoSize = true;
            cbLuuThongTin.CheckedState.BorderColor = Color.FromArgb(31, 111, 235);
            cbLuuThongTin.CheckedState.BorderRadius = 2;
            cbLuuThongTin.CheckedState.BorderThickness = 0;
            cbLuuThongTin.CheckedState.FillColor = Color.FromArgb(31, 111, 235);
            cbLuuThongTin.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbLuuThongTin.Location = new Point(49, 577);
            cbLuuThongTin.Name = "cbLuuThongTin";
            cbLuuThongTin.Size = new Size(247, 28);
            cbLuuThongTin.TabIndex = 23;
            cbLuuThongTin.Text = "Lưu thông tin đăng nhập!";
            cbLuuThongTin.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            cbLuuThongTin.UncheckedState.BorderRadius = 2;
            cbLuuThongTin.UncheckedState.BorderThickness = 1;
            cbLuuThongTin.UncheckedState.FillColor = Color.White;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(49, 447);
            label3.Name = "label3";
            label3.Size = new Size(128, 35);
            label3.TabIndex = 22;
            label3.Text = "Mật Khẩu";
            // 
            // txtMatKhau
            // 
            txtMatKhau.BackColor = Color.Transparent;
            txtMatKhau.BorderColor = Color.FromArgb(225, 229, 234);
            txtMatKhau.BorderRadius = 10;
            txtMatKhau.BorderThickness = 2;
            txtMatKhau.CustomizableEdges = customizableEdges3;
            txtMatKhau.DefaultText = "123456";
            txtMatKhau.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtMatKhau.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtMatKhau.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtMatKhau.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtMatKhau.FocusedState.BorderColor = Color.FromArgb(31, 111, 235);
            txtMatKhau.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtMatKhau.ForeColor = Color.DodgerBlue;
            txtMatKhau.HoverState.BorderColor = Color.FromArgb(31, 111, 235);
            txtMatKhau.Location = new Point(65, 485);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PasswordChar = '●';
            txtMatKhau.PlaceholderText = "";
            txtMatKhau.SelectedText = "";
            txtMatKhau.ShadowDecoration.BorderRadius = 10;
            txtMatKhau.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtMatKhau.ShadowDecoration.Depth = 2;
            txtMatKhau.ShadowDecoration.Enabled = true;
            txtMatKhau.Size = new Size(586, 45);
            txtMatKhau.TabIndex = 21;
            txtMatKhau.TextChanged += txtMatKhau_TextChanged;
            txtMatKhau.KeyPress += txtMatKhau_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(49, 349);
            label2.Name = "label2";
            label2.Size = new Size(128, 35);
            label2.TabIndex = 20;
            label2.Text = "Tài Khoản";
            // 
            // txtTaiKhoan
            // 
            txtTaiKhoan.BackColor = Color.Transparent;
            txtTaiKhoan.BorderColor = Color.FromArgb(225, 229, 234);
            txtTaiKhoan.BorderRadius = 10;
            txtTaiKhoan.BorderThickness = 2;
            txtTaiKhoan.CustomizableEdges = customizableEdges5;
            txtTaiKhoan.DefaultText = "admin";
            txtTaiKhoan.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtTaiKhoan.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtTaiKhoan.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtTaiKhoan.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtTaiKhoan.FocusedState.BorderColor = Color.FromArgb(31, 111, 235);
            txtTaiKhoan.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTaiKhoan.ForeColor = Color.DodgerBlue;
            txtTaiKhoan.HoverState.BorderColor = Color.FromArgb(31, 111, 235);
            txtTaiKhoan.Location = new Point(65, 387);
            txtTaiKhoan.Name = "txtTaiKhoan";
            txtTaiKhoan.PlaceholderText = "";
            txtTaiKhoan.SelectedText = "";
            txtTaiKhoan.ShadowDecoration.BorderRadius = 10;
            txtTaiKhoan.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtTaiKhoan.ShadowDecoration.Depth = 2;
            txtTaiKhoan.ShadowDecoration.Enabled = true;
            txtTaiKhoan.Size = new Size(586, 45);
            txtTaiKhoan.TabIndex = 19;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1382, 853);
            Controls.Add(roundedPanel1);
            Controls.Add(pictureBox1);
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
        private Guna2Panel roundedPanel1;
        private Controls.RainbowTitle rainbowTitle1;
        private Guna2Button btnDangNhap;
        private Guna2CheckBox cbLuuThongTin;
        private Label label3;
        private Guna2TextBox txtMatKhau;
        private Label label2;
        private Guna2TextBox txtTaiKhoan;
    }
}