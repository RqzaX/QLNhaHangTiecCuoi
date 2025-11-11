using System;
using System.Data;
using QLNhaHangTiecCuoi.DAL;

namespace QLNhaHangTiecCuoi.BLL
{
    public class KhuVucBLL
    {
        private KhuVucDAL _khuVucDAL;

        public KhuVucBLL()
        {
            _khuVucDAL = new KhuVucDAL();
        }

        public DataTable LayDanhSachKhuVuc(int chiNhanhId)
        {
            try
            {
                return _khuVucDAL.LayDanhSachKhuVuc(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách khu vực: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhuVucVoiSoBan(int? chiNhanhId = null)
        {
            try
            {
                return _khuVucDAL.LayDanhSachKhuVucVoiSoBan(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách khu vực với số bàn: {ex.Message}");
            }
        }

        public DataRow LayKhuVucById(int khuVucId)
        {
            try
            {
                if (khuVucId <= 0)
                    throw new Exception("ID khu vực không hợp lệ");

                return _khuVucDAL.LayKhuVucById(khuVucId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin khu vực: {ex.Message}");
            }
        }

        public bool CapNhatKhuVuc(int khuVucId, string tenKhuVuc, string moTa)
        {
            try
            {
                if (khuVucId <= 0)
                    throw new Exception("ID khu vực không hợp lệ");

                if (string.IsNullOrWhiteSpace(tenKhuVuc))
                    throw new Exception("Tên khu vực không được để trống");

                return _khuVucDAL.CapNhatKhuVuc(khuVucId, tenKhuVuc.Trim(), moTa?.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật khu vực: {ex.Message}");
            }
        }

        public int ThemKhuVuc(int chiNhanhId, string tenKhuVuc, string moTa)
        {
            try
            {
                if (chiNhanhId <= 0)
                    throw new Exception("ID chi nhánh không hợp lệ");

                if (string.IsNullOrWhiteSpace(tenKhuVuc))
                    throw new Exception("Tên khu vực không được để trống");

                return _khuVucDAL.ThemKhuVuc(chiNhanhId, tenKhuVuc.Trim(), moTa?.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Thêm khu vực: {ex.Message}");
            }
        }

        public bool XoaKhuVuc(int khuVucId)
        {
            try
            {
                if (khuVucId <= 0)
                    throw new Exception("ID khu vực không hợp lệ");

                return _khuVucDAL.XoaKhuVuc(khuVucId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Xóa khu vực: {ex.Message}");
            }
        }
    }
}
