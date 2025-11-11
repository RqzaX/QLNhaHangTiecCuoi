using System;
using System.Data;
using QLNhaHangTiecCuoi.DAL;

namespace QLNhaHangTiecCuoi.BLL
{
    public class KhachHangBLL
    {
        private KhachHangDAL _khachHangDAL;

        public KhachHangBLL()
        {
            _khachHangDAL = new KhachHangDAL();
        }

        public DataTable TimKhachHangTheoSdt(string sdt)
        {
            try
            {
                return _khachHangDAL.TimKhachHangTheoSdt(sdt);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Tìm khách hàng theo SĐT: {ex.Message}");
            }
        }

        public int TaoKhachHang(string hoTen, string sdt, string email, string ghiChu,
            DateTime? ngaySinh = null, string hangCode = "MEM", decimal tongChiTieu = 0,
            int soLanDen = 0, int diem = 0)
        {
            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ tên không được để trống!");

            try
            {
                return _khachHangDAL.TaoKhachHang(hoTen, sdt, email, ghiChu, ngaySinh, hangCode, tongChiTieu, soLanDen, diem);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Tạo khách hàng: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhachHang()
        {
            try
            {
                return _khachHangDAL.LayDanhSachKhachHang();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách khách hàng: {ex.Message}");
            }
        }

        public DataTable LayThongTinKhachHang(int khachHangId)
        {
            try
            {
                return _khachHangDAL.LayThongTinKhachHang(khachHangId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin khách hàng: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhachHangChiTiet(string keyword = null, string hangCode = null)
        {
            try
            {
                return _khachHangDAL.LayDanhSachKhachHangChiTiet(keyword, hangCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách khách hàng chi tiết: {ex.Message}");
            }
        }

        public int DemTongSoKhachHang()
        {
            try
            {
                return _khachHangDAL.DemTongSoKhachHang();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Đếm tổng số khách hàng: {ex.Message}");
            }
        }

        public int DemKhachHangTheoHang(string hangCode)
        {
            try
            {
                return _khachHangDAL.DemKhachHangTheoHang(hangCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Đếm khách hàng theo hạng: {ex.Message}");
            }
        }

        public DataTable LayDanhSachHang()
        {
            try
            {
                return _khachHangDAL.LayDanhSachHang();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách hạng: {ex.Message}");
            }
        }


        public bool CapNhatKhachHang(int khachHangId, string hoTen, string sdt, string email, string ghiChu,
            DateTime? ngaySinh = null, string hangCode = "MEM", decimal tongChiTieu = 0,
            int soLanDen = 0, int diem = 0)
        {
            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ tên không được để trống!");

            try
            {
                return _khachHangDAL.CapNhatKhachHang(khachHangId, hoTen, sdt, email, ghiChu, ngaySinh, hangCode, tongChiTieu, soLanDen, diem);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật khách hàng: {ex.Message}");
            }
        }

        public bool XoaKhachHang(int khachHangId)
        {
            try
            {
                return _khachHangDAL.XoaKhachHang(khachHangId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Xóa khách hàng: {ex.Message}");
            }
        }
    }
}
