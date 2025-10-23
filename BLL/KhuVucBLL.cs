using System;
using System.Data;
using QLNhaHangTiecCuoi.DAL;

namespace QLNhaHangTiecCuoi.BLL
{
    public class KhuVucBLL
    {
        private KhuVucDAL _khuVucDAL;

        public KhuVucBLL()
        {
            _khuVucDAL = new KhuVucDAL();
        }

        public DataTable LayDanhSachKhuVuc(int chiNhanhId)
        {
            try
            {
                return _khuVucDAL.LayDanhSachKhuVuc(chiNhanhId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - Lấy danh sách khu vực: {ex.Message}");
            }
        }
    }
}
