using System;
using System.Data;
using DAL;
using QLNhaHangTiecCuoi.Share;

namespace BLL
{
    public class ChiNhanhBLL
    {
        private ChiNhanhDAL _chiNhanhDAL;

        public ChiNhanhBLL(DatabaseHelper dbHelper)
        {
            _chiNhanhDAL = new ChiNhanhDAL(dbHelper);
        }

        public DataTable LayDanhSachChiNhanh()
        {
            try
            {
                return _chiNhanhDAL.LayDanhSachChiNhanh();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách chi nhánh: {ex.Message}");
            }
        }

        public DataTable LayTatCaChiNhanh()
        {
            try
            {
                return _chiNhanhDAL.LayTatCaChiNhanh();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy tất cả chi nhánh: {ex.Message}");
            }
        }

        public DataTable LayChiNhanhTheoTrangThai(int? trangThai)
        {
            try
            {
                return _chiNhanhDAL.LayChiNhanhTheoTrangThai(trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy chi nhánh theo trạng thái: {ex.Message}");
            }
        }

        public DataTable TimKiemChiNhanh(string keyword, int? trangThai)
        {
            try
            {
                return _chiNhanhDAL.TimKiemChiNhanh(keyword, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Tìm kiếm chi nhánh: {ex.Message}");
            }
        }

        public DataTable LayChiNhanhById(int chiNhanhId)
        {
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            try
            {
                return _chiNhanhDAL.LayChiNhanhById(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy chi nhánh theo ID: {ex.Message}");
            }
        }

        public int ThemChiNhanh(string ten, string diaChi, string sdt, int trangThai = 1)
        {
            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên chi nhánh không được để trống!");

            // Validate trạng thái
            if (trangThai < 0 || trangThai > 1)
                throw new Exception("Trạng thái không hợp lệ! (0: Bảo trì, 1: Đang hoạt động)");

            try
            {
                return _chiNhanhDAL.ThemChiNhanh(ten, diaChi, sdt, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Thêm chi nhánh: {ex.Message}");
            }
        }

        public int DemSoBan(int chiNhanhId)
        {
            if (chiNhanhId <= 0)
                return 0;

            try
            {
                return _chiNhanhDAL.DemSoBan(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Đếm số bàn: {ex.Message}");
            }
        }

        public int DemSoSanh(int chiNhanhId)
        {
            if (chiNhanhId <= 0)
                return 0;

            try
            {
                return _chiNhanhDAL.DemSoSanh(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Đếm số sảnh: {ex.Message}");
            }
        }

        public int DemSoNhanVien(int chiNhanhId)
        {
            if (chiNhanhId <= 0)
                return 0;

            try
            {
                return _chiNhanhDAL.DemSoNhanVien(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Đếm số nhân viên: {ex.Message}");
            }
        }

        public bool CapNhatChiNhanh(int chiNhanhId, string ten, string diaChi, string sdt, int trangThai)
        {
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên chi nhánh không được để trống!");

            if (trangThai < 0 || trangThai > 1)
                throw new Exception("Trạng thái không hợp lệ! (0: Bảo trì, 1: Đang hoạt động)");

            try
            {
                return _chiNhanhDAL.CapNhatChiNhanh(chiNhanhId, ten, diaChi, sdt, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật chi nhánh: {ex.Message}");
            }
        }

        public bool XoaChiNhanh(int chiNhanhId)
        {
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            try
            {
                return _chiNhanhDAL.XoaChiNhanh(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Xóa chi nhánh: {ex.Message}");
            }
        }
    }
}
