using System;
using System.Data;
using QLNhaHangTiecCuoi.DAL;

namespace QLNhaHangTiecCuoi.BLL
{
    public class DatSanhBLL
    {
        private readonly DatSanhDAL _dal;

        public DatSanhBLL()
        {
            _dal = new DatSanhDAL();
        }

        // Kiểm tra sảnh có còn trống
        public bool KiemTraSanhTrong(int sanhId, int caId, DateTime ngayToChuc, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (sanhId <= 0)
                {
                    errorMessage = "Vui lòng chọn sảnh!";
                    return false;
                }

                if (caId <= 0)
                {
                    errorMessage = "Vui lòng chọn ca!";
                    return false;
                }

                if (ngayToChuc.Date < DateTime.Now.Date)
                {
                    errorMessage = "Ngày tổ chức không được ở quá khứ!";
                    return false;
                }

                return _dal.KiemTraSanhTrong(sanhId, caId, ngayToChuc);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi kiểm tra trạng thái sảnh: {ex.Message}";
                return false;
            }
        }

        // Lấy danh sách sảnh
        public DataTable LayDanhSachSanh(int chiNhanhId)
        {
            try
            {
                if (chiNhanhId <= 0)
                    throw new ArgumentException("Chi nhánh không hợp lệ!");

                return _dal.LayDanhSachSanh(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách sảnh: {ex.Message}", ex);
            }
        }

        // Lấy thông tin sảnh
        public DataRow LayThongTinSanh(int sanhId)
        {
            try
            {
                if (sanhId <= 0)
                    throw new ArgumentException("ID sảnh không hợp lệ!");

                return _dal.LayThongTinSanh(sanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin sảnh: {ex.Message}", ex);
            }
        }

        // Lấy danh sách ca
        public DataTable LayDanhSachCa()
        {
            try
            {
                return _dal.LayDanhSachCa();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách ca: {ex.Message}", ex);
            }
        }

        // Lấy danh sách chi nhánh
        public DataTable LayDanhSachChiNhanh()
        {
            try
            {
                return _dal.LayDanhSachChiNhanh();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách chi nhánh: {ex.Message}", ex);
            }
        }

        // Tạo đơn đặt sảnh với validation
        public int TaoDatSanh(int chiNhanhId, int sanhId, int caId, DateTime ngayToChuc,
            int khachHangId, int? soBanDuKien, int? goiId, string ghiChu, TimeSpan? gioToChuc, string trangThai, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Validation
                if (chiNhanhId <= 0)
                {
                    errorMessage = "Vui lòng chọn chi nhánh!";
                    return 0;
                }

                if (sanhId <= 0)
                {
                    errorMessage = "Vui lòng chọn sảnh!";
                    return 0;
                }

                if (caId <= 0)
                {
                    errorMessage = "Vui lòng chọn ca!";
                    return 0;
                }

                if (ngayToChuc.Date < DateTime.Now.Date)
                {
                    errorMessage = "Ngày tổ chức không được ở quá khứ!";
                    return 0;
                }

                if (khachHangId <= 0)
                {
                    errorMessage = "Thông tin khách hàng không hợp lệ!";
                    return 0;
                }

                // Kiểm tra sảnh còn trống
                if (!_dal.KiemTraSanhTrong(sanhId, caId, ngayToChuc))
                {
                    errorMessage = "Sảnh đã được đặt trong thời gian này!";
                    return 0;
                }

                // Tạo đơn đặt sảnh
                int datSanhId = _dal.TaoDatSanh(chiNhanhId, sanhId, caId, ngayToChuc,
                    khachHangId, soBanDuKien, goiId, ghiChu, gioToChuc, trangThai ?? "CHỜ XÁC NHẬN");

                return datSanhId;
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi tạo đơn đặt sảnh: {ex.Message}";
                return 0;
            }
        }

        // Tạo hợp đồng
        public int TaoHopDong(string soHopDong, int datSanhId, DateTime ngayKy, decimal tongDuKien, string dieuKhoan, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(soHopDong))
                {
                    errorMessage = "Số hợp đồng không được để trống!";
                    return 0;
                }

                if (datSanhId <= 0)
                {
                    errorMessage = "ID đơn đặt sảnh không hợp lệ!";
                    return 0;
                }

                return _dal.TaoHopDong(soHopDong, datSanhId, ngayKy, tongDuKien, dieuKhoan);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi tạo hợp đồng: {ex.Message}";
                return 0;
            }
        }

        // Lưu tiền cọc
        public int LuuTienCoc(int hopDongId, decimal soTien, DateTime ngayNop, string hinhThuc, string ghiChu, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (hopDongId <= 0)
                {
                    errorMessage = "ID hợp đồng không hợp lệ!";
                    return 0;
                }

                if (soTien <= 0)
                {
                    errorMessage = "Số tiền cọc phải lớn hơn 0!";
                    return 0;
                }

                return _dal.LuuTienCoc(hopDongId, soTien, ngayNop, hinhThuc, ghiChu);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi lưu tiền cọc: {ex.Message}";
                return 0;
            }
        }

        // Lưu thanh toán
        public int LuuThanhToan(int hopDongId, decimal soTien, DateTime ngayTT, string hinhThuc, string noiDung, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (hopDongId <= 0)
                {
                    errorMessage = "ID hợp đồng không hợp lệ!";
                    return 0;
                }

                if (soTien <= 0)
                {
                    errorMessage = "Số tiền thanh toán phải lớn hơn 0!";
                    return 0;
                }

                return _dal.LuuThanhToan(hopDongId, soTien, ngayTT, hinhThuc, noiDung);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi lưu thanh toán: {ex.Message}";
                return 0;
            }
        }

        // Lưu chi tiết món ăn vào hợp đồng
        public bool LuuChiTietMon(int hopDongId, int monId, decimal soLuong, decimal donGia, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (hopDongId <= 0)
                {
                    errorMessage = "ID hợp đồng không hợp lệ!";
                    return false;
                }

                if (monId <= 0)
                {
                    errorMessage = "ID món ăn không hợp lệ!";
                    return false;
                }

                if (soLuong <= 0)
                {
                    errorMessage = "Số lượng phải lớn hơn 0!";
                    return false;
                }

                return _dal.LuuChiTietMon(hopDongId, monId, soLuong, donGia);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi lưu chi tiết món: {ex.Message}";
                return false;
            }
        }

        // Lưu chi tiết dịch vụ vào hợp đồng
        public bool LuuChiTietDichVu(int hopDongId, int dvId, decimal soLuong, decimal donGia, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (hopDongId <= 0)
                {
                    errorMessage = "ID hợp đồng không hợp lệ!";
                    return false;
                }

                if (dvId <= 0)
                {
                    errorMessage = "ID dịch vụ không hợp lệ!";
                    return false;
                }

                if (soLuong <= 0)
                {
                    errorMessage = "Số lượng phải lớn hơn 0!";
                    return false;
                }

                return _dal.LuuChiTietDichVu(hopDongId, dvId, soLuong, donGia);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi lưu chi tiết dịch vụ: {ex.Message}";
                return false;
            }
        }

        // Cập nhật trạng thái đơn đặt sảnh
        public bool CapNhatTrangThaiDatSanh(int datSanhId, string trangThai, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (datSanhId <= 0)
                {
                    errorMessage = "ID đơn đặt sảnh không hợp lệ!";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(trangThai))
                {
                    errorMessage = "Trạng thái không được để trống!";
                    return false;
                }

                return _dal.CapNhatTrangThaiDatSanh(datSanhId, trangThai);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi cập nhật trạng thái: {ex.Message}";
                return false;
            }
        }

        // Lấy giờ bắt đầu từ ca_id
        public TimeSpan? LayGioBatDauCa(int caId)
        {
            try
            {
                DataTable dtCa = _dal.LayDanhSachCa();
                foreach (DataRow row in dtCa.Rows)
                {
                    if (Convert.ToInt32(row["ca_id"]) == caId)
                    {
                        if (row["gio_bd"] != DBNull.Value)
                        {
                            return ((TimeSpan)row["gio_bd"]);
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // Lấy thông tin đơn đặt sảnh
        public DataRow LayThongTinDatSanh(int datSanhId)
        {
            try
            {
                if (datSanhId <= 0)
                    throw new ArgumentException("ID đơn đặt sảnh không hợp lệ!");

                return _dal.LayThongTinDatSanh(datSanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin đơn đặt sảnh: {ex.Message}", ex);
            }
        }

        // Format tiền
        public string FormatTien(decimal amount)
        {
            return amount.ToString("#,##0") + " đ";
        }

        // Lấy danh sách đơn đặt sảnh
        public DataTable LayDanhSachDatSanh()
        {
            try
            {
                return _dal.LayDanhSachDatSanh();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách đơn đặt sảnh: {ex.Message}", ex);
            }
        }
    }
}

