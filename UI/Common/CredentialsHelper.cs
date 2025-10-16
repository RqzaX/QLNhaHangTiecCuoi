using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace UI.Common
{
    /// <summary>
    /// Hỗ trợ lưu/tải thông tin đăng nhập (thay thế Settings)
    /// </summary>
    public static class CredentialsHelper
    {
        private static string _configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "QLNhaHang",
            "credentials.txt"
        );

        static CredentialsHelper()
        {
            // Tạo thư mục nếu không tồn tại
            string dir = Path.GetDirectoryName(_configPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// Lưu thông tin đăng nhập
        /// </summary>
        public static void SaveCredentials(string taiKhoan, string matKhau, bool luuThongTin)
        {
            try
            {
                if (luuThongTin)
                {
                    string data = $"{taiKhoan}|{matKhau}";
                    File.WriteAllText(_configPath, data, Encoding.UTF8);
                }
                else
                {
                    // Xóa file nếu tồn tại
                    if (File.Exists(_configPath))
                    {
                        File.Delete(_configPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu thông tin: " + ex.Message);
            }
        }

        /// <summary>
        /// Tải thông tin đăng nhập
        /// </summary>
        public static (bool found, string taiKhoan, string matKhau) LoadCredentials()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    return (false, "", "");
                }

                string data = File.ReadAllText(_configPath, Encoding.UTF8);
                if (string.IsNullOrEmpty(data))
                {
                    return (false, "", "");
                }

                string[] parts = data.Split('|');
                if (parts.Length != 2)
                {
                    return (false, "", "");
                }

                return (true, parts[0], parts[1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin: " + ex.Message);
                return (false, "", "");
            }
        }

        /// <summary>
        /// Xóa thông tin đã lưu
        /// </summary>
        public static void ClearCredentials()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    File.Delete(_configPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa thông tin: " + ex.Message);
            }
        }
    }
}