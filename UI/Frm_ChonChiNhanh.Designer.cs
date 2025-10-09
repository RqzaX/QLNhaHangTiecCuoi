namespace UI
{
    partial class Frm_ChonChiNhanh
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
            label1 = new Label();
            btnExit = new Button();
            label2 = new Label();
            label3 = new Label();
            btnTiepTuc = new UI.Controls.RoundedButton();
            borderComboBox1 = new UiControls.BorderComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(31, 22);
            label1.Name = "label1";
            label1.Size = new Size(199, 23);
            label1.TabIndex = 0;
            label1.Text = "Chọn chi nhánh làm việc";
            // 
            // btnExit
            // 
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(517, 0);
            btnExit.Margin = new Padding(3, 2, 3, 2);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(55, 43);
            btnExit.TabIndex = 1;
            btnExit.Text = "✖";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(31, 50);
            label2.Name = "label2";
            label2.Size = new Size(301, 38);
            label2.TabIndex = 2;
            label2.Text = "Bạn có quyền truy cập nhiều chi nhánh.\r\nVui lòng chọn chi nhánh để bắt đầu làm việc.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(22, 137);
            label3.Name = "label3";
            label3.Size = new Size(88, 23);
            label3.TabIndex = 3;
            label3.Text = "Chi nhánh";
            // 
            // btnTiepTuc
            // 
            btnTiepTuc.BackColor = Color.FromArgb(31, 111, 235);
            btnTiepTuc.Font = new Font("Calibri", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTiepTuc.ForeColor = Color.White;
            btnTiepTuc.Location = new Point(445, 220);
            btnTiepTuc.Margin = new Padding(3, 2, 3, 2);
            btnTiepTuc.Name = "btnTiepTuc";
            btnTiepTuc.Padding = new Padding(9, 4, 9, 4);
            btnTiepTuc.Size = new Size(106, 32);
            btnTiepTuc.TabIndex = 5;
            btnTiepTuc.Text = "Tiếp Tục";
            btnTiepTuc.Click += btnTiepTuc_Click;
            // 
            // borderComboBox1
            // 
            borderComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            borderComboBox1.FormattingEnabled = true;
            borderComboBox1.IntegralHeight = false;
            borderComboBox1.ItemHeight = 22;
            borderComboBox1.Location = new Point(22, 163);
            borderComboBox1.Name = "borderComboBox1";
            borderComboBox1.Size = new Size(529, 28);
            borderComboBox1.TabIndex = 6;
            // 
            // Frm_ChonChiNhanh
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(571, 268);
            ControlBox = false;
            Controls.Add(borderComboBox1);
            Controls.Add(btnTiepTuc);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnExit);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Frm_ChonChiNhanh";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Frm_ChonChiNhanh";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnExit;
        private Label label2;
        private Label label3;
        private Controls.RoundedButton btnTiepTuc;
        private UiControls.BorderComboBox borderComboBox1;
    }
}