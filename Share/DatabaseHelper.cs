using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QLNhaHangTiecCuoi.Share
{
    public class DatabaseHelper
    {
        private string _connectionString = @"Server=LAPTOP-R1ZAX\SQLEXPRESS;Database=QL_NhaHangTiecCuoi_V3;Integrated Security=true;TrustServerCertificate=true;";
        public string ConnectionString => _connectionString;

        public DatabaseHelper(string connectionString = null)
        {
            if (!string.IsNullOrEmpty(connectionString))
                _connectionString = connectionString;
        }

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
        public DataTable GetDataTable(string query, SqlParameter[] parameters = null)
        {
            return GetDataTable(query, parameters, 120);
        }

        public DataTable GetDataTable(string query, SqlParameter[] parameters, int timeoutSeconds)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = timeoutSeconds;

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

        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 120;

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

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 120;

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

        public int ExecuteNonQueryInTransaction(SqlConnection conn, SqlTransaction transaction, string query, SqlParameter[] parameters = null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn, transaction);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi ExecuteNonQueryInTransaction: " + ex.Message);
            }
        }
    }
}