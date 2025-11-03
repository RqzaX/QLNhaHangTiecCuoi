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
using UI.Common;
using BLL;
using QLNhaHangTiecCuoi.Share;
using QLNhaHangTiecCuoi.BLL;

namespace UI
{
    public partial class Frm_ChuyenKho : Form
    {
        private readonly NguyenLieuBLL _bll;
        private readonly DatabaseHelper _dbHelper;

        public Frm_ChuyenKho()
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
            btnTaoPhieuChuyen.Click += BtnTaoPhieuChuyen_Click;
        }

        private void LoadData()
        {
            try
            {
                LoadNguyenLieu();
                
                LoadChiNhanh();
                
                dateNgayChuyen.Value = DateTime.Now;
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
                    cbbTuKho.Items.Clear();
                    cbbDenKho.Items.Clear();
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        string displayText = $"{row["ten"]} (ID: {row["chi_nhanh_id"]})";
                        var item = new ComboBoxItem(displayText, Convert.ToInt32(row["chi_nhanh_id"]));
                        
                        cbbTuKho.Items.Add(item);
                        cbbDenKho.Items.Add(item);
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

        private void BtnTaoPhieuChuyen_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu
                if (cbbTuKho.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn chi nhánh nguồn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbbDenKho.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn chi nhánh đích!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbbTuKho.SelectedIndex == cbbDenKho.SelectedIndex)
                {
                    MessageBox.Show("Chi nhánh nguồn và chi nhánh đích không được trùng nhau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cbbNguyenLieu.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn nguyên liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtSoLuong.Text, out decimal soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải là số dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin đã chọn
                var selectedChiNhanhNguon = (ComboBoxItem)cbbTuKho.SelectedItem;
                var selectedChiNhanhDich = (ComboBoxItem)cbbDenKho.SelectedItem;
                var selectedNguyenLieu = (ComboBoxItem)cbbNguyenLieu.SelectedItem;
                
                int chiNhanhNguonId = selectedChiNhanhNguon.Value;
                int chiNhanhDichId = selectedChiNhanhDich.Value;
                int nlId = selectedNguyenLieu.Value;

                if (!_bll.KiemTraTonKhoDu(chiNhanhNguonId, nlId, soLuong))
                {
                    decimal tonHienTai = _bll.LayTonKhoTaiChiNhanh(chiNhanhNguonId, nlId);
                    MessageBox.Show($"Không đủ tồn kho ở chi nhánh nguồn!\nTồn hiện tại: {tonHienTai:N2}\nCần chuyển: {soLuong:N2}", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maPhieu = "PCK" + DateTime.Now.ToString("yyyyMMddHHmmss");
                
                int result = _bll.ChuyenKho(chiNhanhNguonId, chiNhanhDichId, nlId, soLuong);

                if (result > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("CHUYỂN KHO THÀNH CÔNG!");
                    sb.AppendLine("══════════════════════════════");
                    sb.AppendLine($"Mã Phiếu: {maPhieu}");
                    sb.AppendLine($"Từ Chi Nhánh: {selectedChiNhanhNguon.Text}");
                    sb.AppendLine($"Đến Chi Nhánh: {selectedChiNhanhDich.Text}");
                    sb.AppendLine($"Nguyên Liệu: {selectedNguyenLieu.Text}");
                    sb.AppendLine($"Số Lượng: {soLuong:N2}");
                    sb.AppendLine($"Ngày Chuyển: {dateNgayChuyen.Value:dd/MM/yyyy HH:mm}");
                    sb.AppendLine("══════════════════════════════");

                    MessageBox.Show(sb.ToString(), "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ResetForm();
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi chuyển kho!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo phiếu chuyển kho: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

         private void ResetForm()
         {
             cbbTuKho.SelectedIndex = -1;
             cbbDenKho.SelectedIndex = -1;
             cbbNguyenLieu.SelectedIndex = -1;
             txtSoLuong.Text = "";
             dateNgayChuyen.Value = DateTime.Now;
         }
    }
}
