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


        public DataTable GetChiTietGoiTiec(int goiId)
        {
            try
            {
                if (goiId <= 0) throw new ArgumentException("ID gói tiệc không hợp lệ!");
                return _dal.GetChiTietGoiTiec(goiId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetChiTietGoiTiec: {ex.Message}", ex);
            }
        }

        // (Tùy chọn) Lấy chi tiết gói theo ma_goi
        public DataTable GetChiTietGoiTiec_ByMa(string maGoi)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maGoi))
                    throw new ArgumentException("Mã gói không được trống!");
                return _dal.GetChiTietGoiTiec_ByMa(maGoi.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetChiTietGoiTiec_ByMa: {ex.Message}", ex);
            }
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

        public bool ThemGoiTiec(string maGoi, string tenGoi, decimal giaCoBan, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
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
        public int GetMonIdByMaMon(string maMon)
        {
            if (string.IsNullOrWhiteSpace(maMon)) return 0;
            return _dal.GetMonIdByMaMon(maMon.Trim());
        }

        public void ThemMonVaoGoi(int goiId, string maMon, decimal soLuong)
        {
            if (goiId <= 0) throw new ArgumentException("goiId không hợp lệ");
            if (soLuong <= 0) throw new ArgumentException("Số lượng phải > 0");

            int monId = GetMonIdByMaMon(maMon);
            if (monId <= 0) throw new Exception("Không tìm thấy mã món: " + maMon);

            _dal.UpsertMonVaoGoi(goiId, monId, soLuong);
        }

        public void SuaMonTrongGoi(int goiId, string oldMaMon, string newMaMon, decimal newSoLuong)
        {
            if (goiId <= 0) throw new ArgumentException("goiId không hợp lệ");
            if (newSoLuong <= 0) throw new ArgumentException("Số lượng phải > 0");

            int oldId = GetMonIdByMaMon(oldMaMon);
            if (oldId <= 0) throw new Exception("Không tìm thấy mã món cũ: " + oldMaMon);

            int newId = GetMonIdByMaMon(newMaMon);
            if (newId <= 0) throw new Exception("Không tìm thấy mã món mới: " + newMaMon);

            _dal.UpdateMonTrongGoi(goiId, oldId, newId, newSoLuong);
        }

        public void XoaMonKhoiGoi(int goiId, string maMon)
        {
            if (goiId <= 0) throw new ArgumentException("goiId không hợp lệ");

            int monId = GetMonIdByMaMon(maMon);
            if (monId <= 0) throw new Exception("Không tìm thấy mã món: " + maMon);

            _dal.DeleteMonKhoiGoi(goiId, monId);
        }

        public DataTable GetMonTrongGoi(int goiId)
        {
            // alias, dùng chung logic với GetChiTietGoiTiecGG
            return GetChiTietGoiTiec(goiId);
        }
        public int GetGoiIdByTenGoi(string tenGoi)
        {
            if (string.IsNullOrWhiteSpace(tenGoi)) return 0;
            return _dal.GetGoiIdByTenGoi(tenGoi.Trim());
        }
        public int GetGoiIdByMaGoi(string maGoi)
        {
            if (string.IsNullOrWhiteSpace(maGoi)) return 0;
            return _dal.GetGoiIdByMaGoi(maGoi.Trim());
        }
        // Cập nhật gói tiệc

        public bool CapNhatGoiTiec(int goiId, string maGoi, string tenGoi, decimal giaCoBan, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
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

        public string FormatTien(decimal amount)
        {
            return amount.ToString("#,##0") + " đ";
        }

        public decimal ParseTien(string tienText)
        {
            if (string.IsNullOrWhiteSpace(tienText))
                return 0;

            string cleanText = tienText.Replace("đ", "").Replace(".", "").Replace(",", "").Trim();

            if (decimal.TryParse(cleanText, out decimal result))
                return result;

            return 0;
        }

        // Lấy sức chứa tối đa từ sảnh
        public int GetSucChuaToiDaTuSanh()
        {
            try
            {
                return _dal.GetSucChuaToiDaTuSanh();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetSucChuaToiDaTuSanh: {ex.Message}", ex);
            }
        }

        // Lấy sức chứa của gói tiệc cụ thể
        public int GetSucChuaGoiTiec(int goiId)
        {
            try
            {
                if (goiId <= 0)
                    throw new ArgumentException("ID gói tiệc không hợp lệ!");
                
                return _dal.GetSucChuaGoiTiec(goiId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetSucChuaGoiTiec: {ex.Message}", ex);
            }
        }

        // Tính giá mỗi bàn = tổng giá các món + 10% phí dịch vụ
        public decimal TinhGiaMoiBan(int goiId)
        {
            try
            {
                if (goiId <= 0)
                    throw new ArgumentException("ID gói tiệc không hợp lệ!");

                // Tính tổng giá các món
                decimal tongGiaMon = _dal.TinhTongGiaCacMon(goiId);
                
                // Thêm 10% phí dịch vụ
                decimal phiDichVu = tongGiaMon * 0.10m;
                
                // Giá mỗi bàn = tổng giá món + phí dịch vụ
                decimal giaMoiBan = tongGiaMon + phiDichVu;
                
                return giaMoiBan;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - TinhGiaMoiBan: {ex.Message}", ex);
            }
        }

        // ========== DỊCH VỤ ==========
        // Lấy danh sách tất cả dịch vụ
        public DataTable GetAllDichVu()
        {
            try
            {
                return _dal.GetAllDichVu();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetAllDichVu: {ex.Message}", ex);
            }
        }

        // Lấy danh sách dịch vụ trong gói tiệc
        public DataTable GetDichVuTrongGoi(int goiId)
        {
            try
            {
                if (goiId <= 0)
                    throw new ArgumentException("ID gói tiệc không hợp lệ!");
                
                return _dal.GetDichVuTrongGoi(goiId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetDichVuTrongGoi: {ex.Message}", ex);
            }
        }

        // Thêm dịch vụ vào gói
        public void ThemDichVuVaoGoi(int goiId, string maDv)
        {
            if (goiId <= 0) throw new ArgumentException("goiId không hợp lệ");

            int dvId = _dal.GetDichVuIdByMaDv(maDv);
            if (dvId <= 0) throw new Exception("Không tìm thấy mã dịch vụ: " + maDv);

            _dal.ThemDichVuVaoGoi(goiId, dvId);
        }

        // Xóa dịch vụ khỏi gói
        public void XoaDichVuKhoiGoi(int goiId, string maDv)
        {
            if (goiId <= 0) throw new ArgumentException("goiId không hợp lệ");

            int dvId = _dal.GetDichVuIdByMaDv(maDv);
            if (dvId <= 0) throw new Exception("Không tìm thấy mã dịch vụ: " + maDv);

            _dal.XoaDichVuKhoiGoi(goiId, dvId);
        }
    }
}