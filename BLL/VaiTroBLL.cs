using System;
using System.Data;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.BLL
{
    public class VaiTroBLL
    {
        private VaiTroDAL _dal;

        public VaiTroBLL(DatabaseHelper dbHelper)
        {
            _dal = new VaiTroDAL(dbHelper);
        }

        public DataTable LoadData()
        {
            try
            {
                return _dal.LoadData();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi load dữ liệu vai trò: " + ex.Message);
            }
        }

        public DataRow GetById(int vaiTroId)
        {
            if (vaiTroId <= 0)
                throw new Exception("ID vai trò không hợp lệ!");

            try
            {
                return _dal.GetById(vaiTroId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy vai trò: " + ex.Message);
            }
        }

        public bool Insert(string ma, string ten, string moTa)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Mã vai trò không được để trống!");

            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên vai trò không được để trống!");

            try
            {
                return _dal.Insert(ma, ten, moTa);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm vai trò: " + ex.Message);
            }
        }

        public bool Update(int vaiTroId, string ma, string ten, string moTa)
        {
            if (vaiTroId <= 0)
                throw new Exception("ID vai trò không hợp lệ!");

            if (string.IsNullOrWhiteSpace(ma))
                throw new Exception("Mã vai trò không được để trống!");

            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên vai trò không được để trống!");

            try
            {
                return _dal.Update(vaiTroId, ma, ten, moTa);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật vai trò: " + ex.Message);
            }
        }

        public bool Delete(int vaiTroId)
        {
            if (vaiTroId <= 0)
                throw new Exception("ID vai trò không hợp lệ!");

            // Kiểm tra xem có phải vai trò admin không
            DataRow row = GetById(vaiTroId);
            if (row != null)
            {
                string ma = row["ma"]?.ToString()?.ToLower() ?? "";
                string ten = row["ten"]?.ToString()?.ToLower() ?? "";
                
                if (ma == "admin" || ten == "quản trị" || ten == "admin")
                {
                    throw new Exception("Không thể xóa vai trò admin! Vai trò admin được bảo vệ và không thể xóa.");
                }
            }

            try
            {
                return _dal.Delete(vaiTroId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa vai trò: " + ex.Message);
            }
        }
    }
}

