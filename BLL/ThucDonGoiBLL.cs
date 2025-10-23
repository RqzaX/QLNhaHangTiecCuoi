using System;
using System.Data;
using DAL;


namespace BLL
{
    public class ThucDonGoiBLL
    {
        private readonly ThucDonGoiDAL _dal;

        public ThucDonGoiBLL(string connectionString = null)
        {
            _dal = new ThucDonGoiDAL(connectionString);
        }

        // Test kết nối
        public bool TestConnection()
        {
            try
            {
                return _dal.TestConnection();
            }
            catch
            {
                return false;
            }
        }

        // ... các method khác giữ nguyên như cũ

        public DataTable GetDanhSachMonAn()
        {
            try
            {
                return _dal.GetDanhSachMonAn();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetDanhSachMonAn: {ex.Message}", ex);
            }
        }

        public DataTable GetDanhSachGoiTiec()
        {
            try
            {
                return _dal.GetDanhSachGoiTiec();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetDanhSachGoiTiec: {ex.Message}", ex);
            }
        }

        public bool XoaGoiTiec(int goiId)
        {
            try
            {
                return _dal.XoaGoiTiec(goiId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - XoaGoiTiec: {ex.Message}", ex);
            }
        }

        public DataRow GetChiTietMonAn(int monId)
        {
            try
            {
                return _dal.GetChiTietMonAn(monId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetChiTietMonAn: {ex.Message}", ex);
            }
        }

        public DataRow GetChiTietGoiTiec(int goiId)
        {
            try
            {
                return _dal.GetChiTietGoiTiec(goiId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - GetChiTietGoiTiec: {ex.Message}", ex);
            }
        }

        public DataTable TimKiemMonAn(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return GetDanhSachMonAn();

                return _dal.TimKiemMonAn(keyword.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - TimKiemMonAn: {ex.Message}", ex);
            }
        }

        public DataTable TimKiemGoiTiec(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return GetDanhSachGoiTiec();

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

        public int ThemMonAn(string maMon, string tenMon, string nhom, string donViTinh, decimal donGia, bool dangBan)
        {
            if (string.IsNullOrWhiteSpace(maMon)) throw new ArgumentException("Mã món không được trống.");
            if (string.IsNullOrWhiteSpace(tenMon)) throw new ArgumentException("Tên món không được trống.");
            if (string.IsNullOrWhiteSpace(donViTinh)) throw new ArgumentException("Đơn vị tính không được trống.");
            if (donGia < 0) throw new ArgumentException("Đơn giá không hợp lệ.");

            return _dal.ThemMonAn(maMon.Trim(), tenMon.Trim(), nhom?.Trim(), donViTinh.Trim(), donGia, dangBan);
        }

        public int CapNhatMonAn(int monId, string maMon, string tenMon, string nhom, string donViTinh, decimal donGia, bool dangBan)
        {
            if (monId <= 0) throw new ArgumentException("ID món không hợp lệ.");
            if (string.IsNullOrWhiteSpace(maMon)) throw new ArgumentException("Mã món không được trống.");
            if (string.IsNullOrWhiteSpace(tenMon)) throw new ArgumentException("Tên món không được trống.");
            if (string.IsNullOrWhiteSpace(donViTinh)) throw new ArgumentException("Đơn vị tính không được trống.");
            if (donGia < 0) throw new ArgumentException("Đơn giá không hợp lệ.");

            return _dal.CapNhatMonAn(monId, maMon.Trim(), tenMon.Trim(), nhom?.Trim(), donViTinh.Trim(), donGia, dangBan);
        }

        public int XoaMonAn(int monId)
        {
            if (monId <= 0) throw new ArgumentException("ID món không hợp lệ.");
            return _dal.XoaMonAn(monId);
        }
        public (string MaMon, string TenMon, string? Nhom, string DonViTinh, decimal DonGia, bool DangBan)?
    GetMonAnById(int monId)
        {
            if (monId <= 0) return null;
            var r = _dal.GetMonAnByIdRow(monId);
            if (r == null) return null;

            string ma = Convert.ToString(r["ma_mon"]) ?? "";
            string ten = Convert.ToString(r["ten_mon"]) ?? "";
            string? nhom = r["nhom"] == DBNull.Value ? null : Convert.ToString(r["nhom"]);
            string dvt = Convert.ToString(r["don_vi_tinh"]) ?? "";
            decimal gia = r["don_gia"] == DBNull.Value ? 0 : Convert.ToDecimal(r["don_gia"]);
            bool dang = r["dang_ban"] != DBNull.Value && Convert.ToInt32(r["dang_ban"]) == 1;

            return (ma, ten, nhom, dvt, gia, dang);
        }
    }
}