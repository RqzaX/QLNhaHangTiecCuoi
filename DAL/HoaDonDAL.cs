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
                                int? khachHangId = null, int? thamChieuId = null,
                                string? soBanSanh = null, string? tenNguoiBan = null, string? tenNguoiDat = null)
        {
            string sql = @"INSERT INTO hoa_don(chi_nhanh_id, khach_hang_id, loai, tham_chieu_id, ngay_lap, vat, phi_dv, giam_gia, tong_truoc_thue, tong_sau_thue, trang_thai, so_ban_sanh, ten_nguoi_ban, ten_nguoi_dat)
                           OUTPUT INSERTED.hoa_don_id
                           VALUES(@cn, @kh, @loai, @tc, SYSUTCDATETIME(), @vat, @phi, @giam, @truoc, @sau, N'CHỜ TT', @soBanSanh, @tenNguoiBan, @tenNguoiDat)";
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
                new SqlParameter("@sau", tongSauThue),
                new SqlParameter("@soBanSanh", (object?)soBanSanh ?? DBNull.Value),
                new SqlParameter("@tenNguoiBan", (object?)tenNguoiBan ?? DBNull.Value),
                new SqlParameter("@tenNguoiDat", (object?)tenNguoiDat ?? DBNull.Value)
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
        public DataTable GetHoaDonList(int chiNhanhId, string trangThai = "CHỜ TT", int top = 100, string? loai = null)
        {
            string sql = @"SELECT TOP (@top) hoa_don_id, loai, ngay_lap, vat, phi_dv, giam_gia, tong_truoc_thue, tong_sau_thue, trang_thai
                           FROM hoa_don
                           WHERE chi_nhanh_id = @cn 
                             AND (@tt IS NULL OR trang_thai = @tt)
                             AND (@loai IS NULL OR loai = @loai)
                           ORDER BY ngay_lap DESC";
            var p = new[]
            {
                new SqlParameter("@top", top),
                new SqlParameter("@cn", chiNhanhId),
                new SqlParameter("@tt", (object?)trangThai ?? DBNull.Value),
                new SqlParameter("@loai", (object?)loai ?? DBNull.Value)
            };
            return _db.GetDataTable(sql, p);
        }

        // Lấy danh sách hóa đơn đơn giản (loai, tong_sau_thue, ngay_lap) cho khách hàng theo chi nhánh
        public DataTable GetHoaDonForKhachHang(int chiNhanhId, int top = 100)
        {
            string sql = @"SELECT TOP (@top) 
                                loai, 
                                tong_sau_thue, 
                                ngay_lap
                           FROM hoa_don
                           WHERE chi_nhanh_id = @cn
                           ORDER BY ngay_lap DESC";
            var p = new[]
            {
                new SqlParameter("@top", top),
                new SqlParameter("@cn", chiNhanhId)
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

        // Đếm số lượng hóa đơn theo loại (NHAHANG hoặc TIECCUOI)
        public (int NhaHang, int TiecCuoi) GetHoaDonCountByLoai(int? chiNhanhId = null)
        {
            string sql = @"
                SELECT 
                    SUM(CASE WHEN loai = N'NHAHANG' THEN 1 ELSE 0 END) AS nha_hang,
                    SUM(CASE WHEN loai = N'TIECCUOI' THEN 1 ELSE 0 END) AS tiec_cuoi
                FROM hoa_don
                WHERE (@cn IS NULL OR chi_nhanh_id = @cn)";

            var parameters = new List<SqlParameter>();
            if (chiNhanhId.HasValue)
            {
                parameters.Add(new SqlParameter("@cn", chiNhanhId.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@cn", DBNull.Value));
            }

            var dt = _db.GetDataTable(sql, parameters.ToArray());
            if (dt == null || dt.Rows.Count == 0)
                return (0, 0);

            int nhaHang = Convert.ToInt32(dt.Rows[0]["nha_hang"] ?? 0);
            int tiecCuoi = Convert.ToInt32(dt.Rows[0]["tiec_cuoi"] ?? 0);
            return (nhaHang, tiecCuoi);
        }

        // Lấy doanh thu 7 ngày qua theo ngày (từ hôm nay - 7 đến hôm qua, không bao gồm hôm nay)
        public DataTable GetRevenue7Days(int chiNhanhId)
        {
            string sql = @"
                SELECT 
                    dr.ngay,
                    ISNULL(SUM(hd.tong_sau_thue), 0) AS doanh_thu
                FROM (
                    SELECT DATEADD(DAY, -7, CAST(GETDATE() AS DATE)) AS ngay
                    UNION ALL SELECT DATEADD(DAY, -6, CAST(GETDATE() AS DATE))
                    UNION ALL SELECT DATEADD(DAY, -5, CAST(GETDATE() AS DATE))
                    UNION ALL SELECT DATEADD(DAY, -4, CAST(GETDATE() AS DATE))
                    UNION ALL SELECT DATEADD(DAY, -3, CAST(GETDATE() AS DATE))
                    UNION ALL SELECT DATEADD(DAY, -2, CAST(GETDATE() AS DATE))
                    UNION ALL SELECT DATEADD(DAY, -1, CAST(GETDATE() AS DATE))
                ) dr
                LEFT JOIN hoa_don hd ON CAST(DATEADD(HOUR, 7, hd.ngay_lap) AS DATE) = dr.ngay
                    AND hd.chi_nhanh_id = @cn 
                    AND hd.trang_thai = N'ĐÃ THANH TOÁN'
                GROUP BY dr.ngay
                ORDER BY dr.ngay";
            
            var p = new[]
            {
                new SqlParameter("@cn", chiNhanhId)
            };
            return _db.GetDataTable(sql, p);
        }

        // Lấy doanh thu hôm nay và hôm qua để so sánh
        public (decimal HomNay, decimal HomQua) GetRevenueTodayAndYesterday(int chiNhanhId)
        {
            string sql = @"
                SELECT 
                    CAST(DATEADD(HOUR, 7, ngay_lap) AS DATE) AS ngay,
                    ISNULL(SUM(tong_sau_thue), 0) AS doanh_thu
                FROM hoa_don
                WHERE chi_nhanh_id = @cn 
                  AND trang_thai = N'ĐÃ THANH TOÁN'
                  AND CAST(DATEADD(HOUR, 7, ngay_lap) AS DATE) IN (CAST(GETDATE() AS DATE), DATEADD(DAY, -1, CAST(GETDATE() AS DATE)))
                GROUP BY CAST(DATEADD(HOUR, 7, ngay_lap) AS DATE)";
            
            var p = new[] { new SqlParameter("@cn", chiNhanhId) };
            var dt = _db.GetDataTable(sql, p);
            
            decimal homNay = 0;
            decimal homQua = 0;
            DateTime today = DateTime.Now.Date;
            DateTime yesterday = today.AddDays(-1);
            
            foreach (DataRow row in dt.Rows)
            {
                DateTime ngay = Convert.ToDateTime(row["ngay"]).Date;
                decimal doanhThu = Convert.ToDecimal(row["doanh_thu"]);
                
                if (ngay == today)
                    homNay = doanhThu;
                else if (ngay == yesterday)
                    homQua = doanhThu;
            }
            
            return (homNay, homQua);
        }

        // Lấy top 5 món bán chạy nhất (dựa trên tổng số lượng đã bán từ tất cả hóa đơn đã thanh toán)
        public DataTable GetTop5MonBanChay(int? chiNhanhId = null)
        {
            string sql = @"
                SELECT TOP 5
                    hdct.ten_hang,
                    SUM(hdct.so_luong) AS tong_so_luong,
                    COUNT(DISTINCT hdct.hoa_don_id) AS so_lan_goi,
                    SUM(hdct.thanh_tien) AS tong_tien
                FROM dbo.hoa_don_ct hdct
                INNER JOIN dbo.hoa_don hd ON hd.hoa_don_id = hdct.hoa_don_id
                WHERE hdct.loai_hang = N'MÓN'
                  AND hd.trang_thai = N'ĐÃ THANH TOÁN'
                  AND (@cn IS NULL OR hd.chi_nhanh_id = @cn)
                GROUP BY hdct.ten_hang
                ORDER BY SUM(hdct.so_luong) DESC";

            var parameters = new List<SqlParameter>();
            if (chiNhanhId.HasValue)
            {
                parameters.Add(new SqlParameter("@cn", chiNhanhId.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@cn", DBNull.Value));
            }

            return _db.GetDataTable(sql, parameters.ToArray());
        }

        // Lấy top 5 món bán chạy trong tháng
        public DataTable GetTop5MonBanChayTrongThang(int chiNhanhId)
        {
            string sql = @"
                SELECT TOP 5
                    hdct.ten_hang,
                    SUM(hdct.so_luong) AS tong_so_luong,
                    COUNT(DISTINCT hdct.hoa_don_id) AS so_lan_goi,
                    SUM(hdct.thanh_tien) AS tong_tien
                FROM dbo.hoa_don_ct hdct
                INNER JOIN dbo.hoa_don hd ON hd.hoa_don_id = hdct.hoa_don_id
                WHERE hdct.loai_hang = N'MÓN'
                  AND hd.trang_thai = N'ĐÃ THANH TOÁN'
                  AND hd.chi_nhanh_id = @cn
                  AND YEAR(CAST(DATEADD(HOUR, 7, hd.ngay_lap) AS DATE)) = YEAR(GETDATE())
                  AND MONTH(CAST(DATEADD(HOUR, 7, hd.ngay_lap) AS DATE)) = MONTH(GETDATE())
                GROUP BY hdct.ten_hang
                ORDER BY SUM(hdct.thanh_tien) DESC";

            var p = new[]
            {
                new SqlParameter("@cn", chiNhanhId)
            };

            return _db.GetDataTable(sql, p);
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
                                    WHEN hd.so_ban_sanh IS NOT NULL AND hd.so_ban_sanh != N'' THEN hd.so_ban_sanh
                                    WHEN hd.loai = N'NHAHANG' AND db.dat_ban_id IS NOT NULL AND db.ban_id IS NOT NULL THEN CAST(b.so_ban AS NVARCHAR(50))
                                    WHEN hd.loai = N'TIECCUOI' AND ds.dat_sanh_id IS NOT NULL AND ds.sanh_id IS NOT NULL THEN s.ten_sanh
                                    ELSE N'-'
                                END AS ban_sanh,
                                CASE 
                                    WHEN hd.loai = N'NHAHANG' THEN hd.ten_nguoi_ban
                                    WHEN hd.loai = N'TIECCUOI' THEN hd.ten_nguoi_dat
                                    ELSE NULL
                                END AS thu_ngan,
                                km.ten AS ten_km,
                                km.ma_km AS ma_km,
                                hdkm.so_tien_km,
                                (SELECT TOP 1 hinh_thuc FROM thanh_toan WHERE hoa_don_id = hd.hoa_don_id ORDER BY ngay_tt DESC) AS phuong_thuc_tt
                           FROM hoa_don hd
                           LEFT JOIN dat_ban db ON db.dat_ban_id = hd.tham_chieu_id AND hd.loai = N'NHAHANG'
                           LEFT JOIN ban b ON b.ban_id = db.ban_id
                           LEFT JOIN dat_sanh ds ON ds.dat_sanh_id = hd.tham_chieu_id AND hd.loai = N'TIECCUOI'
                           LEFT JOIN sanh s ON s.sanh_id = ds.sanh_id
                           LEFT JOIN hoa_don_km hdkm ON hdkm.hoa_don_id = hd.hoa_don_id
                           LEFT JOIN chuong_trinh_km km ON km.km_id = hdkm.km_id
                           WHERE hd.chi_nhanh_id = @cn 
                             AND hd.trang_thai IN (N'ĐÃ THANH TOÁN', N'HOÀN TIỀN')";
            
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
        public bool ProcessPayment(int hoaDonId, decimal soTien, string hinhThuc, string? thuNgan = null, int? kmId = null, int? voucherId = null, decimal? soTienKm = null)
        {
            try
            {
                // Kiểm tra hóa đơn có tồn tại không
                var hoaDon = GetHoaDonById(hoaDonId);
                if (hoaDon == null)
                {
                    return false; 
                }
                string trangThaiHienTai = hoaDon["trang_thai"]?.ToString()?.Trim() ?? "";
                if (trangThaiHienTai == "ĐÃ THANH TOÁN")
                {
                    return false; 
                }

                // Lấy loại hóa đơn để cập nhật đúng cột thu ngân
                string sqlGetLoai = @"SELECT loai FROM hoa_don WHERE hoa_don_id = @id";
                var pGetLoai = new[] { new SqlParameter("@id", hoaDonId) };
                var dtLoai = _db.GetDataTable(sqlGetLoai, pGetLoai);
                string loaiHoaDon = "";
                if (dtLoai.Rows.Count > 0 && dtLoai.Rows[0]["loai"] != DBNull.Value)
                {
                    loaiHoaDon = dtLoai.Rows[0]["loai"].ToString() ?? "";
                }

                // Cập nhật trạng thái hóa đơn và thu ngân
                string sqlUpdate;
                if (!string.IsNullOrEmpty(thuNgan))
                {
                    if (loaiHoaDon == "NHAHANG")
                    {
                        sqlUpdate = @"UPDATE hoa_don 
                                     SET trang_thai = N'ĐÃ THANH TOÁN', ten_nguoi_ban = @thuNgan
                                     WHERE hoa_don_id = @id AND trang_thai != N'ĐÃ THANH TOÁN'";
                    }
                    else if (loaiHoaDon == "TIECCUOI")
                    {
                        sqlUpdate = @"UPDATE hoa_don 
                                     SET trang_thai = N'ĐÃ THANH TOÁN', ten_nguoi_dat = @thuNgan
                                     WHERE hoa_don_id = @id AND trang_thai != N'ĐÃ THANH TOÁN'";
                    }
                    else
                    {
                        sqlUpdate = @"UPDATE hoa_don 
                                     SET trang_thai = N'ĐÃ THANH TOÁN'
                                     WHERE hoa_don_id = @id AND trang_thai != N'ĐÃ THANH TOÁN'";
                    }
                }
                else
                {
                    sqlUpdate = @"UPDATE hoa_don 
                                     SET trang_thai = N'ĐÃ THANH TOÁN'
                                 WHERE hoa_don_id = @id AND trang_thai != N'ĐÃ THANH TOÁN'";
                }

                var pUpdate = new List<SqlParameter> { new SqlParameter("@id", hoaDonId) };
                if (!string.IsNullOrEmpty(thuNgan) && (loaiHoaDon == "NHAHANG" || loaiHoaDon == "TIECCUOI"))
                {
                    pUpdate.Add(new SqlParameter("@thuNgan", thuNgan));
                }
                int rowsAffected = _db.ExecuteNonQuery(sqlUpdate, pUpdate.ToArray());
                
                if (rowsAffected == 0)
                {
                    return false;
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
                        // Kiểm tra lượt dùng trước khi cập nhật
                        string sqlCheckVoucher = @"SELECT so_lan, da_dung 
                                                    FROM voucher 
                                                    WHERE voucher_id = @vid";
                        var pCheck = new[] { new SqlParameter("@vid", voucherId.Value) };
                        var dtCheck = _db.GetDataTable(sqlCheckVoucher, pCheck);
                        
                        if (dtCheck.Rows.Count > 0)
                        {
                            int soLan = dtCheck.Rows[0]["so_lan"] != DBNull.Value ? Convert.ToInt32(dtCheck.Rows[0]["so_lan"]) : 0;
                            int daDung = dtCheck.Rows[0]["da_dung"] != DBNull.Value ? Convert.ToInt32(dtCheck.Rows[0]["da_dung"]) : 0;
                            
                            if (soLan > 0 && daDung >= soLan)
                            {
                                throw new Exception("Voucher đã sử dụng hết số lượt cho phép.");
                            }
                            
                        string sqlUpdateVoucher = @"UPDATE voucher 
                                                     SET da_dung = da_dung + 1
                                                         WHERE voucher_id = @vid AND (so_lan = 0 OR da_dung < so_lan)";
                        var pUpdateVoucher = new[] { new SqlParameter("@vid", voucherId.Value) };
                            int rowsUpdated = _db.ExecuteNonQuery(sqlUpdateVoucher, pUpdateVoucher);
                            
                            if (rowsUpdated == 0)
                            {
                                throw new Exception("Voucher đã sử dụng hết số lượt cho phép.");
                            }
                        }
                    }
                }

                string sqlCheckLoai = @"SELECT loai, tham_chieu_id, tong_sau_thue FROM hoa_don WHERE hoa_don_id = @id";
                var pCheckLoai = new[] { new SqlParameter("@id", hoaDonId) };
                var dtCheckLoai = _db.GetDataTable(sqlCheckLoai, pCheckLoai);
                
                if (dtCheckLoai.Rows.Count > 0)
                {
                    string loai = dtCheckLoai.Rows[0]["loai"]?.ToString() ?? "";
                    if (loai == "TIECCUOI" && dtCheckLoai.Rows[0]["tham_chieu_id"] != DBNull.Value)
                    {
                        int hopDongId = Convert.ToInt32(dtCheckLoai.Rows[0]["tham_chieu_id"]);
                        
                        // Tính tổng cọc từ hop_dong_coc
                        string sqlTongCoc = @"SELECT ISNULL(SUM(so_tien), 0) AS tong_coc 
                                              FROM hop_dong_coc 
                                              WHERE hop_dong_id = @hdId";
                        var pTongCoc = new[] { new SqlParameter("@hdId", hopDongId) };
                        var dtTongCoc = _db.GetDataTable(sqlTongCoc, pTongCoc);
                        decimal tongCoc = 0;
                        if (dtTongCoc.Rows.Count > 0 && dtTongCoc.Rows[0][0] != DBNull.Value)
                        {
                            tongCoc = Convert.ToDecimal(dtTongCoc.Rows[0][0]);
                        }
                        
                        // Tính tổng thanh toán từ hop_dong_tt
                        string sqlTongThanhToan = @"SELECT ISNULL(SUM(so_tien), 0) AS tong_tt 
                                                     FROM hop_dong_tt 
                                                     WHERE hop_dong_id = @hdId";
                        var pTongTT = new[] { new SqlParameter("@hdId", hopDongId) };
                        var dtTongTT = _db.GetDataTable(sqlTongThanhToan, pTongTT);
                        decimal tongThanhToan = 0;
                        if (dtTongTT.Rows.Count > 0 && dtTongTT.Rows[0][0] != DBNull.Value)
                        {
                            tongThanhToan = Convert.ToDecimal(dtTongTT.Rows[0][0]);
                        }
                        
                        // Tính tổng số tiền đã thanh toán từ thanh_toan (cho hóa đơn)
                        string sqlTongDaThanhToan = @"SELECT ISNULL(SUM(so_tien), 0) AS tong_da_tt 
                                                       FROM thanh_toan 
                                                       WHERE hoa_don_id = @id";
                        var pTongDaTT = new[] { new SqlParameter("@id", hoaDonId) };
                        var dtTongDaTT = _db.GetDataTable(sqlTongDaThanhToan, pTongDaTT);
                        decimal tongDaThanhToan = 0;
                        if (dtTongDaTT.Rows.Count > 0 && dtTongDaTT.Rows[0][0] != DBNull.Value)
                        {
                            tongDaThanhToan = Convert.ToDecimal(dtTongDaTT.Rows[0][0]);
                        }
                        
                        // Tính tổng giảm giá (nếu có)
                        string sqlTongGiamGia = @"SELECT ISNULL(SUM(so_tien_km), 0) AS tong_giam_gia 
                                                  FROM hoa_don_km 
                                                  WHERE hoa_don_id = @id";
                        var pGiamGia = new[] { new SqlParameter("@id", hoaDonId) };
                        var dtGiamGia = _db.GetDataTable(sqlTongGiamGia, pGiamGia);
                        decimal tongGiamGia = 0;
                        if (dtGiamGia.Rows.Count > 0 && dtGiamGia.Rows[0][0] != DBNull.Value)
                        {
                            tongGiamGia = Convert.ToDecimal(dtGiamGia.Rows[0][0]);
                        }
                        
                        // Tính số tiền còn lại: tong_sau_thue - (tong_coc + tong_thanh_toan + tong_da_thanh_toan) - tong_giam_gia
                        decimal tongSauThue = Convert.ToDecimal(dtCheckLoai.Rows[0]["tong_sau_thue"]);
                        decimal tongDaTra = tongCoc + tongThanhToan + tongDaThanhToan;
                        decimal soTienConLai = tongSauThue - tongDaTra - tongGiamGia;
                        
                        if (soTienConLai <= 0)
                        {
                            string sqlGetDatSanh = @"SELECT dat_sanh_id FROM hop_dong WHERE hop_dong_id = @hdId";
                            var pGetDatSanh = new[] { new SqlParameter("@hdId", hopDongId) };
                            var dtDatSanh = _db.GetDataTable(sqlGetDatSanh, pGetDatSanh);
                            
                            if (dtDatSanh.Rows.Count > 0 && dtDatSanh.Rows[0]["dat_sanh_id"] != DBNull.Value)
                            {
                                int datSanhId = Convert.ToInt32(dtDatSanh.Rows[0]["dat_sanh_id"]);
                                
                                string sqlUpdateDatSanh = @"UPDATE dat_sanh 
                                                             SET trang_thai = N'ĐÃ THANH TOÁN'
                                                             WHERE dat_sanh_id = @dsId";
                                var pUpdateDatSanh = new[] { new SqlParameter("@dsId", datSanhId) };
                                _db.ExecuteNonQuery(sqlUpdateDatSanh, pUpdateDatSanh);
                            }
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Cập nhật trạng thái hóa đơn
        public bool CapNhatTrangThaiHoaDon(int hoaDonId, string trangThai)
        {
            try
            {
                string sql = @"UPDATE hoa_don 
                               SET trang_thai = @trangThai
                               WHERE hoa_don_id = @id";
                var p = new[]
                {
                    new SqlParameter("@id", hoaDonId),
                    new SqlParameter("@trangThai", SqlDbType.NVarChar, 50) { Value = trangThai ?? "" }
                };
                
                int rowsAffected = _db.ExecuteNonQuery(sql, p);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Lấy thông tin hóa đơn theo ID (để kiểm tra trước khi thanh toán)
        public DataRow? GetHoaDonById(int hoaDonId)
        {
            string sql = @"SELECT hd.hoa_don_id, hd.chi_nhanh_id, hd.loai, hd.tham_chieu_id, hd.ngay_lap, hd.vat, hd.phi_dv, hd.giam_gia, 
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

        // Xử lý hoàn tiền hóa đơn nhà hàng
        public bool ProcessRefund(int hoaDonId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var hoaDon = GetHoaDonById(hoaDonId);
                if (hoaDon == null)
                {
                    errorMessage = "Không tìm thấy hóa đơn!";
                    return false;
                }

                string loai = hoaDon["loai"]?.ToString() ?? "";
                if (loai != "NHAHANG")
                {
                    errorMessage = "Chức năng hoàn tiền chỉ áp dụng cho hóa đơn nhà hàng!";
                    return false;
                }

                string trangThai = hoaDon["trang_thai"]?.ToString()?.Trim() ?? "";
                if (trangThai != "ĐÃ THANH TOÁN")
                {
                    errorMessage = $"Chỉ có thể hoàn tiền cho hóa đơn đã thanh toán! Trạng thái hiện tại: '{trangThai}'";
                    return false;
                }

                string sql = @"UPDATE hoa_don 
                               SET trang_thai = N'HOÀN TIỀN'
                               WHERE hoa_don_id = @id AND trang_thai = N'ĐÃ THANH TOÁN'";
                var p = new[]
                {
                    new SqlParameter("@id", hoaDonId)
                };
                
                int rowsAffected = _db.ExecuteNonQuery(sql, p);
                if (rowsAffected == 0)
                {
                    var checkHoaDon = GetHoaDonById(hoaDonId);
                    if (checkHoaDon == null)
                    {
                        errorMessage = "Không tìm thấy hóa đơn!";
                    }
                    else
                    {
                        string currentStatus = checkHoaDon["trang_thai"]?.ToString()?.Trim() ?? "";
                        errorMessage = $"Không thể cập nhật trạng thái hóa đơn! Trạng thái hiện tại: '{currentStatus}'. Chỉ có thể hoàn tiền cho hóa đơn đã thanh toán.";
                    }
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi xử lý hoàn tiền: {ex.Message}";
                return false;
            }
        }
    }
}


