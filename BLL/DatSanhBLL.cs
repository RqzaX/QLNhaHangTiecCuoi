using System;
using System.Data;
using DAL;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.BLL
{
    public class DatSanhBLL
    {
        private readonly DatSanhDAL _dal;
        private readonly HoaDonDAL _hoaDonDAL;

        public DatSanhBLL()
        {
            _dal = new DatSanhDAL();
            _hoaDonDAL = new HoaDonDAL(new DatabaseHelper());
        }

        // Kiểm tra sảnh có còn trống
        public bool KiemTraSanhTrong(int sanhId, TimeSpan gioToChuc, DateTime ngayToChuc, out string errorMessage, int? excludeDatSanhId = null)
        {
            errorMessage = string.Empty;

            try
            {
                if (sanhId <= 0)
                {
                    errorMessage = "Vui lòng chọn sảnh!";
                    return false;
                }

                if (ngayToChuc.Date < DateTime.Now.Date)
                {
                    errorMessage = "Ngày tổ chức không được ở quá khứ!";
                    return false;
                }

                // Chuyển đổi TimeSpan gioToChuc thành caId bằng cách tìm ca có gio_bd khớp
                int? caId = null;
                DataTable dtCa = _dal.LayDanhSachCa();
                foreach (DataRow row in dtCa.Rows)
                {
                    if (row["gio_bd"] != DBNull.Value)
                    {
                        TimeSpan gioBd = (TimeSpan)row["gio_bd"];
                        // So sánh giờ bắt đầu (chỉ so sánh giờ và phút)
                        if (gioBd.Hours == gioToChuc.Hours && gioBd.Minutes == gioToChuc.Minutes)
                        {
                            caId = Convert.ToInt32(row["ca_id"]);
                            break;
                        }
                    }
                }

                if (!caId.HasValue)
                {
                    errorMessage = "Không tìm thấy ca phù hợp với giờ tổ chức!";
                    return false;
                }

                return _dal.KiemTraSanhTrong(sanhId, caId.Value, ngayToChuc, excludeDatSanhId);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi kiểm tra trạng thái sảnh: {ex.Message}";
                return false;
            }
        }

        // Overload method để kiểm tra dựa trên ca_id (khớp với UNIQUE constraint)
        public bool KiemTraSanhTrong(int sanhId, int caId, DateTime ngayToChuc, out string errorMessage, int? excludeDatSanhId = null)
        {
            errorMessage = string.Empty;

            try
            {
                if (sanhId <= 0)
                {
                    errorMessage = "Vui lòng chọn sảnh!";
                    return false;
                }

                if (ngayToChuc.Date < DateTime.Now.Date)
                {
                    errorMessage = "Ngày tổ chức không được ở quá khứ!";
                    return false;
                }

                return _dal.KiemTraSanhTrong(sanhId, caId, ngayToChuc, excludeDatSanhId);
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

                // Kiểm tra sảnh còn trống (sử dụng caId trực tiếp)
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

        // Xóa cọc
        public bool XoaCoc(int cocId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (cocId <= 0)
                {
                    errorMessage = "ID cọc không hợp lệ!";
                    return false;
                }

                return _dal.XoaCoc(cocId);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi xóa cọc: {ex.Message}";
                return false;
            }
        }

        // Cập nhật cọc
        public bool CapNhatCoc(int cocId, decimal soTien, DateTime ngayNop, string hinhThuc, string ghiChu, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (cocId <= 0)
                {
                    errorMessage = "ID cọc không hợp lệ!";
                    return false;
                }

                if (soTien <= 0)
                {
                    errorMessage = "Số tiền cọc phải lớn hơn 0!";
                    return false;
                }

                return _dal.CapNhatCoc(cocId, soTien, ngayNop, hinhThuc, ghiChu);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi cập nhật cọc: {ex.Message}";
                return false;
            }
        }

        // Lấy thông tin cọc theo ID
        public DataRow? LayThongTinCoc(int cocId)
        {
            try
            {
                if (cocId <= 0)
                {
                    return null;
                }

                return _dal.LayThongTinCoc(cocId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin cọc: {ex.Message}", ex);
            }
        }

        // Xóa thanh toán
        public bool XoaThanhToan(int ttId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (ttId <= 0)
                {
                    errorMessage = "ID thanh toán không hợp lệ!";
                    return false;
                }

                return _dal.XoaThanhToan(ttId);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi xóa thanh toán: {ex.Message}";
                return false;
            }
        }

        // Cập nhật thanh toán
        public bool CapNhatThanhToan(int ttId, decimal soTien, DateTime ngayTT, string hinhThuc, string noiDung, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (ttId <= 0)
                {
                    errorMessage = "ID thanh toán không hợp lệ!";
                    return false;
                }

                if (soTien <= 0)
                {
                    errorMessage = "Số tiền thanh toán phải lớn hơn 0!";
                    return false;
                }

                return _dal.CapNhatThanhToan(ttId, soTien, ngayTT, hinhThuc, noiDung);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi cập nhật thanh toán: {ex.Message}";
                return false;
            }
        }

        // Lấy thông tin thanh toán theo ID
        public DataRow? LayThongTinThanhToan(int ttId)
        {
            try
            {
                if (ttId <= 0)
                {
                    return null;
                }

                return _dal.LayThongTinThanhToan(ttId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy thông tin thanh toán: {ex.Message}", ex);
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

        // Kiểm tra và cập nhật trạng thái khi thanh toán hết
        public bool KiemTraVaCapNhatTrangThaiTheoTienThanhToan(int datSanhId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (datSanhId <= 0)
                {
                    errorMessage = "ID đơn đặt sảnh không hợp lệ!";
                    return false;
                }

                // Lấy thông tin đặt sảnh
                DataRow datSanhInfo = LayThongTinDatSanh(datSanhId);
                if (datSanhInfo == null)
                {
                    errorMessage = "Không tìm thấy thông tin đặt sảnh!";
                    return false;
                }

                string trangThaiHienTai = datSanhInfo["trang_thai"]?.ToString() ?? "";
                
                if (trangThaiHienTai.ToUpper() != "ĐÃ CỌC")
                {
                    return true;
                }

                int? hopDongId = LayHopDongId(datSanhId);
                if (!hopDongId.HasValue || hopDongId.Value <= 0)
                {
                    return true;
                }

                decimal tongDuKien = LayTongDuKien(hopDongId.Value);
                decimal tongCoc = 0;
                DataTable dtCoc = LayDanhSachCoc(hopDongId.Value);
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

                // Tính tổng thanh toán
                decimal tongThanhToan = 0;
                DataTable dtThanhToan = LayDanhSachThanhToan(hopDongId.Value);
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

                decimal tongConLai = tongDuKien - tongCoc - tongThanhToan;
            
                if (tongConLai <= 0)
                {
                    bool success = CapNhatTrangThaiDatSanh(datSanhId, "ĐÃ THANH TOÁN", out string errorUpdate);
                    if (!success)
                    {
                        errorMessage = errorUpdate;
                        return false;
                    }
                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi kiểm tra và cập nhật trạng thái: {ex.Message}";
                return false;
            }
        }

        // Hủy đặt sảnh - cập nhật trạng thái và ghi chú vào các bảng liên quan
        public bool HuyDatSanh(int datSanhId, DateTime ngayHuy, string lyDoHuy, decimal soTienHoanCoc, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (datSanhId <= 0)
                {
                    errorMessage = "ID đơn đặt sảnh không hợp lệ!";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(lyDoHuy))
                {
                    errorMessage = "Vui lòng nhập lý do hủy!";
                    return false;
                }

                return _dal.HuyDatSanh(datSanhId, ngayHuy, lyDoHuy, soTienHoanCoc);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi hủy đặt sảnh: {ex.Message}";
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

        // Lấy hop_dong_id từ dat_sanh_id
        public int? LayHopDongId(int datSanhId)
        {
            try
            {
                return _dal.LayHopDongId(datSanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy hop_dong_id: {ex.Message}", ex);
            }
        }

        // Lấy dat_sanh_id từ hop_dong_id
        public int? LayDatSanhIdByHopDongId(int hopDongId)
        {
            try
            {
                return _dal.LayDatSanhIdByHopDongId(hopDongId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy dat_sanh_id: {ex.Message}", ex);
            }
        }

        // Lấy danh sách cọc
        public DataTable LayDanhSachCoc(int hopDongId)
        {
            try
            {
                if (hopDongId <= 0)
                    return new DataTable();

                return _dal.LayDanhSachCoc(hopDongId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách cọc: {ex.Message}", ex);
            }
        }

        // Lấy danh sách thanh toán
        public DataTable LayDanhSachThanhToan(int hopDongId)
        {
            try
            {
                if (hopDongId <= 0)
                    return new DataTable();

                return _dal.LayDanhSachThanhToan(hopDongId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách thanh toán: {ex.Message}", ex);
            }
        }

        // Lấy tổng dự kiến
        public decimal LayTongDuKien(int hopDongId)
        {
            try
            {
                if (hopDongId <= 0)
                    return 0;

                return _dal.LayTongDuKien(hopDongId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy tổng dự kiến: {ex.Message}", ex);
            }
        }

        // Lấy tổng số tiền đã cọc từ dat_sanh_id
        public decimal LayTongCocDaThu(int datSanhId)
        {
            try
            {
                if (datSanhId <= 0)
                    return 0;

                int? hopDongId = _dal.LayHopDongId(datSanhId);
                if (!hopDongId.HasValue)
                    return 0;

                decimal tongCoc = 0;
                DataTable dtCoc = _dal.LayDanhSachCoc(hopDongId.Value);
                if (dtCoc != null)
                {
                    foreach (DataRow row in dtCoc.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                            tongCoc += Convert.ToDecimal(row["so_tien"]);
                    }
                }
                return tongCoc;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy tổng cọc đã thu: {ex.Message}", ex);
            }
        }

        // Đổi lịch đặt sảnh
        public bool DoiLichDatSanh(int datSanhId, int chiNhanhId, int sanhId, TimeSpan gioToChuc, DateTime ngayToChuc, string lyDo, string? ghiChuThem, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (datSanhId <= 0)
                {
                    errorMessage = "Mã đặt sảnh không hợp lệ!";
                    return false;
                }

                // Kiểm tra trạng thái đơn đặt sảnh
                DataRow? datSanhInfo = LayThongTinDatSanh(datSanhId);
                if (datSanhInfo == null)
                {
                    errorMessage = "Không tìm thấy thông tin đặt sảnh!";
                    return false;
                }

                if (datSanhInfo["trang_thai"] != DBNull.Value)
                {
                    string trangThai = datSanhInfo["trang_thai"].ToString() ?? "";
                    if (trangThai.ToUpper() == "ĐÃ HỦY")
                    {
                        errorMessage = "Không thể đổi lịch cho đơn đặt sảnh đã bị hủy!";
                        return false;
                    }
                    
                    if (trangThai.ToUpper() == "HOÀN TẤT")
                    {
                        errorMessage = "Không thể đổi lịch cho đơn đặt sảnh đã hoàn tất!";
                        return false;
                    }
                }

                if (chiNhanhId <= 0)
                {
                    errorMessage = "Vui lòng chọn chi nhánh!";
                    return false;
                }

                if (sanhId <= 0)
                {
                    errorMessage = "Vui lòng chọn sảnh!";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(lyDo))
                {
                    errorMessage = "Vui lòng nhập lý do đổi lịch!";
                    return false;
                }

                if (ngayToChuc.Date < DateTime.Now.Date)
                {
                    errorMessage = "Ngày tổ chức không được ở quá khứ!";
                    return false;
                }

                // Kiểm tra nếu thông tin mới giống thông tin cũ
                bool thongTinGiongNhau = true;
                
                if (datSanhInfo["chi_nhanh_id"] != DBNull.Value)
                {
                    int chiNhanhIdCu = Convert.ToInt32(datSanhInfo["chi_nhanh_id"]);
                    if (chiNhanhIdCu != chiNhanhId)
                        thongTinGiongNhau = false;
                }
                else
                {
                    thongTinGiongNhau = false;
                }

                if (thongTinGiongNhau && datSanhInfo["sanh_id"] != DBNull.Value)
                {
                    int sanhIdCu = Convert.ToInt32(datSanhInfo["sanh_id"]);
                    if (sanhIdCu != sanhId)
                        thongTinGiongNhau = false;
                }
                else
                {
                    thongTinGiongNhau = false;
                }

                if (thongTinGiongNhau && datSanhInfo["ngay_to_chuc"] != DBNull.Value)
                {
                    DateTime ngayToChucCu = Convert.ToDateTime(datSanhInfo["ngay_to_chuc"]).Date;
                    if (ngayToChucCu != ngayToChuc.Date)
                        thongTinGiongNhau = false;
                }
                else
                {
                    thongTinGiongNhau = false;
                }

                if (thongTinGiongNhau && datSanhInfo["gio_to_chuc"] != DBNull.Value)
                {
                    TimeSpan gioToChucCu = (TimeSpan)datSanhInfo["gio_to_chuc"];
                    if (gioToChucCu != gioToChuc)
                        thongTinGiongNhau = false;
                }
                else
                {
                    thongTinGiongNhau = false;
                }

                if (thongTinGiongNhau)
                {
                    errorMessage = "Thông tin mới giống với thông tin hiện tại. Vui lòng thay đổi ít nhất một thông tin!";
                    return false;
                }

                // Chuyển đổi TimeSpan gioToChuc thành caId bằng cách tìm ca có gio_bd khớp
                int? caId = null;
                DataTable dtCa = _dal.LayDanhSachCa();
                foreach (DataRow row in dtCa.Rows)
                {
                    if (row["gio_bd"] != DBNull.Value)
                    {
                        TimeSpan gioBd = (TimeSpan)row["gio_bd"];
                        // So sánh giờ bắt đầu (chỉ so sánh giờ và phút)
                        if (gioBd.Hours == gioToChuc.Hours && gioBd.Minutes == gioToChuc.Minutes)
                        {
                            caId = Convert.ToInt32(row["ca_id"]);
                            break;
                        }
                    }
                }

                if (!caId.HasValue)
                {
                    errorMessage = "Không tìm thấy ca phù hợp với giờ tổ chức!";
                    return false;
                }

                // Kiểm tra sảnh có trống không (loại trừ đơn đặt sảnh hiện tại)
                bool sanhTrong = _dal.KiemTraSanhTrong(sanhId, caId.Value, ngayToChuc, datSanhId);
                if (!sanhTrong)
                {
                    errorMessage = "Sảnh đã được đặt vào thời gian này. Vui lòng chọn ngày/ca khác!";
                    return false;
                }

                return _dal.DoiLichDatSanh(datSanhId, chiNhanhId, sanhId, gioToChuc, ngayToChuc, lyDo, ghiChuThem);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi BLL - Đổi lịch đặt sảnh: {ex.Message}";
                return false;
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

        // Lấy danh sách đơn đặt sảnh theo chi nhánh
        public DataTable LayDanhSachDatSanhTheoChiNhanh(int chiNhanhId, int top = 100)
        {
            try
            {
                return _dal.LayDanhSachDatSanhTheoChiNhanh(chiNhanhId, top);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách đơn đặt sảnh theo chi nhánh: {ex.Message}", ex);
            }
        }

        public DataTable LayDanhSachDatSanhHomNay(int chiNhanhId)
        {
            try
            {
                return _dal.LayDanhSachDatSanhHomNay(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách đặt sảnh hôm nay: {ex.Message}", ex);
            }
        }

        // Lấy tổng số đơn đặt sảnh
        public int LayTongSoDon()
        {
            try
            {
                return _dal.LayTongSoDon();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy tổng số đơn: {ex.Message}", ex);
            }
        }

        // Lấy số đơn đã xác nhận
        public int LaySoDonXacNhan()
        {
            try
            {
                return _dal.LaySoDonXacNhan();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy số đơn xác nhận: {ex.Message}", ex);
            }
        }

        // Lấy tổng số sảnh
        public int LayTongSoSanh()
        {
            try
            {
                return _dal.LayTongSoSanh();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy tổng số sảnh: {ex.Message}", ex);
            }
        }

        // Lấy tổng số sảnh theo chi nhánh
        public int LayTongSoSanhTheoChiNhanh(int chiNhanhId)
        {
            try
            {
                return _dal.LayTongSoSanhTheoChiNhanh(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy tổng số sảnh theo chi nhánh: {ex.Message}", ex);
            }
        }

        // Lấy số sảnh đã đặt trong tháng này
        public int LaySoSanhDaDatThangNay(int chiNhanhId)
        {
            try
            {
                return _dal.LaySoSanhDaDatThangNay(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy số sảnh đã đặt: {ex.Message}", ex);
            }
        }

        // Lấy số sự kiện đặt sảnh hôm nay
        public int LaySoSuKienHomNay(int chiNhanhId)
        {
            try
            {
                return _dal.LaySoSuKienHomNay(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy số sự kiện hôm nay: {ex.Message}", ex);
            }
        }

        // Lấy doanh thu tháng
        public decimal LayDoanhThuThang()
        {
            try
            {
                return _dal.LayDoanhThuThang();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy doanh thu tháng: {ex.Message}", ex);
            }
        }

        // Xóa vĩnh viễn đặt sảnh
        public bool XoaDatSanhVinhVien(int datSanhId, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (datSanhId <= 0)
                {
                    errorMessage = "Mã đặt sảnh không hợp lệ!";
                    return false;
                }

                // Kiểm tra xem đặt sảnh có tồn tại không
                DataRow? datSanhInfo = LayThongTinDatSanh(datSanhId);
                if (datSanhInfo == null)
                {
                    errorMessage = "Không tìm thấy đặt sảnh để xóa!";
                    return false;
                }

                return _dal.XoaDatSanhVinhVien(datSanhId);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi BLL - Xóa đặt sảnh vĩnh viễn: {ex.Message}";
                return false;
            }
        }

        // Tạo hóa đơn khi đã cọc
        public int TaoHoaDonKhiDaCoc(int datSanhId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                // Lấy thông tin đặt sảnh
                DataRow? datSanhInfo = LayThongTinDatSanh(datSanhId);
                if (datSanhInfo == null)
                {
                    errorMessage = "Không tìm thấy thông tin đặt sảnh!";
                    return 0;
                }

                int chiNhanhId = Convert.ToInt32(datSanhInfo["chi_nhanh_id"]);
                int khachHangId = Convert.ToInt32(datSanhInfo["khach_hang_id"]);

                // Lấy hợp đồng ID
                int? hopDongId = LayHopDongId(datSanhId);
                if (!hopDongId.HasValue || hopDongId.Value <= 0)
                {
                    errorMessage = "Chưa có hợp đồng cho đặt sảnh này!";
                    return 0;
                }

                int? hoaDonExisting = _dal.GetHoaDonIdByDatSanh(datSanhId);
                if (hoaDonExisting.HasValue && hoaDonExisting.Value > 0)
                {
                    return hoaDonExisting.Value;
                }
                if (DaCoHoaDon(hopDongId.Value))
                {
                    // Đã có hóa đơn cho hợp đồng này
                    hoaDonExisting = _dal.GetHoaDonIdByDatSanh(datSanhId);
                    if (hoaDonExisting.HasValue && hoaDonExisting.Value > 0)
                        return hoaDonExisting.Value;
                    errorMessage = "Đã có hóa đơn cho hợp đồng này!";
                    return 0;
                }

                // Lấy tổng dự kiến
                decimal tongDuKien = LayTongDuKien(hopDongId.Value);
                if (tongDuKien <= 0)
                {
                    errorMessage = "Tổng dự kiến không hợp lệ!";
                    return 0;
                }

                // Tạo hóa đơn (mặc định VAT = 10%, phí dịch vụ = 0, giảm giá = 0)
                decimal vatPercent = 10m;
                decimal phiDv = 0m;
                decimal giamGia = 0m;
                decimal tongTruocThue = tongDuKien;
                decimal vat = Math.Round(tongTruocThue * vatPercent / 100m, 0);
                decimal tongSauThue = tongTruocThue + vat + phiDv - giamGia;

                // Lấy tên sảnh và tên khách hàng (người đặt sảnh)
                string tenSanh = datSanhInfo["ten_sanh"]?.ToString() ?? "";
                string tenKhachHang = datSanhInfo["ten_khach_hang"]?.ToString() ?? "";

                int hoaDonId = _hoaDonDAL.CreateHoaDon(
                    chiNhanhId, 
                    "TIECCUOI", 
                    vatPercent, 
                    phiDv, 
                    giamGia, 
                    tongTruocThue, 
                    tongSauThue,
                    khachHangId,
                    hopDongId.Value,
                    soBanSanh: tenSanh,
                    tenNguoiDat: tenKhachHang
                );

                // Thêm chi tiết hóa đơn từ hợp đồng
                ThemChiTietHoaDonTuHopDong(hoaDonId, hopDongId.Value);

                return hoaDonId;
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi tạo hóa đơn: {ex.Message}";
                return 0;
            }
        }

        // Kiểm tra xem đã có hóa đơn chưa
        private bool DaCoHoaDon(int hopDongId)
        {
            try
            {
                return _dal.DaCoHoaDon(hopDongId);
            }
            catch
            {
                return false;
            }
        }

        // Thêm chi tiết hóa đơn từ hợp đồng
        private void ThemChiTietHoaDonTuHopDong(int hoaDonId, int hopDongId)
        {
            try
            {
                // Lấy chi tiết món từ hợp đồng
                DataTable dtMon = _dal.LayChiTietMonHopDong(hopDongId);
                if (dtMon != null)
                {
                    foreach (DataRow row in dtMon.Rows)
                    {
                        string tenMon = row["ten_mon"]?.ToString() ?? "";
                        decimal soLuong = Convert.ToDecimal(row["so_luong"]);
                        decimal donGia = Convert.ToDecimal(row["don_gia"]);
                        _hoaDonDAL.InsertHoaDonCt(hoaDonId, "MÓN", Convert.ToInt32(row["mon_id"]), tenMon, soLuong, donGia);
                    }
                }

                // Lấy chi tiết dịch vụ từ hợp đồng
                DataTable dtDv = _dal.LayChiTietDichVuHopDong(hopDongId);
                if (dtDv != null)
                {
                    foreach (DataRow row in dtDv.Rows)
                    {
                        string tenDv = row["ten_dv"]?.ToString() ?? "";
                        decimal soLuong = Convert.ToDecimal(row["so_luong"]);
                        decimal donGia = Convert.ToDecimal(row["don_gia"]);
                        _hoaDonDAL.InsertHoaDonCt(hoaDonId, "DV", Convert.ToInt32(row["dv_id"]), tenDv, soLuong, donGia);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi thêm chi tiết hóa đơn: {ex.Message}", ex);
            }
        }
    }
}

