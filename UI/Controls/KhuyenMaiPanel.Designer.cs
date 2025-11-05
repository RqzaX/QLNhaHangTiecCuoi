namespace UI.Controls
{
    partial class KhuyenMaiPanel
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
            lbSoTienGiam = new Label();
            panelLoaiApDung = new Sunny.UI.UIPanel();
            lbHanKM = new Label();
            label1 = new Label();
            lbSoGiamGiaKM = new Label();
            lbTenKM = new Label();
            lbMaKM = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // lbSoTienGiam
            // 
            lbSoTienGiam.AccessibleDescription = "số tiền được giảm khi áp cái mã này";
            lbSoTienGiam.AutoSize = true;
            lbSoTienGiam.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbSoTienGiam.ForeColor = Color.LimeGreen;
            lbSoTienGiam.Location = new Point(144, 99);
            lbSoTienGiam.Name = "lbSoTienGiam";
            lbSoTienGiam.Size = new Size(82, 21);
            lbSoTienGiam.TabIndex = 46;
            lbSoTienGiam.Text = "107.000 đ";
            // 
            // panelLoaiApDung
            // 
            panelLoaiApDung.AccessibleDescription = "áp dụng cho nhà hàng hay là tiệc cưới, tất cả";
            panelLoaiApDung.FillColor = Color.FromArgb(244, 242, 251);
            panelLoaiApDung.FillColor2 = Color.FromArgb(244, 242, 251);
            panelLoaiApDung.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelLoaiApDung.Location = new Point(396, 10);
            panelLoaiApDung.Margin = new Padding(4, 5, 4, 5);
            panelLoaiApDung.MinimumSize = new Size(1, 1);
            panelLoaiApDung.Name = "panelLoaiApDung";
            panelLoaiApDung.Radius = 21;
            panelLoaiApDung.RectColor = Color.FromArgb(102, 58, 183);
            panelLoaiApDung.Size = new Size(73, 25);
            panelLoaiApDung.Style = Sunny.UI.UIStyle.Custom;
            panelLoaiApDung.TabIndex = 45;
            panelLoaiApDung.Text = "Tất cả";
            panelLoaiApDung.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lbHanKM
            // 
            lbHanKM.AccessibleDescription = "hạn kết thúc khuyến mãi";
            lbHanKM.AutoSize = true;
            lbHanKM.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbHanKM.ForeColor = Color.FromArgb(64, 64, 64);
            lbHanKM.Location = new Point(245, 62);
            lbHanKM.Name = "lbHanKM";
            lbHanKM.Size = new Size(85, 20);
            lbHanKM.TabIndex = 44;
            lbHanKM.Text = "30/11/2025";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Gray;
            label1.Location = new Point(7, 79);
            label1.Name = "label1";
            label1.Size = new Size(459, 20);
            label1.TabIndex = 43;
            label1.Text = "——————————————————————————————";
            // 
            // lbSoGiamGiaKM
            // 
            lbSoGiamGiaKM.AccessibleDescription = "giảm ví dụ 20% hay giảm 100.000 đ";
            lbSoGiamGiaKM.AutoSize = true;
            lbSoGiamGiaKM.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbSoGiamGiaKM.ForeColor = Color.FromArgb(64, 64, 64);
            lbSoGiamGiaKM.Location = new Point(13, 62);
            lbSoGiamGiaKM.Name = "lbSoGiamGiaKM";
            lbSoGiamGiaKM.Size = new Size(96, 20);
            lbSoGiamGiaKM.TabIndex = 42;
            lbSoGiamGiaKM.Text = "% Giảm 20 %";
            // 
            // lbTenKM
            // 
            lbTenKM.AccessibleDescription = "tên khuyến mãi";
            lbTenKM.AutoSize = true;
            lbTenKM.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTenKM.Location = new Point(12, 29);
            lbTenKM.Name = "lbTenKM";
            lbTenKM.Size = new Size(177, 20);
            lbTenKM.TabIndex = 41;
            lbTenKM.Text = "Khuyến mãi Khai Trương";
            // 
            // lbMaKM
            // 
            lbMaKM.AutoSize = true;
            lbMaKM.BackColor = Color.Transparent;
            lbMaKM.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbMaKM.ForeColor = Color.MediumSlateBlue;
            lbMaKM.Location = new Point(12, 9);
            lbMaKM.Name = "lbMaKM";
            lbMaKM.Size = new Size(124, 20);
            lbMaKM.TabIndex = 40;
            lbMaKM.Text = "{mã khuyễn mãi}";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(64, 64, 64);
            label4.Location = new Point(13, 99);
            label4.Name = "label4";
            label4.Size = new Size(134, 20);
            label4.TabIndex = 47;
            label4.Text = "Số tiền được giảm:";
            // 
            // KhuyenMaiPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(487, 127);
            Controls.Add(lbSoTienGiam);
            Controls.Add(panelLoaiApDung);
            Controls.Add(lbHanKM);
            Controls.Add(label1);
            Controls.Add(lbSoGiamGiaKM);
            Controls.Add(lbTenKM);
            Controls.Add(lbMaKM);
            Controls.Add(label4);
            FormBorderStyle = FormBorderStyle.None;
            Name = "KhuyenMaiPanel";
            Text = "KhuyenMaiPanel";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbSoTienGiam;
        private Sunny.UI.UIPanel panelLoaiApDung;
        private Label lbHanKM;
        private Label label1;
        private Label lbSoGiamGiaKM;
        private Label lbTenKM;
        private Label lbMaKM;
        private Label label4;
    }
}