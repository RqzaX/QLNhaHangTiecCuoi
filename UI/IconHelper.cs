using System.Drawing;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public static class IconHelper
    {
        public static async Task<Image> LoadImageAsync(string url, int size)
        {
            using (var httpClient = new HttpClient())
            {
                var data = await httpClient.GetByteArrayAsync(url);
                using (var ms = new System.IO.MemoryStream(data))
                {
                    var img = Image.FromStream(ms);
                    return new Bitmap(img, new Size(size, size));
                }
            }
        }
    }
}