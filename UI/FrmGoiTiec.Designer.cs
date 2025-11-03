namespace UI
{
    partial class FrmGoiTiec
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
            btnCapNhat = new UI.Controls.RoundedButton();
            dgvGoiTiec = new DataGridView();
            btnXoa = new UI.Controls.RoundedButton();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtMaGoi = new TextBox();
            txtTenGoi = new TextBox();
            txtGiaGoi = new TextBox();
            label4 = new Label();
            roundedButton1 = new UI.Controls.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)dgvGoiTiec).BeginInit();
            SuspendLayout();
            // 
            // btnCapNhat
            // 
            btnCapNhat.BackColor = Color.FromArgb(128, 255, 128);
            btnCapNhat.BorderThickness = 0;
            btnCapNhat.FlatAppearance.BorderSize = 0;
            btnCapNhat.FlatStyle = FlatStyle.Flat;
            btnCapNhat.Font = new Font("Segoe UI Semibold", 10.5F);
            btnCapNhat.ForeColor = Color.White;
            btnCapNhat.Location = new Point(869, 162);
            btnCapNhat.Name = "btnCapNhat";
            btnCapNhat.Padding = new Padding(10, 6, 10, 6);
            btnCapNhat.Size = new Size(123, 52);
            btnCapNhat.TabIndex = 0;
            btnCapNhat.Text = "Cập nhật";
            btnCapNhat.UseVisualStyleBackColor = false;
            btnCapNhat.Click += btnCapNhat_Click;
            // 
            // dgvGoiTiec
            // 
            dgvGoiTiec.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGoiTiec.Location = new Point(12, 240);
            dgvGoiTiec.Name = "dgvGoiTiec";
            dgvGoiTiec.RowHeadersWidth = 51;
            dgvGoiTiec.Size = new Size(987, 368);
            dgvGoiTiec.TabIndex = 1;
            dgvGoiTiec.CellClick += dgvGoiTiec_CellClick;
            dgvGoiTiec.CellContentClick += dgvGoiTiec_CellContentClick;
            // 
            // btnXoa
            // 
            btnXoa.BackColor = Color.DarkRed;
            btnXoa.BorderThickness = 0;
            btnXoa.FlatAppearance.BorderSize = 0;
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI Semibold", 10.5F);
            btnXoa.ForeColor = Color.WhiteSmoke;
            btnXoa.Location = new Point(751, 162);
            btnXoa.Name = "btnXoa";
            btnXoa.Padding = new Padding(10, 6, 10, 6);
            btnXoa.Size = new Size(112, 52);
            btnXoa.TabIndex = 3;
            btnXoa.Text = "Xóa Gói";
            btnXoa.UseVisualStyleBackColor = false;
            btnXoa.Click += btnXoa_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 82);
            label1.Name = "label1";
            label1.Size = new Size(56, 20);
            label1.TabIndex = 4;
            label1.Text = "Mã gói";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(64, 131);
            label2.Name = "label2";
            label2.Size = new Size(59, 20);
            label2.TabIndex = 4;
            label2.Text = "Tên Gói";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(64, 178);
            label3.Name = "label3";
            label3.Size = new Size(31, 20);
            label3.TabIndex = 4;
            label3.Text = "Giá";
            // 
            // txtMaGoi
            // 
            txtMaGoi.Location = new Point(166, 79);
            txtMaGoi.Name = "txtMaGoi";
            txtMaGoi.Size = new Size(223, 27);
            txtMaGoi.TabIndex = 5;
            // 
            // txtTenGoi
            // 
            txtTenGoi.Location = new Point(166, 128);
            txtTenGoi.Name = "txtTenGoi";
            txtTenGoi.Size = new Size(223, 27);
            txtTenGoi.TabIndex = 6;
            // 
            // txtGiaGoi
            // 
            txtGiaGoi.Location = new Point(166, 175);
            txtGiaGoi.Name = "txtGiaGoi";
            txtGiaGoi.Size = new Size(223, 27);
            txtGiaGoi.TabIndex = 7;
            txtGiaGoi.KeyPress += txtGiaGoi_KeyPress;
            txtGiaGoi.Leave += txtGiaGoi_Leave;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Variable Display", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(343, 9);
            label4.Name = "label4";
            label4.Size = new Size(298, 31);
            label4.TabIndex = 8;
            label4.Text = "Danh sách cách gói tiệc cưới";
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.DarkOrange;
            roundedButton1.BorderThickness = 0;
            roundedButton1.FlatAppearance.BorderSize = 0;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.Location = new Point(604, 162);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(10, 6, 10, 6);
            roundedButton1.Size = new Size(141, 52);
            roundedButton1.TabIndex = 9;
            roundedButton1.Text = "Chọn Gói";
            roundedButton1.UseVisualStyleBackColor = false;
            roundedButton1.Click += roundedButton1_Click;
            // 
            // FrmGoiTiec
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MistyRose;
            ClientSize = new Size(1004, 620);
            Controls.Add(roundedButton1);
            Controls.Add(label4);
            Controls.Add(txtGiaGoi);
            Controls.Add(txtTenGoi);
            Controls.Add(txtMaGoi);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnXoa);
            Controls.Add(dgvGoiTiec);
            Controls.Add(btnCapNhat);
            Name = "FrmGoiTiec";
            Text = "FrmGoiTiec";
            ((System.ComponentModel.ISupportInitialize)dgvGoiTiec).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Controls.RoundedButton btnCapNhat;
        private DataGridView dgvGoiTiec;
        private Controls.RoundedButton btnXoa;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtMaGoi;
        private TextBox txtTenGoi;
        private TextBox txtGiaGoi;
        private Label label4;
        private Controls.RoundedButton roundedButton1;
    }
}