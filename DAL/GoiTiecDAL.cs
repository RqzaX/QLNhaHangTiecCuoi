using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace DAL
{
    public class GoiTiecDAL
    {
        private readonly DatabaseHelper _dbHelper;

        public GoiTiecDAL()
        {
            _dbHelper = new DatabaseHelper();
        }
        // Lấy chi tiết gói theo goi_id
        public DataTable GetChiTietGoiTiec(int goiId)
        {
            const string sql = @"
        SELECT 
            m.ma_mon,
            m.ten_mon,
            gtm.so_luong
        FROM dbo.goi_tiec_mon gtm
        JOIN dbo.mon_an m   ON m.mon_id  = gtm.mon_id
        WHERE gtm.goi_id = @goi_id
        ORDER BY m.ten_mon;";

            var prms = new[]
            {
        new SqlParameter("@goi_id", goiId)
    };

            try
            {
                return _dbHelper.GetDataTable(sql, prms);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetChiTietGoiTiec: {ex.Message}", ex);
            }
        }
        public int GetMonIdByMaMon(string maMon)
        {
            const string sql = "SELECT mon_id FROM dbo.mon_an WHERE ma_mon = @ma";
            var dt = _dbHelper.GetDataTable(sql, new[] { new SqlParameter("@ma", maMon) });
            if (dt.Rows.Count == 0) return 0;
            return Convert.ToInt32(dt.Rows[0]["mon_id"]);
        }

        // Thêm/Upsert món vào gói (nếu đã có thì cập nhật số lượng)
        public int UpsertMonVaoGoi(int goiId, int monId, decimal soLuong)
        {
            const string sql = @"
IF EXISTS (SELECT 1 FROM dbo.goi_tiec_mon WHERE goi_id = @goi_id AND mon_id = @mon_id)
    UPDATE dbo.goi_tiec_mon SET so_luong = @so_luong
    WHERE goi_id = @goi_id AND mon_id = @mon_id;
ELSE
    INSERT INTO dbo.goi_tiec_mon(goi_id, mon_id, so_luong)
    VALUES(@goi_id, @mon_id, @so_luong);";
            return _dbHelper.ExecuteNonQuery(sql, new[]
            {
        new SqlParameter("@goi_id", goiId),
        new SqlParameter("@mon_id", monId),
        new SqlParameter("@so_luong", soLuong)
    });
        }

        // Cập nhật số lượng (và có thể đổi món: xóa cũ → thêm mới)
        public int UpdateMonTrongGoi(int goiId, int oldMonId, int newMonId, decimal newSoLuong)
        {
            // nếu không đổi món, chỉ update số lượng
            if (oldMonId == newMonId)
            {
                const string sql1 = @"UPDATE dbo.goi_tiec_mon
                              SET so_luong = @so_luong
                              WHERE goi_id = @goi_id AND mon_id = @mon_id;";
                return _dbHelper.ExecuteNonQuery(sql1, new[]
                {
            new SqlParameter("@goi_id", goiId),
            new SqlParameter("@mon_id", newMonId),
            new SqlParameter("@so_luong", newSoLuong)
        });
            }

            // đổi sang món khác: xóa cũ → upsert mới (đơn giản, an toàn)
            const string del = @"DELETE FROM dbo.goi_tiec_mon WHERE goi_id=@goi_id AND mon_id=@old_id;";
            _dbHelper.ExecuteNonQuery(del, new[]
            {
        new SqlParameter("@goi_id", goiId),
        new SqlParameter("@old_id", oldMonId)
    });

            return UpsertMonVaoGoi(goiId, newMonId, newSoLuong);
        }

        // Xóa món khỏi gói
        public int DeleteMonKhoiGoi(int goiId, int monId)
        {
            const string sql = @"DELETE FROM dbo.goi_tiec_mon WHERE goi_id=@goi_id AND mon_id=@mon_id;";
            return _dbHelper.ExecuteNonQuery(sql, new[]
            {
        new SqlParameter("@goi_id", goiId),
        new SqlParameter("@mon_id", monId)
    });
        }
        public DataTable GetMonTrongGoi(int goiId)
        {
            // alias cho đồng nhất tên hàm giữa các lớp
            return GetChiTietGoiTiec(goiId);
        }
        public int GetGoiIdByTenGoi(string tenGoi)
        {
            const string sql = "SELECT TOP(1) goi_id FROM dbo.goi_tiec WHERE ten_goi = @ten";
            var dt = _dbHelper.GetDataTable(sql, new[] { new SqlParameter("@ten", tenGoi) });
            if (dt.Rows.Count == 0) return 0;
            return Convert.ToInt32(dt.Rows[0]["goi_id"]);
        }
        public int GetGoiIdByMaGoi(string maGoi)
        {
            const string sql = "SELECT goi_id FROM dbo.goi_tiec WHERE ma_goi = @ma";
            var prms = new[] { new SqlParameter("@ma", maGoi) };
            var dt = _dbHelper.GetDataTable(sql, prms);
            if (dt.Rows.Count == 0) return 0;
            return Convert.ToInt32(dt.Rows[0]["goi_id"]);
        }
        // (Tùy chọn) Lấy chi tiết gói theo ma_goi
        public DataTable GetChiTietGoiTiec_ByMa(string maGoi)
        {
            const string sql = @"
        SELECT 
            m.ma_mon,
            m.ten_mon,
            gtm.so_luong
        FROM dbo.goi_tiec_mon gtm
        JOIN dbo.goi_tiec g ON g.goi_id = gtm.goi_id
        JOIN dbo.mon_an m   ON m.mon_id = gtm.mon_id
        WHERE g.ma_goi = @ma_goi
        ORDER BY m.ten_mon;";

            var prms = new[]
            {
        new SqlParameter("@ma_goi", maGoi)
    };

            try
            {
                return _dbHelper.GetDataTable(sql, prms);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetChiTietGoiTiec_ByMa: {ex.Message}", ex);
            }
        }

       
        public DataTable GetAllGoiTiec()
        {
            const string query = @"
        SELECT 
            goi_id,        -- GIỮ NGUYÊN TÊN CỘT
            ma_goi,
            ten_goi,
            gia_co_ban,
            suc_chua
        FROM dbo.goi_tiec
        ORDER BY goi_id DESC";
            try
            {
                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách gói tiệc: {ex.Message}", ex);
            }
        }

       
        public DataRow GetGoiTiecById(int goiId)
        {
            string query = @"
                SELECT goi_id, ma_goi, ten_goi, gia_co_ban, suc_chua
                FROM dbo.goi_tiec
                WHERE goi_id = @goiId";

            SqlParameter[] parameters = {
                new SqlParameter("@goiId", goiId)
            };

            try
            {
                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin gói tiệc: {ex.Message}", ex);
            }
        }

        // Lấy sức chứa của gói tiệc theo ID
        public int GetSucChuaGoiTiec(int goiId)
        {
            const string sql = @"
                SELECT ISNULL(suc_chua, 0) AS suc_chua
                FROM dbo.goi_tiec
                WHERE goi_id = @goi_id";
            
            var prms = new[]
            {
                new SqlParameter("@goi_id", goiId)
            };

            try
            {
                var dt = _dbHelper.GetDataTable(sql, prms);
                if (dt.Rows.Count > 0 && dt.Rows[0]["suc_chua"] != DBNull.Value)
                {
                    return Convert.ToInt32(dt.Rows[0]["suc_chua"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetSucChuaGoiTiec: {ex.Message}", ex);
            }
        }

        // Tính tổng giá các món trong gói tiệc
        public decimal TinhTongGiaCacMon(int goiId)
        {
            const string sql = @"
                SELECT ISNULL(SUM(gtm.so_luong * m.don_gia), 0) AS tong_gia
                FROM dbo.goi_tiec_mon gtm
                INNER JOIN dbo.mon_an m ON m.mon_id = gtm.mon_id
                WHERE gtm.goi_id = @goi_id";
            
            var prms = new[]
            {
                new SqlParameter("@goi_id", goiId)
            };

            try
            {
                var result = _dbHelper.ExecuteScalar(sql, prms);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
                return 0m;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - TinhTongGiaCacMon: {ex.Message}", ex);
            }
        }

        // Thêm gói tiệc mới

        public bool ThemGoiTiec(string maGoi, string tenGoi, decimal giaCoBan)
        {
            string query = @"
                INSERT INTO dbo.goi_tiec (ma_goi, ten_goi, gia_co_ban)
                VALUES (@maGoi, @tenGoi, @giaCoBan)";

            SqlParameter[] parameters = {
                new SqlParameter("@maGoi", maGoi),
                new SqlParameter("@tenGoi", tenGoi),
                new SqlParameter("@giaCoBan", giaCoBan)
            };

            try
            {
                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm gói tiệc: {ex.Message}", ex);
            }
        }

        public bool CapNhatGoiTiec(int goiId, string maGoi, string tenGoi, decimal giaCoBan)
        {
            string query = @"
                UPDATE dbo.goi_tiec
                SET ma_goi = @maGoi,
                    ten_goi = @tenGoi,
                    gia_co_ban = @giaCoBan
                WHERE goi_id = @goiId";

            SqlParameter[] parameters = {
                new SqlParameter("@goiId", goiId),
                new SqlParameter("@maGoi", maGoi),
                new SqlParameter("@tenGoi", tenGoi),
                new SqlParameter("@giaCoBan", giaCoBan)
            };

            try
            {
                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật gói tiệc: {ex.Message}", ex);
            }
        }

    
        public bool XoaGoiTiec(int goiId)
        {
            string query = "DELETE FROM dbo.goi_tiec WHERE goi_id = @goiId";

            SqlParameter[] parameters = {
                new SqlParameter("@goiId", goiId)
            };

            try
            {
                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa gói tiệc: {ex.Message}", ex);
            }
        }

      
        public bool KiemTraMaGoiTonTai(string maGoi, int? goiIdBoQua = null)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM dbo.goi_tiec 
                WHERE ma_goi = @maGoi";

            if (goiIdBoQua.HasValue)
            {
                query += " AND goi_id != @goiIdBoQua";
            }

            SqlParameter[] parameters = goiIdBoQua.HasValue
                ? new[] {
                    new SqlParameter("@maGoi", maGoi),
                    new SqlParameter("@goiIdBoQua", goiIdBoQua.Value)
                }
                : new[] {
                    new SqlParameter("@maGoi", maGoi)
                };

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra mã gói: {ex.Message}", ex);
            }
        }

    
        public DataTable TimKiemGoiTiec(string keyword)
        {
            string query = @"
                SELECT 
                    goi_id AS [ID],
                    ma_goi AS [Mã Gói],
                    ten_goi AS [Tên Gói],
                    gia_co_ban AS [Giá Cơ Bản]
                FROM dbo.goi_tiec
                WHERE ten_goi LIKE @keyword OR ma_goi LIKE @keyword
                ORDER BY goi_id DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@keyword", $"%{keyword}%")
            };

            try
            {
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm gói tiệc: {ex.Message}", ex);
            }
        }

        // Lấy sức chứa tối đa từ tất cả sảnh
        public int GetSucChuaToiDaTuSanh()
        {
            const string sql = @"
                SELECT ISNULL(MAX(suc_chua), 0) AS suc_chua_toi_da
                FROM dbo.sanh";
            
            try
            {
                var dt = _dbHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0 && dt.Rows[0]["suc_chua_toi_da"] != DBNull.Value)
                {
                    return Convert.ToInt32(dt.Rows[0]["suc_chua_toi_da"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetSucChuaToiDaTuSanh: {ex.Message}", ex);
            }
        }

        // ========== DỊCH VỤ ==========
        // Lấy danh sách tất cả dịch vụ
        public DataTable GetAllDichVu()
        {
            const string sql = @"
                SELECT 
                    dv_id,
                    ma_dv,
                    ten_dv,
                    don_vi_tinh,
                    don_gia,
                    dang_ban
                FROM dbo.dich_vu
                WHERE dang_ban = 1
                ORDER BY ten_dv";
            
            try
            {
                return _dbHelper.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetAllDichVu: {ex.Message}", ex);
            }
        }

        // Lấy danh sách dịch vụ trong gói tiệc
        public DataTable GetDichVuTrongGoi(int goiId)
        {
            const string sql = @"
                SELECT 
                    dv.dv_id,
                    dv.ma_dv,
                    dv.ten_dv,
                    dv.don_vi_tinh,
                    dv.don_gia
                FROM dbo.goi_tiec_dv gtd
                JOIN dbo.dich_vu dv ON dv.dv_id = gtd.dv_id
                WHERE gtd.goi_id = @goi_id
                ORDER BY dv.ten_dv";
            
            var prms = new[]
            {
                new SqlParameter("@goi_id", goiId)
            };
            
            try
            {
                return _dbHelper.GetDataTable(sql, prms);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetDichVuTrongGoi: {ex.Message}", ex);
            }
        }

        // Lấy dv_id từ ma_dv
        public int GetDichVuIdByMaDv(string maDv)
        {
            const string sql = "SELECT dv_id FROM dbo.dich_vu WHERE ma_dv = @ma";
            var dt = _dbHelper.GetDataTable(sql, new[] { new SqlParameter("@ma", maDv) });
            if (dt.Rows.Count == 0) return 0;
            return Convert.ToInt32(dt.Rows[0]["dv_id"]);
        }

        // Thêm dịch vụ vào gói (nếu chưa có)
        public int ThemDichVuVaoGoi(int goiId, int dvId)
        {
            const string sql = @"
IF NOT EXISTS (SELECT 1 FROM dbo.goi_tiec_dv WHERE goi_id = @goi_id AND dv_id = @dv_id)
    INSERT INTO dbo.goi_tiec_dv(goi_id, dv_id)
    VALUES(@goi_id, @dv_id);";
            
            return _dbHelper.ExecuteNonQuery(sql, new[]
            {
                new SqlParameter("@goi_id", goiId),
                new SqlParameter("@dv_id", dvId)
            });
        }

        // Xóa dịch vụ khỏi gói
        public int XoaDichVuKhoiGoi(int goiId, int dvId)
        {
            const string sql = @"DELETE FROM dbo.goi_tiec_dv WHERE goi_id = @goi_id AND dv_id = @dv_id;";
            return _dbHelper.ExecuteNonQuery(sql, new[]
            {
                new SqlParameter("@goi_id", goiId),
                new SqlParameter("@dv_id", dvId)
            });
        }
    }
}