using System;
using System.Data;
using QLNhaHangTiecCuoi.Share;
using QLNhaHangTiecCuoi.DAL;

namespace QLNhaHangTiecCuoi.BLL
{
    public class KOTBLL
    {
        private readonly KOTDAL _kotDAL;

        public KOTBLL(DatabaseHelper dbHelper)
        {
            _kotDAL = new KOTDAL(dbHelper);
        }

        public DataTable LayDanhSachKOT(int chiNhanhId, string? status = null, string? department = null)
        {
            try
            {
                return _kotDAL.LayDanhSachKOT(chiNhanhId, status, department);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách KOT: {ex.Message}");
            }
        }

        public DataTable LayChiTietKOT(int kotId)
        {
            try
            {
                return _kotDAL.LayChiTietKOT(kotId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy chi tiết KOT: {ex.Message}");
            }
        }

        public int TaoKOT(int banId, int chiNhanhId, int nguoiDungId, string loaiKot, string? ghiChu = null, bool uuTien = false)
        {
            try
            {
                return _kotDAL.TaoKOT(banId, chiNhanhId, nguoiDungId, loaiKot, ghiChu, uuTien);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Tạo KOT: {ex.Message}");
            }
        }

        public bool ThemChiTietKOT(int kotId, int monId, int soLuong, string? ghiChu = null)
        {
            try
            {
                return _kotDAL.ThemChiTietKOT(kotId, monId, soLuong, ghiChu);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Thêm chi tiết KOT: {ex.Message}");
            }
        }

        public bool CapNhatTrangThaiKOT(int kotId, string trangThai)
        {
            try
            {
                return _kotDAL.CapNhatTrangThaiKOT(kotId, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật trạng thái KOT: {ex.Message}");
            }
        }

        public DataTable LayThongKeKOT(int chiNhanhId)
        {
            try
            {
                return _kotDAL.LayThongKeKOT(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thống kê KOT: {ex.Message}");
            }
        }

        public bool GuiDonXuongBep(int banId, int chiNhanhId, int nguoiDungId, List<OrderItem> orderItems, string loaiKot = "BẾP")
        {
            try
            {
                // Tạo KOT chính
                int kotId = TaoKOT(banId, chiNhanhId, nguoiDungId, loaiKot);

                // Thêm chi tiết từng món
                foreach (var item in orderItems)
                {
                    ThemChiTietKOT(kotId, item.MonId, item.Quantity, item.Notes);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Gửi đơn xuống bếp: {ex.Message}");
            }
        }

        public class OrderItem
        {
            public int MonId { get; set; }
            public int Quantity { get; set; }
            public string? Notes { get; set; }
        }
    }
}
