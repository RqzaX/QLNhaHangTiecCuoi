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
    public partial class Frm_ChiTietKM : Form
    {
        private ChuongTrinhKMBLL _bll;
        private int _kmId;
        private FrmChuongTrinhKM _parentForm;

        public Frm_ChiTietKM(int kmId, FrmChuongTrinhKM parentForm = null)
        {
            InitializeComponent();
            _bll = new ChuongTrinhKMBLL();
            _kmId = kmId;
            _parentForm = parentForm;
            InitializeLoaiApDung();
            InitializeEvents();
            LoadData();
        }

        private void InitializeLoaiApDung()
        {
            cbbLoaiApDung.Items.Clear();
            cbbLoaiApDung.Items.AddRange(new object[] { "Tất cả", "Nhà hàng", "Tiệc cưới" });
        }

        private void InitializeEvents()
        {
            btnLuu.Click += BtnLuu_Click;
            btnXoa.Click += BtnXoa_Click;
            checkSuDung.CheckedChanged += CheckSuDung_CheckedChanged;
            CBBLoaiKM.SelectedIndexChanged += CBBLoaiKM_SelectedIndexChanged;
        }

        private void LoadData()
        {
            try
            {
                DataRow row = _bll.GetById(_kmId);
                if (row == null)
                {
                    MessageBox.Show("Không tìm thấy chương trình khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtTenCT.Text = row["ten"]?.ToString() ?? "";
                txtMaKM.Text = row["ma_km"]?.ToString() ?? "";

                string hinhThuc = row["hinh_thuc"]?.ToString() ?? "";
                if (hinhThuc == "PERCENT")
                    CBBLoaiKM.SelectedIndex = 0; // "Giảm Theo %"
                else if (hinhThuc == "AMOUNT")
                    CBBLoaiKM.SelectedIndex = 1; // "GIảm Theo Số Tiền"
                else if (hinhThuc == "GIFT")
                    CBBLoaiKM.SelectedIndex = 2; // "Tặng quà"

                decimal giaTri = row["gia_tri"] == DBNull.Value ? 0 : Convert.ToDecimal(row["gia_tri"]);
                if (hinhThuc == "PERCENT")
                    txtGiamTD.Text = giaTri.ToString("0");
                else if (hinhThuc == "AMOUNT")
                    txtGiamTD.Text = giaTri.ToString("#,##0");
                else
                    txtGiamTD.Text = "";

                if (row["tg_bat_dau"] != DBNull.Value)
                    dateNgayBatDau.Value = Convert.ToDateTime(row["tg_bat_dau"]);

                if (row["tg_ket_thuc"] != DBNull.Value)
                    dateNgayKetThuc.Value = Convert.ToDateTime(row["tg_ket_thuc"]);

                // Load loại áp dụng
                string apDungLoai = row["ap_dung_loai"]?.ToString() ?? "ALL";
                if (apDungLoai == "ALL")
                    cbbLoaiApDung.SelectedIndex = 0; // "Tất cả"
                else if (apDungLoai == "NHAHANG")
                    cbbLoaiApDung.SelectedIndex = 1; // "Nhà hàng"
                else if (apDungLoai == "TIECCUOI")
                    cbbLoaiApDung.SelectedIndex = 2; // "Tiệc cưới"

                // Kiểm tra trạng thái và set checkbox
                DateTime now = DateTime.Now;
                DateTime tgBatDau = dateNgayBatDau.Value;
                DateTime tgKetThuc = dateNgayKetThuc.Value;
                checkSuDung.Checked = (now >= tgBatDau && now <= tgKetThuc);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckSuDung_CheckedChanged(object sender, EventArgs e)
        {
            // Không tự động thay đổi ngày khi check/uncheck
            // Chỉ cập nhật ngày khi người dùng bấm Lưu
            // Logic này sẽ được xử lý trong BtnLuu_Click
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
                return;
            }

            string loaiText = CBBLoaiKM.Text?.Trim() ?? "";

            if (loaiText.Equals("Tặng Quà", StringComparison.OrdinalIgnoreCase) ||
                loaiText.Equals("Tặng quà", StringComparison.OrdinalIgnoreCase))
            {
                txtGiamTD.Enabled = false;
                txtGiamTD.Text = "";
            }
            else
            {
                txtGiamTD.Enabled = true;
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa chương trình khuyến mãi này?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = _bll.Delete(_kmId);
                    if (success)
                    {
                        MessageBox.Show("Xóa chương trình khuyến mãi thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reload dữ liệu ở form cha
                        if (_parentForm != null)
                        {
                            _parentForm.ReloadData();
                        }

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa chương trình khuyến mãi. Vui lòng thử lại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
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

                // Kiểm tra mã khuyến mãi trùng (trừ chính nó)
                if (_bll.MaKmExists(txtMaKM.Text.Trim(), _kmId))
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

                // Map loại từ UI sang database
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
                else if (loaiText.Equals("Tặng Quà", StringComparison.OrdinalIgnoreCase) ||
                         loaiText.Equals("Tặng quà", StringComparison.OrdinalIgnoreCase))
                {
                    hinhThuc = "GIFT";
                }
                else
                {
                    MessageBox.Show($"Loại khuyến mãi không hợp lệ: '{loaiText}'. Vui lòng chọn lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate và parse giá trị
                bool isTangQua = hinhThuc == "GIFT";
                decimal giaTri = 0;

                if (!isTangQua)
                {
                    if (string.IsNullOrWhiteSpace(txtGiamTD.Text))
                    {
                        MessageBox.Show("Vui lòng nhập giá trị khuyến mãi!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGiamTD.Focus();
                        return;
                    }

                    string giaTriText = txtGiamTD.Text.Trim().Replace(".", "").Replace(",", "").Replace("đ", "").Replace(" ", "");

                    if (!decimal.TryParse(giaTriText, NumberStyles.Any, CultureInfo.InvariantCulture, out giaTri))
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
                    if (hinhThuc == "PERCENT" && giaTri > 50)
                    {
                        MessageBox.Show("Giá trị giảm theo % không được vượt quá 50%!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGiamTD.Focus();
                        return;
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
                }

                // Xử lý ngày dựa trên checkbox "Sử dụng"
                DateTime tgBatDau = dateNgayBatDau.Value;
                DateTime tgKetThuc = dateNgayKetThuc.Value;
                DateTime now = DateTime.Now.Date; // Chỉ lấy phần ngày, bỏ phần giờ

                if (checkSuDung.Checked)
                {
                    // Nếu check = "Đang áp dụng", đảm bảo ngày hợp lệ
                    if (tgBatDau > now)
                    {
                        tgBatDau = now; // Đặt ngày bắt đầu về hôm nay
                    }
                    if (tgKetThuc < now)
                    {
                        // Nếu ngày kết thúc < hôm nay, kéo dài ít nhất đến hôm nay
                        // Nhưng phải đảm bảo > ngày bắt đầu
                        DateTime newEndDate = now.AddDays(30);
                        if (newEndDate <= tgBatDau)
                        {
                            newEndDate = tgBatDau.AddDays(1);
                        }
                        tgKetThuc = newEndDate;
                    }
                }
                else
                {
                    // Nếu uncheck = "Đã hết hạn", đảm bảo ngày kết thúc < hôm nay
                    if (tgKetThuc >= now)
                    {
                        // Đặt ngày kết thúc về hôm qua, nhưng phải đảm bảo > ngày bắt đầu
                        DateTime yesterday = now.AddDays(-1);
                        if (yesterday > tgBatDau)
                        {
                            tgKetThuc = yesterday;
                        }
                        else
                        {
                            // Nếu hôm qua <= ngày bắt đầu, đặt ngày kết thúc = ngày bắt đầu + 1 ngày
                            // nhưng vẫn phải < hôm nay
                            DateTime minEndDate = tgBatDau.AddDays(1);
                            if (minEndDate < now)
                            {
                                tgKetThuc = minEndDate;
                            }
                            else
                            {
                                // Trường hợp đặc biệt: ngày bắt đầu quá gần hôm nay
                                // Đặt ngày kết thúc = hôm qua
                                tgKetThuc = now.AddDays(-1);
                                // Nếu vẫn không hợp lệ, đặt ngày bắt đầu về trước đó
                                if (tgKetThuc <= tgBatDau)
                                {
                                    tgBatDau = tgKetThuc.AddDays(-1);
                                }
                            }
                        }
                    }
                }

                // Validate ngày sau khi điều chỉnh
                if (tgKetThuc <= tgBatDau)
                {
                    MessageBox.Show("Sau khi điều chỉnh trạng thái, ngày kết thúc phải sau ngày bắt đầu. Vui lòng kiểm tra lại!", "Lỗi",
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

                // Cập nhật với ngày đã điều chỉnh
                bool result = _bll.Update(
                    _kmId,
                    txtMaKM.Text.Trim(),
                    txtTenCT.Text.Trim(),
                    hinhThuc,
                    giaTri,
                    tgBatDau,
                    tgKetThuc,
                    apDungLoai
                );

                if (result)
                {
                    MessageBox.Show("Cập nhật chương trình khuyến mãi thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload dữ liệu ở form cha
                    if (_parentForm != null)
                    {
                        _parentForm.ReloadData();
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật chương trình khuyến mãi. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
