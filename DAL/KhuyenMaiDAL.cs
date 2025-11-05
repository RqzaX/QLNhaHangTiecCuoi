using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace DAL
{
    public class KhuyenMaiDAL
    {
        private readonly DatabaseHelper _db;

        public KhuyenMaiDAL(DatabaseHelper db)
        {
            _db = db;
        }

        // Lấy CTKM còn hiệu lực (mẫu hiển thị)
        public DataTable GetOneActiveProgramPreview()
        {
            var sql = @"SELECT TOP 1 km_id, ma_km, ten, hinh_thuc, gia_tri, tg_ket_thuc, ap_dung_loai
                        FROM chuong_trinh_km
                        WHERE SYSUTCDATETIME() BETWEEN tg_bat_dau AND tg_ket_thuc
                        ORDER BY tg_ket_thuc";
            return _db.GetDataTable(sql);
        }

        // Lấy 1 voucher còn hiệu lực (mẫu hiển thị)
        public DataTable GetOneActiveVoucherPreview()
        {
            var sql = @"SELECT TOP 1 v.voucher_id, v.code, v.so_lan, v.da_dung, v.han_dung,
                                k.ten, k.hinh_thuc, k.gia_tri, k.tg_ket_thuc
                         FROM voucher v
                         JOIN chuong_trinh_km k ON k.km_id = v.km_id
                         WHERE (v.han_dung IS NULL OR v.han_dung >= CAST(GETUTCDATE() AS DATE))
                           AND SYSUTCDATETIME() <= k.tg_ket_thuc
                         ORDER BY v.voucher_id DESC";
            return _db.GetDataTable(sql);
        }

        // Tìm voucher theo code, kèm thông tin CTKM
        public DataTable FindVoucherByCode(string code)
        {
            var sql = @"SELECT TOP 1 v.voucher_id, v.code, v.so_lan, v.da_dung, v.han_dung,
                                k.km_id, k.ma_km, k.ten, k.hinh_thuc, k.gia_tri, k.tg_bat_dau, k.tg_ket_thuc, k.ap_dung_loai
                         FROM voucher v
                         JOIN chuong_trinh_km k ON k.km_id = v.km_id
                         WHERE v.code = @code";
            var p = new[] { new SqlParameter("@code", code) };
            return _db.GetDataTable(sql, p);
        }

        // Danh sách CTKM còn hiệu lực
        public DataTable GetActivePrograms(string scope = null)
        {
            var sql = @"SELECT km_id, ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai
                        FROM chuong_trinh_km
                        WHERE GETDATE() BETWEEN tg_bat_dau AND tg_ket_thuc
                          AND (@scope IS NULL OR ap_dung_loai = 'ALL' OR ap_dung_loai = @scope)
                        ORDER BY tg_ket_thuc";
            var p = new[] { new SqlParameter("@scope", (object?)scope ?? DBNull.Value) };
            return _db.GetDataTable(sql, p);
        }

        // Danh sách voucher còn hiệu lực (lọc theo scope của CTKM)
        public DataTable GetActiveVouchers(string scope = null)
        {
            var sql = @"SELECT v.voucher_id, v.code, v.so_lan, v.da_dung, v.han_dung,
                               k.km_id, k.ten, k.hinh_thuc, k.gia_tri, k.tg_ket_thuc
                        FROM voucher v
                        JOIN chuong_trinh_km k ON k.km_id = v.km_id
                        WHERE (v.han_dung IS NULL OR v.han_dung >= CAST(GETDATE() AS DATE))
                          AND GETDATE() <= k.tg_ket_thuc
                          AND (@scope IS NULL OR k.ap_dung_loai = 'ALL' OR k.ap_dung_loai = @scope)
                        ORDER BY v.voucher_id DESC";
            var p = new[] { new SqlParameter("@scope", (object?)scope ?? DBNull.Value) };
            return _db.GetDataTable(sql, p);
        }

        // Danh sách tất cả CTKM (không lọc ngày/scopes)
        public DataTable GetAllPrograms()
        {
            var sql = @"SELECT km_id, ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai
                        FROM chuong_trinh_km
                        ORDER BY tg_bat_dau DESC, km_id DESC";
            return _db.GetDataTable(sql);
        }

        // Danh sách tất cả voucher (kèm CTKM, không lọc ngày)
        public DataTable GetAllVouchers()
        {
            var sql = @"SELECT v.voucher_id, v.code, v.so_lan, v.da_dung, v.han_dung,
                               k.km_id, k.ten, k.hinh_thuc, k.gia_tri, k.tg_ket_thuc
                        FROM voucher v
                        JOIN chuong_trinh_km k ON k.km_id = v.km_id
                        ORDER BY v.voucher_id DESC";
            return _db.GetDataTable(sql);
        }
    }
}


