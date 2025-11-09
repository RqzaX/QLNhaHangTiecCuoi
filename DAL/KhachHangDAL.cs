using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.DAL
{
    public class KhachHangDAL
    {
        private DatabaseHelper _dbHelper;

        public KhachHangDAL()
        {
            _dbHelper = new DatabaseHelper();
        }

        public DataTable TimKhachHangTheoSdt(string sdt)
        {
            try
            {
                string query = @"
                    SELECT khach_hang_id, ho_ten, sdt, email, ghi_chu
                    FROM khach_hang 
                    WHERE sdt = @sdt";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@sdt", sdt)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm khách hàng theo SĐT: {ex.Message}");
            }
        }

        public int TaoKhachHang(string hoTen, string sdt, string email, string ghiChu)
        {
            try
            {
                string query = @"
                    INSERT INTO khach_hang (ho_ten, sdt, email, ghi_chu)
                    VALUES (@hoTen, @sdt, @email, @ghiChu);
                    SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@hoTen", hoTen),
                    new SqlParameter("@sdt", sdt),
                    new SqlParameter("@email", email ?? ""),
                    new SqlParameter("@ghiChu", ghiChu ?? "")
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tạo khách hàng: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhachHang()
        {
            try
            {
                string query = "SELECT khach_hang_id, ho_ten as ten_khach_hang, sdt as so_dien_thoai FROM khach_hang ORDER BY ho_ten";
                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khách hàng: {ex.Message}");
            }
        }

        public DataTable LayThongTinKhachHang(int khachHangId)
        {
            try
            {
                string query = "SELECT khach_hang_id, ho_ten as ten_khach_hang, sdt as so_dien_thoai FROM khach_hang WHERE khach_hang_id = @khachHangId";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@khachHangId", khachHangId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin khách hàng: {ex.Message}");
            }
        }

        public bool CapNhatKhachHang(int khachHangId, string hoTen, string email, string ghiChu)
        {
            try
            {
                string query = @"
                    UPDATE khach_hang
                    SET ho_ten = @hoTen,
                        email = @email,
                        ghi_chu = @ghiChu
                    WHERE khach_hang_id = @khachHangId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@khachHangId", khachHangId),
                    new SqlParameter("@hoTen", hoTen),
                    new SqlParameter("@email", email ?? ""),
                    new SqlParameter("@ghiChu", ghiChu ?? "")
                };

                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật khách hàng: {ex.Message}");
            }
        }
    }
}
