namespace UI
{
    partial class FrmChiNhanh
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            label2 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label3 = new Label();
            btnThemChiNhanh = new UI.Controls.RoundedButton();
            dgvChiNhanh = new DataGridView();
            TenCN = new DataGridViewTextBoxColumn();
            DiaChi = new DataGridViewTextBoxColumn();
            DienThoai = new DataGridViewTextBoxColumn();
            TrangThai = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewTextBoxColumn();
            label1 = new Label();
            roundedPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiNhanh).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 64);
            label2.Name = "label2";
            label2.Size = new Size(312, 20);
            label2.TabIndex = 12;
            label2.Text = "Quản lý chi nhánh,Khu vực,Bàn ăn và Sảnh tiệc";
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem4.Text = "Chi Nhánh";
            pillItem5.Text = "Khu vực và Bàn";
            pillItem6.Text = "Sảnh tiệc";
            segmentedPill1.Items.Add(pillItem4);
            segmentedPill1.Items.Add(pillItem5);
            segmentedPill1.Items.Add(pillItem6);
            segmentedPill1.Location = new Point(24, 110);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(394, 55);
            segmentedPill1.TabIndex = 13;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Controls.Add(btnThemChiNhanh);
            roundedPanel1.Controls.Add(dgvChiNhanh);
            roundedPanel1.Location = new Point(24, 171);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(1154, 717);
            roundedPanel1.TabIndex = 14;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(15, 36);
            label3.Name = "label3";
            label3.Size = new Size(246, 31);
            label3.TabIndex = 20;
            label3.Text = "Danh Sách Chi Nhánh";
            // 
            // btnThemChiNhanh
            // 
            btnThemChiNhanh.BackColor = Color.Black;
            btnThemChiNhanh.FlatStyle = FlatStyle.Flat;
            btnThemChiNhanh.Font = new Font("Segoe UI Semibold", 10.5F);
            btnThemChiNhanh.ForeColor = Color.White;
            btnThemChiNhanh.Location = new Point(959, 36);
            btnThemChiNhanh.Name = "btnThemChiNhanh";
            btnThemChiNhanh.Padding = new Padding(10, 6, 10, 6);
            btnThemChiNhanh.Size = new Size(180, 40);
            btnThemChiNhanh.TabIndex = 19;
            btnThemChiNhanh.Text = "+ Thêm Chi Nhánh";
            btnThemChiNhanh.UseVisualStyleBackColor = false;
            btnThemChiNhanh.Click += btnThemChiNhanh_Click;
            // 
            // dgvChiNhanh
            // 
            dgvChiNhanh.AllowUserToAddRows = false;
            dgvChiNhanh.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvChiNhanh.BackgroundColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(192, 192, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.Navy;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvChiNhanh.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvChiNhanh.ColumnHeadersHeight = 60;
            dgvChiNhanh.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvChiNhanh.Columns.AddRange(new DataGridViewColumn[] { TenCN, DiaChi, DienThoai, TrangThai, ThaoTac });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(192, 192, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.Navy;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvChiNhanh.DefaultCellStyle = dataGridViewCellStyle4;
            dgvChiNhanh.Location = new Point(15, 106);
            dgvChiNhanh.Name = "dgvChiNhanh";
            dgvChiNhanh.RowHeadersVisible = false;
            dgvChiNhanh.RowHeadersWidth = 51;
            dgvChiNhanh.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiNhanh.Size = new Size(1124, 596);
            dgvChiNhanh.TabIndex = 0;
            dgvChiNhanh.CellClick += dgvChiNhanh_CellClick;
            dgvChiNhanh.CellPainting += dgvChiNhanh_CellPainting;
            dgvChiNhanh.MouseMove += dgvChiNhanh_MouseMove;
            // 
            // TenCN
            // 
            TenCN.HeaderText = "Tên Chi Nhánh";
            TenCN.MinimumWidth = 6;
            TenCN.Name = "TenCN";
            TenCN.Width = 224;
            // 
            // DiaChi
            // 
            DiaChi.HeaderText = "Địa Chỉ";
            DiaChi.MinimumWidth = 6;
            DiaChi.Name = "DiaChi";
            DiaChi.Width = 224;
            // 
            // DienThoai
            // 
            DienThoai.HeaderText = "Số Điện Thoại";
            DienThoai.MinimumWidth = 6;
            DienThoai.Name = "DienThoai";
            DienThoai.Width = 225;
            // 
            // TrangThai
            // 
            TrangThai.HeaderText = "Trạng Thái";
            TrangThai.MinimumWidth = 6;
            TrangThai.Name = "TrangThai";
            TrangThai.Width = 224;
            // 
            // ThaoTac
            // 
            ThaoTac.HeaderText = "Thao Tác";
            ThaoTac.MinimumWidth = 6;
            ThaoTac.Name = "ThaoTac";
            ThaoTac.Width = 224;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(261, 35);
            label1.TabIndex = 11;
            label1.Text = "Chi Nhánh/Bàn/Sảnh";
            // 
            // FrmChiNhanh
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 900);
            Controls.Add(roundedPanel1);
            Controls.Add(segmentedPill1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmChiNhanh";
            Text = "FrmChiNhanh";
            Load += FrmChiNhanh_Load;
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiNhanh).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Controls.RoundedPanel roundedPanel1;
        private DataGridView dgvChiNhanh;
        private Label label3;
        private Controls.RoundedButton btnThemChiNhanh;
        private Label label1;
        private DataGridViewTextBoxColumn TenCN;
        private DataGridViewTextBoxColumn DiaChi;
        private DataGridViewTextBoxColumn DienThoai;
        private DataGridViewTextBoxColumn TrangThai;
        private DataGridViewTextBoxColumn ThaoTac;
    }
}