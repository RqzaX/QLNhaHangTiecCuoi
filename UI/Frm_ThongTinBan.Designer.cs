namespace UI
{
    partial class Frm_ThongTinBan
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
            lblTitle = new Label();
            lblSubtitle = new Label();
            label1 = new Label();
            txtSoBan = new TextBox();
            label2 = new Label();
            txtSucChua = new TextBox();
            label3 = new Label();
            cbbKhuVuc = new ComboBox();
            label4 = new Label();
            cbbTrangThai = new ComboBox();
            btnSua = new UI.Controls.RoundedButton();
            btnXoa = new UI.Controls.RoundedButton();
            btnDong = new UI.Controls.RoundedButton();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
            lblTitle.Location = new Point(30, 30);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(165, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Chi tiết bàn";
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(107, 114, 128);
            lblSubtitle.Location = new Point(30, 70);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(197, 23);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Thông tin chi tiết về bàn";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(31, 41, 55);
            label1.Location = new Point(30, 130);
            label1.Name = "label1";
            label1.Size = new Size(78, 23);
            label1.TabIndex = 2;
            label1.Text = "Số bàn *";
            // 
            // txtSoBan
            // 
            txtSoBan.Font = new Font("Segoe UI", 10F);
            txtSoBan.Location = new Point(30, 160);
            txtSoBan.Margin = new Padding(3, 4, 3, 4);
            txtSoBan.Name = "txtSoBan";
            txtSoBan.ReadOnly = true;
            txtSoBan.Size = new Size(250, 30);
            txtSoBan.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(31, 41, 55);
            label2.Location = new Point(320, 130);
            label2.Name = "label2";
            label2.Size = new Size(95, 23);
            label2.TabIndex = 4;
            label2.Text = "Sức chứa *";
            // 
            // txtSucChua
            // 
            txtSucChua.Font = new Font("Segoe UI", 10F);
            txtSucChua.Location = new Point(320, 160);
            txtSucChua.Margin = new Padding(3, 4, 3, 4);
            txtSucChua.Name = "txtSucChua";
            txtSucChua.ReadOnly = true;
            txtSucChua.Size = new Size(250, 30);
            txtSucChua.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(31, 41, 55);
            label3.Location = new Point(30, 220);
            label3.Name = "label3";
            label3.Size = new Size(87, 23);
            label3.TabIndex = 6;
            label3.Text = "Khu vực *";
            // 
            // cbbKhuVuc
            // 
            cbbKhuVuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbKhuVuc.Enabled = false;
            cbbKhuVuc.Font = new Font("Segoe UI", 10F);
            cbbKhuVuc.FormattingEnabled = true;
            cbbKhuVuc.Location = new Point(30, 250);
            cbbKhuVuc.Margin = new Padding(3, 4, 3, 4);
            cbbKhuVuc.Name = "cbbKhuVuc";
            cbbKhuVuc.Size = new Size(250, 31);
            cbbKhuVuc.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(31, 41, 55);
            label4.Location = new Point(320, 220);
            label4.Name = "label4";
            label4.Size = new Size(92, 23);
            label4.TabIndex = 8;
            label4.Text = "Trạng thái";
            // 
            // cbbTrangThai
            // 
            cbbTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbTrangThai.Enabled = false;
            cbbTrangThai.Font = new Font("Segoe UI", 10F);
            cbbTrangThai.FormattingEnabled = true;
            cbbTrangThai.Location = new Point(320, 250);
            cbbTrangThai.Margin = new Padding(3, 4, 3, 4);
            cbbTrangThai.Name = "cbbTrangThai";
            cbbTrangThai.Size = new Size(250, 31);
            cbbTrangThai.TabIndex = 9;
            // 
            // btnSua
            // 
            btnSua.BackColor = Color.FromArgb(59, 130, 246);
            btnSua.BorderThickness = 0;
            btnSua.CornerRadius = 8;
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.HoverBackColor = Color.FromArgb(37, 99, 235);
            btnSua.Location = new Point(230, 316);
            btnSua.Margin = new Padding(3, 4, 3, 4);
            btnSua.Name = "btnSua";
            btnSua.Padding = new Padding(14, 11, 14, 11);
            btnSua.PressedBackColor = Color.FromArgb(29, 78, 216);
            btnSua.Size = new Size(150, 54);
            btnSua.TabIndex = 10;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = false;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.FromArgb(239, 68, 68);
            btnXoa.BorderThickness = 0;
            btnXoa.CornerRadius = 8;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnXoa.ForeColor = Color.White;
            btnXoa.HoverBackColor = Color.FromArgb(220, 38, 38);
            btnXoa.Location = new Point(55, 316);
            btnXoa.Margin = new Padding(3, 4, 3, 4);
            btnXoa.Name = "btnXoa";
            btnXoa.Padding = new Padding(14, 11, 14, 11);
            btnXoa.PressedBackColor = Color.FromArgb(185, 28, 28);
            btnXoa.Size = new Size(150, 54);
            btnXoa.TabIndex = 12;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnDong
            // 
            btnDong.BackColor = Color.FromArgb(107, 114, 128);
            btnDong.BorderThickness = 0;
            btnDong.CornerRadius = 8;
            btnDong.FlatStyle = FlatStyle.Flat;
            btnDong.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDong.ForeColor = Color.White;
            btnDong.HoverBackColor = Color.FromArgb(75, 85, 99);
            btnDong.Location = new Point(395, 316);
            btnDong.Margin = new Padding(3, 4, 3, 4);
            btnDong.Name = "btnDong";
            btnDong.Padding = new Padding(14, 11, 14, 11);
            btnDong.PressedBackColor = Color.FromArgb(55, 65, 81);
            btnDong.Size = new Size(150, 54);
            btnDong.TabIndex = 11;
            btnDong.Text = "Đóng";
            btnDong.UseVisualStyleBackColor = false;
            btnDong.Click += btnDong_Click;
            // 
            // Frm_ThongTinBan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 400);
            Controls.Add(btnDong);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(cbbTrangThai);
            Controls.Add(label4);
            Controls.Add(cbbKhuVuc);
            Controls.Add(label3);
            Controls.Add(txtSucChua);
            Controls.Add(label2);
            Controls.Add(txtSoBan);
            Controls.Add(label1);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Frm_ThongTinBan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết bàn";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSoBan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSucChua;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbbKhuVuc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbTrangThai;
        private UI.Controls.RoundedButton btnSua;
        private UI.Controls.RoundedButton btnXoa;
        private UI.Controls.RoundedButton btnDong;
    }
}
