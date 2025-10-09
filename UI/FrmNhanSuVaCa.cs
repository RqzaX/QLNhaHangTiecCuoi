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
    public partial class FrmNhanSuVaCa : Form
    {
        public FrmNhanSuVaCa()
        {
            InitializeComponent();
        }
        private const string NS_TEN = "TenNV";
        private const string NS_CV = "ChucVu";
        private const string NS_LH = "LienHe";
        private const string NS_CN = "ChiNhanh";
        private const string NS_NGAY = "NgayVao";
        private const string NS_TT = "TrangThai";
        private const string NS_TTAC = "ThaoTac";
        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void LoadDataNhanSu()
        {
            dgvNhanSu.Rows.Clear();

            AddNV("Nguyễn Văn X", "Quản lý", "0901111111", "nguyenvanx@email.com",
                  "Chi nhánh Quận 1", new DateTime(2023, 1, 15), "Đang làm");

            AddNV("Trần Thị Y", "Phục vụ", "0902222222", "tranthiy@email.com",
                  "Chi nhánh Quận 1", new DateTime(2023, 6, 20), "Đang làm");

            AddNV("Lê Minh Z", "Đầu bếp", "0903333333", "leminhz@email.com",
                  "Chi nhánh Quận 1", new DateTime(2023, 3, 10), "Đang làm");

            AddNV("Phạm Thu T", "Thu ngân", "0904444444", "phamthut@email.com",
                  "Chi nhánh Quận 3", new DateTime(2023, 9, 5), "Nghỉ phép");
        }

        private void AddNV(string ten, string chucVu, string phone, string email,
                   string chiNhanh, DateTime ngayVao, string trangThai)
        {
            string lienHe = $"{phone}\n{email}";
            dgvNhanSu.Rows.Add(ten, chucVu, lienHe, chiNhanh, ngayVao.ToString("dd/M/yyyy"),
                               trangThai, "Chi tiết");
        }
        private void dgvNhanSu_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var col = dgv.Columns[e.ColumnIndex].Name;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Liên hệ: 2 dòng (đt đậm, email xám)
            if (col == NS_LH)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                var parts = (e.FormattedValue?.ToString() ?? "").Split('\n');
                string phone = parts.ElementAtOrDefault(0) ?? "";
                string mail = parts.ElementAtOrDefault(1) ?? "";

                var r = Rectangle.Inflate(e.CellBounds, -8, -6);
                using var br1 = new SolidBrush(e.CellStyle.ForeColor);
                using var br2 = new SolidBrush(Color.FromArgb(110, 119, 135));
                using var f1 = new Font(e.CellStyle.Font, FontStyle.Regular);
                using var f2 = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size - 1f);

                g.DrawString(phone, f1, br1, new RectangleF(r.X, r.Y + 2, r.Width, r.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                g.DrawString(mail, f2, br2, new RectangleF(r.X, r.Y + r.Height / 2f - 2, r.Width, r.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Trạng thái: chip (xanh "Đang làm" / vàng "Nghỉ phép")
            if (col == NS_TT)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string text = Convert.ToString(e.FormattedValue) ?? "";
                bool active = text.Equals("Đang làm", StringComparison.OrdinalIgnoreCase);

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2, 92, 28);
                using var path = Rounded(chip, 14);
                using var fill = new SolidBrush(active ? Color.FromArgb(208, 247, 225) : Color.FromArgb(255, 239, 185));
                using var br = new SolidBrush(active ? Color.FromArgb(16, 128, 67) : Color.FromArgb(159, 108, 0));

                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure(); return p;
        }
        private void dgvNhanSu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvNhanSu.Columns[e.ColumnIndex].Name != NS_TTAC) return;

            string ten = dgvNhanSu.Rows[e.RowIndex].Cells[NS_TEN].Value?.ToString();
            string chuc = dgvNhanSu.Rows[e.RowIndex].Cells[NS_CV].Value?.ToString();
            string cn = dgvNhanSu.Rows[e.RowIndex].Cells[NS_CN].Value?.ToString();
            string ngay = dgvNhanSu.Rows[e.RowIndex].Cells[NS_NGAY].Value?.ToString();

            MessageBox.Show($"Chi tiết nhân sự:\n- Tên: {ten}\n- Chức vụ: {chuc}\n- Chi nhánh: {cn}\n- Ngày vào: {ngay}",
                            "Nhân sự", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        

        private void FrmNhanSuVaCa_Load(object sender, EventArgs e)
        {
            LoadDataNhanSu();
            
        }


        private void cbbNhanSu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
