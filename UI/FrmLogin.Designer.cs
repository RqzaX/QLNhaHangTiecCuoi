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
            roundedButton1 = new UI.Controls.RoundedButton();
            checkBox1 = new CheckBox();
            label3 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            cbbChiNhanh = new ComboBox();
            textBox1 = new TextBox();
            label1 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            roundedPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(46, 184);
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
            roundedPanel1.Controls.Add(label4);
            roundedPanel1.Controls.Add(roundedButton1);
            roundedPanel1.Controls.Add(checkBox1);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Controls.Add(textBox2);
            roundedPanel1.Controls.Add(label2);
            roundedPanel1.Controls.Add(cbbChiNhanh);
            roundedPanel1.Controls.Add(textBox1);
            roundedPanel1.Controls.Add(label1);
            roundedPanel1.Location = new Point(649, 53);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(758, 840);
            roundedPanel1.TabIndex = 1;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.FromArgb(31, 111, 235);
            roundedButton1.CornerRadius = 30;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Calibri", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.Location = new Point(143, 529);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.Size = new Size(500, 65);
            roundedButton1.TabIndex = 8;
            roundedButton1.Text = "Đăng Nhập";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox1.Location = new Point(90, 466);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(247, 28);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "Lưu thông tin đăng nhập!";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(90, 403);
            label3.Name = "label3";
            label3.Size = new Size(128, 35);
            label3.TabIndex = 6;
            label3.Text = "Mật Khẩu";
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox2.ForeColor = Color.DodgerBlue;
            textBox2.Location = new Point(224, 400);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(470, 41);
            textBox2.TabIndex = 5;
            textBox2.Text = "123456";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(90, 357);
            label2.Name = "label2";
            label2.Size = new Size(128, 35);
            label2.TabIndex = 4;
            label2.Text = "Tài Khoản";
            // 
            // cbbChiNhanh
            // 
            cbbChiNhanh.Font = new Font("Calibri", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbbChiNhanh.FormattingEnabled = true;
            cbbChiNhanh.Items.AddRange(new object[] { "HCM-Q1 — Grand Palace Quận 1 — 74 Nguyễn Du, Q.1, TP.HCM", "HCM-Q7 — Grand Palace Phú Mỹ Hưng — 801 Nguyễn Văn Linh, Q.7, TP.HCM", "HCM-TB — Grand Palace Tân Bình — 18 Cộng Hòa, Tân Bình, TP.HCM", "HN-HK — Grand Palace Hoàn Kiếm — 12 Tràng Thi, Hoàn Kiếm, Hà Nội" });
            cbbChiNhanh.Location = new Point(156, 307);
            cbbChiNhanh.Name = "cbbChiNhanh";
            cbbChiNhanh.Size = new Size(587, 36);
            cbbChiNhanh.TabIndex = 3;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Century Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.DodgerBlue;
            textBox1.Location = new Point(224, 354);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(470, 41);
            textBox1.TabIndex = 2;
            textBox1.Text = "admin";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(15, 308);
            label1.Name = "label1";
            label1.Size = new Size(135, 35);
            label1.TabIndex = 1;
            label1.Text = "Chi Nhánh";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(575, 808);
            label4.Name = "label4";
            label4.Size = new Size(168, 20);
            label4.TabIndex = 9;
            label4.Text = "V1.0 - 2025 By Nhóm 11";
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1482, 943);
            Controls.Add(roundedPanel1);
            Controls.Add(pictureBox1);
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmLogin";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Controls.RoundedPanel roundedPanel1;
        private Label label3;
        private TextBox textBox2;
        private Label label2;
        private ComboBox cbbChiNhanh;
        private TextBox textBox1;
        private Label label1;
        private CheckBox checkBox1;
        private Controls.RoundedButton roundedButton1;
        private Label label4;
    }
}