using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using QLNhaHangTiecCuoi.Share;

namespace BLL
{
    public class OrderItemInput
    {
        public int MonId { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public decimal SoLuong { get; set; }
    }

    public class OrderBLL
    {
        private readonly OrderDAL _orderDal;
        private readonly HoaDonDAL _hdDal;

        public OrderBLL(DatabaseHelper db)
        {
            _orderDal = new OrderDAL(db);
            _hdDal = new HoaDonDAL(db);
        }

        // Lưu phiếu order và trả về id
        public int SaveOrder(int chiNhanhId, int? banId, string nhanVien, IEnumerable<OrderItemInput> items)
        {
            int poId = _orderDal.CreateOrderHead(chiNhanhId, banId, nhanVien);
            foreach (var it in items)
            {
                _orderDal.InsertOrderDetail(poId, it.MonId, it.SoLuong, it.DonGia);
            }
            return poId;
        }

        // Tạo hóa đơn từ dữ liệu giỏ hàng hiện tại
        public int CreateInvoiceFromCart(int chiNhanhId, IEnumerable<OrderItemInput> items, decimal vatPercent, decimal phiDv = 0, decimal giamGia = 0, int? banId = null, string? soBan = null, string? tenNguoiBan = null)
        {
            decimal sub = 0;
            foreach (var it in items)
            {
                sub += it.DonGia * it.SoLuong;
            }
            decimal vat = Math.Round(sub * vatPercent / 100m, 0);
            decimal total = sub + vat + phiDv - giamGia;
            if (total < 0) total = 0;

            int hdId = _hdDal.CreateHoaDon(
                chiNhanhId, 
                "NHAHANG", 
                vatPercent, 
                phiDv, 
                giamGia, 
                sub, 
                total,
                khachHangId: null,
                thamChieuId: banId,
                soBanSanh: soBan,
                tenNguoiBan: tenNguoiBan
            );
            foreach (var it in items)
            {
                _hdDal.InsertHoaDonCt(hdId, "MÓN", it.MonId, it.TenMon, it.SoLuong, it.DonGia);
            }
            return hdId;
        }
    }
}


