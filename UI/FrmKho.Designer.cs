namespace UI
{
    partial class FrmKho
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
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label7 = new Label();
            label3 = new Label();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            label1 = new Label();
            label2 = new Label();
            roundedButton1 = new UI.Controls.RoundedButton();
            roundedButton2 = new UI.Controls.RoundedButton();
            roundedButton3 = new UI.Controls.RoundedButton();
            roundedButton4 = new UI.Controls.RoundedButton();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            label9 = new Label();
            label5 = new Label();
            roundedPanel3 = new UI.Controls.RoundedPanel();
            label8 = new Label();
            label4 = new Label();
            roundedPanel4 = new UI.Controls.RoundedPanel();
            label10 = new Label();
            label6 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            borderComboBox1 = new UiControls.BorderComboBox();
            dgvKho = new DataGridView();
            dgvtxtTenNguyenLieu = new DataGridViewTextBoxColumn();
            dgvtxtDonVi = new DataGridViewTextBoxColumn();
            dgvtxtTonKho = new DataGridViewTextBoxColumn();
            dgvtxtTonToiThieu = new DataGridViewTextBoxColumn();
            dgvtxtDungTb = new DataGridViewTextBoxColumn();
            DgvtxtGiaTri = new DataGridViewTextBoxColumn();
            dgvtxtTrangThai = new DataGridViewTextBoxColumn();
            dgvtxtThaoTac = new DataGridViewTextBoxColumn();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
            roundedPanel3.SuspendLayout();
            roundedPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKho).BeginInit();
            SuspendLayout();
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.FromArgb(192, 255, 192);
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(label7);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Location = new Point(38, 126);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(268, 141);
            roundedPanel1.TabIndex = 0;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(20, 81);
            label7.Name = "label7";
            label7.Size = new Size(79, 20);
            label7.TabIndex = 17;
            label7.Text = "44.700.000";
            label7.Click += label3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 31);
            label3.Name = "label3";
            label3.Size = new Size(114, 20);
            label3.TabIndex = 17;
            label3.Text = "Tổng giá trị kho";
            label3.Click += label3_Click;
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(38, 330);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(518, 42);
            roundedTextBox1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(13, 9);
            label1.Name = "label1";
            label1.Size = new Size(159, 35);
            label1.TabIndex = 11;
            label1.Text = "Quản Lý Kho";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 45);
            label2.Name = "label2";
            label2.Size = new Size(250, 20);
            label2.TabIndex = 12;
            label2.Text = "Nhập - Xuất - Kiểm Kê - Chuyển Kho";
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.WhiteSmoke;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.Black;
            roundedButton1.Location = new Point(571, 45);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.Size = new Size(142, 37);
            roundedButton1.TabIndex = 13;
            roundedButton1.Text = "Nhập Kho ";
            roundedButton1.UseVisualStyleBackColor = false;
            roundedButton1.Click += roundedButton1_Click;
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.WhiteSmoke;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.Black;
            roundedButton2.Location = new Point(719, 45);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.Size = new Size(142, 37);
            roundedButton2.TabIndex = 13;
            roundedButton2.Text = "Xuất Kho";
            roundedButton2.UseVisualStyleBackColor = false;
            roundedButton2.Click += roundedButton1_Click;
            // 
            // roundedButton3
            // 
            roundedButton3.BackColor = Color.WhiteSmoke;
            roundedButton3.FlatStyle = FlatStyle.Flat;
            roundedButton3.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton3.ForeColor = Color.Black;
            roundedButton3.Location = new Point(867, 45);
            roundedButton3.Name = "roundedButton3";
            roundedButton3.Padding = new Padding(10, 6, 10, 6);
            roundedButton3.Size = new Size(142, 37);
            roundedButton3.TabIndex = 13;
            roundedButton3.Text = "Chuyển Kho ";
            roundedButton3.UseVisualStyleBackColor = false;
            roundedButton3.Click += roundedButton1_Click;
            // 
            // roundedButton4
            // 
            roundedButton4.BackColor = Color.Black;
            roundedButton4.FlatStyle = FlatStyle.Flat;
            roundedButton4.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton4.ForeColor = Color.White;
            roundedButton4.HoverBackColor = Color.Gray;
            roundedButton4.Location = new Point(1018, 45);
            roundedButton4.Name = "roundedButton4";
            roundedButton4.Padding = new Padding(10, 6, 10, 6);
            roundedButton4.Size = new Size(142, 37);
            roundedButton4.TabIndex = 13;
            roundedButton4.Text = "Kiểm Kê";
            roundedButton4.UseVisualStyleBackColor = false;
            roundedButton4.Click += roundedButton1_Click;
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.FromArgb(192, 255, 192);
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(label9);
            roundedPanel2.Controls.Add(label5);
            roundedPanel2.Location = new Point(611, 126);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(12);
            roundedPanel2.Size = new Size(262, 141);
            roundedPanel2.TabIndex = 14;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(15, 81);
            label9.Name = "label9";
            label9.Size = new Size(17, 20);
            label9.TabIndex = 17;
            label9.Text = "3";
            label9.Click += label3_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 31);
            label5.Name = "label5";
            label5.Size = new Size(96, 20);
            label5.TabIndex = 17;
            label5.Text = "Sắp hết hàng";
            label5.Click += label3_Click;
            // 
            // roundedPanel3
            // 
            roundedPanel3.BackColor = Color.FromArgb(192, 255, 192);
            roundedPanel3.BorderThickness = 5;
            roundedPanel3.Controls.Add(label8);
            roundedPanel3.Controls.Add(label4);
            roundedPanel3.Location = new Point(325, 126);
            roundedPanel3.Name = "roundedPanel3";
            roundedPanel3.Padding = new Padding(12);
            roundedPanel3.Size = new Size(265, 141);
            roundedPanel3.TabIndex = 15;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(15, 81);
            label8.Name = "label8";
            label8.Size = new Size(17, 20);
            label8.TabIndex = 17;
            label8.Text = "6";
            label8.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 31);
            label4.Name = "label4";
            label4.Size = new Size(110, 20);
            label4.TabIndex = 17;
            label4.Text = "Tổng mặt hàng";
            label4.Click += label3_Click;
            // 
            // roundedPanel4
            // 
            roundedPanel4.BackColor = Color.FromArgb(192, 255, 192);
            roundedPanel4.BorderThickness = 5;
            roundedPanel4.Controls.Add(label10);
            roundedPanel4.Controls.Add(label6);
            roundedPanel4.Location = new Point(893, 126);
            roundedPanel4.Name = "roundedPanel4";
            roundedPanel4.Padding = new Padding(12);
            roundedPanel4.Size = new Size(267, 141);
            roundedPanel4.TabIndex = 16;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(15, 81);
            label10.Name = "label10";
            label10.Size = new Size(17, 20);
            label10.TabIndex = 17;
            label10.Text = "0";
            label10.Click += label3_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(15, 31);
            label6.Name = "label6";
            label6.Size = new Size(70, 20);
            label6.TabIndex = 17;
            label6.Text = "Hết hàng";
            label6.Click += label3_Click;
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Nhập Kho";
            pillItem2.Text = "Lịch Sử Giao Dịch";
            pillItem3.Text = "Cảnh báo";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Items.Add(pillItem3);
            segmentedPill1.Location = new Point(38, 273);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(412, 55);
            segmentedPill1.TabIndex = 17;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // borderComboBox1
            // 
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.IntegralHeight = false;
            borderComboBox1.ItemHeight = 26;
            borderComboBox1.Items.AddRange(new object[] { "Tất cả", "Đủ Hàng", "Sắp Hết", "Hết Hàng" });
            borderComboBox1.Location = new Point(611, 330);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(180, 32);
            borderComboBox1.TabIndex = 18;
            // 
            // dgvKho
            // 
            dgvKho.AllowUserToAddRows = false;
            dgvKho.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvKho.BackgroundColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvKho.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvKho.ColumnHeadersHeight = 60;
            dgvKho.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvKho.Columns.AddRange(new DataGridViewColumn[] { dgvtxtTenNguyenLieu, dgvtxtDonVi, dgvtxtTonKho, dgvtxtTonToiThieu, dgvtxtDungTb, DgvtxtGiaTri, dgvtxtTrangThai, dgvtxtThaoTac });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(255, 255, 192);
            dataGridViewCellStyle2.SelectionForeColor = Color.Olive;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvKho.DefaultCellStyle = dataGridViewCellStyle2;
            dgvKho.Location = new Point(12, 378);
            dgvKho.Name = "dgvKho";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 255, 192);
            dataGridViewCellStyle3.SelectionForeColor = Color.Olive;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvKho.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvKho.RowHeadersVisible = false;
            dgvKho.RowHeadersWidth = 51;
            dgvKho.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvKho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKho.Size = new Size(1148, 473);
            dgvKho.TabIndex = 19;
            dgvKho.CellClick += dgvKho_CellClick;
            dgvKho.CellContentClick += dgvKho_CellContentClick;
            dgvKho.CellPainting += dgvKho_CellPainting;
            // 
            // dgvtxtTenNguyenLieu
            // 
            dgvtxtTenNguyenLieu.HeaderText = "Tên Nguyên Liệu";
            dgvtxtTenNguyenLieu.MinimumWidth = 6;
            dgvtxtTenNguyenLieu.Name = "dgvtxtTenNguyenLieu";
            dgvtxtTenNguyenLieu.Width = 143;
            // 
            // dgvtxtDonVi
            // 
            dgvtxtDonVi.HeaderText = "Đơn Vị";
            dgvtxtDonVi.MinimumWidth = 6;
            dgvtxtDonVi.Name = "dgvtxtDonVi";
            dgvtxtDonVi.Width = 143;
            // 
            // dgvtxtTonKho
            // 
            dgvtxtTonKho.HeaderText = "Tồn Kho";
            dgvtxtTonKho.MinimumWidth = 6;
            dgvtxtTonKho.Name = "dgvtxtTonKho";
            dgvtxtTonKho.Width = 143;
            // 
            // dgvtxtTonToiThieu
            // 
            dgvtxtTonToiThieu.HeaderText = "Tồn Tối Thiểu";
            dgvtxtTonToiThieu.MinimumWidth = 6;
            dgvtxtTonToiThieu.Name = "dgvtxtTonToiThieu";
            dgvtxtTonToiThieu.Width = 144;
            // 
            // dgvtxtDungTb
            // 
            dgvtxtDungTb.HeaderText = "Dùng TB/Ngày";
            dgvtxtDungTb.MinimumWidth = 6;
            dgvtxtDungTb.Name = "dgvtxtDungTb";
            dgvtxtDungTb.Width = 143;
            // 
            // DgvtxtGiaTri
            // 
            DgvtxtGiaTri.HeaderText = "Giá Trị";
            DgvtxtGiaTri.MinimumWidth = 6;
            DgvtxtGiaTri.Name = "DgvtxtGiaTri";
            DgvtxtGiaTri.Width = 143;
            // 
            // dgvtxtTrangThai
            // 
            dgvtxtTrangThai.HeaderText = "Trạng Thái";
            dgvtxtTrangThai.MinimumWidth = 6;
            dgvtxtTrangThai.Name = "dgvtxtTrangThai";
            dgvtxtTrangThai.Width = 143;
            // 
            // dgvtxtThaoTac
            // 
            dgvtxtThaoTac.HeaderText = "Thao Tác";
            dgvtxtThaoTac.MinimumWidth = 6;
            dgvtxtThaoTac.Name = "dgvtxtThaoTac";
            dgvtxtThaoTac.Width = 143;
            // 
            // FrmKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1172, 853);
            Controls.Add(dgvKho);
            Controls.Add(borderComboBox1);
            Controls.Add(segmentedPill1);
            Controls.Add(roundedPanel4);
            Controls.Add(roundedPanel3);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedButton4);
            Controls.Add(roundedButton3);
            Controls.Add(roundedButton2);
            Controls.Add(roundedButton1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(roundedTextBox1);
            Controls.Add(roundedPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmKho";
            Text = "Tồn Kho";
            Load += FrmKho_Load;
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            roundedPanel3.ResumeLayout(false);
            roundedPanel3.PerformLayout();
            roundedPanel4.ResumeLayout(false);
            roundedPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKho).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controls.RoundedPanel roundedPanel1;
        private Controls.RoundedTextBox roundedTextBox1;
        private Label label1;
        private Label label2;
        private Controls.RoundedButton roundedButton1;
        private Controls.RoundedButton roundedButton2;
        private Controls.RoundedButton roundedButton3;
        private Controls.RoundedButton roundedButton4;
        private Controls.RoundedPanel roundedPanel2;
        private Controls.RoundedPanel roundedPanel3;
        private Controls.RoundedPanel roundedPanel4;
        private Label label3;
        private Label label5;
        private Label label4;
        private Label label7;
        private Label label8;
        private Label label6;
        private Label label9;
        private Label label10;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private UiControls.BorderComboBox borderComboBox1;
        private DataGridView dgvKho;
        private DataGridViewTextBoxColumn dgvtxtTenNguyenLieu;
        private DataGridViewTextBoxColumn dgvtxtDonVi;
        private DataGridViewTextBoxColumn dgvtxtTonKho;
        private DataGridViewTextBoxColumn dgvtxtTonToiThieu;
        private DataGridViewTextBoxColumn dgvtxtDungTb;
        private DataGridViewTextBoxColumn DgvtxtGiaTri;
        private DataGridViewTextBoxColumn dgvtxtTrangThai;
        private DataGridViewTextBoxColumn dgvtxtThaoTac;
    }
}