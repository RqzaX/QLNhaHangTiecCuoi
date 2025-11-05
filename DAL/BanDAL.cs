using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.DAL
{
    public class BanDAL
    {
        private readonly DatabaseHelper _dbHelper;

        public BanDAL(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public DataTable LayDanhSachBan()
        {
            try
            {
                string query = @"
                    SELECT b.ban_id, b.chi_nhanh_id, b.khu_vuc_id, b.so_ban, b.suc_chua, b.trang_thai,
                           kv.ten_khu_vuc, cn.ten as ten_chi_nhanh
                    FROM ban b
                    LEFT JOIN khu_vuc kv ON b.khu_vuc_id = kv.khu_vuc_id
                    LEFT JOIN chi_nhanh cn ON b.chi_nhanh_id = cn.chi_nhanh_id
                    ORDER BY kv.ten_khu_vuc, b.so_ban";

                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách bàn: {ex.Message}");
            }
        }

        public DataTable LayDanhSachBanTheoKhuVuc(int? khuVucId)
        {
            try
            {
                string query;
                SqlParameter[] parameters;

                if (khuVucId.HasValue)
                {
                    query = @"
                        SELECT b.ban_id, b.chi_nhanh_id, b.khu_vuc_id, b.so_ban, b.suc_chua, b.trang_thai,
                               kv.ten_khu_vuc, cn.ten as ten_chi_nhanh
                        FROM ban b
                        LEFT JOIN khu_vuc kv ON b.khu_vuc_id = kv.khu_vuc_id
                        LEFT JOIN chi_nhanh cn ON b.chi_nhanh_id = cn.chi_nhanh_id
                        WHERE b.khu_vuc_id = @khuVucId
                        ORDER BY b.so_ban";
                    parameters = new SqlParameter[] { new SqlParameter("@khuVucId", khuVucId.Value) };
                }
                else
                {
                    query = @"
                        SELECT b.ban_id, b.chi_nhanh_id, b.khu_vuc_id, b.so_ban, b.suc_chua, b.trang_thai,
                               kv.ten_khu_vuc, cn.ten as ten_chi_nhanh
                        FROM ban b
                        LEFT JOIN khu_vuc kv ON b.khu_vuc_id = kv.khu_vuc_id
                        LEFT JOIN chi_nhanh cn ON b.chi_nhanh_id = cn.chi_nhanh_id
                        ORDER BY kv.ten_khu_vuc, b.so_ban";
                    parameters = new SqlParameter[0];
                }

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách bàn theo khu vực: {ex.Message}");
            }
        }

        public DataTable LayDanhSachBanTheoChiNhanh(int chiNhanhId, int? khuVucId = null)
        {
            try
            {
                string query;
                SqlParameter[] parameters;

                if (khuVucId.HasValue)
                {
                    query = @"
                        SELECT b.ban_id, b.chi_nhanh_id, b.khu_vuc_id, b.so_ban, b.suc_chua, b.trang_thai,
                               kv.ten_khu_vuc, cn.ten as ten_chi_nhanh
                        FROM ban b
                        LEFT JOIN khu_vuc kv ON b.khu_vuc_id = kv.khu_vuc_id
                        LEFT JOIN chi_nhanh cn ON b.chi_nhanh_id = cn.chi_nhanh_id
                        WHERE b.chi_nhanh_id = @chiNhanhId AND b.khu_vuc_id = @khuVucId
                        ORDER BY b.so_ban";
                    parameters = new SqlParameter[] 
                    { 
                        new SqlParameter("@chiNhanhId", chiNhanhId),
                        new SqlParameter("@khuVucId", khuVucId.Value) 
                    };
                }
                else
                {
                    query = @"
                        SELECT b.ban_id, b.chi_nhanh_id, b.khu_vuc_id, b.so_ban, b.suc_chua, b.trang_thai,
                               kv.ten_khu_vuc, cn.ten as ten_chi_nhanh
                        FROM ban b
                        LEFT JOIN khu_vuc kv ON b.khu_vuc_id = kv.khu_vuc_id
                        LEFT JOIN chi_nhanh cn ON b.chi_nhanh_id = cn.chi_nhanh_id
                        WHERE b.chi_nhanh_id = @chiNhanhId
                        ORDER BY kv.ten_khu_vuc, b.so_ban";
                    parameters = new SqlParameter[] 
                    { 
                        new SqlParameter("@chiNhanhId", chiNhanhId) 
                    };
                }

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                // Fallback: nếu query phức tạp bị lỗi, thử query đơn giản hơn
                try
                {
                    System.Diagnostics.Debug.WriteLine($"DEBUG: Query bàn phức tạp bị lỗi, thử fallback query: {ex.Message}");
                    
                    string fallbackQuery;
                    SqlParameter[] fallbackParameters;
                    
                    if (khuVucId.HasValue)
                    {
                        fallbackQuery = @"
                            SELECT ban_id, chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai,
                                   '' as ten_khu_vuc, '' as ten_chi_nhanh
                            FROM ban
                            WHERE chi_nhanh_id = @chiNhanhId AND khu_vuc_id = @khuVucId
                            ORDER BY so_ban";
                        fallbackParameters = new SqlParameter[] 
                        { 
                            new SqlParameter("@chiNhanhId", chiNhanhId),
                            new SqlParameter("@khuVucId", khuVucId.Value) 
                        };
                    }
                    else
                    {
                        fallbackQuery = @"
                            SELECT ban_id, chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai,
                                   '' as ten_khu_vuc, '' as ten_chi_nhanh
                            FROM ban
                            WHERE chi_nhanh_id = @chiNhanhId
                            ORDER BY so_ban";
                        fallbackParameters = new SqlParameter[] 
                        { 
                            new SqlParameter("@chiNhanhId", chiNhanhId) 
                        };
                    }

                    return _dbHelper.GetDataTable(fallbackQuery, fallbackParameters);
                }
                catch (Exception fallbackEx)
                {
                    throw new Exception($"Lỗi lấy danh sách bàn theo chi nhánh: {ex.Message}. Fallback cũng lỗi: {fallbackEx.Message}");
                }
            }
        }

        public DataTable LayThongKeBan()
        {
            try
            {
                string query = @"
                    SELECT 
                        COUNT(*) as tong_ban,
                        SUM(CASE WHEN trang_thai = N'TRỐNG' THEN 1 ELSE 0 END) as ban_trong,
                        SUM(CASE WHEN trang_thai = N'PHỤC VỤ' THEN 1 ELSE 0 END) as dang_su_dung,
                        SUM(CASE WHEN trang_thai = N'ĐÃ ĐẶT' THEN 1 ELSE 0 END) as da_dat_truoc
                    FROM ban";

                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thống kê bàn: {ex.Message}");
            }
        }

        public DataTable LayThongKeBanTheoChiNhanh(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT 
                        COUNT(*) as tong_ban,
                        SUM(CASE WHEN trang_thai = N'TRỐNG' THEN 1 ELSE 0 END) as ban_trong,
                        SUM(CASE WHEN trang_thai = N'PHỤC VỤ' THEN 1 ELSE 0 END) as dang_su_dung,
                        SUM(CASE WHEN trang_thai = N'ĐÃ ĐẶT' THEN 1 ELSE 0 END) as da_dat_truoc
                    FROM ban
                    WHERE chi_nhanh_id = @chiNhanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thống kê bàn theo chi nhánh: {ex.Message}");
            }
        }

        public DataTable LayThongKeBanTheoKhuVuc(int? khuVucId)
        {
            try
            {
                string query;
                SqlParameter[] parameters;

                if (khuVucId.HasValue)
                {
                    query = @"
                        SELECT 
                            COUNT(*) as tong_ban,
                            SUM(CASE WHEN trang_thai = N'TRỐNG' THEN 1 ELSE 0 END) as ban_trong,
                            SUM(CASE WHEN trang_thai = N'PHỤC VỤ' THEN 1 ELSE 0 END) as dang_su_dung,
                            SUM(CASE WHEN trang_thai = N'ĐÃ ĐẶT' THEN 1 ELSE 0 END) as da_dat_truoc
                        FROM ban
                        WHERE khu_vuc_id = @khuVucId";

                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("@khuVucId", khuVucId.Value)
                    };
                }
                else
                {
                    query = @"
                        SELECT 
                            COUNT(*) as tong_ban,
                            SUM(CASE WHEN trang_thai = N'TRỐNG' THEN 1 ELSE 0 END) as ban_trong,
                            SUM(CASE WHEN trang_thai = N'PHỤC VỤ' THEN 1 ELSE 0 END) as dang_su_dung,
                            SUM(CASE WHEN trang_thai = N'ĐÃ ĐẶT' THEN 1 ELSE 0 END) as da_dat_truoc
                        FROM ban";

                    parameters = null;
                }

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thống kê bàn theo khu vực: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhuVuc()
        {
            try
            {
                string query = @"
                    SELECT khu_vuc_id, ten_khu_vuc
                    FROM khu_vuc
                    ORDER BY ten_khu_vuc";

                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khu vực: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhuVucTheoChiNhanh(int chiNhanhId)
        {
            try
            {
                // Query đơn giản hơn - chỉ lấy khu vực có bàn thuộc chi nhánh
                string query = @"
                    SELECT DISTINCT kv.khu_vuc_id, kv.ten_khu_vuc
                    FROM khu_vuc kv
                    WHERE EXISTS (
                        SELECT 1 FROM ban b 
                        WHERE b.khu_vuc_id = kv.khu_vuc_id 
                        AND b.chi_nhanh_id = @chiNhanhId
                    )
                    ORDER BY kv.ten_khu_vuc";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                // Fallback: nếu query phức tạp bị lỗi, thử query đơn giản hơn
                try
                {
                    System.Diagnostics.Debug.WriteLine($"DEBUG: Query phức tạp bị lỗi, thử fallback query: {ex.Message}");
                    
                    string fallbackQuery = @"
                        SELECT khu_vuc_id, ten_khu_vuc
                        FROM khu_vuc
                        ORDER BY ten_khu_vuc";

                    return _dbHelper.GetDataTable(fallbackQuery);
                }
                catch (Exception fallbackEx)
                {
                    throw new Exception($"Lỗi lấy danh sách khu vực theo chi nhánh: {ex.Message}. Fallback cũng lỗi: {fallbackEx.Message}");
                }
            }
        }

        public bool CapNhatTrangThaiBan(int banId, string trangThai)
        {
            try
            {
                string query = "UPDATE ban SET trang_thai = @trangThai WHERE ban_id = @banId";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@trangThai", trangThai),
                    new SqlParameter("@banId", banId)
                };

                return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật trạng thái bàn: {ex.Message}");
            }
        }

        public DataTable LayThongTinDatBan(int banId)
        {
            try
            {
                string query = @"
                    SELECT 
                        db.dat_ban_id,
                        db.ngay_gio,
                        db.so_khach,
                        db.ghi_chu,
                        kh.ho_ten,
                        kh.sdt,
                        kh.email
                    FROM dat_ban db
                    INNER JOIN khach_hang kh ON db.khach_hang_id = kh.khach_hang_id
                    WHERE db.ban_id = @banId 
                    AND db.trang_thai = N'ĐÃ XÁC NHẬN'
                    ORDER BY db.ngay_gio DESC";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@banId", banId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin đặt bàn: {ex.Message}");
            }
        }

        public DataTable LayOrderHienTai(int banId)
        {
            try
            {
                string query = @"
                    SELECT 
                        po.phieu_order_id,
                        po.ngay_gio,
                        po.trang_thai,
                        poc.mon_id,
                        m.ten_mon,
                        poc.so_luong,
                        poc.don_gia,
                        poc.thanh_tien,
                        poc.ghi_chu_bep
                    FROM phieu_order po
                    INNER JOIN phieu_order_ct poc ON po.phieu_order_id = poc.phieu_order_id
                    INNER JOIN mon_an m ON poc.mon_id = m.mon_id
                    WHERE po.ban_id = @banId 
                    AND po.trang_thai IN (N'ĐANG PHỤC VỤ', N'CHỜ THANH TOÁN')
                    ORDER BY po.ngay_gio DESC, poc.order_ct_id";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@banId", banId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy order hiện tại: {ex.Message}");
            }
        }

        public bool TaoDatBan(int chiNhanhId, int banId, int khachHangId, DateTime ngayGio, int soKhach, string ghiChu)
        {
            try
            {
                string query = @"
                    INSERT INTO dat_ban (chi_nhanh_id, ban_id, khach_hang_id, ngay_gio, so_khach, trang_thai, ghi_chu)
                    VALUES (@chiNhanhId, @banId, @khachHangId, @ngayGio, @soKhach, N'ĐÃ XÁC NHẬN', @ghiChu)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId),
                    new SqlParameter("@banId", banId),
                    new SqlParameter("@khachHangId", khachHangId),
                    new SqlParameter("@ngayGio", ngayGio),
                    new SqlParameter("@soKhach", soKhach),
                    new SqlParameter("@ghiChu", ghiChu ?? "")
                };

                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tạo đặt bàn: {ex.Message}");
            }
        }

        public DataTable LayThongTinBan(int banId)
        {
            try
            {
                string query = @"
                    SELECT ban_id, so_ban, suc_chua, trang_thai
                    FROM ban 
                    WHERE ban_id = @banId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@banId", banId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin bàn: {ex.Message}");
            }
        }

        public DataTable LayDanhSachDatBan(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT 
                        'DB' + RIGHT('000' + CAST(db.dat_ban_id AS VARCHAR), 3) as ma_dat_ban,
                        kh.ho_ten,
                        kh.sdt,
                        db.ngay_gio,
                        b.so_ban,
                        kv.ten_khu_vuc,
                        db.so_khach,
                        db.trang_thai,
                        ISNULL(db.ghi_chu, '') as ghi_chu,
                        0 as tien_coc
                    FROM dat_ban db
                    INNER JOIN khach_hang kh ON db.khach_hang_id = kh.khach_hang_id
                    INNER JOIN ban b ON db.ban_id = b.ban_id
                    LEFT JOIN khu_vuc kv ON b.khu_vuc_id = kv.khu_vuc_id
                    WHERE db.chi_nhanh_id = @chiNhanhId
                    ORDER BY 
                        CASE WHEN CAST(db.ngay_gio AS DATE) = CAST(GETDATE() AS DATE) THEN 0 ELSE 1 END,
                        CAST(db.ngay_gio AS DATE) ASC,
                        db.ngay_gio ASC";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách đặt bàn: {ex.Message}");
            }
        }

        public DataTable LayThongKeDatBan(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT 
                        COUNT(*) as TongDatBan,
                        SUM(CASE WHEN db.trang_thai = N'CHỜ XÁC NHẬN' THEN 1 ELSE 0 END) as ChoXacNhan,
                        SUM(CASE WHEN db.trang_thai = N'ĐÃ XÁC NHẬN' THEN 1 ELSE 0 END) as DaXacNhan,
                        SUM(CASE WHEN db.trang_thai = N'ĐÃ PHỤC VỤ' THEN 1 ELSE 0 END) as DaDen,
                        SUM(CASE WHEN db.trang_thai = N'ĐÃ HỦY' THEN 1 ELSE 0 END) as DaHuy
                    FROM dat_ban db
                    WHERE db.chi_nhanh_id = @chiNhanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thống kê đặt bàn: {ex.Message}");
            }
        }

        public bool HuyDatBan(string maDatBan)
        {
            try
            {
                string query = @"
                    UPDATE dat_ban 
                    SET trang_thai = N'ĐÃ HỦY' 
                    WHERE 'DB' + RIGHT('000' + CAST(dat_ban_id AS VARCHAR), 3) = @maDatBan;
                    
                    UPDATE ban 
                    SET trang_thai = N'TRỐNG' 
                    WHERE ban_id = (
                        SELECT ban_id 
                        FROM dat_ban 
                        WHERE 'DB' + RIGHT('000' + CAST(dat_ban_id AS VARCHAR), 3) = @maDatBan
                    )";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@maDatBan", maDatBan)
                };

                return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi hủy đặt bàn: {ex.Message}");
            }
        }

        public bool CapNhatDatBan(string maDatBan, int khachHangId, int banId, int soKhach, DateTime ngayDat, string ghiChu)
        {
            try
            {
                string query = @"
                    UPDATE dat_ban
                    SET khach_hang_id = @khachHangId,
                        ban_id = @banId,
                        so_khach = @soKhach,
                        ngay_gio = @ngayDat,
                        ghi_chu = @ghiChu
                    WHERE 'DB' + RIGHT('000' + CAST(dat_ban_id AS VARCHAR), 3) = @maDatBan";

                SqlParameter[] parameters = {
                    new SqlParameter("@maDatBan", maDatBan),
                    new SqlParameter("@khachHangId", khachHangId),
                    new SqlParameter("@banId", banId),
                    new SqlParameter("@soKhach", soKhach),
                    new SqlParameter("@ngayDat", ngayDat),
                    new SqlParameter("@ghiChu", ghiChu ?? "")
                };

                return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật đặt bàn: {ex.Message}");
            }
        }

        public bool XacNhanDaDen(string maDatBan)
        {
            try
            {
                // Xác nhận đã đến - không cần xóa ghi chú vì ghi chú cảnh báo chỉ là hiển thị
                string query = @"
                    UPDATE dat_ban 
                    SET trang_thai = N'ĐÃ PHỤC VỤ'
                    WHERE ('DB' + RIGHT('000' + CAST(dat_ban_id AS VARCHAR), 3) = @maDatBan)
                    AND trang_thai = N'ĐÃ XÁC NHẬN';
                    
                    UPDATE ban 
                    SET trang_thai = N'PHỤC VỤ' 
                    WHERE ban_id = (
                        SELECT ban_id 
                        FROM dat_ban 
                        WHERE 'DB' + RIGHT('000' + CAST(dat_ban_id AS VARCHAR), 3) = @maDatBan
                    )";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@maDatBan", maDatBan)
                };

                return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xác nhận đã đến: {ex.Message}");
            }
        }

        public bool XacNhanDatBan(string maDatBan)
        {
            try
            {
                string query = @"
                    UPDATE dat_ban 
                    SET trang_thai = N'ĐÃ XÁC NHẬN' 
                    WHERE 'DB' + RIGHT('000' + CAST(dat_ban_id AS VARCHAR), 3) = @maDatBan";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@maDatBan", maDatBan)
                };

                return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xác nhận đặt bàn: {ex.Message}");
            }
        }

        public int CapNhatTrangThaiTreGio()
        {
            // Không cập nhật database, chỉ để tương thích với BLL
            // Logic hiển thị "Trễ giờ đặt" được xử lý hoàn toàn ở UI dựa trên thời gian
            return 0;
        }

        public int TuDongHuyDatBanTreGio()
        {
            try
            {
                // Tự động hủy các đặt bàn trễ giờ quá 2 tiếng (120 phút) kể từ giờ đặt
                // Dựa vào thời gian trực tiếp, không dựa vào ghi chú
                string query = @"
                    UPDATE dat_ban 
                    SET trang_thai = N'ĐÃ HỦY',
                        ghi_chu = CASE 
                            WHEN ghi_chu IS NULL OR ghi_chu = '' THEN N'Đã tự động hủy do khách không đến sau 2 tiếng kể từ giờ đặt.'
                            ELSE ghi_chu + N' (Đã tự động hủy do khách không đến sau 2 tiếng kể từ giờ đặt.)'
                        END
                    WHERE trang_thai = N'ĐÃ XÁC NHẬN'
                    AND ngay_gio < GETDATE()  -- Đã quá giờ đặt
                    AND DATEDIFF(MINUTE, ngay_gio, GETDATE()) >= 120;  -- Quá 2 tiếng
                    
                    UPDATE ban 
                    SET trang_thai = N'TRỐNG' 
                    WHERE ban_id IN (
                        SELECT ban_id 
                        FROM dat_ban 
                        WHERE trang_thai = N'ĐÃ HỦY'
                        AND DATEDIFF(MINUTE, ngay_gio, GETDATE()) >= 120
                        AND ghi_chu LIKE N'%tự động hủy%'
                    )";
                
                return _dbHelper.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tự động hủy đặt bàn trễ giờ: {ex.Message}");
            }
        }
    }
}
