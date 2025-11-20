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
            VanThuan.UI.PillItem pillItem1 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem2 = new VanThuan.UI.PillItem();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label1 = new Label();
            label2 = new Label();
            RpanelTongNV = new UI.Controls.RoundedPanel();
            label9 = new Label();
            label7 = new Label();
            label8 = new Label();
            label4 = new Label();
            label3 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            panelNhanSu = new Guna.UI2.WinForms.Guna2GradientPanel();
            dgvNhanSu = new DataGridView();
            btnThemNV = new UI.Controls.RoundedButton();
            cbbNhanSu = new UiControls.BorderComboBox();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            panelPhanCa = new Guna.UI2.WinForms.Guna2GradientPanel();
            RpanelTongNV.SuspendLayout();
            panelNhanSu.SuspendLayout();
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
            // RpanelTongNV
            // 
            RpanelTongNV.BackColor = Color.FromArgb(255, 192, 192);
            RpanelTongNV.BorderThickness = 5;
            RpanelTongNV.Controls.Add(label9);
            RpanelTongNV.Controls.Add(label7);
            RpanelTongNV.Controls.Add(label8);
            RpanelTongNV.Controls.Add(label4);
            RpanelTongNV.Controls.Add(label3);
            RpanelTongNV.Location = new Point(12, 97);
            RpanelTongNV.Name = "RpanelTongNV";
            RpanelTongNV.Padding = new Padding(12);
            RpanelTongNV.Size = new Size(281, 142);
            RpanelTongNV.TabIndex = 14;
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
            label8.Size = new Size(17, 20);
            label8.TabIndex = 0;
            label8.Text = "0";
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
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Nhân Viên";
            pillItem2.Text = "Phân Ca";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Location = new Point(12, 264);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(237, 55);
            segmentedPill1.TabIndex = 18;
            segmentedPill1.Text = "segmentedPill1";
            segmentedPill1.SelectedIndexChanged += segmentedPill1_SelectedIndexChanged;
            // 
            // panelNhanSu
            // 
            panelNhanSu.Controls.Add(dgvNhanSu);
            panelNhanSu.Controls.Add(btnThemNV);
            panelNhanSu.Controls.Add(cbbNhanSu);
            panelNhanSu.Controls.Add(roundedTextBox1);
            panelNhanSu.CustomizableEdges = customizableEdges1;
            panelNhanSu.Location = new Point(2, 341);
            panelNhanSu.Name = "panelNhanSu";
            panelNhanSu.ShadowDecoration.CustomizableEdges = customizableEdges2;
            panelNhanSu.Size = new Size(1187, 557);
            panelNhanSu.TabIndex = 19;
            // 
            // dgvNhanSu
            // 
            dgvNhanSu.AllowUserToAddRows = false;
            dgvNhanSu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanSu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvNhanSu.BackgroundColor = SystemColors.ButtonHighlight;
            dgvNhanSu.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(255, 192, 255);
            dataGridViewCellStyle1.SelectionForeColor = Color.Purple;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvNhanSu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvNhanSu.ColumnHeadersHeight = 60;
            dgvNhanSu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new Padding(12, 8, 12, 10);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(255, 192, 255);
            dataGridViewCellStyle2.SelectionForeColor = Color.Purple;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvNhanSu.DefaultCellStyle = dataGridViewCellStyle2;
            dgvNhanSu.Dock = DockStyle.Bottom;
            dgvNhanSu.Location = new Point(0, 91);
            dgvNhanSu.Name = "dgvNhanSu";
            dgvNhanSu.ReadOnly = true;
            dgvNhanSu.RowHeadersVisible = false;
            dgvNhanSu.RowHeadersWidth = 51;
            dgvNhanSu.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgvNhanSu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanSu.Size = new Size(1187, 466);
            dgvNhanSu.TabIndex = 26;
            
            // 
            // btnThemNV
            // 
            btnThemNV.BackColor = Color.Black;
            btnThemNV.BorderThickness = 0;
            btnThemNV.FlatStyle = FlatStyle.Flat;
            btnThemNV.Font = new Font("Segoe UI Semibold", 10.5F);
            btnThemNV.ForeColor = Color.White;
            btnThemNV.Location = new Point(911, 15);
            btnThemNV.Name = "btnThemNV";
            btnThemNV.Padding = new Padding(10, 6, 10, 6);
            btnThemNV.Size = new Size(240, 40);
            btnThemNV.TabIndex = 25;
            btnThemNV.Text = "+ Thêm Nhân Viên";
            btnThemNV.UseVisualStyleBackColor = false;
            btnThemNV.Click += btnThemNV_Click;
            // 
            // cbbNhanSu
            // 
            cbbNhanSu.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbbNhanSu.DrawMode = DrawMode.OwnerDrawFixed;
            cbbNhanSu.FormattingEnabled = true;
            cbbNhanSu.IntegralHeight = false;
            cbbNhanSu.ItemHeight = 26;
            cbbNhanSu.Items.AddRange(new object[] { "Tất cả", "Quản Lý", "Phục Vụ ", "Đầu Bếp", "Thu Ngân" });
            cbbNhanSu.Location = new Point(496, 23);
            cbbNhanSu.Name = "cbbNhanSu";
            cbbNhanSu.Size = new Size(215, 32);
            cbbNhanSu.TabIndex = 24;
            cbbNhanSu.SelectedIndexChanged += cbbNhanSu_SelectedIndexChanged;
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(10, 15);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(10, 8, 10, 8);
            roundedTextBox1.Size = new Size(480, 51);
            roundedTextBox1.TabIndex = 23;
            roundedTextBox1.TextChanged += roundedTextBox1_TextChanged;
            // 
            // panelPhanCa
            // 
            panelPhanCa.AutoScroll = true;
            panelPhanCa.CustomizableEdges = customizableEdges3;
            panelPhanCa.Location = new Point(9, 337);
            panelPhanCa.Name = "panelPhanCa";
            panelPhanCa.ShadowDecoration.CustomizableEdges = customizableEdges4;
            panelPhanCa.Size = new Size(1177, 573);
            panelPhanCa.TabIndex = 20;
            panelPhanCa.Visible = false;
            // 
            // FrmNhanSuVaCa
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 900);
            Controls.Add(panelNhanSu);
            Controls.Add(segmentedPill1);
            Controls.Add(RpanelTongNV);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panelPhanCa);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmNhanSuVaCa";
            Text = "FrmNhanSuVaCa";
            Load += FrmNhanSuVaCa_Load;
            RpanelTongNV.ResumeLayout(false);
            RpanelTongNV.PerformLayout();
            panelNhanSu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvNhanSu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Controls.RoundedPanel RpanelTongNV;
        private Label label9;
        private Label label7;
        private Label label8;
        private Label label4;
        private Label label3;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Guna.UI2.WinForms.Guna2GradientPanel panelNhanSu;
        private DataGridView dgvNhanSu;
        private Controls.RoundedButton btnThemNV;
        private UiControls.BorderComboBox cbbNhanSu;
        private Controls.RoundedTextBox roundedTextBox1;
        private Guna.UI2.WinForms.Guna2GradientPanel panelPhanCa;
    }
}