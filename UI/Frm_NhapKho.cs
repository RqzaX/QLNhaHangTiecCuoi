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
using BLL;
using QLNhaHangTiecCuoi.Share;
using QLNhaHangTiecCuoi.BLL;
using UI.Common;

namespace UI
{
    public partial class Frm_NhapKho : Form
    {
        private readonly NguyenLieuBLL _bll;
        private readonly DatabaseHelper _dbHelper;

        public Frm_NhapKho()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bll = new NguyenLieuBLL(_dbHelper);
            
            LoadData();
            WireEvents();
        }

        private void WireEvents()
        {
            btnHuy.Click += BtnHuy_Click;
            btnTaoPhieuNhap.Click += BtnTaoPhieuNhap_Click;
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
                dateNgayNhap.Value = DateTime.Now;
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
                    cbbTenMon.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        string displayText = $"{row["ma_nl"]} - {row["ten_nl"]} ({row["don_vi"]})";
                        cbbTenMon.Items.Add(new ComboBoxItem(displayText, Convert.ToInt32(row["nl_id"])));
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
                    cbbChiNhanh.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        string displayText = $"{row["ten"]} (ID: {row["chi_nhanh_id"]})";
                        cbbChiNhanh.Items.Add(new ComboBoxItem(displayText, Convert.ToInt32(row["chi_nhanh_id"])));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chi nhánh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnTaoPhieuNhap_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu
                if (cbbTenMon.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbbChiNhanh.SelectedIndex == -1)
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
                var selectedNguyenLieu = (ComboBoxItem)cbbTenMon.SelectedItem;
                var selectedChiNhanh = (ComboBoxItem)cbbChiNhanh.SelectedItem;
                
                int nlId = selectedNguyenLieu.Value;
                int chiNhanhId = selectedChiNhanh.Value;
                
                // Tạo mã phiếu nhập kho
                string maPhieu = "PNK" + DateTime.Now.ToString("yyyyMMddHHmmss");
                
                // Nhập kho trực tiếp
                int result = _bll.NhapKho(chiNhanhId, nlId, soLuong);

                if (result > 0)
                {
                    // Hiển thị thông báo thành công
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("NHẬP KHO THÀNH CÔNG!");
                    sb.AppendLine("══════════════════════════════");
                    sb.AppendLine($"Mã Phiếu: {maPhieu}");
                    sb.AppendLine($"Nguyên Liệu: {selectedNguyenLieu.Text}");
                    sb.AppendLine($"Chi Nhánh: {selectedChiNhanh.Text}");
                    sb.AppendLine($"Số Lượng: {soLuong:N2}");
                    sb.AppendLine($"Ngày Nhập: {dateNgayNhap.Value:dd/MM/yyyy HH:mm}");
                    sb.AppendLine("══════════════════════════════");

                    MessageBox.Show(sb.ToString(), "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Reset form
                    ResetForm();
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi nhập kho!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo phiếu nhập kho: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

         private void ResetForm()
         {
             cbbTenMon.SelectedIndex = -1;
             cbbChiNhanh.SelectedIndex = -1;
             txtSoLuong.Text = "";
             dateNgayNhap.Value = DateTime.Now;
         }
    }
}
