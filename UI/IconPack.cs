using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
[SupportedOSPlatform("windows")]
public static class IconPack
{
    // Thư mục chứa icon offline
    public static readonly string IconDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons");

    // Kích thước icon dùng cho menu
    public static readonly Size IconSize = new Size(20, 20);

    // Danh sách icon nguồn (đơn sắc/đậm) – bạn có thể thay URL khác tùy bộ icon bạn thích
    private static readonly Dictionary<string, string> Sources = new Dictionary<string, string>
    {
        ["dashboard"] = "https://img.icons8.com/ios-filled/100/000000/dashboard.png",
        ["pos"] = "https://img.icons8.com/ios-filled/100/000000/pos-terminal.png",
        ["table"] = "https://img.icons8.com/ios-filled/100/000000/table.png",
        ["wedding"] = "https://img.icons8.com/ios-filled/100/000000/wedding-cake.png",
        ["contract"] = "https://img.icons8.com/ios-filled/100/000000/agreement.png",
        ["kitchen"] = "https://img.icons8.com/ios-filled/100/000000/chef-hat.png",
        ["invoice"] = "https://img.icons8.com/ios-filled/100/000000/bill.png",
        ["menu"] = "https://img.icons8.com/ios-filled/100/000000/restaurant-menu.png",
        ["warehouse"] = "https://img.icons8.com/ios-filled/100/000000/warehouse.png",
        ["discount"] = "https://img.icons8.com/ios-filled/100/000000/discount--v1.png",
        ["branch"] = "https://img.icons8.com/ios-filled/100/000000/city-buildings.png",
        ["customer"] = "https://img.icons8.com/ios-filled/100/000000/conference-call.png",
        ["staff"] = "https://img.icons8.com/ios-filled/100/000000/staff.png",
        ["report"] = "https://img.icons8.com/ios-filled/100/000000/combo-chart.png",
        ["settings"] = "https://img.icons8.com/ios-filled/100/000000/settings.png",
        ["shield"] = "https://img.icons8.com/ios-filled/100/000000/verified-account.png",
    };

    // Bảng màu “Office-ish / Fluent” cho từng chức năng (tùy chỉnh thoải mái)
    private static readonly Dictionary<string, Color> Palette = new Dictionary<string, Color>
    {
        ["dashboard"] = ColorTranslator.FromHtml("#1F6FEB"), // Primary (Azure blue)
        ["pos"] = ColorTranslator.FromHtml("#16A34A"), // Green
        ["table"] = ColorTranslator.FromHtml("#0284C7"), // Sky Blue
        ["wedding"] = ColorTranslator.FromHtml("#D946EF"), // Pink / Magenta
        ["contract"] = ColorTranslator.FromHtml("#7C3AED"), // Violet
        ["kitchen"] = ColorTranslator.FromHtml("#F59E0B"), // Amber
        ["invoice"] = ColorTranslator.FromHtml("#0EA5E9"), // Light Blue
        ["menu"] = ColorTranslator.FromHtml("#4F46E5"), // Indigo
        ["warehouse"] = ColorTranslator.FromHtml("#92400E"), // Brown
        ["discount"] = ColorTranslator.FromHtml("#EF4444"), // Red
        ["branch"] = ColorTranslator.FromHtml("#06B6D4"), // Cyan
        ["customer"] = ColorTranslator.FromHtml("#2563EB"), // Blue
        ["staff"] = ColorTranslator.FromHtml("#8B5CF6"), // Purple
        ["report"] = ColorTranslator.FromHtml("#1D4ED8"), // Navy-ish
        ["settings"] = ColorTranslator.FromHtml("#64748B"), // Slate Gray
        ["shield"] = ColorTranslator.FromHtml("#22C55E"), // Emerald
    };

    // Tải offline 1 lần (nếu chưa có)
    public static async Task EnsureDownloadedAsync()
    {
        if (!Directory.Exists(IconDir)) Directory.CreateDirectory(IconDir);

        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        foreach (var kv in Sources)
        {
            string key = kv.Key;
            string url = kv.Value;
            string path = Path.Combine(IconDir, key + ".png");

            if (File.Exists(path)) continue; // đã tải

            try
            {
                var bytes = await http.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(path, bytes);
            }
            catch
            {
                // Nếu tải lỗi → tạo ảnh rỗng để tránh văng app
                using var bmp = new Bitmap(IconSize.Width, IconSize.Height);
                bmp.Save(path, ImageFormat.Png);
            }
        }
    }

    // Nạp tất cả icon vào ImageList, tự scale & nhuộm màu theo Palette
    public static ImageList BuildImageListColored()
    {
        var list = new ImageList { ImageSize = IconSize, ColorDepth = ColorDepth.Depth32Bit };

        foreach (var kv in Sources)
        {
            string key = kv.Key;
            string path = Path.Combine(IconDir, key + ".png");
            if (!File.Exists(path))
            {
                // fallback ảnh rỗng
                using var blank = new Bitmap(IconSize.Width, IconSize.Height);
                list.Images.Add(key, (Image)blank.Clone());
                continue;
            }

            using var src = Image.FromFile(path);
            using var scaled = ResizeHighQuality(src, IconSize);
            var color = Palette.TryGetValue(key, out var c) ? c : Color.Black;
            using var tinted = TintMonochrome(scaled, color);

            list.Images.Add(key, (Image)tinted.Clone());
        }

        return list;
    }

    // Scale mượt
    private static Bitmap ResizeHighQuality(Image src, Size size)
    {
        var bmp = new Bitmap(size.Width, size.Height);
        using var g = Graphics.FromImage(bmp);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.Clear(Color.Transparent);
        g.DrawImage(src, new Rectangle(Point.Empty, size));
        return bmp;
    }

    // Nhuộm màu cho icon đơn sắc: giữ alpha, thay RGB = màu mong muốn
    private static Bitmap TintMonochrome(Image src, Color color)
    {
        var bmp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppPArgb);
        using var g = Graphics.FromImage(bmp);
        g.DrawImage(src, 0, 0);

        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                Color px = bmp.GetPixel(x, y);
                if (px.A == 0) continue; // trong suốt
                // Nếu nguồn là đen/đơn sắc, ta thay RGB bằng màu brand nhưng giữ alpha gốc
                bmp.SetPixel(x, y, Color.FromArgb(px.A, color.R, color.G, color.B));
            }
        }
        return bmp;
    }
}
