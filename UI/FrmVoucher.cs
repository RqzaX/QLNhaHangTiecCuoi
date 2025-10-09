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
    public partial class FrmVoucher : Form
    {
        public FrmVoucher()
        {
            InitializeComponent();
        }

        private void FrmVoucher_Load(object sender, EventArgs e)
        {
            LoadDataKhuyenMai();
        }
        private void LoadDataKhuyenMai()
        {
            dgvKhuyenMai.Rows.Clear();

            AddPromo(
                ten: "Giảm 15% toàn bộ hóa đơn",
                ma: "KHAI_TRUONG",
                loai: "Giảm %",
                giaTri: "15%",
                toiThieu: 500000m,
                giamToiDa: 200000m,
                tu: new DateTime(2025, 10, 1),
                den: new DateTime(2025, 10, 31),
                daDung: 45, gioiHan: 100,
                dangApDung: true
            );

            AddPromo(
                ten: "Giảm 100K cho hóa đơn trên 1 triệu",
                ma: "GIAM100K",
                loai: "Giảm tiền",
                giaTri: Money(100000m),
                toiThieu: 1000000m,
                giamToiDa: 100000m,
                tu: new DateTime(2025, 10, 1),
                den: new DateTime(2025, 12, 31),
                daDung: 28, gioiHan: 200,
                dangApDung: true
            );

            AddPromo(
                ten: "Tặng món tráng miệng",
                ma: "FREE_DESSERT",
                loai: "Tặng quà",
                giaTri: "-",
                toiThieu: 800000m,
                giamToiDa: null,
                tu: new DateTime(2025, 9, 1),
                den: new DateTime(2025, 9, 30),
                daDung: 120, gioiHan: 150,
                dangApDung: false
            );
        }

        private void AddPromo(string ten, string ma, string loai, string giaTri,
                              decimal toiThieu, decimal? giamToiDa,
                              DateTime tu, DateTime den,
                              int daDung, int gioiHan, bool dangApDung)
        {
            string dk = $"Tối thiểu: {Money(toiThieu)}\n" +
                        (giamToiDa.HasValue ? $"Giảm tối đa: {Money(giamToiDa.Value)}" : "");
            string tg = $"{tu:dd/M/yyyy} - {den:dd/M/yyyy}";
            string dd = $"{daDung}/{gioiHan}";
            string tt = dangApDung ? "Đang áp dụng" : "Đã hết hạn";

            dgvKhuyenMai.Rows.Add(ten, ma, loai, giaTri, dk, tg, dd, tt);
        }

        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");
    }
}
