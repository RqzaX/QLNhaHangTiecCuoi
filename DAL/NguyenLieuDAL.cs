using Microsoft.Data.SqlClient;
using System.Data;
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
    ISNULL(tk.sl_ton, 0) AS sl_ton
FROM dbo.nguyen_lieu nl
LEFT JOIN dbo.ton_kho tk
  ON tk.nl_id = nl.nl_id
 AND (@cn IS NULL OR tk.chi_nhanh_id = @cn)   -- <<< lọc theo chi nhánh ngay trong JOIN
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

        /// Lấy danh sách tồn kho với thông tin đầy đủ
        public DataTable GetDanhSachTonKho(int? chiNhanhId = null)
        {
            var sql = @"
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY nl.ten_nl) AS stt,
                    nl.nl_id,
                    nl.ma_nl,
                    nl.ten_nl,
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
