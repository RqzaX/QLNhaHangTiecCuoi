using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace DAL
{
    public class HoaDonDAL
    {
        private readonly DatabaseHelper _db;
        public HoaDonDAL(DatabaseHelper db) { _db = db; }

        // Tạo hóa đơn
        public int CreateHoaDon(int chiNhanhId, string loai, decimal vatPercent, decimal phiDv,
                                decimal giamGia, decimal tongTruocThue, decimal tongSauThue, 
                                int? khachHangId = null, int? thamChieuId = null)
        {
            string sql = @"INSERT INTO hoa_don(chi_nhanh_id, khach_hang_id, loai, tham_chieu_id, ngay_lap, vat, phi_dv, giam_gia, tong_truoc_thue, tong_sau_thue, trang_thai)
                           OUTPUT INSERTED.hoa_don_id
                           VALUES(@cn, @kh, @loai, @tc, SYSUTCDATETIME(), @vat, @phi, @giam, @truoc, @sau, N'CHỜ TT')";
            var p = new[]
            {
                new SqlParameter("@cn", chiNhanhId),
                new SqlParameter("@kh", (object?)khachHangId ?? DBNull.Value),
                new SqlParameter("@loai", loai),
                new SqlParameter("@tc", (object?)thamChieuId ?? DBNull.Value),
                new SqlParameter("@vat", vatPercent),
                new SqlParameter("@phi", phiDv),
                new SqlParameter("@giam", giamGia),
                new SqlParameter("@truoc", tongTruocThue),
                new SqlParameter("@sau", tongSauThue)
            };
            var id = _db.ExecuteScalar(sql, p);
            return Convert.ToInt32(id);
        }

        // Thêm chi tiết hóa đơn
        public void InsertHoaDonCt(int hoaDonId, string loaiHang, int refId, string tenHang, decimal soLuong, decimal donGia)
        {
            string sql = @"INSERT INTO hoa_don_ct(hoa_don_id, loai_hang, ref_id, ten_hang, so_luong, don_gia)
                           VALUES(@id, @loai, @ref, @ten, @sl, @dg)";
            var p = new[]
            {
                new SqlParameter("@id", hoaDonId),
                new SqlParameter("@loai", loaiHang),
                new SqlParameter("@ref", refId),
                new SqlParameter("@ten", tenHang),
                new SqlParameter("@sl", soLuong),
                new SqlParameter("@dg", donGia)
            };
            _db.ExecuteNonQuery(sql, p);
        }

        // Lấy danh sách hóa đơn theo chi nhánh và trạng thái (mặc định: CHỜ TT)
        public DataTable GetHoaDonList(int chiNhanhId, string trangThai = "CHỜ TT", int top = 100)
        {
            string sql = @"SELECT TOP (@top) hoa_don_id, loai, ngay_lap, vat, phi_dv, giam_gia, tong_truoc_thue, tong_sau_thue, trang_thai
                           FROM hoa_don
                           WHERE chi_nhanh_id = @cn AND (@tt IS NULL OR trang_thai = @tt)
                           ORDER BY ngay_lap DESC";
            var p = new[]
            {
                new SqlParameter("@top", top),
                new SqlParameter("@cn", chiNhanhId),
                new SqlParameter("@tt", (object?)trangThai ?? DBNull.Value)
            };
            return _db.GetDataTable(sql, p);
        }

        // Số hóa đơn đang CHỜ THANH TOÁN
        public int GetWaitingInvoicesCount(int chiNhanhId)
        {
            string sql = @"SELECT COUNT(*) FROM hoa_don WHERE chi_nhanh_id=@cn AND trang_thai=N'CHỜ TT'";
            var p = new[] { new SqlParameter("@cn", chiNhanhId) };
            var dt = _db.GetDataTable(sql, p);
            if (dt.Rows.Count == 0) return 0;
            return Convert.ToInt32(dt.Rows[0][0]);
        }

        // Thống kê hóa đơn đã thanh toán theo ngày UTC (COUNT, SUM)
        public (int SoHd, decimal Tong) GetPaidStatsOnDateUtc(int chiNhanhId, DateTime dateUtc)
        {
            string sql = @"SELECT COUNT(*) AS so_hd, COALESCE(SUM(tong_sau_thue),0) AS tong
                           FROM hoa_don
                           WHERE chi_nhanh_id=@cn AND trang_thai=N'ĐÃ THANH TOÁN'
                             AND CAST(ngay_lap AS date)=@d";
            var p = new[]
            {
                new SqlParameter("@cn", chiNhanhId),
                new SqlParameter("@d", dateUtc.Date)
            };
            var dt = _db.GetDataTable(sql, p);
            if (dt.Rows.Count == 0) return (0, 0m);
            int c = Convert.ToInt32(dt.Rows[0]["so_hd"]);
            decimal s = Convert.ToDecimal(dt.Rows[0]["tong"]);
            return (c, s);
        }

        // Lấy lịch sử thanh toán (hóa đơn đã thanh toán) với filter theo ngày
        public DataTable GetPaidInvoicesHistory(int chiNhanhId, DateTime? fromDate = null, DateTime? toDate = null, string? phuongThuc = null, int top = 100)
        {
            string sql = @"SELECT TOP (@top) 
                                hd.hoa_don_id, 
                                hd.loai, 
                                hd.ngay_lap, 
                                hd.vat, 
                                hd.phi_dv, 
                                hd.giam_gia, 
                                hd.tong_truoc_thue, 
                                hd.tong_sau_thue, 
                                hd.trang_thai,
                                hd.tham_chieu_id,
                                CASE 
                                    WHEN hd.loai = N'NHAHANG' AND db.dat_ban_id IS NOT NULL AND db.ban_id IS NOT NULL THEN CAST(b.so_ban AS NVARCHAR(50))
                                    WHEN hd.loai = N'TIECCUOI' AND ds.dat_sanh_id IS NOT NULL AND ds.sanh_id IS NOT NULL THEN s.ten_sanh
                                    ELSE N'-'
                                END AS ban_sanh,
                                km.ten AS ten_km,
                                km.ma_km AS ma_km,
                                hdkm.so_tien_km,
                                (SELECT TOP 1 hinh_thuc FROM thanh_toan WHERE hoa_don_id = hd.hoa_don_id ORDER BY ngay_tt DESC) AS phuong_thuc_tt,
                                NULL AS thu_ngan
                           FROM hoa_don hd
                           LEFT JOIN dat_ban db ON db.dat_ban_id = hd.tham_chieu_id AND hd.loai = N'NHAHANG'
                           LEFT JOIN ban b ON b.ban_id = db.ban_id
                           LEFT JOIN dat_sanh ds ON ds.dat_sanh_id = hd.tham_chieu_id AND hd.loai = N'TIECCUOI'
                           LEFT JOIN sanh s ON s.sanh_id = ds.sanh_id
                           LEFT JOIN hoa_don_km hdkm ON hdkm.hoa_don_id = hd.hoa_don_id
                           LEFT JOIN chuong_trinh_km km ON km.km_id = hdkm.km_id
                           WHERE hd.chi_nhanh_id = @cn";
            
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@top", top),
                new SqlParameter("@cn", chiNhanhId)
            };

            if (fromDate.HasValue)
            {
                sql += " AND CAST(DATEADD(HOUR, 7, hd.ngay_lap) AS date) >= @fromDate";
                parameters.Add(new SqlParameter("@fromDate", fromDate.Value.Date));
            }

            if (toDate.HasValue)
            {
                sql += " AND CAST(DATEADD(HOUR, 7, hd.ngay_lap) AS date) <= @toDate";
                parameters.Add(new SqlParameter("@toDate", toDate.Value.Date));
            }

            if (!string.IsNullOrWhiteSpace(phuongThuc) && phuongThuc != "Tất cả phương thức")
            {
                sql += @" AND EXISTS (
                    SELECT 1 FROM thanh_toan tt 
                    WHERE tt.hoa_don_id = hd.hoa_don_id 
                      AND tt.hinh_thuc = @phuongThuc
                )";
                parameters.Add(new SqlParameter("@phuongThuc", phuongThuc));
            }

            sql += " ORDER BY hd.ngay_lap DESC";

            return _db.GetDataTable(sql, parameters.ToArray());
        }

        // Xử lý thanh toán hóa đơn: cập nhật trạng thái, lưu thanh toán, lưu khuyến mãi
        public bool ProcessPayment(int hoaDonId, decimal soTien, string hinhThuc, int? kmId = null, int? voucherId = null, decimal? soTienKm = null)
        {
            try
            {
                string sqlUpdate = @"UPDATE hoa_don 
                                     SET trang_thai = N'ĐÃ THANH TOÁN'
                                     WHERE hoa_don_id = @id AND trang_thai = N'CHỜ TT'";
                var pUpdate = new[] { new SqlParameter("@id", hoaDonId) };
                int rowsAffected = _db.ExecuteNonQuery(sqlUpdate, pUpdate);
                
                if (rowsAffected == 0)
                {
                    return false; // Hóa đơn không tồn tại hoặc đã được thanh toán
                }

                string sqlPayment = @"INSERT INTO thanh_toan(hoa_don_id, so_tien, ngay_tt, hinh_thuc)
                                      VALUES(@id, @tien, SYSUTCDATETIME(), @hinh)";
                var pPayment = new[]
                {
                    new SqlParameter("@id", hoaDonId),
                    new SqlParameter("@tien", soTien),
                    new SqlParameter("@hinh", (object?)hinhThuc ?? DBNull.Value)
                };
                _db.ExecuteNonQuery(sqlPayment, pPayment);

                if (soTienKm.HasValue && soTienKm.Value > 0 && (kmId.HasValue || voucherId.HasValue))
                {
                    string sqlKm = @"INSERT INTO hoa_don_km(hoa_don_id, km_id, voucher_id, so_tien_km)
                                      VALUES(@id, @km, @voucher, @tien)";
                    var pKm = new[]
                    {
                        new SqlParameter("@id", hoaDonId),
                        new SqlParameter("@km", (object?)kmId ?? DBNull.Value),
                        new SqlParameter("@voucher", (object?)voucherId ?? DBNull.Value),
                        new SqlParameter("@tien", soTienKm.Value)
                    };
                    _db.ExecuteNonQuery(sqlKm, pKm);

                    if (voucherId.HasValue)
                    {
                        string sqlUpdateVoucher = @"UPDATE voucher 
                                                     SET da_dung = da_dung + 1
                                                     WHERE voucher_id = @vid";
                        var pUpdateVoucher = new[] { new SqlParameter("@vid", voucherId.Value) };
                        _db.ExecuteNonQuery(sqlUpdateVoucher, pUpdateVoucher);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Lấy thông tin hóa đơn theo ID (để kiểm tra trước khi thanh toán)
        public DataRow? GetHoaDonById(int hoaDonId)
        {
            string sql = @"SELECT hd.hoa_don_id, hd.chi_nhanh_id, hd.loai, hd.ngay_lap, hd.vat, hd.phi_dv, hd.giam_gia, 
                                  hd.tong_truoc_thue, hd.tong_sau_thue, hd.trang_thai,
                                  CASE 
                                      WHEN hd.loai = N'TIECCUOI' AND ds.dat_sanh_id IS NOT NULL AND ds.sanh_id IS NOT NULL THEN CAST(s.ten_sanh AS NVARCHAR(50))
                                      WHEN hd.loai = N'NHAHANG' AND db.dat_ban_id IS NOT NULL AND db.ban_id IS NOT NULL THEN 
                                          CASE 
                                              WHEN kv.ten_khu_vuc IS NOT NULL AND b.so_ban IS NOT NULL THEN kv.ten_khu_vuc + N'-' + b.so_ban
                                              WHEN b.so_ban IS NOT NULL THEN b.so_ban
                                              ELSE N''
                                          END
                                      ELSE N''
                                  END AS ban_sanh
                           FROM hoa_don hd
                           LEFT JOIN dat_sanh ds ON ds.dat_sanh_id = hd.tham_chieu_id AND hd.loai = N'TIECCUOI'
                           LEFT JOIN sanh s ON s.sanh_id = ds.sanh_id
                           LEFT JOIN dat_ban db ON db.dat_ban_id = hd.tham_chieu_id AND hd.loai = N'NHAHANG'
                           LEFT JOIN ban b ON b.ban_id = db.ban_id
                           LEFT JOIN khu_vuc kv ON kv.khu_vuc_id = b.khu_vuc_id
                           WHERE hd.hoa_don_id = @id";
            var p = new[] { new SqlParameter("@id", hoaDonId) };
            var dt = _db.GetDataTable(sql, p);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}


