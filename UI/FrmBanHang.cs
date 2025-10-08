using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.UI.Notifications;

namespace UI
{
    public partial class FrmBanHang : Form
    {
        public FrmBanHang()
        {
            InitializeComponent();
        }

        private void FrmBanHang_Load(object sender, EventArgs e)
        {

        }

        private void btnTenMon_Click(object sender, EventArgs e)
        {
            ThongBaoGoc.ShowSuccess(this, "Đã thêm món Tôm nướng phô mai", autoHide: true, durationMs: 2500);
        }
    }
}
