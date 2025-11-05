namespace UI.Controls
{
    partial class VoucherPanel
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
            lbHanVoucher = new Label();
            lbSoLanSuDungVoucher = new Label();
            lbSoGiamGiaVoucher = new Label();
            lbTenVoucher = new Label();
            lbMaVoucher = new Label();
            panelHienThiKoApDung = new Sunny.UI.UIPanel();
            SuspendLayout();
            // 
            // lbHanVoucher
            // 
            lbHanVoucher.AccessibleDescription = "hạn sử dụng của voucher/km";
            lbHanVoucher.AutoSize = true;
            lbHanVoucher.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbHanVoucher.ForeColor = Color.FromArgb(64, 64, 64);
            lbHanVoucher.Location = new Point(334, 64);
            lbHanVoucher.Name = "lbHanVoucher";
            lbHanVoucher.Size = new Size(122, 20);
            lbHanVoucher.TabIndex = 42;
            lbHanVoucher.Text = "HSD: 30/11/2025";
            // 
            // lbSoLanSuDungVoucher
            // 
            lbSoLanSuDungVoucher.AccessibleDescription = "số lần dùng / số lượng tối đa lượt dùng";
            lbSoLanSuDungVoucher.AutoSize = true;
            lbSoLanSuDungVoucher.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbSoLanSuDungVoucher.ForeColor = Color.FromArgb(64, 64, 64);
            lbSoLanSuDungVoucher.Location = new Point(334, 40);
            lbSoLanSuDungVoucher.Name = "lbSoLanSuDungVoucher";
            lbSoLanSuDungVoucher.Size = new Size(111, 20);
            lbSoLanSuDungVoucher.TabIndex = 41;
            lbSoLanSuDungVoucher.Text = "Đã dùng: 12/50";
            // 
            // lbSoGiamGiaVoucher
            // 
            lbSoGiamGiaVoucher.AccessibleDescription = "giảm ví dụ 20% hay giảm 100.000 đ";
            lbSoGiamGiaVoucher.AutoSize = true;
            lbSoGiamGiaVoucher.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbSoGiamGiaVoucher.ForeColor = Color.FromArgb(64, 64, 64);
            lbSoGiamGiaVoucher.Location = new Point(27, 64);
            lbSoGiamGiaVoucher.Name = "lbSoGiamGiaVoucher";
            lbSoGiamGiaVoucher.Size = new Size(115, 20);
            lbSoGiamGiaVoucher.TabIndex = 40;
            lbSoGiamGiaVoucher.Text = "Giảm: 100.000 đ";
            // 
            // lbTenVoucher
            // 
            lbTenVoucher.AutoSize = true;
            lbTenVoucher.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTenVoucher.Location = new Point(26, 31);
            lbTenVoucher.Name = "lbTenVoucher";
            lbTenVoucher.Size = new Size(153, 20);
            lbTenVoucher.TabIndex = 39;
            lbTenVoucher.Text = "Voucher Khai Trương";
            // 
            // lbMaVoucher
            // 
            lbMaVoucher.AutoSize = true;
            lbMaVoucher.BackColor = Color.Transparent;
            lbMaVoucher.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbMaVoucher.ForeColor = Color.MediumSlateBlue;
            lbMaVoucher.Location = new Point(26, 11);
            lbMaVoucher.Name = "lbMaVoucher";
            lbMaVoucher.Size = new Size(100, 20);
            lbMaVoucher.TabIndex = 38;
            lbMaVoucher.Text = "{mã voucher}";
            // 
            // panelHienThiKoApDung
            // 
            panelHienThiKoApDung.AccessibleDescription = "hiển thị panel thông tin cái voucher này không áp dụng cho hóa đơn hiện tại";
            panelHienThiKoApDung.FillColor = Color.FromArgb(248, 248, 248);
            panelHienThiKoApDung.FillColor2 = Color.FromArgb(248, 248, 248);
            panelHienThiKoApDung.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelHienThiKoApDung.Location = new Point(352, 10);
            panelHienThiKoApDung.Margin = new Padding(4, 5, 4, 5);
            panelHienThiKoApDung.MinimumSize = new Size(1, 1);
            panelHienThiKoApDung.Name = "panelHienThiKoApDung";
            panelHienThiKoApDung.Radius = 21;
            panelHienThiKoApDung.RectColor = Color.FromArgb(140, 140, 140);
            panelHienThiKoApDung.Size = new Size(118, 25);
            panelHienThiKoApDung.Style = Sunny.UI.UIStyle.Custom;
            panelHienThiKoApDung.TabIndex = 43;
            panelHienThiKoApDung.Text = "Không áp dụng";
            panelHienThiKoApDung.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // VoucherPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(482, 95);
            Controls.Add(panelHienThiKoApDung);
            Controls.Add(lbHanVoucher);
            Controls.Add(lbSoLanSuDungVoucher);
            Controls.Add(lbSoGiamGiaVoucher);
            Controls.Add(lbTenVoucher);
            Controls.Add(lbMaVoucher);
            FormBorderStyle = FormBorderStyle.None;
            Name = "VoucherPanel";
            Text = "VoucherPanel";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbHanVoucher;
        private Label lbSoLanSuDungVoucher;
        private Label lbSoGiamGiaVoucher;
        private Label lbTenVoucher;
        private Label lbMaVoucher;
        private Sunny.UI.UIPanel panelHienThiKoApDung;
    }
}