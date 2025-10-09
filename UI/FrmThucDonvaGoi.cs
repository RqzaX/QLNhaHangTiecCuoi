using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UiControls;

namespace UI
{
    public partial class FrmThucDonvaGoi : Form
    {
        public FrmThucDonvaGoi()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void menuGrid1_Load(object sender, EventArgs e)
        {

        }

        private void FrmThucDonvaGoi_Load(object sender, EventArgs e)
        {
            LoadData_ThucDonVaGoi();
        }
        private void LoadData_ThucDonVaGoi()
        {

            dgvThucDonVaGoi.Rows.Clear();

            AddRow("Gỏi cuốn tôm thịt", "Khai vị", 45000m, 25000m, true);
            AddRow("Salad hải sản", "Khai vị", 85000m, 45000m, true);
            AddRow("Bò nướng lá lốt", "Món chính", 120000m, 60000m, true);
            AddRow("Gà quay bơ tỏi", "Món chính", 150000m, 70000m, true);
            AddRow("Cá hấp xì dầu", "Hải sản", 280000m, 150000m, true);
            AddRow("Tôm hùm nướng phô mai", "Hải sản", 850000m, 450000m, false);
        }
        private void AddRow(string tenMon, string danhMuc, decimal giaBan, decimal giaVon, bool conHang)
        {
            decimal ln = giaBan - giaVon;
            string loiNhuanCell = $"{Money(ln)}\n({ProfitPercent(giaBan, giaVon)})";
            dgvThucDonVaGoi.Rows.Add(
                tenMon,
                danhMuc,
                Money(giaBan),
                Money(giaVon),
                loiNhuanCell,
                conHang ? "Còn hàng" : "Hết hàng",
                "edit|delete" // chỉ marker để vẽ icon ở CellPainting
            );
        }
        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");
        private static string ProfitPercent(decimal giaBan, decimal giaVon)
        {
            if (giaBan <= 0) return "0%";
            var p = (giaBan - giaVon) / giaBan * 100m;
            return Math.Round(p, 1).ToString("0.0") + "%";
        }
    }
}
