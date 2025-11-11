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

        /// <summary>
        /// Cập nhật thông tin sảnh trong database
        /// </summary>
        public bool CapNhatSanh(int sanhId, string tenSanh, int sucChua, decimal phiThueCb)
        {
            try
            {
                string query = @"
                    UPDATE dbo.sanh
                    SET ten_sanh = @tenSanh,
                        suc_chua = @sucChua,
                        phi_thue_cb = @phiThueCb
                    WHERE sanh_id = @sanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@sanhId", sanhId),
                    new SqlParameter("@tenSanh", tenSanh),
                    new SqlParameter("@sucChua", sucChua),
                    new SqlParameter("@phiThueCb", phiThueCb)
                };

                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật sảnh: {ex.Message}");
            }
        }

        public DataTable LayDanhSachChiNhanh()
        {
            try
            {
                string query = @"
                    SELECT chi_nhanh_id, ten
                    FROM dbo.chi_nhanh
                    WHERE trang_thai = 1
                    ORDER BY ten";

                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách chi nhánh: {ex.Message}");
            }
        }

        /// <summary>
        /// Kiểm tra xem sảnh có đang được sử dụng trong dat_sanh không
        /// </summary>
        public int DemSoLuongDatSanh(int sanhId)
        {
            try
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM dbo.dat_sanh
                    WHERE sanh_id = @sanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@sanhId", sanhId)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi kiểm tra số lượng đặt sảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm sảnh mới vào database
        /// </summary>
        public int ThemSanh(int chiNhanhId, string tenSanh, int sucChua, decimal phiThueCb)
        {
            try
            {
                string query = @"
                    INSERT INTO dbo.sanh (chi_nhanh_id, ten_sanh, suc_chua, phi_thue_cb)
                    OUTPUT INSERTED.sanh_id
                    VALUES (@chiNhanhId, @tenSanh, @sucChua, @phiThueCb)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId),
                    new SqlParameter("@tenSanh", tenSanh),
                    new SqlParameter("@sucChua", sucChua),
                    new SqlParameter("@phiThueCb", phiThueCb)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm sảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa sảnh khỏi database
        /// </summary>
        public bool XoaSanh(int sanhId)
        {
            try
            {
                // Kiểm tra xem có đặt sảnh nào đang sử dụng sảnh này không
                int soLuongDatSanh = DemSoLuongDatSanh(sanhId);
                if (soLuongDatSanh > 0)
                {
                    throw new Exception($"Không thể xóa sảnh này vì đang có {soLuongDatSanh} đặt sảnh đang sử dụng. Vui lòng xóa hoặc hủy các đặt sảnh trước!");
                }

                // Xóa sảnh
                string query = @"DELETE FROM dbo.sanh WHERE sanh_id = @sanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@sanhId", sanhId)
                };

                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa sảnh: {ex.Message}");
            }
        }
    }
}

