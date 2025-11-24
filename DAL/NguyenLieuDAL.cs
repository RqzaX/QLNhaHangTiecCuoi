using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.DAL
{
    public class NguyenLieuDAL
    {
        private readonly DatabaseHelper _db;

        public NguyenLieuDAL(DatabaseHelper dbHelper)
        {
            _db = dbHelper;
        }

        public DataTable GetDanhMuc()
        {
            const string sql = @"
                SELECT nl_id, ma_nl, ten_nl, don_vi
                FROM dbo.nguyen_lieu
                ORDER BY ten_nl;";
            return _db.GetDataTable(sql);
        }

      
        public DataTable GetTonKhoByTinhTrang(int tinhTrang, decimal canhBao, int? chiNhanhId)
        {
            var sql = @"
SELECT 
    nl.nl_id, nl.ma_nl, nl.ten_nl, nl.don_vi,
    ISNULL(tk.sl_ton, 0) AS sl_ton,
    ISNULL(tk.ton_toi_thieu, 0) AS ton_toi_thieu
FROM dbo.nguyen_lieu nl
LEFT JOIN dbo.ton_kho tk
  ON tk.nl_id = nl.nl_id
 AND (@cn IS NULL OR tk.chi_nhanh_id = @cn)
WHERE 1 = 1";
            var sb = new System.Text.StringBuilder(sql);
            var prms = new List<SqlParameter> { new SqlParameter("@cn", (object?)chiNhanhId ?? DBNull.Value) };

            switch (tinhTrang)
            {
                case 1: sb.Append(" AND ISNULL(tk.sl_ton,0) > 0 "); break;
                case 2: sb.Append(" AND ISNULL(tk.sl_ton,0) = 0 "); break;
                case 3:
                    sb.Append(" AND ISNULL(tk.sl_ton,0) > 0 AND ISNULL(tk.sl_ton,0) <= @CanhBao ");
                    prms.Add(new SqlParameter("@CanhBao", canhBao));
                    break;
                default: break;
            }

            sb.Append(" ORDER BY nl.ten_nl;");
            return _db.GetDataTable(sb.ToString(), prms.ToArray());
        }

        public DataTable SearchByTinhTrang(string keyword, int tinhTrang, decimal canhBao, int? chiNhanhId)
        {
            var sql = @"
        SELECT 
            nl.nl_id, nl.ma_nl, nl.ten_nl, nl.don_vi,
            ISNULL(tk.sl_ton, 0) AS sl_ton
        FROM dbo.nguyen_lieu nl
        LEFT JOIN dbo.ton_kho tk
        ON tk.nl_id = nl.nl_id
        AND (@cn IS NULL OR tk.chi_nhanh_id = @cn)
    WHERE (nl.ma_nl LIKE @kw OR nl.ten_nl LIKE @kw)";
            var sb = new System.Text.StringBuilder(sql);
            var prms = new List<SqlParameter> {
        new SqlParameter("@kw", "%" + (keyword ?? string.Empty).Trim() + "%"),
        new SqlParameter("@cn", (object?)chiNhanhId ?? DBNull.Value)
    };

            switch (tinhTrang)
            {
                case 1: sb.Append(" AND ISNULL(tk.sl_ton,0) > 0 "); break;
                case 2: sb.Append(" AND ISNULL(tk.sl_ton,0) = 0 "); break;
                case 3:
                    sb.Append(" AND ISNULL(tk.sl_ton,0) > 0 AND ISNULL(tk.sl_ton,0) <= @CanhBao ");
                    prms.Add(new SqlParameter("@CanhBao", canhBao));
                    break;
                default: break;
            }

            sb.Append(" ORDER BY nl.ten_nl;");
            return _db.GetDataTable(sb.ToString(), prms.ToArray());
        }

      
        public DataTable GetByIdWithTon(int nlId)
        {
            const string sql = @"
                SELECT TOP 1 
                    nl.nl_id, nl.ma_nl, nl.ten_nl, nl.don_vi,
                    ISNULL(tk.sl_ton,0) AS sl_ton
                FROM dbo.nguyen_lieu nl
                LEFT JOIN dbo.ton_kho tk ON tk.nl_id = nl.nl_id
                WHERE nl.nl_id = @id;";
            return _db.GetDataTable(sql, new[] { new SqlParameter("@id", nlId) });
        }

      
        public DataTable GetTonKhoTheoChiNhanhCuaNguyenLieu(int nlId)
        {
            const string sql = @"
                SELECT 
                    tk.chi_nhanh_id,
                    ISNULL(cn.ten, ISNULL(cn.ten_cn, N'')) AS ten_chi_nhanh,
                    ISNULL(tk.sl_ton,0) AS sl_ton
                FROM dbo.ton_kho tk
                LEFT JOIN dbo.chi_nhanh cn ON cn.chi_nhanh_id = tk.chi_nhanh_id
                WHERE tk.nl_id = @id
                ORDER BY ten_chi_nhanh;";
            return _db.GetDataTable(sql, new[] { new SqlParameter("@id", nlId) });
        }

     
          public int Insert(string ma, string ten, string donVi)
        {
            const string sql = @"
                INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi)
                VALUES (@ma, @ten, @dv);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var prms = new[]
            {
                new SqlParameter("@ma", ma),
                new SqlParameter("@ten", ten),
                new SqlParameter("@dv", donVi),
            };
            try
            {
                var result = _db.ExecuteScalar(sql, prms);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - Insert: {ex.Message}", ex);
            }
        }

      public int Update(int nlId, string ma, string ten, string donVi)
        {
            const string sql = @"
                UPDATE dbo.nguyen_lieu
                SET ma_nl = @ma, ten_nl = @ten, don_vi = @dv
                WHERE nl_id = @id;";
            var prms = new[]
            {
                new SqlParameter("@id", nlId),
                new SqlParameter("@ma", ma),
                new SqlParameter("@ten", ten),
                new SqlParameter("@dv", donVi),
            };
            return _db.ExecuteNonQuery(sql, prms);
        }

       
        public decimal GetTonKho(int chiNhanhId, int nlId)
        {
            const string sql = @"SELECT sl_ton FROM dbo.ton_kho 
                         WHERE chi_nhanh_id = @cn AND nl_id = @nl";
            var dt = _db.GetDataTable(sql, new[] {
        new SqlParameter("@cn", chiNhanhId),
        new SqlParameter("@nl", nlId)
            });
            if (dt.Rows.Count == 0 || dt.Rows[0]["sl_ton"] == DBNull.Value) return 0m;
            return Convert.ToDecimal(dt.Rows[0]["sl_ton"]);
        }

      
        public int UpsertTonKho(int chiNhanhId, int nlId, decimal slTon)
        {
            const string sql = @"
        IF EXISTS (SELECT 1 FROM dbo.ton_kho WHERE chi_nhanh_id=@cn AND nl_id=@nl)
         UPDATE dbo.ton_kho SET sl_ton=@sl WHERE chi_nhanh_id=@cn AND nl_id=@nl;
        ELSE
            INSERT INTO dbo.ton_kho(chi_nhanh_id, nl_id, sl_ton, ton_toi_thieu) VALUES(@cn, @nl, @sl, 0);";
            return _db.ExecuteNonQuery(sql, new[] {
             new SqlParameter("@cn", chiNhanhId),
             new SqlParameter("@nl", nlId),
                new SqlParameter("@sl", slTon)
         });
        }
        public DataTable GetNguyenLieuById(int nlId)
        {
            const string sql = @"SELECT nl_id, ma_nl, ten_nl, don_vi
                         FROM dbo.nguyen_lieu
                         WHERE nl_id = @id";
            return _db.GetDataTable(sql, new[] { new SqlParameter("@id", nlId) });
        }

        
        public DataTable GetDataTable(string sql, SqlParameter[] parameters = null)
        {
            try
            {
                return _db.GetDataTable(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetDataTable: {ex.Message}", ex);
            }
        }

     
        public int NhapKho(int chiNhanhId, int nlId, decimal soLuong)
        {
            const string sql = @"
                -- Cập nhật tồn kho
                UPDATE dbo.ton_kho 
                SET sl_ton = sl_ton + @soLuong 
                WHERE chi_nhanh_id = @chiNhanhId AND nl_id = @nlId;
                -- Nếu chưa có record tồn kho thì tạo mới
                IF NOT EXISTS (SELECT 1 FROM dbo.ton_kho WHERE chi_nhanh_id = @chiNhanhId AND nl_id = @nlId)
                BEGIN
                    INSERT INTO dbo.ton_kho (chi_nhanh_id, nl_id, sl_ton, ton_toi_thieu)
                    VALUES (@chiNhanhId, @nlId, @soLuong, 0);
                END";
            
            var parameters = new[]
            {
                new SqlParameter("@chiNhanhId", chiNhanhId),
                new SqlParameter("@nlId", nlId),
                new SqlParameter("@soLuong", soLuong)
            };
            
            try
            {
                return _db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - NhapKho: {ex.Message}", ex);
            }
        }

   
        public int XuatKho(int chiNhanhId, int nlId, decimal soLuong)
        {
            const string sql = @"
                -- Cập nhật tồn kho (trừ đi)
                UPDATE dbo.ton_kho 
                SET sl_ton = sl_ton - @soLuong 
                WHERE chi_nhanh_id = @chiNhanhId AND nl_id = @nlId;
                -- Kiểm tra tồn kho không âm
                IF EXISTS (SELECT 1 FROM dbo.ton_kho WHERE chi_nhanh_id = @chiNhanhId AND nl_id = @nlId AND sl_ton < 0)
                BEGIN
                    -- Rollback nếu tồn kho âm
                    UPDATE dbo.ton_kho 
                    SET sl_ton = sl_ton + @soLuong 
                    WHERE chi_nhanh_id = @chiNhanhId AND nl_id = @nlId;
                    RAISERROR('Không đủ tồn kho để xuất', 16, 1);
                END";
            
            var parameters = new[]
            {
                new SqlParameter("@chiNhanhId", chiNhanhId),
                new SqlParameter("@nlId", nlId),
                new SqlParameter("@soLuong", soLuong)
            };
            
            try
            {
                return _db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - XuatKho: {ex.Message}", ex);
            }
        }

      
        public int ChuyenKho(int chiNhanhNguonId, int chiNhanhDichId, int nlId, decimal soLuong)
        {
            const string sql = @"
                -- Trừ tồn kho ở chi nhánh nguồn
                UPDATE dbo.ton_kho 
                SET sl_ton = sl_ton - @soLuong 
                WHERE chi_nhanh_id = @chiNhanhNguonId AND nl_id = @nlId;
                -- Kiểm tra tồn kho không âm
                IF EXISTS (SELECT 1 FROM dbo.ton_kho WHERE chi_nhanh_id = @chiNhanhNguonId AND nl_id = @nlId AND sl_ton < 0)
                BEGIN
                    -- Rollback nếu tồn kho âm
                    UPDATE dbo.ton_kho 
                    SET sl_ton = sl_ton + @soLuong 
                    WHERE chi_nhanh_id = @chiNhanhNguonId AND nl_id = @nlId;
                    RAISERROR('Không đủ tồn kho ở chi nhánh nguồn để chuyển', 16, 1);
                END
                ELSE
                BEGIN
                    -- Cộng tồn kho ở chi nhánh đích
                    UPDATE dbo.ton_kho 
                    SET sl_ton = sl_ton + @soLuong 
                    WHERE chi_nhanh_id = @chiNhanhDichId AND nl_id = @nlId;
                    -- Nếu chưa có record tồn kho ở chi nhánh đích thì tạo mới
                    IF NOT EXISTS (SELECT 1 FROM dbo.ton_kho WHERE chi_nhanh_id = @chiNhanhDichId AND nl_id = @nlId)
                    BEGIN
                        INSERT INTO dbo.ton_kho (chi_nhanh_id, nl_id, sl_ton, ton_toi_thieu)
                        VALUES (@chiNhanhDichId, @nlId, @soLuong, 0);
                    END
                END";
            
            var parameters = new[]
            {
                new SqlParameter("@chiNhanhNguonId", chiNhanhNguonId),
                new SqlParameter("@chiNhanhDichId", chiNhanhDichId),
                new SqlParameter("@nlId", nlId),
                new SqlParameter("@soLuong", soLuong)
            };
            
            try
            {
                return _db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - ChuyenKho: {ex.Message}", ex);
            }
        }

        public int LuuPhieuNhapKho(int chiNhanhId, DateTime ngayNhap, TimeSpan gioNhap, string nhanVienNhap, 
            string ghiChu, List<PhieuNhapKhoChiTiet> chiTietList)
        {
            const string sqlPhieu = @"
                INSERT INTO dbo.phieu_nhap_kho (chi_nhanh_id, ngay_nhap, gio_nhap, nhan_vien_nhap, ghi_chu, trang_thai)
                VALUES (@chiNhanhId, @ngayNhap, @gioNhap, @nhanVienNhap, @ghiChu, N'ĐÃ LƯU');
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            const string sqlChiTiet = @"
                INSERT INTO dbo.phieu_nhap_kho_ct (phieu_nhap_id, nl_id, so_luong, don_vi, ghi_chu)
                VALUES (@phieuNhapId, @nlId, @soLuong, @donVi, @ghiChu);";

            try
            {
                // Lưu phiếu nhập kho
                var phieuParams = new[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId),
                    new SqlParameter("@ngayNhap", ngayNhap.Date),
                    new SqlParameter("@gioNhap", gioNhap),
                    new SqlParameter("@nhanVienNhap", nhanVienNhap ?? (object)DBNull.Value),
                    new SqlParameter("@ghiChu", ghiChu ?? (object)DBNull.Value)
                };

                var phieuNhapId = Convert.ToInt32(_db.ExecuteScalar(sqlPhieu, phieuParams));

                // Lưu chi tiết và cập nhật tồn kho (xuất kho ra bếp)
                foreach (var ct in chiTietList)
                {
                    // Kiểm tra tồn kho trước khi xuất
                    decimal tonHienTai = GetTonKho(chiNhanhId, ct.NlId);
                    if (tonHienTai < ct.SoLuong)
                    {
                        throw new Exception($"Nguyên liệu (ID: {ct.NlId}) không đủ tồn kho. Hiện còn: {tonHienTai:N3}");
                    }

                    var ctParams = new[]
                    {
                        new SqlParameter("@phieuNhapId", phieuNhapId),
                        new SqlParameter("@nlId", ct.NlId),
                        new SqlParameter("@soLuong", ct.SoLuong),
                        new SqlParameter("@donVi", ct.DonVi ?? ""),
                        new SqlParameter("@ghiChu", ct.GhiChu ?? (object)DBNull.Value)
                    };

                    _db.ExecuteNonQuery(sqlChiTiet, ctParams);

                    // Trừ tồn kho
                    XuatKho(chiNhanhId, ct.NlId, ct.SoLuong);
                }

                return phieuNhapId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - LuuPhieuNhapKho: {ex.Message}", ex);
            }
        }

        public int LuuPhieuTraKho(int chiNhanhId, DateTime ngayTra, TimeSpan gioTra, string nhanVienTra,
            string ghiChu, List<PhieuTraKhoChiTiet> chiTietList)
        {
            const string sqlPhieu = @"
                INSERT INTO dbo.phieu_tra_kho (chi_nhanh_id, ngay_tra, gio_tra, nhan_vien_tra, ghi_chu, trang_thai)
                VALUES (@chiNhanhId, @ngayTra, @gioTra, @nhanVienTra, @ghiChu, N'ĐÃ LƯU');
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            const string sqlChiTiet = @"
                INSERT INTO dbo.phieu_tra_kho_ct (phieu_tra_id, nl_id, so_luong_tra, so_luong_ton, so_luong_con_lai, don_vi, ghi_chu)
                VALUES (@phieuTraId, @nlId, @soLuongTra, @soLuongTon, @soLuongConLai, @donVi, @ghiChu);";

            try
            {
                // Lưu phiếu trả kho
                var phieuParams = new[]
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId),
                    new SqlParameter("@ngayTra", ngayTra.Date),
                    new SqlParameter("@gioTra", gioTra),
                    new SqlParameter("@nhanVienTra", nhanVienTra ?? (object)DBNull.Value),
                    new SqlParameter("@ghiChu", ghiChu ?? (object)DBNull.Value)
                };

                var phieuTraId = Convert.ToInt32(_db.ExecuteScalar(sqlPhieu, phieuParams));

                // Lưu chi tiết và cộng tồn kho (trả lại kho)
                foreach (var ct in chiTietList)
                {
                    var ctParams = new[]
                    {
                        new SqlParameter("@phieuTraId", phieuTraId),
                        new SqlParameter("@nlId", ct.NlId),
                        new SqlParameter("@soLuongTra", ct.SoLuongTra),
                        new SqlParameter("@soLuongTon", ct.SoLuongTon),
                        new SqlParameter("@soLuongConLai", ct.SoLuongConLai),
                        new SqlParameter("@donVi", ct.DonVi ?? ""),
                        new SqlParameter("@ghiChu", ct.GhiChu ?? (object)DBNull.Value)
                    };

                    _db.ExecuteNonQuery(sqlChiTiet, ctParams);

                    // Cộng tồn kho
                    NhapKho(chiNhanhId, ct.NlId, ct.SoLuongTra);
                }

                return phieuTraId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - LuuPhieuTraKho: {ex.Message}", ex);
            }
        }

        // Hủy phiếu trả kho - cập nhật trạng thái và hoàn nguyên tồn kho
        public int HuyPhieuTraKho(int phieuTraId)
        {
            const string sqlGetChiTiet = @"
                SELECT nl_id, so_luong_tra
                FROM dbo.phieu_tra_kho_ct
                WHERE phieu_tra_id = @phieuTraId;";

            const string sqlUpdateTrangThai = @"
                UPDATE dbo.phieu_tra_kho
                SET trang_thai = N'HỦY'
                WHERE phieu_tra_id = @phieuTraId AND trang_thai = N'ĐÃ LƯU';";

            try
            {
                // Kiểm tra phiếu có tồn tại và đang ở trạng thái "ĐÃ LƯU" không
                const string sqlCheck = @"
                    SELECT chi_nhanh_id, trang_thai
                    FROM dbo.phieu_tra_kho
                    WHERE phieu_tra_id = @phieuTraId;";
                
                var checkParams = new[] { new SqlParameter("@phieuTraId", phieuTraId) };
                var checkResult = _db.GetDataTable(sqlCheck, checkParams);
                
                if (checkResult == null || checkResult.Rows.Count == 0)
                {
                    throw new Exception("Không tìm thấy phiếu trả kho!");
                }

                var row = checkResult.Rows[0];
                string trangThai = row["trang_thai"]?.ToString() ?? "";
                
                if (trangThai != "ĐÃ LƯU")
                {
                    throw new Exception($"Không thể hủy phiếu với trạng thái: {trangThai}");
                }

                int chiNhanhId = Convert.ToInt32(row["chi_nhanh_id"]);

                // Lấy chi tiết phiếu trả kho - tạo parameter mới
                var chiTietParams = new[] { new SqlParameter("@phieuTraId", phieuTraId) };
                var chiTietData = _db.GetDataTable(sqlGetChiTiet, chiTietParams);
                
                if (chiTietData != null && chiTietData.Rows.Count > 0)
                {
                    // Trừ tồn kho để hoàn nguyên (vì khi lưu đã cộng tồn kho)
                    foreach (DataRow ctRow in chiTietData.Rows)
                    {
                        int nlId = Convert.ToInt32(ctRow["nl_id"]);
                        decimal soLuongTra = Convert.ToDecimal(ctRow["so_luong_tra"]);
                        
                        // Trừ tồn kho (hoàn nguyên)
                        XuatKho(chiNhanhId, nlId, soLuongTra);
                    }
                }

                // Cập nhật trạng thái thành "HỦY" - tạo parameter mới
                var updateParams = new[] { new SqlParameter("@phieuTraId", phieuTraId) };
                return _db.ExecuteNonQuery(sqlUpdateTrangThai, updateParams);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - HuyPhieuTraKho: {ex.Message}", ex);
            }
        }

        // Xóa vĩnh viễn phiếu nhập kho
        public int XoaPhieuNhapKho(int phieuNhapId)
        {
            const string sqlCheck = @"
                SELECT chi_nhanh_id, trang_thai
                FROM dbo.phieu_nhap_kho
                WHERE phieu_nhap_id = @phieuNhapId;";

            const string sqlGetChiTiet = @"
                SELECT nl_id, so_luong
                FROM dbo.phieu_nhap_kho_ct
                WHERE phieu_nhap_id = @phieuNhapId;";

            const string sqlDeleteChiTiet = @"
                DELETE FROM dbo.phieu_nhap_kho_ct
                WHERE phieu_nhap_id = @phieuNhapId;";

            const string sqlDeletePhieu = @"
                DELETE FROM dbo.phieu_nhap_kho
                WHERE phieu_nhap_id = @phieuNhapId;";

            try
            {
                // Kiểm tra phiếu có tồn tại không
                var checkParams = new[] { new SqlParameter("@phieuNhapId", phieuNhapId) };
                var checkResult = _db.GetDataTable(sqlCheck, checkParams);
                
                if (checkResult == null || checkResult.Rows.Count == 0)
                {
                    throw new Exception("Không tìm thấy phiếu nhập kho!");
                }

                var row = checkResult.Rows[0];
                int chiNhanhId = Convert.ToInt32(row["chi_nhanh_id"]);
                string trangThai = row["trang_thai"]?.ToString() ?? "";

                // Nếu phiếu đã lưu, cần trừ tồn kho trước khi xóa
                if (trangThai == "ĐÃ LƯU")
                {
                    var chiTietParams = new[] { new SqlParameter("@phieuNhapId", phieuNhapId) };
                    var chiTietData = _db.GetDataTable(sqlGetChiTiet, chiTietParams);
                    
                    if (chiTietData != null && chiTietData.Rows.Count > 0)
                    {
                        // Trừ tồn kho
                        foreach (DataRow ctRow in chiTietData.Rows)
                        {
                            int nlId = Convert.ToInt32(ctRow["nl_id"]);
                            decimal soLuong = Convert.ToDecimal(ctRow["so_luong"]);
                            
                            XuatKho(chiNhanhId, nlId, soLuong);
                        }
                    }
                }

                // Xóa chi tiết
                var deleteChiTietParams = new[] { new SqlParameter("@phieuNhapId", phieuNhapId) };
                _db.ExecuteNonQuery(sqlDeleteChiTiet, deleteChiTietParams);

                // Xóa phiếu
                var deletePhieuParams = new[] { new SqlParameter("@phieuNhapId", phieuNhapId) };
                return _db.ExecuteNonQuery(sqlDeletePhieu, deletePhieuParams);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - XoaPhieuNhapKho: {ex.Message}", ex);
            }
        }

        // Xóa vĩnh viễn phiếu trả kho
        public int XoaPhieuTraKho(int phieuTraId)
        {
            const string sqlCheck = @"
                SELECT chi_nhanh_id, trang_thai
                FROM dbo.phieu_tra_kho
                WHERE phieu_tra_id = @phieuTraId;";

            const string sqlGetChiTiet = @"
                SELECT nl_id, so_luong_tra
                FROM dbo.phieu_tra_kho_ct
                WHERE phieu_tra_id = @phieuTraId;";

            const string sqlDeleteChiTiet = @"
                DELETE FROM dbo.phieu_tra_kho_ct
                WHERE phieu_tra_id = @phieuTraId;";

            const string sqlDeletePhieu = @"
                DELETE FROM dbo.phieu_tra_kho
                WHERE phieu_tra_id = @phieuTraId;";

            try
            {
                // Kiểm tra phiếu có tồn tại không
                var checkParams = new[] { new SqlParameter("@phieuTraId", phieuTraId) };
                var checkResult = _db.GetDataTable(sqlCheck, checkParams);
                
                if (checkResult == null || checkResult.Rows.Count == 0)
                {
                    throw new Exception("Không tìm thấy phiếu trả kho!");
                }

                var row = checkResult.Rows[0];
                int chiNhanhId = Convert.ToInt32(row["chi_nhanh_id"]);
                string trangThai = row["trang_thai"]?.ToString() ?? "";

                // Nếu phiếu đã lưu, cần cộng lại tồn kho (vì khi lưu đã trừ tồn kho)
                if (trangThai == "ĐÃ LƯU")
                {
                    var chiTietParams = new[] { new SqlParameter("@phieuTraId", phieuTraId) };
                    var chiTietData = _db.GetDataTable(sqlGetChiTiet, chiTietParams);
                    
                    if (chiTietData != null && chiTietData.Rows.Count > 0)
                    {
                        // Cộng lại tồn kho
                        foreach (DataRow ctRow in chiTietData.Rows)
                        {
                            int nlId = Convert.ToInt32(ctRow["nl_id"]);
                            decimal soLuongTra = Convert.ToDecimal(ctRow["so_luong_tra"]);
                            
                            NhapKho(chiNhanhId, nlId, soLuongTra);
                        }
                    }
                }

                // Xóa chi tiết
                var deleteChiTietParams = new[] { new SqlParameter("@phieuTraId", phieuTraId) };
                _db.ExecuteNonQuery(sqlDeleteChiTiet, deleteChiTietParams);

                // Xóa phiếu
                var deletePhieuParams = new[] { new SqlParameter("@phieuTraId", phieuTraId) };
                return _db.ExecuteNonQuery(sqlDeletePhieu, deletePhieuParams);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - XoaPhieuTraKho: {ex.Message}", ex);
            }
        }

        /// Lấy danh sách tồn kho với thông tin đầy đủ
        public DataTable GetDanhSachTonKho(int? chiNhanhId = null)
        {
            var sql = @"
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY nl.ten_nl) AS stt,
                    nl.nl_id,
                    nl.ma_nl,
                    nl.ten_nl,
                    nl.don_vi,
                    ISNULL(tk.sl_ton, 0) AS sl_ton,
                    ISNULL(tk.ton_toi_thieu, 0) AS ton_toi_thieu,
                    CASE 
                        WHEN ISNULL(tk.sl_ton, 0) = 0 THEN N'Hết hàng'
                        WHEN ISNULL(tk.sl_ton, 0) <= ISNULL(tk.ton_toi_thieu, 0) THEN N'Tồn thấp'
                        ELSE N'Đủ tồn'
                    END AS trang_thai
                FROM dbo.nguyen_lieu nl
                LEFT JOIN dbo.ton_kho tk ON tk.nl_id = nl.nl_id 
                    AND (@chiNhanhId IS NULL OR tk.chi_nhanh_id = @chiNhanhId)
                ORDER BY nl.ten_nl;";

            var parameters = new[]
            {
                new SqlParameter("@chiNhanhId", (object?)chiNhanhId ?? DBNull.Value)
            };

            return _db.GetDataTable(sql, parameters);
        }

        /// <summary>
        /// Cập nhật tồn tối thiểu cho nguyên liệu tại chi nhánh
        /// </summary>
        public int CapNhatTonToiThieu(int chiNhanhId, int nlId, decimal tonToiThieu)
        {
            const string sql = @"
                IF EXISTS (SELECT 1 FROM dbo.ton_kho WHERE chi_nhanh_id = @cn AND nl_id = @nl)
                    UPDATE dbo.ton_kho 
                    SET ton_toi_thieu = @tonToiThieu 
                    WHERE chi_nhanh_id = @cn AND nl_id = @nl;
                ELSE
                    INSERT INTO dbo.ton_kho (chi_nhanh_id, nl_id, sl_ton, ton_toi_thieu)
                    VALUES (@cn, @nl, 0, @tonToiThieu);";

            var parameters = new[]
            {
                new SqlParameter("@cn", chiNhanhId),
                new SqlParameter("@nl", nlId),
                new SqlParameter("@tonToiThieu", tonToiThieu)
            };

            try
            {
                return _db.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - CapNhatTonToiThieu: {ex.Message}", ex);
            }
        }

        // Lấy lịch sử nhập/trả kho
        public DataTable GetLichSuNhapTra(int chiNhanhId, DateTime? tuNgay = null, DateTime? denNgay = null,
            string loaiPhieu = null, string keyword = null)
        {
            try
            {
                var sql = new StringBuilder();
                var paramList = new List<SqlParameter>
                {
                    new SqlParameter("@chiNhanhId", chiNhanhId)
                };

                // điều kiện WHERE cho nhập kho
                var whereNhap = new List<string> { "pnk.chi_nhanh_id = @chiNhanhId" };
                if (tuNgay.HasValue)
                {
                    whereNhap.Add("CAST(pnk.ngay_nhap AS DATE) >= @tuNgay");
                    paramList.Add(new SqlParameter("@tuNgay", tuNgay.Value.Date));
                }
                if (denNgay.HasValue)
                {
                    whereNhap.Add("CAST(pnk.ngay_nhap AS DATE) <= @denNgay");
                    if (!paramList.Any(p => p.ParameterName == "@denNgay"))
                        paramList.Add(new SqlParameter("@denNgay", denNgay.Value.Date));
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    whereNhap.Add("pnk.nhan_vien_nhap LIKE @keywordNhap");
                    paramList.Add(new SqlParameter("@keywordNhap", $"%{keyword}%"));
                }

                // điều kiện WHERE cho trả kho
                var whereTra = new List<string> { "ptk.chi_nhanh_id = @chiNhanhId" };
                if (tuNgay.HasValue)
                {
                    whereTra.Add("CAST(ptk.ngay_tra AS DATE) >= @tuNgay");
                }
                if (denNgay.HasValue)
                {
                    whereTra.Add("CAST(ptk.ngay_tra AS DATE) <= @denNgay");
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    whereTra.Add("ptk.nhan_vien_tra LIKE @keywordTra");
                    paramList.Add(new SqlParameter("@keywordTra", $"%{keyword}%"));
                }

                // Lọc theo loại phiếu
                if (!string.IsNullOrWhiteSpace(loaiPhieu))
                {
                    if (loaiPhieu == "NHAP")
                    {
                        sql.AppendLine($@"
                            SELECT 
                                N'Nhập kho' AS loai_phieu,
                                CAST(pnk.ngay_nhap AS DATE) AS ngay,
                                CAST(pnk.gio_nhap AS TIME) AS gio,
                                pnk.nhan_vien_nhap AS nhan_vien,
                                pnk.trang_thai,
                                pnk.ghi_chu,
                                pnk.phieu_nhap_id AS phieu_id
                            FROM dbo.phieu_nhap_kho pnk
                            WHERE {string.Join(" AND ", whereNhap)}
                            ORDER BY ngay DESC, gio DESC
                        ");
                    }
                    else if (loaiPhieu == "TRA")
                    {
                        sql.AppendLine($@"
                            SELECT 
                                N'Trả kho' AS loai_phieu,
                                CAST(ptk.ngay_tra AS DATE) AS ngay,
                                CAST(ptk.gio_tra AS TIME) AS gio,
                                ptk.nhan_vien_tra AS nhan_vien,
                                ptk.trang_thai,
                                ptk.ghi_chu,
                                ptk.phieu_tra_id AS phieu_id
                            FROM dbo.phieu_tra_kho ptk
                            WHERE {string.Join(" AND ", whereTra)}
                            ORDER BY ngay DESC, gio DESC
                        ");
                    }
                }
                else
                {
                    // Lấy cả hai loại
                    sql.AppendLine($@"
                        SELECT 
                            N'Nhập kho' AS loai_phieu,
                            CAST(pnk.ngay_nhap AS DATE) AS ngay,
                            CAST(pnk.gio_nhap AS TIME) AS gio,
                            pnk.nhan_vien_nhap AS nhan_vien,
                            pnk.trang_thai,
                            pnk.ghi_chu,
                            pnk.phieu_nhap_id AS phieu_id
                        FROM dbo.phieu_nhap_kho pnk
                        WHERE {string.Join(" AND ", whereNhap)}
                        
                        UNION ALL
                        
                        SELECT 
                            N'Trả kho' AS loai_phieu,
                            CAST(ptk.ngay_tra AS DATE) AS ngay,
                            CAST(ptk.gio_tra AS TIME) AS gio,
                            ptk.nhan_vien_tra AS nhan_vien,
                            ptk.trang_thai,
                            ptk.ghi_chu,
                            ptk.phieu_tra_id AS phieu_id
                        FROM dbo.phieu_tra_kho ptk
                        WHERE {string.Join(" AND ", whereTra)}
                        
                        ORDER BY ngay DESC, gio DESC
                    ");
                }

                return _db.GetDataTable(sql.ToString(), paramList.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetLichSuNhapTra: {ex.Message}", ex);
            }
        }

        // Lấy chi tiết phiếu nhập kho
        public DataTable GetChiTietPhieuNhap(int phieuNhapId)
        {
            const string sql = @"
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY nl.ten_nl) AS stt,
                    nl.ma_nl,
                    nl.ten_nl,
                    ct.so_luong,
                    ct.don_vi,
                    ct.ghi_chu
                FROM dbo.phieu_nhap_kho_ct ct
                INNER JOIN dbo.nguyen_lieu nl ON nl.nl_id = ct.nl_id
                WHERE ct.phieu_nhap_id = @phieuNhapId
                ORDER BY nl.ten_nl;";

            var parameters = new[]
            {
                new SqlParameter("@phieuNhapId", phieuNhapId)
            };

            try
            {
                return _db.GetDataTable(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetChiTietPhieuNhap: {ex.Message}", ex);
            }
        }

        // Lấy chi tiết phiếu trả kho
        public DataTable GetChiTietPhieuTra(int phieuTraId)
        {
            const string sql = @"
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY nl.ten_nl) AS stt,
                    nl.ma_nl,
                    nl.ten_nl,
                    ct.so_luong_tra,
                    ct.so_luong_ton,
                    ct.so_luong_con_lai,
                    ct.don_vi,
                    ct.ghi_chu
                FROM dbo.phieu_tra_kho_ct ct
                INNER JOIN dbo.nguyen_lieu nl ON nl.nl_id = ct.nl_id
                WHERE ct.phieu_tra_id = @phieuTraId
                ORDER BY nl.ten_nl;";

            var parameters = new[]
            {
                new SqlParameter("@phieuTraId", phieuTraId)
            };

            try
            {
                return _db.GetDataTable(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetChiTietPhieuTra: {ex.Message}", ex);
            }
        }

        // Lấy thông tin phiếu nhập kho
        public DataRow GetThongTinPhieuNhap(int phieuNhapId)
        {
            const string sql = @"
                SELECT 
                    phieu_nhap_id,
                    chi_nhanh_id,
                    ngay_nhap,
                    gio_nhap,
                    nhan_vien_nhap,
                    ghi_chu,
                    trang_thai
                FROM dbo.phieu_nhap_kho
                WHERE phieu_nhap_id = @phieuNhapId;";

            var parameters = new[]
            {
                new SqlParameter("@phieuNhapId", phieuNhapId)
            };

            try
            {
                var dt = _db.GetDataTable(sql, parameters);
                return (dt != null && dt.Rows.Count > 0) ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetThongTinPhieuNhap: {ex.Message}", ex);
            }
        }

        // Lấy thông tin phiếu trả kho
        public DataRow GetThongTinPhieuTra(int phieuTraId)
        {
            const string sql = @"
                SELECT 
                    phieu_tra_id,
                    chi_nhanh_id,
                    ngay_tra,
                    gio_tra,
                    nhan_vien_tra,
                    ghi_chu,
                    trang_thai
                FROM dbo.phieu_tra_kho
                WHERE phieu_tra_id = @phieuTraId;";

            var parameters = new[]
            {
                new SqlParameter("@phieuTraId", phieuTraId)
            };

            try
            {
                var dt = _db.GetDataTable(sql, parameters);
                return (dt != null && dt.Rows.Count > 0) ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi DAL - GetThongTinPhieuTra: {ex.Message}", ex);
            }
        }
    }

    /// Class hỗ trợ cho chi tiết phiếu nhập kho
    public class PhieuNhapKhoChiTiet
    {
        public int NlId { get; set; }
        public decimal SoLuong { get; set; }
        public string DonVi { get; set; }
        public string GhiChu { get; set; }
    }

    /// Class hỗ trợ cho chi tiết phiếu trả kho
    public class PhieuTraKhoChiTiet
    {
        public int NlId { get; set; }
        public decimal SoLuongTra { get; set; }
        public decimal SoLuongTon { get; set; }
        public decimal SoLuongConLai { get; set; }
        public string DonVi { get; set; }
        public string GhiChu { get; set; }
    }
}
