namespace UI.Controls
{
    partial class VaiTroCard
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VaiTroCard));
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
            btnXoa.BackColor = Color.Transparent;
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
            btnXoa.ImageSize = new Size(30, 30);
            btnXoa.Location = new Point(273, 122);
            btnXoa.Margin = new Padding(3, 2, 3, 2);
            btnXoa.Name = "btnXoa";
            btnXoa.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnXoa.Size = new Size(41, 35);
            btnXoa.TabIndex = 8;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnSua
            // 
            btnSua.Animated = true;
            btnSua.AnimatedGIF = true;
            btnSua.BackColor = Color.White;
            btnSua.BorderColor = Color.FromArgb(220, 220, 220);
            btnSua.BorderRadius = 16;
            btnSua.BorderThickness = 1;
            btnSua.CustomizableEdges = customizableEdges7;
            btnSua.DisabledState.BorderColor = Color.DarkGray;
            btnSua.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSua.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSua.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSua.FillColor = Color.Transparent;
            btnSua.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSua.ForeColor = Color.Black;
            btnSua.Image = (Image)resources.GetObject("btnSua.Image");
            btnSua.ImageSize = new Size(30, 30);
            btnSua.Location = new Point(10, 122);
            btnSua.Margin = new Padding(3, 2, 3, 2);
            btnSua.Name = "btnSua";
            btnSua.Padding = new Padding(4, 0, 0, 0);
            btnSua.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnSua.Size = new Size(257, 35);
            btnSua.TabIndex = 7;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // lblUserCount
            // 
            lblUserCount.AutoSize = true;
            lblUserCount.BackColor = Color.Transparent;
            lblUserCount.Font = new Font("Segoe UI", 10F);
            lblUserCount.ForeColor = Color.FromArgb(64, 64, 64);
            lblUserCount.Location = new Point(49, 85);
            lblUserCount.Name = "lblUserCount";
            lblUserCount.Size = new Size(92, 19);
            lblUserCount.TabIndex = 5;
            lblUserCount.Text = "2 người dùng";
            // 
            // picUsers
            // 
            picUsers.BackColor = Color.Transparent;
            picUsers.Image = (Image)resources.GetObject("picUsers.Image");
            picUsers.Location = new Point(19, 83);
            picUsers.Margin = new Padding(3, 2, 3, 2);
            picUsers.Name = "picUsers";
            picUsers.Size = new Size(26, 22);
            picUsers.SizeMode = PictureBoxSizeMode.Zoom;
            picUsers.TabIndex = 4;
            picUsers.TabStop = false;
            // 
            // lbMoTa
            // 
            lbMoTa.BackColor = Color.Transparent;
            lbMoTa.Font = new Font("Segoe UI", 10F);
            lbMoTa.ForeColor = Color.Black;
            lbMoTa.Location = new Point(10, 61);
            lbMoTa.Name = "lbMoTa";
            lbMoTa.Size = new Size(315, 22);
            lbMoTa.TabIndex = 3;
            lbMoTa.Text = "Toàn quyền quản lý hệ thống";
            // 
            // panelVaiTro
            // 
            panelVaiTro.BackColor = Color.Transparent;
            panelVaiTro.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panelVaiTro.Location = new Point(19, 33);
            panelVaiTro.Margin = new Padding(4, 4, 4, 4);
            panelVaiTro.MinimumSize = new Size(1, 1);
            panelVaiTro.Name = "panelVaiTro";
            panelVaiTro.Radius = 20;
            panelVaiTro.Size = new Size(111, 24);
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
            uiLine1.Location = new Point(5, 106);
            uiLine1.Margin = new Padding(3, 2, 3, 2);
            uiLine1.MinimumSize = new Size(1, 1);
            uiLine1.Name = "uiLine1";
            uiLine1.Size = new Size(320, 12);
            uiLine1.TabIndex = 11;
            // 
            // lbTitle
            // 
            lbTitle.BackColor = Color.Transparent;
            lbTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lbTitle.ForeColor = Color.FromArgb(30, 30, 30);
            lbTitle.Location = new Point(16, 6);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new Size(310, 22);
            lbTitle.TabIndex = 0;
            lbTitle.Text = "Quản trị viên";
            // 
            // VaiTroCard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(uiLine1);
            Controls.Add(panelVaiTro);
            Controls.Add(btnXoa);
            Controls.Add(lbTitle);
            Controls.Add(btnSua);
            Controls.Add(lbMoTa);
            Controls.Add(picUsers);
            Controls.Add(lblUserCount);
            Margin = new Padding(3, 2, 3, 2);
            Name = "VaiTroCard";
            Size = new Size(334, 164);
            ((System.ComponentModel.ISupportInitialize)picUsers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbMoTa;
        private System.Windows.Forms.PictureBox picUsers;
        private System.Windows.Forms.Label lblUserCount;
        private Guna.UI2.WinForms.Guna2Button btnSua;
        private Guna.UI2.WinForms.Guna2Button btnXoa;
        private Sunny.UI.UIPanel panelVaiTro;
        private Sunny.UI.UILine uiLine1;
    }
}

