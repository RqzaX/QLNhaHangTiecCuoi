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
    public partial class Frm_TaoDatBan : RoundedBorderForm
    {
        public Frm_TaoDatBan()
        {
            InitializeComponent();
            this.CornerRadius = 15;
            this.BorderColor = Color.Black;
            this.BorderThickness = 2;
            this.BackColor = Color.White; // nền của form
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void roundedTextBox4_Load(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
