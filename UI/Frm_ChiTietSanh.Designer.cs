namespace UI
{
    partial class Frm_ChiTietSanh
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
            txtTenSanh = new TextBox();
            label2 = new Label();
            txtSucChua = new TextBox();
            label3 = new Label();
            txtPhiThueCb = new TextBox();
            label4 = new Label();
            cbbChiNhanh = new ComboBox();
            btnSua = new UI.Controls.RoundedButton();
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
            lblTitle.Text = "Chi tiết sảnh";
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
            lblSubtitle.Text = "Thông tin chi tiết về sảnh";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(31, 41, 55);
            label1.Location = new Point(30, 130);
            label1.Name = "label1";
            label1.Size = new Size(88, 23);
            label1.TabIndex = 2;
            label1.Text = "Tên sảnh *";
            // 
            // txtTenSanh
            // 
            txtTenSanh.Font = new Font("Segoe UI", 10F);
            txtTenSanh.Location = new Point(30, 160);
            txtTenSanh.Margin = new Padding(3, 4, 3, 4);
            txtTenSanh.Name = "txtTenSanh";
            txtTenSanh.ReadOnly = true;
            txtTenSanh.Size = new Size(250, 30);
            txtTenSanh.TabIndex = 3;
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
            label3.Size = new Size(120, 23);
            label3.TabIndex = 6;
            label3.Text = "Phí thuê cơ bản *";
            // 
            // txtPhiThueCb
            // 
            txtPhiThueCb.Font = new Font("Segoe UI", 10F);
            txtPhiThueCb.Location = new Point(30, 250);
            txtPhiThueCb.Margin = new Padding(3, 4, 3, 4);
            txtPhiThueCb.Name = "txtPhiThueCb";
            txtPhiThueCb.ReadOnly = true;
            txtPhiThueCb.Size = new Size(250, 30);
            txtPhiThueCb.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(31, 41, 55);
            label4.Location = new Point(320, 220);
            label4.Name = "label4";
            label4.Size = new Size(95, 23);
            label4.TabIndex = 8;
            label4.Text = "Chi nhánh";
            // 
            // cbbChiNhanh
            // 
            cbbChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbChiNhanh.Enabled = false;
            cbbChiNhanh.Font = new Font("Segoe UI", 10F);
            cbbChiNhanh.FormattingEnabled = true;
            cbbChiNhanh.Location = new Point(320, 250);
            cbbChiNhanh.Margin = new Padding(3, 4, 3, 4);
            cbbChiNhanh.Name = "cbbChiNhanh";
            cbbChiNhanh.Size = new Size(250, 31);
            cbbChiNhanh.TabIndex = 9;
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
            // Frm_ChiTietSanh
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 400);
            Controls.Add(btnDong);
            Controls.Add(btnSua);
            Controls.Add(cbbChiNhanh);
            Controls.Add(label4);
            Controls.Add(txtPhiThueCb);
            Controls.Add(label3);
            Controls.Add(txtSucChua);
            Controls.Add(label2);
            Controls.Add(txtTenSanh);
            Controls.Add(label1);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Frm_ChiTietSanh";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết sảnh";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTenSanh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSucChua;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPhiThueCb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbChiNhanh;
        private UI.Controls.RoundedButton btnSua;
        private UI.Controls.RoundedButton btnDong;
    }
}

