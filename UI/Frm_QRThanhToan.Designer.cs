using Guna.UI2.WinForms;

namespace UI
{
    partial class Frm_QRThanhToan
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_QRThanhToan));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panelMain = new Guna2GradientPanel();
            btnThoat = new Guna2Button();
            btnDong = new Guna2Button();
            lbNoiDung = new Label();
            lbSoTien = new Label();
            lbTieuDe = new Label();
            guna2PictureBox1 = new Guna2PictureBox();
            panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.Transparent;
            panelMain.BorderColor = Color.DimGray;
            panelMain.BorderRadius = 20;
            panelMain.BorderThickness = 1;
            panelMain.Controls.Add(guna2PictureBox1);
            panelMain.Controls.Add(btnThoat);
            panelMain.Controls.Add(btnDong);
            panelMain.Controls.Add(lbNoiDung);
            panelMain.Controls.Add(lbSoTien);
            panelMain.Controls.Add(lbTieuDe);
            panelMain.CustomizableEdges = customizableEdges7;
            panelMain.Dock = DockStyle.Fill;
            panelMain.FillColor = Color.White;
            panelMain.FillColor2 = Color.White;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.ShadowDecoration.BorderRadius = 20;
            panelMain.ShadowDecoration.Color = Color.FromArgb(100, 0, 0, 0);
            panelMain.ShadowDecoration.CustomizableEdges = customizableEdges8;
            panelMain.ShadowDecoration.Depth = 20;
            panelMain.ShadowDecoration.Enabled = true;
            panelMain.Size = new Size(450, 450);
            panelMain.TabIndex = 0;
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.Transparent;
            btnThoat.BorderRadius = 15;
            btnThoat.CustomizableEdges = customizableEdges3;
            btnThoat.DisabledState.BorderColor = Color.DarkGray;
            btnThoat.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThoat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThoat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThoat.FillColor = Color.Transparent;
            btnThoat.Font = new Font("Comic Sans MS", 25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = Color.Black;
            btnThoat.Location = new Point(398, 2);
            btnThoat.Name = "btnThoat";
            btnThoat.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnThoat.Size = new Size(50, 50);
            btnThoat.TabIndex = 5;
            btnThoat.Text = "X";
            btnThoat.Click += btnThoat_Click;
            // 
            // btnDong
            // 
            btnDong.Animated = true;
            btnDong.BorderColor = Color.DimGray;
            btnDong.BorderRadius = 15;
            btnDong.BorderThickness = 1;
            btnDong.CustomizableEdges = customizableEdges5;
            btnDong.DisabledState.BorderColor = Color.DarkGray;
            btnDong.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDong.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDong.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDong.FillColor = Color.White;
            btnDong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDong.ForeColor = Color.Black;
            btnDong.Location = new Point(326, 393);
            btnDong.Name = "btnDong";
            btnDong.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnDong.Size = new Size(112, 45);
            btnDong.TabIndex = 4;
            btnDong.Text = "Đóng";
            btnDong.Click += btnDong_Click;
            // 
            // lbNoiDung
            // 
            lbNoiDung.AutoSize = true;
            lbNoiDung.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbNoiDung.ForeColor = Color.FromArgb(128, 128, 128);
            lbNoiDung.Location = new Point(56, 403);
            lbNoiDung.Name = "lbNoiDung";
            lbNoiDung.Size = new Size(213, 20);
            lbNoiDung.TabIndex = 3;
            lbNoiDung.Text = "Nội dung: Cọc 20% tổng giá trị";
            lbNoiDung.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbSoTien
            // 
            lbSoTien.AutoSize = true;
            lbSoTien.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbSoTien.ForeColor = Color.FromArgb(0, 192, 0);
            lbSoTien.Location = new Point(56, 371);
            lbSoTien.Name = "lbSoTien";
            lbSoTien.Size = new Size(49, 32);
            lbSoTien.TabIndex = 2;
            lbSoTien.Text = "0 ₫";
            lbSoTien.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbTieuDe
            // 
            lbTieuDe.AutoSize = true;
            lbTieuDe.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTieuDe.ForeColor = Color.FromArgb(64, 64, 64);
            lbTieuDe.Location = new Point(32, 22);
            lbTieuDe.Name = "lbTieuDe";
            lbTieuDe.Size = new Size(282, 30);
            lbTieuDe.TabIndex = 1;
            lbTieuDe.Text = "Quét mã QR để thanh toán";
            lbTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.BackColor = Color.Transparent;
            guna2PictureBox1.BorderRadius = 25;
            guna2PictureBox1.CustomizableEdges = customizableEdges1;
            guna2PictureBox1.Image = (Image)resources.GetObject("guna2PictureBox1.Image");
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(86, 71);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PictureBox1.Size = new Size(280, 280);
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            guna2PictureBox1.TabIndex = 6;
            guna2PictureBox1.TabStop = false;
            // 
            // Frm_QRThanhToan
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(450, 450);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Frm_QRThanhToan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "QR Thanh toán";
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna2GradientPanel panelMain;
        private Guna2Button btnThoat;
        private Label lbTieuDe;
        private Label lbSoTien;
        private Label lbNoiDung;
        private Guna2Button btnDong;
        private Guna2PictureBox guna2PictureBox1;
    }
}
