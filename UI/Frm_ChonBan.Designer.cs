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
            label1 = new Label();
            label2 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            trangThaiBan1 = new UI.Controls.TrangThaiBan();
            btnThoat = new Button();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            roundedPanel3 = new UI.Controls.RoundedPanel();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Variable Display", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(14, 12);
            label1.Name = "label1";
            label1.Size = new Size(256, 32);
            label1.TabIndex = 0;
            label1.Text = "Sơ đồ bàn - Chọn bàn";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(14, 47);
            label2.Name = "label2";
            label2.Size = new Size(400, 26);
            label2.TabIndex = 1;
            label2.Text = "Chọn bàn để bắt đầu order hoặc xem chi tiết";
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Tất cả";
            pillItem2.Text = "Khu A - Tầng 1";
            pillItem3.Text = "Khu B - Tầng 1";
            pillItem4.Text = "Khu C - Tầng 2";
            pillItem5.Text = "Khu VIP - Tầng 2";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Items.Add(pillItem3);
            segmentedPill1.Items.Add(pillItem4);
            segmentedPill1.Items.Add(pillItem5);
            segmentedPill1.Location = new Point(90, 89);
            segmentedPill1.Margin = new Padding(3, 4, 3, 4);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(728, 56);
            segmentedPill1.TabIndex = 2;
            segmentedPill1.TabStop = false;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // trangThaiBan1
            // 
            trangThaiBan1.Font = new Font("Segoe UI", 10F);
            trangThaiBan1.Location = new Point(38, 220);
            trangThaiBan1.Margin = new Padding(3, 4, 3, 4);
            trangThaiBan1.Name = "trangThaiBan1";
            trangThaiBan1.Size = new Size(229, 237);
            trangThaiBan1.Status = UI.Controls.TableStatus.DangDung;
            trangThaiBan1.TabIndex = 3;
            trangThaiBan1.TabStop = false;
            trangThaiBan1.Text = "trangThaiBan1";
            // 
            // btnThoat
            // 
            btnThoat.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnThoat.BackColor = Color.Transparent;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.Font = new Font("Segoe UI Variable Small", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = Color.Black;
            btnThoat.Location = new Point(897, -3);
            btnThoat.Margin = new Padding(3, 4, 3, 4);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(69, 65);
            btnThoat.TabIndex = 8;
            btnThoat.Text = "✖";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnThoat_Click;
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.ForestGreen;
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.ForeColor = Color.Transparent;
            roundedPanel1.Location = new Point(26, 156);
            roundedPanel1.Margin = new Padding(3, 4, 3, 4);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(14, 16, 14, 16);
            roundedPanel1.Size = new Size(27, 31);
            roundedPanel1.TabIndex = 9;
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.Crimson;
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.ForeColor = Color.Transparent;
            roundedPanel2.Location = new Point(123, 156);
            roundedPanel2.Margin = new Padding(3, 4, 3, 4);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(14, 16, 14, 16);
            roundedPanel2.Size = new Size(27, 31);
            roundedPanel2.TabIndex = 10;
            // 
            // roundedPanel3
            // 
            roundedPanel3.BackColor = Color.Gold;
            roundedPanel3.BorderThickness = 5;
            roundedPanel3.ForeColor = Color.Transparent;
            roundedPanel3.Location = new Point(263, 156);
            roundedPanel3.Margin = new Padding(3, 4, 3, 4);
            roundedPanel3.Name = "roundedPanel3";
            roundedPanel3.Padding = new Padding(14, 16, 14, 16);
            roundedPanel3.Size = new Size(27, 31);
            roundedPanel3.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(61, 160);
            label3.Name = "label3";
            label3.Size = new Size(62, 26);
            label3.TabIndex = 12;
            label3.Text = "Trống";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(158, 160);
            label4.Name = "label4";
            label4.Size = new Size(110, 26);
            label4.TabIndex = 13;
            label4.Text = "Đang dùng";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Variable Small", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(297, 160);
            label5.Name = "label5";
            label5.Size = new Size(70, 26);
            label5.TabIndex = 14;
            label5.Text = "Đã đặt";
            // 
            // Frm_ChonBan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.White;
            ClientSize = new Size(966, 688);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(roundedPanel3);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(btnThoat);
            Controls.Add(trangThaiBan1);
            Controls.Add(segmentedPill1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Frm_ChonBan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Frm_ChonBan";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Controls.TrangThaiBan trangThaiBan1;
        private Button btnThoat;
        private Controls.RoundedPanel roundedPanel1;
        private Controls.RoundedPanel roundedPanel2;
        private Controls.RoundedPanel roundedPanel3;
        private Label label3;
        private Label label4;
        private Label label5;
    }
}