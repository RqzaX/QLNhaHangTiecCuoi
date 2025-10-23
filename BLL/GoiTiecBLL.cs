using System;
using System.Data;
using DAL;

namespace BLL
{
    public class GoiTiecBLL
    {
        private readonly GoiTiecDAL _dal;

        public GoiTiecBLL()
        {
            _dal = new GoiTiecDAL();
        }

        // Lấy tất cả gói tiệc
        public DataTable GetAllGoiTiec()
        {
            try
            {
                return _dal.GetAllGoiTiec();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetAllGoiTiec: {ex.Message}", ex);
            }
        }

        // Lấy thông tin gói tiệc theo ID
        public DataRow GetGoiTiecById(int goiId)
        {
            try
            {
                if (goiId <= 0)
                    throw new ArgumentException("ID gói tiệc không hợp lệ!");

                return _dal.GetGoiTiecById(goiId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetGoiTiecById: {ex.Message}", ex);
            }
        }

        // Thêm gói tiệc mới
        public bool ThemGoiTiec(string maGoi, string tenGoi, decimal giaCoBan, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Validate dữ liệu
                if (string.IsNullOrWhiteSpace(maGoi))
                {
                    errorMessage = "Vui lòng nhập mã gói!";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(tenGoi))
                {
                    errorMessage = "Vui lòng nhập tên gói!";
                    return false;
                }

                if (giaCoBan < 0)
                {
                    errorMessage = "Giá cơ bản không được âm!";
                    return false;
                }

                // Kiểm tra mã gói đã tồn tại
                if (_dal.KiemTraMaGoiTonTai(maGoi.Trim()))
                {
                    errorMessage = $"Mã gói '{maGoi}' đã tồn tại!";
                    return false;
                }

                return _dal.ThemGoiTiec(maGoi.Trim(), tenGoi.Trim(), giaCoBan);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi khi thêm gói tiệc: {ex.Message}";
                return false;
            }
        }

        // Cập nhật gói tiệc
        public bool CapNhatGoiTiec(int goiId, string maGoi, string tenGoi, decimal giaCoBan, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Validate dữ liệu
                if (goiId <= 0)
                {
                    errorMessage = "ID gói tiệc không hợp lệ!";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(maGoi))
                {
                    errorMessage = "Vui lòng nhập mã gói!";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(tenGoi))
                {
                    errorMessage = "Vui lòng nhập tên gói!";
                    return false;
                }

                if (giaCoBan < 0)
                {
                    errorMessage = "Giá cơ bản không được âm!";
                    return false;
                }

                // Kiểm tra mã gói đã tồn tại (trừ chính nó)
                if (_dal.KiemTraMaGoiTonTai(maGoi.Trim(), goiId))
                {
                    errorMessage = $"Mã gói '{maGoi}' đã tồn tại!";
                    return false;
                }

                return _dal.CapNhatGoiTiec(goiId, maGoi.Trim(), tenGoi.Trim(), giaCoBan);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi khi cập nhật gói tiệc: {ex.Message}";
                return false;
            }
        }

        // Xóa gói tiệc
        public bool XoaGoiTiec(int goiId, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (goiId <= 0)
                {
                    errorMessage = "ID gói tiệc không hợp lệ!";
                    return false;
                }

                return _dal.XoaGoiTiec(goiId);
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi khi xóa gói tiệc: {ex.Message}\n\nCó thể gói này đang được sử dụng trong hệ thống.";
                return false;
            }
        }

        // Tìm kiếm gói tiệc
        public DataTable TimKiemGoiTiec(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return GetAllGoiTiec();

                return _dal.TimKiemGoiTiec(keyword.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - TimKiemGoiTiec: {ex.Message}", ex);
            }
        }

        // Format tiền
        public string FormatTien(decimal amount)
        {
            return amount.ToString("#,##0") + " đ";
        }

        // Parse tiền từ string
        public decimal ParseTien(string tienText)
        {
            if (string.IsNullOrWhiteSpace(tienText))
                return 0;

            // Loại bỏ các ký tự không phải số
            string cleanText = tienText.Replace("đ", "").Replace(".", "").Replace(",", "").Trim();

            if (decimal.TryParse(cleanText, out decimal result))
                return result;

            return 0;
        }
    }
}