using Sunny.UI;
namespace UI
{
    partial class test
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            dgvTest = new UIDataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dgvTest).BeginInit();
            SuspendLayout();
            // 
            // dgvTest
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(243, 249, 255);
            dgvTest.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvTest.AutoScrollToBottom = true;
            dgvTest.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvTest.BackgroundColor = Color.FromArgb(243, 249, 255);
            dgvTest.BorderStyle = BorderStyle.None;
            dgvTest.CellBorderStyle = DataGridViewCellBorderStyle.RaisedHorizontal;
            dgvTest.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvTest.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvTest.ColumnHeadersHeight = 32;
            dgvTest.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTest.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8, Column9, Column10 });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(220, 236, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvTest.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTest.Dock = DockStyle.Bottom;
            dgvTest.EnableHeadersVisualStyles = false;
            dgvTest.Font = new Font("Microsoft Sans Serif", 12F);
            dgvTest.GridColor = Color.FromArgb(104, 173, 255);
            dgvTest.Location = new Point(0, 207);
            dgvTest.Name = "dgvTest";
            dgvTest.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(243, 249, 255);
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvTest.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvTest.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(220, 236, 255);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(48, 48, 48);
            dgvTest.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvTest.SelectedIndex = -1;
            dgvTest.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTest.Size = new Size(1040, 449);
            dgvTest.Style = UIStyle.Custom;
            dgvTest.TabIndex = 0;
            // 
            // Column1
            // 
            Column1.Frozen = true;
            Column1.HeaderText = "mã hđ";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Width = 77;
            // 
            // Column2
            // 
            Column2.Frozen = true;
            Column2.HeaderText = "bàn/sảnh";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Width = 99;
            // 
            // Column3
            // 
            Column3.Frozen = true;
            Column3.HeaderText = "số tiền";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Width = 80;
            // 
            // Column4
            // 
            Column4.Frozen = true;
            Column4.HeaderText = "khuyến mãi";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.Width = 113;
            // 
            // Column5
            // 
            Column5.Frozen = true;
            Column5.HeaderText = "phương thức";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Width = 122;
            // 
            // Column6
            // 
            Column6.Frozen = true;
            Column6.HeaderText = "ngày";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.Width = 67;
            // 
            // Column7
            // 
            Column7.Frozen = true;
            Column7.HeaderText = "thời gian";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.Width = 93;
            // 
            // Column8
            // 
            Column8.Frozen = true;
            Column8.HeaderText = "thu ngân";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            Column8.Width = 96;
            // 
            // Column9
            // 
            Column9.Frozen = true;
            Column9.HeaderText = "trạng thái";
            Column9.Name = "Column9";
            Column9.ReadOnly = true;
            // 
            // Column10
            // 
            Column10.Frozen = true;
            Column10.HeaderText = "Thao tác";
            Column10.Name = "Column10";
            Column10.ReadOnly = true;
            Column10.Text = "test1";
            Column10.Width = 76;
            // 
            // test
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1040, 656);
            Controls.Add(dgvTest);
            FormBorderStyle = FormBorderStyle.None;
            Name = "test";
            Text = "test";
            ((System.ComponentModel.ISupportInitialize)dgvTest).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIDataGridView dgvTest;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewButtonColumn Column10;
    }
}