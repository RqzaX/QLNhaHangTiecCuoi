using System;
using System.Data;
using System.Linq;
using QLNhaHangTiecCuoi.Share;
using Microsoft.Data.SqlClient;

namespace QLNhaHangTiecCuoi.DAL
{
    public class KOTDAL
    {
        private readonly DatabaseHelper _dbHelper;

        public KOTDAL(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public DataTable LayDanhSachKOT(int chiNhanhId, string? status = null, string? department = null)
        {
            try
            {
                // Kiểm tra chiNhanhId hợp lệ
                if (chiNhanhId <= 0)
                {
                    return new DataTable();
                }
                
                string query = @"
                    SELECT 
                        po.phieu_order_id as kot_id,
                        'KOT' + RIGHT('000' + CAST(po.phieu_order_id AS VARCHAR), 3) as ma_kot,
                        po.ban_id,
                        ISNULL(b.so_ban, N'TIỆC') as so_ban,
                        po.ngay_gio as thoi_gian_dat,
                        po.trang_thai,
                        CASE 
                            WHEN po.ban_id IS NOT NULL THEN N'BẾP'
                            ELSE N'BAR'
                        END as loai_kot,
                        NULL as ghi_chu,
                        0 as uu_tien
                    FROM phieu_order po
                    LEFT JOIN ban b ON po.ban_id = b.ban_id
                    WHERE po.chi_nhanh_id = @chiNhanhId";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                if (!string.IsNullOrEmpty(status))
                {
                    query += " AND po.trang_thai = @status";
                    Array.Resize(ref parameters, parameters.Length + 1);
                    parameters[parameters.Length - 1] = new SqlParameter("@status", status);
                }

                query += " ORDER BY po.ngay_gio DESC";
                
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - Lấy danh sách KOT: {ex.Message}");
            }
        }

        public DataTable LayChiTietKOT(int kotId)
        {
            try
            {
                string query = @"
                    SELECT 
                        poc.phieu_order_id as kot_id,
                        poc.mon_id,
                        m.ten_mon,
                        poc.so_luong,
                        ISNULL(poc.ghi_chu_bep, '') as ghi_chu_bep
                    FROM phieu_order_ct poc
                    INNER JOIN mon_an m ON poc.mon_id = m.mon_id
                    WHERE poc.phieu_order_id = @kotId
                    ORDER BY poc.order_ct_id";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@kotId", kotId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - Lấy chi tiết KOT: {ex.Message}");
            }
        }

        public int TaoKOT(int banId, int chiNhanhId, int nguoiDungId, string loaiKot, string? ghiChu = null, bool uuTien = false)
        {
            try
            {
                string query = @"
                    INSERT INTO phieu_order (chi_nhanh_id, ban_id, ngay_gio, nhan_vien, trang_thai)
                    VALUES (@chiNhanhId, @banId, @ngayGio, @nhanVien, @trangThai);
                    SELECT SCOPE_IDENTITY();";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId),
                    new SqlParameter("@banId", banId),
                    new SqlParameter("@ngayGio", DateTime.Now),
                    new SqlParameter("@nhanVien", nguoiDungId.ToString()),
                    new SqlParameter("@trangThai", "ĐANG PHỤC VỤ")
                };

                var result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - Tạo KOT: {ex.Message}");
            }
        }

        public bool ThemChiTietKOT(int kotId, int monId, int soLuong, string? ghiChu = null)
        {
            try
            {
                string query = @"
                    INSERT INTO phieu_order_ct (phieu_order_id, mon_id, so_luong, don_gia, ghi_chu_bep)
                    VALUES (@phieuOrderId, @monId, @soLuong, @donGia, @ghiChu)";

                // Lấy đơn giá từ bảng mon_an
                string getPriceQuery = "SELECT don_gia FROM mon_an WHERE mon_id = @monId";
                var priceParam = new SqlParameter("@monId", monId);
                var donGia = _dbHelper.ExecuteScalar(getPriceQuery, new SqlParameter[] { priceParam });

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@phieuOrderId", kotId),
                    new SqlParameter("@monId", monId),
                    new SqlParameter("@soLuong", soLuong),
                    new SqlParameter("@donGia", donGia ?? 0),
                    new SqlParameter("@ghiChu", ghiChu ?? (object)DBNull.Value)
                };

                int result = _dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - Thêm chi tiết KOT: {ex.Message}");
            }
        }

        public bool CapNhatTrangThaiKOT(int kotId, string trangThai)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(trangThai))
                {
                    throw new ArgumentException("Trạng thái không được để trống");
                }

                // Trim và validate giá trị trạng thái
                string trangThaiTrimmed = trangThai.Trim();
                string[] validStatuses = { "ĐANG PHỤC VỤ", "CHỜ THANH TOÁN", "ĐÃ ĐÓNG", "SẴN SÀNG" };
                if (!validStatuses.Contains(trangThaiTrimmed))
                {
                    throw new ArgumentException($"Trạng thái không hợp lệ: {trangThaiTrimmed}");
                }

                string query = "UPDATE phieu_order SET trang_thai = @trangThai WHERE phieu_order_id = @kotId";

                var trangThaiParam = new SqlParameter("@trangThai", System.Data.SqlDbType.NVarChar, 20)
                {
                    Value = trangThaiTrimmed
                };

                var parameters = new SqlParameter[]
                {
                    trangThaiParam,
                    new SqlParameter("@kotId", kotId)
                };

                int result = _dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - Cập nhật trạng thái KOT: {ex.Message}");
            }
        }


        public DataTable LayThongKeKOT(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT 
                        COUNT(*) as tong_kot,
                        SUM(CASE WHEN trang_thai = N'ĐANG PHỤC VỤ' THEN 1 ELSE 0 END) as cho_lam,
                        SUM(CASE WHEN trang_thai = N'CHỜ THANH TOÁN' THEN 1 ELSE 0 END) as dang_lam,
                        SUM(CASE WHEN trang_thai = N'ĐÃ ĐÓNG' THEN 1 ELSE 0 END) as san_sang
                    FROM phieu_order po
                    WHERE po.chi_nhanh_id = @chiNhanhId";

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - Lấy thống kê KOT: {ex.Message}");
            }
        }

    }
}
