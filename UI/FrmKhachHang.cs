using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmKhachHang : Form
    {
        private const string KH_TEN = "TenKH";
        private const string KH_LH = "LienHe";
        private const string KH_HANG = "Hang";
        private const string KH_TONG = "TongChiTieu";
        private const string KH_SOLAN = "SoLanDen";
        private const string KH_DIEM = "DiemTichLuy";
        private const string KH_CUOI = "LanCuoi";
        private const string KH_TTAC = "ThaoTac";
        public FrmKhachHang()
        {
            InitializeComponent();
            
           
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void dgvKhachHang_MouseEnter(object sender, EventArgs e)
        {

        }

        private void dgvKhachHang_MouseLeave(object sender, EventArgs e)
        {

        }
        private void dgvKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void InitDgvKhachHang()
        {
            var dgv = dgvKhachHang;
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (dgv.Columns.Count == 0)
            {
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_TEN, HeaderText = "Tên KH", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 260 });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_LH, HeaderText = "Liên hệ", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 260 });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_HANG, HeaderText = "Hạng", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_TONG, HeaderText = "Tổng chi tiêu", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_SOLAN, HeaderText = "Số lần đến", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_DIEM, HeaderText = "Điểm tích lũy", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KH_CUOI, HeaderText = "Lần cuối", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

                // “Chi tiết” dạng Link (nút bấm)
                dgv.Columns.Add(new DataGridViewLinkColumn
                {
                    Name = KH_TTAC,
                    HeaderText = "Thao tác",
                    Text = "Chi tiết",
                    UseColumnTextForLinkValue = true,
                    LinkBehavior = LinkBehavior.HoverUnderline,
                    LinkColor = Color.FromArgb(23, 82, 255),
                    ActiveLinkColor = Color.FromArgb(23, 82, 255),
                    VisitedLinkColor = Color.FromArgb(23, 82, 255)
                });
            }

            // Style
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgv.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.RowTemplate.Height = 56;

            // 2 cột hiển thị 2 dòng
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.Columns[KH_TEN].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.Columns[KH_LH].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Sự kiện custom + click + hover

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
                          string hang, decimal tongChi, int soLan, int diem, DateTime lanCuoi)
        {
            string tenCell = $"{ten}\nSN: {sn:dd/M/yyyy}";
            string lhCell = $"{phone}\n{email}";
            dgvKhachHang.Rows.Add(
                tenCell, lhCell, hang,
                Money(tongChi),
                $"{soLan} lần",
                diem.ToString(),                    // sẽ vẽ kèm sao
                lanCuoi.ToString("d/M/yyyy"),
                "Chi tiết"
            );
        }

        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");

        private void FrmKhachHang_Load(object sender, EventArgs e)
        {
            LoadDataKhachHang();
            InitDgvKhachHang();
        }

        private void dgvKhachHang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvKhachHang.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvKhachHang.Columns[e.ColumnIndex].Name != KH_TTAC) return;

            string ten = (dgvKhachHang.Rows[e.RowIndex].Cells[KH_TEN].Value?.ToString() ?? "").Split('\n')[0];
            MessageBox.Show($"Xem chi tiết khách hàng: {ten}", "Khách hàng",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvKhachHang_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            string col = dgv.Columns[e.ColumnIndex].Name;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Tên KH / Liên hệ: 2 dòng
            if (col == KH_TEN || col == KH_LH)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                var parts = (e.FormattedValue?.ToString() ?? "").Split('\n');
                string top = parts.ElementAtOrDefault(0) ?? "";
                string sub = parts.ElementAtOrDefault(1) ?? "";

                var r = Rectangle.Inflate(e.CellBounds, -8, -6);
                using var fTop = new Font(e.CellStyle.Font, FontStyle.Regular);
                using var fSub = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size - 1f);
                using var brTop = new SolidBrush(e.CellStyle.ForeColor);
                using var brSub = new SolidBrush(Color.FromArgb(110, 119, 135));

                g.DrawString(top, fTop, brTop, new RectangleF(r.X, r.Y + 2, r.Width, r.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                g.DrawString(sub, fSub, brSub, new RectangleF(r.X, r.Y + r.Height / 2f - 2, r.Width, r.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Hạng: chip màu
            if (col == KH_HANG)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string hang = Convert.ToString(e.FormattedValue) ?? "";
                var (bg, fg) = HangColors(hang);

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 26) / 2, 96, 26);
                using var path = Rounded(chip, 13);
                using var fill = new SolidBrush(bg);
                using var br = new SolidBrush(fg);

                g.FillPath(fill, path);
                g.DrawString(hang, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Điểm tích lũy: ngôi sao + số
            if (col == KH_DIEM)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                int x = e.CellBounds.X + 8;
                int y = e.CellBounds.Y + (e.CellBounds.Height - 18) / 2;

                DrawStar(g, new Rectangle(x, y, 18, 18), Color.FromArgb(245, 158, 11));
                using var f = new Font(e.CellStyle.Font, FontStyle.Regular);
                using var br = new SolidBrush(e.CellStyle.ForeColor);
                g.DrawString(e.FormattedValue?.ToString() ?? "0", f, br, new PointF(x + 24, y + 1));

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }
        }
        private (Color bg, Color fg) HangColors(string hang)
        {
            switch (hang.Trim().ToLowerInvariant())
            {
                case "vàng": return (Color.FromArgb(255, 246, 204), Color.FromArgb(161, 98, 7));
                case "bạc": return (Color.FromArgb(229, 231, 235), Color.FromArgb(75, 85, 99));
                case "vip": return (Color.FromArgb(237, 233, 254), Color.FromArgb(91, 33, 182));
                default: return (Color.FromArgb(229, 231, 235), Color.FromArgb(55, 65, 81)); // Thành viên
            }
        }

        private static GraphicsPath Rounded(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private void DrawStar(Graphics g, Rectangle r, Color c)
        {
            using var sb = new SolidBrush(c);
            PointF[] pts = new PointF[10];
            double cx = r.Left + r.Width / 2.0, cy = r.Top + r.Height / 2.0;
            double R = r.Width / 2.0, r2 = R * 0.5, a = -Math.PI / 2;
            for (int i = 0; i < 10; i++)
            {
                double rad = (i % 2 == 0) ? R : r2;
                pts[i] = new PointF((float)(cx + rad * Math.Cos(a)), (float)(cy + rad * Math.Sin(a)));
                a += Math.PI / 5.0;
            }
            g.FillPolygon(sb, pts);
        }

        private void dgvKhachHang_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvKhachHang.Cursor =
                (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                 dgvKhachHang.Columns[e.ColumnIndex].Name == KH_TTAC)
                ? Cursors.Hand : Cursors.Default;
        }

        private void dgvKhachHang_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvKhachHang.Cursor = Cursors.Default;
        }
    }
}

