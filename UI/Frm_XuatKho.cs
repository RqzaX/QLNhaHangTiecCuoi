using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Common;
using BLL;
using QLNhaHangTiecCuoi.Share;
using QLNhaHangTiecCuoi.BLL;

namespace UI
{
    public partial class Frm_XuatKho : Form
    {
        private readonly NguyenLieuBLL _bll;
        private readonly DatabaseHelper _dbHelper;

        public Frm_XuatKho()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bll = new NguyenLieuBLL(_dbHelper);
            
            LoadData();
            WireEvents();
        }

        private void WireEvents()
        {
            roundedButton1.Click += RoundedButton1_Click; // Hủy
            roundedButton2.Click += RoundedButton2_Click; // Tạo phiếu
        }

        private void LoadData()
        {
            try
            {
                // Load danh sách nguyên liệu
                LoadNguyenLieu();
                
                // Load danh sách chi nhánh
                LoadChiNhanh();
                
                // Set ngày mặc định
                dateNgayXuat.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNguyenLieu()
        {
            try
            {
                var dt = _bll.LayDanhMuc();
                if (dt != null && dt.Rows.Count > 0)
                {
                    cbbNguyenLieu.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        string displayText = $"{row["ma_nl"]} - {row["ten_nl"]} ({row["don_vi"]})";
                        cbbNguyenLieu.Items.Add(new ComboBoxItem(displayText, Convert.ToInt32(row["nl_id"])));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nguyên liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiNhanh()
        {
            try
            {
                var dt = _bll.LayTatCaChiNhanh();
                if (dt != null && dt.Rows.Count > 0)
                {
                    cbbKhoXuat.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        string displayText = $"{row["ten"]} (ID: {row["chi_nhanh_id"]})";
                        cbbKhoXuat.Items.Add(new ComboBoxItem(displayText, Convert.ToInt32(row["chi_nhanh_id"])));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chi nhánh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RoundedButton1_Click(object sender, EventArgs e)
        {
            this.Close(); // Hủy → đóng form
        }

        private void RoundedButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu bắt buộc
                if (cbbNguyenLieu.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbbKhoXuat.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn chi nhánh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtSoLuong.Text, out decimal soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải là số dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin đã chọn
                var selectedNguyenLieu = (ComboBoxItem)cbbNguyenLieu.SelectedItem;
                var selectedChiNhanh = (ComboBoxItem)cbbKhoXuat.SelectedItem;
                
                int nlId = selectedNguyenLieu.Value;
                int chiNhanhId = selectedChiNhanh.Value;

                // Kiểm tra tồn kho
                if (!_bll.KiemTraTonKhoDu(chiNhanhId, nlId, soLuong))
                {
                    decimal tonHienTai = _bll.LayTonKhoTaiChiNhanh(chiNhanhId, nlId);
                    MessageBox.Show($"Không đủ tồn kho!\nTồn hiện tại: {tonHienTai:N2}\nCần xuất: {soLuong:N2}", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo mã phiếu xuất kho
                string maPhieu = "PXK" + DateTime.Now.ToString("yyyyMMddHHmmss");
                
                // Xuất kho trực tiếp
                int result = _bll.XuatKho(chiNhanhId, nlId, soLuong);

                if (result > 0)
                {
                    // Hiển thị thông báo thành công
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("XUẤT KHO THÀNH CÔNG!");
                    sb.AppendLine("══════════════════════════════");
                    sb.AppendLine($"Mã Phiếu: {maPhieu}");
                    sb.AppendLine($"Nguyên Liệu: {selectedNguyenLieu.Text}");
                    sb.AppendLine($"Chi Nhánh: {selectedChiNhanh.Text}");
                    sb.AppendLine($"Số Lượng: {soLuong:N2}");
                    sb.AppendLine($"Ngày Xuất: {dateNgayXuat.Value:dd/MM/yyyy HH:mm}");
                    sb.AppendLine("══════════════════════════════");

                    MessageBox.Show(sb.ToString(), "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Reset form
                    ResetForm();
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xuất kho!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo phiếu xuất kho: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

         private void ResetForm()
         {
             cbbKhoXuat.SelectedIndex = -1;
             cbbNguyenLieu.SelectedIndex = -1;
             txtSoLuong.Text = "";
             dateNgayXuat.Value = DateTime.Now;
         }
    }
}
