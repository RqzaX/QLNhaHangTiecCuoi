namespace UI
{
    partial class Frm_VaiTroPanel
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_VaiTroPanel));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            btnXoa = new Guna.UI2.WinForms.Guna2Button();
            btnSua = new Guna.UI2.WinForms.Guna2Button();
            lblUserCount = new Label();
            picUsers = new PictureBox();
            lbMoTa = new Label();
            panelVaiTro = new Sunny.UI.UIPanel();
            uiLine1 = new Sunny.UI.UILine();
            lbTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)picUsers).BeginInit();
            SuspendLayout();
            // 
            // btnXoa
            // 
            btnXoa.Animated = true;
            btnXoa.AnimatedGIF = true;
            btnXoa.BorderColor = Color.FromArgb(220, 220, 220);
            btnXoa.BorderRadius = 8;
            btnXoa.BorderThickness = 1;
            btnXoa.CustomizableEdges = customizableEdges5;
            btnXoa.DisabledState.BorderColor = Color.DarkGray;
            btnXoa.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXoa.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXoa.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXoa.FillColor = Color.White;
            btnXoa.Font = new Font("Segoe UI", 9F);
            btnXoa.ForeColor = Color.FromArgb(150, 150, 150);
            btnXoa.Image = (Image)resources.GetObject("btnXoa.Image");
            btnXoa.ImageOffset = new Point(1, 0);
            btnXoa.ImageSize = new Size(30, 30);
            btnXoa.Location = new Point(312, 188);
            btnXoa.Name = "btnXoa";
            btnXoa.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnXoa.Size = new Size(47, 47);
            btnXoa.TabIndex = 8;
            // 
            // btnSua
            // 
            btnSua.Animated = true;
            btnSua.AnimatedGIF = true;
            btnSua.BorderColor = Color.FromArgb(220, 220, 220);
            btnSua.BorderRadius = 16;
            btnSua.BorderThickness = 1;
            btnSua.CustomizableEdges = customizableEdges7;
            btnSua.DisabledState.BorderColor = Color.DarkGray;
            btnSua.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSua.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSua.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSua.FillColor = Color.White;
            btnSua.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSua.ForeColor = Color.FromArgb(70, 70, 77);
            btnSua.Image = (Image)resources.GetObject("btnSua.Image");
            btnSua.ImageSize = new Size(30, 30);
            btnSua.Location = new Point(12, 188);
            btnSua.Name = "btnSua";
            btnSua.Padding = new Padding(5, 0, 0, 0);
            btnSua.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnSua.Size = new Size(294, 47);
            btnSua.TabIndex = 7;
            btnSua.Text = "Sửa";
            // 
            // lblUserCount
            // 
            lblUserCount.AutoSize = true;
            lblUserCount.Font = new Font("Segoe UI", 10F);
            lblUserCount.ForeColor = Color.FromArgb(64, 64, 64);
            lblUserCount.Location = new Point(56, 138);
            lblUserCount.Name = "lblUserCount";
            lblUserCount.Size = new Size(113, 23);
            lblUserCount.TabIndex = 5;
            lblUserCount.Text = "2 người dùng";
            // 
            // picUsers
            // 
            picUsers.Image = (Image)resources.GetObject("picUsers.Image");
            picUsers.Location = new Point(22, 136);
            picUsers.Name = "picUsers";
            picUsers.Size = new Size(30, 30);
            picUsers.SizeMode = PictureBoxSizeMode.Zoom;
            picUsers.TabIndex = 4;
            picUsers.TabStop = false;
            // 
            // lbMoTa
            // 
            lbMoTa.Font = new Font("Segoe UI", 10F);
            lbMoTa.ForeColor = Color.Black;
            lbMoTa.Location = new Point(12, 106);
            lbMoTa.Name = "lbMoTa";
            lbMoTa.Size = new Size(360, 30);
            lbMoTa.TabIndex = 3;
            lbMoTa.Text = "Toàn quyền quản lý hệ thống";
            // 
            // panelVaiTro
            // 
            panelVaiTro.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panelVaiTro.Location = new Point(22, 44);
            panelVaiTro.Margin = new Padding(4, 5, 4, 5);
            panelVaiTro.MinimumSize = new Size(1, 1);
            panelVaiTro.Name = "panelVaiTro";
            panelVaiTro.Radius = 20;
            panelVaiTro.Size = new Size(127, 32);
            panelVaiTro.TabIndex = 10;
            panelVaiTro.Text = "admin";
            panelVaiTro.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // uiLine1
            // 
            uiLine1.BackColor = Color.Transparent;
            uiLine1.Font = new Font("Microsoft Sans Serif", 12F);
            uiLine1.ForeColor = Color.FromArgb(48, 48, 48);
            uiLine1.LineColor = Color.Silver;
            uiLine1.Location = new Point(6, 167);
            uiLine1.MinimumSize = new Size(1, 1);
            uiLine1.Name = "uiLine1";
            uiLine1.Size = new Size(366, 16);
            uiLine1.TabIndex = 11;
            // 
            // lbTitle
            // 
            lbTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lbTitle.ForeColor = Color.FromArgb(30, 30, 30);
            lbTitle.Location = new Point(18, 9);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new Size(200, 30);
            lbTitle.TabIndex = 0;
            lbTitle.Text = "Quản trị viên";
            // 
            // Frm_VaiTroPanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(382, 250);
            Controls.Add(uiLine1);
            Controls.Add(panelVaiTro);
            Controls.Add(btnXoa);
            Controls.Add(lbTitle);
            Controls.Add(btnSua);
            Controls.Add(lbMoTa);
            Controls.Add(picUsers);
            Controls.Add(lblUserCount);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Frm_VaiTroPanel";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Vai Trò Panel";
            ((System.ComponentModel.ISupportInitialize)picUsers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lbMoTa;
        private System.Windows.Forms.PictureBox picUsers;
        private System.Windows.Forms.Label lblUserCount;
        private Guna.UI2.WinForms.Guna2Button btnSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Sunny.UI.UIPanel panelVaiTro;
        private Sunny.UI.UILine uiLine1;
        private Label lbTitle;
    }
}
