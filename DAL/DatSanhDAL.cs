using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.DAL
{
    public class DatSanhDAL
    {
        private readonly DatabaseHelper _dbHelper;

        public DatSanhDAL()
        {
            _dbHelper = new DatabaseHelper();
        }

        // Kiểm tra sảnh có còn trống trong thời gian này không
        public bool KiemTraSanhTrong(int sanhId, TimeSpan gioToChuc, DateTime ngayToChuc, int? excludeDatSanhId = null)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM dbo.dat_sanh 
                WHERE sanh_id = @sanhId 
                  AND gio_to_chuc = @gioToChuc
                  AND ngay_to_chuc = @ngayToChuc
                  AND trang_thai NOT IN (N'ĐÃ HỦY', N'HOÀN TẤT')";

            if (excludeDatSanhId.HasValue)
            {
                query += " AND dat_sanh_id != @excludeDatSanhId";
            }

            SqlParameter[] parameters = {
                new SqlParameter("@sanhId", sanhId),
                new SqlParameter("@gioToChuc", gioToChuc),
                new SqlParameter("@ngayToChuc", ngayToChuc.Date)
            };

            if (excludeDatSanhId.HasValue)
            {
                var paramList = parameters.ToList();
                paramList.Add(new SqlParameter("@excludeDatSanhId", excludeDatSanhId.Value));
                parameters = paramList.ToArray();
            }

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result) == 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi kiểm tra trạng thái sảnh: {ex.Message}", ex);
            }
        }

        // Lấy danh sách sảnh theo chi nhánh
        public DataTable LayDanhSachSanh(int chiNhanhId)
        {
            string query = @"
                SELECT 
                    sanh_id,
                    ten_sanh,
                    suc_chua,
                    phi_thue_cb
                FROM dbo.sanh
                WHERE chi_nhanh_id = @chiNhanhId
                ORDER BY ten_sanh";

            SqlParameter[] parameters = {
                new SqlParameter("@chiNhanhId", chiNhanhId)
            };

            try
            {
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách sảnh: {ex.Message}", ex);
            }
        }

        // Lấy thông tin sảnh
        public DataRow LayThongTinSanh(int sanhId)
        {
            string query = @"
                SELECT 
                    sanh_id,
                    ten_sanh,
                    suc_chua,
                    phi_thue_cb
                FROM dbo.sanh
                WHERE sanh_id = @sanhId";

            SqlParameter[] parameters = {
                new SqlParameter("@sanhId", sanhId)
            };

            try
            {
                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin sảnh: {ex.Message}", ex);
            }
        }

        // Lấy danh sách ca
        public DataTable LayDanhSachCa()
        {
            string query = @"
                SELECT 
                    ca_id,
                    ten_ca,
                    gio_bd,
                    gio_kt
                FROM dbo.ca
                ORDER BY gio_bd";

            try
            {
                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách ca: {ex.Message}", ex);
            }
        }

        // Lấy danh sách chi nhánh
        public DataTable LayDanhSachChiNhanh()
        {
            string query = @"
                SELECT 
                    chi_nhanh_id,
                    ten
                FROM dbo.chi_nhanh
                WHERE trang_thai = 1
                ORDER BY chi_nhanh_id";

            try
            {
                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách chi nhánh: {ex.Message}", ex);
            }
        }

        // Tạo đơn đặt sảnh
        public int TaoDatSanh(int chiNhanhId, int sanhId, int caId, DateTime ngayToChuc,
            int khachHangId, int? soBanDuKien, int? goiId, string ghiChu, TimeSpan? gioToChuc, string trangThai)
        {
            string query = @"
                INSERT INTO dbo.dat_sanh (
                    chi_nhanh_id, sanh_id, ca_id, ngay_to_chuc,
                    khach_hang_id, so_ban_du_kien, goi_id, trang_thai, ghi_chu, gio_to_chuc
                )
                VALUES (
                    @chiNhanhId, @sanhId, @caId, @ngayToChuc,
                    @khachHangId, @soBanDuKien, @goiId, @trangThai, @ghiChu, @gioToChuc
                );
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@chiNhanhId", chiNhanhId),
                new SqlParameter("@sanhId", sanhId),
                new SqlParameter("@caId", caId),
                new SqlParameter("@ngayToChuc", ngayToChuc.Date),
                new SqlParameter("@khachHangId", khachHangId),
                new SqlParameter("@soBanDuKien", (object)soBanDuKien ?? DBNull.Value),
                new SqlParameter("@goiId", (object)goiId ?? DBNull.Value),
                new SqlParameter("@trangThai", trangThai ?? "CHỜ XÁC NHẬN"),
                new SqlParameter("@ghiChu", ghiChu ?? ""),
                new SqlParameter("@gioToChuc", (object)gioToChuc ?? DBNull.Value)
            };

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tạo đơn đặt sảnh: {ex.Message}", ex);
            }
        }

        // Tạo hợp đồng
        public int TaoHopDong(string soHopDong, int datSanhId, DateTime ngayKy, decimal tongDuKien, string dieuKhoan)
        {
            string query = @"
                INSERT INTO dbo.hop_dong (so_hop_dong, dat_sanh_id, ngay_ky, tong_du_kien, dieu_khoan)
                VALUES (@soHopDong, @datSanhId, @ngayKy, @tongDuKien, @dieuKhoan);
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@soHopDong", soHopDong),
                new SqlParameter("@datSanhId", datSanhId),
                new SqlParameter("@ngayKy", ngayKy.Date),
                new SqlParameter("@tongDuKien", tongDuKien),
                new SqlParameter("@dieuKhoan", dieuKhoan ?? "")
            };

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tạo hợp đồng: {ex.Message}", ex);
            }
        }

        // Lưu tiền cọc
        public int LuuTienCoc(int hopDongId, decimal soTien, DateTime ngayNop, string hinhThuc, string ghiChu)
        {
            string query = @"
                INSERT INTO dbo.hop_dong_coc (hop_dong_id, so_tien, ngay_nop, hinh_thuc, ghi_chu)
                VALUES (@hopDongId, @soTien, @ngayNop, @hinhThuc, @ghiChu);
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId),
                new SqlParameter("@soTien", soTien),
                new SqlParameter("@ngayNop", ngayNop),
                new SqlParameter("@hinhThuc", hinhThuc ?? ""),
                new SqlParameter("@ghiChu", ghiChu ?? "")
            };

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu tiền cọc: {ex.Message}", ex);
            }
        }

        // Lưu thanh toán
        public int LuuThanhToan(int hopDongId, decimal soTien, DateTime ngayTT, string hinhThuc, string noiDung)
        {
            string query = @"
                INSERT INTO dbo.hop_dong_tt (hop_dong_id, so_tien, ngay_tt, hinh_thuc, noi_dung)
                VALUES (@hopDongId, @soTien, @ngayTT, @hinhThuc, @noiDung);
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId),
                new SqlParameter("@soTien", soTien),
                new SqlParameter("@ngayTT", ngayTT),
                new SqlParameter("@hinhThuc", hinhThuc ?? ""),
                new SqlParameter("@noiDung", noiDung ?? "")
            };

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu thanh toán: {ex.Message}", ex);
            }
        }

        // Lưu chi tiết món ăn vào hợp đồng
        public bool LuuChiTietMon(int hopDongId, int monId, decimal soLuong, decimal donGia)
        {
            string query = @"
                INSERT INTO dbo.hop_dong_ct_mon (hop_dong_id, mon_id, so_luong, don_gia)
                VALUES (@hopDongId, @monId, @soLuong, @donGia);";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId),
                new SqlParameter("@monId", monId),
                new SqlParameter("@soLuong", soLuong),
                new SqlParameter("@donGia", donGia)
            };

            try
            {
                _dbHelper.ExecuteNonQuery(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu chi tiết món: {ex.Message}", ex);
            }
        }

        // Xóa cọc
        public bool XoaCoc(int cocId)
        {
            string query = @"
                DELETE FROM dbo.hop_dong_coc
                WHERE coc_id = @cocId;";

            SqlParameter[] parameters = {
                new SqlParameter("@cocId", cocId)
            };

            try
            {
                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa cọc: {ex.Message}", ex);
            }
        }

        // Cập nhật cọc
        public bool CapNhatCoc(int cocId, decimal soTien, DateTime ngayNop, string hinhThuc, string ghiChu)
        {
            string query = @"
                UPDATE dbo.hop_dong_coc
                SET so_tien = @soTien,
                    ngay_nop = @ngayNop,
                    hinh_thuc = @hinhThuc,
                    ghi_chu = @ghiChu
                WHERE coc_id = @cocId;";

            SqlParameter[] parameters = {
                new SqlParameter("@cocId", cocId),
                new SqlParameter("@soTien", soTien),
                new SqlParameter("@ngayNop", ngayNop),
                new SqlParameter("@hinhThuc", hinhThuc ?? ""),
                new SqlParameter("@ghiChu", ghiChu ?? "")
            };

            try
            {
                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật cọc: {ex.Message}", ex);
            }
        }

        // Lấy thông tin cọc theo ID
        public DataRow? LayThongTinCoc(int cocId)
        {
            string query = @"
                SELECT coc_id, hop_dong_id, so_tien, ngay_nop, hinh_thuc, ghi_chu
                FROM dbo.hop_dong_coc
                WHERE coc_id = @cocId;";

            SqlParameter[] parameters = {
                new SqlParameter("@cocId", cocId)
            };

            try
            {
                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin cọc: {ex.Message}", ex);
            }
        }

        // Xóa thanh toán
        public bool XoaThanhToan(int ttId)
        {
            string query = @"
                DELETE FROM dbo.hop_dong_tt
                WHERE tt_id = @ttId;";

            SqlParameter[] parameters = {
                new SqlParameter("@ttId", ttId)
            };

            try
            {
                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa thanh toán: {ex.Message}", ex);
            }
        }

        // Cập nhật thanh toán
        public bool CapNhatThanhToan(int ttId, decimal soTien, DateTime ngayTT, string hinhThuc, string noiDung)
        {
            string query = @"
                UPDATE dbo.hop_dong_tt
                SET so_tien = @soTien,
                    ngay_tt = @ngayTT,
                    hinh_thuc = @hinhThuc,
                    noi_dung = @noiDung
                WHERE tt_id = @ttId;";

            SqlParameter[] parameters = {
                new SqlParameter("@ttId", ttId),
                new SqlParameter("@soTien", soTien),
                new SqlParameter("@ngayTT", ngayTT),
                new SqlParameter("@hinhThuc", hinhThuc ?? ""),
                new SqlParameter("@noiDung", noiDung ?? "")
            };

            try
            {
                int rowsAffected = _dbHelper.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật thanh toán: {ex.Message}", ex);
            }
        }

        // Lấy thông tin thanh toán theo ID
        public DataRow? LayThongTinThanhToan(int ttId)
        {
            string query = @"
                SELECT tt_id, hop_dong_id, so_tien, ngay_tt, hinh_thuc, noi_dung
                FROM dbo.hop_dong_tt
                WHERE tt_id = @ttId;";

            SqlParameter[] parameters = {
                new SqlParameter("@ttId", ttId)
            };

            try
            {
                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin thanh toán: {ex.Message}", ex);
            }
        }

        // Lưu chi tiết dịch vụ vào hợp đồng
        public bool LuuChiTietDichVu(int hopDongId, int dvId, decimal soLuong, decimal donGia)
        {
            string query = @"
                INSERT INTO dbo.hop_dong_ct_dv (hop_dong_id, dv_id, so_luong, don_gia)
                VALUES (@hopDongId, @dvId, @soLuong, @donGia);";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId),
                new SqlParameter("@dvId", dvId),
                new SqlParameter("@soLuong", soLuong),
                new SqlParameter("@donGia", donGia)
            };

            try
            {
                _dbHelper.ExecuteNonQuery(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu chi tiết dịch vụ: {ex.Message}", ex);
            }
        }

        // Cập nhật trạng thái đơn đặt sảnh
        public bool CapNhatTrangThaiDatSanh(int datSanhId, string trangThai)
        {
            string query = @"
                UPDATE dbo.dat_sanh
                SET trang_thai = @trangThai
                WHERE dat_sanh_id = @datSanhId;";

            SqlParameter[] parameters = {
                new SqlParameter("@datSanhId", datSanhId),
                new SqlParameter("@trangThai", trangThai)
            };

            try
            {
                _dbHelper.ExecuteNonQuery(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật trạng thái: {ex.Message}", ex);
            }
        }

        // Hủy đặt sảnh - cập nhật trạng thái và ghi chú vào các bảng liên quan
        public bool HuyDatSanh(int datSanhId, DateTime ngayHuy, string lyDoHuy, decimal soTienHoanCoc)
        {
            try
            {
                // Lấy ghi chú hiện tại
                string queryGetGhiChu = @"
                    SELECT ISNULL(ghi_chu, N'') AS ghi_chu
                    FROM dbo.dat_sanh
                    WHERE dat_sanh_id = @datSanhId";

                SqlParameter[] paramsGet = {
                    new SqlParameter("@datSanhId", datSanhId)
                };

                string ghiChuHienTai = "";
                DataTable dt = _dbHelper.GetDataTable(queryGetGhiChu, paramsGet);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["ghi_chu"] != DBNull.Value)
                {
                    ghiChuHienTai = dt.Rows[0]["ghi_chu"].ToString() ?? "";
                }

                string ghiChuMoi = "";
                if (!string.IsNullOrWhiteSpace(ghiChuHienTai))
                {
                    ghiChuMoi = ghiChuHienTai + "\n";
                }

                ghiChuMoi += $"Ngày hủy: {ngayHuy:dd/MM/yyyy} | Lý do hủy: {lyDoHuy}";
                if (soTienHoanCoc > 0)
                {
                    ghiChuMoi += $" | Số tiền hoàn cọc: {soTienHoanCoc:N0} ₫";
                }

                using (var connection = new SqlConnection(_dbHelper.ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string queryDatSanh = @"
                                UPDATE dbo.dat_sanh
                                SET trang_thai = N'ĐÃ HỦY',
                                    ghi_chu = @ghiChu
                                WHERE dat_sanh_id = @datSanhId;";

                            SqlParameter[] paramsDatSanh = {
                                new SqlParameter("@datSanhId", datSanhId),
                                new SqlParameter("@ghiChu", ghiChuMoi)
                            };

                            using (var cmd = new SqlCommand(queryDatSanh, connection, transaction))
                            {
                                cmd.Parameters.AddRange(paramsDatSanh);
                                cmd.ExecuteNonQuery();
                            }

                            string queryHopDongId = @"
                                SELECT hop_dong_id
                                FROM dbo.hop_dong
                                WHERE dat_sanh_id = @datSanhId;";

                            int? hopDongId = null;
                            using (var cmd = new SqlCommand(queryHopDongId, connection, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@datSanhId", datSanhId));
                                object result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    hopDongId = Convert.ToInt32(result);
                                }
                            }

                            if (hopDongId.HasValue)
                            {
                                string queryCoc = @"
                                    UPDATE dbo.hop_dong_coc
                                    SET ghi_chu = @ghiChu
                                    WHERE hop_dong_id = @hopDongId;";

                                SqlParameter[] paramsCoc = {
                                    new SqlParameter("@hopDongId", hopDongId.Value),
                                    new SqlParameter("@ghiChu", ghiChuMoi)
                                };

                                using (var cmd = new SqlCommand(queryCoc, connection, transaction))
                                {
                                    cmd.Parameters.AddRange(paramsCoc);
                                    cmd.ExecuteNonQuery();
                                }

                                string queryTT = @"
                                    UPDATE dbo.hop_dong_tt
                                    SET noi_dung = @ghiChu
                                    WHERE hop_dong_id = @hopDongId;";

                                SqlParameter[] paramsTT = {
                                    new SqlParameter("@hopDongId", hopDongId.Value),
                                    new SqlParameter("@ghiChu", ghiChuMoi)
                                };

                                using (var cmd = new SqlCommand(queryTT, connection, transaction))
                                {
                                    cmd.Parameters.AddRange(paramsTT);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi hủy đặt sảnh: {ex.Message}", ex);
            }
        }

        // Lấy thông tin đơn đặt sảnh
        public DataRow LayThongTinDatSanh(int datSanhId)
        {
            string query = @"
                SELECT 
                    ds.dat_sanh_id,
                    ds.chi_nhanh_id,
                    cn.ten AS ten_chi_nhanh,
                    ds.sanh_id,
                    s.ten_sanh,
                    s.suc_chua,
                    s.phi_thue_cb,
                    ds.ca_id,
                    c.ten_ca,
                    c.gio_bd,
                    c.gio_kt,
                    ds.gio_to_chuc,
                    ds.ngay_to_chuc,
                    ds.khach_hang_id,
                    kh.ho_ten AS ten_khach_hang,
                    kh.sdt,
                    ds.so_ban_du_kien,
                    ds.goi_id,
                    g.ten_goi,
                    g.gia_co_ban,
                    ds.trang_thai,
                    ds.ghi_chu,
                    hd.so_hop_dong,
                    hd.ngay_ky,
                    hd.dieu_khoan
                FROM dbo.dat_sanh ds
                INNER JOIN dbo.chi_nhanh cn ON ds.chi_nhanh_id = cn.chi_nhanh_id
                INNER JOIN dbo.sanh s ON ds.sanh_id = s.sanh_id
                INNER JOIN dbo.ca c ON ds.ca_id = c.ca_id
                INNER JOIN dbo.khach_hang kh ON ds.khach_hang_id = kh.khach_hang_id
                LEFT JOIN dbo.goi_tiec g ON ds.goi_id = g.goi_id
                LEFT JOIN dbo.hop_dong hd ON ds.dat_sanh_id = hd.dat_sanh_id
                WHERE ds.dat_sanh_id = @datSanhId";

            SqlParameter[] parameters = {
                new SqlParameter("@datSanhId", datSanhId)
            };

            try
            {
                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy thông tin đơn đặt sảnh: {ex.Message}", ex);
            }
        }

        // Lấy danh sách đơn đặt sảnh để hiển thị trong DataGridView
        public DataTable LayDanhSachDatSanh()
        {
            string query = @"
                SELECT 
                    ds.dat_sanh_id,
                    ds.ngay_to_chuc,
                    ds.gio_to_chuc,
                    kh.ho_ten AS ten_khach_hang,
                    kh.sdt,
                    s.ten_sanh,
                    ds.so_ban_du_kien,
                    CASE 
                        WHEN ds.so_ban_du_kien IS NOT NULL THEN ds.so_ban_du_kien * 10
                        ELSE NULL
                    END AS so_khach_du_kien,
                    ds.trang_thai,
                    ISNULL(hd.tong_du_kien, 0) AS tong_tien,
                    ISNULL((
                        SELECT SUM(hdc.so_tien)
                        FROM dbo.hop_dong_coc hdc
                        WHERE hdc.hop_dong_id = hd.hop_dong_id
                          AND hd.hop_dong_id IS NOT NULL
                    ), 0) AS tien_coc
                FROM dbo.dat_sanh ds
                INNER JOIN dbo.khach_hang kh ON ds.khach_hang_id = kh.khach_hang_id
                INNER JOIN dbo.sanh s ON ds.sanh_id = s.sanh_id
                LEFT JOIN dbo.hop_dong hd ON ds.dat_sanh_id = hd.dat_sanh_id
                ORDER BY ds.ngay_to_chuc DESC, ds.dat_sanh_id DESC";

            try
            {
                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách đơn đặt sảnh: {ex.Message}", ex);
            }
        }

        // Lấy hop_dong_id từ dat_sanh_id
        public int? LayHopDongId(int datSanhId)
        {
            string query = @"
                SELECT hop_dong_id
                FROM dbo.hop_dong
                WHERE dat_sanh_id = @datSanhId";

            SqlParameter[] parameters = {
                new SqlParameter("@datSanhId", datSanhId)
            };

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                if (result != null && result != DBNull.Value)
                    return Convert.ToInt32(result);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy hop_dong_id: {ex.Message}", ex);
            }
        }

        // Lấy danh sách cọc từ hop_dong_id
        public DataTable LayDanhSachCoc(int hopDongId)
        {
            string query = @"
                SELECT 
                    coc_id,
                    so_tien,
                    ngay_nop,
                    hinh_thuc,
                    ghi_chu
                FROM dbo.hop_dong_coc
                WHERE hop_dong_id = @hopDongId
                ORDER BY ngay_nop DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId)
            };

            try
            {
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách cọc: {ex.Message}", ex);
            }
        }

        // Lấy danh sách thanh toán từ hop_dong_id
        public DataTable LayDanhSachThanhToan(int hopDongId)
        {
            string query = @"
                SELECT 
                    tt_id,
                    so_tien,
                    ngay_tt,
                    hinh_thuc,
                    noi_dung
                FROM dbo.hop_dong_tt
                WHERE hop_dong_id = @hopDongId
                ORDER BY ngay_tt DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId)
            };

            try
            {
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách thanh toán: {ex.Message}", ex);
            }
        }

        // Lấy tổng dự kiến từ hop_dong
        public decimal LayTongDuKien(int hopDongId)
        {
            string query = @"
                SELECT ISNULL(tong_du_kien, 0)
                FROM dbo.hop_dong
                WHERE hop_dong_id = @hopDongId";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId)
            };

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                if (result != null && result != DBNull.Value)
                    return Convert.ToDecimal(result);
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tổng dự kiến: {ex.Message}", ex);
            }
        }

        // Đổi lịch đặt sảnh
        public bool DoiLichDatSanh(int datSanhId, int chiNhanhId, int sanhId, TimeSpan gioToChuc, DateTime ngayToChuc, string lyDo, string? ghiChuThem)
        {
            try
            {
                // Lấy ghi chú hiện tại
                string queryGetGhiChu = @"
                    SELECT ISNULL(ghi_chu, N'') AS ghi_chu
                    FROM dbo.dat_sanh
                    WHERE dat_sanh_id = @datSanhId";

                SqlParameter[] paramsGet = {
                    new SqlParameter("@datSanhId", datSanhId)
                };

                string ghiChuHienTai = "";
                DataTable dt = _dbHelper.GetDataTable(queryGetGhiChu, paramsGet);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["ghi_chu"] != DBNull.Value)
                {
                    ghiChuHienTai = dt.Rows[0]["ghi_chu"].ToString() ?? "";
                }

                // Tạo ghi chú mới
                string ghiChuMoi = "";
                if (!string.IsNullOrWhiteSpace(ghiChuHienTai))
                {
                    ghiChuMoi = ghiChuHienTai + "\n";
                }

                ghiChuMoi += $"Lý do đổi sảnh: {lyDo}";
                if (!string.IsNullOrWhiteSpace(ghiChuThem))
                {
                    ghiChuMoi += $" | Ghi chú: {ghiChuThem}";
                }

                // Tìm ca_id dựa trên giờ tổ chức
                string queryGetCa = @"
                    SELECT ca_id
                    FROM dbo.ca
                    WHERE gio_bd = @gioToChuc";

                SqlParameter[] paramsCa = {
                    new SqlParameter("@gioToChuc", gioToChuc)
                };

                int caId = 0;
                DataTable dtCa = _dbHelper.GetDataTable(queryGetCa, paramsCa);
                if (dtCa != null && dtCa.Rows.Count > 0 && dtCa.Rows[0]["ca_id"] != DBNull.Value)
                {
                    caId = Convert.ToInt32(dtCa.Rows[0]["ca_id"]);
                }
                else
                {
                    // Tạm giờ bắt đầu tiệc test
                    string tenCa = gioToChuc.Hours == 10 ? "Ca sáng" : "Ca tối";
                    TimeSpan gioKt = gioToChuc.Hours == 10 ? new TimeSpan(13, 30, 0) : new TimeSpan(20, 30, 0);

                    string queryInsertCa = @"
                        INSERT INTO dbo.ca (ten_ca, gio_bd, gio_kt)
                        OUTPUT INSERTED.ca_id
                        VALUES (@tenCa, @gioBd, @gioKt)";

                    SqlParameter[] paramsInsertCa = {
                        new SqlParameter("@tenCa", tenCa),
                        new SqlParameter("@gioBd", gioToChuc),
                        new SqlParameter("@gioKt", gioKt)
                    };

                    object result = _dbHelper.ExecuteScalar(queryInsertCa, paramsInsertCa);
                    if (result != null && result != DBNull.Value)
                    {
                        caId = Convert.ToInt32(result);
                    }
                    else
                    {
                        string queryGetCaDefault = @"SELECT TOP 1 ca_id FROM dbo.ca ORDER BY ca_id";
                        object caIdDefault = _dbHelper.ExecuteScalar(queryGetCaDefault);
                        if (caIdDefault != null && caIdDefault != DBNull.Value)
                        {
                            caId = Convert.ToInt32(caIdDefault);
                        }
                        else
                        {
                            throw new Exception("Không tìm thấy ca nào trong database!");
                        }
                    }
                }

                // Cập nhật dat_sanh
                string query = @"
                    UPDATE dbo.dat_sanh
                    SET chi_nhanh_id = @chiNhanhId,
                        sanh_id = @sanhId,
                        ca_id = @caId,
                        ngay_to_chuc = @ngayToChuc,
                        gio_to_chuc = @gioToChuc,
                        ghi_chu = @ghiChu
                    WHERE dat_sanh_id = @datSanhId";

                SqlParameter[] parameters = {
                    new SqlParameter("@datSanhId", datSanhId),
                    new SqlParameter("@chiNhanhId", chiNhanhId),
                    new SqlParameter("@sanhId", sanhId),
                    new SqlParameter("@caId", caId),
                    new SqlParameter("@ngayToChuc", ngayToChuc.Date),
                    new SqlParameter("@gioToChuc", gioToChuc),
                    new SqlParameter("@ghiChu", ghiChuMoi)
                };

                _dbHelper.ExecuteNonQuery(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đổi lịch đặt sảnh: {ex.Message}", ex);
            }
        }

        // Lấy tổng số đơn đặt sảnh
        public int LayTongSoDon()
        {
            string query = @"
                SELECT COUNT(*) 
                FROM dbo.dat_sanh";

            try
            {
                object result = _dbHelper.ExecuteScalar(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tổng số đơn: {ex.Message}", ex);
            }
        }

        // Lấy số đơn đã xác nhận (ĐÃ CỌC hoặc ĐÃ THANH TOÁN)
        public int LaySoDonXacNhan()
        {
            string query = @"
                SELECT COUNT(*) 
                FROM dbo.dat_sanh
                WHERE trang_thai IN (N'ĐÃ CỌC', N'ĐÃ THANH TOÁN', N'HOÀN TẤT')";

            try
            {
                object result = _dbHelper.ExecuteScalar(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy số đơn xác nhận: {ex.Message}", ex);
            }
        }

        // Lấy tổng số sảnh
        public int LayTongSoSanh()
        {
            string query = @"
                SELECT COUNT(DISTINCT sanh_id) 
                FROM dbo.dat_sanh
                WHERE trang_thai NOT IN (N'ĐÃ HỦY')";

            try
            {
                object result = _dbHelper.ExecuteScalar(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tổng số sảnh: {ex.Message}", ex);
            }
        }

        // Lấy doanh thu tháng (tổng tiền từ các hợp đồng trong tháng hiện tại)
        public decimal LayDoanhThuThang()
        {
            string query = @"
                SELECT ISNULL(SUM(hd.tong_du_kien), 0)
                FROM dbo.hop_dong hd
                INNER JOIN dbo.dat_sanh ds ON hd.dat_sanh_id = ds.dat_sanh_id
                WHERE YEAR(ds.ngay_to_chuc) = YEAR(GETDATE())
                  AND MONTH(ds.ngay_to_chuc) = MONTH(GETDATE())
                  AND ds.trang_thai NOT IN (N'ĐÃ HỦY')";

            try
            {
                object result = _dbHelper.ExecuteScalar(query);
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy doanh thu tháng: {ex.Message}", ex);
            }
        }

        // Xóa vĩnh viễn đặt sảnh
        public bool XoaDatSanhVinhVien(int datSanhId)
        {
            try
            {
                using (var connection = new SqlConnection(_dbHelper.ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Lấy hop_dong_id nếu có
                            string queryGetHopDongId = @"
                                SELECT hop_dong_id
                                FROM dbo.hop_dong
                                WHERE dat_sanh_id = @datSanhId;";

                            int? hopDongId = null;
                            using (var cmd = new SqlCommand(queryGetHopDongId, connection, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@datSanhId", datSanhId));
                                object result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    hopDongId = Convert.ToInt32(result);
                                }
                            }

                            if (hopDongId.HasValue)
                            {
                                // Xóa hop_dong_ct_mon (chi tiết món)
                                string queryDeleteCTMon = @"
                                    DELETE FROM dbo.hop_dong_ct_mon
                                    WHERE hop_dong_id = @hopDongId;";

                                using (var cmd = new SqlCommand(queryDeleteCTMon, connection, transaction))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@hopDongId", hopDongId.Value));
                                    cmd.ExecuteNonQuery();
                                }

                                // Xóa hop_dong_ct_dv (chi tiết dịch vụ)
                                string queryDeleteCTDV = @"
                                    DELETE FROM dbo.hop_dong_ct_dv
                                    WHERE hop_dong_id = @hopDongId;";

                                using (var cmd = new SqlCommand(queryDeleteCTDV, connection, transaction))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@hopDongId", hopDongId.Value));
                                    cmd.ExecuteNonQuery();
                                }

                                // Xóa hop_dong_tt
                                string queryDeleteTT = @"
                                    DELETE FROM dbo.hop_dong_tt
                                    WHERE hop_dong_id = @hopDongId;";

                                using (var cmd = new SqlCommand(queryDeleteTT, connection, transaction))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@hopDongId", hopDongId.Value));
                                    cmd.ExecuteNonQuery();
                                }

                                // Xóa hop_dong_coc
                                string queryDeleteCoc = @"
                                    DELETE FROM dbo.hop_dong_coc
                                    WHERE hop_dong_id = @hopDongId;";

                                using (var cmd = new SqlCommand(queryDeleteCoc, connection, transaction))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@hopDongId", hopDongId.Value));
                                    cmd.ExecuteNonQuery();
                                }

                                // Xóa hop_dong
                                string queryDeleteHopDong = @"
                                    DELETE FROM dbo.hop_dong
                                    WHERE hop_dong_id = @hopDongId;";

                                using (var cmd = new SqlCommand(queryDeleteHopDong, connection, transaction))
                                {
                                    cmd.Parameters.Add(new SqlParameter("@hopDongId", hopDongId.Value));
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Xóa phieu_order nếu có
                            string queryDeletePhieuOrder = @"
                                DELETE FROM dbo.phieu_order
                                WHERE dat_sanh_id = @datSanhId;";

                            using (var cmd = new SqlCommand(queryDeletePhieuOrder, connection, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@datSanhId", datSanhId));
                                cmd.ExecuteNonQuery();
                            }

                            // Xóa dat_sanh
                            string queryDeleteDatSanh = @"
                                DELETE FROM dbo.dat_sanh
                                WHERE dat_sanh_id = @datSanhId;";

                            using (var cmd = new SqlCommand(queryDeleteDatSanh, connection, transaction))
                            {
                                cmd.Parameters.Add(new SqlParameter("@datSanhId", datSanhId));
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected == 0)
                                {
                                    throw new Exception("Không tìm thấy đặt sảnh để xóa!");
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa đặt sảnh vĩnh viễn: {ex.Message}", ex);
            }
        }

        // Kiểm tra xem đã có hóa đơn chưa
        public bool DaCoHoaDon(int hopDongId)
        {
            string query = @"
                SELECT COUNT(*)
                FROM dbo.hoa_don
                WHERE loai = N'TIECCUOI' AND tham_chieu_id = @hopDongId";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId)
            };

            try
            {
                object result = _dbHelper.ExecuteScalar(query, parameters);
                int count = Convert.ToInt32(result);
                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        // Lấy chi tiết món từ hợp đồng
        public DataTable LayChiTietMonHopDong(int hopDongId)
        {
            string query = @"
                SELECT 
                    hdctm.mon_id,
                    ma.ten_mon,
                    hdctm.so_luong,
                    hdctm.don_gia
                FROM dbo.hop_dong_ct_mon hdctm
                INNER JOIN dbo.mon_an ma ON hdctm.mon_id = ma.mon_id
                WHERE hdctm.hop_dong_id = @hopDongId";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId)
            };

            try
            {
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy chi tiết món hợp đồng: {ex.Message}", ex);
            }
        }

        // Lấy chi tiết dịch vụ từ hợp đồng
        public DataTable LayChiTietDichVuHopDong(int hopDongId)
        {
            string query = @"
                SELECT 
                    hdctdv.dv_id,
                    dv.ten_dv,
                    hdctdv.so_luong,
                    hdctdv.don_gia
                FROM dbo.hop_dong_ct_dv hdctdv
                INNER JOIN dbo.dich_vu dv ON hdctdv.dv_id = dv.dv_id
                WHERE hdctdv.hop_dong_id = @hopDongId";

            SqlParameter[] parameters = {
                new SqlParameter("@hopDongId", hopDongId)
            };

            try
            {
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy chi tiết dịch vụ hợp đồng: {ex.Message}", ex);
            }
        }

        // Lấy danh sách dat_sanh theo chi nhánh để hiển thị trong panelHDGD
        public DataTable LayDanhSachDatSanhTheoChiNhanh(int chiNhanhId, int top = 100)
        {
            // Validate top để tránh SQL injection (chỉ cho phép giá trị hợp lệ)
            if (top <= 0) top = 100;
            if (top > 1000) top = 1000; // Giới hạn tối đa 1000 bản ghi

            // Sử dụng ROW_NUMBER() để đảm bảo an toàn và tương thích với mọi phiên bản SQL Server
            string query = @"
                SELECT 
                    dat_sanh_id,
                    ngay_to_chuc,
                    trang_thai,
                    ten_khach_hang,
                    ten_sanh,
                    gia_goi_tiec
                FROM (
                    SELECT 
                        ds.dat_sanh_id,
                        ds.ngay_to_chuc,
                        ds.trang_thai,
                        kh.ho_ten AS ten_khach_hang,
                        s.ten_sanh,
                        ISNULL(gt.gia_co_ban, 0) AS gia_goi_tiec,
                        ROW_NUMBER() OVER (ORDER BY ds.ngay_to_chuc DESC, ds.dat_sanh_id DESC) AS RowNum
                    FROM dbo.dat_sanh ds
                    INNER JOIN dbo.khach_hang kh ON ds.khach_hang_id = kh.khach_hang_id
                    INNER JOIN dbo.sanh s ON ds.sanh_id = s.sanh_id
                    LEFT JOIN dbo.goi_tiec gt ON ds.goi_id = gt.goi_id
                    WHERE ds.chi_nhanh_id = @chiNhanhId
                ) AS RankedData
                WHERE RowNum <= @top
                ORDER BY ngay_to_chuc DESC, dat_sanh_id DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@chiNhanhId", chiNhanhId),
                new SqlParameter("@top", top)
            };

            try
            {
                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách đặt sảnh theo chi nhánh: {ex.Message}", ex);
            }
        }
    }
}

