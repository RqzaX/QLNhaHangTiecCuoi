using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;

[SupportedOSPlatform("windows")]
public class RoundedBorderForm : Form
{
    // Public properties để dễ tùy chỉnh từ code/designer
    public int CornerRadius { get; set; } = 14;
    public Color BorderColor { get; set; } = Color.Black;
    public int BorderThickness { get; set; } = 2;
    public bool ShowDropShadow { get; set; } = false; // optional DWM shadow

    public RoundedBorderForm()
    {
        // No default OS border
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        DoubleBuffered = true;
        BackColor = Color.White;
        // cho phép kéo form bằng bất kỳ chỗ nào (mouse down handler)
        MouseDown += RoundedBorderForm_MouseDown;
        // update region khi kích thước thay đổi
        Resize += (s, e) => UpdateRegion();
        // khi paint -> vẽ viền
        Paint += RoundedBorderForm_Paint;
        // enable drop shadow if asked
        if (ShowDropShadow) EnableDropShadow();
    }

    private void RoundedBorderForm_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        // vẽ viền (inside)
        if (BorderThickness > 0)
        {
            using (var pen = new Pen(BorderColor, BorderThickness))
            {
                // vẽ đường viền nằm chính giữa viền của rect (offset nửa đường kính bút)
                var half = Math.Max(1, BorderThickness) / 2f;
                var r = new RectangleF(half, half, ClientSize.Width - BorderThickness, ClientSize.Height - BorderThickness);
                using (var path = GetRoundedRectPath(Rectangle.Round(r), Math.Max(0, CornerRadius)))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }

    private void UpdateRegion()
    {
        // đặt Region để form có góc bo tròn thực sự (mouse events / hit test đúng)
        using (var path = GetRoundedRectPath(ClientRectangle, Math.Max(0, CornerRadius)))
        {
            this.Region?.Dispose();
            this.Region = new Region(path);
        }
        Invalidate();
    }

    // Tạo GraphicsPath hình chữ nhật bo tròn
    private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        if (radius <= 0)
        {
            path.AddRectangle(rect);
            path.CloseFigure();
            return path;
        }

        int d = radius * 2;
        Rectangle arc = new Rectangle(rect.Location, new Size(d, d));

        // top-left
        path.AddArc(arc, 180, 90);

        // top-right
        arc.X = rect.Right - d;
        path.AddArc(arc, 270, 90);

        // bottom-right
        arc.Y = rect.Bottom - d;
        path.AddArc(arc, 0, 90);

        // bottom-left
        arc.X = rect.Left;
        path.AddArc(arc, 90, 90);

        path.CloseFigure();
        return path;
    }

    // --- drag window when mouse down anywhere on the form ---
    private void RoundedBorderForm_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, IntPtr.Zero);
        }
    }

    // P/Invoke để kéo form
    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    private const int WM_NCLBUTTONDOWN = 0xA1;
    private static readonly IntPtr HTCAPTION = new IntPtr(0x2);

    // --- optional: enable native drop shadow on Windows (DWM) ---
    [DllImport("dwmapi.dll")]
    private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);

    [StructLayout(LayoutKind.Sequential)]
    private struct MARGINS { public int left, right, top, bottom; }

    private void EnableDropShadow()
    {
        try
        {
            var margins = new MARGINS() { left = 1, right = 1, top = 1, bottom = 1 };
            DwmExtendFrameIntoClientArea(this.Handle, ref margins);
        }
        catch
        {
            // DWM không có (ví dụ khả năng trên Windows XP) -> bỏ qua
        }
    }

    // ---- tiện: override OnShown để áp region lần đầu ----
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        UpdateRegion();
    }
}
