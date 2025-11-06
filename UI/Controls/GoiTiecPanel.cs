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
using Guna.UI2.WinForms;

namespace UI.Controls
{
    public partial class GoiTiecPanel : UserControl
    {
        public int GoiId { get; private set; }
        public string TenGoi { get; private set; }
        public decimal GiaCoBan { get; private set; }
        public bool IsSelected { get; private set; }
        
        private Color _defaultBackColor = Color.White;
        private Color _hoverBackColor = Color.FromArgb(245, 247, 250);
        private Color _selectedBackColor = Color.FromArgb(240, 245, 255);
        private Color _borderColor = Color.FromArgb(220, 220, 220);
        private Color _hoverBorderColor = Color.FromArgb(180, 200, 240);
        private Color _selectedBorderColor = Color.FromArgb(100, 150, 255);
        private int _borderRadius = 15;
        private int _borderThickness = 1;
        private bool _isHovering = false;
        
        public event EventHandler<GoiTiecSelectedEventArgs> GoiTiecSelected;

        public GoiTiecPanel()
        {
            InitializeComponent();
            this.IsSelected = false;
            SetupPanel();
        }
        
        private void SetupPanel()
        {
            // Cấu hình bo tròn và viền
            this.BackColor = _defaultBackColor;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            
            // Gắn sự kiện hover và paint
            this.MouseEnter += GoiTiecPanel_MouseEnter;
            this.MouseLeave += GoiTiecPanel_MouseLeave;
            this.Paint += GoiTiecPanel_Paint;
        }
        
        private void GoiTiecPanel_Paint(object sender, PaintEventArgs e)
        {
            // Vẽ border bo tròn
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            GraphicsPath path = GetRoundedRectanglePath(rect, _borderRadius);
            
            // Vẽ background
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                g.FillPath(brush, path);
            }
            
            // Vẽ border
            Color currentBorderColor = IsSelected ? _selectedBorderColor : 
                                      (_isHovering ? _hoverBorderColor : _borderColor);
            int currentBorderThickness = IsSelected ? 2 : _borderThickness;
            
            using (Pen pen = new Pen(currentBorderColor, currentBorderThickness))
            {
                g.DrawPath(pen, path);
            }
        }
        
        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseAllFigures();
            
            return path;
        }

        public GoiTiecPanel(int goiId, string tenGoi, decimal giaCoBan) : this()
        {
            GoiId = goiId;
            TenGoi = tenGoi;
            GiaCoBan = giaCoBan;
            
            LoadData();
        }

        private void LoadData()
        {
            lbTenGoi.Text = TenGoi;
            lbSoTienCua1Ban.Text = FormatTien(GiaCoBan) + "/Bàn";
            
            // Gắn sự kiện click để chọn gói
            this.Click += GoiTiecPanel_Click;
            lbTenGoi.Click += GoiTiecPanel_Click;
            lbSoTienCua1Ban.Click += GoiTiecPanel_Click;
            uiPanel1.Click += GoiTiecPanel_Click;
            btnChiTietGoi.Click += BtnChiTietGoi_Click;
            
            // Gắn sự kiện hover cho các control con
            lbTenGoi.MouseEnter += GoiTiecPanel_MouseEnter;
            lbTenGoi.MouseLeave += GoiTiecPanel_MouseLeave;
            lbSoTienCua1Ban.MouseEnter += GoiTiecPanel_MouseEnter;
            lbSoTienCua1Ban.MouseLeave += GoiTiecPanel_MouseLeave;
            uiPanel1.MouseEnter += GoiTiecPanel_MouseEnter;
            uiPanel1.MouseLeave += GoiTiecPanel_MouseLeave;
        }
        
        private void GoiTiecPanel_MouseEnter(object sender, EventArgs e)
        {
            _isHovering = true;
            if (!IsSelected)
            {
                this.BackColor = _hoverBackColor;
                this.Invalidate(); // Vẽ lại để cập nhật border
            }
        }
        
        private void GoiTiecPanel_MouseLeave(object sender, EventArgs e)
        {
            _isHovering = false;
            if (!IsSelected)
            {
                this.BackColor = _defaultBackColor;
                this.Invalidate(); // Vẽ lại để cập nhật border
            }
        }

        private string FormatTien(decimal amount)
        {
            return amount.ToString("#,##0");
        }

        private void GoiTiecPanel_Click(object sender, EventArgs e)
        {
            // Bỏ qua nếu click vào nút Chi tiết
            if (sender == btnChiTietGoi)
            {
                return;
            }
            
            if (!IsSelected)
            {
                this.BackColor = Color.FromArgb(200, 220, 255);
                this.Invalidate();
                
                System.Threading.Tasks.Task.Delay(150).ContinueWith(_ =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => SetSelected(true)));
                    }
                    else
                    {
                        SetSelected(true);
                    }
                });
            }
            
            // Chọn gói này
            SetSelected(true);
            
            // Thông báo cho parent
            GoiTiecSelected?.Invoke(this, new GoiTiecSelectedEventArgs
            {
                GoiId = this.GoiId,
                TenGoi = this.TenGoi,
                GiaCoBan = this.GiaCoBan
            });
        }

        private void BtnChiTietGoi_Click(object sender, EventArgs e)
        {
            // Mở form chi tiết gói tiệc
            var frmChiTiet = new UI.FrmChiTietGoiTiec(this.GoiId);
            frmChiTiet.ShowDialog();
        }

        public void SetSelected(bool selected)
        {
            IsSelected = selected;
            
            if (selected)
            {
                // Highlight khi được chọn
                this.BackColor = _selectedBackColor;
                uiPanel1.Style = Sunny.UI.UIStyle.Blue;
            }
            else
            {
                // Màu mặc định
                this.BackColor = _defaultBackColor;
                uiPanel1.Style = Sunny.UI.UIStyle.Gray;
            }
            
            this.Invalidate(); // Vẽ lại để cập nhật border
        }
    }

    public class GoiTiecSelectedEventArgs : EventArgs
    {
        public int GoiId { get; set; }
        public string TenGoi { get; set; }
        public decimal GiaCoBan { get; set; }
    }
}
