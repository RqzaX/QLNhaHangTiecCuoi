using Microsoft.Data.SqlClient;
using System.Data;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.DAL
{
    public class NguoiDungDAL
    {
        public DatabaseHelper _dbHelper;

        public NguoiDungDAL(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

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
        public DataTable LayDanhSachChiNhanh()
        {
            string query = @"
                SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai
                FROM dbo.chi_nhanh
                WHERE trang_thai = 1
                ORDER BY ten";

            return _dbHelper.GetDataTable(query);
        }
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

        public DataTable LayChiNhanhTheoNguoiDung(int nguoiDungId)
        {
            string query = @"
                SELECT c.chi_nhanh_id, c.ten, c.dia_chi, c.sdt, c.trang_thai
                FROM dbo.chi_nhanh c
                INNER JOIN dbo.nguoi_dung_chi_nhanh nc ON c.chi_nhanh_id = nc.chi_nhanh_id
                WHERE nc.nguoi_dung_id = @nguoiDungId 
                  AND c.trang_thai = 1
                ORDER BY c.ten";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungId", nguoiDungId)
            };

            return _dbHelper.GetDataTable(query, parameters);
        }

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