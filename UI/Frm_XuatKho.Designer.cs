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
            cbbNguyenLieu = new UiControls.BorderComboBox();
            label3 = new Label();
            cbbKhoXuat = new UiControls.BorderComboBox();
            label4 = new Label();
            dateNgayXuat = new DateTimePicker();
            label5 = new Label();
            txtSoLuong = new UI.Controls.RoundedTextBox();
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
            label1.Size = new Size(145, 28);
            label1.TabIndex = 11;
            label1.Text = "Phiếu Xuất Kho";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 40);
            label2.Name = "label2";
            label2.Size = new Size(243, 20);
            label2.TabIndex = 10;
            label2.Text = "Xuất nguyên liệu từ kho để sử dụng";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(12, 86);
            label8.Name = "label8";
            label8.Size = new Size(93, 20);
            label8.TabIndex = 9;
            label8.Text = "Nguyên liệu";
            // 
            // cbbNguyenLieu
            // 
            cbbNguyenLieu.DrawMode = DrawMode.OwnerDrawFixed;
            cbbNguyenLieu.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbbNguyenLieu.FormattingEnabled = true;
            cbbNguyenLieu.IntegralHeight = false;
            cbbNguyenLieu.ItemHeight = 26;
            cbbNguyenLieu.Location = new Point(12, 109);
            cbbNguyenLieu.Name = "cbbNguyenLieu";
            cbbNguyenLieu.Size = new Size(600, 32);
            cbbNguyenLieu.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(12, 150);
            label3.Name = "label3";
            label3.Size = new Size(74, 20);
            label3.TabIndex = 7;
            label3.Text = "Kho Xuất";
            // 
            // cbbKhoXuat
            // 
            cbbKhoXuat.DrawMode = DrawMode.OwnerDrawFixed;
            cbbKhoXuat.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbbKhoXuat.IntegralHeight = false;
            cbbKhoXuat.ItemHeight = 26;
            cbbKhoXuat.Location = new Point(12, 173);
            cbbKhoXuat.Name = "cbbKhoXuat";
            cbbKhoXuat.Size = new Size(600, 32);
            cbbKhoXuat.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(12, 220);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 5;
            label4.Text = "Số Lượng";
            // 
            // dateNgayXuat
            // 
            dateNgayXuat.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dateNgayXuat.Format = DateTimePickerFormat.Short;
            dateNgayXuat.Location = new Point(230, 243);
            dateNgayXuat.Name = "dateNgayXuat";
            dateNgayXuat.Size = new Size(382, 34);
            dateNgayXuat.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(230, 220);
            label5.Name = "label5";
            label5.Size = new Size(83, 20);
            label5.TabIndex = 4;
            label5.Text = "Ngày Xuất";
            // 
            // txtSoLuong
            // 
            txtSoLuong.BackColor = Color.White;
            txtSoLuong.Font = new Font("Segoe UI", 10F);
            txtSoLuong.ForeColor = Color.Black;
            txtSoLuong.Location = new Point(12, 243);
            txtSoLuong.Name = "txtSoLuong";
            txtSoLuong.Padding = new Padding(10, 8, 10, 8);
            txtSoLuong.Size = new Size(200, 34);
            txtSoLuong.TabIndex = 0;
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.White;
            roundedButton1.BorderThickness = 0;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.Location = new Point(264, 323);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(12, 8, 12, 8);
            roundedButton1.Size = new Size(94, 48);
            roundedButton1.TabIndex = 1;
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
            roundedButton2.HoverBackColor = Color.DarkGray;
            roundedButton2.Location = new Point(364, 323);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(12, 8, 12, 8);
            roundedButton2.Size = new Size(161, 48);
            roundedButton2.TabIndex = 0;
            roundedButton2.Text = "Tạo Phiếu Xuất";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // Frm_XuatKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 381);
            Controls.Add(roundedButton2);
            Controls.Add(roundedButton1);
            Controls.Add(dateNgayXuat);
            Controls.Add(label5);
            Controls.Add(txtSoLuong);
            Controls.Add(label4);
            Controls.Add(cbbKhoXuat);
            Controls.Add(label3);
            Controls.Add(cbbNguyenLieu);
            Controls.Add(label8);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Frm_XuatKho";
            Text = "Phiếu Xuất Kho";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label8;
        private UiControls.BorderComboBox cbbNguyenLieu;
        private Label label3;
        private UiControls.BorderComboBox cbbKhoXuat;
        private Label label4;
        private DateTimePicker dateNgayXuat;
        private Label label5;
        private UI.Controls.RoundedTextBox txtSoLuong;
        private UI.Controls.RoundedButton roundedButton1;
        private UI.Controls.RoundedButton roundedButton2;
    }
}