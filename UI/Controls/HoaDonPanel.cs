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
            get => panelMaHoaDon.Text;
            set => panelMaHoaDon.Text = value;
        }

        public string TableName
        {
            get => lbSoBan.Text;
            set => lbSoBan.Text = value;
        }

        public string GuestsAndDishes
        {
            get => lbSoKhach_SoMon.Text;
            set => lbSoKhach_SoMon.Text = value;
        }

        public string Subtotal
        {
            get => lbTamTinh.Text;
            set => lbTamTinh.Text = value;
        }

        public string Vat
        {
            get => lbVAT.Text;
            set => lbVAT.Text = value;
        }

        public string Total
        {
            get => lbTongCong.Text;
            set => lbTongCong.Text = value;
        }

        public void SetStartTime(DateTime startTime)
        {
            lbThoiGianLap.Text = $"Bắt đầu: {startTime:HH:mm}";
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
