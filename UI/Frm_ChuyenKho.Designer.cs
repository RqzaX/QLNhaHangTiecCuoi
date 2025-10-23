namespace UI
{
    partial class Frm_ChuyenKho
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
            borderComboBox2 = new UiControls.BorderComboBox();
            label3 = new Label();
            label4 = new Label();
            dateTimePicker1 = new DateTimePicker();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            label5 = new Label();
            btnHuy = new UI.Controls.RoundedButton();
            btnTaoPhieuChuyen = new UI.Controls.RoundedButton();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(170, 28);
            label1.TabIndex = 3;
            label1.Text = "Phiếu Chuyển Kho";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 37);
            label2.Name = "label2";
            label2.Size = new Size(221, 20);
            label2.TabIndex = 4;
            label2.Text = "Chuyển hàng giữa các chi nhánh";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(12, 92);
            label8.Name = "label8";
            label8.Size = new Size(60, 20);
            label8.TabIndex = 9;
            label8.Text = "Từ Kho";
            label8.Click += label8_Click;
            // 
            // borderComboBox1
            // 
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.IntegralHeight = false;
            borderComboBox1.ItemHeight = 26;
            borderComboBox1.Items.AddRange(new object[] { "Kho Trung Tâm", "Kho Quận 1", "Kho Quận 3" });
            borderComboBox1.Location = new Point(12, 115);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(237, 32);
            borderComboBox1.TabIndex = 10;
            borderComboBox1.Text = "Chọn Kho Nguồn";
            // 
            // borderComboBox2
            // 
            borderComboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox2.FormattingEnabled = true;
            borderComboBox2.IntegralHeight = false;
            borderComboBox2.ItemHeight = 26;
            borderComboBox2.Items.AddRange(new object[] { "Kho Trung Tâm", "Kho Quận 1", "Kho Quận 3" });
            borderComboBox2.Location = new Point(378, 115);
            borderComboBox2.Name = "borderComboBox2";
            borderComboBox2.Size = new Size(240, 32);
            borderComboBox2.TabIndex = 10;
            borderComboBox2.Text = "Chọn Kho Đích";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(378, 92);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 9;
            label3.Text = "Đến Kho";
            label3.Click += label8_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 177);
            label4.Name = "label4";
            label4.Size = new Size(102, 20);
            label4.TabIndex = 9;
            label4.Text = "Ngày Chuyển";
            label4.Click += label8_Click;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(12, 211);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(606, 27);
            dateTimePicker1.TabIndex = 11;
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(12, 311);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(606, 61);
            roundedTextBox1.TabIndex = 12;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 288);
            label5.Name = "label5";
            label5.Size = new Size(64, 20);
            label5.TabIndex = 13;
            label5.Text = "Ghi Chú";
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.White;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI Semibold", 10.5F);
            btnHuy.ForeColor = Color.Black;
            btnHuy.Location = new Point(336, 424);
            btnHuy.Name = "btnHuy";
            btnHuy.Padding = new Padding(10, 6, 10, 6);
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 14;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // btnTaoPhieuChuyen
            // 
            btnTaoPhieuChuyen.BackColor = Color.Black;
            btnTaoPhieuChuyen.FlatAppearance.BorderSize = 0;
            btnTaoPhieuChuyen.FlatStyle = FlatStyle.Flat;
            btnTaoPhieuChuyen.Font = new Font("Segoe UI Semibold", 10.5F);
            btnTaoPhieuChuyen.ForeColor = Color.White;
            btnTaoPhieuChuyen.Location = new Point(451, 424);
            btnTaoPhieuChuyen.Name = "btnTaoPhieuChuyen";
            btnTaoPhieuChuyen.Padding = new Padding(10, 6, 10, 6);
            btnTaoPhieuChuyen.Size = new Size(167, 29);
            btnTaoPhieuChuyen.TabIndex = 14;
            btnTaoPhieuChuyen.Text = "Tạo Phiếu Chuyển";
            btnTaoPhieuChuyen.UseVisualStyleBackColor = false;
            btnTaoPhieuChuyen.Click += btnTaoPhieuChuyen_Click;
            // 
            // Frm_ChuyenKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(630, 478);
            Controls.Add(btnTaoPhieuChuyen);
            Controls.Add(btnHuy);
            Controls.Add(label5);
            Controls.Add(roundedTextBox1);
            Controls.Add(dateTimePicker1);
            Controls.Add(borderComboBox2);
            Controls.Add(borderComboBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label8);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Frm_ChuyenKho";
            Text = "Frm_ChuyenKho";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label8;
        private UiControls.BorderComboBox borderComboBox1;
        private UiControls.BorderComboBox borderComboBox2;
        private Label label3;
        private Label label4;
        private DateTimePicker dateTimePicker1;
        private Controls.RoundedTextBox roundedTextBox1;
        private Label label5;
        private Controls.RoundedButton btnHuy;
        private Controls.RoundedButton btnTaoPhieuChuyen;
    }
}