using BLL;
using QLNhaHangTiecCuoi.Share;
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
using UI.Controls;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class ApDungVoucher : Form
    {
        private readonly DatabaseHelper _db = new DatabaseHelper();
        private readonly KhuyenMaiBLL _kmBll;
        public decimal BillTotal { get; set; } = 0m; // tổng tiền hóa đơn hiện tại (truyền từ form gọi)

        // Kết quả chọn/áp dụng
        public bool IsApplied { get; private set; }
        public int? VoucherId { get; private set; }
        public int? ProgramId { get; private set; }
        public string ProgramName { get; private set; } = string.Empty;
        public string ProgramCode { get; private set; } = string.Empty; 
        public string DiscountType { get; private set; } = ""; 
        public decimal DiscountValue { get; private set; } // % hoặc số tiền
        public string ApplyScope { get; private set; } = "ALL"; // ALL | NHAHANG | TIECCUOI

        public ApDungVoucher()
        {
            InitializeComponent();
            _kmBll = new KhuyenMaiBLL(_db);
            btnThoat.Click += (s, e) => Close();
            btnApDung.Click += BtnApDung_Click;
            Shown += (s, e) => { TryLoadPreviewData(); TogglePanels(); };
            // Bo tròn form ngay khi tạo
            ApplyRoundedCorners(18);
        }

        // truyền tổng tiền hóa đơn để hiển thị số tiền giảm chính xác
        public ApDungVoucher(decimal billTotal) : this()
        {
            BillTotal = billTotal;
        }

        private void TogglePanels()
        {
            bool showVoucher = segmentedPill1.SelectedIndex == 0;
            panelKhuyenMai.Visible = showVoucher;
            panelVoucher.Visible = !showVoucher;

            segmentedPill1.Click += (s, e) =>
            {
                bool sv = segmentedPill1.SelectedIndex == 1;
                panelVoucher.Visible = sv;
                panelKhuyenMai.Visible = !sv;
            };
        }
        // Hiển thị dữ liệu mẫu khuyến mãi và voucher
        private void TryLoadPreviewData()
        {
            try
            {
                // Hiển thị tất cả CTKM (kể cả hết hiệu lực)
                var kmList = _kmBll.GetAllPrograms();
                panelKhuyenMai.Controls.Clear();
                int y = 4;
                foreach (DataRow r in kmList.Rows)
                {
                    var item = new KhuyenMaiPanel();
                    item.TopLevel = false;
                    item.FormBorderStyle = FormBorderStyle.None;
                    item.SetData(
                        Convert.ToString(r["ma_km"]) ?? string.Empty,
                        Convert.ToString(r["ten"]) ?? string.Empty,
                        Convert.ToString(r["hinh_thuc"]) ?? string.Empty,
                        r["gia_tri"] == DBNull.Value ? 0m : Convert.ToDecimal(r["gia_tri"]),
                        r["tg_ket_thuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(r["tg_ket_thuc"]),
                        Convert.ToString(r["ap_dung_loai"]) ?? string.Empty,
                        BillTotal
                    );
                    item.Cursor = Cursors.Hand;
                    EventHandler onEnter = (s, e) => { item.BackColor = Color.FromArgb(248, 248, 255); };
                    EventHandler onLeave = (s, e) => { item.BackColor = Color.White; };
                    EventHandler onClick = (s, e) =>
                    {
                        // chọn khuyến mãi và trả về
                        IsApplied = true;
                        VoucherId = null;
                        ProgramId = r.Table.Columns.Contains("km_id") && r["km_id"] != DBNull.Value ? Convert.ToInt32(r["km_id"]) : null;
                        ProgramName = Convert.ToString(r["ten"]) ?? string.Empty;
                        ProgramCode = Convert.ToString(r["ma_km"]) ?? string.Empty;
                        DiscountType = Convert.ToString(r["hinh_thuc"]) ?? string.Empty;
                        DiscountValue = r["gia_tri"] == DBNull.Value ? 0m : Convert.ToDecimal(r["gia_tri"]);
                        ApplyScope = Convert.ToString(r["ap_dung_loai"]) ?? "ALL";
                        DialogResult = DialogResult.OK;
                        Close();
                    };
                    AttachInteractiveHandlers(item, onClick, onEnter, onLeave);
                    item.Location = new Point(3, y);
                    panelKhuyenMai.Controls.Add(item);
                    item.Show();
                    y += item.Height + 8;
                }

                // Hiển thị tất cả voucher (kể cả hết hiệu lực)
                var vcList = _kmBll.GetAllVouchers();
                guna2Panel2.Controls.Clear();
                int y2 = 4;
                foreach (DataRow r in vcList.Rows)
                {
                    DateTime han = r["han_dung"] == DBNull.Value ? Convert.ToDateTime(r["tg_ket_thuc"]) : Convert.ToDateTime(r["han_dung"]);
                    var item = new VoucherPanel();
                    item.TopLevel = false;
                    item.FormBorderStyle = FormBorderStyle.None;
                    item.SetData(
                        Convert.ToString(r["code"]) ?? string.Empty,
                        Convert.ToString(r["ten"]) ?? string.Empty,
                        Convert.ToString(r["hinh_thuc"]) ?? string.Empty,
                        r["gia_tri"] == DBNull.Value ? 0m : Convert.ToDecimal(r["gia_tri"]),
                        han,
                        r.Table.Columns.Contains("da_dung") && r["da_dung"] != DBNull.Value ? Convert.ToInt32(r["da_dung"]) : 0,
                        r.Table.Columns.Contains("so_lan") && r["so_lan"] != DBNull.Value ? Convert.ToInt32(r["so_lan"]) : 0,
                        false
                    );
                    // hover + click chọn voucher
                    item.Cursor = Cursors.Hand;
                    EventHandler vEnter = (s, e) => { item.BackColor = Color.FromArgb(248, 248, 255); };
                    EventHandler vLeave = (s, e) => { item.BackColor = Color.White; };
                    EventHandler vClick = (s, e) =>
                    {
                        IsApplied = true;
                        VoucherId = r.Table.Columns.Contains("voucher_id") && r["voucher_id"] != DBNull.Value ? Convert.ToInt32(r["voucher_id"]) : null;
                        ProgramId = r.Table.Columns.Contains("km_id") && r["km_id"] != DBNull.Value ? Convert.ToInt32(r["km_id"]) : null;
                        ProgramName = Convert.ToString(r["ten"]) ?? string.Empty;
                        ProgramCode = Convert.ToString(r["code"]) ?? string.Empty;
                        DiscountType = Convert.ToString(r["hinh_thuc"]) ?? string.Empty;
                        DiscountValue = r["gia_tri"] == DBNull.Value ? 0m : Convert.ToDecimal(r["gia_tri"]);
                        DialogResult = DialogResult.OK;
                        Close();
                    };
                    AttachInteractiveHandlers(item, vClick, vEnter, vLeave);
                    item.Location = new Point(3, y2);
                    guna2Panel2.Controls.Add(item);
                    item.Show();
                    y2 += item.Height + 8;
                }
            }
            catch { /* errors */ }
        }
        // Gắn sự kiện hover và click cho tất cả control con
        private void AttachInteractiveHandlers(Control root, EventHandler click, EventHandler enter, EventHandler leave)
        {
            root.Click += click;
            root.MouseEnter += enter;
            root.MouseLeave += leave;
            foreach (Control child in root.Controls)
            {
                AttachInteractiveHandlers(child, click, enter, leave);
            }
        }
        // Áp dụng voucher
        private async void BtnApDung_Click(object? sender, EventArgs e)
        {
            string code = txtMaVoucher.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Vui lòng nhập mã voucher.");
                return;
            }

            try
            {
                var res = _kmBll.ApplyVoucherCode(code);
                if (!res.IsApplied)
                {
                    MessageBox.Show(res.Error);
                    return;
                }

                VoucherId = res.VoucherId;
                ProgramId = res.ProgramId;
                ProgramName = res.ProgramName;
                ProgramCode = res.ProgramCode;
                DiscountType = res.DiscountType;
                DiscountValue = res.DiscountValue;
                ApplyScope = res.ApplyScope;

                IsApplied = true;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi áp dụng voucher: " + ex.Message);
            }
        }
        // Vẽ bo tròn + viền đen cho form — giữ nguyên layout Designer
        private void ApplyRoundedCorners(int radius)
        {
            using (var path = new GraphicsPath())
            {
                int d = radius * 2;
                var rect = new Rectangle(0, 0, Width, Height);
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                Region = new Region(path);
            }
            Invalidate();
        }
        // Khi form thay đổi kích thước, vẽ lại bo tròn
        private void ApDungVoucher_Resize(object sender, EventArgs e)
        {
            ApplyRoundedCorners(18);
        }
        // Vẽ viền đen cho form
        private void ApDungVoucher_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.Black, 2))
            {
                var rect = new Rectangle(1, 1, Width - 2, Height - 2);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    int d = 36; // 2*radius
                    path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                    path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
