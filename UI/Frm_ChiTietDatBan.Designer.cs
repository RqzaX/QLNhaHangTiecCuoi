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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlMain = new Guna2Panel();
            pnlHeader = new Guna2Panel();
            btnClose = new Guna2Button();
            pnlTitleIcon = new Guna2Panel();
            lblTitleIcon = new Label();
            lblTitle = new Label();
            lblSubtitle = new Label();
            pnlStatusBadge = new Guna2Panel();
            lblStatusIcon = new Label();
            lblStatus = new Label();
            pnlThongTinKhachHang = new Guna2Panel();
            lblKhachHangTitle = new Label();
            lblTenKhachHang = new Label();
            lblTenKhachHangValue = new Label();
            lblEmail = new Label();
            lblEmailIcon = new Label();
            lblEmailValue = new Label();
            lblSoDienThoai = new Label();
            lblPhoneIcon = new Label();
            lblSoDienThoaiValue = new Label();
            pnlThongTinDatBan = new Guna2Panel();
            lblThongTinTitle = new Label();
            lblNgayDat = new Label();
            lblNgayDatValue = new Label();
            lblBan = new Label();
            lblBanValue = new Label();
            lblSoKhach = new Label();
            lblSoKhachIcon = new Label();
            lblSoKhachValue = new Label();
            lblGio = new Label();
            lblGioIcon = new Label();
            lblGioValue = new Label();
            lblKhuVuc = new Label();
            lblKhuVucIcon = new Label();
            lblKhuVucValue = new Label();
            lblTienCoc = new Label();
            lblTienCocValue = new Label();
            pnlGhiChu = new Guna2Panel();
            lblGhiChuTitle = new Label();
            lblGhiChuIcon = new Label();
            lblGhiChuValue = new Label();
            pnlFooter = new Guna2Panel();
            lblTimestamp = new Label();
            btnDong = new Guna2Button();
            btnGuiTinNhan = new Guna2Button();
            lblGuiTinNhanIcon = new Label();
            pnlMain.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlTitleIcon.SuspendLayout();
            pnlStatusBadge.SuspendLayout();
            pnlThongTinKhachHang.SuspendLayout();
            pnlThongTinDatBan.SuspendLayout();
            pnlGhiChu.SuspendLayout();
            pnlFooter.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.Controls.Add(pnlHeader);
            pnlMain.Controls.Add(pnlStatusBadge);
            pnlMain.Controls.Add(pnlThongTinKhachHang);
            pnlMain.Controls.Add(pnlThongTinDatBan);
            pnlMain.Controls.Add(pnlGhiChu);
            pnlMain.Controls.Add(pnlFooter);
            pnlMain.CustomizableEdges = customizableEdges21;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.ShadowDecoration.CustomizableEdges = customizableEdges22;
            pnlMain.Size = new Size(200, 100);
            pnlMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(pnlTitleIcon);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.CustomizableEdges = customizableEdges5;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges6;
            pnlHeader.Size = new Size(200, 100);
            pnlHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.CustomizableEdges = customizableEdges1;
            btnClose.Font = new Font("Segoe UI", 9F);
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(0, 0);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnClose.Size = new Size(180, 45);
            btnClose.TabIndex = 0;
            btnClose.Click += btnClose_Click;
            // 
            // pnlTitleIcon
            // 
            pnlTitleIcon.Controls.Add(lblTitleIcon);
            pnlTitleIcon.CustomizableEdges = customizableEdges3;
            pnlTitleIcon.Location = new Point(0, 0);
            pnlTitleIcon.Name = "pnlTitleIcon";
            pnlTitleIcon.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlTitleIcon.Size = new Size(200, 100);
            pnlTitleIcon.TabIndex = 1;
            // 
            // lblTitleIcon
            // 
            lblTitleIcon.Location = new Point(0, 0);
            lblTitleIcon.Name = "lblTitleIcon";
            lblTitleIcon.Size = new Size(100, 23);
            lblTitleIcon.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 2;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Location = new Point(0, 0);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(100, 23);
            lblSubtitle.TabIndex = 3;
            // 
            // pnlStatusBadge
            // 
            pnlStatusBadge.Controls.Add(lblStatusIcon);
            pnlStatusBadge.Controls.Add(lblStatus);
            pnlStatusBadge.CustomizableEdges = customizableEdges7;
            pnlStatusBadge.Location = new Point(0, 0);
            pnlStatusBadge.Name = "pnlStatusBadge";
            pnlStatusBadge.ShadowDecoration.CustomizableEdges = customizableEdges8;
            pnlStatusBadge.Size = new Size(200, 100);
            pnlStatusBadge.TabIndex = 1;
            // 
            // lblStatusIcon
            // 
            lblStatusIcon.Location = new Point(0, 0);
            lblStatusIcon.Name = "lblStatusIcon";
            lblStatusIcon.Size = new Size(100, 23);
            lblStatusIcon.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(0, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(100, 23);
            lblStatus.TabIndex = 1;
            // 
            // pnlThongTinKhachHang
            // 
            pnlThongTinKhachHang.Controls.Add(lblKhachHangTitle);
            pnlThongTinKhachHang.Controls.Add(lblTenKhachHang);
            pnlThongTinKhachHang.Controls.Add(lblTenKhachHangValue);
            pnlThongTinKhachHang.Controls.Add(lblEmail);
            pnlThongTinKhachHang.Controls.Add(lblEmailIcon);
            pnlThongTinKhachHang.Controls.Add(lblEmailValue);
            pnlThongTinKhachHang.Controls.Add(lblSoDienThoai);
            pnlThongTinKhachHang.Controls.Add(lblPhoneIcon);
            pnlThongTinKhachHang.Controls.Add(lblSoDienThoaiValue);
            pnlThongTinKhachHang.CustomizableEdges = customizableEdges9;
            pnlThongTinKhachHang.Location = new Point(0, 0);
            pnlThongTinKhachHang.Name = "pnlThongTinKhachHang";
            pnlThongTinKhachHang.ShadowDecoration.CustomizableEdges = customizableEdges10;
            pnlThongTinKhachHang.Size = new Size(200, 100);
            pnlThongTinKhachHang.TabIndex = 2;
            // 
            // lblKhachHangTitle
            // 
            lblKhachHangTitle.Location = new Point(0, 0);
            lblKhachHangTitle.Name = "lblKhachHangTitle";
            lblKhachHangTitle.Size = new Size(100, 23);
            lblKhachHangTitle.TabIndex = 0;
            // 
            // lblTenKhachHang
            // 
            lblTenKhachHang.Location = new Point(0, 0);
            lblTenKhachHang.Name = "lblTenKhachHang";
            lblTenKhachHang.Size = new Size(100, 23);
            lblTenKhachHang.TabIndex = 1;
            // 
            // lblTenKhachHangValue
            // 
            lblTenKhachHangValue.Location = new Point(0, 0);
            lblTenKhachHangValue.Name = "lblTenKhachHangValue";
            lblTenKhachHangValue.Size = new Size(100, 23);
            lblTenKhachHangValue.TabIndex = 2;
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(0, 0);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 23);
            lblEmail.TabIndex = 3;
            // 
            // lblEmailIcon
            // 
            lblEmailIcon.Location = new Point(0, 0);
            lblEmailIcon.Name = "lblEmailIcon";
            lblEmailIcon.Size = new Size(100, 23);
            lblEmailIcon.TabIndex = 4;
            // 
            // lblEmailValue
            // 
            lblEmailValue.Location = new Point(0, 0);
            lblEmailValue.Name = "lblEmailValue";
            lblEmailValue.Size = new Size(100, 23);
            lblEmailValue.TabIndex = 5;
            // 
            // lblSoDienThoai
            // 
            lblSoDienThoai.Location = new Point(0, 0);
            lblSoDienThoai.Name = "lblSoDienThoai";
            lblSoDienThoai.Size = new Size(100, 23);
            lblSoDienThoai.TabIndex = 6;
            // 
            // lblPhoneIcon
            // 
            lblPhoneIcon.Location = new Point(0, 0);
            lblPhoneIcon.Name = "lblPhoneIcon";
            lblPhoneIcon.Size = new Size(100, 23);
            lblPhoneIcon.TabIndex = 7;
            // 
            // lblSoDienThoaiValue
            // 
            lblSoDienThoaiValue.Location = new Point(0, 0);
            lblSoDienThoaiValue.Name = "lblSoDienThoaiValue";
            lblSoDienThoaiValue.Size = new Size(100, 23);
            lblSoDienThoaiValue.TabIndex = 8;
            // 
            // pnlThongTinDatBan
            // 
            pnlThongTinDatBan.Controls.Add(lblThongTinTitle);
            pnlThongTinDatBan.Controls.Add(lblNgayDat);
            pnlThongTinDatBan.Controls.Add(lblNgayDatValue);
            pnlThongTinDatBan.Controls.Add(lblBan);
            pnlThongTinDatBan.Controls.Add(lblBanValue);
            pnlThongTinDatBan.Controls.Add(lblSoKhach);
            pnlThongTinDatBan.Controls.Add(lblSoKhachIcon);
            pnlThongTinDatBan.Controls.Add(lblSoKhachValue);
            pnlThongTinDatBan.Controls.Add(lblGio);
            pnlThongTinDatBan.Controls.Add(lblGioIcon);
            pnlThongTinDatBan.Controls.Add(lblGioValue);
            pnlThongTinDatBan.Controls.Add(lblKhuVuc);
            pnlThongTinDatBan.Controls.Add(lblKhuVucIcon);
            pnlThongTinDatBan.Controls.Add(lblKhuVucValue);
            pnlThongTinDatBan.Controls.Add(lblTienCoc);
            pnlThongTinDatBan.Controls.Add(lblTienCocValue);
            pnlThongTinDatBan.CustomizableEdges = customizableEdges11;
            pnlThongTinDatBan.Location = new Point(0, 0);
            pnlThongTinDatBan.Name = "pnlThongTinDatBan";
            pnlThongTinDatBan.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnlThongTinDatBan.Size = new Size(200, 100);
            pnlThongTinDatBan.TabIndex = 3;
            // 
            // lblThongTinTitle
            // 
            lblThongTinTitle.Location = new Point(0, 0);
            lblThongTinTitle.Name = "lblThongTinTitle";
            lblThongTinTitle.Size = new Size(100, 23);
            lblThongTinTitle.TabIndex = 0;
            // 
            // lblNgayDat
            // 
            lblNgayDat.Location = new Point(0, 0);
            lblNgayDat.Name = "lblNgayDat";
            lblNgayDat.Size = new Size(100, 23);
            lblNgayDat.TabIndex = 1;
            // 
            // lblNgayDatValue
            // 
            lblNgayDatValue.Location = new Point(0, 0);
            lblNgayDatValue.Name = "lblNgayDatValue";
            lblNgayDatValue.Size = new Size(100, 23);
            lblNgayDatValue.TabIndex = 2;
            // 
            // lblBan
            // 
            lblBan.Location = new Point(0, 0);
            lblBan.Name = "lblBan";
            lblBan.Size = new Size(100, 23);
            lblBan.TabIndex = 3;
            // 
            // lblBanValue
            // 
            lblBanValue.Location = new Point(0, 0);
            lblBanValue.Name = "lblBanValue";
            lblBanValue.Size = new Size(100, 23);
            lblBanValue.TabIndex = 4;
            // 
            // lblSoKhach
            // 
            lblSoKhach.Location = new Point(0, 0);
            lblSoKhach.Name = "lblSoKhach";
            lblSoKhach.Size = new Size(100, 23);
            lblSoKhach.TabIndex = 5;
            // 
            // lblSoKhachIcon
            // 
            lblSoKhachIcon.Location = new Point(0, 0);
            lblSoKhachIcon.Name = "lblSoKhachIcon";
            lblSoKhachIcon.Size = new Size(100, 23);
            lblSoKhachIcon.TabIndex = 6;
            // 
            // lblSoKhachValue
            // 
            lblSoKhachValue.Location = new Point(0, 0);
            lblSoKhachValue.Name = "lblSoKhachValue";
            lblSoKhachValue.Size = new Size(100, 23);
            lblSoKhachValue.TabIndex = 7;
            // 
            // lblGio
            // 
            lblGio.Location = new Point(0, 0);
            lblGio.Name = "lblGio";
            lblGio.Size = new Size(100, 23);
            lblGio.TabIndex = 8;
            // 
            // lblGioIcon
            // 
            lblGioIcon.Location = new Point(0, 0);
            lblGioIcon.Name = "lblGioIcon";
            lblGioIcon.Size = new Size(100, 23);
            lblGioIcon.TabIndex = 9;
            // 
            // lblGioValue
            // 
            lblGioValue.Location = new Point(0, 0);
            lblGioValue.Name = "lblGioValue";
            lblGioValue.Size = new Size(100, 23);
            lblGioValue.TabIndex = 10;
            // 
            // lblKhuVuc
            // 
            lblKhuVuc.Location = new Point(0, 0);
            lblKhuVuc.Name = "lblKhuVuc";
            lblKhuVuc.Size = new Size(100, 23);
            lblKhuVuc.TabIndex = 11;
            // 
            // lblKhuVucIcon
            // 
            lblKhuVucIcon.Location = new Point(0, 0);
            lblKhuVucIcon.Name = "lblKhuVucIcon";
            lblKhuVucIcon.Size = new Size(100, 23);
            lblKhuVucIcon.TabIndex = 12;
            // 
            // lblKhuVucValue
            // 
            lblKhuVucValue.Location = new Point(0, 0);
            lblKhuVucValue.Name = "lblKhuVucValue";
            lblKhuVucValue.Size = new Size(100, 23);
            lblKhuVucValue.TabIndex = 13;
            // 
            // lblTienCoc
            // 
            lblTienCoc.Location = new Point(0, 0);
            lblTienCoc.Name = "lblTienCoc";
            lblTienCoc.Size = new Size(100, 23);
            lblTienCoc.TabIndex = 14;
            // 
            // lblTienCocValue
            // 
            lblTienCocValue.Location = new Point(0, 0);
            lblTienCocValue.Name = "lblTienCocValue";
            lblTienCocValue.Size = new Size(100, 23);
            lblTienCocValue.TabIndex = 15;
            // 
            // pnlGhiChu
            // 
            pnlGhiChu.Controls.Add(lblGhiChuTitle);
            pnlGhiChu.Controls.Add(lblGhiChuIcon);
            pnlGhiChu.Controls.Add(lblGhiChuValue);
            pnlGhiChu.CustomizableEdges = customizableEdges13;
            pnlGhiChu.Location = new Point(0, 0);
            pnlGhiChu.Name = "pnlGhiChu";
            pnlGhiChu.ShadowDecoration.CustomizableEdges = customizableEdges14;
            pnlGhiChu.Size = new Size(200, 100);
            pnlGhiChu.TabIndex = 4;
            // 
            // lblGhiChuTitle
            // 
            lblGhiChuTitle.Location = new Point(0, 0);
            lblGhiChuTitle.Name = "lblGhiChuTitle";
            lblGhiChuTitle.Size = new Size(100, 23);
            lblGhiChuTitle.TabIndex = 0;
            // 
            // lblGhiChuIcon
            // 
            lblGhiChuIcon.Location = new Point(0, 0);
            lblGhiChuIcon.Name = "lblGhiChuIcon";
            lblGhiChuIcon.Size = new Size(100, 23);
            lblGhiChuIcon.TabIndex = 1;
            // 
            // lblGhiChuValue
            // 
            lblGhiChuValue.Location = new Point(0, 0);
            lblGhiChuValue.Name = "lblGhiChuValue";
            lblGhiChuValue.Size = new Size(100, 23);
            lblGhiChuValue.TabIndex = 2;
            // 
            // pnlFooter
            // 
            pnlFooter.Controls.Add(lblTimestamp);
            pnlFooter.Controls.Add(btnDong);
            pnlFooter.Controls.Add(btnGuiTinNhan);
            pnlFooter.Controls.Add(lblGuiTinNhanIcon);
            pnlFooter.CustomizableEdges = customizableEdges19;
            pnlFooter.Location = new Point(0, 0);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.ShadowDecoration.CustomizableEdges = customizableEdges20;
            pnlFooter.Size = new Size(200, 100);
            pnlFooter.TabIndex = 5;
            // 
            // lblTimestamp
            // 
            lblTimestamp.Location = new Point(0, 0);
            lblTimestamp.Name = "lblTimestamp";
            lblTimestamp.Size = new Size(100, 23);
            lblTimestamp.TabIndex = 0;
            // 
            // btnDong
            // 
            btnDong.CustomizableEdges = customizableEdges15;
            btnDong.Font = new Font("Segoe UI", 9F);
            btnDong.ForeColor = Color.White;
            btnDong.Location = new Point(0, 0);
            btnDong.Name = "btnDong";
            btnDong.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnDong.Size = new Size(180, 45);
            btnDong.TabIndex = 1;
            btnDong.Click += btnDong_Click;
            // 
            // btnGuiTinNhan
            // 
            btnGuiTinNhan.CustomizableEdges = customizableEdges17;
            btnGuiTinNhan.Font = new Font("Segoe UI", 9F);
            btnGuiTinNhan.ForeColor = Color.White;
            btnGuiTinNhan.Location = new Point(0, 0);
            btnGuiTinNhan.Name = "btnGuiTinNhan";
            btnGuiTinNhan.ShadowDecoration.CustomizableEdges = customizableEdges18;
            btnGuiTinNhan.Size = new Size(180, 45);
            btnGuiTinNhan.TabIndex = 2;
            btnGuiTinNhan.Click += btnGuiTinNhan_Click;
            // 
            // lblGuiTinNhanIcon
            // 
            lblGuiTinNhanIcon.Location = new Point(0, 0);
            lblGuiTinNhanIcon.Name = "lblGuiTinNhanIcon";
            lblGuiTinNhanIcon.Size = new Size(100, 23);
            lblGuiTinNhanIcon.TabIndex = 3;
            // 
            // Frm_ChiTietDatBan
            // 
            BackColor = Color.White;
            ClientSize = new Size(650, 720);
            Controls.Add(pnlMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Frm_ChiTietDatBan";
            StartPosition = FormStartPosition.CenterParent;
            pnlMain.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlTitleIcon.ResumeLayout(false);
            pnlStatusBadge.ResumeLayout(false);
            pnlThongTinKhachHang.ResumeLayout(false);
            pnlThongTinDatBan.ResumeLayout(false);
            pnlGhiChu.ResumeLayout(false);
            pnlFooter.ResumeLayout(false);
            ResumeLayout(false);
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

