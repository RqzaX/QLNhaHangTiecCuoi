using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using UI.Controls;

namespace UI
{
    partial class Frm_DatBan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_DatBan));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pictureBox1 = new PictureBox();
            lbGio = new Label();
            lbTenKhach_SoBan = new Label();
            panelGhiChu = new Sunny.UI.UIPanel();
            lbSoKhach = new Label();
            lbKhu = new Label();
            lbNgay = new Label();
            btnChinhSua = new Guna2Button();
            btnDaDen = new Guna2Button();
            btnHuy = new Guna2Button();
            panelTrangThaiBan = new Sunny.UI.UIPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(48, 43);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lbGio
            // 
            lbGio.AccessibleDescription = "giờ đặt bàn";
            lbGio.AutoSize = true;
            lbGio.BackColor = Color.Transparent;
            lbGio.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbGio.Location = new Point(66, 9);
            lbGio.Name = "lbGio";
            lbGio.Size = new Size(58, 28);
            lbGio.TabIndex = 1;
            lbGio.Text = "12:00";
            // 
            // lbTenKhach_SoBan
            // 
            lbTenKhach_SoBan.AccessibleDescription = "tên khách - số bàn";
            lbTenKhach_SoBan.AutoSize = true;
            lbTenKhach_SoBan.BackColor = Color.Transparent;
            lbTenKhach_SoBan.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTenKhach_SoBan.Location = new Point(66, 45);
            lbTenKhach_SoBan.Name = "lbTenKhach_SoBan";
            lbTenKhach_SoBan.Size = new Size(238, 25);
            lbTenKhach_SoBan.TabIndex = 4;
            lbTenKhach_SoBan.Text = "Nguyễn Văn A - Bàn T01-32";
            // 
            // panelGhiChu
            // 
            panelGhiChu.AccessibleDescription = "ghi chú đặt bàn";
            panelGhiChu.FillColor = Color.FromArgb(253, 249, 241);
            panelGhiChu.FillColor2 = Color.FromArgb(253, 249, 241);
            panelGhiChu.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            panelGhiChu.Location = new Point(10, 107);
            panelGhiChu.Margin = new Padding(4, 5, 4, 5);
            panelGhiChu.MinimumSize = new Size(1, 1);
            panelGhiChu.Name = "panelGhiChu";
            panelGhiChu.Radius = 25;
            panelGhiChu.RectColor = Color.FromArgb(220, 155, 40);
            panelGhiChu.Size = new Size(502, 29);
            panelGhiChu.Style = Sunny.UI.UIStyle.Custom;
            panelGhiChu.TabIndex = 4;
            panelGhiChu.Text = "Ghi chú:";
            panelGhiChu.TextAlignment = ContentAlignment.MiddleLeft;
            // 
            // lbSoKhach
            // 
            lbSoKhach.AccessibleDescription = "số khách";
            lbSoKhach.AutoSize = true;
            lbSoKhach.BackColor = Color.Transparent;
            lbSoKhach.Font = new Font("Segoe UI", 10.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbSoKhach.Location = new Point(182, 77);
            lbSoKhach.Name = "lbSoKhach";
            lbSoKhach.Size = new Size(74, 25);
            lbSoKhach.TabIndex = 5;
            lbSoKhach.Text = "4 khách";
            // 
            // lbKhu
            // 
            lbKhu.AccessibleDescription = "khu vực";
            lbKhu.AutoSize = true;
            lbKhu.BackColor = Color.Transparent;
            lbKhu.Font = new Font("Segoe UI", 10.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbKhu.Location = new Point(283, 77);
            lbKhu.Name = "lbKhu";
            lbKhu.Size = new Size(99, 25);
            lbKhu.TabIndex = 6;
            lbKhu.Text = "Khu tầng 1";
            // 
            // lbNgay
            // 
            lbNgay.AccessibleDescription = "ngày khách sử dụng bàn đặt";
            lbNgay.AutoSize = true;
            lbNgay.BackColor = Color.Transparent;
            lbNgay.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbNgay.Location = new Point(6, 77);
            lbNgay.Name = "lbNgay";
            lbNgay.Size = new Size(106, 25);
            lbNgay.TabIndex = 7;
            lbNgay.Text = "22/22/2025";
            // 
            // btnChinhSua
            // 
            btnChinhSua.AccessibleDescription = "nút chỉnh sửa";
            btnChinhSua.BorderRadius = 15;
            btnChinhSua.CustomizableEdges = customizableEdges1;
            btnChinhSua.DisabledState.BorderColor = Color.DarkGray;
            btnChinhSua.DisabledState.CustomBorderColor = Color.DarkGray;
            btnChinhSua.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnChinhSua.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnChinhSua.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnChinhSua.ForeColor = Color.White;
            btnChinhSua.Location = new Point(519, 14);
            btnChinhSua.Name = "btnChinhSua";
            btnChinhSua.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnChinhSua.Size = new Size(108, 35);
            btnChinhSua.TabIndex = 8;
            btnChinhSua.Text = "Chỉnh sửa";
            // 
            // btnDaDen
            // 
            btnDaDen.AccessibleDescription = "nút khách hàng đã đến";
            btnDaDen.BorderRadius = 15;
            btnDaDen.CustomizableEdges = customizableEdges3;
            btnDaDen.DisabledState.BorderColor = Color.DarkGray;
            btnDaDen.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDaDen.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDaDen.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDaDen.FillColor = Color.FromArgb(0, 192, 0);
            btnDaDen.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDaDen.ForeColor = Color.White;
            btnDaDen.Location = new Point(519, 55);
            btnDaDen.Name = "btnDaDen";
            btnDaDen.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnDaDen.Size = new Size(108, 35);
            btnDaDen.TabIndex = 9;
            btnDaDen.Text = "Đã đến";
            // 
            // btnHuy
            // 
            btnHuy.AccessibleDescription = "nút hủy đặt bàn";
            btnHuy.BorderRadius = 15;
            btnHuy.CustomizableEdges = customizableEdges5;
            btnHuy.DisabledState.BorderColor = Color.DarkGray;
            btnHuy.DisabledState.CustomBorderColor = Color.DarkGray;
            btnHuy.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnHuy.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnHuy.FillColor = Color.FromArgb(192, 0, 0);
            btnHuy.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(519, 96);
            btnHuy.Name = "btnHuy";
            btnHuy.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnHuy.Size = new Size(108, 35);
            btnHuy.TabIndex = 10;
            btnHuy.Text = "Hủy";
            // 
            // panelTrangThaiBan
            // 
            panelTrangThaiBan.AccessibleDescription = "trạng thái bàn (đã đến, đã xác nhận, đã hủy)";
            panelTrangThaiBan.FillColor = Color.FromArgb(238, 251, 250);
            panelTrangThaiBan.FillColor2 = Color.FromArgb(238, 251, 250);
            panelTrangThaiBan.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelTrangThaiBan.Location = new Point(133, 10);
            panelTrangThaiBan.Margin = new Padding(4, 5, 4, 5);
            panelTrangThaiBan.MinimumSize = new Size(1, 1);
            panelTrangThaiBan.Name = "panelTrangThaiBan";
            panelTrangThaiBan.Radius = 25;
            panelTrangThaiBan.RectColor = Color.FromArgb(0, 190, 172);
            panelTrangThaiBan.Size = new Size(161, 29);
            panelTrangThaiBan.Style = Sunny.UI.UIStyle.Custom;
            panelTrangThaiBan.TabIndex = 3;
            panelTrangThaiBan.Text = "Đã đến";
            panelTrangThaiBan.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // panelBoDemThoiGian
            // 
            panelBoDemThoiGian = new Sunny.UI.UIPanel();
            panelBoDemThoiGian.AccessibleDescription = "bộ đếm ngược thời gian đến lúc hủy";
            panelBoDemThoiGian.FillColor = Color.FromArgb(255, 255, 255);
            panelBoDemThoiGian.FillColor2 = Color.FromArgb(255, 255, 255);
            panelBoDemThoiGian.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            panelBoDemThoiGian.Location = new Point(300, 10);
            panelBoDemThoiGian.Margin = new Padding(4, 5, 4, 5);
            panelBoDemThoiGian.MinimumSize = new Size(1, 1);
            panelBoDemThoiGian.Name = "panelBoDemThoiGian";
            panelBoDemThoiGian.Radius = 25;
            panelBoDemThoiGian.RectColor = Color.FromArgb(239, 68, 68);
            panelBoDemThoiGian.Size = new Size(100, 29);
            panelBoDemThoiGian.Style = Sunny.UI.UIStyle.Custom;
            panelBoDemThoiGian.TabIndex = 11;
            panelBoDemThoiGian.Text = "02:00:00";
            panelBoDemThoiGian.TextAlignment = ContentAlignment.MiddleCenter;
            panelBoDemThoiGian.Visible = false;
            // 
            // Frm_DatBan
            // 
            BackColor = Color.White;
            Controls.Add(btnHuy);
            Controls.Add(btnDaDen);
            Controls.Add(btnChinhSua);
            Controls.Add(lbNgay);
            Controls.Add(lbKhu);
            Controls.Add(lbSoKhach);
            Controls.Add(panelTrangThaiBan);
            Controls.Add(panelGhiChu);
            Controls.Add(lbTenKhach_SoBan);
            Controls.Add(lbGio);
            Controls.Add(pictureBox1);
            Controls.Add(panelBoDemThoiGian);
            Name = "Frm_DatBan";
            Size = new Size(638, 144);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }
        private PictureBox pictureBox1;
        private Label lbGio;
        private Label lbTenKhach_SoBan;
        private Sunny.UI.UIPanel panelGhiChu;
        private Label lbSoKhach;
        private Label lbKhu;
        private Label lbNgay;
        private Guna2Button btnChinhSua;
        private Guna2Button btnDaDen;
        private Guna2Button btnHuy;
        private Sunny.UI.UIPanel panelTrangThaiBan;
        private Sunny.UI.UIPanel panelBoDemThoiGian;

        #endregion

        // Control declarations - Designer will manage these

    }
}

