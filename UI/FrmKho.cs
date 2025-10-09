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
    public partial class FrmKho : Form
    {
        public FrmKho()
        {
            InitializeComponent();
        }

        private void roundedButton1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FrmKho_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            dgvKho.Rows.Clear();

            AddRow("Thịt bò Úc", "kg", 45, 20, 15, 13500000m);
            AddRow("Tôm sú", "kg", 12, 15, 8, 7200000m);
            AddRow("Cá hồi Na Uy", "kg", 8, 10, 6, 6400000m);
            AddRow("Rau xà lách", "kg", 25, 10, 12, 500000m);
            AddRow("Bia Tiger", "thùng", 45, 30, 20, 13500000m);
            AddRow("Coca Cola", "thùng", 18, 20, 15, 3600000m);
        }
        private void AddRow(string ten, string dv, int tonKho, int tonToiThieu, int dungTB, decimal giaTri)
        {
            string giaTriStr = string.Format("{0:#,0} đ", giaTri).Replace(",", ".");
            bool sapHet = tonKho <= (tonToiThieu + (int)Math.Round(1.5 * dungTB));
            string trangThai = sapHet ? "Sắp hết" : "Đủ hàng";

            dgvKho.Rows.Add(ten, dv, tonKho, tonToiThieu, dungTB, giaTriStr, trangThai, "Chi tiết");
        }
    }
}
