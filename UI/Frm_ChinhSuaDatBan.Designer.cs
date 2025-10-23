namespace UI
{
    partial class Frm_ChinhSuaDatBan
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
            panelMain = new Panel();
            groupBoxThongTin = new GroupBox();
            txtGhiChu = new TextBox();
            label7 = new Label();
            cboGioDat = new ComboBox();
            label8 = new Label();
            txtSucChuaBan = new TextBox();
            label9 = new Label();
            dtpNgayDat = new DateTimePicker();
            label6 = new Label();
            cboBan = new ComboBox();
            label5 = new Label();
            cboKhuVuc = new ComboBox();
            label4 = new Label();
            txtSoKhach = new TextBox();
            label3 = new Label();
            txtSoDienThoai = new TextBox();
            label2 = new Label();
            txtTenKhachHang = new TextBox();
            label1 = new Label();
            txtMaDatBan = new TextBox();
            lblMaDatBan = new Label();
            panelButtons = new Panel();
            btnHuy = new Button();
            btnLuu = new Button();
            panelMain.SuspendLayout();
            groupBoxThongTin.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(groupBoxThongTin);
            panelMain.Controls.Add(panelButtons);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(0, 0);
            panelMain.Margin = new Padding(4, 3, 4, 3);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(583, 614);
            panelMain.TabIndex = 0;
            // 
            // groupBoxThongTin
            // 
            groupBoxThongTin.Controls.Add(txtGhiChu);
            groupBoxThongTin.Controls.Add(label7);
            groupBoxThongTin.Controls.Add(cboGioDat);
            groupBoxThongTin.Controls.Add(label8);
            groupBoxThongTin.Controls.Add(txtSucChuaBan);
            groupBoxThongTin.Controls.Add(label9);
            groupBoxThongTin.Controls.Add(dtpNgayDat);
            groupBoxThongTin.Controls.Add(label6);
            groupBoxThongTin.Controls.Add(cboBan);
            groupBoxThongTin.Controls.Add(label5);
            groupBoxThongTin.Controls.Add(cboKhuVuc);
            groupBoxThongTin.Controls.Add(label4);
            groupBoxThongTin.Controls.Add(txtSoKhach);
            groupBoxThongTin.Controls.Add(label3);
            groupBoxThongTin.Controls.Add(txtSoDienThoai);
            groupBoxThongTin.Controls.Add(label2);
            groupBoxThongTin.Controls.Add(txtTenKhachHang);
            groupBoxThongTin.Controls.Add(label1);
            groupBoxThongTin.Controls.Add(txtMaDatBan);
            groupBoxThongTin.Controls.Add(lblMaDatBan);
            groupBoxThongTin.Dock = DockStyle.Fill;
            groupBoxThongTin.Font = new Font("Segoe UI", 10F);
            groupBoxThongTin.Location = new Point(0, 0);
            groupBoxThongTin.Margin = new Padding(4, 3, 4, 3);
            groupBoxThongTin.Name = "groupBoxThongTin";
            groupBoxThongTin.Padding = new Padding(4, 3, 4, 3);
            groupBoxThongTin.Size = new Size(583, 545);
            groupBoxThongTin.TabIndex = 0;
            groupBoxThongTin.TabStop = false;
            groupBoxThongTin.Text = "Thông tin đặt bàn";
            // 
            // txtGhiChu
            // 
            txtGhiChu.Font = new Font("Segoe UI", 10F);
            txtGhiChu.Location = new Point(175, 467);
            txtGhiChu.Margin = new Padding(4, 3, 4, 3);
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.ScrollBars = ScrollBars.Vertical;
            txtGhiChu.Size = new Size(349, 69);
            txtGhiChu.TabIndex = 21;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.Location = new Point(61, 470);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(59, 19);
            label7.TabIndex = 20;
            label7.Text = "Ghi chú:";
            // 
            // cboGioDat
            // 
            cboGioDat.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGioDat.Font = new Font("Segoe UI", 10F);
            cboGioDat.FormattingEnabled = true;
            cboGioDat.Location = new Point(175, 426);
            cboGioDat.Margin = new Padding(4, 3, 4, 3);
            cboGioDat.Name = "cboGioDat";
            cboGioDat.Size = new Size(233, 25);
            cboGioDat.TabIndex = 19;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F);
            label8.Location = new Point(60, 432);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(57, 19);
            label8.TabIndex = 18;
            label8.Text = "Giờ đặt:";
            // 
            // txtSucChuaBan
            // 
            txtSucChuaBan.Font = new Font("Segoe UI", 10F);
            txtSucChuaBan.Location = new Point(175, 330);
            txtSucChuaBan.Margin = new Padding(4, 3, 4, 3);
            txtSucChuaBan.Name = "txtSucChuaBan";
            txtSucChuaBan.ReadOnly = true;
            txtSucChuaBan.Size = new Size(233, 25);
            txtSucChuaBan.TabIndex = 17;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.Location = new Point(59, 333);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(93, 19);
            label9.TabIndex = 16;
            label9.Text = "Sức chứa bàn:";
            // 
            // dtpNgayDat
            // 
            dtpNgayDat.Font = new Font("Segoe UI", 10F);
            dtpNgayDat.Format = DateTimePickerFormat.Custom;
            dtpNgayDat.Location = new Point(175, 380);
            dtpNgayDat.Margin = new Padding(4, 3, 4, 3);
            dtpNgayDat.Name = "dtpNgayDat";
            dtpNgayDat.Size = new Size(233, 25);
            dtpNgayDat.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(59, 386);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(68, 19);
            label6.TabIndex = 14;
            label6.Text = "Ngày đặt:";
            // 
            // cboBan
            // 
            cboBan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBan.Font = new Font("Segoe UI", 10F);
            cboBan.FormattingEnabled = true;
            cboBan.Location = new Point(175, 299);
            cboBan.Margin = new Padding(4, 3, 4, 3);
            cboBan.Name = "cboBan";
            cboBan.Size = new Size(233, 25);
            cboBan.TabIndex = 13;
            cboBan.SelectedIndexChanged += cboBan_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.Location = new Point(59, 302);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(35, 19);
            label5.TabIndex = 12;
            label5.Text = "Bàn:";
            // 
            // cboKhuVuc
            // 
            cboKhuVuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhuVuc.Font = new Font("Segoe UI", 10F);
            cboKhuVuc.FormattingEnabled = true;
            cboKhuVuc.Location = new Point(175, 240);
            cboKhuVuc.Margin = new Padding(4, 3, 4, 3);
            cboKhuVuc.Name = "cboKhuVuc";
            cboKhuVuc.Size = new Size(233, 25);
            cboKhuVuc.TabIndex = 11;
            cboKhuVuc.SelectedIndexChanged += cboKhuVuc_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(59, 246);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(61, 19);
            label4.TabIndex = 10;
            label4.Text = "Khu vực:";
            // 
            // txtSoKhach
            // 
            txtSoKhach.Font = new Font("Segoe UI", 10F);
            txtSoKhach.Location = new Point(175, 192);
            txtSoKhach.Margin = new Padding(4, 3, 4, 3);
            txtSoKhach.Name = "txtSoKhach";
            txtSoKhach.Size = new Size(116, 25);
            txtSoKhach.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(59, 192);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(67, 19);
            label3.TabIndex = 8;
            label3.Text = "Số khách:";
            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.Font = new Font("Segoe UI", 10F);
            txtSoDienThoai.Location = new Point(175, 145);
            txtSoDienThoai.Margin = new Padding(4, 3, 4, 3);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(233, 25);
            txtSoDienThoai.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(59, 145);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(92, 19);
            label2.TabIndex = 6;
            label2.Text = "Số điện thoại:";
            // 
            // txtTenKhachHang
            // 
            txtTenKhachHang.Font = new Font("Segoe UI", 10F);
            txtTenKhachHang.Location = new Point(175, 98);
            txtTenKhachHang.Margin = new Padding(4, 3, 4, 3);
            txtTenKhachHang.Name = "txtTenKhachHang";
            txtTenKhachHang.Size = new Size(233, 25);
            txtTenKhachHang.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F);
            label1.Location = new Point(58, 98);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(108, 19);
            label1.TabIndex = 4;
            label1.Text = "Tên khách hàng:";
            // 
            // txtMaDatBan
            // 
            txtMaDatBan.Font = new Font("Segoe UI", 10F);
            txtMaDatBan.Location = new Point(175, 58);
            txtMaDatBan.Margin = new Padding(4, 3, 4, 3);
            txtMaDatBan.Name = "txtMaDatBan";
            txtMaDatBan.ReadOnly = true;
            txtMaDatBan.Size = new Size(116, 25);
            txtMaDatBan.TabIndex = 1;
            // 
            // lblMaDatBan
            // 
            lblMaDatBan.AutoSize = true;
            lblMaDatBan.Font = new Font("Segoe UI", 10F);
            lblMaDatBan.Location = new Point(58, 58);
            lblMaDatBan.Margin = new Padding(4, 0, 4, 0);
            lblMaDatBan.Name = "lblMaDatBan";
            lblMaDatBan.Size = new Size(83, 19);
            lblMaDatBan.TabIndex = 0;
            lblMaDatBan.Text = "Mã đặt bàn:";
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(btnHuy);
            panelButtons.Controls.Add(btnLuu);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 545);
            panelButtons.Margin = new Padding(4, 3, 4, 3);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(583, 69);
            panelButtons.TabIndex = 1;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(239, 68, 68);
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(330, 17);
            btnHuy.Margin = new Padding(4, 3, 4, 3);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(117, 40);
            btnHuy.TabIndex = 1;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(34, 197, 94);
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(129, 17);
            btnLuu.Margin = new Padding(4, 3, 4, 3);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(117, 40);
            btnLuu.TabIndex = 0;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = false;
            btnLuu.Click += btnLuu_Click;
            // 
            // Frm_ChinhSuaDatBan
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(583, 614);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Frm_ChinhSuaDatBan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chỉnh sửa đặt bàn";
            panelMain.ResumeLayout(false);
            groupBoxThongTin.ResumeLayout(false);
            groupBoxThongTin.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox groupBoxThongTin;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboGioDat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSucChuaBan;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpNgayDat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboBan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboKhuVuc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSoKhach;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTenKhachHang;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaDatBan;
        private System.Windows.Forms.Label lblMaDatBan;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
    }
}
