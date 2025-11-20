using System;
using System.Data;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.BLL
{
    public class BanBLL
    {
        private readonly BanDAL _banDAL;

        public BanBLL(DatabaseHelper dbHelper)
        {
            _banDAL = new BanDAL(dbHelper);
        }

        public DataTable LayDanhSachBan(int chiNhanhId, int? khuVucId = null)
        {
            try
            {
                return _banDAL.LayDanhSachBanTheoChiNhanh(chiNhanhId, khuVucId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách bàn: {ex.Message}");
            }
        }

        public DataTable LayDanhSachBanTheoKhuVuc(int? khuVucId)
        {
            try
            {
                return _banDAL.LayDanhSachBanTheoKhuVuc(khuVucId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách bàn theo khu vực: {ex.Message}");
            }
        }

        public DataTable LayDanhSachBanTheoChiNhanh(int chiNhanhId, int? khuVucId = null)
        {
            try
            {
                return _banDAL.LayDanhSachBanTheoChiNhanh(chiNhanhId, khuVucId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách bàn theo chi nhánh: {ex.Message}");
            }
        }

        public DataTable LayThongKeBan()
        {
            try
            {
                return _banDAL.LayThongKeBan();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thống kê bàn: {ex.Message}");
            }
        }

        public DataTable LayThongKeBanTheoChiNhanh(int chiNhanhId)
        {
            try
            {
                return _banDAL.LayThongKeBanTheoChiNhanh(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thống kê bàn theo chi nhánh: {ex.Message}");
            }
        }

        public (int SoBanDangPhucVu, int TongKhach, int TongBan) GetBanDangPhucVuInfo(int chiNhanhId)
        {
            try
            {
                return _banDAL.GetBanDangPhucVuInfo(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin bàn đang phục vụ: {ex.Message}");
            }
        }

        public DataTable LayThongKeBanTheoKhuVuc(int? khuVucId)
        {
            try
            {
                return _banDAL.LayThongKeBanTheoKhuVuc(khuVucId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thống kê bàn theo khu vực: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhuVuc()
        {
            try
            {
                return _banDAL.LayDanhSachKhuVuc();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách khu vực: {ex.Message}");
            }
        }

        public DataTable LayDanhSachKhuVucTheoChiNhanh(int chiNhanhId)
        {
            try
            {
                return _banDAL.LayDanhSachKhuVucTheoChiNhanh(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách khu vực theo chi nhánh: {ex.Message}");
            }
        }

        public bool CapNhatBan(int banId, string soBan, int sucChua, int? khuVucId, string trangThai)
        {
            try
            {
                return _banDAL.CapNhatBan(banId, soBan, sucChua, khuVucId, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật bàn: {ex.Message}");
            }
        }

        public int ThemBan(int chiNhanhId, string soBan, int sucChua, int? khuVucId, string trangThai)
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(soBan))
                {
                    throw new Exception("Số bàn không được để trống!");
                }
                if (sucChua <= 0)
                {
                    throw new Exception("Sức chứa phải lớn hơn 0!");
                }
                if (string.IsNullOrWhiteSpace(trangThai))
                {
                    trangThai = "TRỐNG";
                }

                return _banDAL.ThemBan(chiNhanhId, soBan, sucChua, khuVucId, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Thêm bàn: {ex.Message}");
            }
        }

        public bool XoaBan(int banId)
        {
            try
            {
                return _banDAL.XoaBan(banId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Xóa bàn: {ex.Message}");
            }
        }

        public bool CapNhatTrangThaiBan(int banId, string trangThai)
        {
            try
            {
                return _banDAL.CapNhatTrangThaiBan(banId, trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật trạng thái bàn: {ex.Message}");
            }
        }

        public DataTable LayThongTinDatBan(int banId)
        {
            try
            {
                return _banDAL.LayThongTinDatBan(banId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin đặt bàn: {ex.Message}");
            }
        }

        public DataTable LayOrderHienTai(int banId)
        {
            try
            {
                return _banDAL.LayOrderHienTai(banId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy order hiện tại: {ex.Message}");
            }
        }

        public bool TaoDatBan(int chiNhanhId, int banId, int khachHangId, DateTime ngayGio, int soKhach, string ghiChu)
        {
            try
            {
                return _banDAL.TaoDatBan(chiNhanhId, banId, khachHangId, ngayGio, soKhach, ghiChu);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Tạo đặt bàn: {ex.Message}");
            }
        }

        public DataTable LayThongTinBan(int banId)
        {
            try
            {
                return _banDAL.LayThongTinBan(banId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin bàn: {ex.Message}");
            }
        }

        public DataTable LayDanhSachDatBan(int chiNhanhId)
        {
            try
            {
                return _banDAL.LayDanhSachDatBan(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách đặt bàn: {ex.Message}");
            }
        }

        public DataTable LayDanhSachDatBanHomNay(int chiNhanhId)
        {
            try
            {
                return _banDAL.LayDanhSachDatBanHomNay(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách đặt bàn hôm nay: {ex.Message}");
            }
        }

        public DataTable LayThongKeDatBan(int chiNhanhId)
        {
            try
            {
                return _banDAL.LayThongKeDatBan(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thống kê đặt bàn: {ex.Message}");
            }
        }

        public bool HuyDatBan(string maDatBan)
        {
            try
            {
                return _banDAL.HuyDatBan(maDatBan);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Hủy đặt bàn: {ex.Message}");
            }
        }

        public bool CapNhatDatBan(string maDatBan, int khachHangId, int banId, int soKhach, DateTime ngayDat, string ghiChu)
        {
            try
            {
                return _banDAL.CapNhatDatBan(maDatBan, khachHangId, banId, soKhach, ngayDat, ghiChu);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật đặt bàn: {ex.Message}");
            }
        }

        public bool XacNhanDaDen(string maDatBan)
        {
            try
            {
                return _banDAL.XacNhanDaDen(maDatBan);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Xác nhận đã đến: {ex.Message}");
            }
        }

        public bool XacNhanDatBan(string maDatBan)
        {
            try
            {
                return _banDAL.XacNhanDatBan(maDatBan);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Xác nhận đặt bàn: {ex.Message}");
            }
        }

        public int CapNhatTrangThaiTreGio()
        {
            try
            {
                return _banDAL.CapNhatTrangThaiTreGio();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Cập nhật trạng thái trễ giờ: {ex.Message}");
            }
        }

        public int TuDongHuyDatBanTreGio()
        {
            try
            {
                return _banDAL.TuDongHuyDatBanTreGio();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Tự động hủy đặt bàn trễ giờ: {ex.Message}");
            }
        }
    }
}
