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

        public int TaoKhachHang(string hoTen, string sdt, string email, string ghiChu,
            DateTime? ngaySinh = null, string hangCode = "MEM", decimal tongChiTieu = 0,
            int soLanDen = 0, int diem = 0)
        {
            try
            {
                string query = @"
                    INSERT INTO khach_hang (ho_ten, sdt, email, ghi_chu, ngay_sinh, hang_code, tong_chi_tieu, so_lan_den, diem)
                    VALUES (@hoTen, @sdt, @email, @ghiChu, @ngaySinh, @hangCode, @tongChiTieu, @soLanDen, @diem);
                    SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@hoTen", hoTen),
                    new SqlParameter("@sdt", sdt ?? (object)DBNull.Value),
                    new SqlParameter("@email", email ?? (object)DBNull.Value),
                    new SqlParameter("@ghiChu", ghiChu ?? (object)DBNull.Value),
                    new SqlParameter("@ngaySinh", ngaySinh.HasValue ? (object)ngaySinh.Value : DBNull.Value),
                    new SqlParameter("@hangCode", hangCode),
                    new SqlParameter("@tongChiTieu", tongChiTieu),
                    new SqlParameter("@soLanDen", soLanDen),
                    new SqlParameter("@diem", diem)
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

        public DataTable LayDanhSachKhachHangChiTiet(string keyword = null, string hangCode = null)
        {
            try
            {
                string query = @"
                    SELECT 
                        kh.khach_hang_id,
                        kh.ho_ten,
                        kh.ngay_sinh,
                        kh.sdt,
                        kh.email,
                        kh.hang_code,
                        ISNULL(dm.ten_hang, N'Thành viên') as ten_hang,
                        kh.tong_chi_tieu,
                        kh.so_lan_den,
                        kh.diem,
                        kh.lan_cuoi_den,
                        kh.ghi_chu,
                        -- Tính số tiền còn lại để lên hạng tiếp theo
                        CASE 
                            WHEN next_hang.min_tich_luy IS NOT NULL 
                            THEN next_hang.min_tich_luy - kh.tong_chi_tieu
                            ELSE NULL
                        END as con_lai_len_hang
                    FROM dbo.khach_hang kh
                    LEFT JOIN dbo.dm_hang_kh dm ON kh.hang_code = dm.hang_code
                    LEFT JOIN dbo.dm_hang_kh next_hang ON next_hang.thu_tu = dm.thu_tu + 1
                    WHERE 1=1";

                List<SqlParameter> parameters = new List<SqlParameter>();

                // Tìm kiếm theo keyword
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query += @" AND (kh.ho_ten LIKE @keyword 
                                OR kh.sdt LIKE @keyword 
                                OR kh.email LIKE @keyword)";
                    parameters.Add(new SqlParameter("@keyword", $"%{keyword}%"));
                }

                // Lọc theo hạng
                if (!string.IsNullOrWhiteSpace(hangCode) && hangCode != "ALL")
                {
                    query += " AND kh.hang_code = @hangCode";
                    parameters.Add(new SqlParameter("@hangCode", hangCode));
                }

                query += " ORDER BY kh.ho_ten";

                return _dbHelper.GetDataTable(query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách khách hàng chi tiết: {ex.Message}");
            }
        }

        public int DemTongSoKhachHang()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM dbo.khach_hang";
                object result = _dbHelper.ExecuteScalar(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đếm tổng số khách hàng: {ex.Message}");
            }
        }

        public int DemKhachHangTheoHang(string hangCode)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM dbo.khach_hang WHERE hang_code = @hangCode";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@hangCode", hangCode)
                };
                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đếm khách hàng theo hạng: {ex.Message}");
            }
        }

        public DataTable LayDanhSachHang()
        {
            try
            {
                string query = @"
                    SELECT hang_code, ten_hang, min_tich_luy, thu_tu
                    FROM dbo.dm_hang_kh
                    ORDER BY thu_tu";
                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách hạng: {ex.Message}");
            }
        }

        public DataTable LayThongTinKhachHang(int khachHangId)
        {
            try
            {
                string query = @"
                    SELECT 
                        kh.khach_hang_id,
                        kh.ho_ten,
                        kh.ngay_sinh,
                        kh.sdt,
                        kh.email,
                        kh.hang_code,
                        ISNULL(dm.ten_hang, N'Thành viên') as ten_hang,
                        kh.tong_chi_tieu,
                        kh.so_lan_den,
                        kh.diem,
                        kh.lan_cuoi_den,
                        kh.ghi_chu
                    FROM dbo.khach_hang kh
                    LEFT JOIN dbo.dm_hang_kh dm ON kh.hang_code = dm.hang_code
                    WHERE kh.khach_hang_id = @khachHangId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@khachHangId", khachHangId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin khách hàng: {ex.Message}");
            }
        }

        public bool CapNhatKhachHang(int khachHangId, string hoTen, string sdt, string email, string ghiChu,
            DateTime? ngaySinh = null, string hangCode = "MEM", decimal tongChiTieu = 0,
            int soLanDen = 0, int diem = 0)
        {
            try
            {
                string query = @"
                    UPDATE khach_hang 
                    SET ho_ten = @hoTen,
                        sdt = @sdt,
                        email = @email,
                        ghi_chu = @ghiChu,
                        ngay_sinh = @ngaySinh,
                        hang_code = @hangCode,
                        tong_chi_tieu = @tongChiTieu,
                        so_lan_den = @soLanDen,
                        diem = @diem
                    WHERE khach_hang_id = @khachHangId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@khachHangId", khachHangId),
                    new SqlParameter("@hoTen", hoTen),
                    new SqlParameter("@sdt", sdt ?? (object)DBNull.Value),
                    new SqlParameter("@email", email ?? (object)DBNull.Value),
                    new SqlParameter("@ghiChu", ghiChu ?? (object)DBNull.Value),
                    new SqlParameter("@ngaySinh", ngaySinh.HasValue ? (object)ngaySinh.Value : DBNull.Value),
                    new SqlParameter("@hangCode", hangCode),
                    new SqlParameter("@tongChiTieu", tongChiTieu),
                    new SqlParameter("@soLanDen", soLanDen),
                    new SqlParameter("@diem", diem)
                };

                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật khách hàng: {ex.Message}");
            }
        }

        public bool XoaKhachHang(int khachHangId)
        {
            try
            {
                string query = "DELETE FROM khach_hang WHERE khach_hang_id = @khachHangId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@khachHangId", khachHangId)
                };

                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa khách hàng: {ex.Message}");
            }
        }
    }
}
