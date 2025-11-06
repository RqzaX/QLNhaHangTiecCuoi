using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ChuongTrinhKMBLL
    {
        private ChuongTrinhKMDAL _dal;

        public ChuongTrinhKMBLL()
        {
            _dal = new ChuongTrinhKMDAL();
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

        public bool Add(string maKm, string ten, string hinhThuc, decimal giaTri,
                        DateTime tgBatDau, DateTime tgKetThuc, string apDungLoai)
        {
            try
            {
                if (string.IsNullOrEmpty(maKm?.Trim()))
                    throw new Exception("Mã khuyến mãi không được trống");
                if (string.IsNullOrEmpty(ten?.Trim()))
                    throw new Exception("Tên khuyến mãi không được trống");
                if (giaTri < 0)
                    throw new Exception("Giá trị khuyến mãi không được âm");
                if (tgKetThuc <= tgBatDau)
                    throw new Exception("Thời gian kết thúc phải sau thời gian bắt đầu");

                return _dal.Insert(maKm.Trim(), ten.Trim(), hinhThuc, giaTri, tgBatDau, tgKetThuc, apDungLoai);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL Add: " + ex.Message);
            }
        }

        public bool Update(int kmId, string maKm, string ten, string hinhThuc, decimal giaTri,
                           DateTime tgBatDau, DateTime tgKetThuc, string apDungLoai)
        {
            try
            {
                if (kmId <= 0)
                    throw new Exception("ID khuyến mãi không hợp lệ");
                if (string.IsNullOrEmpty(maKm?.Trim()))
                    throw new Exception("Mã khuyến mãi không được trống");
                if (string.IsNullOrEmpty(ten?.Trim()))
                    throw new Exception("Tên khuyến mãi không được trống");
                if (giaTri < 0)
                    throw new Exception("Giá trị khuyến mãi không được âm");
                if (tgKetThuc <= tgBatDau)
                    throw new Exception("Thời gian kết thúc phải sau thời gian bắt đầu");

                return _dal.Update(kmId, maKm.Trim(), ten.Trim(), hinhThuc, giaTri, tgBatDau, tgKetThuc, apDungLoai);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL Update: " + ex.Message);
            }
        }

        public int CountVouchersByKmId(int kmId)
        {
            try
            {
                if (kmId <= 0)
                    throw new Exception("ID khuyến mãi không hợp lệ");

                return _dal.CountVouchersByKmId(kmId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL CountVouchersByKmId: " + ex.Message);
            }
        }

        public bool Delete(int kmId)
        {
            try
            {
                if (kmId <= 0)
                    throw new Exception("ID khuyến mãi không hợp lệ");

                return _dal.Delete(kmId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL Delete: " + ex.Message);
            }
        }

        public bool MaKmExists(string maKm, int? excludeKmId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(maKm?.Trim()))
                    return false;

                return _dal.MaKmExists(maKm.Trim(), excludeKmId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL MaKmExists: " + ex.Message);
            }
        }

        public DataRow GetById(int kmId)
        {
            try
            {
                if (kmId <= 0)
                    throw new Exception("ID khuyến mãi không hợp lệ");

                return _dal.GetById(kmId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL GetById: " + ex.Message);
            }
        }
    }
}
