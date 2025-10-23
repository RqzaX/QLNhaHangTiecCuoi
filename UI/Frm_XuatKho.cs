using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Frm_XuatKho : Form
    {
        public Frm_XuatKho()
        {
            InitializeComponent();
            roundedButton2.Click += RoundedButton2_Click; // Tạo phiếu
            roundedButton1.Click += RoundedButton1_Click; // Hủy
        }

        private void RoundedButton1_Click(object sender, EventArgs e)
        {
            this.Close(); // Hủy → đóng form
        }

        private void RoundedButton2_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu bắt buộc
            if (borderComboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn món ăn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (borderComboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn kho xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(roundedTextBox2.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thu thập thông tin
            string monAn = borderComboBox1.SelectedItem.ToString();
            string khoXuat = borderComboBox2.SelectedItem.ToString();
            string ngayXuat = dateTimePicker1.Value.ToString("dd/MM/yyyy HH:mm");
            string ghiChu = string.IsNullOrWhiteSpace(roundedTextBox1.Text) ? "Không có" : roundedTextBox1.Text.Trim();

            // Tạo mã phiếu xuất kho
            string maPhieu = "PXK" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Tạo nội dung chi tiết
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("PHIẾU XUẤT KHO ĐÃ TẠO THÀNH CÔNG!");
            sb.AppendLine("══════════════════════════════");
            sb.AppendLine($"Mã Phiếu: {maPhieu}");
            sb.AppendLine($"Món Ăn: {monAn}");
            sb.AppendLine($"Kho Xuất: {khoXuat}");
            sb.AppendLine($"Số Lượng: {soLuong}");
            sb.AppendLine($"Ngày Xuất: {ngayXuat}");
            sb.AppendLine($"Ghi Chú: {ghiChu}");
            sb.AppendLine("══════════════════════════════\n");

            // Hiển thị thông báo
            MessageBox.Show(sb.ToString(), "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close(); // Đóng form
        }
    }
}
