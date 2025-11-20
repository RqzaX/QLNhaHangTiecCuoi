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
            btnDanhSachNhanVien = new Guna.UI2.WinForms.Guna2Button();
            panelDanhSachVaiTro = new Panel();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(10, 33);
            label2.Name = "label2";
            label2.Size = new Size(258, 19);
            label2.TabIndex = 19;
            label2.Text = "Quản lý vai trò và phân quyền truy cập";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(10, 7);
            label1.Name = "label1";
            label1.Size = new Size(189, 27);
            label1.TabIndex = 18;
            label1.Text = "Phân quyền (RBAC)";
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(label7);
            roundedPanel1.Controls.Add(label4);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.CornerRadius = 16;
            roundedPanel1.Location = new Point(10, 71);
            roundedPanel1.Margin = new Padding(3, 2, 3, 2);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(10, 9, 10, 9);
            roundedPanel1.Size = new Size(266, 88);
            roundedPanel1.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(64, 64, 64);
            label7.Location = new Point(13, 63);
            label7.Name = "label7";
            label7.Size = new Size(145, 19);
            label7.TabIndex = 24;
            label7.Text = "Vai trò đã định nghĩa";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(13, 37);
            label4.Name = "label4";
            label4.Size = new Size(23, 27);
            label4.TabIndex = 24;
            label4.Text = "5";
            label4.Click += label4_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(13, 9);
            label3.Name = "label3";
            label3.Size = new Size(99, 23);
            label3.TabIndex = 23;
            label3.Text = "Tổng vai trò";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.White;
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(label8);
            roundedPanel2.Controls.Add(label5);
            roundedPanel2.Controls.Add(label6);
            roundedPanel2.CornerRadius = 16;
            roundedPanel2.Location = new Point(282, 71);
            roundedPanel2.Margin = new Padding(3, 2, 3, 2);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(10, 9, 10, 9);
            roundedPanel2.Size = new Size(266, 88);
            roundedPanel2.TabIndex = 22;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.FromArgb(64, 64, 64);
            label8.Location = new Point(13, 63);
            label8.Name = "label8";
            label8.Size = new Size(185, 19);
            label8.TabIndex = 25;
            label8.Text = "Người dùng trong hệ thống";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(13, 37);
            label5.Name = "label5";
            label5.Size = new Size(34, 27);
            label5.TabIndex = 26;
            label5.Text = "36";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Calibri", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(13, 9);
            label6.Name = "label6";
            label6.Size = new Size(138, 23);
            label6.TabIndex = 25;
            label6.Text = "Tổng người dùng";
            // 
            // btnDanhSachNhanVien
            // 
            btnDanhSachNhanVien.Animated = true;
            btnDanhSachNhanVien.BorderColor = Color.Gray;
            btnDanhSachNhanVien.BorderRadius = 20;
            btnDanhSachNhanVien.BorderThickness = 1;
            btnDanhSachNhanVien.CustomizableEdges = customizableEdges1;
            btnDanhSachNhanVien.DisabledState.BorderColor = Color.DarkGray;
            btnDanhSachNhanVien.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDanhSachNhanVien.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDanhSachNhanVien.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDanhSachNhanVien.FillColor = Color.White;
            btnDanhSachNhanVien.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhSachNhanVien.ForeColor = Color.Black;
            btnDanhSachNhanVien.Image = (Image)resources.GetObject("btnDanhSachNhanVien.Image");
            btnDanhSachNhanVien.Location = new Point(783, 133);
            btnDanhSachNhanVien.Margin = new Padding(3, 2, 3, 2);
            btnDanhSachNhanVien.Name = "btnDanhSachNhanVien";
            btnDanhSachNhanVien.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnDanhSachNhanVien.Size = new Size(215, 42);
            btnDanhSachNhanVien.TabIndex = 23;
            btnDanhSachNhanVien.Text = "Danh Sách Nhân Viên";
            // 
            // panelDanhSachVaiTro
            // 
            panelDanhSachVaiTro.Dock = DockStyle.Bottom;
            panelDanhSachVaiTro.Location = new Point(0, 179);
            panelDanhSachVaiTro.Margin = new Padding(3, 2, 3, 2);
            panelDanhSachVaiTro.Name = "panelDanhSachVaiTro";
            panelDanhSachVaiTro.Size = new Size(1041, 496);
            panelDanhSachVaiTro.TabIndex = 24;
            // 
            // FrmPhanQuyen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1041, 675);
            Controls.Add(panelDanhSachVaiTro);
            Controls.Add(btnDanhSachNhanVien);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
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
        private Guna.UI2.WinForms.Guna2Button btnDanhSachNhanVien;
        private Label label7;
        private Label label8;
        private Panel panelDanhSachVaiTro;
    }
}