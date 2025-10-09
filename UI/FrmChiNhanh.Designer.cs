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
            VanThuan.UI.PillItem pillItem1 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem2 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem3 = new VanThuan.UI.PillItem();
            label2 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label3 = new Label();
            roundedButton2 = new UI.Controls.RoundedButton();
            dgvChiNhanh = new DataGridView();
            label1 = new Label();
            dgvtxtTenChiNhanh = new DataGridViewTextBoxColumn();
            dgvtxtDiaChi = new DataGridViewTextBoxColumn();
            dgvtxtDienThoai = new DataGridViewTextBoxColumn();
            dgvtxtTrangThai = new DataGridViewTextBoxColumn();
            ThaoTac = new DataGridViewTextBoxColumn();
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
            pillItem1.Text = "Chi Nhánh";
            pillItem2.Text = "Khu vực và Bàn";
            pillItem3.Text = "Sảnh tiệc";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Items.Add(pillItem3);
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
            roundedPanel1.Controls.Add(roundedButton2);
            roundedPanel1.Controls.Add(dgvChiNhanh);
            roundedPanel1.Location = new Point(24, 171);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(12);
            roundedPanel1.Size = new Size(1154, 402);
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
            // roundedButton2
            // 
            roundedButton2.BackColor = Color.Black;
            roundedButton2.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton2.ForeColor = Color.White;
            roundedButton2.Location = new Point(959, 36);
            roundedButton2.Name = "roundedButton2";
            roundedButton2.Padding = new Padding(10, 6, 10, 6);
            roundedButton2.Size = new Size(180, 40);
            roundedButton2.TabIndex = 19;
            roundedButton2.Text = "+ Thêm Chi Nhánh";
            // 
            // dgvChiNhanh
            // 
            dgvChiNhanh.AllowUserToAddRows = false;
            dgvChiNhanh.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiNhanh.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiNhanh.Columns.AddRange(new DataGridViewColumn[] { dgvtxtTenChiNhanh, dgvtxtDiaChi, dgvtxtDienThoai, dgvtxtTrangThai, ThaoTac });
            dgvChiNhanh.Location = new Point(0, 102);
            dgvChiNhanh.Name = "dgvChiNhanh";
            dgvChiNhanh.RowHeadersVisible = false;
            dgvChiNhanh.RowHeadersWidth = 51;
            dgvChiNhanh.Size = new Size(1154, 300);
            dgvChiNhanh.TabIndex = 0;
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
            // dgvtxtTenChiNhanh
            // 
            dgvtxtTenChiNhanh.HeaderText = "Tên Chi Nhánh";
            dgvtxtTenChiNhanh.MinimumWidth = 6;
            dgvtxtTenChiNhanh.Name = "dgvtxtTenChiNhanh";
            // 
            // dgvtxtDiaChi
            // 
            dgvtxtDiaChi.HeaderText = "Địa Chỉ";
            dgvtxtDiaChi.MinimumWidth = 6;
            dgvtxtDiaChi.Name = "dgvtxtDiaChi";
            // 
            // dgvtxtDienThoai
            // 
            dgvtxtDienThoai.HeaderText = "Số Điện Thoại";
            dgvtxtDienThoai.MinimumWidth = 6;
            dgvtxtDienThoai.Name = "dgvtxtDienThoai";
            // 
            // dgvtxtTrangThai
            // 
            dgvtxtTrangThai.HeaderText = "Trạng Thái";
            dgvtxtTrangThai.MinimumWidth = 6;
            dgvtxtTrangThai.Name = "dgvtxtTrangThai";
            // 
            // ThaoTac
            // 
            ThaoTac.HeaderText = "Thao Tác";
            ThaoTac.MinimumWidth = 6;
            ThaoTac.Name = "ThaoTac";
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
        private Controls.RoundedButton roundedButton2;
        private Label label1;
        private DataGridViewTextBoxColumn dgvtxtTenChiNhanh;
        private DataGridViewTextBoxColumn dgvtxtDiaChi;
        private DataGridViewTextBoxColumn dgvtxtDienThoai;
        private DataGridViewTextBoxColumn dgvtxtTrangThai;
        private DataGridViewTextBoxColumn ThaoTac;
    }
}