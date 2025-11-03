using Guna.UI2.WinForms;

namespace UI
{
    partial class Frm_TaoDatBan
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
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtSoDienThoai = new Guna2TextBox();
            txtTenKhachHang = new Guna2TextBox();
            label5 = new Label();
            txtEmail = new Guna2TextBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            dateNgay = new Guna2DateTimePicker();
            btnExit = new Guna2Button();
            label9 = new Label();
            timeGio = new UI.Controls.TimePickerExStyled();
            txtSoKhach = new Guna2TextBox();
            label10 = new Label();
            label11 = new Label();
            cbbKhuVuc = new Guna2ComboBox();
            cbbSoBan = new Guna2ComboBox();
            label12 = new Label();
            label14 = new Label();
            txtGhiChu = new Guna2TextBox();
            btnTaoDatBan = new Guna2Button();
            btnHuy = new Guna2Button();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 35);
            label2.Name = "label2";
            label2.Size = new Size(313, 19);
            label2.TabIndex = 13;
            label2.Text = "Nhập đầy đủ thông tin đặt bàn cho khách hàng";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(163, 27);
            label1.TabIndex = 12;
            label1.Text = "Tạo đặt bàn mới";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(43, 75);
            label3.Name = "label3";
            label3.Size = new Size(148, 19);
            label3.TabIndex = 14;
            label3.Text = "Thông tin khách hàng";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 94);
            label4.Name = "label4";
            label4.Size = new Size(115, 21);
            label4.TabIndex = 15;
            label4.Text = "Số điện thoại *";
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.BorderRadius = 18;
            txtSoDienThoai.DefaultText = "";
            txtSoDienThoai.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSoDienThoai.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSoDienThoai.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSoDienThoai.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSoDienThoai.FillColor = Color.White;
            txtSoDienThoai.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoDienThoai.Font = new Font("Segoe UI", 10F);
            txtSoDienThoai.ForeColor = Color.Black;
            txtSoDienThoai.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoDienThoai.Location = new Point(25, 118);
            txtSoDienThoai.Margin = new Padding(10, 8, 10, 8);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.PasswordChar = '\0';
            txtSoDienThoai.PlaceholderText = "Nhập SĐT để tìm khách hàng";
            txtSoDienThoai.SelectedText = "";
            txtSoDienThoai.Size = new Size(239, 36);
            txtSoDienThoai.TabIndex = 16;
            // 
            // txtTenKhachHang
            // 
            txtTenKhachHang.BorderRadius = 18;
            txtTenKhachHang.DefaultText = "";
            txtTenKhachHang.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtTenKhachHang.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtTenKhachHang.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtTenKhachHang.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtTenKhachHang.FillColor = Color.White;
            txtTenKhachHang.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTenKhachHang.Font = new Font("Segoe UI", 10F);
            txtTenKhachHang.ForeColor = Color.Black;
            txtTenKhachHang.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTenKhachHang.Location = new Point(282, 118);
            txtTenKhachHang.Margin = new Padding(10, 8, 10, 8);
            txtTenKhachHang.Name = "txtTenKhachHang";
            txtTenKhachHang.PasswordChar = '\0';
            txtTenKhachHang.PlaceholderText = "Nhập tên khách hàng";
            txtTenKhachHang.SelectedText = "";
            txtTenKhachHang.Size = new Size(289, 36);
            txtTenKhachHang.TabIndex = 18;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(273, 94);
            label5.Name = "label5";
            label5.Size = new Size(130, 21);
            label5.TabIndex = 17;
            label5.Text = "Tên khách hàng *";
            // 
            // txtEmail
            // 
            txtEmail.BorderRadius = 18;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FillColor = Color.White;
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.ForeColor = Color.Black;
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(591, 118);
            txtEmail.Margin = new Padding(10, 8, 10, 8);
            txtEmail.Name = "txtEmail";
            txtEmail.PasswordChar = '\0';
            txtEmail.PlaceholderText = "email@example.com";
            txtEmail.SelectedText = "";
            txtEmail.Size = new Size(281, 36);
            txtEmail.TabIndex = 20;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(577, 94);
            label6.Name = "label6";
            label6.Size = new Size(48, 21);
            label6.TabIndex = 19;
            label6.Text = "Email";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(43, 177);
            label7.Name = "label7";
            label7.Size = new Size(124, 19);
            label7.TabIndex = 21;
            label7.Text = "Thông tin đặt bàn";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(19, 200);
            label8.Name = "label8";
            label8.Size = new Size(57, 21);
            label8.TabIndex = 22;
            label8.Text = "Ngày *";
            // 
            // dateNgay
            // 
            dateNgay.BorderRadius = 18;
            dateNgay.Checked = true;
            dateNgay.FillColor = Color.White;
            dateNgay.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold);
            dateNgay.Format = DateTimePickerFormat.Short;
            dateNgay.Location = new Point(32, 229);
            dateNgay.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dateNgay.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dateNgay.Name = "dateNgay";
            dateNgay.Size = new Size(171, 36);
            dateNgay.TabIndex = 24;
            dateNgay.Value = new DateTime(2025, 11, 1, 22, 27, 54, 0);
            // 
            // btnExit
            // 
            btnExit.Animated = true;
            btnExit.BorderRadius = 10;
            btnExit.DisabledState.BorderColor = Color.DarkGray;
            btnExit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExit.FillColor = Color.Transparent;
            btnExit.Font = new Font("Calibri", 16.2F, FontStyle.Bold);
            btnExit.ForeColor = Color.Black;
            btnExit.Location = new Point(830, 8);
            btnExit.Margin = new Padding(3, 2, 3, 2);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(55, 43);
            btnExit.TabIndex = 25;
            btnExit.Text = "✖";
            btnExit.Click += btnExit_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(259, 200);
            label9.Name = "label9";
            label9.Size = new Size(48, 21);
            label9.TabIndex = 26;
            label9.Text = "Giờ *";
            // 
            // timeGio
            // 
            timeGio.BackColor = Color.Transparent;
            timeGio.EndTime = TimeSpan.Parse("22:00:00");
            timeGio.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            timeGio.IntervalMinutes = 30;
            timeGio.Location = new Point(259, 224);
            timeGio.MinimumSize = new Size(120, 34);
            timeGio.Name = "timeGio";
            timeGio.Placeholder = "Chọn giờ";
            timeGio.SelectedTime = null;
            timeGio.Size = new Size(150, 38);
            timeGio.StartTime = TimeSpan.Parse("10:00:00");
            timeGio.TabIndex = 27;
            // 
            // txtSoKhach
            // 
            txtSoKhach.BorderRadius = 18;
            txtSoKhach.DefaultText = "";
            txtSoKhach.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSoKhach.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSoKhach.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSoKhach.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSoKhach.FillColor = Color.White;
            txtSoKhach.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoKhach.Font = new Font("Segoe UI", 10F);
            txtSoKhach.ForeColor = Color.Black;
            txtSoKhach.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoKhach.Location = new Point(435, 224);
            txtSoKhach.Margin = new Padding(10, 8, 10, 8);
            txtSoKhach.Name = "txtSoKhach";
            txtSoKhach.PasswordChar = '\0';
            txtSoKhach.PlaceholderText = "0";
            txtSoKhach.SelectedText = "";
            txtSoKhach.Size = new Size(123, 36);
            txtSoKhach.TabIndex = 29;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(426, 200);
            label10.Name = "label10";
            label10.Size = new Size(84, 21);
            label10.TabIndex = 28;
            label10.Text = "Số khách *";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(12, 285);
            label11.Name = "label11";
            label11.Size = new Size(79, 21);
            label11.TabIndex = 30;
            label11.Text = "Khu vực *";
            // 
            // cbbKhuVuc
            // 
            cbbKhuVuc.BackColor = Color.Transparent;
            cbbKhuVuc.BorderRadius = 18;
            cbbKhuVuc.DrawMode = DrawMode.OwnerDrawFixed;
            cbbKhuVuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbKhuVuc.FocusedColor = Color.FromArgb(94, 148, 255);
            cbbKhuVuc.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbbKhuVuc.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold);
            cbbKhuVuc.ForeColor = Color.Black;
            cbbKhuVuc.ItemHeight = 30;
            cbbKhuVuc.Location = new Point(25, 309);
            cbbKhuVuc.MaxDropDownItems = 10;
            cbbKhuVuc.Name = "cbbKhuVuc";
            cbbKhuVuc.Size = new Size(249, 36);
            cbbKhuVuc.TabIndex = 31;
            // 
            // cbbSoBan
            // 
            cbbSoBan.BackColor = Color.Transparent;
            cbbSoBan.BorderRadius = 18;
            cbbSoBan.DrawMode = DrawMode.OwnerDrawFixed;
            cbbSoBan.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbSoBan.FocusedColor = Color.FromArgb(94, 148, 255);
            cbbSoBan.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbbSoBan.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold);
            cbbSoBan.ForeColor = Color.Black;
            cbbSoBan.ItemHeight = 30;
            cbbSoBan.Location = new Point(308, 309);
            cbbSoBan.Name = "cbbSoBan";
            cbbSoBan.Size = new Size(249, 36);
            cbbSoBan.TabIndex = 33;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.Location = new Point(295, 285);
            label12.Name = "label12";
            label12.Size = new Size(49, 21);
            label12.TabIndex = 32;
            label12.Text = "Bàn *";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(12, 359);
            label14.Name = "label14";
            label14.Size = new Size(191, 21);
            label14.TabIndex = 36;
            label14.Text = "Ghi chú - Yêu cầu đặc biệt";
            // 
            // txtGhiChu
            // 
            txtGhiChu.BorderRadius = 18;
            txtGhiChu.DefaultText = "";
            txtGhiChu.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtGhiChu.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtGhiChu.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtGhiChu.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtGhiChu.FillColor = Color.White;
            txtGhiChu.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtGhiChu.Font = new Font("Segoe UI", 10F);
            txtGhiChu.ForeColor = Color.Black;
            txtGhiChu.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtGhiChu.Location = new Point(26, 383);
            txtGhiChu.Margin = new Padding(10, 8, 10, 8);
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.PasswordChar = '\0';
            txtGhiChu.PlaceholderText = "Ví dụ: Chỗ ngồi gần cửa sổ, có trẻ em, sinh nhật, dị ứng thực phẩm,...";
            txtGhiChu.SelectedText = "";
            txtGhiChu.Size = new Size(842, 108);
            txtGhiChu.TabIndex = 37;
            // 
            // btnTaoDatBan
            // 
            btnTaoDatBan.Animated = true;
            btnTaoDatBan.BorderRadius = 17;
            btnTaoDatBan.BorderThickness = 2;
            btnTaoDatBan.DisabledState.BorderColor = Color.DarkGray;
            btnTaoDatBan.DisabledState.CustomBorderColor = Color.DarkGray;
            btnTaoDatBan.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnTaoDatBan.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnTaoDatBan.FillColor = Color.FromArgb(31, 111, 235);
            btnTaoDatBan.Font = new Font("Arial", 12.75F, FontStyle.Bold);
            btnTaoDatBan.ForeColor = Color.White;
            btnTaoDatBan.Location = new Point(711, 503);
            btnTaoDatBan.Name = "btnTaoDatBan";
            btnTaoDatBan.ShadowDecoration.BorderRadius = 17;
            btnTaoDatBan.ShadowDecoration.Depth = 3;
            btnTaoDatBan.ShadowDecoration.Enabled = true;
            btnTaoDatBan.Size = new Size(150, 38);
            btnTaoDatBan.TabIndex = 38;
            btnTaoDatBan.Text = "+ Tạo đặt bàn";
            // 
            // btnHuy
            // 
            btnHuy.Animated = true;
            btnHuy.BorderRadius = 17;
            btnHuy.BorderThickness = 2;
            btnHuy.DisabledState.BorderColor = Color.DarkGray;
            btnHuy.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHuy.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHuy.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHuy.FillColor = Color.White;
            btnHuy.Font = new Font("Arial", 12.75F, FontStyle.Bold);
            btnHuy.ForeColor = Color.Black;
            btnHuy.HoverState.FillColor = Color.FromArgb(240, 240, 240);
            btnHuy.Location = new Point(631, 503);
            btnHuy.Name = "btnHuy";
            btnHuy.ShadowDecoration.BorderRadius = 17;
            btnHuy.Size = new Size(74, 38);
            btnHuy.TabIndex = 39;
            btnHuy.Text = "Hủy";
            btnHuy.Click += btnHuy_Click;
            // 
            // Frm_TaoDatBan
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(894, 553);
            Controls.Add(btnHuy);
            Controls.Add(btnTaoDatBan);
            Controls.Add(txtGhiChu);
            Controls.Add(label14);
            Controls.Add(cbbSoBan);
            Controls.Add(label12);
            Controls.Add(cbbKhuVuc);
            Controls.Add(label11);
            Controls.Add(txtSoKhach);
            Controls.Add(label10);
            Controls.Add(timeGio);
            Controls.Add(label9);
            Controls.Add(btnExit);
            Controls.Add(dateNgay);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(txtEmail);
            Controls.Add(label6);
            Controls.Add(txtTenKhachHang);
            Controls.Add(label5);
            Controls.Add(txtSoDienThoai);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Frm_TaoDatBan";
            Text = "Frm_TaoDatBan";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Label label3;
        private Label label4;
        private Guna2TextBox txtSoDienThoai;
        private Guna2TextBox txtTenKhachHang;
        private Label label5;
        private Guna2TextBox txtEmail;
        private Label label6;
        private Label label7;
        private Label label8;
        private Guna2DateTimePicker dateNgay;
        private Guna2Button btnExit;
        private Label label9;
        private Controls.TimePickerExStyled timeGio;
        private Guna2TextBox txtSoKhach;
        private Label label10;
        private Label label11;
        private Guna2ComboBox cbbKhuVuc;
        private Guna2ComboBox cbbSoBan;
        private Label label12;
        private Label label14;
        private Guna2TextBox txtGhiChu;
        private Guna2Button btnTaoDatBan;
        private Guna2Button btnHuy;
    }
}