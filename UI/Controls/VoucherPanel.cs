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
using System.Drawing.Drawing2D;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class VoucherPanel : Form
    {
        public VoucherPanel()
        {
            InitializeComponent();
            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            AutoScroll = true;
            // bo tròn + viền đen nhạt
            ApplyRoundedCorners(18);
            Resize += (s, e) => ApplyRoundedCorners(18);
            Paint += VoucherPanel_Paint;
        }

        public void Render(DataTable voucherTable)
        {
            if (voucherTable == null)
            {
                Controls.Clear();
                return;
            }

            Controls.Clear();

            int y = 4;
            foreach (DataRow r in voucherTable.Rows)
            {
                DateTime han = r.Table.Columns.Contains("han_dung") && r["han_dung"] != DBNull.Value
                    ? Convert.ToDateTime(r["han_dung"]) 
                    : Convert.ToDateTime(r["tg_ket_thuc"]);

                var card = CreateVoucherCard(
                    Convert.ToString(r["code"]) ?? string.Empty,
                    Convert.ToString(r["ten"]) ?? string.Empty,
                    Convert.ToString(r["hinh_thuc"]) ?? string.Empty,
                    r["gia_tri"] == DBNull.Value ? 0m : Convert.ToDecimal(r["gia_tri"]),
                    han,
                    r.Table.Columns.Contains("da_dung") && r["da_dung"] != DBNull.Value ? Convert.ToInt32(r["da_dung"]) : 0,
                    r.Table.Columns.Contains("so_lan") && r["so_lan"] != DBNull.Value ? Convert.ToInt32(r["so_lan"]) : 0
                );

                card.Location = new Point(3, y);
                y += card.Height + 8;
                Controls.Add(card);
            }
        }

        public void SetData(string code, string ten, string hinhThuc, decimal giaTri, DateTime han, int daDung, int soLan, bool khongApDung = false)
        {
            bool percent = string.Equals(hinhThuc, "PERCENT", StringComparison.OrdinalIgnoreCase);
            lbMaVoucher.Text = code;
            lbTenVoucher.Text = ten;
            lbSoGiamGiaVoucher.Text = percent ? $"Giảm: {giaTri}%" : $"Giảm: {FormatCurrency(giaTri)}";
            lbSoLanSuDungVoucher.Text = $"Đã dùng: {daDung}/{soLan}";
            lbHanVoucher.Text = $"HSD: {han:dd/MM/yyyy}";
            panelHienThiKoApDung.Visible = khongApDung;
        }

        private Guna.UI2.WinForms.Guna2GradientPanel CreateVoucherCard(string code, string ten, string hinhThuc, decimal giaTri, DateTime han, int daDung, int soLan)
        {
            var card = new Guna.UI2.WinForms.Guna2GradientPanel()
            {
                BorderColor = Color.FromArgb(224, 224, 224),
                BorderRadius = 25,
                BorderThickness = 2,
                FillColor = Color.White,
                FillColor2 = Color.White,
                Size = new Size(482, 95)
            };

            var lbMa = new Label(){ Text = code, ForeColor = Color.MediumSlateBlue, Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), Location = new Point(14, 11), AutoSize = true };
            var lbTen = new Label(){ Text = ten, Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold), Location = new Point(14, 31), AutoSize = true };
            var lbGiam = new Label(){ Text = hinhThuc == "PERCENT" ? $"Giảm: {giaTri}%" : $"Giảm: {FormatCurrency(giaTri)}", Location = new Point(15, 64), AutoSize = true, ForeColor = Color.FromArgb(64,64,64) };
            var lbUsed = new Label(){ Text = $"Đã dùng: {daDung}/{soLan}", Location = new Point(322, 40), AutoSize = true, ForeColor = Color.FromArgb(64,64,64) };
            var lbHan = new Label(){ Text = $"HSD: {han:dd/MM/yyyy}", Location = new Point(322, 64), AutoSize = true, ForeColor = Color.FromArgb(64,64,64) };

            card.Controls.Add(lbMa);
            card.Controls.Add(lbTen);
            card.Controls.Add(lbGiam);
            card.Controls.Add(lbUsed);
            card.Controls.Add(lbHan);
            return card;
        }

        private static string FormatCurrency(decimal value)
        {
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0} đ", value);
        }

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

        private void VoucherPanel_Paint(object? sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(120, 0, 0, 0), 1.5f))
            {
                var rect = new Rectangle(1, 1, Width - 2, Height - 2);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    int d = 36;
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
