namespace UI
{
    partial class Frm_XuatKho
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label8 = new Label();
            borderComboBox1 = new UiControls.BorderComboBox();
            label3 = new Label();
            borderComboBox2 = new UiControls.BorderComboBox();
            label4 = new Label();
            dateTimePicker1 = new DateTimePicker();
            label5 = new Label();
            roundedTextBox2 = new UI.Controls.RoundedTextBox();
            label6 = new Label();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            roundedButton1 = new UI.Controls.RoundedButton();
            roundedButton2 = new UI.Controls.RoundedButton();
            SuspendLayout();

            // label1 - Tiêu đề
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(160, 28);
            label1.Text = "Phiếu Xuất Kho";

            // label2 - Mô tả
            label2.AutoSize = true;
            label2.Location = new Point(12, 40);
            label2.Name = "label2";
            label2.Size = new Size(250, 20);
            label2.Text = "Xuất nguyên liệu từ kho để sử dụng";

            // label8 - Món ăn
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(12, 80);
            label8.Name = "label8";
            label8.Text = "Món Ăn";

            // borderComboBox1 - Chọn món
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.ItemHeight = 26;
            borderComboBox1.Items.AddRange(new object[] { "Phở Bò", "Cơm Tấm", "Bánh Mì", "Cá Kho", "Gà Chiên" });
            borderComboBox1.Location = new Point(12, 103);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(600, 32);
            borderComboBox1.Text = "Chọn món ăn";

            // label3 - Kho xuất
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(12, 150);
            label3.Name = "label3";
            label3.Text = "Kho Xuất";

            // borderComboBox2 - Chọn kho
            borderComboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox2.ItemHeight = 26;
            borderComboBox2.Items.AddRange(new object[] { "Kho Trung Tâm", "Kho Quận 1", "Kho Quận 3" });
            borderComboBox2.Location = new Point(12, 173);
            borderComboBox2.Name = "borderComboBox2";
            borderComboBox2.Size = new Size(600, 32);
            borderComboBox2.Text = "Chọn kho xuất";

            // label4 - Số lượng
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(12, 220);
            label4.Name = "label4";
            label4.Text = "Số Lượng";

            // roundedTextBox2 - Nhập số lượng
            roundedTextBox2.BackColor = Color.White;
            roundedTextBox2.Font = new Font("Segoe UI", 10F);
            roundedTextBox2.Location = new Point(12, 243);
            roundedTextBox2.Name = "roundedTextBox2";
            roundedTextBox2.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox2.Size = new Size(200, 51);
            roundedTextBox2.TabIndex = 0;

            // label5 - Ngày xuất
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(230, 220);
            label5.Name = "label5";
            label5.Text = "Ngày Xuất";

            // dateTimePicker1
            dateTimePicker1.Location = new Point(230, 243);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(382, 27);
            dateTimePicker1.TabIndex = 1;

            // label6 - Ghi chú
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(12, 300);
            label6.Name = "label6";
            label6.Text = "Ghi Chú";

            // roundedTextBox1 - Ghi chú
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.Location = new Point(12, 323);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(600, 82);
            roundedTextBox1.TabIndex = 2;

            // roundedButton1 - Hủy
            roundedButton1.BackColor = Color.White;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.Location = new Point(330, 430);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Size = new Size(94, 29);
            roundedButton1.Text = "Hủy";
            roundedButton1.UseVisualStyleBackColor = false;

            // roundedButton2 - Tạo phiếu
            roundedButton2.BackColor = Color.Black;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.HoverBackColor = Color.DarkGray;
            roundedButton2.Location = new Point(430, 430);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Size = new Size(161, 29);
            roundedButton2.Text = "Tạo Phiếu Xuất";
            roundedButton2.UseVisualStyleBackColor = false;

            // Form
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(624, 480);
            this.Controls.Add(roundedButton2);
            this.Controls.Add(roundedButton1);
            this.Controls.Add(roundedTextBox1);
            this.Controls.Add(label6);
            this.Controls.Add(dateTimePicker1);
            this.Controls.Add(label5);
            this.Controls.Add(roundedTextBox2);
            this.Controls.Add(label4);
            this.Controls.Add(borderComboBox2);
            this.Controls.Add(label3);
            this.Controls.Add(borderComboBox1);
            this.Controls.Add(label8);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Name = "Frm_XuatKho";
            this.Text = "Phiếu Xuất Kho";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label8;
        private UiControls.BorderComboBox borderComboBox1;
        private Label label3;
        private UiControls.BorderComboBox borderComboBox2;
        private Label label4;
        private DateTimePicker dateTimePicker1;
        private Label label5;
        private UI.Controls.RoundedTextBox roundedTextBox2;
        private Label label6;
        private UI.Controls.RoundedTextBox roundedTextBox1;
        private UI.Controls.RoundedButton roundedButton1;
        private UI.Controls.RoundedButton roundedButton2;
    }
}