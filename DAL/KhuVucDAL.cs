using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.DAL
{
    public class KhuVucDAL
    {
        private DatabaseHelper _dbHelper;

        public KhuVucDAL()
        {
            _dbHelper = new DatabaseHelper();
            EnsureMoTaColumnExists();
        }

        private void EnsureMoTaColumnExists()
        {
            try
            {
                // Kiểm tra xem cột mo_ta đã tồn tại chưa
                string checkQuery = @"
                    SELECT COUNT(*) 
                    FROM sys.columns 
                    WHERE object_id = OBJECT_ID(N'dbo.khu_vuc') 
                    AND name = N'mo_ta'";

                object result = _dbHelper.ExecuteScalar(checkQuery);
                int count = Convert.ToInt32(result);

                if (count == 0)
                {
                    // Cột chưa tồn tại, thêm vào
                    string alterQuery = @"
                        ALTER TABLE dbo.khu_vuc
                        ADD mo_ta NVARCHAR(300) NULL";

                    _dbHelper.ExecuteNonQuery(alterQuery);

                    // Cập nhật mô tả mặc định cho các khu vực đã tồn tại
                    string updateQuery = @"
                        UPDATE dbo.khu_vuc
                        SET mo_ta = CASE 
                            WHEN ten_khu_vuc = N'Tầng 1' THEN N'Khu vực chính, gần cửa sổ'
                            WHEN ten_khu_vuc = N'Tầng 2' THEN N'Khu vực VIP, yên tĩnh'
                            WHEN ten_khu_vuc = N'Ngoài trời' THEN N'Không gian thoáng mát, view đẹp'
                            WHEN ten_khu_vuc LIKE N'%Khu A%' OR ten_khu_vuc LIKE N'Khu A%' THEN N'Khu vực chính, gần cửa sổ'
                            WHEN ten_khu_vuc LIKE N'%Khu B%' OR ten_khu_vuc LIKE N'Khu B%' THEN N'Khu vực VIP, yên tĩnh'
                            WHEN ten_khu_vuc LIKE N'%Khu VIP%' OR ten_khu_vuc LIKE N'Khu VIP%' THEN N'Khu vực VIP, yên tĩnh'
                            WHEN ten_khu_vuc LIKE N'%Sân thượng%' THEN N'Không gian thoáng mát, view đẹp'
                            ELSE N'Khu vực chính'
                        END
                        WHERE mo_ta IS NULL";

                    _dbHelper.ExecuteNonQuery(updateQuery);
                }
                else
                {
                    // Cột đã tồn tại, nhưng cập nhật mô tả cho các khu vực chưa có mô tả
                    string updateQuery = @"
                        UPDATE dbo.khu_vuc
                        SET mo_ta = CASE 
                            WHEN ten_khu_vuc = N'Tầng 1' THEN N'Khu vực chính, gần cửa sổ'
                            WHEN ten_khu_vuc = N'Tầng 2' THEN N'Khu vực VIP, yên tĩnh'
                            WHEN ten_khu_vuc = N'Ngoài trời' THEN N'Không gian thoáng mát, view đẹp'
                            WHEN ten_khu_vuc LIKE N'%Khu A%' OR ten_khu_vuc LIKE N'Khu A%' THEN N'Khu vực chính, gần cửa sổ'
                            WHEN ten_khu_vuc LIKE N'%Khu B%' OR ten_khu_vuc LIKE N'Khu B%' THEN N'Khu vực VIP, yên tĩnh'
                            WHEN ten_khu_vuc LIKE N'%Khu VIP%' OR ten_khu_vuc LIKE N'Khu VIP%' THEN N'Khu vực VIP, yên tĩnh'
                            WHEN ten_khu_vuc LIKE N'%Sân thượng%' THEN N'Không gian thoáng mát, view đẹp'
                            ELSE N'Khu vực chính'
                        END
                        WHERE mo_ta IS NULL OR mo_ta = ''";

                    _dbHelper.ExecuteNonQuery(updateQuery);
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để không làm gián đoạn ứng dụng
                // Có thể thêm logging ở đây nếu cần
                System.Diagnostics.Debug.WriteLine($"Lỗi kiểm tra/thêm cột mo_ta: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhuVuc(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT khu_vuc_id, ten_khu_vuc
                    FROM khu_vuc 
                    WHERE chi_nhanh_id = @chiNhanhId
                    ORDER BY ten_khu_vuc";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khu vực: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhuVucVoiSoBan(int? chiNhanhId = null)
        {
            try
            {
                // Tối ưu query bằng cách dùng CTE để pre-aggregate số bàn
                // Thêm NOLOCK hint để giảm blocking và tăng tốc độ đọc
                string query = @"
                    WITH BanStats AS (
                        SELECT 
                            khu_vuc_id,
                            COUNT(ban_id) AS SoBan
                        FROM dbo.ban WITH (NOLOCK)
                        GROUP BY khu_vuc_id
                    )
                    SELECT 
                        kv.khu_vuc_id,
                        kv.ten_khu_vuc,
                        kv.mo_ta,
                        ISNULL(bs.SoBan, 0) AS so_ban,
                        CAST(ISNULL(bs.SoBan, 0) AS NVARCHAR(10)) + N' bàn' AS so_ban_text
                    FROM dbo.khu_vuc kv WITH (NOLOCK)
                    LEFT JOIN BanStats bs ON bs.khu_vuc_id = kv.khu_vuc_id
                    WHERE (@chiNhanhId IS NULL OR kv.chi_nhanh_id = @chiNhanhId)
                    ORDER BY kv.ten_khu_vuc";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId ?? (object)DBNull.Value)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khu vực với số bàn: {ex.Message}");
            }
        }

        public DataRow LayKhuVucById(int khuVucId)
        {
            try
            {
                string query = @"
                    SELECT 
                        kv.khu_vuc_id,
                        kv.ten_khu_vuc,
                        kv.mo_ta,
                        kv.chi_nhanh_id,
                        cn.ten AS ten_chi_nhanh,
                        ISNULL(COUNT(b.ban_id), 0) AS so_ban
                    FROM dbo.khu_vuc kv WITH (NOLOCK)
                    INNER JOIN dbo.chi_nhanh cn WITH (NOLOCK) ON cn.chi_nhanh_id = kv.chi_nhanh_id
                    LEFT JOIN dbo.ban b WITH (NOLOCK) ON b.khu_vuc_id = kv.khu_vuc_id
                    WHERE kv.khu_vuc_id = @khuVucId
                    GROUP BY kv.khu_vuc_id, kv.ten_khu_vuc, kv.mo_ta, kv.chi_nhanh_id, cn.ten";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@khuVucId", khuVucId)
                };

                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin khu vực: {ex.Message}");
            }
        }

        public bool CapNhatKhuVuc(int khuVucId, string tenKhuVuc, string moTa)
        {
            try
            {
                string query = @"
                    UPDATE dbo.khu_vuc
                    SET ten_khu_vuc = @tenKhuVuc,
                        mo_ta = @moTa
                    WHERE khu_vuc_id = @khuVucId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@khuVucId", khuVucId),
                    new SqlParameter("@tenKhuVuc", tenKhuVuc ?? (object)DBNull.Value),
                    new SqlParameter("@moTa", moTa ?? (object)DBNull.Value)
                };

                int result = _dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật khu vực: {ex.Message}");
            }
        }

        public int ThemKhuVuc(int chiNhanhId, string tenKhuVuc, string moTa)
        {
            try
            {
                string query = @"
                    INSERT INTO dbo.khu_vuc (chi_nhanh_id, ten_khu_vuc, mo_ta)
                    OUTPUT INSERTED.khu_vuc_id
                    VALUES (@chiNhanhId, @tenKhuVuc, @moTa)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId),
                    new SqlParameter("@tenKhuVuc", tenKhuVuc ?? (object)DBNull.Value),
                    new SqlParameter("@moTa", moTa ?? (object)DBNull.Value)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm khu vực: {ex.Message}");
            }
        }

        public bool XoaKhuVuc(int khuVucId)
        {
            try
            {
                // Xóa các bàn trong khu vực trước
                string deleteBanQuery = "DELETE FROM dbo.ban WHERE khu_vuc_id = @khuVucId";
                SqlParameter[] banParams = new SqlParameter[]
                {
                    new SqlParameter("@khuVucId", khuVucId)
                };
                _dbHelper.ExecuteNonQuery(deleteBanQuery, banParams);

                // Sau đó xóa khu vực
                string deleteKhuVucQuery = "DELETE FROM dbo.khu_vuc WHERE khu_vuc_id = @khuVucId";
                SqlParameter[] khuVucParams = new SqlParameter[]
                {
                    new SqlParameter("@khuVucId", khuVucId)
                };

                int result = _dbHelper.ExecuteNonQuery(deleteKhuVucQuery, khuVucParams);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa khu vực: {ex.Message}");
            }
        }
    }
}
