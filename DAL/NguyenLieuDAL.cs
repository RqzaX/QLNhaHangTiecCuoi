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
            INSERT INTO dbo.ton_kho(chi_nhanh_id, nl_id, sl_ton) VALUES(@cn, @nl, @sl);";
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
                    INSERT INTO dbo.ton_kho (chi_nhanh_id, nl_id, sl_ton)
                    VALUES (@chiNhanhId, @nlId, @soLuong);
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
                        INSERT INTO dbo.ton_kho (chi_nhanh_id, nl_id, sl_ton)
                        VALUES (@chiNhanhDichId, @nlId, @soLuong);
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
    }
}
