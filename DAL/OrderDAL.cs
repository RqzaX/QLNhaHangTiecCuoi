using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace DAL
{
    public class OrderDAL
    {
        private readonly DatabaseHelper _db;
        public OrderDAL(DatabaseHelper db) { _db = db; }

        // Tạo phiếu order (đầu)
        public int CreateOrderHead(int chiNhanhId, int? banId, string nhanVien)
        {
            string sql = @"INSERT INTO phieu_order(chi_nhanh_id, ban_id, ngay_gio, nhan_vien, trang_thai)
                           OUTPUT INSERTED.phieu_order_id
                           VALUES(@cn, @ban, SYSUTCDATETIME(), @nv, N'ĐANG PHỤC VỤ')";
            var p = new[]
            {
                new SqlParameter("@cn", chiNhanhId),
                new SqlParameter("@ban", (object?)banId ?? DBNull.Value),
                new SqlParameter("@nv", (object?)nhanVien ?? DBNull.Value)
            };
            var idObj = _db.ExecuteScalar(sql, p);
            return Convert.ToInt32(idObj);
        }

        // Thêm chi tiết món vào phiếu order
        public void InsertOrderDetail(int phieuOrderId, int monId, decimal soLuong, decimal donGia)
        {
            string sql = @"INSERT INTO phieu_order_ct(phieu_order_id, mon_id, so_luong, don_gia)
                           VALUES(@po, @mon, @sl, @dg)";
            var p = new[]
            {
                new SqlParameter("@po", phieuOrderId),
                new SqlParameter("@mon", monId),
                new SqlParameter("@sl", soLuong),
                new SqlParameter("@dg", donGia)
            };
            _db.ExecuteNonQuery(sql, p);
        }

        // Cập nhật trạng thái phiếu
        public void UpdateOrderStatus(int phieuOrderId, string trangThai)
        {
            string sql = "UPDATE phieu_order SET trang_thai = @tt WHERE phieu_order_id = @id";
            var p = new[] { new SqlParameter("@tt", trangThai), new SqlParameter("@id", phieuOrderId) };
            _db.ExecuteNonQuery(sql, p);
        }

        // Kiểm tra xem đã có phieu_order đang phục vụ cho bàn này chưa
        public int? GetActiveOrderIdByBanId(int chiNhanhId, int banId)
        {
            string sql = @"SELECT TOP 1 phieu_order_id 
                          FROM phieu_order 
                          WHERE chi_nhanh_id = @cn AND ban_id = @ban AND trang_thai = N'ĐANG PHỤC VỤ'
                          ORDER BY ngay_gio DESC";
            var p = new[]
            {
                new SqlParameter("@cn", chiNhanhId),
                new SqlParameter("@ban", banId)
            };
            var result = _db.ExecuteScalar(sql, p);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : (int?)null;
        }
    }
}


