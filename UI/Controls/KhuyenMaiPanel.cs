using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class KhuyenMaiPanel : Form
    {
        public KhuyenMaiPanel()
        {
            InitializeComponent();
            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            AutoScroll = true;
            ApplyRoundedCorners(18);
            Resize += (s, e) => ApplyRoundedCorners(18);
            Paint += KhuyenMaiPanel_Paint;
        }

        public void SetData(string ma, string ten, string hinhThuc, decimal giaTri, DateTime han, string scope, decimal billTotal)
        {
            var percent = string.Equals(hinhThuc, "PERCENT", StringComparison.OrdinalIgnoreCase);
            lbMaKM.Text = ma;
            lbTenKM.Text = ten;
            lbSoGiamGiaKM.Text = percent ? $"% Giảm {giaTri} %" : $"Giảm {FormatCurrency(giaTri)}";
            lbHanKM.Text = han.ToString("dd/MM/yyyy");
            panelLoaiApDung.Text = ScopeToText(scope);
            // Tính số tiền được giảm dựa trên tổng tiền hóa đơn hiện tại
            decimal discountAmount = 0m;
            if (percent)
            {
                discountAmount = Math.Round(billTotal * (giaTri / 100m), 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                discountAmount = Math.Min(giaTri, billTotal);
            }
            lbSoTienGiam.Text = FormatCurrency(discountAmount);
        }

        private static string FormatCurrency(decimal value)
        {
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0} đ", value);
        }

        private static string ScopeToText(string? scope)
        {
            if (string.IsNullOrWhiteSpace(scope)) return "Tất cả";
            switch (scope.Trim().ToUpperInvariant())
            {
                case "ALL": return "Tất cả";
                case "NHAHANG": return "Nhà hàng";
                case "TIECCUOI": return "Tiệc cưới";
                default: return scope;
            }
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

        private void KhuyenMaiPanel_Paint(object? sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(120, 0, 0, 0), 1.5f))
            {
                var rect = new Rectangle(1, 1, Width - 2, Height - 2);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    int d = 36; // 2*radius (18)
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
