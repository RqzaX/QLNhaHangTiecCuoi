namespace UI
{
    partial class FrmBaoCao
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
            components = new System.ComponentModel.Container();
            VanThuan.UI.PillItem pillItem1 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem2 = new VanThuan.UI.PillItem();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            label2 = new Label();
            roundedButton2 = new UI.Controls.RoundedButton();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            PanelBanChay = new UI.Controls.RoundedPanel();
            roundedShadowPanel5 = new UiControls.RoundedShadowPanel();
            roundedPanel12 = new UI.Controls.RoundedPanel();
            label37 = new Label();
            label36 = new Label();
            label34 = new Label();
            label35 = new Label();
            roundedShadowPanel4 = new UiControls.RoundedShadowPanel();
            roundedPanel11 = new UI.Controls.RoundedPanel();
            label33 = new Label();
            label32 = new Label();
            label30 = new Label();
            label31 = new Label();
            roundedShadowPanel3 = new UiControls.RoundedShadowPanel();
            roundedPanel10 = new UI.Controls.RoundedPanel();
            label29 = new Label();
            label28 = new Label();
            label26 = new Label();
            label27 = new Label();
            roundedShadowPanel2 = new UiControls.RoundedShadowPanel();
            roundedPanel9 = new UI.Controls.RoundedPanel();
            label25 = new Label();
            label22 = new Label();
            label24 = new Label();
            label23 = new Label();
            roundedShadowPanel1 = new UiControls.RoundedShadowPanel();
            roundedPanel8 = new UI.Controls.RoundedPanel();
            label21 = new Label();
            label19 = new Label();
            label20 = new Label();
            label18 = new Label();
            label17 = new Label();
            imageList1 = new ImageList(components);
            PanelBieuDo = new UI.Controls.RoundedPanel();
            PanelCanhBao = new Guna.UI2.WinForms.Guna2GradientPanel();
            panel1 = new Panel();
            label3 = new Label();
            borderComboBox1 = new UiControls.BorderComboBox();
            PanelBanChay.SuspendLayout();
            roundedShadowPanel5.SuspendLayout();
            roundedPanel12.SuspendLayout();
            roundedShadowPanel4.SuspendLayout();
            roundedPanel11.SuspendLayout();
            roundedShadowPanel3.SuspendLayout();
            roundedPanel10.SuspendLayout();
            roundedShadowPanel2.SuspendLayout();
            roundedPanel9.SuspendLayout();
            roundedShadowPanel1.SuspendLayout();
            roundedPanel8.SuspendLayout();
            PanelCanhBao.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(252, 35);
            label1.TabIndex = 13;
            label1.Text = "Báo cáo và Thống Kê";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(312, 23);
            label2.TabIndex = 14;
            label2.Text = "Phân tích dữ liệu và cảnh báo hệ thống";
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.BorderColor = Color.Black;
            roundedButton2.BorderThickness = 2;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.HoverBackColor = Color.LightGray;
            roundedButton2.Location = new Point(959, 35);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.PressedBackColor = Color.FromArgb(192, 255, 255);
            roundedButton2.Size = new Size(180, 40);
            roundedButton2.TabIndex = 22;
            roundedButton2.Text = "Xuất Báo Cáo";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Tổng Quan";
            pillItem2.Text = "Cảnh Báo";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Location = new Point(14, 251);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(244, 55);
            segmentedPill1.TabIndex = 27;
            segmentedPill1.Text = "segmentedPill1";
            segmentedPill1.SelectedIndexChanged += segmentedPill1_SelectedIndexChanged;
            // 
            // PanelBanChay
            // 
            PanelBanChay.BackColor = Color.White;
            PanelBanChay.BorderThickness = 5;
            PanelBanChay.Controls.Add(roundedShadowPanel5);
            PanelBanChay.Controls.Add(roundedShadowPanel4);
            PanelBanChay.Controls.Add(roundedShadowPanel3);
            PanelBanChay.Controls.Add(roundedShadowPanel2);
            PanelBanChay.Controls.Add(roundedShadowPanel1);
            PanelBanChay.Controls.Add(label17);
            PanelBanChay.Location = new Point(11, 853);
            PanelBanChay.Name = "PanelBanChay";
            PanelBanChay.Padding = new Padding(12);
            PanelBanChay.Size = new Size(1139, 551);
            PanelBanChay.TabIndex = 30;
            // 
            // roundedShadowPanel5
            // 
            roundedShadowPanel5.BackColor = Color.White;
            roundedShadowPanel5.BorderColor = Color.FromArgb(220, 220, 220);
            roundedShadowPanel5.Controls.Add(roundedPanel12);
            roundedShadowPanel5.Controls.Add(label36);
            roundedShadowPanel5.Controls.Add(label34);
            roundedShadowPanel5.Controls.Add(label35);
            roundedShadowPanel5.ForeColor = Color.Black;
            roundedShadowPanel5.Location = new Point(16, 444);
            roundedShadowPanel5.Name = "roundedShadowPanel5";
            roundedShadowPanel5.Padding = new Padding(8);
            roundedShadowPanel5.Size = new Size(1107, 92);
            roundedShadowPanel5.TabIndex = 5;
            // 
            // roundedPanel12
            // 
            roundedPanel12.BackColor = Color.FromArgb(192, 255, 255);
            roundedPanel12.BorderThickness = 5;
            roundedPanel12.Controls.Add(label37);
            roundedPanel12.Location = new Point(11, 11);
            roundedPanel12.Name = "roundedPanel12";
            roundedPanel12.Padding = new Padding(12);
            roundedPanel12.Size = new Size(68, 63);
            roundedPanel12.TabIndex = 1;
            // 
            // label37
            // 
            label37.AutoSize = true;
            label37.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label37.ForeColor = Color.Teal;
            label37.Location = new Point(15, 12);
            label37.Name = "label37";
            label37.Size = new Size(50, 38);
            label37.TabIndex = 0;
            label37.Text = "#5";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.BackColor = Color.Transparent;
            label36.Font = new Font("Segoe UI", 10.2F);
            label36.Location = new Point(84, 8);
            label36.Name = "label36";
            label36.Size = new Size(151, 23);
            label36.TabIndex = 0;
            label36.Text = "Canh Chua Cá Lóc";
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Font = new Font("Segoe UI", 10.2F);
            label34.Location = new Point(963, 27);
            label34.Name = "label34";
            label34.Size = new Size(99, 23);
            label34.TabIndex = 0;
            label34.Text = "123.234.000";
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.BackColor = Color.Transparent;
            label35.Font = new Font("Segoe UI", 10.2F);
            label35.Location = new Point(84, 40);
            label35.Name = "label35";
            label35.Size = new Size(74, 23);
            label35.TabIndex = 0;
            label35.Text = "200 Đơn";
            // 
            // roundedShadowPanel4
            // 
            roundedShadowPanel4.BackColor = Color.White;
            roundedShadowPanel4.BorderColor = Color.FromArgb(220, 220, 220);
            roundedShadowPanel4.Controls.Add(roundedPanel11);
            roundedShadowPanel4.Controls.Add(label32);
            roundedShadowPanel4.Controls.Add(label30);
            roundedShadowPanel4.Controls.Add(label31);
            roundedShadowPanel4.ForeColor = Color.Black;
            roundedShadowPanel4.Location = new Point(16, 350);
            roundedShadowPanel4.Name = "roundedShadowPanel4";
            roundedShadowPanel4.Padding = new Padding(8);
            roundedShadowPanel4.Size = new Size(1107, 88);
            roundedShadowPanel4.TabIndex = 4;
            // 
            // roundedPanel11
            // 
            roundedPanel11.BackColor = Color.FromArgb(192, 255, 255);
            roundedPanel11.BorderThickness = 5;
            roundedPanel11.Controls.Add(label33);
            roundedPanel11.Location = new Point(10, 8);
            roundedPanel11.Name = "roundedPanel11";
            roundedPanel11.Padding = new Padding(12);
            roundedPanel11.Size = new Size(68, 63);
            roundedPanel11.TabIndex = 1;
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label33.ForeColor = Color.Teal;
            label33.Location = new Point(15, 12);
            label33.Name = "label33";
            label33.Size = new Size(50, 38);
            label33.TabIndex = 0;
            label33.Text = "#4";
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.BackColor = Color.Transparent;
            label32.Font = new Font("Segoe UI", 10.2F);
            label32.Location = new Point(84, 8);
            label32.Name = "label32";
            label32.Size = new Size(122, 23);
            label32.TabIndex = 0;
            label32.Text = "Cua Hoàng Đế";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.BackColor = Color.Transparent;
            label30.Font = new Font("Segoe UI", 10.2F);
            label30.Location = new Point(84, 40);
            label30.Name = "label30";
            label30.Size = new Size(74, 23);
            label30.TabIndex = 0;
            label30.Text = "221 Đơn";
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Font = new Font("Segoe UI", 10.2F);
            label31.Location = new Point(963, 27);
            label31.Name = "label31";
            label31.Size = new Size(99, 23);
            label31.TabIndex = 0;
            label31.Text = "123.234.000";
            // 
            // roundedShadowPanel3
            // 
            roundedShadowPanel3.BackColor = Color.White;
            roundedShadowPanel3.BorderColor = Color.FromArgb(220, 220, 220);
            roundedShadowPanel3.Controls.Add(roundedPanel10);
            roundedShadowPanel3.Controls.Add(label28);
            roundedShadowPanel3.Controls.Add(label26);
            roundedShadowPanel3.Controls.Add(label27);
            roundedShadowPanel3.ForeColor = Color.Black;
            roundedShadowPanel3.Location = new Point(16, 252);
            roundedShadowPanel3.Name = "roundedShadowPanel3";
            roundedShadowPanel3.Padding = new Padding(8);
            roundedShadowPanel3.Size = new Size(1107, 92);
            roundedShadowPanel3.TabIndex = 3;
            // 
            // roundedPanel10
            // 
            roundedPanel10.BackColor = Color.FromArgb(192, 255, 255);
            roundedPanel10.BorderThickness = 5;
            roundedPanel10.Controls.Add(label29);
            roundedPanel10.Location = new Point(11, 11);
            roundedPanel10.Name = "roundedPanel10";
            roundedPanel10.Padding = new Padding(12);
            roundedPanel10.Size = new Size(68, 63);
            roundedPanel10.TabIndex = 1;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label29.ForeColor = Color.Teal;
            label29.Location = new Point(15, 12);
            label29.Name = "label29";
            label29.Size = new Size(50, 38);
            label29.TabIndex = 0;
            label29.Text = "#3";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.BackColor = Color.Transparent;
            label28.Font = new Font("Segoe UI", 10.2F);
            label28.Location = new Point(84, 8);
            label28.Name = "label28";
            label28.Size = new Size(129, 23);
            label28.TabIndex = 0;
            label28.Text = "Gà Quay Bơ Tỏi";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new Font("Segoe UI", 10.2F);
            label26.Location = new Point(963, 27);
            label26.Name = "label26";
            label26.Size = new Size(90, 23);
            label26.TabIndex = 0;
            label26.Text = "44.000.000";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.BackColor = Color.Transparent;
            label27.Font = new Font("Segoe UI", 10.2F);
            label27.Location = new Point(84, 40);
            label27.Name = "label27";
            label27.Size = new Size(74, 23);
            label27.TabIndex = 0;
            label27.Text = "340 Đơn";
            // 
            // roundedShadowPanel2
            // 
            roundedShadowPanel2.BackColor = Color.White;
            roundedShadowPanel2.BorderColor = Color.FromArgb(220, 220, 220);
            roundedShadowPanel2.Controls.Add(roundedPanel9);
            roundedShadowPanel2.Controls.Add(label22);
            roundedShadowPanel2.Controls.Add(label24);
            roundedShadowPanel2.Controls.Add(label23);
            roundedShadowPanel2.ForeColor = Color.Black;
            roundedShadowPanel2.Location = new Point(15, 157);
            roundedShadowPanel2.Name = "roundedShadowPanel2";
            roundedShadowPanel2.Padding = new Padding(8);
            roundedShadowPanel2.Size = new Size(1107, 89);
            roundedShadowPanel2.TabIndex = 2;
            // 
            // roundedPanel9
            // 
            roundedPanel9.BackColor = Color.FromArgb(192, 255, 255);
            roundedPanel9.BorderThickness = 5;
            roundedPanel9.Controls.Add(label25);
            roundedPanel9.Location = new Point(11, 8);
            roundedPanel9.Name = "roundedPanel9";
            roundedPanel9.Padding = new Padding(12);
            roundedPanel9.Size = new Size(68, 63);
            roundedPanel9.TabIndex = 1;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label25.ForeColor = Color.Teal;
            label25.Location = new Point(15, 12);
            label25.Name = "label25";
            label25.Size = new Size(50, 38);
            label25.TabIndex = 0;
            label25.Text = "#2";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.BackColor = Color.Transparent;
            label22.Font = new Font("Segoe UI", 10.2F);
            label22.Location = new Point(85, 8);
            label22.Name = "label22";
            label22.Size = new Size(139, 23);
            label22.TabIndex = 0;
            label22.Text = "Bò Nướng Lá Lốt";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.BackColor = Color.Transparent;
            label24.Font = new Font("Segoe UI", 10.2F);
            label24.Location = new Point(85, 40);
            label24.Name = "label24";
            label24.Size = new Size(74, 23);
            label24.TabIndex = 0;
            label24.Text = "234 Đơn";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new Font("Segoe UI", 10.2F);
            label23.Location = new Point(964, 27);
            label23.Name = "label23";
            label23.Size = new Size(90, 23);
            label23.TabIndex = 0;
            label23.Text = "33.000.000";
            // 
            // roundedShadowPanel1
            // 
            roundedShadowPanel1.BackColor = Color.White;
            roundedShadowPanel1.BorderColor = Color.FromArgb(220, 220, 220);
            roundedShadowPanel1.Controls.Add(roundedPanel8);
            roundedShadowPanel1.Controls.Add(label19);
            roundedShadowPanel1.Controls.Add(label20);
            roundedShadowPanel1.Controls.Add(label18);
            roundedShadowPanel1.ForeColor = Color.Black;
            roundedShadowPanel1.Location = new Point(15, 62);
            roundedShadowPanel1.Name = "roundedShadowPanel1";
            roundedShadowPanel1.Padding = new Padding(8);
            roundedShadowPanel1.Size = new Size(1107, 89);
            roundedShadowPanel1.TabIndex = 1;
            // 
            // roundedPanel8
            // 
            roundedPanel8.BackColor = Color.FromArgb(192, 255, 255);
            roundedPanel8.BorderThickness = 5;
            roundedPanel8.Controls.Add(label21);
            roundedPanel8.Location = new Point(12, 8);
            roundedPanel8.Name = "roundedPanel8";
            roundedPanel8.Padding = new Padding(12);
            roundedPanel8.Size = new Size(68, 63);
            roundedPanel8.TabIndex = 1;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label21.ForeColor = Color.Teal;
            label21.Location = new Point(15, 12);
            label21.Name = "label21";
            label21.Size = new Size(50, 38);
            label21.TabIndex = 0;
            label21.Text = "#1";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.BackColor = Color.Transparent;
            label19.Font = new Font("Segoe UI", 10.2F);
            label19.Location = new Point(85, 40);
            label19.Name = "label19";
            label19.Size = new Size(74, 23);
            label19.TabIndex = 0;
            label19.Text = "145 Đơn";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Segoe UI", 10.2F);
            label20.Location = new Point(964, 27);
            label20.Name = "label20";
            label20.Size = new Size(99, 23);
            label20.TabIndex = 0;
            label20.Text = "123.234.000";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.BackColor = Color.Transparent;
            label18.Font = new Font("Segoe UI", 10.2F);
            label18.Location = new Point(85, 8);
            label18.Name = "label18";
            label18.Size = new Size(142, 23);
            label18.TabIndex = 0;
            label18.Text = "Tôm Hùm Nướng";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label17.ForeColor = Color.Red;
            label17.Location = new Point(15, 21);
            label17.Name = "label17";
            label17.Size = new Size(181, 25);
            label17.TabIndex = 0;
            label17.Text = "Top 5 Món Bán Chạy";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // PanelBieuDo
            // 
            PanelBieuDo.BackColor = Color.White;
            PanelBieuDo.BorderThickness = 5;
            PanelBieuDo.Location = new Point(11, 312);
            PanelBieuDo.Name = "PanelBieuDo";
            PanelBieuDo.Padding = new Padding(12);
            PanelBieuDo.Size = new Size(1156, 535);
            PanelBieuDo.TabIndex = 32;
            // 
            // PanelCanhBao
            // 
            PanelCanhBao.AutoScroll = true;
            PanelCanhBao.BackColor = SystemColors.ButtonHighlight;
            PanelCanhBao.Controls.Add(panel1);
            PanelCanhBao.CustomizableEdges = customizableEdges1;
            PanelCanhBao.Location = new Point(11, 312);
            PanelCanhBao.Name = "PanelCanhBao";
            PanelCanhBao.ShadowDecoration.CustomizableEdges = customizableEdges2;
            PanelCanhBao.Size = new Size(1145, 578);
            PanelCanhBao.TabIndex = 34;
            PanelCanhBao.Visible = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(label3);
            panel1.Location = new Point(6, 15);
            panel1.Name = "panel1";
            panel1.Size = new Size(1146, 57);
            panel1.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(17, 10);
            label3.Name = "label3";
            label3.Size = new Size(163, 38);
            label3.TabIndex = 0;
            label3.Text = "Cảnh Báo !";
            // 
            // borderComboBox1
            // 
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.IntegralHeight = false;
            borderComboBox1.ItemHeight = 26;
            borderComboBox1.Items.AddRange(new object[] { "Hôm Nay", "Tuấn Này", "Tháng Này", "Quý Này", "Năm Này", "Tùy Chỉnh" });
            borderComboBox1.Location = new Point(781, 35);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(151, 32);
            borderComboBox1.TabIndex = 15;
            // 
            // FrmBaoCao
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1190, 900);
            Controls.Add(PanelBanChay);
            Controls.Add(segmentedPill1);
            Controls.Add(roundedButton2);
            Controls.Add(borderComboBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(PanelBieuDo);
            Controls.Add(PanelCanhBao);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmBaoCao";
            Text = "FrmBaoCao";
            Activated += FrmBaoCao_Activated;
            Load += FrmBaoCao_Load;
            PanelBanChay.ResumeLayout(false);
            PanelBanChay.PerformLayout();
            roundedShadowPanel5.ResumeLayout(false);
            roundedShadowPanel5.PerformLayout();
            roundedPanel12.ResumeLayout(false);
            roundedPanel12.PerformLayout();
            roundedShadowPanel4.ResumeLayout(false);
            roundedShadowPanel4.PerformLayout();
            roundedPanel11.ResumeLayout(false);
            roundedPanel11.PerformLayout();
            roundedShadowPanel3.ResumeLayout(false);
            roundedShadowPanel3.PerformLayout();
            roundedPanel10.ResumeLayout(false);
            roundedPanel10.PerformLayout();
            roundedShadowPanel2.ResumeLayout(false);
            roundedShadowPanel2.PerformLayout();
            roundedPanel9.ResumeLayout(false);
            roundedPanel9.PerformLayout();
            roundedShadowPanel1.ResumeLayout(false);
            roundedShadowPanel1.PerformLayout();
            roundedPanel8.ResumeLayout(false);
            roundedPanel8.PerformLayout();
            PanelCanhBao.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Controls.RoundedButton roundedButton2;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Controls.RoundedPanel PanelBanChay;
        private UiControls.RoundedShadowPanel roundedShadowPanel5;
        private UiControls.RoundedShadowPanel roundedShadowPanel4;
        private UiControls.RoundedShadowPanel roundedShadowPanel3;
        private UiControls.RoundedShadowPanel roundedShadowPanel2;
        private UiControls.RoundedShadowPanel roundedShadowPanel1;
        private Label label17;
        private Controls.RoundedPanel roundedPanel9;
        private Label label25;
        private Label label22;
        private Label label24;
        private Label label23;
        private Controls.RoundedPanel roundedPanel8;
        private Label label21;
        private Label label19;
        private Label label20;
        private Label label18;
        private Controls.RoundedPanel roundedPanel12;
        private Label label37;
        private Label label36;
        private Label label34;
        private Label label35;
        private Controls.RoundedPanel roundedPanel11;
        private Label label33;
        private Label label32;
        private Label label30;
        private Label label31;
        private Controls.RoundedPanel roundedPanel10;
        private Label label29;
        private Label label28;
        private Label label26;
        private Label label27;
        private ImageList imageList1;
        private Controls.RoundedPanel PanelBieuDo;
        private Guna.UI2.WinForms.Guna2GradientPanel PanelCanhBao;
        private UiControls.BorderComboBox borderComboBox1;
        private Panel panel1;
        private Label label3;
    }
}