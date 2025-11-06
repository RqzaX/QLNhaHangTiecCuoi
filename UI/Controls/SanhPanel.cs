using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Controls
{
    public partial class SanhPanel : UserControl
    {
        public int SanhId { get; private set; }
        public event EventHandler ChiTietClicked;
        public event EventHandler SuaClicked;

        public SanhPanel()
        {
            InitializeComponent();
            // Thêm border và shadow effect
            this.Paint += SanhPanel_Paint;
        }

        private void SanhPanel_Paint(object sender, PaintEventArgs e)
        {
            // Vẽ border mỏng
            using (Pen pen = new Pen(Color.FromArgb(229, 231, 235), 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }
        }

        public void LoadData(int sanhId, string tenSanh, string tenChiNhanh, int sucChua, decimal phiThueCb, string trangThai = "Hoạt động")
        {
            SanhId = sanhId;
            lblSanh.Text = tenSanh;
            lblChiNhanh.Text = tenChiNhanh;
            lblSucChua.Text = $"{sucChua:N0} người";
            lblPhiThue.Text = $"{phiThueCb:N0} ₫";
            panelhoatdong.Text = trangThai;
            
            // Set màu cho status badge
            if (trangThai == "Hoạt động")
            {
                panelhoatdong.FillColor = Color.FromArgb(34, 197, 94);
                panelhoatdong.FillColor2 = Color.FromArgb(34, 197, 94);
                panelhoatdong.ForeColor = Color.White;
            }
            else
            {
                panelhoatdong.FillColor = Color.FromArgb(239, 68, 68);
                panelhoatdong.FillColor2 = Color.FromArgb(239, 68, 68);
                panelhoatdong.ForeColor = Color.White;
            }
        }

        private void panelhoatdong_Click(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SuaClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            ChiTietClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
