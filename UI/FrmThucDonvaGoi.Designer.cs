namespace UI
{
    partial class FrmThucDonvaGoi
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            label2 = new Label();
            label1 = new Label();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            roundedButton2 = new UI.Controls.RoundedButton();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            dgvThucDonVaGoi = new DataGridView();
            TenMon = new DataGridViewTextBoxColumn();
            DanhMuc = new DataGridViewTextBoxColumn();
            GiaBan = new DataGridViewTextBoxColumn();
            GiaVon = new DataGridViewTextBoxColumn();
            LoiNhuan = new DataGridViewTextBoxColumn();
            TrangThai = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvThucDonVaGoi).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(51, 9);
            label2.Name = "label2";
            label2.Size = new Size(317, 35);
            label2.TabIndex = 9;
            label2.Text = "Thực đơn và Gói tiệc cưới";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(98, 44);
            label1.Name = "label1";
            label1.Size = new Size(219, 20);
            label1.TabIndex = 10;
            label1.Text = "Quản lý món ăn và gói tiệc cưới";
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(38, 185);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(503, 45);
            roundedTextBox1.TabIndex = 11;
            // 
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.Location = new Point(980, 185);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.Size = new Size(180, 40);
            roundedButton2.TabIndex = 12;
            roundedButton2.Text = "+ Thêm Món Mới";
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Thực đơn";
            pillItem2.Text = "Gói Tiệc Cưới";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Location = new Point(38, 109);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(258, 55);
            segmentedPill1.TabIndex = 14;
            segmentedPill1.Text = "segmentedPill1";
            // 
            // dgvThucDonVaGoi
            // 
            dgvThucDonVaGoi.AllowUserToAddRows = false;
            dgvThucDonVaGoi.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvThucDonVaGoi.BackgroundColor = SystemColors.ControlLightLight;
            dgvThucDonVaGoi.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(192, 255, 192);
            dataGridViewCellStyle1.SelectionForeColor = Color.Green;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dgvThucDonVaGoi.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvThucDonVaGoi.ColumnHeadersHeight = 50;
            dgvThucDonVaGoi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvThucDonVaGoi.Columns.AddRange(new DataGridViewColumn[] { TenMon, DanhMuc, GiaBan, GiaVon, LoiNhuan, TrangThai, ThaoTac });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new Padding(12, 6, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(192, 255, 192);
            dataGridViewCellStyle2.SelectionForeColor = Color.Green;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvThucDonVaGoi.DefaultCellStyle = dataGridViewCellStyle2;
            dgvThucDonVaGoi.Location = new Point(12, 236);
            dgvThucDonVaGoi.Name = "dgvThucDonVaGoi";
            dgvThucDonVaGoi.RowHeadersVisible = false;
            dgvThucDonVaGoi.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            dgvThucDonVaGoi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThucDonVaGoi.Size = new Size(1148, 592);
            dgvThucDonVaGoi.TabIndex = 15;
            dgvThucDonVaGoi.CellClick += dgvThucDonVaGoi_CellClick;
            dgvThucDonVaGoi.CellPainting += dgvThucDonVaGoi_CellPainting;
            dgvThucDonVaGoi.MouseMove += dgvThucDonVaGoi_MouseMove;
            // 
            // TenMon
            // 
            TenMon.HeaderText = "Tên Món";
            TenMon.MinimumWidth = 6;
            TenMon.Name = "TenMon";
            TenMon.Width = 164;
            // 
            // DanhMuc
            // 
            DanhMuc.HeaderText = "Danh Mục";
            DanhMuc.MinimumWidth = 6;
            DanhMuc.Name = "DanhMuc";
            DanhMuc.Width = 164;
            // 
            // GiaBan
            // 
            GiaBan.HeaderText = "Giá Bán";
            GiaBan.MinimumWidth = 6;
            GiaBan.Name = "GiaBan";
            GiaBan.Width = 164;
            // 
            // GiaVon
            // 
            GiaVon.HeaderText = "Giá Vốn";
            GiaVon.MinimumWidth = 6;
            GiaVon.Name = "GiaVon";
            GiaVon.Width = 164;
            // 
            // LoiNhuan
            // 
            LoiNhuan.HeaderText = "Lợi Nhuận";
            LoiNhuan.MinimumWidth = 6;
            LoiNhuan.Name = "LoiNhuan";
            LoiNhuan.Width = 164;
            // 
            // TrangThai
            // 
            TrangThai.HeaderText = "Trạng Thái";
            TrangThai.MinimumWidth = 6;
            TrangThai.Name = "TrangThai";
            TrangThai.Width = 163;
            // 
            // ThaoTac
            // 
            ThaoTac.HeaderText = "Thao Tác";
            ThaoTac.MinimumWidth = 6;
            ThaoTac.Name = "ThaoTac";
            ThaoTac.Width = 164;
            // 
            // FrmThucDonvaGoi
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1172, 853);
            Controls.Add(dgvThucDonVaGoi);
            Controls.Add(segmentedPill1);
            Controls.Add(roundedButton2);
            Controls.Add(roundedTextBox1);
            Controls.Add(label1);
            Controls.Add(label2);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmThucDonvaGoi";
            Text = "FrmThucDonvaGoi";
            Load += FrmThucDonvaGoi_Load;
            ((System.ComponentModel.ISupportInitialize)dgvThucDonVaGoi).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label1;
        private Controls.RoundedTextBox roundedTextBox1;
        private Controls.RoundedButton roundedButton2;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private DataGridView dgvThucDonVaGoi;
        private DataGridViewTextBoxColumn TenMon;
        private DataGridViewTextBoxColumn DanhMuc;
        private DataGridViewTextBoxColumn GiaBan;
        private DataGridViewTextBoxColumn GiaVon;
        private DataGridViewTextBoxColumn LoiNhuan;
        private DataGridViewTextBoxColumn TrangThai;
        private DataGridViewTextBoxColumn ThaoTac;
    }
}