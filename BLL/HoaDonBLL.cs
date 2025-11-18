using System;
using System.Data;
using DAL;
using QLNhaHangTiecCuoi.Share;

namespace BLL
{
    public class HoaDonBLL
    {
        private readonly HoaDonDAL _dal;
        public HoaDonBLL(DatabaseHelper db) { _dal = new HoaDonDAL(db); }

        public DataTable GetHoaDonList(int chiNhanhId, string trangThai = "CHỜ TT", int top = 100, string? loai = null)
        {
            return _dal.GetHoaDonList(chiNhanhId, trangThai, top, loai);
        }

        public int GetWaitingInvoicesCount(int chiNhanhId)
        {
            return _dal.GetWaitingInvoicesCount(chiNhanhId);
        }

        public (int SoHd, decimal Tong) GetPaidStatsOnDateUtc(int chiNhanhId, DateTime dateUtc)
        {
            return _dal.GetPaidStatsOnDateUtc(chiNhanhId, dateUtc);
        }

        public DataTable GetPaidInvoicesHistory(int chiNhanhId, DateTime? fromDate = null, DateTime? toDate = null, string? phuongThuc = null, int top = 100)
        {
            return _dal.GetPaidInvoicesHistory(chiNhanhId, fromDate, toDate, phuongThuc, top);
        }

        public bool ProcessPayment(int hoaDonId, decimal soTien, string hinhThuc, out string errorMessage, string? thuNgan = null, int? kmId = null, int? voucherId = null, decimal? soTienKm = null)
        {
            errorMessage = string.Empty;
            try
            {
                return _dal.ProcessPayment(hoaDonId, soTien, hinhThuc, thuNgan, kmId, voucherId, soTienKm);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public DataRow? GetHoaDonById(int hoaDonId)
        {
            return _dal.GetHoaDonById(hoaDonId);
        }

        public DataTable GetHoaDonForKhachHang(int chiNhanhId, int top = 100)
        {
            return _dal.GetHoaDonForKhachHang(chiNhanhId, top);
        }

        // Lấy số tiền còn lại cần thanh toán cho hóa đơn tiệc cưới (tổng hóa đơn - tổng cọc - tổng thanh toán)
        public decimal LaySoTienConLai(int hoaDonId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var hoaDon = GetHoaDonById(hoaDonId);
                if (hoaDon == null)
                {
                    errorMessage = "Không tìm thấy hóa đơn!";
                    return 0;
                }

                // Chỉ áp dụng cho hóa đơn tiệc cưới
                string loai = hoaDon["loai"]?.ToString() ?? "";
                if (loai != "TIECCUOI")
                {
                    // Hóa đơn nhà hàng: trả về tổng hóa đơn
                    return Convert.ToDecimal(hoaDon["tong_sau_thue"]);
                }

                // Lấy hop_dong_id từ tham_chieu_id
                if (hoaDon["tham_chieu_id"] == DBNull.Value)
                {
                    errorMessage = "Hóa đơn không liên kết với hợp đồng!";
                    return Convert.ToDecimal(hoaDon["tong_sau_thue"]);
                }

                int hopDongId = Convert.ToInt32(hoaDon["tham_chieu_id"]);
                decimal tongHoaDon = Convert.ToDecimal(hoaDon["tong_sau_thue"]);

                // Tính tổng cọc
                decimal tongCoc = 0;
                var datSanhBLL = new QLNhaHangTiecCuoi.BLL.DatSanhBLL();
                var dtCoc = datSanhBLL.LayDanhSachCoc(hopDongId);
                if (dtCoc != null && dtCoc.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCoc.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                        {
                            tongCoc += Convert.ToDecimal(row["so_tien"]);
                        }
                    }
                }

                // Tính tổng thanh toán từ hop_dong_tt
                decimal tongThanhToan = 0;
                var dtThanhToan = datSanhBLL.LayDanhSachThanhToan(hopDongId);
                if (dtThanhToan != null && dtThanhToan.Rows.Count > 0)
                {
                    foreach (DataRow row in dtThanhToan.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                        {
                            tongThanhToan += Convert.ToDecimal(row["so_tien"]);
                        }
                    }
                }

                // Số tiền còn lại = Tổng hóa đơn - Tổng cọc - Tổng thanh toán
                decimal soTienConLai = tongHoaDon - tongCoc - tongThanhToan;
                if (soTienConLai < 0)
                    soTienConLai = 0;

                return soTienConLai;
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi tính số tiền còn lại: {ex.Message}";
                return 0;
            }
        }

        // Kiểm tra và cập nhật trạng thái hóa đơn khi số tiền còn lại = 0 hoặc trạng thái đặt sảnh = "ĐÃ THANH TOÁN"
        public bool KiemTraVaCapNhatTrangThaiHoaDon(int hoaDonId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var hoaDon = GetHoaDonById(hoaDonId);
                if (hoaDon == null)
                {
                    errorMessage = "Không tìm thấy hóa đơn!";
                    return false;
                }

                // Chỉ áp dụng cho hóa đơn tiệc cưới
                string loai = hoaDon["loai"]?.ToString() ?? "";
                if (loai != "TIECCUOI")
                {
                    return true;
                }

                string trangThaiHoaDon = hoaDon["trang_thai"]?.ToString() ?? "";
                if (trangThaiHoaDon == "ĐÃ THANH TOÁN")
                {
                    return true; // Đã thanh toán rồi
                }

                // Lấy hop_dong_id từ tham_chieu_id
                if (hoaDon["tham_chieu_id"] == DBNull.Value)
                {
                    return true; // Không có hợp đồng
                }

                int hopDongId = Convert.ToInt32(hoaDon["tham_chieu_id"]);

                // Lấy dat_sanh_id từ hop_dong
                var datSanhBLL = new QLNhaHangTiecCuoi.BLL.DatSanhBLL();
                int? datSanhId = datSanhBLL.LayDatSanhIdByHopDongId(hopDongId);
                if (!datSanhId.HasValue || datSanhId.Value <= 0)
                {
                    return true; // Không tìm thấy đặt sảnh
                }

                // Kiểm tra trạng thái đặt sảnh
                var datSanhInfo = datSanhBLL.LayThongTinDatSanh(datSanhId.Value);
                if (datSanhInfo != null)
                {
                    string trangThaiDatSanh = datSanhInfo["trang_thai"]?.ToString() ?? "";
                    if (trangThaiDatSanh == "ĐÃ THANH TOÁN")
                    {
                        return _dal.CapNhatTrangThaiHoaDon(hoaDonId, "ĐÃ THANH TOÁN");
                    }
                }

                // Kiểm tra số tiền còn lại
                decimal soTienConLai = LaySoTienConLai(hoaDonId, out string error);
                if (soTienConLai <= 0)
                {
                    return _dal.CapNhatTrangThaiHoaDon(hoaDonId, "ĐÃ THANH TOÁN");
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi kiểm tra và cập nhật trạng thái hóa đơn: {ex.Message}";
                return false;
            }
        }

        // Xử lý hoàn tiền hóa đơn nhà hàng
        public bool ProcessRefund(int hoaDonId, out string errorMessage)
        {
            return _dal.ProcessRefund(hoaDonId, out errorMessage);
        }

        // Đếm số lượng hóa đơn theo loại (NHAHANG hoặc TIECCUOI)
        public (int NhaHang, int TiecCuoi) GetHoaDonCountByLoai(int? chiNhanhId = null)
        {
            return _dal.GetHoaDonCountByLoai(chiNhanhId);
        }

        // Lấy top 5 món bán chạy nhất
        public DataTable GetTop5MonBanChay(int? chiNhanhId = null)
        {
            return _dal.GetTop5MonBanChay(chiNhanhId);
        }
    }
}


