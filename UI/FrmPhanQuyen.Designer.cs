namespace UI
{
    partial class FrmPhanQuyen
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
            label2 = new Label();
            label1 = new Label();
            roleCard1 = new UI.Controls.RoleCard();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label4 = new Label();
            label3 = new Label();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            label5 = new Label();
            label6 = new Label();
            roundedPanel3 = new UI.Controls.RoundedPanel();
            label7 = new Label();
            label8 = new Label();
            roundedButton1 = new UI.Controls.RoundedButton();
            roleCard2 = new UI.Controls.RoleCard();
            roleCard3 = new UI.Controls.RoleCard();
            roleCard4 = new UI.Controls.RoleCard();
            roleCard5 = new UI.Controls.RoleCard();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
            roundedPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(335, 24);
            label2.TabIndex = 19;
            label2.Text = "Quản lý vai trò và phân quyền truy cập";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(240, 35);
            label1.TabIndex = 18;
            label1.Text = "Phân quyền (RBAC)";
            // 
            // roleCard1
            // 
            roleCard1.AccentColor = Color.BlueViolet;
            roleCard1.BackColor = Color.White;
            roleCard1.CornerRadius = 16;
            roleCard1.Description = "Toàn quyền quản lý hệ thống";
            roleCard1.Location = new Point(12, 396);
            roleCard1.Name = "roleCard1";
            roleCard1.Padding = new Padding(20);
            roleCard1.ShowEditButton = true;
            roleCard1.Size = new Size(335, 308);
            roleCard1.TabIndex = 20;
            roleCard1.TagText = "admin";
            roleCard1.Text = "roleCard1";
            roleCard1.Title = "Quản trị viên";
            roleCard1.UserCount = 2;
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(label4);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Location = new Point(12, 95);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(366, 185);
            roundedPanel1.TabIndex = 21;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(36, 100);
            label4.Name = "label4";
            label4.Size = new Size(29, 35);
            label4.TabIndex = 24;
            label4.Text = "5";
            label4.Click += label4_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(34, 40);
            label3.Name = "label3";
            label3.Size = new Size(120, 28);
            label3.TabIndex = 23;
            label3.Text = "Tổng vai trò";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.White;
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(label5);
            roundedPanel2.Controls.Add(label6);
            roundedPanel2.Location = new Point(384, 95);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(12);
            roundedPanel2.Size = new Size(366, 185);
            roundedPanel2.TabIndex = 22;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(47, 100);
            label5.Name = "label5";
            label5.Size = new Size(43, 35);
            label5.TabIndex = 26;
            label5.Text = "36";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Calibri", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(45, 40);
            label6.Name = "label6";
            label6.Size = new Size(168, 28);
            label6.TabIndex = 25;
            label6.Text = "Tổng người dùng";
            // 
            // roundedPanel3
            // 
            roundedPanel3.BackColor = Color.White;
            roundedPanel3.BorderThickness = 5;
            roundedPanel3.Controls.Add(label7);
            roundedPanel3.Controls.Add(label8);
            roundedPanel3.Location = new Point(756, 95);
            roundedPanel3.Name = "roundedPanel3";
            roundedPanel3.Padding = new Padding(12);
            roundedPanel3.Size = new Size(366, 185);
            roundedPanel3.TabIndex = 22;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(58, 100);
            label7.Name = "label7";
            label7.Size = new Size(43, 35);
            label7.TabIndex = 28;
            label7.Text = "35";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Calibri", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(56, 40);
            label8.Name = "label8";
            label8.Size = new Size(158, 28);
            label8.TabIndex = 27;
            label8.Text = "Tổng quyền hạn";
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.Black;
            roundedButton1.BorderColor = Color.Black;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.Location = new Point(935, 335);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.Size = new Size(187, 45);
            roundedButton1.TabIndex = 23;
            roundedButton1.Text = "+ Thêm vai trò mới";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // roleCard2
            // 
            roleCard2.AccentColor = Color.FromArgb(0, 192, 192);
            roleCard2.BackColor = Color.White;
            roleCard2.CornerRadius = 16;
            roleCard2.Description = "Quản lý chi nhánh và nhân viên";
            roleCard2.Location = new Point(384, 396);
            roleCard2.Name = "roleCard2";
            roleCard2.Padding = new Padding(20);
            roleCard2.ShowEditButton = true;
            roleCard2.Size = new Size(335, 308);
            roleCard2.TabIndex = 24;
            roleCard2.TagText = "manager";
            roleCard2.Text = "roleCard2";
            roleCard2.Title = "Quản lý";
            roleCard2.UserCount = 4;
            // 
            // roleCard3
            // 
            roleCard3.AccentColor = Color.ForestGreen;
            roleCard3.BackColor = Color.White;
            roleCard3.CornerRadius = 16;
            roleCard3.Description = "Thanh toán và xuất hóa đơn";
            roleCard3.Location = new Point(758, 396);
            roleCard3.Name = "roleCard3";
            roleCard3.Padding = new Padding(20);
            roleCard3.ShowEditButton = true;
            roleCard3.Size = new Size(335, 308);
            roleCard3.TabIndex = 25;
            roleCard3.TagText = "cashier";
            roleCard3.Text = "roleCard3";
            roleCard3.Title = "Thu ngân";
            roleCard3.UserCount = 7;
            // 
            // roleCard4
            // 
            roleCard4.AccentColor = Color.IndianRed;
            roleCard4.BackColor = Color.White;
            roleCard4.CornerRadius = 16;
            roleCard4.Description = "Xem và xử lý order bếp";
            roleCard4.Location = new Point(384, 710);
            roleCard4.Name = "roleCard4";
            roleCard4.Padding = new Padding(20);
            roleCard4.ShowEditButton = true;
            roleCard4.Size = new Size(335, 308);
            roleCard4.TabIndex = 27;
            roleCard4.TagText = "master chef";
            roleCard4.Text = "roleCard4";
            roleCard4.Title = "Đầu bếp";
            roleCard4.UserCount = 6;
            // 
            // roleCard5
            // 
            roleCard5.AccentColor = Color.Chocolate;
            roleCard5.BackColor = Color.White;
            roleCard5.CornerRadius = 16;
            roleCard5.Description = "Gọi món và phục vụ khách";
            roleCard5.Location = new Point(12, 710);
            roleCard5.Name = "roleCard5";
            roleCard5.Padding = new Padding(20);
            roleCard5.ShowEditButton = true;
            roleCard5.Size = new Size(335, 308);
            roleCard5.TabIndex = 26;
            roleCard5.TagText = "server";
            roleCard5.Text = "roleCard5";
            roleCard5.Title = "Phục vụ";
            roleCard5.UserCount = 15;
            // 
            // FrmPhanQuyen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1190, 900);
            Controls.Add(roleCard4);
            Controls.Add(roleCard5);
            Controls.Add(roleCard3);
            Controls.Add(roleCard2);
            Controls.Add(roundedButton1);
            Controls.Add(roundedPanel3);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(roleCard1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmPhanQuyen";
            Text = "FrmPhanQuyen";
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            roundedPanel3.ResumeLayout(false);
            roundedPanel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Controls.RoleCard roleCard1;
        private Controls.RoundedPanel roundedPanel1;
        private Label label4;
        private Label label3;
        private Controls.RoundedPanel roundedPanel2;
        private Controls.RoundedPanel roundedPanel3;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Controls.RoundedButton roundedButton1;
        private Controls.RoleCard roleCard2;
        private Controls.RoleCard roleCard3;
        private Controls.RoleCard roleCard4;
        private Controls.RoleCard roleCard5;
    }
}