using Guna.UI2.WinForms;

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
            btnExit = new Guna2ControlBox();
            label2 = new Label();
            label3 = new Label();
            btnTiepTuc = new Guna2Button();
            cbbChonChiNhanh = new Guna2ComboBox();
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
            btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExit.Animated = true;
            btnExit.BorderRadius = 10;
            btnExit.FillColor = Color.Transparent;
            btnExit.IconColor = Color.Black;
            btnExit.Location = new Point(517, 0);
            btnExit.Margin = new Padding(3, 2, 3, 2);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(55, 43);
            btnExit.TabIndex = 1;
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
            btnTiepTuc.Animated = true;
            btnTiepTuc.BorderRadius = 15;
            btnTiepTuc.DisabledState.BorderColor = Color.DarkGray;
            btnTiepTuc.DisabledState.CustomBorderColor = Color.DarkGray;
            btnTiepTuc.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnTiepTuc.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnTiepTuc.FillColor = Color.FromArgb(31, 111, 235);
            btnTiepTuc.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTiepTuc.ForeColor = Color.White;
            btnTiepTuc.Location = new Point(445, 220);
            btnTiepTuc.Margin = new Padding(3, 2, 3, 2);
            btnTiepTuc.Name = "btnTiepTuc";
            btnTiepTuc.ShadowDecoration.BorderRadius = 15;
            btnTiepTuc.ShadowDecoration.Depth = 3;
            btnTiepTuc.ShadowDecoration.Enabled = true;
            btnTiepTuc.Size = new Size(106, 32);
            btnTiepTuc.TabIndex = 5;
            btnTiepTuc.Text = "Tiếp Tục";
            btnTiepTuc.Click += btnTiepTuc_Click;
            // 
            // cbbChonChiNhanh
            // 
            cbbChonChiNhanh.BackColor = Color.Transparent;
            cbbChonChiNhanh.BorderColor = Color.FromArgb(225, 229, 234);
            cbbChonChiNhanh.BorderRadius = 10;
            cbbChonChiNhanh.BorderThickness = 2;
            cbbChonChiNhanh.DrawMode = DrawMode.OwnerDrawFixed;
            cbbChonChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbChonChiNhanh.FillColor = Color.White;
            cbbChonChiNhanh.FocusedColor = Color.FromArgb(31, 111, 235);
            cbbChonChiNhanh.FocusedState.BorderColor = Color.FromArgb(31, 111, 235);
            cbbChonChiNhanh.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cbbChonChiNhanh.ForeColor = Color.FromArgb(68, 88, 112);
            cbbChonChiNhanh.ItemHeight = 30;
            cbbChonChiNhanh.Location = new Point(31, 163);
            cbbChonChiNhanh.Name = "cbbChonChiNhanh";
            cbbChonChiNhanh.Size = new Size(487, 36);
            cbbChonChiNhanh.TabIndex = 6;
            // 
            // Frm_ChonChiNhanh
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(571, 268);
            ControlBox = false;
            Controls.Add(cbbChonChiNhanh);
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
            Load += Frm_ChonChiNhanh_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Guna2ControlBox btnExit;
        private Label label2;
        private Label label3;
        private Guna2Button btnTiepTuc;
        private Guna2ComboBox cbbChonChiNhanh;
    }
}