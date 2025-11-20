using System.Collections.Generic;

namespace UI.Common
{
    public static class Session
    {
        public static int NguoiDungId { get; set; }
        public static string TaiKhoan { get; set; }
        public static string HoTen { get; set; }
        public static int ChiNhanhId { get; set; }
        public static string TenChiNhanh { get; set; }
        public static List<string> VaiTro { get; set; } = new List<string>();
        
        public static bool HasRole(string roleCode)
        {
            return VaiTro != null && VaiTro.Contains(roleCode);
        }
        
        public static bool HasAnyRole(params string[] roleCodes)
        {
            if (VaiTro == null) return false;
            foreach (var role in roleCodes)
            {
                if (VaiTro.Contains(role)) return true;
            }
            return false;
        }
    }
}