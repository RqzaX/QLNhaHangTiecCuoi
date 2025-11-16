using System;
using System.Data;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.BLL
{
    public class SanhBLL
    {
        private readonly SanhDAL _sanhDAL;

        public SanhBLL(DatabaseHelper dbHelper)
        {
            _sanhDAL = new SanhDAL(dbHelper);
        }

        public DataTable LayDanhSachSanhTheoChiNhanh(int chiNhanhId)
        {
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            try
            {
                return _sanhDAL.LayDanhSachSanhTheoChiNhanh(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách sảnh: {ex.Message}");
            }
        }

        public DataTable LayThongTinSanh(int sanhId)
        {
            if (sanhId <= 0)
                throw new Exception("ID sảnh không hợp lệ!");

            try
            {
                return _sanhDAL.LayThongTinSanh(sanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin sảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật thông tin sảnh
        /// </summary>
        public bool CapNhatSanh(int sanhId, string tenSanh, int sucChua, decimal phiThueCb)
        {
            if (sanhId <= 0)
                throw new Exception("ID sảnh không hợp lệ!");

            if (string.IsNullOrWhiteSpace(tenSanh))
                throw new Exception("Tên sảnh không được để trống!");

            if (sucChua <= 0)
                throw new Exception("Sức chứa phải lớn hơn 0!");

            if (phiThueCb < 0)
                throw new Exception("Phí thuê cơ bản không được âm!");

            try
            {
                return _sanhDAL.CapNhatSanh(sanhId, tenSanh, sucChua, phiThueCb);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật sảnh: {ex.Message}");
            }
        }
        public bool XoaSanh(int sanhId)
        {
            if (sanhId <= 0)
                throw new Exception("ID sảnh không hợp lệ!");

            try
            {
                return _sanhDAL.XoaSanh(sanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Xóa sảnh: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm sảnh mới
        /// </summary>
        public int ThemSanh(int chiNhanhId, string tenSanh, int sucChua, decimal phiThueCb)
        {
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            if (string.IsNullOrWhiteSpace(tenSanh))
                throw new Exception("Tên sảnh không được để trống!");

            if (sucChua <= 0)
                throw new Exception("Sức chứa phải lớn hơn 0!");

            if (phiThueCb < 0)
                throw new Exception("Phí thuê cơ bản không được âm!");

            try
            {
                return _sanhDAL.ThemSanh(chiNhanhId, tenSanh, sucChua, phiThueCb);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Thêm sảnh: {ex.Message}");
            }
        }
    }
}

