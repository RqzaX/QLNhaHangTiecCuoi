namespace UI.Controls
{
    partial class HoaDonPanel
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
            lbSoBan = new Label();
            lbSoKhach_SoMon = new Label();
            label3 = new Label();
            panelMaHoaDon = new Sunny.UI.UIPanel();
            label4 = new Label();
            label5 = new Label();
            lbThoiGianLap = new Label();
            lbTamTinh = new Label();
            lbVAT = new Label();
            lbTongCong = new Label();
            label10 = new Label();
            label11 = new Label();
            SuspendLayout();
            // 
            // lbSoBan
            // 
            lbSoBan.AccessibleDescription = "tên Khu vực, tên số bàn";
            lbSoBan.AutoSize = true;
            lbSoBan.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbSoBan.Location = new Point(12, 9);
            lbSoBan.Name = "lbSoBan";
            lbSoBan.Size = new Size(89, 21);
            lbSoBan.TabIndex = 0;
            lbSoBan.Text = "Bàn T01-22";
            // 
            // lbSoKhach_SoMon
            // 
            lbSoKhach_SoMon.AutoSize = true;
            lbSoKhach_SoMon.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbSoKhach_SoMon.ForeColor = Color.FromArgb(64, 64, 64);
            lbSoKhach_SoMon.Location = new Point(12, 30);
            lbSoKhach_SoMon.Name = "lbSoKhach_SoMon";
            lbSoKhach_SoMon.Size = new Size(117, 20);
            lbSoKhach_SoMon.TabIndex = 1;
            lbSoKhach_SoMon.Text = "4 Khách - 8 Món";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(64, 64, 64);
            label3.Location = new Point(12, 64);
            label3.Name = "label3";
            label3.Size = new Size(67, 20);
            label3.TabIndex = 2;
            label3.Text = "Tạm tính";
            // 
            // panelMaHoaDon
            // 
            panelMaHoaDon.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelMaHoaDon.Location = new Point(363, 13);
            panelMaHoaDon.Margin = new Padding(4, 5, 4, 5);
            panelMaHoaDon.MinimumSize = new Size(1, 1);
            panelMaHoaDon.Name = "panelMaHoaDon";
            panelMaHoaDon.Radius = 20;
            panelMaHoaDon.Size = new Size(107, 28);
            panelMaHoaDon.TabIndex = 3;
            panelMaHoaDon.Text = "HD1";
            panelMaHoaDon.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(64, 64, 64);
            label4.Location = new Point(12, 84);
            label4.Name = "label4";
            label4.Size = new Size(68, 20);
            label4.TabIndex = 4;
            label4.Text = "VAT (8%)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 116);
            label5.Name = "label5";
            label5.Size = new Size(80, 20);
            label5.TabIndex = 5;
            label5.Text = "Tổng cộng";
            // 
            // lbThoiGianLap
            // 
            lbThoiGianLap.AutoSize = true;
            lbThoiGianLap.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbThoiGianLap.ForeColor = Color.FromArgb(64, 64, 64);
            lbThoiGianLap.Location = new Point(12, 160);
            lbThoiGianLap.Name = "lbThoiGianLap";
            lbThoiGianLap.Size = new Size(94, 20);
            lbThoiGianLap.TabIndex = 6;
            lbThoiGianLap.Text = "Bắt đầu: 8:30";
            // 
            // lbTamTinh
            // 
            lbTamTinh.AutoSize = false;
            lbTamTinh.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbTamTinh.ForeColor = Color.FromArgb(64, 64, 64);
            lbTamTinh.Location = new Point(320, 64);
            lbTamTinh.Name = "lbTamTinh";
            lbTamTinh.Size = new Size(150, 20);
            lbTamTinh.TabIndex = 7;
            lbTamTinh.Text = "850.000 đ";
            lbTamTinh.TextAlign = ContentAlignment.MiddleRight;
            lbTamTinh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            // 
            // lbVAT
            // 
            lbVAT.AutoSize = false;
            lbVAT.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbVAT.ForeColor = Color.FromArgb(64, 64, 64);
            lbVAT.Location = new Point(320, 84);
            lbVAT.Name = "lbVAT";
            lbVAT.Size = new Size(150, 20);
            lbVAT.TabIndex = 8;
            lbVAT.Text = "85.000 đ";
            lbVAT.TextAlign = ContentAlignment.MiddleRight;
            lbVAT.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            // 
            // lbTongCong
            // 
            lbTongCong.AutoSize = false;
            lbTongCong.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbTongCong.Location = new Point(320, 116);
            lbTongCong.Name = "lbTongCong";
            lbTongCong.Size = new Size(150, 20);
            lbTongCong.TabIndex = 9;
            lbTongCong.Text = "935.000 đ";
            lbTongCong.TextAlign = ContentAlignment.MiddleRight;
            lbTongCong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(6, 139);
            label10.Name = "label10";
            label10.Size = new Size(474, 20);
            label10.TabIndex = 10;
            label10.Text = "———————————————————————————————";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(6, 98);
            label11.Name = "label11";
            label11.Size = new Size(474, 20);
            label11.TabIndex = 11;
            label11.Text = "———————————————————————————————";
            // 
            // HoaDonPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(482, 189);
            Controls.Add(lbTongCong);
            Controls.Add(lbVAT);
            Controls.Add(lbTamTinh);
            Controls.Add(lbThoiGianLap);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(panelMaHoaDon);
            Controls.Add(label3);
            Controls.Add(lbSoKhach_SoMon);
            Controls.Add(lbSoBan);
            Controls.Add(label11);
            Controls.Add(label10);
            FormBorderStyle = FormBorderStyle.None;
            Name = "HoaDonPanel";
            Text = "HoaDonPanel";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbSoBan;
        private Label lbSoKhach_SoMon;
        private Label label3;
        private Sunny.UI.UIPanel panelMaHoaDon;
        private Label label4;
        private Label label5;
        private Label lbThoiGianLap;
        private Label lbTamTinh;
        private Label lbVAT;
        private Label lbTongCong;
        private Label label10;
        private Label label11;
    }
}