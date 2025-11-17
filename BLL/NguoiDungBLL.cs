using System;
using System.Data;
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
    }
}