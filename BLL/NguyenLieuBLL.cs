using System.Data;
using QLNhaHangTiecCuoi.DAL;
using QLNhaHangTiecCuoi.Share;

namespace QLNhaHangTiecCuoi.BLL
{
    public class NguyenLieuBLL
    {
        private readonly NguyenLieuDAL _dal;
        private const decimal NGUONG_CANH_BAO_MAC_DINH = 5m;

        public NguyenLieuBLL(DatabaseHelper dbHelper)
        {
            _dal = new NguyenLieuDAL(dbHelper);
        }

        public DataTable LayDanhMuc() => _dal.GetDanhMuc();

        public DataTable LayTonKhoTheoTinhTrang(int tinhTrang, int? chiNhanhId = null, decimal? canhBao = null)
    => _dal.GetTonKhoByTinhTrang(tinhTrang, canhBao ?? NGUONG_CANH_BAO_MAC_DINH, chiNhanhId);

        public DataTable TimKiemTheoTinhTrang(string keyword, int tinhTrang, int? chiNhanhId = null, decimal? canhBao = null)
            => _dal.SearchByTinhTrang(keyword, tinhTrang, canhBao ?? NGUONG_CANH_BAO_MAC_DINH, chiNhanhId);
        // Chi tiết
        public DataTable LayTheoIdCoTon(int nlId) => _dal.GetByIdWithTon(nlId);
        public DataTable LayTonKhoTheoChiNhanhCuaNguyenLieu(int nlId) => _dal.GetTonKhoTheoChiNhanhCuaNguyenLieu(nlId);

        // Cập nhật
        public int Sua(int nlId, string ma, string ten, string donVi) => _dal.Update(nlId, ma, ten, donVi);
        public decimal LayTonKhoTaiChiNhanh(int chiNhanhId, int nlId)
     => _dal.GetTonKho(chiNhanhId, nlId);

        public int CapNhatTonKho(int chiNhanhId, int nlId, decimal slTon)
            => _dal.UpsertTonKho(chiNhanhId, nlId, slTon);
        public DataRow LayNguyenLieuById(int nlId)
        {
            var dt = _dal.GetNguyenLieuById(nlId);
            return (dt != null && dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }
    }
}
