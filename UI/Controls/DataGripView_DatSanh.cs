using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    public class RowActionEventArgs : EventArgs
    {
        public int RowIndex { get; }
        public RowActionEventArgs(int rowIndex) => RowIndex = rowIndex;
    }
    [SupportedOSPlatform("windows")]
    [ToolboxItem(true)]
    public class DataGripView_DatSanh : DataGridView
    {
        // Chỉ số cột – gán trong Form sau khi tạo cột xong
        [Browsable(true), Category("Columns")]
        public int ColMaDonIndex { get; set; } = -1;

        [Browsable(true), Category("Columns")]
        public int ColTrangThaiIndex { get; set; } = -1;

        [Browsable(true), Category("Columns")]
        public int ColThaoTacIndex { get; set; } = -1;

        // Sự kiện click
        public event EventHandler<RowActionEventArgs>? DetailClicked;
        public event EventHandler<RowActionEventArgs>? ConfirmClicked;
        public event EventHandler<RowActionEventArgs>? OrderCodeClicked;

        // Style cho nút
        private const int BtnH = 30;
        private const int BtnWDetail = 88;
        private const int BtnWConfirm = 96;
        private const int Gap = 8;
        private const int PadRight = 10;
        private readonly Font _btnFont = new Font("Segoe UI", 9f, FontStyle.Bold);
        private readonly Font _pillFont = new Font("Segoe UI", 9f, FontStyle.Bold);

        [SupportedOSPlatform("windows")]
        public DataGripView_DatSanh()
        {
            // Chống flicker + cấu hình phù hợp vẽ custom
            DoubleBuffered = true;
            ReadOnly = true;
            EditMode = DataGridViewEditMode.EditProgrammatically;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AllowUserToAddRows = false;
            RowHeadersVisible = false;

            CellPainting += OnCellPainting_Custom;
            CellMouseClick += OnCellMouseClick_Custom;
            CellMouseMove += OnCellMouseMove_Custom;
        }

        // ======= VẼ NÚT & PILL TRẠNG THÁI =======
        private void OnCellPainting_Custom(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Vẽ pill trạng thái (nếu là cột trạng thái)
            if (ColTrangThaiIndex >= 0 && e.ColumnIndex == ColTrangThaiIndex)
            {
                e.Handled = true;
                e.PaintBackground(e.ClipBounds, true);

                string status = (Rows[e.RowIndex].Cells[ColTrangThaiIndex].Value ?? "").ToString();
                DrawStatusPill(e.Graphics, e.CellBounds, status);

                e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                return;
            }

            // Vẽ cột Thao tác
            if (ColThaoTacIndex >= 0 && e.ColumnIndex == ColThaoTacIndex)
            {
                e.Handled = true;
                e.PaintBackground(e.ClipBounds, true);

                var cell = e.CellBounds;
                int y = cell.Y + (cell.Height - BtnH) / 2;

                string status = (ColTrangThaiIndex >= 0
                    ? (Rows[e.RowIndex].Cells[ColTrangThaiIndex].Value ?? "").ToString()
                    : "");

                bool showConfirm = status == "Chờ xác nhận";

                Rectangle rConfirm = Rectangle.Empty;
                Rectangle rDetail;

                if (showConfirm)
                {
                    rConfirm = new Rectangle(cell.Right - BtnWConfirm - PadRight, y, BtnWConfirm, BtnH);
                    rDetail = new Rectangle(rConfirm.Left - Gap - BtnWDetail, y, BtnWDetail, BtnH);
                    DrawButton(e.Graphics, rDetail, "Chi tiết", false);
                    DrawButton(e.Graphics, rConfirm, "Xác nhận", true);
                }
                else
                {
                    rDetail = new Rectangle(cell.Right - BtnWDetail - PadRight, y, BtnWDetail, BtnH);
                    DrawButton(e.Graphics, rDetail, "Chi tiết", false);
                }

                e.Paint(e.CellBounds, DataGridViewPaintParts.Border);
                return;
            }
        }

        private static GraphicsPath RoundRect(Rectangle rect, int radius = 14)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void DrawButton(Graphics g, Rectangle rect, string text, bool primary)
        {
            using (var sm = new SmoothingContext(g))
            {
                using var path = RoundRect(rect, 14);
                using var bg = new SolidBrush(primary ? Color.FromArgb(36, 142, 255) : Color.White);
                using var brd = new Pen(primary ? Color.FromArgb(36, 142, 255) : Color.FromArgb(216, 223, 230), 1.5f);
                g.FillPath(bg, path);
                g.DrawPath(brd, path);

                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using var ft = new SolidBrush(primary ? Color.White : Color.Black);
                g.DrawString(text, _btnFont, ft, rect, sf);
            }
        }

        private void DrawStatusPill(Graphics g, Rectangle cellBounds, string status)
        {
            // mặc định pill xanh lá cho "Đã xác nhận"
            bool success = status == "Đã xác nhận";
            bool warning = status == "Chờ xác nhận";

            Color bg = success ? Color.FromArgb(214, 243, 228)
                               : warning ? Color.FromArgb(255, 244, 219)
                                         : Color.FromArgb(230, 234, 240);

            Color fg = success ? Color.FromArgb(24, 128, 84)
                               : warning ? Color.FromArgb(173, 101, 0)
                                         : Color.FromArgb(64, 72, 82);

            // pill căn giữa trái (có thể chỉnh)
            var sz = TextRenderer.MeasureText(status, _pillFont);
            var w = sz.Width + 20;
            var h = 26;
            var x = cellBounds.X + 12;
            var y = cellBounds.Y + (cellBounds.Height - h) / 2;

            var rect = new Rectangle(x, y, w, h);
            using (var sm = new SmoothingContext(g))
            {
                using var path = RoundRect(rect, 13);
                using var bgBr = new SolidBrush(bg);
                g.FillPath(bgBr, path);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using var ft = new SolidBrush(fg);
                g.DrawString(status, _pillFont, ft, rect, sf);
            }
        }

        // ======= HIT-TEST CHUẨN (không lệch) =======
        private void OnCellMouseClick_Custom(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.Button != MouseButtons.Left) return;

            if (ColMaDonIndex >= 0 && e.ColumnIndex == ColMaDonIndex)
            {
                OrderCodeClicked?.Invoke(this, new RowActionEventArgs(e.RowIndex));
                return;
            }

            if (ColThaoTacIndex < 0 || e.ColumnIndex != ColThaoTacIndex) return;

            // Tính lại hình học nút theo ô hiển thị
            var cell = GetCellDisplayRectangle(ColThaoTacIndex, e.RowIndex, true);
            int y = cell.Y + (cell.Height - BtnH) / 2;

            string status = (ColTrangThaiIndex >= 0
                ? (Rows[e.RowIndex].Cells[ColTrangThaiIndex].Value ?? "").ToString()
                : "");

            bool showConfirm = status == "Chờ xác nhận";

            Rectangle rDetail, rConfirm = Rectangle.Empty;

            if (showConfirm)
            {
                rConfirm = new Rectangle(cell.Right - BtnWConfirm - PadRight, y, BtnWConfirm, BtnH);
                rDetail = new Rectangle(rConfirm.Left - Gap - BtnWDetail, y, BtnWDetail, BtnH);
            }
            else
            {
                rDetail = new Rectangle(cell.Right - BtnWDetail - PadRight, y, BtnWDetail, BtnH);
            }

            Point mousePos = PointToClient(Cursor.Position);

            if (rDetail.Contains(mousePos))
            {
                DetailClicked?.Invoke(this, new RowActionEventArgs(e.RowIndex));
            }
            else if (showConfirm && !rConfirm.IsEmpty && rConfirm.Contains(mousePos))
            {
                ConfirmClicked?.Invoke(this, new RowActionEventArgs(e.RowIndex));
            }
        }

        private void OnCellMouseMove_Custom(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && ColThaoTacIndex >= 0 && e.ColumnIndex == ColThaoTacIndex)
            {
                var cell = GetCellDisplayRectangle(ColThaoTacIndex, e.RowIndex, true);
                int y = cell.Y + (cell.Height - BtnH) / 2;

                string? status = ColTrangThaiIndex >= 0
                    ? (Rows[e.RowIndex].Cells[ColTrangThaiIndex].Value ?? "").ToString()
                    : "";

                bool showConfirm = status == "Chờ xác nhận";

                Rectangle rDetail, rConfirm = Rectangle.Empty;
                if (showConfirm)
                {
                    rConfirm = new Rectangle(cell.Right - BtnWConfirm - PadRight, y, BtnWConfirm, BtnH);
                    rDetail = new Rectangle(rConfirm.Left - Gap - BtnWDetail, y, BtnWDetail, BtnH);
                }
                else
                {
                    rDetail = new Rectangle(cell.Right - BtnWDetail - PadRight, y, BtnWDetail, BtnH);
                }

                Point p = PointToClient(Cursor.Position);
                Cursor = (rDetail.Contains(p) || (!rConfirm.IsEmpty && rConfirm.Contains(p)))
                    ? Cursors.Hand : Cursors.Default;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        // ======= Helpers =======
        private sealed class SmoothingContext : IDisposable
        {
            private readonly Graphics _g;
            private readonly SmoothingMode _oldSm;
            private readonly PixelOffsetMode _oldPm;

            public SmoothingContext(Graphics g)
            {
                _g = g;
                _oldSm = g.SmoothingMode;
                _oldPm = g.PixelOffsetMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }

            public void Dispose()
            {
                _g.SmoothingMode = _oldSm;
                _g.PixelOffsetMode = _oldPm;
            }
        }
    }
}
