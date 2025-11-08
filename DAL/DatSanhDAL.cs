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
        public bool KiemTraSanhTrong(int sanhId, int caId, DateTime ngayToChuc)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM dbo.dat_sanh 
                WHERE sanh_id = @sanhId 
                  AND ca_id = @caId 
                  AND ngay_to_chuc = @ngayToChuc
                  AND trang_thai NOT IN (N'ĐÃ HỦY', N'HOÀN TẤT')";

            SqlParameter[] parameters = {
                new SqlParameter("@sanhId", sanhId),
                new SqlParameter("@caId", caId),
                new SqlParameter("@ngayToChuc", ngayToChuc.Date)
            };

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
        public bool HuyDatSanh(int datSanhId)
        {
            string ghiChu = "Khách hàng hủy đặt sảnh";
            
            try
            {
                using (var connection = new SqlConnection(_dbHelper.ConnectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Cập nhật trạng thái và ghi chú trong dat_sanh
                            string queryDatSanh = @"
                                UPDATE dbo.dat_sanh
                                SET trang_thai = N'ĐÃ HỦY',
                                    ghi_chu = @ghiChu
                                WHERE dat_sanh_id = @datSanhId;";

                            SqlParameter[] paramsDatSanh = {
                                new SqlParameter("@datSanhId", datSanhId),
                                new SqlParameter("@ghiChu", ghiChu)
                            };

                            using (var cmd = new SqlCommand(queryDatSanh, connection, transaction))
                            {
                                cmd.Parameters.AddRange(paramsDatSanh);
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Lấy hop_dong_id từ dat_sanh_id
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

                            // 3. Cập nhật ghi chú trong hop_dong_coc (nếu có)
                            if (hopDongId.HasValue)
                            {
                                string queryCoc = @"
                                    UPDATE dbo.hop_dong_coc
                                    SET ghi_chu = @ghiChu
                                    WHERE hop_dong_id = @hopDongId;";

                                SqlParameter[] paramsCoc = {
                                    new SqlParameter("@hopDongId", hopDongId.Value),
                                    new SqlParameter("@ghiChu", ghiChu)
                                };

                                using (var cmd = new SqlCommand(queryCoc, connection, transaction))
                                {
                                    cmd.Parameters.AddRange(paramsCoc);
                                    cmd.ExecuteNonQuery();
                                }

                                // 4. Cập nhật ghi chú trong hop_dong_tt (nếu có)
                                string queryTT = @"
                                    UPDATE dbo.hop_dong_tt
                                    SET noi_dung = @ghiChu
                                    WHERE hop_dong_id = @hopDongId;";

                                SqlParameter[] paramsTT = {
                                    new SqlParameter("@hopDongId", hopDongId.Value),
                                    new SqlParameter("@ghiChu", ghiChu)
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
    }
}

