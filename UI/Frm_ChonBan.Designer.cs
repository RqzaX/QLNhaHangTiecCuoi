namespace UI
{
    partial class Frm_ChonBan
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
            VanThuan.UI.PillItem pillItem1 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem2 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem3 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem4 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem5 = new VanThuan.UI.PillItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_ChonBan));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            panelDanhSachBan = new Panel();
            roundedPanel4 = new UI.Controls.RoundedPanel();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            roundedPanel5 = new UI.Controls.RoundedPanel();
            roundedPanel6 = new UI.Controls.RoundedPanel();
            btnThoat = new UI.Controls.RoundedButton();
            roundedPanel7 = new UI.Controls.RoundedPanel();
            pictureBox1 = new PictureBox();
            lbTongSoBan = new Label();
            label6 = new Label();
            roundedPanel8 = new UI.Controls.RoundedPanel();
            pictureBox2 = new PictureBox();
            lbBanTrong = new Label();
            label9 = new Label();
            roundedPanel9 = new UI.Controls.RoundedPanel();
            pictureBox3 = new PictureBox();
            lbDangPhucVu = new Label();
            label11 = new Label();
            roundedPanel10 = new UI.Controls.RoundedPanel();
            pictureBox4 = new PictureBox();
            lbDaDatTruoc = new Label();
            label13 = new Label();
            roundedPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            roundedPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            roundedPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            roundedPanel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Variable Display", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(200, 26);
            label1.TabIndex = 0;
            label1.Text = "Sơ đồ bàn - Chọn bàn";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 35);
            label2.Name = "label2";
            label2.Size = new Size(313, 20);
            label2.TabIndex = 1;
            label2.Text = "Chọn bàn để bắt đầu order hoặc xem chi tiết";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(55, 173);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 12;
            label3.Text = "Trống";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(140, 173);
            label4.Name = "label4";
            label4.Size = new Size(86, 20);
            label4.TabIndex = 13;
            label4.Text = "Đang dùng";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(262, 173);
            label5.Name = "label5";
            label5.Size = new Size(54, 20);
            label5.TabIndex = 14;
            label5.Text = "Đã đặt";
            // 
            // panelDanhSachBan
            // 
            panelDanhSachBan.AutoScroll = true;
            panelDanhSachBan.BackColor = Color.Transparent;
            panelDanhSachBan.ForeColor = Color.Transparent;
            panelDanhSachBan.Location = new Point(2, 196);
            panelDanhSachBan.Margin = new Padding(3, 2, 3, 2);
            panelDanhSachBan.Name = "panelDanhSachBan";
            panelDanhSachBan.Size = new Size(808, 309);
            panelDanhSachBan.TabIndex = 15;
            panelDanhSachBan.Paint += panelDanhSachBan_Paint;
            // 
            // roundedPanel4
            // 
            roundedPanel4.BackColor = Color.LimeGreen;
            roundedPanel4.BorderColor = Color.Black;
            roundedPanel4.BorderThickness = 2;
            roundedPanel4.ForeColor = Color.White;
            roundedPanel4.Location = new Point(31, 168);
            roundedPanel4.Margin = new Padding(3, 2, 3, 2);
            roundedPanel4.Name = "roundedPanel4";
            roundedPanel4.Padding = new Padding(10, 9, 10, 9);
            roundedPanel4.Size = new Size(25, 25);
            roundedPanel4.TabIndex = 16;
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Tất cả";
            pillItem2.Text = "Khu A - Tầng 1";
            pillItem3.Text = "Khu B - Tầng 1";
            pillItem4.Text = "Khu C- Tầng 2";
            pillItem5.Text = "Khu VIP - Tầng 2";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Items.Add(pillItem3);
            segmentedPill1.Items.Add(pillItem4);
            segmentedPill1.Items.Add(pillItem5);
            segmentedPill1.Location = new Point(12, 128);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.RightToLeft = RightToLeft.Yes;
            segmentedPill1.Size = new Size(712, 39);
            segmentedPill1.TabIndex = 18;
            segmentedPill1.TabStop = false;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // roundedPanel5
            // 
            roundedPanel5.BackColor = Color.Red;
            roundedPanel5.BorderColor = Color.Black;
            roundedPanel5.BorderThickness = 2;
            roundedPanel5.ForeColor = Color.Transparent;
            roundedPanel5.Location = new Point(114, 169);
            roundedPanel5.Margin = new Padding(3, 2, 3, 2);
            roundedPanel5.Name = "roundedPanel5";
            roundedPanel5.Padding = new Padding(10, 9, 10, 9);
            roundedPanel5.Size = new Size(25, 25);
            roundedPanel5.TabIndex = 17;
            // 
            // roundedPanel6
            // 
            roundedPanel6.BackColor = Color.Gold;
            roundedPanel6.BorderColor = Color.Black;
            roundedPanel6.BorderThickness = 2;
            roundedPanel6.ForeColor = Color.Transparent;
            roundedPanel6.Location = new Point(236, 169);
            roundedPanel6.Margin = new Padding(3, 2, 3, 2);
            roundedPanel6.Name = "roundedPanel6";
            roundedPanel6.Padding = new Padding(10, 9, 10, 9);
            roundedPanel6.Size = new Size(25, 25);
            roundedPanel6.TabIndex = 18;
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.FromArgb(239, 68, 68);
            btnThoat.CornerRadius = 16;
            btnThoat.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = Color.White;
            btnThoat.HoverBackColor = Color.FromArgb(220, 38, 38);
            btnThoat.Location = new Point(761, 11);
            btnThoat.Margin = new Padding(3, 2, 3, 2);
            btnThoat.Name = "btnThoat";
            btnThoat.PressedBackColor = Color.FromArgb(185, 28, 28);
            btnThoat.Size = new Size(48, 32);
            btnThoat.TabIndex = 19;
            btnThoat.Text = "✖";
            btnThoat.Click += btnThoat_Click_1;
            // 
            // roundedPanel7
            // 
            roundedPanel7.BackColor = Color.FromArgb(224, 237, 254);
            roundedPanel7.BorderColor = Color.SkyBlue;
            roundedPanel7.BorderThickness = 2;
            roundedPanel7.Controls.Add(pictureBox1);
            roundedPanel7.Controls.Add(lbTongSoBan);
            roundedPanel7.Controls.Add(label6);
            roundedPanel7.Location = new Point(12, 57);
            roundedPanel7.Margin = new Padding(3, 2, 3, 2);
            roundedPanel7.Name = "roundedPanel7";
            roundedPanel7.Padding = new Padding(10, 9, 10, 9);
            roundedPanel7.Size = new Size(167, 68);
            roundedPanel7.TabIndex = 20;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(6, 10);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(62, 46);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 24;
            pictureBox1.TabStop = false;
            // 
            // lbTongSoBan
            // 
            lbTongSoBan.AutoSize = true;
            lbTongSoBan.Font = new Font("Segoe UI Variable Display Semib", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTongSoBan.Location = new Point(78, 37);
            lbTongSoBan.Name = "lbTongSoBan";
            lbTongSoBan.Size = new Size(28, 21);
            lbTongSoBan.TabIndex = 23;
            lbTongSoBan.Text = "34";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(72, 19);
            label6.Name = "label6";
            label6.Size = new Size(94, 20);
            label6.TabIndex = 22;
            label6.Text = "Tổng số bàn";
            // 
            // roundedPanel8
            // 
            roundedPanel8.BackColor = Color.Honeydew;
            roundedPanel8.BorderColor = Color.LimeGreen;
            roundedPanel8.BorderThickness = 2;
            roundedPanel8.Controls.Add(pictureBox2);
            roundedPanel8.Controls.Add(lbBanTrong);
            roundedPanel8.Controls.Add(label9);
            roundedPanel8.Location = new Point(191, 57);
            roundedPanel8.Margin = new Padding(3, 2, 3, 2);
            roundedPanel8.Name = "roundedPanel8";
            roundedPanel8.Padding = new Padding(10, 9, 10, 9);
            roundedPanel8.Size = new Size(167, 68);
            roundedPanel8.TabIndex = 21;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(10, 10);
            pictureBox2.Margin = new Padding(3, 2, 3, 2);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(62, 46);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 25;
            pictureBox2.TabStop = false;
            // 
            // lbBanTrong
            // 
            lbBanTrong.AutoSize = true;
            lbBanTrong.Font = new Font("Segoe UI Variable Display Semib", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbBanTrong.Location = new Point(78, 37);
            lbBanTrong.Name = "lbBanTrong";
            lbBanTrong.Size = new Size(28, 21);
            lbBanTrong.TabIndex = 25;
            lbBanTrong.Text = "24";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(79, 19);
            label9.Name = "label9";
            label9.Size = new Size(77, 20);
            label9.TabIndex = 24;
            label9.Text = "Bàn trống";
            // 
            // roundedPanel9
            // 
            roundedPanel9.BackColor = Color.MistyRose;
            roundedPanel9.BorderColor = Color.Red;
            roundedPanel9.BorderThickness = 2;
            roundedPanel9.Controls.Add(pictureBox3);
            roundedPanel9.Controls.Add(lbDangPhucVu);
            roundedPanel9.Controls.Add(label11);
            roundedPanel9.Location = new Point(370, 57);
            roundedPanel9.Margin = new Padding(3, 2, 3, 2);
            roundedPanel9.Name = "roundedPanel9";
            roundedPanel9.Padding = new Padding(10, 9, 10, 9);
            roundedPanel9.Size = new Size(175, 68);
            roundedPanel9.TabIndex = 21;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(7, 10);
            pictureBox3.Margin = new Padding(3, 2, 3, 2);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(62, 46);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 26;
            pictureBox3.TabStop = false;
            // 
            // lbDangPhucVu
            // 
            lbDangPhucVu.AutoSize = true;
            lbDangPhucVu.Font = new Font("Segoe UI Variable Display Semib", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbDangPhucVu.Location = new Point(72, 37);
            lbDangPhucVu.Name = "lbDangPhucVu";
            lbDangPhucVu.Size = new Size(25, 21);
            lbDangPhucVu.TabIndex = 27;
            lbDangPhucVu.Text = "12";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(68, 19);
            label11.Name = "label11";
            label11.Size = new Size(105, 20);
            label11.TabIndex = 26;
            label11.Text = "Đang phục vụ";
            // 
            // roundedPanel10
            // 
            roundedPanel10.BackColor = Color.Cornsilk;
            roundedPanel10.BorderColor = Color.Gold;
            roundedPanel10.BorderThickness = 2;
            roundedPanel10.Controls.Add(pictureBox4);
            roundedPanel10.Controls.Add(lbDaDatTruoc);
            roundedPanel10.Controls.Add(label13);
            roundedPanel10.Location = new Point(554, 57);
            roundedPanel10.Margin = new Padding(3, 2, 3, 2);
            roundedPanel10.Name = "roundedPanel10";
            roundedPanel10.Padding = new Padding(10, 9, 10, 9);
            roundedPanel10.Size = new Size(173, 68);
            roundedPanel10.TabIndex = 21;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.Transparent;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(9, 10);
            pictureBox4.Margin = new Padding(3, 2, 3, 2);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(62, 46);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 28;
            pictureBox4.TabStop = false;
            // 
            // lbDaDatTruoc
            // 
            lbDaDatTruoc.AutoSize = true;
            lbDaDatTruoc.Font = new Font("Segoe UI Variable Display Semib", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbDaDatTruoc.Location = new Point(78, 37);
            lbDaDatTruoc.Name = "lbDaDatTruoc";
            lbDaDatTruoc.Size = new Size(19, 21);
            lbDaDatTruoc.TabIndex = 29;
            lbDaDatTruoc.Text = "3";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label13.Location = new Point(76, 19);
            label13.Name = "label13";
            label13.Size = new Size(94, 20);
            label13.TabIndex = 28;
            label13.Text = "Đã đặt trước";
            // 
            // Frm_ChonBan
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.White;
            ClientSize = new Size(821, 516);
            Controls.Add(roundedPanel10);
            Controls.Add(roundedPanel9);
            Controls.Add(roundedPanel8);
            Controls.Add(roundedPanel7);
            Controls.Add(btnThoat);
            Controls.Add(roundedPanel6);
            Controls.Add(roundedPanel5);
            Controls.Add(segmentedPill1);
            Controls.Add(roundedPanel4);
            Controls.Add(panelDanhSachBan);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Frm_ChonBan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Frm_ChonBan";
            Load += Frm_ChonBan_Load;
            roundedPanel7.ResumeLayout(false);
            roundedPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            roundedPanel8.ResumeLayout(false);
            roundedPanel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            roundedPanel9.ResumeLayout(false);
            roundedPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            roundedPanel10.ResumeLayout(false);
            roundedPanel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Controls.RoundedPanel roundedPanel1;
        private Controls.RoundedPanel roundedPanel2;
        private Controls.RoundedPanel roundedPanel3;
        private Label label3;
        private Label label4;
        private Label label5;
        private Panel panelDanhSachBan;
        private Controls.RoundedPanel roundedPanel4;
        private Controls.RoundedPanel roundedPanel5;
        private Controls.RoundedPanel roundedPanel6;
        private UI.Controls.RoundedButton btnThoat;
        private Controls.RoundedPanel roundedPanel7;
        private Controls.RoundedPanel roundedPanel8;
        private Controls.RoundedPanel roundedPanel9;
        private Controls.RoundedPanel roundedPanel10;
        private Label lbTongSoBan;
        private Label label6;
        private Label lbBanTrong;
        private Label label9;
        private Label lbDangPhucVu;
        private Label label11;
        private Label lbDaDatTruoc;
        private Label label13;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
    }
}