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
            label3 = new Label();
            panel1 = new Panel();
            panelNhomMon = new Panel();
            roundedButton1 = new UI.Controls.RoundedButton();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            panel3 = new Panel();
            btnChonBan = new UI.Controls.RoundedButton();
            label12 = new Label();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            roundedButton10 = new UI.Controls.RoundedButton();
            roundedButton9 = new UI.Controls.RoundedButton();
            panel5 = new Panel();
            panel6 = new Panel();
            orderItemCard2 = new UI.Controls.OrderItemCard();
            orderItemCard1 = new UI.Controls.OrderItemCard();
            pictureBox1 = new PictureBox();
            label5 = new Label();
            label4 = new Label();
            panelDanhSachMon = new Panel();
            roundedButton8 = new UI.Controls.RoundedButton();
            panel1.SuspendLayout();
            panelNhomMon.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(11, 44);
            label2.Name = "label2";
            label2.Size = new Size(205, 24);
            label2.TabIndex = 9;
            label2.Text = "Gọi món và thanh toán";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(11, 9);
            label1.Name = "label1";
            label1.Size = new Size(293, 35);
            label1.TabIndex = 8;
            label1.Text = "Bán hàng (Point of Sale)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(1056, 877);
            label3.Name = "label3";
            label3.Size = new Size(138, 24);
            label3.TabIndex = 10;
            label3.Text = "V0.2 - Nhóm 11";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(panelNhomMon);
            panel1.Controls.Add(roundedTextBox1);
            panel1.Location = new Point(-2, 91);
            panel1.Name = "panel1";
            panel1.Size = new Size(762, 141);
            panel1.TabIndex = 11;
            // 
            // panelNhomMon
            // 
            panelNhomMon.AutoScroll = true;
            panelNhomMon.Controls.Add(roundedButton1);
            panelNhomMon.Location = new Point(3, 67);
            panelNhomMon.Name = "panelNhomMon";
            panelNhomMon.Size = new Size(759, 73);
            panelNhomMon.TabIndex = 1;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.Black;
            roundedButton1.BorderColor = Color.Black;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.HoverBackColor = Color.FromArgb(64, 64, 64);
            roundedButton1.Location = new Point(7, 5);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 5, 10, 5);
            roundedButton1.PressedBackColor = Color.Gray;
            roundedButton1.Size = new Size(125, 45);
            roundedButton1.TabIndex = 26;
            roundedButton1.Text = "Tất cả";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(14, 20);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(704, 40);
            roundedTextBox1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(btnChonBan);
            panel3.Controls.Add(label12);
            panel3.Controls.Add(label11);
            panel3.Controls.Add(label10);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(roundedButton10);
            panel3.Controls.Add(roundedButton9);
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(pictureBox1);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label4);
            panel3.Location = new Point(759, 91);
            panel3.Name = "panel3";
            panel3.Size = new Size(434, 783);
            panel3.TabIndex = 12;
            // 
            // btnChonBan
            // 
            btnChonBan.BackColor = Color.White;
            btnChonBan.FlatStyle = FlatStyle.Flat;
            btnChonBan.Font = new Font("Segoe UI Semibold", 10.5F);
            btnChonBan.ForeColor = Color.Black;
            btnChonBan.Location = new Point(33, 52);
            btnChonBan.Name = "btnChonBan";
            btnChonBan.Padding = new Padding(10, 5, 10, 5);
            btnChonBan.Size = new Size(358, 51);
            btnChonBan.TabIndex = 8;
            btnChonBan.Text = "Chọn bàn để bắt đầu";
            btnChonBan.UseVisualStyleBackColor = false;
            btnChonBan.Click += btnChonBan_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.Location = new Point(328, 663);
            label12.Name = "label12";
            label12.Size = new Size(91, 24);
            label12.TabIndex = 25;
            label12.Text = "310.000 đ";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(338, 617);
            label11.Name = "label11";
            label11.Size = new Size(81, 24);
            label11.TabIndex = 24;
            label11.Text = "22.000 đ";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(328, 583);
            label10.Name = "label10";
            label10.Size = new Size(91, 24);
            label10.TabIndex = 23;
            label10.Text = "270.000 đ";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Calibri", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.Black;
            label9.Location = new Point(14, 645);
            label9.Name = "label9";
            label9.Size = new Size(414, 18);
            label9.TabIndex = 22;
            label9.Text = "⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯⎯";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(14, 617);
            label8.Name = "label8";
            label8.Size = new Size(81, 24);
            label8.TabIndex = 21;
            label8.Text = "VAT (8%)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(14, 583);
            label7.Name = "label7";
            label7.Size = new Size(82, 24);
            label7.TabIndex = 20;
            label7.Text = "Tạm tính";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(14, 663);
            label6.Name = "label6";
            label6.Size = new Size(93, 24);
            label6.TabIndex = 19;
            label6.Text = "Tổng cộng";
            // 
            // roundedButton10
            // 
            roundedButton10.BackColor = Color.Black;
            roundedButton10.FlatStyle = FlatStyle.Flat;
            roundedButton10.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton10.ForeColor = Color.White;
            roundedButton10.Location = new Point(14, 693);
            roundedButton10.Name = "roundedButton10";
            roundedButton10.Padding = new Padding(10, 5, 10, 5);
            roundedButton10.Size = new Size(405, 40);
            roundedButton10.TabIndex = 18;
            roundedButton10.Text = "Gửi xuống bếp";
            roundedButton10.UseVisualStyleBackColor = false;
            // 
            // roundedButton9
            // 
            roundedButton9.BackColor = Color.White;
            roundedButton9.FlatStyle = FlatStyle.Flat;
            roundedButton9.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton9.ForeColor = Color.Black;
            roundedButton9.Location = new Point(14, 740);
            roundedButton9.Name = "roundedButton9";
            roundedButton9.Padding = new Padding(10, 5, 10, 5);
            roundedButton9.Size = new Size(404, 40);
            roundedButton9.TabIndex = 8;
            roundedButton9.Text = "Thanh toán";
            roundedButton9.UseVisualStyleBackColor = false;
            // 
            // panel5
            // 
            panel5.AutoScroll = true;
            panel5.Controls.Add(panel6);
            panel5.Controls.Add(orderItemCard1);
            panel5.Location = new Point(0, 167);
            panel5.Name = "panel5";
            panel5.Size = new Size(459, 413);
            panel5.TabIndex = 17;
            // 
            // panel6
            // 
            panel6.AutoScroll = true;
            panel6.Controls.Add(orderItemCard2);
            panel6.Location = new Point(3, -1);
            panel6.Name = "panel6";
            panel6.Size = new Size(428, 403);
            panel6.TabIndex = 18;
            // 
            // orderItemCard2
            // 
            orderItemCard2.Font = new Font("Segoe UI", 10F);
            orderItemCard2.Location = new Point(3, -1);
            orderItemCard2.Margin = new Padding(3, 4, 3, 4);
            orderItemCard2.Name = "orderItemCard2";
            orderItemCard2.Size = new Size(413, 225);
            orderItemCard2.TabIndex = 0;
            orderItemCard2.Text = "orderItemCard2";
            // 
            // orderItemCard1
            // 
            orderItemCard1.Font = new Font("Segoe UI", 10F);
            orderItemCard1.Location = new Point(3, 4);
            orderItemCard1.Margin = new Padding(3, 4, 3, 4);
            orderItemCard1.Name = "orderItemCard1";
            orderItemCard1.Size = new Size(415, 160);
            orderItemCard1.TabIndex = 0;
            orderItemCard1.Text = "orderItemCard1";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(14, 133);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(26, 28);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 16;
            pictureBox1.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(37, 133);
            label5.Name = "label5";
            label5.Size = new Size(133, 28);
            label5.TabIndex = 15;
            label5.Text = "Đơn hàng (2)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(7, 9);
            label4.Name = "label4";
            label4.Size = new Size(139, 24);
            label4.TabIndex = 15;
            label4.Text = "Chọn bàn/sảnh";
            // 
            // panelDanhSachMon
            // 
            panelDanhSachMon.AutoScroll = true;
            panelDanhSachMon.BackColor = SystemColors.Control;
            panelDanhSachMon.Location = new Point(1, 239);
            panelDanhSachMon.Name = "panelDanhSachMon";
            panelDanhSachMon.Size = new Size(755, 635);
            panelDanhSachMon.TabIndex = 13;
            // 
            // roundedButton8
            // 
            roundedButton8.BackColor = Color.FromArgb(31, 111, 235);
            roundedButton8.CornerRadius = 20;
            roundedButton8.FlatStyle = FlatStyle.Flat;
            roundedButton8.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            roundedButton8.ForeColor = Color.White;
            roundedButton8.Location = new Point(855, 19);
            roundedButton8.Name = "roundedButton8";
            roundedButton8.Padding = new Padding(10, 5, 10, 5);
            roundedButton8.Size = new Size(295, 56);
            roundedButton8.TabIndex = 14;
            roundedButton8.Text = "Số bàn đang phục vụ: 3";
            roundedButton8.UseVisualStyleBackColor = false;
            // 
            // FrmBanHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 900);
            ControlBox = false;
            Controls.Add(roundedButton8);
            Controls.Add(panelDanhSachMon);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmBanHang";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmBanHang";
            Load += FrmBanHang_Load;
            panel1.ResumeLayout(false);
            panelNhomMon.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel5.ResumeLayout(false);
            panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Label label3;
        private Panel panel1;
        private Controls.RoundedTextBox roundedTextBox1;
        private Panel panelNhomMon;
        private Panel panel3;
        private Panel panelDanhSachMon;
        private Controls.RoundedButton roundedButton8;
        private Label label4;
        private Controls.RoundedButton roundedButton10;
        private Controls.RoundedButton roundedButton9;
        private Panel panel5;
        private PictureBox pictureBox1;
        private Label label5;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label12;
        private Label label11;
        private Label label10;
        private Label label9;
        private Controls.RoundedButton btnChonBan;
        private Panel panel6;
        private Controls.OrderItemCard orderItemCard2;
        private Controls.OrderItemCard orderItemCard1;
        private Controls.RoundedButton roundedButton1;
    }
}