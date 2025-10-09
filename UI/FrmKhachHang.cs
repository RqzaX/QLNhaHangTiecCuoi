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
    public partial class FrmKhachHang : Form
    {
        public FrmKhachHang()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dgvKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgvKhachHang_CellFormatting_ColorHang(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvKhachHang.Columns[e.ColumnIndex].Name != "Hang") return;

            string val = (e.Value ?? "").ToString().Trim();

            // Màu chữ theo hạng
            if (val.Equals("VIP", StringComparison.OrdinalIgnoreCase))
                e.CellStyle.ForeColor = Color.FromArgb(126, 34, 206);      // tím
            else if (val.Equals("Vàng", StringComparison.OrdinalIgnoreCase))
                e.CellStyle.ForeColor = Color.FromArgb(161, 98, 7);        // vàng đậm
            else if (val.Equals("Bạc", StringComparison.OrdinalIgnoreCase))
                e.CellStyle.ForeColor = Color.FromArgb(71, 85, 105);       // xám
            else
                e.CellStyle.ForeColor = Color.FromArgb(23, 23, 23);        // mặc định

            e.CellStyle.Font = new Font("Segoe UI Semibold", 10f);         // cho chữ “Hạng” nổi hơn
        }
        private void LoadDataKhachHang()
        {
            dgvKhachHang.Rows.Clear();

            AddKH_Simple("Nguyễn Văn A", new DateTime(1990, 5, 15), "0901234567", "nguyenvana@email.com",
                         "Vàng", 25000000m, 15, 2500, new DateTime(2025, 10, 5));

            AddKH_Simple("Trần Thị B", new DateTime(1992, 8, 20), "0912345678", "tranthib@email.com",
                         "Bạc", 12000000m, 8, 1200, new DateTime(2025, 9, 28));

            AddKH_Simple("Lê Minh C", new DateTime(1988, 12, 10), "0923456789", "leminhc@email.com",
                         "VIP", 45000000m, 25, 4500, new DateTime(2025, 10, 6));

            AddKH_Simple("Phạm Thu D", new DateTime(1995, 3, 25), "0934567890", "phamthud@email.com",
                         "Thành viên", 3500000m, 3, 350, new DateTime(2025, 9, 15));
        }

        private void AddKH_Simple(string ten, DateTime sn, string phone, string email,
                                  string hang, decimal chiTieu, int soLan, int diem, DateTime lanCuoi)
        {
            string tenCell = $"{ten}\nSN: {sn:dd/M/yyyy}";
            string lienHe = $"📞 {phone}\n✉ {email}"; // hoặc bỏ icon nếu không muốn

            dgvKhachHang.Rows.Add(
                tenCell,
                lienHe,
                hang,                               // màu sẽ đổi ở CellFormatting
                Money(chiTieu),
                $"{soLan} lần",
                diem.ToString(),                    // chỉ là số, KHÔNG vẽ sao
                $"{lanCuoi:dd/M/yyyy}",
                "Chi tiết"
            );
        }

        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");

        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            LoadDataKhachHang();
        }

        private void dgvKhachHang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvKhachHang.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }
    }
}
