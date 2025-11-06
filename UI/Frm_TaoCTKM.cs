using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using System.Globalization;

namespace UI
{
    public partial class Frm_TaoCTKM : Form
    {
        private ChuongTrinhKMBLL _bll;

        public Frm_TaoCTKM()
        {
            InitializeComponent();
            _bll = new ChuongTrinhKMBLL();
            InitializeEvents();
            InitializeLoaiApDung();
        }

        private void InitializeLoaiApDung()
        {
            cbbLoaiApDung.Items.Clear();
            cbbLoaiApDung.Items.AddRange(new object[] { "Tất cả", "Nhà hàng", "Tiệc cưới" });
            cbbLoaiApDung.SelectedIndex = 0; // Mặc định chọn "Tất cả"
        }

        private void InitializeEvents()
        {
            btnTao.Click += BtnTao_Click;
            btnHuy.Click += BtnHuy_Click;
            CBBLoaiKM.SelectedIndexChanged += CBBLoaiKM_SelectedIndexChanged;
            txtGiamTD.TextChanged += TxtGiamTD_TextChanged;
            txtGiamTD.KeyPress += TxtGiamTD_KeyPress;
        }

        private void CBBLoaiKM_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGiamTDValidation();
        }

        private void UpdateGiamTDValidation()
        {
            if (CBBLoaiKM.SelectedIndex < 0 || string.IsNullOrWhiteSpace(CBBLoaiKM.Text) || CBBLoaiKM.Text == "Chọn Loại")
            {
                txtGiamTD.Enabled = false;
                txtGiamTD.Text = "";
                return;
            }

            txtGiamTD.Enabled = true;
        }

        private void TxtGiamTD_TextChanged(object sender, EventArgs e)
        {
            // Validate real-time nếu cần
            ValidateGiamTD();
        }

        private void TxtGiamTD_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép số, dấu chấm, dấu phẩy và backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != ' ' && e.KeyChar != 'đ')
            {
                e.Handled = true;
            }
        }

        private bool ValidateGiamTD()
        {
            if (!txtGiamTD.Enabled || string.IsNullOrWhiteSpace(txtGiamTD.Text))
            {
                return true; // Không cần validate nếu disabled hoặc empty
            }

            string loaiText = CBBLoaiKM.Text;
            string giaTriText = txtGiamTD.Text.Trim().Replace(".", "").Replace(",", "").Replace("đ", "").Replace(" ", "");

            if (string.IsNullOrWhiteSpace(giaTriText))
            {
                return true;
            }

            if (decimal.TryParse(giaTriText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal giaTri))
            {
                if (loaiText.Contains("%") || loaiText == "Giảm Theo %")
                {
                    if (giaTri > 50)
                    {
                        txtGiamTD.BackColor = Color.FromArgb(255, 200, 200); // Màu đỏ nhạt
                        return false;
                    }
                }
                else if (loaiText.Contains("Số Tiền") || loaiText == "GIảm Theo Số Tiền")
                {
                    if (giaTri < 100000)
                    {
                        txtGiamTD.BackColor = Color.FromArgb(255, 200, 200); // Màu đỏ nhạt
                        return false;
                    }
                    if (giaTri > 10000000)
                    {
                        txtGiamTD.BackColor = Color.FromArgb(255, 200, 200); // Màu đỏ nhạt
                        return false;
                    }
                }
            }

            txtGiamTD.BackColor = Color.White; // Màu trắng bình thường
            return true;
        }

        private void BtnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnTao_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu
                if (string.IsNullOrWhiteSpace(txtTenCT.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên chương trình!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenCT.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMaKM.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaKM.Focus();
                    return;
                }

                // Kiểm tra mã khuyến mãi đã tồn tại chưa
                if (_bll.MaKmExists(txtMaKM.Text.Trim()))
                {
                    MessageBox.Show("Mã khuyến mãi đã tồn tại! Vui lòng nhập mã khác.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaKM.Focus();
                    return;
                }

                if (CBBLoaiKM.SelectedIndex < 0 || string.IsNullOrWhiteSpace(CBBLoaiKM.Text) || CBBLoaiKM.Text == "Chọn Loại")
                {
                    MessageBox.Show("Vui lòng chọn loại khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CBBLoaiKM.Focus();
                    return;
                }

                string loaiText = CBBLoaiKM.Text.Trim();
                string hinhThuc = "";

                // Map loại từ UI sang database (so sánh không phân biệt hoa thường)
                if (loaiText.Contains("%", StringComparison.OrdinalIgnoreCase) ||
                    loaiText.Equals("Giảm Theo %", StringComparison.OrdinalIgnoreCase))
                {
                    hinhThuc = "PERCENT";
                }
                else if (loaiText.Contains("Số Tiền", StringComparison.OrdinalIgnoreCase) ||
                         loaiText.Equals("Giảm Theo Số Tiền", StringComparison.OrdinalIgnoreCase) ||
                         loaiText.Equals("GIảm Theo Số Tiền", StringComparison.OrdinalIgnoreCase))
                {
                    hinhThuc = "AMOUNT";
                }
                else
                {
                    MessageBox.Show($"Loại khuyến mãi không hợp lệ: '{loaiText}'. Vui lòng chọn lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate và parse giá trị
                if (string.IsNullOrWhiteSpace(txtGiamTD.Text))
                {
                    MessageBox.Show("Vui lòng nhập giá trị khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiamTD.Focus();
                    return;
                }

                string giaTriText = txtGiamTD.Text.Trim().Replace(".", "").Replace(",", "").Replace("đ", "").Replace(" ", "");

                if (!decimal.TryParse(giaTriText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal giaTri))
                {
                    MessageBox.Show("Giá trị khuyến mãi không hợp lệ! Vui lòng nhập số.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiamTD.Focus();
                    return;
                }

                if (giaTri <= 0)
                {
                    MessageBox.Show("Giá trị khuyến mãi phải lớn hơn 0!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiamTD.Focus();
                    return;
                }

                // Ràng buộc theo loại
                if (hinhThuc == "PERCENT")
                {
                    if (giaTri > 50)
                    {
                        MessageBox.Show("Giá trị giảm theo % không được vượt quá 50%!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGiamTD.Focus();
                        return;
                    }
                }
                else if (hinhThuc == "AMOUNT")
                {
                    if (giaTri < 100000)
                    {
                        MessageBox.Show("Giá trị giảm theo số tiền không được nhỏ hơn 100.000 đ!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGiamTD.Focus();
                        return;
                    }
                    if (giaTri > 10000000)
                    {
                        MessageBox.Show("Giá trị giảm theo số tiền không được vượt quá 10.000.000 đ!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGiamTD.Focus();
                        return;
                    }
                }

                // Validate ngày
                if (dateNgayKetThuc.Value <= dateNgayBatDau.Value)
                {
                    MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dateNgayKetThuc.Focus();
                    return;
                }

                // Lấy loại áp dụng từ ComboBox
                string apDungLoai = "ALL"; // Mặc định
                if (cbbLoaiApDung.SelectedIndex >= 0)
                {
                    string selectedText = cbbLoaiApDung.Text;
                    if (selectedText == "Tất cả")
                        apDungLoai = "ALL";
                    else if (selectedText == "Nhà hàng")
                        apDungLoai = "NHAHANG";
                    else if (selectedText == "Tiệc cưới")
                        apDungLoai = "TIECCUOI";
                }

                // Thêm mới vào database
                bool result = _bll.Add(
                    txtMaKM.Text.Trim(),
                    txtTenCT.Text.Trim(),
                    hinhThuc,
                    giaTri,
                    dateNgayBatDau.Value,
                    dateNgayKetThuc.Value,
                    apDungLoai
                );

                if (result)
                {
                    MessageBox.Show("Tạo chương trình khuyến mãi thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể tạo chương trình khuyến mãi. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo chương trình khuyến mãi:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
