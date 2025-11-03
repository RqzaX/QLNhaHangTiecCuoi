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
   
        public DataTable LayTheoIdCoTon(int nlId) => _dal.GetByIdWithTon(nlId);
        public DataTable LayTonKhoTheoChiNhanhCuaNguyenLieu(int nlId) => _dal.GetTonKhoTheoChiNhanhCuaNguyenLieu(nlId);

       
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

     
        public DataTable LayChiNhanhCoDuLieuTonKho()
        {
            try
            {
         
                var result = new DataTable();
                result.Columns.Add("chi_nhanh_id", typeof(int));
                result.Columns.Add("ten", typeof(string));
                result.Columns.Add("co_du_lieu", typeof(bool));

               
                string sql = "SELECT chi_nhanh_id, ten FROM dbo.chi_nhanh WHERE trang_thai = 1 ORDER BY chi_nhanh_id";
                var allBranches = _dal.GetDataTable(sql);
                
                if (allBranches != null)
                {
                    foreach (System.Data.DataRow row in allBranches.Rows)
                    {
                        int branchId = Convert.ToInt32(row["chi_nhanh_id"]);
                        string branchName = row["ten"].ToString();
                        
                      
                        var inventoryData = LayTonKhoTheoTinhTrang(0, branchId);
                        bool hasInventory = false;
                        
                        if (inventoryData != null && inventoryData.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow invRow in inventoryData.Rows)
                            {
                                if (invRow["sl_ton"] != DBNull.Value && Convert.ToDecimal(invRow["sl_ton"]) > 0)
                                {
                                    hasInventory = true;
                                    break;
                                }
                            }
                        }
                        
                        result.Rows.Add(branchId, branchName, hasInventory);
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - LayChiNhanhCoDuLieuTonKho: {ex.Message}", ex);
            }
        }

        
        public DataTable LayTatCaChiNhanh()
        {
            try
            {
                string sql = "SELECT chi_nhanh_id, ten FROM dbo.chi_nhanh WHERE trang_thai = 1 ORDER BY chi_nhanh_id";
                return _dal.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - LayTatCaChiNhanh: {ex.Message}", ex);
            }
        }


        public int NhapKho(int chiNhanhId, int nlId, decimal soLuong)
        {
            try
            {
                return _dal.NhapKho(chiNhanhId, nlId, soLuong);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - NhapKho: {ex.Message}", ex);
            }
        }

     
        public int XuatKho(int chiNhanhId, int nlId, decimal soLuong)
        {
            try
            {
                return _dal.XuatKho(chiNhanhId, nlId, soLuong);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - XuatKho: {ex.Message}", ex);
            }
        }

        
        public int ChuyenKho(int chiNhanhNguonId, int chiNhanhDichId, int nlId, decimal soLuong)
        {
            try
            {
                return _dal.ChuyenKho(chiNhanhNguonId, chiNhanhDichId, nlId, soLuong);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - ChuyenKho: {ex.Message}", ex);
            }
        }

        public bool KiemTraTonKhoDu(int chiNhanhId, int nlId, decimal soLuongCan)
        {
            try
            {
                decimal tonHienTai = LayTonKhoTaiChiNhanh(chiNhanhId, nlId);
                return tonHienTai >= soLuongCan;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL - KiemTraTonKhoDu: {ex.Message}", ex);
            }
        }
    }
}
