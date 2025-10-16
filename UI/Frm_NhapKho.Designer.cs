namespace UI
{
    partial class Frm_NhapKho
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
            label8 = new Label();
            borderComboBox1 = new UiControls.BorderComboBox();
            label3 = new Label();
            label4 = new Label();
            roundedTextBox2 = new UI.Controls.RoundedTextBox();
            dateTimePicker1 = new DateTimePicker();
            label5 = new Label();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
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
            label1.Size = new Size(153, 28);
            label1.TabIndex = 1;
            label1.Text = "Phiếu Nhập Kho";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 46);
            label2.Name = "label2";
            label2.Size = new Size(205, 20);
            label2.TabIndex = 2;
            label2.Text = "Tạo phiếu nhập hàng vào kho";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(12, 88);
            label8.Name = "label8";
            label8.Size = new Size(70, 20);
            label8.TabIndex = 5;
            label8.Text = "Tên Món";
            // 
            // borderComboBox1
            // 
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.IntegralHeight = false;
            borderComboBox1.ItemHeight = 26;
            borderComboBox1.Items.AddRange(new object[] { "Khai Vị", "Món Chính", "Hải Sản", "Canh/Súp", "Đồ Uống", "Tráng Miệng" });
            borderComboBox1.Location = new Point(12, 111);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(600, 32);
            borderComboBox1.TabIndex = 7;
            borderComboBox1.Text = "Chọn Danh Mục";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 171);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 5;
            label3.Text = "Ngày Nhập";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(355, 171);
            label4.Name = "label4";
            label4.Size = new Size(91, 20);
            label4.TabIndex = 5;
            label4.Text = "Số Hóa Đơn";
            // 
            // roundedTextBox2
            // 
            roundedTextBox2.BackColor = Color.White;
            roundedTextBox2.Font = new Font("Segoe UI", 10F);
            roundedTextBox2.ForeColor = Color.Black;
            roundedTextBox2.Location = new Point(355, 194);
            roundedTextBox2.Name = "roundedTextBox2";
            roundedTextBox2.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox2.Size = new Size(257, 51);
            roundedTextBox2.TabIndex = 6;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(12, 194);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(250, 27);
            dateTimePicker1.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 268);
            label5.Name = "label5";
            label5.Size = new Size(64, 20);
            label5.TabIndex = 5;
            label5.Text = "Ghi Chú";
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(12, 291);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(600, 82);
            roundedTextBox1.TabIndex = 6;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.White;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.HoverBackColor = Color.White;
            roundedButton1.Location = new Point(325, 396);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.PressedBackColor = Color.White;
            roundedButton1.Size = new Size(94, 29);
            roundedButton1.TabIndex = 9;
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
            roundedButton2.HoverBackColor = Color.White;
            roundedButton2.Location = new Point(425, 396);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.PressedBackColor = Color.White;
            roundedButton2.Size = new Size(161, 29);
            roundedButton2.TabIndex = 9;
            roundedButton2.Text = "Tạo Phiếu Nhập";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // Frm_NhapKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 455);
            Controls.Add(roundedButton2);
            Controls.Add(roundedButton1);
            Controls.Add(dateTimePicker1);
            Controls.Add(borderComboBox1);
            Controls.Add(roundedTextBox1);
            Controls.Add(roundedTextBox2);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label8);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Frm_NhapKho";
            Text = "Frm_NhapKho";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label8;
        private UiControls.BorderComboBox borderComboBox1;
        private Label label3;
        private Label label4;
        private Controls.RoundedTextBox roundedTextBox2;
        private DateTimePicker dateTimePicker1;
        private Label label5;
        private Controls.RoundedTextBox roundedTextBox1;
        private Controls.RoundedButton roundedButton1;
        private Controls.RoundedButton roundedButton2;
    }
}