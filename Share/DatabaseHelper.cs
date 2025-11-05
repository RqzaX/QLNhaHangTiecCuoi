using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QLNhaHangTiecCuoi.Share
{
    public class DatabaseHelper
    {
        private string _connectionString = @"Data Source=LAPTOP-2L5G5GIH\SQLEXPRESS03;Initial Catalog=QL_NhaHangTiecCuoi_V3;Integrated Security=True;Encrypt=True";

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
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;

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
                    cmd.CommandTimeout = 30;

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
                    cmd.CommandTimeout = 30;

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
}