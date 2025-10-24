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
            cbbTuKho = new UiControls.BorderComboBox();
            cbbDenKho = new UiControls.BorderComboBox();
            label3 = new Label();
            label4 = new Label();
            dateNgayChuyen = new DateTimePicker();
            btnHuy = new UI.Controls.RoundedButton();
            btnTaoPhieuChuyen = new UI.Controls.RoundedButton();
            cbbNguyenLieu = new UiControls.BorderComboBox();
            label6 = new Label();
            label7 = new Label();
            txtSoLuong = new UI.Controls.RoundedTextBox();
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
            // 
            // cbbTuKho
            // 
            cbbTuKho.DrawMode = DrawMode.OwnerDrawFixed;
            cbbTuKho.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbbTuKho.FormattingEnabled = true;
            cbbTuKho.IntegralHeight = false;
            cbbTuKho.ItemHeight = 26;
            cbbTuKho.Items.AddRange(new object[] { "Kho Trung Tâm", "Kho Quận 1", "Kho Quận 3" });
            cbbTuKho.Location = new Point(12, 115);
            cbbTuKho.Name = "cbbTuKho";
            cbbTuKho.Size = new Size(237, 32);
            cbbTuKho.TabIndex = 10;
            cbbTuKho.Text = "Chọn Kho Nguồn";
            // 
            // cbbDenKho
            // 
            cbbDenKho.DrawMode = DrawMode.OwnerDrawFixed;
            cbbDenKho.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbbDenKho.FormattingEnabled = true;
            cbbDenKho.IntegralHeight = false;
            cbbDenKho.ItemHeight = 26;
            cbbDenKho.Items.AddRange(new object[] { "Kho Trung Tâm", "Kho Quận 1", "Kho Quận 3" });
            cbbDenKho.Location = new Point(378, 115);
            cbbDenKho.Name = "cbbDenKho";
            cbbDenKho.Size = new Size(240, 32);
            cbbDenKho.TabIndex = 10;
            cbbDenKho.Text = "Chọn Kho Đích";
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
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(32, 240);
            label4.Name = "label4";
            label4.Size = new Size(102, 20);
            label4.TabIndex = 9;
            label4.Text = "Ngày Chuyển";
            // 
            // dateNgayChuyen
            // 
            dateNgayChuyen.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dateNgayChuyen.Format = DateTimePickerFormat.Short;
            dateNgayChuyen.Location = new Point(32, 263);
            dateNgayChuyen.Name = "dateNgayChuyen";
            dateNgayChuyen.Size = new Size(188, 31);
            dateNgayChuyen.TabIndex = 11;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.White;
            btnHuy.BorderThickness = 0;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI Semibold", 10.5F);
            btnHuy.ForeColor = Color.Black;
            btnHuy.Location = new Point(275, 261);
            btnHuy.Name = "btnHuy";
            btnHuy.Padding = new Padding(10, 6, 10, 6);
            btnHuy.Size = new Size(94, 41);
            btnHuy.TabIndex = 14;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            // 
            // btnTaoPhieuChuyen
            // 
            btnTaoPhieuChuyen.BackColor = Color.Black;
            btnTaoPhieuChuyen.BorderThickness = 0;
            btnTaoPhieuChuyen.FlatAppearance.BorderSize = 0;
            btnTaoPhieuChuyen.FlatStyle = FlatStyle.Flat;
            btnTaoPhieuChuyen.Font = new Font("Segoe UI Semibold", 10.5F);
            btnTaoPhieuChuyen.ForeColor = Color.White;
            btnTaoPhieuChuyen.Location = new Point(390, 263);
            btnTaoPhieuChuyen.Name = "btnTaoPhieuChuyen";
            btnTaoPhieuChuyen.Padding = new Padding(10, 6, 10, 6);
            btnTaoPhieuChuyen.Size = new Size(194, 41);
            btnTaoPhieuChuyen.TabIndex = 14;
            btnTaoPhieuChuyen.Text = "Tạo Phiếu Chuyển";
            btnTaoPhieuChuyen.UseVisualStyleBackColor = false;
            // 
            // cbbNguyenLieu
            // 
            cbbNguyenLieu.DrawMode = DrawMode.OwnerDrawFixed;
            cbbNguyenLieu.FormattingEnabled = true;
            cbbNguyenLieu.IntegralHeight = false;
            cbbNguyenLieu.ItemHeight = 26;
            cbbNguyenLieu.Location = new Point(12, 191);
            cbbNguyenLieu.Name = "cbbNguyenLieu";
            cbbNguyenLieu.Size = new Size(406, 32);
            cbbNguyenLieu.TabIndex = 16;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 165);
            label6.Name = "label6";
            label6.Size = new Size(93, 20);
            label6.TabIndex = 15;
            label6.Text = "Nguyên liệu";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(424, 162);
            label7.Name = "label7";
            label7.Size = new Size(71, 20);
            label7.TabIndex = 17;
            label7.Text = "Số lượng";
            // 
            // txtSoLuong
            // 
            txtSoLuong.BackColor = Color.White;
            txtSoLuong.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtSoLuong.ForeColor = Color.Black;
            txtSoLuong.Location = new Point(441, 185);
            txtSoLuong.Name = "txtSoLuong";
            txtSoLuong.Padding = new Padding(10, 8, 10, 8);
            txtSoLuong.Size = new Size(177, 44);
            txtSoLuong.TabIndex = 18;
            // 
            // Frm_ChuyenKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(630, 322);
            Controls.Add(txtSoLuong);
            Controls.Add(label7);
            Controls.Add(cbbNguyenLieu);
            Controls.Add(label6);
            Controls.Add(btnTaoPhieuChuyen);
            Controls.Add(btnHuy);
            Controls.Add(dateNgayChuyen);
            Controls.Add(cbbDenKho);
            Controls.Add(cbbTuKho);
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
        private UiControls.BorderComboBox cbbTuKho;
        private UiControls.BorderComboBox cbbDenKho;
        private Label label3;
        private Label label4;
        private DateTimePicker dateNgayChuyen;
        private Controls.RoundedButton btnHuy;
        private Controls.RoundedButton btnTaoPhieuChuyen;
        private UiControls.BorderComboBox cbbNguyenLieu;
        private Label label6;
        private Label label7;
        private Controls.RoundedTextBox txtSoLuong;
    }
}