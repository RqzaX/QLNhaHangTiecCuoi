using DAL;
using QLNhaHangTiecCuoi.Share;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MonAnBLL
    {
        private readonly MonAnDAL _dal;

        public MonAnBLL(DatabaseHelper dbHelper)
        {
            _dal = new MonAnDAL(dbHelper);
        }
        public DataTable LayTatCaMonAn()
        {
            try
            {
                return _dal.LayTatCaMonAn();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách món ăn: {ex.Message}");
            }
        }
        public DataTable LayMonAnTheoNhom(string nhom)
        {
            if (string.IsNullOrWhiteSpace(nhom))
                return LayTatCaMonAn();

            try
            {
                return _dal.LayMonAnTheoNhom(nhom);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách món ăn: {ex.Message}");
            }
        }
        public DataTable LayDanhSachNhomMon()
        {
            try
            {
                return _dal.LayDanhSachNhomMon();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách nhóm: {ex.Message}");
            }
        }
        public (bool success, string message) ThemMonAn(string maMon, string tenMon, string nhom, string donViTinh, decimal donGia)
        {
            if (string.IsNullOrWhiteSpace(maMon))
                return (false, "Mã món không được để trống!");

            if (string.IsNullOrWhiteSpace(tenMon))
                return (false, "Tên món không được để trống!");

            if (donGia < 0)
                return (false, "Đơn giá không được âm!");

            try
            {
                int result = _dal.ThemMonAn(maMon, tenMon, nhom, donViTinh, donGia);
                return result > 0 ? (true, "Thêm thành công!") : (false, "Thêm thất bại!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }
        public (bool success, string message) CapNhatMonAn(int monId, string tenMon, string nhom, string donViTinh, decimal donGia)
        {
            if (monId <= 0)
                return (false, "ID món không hợp lệ!");

            if (string.IsNullOrWhiteSpace(tenMon))
                return (false, "Tên món không được để trống!");

            if (donGia < 0)
                return (false, "Đơn giá không được âm!");

            try
            {
                int result = _dal.CapNhatMonAn(monId, tenMon, nhom, donViTinh, donGia);
                return result > 0 ? (true, "Cập nhật thành công!") : (false, "Cập nhật thất bại!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }
        public (bool success, string message) XoaMonAn(int monId)
        {
            if (monId <= 0)
                return (false, "ID món không hợp lệ!");

            try
            {
                int result = _dal.XoaMonAn(monId);
                return result > 0 ? (true, "Xóa thành công!") : (false, "Xóa thất bại!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }
        
        public DataTable TimKiemMonAn(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return LayTatCaMonAn();

            try
            {
                return _dal.TimKiemMonAn(searchText.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm món ăn: {ex.Message}");
            }
        }
        
        public DataTable TimKiemMonAnTheoNhom(string searchText, string nhom)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return LayMonAnTheoNhom(nhom);

            try
            {
                return _dal.TimKiemMonAnTheoNhom(searchText.Trim(), nhom);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm món ăn theo nhóm: {ex.Message}");
            }
        }
    }
}
