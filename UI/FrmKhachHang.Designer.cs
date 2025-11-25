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
            VanThuan.UI.PillItem pillItem4 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem5 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem6 = new VanThuan.UI.PillItem();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            label2 = new Label();
            panelTongKhach = new UI.Controls.RoundedPanel();
            label4 = new Label();
            label3 = new Label();
            khachVip = new UI.Controls.RoundedPanel();
            label7 = new Label();
            label6 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            txtTimKiem = new UI.Controls.RoundedTextBox();
            cbbLocHang = new UiControls.BorderComboBox();
            btnThem = new UI.Controls.RoundedButton();
            dgvKhachHang = new DataGridView();
            TenKH = new DataGridViewTextBoxColumn();
            LienHe = new DataGridViewTextBoxColumn();
            Hang = new DataGridViewTextBoxColumn();
            TongChiTieu = new DataGridViewTextBoxColumn();
            SoLanDen = new DataGridViewTextBoxColumn();
            DiemTichLuy = new DataGridViewTextBoxColumn();
            label9 = new Label();
            label10 = new Label();
            roundedPanel4 = new UI.Controls.RoundedPanel();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panelHDGD = new Guna.UI2.WinForms.Guna2GradientPanel();
            panelCTTT = new Guna.UI2.WinForms.Guna2GradientPanel();
            btnInDs = new Guna.UI2.WinForms.Guna2Button();
            panelTongKhach.SuspendLayout();
            khachVip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKhachHang).BeginInit();
            roundedPanel4.SuspendLayout();
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
            // panelTongKhach
            // 
            panelTongKhach.BackColor = Color.FromArgb(255, 224, 192);
            panelTongKhach.BorderThickness = 5;
            panelTongKhach.Controls.Add(label4);
            panelTongKhach.Controls.Add(label3);
            panelTongKhach.Location = new Point(21, 114);
            panelTongKhach.Name = "panelTongKhach";
            panelTongKhach.Padding = new Padding(12);
            panelTongKhach.Size = new Size(266, 144);
            panelTongKhach.TabIndex = 14;
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
            // khachVip
            // 
            khachVip.BackColor = Color.FromArgb(255, 224, 192);
            khachVip.BorderThickness = 5;
            khachVip.Controls.Add(label7);
            khachVip.Controls.Add(label6);
            khachVip.Location = new Point(437, 114);
            khachVip.Name = "khachVip";
            khachVip.Padding = new Padding(12);
            khachVip.Size = new Size(261, 144);
            khachVip.TabIndex = 15;
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
            label6.Size = new Size(115, 20);
            label6.TabIndex = 0;
            label6.Text = "Khách Hạng Vip";
            label6.Click += label3_Click;
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem4.Text = "Danh Sách KH";
            pillItem5.Text = "Hoạt Động Gần Đây";
            pillItem6.Text = "Chương Trình Thân Thiết";
            segmentedPill1.Items.Add(pillItem4);
            segmentedPill1.Items.Add(pillItem5);
            segmentedPill1.Items.Add(pillItem6);
            segmentedPill1.Location = new Point(21, 288);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(588, 55);
            segmentedPill1.TabIndex = 18;
            segmentedPill1.Text = "segmentedPill1";
            segmentedPill1.SelectedIndexChanged += segmentedPill1_SelectedIndexChanged;
            // 
            // txtTimKiem
            // 
            txtTimKiem.BackColor = Color.White;
            txtTimKiem.Font = new Font("Segoe UI", 10F);
            txtTimKiem.ForeColor = Color.Black;
            txtTimKiem.Location = new Point(21, 349);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Padding = new Padding(10, 8, 10, 8);
            txtTimKiem.Size = new Size(581, 51);
            txtTimKiem.TabIndex = 19;
            // 
            // cbbLocHang
            // 
            cbbLocHang.DrawMode = DrawMode.OwnerDrawFixed;
            cbbLocHang.FormattingEnabled = true;
            cbbLocHang.IntegralHeight = false;
            cbbLocHang.ItemHeight = 26;
            cbbLocHang.Location = new Point(653, 357);
            cbbLocHang.Name = "cbbLocHang";
            cbbLocHang.Size = new Size(206, 32);
            cbbLocHang.TabIndex = 20;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.Black;
            btnThem.BorderThickness = 0;
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI Semibold", 10.5F);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(1012, 349);
            btnThem.Name = "btnThem";
            btnThem.Padding = new Padding(10, 6, 10, 6);
            btnThem.Size = new Size(166, 40);
            btnThem.TabIndex = 21;
            btnThem.Text = "+ Thêm KH Mới";
            btnThem.UseVisualStyleBackColor = false;
            btnThem.Click += btnThemKH_Click;
            // 
            // dgvKhachHang
            // 
            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.AllowUserToResizeColumns = false;
            dgvKhachHang.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvKhachHang.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvKhachHang.BackgroundColor = SystemColors.ControlLightLight;
            dgvKhachHang.ColumnHeadersHeight = 60;
            dgvKhachHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvKhachHang.Columns.AddRange(new DataGridViewColumn[] { TenKH, LienHe, Hang, TongChiTieu, SoLanDen, DiemTichLuy });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(255, 192, 192);
            dataGridViewCellStyle4.SelectionForeColor = Color.Maroon;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvKhachHang.DefaultCellStyle = dataGridViewCellStyle4;
            dgvKhachHang.Location = new Point(12, 418);
            dgvKhachHang.Name = "dgvKhachHang";
            dgvKhachHang.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvKhachHang.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvKhachHang.RowHeadersVisible = false;
            dgvKhachHang.RowHeadersWidth = 50;
            dgvKhachHang.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvKhachHang.RowsDefaultCellStyle = dataGridViewCellStyle6;
            dgvKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhachHang.Size = new Size(1166, 470);
            dgvKhachHang.TabIndex = 22;
            dgvKhachHang.CellClick += dgvKhachHang_CellClick;
            dgvKhachHang.CellContentClick += dgvKhachHang_CellContentClick;
            dgvKhachHang.CellDoubleClick += dgvKhachHang_CellDoubleClick;
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
            TenKH.ReadOnly = true;
            TenKH.Width = 125;
            // 
            // LienHe
            // 
            LienHe.HeaderText = "Liên Hệ";
            LienHe.MinimumWidth = 6;
            LienHe.Name = "LienHe";
            LienHe.ReadOnly = true;
            LienHe.Width = 125;
            // 
            // Hang
            // 
            Hang.HeaderText = "Hạng";
            Hang.MinimumWidth = 6;
            Hang.Name = "Hang";
            Hang.ReadOnly = true;
            Hang.Width = 125;
            // 
            // TongChiTieu
            // 
            TongChiTieu.HeaderText = "Tổng Chi Tiêu";
            TongChiTieu.MinimumWidth = 6;
            TongChiTieu.Name = "TongChiTieu";
            TongChiTieu.ReadOnly = true;
            TongChiTieu.Width = 125;
            // 
            // SoLanDen
            // 
            SoLanDen.HeaderText = "Số Lần Đến";
            SoLanDen.MinimumWidth = 6;
            SoLanDen.Name = "SoLanDen";
            SoLanDen.ReadOnly = true;
            SoLanDen.Width = 125;
            // 
            // DiemTichLuy
            // 
            DiemTichLuy.HeaderText = "Điểm Tích Lũy";
            DiemTichLuy.MinimumWidth = 6;
            DiemTichLuy.Name = "DiemTichLuy";
            DiemTichLuy.ReadOnly = true;
            DiemTichLuy.Width = 125;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(30, 12);
            label9.Name = "label9";
            label9.Size = new Size(126, 20);
            label9.TabIndex = 0;
            label9.Text = "Khách Hạng Vàng";
            label9.Click += label3_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(30, 75);
            label10.Name = "label10";
            label10.Size = new Size(15, 20);
            label10.TabIndex = 0;
            label10.Text = "1";
            label10.Click += label3_Click;
            // 
            // roundedPanel4
            // 
            roundedPanel4.BackColor = Color.FromArgb(255, 224, 192);
            roundedPanel4.BorderThickness = 5;
            roundedPanel4.Controls.Add(label10);
            roundedPanel4.Controls.Add(label9);
            roundedPanel4.Location = new Point(848, 114);
            roundedPanel4.Name = "roundedPanel4";
            roundedPanel4.Padding = new Padding(12);
            roundedPanel4.Size = new Size(269, 144);
            roundedPanel4.TabIndex = 17;
            // 
            // panelHDGD
            // 
            panelHDGD.AutoScroll = true;
            panelHDGD.CustomizableEdges = customizableEdges7;
            panelHDGD.Location = new Point(12, 346);
            panelHDGD.Name = "panelHDGD";
            panelHDGD.ShadowDecoration.CustomizableEdges = customizableEdges8;
            panelHDGD.Size = new Size(1166, 539);
            panelHDGD.TabIndex = 23;
            panelHDGD.Visible = false;
            // 
            // panelCTTT
            // 
            panelCTTT.AutoScroll = true;
            panelCTTT.CustomizableEdges = customizableEdges9;
            panelCTTT.Location = new Point(12, 346);
            panelCTTT.Name = "panelCTTT";
            panelCTTT.ShadowDecoration.CustomizableEdges = customizableEdges10;
            panelCTTT.Size = new Size(1163, 542);
            panelCTTT.TabIndex = 0;
            panelCTTT.Visible = false;
            // 
            // btnInDs
            // 
            btnInDs.BorderRadius = 19;
            btnInDs.CustomizableEdges = customizableEdges11;
            btnInDs.DisabledState.BorderColor = Color.DarkGray;
            btnInDs.DisabledState.CustomBorderColor = Color.DarkGray;
            btnInDs.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnInDs.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnInDs.FillColor = Color.Black;
            btnInDs.Font = new Font("Segoe UI", 9F);
            btnInDs.ForeColor = Color.White;
            btnInDs.Location = new Point(892, 22);
            btnInDs.Name = "btnInDs";
            btnInDs.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnInDs.Size = new Size(225, 56);
            btnInDs.TabIndex = 24;
            btnInDs.Text = "In Danh Sách Khách Hàng";
            btnInDs.Click += guna2Button1_Click;
            // 
            // FrmKhachHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1190, 900);
            Controls.Add(btnInDs);
            Controls.Add(panelCTTT);
            Controls.Add(panelHDGD);
            Controls.Add(dgvKhachHang);
            Controls.Add(btnThem);
            Controls.Add(cbbLocHang);
            Controls.Add(txtTimKiem);
            Controls.Add(segmentedPill1);
            Controls.Add(roundedPanel4);
            Controls.Add(khachVip);
            Controls.Add(panelTongKhach);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmKhachHang";
            Text = "FrmKhachHang";
            Load += FrmKhachHang_Load;
            panelTongKhach.ResumeLayout(false);
            panelTongKhach.PerformLayout();
            khachVip.ResumeLayout(false);
            khachVip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKhachHang).EndInit();
            roundedPanel4.ResumeLayout(false);
            roundedPanel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Controls.RoundedPanel panelTongKhach;
        private Label label4;
        private Label label3;
        private Controls.RoundedPanel khachVip;
        private Label label7;
        private Label label6;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Controls.RoundedTextBox txtTimKiem;
        private UiControls.BorderComboBox cbbLocHang;
        private Controls.RoundedButton btnThem;
        private DataGridView dgvKhachHang;
        private Label label9;
        private Label label10;
        private Controls.RoundedPanel roundedPanel4;
        private DataGridViewTextBoxColumn TenKH;
        private DataGridViewTextBoxColumn LienHe;
        private DataGridViewTextBoxColumn Hang;
        private DataGridViewTextBoxColumn TongChiTieu;
        private DataGridViewTextBoxColumn SoLanDen;
        private DataGridViewTextBoxColumn DiemTichLuy;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Guna.UI2.WinForms.Guna2GradientPanel panelHDGD;
        private Guna.UI2.WinForms.Guna2GradientPanel panelCTTT;
        private Guna.UI2.WinForms.Guna2Button btnInDs;
    }
}