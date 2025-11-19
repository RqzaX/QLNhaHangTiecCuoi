namespace UI
{
    partial class Frm_ThemNV
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
            label3 = new Label();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            label4 = new Label();
            roundedComboBox1 = new UI.Controls.RoundedComboBox();
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
            label1.Size = new Size(195, 28);
            label1.TabIndex = 6;
            label1.Text = "Thêm Nhân Viên Mới";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 37);
            label2.Name = "label2";
            label2.Size = new Size(217, 20);
            label2.TabIndex = 7;
            label2.Text = "Nhập Thông Tin Nhân Viên Mới";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 85);
            label3.Name = "label3";
            label3.Size = new Size(80, 20);
            label3.TabIndex = 16;
            label3.Text = "Họ  và tên";
            label3.Click += label3_Click;
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(12, 108);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(462, 51);
            roundedTextBox1.TabIndex = 17;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 180);
            label4.Name = "label4";
            label4.Size = new Size(67, 20);
            label4.TabIndex = 16;
            label4.Text = "Chức Vụ";
            label4.Click += label3_Click;
            // 
            // roundedComboBox1
            // 
            roundedComboBox1.BackColor = Color.FromArgb(248, 248, 250);
            roundedComboBox1.FlatStyle = FlatStyle.Flat;
            roundedComboBox1.ForeColor = Color.FromArgb(70, 70, 70);
            roundedComboBox1.FormattingEnabled = true;
            roundedComboBox1.Location = new Point(12, 203);
            roundedComboBox1.Name = "roundedComboBox1";
            roundedComboBox1.Size = new Size(242, 28);
            roundedComboBox1.TabIndex = 18;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.White;
            roundedButton1.BorderThickness = 0;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.Location = new Point(230, 355);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.Size = new Size(94, 43);
            roundedButton1.TabIndex = 21;
            roundedButton1.Text = "Hủy";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.BorderThickness = 0;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.Location = new Point(330, 355);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.Size = new Size(144, 43);
            roundedButton2.TabIndex = 21;
            roundedButton2.Text = "Thêm NV";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // Frm_ThemNV
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(510, 423);
            Controls.Add(roundedButton2);
            Controls.Add(roundedButton1);
            Controls.Add(roundedComboBox1);
            Controls.Add(roundedTextBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Frm_ThemNV";
            Text = "Frm_ThemNV";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Controls.RoundedTextBox roundedTextBox1;
        private Label label4;
        private Controls.RoundedComboBox roundedComboBox1;
        private Controls.RoundedButton roundedButton1;
        private Controls.RoundedButton roundedButton2;
    }
}