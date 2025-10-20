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
    public class MonAnDAL
    {
        private readonly DatabaseHelper _dbHelper;

        public MonAnDAL(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public DataTable LayTatCaMonAn()
        {
            string query = @"
                SELECT mon_id, ma_mon, ten_mon, nhom, don_vi_tinh, don_gia, dang_ban
                FROM dbo.mon_an
                WHERE dang_ban = 1
                ORDER BY ten_mon";

            return _dbHelper.GetDataTable(query);
        }
        public DataTable LayMonAnTheoNhom(string nhom)
        {
            string query = @"
                SELECT mon_id, ma_mon, ten_mon, nhom, don_vi_tinh, don_gia, dang_ban
                FROM dbo.mon_an
                WHERE dang_ban = 1 AND nhom = @nhom
                ORDER BY ten_mon";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nhom", nhom)
            };

            return _dbHelper.GetDataTable(query, parameters);
        }
        public DataTable LayDanhSachNhomMon()
        {
            string query = @"
                SELECT DISTINCT nhom
                FROM dbo.mon_an
                WHERE dang_ban = 1 AND nhom IS NOT NULL
                ORDER BY nhom";

            return _dbHelper.GetDataTable(query);
        }
        public int ThemMonAn(string maMon, string tenMon, string nhom, string donViTinh, decimal donGia)
        {
            string query = @"
                INSERT INTO dbo.mon_an (ma_mon, ten_mon, nhom, don_vi_tinh, don_gia, dang_ban)
                VALUES (@maMon, @tenMon, @nhom, @donViTinh, @donGia, 1)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@maMon", maMon),
                new SqlParameter("@tenMon", tenMon),
                new SqlParameter("@nhom", nhom ?? (object)DBNull.Value),
                new SqlParameter("@donViTinh", donViTinh),
                new SqlParameter("@donGia", donGia)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters);
        }
        public int CapNhatMonAn(int monId, string tenMon, string nhom, string donViTinh, decimal donGia)
        {
            string query = @"
                UPDATE dbo.mon_an
                SET ten_mon = @tenMon, nhom = @nhom, don_vi_tinh = @donViTinh, don_gia = @donGia
                WHERE mon_id = @monId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@monId", monId),
                new SqlParameter("@tenMon", tenMon),
                new SqlParameter("@nhom", nhom ?? (object)DBNull.Value),
                new SqlParameter("@donViTinh", donViTinh),
                new SqlParameter("@donGia", donGia)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters);
        }
        public int XoaMonAn(int monId)
        {
            string query = @"
                UPDATE dbo.mon_an
                SET dang_ban = 0
                WHERE mon_id = @monId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@monId", monId)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters);
        }
    }
}
