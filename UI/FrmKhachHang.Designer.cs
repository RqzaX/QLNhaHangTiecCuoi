namespace UI
{
    partial class FrmKhachHang
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
            VanThuan.UI.PillItem pillItem1 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem2 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem3 = new VanThuan.UI.PillItem();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            label1 = new Label();
            label2 = new Label();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            roundedPanel3 = new UI.Controls.RoundedPanel();
            label13 = new Label();
            label14 = new Label();
            label12 = new Label();
            roundedPanel4 = new UI.Controls.RoundedPanel();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            borderComboBox1 = new UiControls.BorderComboBox();
            roundedButton2 = new UI.Controls.RoundedButton();
            dgvKhachHang = new DataGridView();
            TenKH = new DataGridViewTextBoxColumn();
            LienHe = new DataGridViewTextBoxColumn();
            Hang = new DataGridViewTextBoxColumn();
            TongChiTieu = new DataGridViewTextBoxColumn();
            SoLanDen = new DataGridViewTextBoxColumn();
            DiemTichLuy = new DataGridViewTextBoxColumn();
            LanCuoi = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewTextBoxColumn();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
            roundedPanel3.SuspendLayout();
            roundedPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKhachHang).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(324, 35);
            label1.TabIndex = 12;
            label1.Text = "Quản Lý Khách Hàng(CRM)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 55);
            label2.Name = "label2";
            label2.Size = new Size(338, 23);
            label2.TabIndex = 13;
            label2.Text = "Quản lý thông tin và chăm sóc khách hàng";
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.FromArgb(255, 224, 192);
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(label5);
            roundedPanel1.Controls.Add(label4);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Location = new Point(21, 114);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(266, 144);
            roundedPanel1.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(25, 112);
            label5.Name = "label5";
            label5.Size = new Size(122, 20);
            label5.TabIndex = 0;
            label5.Text = "+12% Tháng này";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(25, 75);
            label4.Name = "label4";
            label4.Size = new Size(18, 20);
            label4.TabIndex = 0;
            label4.Text = "4";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 12);
            label3.Name = "label3";
            label3.Size = new Size(122, 20);
            label3.TabIndex = 0;
            label3.Text = "Tổng khách hàng";
            label3.Click += label3_Click;
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.FromArgb(255, 224, 192);
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(label8);
            roundedPanel2.Controls.Add(label7);
            roundedPanel2.Controls.Add(label6);
            roundedPanel2.Location = new Point(316, 114);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(12);
            roundedPanel2.Size = new Size(261, 144);
            roundedPanel2.TabIndex = 15;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(40, 112);
            label8.Name = "label8";
            label8.Size = new Size(164, 20);
            label8.TabIndex = 0;
            label8.Text = "25% Tổng Khách Hàng";
            label8.Click += label3_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(40, 75);
            label7.Name = "label7";
            label7.Size = new Size(15, 20);
            label7.TabIndex = 0;
            label7.Text = "1";
            label7.Click += label3_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(40, 12);
            label6.Name = "label6";
            label6.Size = new Size(75, 20);
            label6.TabIndex = 0;
            label6.Text = "Khách Vip";
            label6.Click += label3_Click;
            // 
            // roundedPanel3
            // 
            roundedPanel3.BackColor = Color.FromArgb(255, 224, 192);
            roundedPanel3.BorderThickness = 5;
            roundedPanel3.Controls.Add(label13);
            roundedPanel3.Controls.Add(label14);
            roundedPanel3.Controls.Add(label12);
            roundedPanel3.Location = new Point(918, 114);
            roundedPanel3.Name = "roundedPanel3";
            roundedPanel3.Padding = new Padding(12);
            roundedPanel3.Size = new Size(260, 144);
            roundedPanel3.TabIndex = 16;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label13.Location = new Point(28, 112);
            label13.Name = "label13";
            label13.Size = new Size(102, 20);
            label13.TabIndex = 0;
            label13.Text = "Cần chăm sóc";
            label13.Click += label3_Click;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(28, 75);
            label14.Name = "label14";
            label14.Size = new Size(17, 20);
            label14.TabIndex = 0;
            label14.Text = "8";
            label14.Click += label3_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(28, 12);
            label12.Name = "label12";
            label12.Size = new Size(132, 20);
            label12.TabIndex = 0;
            label12.Text = "Sinh nhật tháng 10";
            label12.Click += label3_Click;
            // 
            // roundedPanel4
            // 
            roundedPanel4.BackColor = Color.FromArgb(255, 224, 192);
            roundedPanel4.BorderThickness = 5;
            roundedPanel4.Controls.Add(label11);
            roundedPanel4.Controls.Add(label10);
            roundedPanel4.Controls.Add(label9);
            roundedPanel4.Location = new Point(612, 114);
            roundedPanel4.Name = "roundedPanel4";
            roundedPanel4.Padding = new Padding(12);
            roundedPanel4.Size = new Size(269, 144);
            roundedPanel4.TabIndex = 17;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(30, 112);
            label11.Name = "label11";
            label11.Size = new Size(113, 20);
            label11.TabIndex = 0;
            label11.Text = "Từ Khách Hàng";
            label11.Click += label3_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(30, 75);
            label10.Name = "label10";
            label10.Size = new Size(51, 20);
            label10.TabIndex = 0;
            label10.Text = "85.5M";
            label10.Click += label3_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(30, 12);
            label9.Name = "label9";
            label9.Size = new Size(114, 20);
            label9.TabIndex = 0;
            label9.Text = "Tổng doanh thu";
            label9.Click += label3_Click;
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Danh Sách KH";
            pillItem2.Text = "Hoạt Động Gần Đây";
            pillItem3.Text = "Chương Trình Thân Thiết";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Items.Add(pillItem3);
            segmentedPill1.Location = new Point(21, 288);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(588, 55);
            segmentedPill1.TabIndex = 18;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(21, 349);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(581, 51);
            roundedTextBox1.TabIndex = 19;
            // 
            // borderComboBox1
            // 
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.IntegralHeight = false;
            borderComboBox1.ItemHeight = 26;
            borderComboBox1.Items.AddRange(new object[] { "Tất Cả Hạng", "Vàng", "Bạc", "Thành viên" });
            borderComboBox1.Location = new Point(642, 349);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(151, 32);
            borderComboBox1.TabIndex = 20;
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.Location = new Point(1012, 349);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.Size = new Size(166, 40);
            roundedButton2.TabIndex = 21;
            roundedButton2.Text = "+ Thêm KH Mới";
            // 
            // dgvKhachHang
            // 
            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvKhachHang.BackgroundColor = SystemColors.ControlLightLight;
            dgvKhachHang.ColumnHeadersHeight = 60;
            dgvKhachHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvKhachHang.Columns.AddRange(new DataGridViewColumn[] { TenKH, LienHe, Hang, TongChiTieu, SoLanDen, DiemTichLuy, LanCuoi, ThaoTac });
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(255, 192, 192);
            dataGridViewCellStyle1.SelectionForeColor = Color.Maroon;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvKhachHang.DefaultCellStyle = dataGridViewCellStyle1;
            dgvKhachHang.Location = new Point(12, 418);
            dgvKhachHang.Name = "dgvKhachHang";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvKhachHang.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvKhachHang.RowHeadersVisible = false;
            dgvKhachHang.RowHeadersWidth = 50;
            dgvKhachHang.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvKhachHang.RowsDefaultCellStyle = dataGridViewCellStyle3;
            dgvKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhachHang.Size = new Size(1157, 470);
            dgvKhachHang.TabIndex = 22;
            dgvKhachHang.CellClick += dgvKhachHang_CellClick;
            dgvKhachHang.CellContentClick += dgvKhachHang_CellContentClick;
            dgvKhachHang.CellMouseEnter += dgvKhachHang_CellMouseEnter;
            dgvKhachHang.CellMouseLeave += dgvKhachHang_CellMouseLeave;
            dgvKhachHang.CellPainting += dgvKhachHang_CellPainting;
            dgvKhachHang.DataBindingComplete += dgvKhachHang_DataBindingComplete;
            dgvKhachHang.MouseEnter += dgvKhachHang_MouseEnter;
            dgvKhachHang.MouseLeave += dgvKhachHang_MouseLeave;
            // 
            // TenKH
            // 
            TenKH.HeaderText = "Tên KH";
            TenKH.MinimumWidth = 6;
            TenKH.Name = "TenKH";
            TenKH.Width = 125;
            // 
            // LienHe
            // 
            LienHe.HeaderText = "Liên Hệ";
            LienHe.MinimumWidth = 6;
            LienHe.Name = "LienHe";
            LienHe.Width = 125;
            // 
            // Hang
            // 
            Hang.HeaderText = "Hạng";
            Hang.MinimumWidth = 6;
            Hang.Name = "Hang";
            Hang.Width = 125;
            // 
            // TongChiTieu
            // 
            TongChiTieu.HeaderText = "Tổng Chi Tiêu";
            TongChiTieu.MinimumWidth = 6;
            TongChiTieu.Name = "TongChiTieu";
            TongChiTieu.Width = 125;
            // 
            // SoLanDen
            // 
            SoLanDen.HeaderText = "Số Lần Đến";
            SoLanDen.MinimumWidth = 6;
            SoLanDen.Name = "SoLanDen";
            SoLanDen.Width = 125;
            // 
            // DiemTichLuy
            // 
            DiemTichLuy.HeaderText = "Điểm Tích Lũy";
            DiemTichLuy.MinimumWidth = 6;
            DiemTichLuy.Name = "DiemTichLuy";
            DiemTichLuy.Width = 125;
            // 
            // LanCuoi
            // 
            LanCuoi.HeaderText = "Lần Cuối";
            LanCuoi.MinimumWidth = 6;
            LanCuoi.Name = "LanCuoi";
            LanCuoi.Width = 125;
            // 
            // ThaoTac
            // 
            ThaoTac.HeaderText = "Thao Thác";
            ThaoTac.MinimumWidth = 6;
            ThaoTac.Name = "ThaoTac";
            ThaoTac.Width = 125;
            // 
            // FrmKhachHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 900);
            Controls.Add(dgvKhachHang);
            Controls.Add(roundedButton2);
            Controls.Add(borderComboBox1);
            Controls.Add(roundedTextBox1);
            Controls.Add(segmentedPill1);
            Controls.Add(roundedPanel4);
            Controls.Add(roundedPanel3);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmKhachHang";
            Text = "FrmKhachHang";
            Load += FrmKhachHang_Load;
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            roundedPanel3.ResumeLayout(false);
            roundedPanel3.PerformLayout();
            roundedPanel4.ResumeLayout(false);
            roundedPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKhachHang).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Controls.RoundedPanel roundedPanel1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Controls.RoundedPanel roundedPanel2;
        private Controls.RoundedPanel roundedPanel3;
        private Controls.RoundedPanel roundedPanel4;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label13;
        private Label label14;
        private Label label12;
        private Label label11;
        private Label label10;
        private Label label9;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Controls.RoundedTextBox roundedTextBox1;
        private UiControls.BorderComboBox borderComboBox1;
        private Controls.RoundedButton roundedButton2;
        private DataGridView dgvKhachHang;
        private DataGridViewTextBoxColumn TenKH;
        private DataGridViewTextBoxColumn LienHe;
        private DataGridViewTextBoxColumn Hang;
        private DataGridViewTextBoxColumn TongChiTieu;
        private DataGridViewTextBoxColumn SoLanDen;
        private DataGridViewTextBoxColumn DiemTichLuy;
        private DataGridViewTextBoxColumn LanCuoi;
        private DataGridViewTextBoxColumn ThaoTac;
    }
}