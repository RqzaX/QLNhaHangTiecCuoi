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

        public bool Add(int kmId, string code, int soLan, DateTime? hanDung)
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

                return _dal.Insert(kmId, code.Trim().ToUpper(), soLan, hanDung);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL Add: " + ex.Message);
            }
        }

        public bool Update(int voucherId, int kmId, string code, int soLan, DateTime? hanDung)
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

                return _dal.Update(voucherId, kmId, code.Trim().ToUpper(), soLan, hanDung);
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
    }
}
