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
    public partial class FrmVoucher : Form
    {
        public FrmVoucher()
        {
            InitializeComponent();
        }
        private const string KM_TEN = "TenCT";
        private const string KM_MA = "MaKM";
        private const string KM_LOAI = "Loai";
        private const string KM_GIA = "GiaTri";
        private const string KM_DK = "DieuKien";
        private const string KM_TG = "ThoiGian";
        private const string KM_LIMIT = "DaDung";
        private const string KM_TT = "TrangThai";

        private void FrmVoucher_Load(object sender, EventArgs e)
        {
            InitDgvKhuyenMai();
            LoadDataKhuyenMai();
        }
        private void InitDgvKhuyenMai()
        {
            var dgv = dgvKhuyenMai;

            dgv.DataSource = null;
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KM_TEN, HeaderText = "Tên chương trình", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 300 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KM_MA, HeaderText = "Mã KM", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KM_LOAI, HeaderText = "Loại", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KM_GIA, HeaderText = "Giá trị", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KM_DK, HeaderText = "Điều kiện", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, FillWeight = 280 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KM_TG, HeaderText = "Thời gian", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KM_LIMIT, HeaderText = "Đã dùng/Giới hạn", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = KM_TT, HeaderText = "Trạng thái", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            // style tổng
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgv.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgv.RowTemplate.Height = 56;


        }

        private void LoadDataKhuyenMai()
        {
            dgvKhuyenMai.Rows.Clear();

            // Ten, Ma, Loai, GiaTri(text), DieuKien(top, bottom), ThoiGian, DaDung/GioiHan, TrangThai
            AddKM(
                "Giảm 15% toàn bộ hóa đơn", "KHAI_TRUONG", "Giảm %", "15%",
                $"Tối thiểu: {Money(500000)}\nGiảm tối đa: {Money(200000)}",
                "1/10/2025 - 31/10/2025", "45/100", "Đang áp dụng");

            AddKM(
                "Giảm 100K cho hóa đơn trên 1 triệu", "GIAM100K", "Giảm tiền", $"{Money(100000)}",
                $"Tối thiểu: {Money(1000000)}\nGiảm tối đa: {Money(100000)}",
                "1/10/2025 - 31/12/2025", "28/200", "Đang áp dụng");

            AddKM(
                "Tặng món tráng miệng", "FREE_DESSERT", "Tặng quà", "-",
                $"Tối thiểu: {Money(800000)}\n",
                "1/9/2025 - 30/9/2025", "120/150", "Đã hết hạn");
        }

        private void AddKM(string ten, string ma, string loai, string giatri,
                           string dieukienTwoLines, string thoigian,
                           string dadung, string trangthai)
        {
            int r = dgvKhuyenMai.Rows.Add();
            var row = dgvKhuyenMai.Rows[r];
            row.Cells[KM_TEN].Value = ten;
            row.Cells[KM_MA].Value = ma;
            row.Cells[KM_LOAI].Value = loai;
            row.Cells[KM_GIA].Value = giatri;
            row.Cells[KM_DK].Value = dieukienTwoLines; // “top\nbottom”
            row.Cells[KM_TG].Value = thoigian;
            row.Cells[KM_LIMIT].Value = dadung;
            row.Cells[KM_TT].Value = trangthai;
        }

        private static string Money(decimal v) => string.Format("{0:#,0} đ", v).Replace(",", ".");

        // =============== CUSTOM DRAW ===============
        private static System.Drawing.Drawing2D.GraphicsPath Round(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure(); return p;
        }

        private void dgvKhuyenMai_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var dgv = (DataGridView)sender;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            string col = dgv.Columns[e.ColumnIndex].Name;

            // Mã KM -> chip xám nhạt
            if (col == KM_MA)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string text = Convert.ToString(e.FormattedValue) ?? "";
                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2,
                                         Math.Max(110, TextRenderer.MeasureText(text, new Font("Segoe UI Semibold", 9f)).Width + 22),
                                         28);
                using var path = Round(chip, 14);
                using var fill = new SolidBrush(Color.FromArgb(243, 244, 246));
                using var br = new SolidBrush(Color.FromArgb(55, 65, 81));
                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Điều kiện -> 2 dòng
            if (col == KM_DK)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                var parts = (Convert.ToString(e.FormattedValue) ?? "").Split('\n');
                string top = parts.ElementAtOrDefault(0) ?? "";
                string bottom = parts.ElementAtOrDefault(1) ?? "";

                var r = Rectangle.Inflate(e.CellBounds, -8, -6);
                using var brTop = new SolidBrush(e.CellStyle.ForeColor);
                using var brSub = new SolidBrush(Color.FromArgb(110, 119, 135));
                using var fTop = new Font(e.CellStyle.Font, FontStyle.Regular);
                using var fSub = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size - 1f);

                g.DrawString(top, fTop, brTop, new RectangleF(r.X, r.Y + 2, r.Width, r.Height / 2f),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
                if (!string.IsNullOrWhiteSpace(bottom))
                    g.DrawString(bottom, fSub, brSub, new RectangleF(r.X, r.Y + r.Height / 2f - 2, r.Width, r.Height / 2f),
                        new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Trạng thái -> chip xanh/ xám
            if (col == KM_TT)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string text = Convert.ToString(e.FormattedValue) ?? "";
                bool active = text.Equals("Đang áp dụng", StringComparison.OrdinalIgnoreCase);

                var chip = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 28) / 2, 120, 28);
                using var path = Round(chip, 14);
                using var fill = new SolidBrush(active ? Color.FromArgb(209, 250, 229) : Color.FromArgb(243, 244, 246));
                using var br = new SolidBrush(active ? Color.FromArgb(16, 128, 67) : Color.FromArgb(55, 65, 81));
                g.FillPath(fill, path);
                g.DrawString(text, new Font("Segoe UI Semibold", 9f), br, chip,
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                e.Paint(e.ClipBounds, DataGridViewPaintParts.Border);
                return;
            }

        }
    }
}
