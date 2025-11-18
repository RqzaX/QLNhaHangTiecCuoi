using Microsoft.Data.SqlClient;
using System.Data;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.DAL
{
    public class VaiTroDAL
    {
        public DatabaseHelper _dbHelper;

        public VaiTroDAL(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public DataTable LoadData()
        {
            string query = @"
                SELECT 
                    vt.vai_tro_id,
                    vt.ma,
                    vt.ten,
                    vt.mo_ta,
                    COUNT(DISTINCT ndvt.nguoi_dung_id) as so_nguoi_dung
                FROM dbo.vai_tro vt
                LEFT JOIN dbo.nguoi_dung_vai_tro ndvt ON vt.vai_tro_id = ndvt.vai_tro_id
                GROUP BY vt.vai_tro_id, vt.ma, vt.ten, vt.mo_ta
                ORDER BY vt.ten";

            return _dbHelper.GetDataTable(query);
        }

        public DataRow GetById(int vaiTroId)
        {
            string query = @"
                SELECT vai_tro_id, ma, ten, mo_ta
                FROM dbo.vai_tro
                WHERE vai_tro_id = @vaiTroId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@vaiTroId", vaiTroId)
            };

            DataTable dt = _dbHelper.GetDataTable(query, parameters);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

        public bool Insert(string ma, string ten, string moTa)
        {
            string query = @"
                INSERT INTO dbo.vai_tro (ma, ten, mo_ta)
                VALUES (@ma, @ten, @moTa)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ma", ma),
                new SqlParameter("@ten", ten),
                new SqlParameter("@moTa", string.IsNullOrEmpty(moTa) ? DBNull.Value : moTa)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Update(int vaiTroId, string ma, string ten, string moTa)
        {
            string query = @"
                UPDATE dbo.vai_tro
                SET ma = @ma, ten = @ten, mo_ta = @moTa
                WHERE vai_tro_id = @vaiTroId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@vaiTroId", vaiTroId),
                new SqlParameter("@ma", ma),
                new SqlParameter("@ten", ten),
                new SqlParameter("@moTa", string.IsNullOrEmpty(moTa) ? DBNull.Value : moTa)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int vaiTroId)
        {
            // Xóa các quan hệ trước
            string deleteRelationQuery = @"
                DELETE FROM dbo.nguoi_dung_vai_tro
                WHERE vai_tro_id = @vaiTroId";

            SqlParameter[] relationParams = new SqlParameter[]
            {
                new SqlParameter("@vaiTroId", vaiTroId)
            };

            _dbHelper.ExecuteNonQuery(deleteRelationQuery, relationParams);

            // Xóa vai trò
            string query = @"
                DELETE FROM dbo.vai_tro
                WHERE vai_tro_id = @vaiTroId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@vaiTroId", vaiTroId)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}

