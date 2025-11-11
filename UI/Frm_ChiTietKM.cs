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
        private bool _isLoadingData = false; // Flag để tránh event handler chạy khi load data

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
                _isLoadingData = true; // Đặt flag để tránh event handler chạy

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
                    CBBLoaiKM.SelectedIndex = 0;
                else if (hinhThuc == "AMOUNT")
                    CBBLoaiKM.SelectedIndex = 1;

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

                string apDungLoai = row["ap_dung_loai"]?.ToString() ?? "ALL";
                if (apDungLoai == "ALL")
                    cbbLoaiApDung.SelectedIndex = 0;
                else if (apDungLoai == "NHAHANG")
                    cbbLoaiApDung.SelectedIndex = 1;
                else if (apDungLoai == "TIECCUOI")
                    cbbLoaiApDung.SelectedIndex = 2;

                DateTime now = DateTime.Now.Date;
                DateTime tgBatDau = dateNgayBatDau.Value.Date;
                DateTime tgKetThuc = dateNgayKetThuc.Value.Date;
                // Checkbox "Sử dụng" = true nếu CTKM đang trong thời gian áp dụng
                checkSuDung.Checked = (tgBatDau <= now && tgKetThuc >= now);

                _isLoadingData = false; // Reset flag sau khi load xong
            }
            catch (Exception ex)
            {
                _isLoadingData = false; // Reset flag nếu có lỗi
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckSuDung_CheckedChanged(object sender, EventArgs e)
        {
            // Bỏ qua nếu đang load data
            if (_isLoadingData)
                return;

            DateTime now = DateTime.Now.Date;
            DateTime tgBatDau = dateNgayBatDau.Value.Date;
            DateTime tgKetThuc;

            if (checkSuDung.Checked)
            {
                // Khi check (sử dụng) -> ngày kết thúc tăng 2 ngày so với ngày hiện tại
                tgKetThuc = now.AddDays(2);

                // Đảm bảo ngày bắt đầu <= ngày kết thúc
                // Nếu ngày bắt đầu > ngày kết thúc, đặt ngày bắt đầu = hôm nay
                if (tgBatDau > tgKetThuc)
                {
                    tgBatDau = now;
                }

                // Cập nhật giá trị vào datePicker
                dateNgayBatDau.Value = tgBatDau;
                dateNgayKetThuc.Value = tgKetThuc;
            }
            else
            {
                // Khi uncheck (không sử dụng) -> điều chỉnh ngày kết thúc để đảm bảo trạng thái "đã hết hạn"
                // Đặt ngày kết thúc = hôm qua để đảm bảo trạng thái là "hết hạn"
                tgKetThuc = now.AddDays(-1);

                // Đảm bảo ngày kết thúc > ngày bắt đầu để không bị lỗi validation trong BLL
                // BLL yêu cầu: tgKetThuc > tgBatDau (không được <=)
                if (tgBatDau >= tgKetThuc)
                {
                    // Nếu ngày bắt đầu >= ngày kết thúc (hôm qua), đặt ngày bắt đầu = ngày kết thúc - 1 ngày
                    tgBatDau = tgKetThuc.AddDays(-1);
                }

                // Cập nhật giá trị vào datePicker
                dateNgayBatDau.Value = tgBatDau;
                dateNgayKetThuc.Value = tgKetThuc;
            }
        }

        // Method để thông báo cho các form Voucher đang mở để reload dữ liệu
        private void NotifyVoucherFormsToReload()
        {
            try
            {
                // Tìm tất cả các form Voucher đang mở và reload dữ liệu
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Frm_Voucher voucherForm)
                    {
                        // Gọi method ReloadData của form Voucher
                        voucherForm.ReloadData();
                    }
                }
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi nếu không tìm thấy form Voucher
                System.Diagnostics.Debug.WriteLine($"Không thể thông báo form Voucher: {ex.Message}");
            }
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

            txtGiamTD.Enabled = true;
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int voucherCount = _bll.CountVouchersByKmId(_kmId);
                string confirmMessage = "Bạn có chắc chắn muốn xóa chương trình khuyến mãi này?";

                if (voucherCount > 0)
                {
                    confirmMessage += $"\n\nLưu ý: Sẽ xóa luôn {voucherCount} voucher đang sử dụng chương trình khuyến mãi này.";
                }

                DialogResult result = MessageBox.Show(
                    confirmMessage,
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    voucherCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = _bll.Delete(_kmId);
                    if (success)
                    {
                        // Thông báo cho các form Voucher đang mở để reload dữ liệu
                        // Vì các voucher liên quan đã bị xóa
                        NotifyVoucherFormsToReload();

                        string successMessage = "Xóa chương trình khuyến mãi thành công!";
                        if (voucherCount > 0)
                        {
                            successMessage += $"\nĐã xóa {voucherCount} voucher liên quan.";
                        }

                        MessageBox.Show(successMessage, "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

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

                decimal giaTri = 0;

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

                if (hinhThuc == "PERCENT")
                {
                    if (giaTri < 5)
                    {
                        MessageBox.Show("Giá trị giảm theo % không được nhỏ hơn 5%!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGiamTD.Focus();
                        return;
                    }
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
                    if (giaTri < 10000)
                    {
                        MessageBox.Show("Giá trị giảm theo số tiền không được nhỏ hơn 10.000 đ!", "Lỗi",
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

                // Lấy giá trị ngày từ datePicker (đã được điều chỉnh bởi CheckSuDung_CheckedChanged)
                DateTime tgBatDau = dateNgayBatDau.Value.Date;
                DateTime tgKetThuc = dateNgayKetThuc.Value.Date;

                // Validation: đảm bảo ngày kết thúc > ngày bắt đầu (BLL yêu cầu strict >)
                // Nếu không hợp lệ, điều chỉnh lại dựa trên trạng thái checkbox
                if (tgKetThuc <= tgBatDau)
                {
                    DateTime now = DateTime.Now.Date;

                    if (!checkSuDung.Checked)
                    {
                        // Nếu uncheck (hết hạn), đặt ngày kết thúc = hôm qua
                        tgKetThuc = now.AddDays(-1);
                        // Đảm bảo ngày kết thúc > ngày bắt đầu
                        if (tgBatDau >= tgKetThuc)
                        {
                            // Nếu ngày bắt đầu >= hôm qua, đặt ngày bắt đầu = hôm qua - 1 ngày
                            tgBatDau = tgKetThuc.AddDays(-1);
                        }
                    }
                    else
                    {
                        // Nếu check (đang áp dụng), đặt ngày kết thúc = hôm nay + 2 ngày
                        tgKetThuc = now.AddDays(2);
                        // Đảm bảo ngày kết thúc > ngày bắt đầu
                        if (tgBatDau >= tgKetThuc)
                        {
                            // Nếu ngày bắt đầu >= hôm nay + 2, đặt ngày bắt đầu = hôm nay
                            tgBatDau = now;
                        }
                    }

                    // Cập nhật lại giá trị vào datePicker
                    dateNgayBatDau.Value = tgBatDau;
                    dateNgayKetThuc.Value = tgKetThuc;

                    // Lấy lại giá trị sau khi điều chỉnh
                    tgBatDau = dateNgayBatDau.Value.Date;
                    tgKetThuc = dateNgayKetThuc.Value.Date;
                }

                string apDungLoai = "ALL";
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
                    // Thông báo cho các form Voucher đang mở để reload dữ liệu
                    // Vì trạng thái voucher phụ thuộc vào trạng thái CTKM
                    NotifyVoucherFormsToReload();

                    MessageBox.Show("Cập nhật chương trình khuyến mãi thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

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
