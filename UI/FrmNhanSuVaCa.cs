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
    public partial class FrmNhanSuVaCa : Form
    {
        public FrmNhanSuVaCa()
        {
            InitializeComponent();
        }

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
            // cột Liên hệ 2 dòng (phone \n email)
            string lienHe = $"{phone}\n{email}";
            dgvNhanSu.Rows.Add(
                ten, chucVu, lienHe, chiNhanh,
                ngayVao.ToString("dd/M/yyyy"),
                trangThai, "Chi tiết"
            );
        }
        private void dgvNhanSu_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Liên hệ: 2 dòng (điện thoại đậm, email xám)
            if (dgv.Columns[e.ColumnIndex].Name == "LienHe")
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);
                var parts = (e.FormattedValue?.ToString() ?? "").Split('\n');
                string phone = parts.Length > 0 ? parts[0] : "";
                string mail = parts.Length > 1 ? parts[1] : "";

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

            // Trạng thái: chip xanh/vàng
            if (dgv.Columns[e.ColumnIndex].Name == "TrangThai")
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string text = e.FormattedValue?.ToString() ?? "";
                bool active = text.Equals("Đang làm", StringComparison.OrdinalIgnoreCase);

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 26) / 2, 96, 26);
                using var path = Rounded(chip, 13);
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
            p.CloseFigure();
            return p;
        }
        private void dgvNhanSu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvNhanSu.Columns[e.ColumnIndex].Name == "ChiTiet")
            {
                string ten = dgvNhanSu.Rows[e.RowIndex].Cells["TenNV"].Value?.ToString();
                MessageBox.Show($"Xem chi tiết nhân viên: {ten}");
            }
        }

        private void FrmNhanSuVaCa_Load(object sender, EventArgs e)
        {
            LoadDataNhanSu();
        }
    }
}
