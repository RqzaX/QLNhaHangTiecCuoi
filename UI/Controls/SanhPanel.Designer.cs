namespace UI.Controls
{
    partial class SanhPanel
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblSanh = new Label();
            panelhoatdong = new Sunny.UI.UIPanel();
            lblChiNhanh = new Label();
            label1 = new Label();
            lblSucChua = new Label();
            label2 = new Label();
            lblPhiThue = new Label();
            btnChiTiet = new Guna.UI2.WinForms.Guna2Button();
            btnSua = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();
            // 
            // lblSanh
            // 
            lblSanh.AutoSize = true;
            lblSanh.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSanh.ForeColor = Color.FromArgb(17, 24, 39);
            lblSanh.Location = new Point(20, 20);
            lblSanh.Name = "lblSanh";
            lblSanh.Size = new Size(165, 37);
            lblSanh.TabIndex = 0;
            lblSanh.Text = "Sảnh Diamond";
            // 
            // panelhoatdong
            // 
            panelhoatdong.AccessibleDescription = "hoatdong";
            panelhoatdong.FillColor = Color.FromArgb(34, 197, 94);
            panelhoatdong.FillColor2 = Color.FromArgb(34, 197, 94);
            panelhoatdong.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelhoatdong.ForeColor = Color.White;
            panelhoatdong.Location = new Point(250, 22);
            panelhoatdong.Margin = new Padding(0);
            panelhoatdong.MinimumSize = new Size(1, 1);
            panelhoatdong.Name = "panelhoatdong";
            panelhoatdong.Radius = 12;
            panelhoatdong.RectColor = Color.FromArgb(34, 197, 94);
            panelhoatdong.Size = new Size(100, 28);
            panelhoatdong.Style = Sunny.UI.UIStyle.Custom;
            panelhoatdong.TabIndex = 44;
            panelhoatdong.Text = "Hoạt động";
            panelhoatdong.TextAlignment = ContentAlignment.MiddleCenter;
            panelhoatdong.Click += panelhoatdong_Click;
            // 
            // lblChiNhanh
            // 
            lblChiNhanh.AutoSize = true;
            lblChiNhanh.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblChiNhanh.ForeColor = Color.FromArgb(107, 114, 128);
            lblChiNhanh.Location = new Point(20, 65);
            lblChiNhanh.Name = "lblChiNhanh";
            lblChiNhanh.Size = new Size(160, 25);
            lblChiNhanh.TabIndex = 0;
            lblChiNhanh.Text = "Chi nhánh Hà Nội";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(107, 114, 128);
            label1.Location = new Point(20, 110);
            label1.Name = "label1";
            label1.Size = new Size(88, 24);
            label1.TabIndex = 0;
            label1.Text = "Sức chứa:";
            // 
            // lblSucChua
            // 
            lblSucChua.AutoSize = true;
            lblSucChua.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSucChua.ForeColor = Color.FromArgb(17, 24, 39);
            lblSucChua.Location = new Point(140, 110);
            lblSucChua.Name = "lblSucChua";
            lblSucChua.Size = new Size(90, 24);
            lblSucChua.TabIndex = 0;
            lblSucChua.Text = "500 người";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(107, 114, 128);
            label2.Location = new Point(20, 145);
            label2.Name = "label2";
            label2.Size = new Size(96, 24);
            label2.TabIndex = 0;
            label2.Text = "Phí cơ bản:";
            // 
            // lblPhiThue
            // 
            lblPhiThue.AutoSize = true;
            lblPhiThue.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPhiThue.ForeColor = Color.FromArgb(17, 24, 39);
            lblPhiThue.Location = new Point(140, 145);
            lblPhiThue.Name = "lblPhiThue";
            lblPhiThue.Size = new Size(130, 24);
            lblPhiThue.TabIndex = 0;
            lblPhiThue.Text = "30.000.000 ₫";
            // 
            // btnChiTiet
            // 
            btnChiTiet.BorderRadius = 8;
            btnChiTiet.CustomizableEdges = customizableEdges1;
            btnChiTiet.DisabledState.BorderColor = Color.DarkGray;
            btnChiTiet.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChiTiet.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChiTiet.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChiTiet.FillColor = Color.FromArgb(59, 130, 246);
            btnChiTiet.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnChiTiet.ForeColor = Color.White;
            btnChiTiet.Location = new Point(20, 185);
            btnChiTiet.Name = "btnChiTiet";
            btnChiTiet.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnChiTiet.Size = new Size(100, 36);
            btnChiTiet.TabIndex = 45;
            btnChiTiet.Text = "Chi tiết";
            btnChiTiet.Click += btnChiTiet_Click;
            // 
            // btnSua
            // 
            btnSua.BorderRadius = 8;
            btnSua.CustomizableEdges = customizableEdges3;
            btnSua.DisabledState.BorderColor = Color.DarkGray;
            btnSua.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSua.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSua.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSua.FillColor = Color.FromArgb(59, 130, 246);
            btnSua.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(250, 185);
            btnSua.Name = "btnSua";
            btnSua.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnSua.Size = new Size(100, 36);
            btnSua.TabIndex = 45;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // SanhPanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BorderStyle = BorderStyle.None;
            Controls.Add(btnChiTiet);
            Controls.Add(btnSua);
            Controls.Add(panelhoatdong);
            Controls.Add(lblChiNhanh);
            Controls.Add(lblSucChua);
            Controls.Add(lblPhiThue);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblSanh);
            Name = "SanhPanel";
            Size = new Size(370, 240);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSanh;
        private Sunny.UI.UIPanel panelhoatdong;
        private Label lblChiNhanh;
        private Label label1;
        private Label lblSucChua;
        private Label label2;
        private Label lblPhiThue;
        private Guna.UI2.WinForms.Guna2Button btnChiTiet;
        private Guna.UI2.WinForms.Guna2Button btnSua;
    }
}
