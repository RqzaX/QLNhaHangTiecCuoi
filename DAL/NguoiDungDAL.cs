using System;
using System.Collections.Generic;
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
                        SELECT TOP 1 vt.vai_tro_id
                        FROM dbo.nguoi_dung_vai_tro ndvt 
                        INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
                        WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY vt.vai_tro_id
                    ), 0) AS VaiTroId,
                    ISNULL((
                        SELECT TOP 1 vt.ten 
                        FROM dbo.nguoi_dung_vai_tro ndvt 
                        INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
                        WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY vt.vai_tro_id
                    ), N'Chưa phân quyền') AS ChucVu,
                    ISNULL((
                        SELECT TOP 1 cn.chi_nhanh_id
                        FROM dbo.nguoi_dung_chi_nhanh ndcn 
                        INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
                        WHERE ndcn.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY cn.chi_nhanh_id
                    ), 0) AS ChiNhanhId,
                    ISNULL((
                        SELECT TOP 1 cn.ten 
                        FROM dbo.nguoi_dung_chi_nhanh ndcn 
                        INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
                        WHERE ndcn.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY cn.chi_nhanh_id
                    ), N'Chưa phân chi nhánh') AS ChiNhanh
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
       
        /// </summary>
        public DataTable LayNhanVienChuaTrongCa(int caId, int chiNhanhId)
        {
            string query = @"
                WITH ChiNhanhNguoiDung AS (
                    SELECT nguoi_dung_id, MIN(chi_nhanh_id) AS chi_nhanh_id
                    FROM dbo.nguoi_dung_chi_nhanh
                    GROUP BY nguoi_dung_id
                ),
                SoCa AS (
                    SELECT nguoi_dung_id, COUNT(*) AS so_ca
                    FROM dbo.nguoi_dung_ca
                    WHERE trang_thai = 1
                    GROUP BY nguoi_dung_id
                ),
                DaCoTrongCa AS (
                    SELECT nguoi_dung_id
                    FROM dbo.nguoi_dung_ca
                    WHERE ca_id = @caId AND chi_nhanh_id = @chiNhanhId AND trang_thai = 1
                )
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
                LEFT JOIN ChiNhanhNguoiDung ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
                LEFT JOIN SoCa sc ON nd.nguoi_dung_id = sc.nguoi_dung_id
                LEFT JOIN DaCoTrongCa dctc ON nd.nguoi_dung_id = dctc.nguoi_dung_id
                WHERE nd.hoat_dong = 1
                  AND (ndcn.chi_nhanh_id = @chiNhanhId OR ndcn.chi_nhanh_id IS NULL)
                  AND dctc.nguoi_dung_id IS NULL
                  AND ISNULL(sc.so_ca, 0) < 3
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

        /// <summary>
        /// Lấy thông tin chi tiết của một nhân viên
        public DataTable LayThongTinNhanVien(int nguoiDungId)
        {
            string query = @"
                SELECT 
                    nd.nguoi_dung_id,
                    nd.tai_khoan,
                    nd.ho_ten,
                    nd.hoat_dong,
                    ISNULL((
                        SELECT TOP 1 vt.vai_tro_id
                        FROM dbo.nguoi_dung_vai_tro ndvt 
                        INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
                        WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY vt.vai_tro_id
                    ), 0) AS vai_tro_id,
                    ISNULL((
                        SELECT TOP 1 cn.chi_nhanh_id
                        FROM dbo.nguoi_dung_chi_nhanh ndcn 
                        INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
                        WHERE ndcn.nguoi_dung_id = nd.nguoi_dung_id
                        ORDER BY cn.chi_nhanh_id
                    ), 0) AS chi_nhanh_id
                FROM dbo.nguoi_dung nd
                WHERE nd.nguoi_dung_id = @nguoiDungId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungId", nguoiDungId)
            };

            return _dbHelper.GetDataTable(query, parameters);
        }

        /// <summary>
        /// Thêm nhân viên mới (overload 1: với tài khoản và mật khẩu)
        /// </summary>
        public int ThemNhanVien(string hoTen, string taiKhoan, string matKhau, int vaiTroId, int chiNhanhId, bool hoatDong)
        {
            string insertQuery = @"
                INSERT INTO dbo.nguoi_dung (tai_khoan, mat_khau, ho_ten, hoat_dong)
                OUTPUT INSERTED.nguoi_dung_id
                VALUES (@taiKhoan, @matKhau, @hoTen, @hoatDong)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@taiKhoan", taiKhoan),
                new SqlParameter("@matKhau", matKhau),
                new SqlParameter("@hoTen", hoTen),
                new SqlParameter("@hoatDong", hoatDong)
            };

            object result = _dbHelper.ExecuteScalar(insertQuery, parameters);
            int nguoiDungId = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

            if (nguoiDungId > 0)
            {
                // Gán vai trò
                if (vaiTroId > 0)
                {
                    string assignVaiTroQuery = @"
                        IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro 
                                      WHERE nguoi_dung_id = @nguoiDungId AND vai_tro_id = @vaiTroId)
                        INSERT INTO dbo.nguoi_dung_vai_tro (nguoi_dung_id, vai_tro_id)
                        VALUES (@nguoiDungId, @vaiTroId)";

                    SqlParameter[] vaiTroParams = new SqlParameter[]
                    {
                        new SqlParameter("@nguoiDungId", nguoiDungId),
                        new SqlParameter("@vaiTroId", vaiTroId)
                    };
                    _dbHelper.ExecuteNonQuery(assignVaiTroQuery, vaiTroParams);
                }

                // Gán chi nhánh
                if (chiNhanhId > 0)
                {
                    string assignChiNhanhQuery = @"
                        IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh 
                                      WHERE nguoi_dung_id = @nguoiDungId AND chi_nhanh_id = @chiNhanhId)
                        INSERT INTO dbo.nguoi_dung_chi_nhanh (nguoi_dung_id, chi_nhanh_id)
                        VALUES (@nguoiDungId, @chiNhanhId)";

                    SqlParameter[] chiNhanhParams = new SqlParameter[]
                    {
                        new SqlParameter("@nguoiDungId", nguoiDungId),
                        new SqlParameter("@chiNhanhId", chiNhanhId)
                    };
                    _dbHelper.ExecuteNonQuery(assignChiNhanhQuery, chiNhanhParams);
                }
            }

            return nguoiDungId;
        }

        /// <summary>
        /// Thêm nhân viên mới (overload 2: không có tài khoản/mật khẩu, tự tạo)
        /// </summary>
        public int ThemNhanVien(string hoTen, int vaiTroId, int chiNhanhId)
        {
            // Tạo tài khoản tự động từ họ tên
            string taiKhoan = GenerateTaiKhoan(hoTen);
            string matKhau = "123456"; // Mật khẩu mặc định

            return ThemNhanVien(hoTen, taiKhoan, matKhau, vaiTroId, chiNhanhId, true);
        }

        /// <summary>
        /// Tạo tài khoản tự động từ họ tên
        /// </summary>
        private string GenerateTaiKhoan(string hoTen)
        {
            // Loại bỏ dấu và chuyển thành chữ thường
            string taiKhoan = RemoveVietnameseAccents(hoTen.ToLower().Trim());
            // Thay thế khoảng trắng bằng dấu gạch dưới
            taiKhoan = taiKhoan.Replace(" ", "_");
            // Lấy 8 ký tự đầu
            taiKhoan = taiKhoan.Length > 8 ? taiKhoan.Substring(0, 8) : taiKhoan;
            
            // Kiểm tra trùng lặp và thêm số nếu cần
            int counter = 1;
            string originalTaiKhoan = taiKhoan;
            while (TaiKhoanExists(taiKhoan))
            {
                taiKhoan = originalTaiKhoan + counter.ToString();
                counter++;
            }

            return taiKhoan;
        }

        /// <summary>
        /// Kiểm tra tài khoản đã tồn tại chưa
        /// </summary>
        private bool TaiKhoanExists(string taiKhoan)
        {
            string query = "SELECT COUNT(*) FROM dbo.nguoi_dung WHERE tai_khoan = @taiKhoan";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@taiKhoan", taiKhoan)
            };
            object result = _dbHelper.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Loại bỏ dấu tiếng Việt
        /// </summary>
        private string RemoveVietnameseAccents(string text)
        {
            string[] vietnameseChars = { "à", "á", "ạ", "ả", "ã", "â", "ầ", "ấ", "ậ", "ẩ", "ẫ", "ă", "ằ", "ắ", "ặ", "ẳ", "ẵ",
                                         "è", "é", "ẹ", "ẻ", "ẽ", "ê", "ề", "ế", "ệ", "ể", "ễ",
                                         "ì", "í", "ị", "ỉ", "ĩ",
                                         "ò", "ó", "ọ", "ỏ", "õ", "ô", "ồ", "ố", "ộ", "ổ", "ỗ", "ơ", "ờ", "ớ", "ợ", "ở", "ỡ",
                                         "ù", "ú", "ụ", "ủ", "ũ", "ư", "ừ", "ứ", "ự", "ử", "ữ",
                                         "ỳ", "ý", "ỵ", "ỷ", "ỹ",
                                         "đ" };
            string[] replaceChars = { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                      "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e",
                                      "i", "i", "i", "i", "i",
                                      "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
                                      "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u",
                                      "y", "y", "y", "y", "y",
                                      "d" };

            for (int i = 0; i < vietnameseChars.Length; i++)
            {
                text = text.Replace(vietnameseChars[i], replaceChars[i]);
                text = text.Replace(vietnameseChars[i].ToUpper(), replaceChars[i].ToUpper());
            }
            return text;
        }

        /// <summary>
        /// Cập nhật nhân viên (overload 1: đầy đủ thông tin)
        /// </summary>
        public bool CapNhatNhanVien(int nguoiDungId, string hoTen, string taiKhoan, bool hoatDong, int vaiTroId, int chiNhanhId, string matKhau)
        {
            // Cập nhật thông tin cơ bản
            string updateQuery = @"
                UPDATE dbo.nguoi_dung
                SET ho_ten = @hoTen,
                    tai_khoan = @taiKhoan,
                    hoat_dong = @hoatDong";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@nguoiDungId", nguoiDungId),
                new SqlParameter("@hoTen", hoTen),
                new SqlParameter("@taiKhoan", taiKhoan),
                new SqlParameter("@hoatDong", hoatDong)
            };

            // Cập nhật mật khẩu nếu có
            if (!string.IsNullOrWhiteSpace(matKhau))
            {
                updateQuery += ", mat_khau = @matKhau";
                parameters.Add(new SqlParameter("@matKhau", matKhau));
            }

            updateQuery += " WHERE nguoi_dung_id = @nguoiDungId";

            _dbHelper.ExecuteNonQuery(updateQuery, parameters.ToArray());

            // Cập nhật vai trò
            if (vaiTroId > 0)
            {
                // Xóa vai trò cũ
                string deleteVaiTroQuery = "DELETE FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nguoiDungId";
                _dbHelper.ExecuteNonQuery(deleteVaiTroQuery, new SqlParameter[] { new SqlParameter("@nguoiDungId", nguoiDungId) });

                // Thêm vai trò mới
                string insertVaiTroQuery = @"
                    INSERT INTO dbo.nguoi_dung_vai_tro (nguoi_dung_id, vai_tro_id)
                    VALUES (@nguoiDungId, @vaiTroId)";
                _dbHelper.ExecuteNonQuery(insertVaiTroQuery, new SqlParameter[]
                {
                    new SqlParameter("@nguoiDungId", nguoiDungId),
                    new SqlParameter("@vaiTroId", vaiTroId)
                });
            }

            // Cập nhật chi nhánh
            if (chiNhanhId > 0)
            {
                // Xóa chi nhánh cũ
                string deleteChiNhanhQuery = "DELETE FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nguoiDungId";
                _dbHelper.ExecuteNonQuery(deleteChiNhanhQuery, new SqlParameter[] { new SqlParameter("@nguoiDungId", nguoiDungId) });

                // Thêm chi nhánh mới
                string insertChiNhanhQuery = @"
                    INSERT INTO dbo.nguoi_dung_chi_nhanh (nguoi_dung_id, chi_nhanh_id)
                    VALUES (@nguoiDungId, @chiNhanhId)";
                _dbHelper.ExecuteNonQuery(insertChiNhanhQuery, new SqlParameter[]
                {
                    new SqlParameter("@nguoiDungId", nguoiDungId),
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                });
            }

            return true;
        }

        /// <summary>
        /// Cập nhật nhân viên (overload 2: chỉ họ tên và vai trò)
        /// </summary>
        public bool CapNhatNhanVien(int nguoiDungId, string hoTen, int vaiTroId)
        {
            // Cập nhật họ tên
            string updateQuery = @"
                UPDATE dbo.nguoi_dung
                SET ho_ten = @hoTen
                WHERE nguoi_dung_id = @nguoiDungId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungId", nguoiDungId),
                new SqlParameter("@hoTen", hoTen)
            };

            _dbHelper.ExecuteNonQuery(updateQuery, parameters);

            // Cập nhật vai trò nếu có
            if (vaiTroId > 0)
            {
                // Xóa vai trò cũ
                string deleteVaiTroQuery = "DELETE FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nguoiDungId";
                _dbHelper.ExecuteNonQuery(deleteVaiTroQuery, new SqlParameter[] { new SqlParameter("@nguoiDungId", nguoiDungId) });

                // Thêm vai trò mới
                string insertVaiTroQuery = @"
                    INSERT INTO dbo.nguoi_dung_vai_tro (nguoi_dung_id, vai_tro_id)
                    VALUES (@nguoiDungId, @vaiTroId)";
                _dbHelper.ExecuteNonQuery(insertVaiTroQuery, new SqlParameter[]
                {
                    new SqlParameter("@nguoiDungId", nguoiDungId),
                    new SqlParameter("@vaiTroId", vaiTroId)
                });
            }

            return true;
        }

        /// <summary>
        /// Xóa (vô hiệu hóa) nhân viên
        /// </summary>
        public bool XoaNhanVien(int nguoiDungId)
        {
            string query = @"
                UPDATE dbo.nguoi_dung
                SET hoat_dong = 0
                WHERE nguoi_dung_id = @nguoiDungId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungId", nguoiDungId)
            };

            int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
            return rowsAffected > 0;
        }

        /// <summary>
        /// Gán chi nhánh cho người dùng
        /// </summary>
        public bool GanChiNhanhChoNguoiDung(int nguoiDungId, int chiNhanhId)
        {
            // Xóa chi nhánh cũ
            string deleteQuery = "DELETE FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nguoiDungId";
            _dbHelper.ExecuteNonQuery(deleteQuery, new SqlParameter[] { new SqlParameter("@nguoiDungId", nguoiDungId) });

            // Thêm chi nhánh mới
            string insertQuery = @"
                IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh 
                              WHERE nguoi_dung_id = @nguoiDungId AND chi_nhanh_id = @chiNhanhId)
                INSERT INTO dbo.nguoi_dung_chi_nhanh (nguoi_dung_id, chi_nhanh_id)
                VALUES (@nguoiDungId, @chiNhanhId)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nguoiDungId", nguoiDungId),
                new SqlParameter("@chiNhanhId", chiNhanhId)
            };

            int rowsAffected = _dbHelper.ExecuteNonQuery(insertQuery, parameters);
            return rowsAffected > 0;
        }
    }
}