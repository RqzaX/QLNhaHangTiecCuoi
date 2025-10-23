namespace UI
{
    partial class FrmBanHang
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBanHang));
            label2 = new Label();
            label1 = new Label();
            panel1 = new Panel();
            panelNhomMon = new Panel();
            roundedButton1 = new UI.Controls.RoundedButton();
            txtTimMon = new UI.Controls.RoundedTextBox();
            panel3 = new Panel();
            panelGioHang = new Panel();
            btnChonBan = new UI.Controls.RoundedButton();
            lbTongCong = new Label();
            lbVAT = new Label();
            lbTamTinh = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            roundedButton10 = new UI.Controls.RoundedButton();
            roundedButton9 = new UI.Controls.RoundedButton();
            pictureBox1 = new PictureBox();
            labelDonHang = new Label();
            label4 = new Label();
            panelDanhSachMon = new Panel();
            roundedButton8 = new UI.Controls.RoundedButton();
            btnXoaTatCaMon = new UI.Controls.RoundedButton();
            panel1.SuspendLayout();
            panelNhomMon.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(10, 33);
            label2.Name = "label2";
            label2.Size = new Size(156, 19);
            label2.TabIndex = 9;
            label2.Text = "Gọi món và thanh toán";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(10, 7);
            label1.Name = "label1";
            label1.Size = new Size(231, 27);
            label1.TabIndex = 8;
            label1.Text = "Bán hàng (Point of Sale)";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(panelNhomMon);
            panel1.Controls.Add(txtTimMon);
            panel1.Location = new Point(-2, 68);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(667, 106);
            panel1.TabIndex = 11;
            // 
            // panelNhomMon
            // 
            panelNhomMon.AutoScroll = true;
            panelNhomMon.Controls.Add(roundedButton1);
            panelNhomMon.Location = new Point(3, 32);
            panelNhomMon.Margin = new Padding(3, 2, 3, 2);
            panelNhomMon.Name = "panelNhomMon";
            panelNhomMon.Size = new Size(664, 73);
            panelNhomMon.TabIndex = 1;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.Black;
            roundedButton1.BorderColor = Color.Black;
            roundedButton1.BorderThickness = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.HoverBackColor = Color.FromArgb(64, 64, 64);
            roundedButton1.Location = new Point(6, 4);
            roundedButton1.Margin = new Padding(3, 2, 3, 2);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(9, 4, 9, 4);
            roundedButton1.PressedBackColor = Color.Gray;
            roundedButton1.Size = new Size(109, 34);
            roundedButton1.TabIndex = 26;
            roundedButton1.Text = "Tất cả";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // txtTimMon
            // 
            txtTimMon.BackColor = Color.White;
            txtTimMon.Font = new Font("Segoe UI", 10F);
            txtTimMon.ForeColor = Color.Black;
            txtTimMon.Location = new Point(12, 2);
            txtTimMon.Margin = new Padding(3, 2, 3, 2);
            txtTimMon.Name = "txtTimMon";
            txtTimMon.Padding = new Padding(9, 6, 9, 6);
            txtTimMon.Size = new Size(616, 30);
            txtTimMon.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(btnXoaTatCaMon);
            panel3.Controls.Add(panelGioHang);
            panel3.Controls.Add(btnChonBan);
            panel3.Controls.Add(lbTongCong);
            panel3.Controls.Add(lbVAT);
            panel3.Controls.Add(lbTamTinh);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(roundedButton10);
            panel3.Controls.Add(roundedButton9);
            panel3.Controls.Add(pictureBox1);
            panel3.Controls.Add(labelDonHang);
            panel3.Controls.Add(label4);
            panel3.Location = new Point(664, 68);
            panel3.Margin = new Padding(3, 2, 3, 2);
            panel3.Name = "panel3";
            panel3.Size = new Size(380, 587);
            panel3.TabIndex = 12;
            // 
            // panelGioHang
            // 
            panelGioHang.AutoScroll = true;
            panelGioHang.Location = new Point(0, 127);
            panelGioHang.Margin = new Padding(0);
            panelGioHang.Name = "panelGioHang";
            panelGioHang.Size = new Size(365, 308);
            panelGioHang.TabIndex = 18;
            // 
            // btnChonBan
            // 
            btnChonBan.BackColor = Color.DeepSkyBlue;
            btnChonBan.BorderThickness = 0;
            btnChonBan.FlatStyle = FlatStyle.Flat;
            btnChonBan.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChonBan.ForeColor = Color.Black;
            btnChonBan.Location = new Point(29, 39);
            btnChonBan.Margin = new Padding(3, 2, 3, 2);
            btnChonBan.Name = "btnChonBan";
            btnChonBan.Padding = new Padding(9, 4, 9, 4);
            btnChonBan.Size = new Size(313, 38);
            btnChonBan.TabIndex = 8;
            btnChonBan.Text = "Chọn bàn để bắt đầu";
            btnChonBan.UseVisualStyleBackColor = false;
            btnChonBan.Click += btnChonBan_Click;
            // 
            // lbTongCong
            // 
            lbTongCong.AutoSize = true;
            lbTongCong.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbTongCong.Location = new Point(260, 498);
            lbTongCong.Name = "lbTongCong";
            lbTongCong.Size = new Size(30, 19);
            lbTongCong.TabIndex = 25;
            lbTongCong.Text = "0 đ";
            lbTongCong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbVAT
            // 
            lbVAT.AutoSize = true;
            lbVAT.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbVAT.Location = new Point(260, 463);
            lbVAT.Name = "lbVAT";
            lbVAT.Size = new Size(30, 19);
            lbVAT.TabIndex = 24;
            lbVAT.Text = "0 đ";
            lbVAT.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbTamTinh
            // 
            lbTamTinh.AutoSize = true;
            lbTamTinh.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbTamTinh.Location = new Point(260, 437);
            lbTamTinh.Name = "lbTamTinh";
            lbTamTinh.Size = new Size(30, 19);
            lbTamTinh.TabIndex = 23;
            lbTamTinh.Text = "0 đ";
            lbTamTinh.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.Black;
            label9.Location = new Point(12, 484);
            label9.Name = "label9";
            label9.Size = new Size(355, 14);
            label9.TabIndex = 22;
            label9.Text = "⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(12, 463);
            label8.Name = "label8";
            label8.Size = new Size(66, 19);
            label8.TabIndex = 21;
            label8.Text = "VAT (8%)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(12, 437);
            label7.Name = "label7";
            label7.Size = new Size(65, 19);
            label7.TabIndex = 20;
            label7.Text = "Tạm tính";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 497);
            label6.Name = "label6";
            label6.Size = new Size(75, 19);
            label6.TabIndex = 19;
            label6.Text = "Tổng cộng";
            // 
            // roundedButton10
            // 
            roundedButton10.BackColor = Color.Black;
            roundedButton10.BorderThickness = 0;
            roundedButton10.FlatStyle = FlatStyle.Flat;
            roundedButton10.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton10.ForeColor = Color.White;
            roundedButton10.Location = new Point(12, 520);
            roundedButton10.Margin = new Padding(3, 2, 3, 2);
            roundedButton10.Name = "roundedButton10";
            roundedButton10.Padding = new Padding(9, 4, 9, 4);
            roundedButton10.Size = new Size(342, 30);
            roundedButton10.TabIndex = 18;
            roundedButton10.Text = "Gửi xuống bếp";
            roundedButton10.UseVisualStyleBackColor = false;
            // 
            // roundedButton9
            // 
            roundedButton9.BackColor = Color.White;
            roundedButton9.BorderThickness = 0;
            roundedButton9.FlatStyle = FlatStyle.Flat;
            roundedButton9.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton9.ForeColor = Color.Black;
            roundedButton9.Location = new Point(12, 555);
            roundedButton9.Margin = new Padding(3, 2, 3, 2);
            roundedButton9.Name = "roundedButton9";
            roundedButton9.Padding = new Padding(9, 4, 9, 4);
            roundedButton9.Size = new Size(342, 30);
            roundedButton9.TabIndex = 8;
            roundedButton9.Text = "Thanh toán";
            roundedButton9.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 100);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(23, 21);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 16;
            pictureBox1.TabStop = false;
            // 
            // labelDonHang
            // 
            labelDonHang.AutoSize = true;
            labelDonHang.Font = new Font("Calibri", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDonHang.Location = new Point(32, 100);
            labelDonHang.Name = "labelDonHang";
            labelDonHang.Size = new Size(102, 22);
            labelDonHang.TabIndex = 15;
            labelDonHang.Text = "Đơn hàng (0)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(6, 7);
            label4.Name = "label4";
            label4.Size = new Size(107, 19);
            label4.TabIndex = 15;
            label4.Text = "Chọn bàn/sảnh";
            // 
            // panelDanhSachMon
            // 
            panelDanhSachMon.AutoScroll = true;
            panelDanhSachMon.BackColor = SystemColors.Control;
            panelDanhSachMon.Location = new Point(1, 179);
            panelDanhSachMon.Margin = new Padding(3, 2, 3, 2);
            panelDanhSachMon.Name = "panelDanhSachMon";
            panelDanhSachMon.Size = new Size(661, 476);
            panelDanhSachMon.TabIndex = 13;
            // 
            // roundedButton8
            // 
            roundedButton8.BackColor = Color.FromArgb(31, 111, 235);
            roundedButton8.BorderThickness = 0;
            roundedButton8.CornerRadius = 20;
            roundedButton8.FlatStyle = FlatStyle.Flat;
            roundedButton8.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            roundedButton8.ForeColor = Color.White;
            roundedButton8.Location = new Point(748, 14);
            roundedButton8.Margin = new Padding(3, 2, 3, 2);
            roundedButton8.Name = "roundedButton8";
            roundedButton8.Padding = new Padding(9, 4, 9, 4);
            roundedButton8.Size = new Size(258, 42);
            roundedButton8.TabIndex = 14;
            roundedButton8.Text = "Số bàn đang phục vụ: 3";
            roundedButton8.UseVisualStyleBackColor = false;
            // 
            // btnXoaTatCaMon
            // 
            btnXoaTatCaMon.BackColor = Color.IndianRed;
            btnXoaTatCaMon.BorderThickness = 0;
            btnXoaTatCaMon.FlatStyle = FlatStyle.Flat;
            btnXoaTatCaMon.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXoaTatCaMon.ForeColor = Color.White;
            btnXoaTatCaMon.Location = new Point(231, 100);
            btnXoaTatCaMon.Margin = new Padding(3, 2, 3, 2);
            btnXoaTatCaMon.Name = "btnXoaTatCaMon";
            btnXoaTatCaMon.Padding = new Padding(9, 4, 9, 4);
            btnXoaTatCaMon.Size = new Size(111, 22);
            btnXoaTatCaMon.TabIndex = 26;
            btnXoaTatCaMon.Text = "Xóa tất cả món";
            btnXoaTatCaMon.UseVisualStyleBackColor = false;
            // 
            // FrmBanHang
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1041, 675);
            ControlBox = false;
            Controls.Add(roundedButton8);
            Controls.Add(panelDanhSachMon);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmBanHang";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmBanHang";
            Load += FrmBanHang_Load;
            panel1.ResumeLayout(false);
            panelNhomMon.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Panel panel1;
        private Controls.RoundedTextBox txtTimMon;
        private Panel panelNhomMon;
        private Panel panel3;
        private Panel panelDanhSachMon;
        private Controls.RoundedButton roundedButton8;
        private Label label4;
        private Controls.RoundedButton roundedButton10;
        private Controls.RoundedButton roundedButton9;
        private PictureBox pictureBox1;
        private Label labelDonHang;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label lbTongCong;
        private Label lbVAT;
        private Label lbTamTinh;
        private Label label9;
        private Controls.RoundedButton btnChonBan;
        private Controls.RoundedButton roundedButton1;
        private Panel panelGioHang;
        private Controls.RoundedButton btnXoaTatCaMon;
    }
}