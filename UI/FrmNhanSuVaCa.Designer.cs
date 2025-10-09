namespace UI
{
    partial class FrmNhanSuVaCa
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
            label1 = new Label();
            label2 = new Label();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label9 = new Label();
            label7 = new Label();
            label8 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            label17 = new Label();
            label16 = new Label();
            label15 = new Label();
            roundedPanel3 = new UI.Controls.RoundedPanel();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            roundedPanel4 = new UI.Controls.RoundedPanel();
            label6 = new Label();
            label10 = new Label();
            label11 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            cbbNhanSu = new UiControls.BorderComboBox();
            roundedButton2 = new UI.Controls.RoundedButton();
            dgvNhanSu = new DataGridView();
            dgvtxtTenNV = new DataGridViewTextBoxColumn();
            dgvtxtChucVu = new DataGridViewTextBoxColumn();
            dgvtxtLienHe = new DataGridViewTextBoxColumn();
            ChiNhanh = new DataGridViewTextBoxColumn();
            NgayVaoLam = new DataGridViewTextBoxColumn();
            TrangThai = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewTextBoxColumn();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
            roundedPanel3.SuspendLayout();
            roundedPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNhanSu).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(336, 35);
            label1.TabIndex = 12;
            label1.Text = "Quản Lý Nhân Sự và Ca Làm";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(336, 23);
            label2.TabIndex = 13;
            label2.Text = "Quản lý  nhân viên, chấm công và phân ca";
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(label9);
            roundedPanel1.Controls.Add(label7);
            roundedPanel1.Controls.Add(label8);
            roundedPanel1.Controls.Add(label5);
            roundedPanel1.Controls.Add(label4);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Location = new Point(12, 95);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(281, 142);
            roundedPanel1.TabIndex = 14;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(320, 22);
            label9.Name = "label9";
            label9.Size = new Size(121, 20);
            label9.TabIndex = 0;
            label9.Text = "Tổng Nhân Viên";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(320, 22);
            label7.Name = "label7";
            label7.Size = new Size(121, 20);
            label7.TabIndex = 0;
            label7.Text = "Tổng Nhân Viên";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(38, 77);
            label8.Name = "label8";
            label8.Size = new Size(18, 20);
            label8.TabIndex = 0;
            label8.Text = "4";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(38, 110);
            label5.Name = "label5";
            label5.Size = new Size(116, 20);
            label5.TabIndex = 0;
            label5.Text = "3 đang làm việc";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(38, 77);
            label4.Name = "label4";
            label4.Size = new Size(18, 20);
            label4.TabIndex = 0;
            label4.Text = "4";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(38, 22);
            label3.Name = "label3";
            label3.Size = new Size(121, 20);
            label3.TabIndex = 0;
            label3.Text = "Tổng Nhân Viên";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.White;
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(label17);
            roundedPanel2.Controls.Add(label16);
            roundedPanel2.Controls.Add(label15);
            roundedPanel2.Location = new Point(913, 95);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(12);
            roundedPanel2.Size = new Size(265, 142);
            roundedPanel2.TabIndex = 15;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label17.Location = new Point(15, 22);
            label17.Name = "label17";
            label17.Size = new Size(123, 20);
            label17.TabIndex = 0;
            label17.Text = "Tăng ca tháng 10";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label16.Location = new Point(15, 110);
            label16.Name = "label16";
            label16.Size = new Size(98, 20);
            label16.TabIndex = 0;
            label16.Text = "Cả chi nhánh";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.Location = new Point(15, 77);
            label15.Name = "label15";
            label15.Size = new Size(35, 20);
            label15.TabIndex = 0;
            label15.Text = "45h";
            // 
            // roundedPanel3
            // 
            roundedPanel3.BackColor = Color.White;
            roundedPanel3.BorderThickness = 5;
            roundedPanel3.Controls.Add(label14);
            roundedPanel3.Controls.Add(label13);
            roundedPanel3.Controls.Add(label12);
            roundedPanel3.Location = new Point(613, 95);
            roundedPanel3.Name = "roundedPanel3";
            roundedPanel3.Padding = new Padding(12);
            roundedPanel3.Size = new Size(273, 142);
            roundedPanel3.TabIndex = 16;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(15, 22);
            label14.Name = "label14";
            label14.Size = new Size(101, 20);
            label14.TabIndex = 0;
            label14.Text = "Ca đang chạy";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label13.Location = new Point(15, 110);
            label13.Name = "label13";
            label13.Size = new Size(78, 20);
            label13.TabIndex = 0;
            label13.Text = "6:00-14:00";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.Location = new Point(15, 77);
            label12.Name = "label12";
            label12.Size = new Size(62, 20);
            label12.TabIndex = 0;
            label12.Text = "Ca sáng";
            label12.Click += label12_Click;
            // 
            // roundedPanel4
            // 
            roundedPanel4.BackColor = Color.White;
            roundedPanel4.BorderThickness = 5;
            roundedPanel4.Controls.Add(label6);
            roundedPanel4.Controls.Add(label10);
            roundedPanel4.Controls.Add(label11);
            roundedPanel4.Location = new Point(317, 95);
            roundedPanel4.Name = "roundedPanel4";
            roundedPanel4.Padding = new Padding(12);
            roundedPanel4.Size = new Size(267, 142);
            roundedPanel4.TabIndex = 17;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(15, 22);
            label6.Name = "label6";
            label6.Size = new Size(121, 20);
            label6.TabIndex = 0;
            label6.Text = "Có mặt hôm nay";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(15, 110);
            label10.Name = "label10";
            label10.Size = new Size(67, 20);
            label10.TabIndex = 0;
            label10.Text = "67% tỉ lệ";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(15, 77);
            label11.Name = "label11";
            label11.Size = new Size(31, 20);
            label11.TabIndex = 0;
            label11.Text = "2/3";
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem4.Text = "Nhân Viên";
            pillItem5.Text = "Phân Ca";
            pillItem6.Text = "Chấm Công";
            segmentedPill1.Items.Add(pillItem4);
            segmentedPill1.Items.Add(pillItem5);
            segmentedPill1.Items.Add(pillItem6);
            segmentedPill1.Location = new Point(12, 264);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(351, 55);
            segmentedPill1.TabIndex = 18;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(26, 347);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(480, 51);
            roundedTextBox1.TabIndex = 19;
            // 
            // cbbNhanSu
            // 
            cbbNhanSu.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbbNhanSu.DrawMode = DrawMode.OwnerDrawFixed;
            cbbNhanSu.FormattingEnabled = true;
            cbbNhanSu.IntegralHeight = false;
            cbbNhanSu.ItemHeight = 26;
            cbbNhanSu.Items.AddRange(new object[] { "Tất cả", "Ca sáng", "Ca chiều" });
            cbbNhanSu.Location = new Point(530, 347);
            cbbNhanSu.Name = "cbbNhanSu";
            cbbNhanSu.Size = new Size(176, 32);
            cbbNhanSu.TabIndex = 20;
            cbbNhanSu.SelectedIndexChanged += cbbNhanSu_SelectedIndexChanged;
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.BorderColor = Color.Black;
            roundedButton2.BorderThickness = 2;
            roundedButton2.FlatAppearance.BorderSize = 0;
            roundedButton2.FlatStyle = FlatStyle.Flat;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.HoverBackColor = Color.LightGray;
            roundedButton2.Location = new Point(998, 347);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.PressedBackColor = Color.FromArgb(192, 255, 255);
            roundedButton2.Size = new Size(180, 40);
            roundedButton2.TabIndex = 21;
            roundedButton2.Text = "+ Thêm Nhân Viên";
            roundedButton2.UseVisualStyleBackColor = false;
            // 
            // dgvNhanSu
            // 
            dgvNhanSu.AllowUserToAddRows = false;
            dgvNhanSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanSu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNhanSu.Columns.AddRange(new DataGridViewColumn[] { dgvtxtTenNV, dgvtxtChucVu, dgvtxtLienHe, ChiNhanh, NgayVaoLam, TrangThai, ThaoTac });
            dgvNhanSu.Dock = DockStyle.Bottom;
            dgvNhanSu.Location = new Point(0, 427);
            dgvNhanSu.Name = "dgvNhanSu";
            dgvNhanSu.RowHeadersVisible = false;
            dgvNhanSu.RowHeadersWidth = 51;
            dgvNhanSu.Size = new Size(1190, 473);
            dgvNhanSu.TabIndex = 22;
            // 
            // dgvtxtTenNV
            // 
            dgvtxtTenNV.HeaderText = "Tên NV";
            dgvtxtTenNV.MinimumWidth = 6;
            dgvtxtTenNV.Name = "dgvtxtTenNV";
            // 
            // dgvtxtChucVu
            // 
            dgvtxtChucVu.HeaderText = "Chức Vụ";
            dgvtxtChucVu.MinimumWidth = 6;
            dgvtxtChucVu.Name = "dgvtxtChucVu";
            // 
            // dgvtxtLienHe
            // 
            dgvtxtLienHe.HeaderText = "Liên Hệ";
            dgvtxtLienHe.MinimumWidth = 6;
            dgvtxtLienHe.Name = "dgvtxtLienHe";
            // 
            // ChiNhanh
            // 
            ChiNhanh.HeaderText = "Chi Nhánh";
            ChiNhanh.MinimumWidth = 6;
            ChiNhanh.Name = "ChiNhanh";
            // 
            // NgayVaoLam
            // 
            NgayVaoLam.HeaderText = "Ngày Vào Làm";
            NgayVaoLam.MinimumWidth = 6;
            NgayVaoLam.Name = "NgayVaoLam";
            // 
            // TrangThai
            // 
            TrangThai.HeaderText = "Trạng Thái";
            TrangThai.MinimumWidth = 6;
            TrangThai.Name = "TrangThai";
            // 
            // ThaoTac
            // 
            ThaoTac.HeaderText = "Thao Tác";
            ThaoTac.MinimumWidth = 6;
            ThaoTac.Name = "ThaoTac";
            // 
            // FrmNhanSuVaCa
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 900);
            Controls.Add(dgvNhanSu);
            Controls.Add(roundedButton2);
            Controls.Add(cbbNhanSu);
            Controls.Add(roundedTextBox1);
            Controls.Add(segmentedPill1);
            Controls.Add(roundedPanel4);
            Controls.Add(roundedPanel3);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNhanSuVaCa";
            Text = "FrmNhanSuVaCa";
            Load += FrmNhanSuVaCa_Load;
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            roundedPanel3.ResumeLayout(false);
            roundedPanel3.PerformLayout();
            roundedPanel4.ResumeLayout(false);
            roundedPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNhanSu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Controls.RoundedPanel roundedPanel1;
        private Label label9;
        private Label label7;
        private Label label8;
        private Label label5;
        private Label label4;
        private Label label3;
        private Controls.RoundedPanel roundedPanel2;
        private Label label17;
        private Label label16;
        private Label label15;
        private Controls.RoundedPanel roundedPanel3;
        private Label label14;
        private Label label13;
        private Label label12;
        private Controls.RoundedPanel roundedPanel4;
        private Label label6;
        private Label label10;
        private Label label11;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Controls.RoundedTextBox roundedTextBox1;
        private UiControls.BorderComboBox cbbNhanSu;
        private Controls.RoundedButton roundedButton2;
        private DataGridView dgvNhanSu;
        private DataGridViewTextBoxColumn dgvtxtTenNV;
        private DataGridViewTextBoxColumn dgvtxtChucVu;
        private DataGridViewTextBoxColumn dgvtxtLienHe;
        private DataGridViewTextBoxColumn ChiNhanh;
        private DataGridViewTextBoxColumn NgayVaoLam;
        private DataGridViewTextBoxColumn TrangThai;
        private DataGridViewTextBoxColumn ThaoTac;
    }
}