using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FrmBaoCao : Form
    {
        public FrmBaoCao()
        {
            InitializeComponent();
        }

        private void FrmBaoCao_Load(object sender, EventArgs e)
        {
           
            var chart = new UiControls.MiniBarChart
            {
                Dock = DockStyle.Fill,
                Title = "Doanh thu & Lợi nhuận (Triệu VNĐ)",
                Labels = new[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10" },
                SeriesA = new float[] { 450, 520, 490, 660, 730, 690, 860, 940, 780, 980 },
                SeriesB = new float[] { 180, 210, 170, 260, 290, 270, 340, 370, 320, 380 }
            };
            panel1.Controls.Add(chart); // hoặc this.Controls.Add(chart);
        }

    }

}
