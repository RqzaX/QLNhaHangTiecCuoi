using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace UI
{
    partial class Frm_ChiTietDatBan
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(650, 720);
            this.BackColor = Color.White;
            this.Padding = new Padding(0);

            // Main panel với viền đen và bo tròn
            pnlMain = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderRadius = 15,
                BorderColor = Color.Black,
                BorderThickness = 2
            };
            this.Controls.Add(pnlMain);

            // Header panel
            pnlHeader = new Guna2Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 15, 20, 10)
            };
            pnlMain.Controls.Add(pnlHeader);

            // Close button
            btnClose = new Guna2Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Size = new Size(30, 30),
                Location = new Point(600, 10),
                BorderRadius = 15,
                FillColor = Color.Transparent,
                ForeColor = Color.FromArgb(107, 114, 128),
                Animated = true,
                UseTransparentBackground = true,
                Cursor = Cursors.Hand
            };
            btnClose.Click += btnClose_Click;
            pnlHeader.Controls.Add(btnClose);

            // Title với icon
            pnlTitleIcon = new Guna2Panel
            {
                BackColor = Color.FromArgb(59, 130, 246),
                Size = new Size(40, 40),
                Location = new Point(20, 10),
                BorderRadius = 20
            };
            pnlHeader.Controls.Add(pnlTitleIcon);

            lblTitleIcon = new Label
            {
                Text = "", // Icon sẽ thêm sau
                Font = new Font("Segoe UI", 16F),
                AutoSize = false,
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                ForeColor = Color.White
            };
            pnlTitleIcon.Controls.Add(lblTitleIcon);

            lblTitle = new Label
            {
                Text = "Chi tiết đặt bàn",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(70, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlHeader.Controls.Add(lblTitle);

            lblSubtitle = new Label
            {
                Text = "Xem và quản lý thông tin đặt bàn",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(70, 42),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlHeader.Controls.Add(lblSubtitle);

            // Status badge
            pnlStatusBadge = new Guna2Panel
            {
                Height = 40,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(239, 68, 68),
                BorderRadius = 20,
                Padding = new Padding(20, 8, 20, 8),
                Margin = new Padding(20, 10, 20, 10)
            };
            pnlMain.Controls.Add(pnlStatusBadge);

            lblStatusIcon = new Label
            {
                Text = "✕",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlStatusBadge.Controls.Add(lblStatusIcon);

            lblStatus = new Label
            {
                Text = "Đã hủy",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlStatusBadge.Controls.Add(lblStatus);

            // Thông tin khách hàng panel
            pnlThongTinKhachHang = new Guna2Panel
            {
                Height = 100,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                BorderRadius = 12,
                BorderColor = Color.FromArgb(225, 229, 234),
                BorderThickness = 1,
                Padding = new Padding(20, 15, 20, 15),
                Margin = new Padding(20, 10, 20, 10)
            };
            pnlMain.Controls.Add(pnlThongTinKhachHang);

            lblKhachHangTitle = new Label
            {
                Text = "Thông tin khách hàng",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(0, 0),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblKhachHangTitle);

            // Left column - Tên và Email
            lblTenKhachHang = new Label
            {
                Text = "Tên khách hàng",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(0, 35),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblTenKhachHang);

            lblTenKhachHangValue = new Label
            {
                Text = "Hoàng Văn E",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(0, 53),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblTenKhachHangValue);

            lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(0, 80),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblEmail);

            lblEmailIcon = new Label
            {
                Text = "", // Icon email
                Font = new Font("Segoe UI", 12F),
                Location = new Point(50, 78),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblEmailIcon);

            lblEmailValue = new Label
            {
                Text = "hoangvane@email.com",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(70, 78),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblEmailValue);

            // Right column - Số điện thoại
            lblSoDienThoai = new Label
            {
                Text = "Số điện thoại",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(280, 35),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblSoDienThoai);

            lblPhoneIcon = new Label
            {
                Text = "", // Icon phone
                Font = new Font("Segoe UI", 12F),
                Location = new Point(280, 53),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblPhoneIcon);

            lblSoDienThoaiValue = new Label
            {
                Text = "0945678901",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(300, 53),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinKhachHang.Controls.Add(lblSoDienThoaiValue);

            // Thông tin đặt bàn panel
            pnlThongTinDatBan = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderRadius = 12,
                BorderColor = Color.FromArgb(225, 229, 234),
                BorderThickness = 1,
                Padding = new Padding(20, 15, 20, 15),
                Margin = new Padding(20, 10, 20, 10)
            };
            pnlMain.Controls.Add(pnlThongTinDatBan);

            lblThongTinTitle = new Label
            {
                Text = "Thông tin đặt bàn",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(0, 0),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblThongTinTitle);

            // Left column
            lblNgayDat = new Label
            {
                Text = "Ngày đặt",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(0, 40),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblNgayDat);

            lblNgayDatValue = new Label
            {
                Text = "18/10/2025",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(0, 58),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblNgayDatValue);

            lblBan = new Label
            {
                Text = "Bàn",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(0, 90),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblBan);

            lblBanValue = new Label
            {
                Text = "Bàn A04",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(0, 108),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblBanValue);

            lblSoKhach = new Label
            {
                Text = "Số khách",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(0, 140),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblSoKhach);

            lblSoKhachIcon = new Label
            {
                Text = "", // Icon person
                Font = new Font("Segoe UI", 12F),
                Location = new Point(80, 158),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblSoKhachIcon);

            lblSoKhachValue = new Label
            {
                Text = "4 người",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(100, 158),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblSoKhachValue);

            // Right column
            lblGio = new Label
            {
                Text = "Giờ",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(280, 40),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblGio);

            lblGioIcon = new Label
            {
                Text = "", // Icon clock
                Font = new Font("Segoe UI", 12F),
                Location = new Point(320, 58),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblGioIcon);

            lblGioValue = new Label
            {
                Text = "13:00",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(340, 58),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblGioValue);

            lblKhuVuc = new Label
            {
                Text = "Khu vực",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(280, 90),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblKhuVuc);

            lblKhuVucIcon = new Label
            {
                Text = "", // Icon location
                Font = new Font("Segoe UI", 12F),
                Location = new Point(320, 108),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblKhuVucIcon);

            lblKhuVucValue = new Label
            {
                Text = "Khu A",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.Black,
                Location = new Point(340, 108),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblKhuVucValue);

            lblTienCoc = new Label
            {
                Text = "Tiền cọc",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(280, 140),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblTienCoc);

            lblTienCocValue = new Label
            {
                Text = "Chưa cọc",
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                ForeColor = Color.FromArgb(34, 197, 94),
                Location = new Point(280, 158),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlThongTinDatBan.Controls.Add(lblTienCocValue);

            // Ghi chú panel
            pnlGhiChu = new Guna2Panel
            {
                Height = 80,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(254, 243, 199),
                BorderRadius = 12,
                Padding = new Padding(15, 10, 15, 10),
                Margin = new Padding(20, 10, 20, 20)
            };
            pnlMain.Controls.Add(pnlGhiChu);

            lblGhiChuTitle = new Label
            {
                Text = "Ghi chú",
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 83, 9),
                Location = new Point(15, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlGhiChu.Controls.Add(lblGhiChuTitle);

            lblGhiChuIcon = new Label
            {
                Text = "", // Icon speech bubble
                Font = new Font("Segoe UI", 12F),
                Location = new Point(70, 8),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlGhiChu.Controls.Add(lblGhiChuIcon);

            lblGhiChuValue = new Label
            {
                Text = "Khách hủy do bận việc",
                Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 83, 9),
                Location = new Point(15, 35),
                AutoSize = false,
                Size = new Size(520, 40),
                BackColor = Color.Transparent
            };
            pnlGhiChu.Controls.Add(lblGhiChuValue);

            // Footer
            pnlFooter = new Guna2Panel
            {
                Height = 70,
                Dock = DockStyle.Bottom,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 15)
            };
            pnlMain.Controls.Add(pnlFooter);

            lblTimestamp = new Label
            {
                Text = "Tạo lúc: 11:00:00 12/10/2025",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, 15),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            pnlFooter.Controls.Add(lblTimestamp);

            // Buttons
            btnDong = new Guna2Button
            {
                Text = "Đóng",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Size = new Size(100, 40),
                Location = new Point(380, 15),
                BorderRadius = 10,
                FillColor = Color.White,
                ForeColor = Color.FromArgb(107, 114, 128),
                BorderColor = Color.FromArgb(209, 213, 219),
                BorderThickness = 1,
                Animated = true,
                Cursor = Cursors.Hand
            };
            btnDong.Click += btnDong_Click;
            pnlFooter.Controls.Add(btnDong);

            btnGuiTinNhan = new Guna2Button
            {
                Text = "   Gửi tin nhắn",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Size = new Size(140, 40),
                Location = new Point(490, 15),
                BorderRadius = 10,
                FillColor = Color.FromArgb(17, 24, 39),
                ForeColor = Color.White,
                Animated = true,
                Cursor = Cursors.Hand,
                TextAlign = HorizontalAlignment.Left,
                Padding = new Padding(35, 0, 0, 0)
            };
            btnGuiTinNhan.Click += btnGuiTinNhan_Click;
            pnlFooter.Controls.Add(btnGuiTinNhan);

            lblGuiTinNhanIcon = new Label
            {
                Text = "", // Icon message - sẽ thêm ảnh gif sau
                Font = new Font("Segoe UI", 12F),
                Location = new Point(500, 23),
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.White
            };
            pnlFooter.Controls.Add(lblGuiTinNhanIcon);
            lblGuiTinNhanIcon.BringToFront();

            this.ResumeLayout(false);
        }

        // Control declarations
        private Guna2Panel pnlMain;
        private Guna2Panel pnlHeader;
        private Guna2Panel pnlTitleIcon;
        private Guna2Panel pnlStatusBadge;
        private Guna2Panel pnlThongTinKhachHang;
        private Guna2Panel pnlThongTinDatBan;
        private Guna2Panel pnlGhiChu;
        private Guna2Panel pnlFooter;
        private Guna2Button btnClose;
        private Guna2Button btnDong;
        private Guna2Button btnGuiTinNhan;
        private Label lblTitleIcon;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblStatusIcon;
        private Label lblStatus;
        private Label lblKhachHangTitle;
        private Label lblTenKhachHang;
        private Label lblTenKhachHangValue;
        private Label lblEmail;
        private Label lblEmailIcon;
        private Label lblEmailValue;
        private Label lblSoDienThoai;
        private Label lblPhoneIcon;
        private Label lblSoDienThoaiValue;
        private Label lblThongTinTitle;
        private Label lblNgayDat;
        private Label lblNgayDatValue;
        private Label lblBan;
        private Label lblBanValue;
        private Label lblSoKhach;
        private Label lblSoKhachIcon;
        private Label lblSoKhachValue;
        private Label lblGio;
        private Label lblGioIcon;
        private Label lblGioValue;
        private Label lblKhuVuc;
        private Label lblKhuVucIcon;
        private Label lblKhuVucValue;
        private Label lblTienCoc;
        private Label lblTienCocValue;
        private Label lblGhiChuTitle;
        private Label lblGhiChuIcon;
        private Label lblGhiChuValue;
        private Label lblTimestamp;
        private Label lblGuiTinNhanIcon;
    }
}

