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
using UiControls;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmLogin : Form
    {
        public class DimOverlay : Form
        {
            public DimOverlay(Form owner)
            {
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.Manual;
                ShowInTaskbar = false;
                BackColor = Color.Black;
                Opacity = 0.45;            // độ tối
                Owner = owner;
                Bounds = owner.Bounds;

                owner.LocationChanged += (_, __) => this.Bounds = owner.Bounds;
                owner.SizeChanged += (_, __) => this.Bounds = owner.Bounds;
            }
        }

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnDangNhap_Click_1(object sender, EventArgs e)
        {
            using (var overlay = new DimOverlay(this))
            using (var frm = new Frm_ChonChiNhanh())
            {
                overlay.Show();
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowInTaskbar = false;

                var result = frm.ShowDialog(overlay);
                overlay.Close();

                if (result == DialogResult.OK)
                {
                    var home = new FrmTrangChu();
                    this.Hide();
                    home.FormClosed += (s, args) => this.Close(); 
                    home.Show();
                }
            }

        }
    }
}
