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

       
        public DataTable GetAllGoiTiec()
        {
            string query = @"
                SELECT 
                    goi_id AS [ID],
                    ma_goi AS [Mã Gói],
                    ten_goi AS [Tên Gói],
                    gia_co_ban AS [Giá Cơ Bản]
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
                throw new Exception($"Lỗi khi lấy thông tin gói tiệc: {ex.Message}", ex);
            }
        }

   
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
    }
}