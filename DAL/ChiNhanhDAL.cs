using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;

namespace DAL
{
    public class ChiNhanhDAL
    {
        private DatabaseHelper _dbHelper;

        public ChiNhanhDAL(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public DataTable LayDanhSachChiNhanh()
        {
            try
            {
                string query = @"
                    SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai
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

        public DataTable LayTatCaChiNhanh()
        {
            try
            {
                string query = @"
                    SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai
                    FROM dbo.chi_nhanh
                    ORDER BY ten";

                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tất cả chi nhánh: {ex.Message}");
            }
        }

        public DataTable LayChiNhanhTheoTrangThai(int? trangThai)
        {
            try
            {
                string query;
                SqlParameter[] parameters = null;

                if (trangThai.HasValue)
                {
                    query = @"
                        SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai
                        FROM dbo.chi_nhanh
                        WHERE trang_thai = @trangThai
                        ORDER BY ten";
                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("@trangThai", trangThai.Value)
                    };
                }
                else
                {
                    query = @"
                        SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai
                        FROM dbo.chi_nhanh
                        ORDER BY ten";
                }

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy chi nhánh theo trạng thái: {ex.Message}");
            }
        }

        public DataTable TimKiemChiNhanh(string keyword, int? trangThai)
        {
            try
            {
                string query;
                List<SqlParameter> parameters = new List<SqlParameter>();

                query = @"
                    SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai
                    FROM dbo.chi_nhanh
                    WHERE (ten LIKE @keyword OR dia_chi LIKE @keyword OR sdt LIKE @keyword)";

                parameters.Add(new SqlParameter("@keyword", $"%{keyword}%"));

                if (trangThai.HasValue)
                {
                    query += " AND trang_thai = @trangThai";
                    parameters.Add(new SqlParameter("@trangThai", trangThai.Value));
                }

                query += " ORDER BY ten";

                return _dbHelper.GetDataTable(query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm chi nhánh: {ex.Message}");
            }
        }

        public DataTable LayChiNhanhById(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai
                    FROM dbo.chi_nhanh
                    WHERE chi_nhanh_id = @chiNhanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                return _dbHelper.GetDataTable(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy chi nhánh theo ID: {ex.Message}");
            }
        }

        public int ThemChiNhanh(string ten, string diaChi, string sdt, int trangThai)
        {
            try
            {
                string query = @"
                    INSERT INTO dbo.chi_nhanh (ten, dia_chi, sdt, trang_thai)
                    VALUES (@ten, @diaChi, @sdt, @trangThai);
                    SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@diaChi", diaChi ?? (object)DBNull.Value),
                    new SqlParameter("@sdt", sdt ?? (object)DBNull.Value),
                    new SqlParameter("@trangThai", trangThai)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                int chiNhanhId = Convert.ToInt32(result);

                return chiNhanhId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm chi nhánh: {ex.Message}");
            }
        }

        public int DemSoBan(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM dbo.ban 
                    WHERE chi_nhanh_id = @chiNhanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đếm số bàn: {ex.Message}");
            }
        }

        public int DemSoSanh(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM dbo.sanh 
                    WHERE chi_nhanh_id = @chiNhanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đếm số sảnh: {ex.Message}");
            }
        }

        public int DemSoNhanVien(int chiNhanhId)
        {
            try
            {
                string query = @"
                    SELECT COUNT(DISTINCT nd.nguoi_dung_id) 
                    FROM dbo.nguoi_dung nd
                    INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
                    WHERE ndcn.chi_nhanh_id = @chiNhanhId
                      AND nd.hoat_dong = 1";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đếm số nhân viên: {ex.Message}");
            }
        }

        public bool CapNhatChiNhanh(int chiNhanhId, string ten, string diaChi, string sdt, int trangThai)
        {
            try
            {
                string query = @"
                    UPDATE dbo.chi_nhanh
                    SET ten = @ten,
                        dia_chi = @diaChi,
                        sdt = @sdt,
                        trang_thai = @trangThai
                    WHERE chi_nhanh_id = @chiNhanhId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId),
                    new SqlParameter("@ten", ten),
                    new SqlParameter("@diaChi", diaChi ?? (object)DBNull.Value),
                    new SqlParameter("@sdt", sdt ?? (object)DBNull.Value),
                    new SqlParameter("@trangThai", trangThai)
                };

                int result = _dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật chi nhánh: {ex.Message}");
            }
        }

        public bool XoaChiNhanh(int chiNhanhId)
        {
            try
            {
                // Xóa các bản ghi liên quan trước (do foreign key constraints)
                // Thứ tự xóa: xóa các bảng con trước, sau đó xóa bảng cha
                
                using (SqlConnection conn = new SqlConnection(_dbHelper.ConnectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();
                    
                    try
                    {
                        // Xóa nguoi_dung_chi_nhanh
                        string query1 = "DELETE FROM dbo.nguoi_dung_chi_nhanh WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query1, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa ton_kho
                        string query2 = "DELETE FROM dbo.ton_kho WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query2, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa ban (sẽ tự động xóa dat_ban và phieu_order liên quan nếu có cascade)
                        string query3 = "DELETE FROM dbo.ban WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query3, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa khu_vuc
                        string query4 = "DELETE FROM dbo.khu_vuc WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query4, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa hop_dong_ct_mon (chi tiết hợp đồng - món ăn)
                        string query5a = @"DELETE FROM dbo.hop_dong_ct_mon 
                                          WHERE hop_dong_id IN (
                                              SELECT hop_dong_id FROM dbo.hop_dong 
                                              WHERE dat_sanh_id IN (
                                                  SELECT dat_sanh_id FROM dbo.dat_sanh WHERE chi_nhanh_id = @chiNhanhId
                                              )
                                          )";
                        using (SqlCommand cmd = new SqlCommand(query5a, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa hop_dong_ct_dv (chi tiết hợp đồng - dịch vụ)
                        string query5b = @"DELETE FROM dbo.hop_dong_ct_dv 
                                          WHERE hop_dong_id IN (
                                              SELECT hop_dong_id FROM dbo.hop_dong 
                                              WHERE dat_sanh_id IN (
                                                  SELECT dat_sanh_id FROM dbo.dat_sanh WHERE chi_nhanh_id = @chiNhanhId
                                              )
                                          )";
                        using (SqlCommand cmd = new SqlCommand(query5b, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa hop_dong (hợp đồng)
                        string query5c = @"DELETE FROM dbo.hop_dong 
                                          WHERE dat_sanh_id IN (
                                              SELECT dat_sanh_id FROM dbo.dat_sanh WHERE chi_nhanh_id = @chiNhanhId
                                          )";
                        using (SqlCommand cmd = new SqlCommand(query5c, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa dat_sanh (đặt sảnh)
                        string query5 = "DELETE FROM dbo.dat_sanh WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query5, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa sanh (sảnh)
                        string query6 = "DELETE FROM dbo.sanh WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query6, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa dat_ban
                        string query7 = "DELETE FROM dbo.dat_ban WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query7, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa phieu_order_ct (chi tiết order)
                        string query8a = @"DELETE FROM dbo.phieu_order_ct 
                                          WHERE phieu_order_id IN (
                                              SELECT phieu_order_id FROM dbo.phieu_order WHERE chi_nhanh_id = @chiNhanhId
                                          )";
                        using (SqlCommand cmd = new SqlCommand(query8a, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa phieu_order
                        string query8 = "DELETE FROM dbo.phieu_order WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query8, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa hoa_don_ct (chi tiết hóa đơn)
                        string query9a = @"DELETE FROM dbo.hoa_don_ct 
                                          WHERE hoa_don_id IN (
                                              SELECT hoa_don_id FROM dbo.hoa_don WHERE chi_nhanh_id = @chiNhanhId
                                          )";
                        using (SqlCommand cmd = new SqlCommand(query9a, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa hoa_don_km (khuyến mãi hóa đơn)
                        string query9b = @"DELETE FROM dbo.hoa_don_km 
                                          WHERE hoa_don_id IN (
                                              SELECT hoa_don_id FROM dbo.hoa_don WHERE chi_nhanh_id = @chiNhanhId
                                          )";
                        using (SqlCommand cmd = new SqlCommand(query9b, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa thanh_toan (thanh toán)
                        string query9c = @"DELETE FROM dbo.thanh_toan 
                                          WHERE hoa_don_id IN (
                                              SELECT hoa_don_id FROM dbo.hoa_don WHERE chi_nhanh_id = @chiNhanhId
                                          )";
                        using (SqlCommand cmd = new SqlCommand(query9c, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Xóa hoa_don
                        string query9d = "DELETE FROM dbo.hoa_don WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query9d, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            cmd.ExecuteNonQuery();
                        }
                        
                        // Cuối cùng xóa chi_nhanh
                        string query9 = "DELETE FROM dbo.chi_nhanh WHERE chi_nhanh_id = @chiNhanhId";
                        using (SqlCommand cmd = new SqlCommand(query9, conn, transaction))
                        {
                            cmd.Parameters.Add(new SqlParameter("@chiNhanhId", chiNhanhId));
                            int result = cmd.ExecuteNonQuery();
                            
                            if (result > 0)
                            {
                                transaction.Commit();
                                return true;
                            }
                            else
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa chi nhánh: {ex.Message}");
            }
        }
    }
}
