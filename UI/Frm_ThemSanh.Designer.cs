namespace UI
{
    partial class Frm_ThemSanh
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // 🔶 Khai báo control do Designer quản lý
        private Guna.UI2.WinForms.Guna2TextBox txtTenSanh;
        private Guna.UI2.WinForms.Guna2NumericUpDown numSucChua;
        private Guna.UI2.WinForms.Guna2TextBox txtPhiThue;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnThoat;
        private System.Windows.Forms.Label lblTenSanh;
        private System.Windows.Forms.Label lblSucChua;
        private System.Windows.Forms.Label lblPhi;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// UI do Designer sinh — KHÔNG sửa tên và KHÔNG chuyển sang .cs
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtTenSanh = new Guna.UI2.WinForms.Guna2TextBox();
            this.numSucChua = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.txtPhiThue = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.btnThoat = new Guna.UI2.WinForms.Guna2Button();
            this.lblTenSanh = new System.Windows.Forms.Label();
            this.lblSucChua = new System.Windows.Forms.Label();
            this.lblPhi = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numSucChua)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTenSanh
            // 
            this.lblTenSanh.AutoSize = true;
            this.lblTenSanh.Location = new System.Drawing.Point(32, 28);
            this.lblTenSanh.Name = "lblTenSanh";
            this.lblTenSanh.Size = new System.Drawing.Size(63, 15);
            this.lblTenSanh.TabIndex = 0;
            this.lblTenSanh.Text = "Tên sảnh:";
            // 
            // txtTenSanh
            // 
            this.txtTenSanh.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenSanh.Location = new System.Drawing.Point(140, 22);
            this.txtTenSanh.Name = "txtTenSanh";
            this.txtTenSanh.Size = new System.Drawing.Size(330, 36);
            this.txtTenSanh.TabIndex = 1;
            // 
            // lblSucChua
            // 
            this.lblSucChua.AutoSize = true;
            this.lblSucChua.Location = new System.Drawing.Point(32, 84);
            this.lblSucChua.Name = "lblSucChua";
            this.lblSucChua.Size = new System.Drawing.Size(62, 15);
            this.lblSucChua.TabIndex = 0;
            this.lblSucChua.Text = "Sức chứa:";
            // 
            // numSucChua
            // 
            this.numSucChua.Location = new System.Drawing.Point(140, 76);
            this.numSucChua.Name = "numSucChua";
            this.numSucChua.Size = new System.Drawing.Size(120, 36);
            this.numSucChua.TabIndex = 2;
            this.numSucChua.Minimum = 0;
            this.numSucChua.Maximum = 10000;
            // 
            // lblPhi
            // 
            this.lblPhi.AutoSize = true;
            this.lblPhi.Location = new System.Drawing.Point(32, 138);
            this.lblPhi.Name = "lblPhi";
            this.lblPhi.Size = new System.Drawing.Size(100, 15);
            this.lblPhi.TabIndex = 0;
            this.lblPhi.Text = "Phí thuê cơ bản:";
            // 
            // txtPhiThue
            // 
            this.txtPhiThue.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPhiThue.Location = new System.Drawing.Point(140, 130);
            this.txtPhiThue.Name = "txtPhiThue";
            this.txtPhiThue.Size = new System.Drawing.Size(180, 36);
            this.txtPhiThue.TabIndex = 3;
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(140, 200);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(120, 40);
            this.btnLuu.TabIndex = 5;
            this.btnLuu.Text = "Lưu";
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(280, 200);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(120, 40);
            this.btnThoat.TabIndex = 6;
            this.btnThoat.Text = "Thoát";
            // 
            // Frm_ThemSanh
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 280);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.txtPhiThue);
            this.Controls.Add(this.lblPhi);
            this.Controls.Add(this.numSucChua);
            this.Controls.Add(this.lblSucChua);
            this.Controls.Add(this.txtTenSanh);
            this.Controls.Add(this.lblTenSanh);
            this.Name = "Frm_ThemSanh";
            this.Text = "Thêm Sảnh";
            ((System.ComponentModel.ISupportInitialize)(this.numSucChua)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
