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
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblThoiGianDat = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblGhiChu = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSoKhach = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSDT = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblKhachHang = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvOrder = new System.Windows.Forms.DataGridView();
            this.btnTiepNhanKhach = new UI.Controls.RoundedButton();
            this.btnOrderThemMon = new UI.Controls.RoundedButton();
            this.btnDong = new UI.Controls.RoundedButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thông tin bàn";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblThoiGianDat);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblGhiChu);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblSoKhach);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblSDT);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblKhachHang);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.groupBox1.Location = new System.Drawing.Point(20, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(15);
            this.groupBox1.Size = new System.Drawing.Size(500, 200);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin đặt bàn";
            // 
            // lblThoiGianDat
            // 
            this.lblThoiGianDat.AutoSize = true;
            this.lblThoiGianDat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblThoiGianDat.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblThoiGianDat.Location = new System.Drawing.Point(120, 160);
            this.lblThoiGianDat.Name = "lblThoiGianDat";
            this.lblThoiGianDat.Size = new System.Drawing.Size(50, 19);
            this.lblThoiGianDat.TabIndex = 9;
            this.lblThoiGianDat.Text = "label6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.label5.Location = new System.Drawing.Point(20, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 19);
            this.label5.TabIndex = 8;
            this.label5.Text = "Thời gian đặt:";
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.AutoSize = true;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblGhiChu.Location = new System.Drawing.Point(120, 130);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(50, 19);
            this.lblGhiChu.TabIndex = 7;
            this.lblGhiChu.Text = "label6";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.label4.Location = new System.Drawing.Point(20, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ghi chú:";
            // 
            // lblSoKhach
            // 
            this.lblSoKhach.AutoSize = true;
            this.lblSoKhach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblSoKhach.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblSoKhach.Location = new System.Drawing.Point(120, 100);
            this.lblSoKhach.Name = "lblSoKhach";
            this.lblSoKhach.Size = new System.Drawing.Size(50, 19);
            this.lblSoKhach.TabIndex = 5;
            this.lblSoKhach.Text = "label6";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "Số khách:";
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblSDT.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblSDT.Location = new System.Drawing.Point(120, 70);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(50, 19);
            this.lblSDT.TabIndex = 3;
            this.lblSDT.Text = "label4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.label2.Location = new System.Drawing.Point(20, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "SĐT:";
            // 
            // lblKhachHang
            // 
            this.lblKhachHang.AutoSize = true;
            this.lblKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblKhachHang.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblKhachHang.Location = new System.Drawing.Point(120, 40);
            this.lblKhachHang.Name = "lblKhachHang";
            this.lblKhachHang.Size = new System.Drawing.Size(50, 19);
            this.lblKhachHang.TabIndex = 1;
            this.lblKhachHang.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Khách hàng:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvOrder);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(31, 41, 55);
            this.groupBox2.Location = new System.Drawing.Point(20, 290);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(15);
            this.groupBox2.Size = new System.Drawing.Size(500, 200);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Món đã order (chỉ xem)";
            // 
            // dgvOrder
            // 
            this.dgvOrder.AllowUserToAddRows = false;
            this.dgvOrder.AllowUserToDeleteRows = false;
            this.dgvOrder.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrder.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrder.EnableHeadersVisualStyles = false;
            this.dgvOrder.GridColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.dgvOrder.Location = new System.Drawing.Point(15, 19);
            this.dgvOrder.Name = "dgvOrder";
            this.dgvOrder.ReadOnly = true;
            this.dgvOrder.RowHeadersVisible = false;
            this.dgvOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrder.Size = new System.Drawing.Size(470, 166);
            this.dgvOrder.TabIndex = 0;
            // 
            // btnTiepNhanKhach
            // 
            this.btnTiepNhanKhach.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.btnTiepNhanKhach.CornerRadius = 8;
            this.btnTiepNhanKhach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTiepNhanKhach.ForeColor = System.Drawing.Color.White;
            this.btnTiepNhanKhach.HoverBackColor = System.Drawing.Color.FromArgb(22, 163, 74);
            this.btnTiepNhanKhach.Location = new System.Drawing.Point(20, 510);
            this.btnTiepNhanKhach.Name = "btnTiepNhanKhach";
            this.btnTiepNhanKhach.PressedBackColor = System.Drawing.Color.FromArgb(21, 128, 61);
            this.btnTiepNhanKhach.Size = new System.Drawing.Size(140, 42);
            this.btnTiepNhanKhach.TabIndex = 3;
            this.btnTiepNhanKhach.Text = "Tiếp nhận khách";
            this.btnTiepNhanKhach.Click += new System.EventHandler(this.btnTiepNhanKhach_Click);
            // 
            // btnOrderThemMon
            // 
            this.btnOrderThemMon.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnOrderThemMon.CornerRadius = 8;
            this.btnOrderThemMon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOrderThemMon.ForeColor = System.Drawing.Color.White;
            this.btnOrderThemMon.HoverBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnOrderThemMon.Location = new System.Drawing.Point(180, 510);
            this.btnOrderThemMon.Name = "btnOrderThemMon";
            this.btnOrderThemMon.PressedBackColor = System.Drawing.Color.FromArgb(29, 78, 216);
            this.btnOrderThemMon.Size = new System.Drawing.Size(140, 42);
            this.btnOrderThemMon.TabIndex = 4;
            this.btnOrderThemMon.Text = "Order thêm món";
            this.btnOrderThemMon.Click += new System.EventHandler(this.btnOrderThemMon_Click);
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.btnDong.CornerRadius = 8;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDong.ForeColor = System.Drawing.Color.White;
            this.btnDong.HoverBackColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.btnDong.Location = new System.Drawing.Point(440, 510);
            this.btnDong.Name = "btnDong";
            this.btnDong.PressedBackColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnDong.Size = new System.Drawing.Size(80, 42);
            this.btnDong.TabIndex = 5;
            this.btnDong.Text = "Đóng";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // Frm_ThongTinBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(249, 250, 251);
            this.ClientSize = new System.Drawing.Size(550, 570);
            this.Controls.Add(this.btnDong);
            this.Controls.Add(this.btnOrderThemMon);
            this.Controls.Add(this.btnTiepNhanKhach);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_ThongTinBan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thông tin bàn";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblThoiGianDat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblGhiChu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSoKhach;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblKhachHang;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvOrder;
        private UI.Controls.RoundedButton btnTiepNhanKhach;
        private UI.Controls.RoundedButton btnOrderThemMon;
        private UI.Controls.RoundedButton btnDong;
    }
}
