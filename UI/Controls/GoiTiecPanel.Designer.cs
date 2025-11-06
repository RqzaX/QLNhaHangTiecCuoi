namespace UI.Controls
{
    partial class GoiTiecPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoiTiecPanel));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            uiPanel1 = new Sunny.UI.UIPanel();
            pictureBox1 = new PictureBox();
            lbTenGoi = new Label();
            lbSoTienCua1Ban = new Label();
            btnChiTietGoi = new Guna.UI2.WinForms.Guna2Button();
            uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // uiPanel1
            // 
            uiPanel1.Controls.Add(pictureBox1);
            uiPanel1.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel1.Location = new Point(12, 10);
            uiPanel1.Margin = new Padding(4, 5, 4, 5);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.Radius = 35;
            uiPanel1.Size = new Size(55, 55);
            uiPanel1.TabIndex = 0;
            uiPanel1.Text = null;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(4, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(47, 47);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lbTenGoi
            // 
            lbTenGoi.AccessibleDescription = "tên gói tiệc";
            lbTenGoi.AutoSize = true;
            lbTenGoi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTenGoi.Location = new Point(85, 14);
            lbTenGoi.Name = "lbTenGoi";
            lbTenGoi.Size = new Size(94, 21);
            lbTenGoi.TabIndex = 8;
            lbTenGoi.Text = "Gói tiệc VIP";
            // 
            // lbSoTienCua1Ban
            // 
            lbSoTienCua1Ban.AccessibleDescription = "số tiền của 1 bàn trong gói";
            lbSoTienCua1Ban.AutoSize = true;
            lbSoTienCua1Ban.BackColor = Color.Transparent;
            lbSoTienCua1Ban.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbSoTienCua1Ban.ForeColor = Color.LightSeaGreen;
            lbSoTienCua1Ban.Location = new Point(85, 40);
            lbSoTienCua1Ban.Name = "lbSoTienCua1Ban";
            lbSoTienCua1Ban.Size = new Size(129, 21);
            lbSoTienCua1Ban.TabIndex = 9;
            lbSoTienCua1Ban.Text = "5.000.000 đ/Bàn";
            // 
            // btnChiTietGoi
            // 
            btnChiTietGoi.BorderColor = Color.DimGray;
            btnChiTietGoi.BorderRadius = 18;
            btnChiTietGoi.BorderThickness = 1;
            btnChiTietGoi.CustomizableEdges = customizableEdges1;
            btnChiTietGoi.DisabledState.BorderColor = Color.DarkGray;
            btnChiTietGoi.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChiTietGoi.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChiTietGoi.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChiTietGoi.FillColor = Color.White;
            btnChiTietGoi.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChiTietGoi.ForeColor = Color.Black;
            btnChiTietGoi.Image = (Image)resources.GetObject("btnChiTietGoi.Image");
            btnChiTietGoi.Location = new Point(388, 15);
            btnChiTietGoi.Name = "btnChiTietGoi";
            btnChiTietGoi.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnChiTietGoi.Size = new Size(180, 45);
            btnChiTietGoi.TabIndex = 10;
            btnChiTietGoi.Text = "Chi tiết";
            // 
            // GoiTiecPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.White;
            Controls.Add(btnChiTietGoi);
            Controls.Add(lbSoTienCua1Ban);
            Controls.Add(lbTenGoi);
            Controls.Add(uiPanel1);
            Name = "GoiTiecPanel";
            Size = new Size(580, 74);
            uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private PictureBox pictureBox1;
        private Label lbTenGoi;
        private Label lbSoTienCua1Ban;
        private Guna.UI2.WinForms.Guna2Button btnChiTietGoi;
    }
}