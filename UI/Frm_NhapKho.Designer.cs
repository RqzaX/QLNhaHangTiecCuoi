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
            label3 = new Label();
            dateNgayNhap = new DateTimePicker();
            btnHuy = new UI.Controls.RoundedButton();
            btnTaoPhieuNhap = new UI.Controls.RoundedButton();
            cbbChiNhanh = new UiControls.BorderComboBox();
            label6 = new Label();
            cbbTenMon = new UiControls.BorderComboBox();
            label7 = new Label();
            txtSoLuong = new UI.Controls.RoundedTextBox();
            btnThemNL = new UI.Controls.RoundedButton();
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
            label8.Location = new Point(13, 153);
            label8.Name = "label8";
            label8.Size = new Size(70, 20);
            label8.TabIndex = 5;
            label8.Text = "Tên Món";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(13, 217);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 5;
            label3.Text = "Ngày Nhập";
            // 
            // dateNgayNhap
            // 
            dateNgayNhap.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dateNgayNhap.Format = DateTimePickerFormat.Short;
            dateNgayNhap.Location = new Point(46, 240);
            dateNgayNhap.Name = "dateNgayNhap";
            dateNgayNhap.Size = new Size(250, 31);
            dateNgayNhap.TabIndex = 8;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.White;
            btnHuy.BorderThickness = 0;
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI Semibold", 10.5F);
            btnHuy.ForeColor = Color.Black;
            btnHuy.HoverBackColor = Color.White;
            btnHuy.Location = new Point(300, 297);
            btnHuy.Name = "btnHuy";
            btnHuy.Padding = new Padding(10, 6, 10, 6);
            btnHuy.PressedBackColor = Color.White;
            btnHuy.Size = new Size(94, 47);
            btnHuy.TabIndex = 9;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            // 
            // btnTaoPhieuNhap
            // 
            btnTaoPhieuNhap.BackColor = Color.Black;
            btnTaoPhieuNhap.BorderThickness = 0;
            btnTaoPhieuNhap.FlatAppearance.BorderSize = 0;
            btnTaoPhieuNhap.FlatStyle = FlatStyle.Flat;
            btnTaoPhieuNhap.Font = new Font("Segoe UI Semibold", 10.5F);
            btnTaoPhieuNhap.ForeColor = Color.White;
            btnTaoPhieuNhap.HoverBackColor = Color.White;
            btnTaoPhieuNhap.Location = new Point(400, 297);
            btnTaoPhieuNhap.Name = "btnTaoPhieuNhap";
            btnTaoPhieuNhap.Padding = new Padding(10, 6, 10, 6);
            btnTaoPhieuNhap.PressedBackColor = Color.White;
            btnTaoPhieuNhap.Size = new Size(187, 47);
            btnTaoPhieuNhap.TabIndex = 9;
            btnTaoPhieuNhap.Text = "Tạo Phiếu Nhập";
            btnTaoPhieuNhap.UseVisualStyleBackColor = false;
            // 
            // cbbChiNhanh
            // 
            cbbChiNhanh.DrawMode = DrawMode.OwnerDrawFixed;
            cbbChiNhanh.FormattingEnabled = true;
            cbbChiNhanh.IntegralHeight = false;
            cbbChiNhanh.ItemHeight = 26;
            cbbChiNhanh.Location = new Point(12, 102);
            cbbChiNhanh.Name = "cbbChiNhanh";
            cbbChiNhanh.Size = new Size(600, 32);
            cbbChiNhanh.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 79);
            label6.Name = "label6";
            label6.Size = new Size(79, 20);
            label6.TabIndex = 10;
            label6.Text = "Chi nhánh";
            // 
            // cbbTenMon
            // 
            cbbTenMon.DrawMode = DrawMode.OwnerDrawFixed;
            cbbTenMon.FormattingEnabled = true;
            cbbTenMon.IntegralHeight = false;
            cbbTenMon.ItemHeight = 26;
            cbbTenMon.Location = new Point(13, 176);
            cbbTenMon.Name = "cbbTenMon";
            cbbTenMon.Size = new Size(250, 32);
            cbbTenMon.TabIndex = 12;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(375, 153);
            label7.Name = "label7";
            label7.Size = new Size(71, 20);
            label7.TabIndex = 13;
            label7.Text = "Số lượng";
            // 
            // txtSoLuong
            // 
            txtSoLuong.BackColor = Color.White;
            txtSoLuong.Font = new Font("Segoe UI", 10F);
            txtSoLuong.ForeColor = Color.Black;
            txtSoLuong.Location = new Point(375, 176);
            txtSoLuong.Name = "txtSoLuong";
            txtSoLuong.Padding = new Padding(10, 8, 10, 8);
            txtSoLuong.Size = new Size(195, 32);
            txtSoLuong.TabIndex = 14;
            // 
            // btnThemNL
            // 
            btnThemNL.BackColor = Color.FromArgb(0, 120, 215);
            btnThemNL.BorderThickness = 0;
            btnThemNL.FlatAppearance.BorderSize = 0;
            btnThemNL.FlatStyle = FlatStyle.Flat;
            btnThemNL.Font = new Font("Segoe UI Semibold", 9F);
            btnThemNL.ForeColor = Color.White;
            btnThemNL.HoverBackColor = Color.FromArgb(0, 100, 180);
            btnThemNL.Location = new Point(269, 176);
            btnThemNL.Name = "btnThemNL";
            btnThemNL.Padding = new Padding(8, 4, 8, 4);
            btnThemNL.PressedBackColor = Color.FromArgb(0, 80, 150);
            btnThemNL.Size = new Size(100, 32);
            btnThemNL.TabIndex = 15;
            btnThemNL.Text = "+ Thêm mới";
            btnThemNL.UseVisualStyleBackColor = false;
            // 
            // Frm_NhapKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(620, 355);
            Controls.Add(btnThemNL);
            Controls.Add(txtSoLuong);
            Controls.Add(label7);
            Controls.Add(cbbTenMon);
            Controls.Add(cbbChiNhanh);
            Controls.Add(label6);
            Controls.Add(btnTaoPhieuNhap);
            Controls.Add(btnHuy);
            Controls.Add(dateNgayNhap);
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
        private Label label3;
        private DateTimePicker dateNgayNhap;
        private Controls.RoundedButton btnHuy;
        private Controls.RoundedButton btnTaoPhieuNhap;
        private UiControls.BorderComboBox cbbChiNhanh;
        private Label label6;
        private UiControls.BorderComboBox cbbTenMon;
        private Label label7;
        private Controls.RoundedTextBox txtSoLuong;
        private Controls.RoundedButton btnThemNL;
    }
}