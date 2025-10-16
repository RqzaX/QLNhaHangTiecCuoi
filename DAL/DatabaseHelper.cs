// ========== DAL LAYER (Data Access Layer) ==========
using QLNhaHangTiecCuoi.DAL;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QLNhaHangTiecCuoi.DAL
{
    public class DatabaseHelper
    {
        private string _connectionString = @"Server=LAPTOP-R1ZAX\SQLEXPRESS;Database=QL_NhaHangTiecCuoi_V3;Integrated Security=true;TrustServerCertificate=true;";

        public DatabaseHelper(string connectionString = null)
        {
            if (!string.IsNullOrEmpty(connectionString))
                _connectionString = connectionString;
        }

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy DataTable từ stored procedure hoặc câu query
        /// </summary>
        public DataTable GetDataTable(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi truy vấn database: " + ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// Lấy một giá trị scalar
        /// </summary>
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi ExecuteScalar: " + ex.Message);
            }
        }

        /// <summary>
        /// Thực thi non-query (Insert, Update, Delete)
        /// </summary>
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi ExecuteNonQuery: " + ex.Message);
            }
        }
    }

    // ===== DAL: Xử lý đăng nhập =====
    public class NguoiDungDAL
    {
        private DatabaseHelper _dbHelper;

        public NguoiDungDAL(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        /// <summary>
        /// Kiểm tra thông tin đăng nhập
        /// </summary>
        public DataTable DangNhap(string taiKhoan, string matKhau)
        {
            string query = @"
                SELECT nguoi_dung_id, tai_khoan, ho_ten, hoat_dong
                FROM dbo.nguoi_dung
                WHERE tai_khoan = @taiKhoan 
                  AND mat_khau = @matKhau
                  AND hoat_dong = 1";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@taiKhoan", taiKhoan),
                new SqlParameter("@matKhau", matKhau)
            };

            return _dbHelper.GetDataTable(query, parameters);
        }

        /// <summary>
        /// Lấy các chi nhánh theo người dùng
        /// </summary>
        public DataTable LayDanhSachChiNhanh()
        {
            string query = @"
                SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai
                FROM dbo.chi_nhanh
                WHERE trang_thai = 1
                ORDER BY ten";

            return _dbHelper.GetDataTable(query);
        }

        /// <summary>
        /// Lấy thông tin chi nhánh theo ID
        /// </summary>
        public DataTable LayChiNhanhById(int chiNhanhId)
        {
            string query = @"
                SELECT chi_nhanh_id, ten, dia_chi, sdt
                FROM dbo.chi_nhanh
                WHERE chi_nhanh_id = @chiNhanhId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@chiNhanhId", chiNhanhId)
            };

            return _dbHelper.GetDataTable(query, parameters);
        }

        /// <summary>
        /// Cập nhật thông tin đăng nhập lần cuối
        /// </summary>
        public void CapNhatDangNhapLanCuoi(int nguoiDungId)
        {
            string query = @"
                UPDATE dbo.nguoi_dung
                SET hoat_dong = 1
                WHERE nguoi_dung_id = @nguoiDungId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungId", nguoiDungId)
            };

            _dbHelper.ExecuteNonQuery(query, parameters);
        }
    }
}