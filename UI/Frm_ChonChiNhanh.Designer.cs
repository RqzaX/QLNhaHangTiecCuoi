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
            cbbChiNhanh = new UiControls.BorderComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(35, 29);
            label1.Name = "label1";
            label1.Size = new Size(242, 28);
            label1.TabIndex = 0;
            label1.Text = "Chọn chi nhánh làm việc";
            // 
            // btnExit
            // 
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(591, 0);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(63, 57);
            btnExit.TabIndex = 1;
            btnExit.Text = "✖";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(35, 66);
            label2.Name = "label2";
            label2.Size = new Size(386, 48);
            label2.TabIndex = 2;
            label2.Text = "Bạn có quyền truy cập nhiều chi nhánh.\r\nVui lòng chọn chi nhánh để bắt đầu làm việc.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(35, 183);
            label3.Name = "label3";
            label3.Size = new Size(106, 28);
            label3.TabIndex = 3;
            label3.Text = "Chi nhánh";
            // 
            // btnTiepTuc
            // 
            btnTiepTuc.BackColor = Color.FromArgb(31, 111, 235);
            btnTiepTuc.FlatAppearance.BorderSize = 0;
            btnTiepTuc.FlatStyle = FlatStyle.Flat;
            btnTiepTuc.Font = new Font("Calibri", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTiepTuc.ForeColor = Color.White;
            btnTiepTuc.Location = new Point(509, 294);
            btnTiepTuc.Name = "btnTiepTuc";
            btnTiepTuc.Padding = new Padding(10, 6, 10, 6);
            btnTiepTuc.Size = new Size(121, 42);
            btnTiepTuc.TabIndex = 5;
            btnTiepTuc.Text = "Tiếp Tục";
            btnTiepTuc.UseVisualStyleBackColor = false;
            btnTiepTuc.Click += btnTiepTuc_Click;
            // 
            // cbbChiNhanh
            // 
            cbbChiNhanh.DrawMode = DrawMode.OwnerDrawFixed;
            cbbChiNhanh.FlatStyle = FlatStyle.Popup;
            cbbChiNhanh.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbbChiNhanh.FormattingEnabled = true;
            cbbChiNhanh.IntegralHeight = false;
            cbbChiNhanh.ItemHeight = 23;
            cbbChiNhanh.Items.AddRange(new object[] { "Chi nhánh Bình Thạnh", "Chi nhánh Quận 1", "Chi nhánh Quận 3", "Chi nhánh Thủ Đức" });
            cbbChiNhanh.Location = new Point(35, 214);
            cbbChiNhanh.Name = "cbbChiNhanh";
            cbbChiNhanh.Size = new Size(595, 29);
            cbbChiNhanh.TabIndex = 6;
            cbbChiNhanh.Text = "Chi nhánh Quận 1";
            // 
            // Frm_ChonChiNhanh
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(653, 357);
            ControlBox = false;
            Controls.Add(cbbChiNhanh);
            Controls.Add(btnTiepTuc);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnExit);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
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
        private UiControls.BorderComboBox cbbChiNhanh;
    }
}