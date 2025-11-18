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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPhanQuyen));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label2 = new Label();
            label1 = new Label();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label7 = new Label();
            label4 = new Label();
            label3 = new Label();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            label8 = new Label();
            label5 = new Label();
            label6 = new Label();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            panelDanhSachVaiTro = new Panel();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
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
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.CornerRadius = 16;
            roundedPanel1.Controls.Add(label7);
            roundedPanel1.Controls.Add(label4);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Location = new Point(12, 95);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(304, 118);
            roundedPanel1.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(64, 64, 64);
            label7.Location = new Point(15, 84);
            label7.Name = "label7";
            label7.Size = new Size(185, 24);
            label7.TabIndex = 24;
            label7.Text = "Vai trò đã định nghĩa";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(15, 49);
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
            label3.Location = new Point(15, 12);
            label3.Name = "label3";
            label3.Size = new Size(120, 28);
            label3.TabIndex = 23;
            label3.Text = "Tổng vai trò";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.White;
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.CornerRadius = 16;
            roundedPanel2.Controls.Add(label8);
            roundedPanel2.Controls.Add(label5);
            roundedPanel2.Controls.Add(label6);
            roundedPanel2.Location = new Point(322, 95);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(12);
            roundedPanel2.Size = new Size(304, 118);
            roundedPanel2.TabIndex = 22;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.FromArgb(64, 64, 64);
            label8.Location = new Point(15, 84);
            label8.Name = "label8";
            label8.Size = new Size(238, 24);
            label8.TabIndex = 25;
            label8.Text = "Người dùng trong hệ thống";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(15, 49);
            label5.Name = "label5";
            label5.Size = new Size(43, 35);
            label5.TabIndex = 26;
            label5.Text = "36";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Calibri", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(15, 12);
            label6.Name = "label6";
            label6.Size = new Size(168, 28);
            label6.TabIndex = 25;
            label6.Text = "Tổng người dùng";
            // 
            // guna2Button1
            // 
            guna2Button1.Animated = true;
            guna2Button1.BorderColor = Color.Gray;
            guna2Button1.BorderRadius = 20;
            guna2Button1.BorderThickness = 1;
            guna2Button1.CustomizableEdges = customizableEdges1;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.White;
            guna2Button1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.Black;
            guna2Button1.Image = (Image)resources.GetObject("guna2Button1.Image");
            guna2Button1.Location = new Point(912, 22);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button1.Size = new Size(225, 56);
            guna2Button1.TabIndex = 23;
            guna2Button1.Text = "Tạo vai trò mới";
            // 
            // panelDanhSachVaiTro
            // 
            panelDanhSachVaiTro.Dock = DockStyle.Bottom;
            panelDanhSachVaiTro.Location = new Point(0, 238);
            panelDanhSachVaiTro.Name = "panelDanhSachVaiTro";
            panelDanhSachVaiTro.Size = new Size(1190, 662);
            panelDanhSachVaiTro.TabIndex = 24;
            // 
            // FrmPhanQuyen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1190, 900);
            Controls.Add(panelDanhSachVaiTro);
            Controls.Add(guna2Button1);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmPhanQuyen";
            Text = "FrmPhanQuyen";
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Controls.RoundedPanel roundedPanel1;
        private Label label4;
        private Label label3;
        private Controls.RoundedPanel roundedPanel2;
        private Label label5;
        private Label label6;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Label label7;
        private Label label8;
        private Panel panelDanhSachVaiTro;
    }
}