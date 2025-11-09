using Guna.UI2.WinForms;

namespace UI
{
    partial class Frm_HuyDatSanh
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panelMain = new Guna2GradientPanel();
            label1 = new Label();
            txtSoTienHoanCoc = new Guna2TextBox();
            panelButtons = new Panel();
            btnQuayLai = new Guna2Button();
            btnXacNhanHuy = new Guna2Button();
            lbHelperText = new Label();
            txtPhanTram = new Guna2TextBox();
            lbSoTienHoanCoc = new Label();
            txtLyDoHuy = new Guna2TextBox();
            lbLyDoHuy = new Label();
            dtpNgayHuy = new Guna2DateTimePicker();
            lbNgayHuy = new Label();
            lbSubtitle = new Label();
            lbTitle = new Label();
            btnClose = new Guna2Button();
            panelMain.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.Transparent;
            panelMain.BorderColor = Color.Black;
            panelMain.BorderRadius = 20;
            panelMain.BorderThickness = 2;
            panelMain.Controls.Add(label1);
            panelMain.Controls.Add(txtSoTienHoanCoc);
            panelMain.Controls.Add(panelButtons);
            panelMain.Controls.Add(lbHelperText);
            panelMain.Controls.Add(txtPhanTram);
            panelMain.Controls.Add(lbSoTienHoanCoc);
            panelMain.Controls.Add(txtLyDoHuy);
            panelMain.Controls.Add(lbLyDoHuy);
            panelMain.Controls.Add(dtpNgayHuy);
            panelMain.Controls.Add(lbNgayHuy);
            panelMain.Controls.Add(lbSubtitle);
            panelMain.Controls.Add(lbTitle);
            panelMain.Controls.Add(btnClose);
            panelMain.CustomizableEdges = customizableEdges7;
            panelMain.Dock = DockStyle.Fill;
            panelMain.FillColor = Color.White;
            panelMain.FillColor2 = Color.White;
            panelMain.Location = new Point(0, 0);
            panelMain.Name = "panelMain";
            panelMain.ShadowDecoration.BorderRadius = 20;
            panelMain.ShadowDecoration.Color = Color.FromArgb(100, 0, 0, 0);
            panelMain.ShadowDecoration.CustomizableEdges = customizableEdges8;
            panelMain.ShadowDecoration.Depth = 20;
            panelMain.ShadowDecoration.Enabled = true;
            panelMain.Size = new Size(500, 500);
            panelMain.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(64, 64, 64);
            label1.Location = new Point(253, 340);
            label1.Name = "label1";
            label1.Size = new Size(191, 20);
            label1.TabIndex = 12;
            label1.Text = "Số tiền cần hoàn cọc (VNĐ)";
            // 
            // txtSoTienHoanCoc
            // 
            txtSoTienHoanCoc.BorderRadius = 15;
            txtSoTienHoanCoc.CustomizableEdges = customizableEdges1;
            txtSoTienHoanCoc.DefaultText = "";
            txtSoTienHoanCoc.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSoTienHoanCoc.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSoTienHoanCoc.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSoTienHoanCoc.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSoTienHoanCoc.Enabled = false;
            txtSoTienHoanCoc.FillColor = Color.FromArgb(240, 240, 240);
            txtSoTienHoanCoc.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoTienHoanCoc.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtSoTienHoanCoc.ForeColor = Color.Black;
            txtSoTienHoanCoc.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoTienHoanCoc.Location = new Point(258, 365);
            txtSoTienHoanCoc.Margin = new Padding(4);
            txtSoTienHoanCoc.Name = "txtSoTienHoanCoc";
            txtSoTienHoanCoc.PlaceholderForeColor = Color.Black;
            txtSoTienHoanCoc.PlaceholderText = "0 đ";
            txtSoTienHoanCoc.ReadOnly = true;
            txtSoTienHoanCoc.SelectedText = "";
            txtSoTienHoanCoc.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSoTienHoanCoc.Size = new Size(192, 41);
            txtSoTienHoanCoc.TabIndex = 11;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(btnQuayLai);
            panelButtons.Controls.Add(btnXacNhanHuy);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 435);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(500, 65);
            panelButtons.TabIndex = 10;
            // 
            // btnQuayLai
            // 
            btnQuayLai.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnQuayLai.BorderColor = Color.FromArgb(224, 224, 224);
            btnQuayLai.BorderRadius = 15;
            btnQuayLai.BorderThickness = 1;
            btnQuayLai.CustomizableEdges = customizableEdges3;
            btnQuayLai.DisabledState.BorderColor = Color.DarkGray;
            btnQuayLai.DisabledState.CustomBorderColor = Color.DarkGray;
            btnQuayLai.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnQuayLai.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnQuayLai.FillColor = Color.White;
            btnQuayLai.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnQuayLai.ForeColor = Color.Black;
            btnQuayLai.Location = new Point(251, 13);
            btnQuayLai.Name = "btnQuayLai";
            btnQuayLai.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnQuayLai.Size = new Size(100, 40);
            btnQuayLai.TabIndex = 0;
            btnQuayLai.Text = "Quay lại";
            btnQuayLai.Click += BtnQuayLai_Click;
            // 
            // btnXacNhanHuy
            // 
            btnXacNhanHuy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnXacNhanHuy.BorderColor = Color.FromArgb(192, 0, 0);
            btnXacNhanHuy.BorderRadius = 15;
            btnXacNhanHuy.BorderThickness = 1;
            btnXacNhanHuy.CustomizableEdges = customizableEdges5;
            btnXacNhanHuy.DisabledState.BorderColor = Color.DarkGray;
            btnXacNhanHuy.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXacNhanHuy.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXacNhanHuy.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXacNhanHuy.FillColor = Color.FromArgb(192, 0, 0);
            btnXacNhanHuy.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXacNhanHuy.ForeColor = Color.White;
            btnXacNhanHuy.Location = new Point(357, 13);
            btnXacNhanHuy.Name = "btnXacNhanHuy";
            btnXacNhanHuy.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnXacNhanHuy.Size = new Size(131, 40);
            btnXacNhanHuy.TabIndex = 1;
            btnXacNhanHuy.Text = "Xác nhận hủy";
            btnXacNhanHuy.Click += BtnXacNhanHuy_Click;
            // 
            // lbHelperText
            // 
            lbHelperText.AutoSize = true;
            lbHelperText.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbHelperText.ForeColor = Color.FromArgb(128, 128, 128);
            lbHelperText.Location = new Point(50, 410);
            lbHelperText.Name = "lbHelperText";
            lbHelperText.Size = new Size(438, 17);
            lbHelperText.TabIndex = 9;
            lbHelperText.Text = "Tính theo chính sách hoàn cọc dựa trên thời điểm hủy so với ngày tổ chức";
            // 
            // txtPhanTram
            // 
            txtPhanTram.BorderColor = Color.LightGray;
            txtPhanTram.BorderRadius = 15;
            txtPhanTram.CustomizableEdges = customizableEdges7;
            txtPhanTram.DefaultText = "";
            txtPhanTram.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPhanTram.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPhanTram.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPhanTram.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPhanTram.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPhanTram.ForeColor = Color.Black;
            txtPhanTram.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPhanTram.Location = new Point(50, 365);
            txtPhanTram.Name = "txtPhanTram";
            txtPhanTram.PlaceholderText = "0 %";
            txtPhanTram.SelectedText = "";
            txtPhanTram.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtPhanTram.Size = new Size(164, 40);
            txtPhanTram.TabIndex = 8;
            txtPhanTram.TextChanged += TxtPhanTram_TextChanged;
            txtPhanTram.KeyPress += TxtPhanTram_KeyPress;
            // 
            // lbSoTienHoanCoc
            // 
            lbSoTienHoanCoc.AutoSize = true;
            lbSoTienHoanCoc.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbSoTienHoanCoc.ForeColor = Color.FromArgb(64, 64, 64);
            lbSoTienHoanCoc.Location = new Point(50, 340);
            lbSoTienHoanCoc.Name = "lbSoTienHoanCoc";
            lbSoTienHoanCoc.Size = new Size(146, 20);
            lbSoTienHoanCoc.TabIndex = 7;
            lbSoTienHoanCoc.Text = "Nhập % số tiền cọc *";
            // 
            // txtLyDoHuy
            // 
            txtLyDoHuy.BorderColor = Color.LightGray;
            txtLyDoHuy.BorderRadius = 10;
            txtLyDoHuy.CustomizableEdges = customizableEdges9;
            txtLyDoHuy.DefaultText = "";
            txtLyDoHuy.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtLyDoHuy.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtLyDoHuy.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtLyDoHuy.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtLyDoHuy.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLyDoHuy.ForeColor = Color.Black;
            txtLyDoHuy.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtLyDoHuy.Location = new Point(50, 220);
            txtLyDoHuy.Multiline = true;
            txtLyDoHuy.Name = "txtLyDoHuy";
            txtLyDoHuy.PlaceholderText = "Nhập lý do hủy đặt sảnh...";
            txtLyDoHuy.SelectedText = "";
            txtLyDoHuy.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtLyDoHuy.Size = new Size(400, 100);
            txtLyDoHuy.TabIndex = 6;
            // 
            // lbLyDoHuy
            // 
            lbLyDoHuy.AutoSize = true;
            lbLyDoHuy.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbLyDoHuy.ForeColor = Color.FromArgb(64, 64, 64);
            lbLyDoHuy.Location = new Point(50, 195);
            lbLyDoHuy.Name = "lbLyDoHuy";
            lbLyDoHuy.Size = new Size(71, 20);
            lbLyDoHuy.TabIndex = 5;
            lbLyDoHuy.Text = "Lý do hủy";
            // 
            // dtpNgayHuy
            // 
            dtpNgayHuy.BorderColor = Color.LightGray;
            dtpNgayHuy.BorderRadius = 10;
            dtpNgayHuy.BorderThickness = 1;
            dtpNgayHuy.Checked = true;
            dtpNgayHuy.CustomizableEdges = customizableEdges5;
            dtpNgayHuy.Enabled = false;
            dtpNgayHuy.FillColor = Color.FromArgb(240, 240, 240);
            dtpNgayHuy.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpNgayHuy.Format = DateTimePickerFormat.Short;
            dtpNgayHuy.Location = new Point(50, 135);
            dtpNgayHuy.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpNgayHuy.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpNgayHuy.Name = "dtpNgayHuy";
            dtpNgayHuy.ShadowDecoration.CustomizableEdges = customizableEdges6;
            dtpNgayHuy.Size = new Size(400, 40);
            dtpNgayHuy.TabIndex = 4;
            dtpNgayHuy.Value = new DateTime(2025, 11, 9, 0, 0, 0, 0);
            // 
            // lbNgayHuy
            // 
            lbNgayHuy.AutoSize = true;
            lbNgayHuy.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbNgayHuy.ForeColor = Color.FromArgb(64, 64, 64);
            lbNgayHuy.Location = new Point(50, 110);
            lbNgayHuy.Name = "lbNgayHuy";
            lbNgayHuy.Size = new Size(71, 20);
            lbNgayHuy.TabIndex = 3;
            lbNgayHuy.Text = "Ngày hủy";
            // 
            // lbSubtitle
            // 
            lbSubtitle.AutoSize = true;
            lbSubtitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbSubtitle.ForeColor = Color.FromArgb(64, 64, 64);
            lbSubtitle.Location = new Point(50, 65);
            lbSubtitle.Name = "lbSubtitle";
            lbSubtitle.Size = new Size(331, 19);
            lbSubtitle.TabIndex = 2;
            lbSubtitle.Text = "Vui lòng nhập lý do hủy và số tiền hoàn cọc (nếu có)";
            // 
            // lbTitle
            // 
            lbTitle.AutoSize = true;
            lbTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTitle.ForeColor = Color.FromArgb(192, 0, 0);
            lbTitle.Location = new Point(50, 25);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new Size(201, 32);
            lbTitle.TabIndex = 1;
            lbTitle.Text = "⚠ Hủy đặt sảnh";
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Transparent;
            btnClose.BorderRadius = 15;
            btnClose.CustomizableEdges = customizableEdges11;
            btnClose.DisabledState.BorderColor = Color.DarkGray;
            btnClose.DisabledState.CustomBorderColor = Color.DarkGray;
            btnClose.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnClose.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnClose.FillColor = Color.Transparent;
            btnClose.Font = new Font("Comic Sans MS", 25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(448, 2);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnClose.Size = new Size(50, 50);
            btnClose.TabIndex = 0;
            btnClose.Text = "X";
            btnClose.Click += BtnClose_Click;
            // 
            // Frm_HuyDatSanh
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(500, 500);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Frm_HuyDatSanh";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Hủy đặt sảnh";
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna2GradientPanel panelMain;
        private Guna2Button btnClose;
        private Label lbTitle;
        private Label lbSubtitle;
        private Label lbNgayHuy;
        private Guna2DateTimePicker dtpNgayHuy;
        private Label lbLyDoHuy;
        private Guna2TextBox txtLyDoHuy;
        private Label lbSoTienHoanCoc;
        private Guna2TextBox txtPhanTram;
        private Label lbHelperText;
        private Panel panelButtons;
        private Guna2Button btnQuayLai;
        private Guna2Button btnXacNhanHuy;
        private Label label1;
        private Guna2TextBox txtSoTienHoanCoc;
    }
}

