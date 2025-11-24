using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace UI.Controls
{
    public partial class HoaDonPanel : Form
    {
        public event EventHandler? Selected;

        // Simple data bindings for demo/display purposes
        public string InvoiceCode
        {
            get
            {
                if (panelMaHoaDon != null && !panelMaHoaDon.IsDisposed)
                    return panelMaHoaDon.Text;
                return "";
            }
            set
            {
                if (panelMaHoaDon != null && !panelMaHoaDon.IsDisposed)
                    panelMaHoaDon.Text = value;
            }
        }

        public string TableName
        {
            get
            {
                if (lbSoBan != null && !lbSoBan.IsDisposed)
                    return lbSoBan.Text;
                return "";
            }
            set
            {
                if (lbSoBan != null && !lbSoBan.IsDisposed)
                    lbSoBan.Text = value;
            }
        }

        public string GuestsAndDishes
        {
            get
            {
                if (lbSoKhach_SoMon != null && !lbSoKhach_SoMon.IsDisposed)
                    return lbSoKhach_SoMon.Text;
                return "";
            }
            set
            {
                if (lbSoKhach_SoMon != null && !lbSoKhach_SoMon.IsDisposed)
                    lbSoKhach_SoMon.Text = value;
            }
        }

        public string Subtotal
        {
            get
            {
                if (lbTamTinh != null && !lbTamTinh.IsDisposed)
                    return lbTamTinh.Text;
                return "";
            }
            set
            {
                if (lbTamTinh != null && !lbTamTinh.IsDisposed)
                    lbTamTinh.Text = value;
            }
        }

        public string Vat
        {
            get
            {
                if (lbVAT != null && !lbVAT.IsDisposed)
                    return lbVAT.Text;
                return "";
            }
            set
            {
                if (lbVAT != null && !lbVAT.IsDisposed)
                    lbVAT.Text = value;
            }
        }

        public string Total
        {
            get
            {
                if (lbTongCong != null && !lbTongCong.IsDisposed)
                    return lbTongCong.Text;
                return "";
            }
            set
            {
                if (lbTongCong != null && !lbTongCong.IsDisposed)
                    lbTongCong.Text = value;
            }
        }

        public void SetStartTime(DateTime startTime)
        {
            if (lbThoiGianLap != null && !lbThoiGianLap.IsDisposed)
                lbThoiGianLap.Text = $"Bắt đầu: {startTime:HH:mm}";
        }

        public void SetNgayToChuc(TimeSpan gioToChuc, DateTime ngayToChuc)
        {
            if (lbThoiGianLap != null && !lbThoiGianLap.IsDisposed)
            {
                string gioStr = $"{(int)gioToChuc.TotalHours:D2}h{gioToChuc.Minutes:D2}";
                lbThoiGianLap.Text = $"Ngày tổ chức: {gioStr} - {ngayToChuc:dd/MM/yyyy}";
            }
        }

        public void SetVatPercent(decimal vatPercent)
        {
            if (IsDisposed || Disposing) return;
            
            try
            {
                foreach (Control ctrl in Controls)
                {
                    if (ctrl != null && !ctrl.IsDisposed && ctrl.Name == "label4")
                    {
                        ctrl.Text = $"VAT ({vatPercent:0}%)";
                        break;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // Control đã bị dispose, bỏ qua
            }
        }

        public HoaDonPanel()
        {
            InitializeComponent();

            // Ensure click anywhere in the panel selects this invoice
            Click += OnAnyClick;
            foreach (Control child in Controls)
            {
                child.Click += OnAnyClick;
            }

            // Bo tròn góc cho form hiển thị hóa đơn
            SizeChanged += (_, __) => ApplyRoundedCorners(20);
            Load += (_, __) => ApplyRoundedCorners(20);
        }

        private void OnAnyClick(object? sender, EventArgs e)
        {
            Selected?.Invoke(this, EventArgs.Empty);
        }

        // Tạo vùng hiển thị bo tròn theo bán kính
        private void ApplyRoundedCorners(int radius)
        {
            using (var path = new GraphicsPath())
            {
                int diameter = radius * 2;
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // Vẽ 4 cung tròn ở 4 góc
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();

                Region = new Region(path);
            }
        }
    }
}
