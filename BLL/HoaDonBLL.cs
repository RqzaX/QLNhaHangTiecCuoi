using System.Data;
using DAL;
using QLNhaHangTiecCuoi.Share;

namespace BLL
{
    public class HoaDonBLL
    {
        private readonly HoaDonDAL _dal;
        public HoaDonBLL(DatabaseHelper db) { _dal = new HoaDonDAL(db); }

        public DataTable GetHoaDonList(int chiNhanhId, string trangThai = "CHỜ TT", int top = 100)
        {
            return _dal.GetHoaDonList(chiNhanhId, trangThai, top);
        }

        public int GetWaitingInvoicesCount(int chiNhanhId)
        {
            return _dal.GetWaitingInvoicesCount(chiNhanhId);
        }

        public (int SoHd, decimal Tong) GetPaidStatsOnDateUtc(int chiNhanhId, DateTime dateUtc)
        {
            return _dal.GetPaidStatsOnDateUtc(chiNhanhId, dateUtc);
        }

        public DataTable GetPaidInvoicesHistory(int chiNhanhId, DateTime? fromDate = null, DateTime? toDate = null, string? phuongThuc = null, int top = 100)
        {
            return _dal.GetPaidInvoicesHistory(chiNhanhId, fromDate, toDate, phuongThuc, top);
        }

        public bool ProcessPayment(int hoaDonId, decimal soTien, string hinhThuc, int? kmId = null, int? voucherId = null, decimal? soTienKm = null)
        {
            return _dal.ProcessPayment(hoaDonId, soTien, hinhThuc, kmId, voucherId, soTienKm);
        }

        public DataRow? GetHoaDonById(int hoaDonId)
        {
            return _dal.GetHoaDonById(hoaDonId);
        }

        public DataTable GetHoaDonForKhachHang(int chiNhanhId, int top = 100)
        {
            return _dal.GetHoaDonForKhachHang(chiNhanhId, top);
        }
    }
}


