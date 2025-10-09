using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Frm_ChonBan : Form
    {
        private Color _borderColor = Color.Black;
        private int _borderThickness = 2;
        public Frm_ChonBan()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            UpdateRegion(18); // bán kính 18px
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegion(18);
        }

        private void UpdateRegion(int radius)
        {
            var r = new Rectangle(0, 0, Width, Height);
            using var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            Region = new Region(path);
        }

        private void trangThaiBan2_TransferClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Đã chọn bàn này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Viền trùng với Region bo góc
            int radius = 18;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1); // -1 để không bị cắt
            using (var path = new GraphicsPath())
            {
                int d = radius * 2;
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                using var pen = new Pen(_borderColor, _borderThickness);
                // PenAlignment.Inset giúp nét nằm phía trong, không bị cắt mép
                pen.Alignment = PenAlignment.Inset;
                g.DrawPath(pen, path);
            }
        }

    }
}
