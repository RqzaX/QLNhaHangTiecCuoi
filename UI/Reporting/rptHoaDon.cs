using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace UI.Reporting
{
    public partial class rptHoaDon : DevExpress.XtraReports.UI.XtraReport
    {
        public rptHoaDon()
        {
            InitializeComponent();
            //SetFormatting();

        }
        private void SetFormatting()
        {
            lbSoLuong.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[sp_InHoaDonChoKhach].[Result2].[so_luong]"));
            lbSoLuong.TextFormatString = "{0:#,0}";
            // Định dạng cho thành tiền
            lbThanhTien.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[sp_InHoaDonChoKhach].[Result2].[thanh_tien]"));
            lbThanhTien.TextFormatString = "{0:#,0} đ";
        }
    }
}
