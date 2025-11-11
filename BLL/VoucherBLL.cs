using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class VoucherBLL
    {
        private VoucherDAL _dal;

        public VoucherBLL()
        {
            _dal = new VoucherDAL();
        }

        public DataTable LoadData()
        {
            try
            {
                return _dal.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL LoadData: " + ex.Message);
            }
        }

        public DataTable GetChuongTrinhKM()
        {
            try
            {
                return _dal.GetChuongTrinhKM();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL GetChuongTrinhKM: " + ex.Message);
            }
        }

        public int Add(int kmId, string code, int soLan, DateTime? hanDung)
        {
            try
            {
                if (kmId <= 0)
                    throw new Exception("Chương trình khuyến mãi không được trống");
                if (string.IsNullOrEmpty(code?.Trim()))
                    throw new Exception("Mã voucher không được trống");
                if (_dal.CodeExists(code))
                    throw new Exception("Mã voucher đã tồn tại");
                if (soLan <= 0)
                    throw new Exception("Số lần phải lớn hơn 0");
                if (hanDung.HasValue && hanDung.Value < DateTime.Now.Date)
                    throw new Exception("Hạn dùng phải từ hôm nay trở đi");

                DataRow kmRow = _dal.GetChuongTrinhKMById(kmId);
                if (kmRow != null)
                {
                    // Đảm bảo chỉ so sánh phần Date (bỏ phần time) để so sánh chính xác
                    DateTime tgBatDau = kmRow["tg_bat_dau"] == DBNull.Value ? DateTime.Now.Date : Convert.ToDateTime(kmRow["tg_bat_dau"]).Date;
                    DateTime tgKetThuc = kmRow["tg_ket_thuc"] == DBNull.Value ? DateTime.Now.Date : Convert.ToDateTime(kmRow["tg_ket_thuc"]).Date;
                    DateTime now = DateTime.Now.Date;

                    // Chỉ cho phép tạo voucher khi CTKM đang trong thời gian áp dụng
                    // Điều kiện: tgBatDau <= now && tgKetThuc >= now
                    if (tgBatDau > now)
                    {
                        throw new Exception($"Chương trình khuyến mãi chưa bắt đầu (bắt đầu từ {tgBatDau:dd/MM/yyyy}). Chỉ có thể tạo voucher khi chương trình đang áp dụng.");
                    }
                    if (tgKetThuc < now)
                    {
                        throw new Exception($"Chương trình khuyến mãi đã kết thúc (kết thúc ngày {tgKetThuc:dd/MM/yyyy}). Chỉ có thể tạo voucher khi chương trình đang áp dụng.");
                    }
                }

                int voucherId = _dal.Insert(kmId, code.Trim().ToUpper(), soLan, hanDung);
                return voucherId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL Add: " + ex.Message);
            }
        }

        public bool Update(int voucherId, int kmId, string code, int soLan, DateTime? hanDung, int? daDung = null)
        {
            try
            {
                if (voucherId <= 0)
                    throw new Exception("ID voucher không hợp lệ");
                if (kmId <= 0)
                    throw new Exception("Chương trình khuyến mãi không được trống");
                if (string.IsNullOrEmpty(code?.Trim()))
                    throw new Exception("Mã voucher không được trống");
                if (_dal.CodeExists(code, voucherId))
                    throw new Exception("Mã voucher đã tồn tại");
                if (soLan <= 0)
                    throw new Exception("Số lần phải lớn hơn 0");
                if (hanDung.HasValue && hanDung.Value < DateTime.Now.Date)
                    throw new Exception("Hạn dùng phải từ hôm nay trở đi");
                if (daDung.HasValue && daDung.Value < 0)
                    throw new Exception("Số lượt đã dùng không được nhỏ hơn 0");
                if (daDung.HasValue && soLan > 0 && daDung.Value > soLan)
                    throw new Exception("Số lượt đã dùng không được lớn hơn số lượt dùng");

                return _dal.Update(voucherId, kmId, code.Trim().ToUpper(), soLan, hanDung, daDung);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL Update: " + ex.Message);
            }
        }

        public bool Delete(int voucherId)
        {
            try
            {
                if (voucherId <= 0)
                    throw new Exception("ID voucher không hợp lệ");

                return _dal.Delete(voucherId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL Delete: " + ex.Message);
            }
        }

        public DataRow GetById(int voucherId)
        {
            try
            {
                if (voucherId <= 0)
                    throw new Exception("ID voucher không hợp lệ");

                return _dal.GetById(voucherId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL GetById: " + ex.Message);
            }
        }
    }
}
