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

        public int TaoKhachHang(string hoTen, string sdt, string email, string ghiChu)
        {
            try
            {
                return _khachHangDAL.TaoKhachHang(hoTen, sdt, email, ghiChu);
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
    }
}
