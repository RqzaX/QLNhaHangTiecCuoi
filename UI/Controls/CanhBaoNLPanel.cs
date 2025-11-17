using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Controls
{
    public partial class CanhBaoNLPanel : UserControl
    {
        public CanhBaoNLPanel()
        {
            InitializeComponent();
            // Đăng ký event để cập nhật vị trí khi uiPanel1 resize
            if (uiPanel1 != null)
            {
                uiPanel1.SizeChanged += UiPanel1_SizeChanged;
            }
            // Căn giữa các label sau khi khởi tạo
            this.HandleCreated += (s, e) => UpdateLabelsVerticalCenter();
        }

        private void UiPanel1_SizeChanged(object sender, EventArgs e)
        {
            UpdatePanel2Position();
        }

        /// <summary>
        /// Set thông tin nguyên liệu cho panel
        /// </summary>
        public void SetData(string tenNguyenLieu, decimal soLuong, bool hetHang = false)
        {
            if (label1 != null)
                label1.Text = tenNguyenLieu ?? "Chưa có tên";
            
            if (label2 != null)
                label2.Text = soLuong.ToString("N2");

            // Nếu hết hàng (số lượng = 0), đổi màu cảnh báo thành đỏ
            if (uiPanel2 != null)
            {
                if (hetHang)
                {
                    uiPanel2.FillColor = Color.FromArgb(255, 99, 99); // Đỏ
                    uiPanel2.Text = "Hết hàng";
                }
                else
                {
                    uiPanel2.FillColor = Color.FromArgb(255, 255, 128); // Vàng
                    uiPanel2.Text = "Sắp hết";
                }

                // Đảm bảo uiPanel2 luôn ở bên phải và căn giữa theo chiều dọc
                UpdatePanel2Position();
            }
        }

        private void UpdatePanel2Position()
        {
            if (uiPanel2 != null && uiPanel1 != null)
            {
                int panelHeight = uiPanel1.Height;
                int panel2Height = uiPanel2.Height;
                int panelWidth = uiPanel1.Width;
                int panel2Width = uiPanel2.Width;
                
                // Tính vị trí X: bên phải trừ margin (10px) trừ width của panel2
                int xPos = panelWidth - panel2Width - 10;
                int yPos = (panelHeight - panel2Height) / 2;
                
                uiPanel2.Location = new Point(xPos, yPos);
            }

            // Căn giữa các label theo chiều dọc
            UpdateLabelsVerticalCenter();
        }

        private void UpdateLabelsVerticalCenter()
        {
            if (uiPanel1 != null)
            {
                int panelHeight = uiPanel1.Height;

                // Căn giữa label1 (tên nguyên liệu)
                if (label1 != null)
                {
                    int label1Height = label1.Height;
                    int yPos1 = (panelHeight - label1Height) / 2;
                    label1.Location = new Point(label1.Location.X, yPos1);
                }

                // Căn giữa label2 (số lượng)
                if (label2 != null)
                {
                    int label2Height = label2.Height;
                    int yPos2 = (panelHeight - label2Height) / 2;
                    label2.Location = new Point(label2.Location.X, yPos2);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Đảm bảo uiPanel2 luôn ở đúng vị trí khi UserControl resize
            UpdatePanel2Position();
        }
    }
}
