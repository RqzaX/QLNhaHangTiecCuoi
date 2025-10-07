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
            checkBox1 = new CheckBox();
            label3 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            textBox1 = new TextBox();
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
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(rainbowTitle1);
            roundedPanel1.Controls.Add(btnDangNhap);
            roundedPanel1.Controls.Add(checkBox1);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Controls.Add(textBox2);
            roundedPanel1.Controls.Add(label2);
            roundedPanel1.Controls.Add(textBox1);
            roundedPanel1.Location = new Point(649, 39);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(692, 772);
            roundedPanel1.TabIndex = 1;
            // 
            // rainbowTitle1
            // 
            rainbowTitle1.BackColor = Color.White;
            rainbowTitle1.Font = new Font("Calibri", 40.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rainbowTitle1.ForeColor = Color.White;
            rainbowTitle1.Location = new Point(32, 46);
            rainbowTitle1.Name = "rainbowTitle1";
            rainbowTitle1.Size = new Size(636, 254);
            rainbowTitle1.TabIndex = 25;
            rainbowTitle1.Text = "Hệ thống Quản lý Nhà hàng Tiệc cưới";
            // 
            // btnDangNhap
            // 
            btnDangNhap.BackColor = Color.FromArgb(31, 111, 235);
            btnDangNhap.BorderThickness = 2;
            btnDangNhap.CornerRadius = 27;
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnDangNhap.Font = new Font("Calibri", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangNhap.ForeColor = Color.White;
            btnDangNhap.Location = new Point(103, 621);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Padding = new Padding(10, 6, 10, 6);
            btnDangNhap.Size = new Size(500, 65);
            btnDangNhap.TabIndex = 24;
            btnDangNhap.Text = "Đăng Nhập";
            btnDangNhap.UseVisualStyleBackColor = false;
            btnDangNhap.Click += btnDangNhap_Click_1;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox1.Location = new Point(49, 577);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(247, 28);
            checkBox1.TabIndex = 23;
            checkBox1.Text = "Lưu thông tin đăng nhập!";
            checkBox1.UseVisualStyleBackColor = true;
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
            // textBox2
            // 
            textBox2.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox2.ForeColor = Color.DodgerBlue;
            textBox2.Location = new Point(65, 485);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(586, 41);
            textBox2.TabIndex = 21;
            textBox2.Text = "123456";
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
            // textBox1
            // 
            textBox1.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.DodgerBlue;
            textBox1.Location = new Point(65, 387);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(586, 41);
            textBox1.TabIndex = 19;
            textBox1.Text = "admin";
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
        private CheckBox checkBox1;
        private Label label3;
        private TextBox textBox2;
        private Label label2;
        private TextBox textBox1;
    }
}