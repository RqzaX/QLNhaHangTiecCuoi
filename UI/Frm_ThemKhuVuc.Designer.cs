using System.Drawing;

namespace UI
{
    partial class Frm_ThemKhuVuc
    {
        private System.ComponentModel.IContainer components = null;

        // 🔶 Các control do Designer quản lý
        private Guna.UI2.WinForms.Guna2TextBox txtTenKhuVuc;
        private Guna.UI2.WinForms.Guna2TextBox txtMoTa;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2Button btnDong;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblTenKV;
        private System.Windows.Forms.Label lblMoTa;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.txtTenKhuVuc = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMoTa = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.btnDong = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTenKV = new System.Windows.Forms.Label();
            this.lblMoTa = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // ====== Form ======
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_ThemKhuVuc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm khu vực mới";

            // ====== Title ======
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(210, 30);
            this.lblTitle.Text = "Thêm khu vực mới";

            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = Color.FromArgb(107, 114, 128);
            this.lblSubtitle.Location = new System.Drawing.Point(30, 70);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(220, 19);
            this.lblSubtitle.Text = "Nhập thông tin khu vực mới";

            // ====== Label Tên KV ======
            this.lblTenKV.AutoSize = true;
            this.lblTenKV.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTenKV.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblTenKV.Location = new System.Drawing.Point(30, 130);
            this.lblTenKV.Name = "lblTenKV";
            this.lblTenKV.Size = new System.Drawing.Size(96, 19);
            this.lblTenKV.Text = "Tên khu vực *";

            // ====== txtTenKhuVuc ======
            this.txtTenKhuVuc.Font = new Font("Segoe UI", 10F);
            this.txtTenKhuVuc.Location = new System.Drawing.Point(30, 160);
            this.txtTenKhuVuc.Name = "txtTenKhuVuc";
            this.txtTenKhuVuc.Size = new System.Drawing.Size(540, 50);
            this.txtTenKhuVuc.BorderRadius = 18;
            this.txtTenKhuVuc.PlaceholderText = "Nhập tên khu vực";
            this.txtTenKhuVuc.TabIndex = 0;

            // ====== Label Mô tả ======
            this.lblMoTa.AutoSize = true;
            this.lblMoTa.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblMoTa.ForeColor = Color.FromArgb(31, 41, 55);
            this.lblMoTa.Location = new System.Drawing.Point(30, 230);
            this.lblMoTa.Name = "lblMoTa";
            this.lblMoTa.Size = new System.Drawing.Size(49, 19);
            this.lblMoTa.Text = "Mô tả";

            // ====== txtMoTa ======
            this.txtMoTa.Font = new Font("Segoe UI", 10F);
            this.txtMoTa.Location = new System.Drawing.Point(30, 260);
            this.txtMoTa.Name = "txtMoTa";
            this.txtMoTa.Size = new System.Drawing.Size(540, 100);
            this.txtMoTa.BorderRadius = 18;
            this.txtMoTa.PlaceholderText = "Nhập mô tả khu vực (tùy chọn)";
            this.txtMoTa.Multiline = true;
            this.txtMoTa.TabIndex = 1;

            // ====== btnLuu ======
            this.btnLuu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnLuu.FillColor = Color.FromArgb(34, 197, 94);
            this.btnLuu.ForeColor = Color.White;
            this.btnLuu.Location = new System.Drawing.Point(350, 380);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(100, 45);
            this.btnLuu.BorderRadius = 18;
            this.btnLuu.TabIndex = 2;
            this.btnLuu.Text = "Lưu";

            // ====== btnDong ======
            this.btnDong.Font = new Font("Segoe UI", 10F);
            this.btnDong.FillColor = Color.FromArgb(108, 117, 125);
            this.btnDong.ForeColor = Color.White;
            this.btnDong.Location = new System.Drawing.Point(470, 380);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(100, 45);
            this.btnDong.BorderRadius = 18;
            this.btnDong.TabIndex = 3;
            this.btnDong.Text = "Đóng";

            // ====== Add Controls ======
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTenKV);
            this.Controls.Add(this.txtTenKhuVuc);
            this.Controls.Add(this.lblMoTa);
            this.Controls.Add(this.txtMoTa);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnDong);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
