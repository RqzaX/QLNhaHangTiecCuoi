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
    }
}
