using System;
using System.Data;
using DAL;
using QLNhaHangTiecCuoi.Share;

namespace BLL
{
    public class KhuyenMaiResult
    {
        public bool IsApplied { get; set; }
        public int? VoucherId { get; set; }
        public int? ProgramId { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string ProgramCode { get; set; } = string.Empty;
        public string DiscountType { get; set; } = string.Empty; // PERCENT | AMOUNT
        public decimal DiscountValue { get; set; }
        public string ApplyScope { get; set; } = "ALL";
        public string Error { get; set; } = string.Empty;
    }

    public class KhuyenMaiBLL
    {
        private readonly KhuyenMaiDAL _dal;

        public KhuyenMaiBLL(DatabaseHelper db)
        {
            _dal = new KhuyenMaiDAL(db);
        }

        // Lấy dữ liệu mẫu hiển thị
        public (DataTable km, DataTable vc) GetPreview()
        {
            return (_dal.GetOneActiveProgramPreview(), _dal.GetOneActiveVoucherPreview());
        }

        // Danh sách đầy đủ
        public DataTable GetActivePrograms(string scope = null)
        {
            return _dal.GetActivePrograms(scope);
        }

        public DataTable GetActiveVouchers(string scope = null)
        {
            return _dal.GetActiveVouchers(scope);
        }

        // Tất cả CTKM & voucher (không lọc theo hiệu lực)
        public DataTable GetAllPrograms()
        {
            return _dal.GetAllPrograms();
        }

        public DataTable GetAllVouchers()
        {
            return _dal.GetAllVouchers();
        }

        // Kiểm tra và áp dụng voucher code
        public KhuyenMaiResult ApplyVoucherCode(string code)
        {
            var result = new KhuyenMaiResult();
            var dt = _dal.FindVoucherByCode(code);
            if (dt.Rows.Count == 0)
            {
                result.Error = "Mã voucher không tồn tại.";
                return result;
            }

            var r = dt.Rows[0];
            DateTime now = DateTime.UtcNow;
            DateTime tgBd = Convert.ToDateTime(r["tg_bat_dau"]);
            DateTime tgKt = Convert.ToDateTime(r["tg_ket_thuc"]);
            DateTime? han = r["han_dung"] == DBNull.Value ? null : Convert.ToDateTime(r["han_dung"]);

            if (!(now >= tgBd && now <= tgKt))
            {
                result.Error = "Chương trình khuyến mãi đã hết hiệu lực.";
                return result;
            }
            if (han.HasValue && han.Value < now.Date)
            {
                result.Error = "Voucher đã hết hạn sử dụng.";
                return result;
            }
            int soLan = Convert.ToInt32(r["so_lan"]);
            int daDung = Convert.ToInt32(r["da_dung"]);
            if (daDung >= soLan)
            {
                result.Error = "Voucher đã sử dụng hết số lượt cho phép.";
                return result;
            }

            result.IsApplied = true;
            result.VoucherId = Convert.ToInt32(r["voucher_id"]);
            result.ProgramId = Convert.ToInt32(r["km_id"]);
            result.ProgramName = r["ten"].ToString() ?? string.Empty;
            result.ProgramCode = r["code"].ToString() ?? string.Empty;
            result.DiscountType = r["hinh_thuc"].ToString() ?? string.Empty;
            result.DiscountValue = Convert.ToDecimal(r["gia_tri"]);
            result.ApplyScope = r["ap_dung_loai"].ToString() ?? "ALL";
            return result;
        }
    }
}


