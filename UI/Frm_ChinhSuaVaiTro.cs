using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_ChinhSuaVaiTro : Form
    {
        public Frm_ChinhSuaVaiTro()
        {
            InitializeComponent();
            SetupIcons();
        }

        private void SetupIcons()
        {
            btnLuu.Paint += BtnLuu_Paint;
        }

        private void PicShield_Paint(object sender, PaintEventArgs e)
        {
            var pic = sender as PictureBox;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Color shieldColor = Color.FromArgb(94, 148, 255);
            using (var pen = new Pen(shieldColor, 2f))
            using (var brush = new SolidBrush(Color.FromArgb(30, shieldColor)))
            {
                var centerX = pic.Width / 2f;
                var centerY = pic.Height / 2f;
                var size = 14f;

                PointF[] shieldPoints = {
                    new PointF(centerX, centerY - size/2),
                    new PointF(centerX + size*0.4f, centerY - size*0.2f),
                    new PointF(centerX + size*0.4f, centerY + size*0.2f),
                    new PointF(centerX, centerY + size/2),
                    new PointF(centerX - size*0.4f, centerY + size*0.2f),
                    new PointF(centerX - size*0.4f, centerY - size*0.2f)
                };
                g.FillPolygon(brush, shieldPoints);
                g.DrawPolygon(pen, shieldPoints);
            }
        }

        private void BtnLuu_Paint(object sender, PaintEventArgs e)
        {
            var btn = sender as Guna.UI2.WinForms.Guna2Button;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw checkmark icon on the left
            using (var pen = new Pen(Color.White, 2.5f))
            {
                int iconX = 15;
                int iconY = btn.Height / 2;
                // Checkmark
                g.DrawLine(pen, iconX, iconY, iconX + 4, iconY + 4);
                g.DrawLine(pen, iconX + 4, iconY + 4, iconX + 10, iconY - 2);
            }

            // Text is drawn by the button itself, offset to the right of icon
        }

        public Frm_ChinhSuaVaiTro(string tenVaiTro, string maVaiTro, string moTa) : this()
        {
            txtTenVaiTro.Text = tenVaiTro;
            txtMaVaiTro.Text = maVaiTro;
            txtMoTa.Text = moTa;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtTenVaiTro.Text))
            {
                MessageBox.Show("Vui lòng nhập tên vai trò!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenVaiTro.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMaVaiTro.Text))
            {
                MessageBox.Show("Vui lòng nhập mã vai trò!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaVaiTro.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string TenVaiTro => txtTenVaiTro.Text.Trim();
        public string MaVaiTro => txtMaVaiTro.Text.Trim();
        public string MoTa => txtMoTa.Text.Trim();
    }
}

