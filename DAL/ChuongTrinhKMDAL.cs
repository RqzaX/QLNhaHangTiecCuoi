using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ChuongTrinhKMDAL
    {
        private DatabaseHelper _dbHelper;

        public ChuongTrinhKMDAL()
        {
            _dbHelper = new DatabaseHelper();
        }

        public DataTable GetAll()
        {
            try
            {
                
                string query = @"
                    SELECT 
                        km.km_id AS 'ID',
                        km.ma_km AS 'MaKM',
                        km.ten AS 'TenCT',
                        km.hinh_thuc AS 'HinhThuc',
                        km.gia_tri AS 'GiaTri',
                        km.tg_bat_dau AS 'TgBatDau',
                        km.tg_ket_thuc AS 'TgKetThuc',
                        km.ap_dung_loai AS 'ApDungLoai',
                        ISNULL(SUM(CAST(v.da_dung AS BIGINT)), 0) AS 'DaDung',
                        ISNULL(SUM(CAST(v.so_lan AS BIGINT)), 0) AS 'TongSoLan'
                    FROM dbo.chuong_trinh_km km
                    LEFT JOIN dbo.voucher v ON v.km_id = km.km_id
                    GROUP BY 
                        km.km_id, 
                        km.ma_km, 
                        km.ten, 
                        km.hinh_thuc, 
                        km.gia_tri, 
                        km.tg_bat_dau, 
                        km.tg_ket_thuc, 
                        km.ap_dung_loai
                    ORDER BY km.tg_bat_dau DESC";

                
                return _dbHelper.GetDataTable(query, null, 300);
            }
            catch (Exception ex)
            {
                
                if (ex.Message.Contains("Timeout") || ex.Message.Contains("timeout"))
                {
                    try
                    {
                        string queryBasic = @"
                            SELECT 
                                km.km_id AS 'ID',
                                km.ma_km AS 'MaKM',
                                km.ten AS 'TenCT',
                                km.hinh_thuc AS 'HinhThuc',
                                km.gia_tri AS 'GiaTri',
                                km.tg_bat_dau AS 'TgBatDau',
                                km.tg_ket_thuc AS 'TgKetThuc',
                                km.ap_dung_loai AS 'ApDungLoai',
                                0 AS 'DaDung',
                                0 AS 'TongSoLan'
                            FROM dbo.chuong_trinh_km km
                            ORDER BY km.tg_bat_dau DESC";
                        
                        return _dbHelper.GetDataTable(queryBasic, null, 60);
                    }
                    catch (Exception ex2)
                    {
                        throw new Exception("Lỗi DAL GetAll: " + ex2.Message);
                    }
                }
                throw new Exception("Lỗi DAL GetAll: " + ex.Message);
            }
        }

        public bool Insert(string maKm, string ten, string hinhThuc, decimal giaTri,
                           DateTime tgBatDau, DateTime tgKetThuc, string apDungLoai)
        {
            try
            {
                string query = @"
                    INSERT INTO dbo.chuong_trinh_km 
                    (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
                    VALUES 
                    (@MaKm, @Ten, @HinhThuc, @GiaTri, @TgBatDau, @TgKetThuc, @ApDungLoai)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaKm", maKm),
                    new SqlParameter("@Ten", ten),
                    new SqlParameter("@HinhThuc", hinhThuc),
                    new SqlParameter("@GiaTri", giaTri),
                    new SqlParameter("@TgBatDau", tgBatDau),
                    new SqlParameter("@TgKetThuc", tgKetThuc),
                    new SqlParameter("@ApDungLoai", apDungLoai ?? "ALL")
                };

                int result = _dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL Insert: " + ex.Message);
            }
        }

        public bool Update(int kmId, string maKm, string ten, string hinhThuc, decimal giaTri,
                           DateTime tgBatDau, DateTime tgKetThuc, string apDungLoai)
        {
            try
            {
                string query = @"
                    UPDATE dbo.chuong_trinh_km
                    SET 
                        ma_km = @MaKm,
                        ten = @Ten,
                        hinh_thuc = @HinhThuc,
                        gia_tri = @GiaTri,
                        tg_bat_dau = @TgBatDau,
                        tg_ket_thuc = @TgKetThuc,
                        ap_dung_loai = @ApDungLoai
                    WHERE km_id = @KmId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@KmId", kmId),
                    new SqlParameter("@MaKm", maKm),
                    new SqlParameter("@Ten", ten),
                    new SqlParameter("@HinhThuc", hinhThuc),
                    new SqlParameter("@GiaTri", giaTri),
                    new SqlParameter("@TgBatDau", tgBatDau),
                    new SqlParameter("@TgKetThuc", tgKetThuc),
                    new SqlParameter("@ApDungLoai", apDungLoai ?? "ALL")
                };

                int result = _dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL Update: " + ex.Message);
            }
        }

        public int CountVouchersByKmId(int kmId)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM dbo.voucher WHERE km_id = @KmId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@KmId", kmId)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL CountVouchersByKmId: " + ex.Message);
            }
        }

        public bool Delete(int kmId)
        {
            SqlConnection conn = null;
            SqlTransaction transaction = null;
            try
            {
                conn = new SqlConnection(_dbHelper.ConnectionString);
                conn.Open();
                transaction = conn.BeginTransaction();

                string deleteHoaDonKmQuery = @"
                    DELETE FROM dbo.hoa_don_km 
                    WHERE voucher_id IN (SELECT voucher_id FROM dbo.voucher WHERE km_id = @KmId)";
                SqlParameter[] param1 = new SqlParameter[] { new SqlParameter("@KmId", kmId) };
                _dbHelper.ExecuteNonQueryInTransaction(conn, transaction, deleteHoaDonKmQuery, param1);

                string deleteVouchersQuery = @"DELETE FROM dbo.voucher WHERE km_id = @KmId";
                SqlParameter[] param2 = new SqlParameter[] { new SqlParameter("@KmId", kmId) };
                _dbHelper.ExecuteNonQueryInTransaction(conn, transaction, deleteVouchersQuery, param2);

                string deleteHoaDonKmByKmIdQuery = @"DELETE FROM dbo.hoa_don_km WHERE km_id = @KmId";
                SqlParameter[] param3 = new SqlParameter[] { new SqlParameter("@KmId", kmId) };
                _dbHelper.ExecuteNonQueryInTransaction(conn, transaction, deleteHoaDonKmByKmIdQuery, param3);

                string deleteKmQuery = @"DELETE FROM dbo.chuong_trinh_km WHERE km_id = @KmId";
                SqlParameter[] param4 = new SqlParameter[] { new SqlParameter("@KmId", kmId) };
                int result = _dbHelper.ExecuteNonQueryInTransaction(conn, transaction, deleteKmQuery, param4);

                transaction.Commit();
                return result > 0;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch { }
                }
                throw new Exception("Lỗi DAL Delete: " + ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                transaction?.Dispose();
                conn?.Dispose();
            }
        }

        public bool MaKmExists(string maKm, int? excludeKmId = null)
        {
            try
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM dbo.chuong_trinh_km 
                    WHERE ma_km = @MaKm";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@MaKm", maKm)
                };

                if (excludeKmId.HasValue)
                {
                    query += " AND km_id != @ExcludeKmId";
                    parameters.Add(new SqlParameter("@ExcludeKmId", excludeKmId.Value));
                }

                object result = _dbHelper.ExecuteScalar(query, parameters.ToArray());
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL MaKmExists: " + ex.Message);
            }
        }

        public DataRow GetById(int kmId)
        {
            try
            {
                string query = @"
                    SELECT 
                        km_id,
                        ma_km,
                        ten,
                        hinh_thuc,
                        gia_tri,
                        tg_bat_dau,
                        tg_ket_thuc,
                        ap_dung_loai
                    FROM dbo.chuong_trinh_km
                    WHERE km_id = @KmId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@KmId", kmId)
                };

                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL GetById: " + ex.Message);
            }
        }
    }
}
