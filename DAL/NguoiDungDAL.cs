using System;
using System.Data;
using Microsoft.Data.SqlClient;
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

        public DataTable LayVaiTroByNguoiDungId(int nguoiDungId)
        {
            string query = @"
                SELECT vt.ma, vt.ten
                FROM dbo.vai_tro vt
                INNER JOIN dbo.nguoi_dung_vai_tro ndvt ON vt.vai_tro_id = ndvt.vai_tro_id
                WHERE ndvt.nguoi_dung_id = @nguoiDungId
                ORDER BY vt.ma";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungId", nguoiDungId)
            };

            return _dbHelper.GetDataTable(query, parameters);
        }

        /// <summary>
        /// Lấy danh sách nhân viên với tên, chức vụ và chi nhánh
        /// </summary>
        public DataTable LayDanhSachNhanVien()
        {
            string query = @"
                SELECT 
                    nd.nguoi_dung_id AS NguoiDungId,
                    nd.tai_khoan AS TaiKhoan,
                    nd.ho_ten AS TenNV,
                    nd.mat_khau AS MatKhau,
                    nd.hoat_dong AS HoatDong,
                    ISNULL((
                        SELECT TOP 1 vt.ten 
                        FROM dbo.nguoi_dung_vai_tro ndvt 
                        INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
                        WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY vt.vai_tro_id
                    ), N'Chưa phân quyền') AS ChucVu,
                    ISNULL((
                        SELECT TOP 1 cn.ten 
                        FROM dbo.nguoi_dung_chi_nhanh ndcn 
                        INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
                        WHERE ndcn.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY cn.chi_nhanh_id
                    ), N'Chưa phân chi nhánh') AS ChiNhanh,
                    ISNULL((
                        SELECT TOP 1 vt.vai_tro_id 
                        FROM dbo.nguoi_dung_vai_tro ndvt 
                        INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
                        WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY vt.vai_tro_id
                    ), 0) AS VaiTroId,
                    ISNULL((
                        SELECT TOP 1 cn.chi_nhanh_id 
                        FROM dbo.nguoi_dung_chi_nhanh ndcn 
                        INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
                        WHERE ndcn.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY cn.chi_nhanh_id
                    ), 0) AS ChiNhanhId
                FROM dbo.nguoi_dung nd
                ORDER BY nd.ho_ten";

            return _dbHelper.GetDataTable(query);
        }

        /// <summary>
        /// Lấy danh sách phân ca cho nhân viên
        /// </summary>
        public DataTable LayDanhSachPhanCa()
        {
            string query = @"
                SELECT 
                    ndc.nguoi_dung_ca_id,
                    ndc.nguoi_dung_id,
                    ndc.chi_nhanh_id,
                    ndc.ca_id,
                    c.ten_ca,
                    c.gio_bd,
                    c.gio_kt,
                    nd.ho_ten,
                    nd.tai_khoan,
                    cn.ten AS ten_chi_nhanh,
                    ndc.trang_thai
                FROM dbo.nguoi_dung_ca ndc
                INNER JOIN dbo.nguoi_dung nd ON ndc.nguoi_dung_id = nd.nguoi_dung_id
                INNER JOIN dbo.chi_nhanh cn ON ndc.chi_nhanh_id = cn.chi_nhanh_id
                INNER JOIN dbo.ca c ON ndc.ca_id = c.ca_id
                WHERE ndc.trang_thai = 1
                ORDER BY c.gio_bd, nd.ho_ten";

            return _dbHelper.GetDataTable(query);
        }

        /// <summary>
        /// Lấy danh sách nhân viên trong ca (theo ca_id và chi_nhanh_id)
        /// </summary>
        public DataTable LayNhanVienTrongCa(int caId, int chiNhanhId)
        {
            string query = @"
                SELECT 
                    ndc.nguoi_dung_ca_id,
                    ndc.nguoi_dung_id,
                    nd.ho_ten,
                    nd.tai_khoan,
                    ISNULL((
                        SELECT TOP 1 vt.ten 
                        FROM dbo.nguoi_dung_vai_tro ndvt 
                        INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
                        WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY vt.vai_tro_id
                    ), N'Chưa phân quyền') AS chuc_vu
                FROM dbo.nguoi_dung_ca ndc
                INNER JOIN dbo.nguoi_dung nd ON ndc.nguoi_dung_id = nd.nguoi_dung_id
                WHERE ndc.ca_id = @caId 
                  AND ndc.chi_nhanh_id = @chiNhanhId
                  AND ndc.trang_thai = 1
                  AND nd.hoat_dong = 1
                ORDER BY nd.ho_ten";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@caId", caId),
                new SqlParameter("@chiNhanhId", chiNhanhId)
            };

            return _dbHelper.GetDataTable(query, parameters);
        }

        /// <summary>
        /// Lấy danh sách nhân viên chưa có trong ca (bao gồm: không hoạt động trong ca đó, không có trong ca đó, và chưa ở trong ca nào)
        /// </summary>
        public DataTable LayNhanVienChuaTrongCa(int caId, int chiNhanhId)
        {
            string query = @"
                SELECT 
                    nd.nguoi_dung_id,
                    nd.ho_ten,
                    nd.tai_khoan,
                    ISNULL((
                        SELECT TOP 1 vt.ten 
                        FROM dbo.nguoi_dung_vai_tro ndvt 
                        INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
                        WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY vt.vai_tro_id
                    ), N'Chưa phân quyền') AS chuc_vu
                FROM dbo.nguoi_dung nd
                INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
                LEFT JOIN dbo.nguoi_dung_ca ndc ON ndc.nguoi_dung_id = nd.nguoi_dung_id
                    AND ndc.ca_id = @caId
                    AND ndc.chi_nhanh_id = @chiNhanhId
                    AND ndc.trang_thai = 1
                WHERE ndcn.chi_nhanh_id = @chiNhanhId
                  AND nd.hoat_dong = 1
                  AND ndc.nguoi_dung_ca_id IS NULL
                ORDER BY nd.ho_ten";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@caId", caId),
                new SqlParameter("@chiNhanhId", chiNhanhId)
            };

            return _dbHelper.GetDataTable(query, parameters);
        }

        /// <summary>
     
        /// Cho phép 1 nhân viên có nhiều ca khác nhau trong cùng 1 chi nhánh
        /// </summary>
        public int ThemNhanVienVaoCa(int nguoiDungId, int chiNhanhId, int caId)
        {
            // Sử dụng MERGE để xử lý cả trường hợp INSERT và UPDATE trong một lệnh
            string mergeQuery = @"
                MERGE dbo.nguoi_dung_ca AS target
                USING (SELECT @nguoiDungId AS nguoi_dung_id, @chiNhanhId AS chi_nhanh_id, @caId AS ca_id) AS source
                ON target.nguoi_dung_id = source.nguoi_dung_id
                   AND target.chi_nhanh_id = source.chi_nhanh_id
                   AND target.ca_id = source.ca_id
                WHEN MATCHED THEN
                    UPDATE SET 
                        trang_thai = 1
                WHEN NOT MATCHED THEN
                    INSERT (nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
                    VALUES (@nguoiDungId, @chiNhanhId, @caId, 1)
                OUTPUT INSERTED.nguoi_dung_ca_id;";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungId", nguoiDungId),
                new SqlParameter("@chiNhanhId", chiNhanhId),
                new SqlParameter("@caId", caId)
            };

            object result = _dbHelper.ExecuteScalar(mergeQuery, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        /// <summary>
        /// Xóa nhân viên khỏi ca (DELETE record khỏi nguoi_dung_ca)
        /// </summary>
        public bool XoaNhanVienKhoiCa(int nguoiDungCaId)
        {
            string query = @"
                DELETE FROM dbo.nguoi_dung_ca
                WHERE nguoi_dung_ca_id = @nguoiDungCaId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungCaId", nguoiDungCaId)
            };

            int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        // Lấy danh sách chức vụ (vai trò)
        public DataTable LayDanhSachChucVu()
        {
            string query = @"
                SELECT 
                    vai_tro_id,
                    ten AS ten_chuc_vu,
                    ma AS ma_chuc_vu
                FROM dbo.vai_tro
                ORDER BY ten";

            return _dbHelper.GetDataTable(query);
        }

        public int ThemNhanVien(string hoTen, string taiKhoan, string matKhau, int vaiTroId, int chiNhanhId, bool hoatDong)
        {
            using (var conn = new SqlConnection(_dbHelper.ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra tài khoản đã tồn tại chưa
                        string checkQuery = "SELECT COUNT(*) FROM dbo.nguoi_dung WHERE tai_khoan = @TaiKhoan";
                        var cmdCheck = new SqlCommand(checkQuery, conn, tran);
                        cmdCheck.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        int count = (int)cmdCheck.ExecuteScalar();
                        
                        if (count > 0)
                        {
                            throw new Exception($"Tài khoản '{taiKhoan}' đã tồn tại trong hệ thống. Vui lòng chọn tài khoản khác.");
                        }

                        string insertQuery = @"
                            INSERT INTO dbo.nguoi_dung (tai_khoan, mat_khau, ho_ten, hoat_dong)
                            VALUES (@TaiKhoan, @MatKhau, @HoTen, @HoatDong);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        var cmdInsert = new SqlCommand(insertQuery, conn, tran);
                        cmdInsert.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        cmdInsert.Parameters.AddWithValue("@MatKhau", matKhau);
                        cmdInsert.Parameters.AddWithValue("@HoTen", hoTen);
                        cmdInsert.Parameters.AddWithValue("@HoatDong", hoatDong);

                        int newId = (int)cmdInsert.ExecuteScalar();

                        if (vaiTroId > 0)
                        {
                            string insertRole = @"
                                INSERT INTO dbo.nguoi_dung_vai_tro (nguoi_dung_id, vai_tro_id)
                                VALUES (@NguoiDungId, @VaiTroId)";
                            var cmdRole = new SqlCommand(insertRole, conn, tran);
                            cmdRole.Parameters.AddWithValue("@NguoiDungId", newId);
                            cmdRole.Parameters.AddWithValue("@VaiTroId", vaiTroId);
                            cmdRole.ExecuteNonQuery();
                        }

                        if (chiNhanhId > 0)
                        {
                            string insertBranch = @"
                                INSERT INTO dbo.nguoi_dung_chi_nhanh (nguoi_dung_id, chi_nhanh_id)
                                VALUES (@NguoiDungId, @ChiNhanhId)";
                            var cmdBranch = new SqlCommand(insertBranch, conn, tran);
                            cmdBranch.Parameters.AddWithValue("@NguoiDungId", newId);
                            cmdBranch.Parameters.AddWithValue("@ChiNhanhId", chiNhanhId);
                            cmdBranch.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return newId;
                    }
                    catch (SqlException sqlEx)
                    {
                        tran.Rollback();
                        // Xử lý lỗi UNIQUE KEY constraint
                        if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                        {
                            throw new Exception($"Tài khoản '{taiKhoan}' đã tồn tại trong hệ thống. Vui lòng chọn tài khoản khác.");
                        }
                        throw new Exception("Lỗi thêm nhân viên: " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("Lỗi thêm nhân viên: " + ex.Message);
                    }
                }
            }
        }

        public bool CapNhatNhanVien(int nguoiDungId, string hoTen, string taiKhoan, bool hoatDong, int vaiTroId, int chiNhanhId, string matKhau = null)
        {
            using (var conn = new SqlConnection(_dbHelper.ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra tài khoản đã tồn tại chưa (trừ chính nhân viên đang cập nhật)
                        string checkQuery = "SELECT COUNT(*) FROM dbo.nguoi_dung WHERE tai_khoan = @TaiKhoan AND nguoi_dung_id != @NguoiDungId";
                        var cmdCheck = new SqlCommand(checkQuery, conn, tran);
                        cmdCheck.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        cmdCheck.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                        int count = (int)cmdCheck.ExecuteScalar();
                        
                        if (count > 0)
                        {
                            throw new Exception($"Tài khoản '{taiKhoan}' đã được sử dụng bởi nhân viên khác. Vui lòng chọn tài khoản khác.");
                        }

                        string updateQuery = @"
                            UPDATE dbo.nguoi_dung
                            SET ho_ten = @HoTen,
                                tai_khoan = @TaiKhoan,
                                hoat_dong = @HoatDong{0}
                            WHERE nguoi_dung_id = @NguoiDungId";

                        string passwordClause = string.IsNullOrWhiteSpace(matKhau) ? "" : ", mat_khau = @MatKhau";
                        updateQuery = string.Format(updateQuery, passwordClause);

                        var cmdUpdate = new SqlCommand(updateQuery, conn, tran);
                        cmdUpdate.Parameters.AddWithValue("@HoTen", hoTen);
                        cmdUpdate.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        cmdUpdate.Parameters.AddWithValue("@HoatDong", hoatDong);
                        cmdUpdate.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                        if (!string.IsNullOrWhiteSpace(matKhau))
                        {
                            cmdUpdate.Parameters.AddWithValue("@MatKhau", matKhau);
                        }
                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                        
                        if (rowsAffected == 0)
                        {
                            throw new Exception("Không tìm thấy nhân viên cần cập nhật.");
                        }

                        string deleteRole = "DELETE FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @NguoiDungId";
                        var cmdDeleteRole = new SqlCommand(deleteRole, conn, tran);
                        cmdDeleteRole.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                        cmdDeleteRole.ExecuteNonQuery();

                        if (vaiTroId > 0)
                        {
                            string insertRole = @"
                                INSERT INTO dbo.nguoi_dung_vai_tro (nguoi_dung_id, vai_tro_id)
                                VALUES (@NguoiDungId, @VaiTroId)";
                            var cmdInsertRole = new SqlCommand(insertRole, conn, tran);
                            cmdInsertRole.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                            cmdInsertRole.Parameters.AddWithValue("@VaiTroId", vaiTroId);
                            cmdInsertRole.ExecuteNonQuery();
                        }

                        string deleteBranch = "DELETE FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @NguoiDungId";
                        var cmdDeleteBranch = new SqlCommand(deleteBranch, conn, tran);
                        cmdDeleteBranch.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                        cmdDeleteBranch.ExecuteNonQuery();

                        if (chiNhanhId > 0)
                        {
                            string insertBranch = @"
                                INSERT INTO dbo.nguoi_dung_chi_nhanh (nguoi_dung_id, chi_nhanh_id)
                                VALUES (@NguoiDungId, @ChiNhanhId)";
                            var cmdInsertBranch = new SqlCommand(insertBranch, conn, tran);
                            cmdInsertBranch.Parameters.AddWithValue("@NguoiDungId", nguoiDungId);
                            cmdInsertBranch.Parameters.AddWithValue("@ChiNhanhId", chiNhanhId);
                            cmdInsertBranch.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch (SqlException sqlEx)
                    {
                        tran.Rollback();
                        // Xử lý lỗi UNIQUE KEY constraint
                        if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
                        {
                            throw new Exception($"Tài khoản '{taiKhoan}' đã được sử dụng bởi nhân viên khác. Vui lòng chọn tài khoản khác.");
                        }
                        throw new Exception("Lỗi cập nhật nhân viên: " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("Lỗi cập nhật nhân viên: " + ex.Message);
                    }
                }
            }
        }

        public bool XoaNhanVien(int nguoiDungId)
        {
            try
            {
                // Kiểm tra nhân viên có tồn tại không
                string checkQuery = "SELECT COUNT(*) FROM dbo.nguoi_dung WHERE nguoi_dung_id = @NguoiDungId";
                SqlParameter[] checkParams = new SqlParameter[]
                {
                    new SqlParameter("@NguoiDungId", nguoiDungId)
                };
                int count = Convert.ToInt32(_dbHelper.ExecuteScalar(checkQuery, checkParams));
                
                if (count == 0)
                {
                    throw new Exception("Không tìm thấy nhân viên cần xóa.");
                }

                string query = @"
                    UPDATE dbo.nguoi_dung
                    SET hoat_dong = 0
                    WHERE nguoi_dung_id = @NguoiDungId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@NguoiDungId", nguoiDungId)
                };

                int rows = _dbHelper.ExecuteNonQuery(query, parameters);
                
                if (rows == 0)
                {
                    throw new Exception("Không thể xóa nhân viên. Vui lòng thử lại.");
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xóa nhân viên: " + ex.Message);
            }
        }
    }
}