using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.Share;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class VoucherDAL
    {
        private DatabaseHelper _dbHelper;

        public VoucherDAL()
        {
            _dbHelper = new DatabaseHelper();
        }

        public DataTable GetAll()
        {
            try
            {
                string query = @"
                    SELECT 
                        v.voucher_id AS 'ID',
                        v.code AS 'Code',
                        v.km_id AS 'KmId',
                        k.ten AS 'TenKM',
                        k.gia_tri AS 'GiaTri',
                        k.hinh_thuc AS 'HinhThuc',
                        k.tg_bat_dau AS 'NgayPhatHanh',
                        ISNULL(v.han_dung, k.tg_ket_thuc) AS 'NgayHetHan',
                        v.so_lan AS 'SoLan',
                        v.da_dung AS 'DaDung',
                        v.han_dung AS 'HanDung',
                        k.tg_bat_dau AS 'TgBatDau',
                        k.tg_ket_thuc AS 'TgKetThuc',
                        -- Lấy khách hàng đầu tiên sử dụng voucher (nếu có)
                        -- Lấy từ dat_ban (nếu hoa_don loại NHAHANG) hoặc dat_sanh (nếu loại TIECCUOI)
                        (SELECT TOP 1 kh.ho_ten 
                         FROM dbo.hoa_don_km hdkm
                         INNER JOIN dbo.hoa_don hd ON hd.hoa_don_id = hdkm.hoa_don_id
                         LEFT JOIN dbo.dat_ban db ON hd.loai = N'NHAHANG' AND hd.tham_chieu_id = db.dat_ban_id
                         LEFT JOIN dbo.hop_dong hd_hd ON hd.loai = N'TIECCUOI' AND hd.tham_chieu_id = hd_hd.hop_dong_id
                         LEFT JOIN dbo.dat_sanh ds ON hd_hd.dat_sanh_id = ds.dat_sanh_id
                         LEFT JOIN dbo.khach_hang kh ON (db.khach_hang_id = kh.khach_hang_id OR ds.khach_hang_id = kh.khach_hang_id)
                         WHERE hdkm.voucher_id = v.voucher_id
                         ORDER BY hd.ngay_lap DESC) AS 'KhachHang',
                        -- Đơn tối thiểu mặc định (có thể lấy từ CTKM sau)
                        0 AS 'DonToiThieu'
                    FROM dbo.voucher v
                    INNER JOIN dbo.chuong_trinh_km k ON v.km_id = k.km_id
                    ORDER BY v.voucher_id DESC";

                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL GetAll: " + ex.Message);
            }
        }

        public DataTable GetChuongTrinhKM()
        {
            try
            {
                string query = @"
                    SELECT 
                        km_id,
                        ma_km,
                        ten
                    FROM dbo.chuong_trinh_km
                    ORDER BY ten";

                return _dbHelper.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL GetChuongTrinhKM: " + ex.Message);
            }
        }

        public bool Insert(int kmId, string code, int soLan, DateTime? hanDung)
        {
            try
            {
                string query = @"
                    INSERT INTO dbo.voucher 
                    (km_id, code, so_lan, da_dung, han_dung)
                    VALUES 
                    (@KmId, @Code, @SoLan, 0, @HanDung)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@KmId", kmId),
                    new SqlParameter("@Code", code),
                    new SqlParameter("@SoLan", soLan),
                    new SqlParameter("@HanDung", hanDung ?? (object)DBNull.Value)
                };

                int result = _dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL Insert: " + ex.Message);
            }
        }

        public bool Update(int voucherId, int kmId, string code, int soLan, DateTime? hanDung, int? daDung = null)
        {
            try
            {
                string query = @"
                    UPDATE dbo.voucher
                    SET 
                        km_id = @KmId,
                        code = @Code,
                        so_lan = @SoLan,
                        han_dung = @HanDung" +
                        (daDung.HasValue ? ",\n                        da_dung = @DaDung" : "") + @"
                    WHERE voucher_id = @VoucherId";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@VoucherId", voucherId),
                    new SqlParameter("@KmId", kmId),
                    new SqlParameter("@Code", code),
                    new SqlParameter("@SoLan", soLan),
                    new SqlParameter("@HanDung", hanDung ?? (object)DBNull.Value)
                };

                if (daDung.HasValue)
                {
                    parameters.Add(new SqlParameter("@DaDung", daDung.Value));
                }

                int result = _dbHelper.ExecuteNonQuery(query, parameters.ToArray());
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL Update: " + ex.Message);
            }
        }

        public bool Delete(int voucherId)
        {
            try
            {
                string query = @"DELETE FROM dbo.voucher WHERE voucher_id = @VoucherId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@VoucherId", voucherId)
                };

                int result = _dbHelper.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL Delete: " + ex.Message);
            }
        }

        public bool CodeExists(string code, int excludeVoucherId = 0)
        {
            try
            {
                string query = @"
                    SELECT COUNT(*) FROM dbo.voucher 
                    WHERE code = @Code 
                    AND (@ExcludeVoucherId = 0 OR voucher_id != @ExcludeVoucherId)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Code", code),
                    new SqlParameter("@ExcludeVoucherId", excludeVoucherId)
                };

                object result = _dbHelper.ExecuteScalar(query, parameters);
                return (int)result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL CodeExists: " + ex.Message);
            }
        }

        public DataRow GetById(int voucherId)
        {
            try
            {
                string query = @"
                    SELECT 
                        v.voucher_id AS 'ID',
                        v.code AS 'Code',
                        v.km_id AS 'KmId',
                        k.ten AS 'TenKM',
                        k.gia_tri AS 'GiaTri',
                        k.hinh_thuc AS 'HinhThuc',
                        k.tg_bat_dau AS 'NgayPhatHanh',
                        ISNULL(v.han_dung, k.tg_ket_thuc) AS 'NgayHetHan',
                        v.so_lan AS 'SoLan',
                        v.da_dung AS 'DaDung',
                        v.han_dung AS 'HanDung',
                        k.tg_bat_dau AS 'TgBatDau',
                        k.tg_ket_thuc AS 'TgKetThuc'
                    FROM dbo.voucher v
                    INNER JOIN dbo.chuong_trinh_km k ON v.km_id = k.km_id
                    WHERE v.voucher_id = @VoucherId";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@VoucherId", voucherId)
                };

                DataTable dt = _dbHelper.GetDataTable(query, parameters);
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DAL GetById: " + ex.Message);
            }
        }
    }
}
