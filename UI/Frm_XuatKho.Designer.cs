namespace UI
{
    partial class Frm_XuatKho
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
            label8 = new Label();
            dateTimePicker1 = new DateTimePicker();
            label3 = new Label();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            label4 = new Label();
            roundedButton2 = new UI.Controls.RoundedButton();
            roundedButton1 = new UI.Controls.RoundedButton();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(145, 28);
            label1.TabIndex = 2;
            label1.Text = "Phiếu Xuất Kho";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 37);
            label2.Name = "label2";
            label2.Size = new Size(153, 20);
            label2.TabIndex = 3;
            label2.Text = "Xuất hàng ra khỏi kho";
            // 
            // borderComboBox1
            // 
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.IntegralHeight = false;
            borderComboBox1.ItemHeight = 26;
            borderComboBox1.Items.AddRange(new object[] { "Sản Xuất", "Hư Hỏng", "Hết Hạn", "Khác" });
            borderComboBox1.Location = new Point(12, 104);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(600, 32);
            borderComboBox1.TabIndex = 9;
            borderComboBox1.Text = "Chọn lý do";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(12, 81);
            label8.Name = "label8";
            label8.Size = new Size(82, 20);
            label8.TabIndex = 8;
            label8.Text = "Lý do xuất";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(12, 180);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(250, 27);
            dateTimePicker1.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 157);
            label3.Name = "label3";
            label3.Size = new Size(83, 20);
            label3.TabIndex = 8;
            label3.Text = "Ngày Xuất";
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(12, 297);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(597, 95);
            roundedTextBox1.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 260);
            label4.Name = "label4";
            label4.Size = new Size(64, 20);
            label4.TabIndex = 12;
            label4.Text = "Ghi Chú";
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.HoverBackColor = Color.White;
            roundedButton2.Location = new Point(451, 447);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.PressedBackColor = Color.White;
            roundedButton2.Size = new Size(161, 29);
            roundedButton2.TabIndex = 13;
            roundedButton2.Text = "Tạo Phiếu Xuất";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.White;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.HoverBackColor = Color.White;
            roundedButton1.Location = new Point(334, 447);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.PressedBackColor = Color.White;
            roundedButton1.Size = new Size(94, 29);
            roundedButton1.TabIndex = 14;
            roundedButton1.Text = "Hủy";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // Frm_XuatKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(621, 504);
            Controls.Add(roundedButton1);
            Controls.Add(roundedButton2);
            Controls.Add(label4);
            Controls.Add(roundedTextBox1);
            Controls.Add(dateTimePicker1);
            Controls.Add(borderComboBox1);
            Controls.Add(label3);
            Controls.Add(label8);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Frm_XuatKho";
            Text = "Frm_XuatKho";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private UiControls.BorderComboBox borderComboBox1;
        private Label label8;
        private DateTimePicker dateTimePicker1;
        private Label label3;
        private Controls.RoundedTextBox roundedTextBox1;
        private Label label4;
        private Controls.RoundedButton roundedButton2;
        private Controls.RoundedButton roundedButton1;
    }
}