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
    public partial class Frm_NhapKho : Form
    {
        public Frm_NhapKho()
        {
            InitializeComponent();

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTaoPhieuNhap_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu
            if (borderComboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn danh mục món ăn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(roundedTextBox2.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập số hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thu thập thông tin
            string danhMuc = borderComboBox1.SelectedItem.ToString();
            string soHoaDon = roundedTextBox2.Text.Trim();
            string ngayNhap = dateTimePicker1.Value.ToString("dd/MM/yyyy");
            string ghiChu = string.IsNullOrWhiteSpace(roundedTextBox1.Text) ? "Không có" : roundedTextBox1.Text.Trim();

            // Tạo mã phiếu tự động (ví dụ)
            string maPhieu = "PNK" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Tạo nội dung thông báo chi tiết
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("PHIẾU NHẬP KHO ĐÃ TẠO THÀNH CÔNG!");
            sb.AppendLine("══════════════════════════════");
            sb.AppendLine($"Mã Phiếu: {maPhieu}");
            sb.AppendLine($"Danh Mục: {danhMuc}");
            sb.AppendLine($"Số Hóa Đơn: {soHoaDon}");
            sb.AppendLine($"Ngày Nhập: {ngayNhap}");
            sb.AppendLine($"Ghi Chú: {ghiChu}");
            sb.AppendLine("══════════════════════════════\n");

            // Hiển thị thông báo
            MessageBox.Show(sb.ToString(), "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }
    }
}
