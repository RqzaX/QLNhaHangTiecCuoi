namespace UI
{
    partial class Frm_ThemThucDon
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
            label1 = new Label();
            label2 = new Label();
            borderComboBox1 = new UiControls.BorderComboBox();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            roundedTextBox2 = new UI.Controls.RoundedTextBox();
            roundedTextBox3 = new UI.Controls.RoundedTextBox();
            roundedTextBox4 = new UI.Controls.RoundedTextBox();
            roundedTextBox5 = new UI.Controls.RoundedTextBox();
            roundedButton1 = new UI.Controls.RoundedButton();
            roundedButton2 = new UI.Controls.RoundedButton();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(146, 28);
            label1.TabIndex = 0;
            label1.Text = "Thêm Món Mới";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 37);
            label2.Name = "label2";
            label2.Size = new Size(156, 17);
            label2.TabIndex = 1;
            label2.Text = "Thêm Thông tin món mới";
            // 
            // borderComboBox1
            // 
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.IntegralHeight = false;
            borderComboBox1.ItemHeight = 26;
            borderComboBox1.Items.AddRange(new object[] { "Khai Vị", "Món Chính", "Hải Sản", "Canh/Súp", "Đồ Uống", "Tráng Miệng" });
            borderComboBox1.Location = new Point(12, 205);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(282, 32);
            borderComboBox1.TabIndex = 2;
            borderComboBox1.Text = "Chọn Danh Mục";
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(12, 114);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(668, 51);
            roundedTextBox1.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(398, 182);
            label3.Name = "label3";
            label3.Size = new Size(63, 20);
            label3.TabIndex = 4;
            label3.Text = "Giá Bán";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 182);
            label4.Name = "label4";
            label4.Size = new Size(80, 20);
            label4.TabIndex = 5;
            label4.Text = "Danh Mục";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(398, 274);
            label5.Name = "label5";
            label5.Size = new Size(56, 20);
            label5.TabIndex = 4;
            label5.Text = "Đơn Vị";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 274);
            label6.Name = "label6";
            label6.Size = new Size(64, 20);
            label6.TabIndex = 4;
            label6.Text = "Giá Vốn";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(12, 364);
            label7.Name = "label7";
            label7.Size = new Size(53, 20);
            label7.TabIndex = 4;
            label7.Text = "Mô Tả";
            label7.Click += label3_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(12, 80);
            label8.Name = "label8";
            label8.Size = new Size(70, 20);
            label8.TabIndex = 4;
            label8.Text = "Tên Món";
            label8.Click += label3_Click;
            // 
            // roundedTextBox2
            // 
            roundedTextBox2.BackColor = Color.White;
            roundedTextBox2.Font = new Font("Segoe UI", 10F);
            roundedTextBox2.ForeColor = Color.Black;
            roundedTextBox2.Location = new Point(398, 205);
            roundedTextBox2.Name = "roundedTextBox2";
            roundedTextBox2.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox2.Size = new Size(282, 51);
            roundedTextBox2.TabIndex = 6;
            // 
            // roundedTextBox3
            // 
            roundedTextBox3.BackColor = Color.White;
            roundedTextBox3.Font = new Font("Segoe UI", 10F);
            roundedTextBox3.ForeColor = Color.Black;
            roundedTextBox3.Location = new Point(12, 297);
            roundedTextBox3.Name = "roundedTextBox3";
            roundedTextBox3.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox3.Size = new Size(282, 51);
            roundedTextBox3.TabIndex = 6;
            roundedTextBox3.Load += roundedTextBox3_Load;
            // 
            // roundedTextBox4
            // 
            roundedTextBox4.BackColor = Color.White;
            roundedTextBox4.Font = new Font("Segoe UI", 10F);
            roundedTextBox4.ForeColor = Color.Black;
            roundedTextBox4.Location = new Point(398, 297);
            roundedTextBox4.Name = "roundedTextBox4";
            roundedTextBox4.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox4.Size = new Size(282, 51);
            roundedTextBox4.TabIndex = 6;
            roundedTextBox4.Load += roundedTextBox3_Load;
            // 
            // roundedTextBox5
            // 
            roundedTextBox5.BackColor = Color.White;
            roundedTextBox5.Font = new Font("Segoe UI", 10F);
            roundedTextBox5.ForeColor = Color.Black;
            roundedTextBox5.Location = new Point(12, 387);
            roundedTextBox5.Name = "roundedTextBox5";
            roundedTextBox5.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox5.Size = new Size(668, 109);
            roundedTextBox5.TabIndex = 6;
            roundedTextBox5.Load += roundedTextBox3_Load;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.White;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.HoverBackColor = Color.White;
            roundedButton1.Location = new Point(433, 540);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.PressedBackColor = Color.White;
            roundedButton1.Size = new Size(94, 29);
            roundedButton1.TabIndex = 7;
            roundedButton1.Text = "Hủy";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.HoverBackColor = Color.Transparent;
            roundedButton2.Location = new Point(560, 540);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.PressedBackColor = Color.White;
            roundedButton2.Size = new Size(120, 29);
            roundedButton2.TabIndex = 7;
            roundedButton2.Text = "Thêm Món";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // Frm_ThemThucDon
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(712, 588);
            Controls.Add(roundedButton2);
            Controls.Add(roundedButton1);
            Controls.Add(roundedTextBox5);
            Controls.Add(roundedTextBox4);
            Controls.Add(roundedTextBox3);
            Controls.Add(roundedTextBox2);
            Controls.Add(label4);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(label3);
            Controls.Add(roundedTextBox1);
            Controls.Add(borderComboBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Frm_ThemThucDon";
            Text = "Frm_ThemThucDon";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private UiControls.BorderComboBox borderComboBox1;
        private Controls.RoundedTextBox roundedTextBox1;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Controls.RoundedTextBox roundedTextBox2;
        private Controls.RoundedTextBox roundedTextBox3;
        private Controls.RoundedTextBox roundedTextBox4;
        private Controls.RoundedTextBox roundedTextBox5;
        private Controls.RoundedButton roundedButton1;
        private Controls.RoundedButton roundedButton2;
    }
}