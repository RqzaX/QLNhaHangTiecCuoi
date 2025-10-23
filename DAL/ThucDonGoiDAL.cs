using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace DAL
{
    public class ThucDonGoiDAL
    {
        private readonly DatabaseHelper _dbHelper;

        public ThucDonGoiDAL(string connectionString = null)
        {
            _dbHelper = new DatabaseHelper(connectionString);
        }

        // Lấy danh sách món ăn
        public DataTable GetDanhSachMonAn()
        {
            string query = @"
                SELECT 
                    mon_id AS [ID],
                    ten_mon AS [TenMon],
                    ISNULL(nhom, N'Chưa phân loại') AS [DanhMuc],
                    don_gia AS [GiaBan],
                    0 AS [GiaVon],
                    CASE WHEN dang_ban = 1 THEN N'Còn hàng' ELSE N'Hết hàng' END AS [TrangThai]
                FROM dbo.mon_an
                ORDER BY ten_mon";

            try
            {
                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách món ăn: {ex.Message}", ex);
            }
        }

        // Lấy danh sách gói tiệc
        public DataTable GetDanhSachGoiTiec()
        {
            string query = @"
                SELECT 
                    goi_id AS [ID],
                    ten_goi AS [TenMon],
                    N'Gói tiệc cưới' AS [DanhMuc],
                    gia_co_ban AS [GiaBan],
                    0 AS [GiaVon],
                    N'Có sẵn' AS [TrangThai]
                FROM dbo.goi_tiec
                ORDER BY ten_goi";

            try
            {
                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách gói tiệc: {ex.Message}", ex);
            }
        }

        // Xóa món ăn
       

        // Xóa gói tiệc
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

        // Lấy chi tiết món ăn
        public DataRow GetChiTietMonAn(int monId)
        {
            string query = @"
                SELECT mon_id, ma_mon, ten_mon, nhom, don_vi_tinh, don_gia, dang_ban
                FROM dbo.mon_an
                WHERE mon_id = @monId";

            SqlParameter[] parameters = {
                new SqlParameter("@monId", monId)
            };

            try
            {
                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy chi tiết món ăn: {ex.Message}", ex);
            }
        }

        // Lấy chi tiết gói tiệc
        public DataRow GetChiTietGoiTiec(int goiId)
        {
            string query = @"
                SELECT goi_id, ma_goi, ten_goi, gia_co_ban
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
                throw new Exception($"Lỗi khi lấy chi tiết gói tiệc: {ex.Message}", ex);
            }
        }

        // Tìm kiếm món ăn
        public DataTable TimKiemMonAn(string keyword)
        {
            string query = @"
                SELECT 
                    mon_id AS [ID],
                    ten_mon AS [TenMon],
                    ISNULL(nhom, N'Chưa phân loại') AS [DanhMuc],
                    don_gia AS [GiaBan],
                    0 AS [GiaVon],
                    CASE WHEN dang_ban = 1 THEN N'Còn hàng' ELSE N'Hết hàng' END AS [TrangThai]
                FROM dbo.mon_an
                WHERE ten_mon LIKE @keyword OR ma_mon LIKE @keyword OR nhom LIKE @keyword
                ORDER BY ten_mon";

            SqlParameter[] parameters = {
                new SqlParameter("@keyword", $"%{keyword}%")
            };

            try
            {
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm món ăn: {ex.Message}", ex);
            }
        }

        // Tìm kiếm gói tiệc
        public DataTable TimKiemGoiTiec(string keyword)
        {
            string query = @"
                SELECT 
                    goi_id AS [ID],
                    ten_goi AS [TenMon],
                    N'Gói tiệc cưới' AS [DanhMuc],
                    gia_co_ban AS [GiaBan],
                    0 AS [GiaVon],
                    N'Có sẵn' AS [TrangThai]
                FROM dbo.goi_tiec
                WHERE ten_goi LIKE @keyword OR ma_goi LIKE @keyword
                ORDER BY ten_goi";

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

        // Test kết nối
        public bool TestConnection()
        {
            return _dbHelper.TestConnection();
        }

        // INSERT
        public int ThemMonAn(string maMon, string tenMon, string nhom, string donViTinh, decimal donGia, bool dangBan)
        {
            const string sql = @"
        INSERT INTO dbo.mon_an (ma_mon, ten_mon, nhom, don_vi_tinh, don_gia, dang_ban)
        VALUES (@ma, @ten, @nhom, @dvt, @gia, @dang);
        SELECT CAST(SCOPE_IDENTITY() AS int);";

            var prms = new[]
            {
        new SqlParameter("@ma",   (object)maMon ?? DBNull.Value),
        new SqlParameter("@ten",  (object)tenMon ?? DBNull.Value),
        new SqlParameter("@nhom", string.IsNullOrWhiteSpace(nhom) ? (object)DBNull.Value : nhom),
        new SqlParameter("@dvt",  (object)donViTinh ?? DBNull.Value),
        new SqlParameter("@gia",  donGia),
        new SqlParameter("@dang", dangBan ? 1 : 0),
    };

            try
            {
                var id = _dbHelper.ExecuteScalar(sql, prms);
                return id == null ? 0 : Convert.ToInt32(id);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã món đã tồn tại.", ex);
            }
        }

        // UPDATE
        public int CapNhatMonAn(int monId, string maMon, string tenMon, string nhom, string donViTinh, decimal donGia, bool dangBan)
        {
            const string sql = @"
        UPDATE dbo.mon_an
        SET ma_mon=@ma, ten_mon=@ten, nhom=@nhom, don_vi_tinh=@dvt, don_gia=@gia, dang_ban=@dang
        WHERE mon_id=@id;";

            var prms = new[]
            {
        new SqlParameter("@id",   monId),
        new SqlParameter("@ma",   (object)maMon ?? DBNull.Value),
        new SqlParameter("@ten",  (object)tenMon ?? DBNull.Value),
        new SqlParameter("@nhom", string.IsNullOrWhiteSpace(nhom) ? (object)DBNull.Value : nhom),
        new SqlParameter("@dvt",  (object)donViTinh ?? DBNull.Value),
        new SqlParameter("@gia",  donGia),
        new SqlParameter("@dang", dangBan ? 1 : 0),
    };

            try
            {
                return _dbHelper.ExecuteNonQuery(sql, prms);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new Exception("Mã món đã tồn tại.", ex);
            }
        }

       
        public int XoaMonAn(int monId)
        {
            const string sql = @"DELETE FROM dbo.mon_an WHERE mon_id=@id;";
            return _dbHelper.ExecuteNonQuery(sql, new[] { new SqlParameter("@id", monId) });
        }

        public DataRow? GetMonAnByIdRow(int monId)
        {
            const string sql = @"
        SELECT mon_id, ma_mon, ten_mon, nhom, don_vi_tinh, don_gia, dang_ban
        FROM dbo.mon_an
        WHERE mon_id = @id;";

            var prms = new[] { new SqlParameter("@id", monId) };

            // Dùng đúng helper có sẵn
            DataTable dt = _dbHelper.GetDataTable(sql, prms);
            return (dt != null && dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }
    }
}