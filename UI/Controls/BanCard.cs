using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [ToolboxItem(true)]
    [SupportedOSPlatform("windows")]
    public class BanCard : UserControl
    {
        private string _soBan = "A01";
        private string _khuVuc = "Khu vực A - Tầng 1";
        private int _sucChua = 4;
        private string _trangThai = "Trống";
        private bool _hover = false;

        [Category("Data")]
        public string SoBan
        {
            get => _soBan;
            set { _soBan = value ?? ""; Invalidate(); }
        }

        [Category("Data")]
        public string KhuVuc
        {
            get => _khuVuc;
            set { _khuVuc = value ?? ""; Invalidate(); }
        }

        [Category("Data")]
        public int SucChua
        {
            get => _sucChua;
            set { _sucChua = value; Invalidate(); }
        }

        [Category("Data")]
        public string TrangThai
        {
            get => _trangThai;
            set { _trangThai = value ?? ""; Invalidate(); }
        }

        public BanCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            UpdateStyles();

            Size = new Size(550, 80);
            BackColor = Color.White;
            Cursor = Cursors.Hand;

            MouseEnter += (s, e) => { _hover = true; Invalidate(); };
            MouseLeave += (s, e) => { _hover = false; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var rect = ClientRectangle;
            rect.Inflate(-1, -1);

            // Background - light grey
            Color bgColor = _hover ? Color.FromArgb(245, 245, 245) : Color.FromArgb(249, 250, 251);
            using (var bgBrush = new SolidBrush(bgColor))
            using (var path = RoundRect(rect, 12))
            {
                g.FillPath(bgBrush, path);
            }

            int x = 16;
            int y = (Height - 48) / 2; // Center vertically

            // Icon (left side) - very light green background
            Color iconBgColor = Color.FromArgb(240, 253, 244); // Very light green
            Color iconSymbolColor = Color.FromArgb(34, 197, 94); // Bright green

            int iconSize = 48;
            var iconRect = new Rectangle(x, y, iconSize, iconSize);
            using (var iconPath = RoundRect(iconRect, 8))
            using (var iconBrush = new SolidBrush(iconBgColor))
            {
                g.FillPath(iconBrush, iconPath);
            }

            // Icon symbols (Ψ and α stacked)
            using (var symbolFont = new Font("Segoe UI Symbol", 18, FontStyle.Bold))
            using (var symbolBrush = new SolidBrush(iconSymbolColor))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                // Draw Ψ (top)
                var psiRect = new RectangleF(iconRect.X, iconRect.Y + 2, iconRect.Width, iconRect.Height / 2 - 2);
                g.DrawString("Ψ", symbolFont, symbolBrush, psiRect, sf);
                // Draw α (bottom)
                var alphaRect = new RectangleF(iconRect.X, iconRect.Y + iconRect.Height / 2, iconRect.Width, iconRect.Height / 2);
                using (var alphaFont = new Font("Segoe UI Symbol", 16, FontStyle.Bold))
                {
                    g.DrawString("α", alphaFont, symbolBrush, alphaRect, sf);
                }
            }

            x += iconSize + 16;
            y = 16; // Reset y for text

            // Table code (B01) - large, bold, dark grey
            using (var font = new Font("Segoe UI", 16, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(55, 65, 81)))
            {
                g.DrawString(_soBan, font, brush, x, y);
            }

            // Location and capacity - smaller, regular, dark grey
            y += 30;
            // Extract just the area name (e.g., "Khu A" from "Khu A - Tầng 1")
            string khuVucShort = _khuVuc;
            if (_khuVuc.Contains("•"))
            {
                khuVucShort = _khuVuc.Split('•')[0].Trim();
            }
            else if (_khuVuc.Contains("-"))
            {
                khuVucShort = _khuVuc.Split('-')[0].Trim();
            }
            string locationText = $"{khuVucShort} • {_sucChua} người";
            using (var font = new Font("Segoe UI", 11))
            using (var brush = new SolidBrush(Color.FromArgb(107, 114, 128)))
            {
                g.DrawString(locationText, font, brush, x, y);
            }

            // Status badge (right side) - light green pill with bright green text
            string statusText = _trangThai.ToUpper();
            Color statusBgColor, statusTextColor;
            if (statusText.Contains("TRỐNG") || statusText.Contains("VỆ SINH"))
            {
                statusBgColor = Color.FromArgb(220, 252, 231); // Light green
                statusTextColor = Color.FromArgb(22, 163, 74); // Bright green
            }
            else if (statusText.Contains("PHỤC VỤ") || statusText.Contains("ĐANG"))
            {
                statusBgColor = Color.FromArgb(219, 234, 254);
                statusTextColor = Color.FromArgb(37, 99, 235);
            }
            else
            {
                statusBgColor = Color.FromArgb(254, 243, 199);
                statusTextColor = Color.FromArgb(217, 119, 6);
            }

            using (var statusFont = new Font("Segoe UI", 9, FontStyle.Bold))
            {
                var statusSize = g.MeasureString(statusText, statusFont);
                int statusWidth = (int)statusSize.Width + 20;
                int statusHeight = 26;
                int statusX = Width - statusWidth - 50; // Leave space for edit icon
                int statusY = (Height - statusHeight) / 2;

                var statusRect = new Rectangle(statusX, statusY, statusWidth, statusHeight);
                using (var statusPath = RoundRect(statusRect, 13)) // More rounded (pill shape)
                using (var statusBrush = new SolidBrush(statusBgColor))
                {
                    g.FillPath(statusBrush, statusPath);
                }

                var statusTextRect = new RectangleF(statusRect.X, statusRect.Y, statusRect.Width, statusRect.Height);
                var statusSf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                using (var statusTextBrush = new SolidBrush(statusTextColor))
                {
                    g.DrawString(statusText, statusFont, statusTextBrush, statusTextRect, statusSf);
                }
            }

            // Edit icon (pencil) - outlined style
            int iconX = Width - 32;
            int iconY = (Height - 16) / 2;
            using (var iconPen = new Pen(Color.FromArgb(107, 114, 128), 1.5f))
            {
                // Draw pencil icon as simple outline
                Point[] pencilPoints = new Point[]
                {
                    new Point(iconX, iconY - 4),
                    new Point(iconX + 8, iconY - 4),
                    new Point(iconX + 10, iconY - 2),
                    new Point(iconX + 10, iconY + 2),
                    new Point(iconX + 8, iconY + 4),
                    new Point(iconX, iconY + 4),
                    new Point(iconX, iconY - 4)
                };
                g.DrawLines(iconPen, pencilPoints);
                // Draw pencil tip
                g.DrawLine(iconPen, iconX + 8, iconY, iconX + 12, iconY - 4);
            }
        }

        private GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var gp = new GraphicsPath();
            if (radius <= 0)
            {
                gp.AddRectangle(r);
                gp.CloseFigure();
                return gp;
            }
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
    }
}

