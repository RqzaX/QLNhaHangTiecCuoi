using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Controls
{
    public partial class VaiTroCard : UserControl
    {
        private int _vaiTroId;
        private string _tenVaiTro;
        private string _maVaiTro;
        private string _moTa;
        private int _soNguoiDung;
        private bool _isHovered = false;

        public event EventHandler EditClicked;
        public event EventHandler DeleteClicked;

        public VaiTroCard()
        {
            InitializeComponent();
            SetupCustomPaint();
        }

        public VaiTroCard(int vaiTroId, string tenVaiTro, string maVaiTro, string moTa, int soNguoiDung)
            : this()
        {
            _vaiTroId = vaiTroId;
            _tenVaiTro = tenVaiTro;
            _maVaiTro = maVaiTro;
            _moTa = moTa;
            _soNguoiDung = soNguoiDung;

            LoadData();
        }

        private void LoadData()
        {
            lbTitle.Text = _tenVaiTro;
            panelVaiTro.Text = FormatMaVaiTro(_maVaiTro);
            lbMoTa.Text = string.IsNullOrEmpty(_moTa) ? "Không có mô tả" : _moTa;
            lblUserCount.Text = $"{_soNguoiDung} người dùng";
            
            if (IsAdmin())
            {
                btnXoa.Visible = false;
            }
            
            ApplyRandomStyleToPanel();
        }

        private bool IsAdmin()
        {
            string maLower = _maVaiTro?.ToLower() ?? "";
            string tenLower = _tenVaiTro?.ToLower() ?? "";
            
            return maLower == "admin" || tenLower == "quản trị" || tenLower == "admin";
        }

        public bool IsAdminRole => IsAdmin();

        private void ApplyRandomStyleToPanel()
        {
            Color[] colors = new Color[]
            {
                Color.FromArgb(59, 130, 246),   // Blue
                Color.FromArgb(139, 92, 246),    // Purple
                Color.FromArgb(236, 72, 153),    // Pink
                Color.FromArgb(34, 197, 94),     // Green
                Color.FromArgb(251, 146, 60),     // Orange
                Color.FromArgb(239, 68, 68),     // Red
                Color.FromArgb(168, 85, 247),    // Violet
                Color.FromArgb(14, 165, 233),    // Sky
                Color.FromArgb(20, 184, 166),    // Teal
                Color.FromArgb(245, 158, 11),    // Amber
                Color.FromArgb(99, 102, 241),    // Indigo
                Color.FromArgb(249, 115, 22),    // Orange-600
                Color.FromArgb(6, 182, 212),     // Cyan
                Color.FromArgb(147, 51, 234),    // Purple-600
                Color.FromArgb(16, 185, 129),    // Emerald
            };

            // Sử dụng hash code của ID và tên để đảm bảo phân phối đều và tránh trùng
            int hash = (_vaiTroId.GetHashCode() + _tenVaiTro.GetHashCode()) % colors.Length;
            if (hash < 0) hash = Math.Abs(hash);
            
            Color selectedColor = colors[hash];

            // Áp dụng màu cho SunnyUI Panel
            panelVaiTro.FillColor = selectedColor;
            panelVaiTro.RectColor = selectedColor;
            panelVaiTro.ForeColor = Color.White;
        }

        private string FormatMaVaiTro(string maVaiTro)
        {
            if (string.IsNullOrEmpty(maVaiTro))
                return maVaiTro;

            return maVaiTro.ToLower();
        }

        private void SetupCustomPaint()
        {
            this.Paint += VaiTroCard_Paint;
            this.MouseEnter += VaiTroCard_MouseEnter;
            this.MouseLeave += VaiTroCard_MouseLeave;

            // Setup hover cho tất cả control con để hover không bị mất
            SetupHoverForChildControls(this);

            // Setup users icon
            picUsers.Paint += PicUsers_Paint;

            // Setup button icons
            btnSua.Paint += BtnSua_Paint;
            btnXoa.Paint += BtnXoa_Paint;
        }

        private void SetupHoverForChildControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                control.MouseEnter += (s, e) =>
                {
                    if (!_isHovered)
                    {
                        _isHovered = true;
                        this.Invalidate();
                    }
                };
                control.MouseLeave += (s, e) =>
                {
                    // Kiểm tra xem mouse có thực sự rời khỏi control cha không
                    Point clientPos = this.PointToClient(Cursor.Position);
                    if (!this.ClientRectangle.Contains(clientPos))
                    {
                        _isHovered = false;
                        this.Invalidate();
                    }
                };
                
                // Đệ quy cho các control con
                if (control.HasChildren)
                {
                    SetupHoverForChildControls(control);
                }
            }
        }

        private void PicUsers_Paint(object sender, PaintEventArgs e)
        {
            var pic = sender as PictureBox;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Nếu không có image, vẽ icon
            if (pic.Image == null)
            {
                Color iconColor = Color.FromArgb(150, 150, 150);
                using (var pen = new Pen(iconColor, 1.5f))
                {
                    // Draw two user icons
                    // First user
                    g.DrawEllipse(pen, 2, 2, 6, 6);
                    g.DrawArc(pen, 0, 6, 10, 8, 30, 120);

                    // Second user
                    g.DrawEllipse(pen, 10, 3, 5, 5);
                    g.DrawArc(pen, 8, 7, 10, 7, 30, 120);
                }
            }
        }

        private void VaiTroCard_MouseEnter(object sender, EventArgs e)
        {
            _isHovered = true;
            this.Invalidate();
        }

        private void VaiTroCard_MouseLeave(object sender, EventArgs e)
        {
            // Kiểm tra xem mouse có thực sự rời khỏi control không
            Point clientPos = this.PointToClient(Cursor.Position);
            if (!this.ClientRectangle.Contains(clientPos))
            {
                _isHovered = false;
                this.Invalidate();
            }
        }

        private void VaiTroCard_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_isHovered)
            {
                var rect = this.ClientRectangle;
                
                // Chỉ vẽ border xanh, không có shadow
                rect.Inflate(-1, -1);
                using (var path = CreateRoundedRectangle(rect, 20))
                {
                    using (var pen = new Pen(Color.FromArgb(59, 130, 246), 1.5f))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int diameter = radius * 2;
            var arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));

            path.AddArc(arcRect, 180, 90);
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void BtnSua_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void BtnXoa_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            EditClicked?.Invoke(this, e);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, e);
        }

        public int VaiTroId => _vaiTroId;
        public string TenVaiTro => _tenVaiTro;
        public string MaVaiTro => _maVaiTro;
        public string MoTa => _moTa;
    }
}

