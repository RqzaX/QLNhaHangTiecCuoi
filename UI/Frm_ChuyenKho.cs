using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Controls;

namespace UI
{
    public partial class Frm_ChuyenKho : Form
    {
        public Frm_ChuyenKho()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTaoPhieuChuyen_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu
            if (borderComboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn kho nguồn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (borderComboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn kho đích!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (borderComboBox1.SelectedIndex == borderComboBox2.SelectedIndex)
            {
                MessageBox.Show("Kho nguồn và kho đích không được trùng nhau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thu thập thông tin
            string khoNguon = borderComboBox1.SelectedItem.ToString();
            string khoDich = borderComboBox2.SelectedItem.ToString();
            string ngayChuyen = dateTimePicker1.Value.ToString("dd/MM/yyyy HH:mm");
            string ghiChu = string.IsNullOrWhiteSpace(roundedTextBox1.Text) ? "Không có" : roundedTextBox1.Text.Trim();

            // Tạo mã phiếu chuyển kho
            string maPhieu = "PCK" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Tạo nội dung chi tiết
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("PHIẾU CHUYỂN KHO ĐÃ TẠO THÀNH CÔNG!");
            sb.AppendLine("══════════════════════════════");
            sb.AppendLine($"Mã Phiếu: {maPhieu}");
            sb.AppendLine($"Từ Kho: {khoNguon}");
            sb.AppendLine($"Đến Kho: {khoDich}");
            sb.AppendLine($"Ngày Chuyển: {ngayChuyen}");
            sb.AppendLine($"Ghi Chú: {ghiChu}");
            sb.AppendLine("══════════════════════════════\n");

            // Hiển thị thông báo
            MessageBox.Show(sb.ToString(), "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }
    }
}
