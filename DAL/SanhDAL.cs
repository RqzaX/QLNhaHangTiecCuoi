using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.DAL
{
    public class SanhDAL
    {
        private readonly DatabaseHelper _dbHelper;

        public SanhDAL(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public DataTable LayDanhSachSanhTheoChiNhanh(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT s.sanh_id, s.chi_nhanh_id, s.ten_sanh, s.suc_chua, s.phi_thue_cb,
                           cn.ten as ten_chi_nhanh
                    FROM dbo.sanh s
                    LEFT JOIN dbo.chi_nhanh cn ON s.chi_nhanh_id = cn.chi_nhanh_id
                    WHERE s.chi_nhanh_id = @chiNhanhId
                    ORDER BY s.ten_sanh";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách sảnh: {ex.Message}");
            }
        }

        public DataTable LayThongTinSanh(int sanhId)
        {
            try
            {
                string query = @"
                    SELECT s.sanh_id, s.chi_nhanh_id, s.ten_sanh, s.suc_chua, s.phi_thue_cb,
                           cn.ten as ten_chi_nhanh
                    FROM dbo.sanh s
                    LEFT JOIN dbo.chi_nhanh cn ON s.chi_nhanh_id = cn.chi_nhanh_id
                    WHERE s.sanh_id = @sanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@sanhId", sanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin sảnh: {ex.Message}");
            }
        }
    }
}

