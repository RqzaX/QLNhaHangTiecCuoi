using System;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.BLL
{
    public class NguoiDungBLL
    {
        private NguoiDungDAL _dal;

        public NguoiDungBLL(DatabaseHelper dbHelper)
        {
            _dal = new NguoiDungDAL(dbHelper);
        }

        public (bool success, string message, int nguoiDungId, string hoTen) XacThucDangNhap(string taiKhoan, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(taiKhoan))
                return (false, "Tài khoản không được để trống!", 0, "");

            if (string.IsNullOrWhiteSpace(matKhau))
                return (false, "Mật khẩu không được để trống!", 0, "");

            if (taiKhoan.Length < 3)
                return (false, "Tài khoản tối thiểu 3 ký tự!", 0, "");

            if (matKhau.Length < 6)
                return (false, "Mật khẩu tối thiểu 6 ký tự!", 0, "");

            try
            {
                DataTable dt = _dal.DangNhap(taiKhoan, matKhau);

                if (dt == null || dt.Rows.Count == 0)
                    return (false, "Tài khoản hoặc mật khẩu không đúng!", 0, "");

                int nguoiDungId = (int)dt.Rows[0]["nguoi_dung_id"];
                string hoTen = dt.Rows[0]["ho_ten"].ToString();

                // Cập nhật thông tin đăng nhập
                _dal.CapNhatDangNhapLanCuoi(nguoiDungId);

                return (true, "Đăng nhập thành công!", nguoiDungId, hoTen);
            }
            catch (Exception ex)
            {
                return (false, "Lỗi: " + ex.Message, 0, "");
            }
        }

        public DataTable LayVaiTroByNguoiDungId(int nguoiDungId)
        {
            try
            {
                return _dal.LayVaiTroByNguoiDungId(nguoiDungId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy vai trò: " + ex.Message);
            }
        }

        public DataTable LayDanhSachChiNhanh()
        {
            try
            {
                return _dal.LayDanhSachChiNhanh();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách chi nhánh: " + ex.Message);
            }
        }

        public DataTable LayChiNhanhById(int chiNhanhId)
        {
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            try
            {
                return _dal.LayChiNhanhById(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy chi nhánh: " + ex.Message);
            }
        }

        public DataTable LayChiNhanhTheoNguoiDung(int nguoiDungId)
        {
            if (nguoiDungId <= 0)
                throw new Exception("ID người dùng không hợp lệ!");

            try
            {
                return _dal.LayChiNhanhTheoNguoiDung(nguoiDungId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy chi nhánh theo người dùng: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên với tên, chức vụ và chi nhánh
        /// </summary>
        public DataTable LayDanhSachNhanVien()
        {
            try
            {
                return _dal.LayDanhSachNhanVien();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách phân ca cho nhân viên
        /// </summary>
        public DataTable LayDanhSachPhanCa()
        {
            try
            {
                return _dal.LayDanhSachPhanCa();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách phân ca: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên trong ca
        /// </summary>
        public DataTable LayNhanVienTrongCa(int caId, int chiNhanhId)
        {
            if (caId <= 0)
                throw new Exception("ID ca không hợp lệ!");
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            try
            {
                return _dal.LayNhanVienTrongCa(caId, chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách nhân viên trong ca: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên chưa có trong ca
        /// </summary>
        public DataTable LayNhanVienChuaTrongCa(int caId, int chiNhanhId)
        {
            if (caId <= 0)
                throw new Exception("ID ca không hợp lệ!");
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            try
            {
                return _dal.LayNhanVienChuaTrongCa(caId, chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách nhân viên chưa có trong ca: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm nhân viên vào ca
        /// </summary>
        public int ThemNhanVienVaoCa(int nguoiDungId, int chiNhanhId, int caId)
        {
            if (nguoiDungId <= 0)
                throw new Exception("ID người dùng không hợp lệ!");
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");
            if (caId <= 0)
                throw new Exception("ID ca không hợp lệ!");

            try
            {
                return _dal.ThemNhanVienVaoCa(nguoiDungId, chiNhanhId, caId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thêm nhân viên vào ca: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa nhân viên khỏi ca
        /// </summary>
        public bool XoaNhanVienKhoiCa(int nguoiDungCaId)
        {
            if (nguoiDungCaId <= 0)
                throw new Exception("ID phân ca không hợp lệ!");

            try
            {
                return _dal.XoaNhanVienKhoiCa(nguoiDungCaId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xóa nhân viên khỏi ca: " + ex.Message);
            }
        }

        // Lấy danh sách chức vụ (vai trò)
        public DataTable LayDanhSachChucVu()
        {
            return _dal.LayDanhSachChucVu();
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một nhân viên
        /// </summary>
        public DataTable LayThongTinNhanVien(int nguoiDungId)
        {
            if (nguoiDungId <= 0)
                throw new Exception("ID người dùng không hợp lệ!");

            try
            {
                return _dal.LayThongTinNhanVien(nguoiDungId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy thông tin nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm nhân viên mới (overload 1: với tài khoản và mật khẩu)
        /// </summary>
        public int ThemNhanVien(string hoTen, string taiKhoan, string matKhau, int vaiTroId, int chiNhanhId, bool hoatDong)
        {
            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ tên không được để trống!");
            if (string.IsNullOrWhiteSpace(taiKhoan))
                throw new Exception("Tài khoản không được để trống!");
            if (string.IsNullOrWhiteSpace(matKhau))
                throw new Exception("Mật khẩu không được để trống!");
            if (vaiTroId <= 0)
                throw new Exception("Vai trò không hợp lệ!");
            if (chiNhanhId <= 0)
                throw new Exception("Chi nhánh không hợp lệ!");

            try
            {
                return _dal.ThemNhanVien(hoTen, taiKhoan, matKhau, vaiTroId, chiNhanhId, hoatDong);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thêm nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Thêm nhân viên mới (overload 2: không có tài khoản/mật khẩu, tự tạo)
        /// </summary>
        public int ThemNhanVien(string hoTen, int vaiTroId, int chiNhanhId)
        {
            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ tên không được để trống!");
            if (vaiTroId <= 0)
                throw new Exception("Vai trò không hợp lệ!");
            if (chiNhanhId <= 0)
                throw new Exception("Chi nhánh không hợp lệ!");

            try
            {
                return _dal.ThemNhanVien(hoTen, vaiTroId, chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi thêm nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật nhân viên (overload 1: đầy đủ thông tin)
        /// </summary>
        public bool CapNhatNhanVien(int nguoiDungId, string hoTen, string taiKhoan, bool hoatDong, int vaiTroId, int chiNhanhId, string matKhau)
        {
            if (nguoiDungId <= 0)
                throw new Exception("ID người dùng không hợp lệ!");
            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ tên không được để trống!");
            if (string.IsNullOrWhiteSpace(taiKhoan))
                throw new Exception("Tài khoản không được để trống!");
            if (vaiTroId <= 0)
                throw new Exception("Vai trò không hợp lệ!");
            if (chiNhanhId <= 0)
                throw new Exception("Chi nhánh không hợp lệ!");

            try
            {
                return _dal.CapNhatNhanVien(nguoiDungId, hoTen, taiKhoan, hoatDong, vaiTroId, chiNhanhId, matKhau);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật nhân viên (overload 2: chỉ họ tên và vai trò)
        /// </summary>
        public bool CapNhatNhanVien(int nguoiDungId, string hoTen, int vaiTroId)
        {
            if (nguoiDungId <= 0)
                throw new Exception("ID người dùng không hợp lệ!");
            if (string.IsNullOrWhiteSpace(hoTen))
                throw new Exception("Họ tên không được để trống!");
            if (vaiTroId <= 0)
                throw new Exception("Vai trò không hợp lệ!");

            try
            {
                return _dal.CapNhatNhanVien(nguoiDungId, hoTen, vaiTroId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Xóa (vô hiệu hóa) nhân viên
        /// </summary>
        public bool XoaNhanVien(int nguoiDungId)
        {
            if (nguoiDungId <= 0)
                throw new Exception("ID người dùng không hợp lệ!");

            try
            {
                return _dal.XoaNhanVien(nguoiDungId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xóa nhân viên: " + ex.Message);
            }
        }

        /// <summary>
        /// Gán chi nhánh cho người dùng
        /// </summary>
        public bool GanChiNhanhChoNguoiDung(int nguoiDungId, int chiNhanhId)
        {
            if (nguoiDungId <= 0)
                throw new Exception("ID người dùng không hợp lệ!");
            if (chiNhanhId <= 0)
                throw new Exception("ID chi nhánh không hợp lệ!");

            try
            {
                return _dal.GanChiNhanhChoNguoiDung(nguoiDungId, chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi gán chi nhánh cho người dùng: " + ex.Message);
            }
        }
    }
}