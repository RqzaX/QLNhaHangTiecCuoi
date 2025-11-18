using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using QLNhaHangTiecCuoi.Share;
using UI.Common;
using static UI.Controls.GunaToast;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class HoaDonThanhToanPanel : Form
    {
        private decimal subtotalValue = 0;
        private decimal vatPercent = 0;
        private decimal discountValue = 0;
        private decimal depositValue = 0;
        private decimal baseTotalForDiscount = 0;
        private decimal soTienConLaiGoc = 0;
        public int HoaDonId { get; set; }
        private int? _kmId = null;
        private int? _voucherId = null;
        private readonly DatabaseHelper _db = new DatabaseHelper();
        private readonly HoaDonBLL _hoaDonBLL;
        public event EventHandler? PaymentCompleted;

        public HoaDonThanhToanPanel()
        {
            InitializeComponent();
            _hoaDonBLL = new HoaDonBLL(_db);
            SetVoucherApplied(false);
            SelectPaymentMethodCash();

            btnVoucher.Click += OnApplyVoucherClick;
            btnBoKhuyenMai.Click += (s, e) => SetVoucherApplied(false);

            btnTienMat.Click += (s, e) => SelectPaymentMethodCash();
            btnThe.Click += (s, e) => SelectPaymentMethodCard();
            btnChuyenKhoan.Click += (s, e) => SelectPaymentMethodBankTransfer();

            txtTienNhan.KeyPress += TxtTienNhan_KeyPress;
            txtTienNhan.TextChanged += TxtTienNhan_TextChanged;
            btnThanhToan.Click += BtnThanhToan_Click;
            btnHienThịQR.Click += BtnHienThiQR_Click;
        }

        public void SetTitle(string title)
        {
            label1.Text = title;
        }

        // Bind số liệu từ hóa đơn thực tế
        public void BindAmounts(decimal subtotal, decimal vatPercentInput, decimal total)
        {
            subtotalValue = subtotal;
            vatPercent = vatPercentInput;
            soTienConLaiGoc = total;
            lbTamTinh.Text = FormatCurrency(subtotalValue);
            label6.Text = $"VAT ({vatPercent:0}%)";

            var vatValue = Math.Round(subtotalValue * vatPercent / 100m, 0);
            lbVAT.Text = FormatCurrency(vatValue);
            lbTongCong.Text = FormatCurrency(total);
            btnThanhToan.Text = $"Thanh toán {FormatCurrency(total)}";
        }
        // Hiển thị tiền cọc (dành cho hóa đơn tiệc cưới)
        public void SetDeposit(decimal amount)
        {
            depositValue = amount < 0 ? -amount : amount;
            lbKhuyenMai.Visible = true;
            lbSoTienGiamGia.Visible = true;
            lbKhuyenMai.Text = "Tiền cọc";
            lbSoTienGiamGia.Text = FormatCurrency(-depositValue);
            UpdateTotals(); // Cập nhật tổng sau khi set tiền cọc
        }
        // Thiết lập số tiền gốc để tính khuyến mãi (áp dụng cho tổng)
        public void SetBaseDiscountTotal(decimal total)
        {
            baseTotalForDiscount = total < 0 ? 0 : total;
        }
        // Áp dụng voucher
        private void OnApplyVoucherClick(object? sender, EventArgs e)
        {
            decimal currentTotal = ParseCurrency(lbTongCong.Text);
            if (currentTotal <= 0)
            {
                ParseMoneyAndVat();
                var vatValue = Math.Round(subtotalValue * vatPercent / 100m, 0);
                currentTotal = subtotalValue + vatValue;
            }
            
            // Lấy loại hóa đơn từ HoaDonId
            string invoiceLoai = "";
            if (HoaDonId > 0)
            {
                try
                {
                    var hoaDon = _hoaDonBLL.GetHoaDonById(HoaDonId);
                    if (hoaDon != null)
                    {
                        invoiceLoai = hoaDon["loai"]?.ToString() ?? "";
                    }
                }
                catch { }
            }
            
            using (var f = new UI.ApDungVoucher(currentTotal, invoiceLoai))
            {
                if (f.ShowDialog(this) == DialogResult.OK && f.IsApplied)
                {
                    lbTenKhuyenMai.Text = f.ProgramName;
                    lbMaKM.Text = string.IsNullOrEmpty(f.ProgramCode) ? "" : f.ProgramCode;


                    decimal currentTotalForDiscount = baseTotalForDiscount > 0
                        ? baseTotalForDiscount
                        : ParseCurrency(lbTongCong.Text);
                    if (currentTotalForDiscount <= 0)
                    {
                        ParseMoneyAndVat();
                        var vatValue = Math.Round(subtotalValue * vatPercent / 100m, 0);
                        currentTotalForDiscount = subtotalValue + vatValue;
                    }

                    decimal calculatedDiscount = 0;
                    if (f.DiscountType == "PERCENT")
                    {
                        calculatedDiscount = Math.Round(currentTotalForDiscount * f.DiscountValue / 100m, 0);
                        lbGiam.Text = $"Giảm {f.DiscountValue}%";
                    }
                    else
                    {
                        calculatedDiscount = f.DiscountValue;
                        lbGiam.Text = "Giảm tiền";
                    }

                    calculatedDiscount = Math.Min(calculatedDiscount, currentTotalForDiscount);

                    discountValue = calculatedDiscount;
                    lbSoTienGiam.Text = FormatCurrency(-discountValue);
                    SetVoucherApplied(true);

                    _kmId = f.ProgramId;
                    _voucherId = f.VoucherId;

                    UpdateTotals();
                }
            }
        }
        // Hiển thị thông tin khuyến mãi và voucher
        private void SetVoucherApplied(bool applied)
        {
            panelThongTinKhuyenMai.Visible = applied;
            btnVoucher.Visible = !applied;
            bool showDeposit = depositValue > 0;
            lbKhuyenMai.Visible = showDeposit || applied;
            lbSoTienGiamGia.Visible = showDeposit || applied;

            if (!applied)
            {
                // Nếu có tiền cọc, hiển thị tiền cọc
                if (showDeposit)
                {
                    lbKhuyenMai.Text = "Tiền cọc";
                    lbSoTienGiamGia.Text = FormatCurrency(-depositValue);
                }
                else
                {
                    lbKhuyenMai.Text = "Khuyến mãi";
                    lbSoTienGiamGia.Text = "";
                }
                lbTenKhuyenMai.Text = "Khuyễn mãi {tên khuyến mãi}";
                lbMaKM.Text = "{mã khuyễn mãi}";
                lbGiam.Text = "";
                lbSoTienGiam.Text = "";
            }
            else
            {
                // Khi có khuyến mãi, vẫn hiển thị tiền cọc nếu có
                if (showDeposit)
                {
                    lbKhuyenMai.Text = "Tiền cọc";
                    lbSoTienGiamGia.Text = FormatCurrency(-depositValue);
                }
                else
                {
                    lbKhuyenMai.Text = "Khuyến mãi";
                    lbSoTienGiamGia.Text = FormatCurrency(-discountValue);
                }
                if (string.IsNullOrWhiteSpace(lbGiam.Text)) lbGiam.Text = "Giảm 0%";
                if (string.IsNullOrWhiteSpace(lbSoTienGiam.Text)) lbSoTienGiam.Text = "-0 đ";
            }

            if (!applied)
            {
                discountValue = 0;
                _kmId = null;
                _voucherId = null;
                UpdateTotals();
            }
        }
        // Reset các nút thanh toán và ẩn tiền thừa
        private void ResetPaymentButtons()
        {
            btnTienMat.BorderThickness = 1;
            btnTienMat.FillColor = Color.White;
            btnTienMat.BorderColor = Color.FromArgb(224, 224, 224);
            btnThe.BorderThickness = 1;
            btnThe.FillColor = Color.White;
            btnThe.BorderColor = Color.FromArgb(224, 224, 224);
            btnChuyenKhoan.BorderThickness = 1;
            btnChuyenKhoan.FillColor = Color.White;
            btnChuyenKhoan.BorderColor = Color.FromArgb(224, 224, 224);

            txtTienNhan.Visible = false;
            btnHienThịQR.Visible = false;
            lbTextTienThua.Visible = false;
            lbSoTienThua.Visible = false;
        }
        // Chọn phương thức thanh toán tiền mặt
        private void SelectPaymentMethodCash()
        {
            ResetPaymentButtons();
            btnTienMat.BorderThickness = 2;
            btnTienMat.FillColor = Color.White;
            btnTienMat.BorderColor = Color.Black;
            txtTienNhan.Visible = true;
            TxtTienNhan_TextChanged(null, EventArgs.Empty);
        }
        // Chọn phương thức thanh toán thẻ
        private void SelectPaymentMethodCard()
        {
            ResetPaymentButtons();
            btnThe.BorderThickness = 2;
            btnThe.BorderColor = Color.Black;
            lbTextTienThua.Visible = false;
            lbSoTienThua.Visible = false;
            txtTienNhan.Text = string.Empty;
        }
        // Chọn phương thức thanh toán chuyển khoản
        private void SelectPaymentMethodBankTransfer()
        {
            ResetPaymentButtons();
            btnChuyenKhoan.BorderThickness = 2;
            btnChuyenKhoan.BorderColor = Color.Black;
            btnHienThịQR.Visible = true;
            lbTextTienThua.Visible = false;
            lbSoTienThua.Visible = false;
            txtTienNhan.Text = string.Empty;
        }
        // Tính tổng tiền và VAT
        private void ParseMoneyAndVat()
        {
            subtotalValue = ParseCurrency(lbTamTinh.Text);
            var percentText = label6.Text;
            var openIdx = percentText.IndexOf('(');
            var closeIdx = percentText.IndexOf('%');
            if (openIdx >= 0 && closeIdx > openIdx)
            {
                var p = percentText.Substring(openIdx + 1, closeIdx - openIdx - 1);
                decimal.TryParse(p, out vatPercent);
            }
            else
            {
                vatPercent = 0;
            }
        }
        // Cập nhật tổng tiền sau khi áp dụng khuyến mãi
        private void UpdateTotals()
        {

            decimal currentTotal = soTienConLaiGoc;
            if (currentTotal <= 0)
            {
                ParseMoneyAndVat();
                var vatValue = Math.Round(subtotalValue * vatPercent / 100m, 0);
                currentTotal = subtotalValue + vatValue;
            }
            else
            {
                ParseMoneyAndVat();
                var vatValue = Math.Round(subtotalValue * vatPercent / 100m, 0);
            }

            var total = currentTotal - discountValue;
            if (total < 0) total = 0;

            ParseMoneyAndVat();
            var vatValueDisplay = Math.Round(subtotalValue * vatPercent / 100m, 0);
            lbVAT.Text = FormatCurrency(vatValueDisplay);
            
            lbTongCong.Text = FormatCurrency(total);
            btnThanhToan.Text = $"Thanh toán {FormatCurrency(total)}";
            
            if (depositValue > 0)
            {
                lbKhuyenMai.Text = "Tiền cọc";
                lbKhuyenMai.Visible = true;
                lbSoTienGiamGia.Visible = true;
                lbSoTienGiamGia.Text = FormatCurrency(-depositValue);
            }
            else if (discountValue > 0)
            {
                // Hiển thị khuyến mãi nếu có (không có tiền cọc)
                lbKhuyenMai.Text = "Khuyến mãi";
                lbKhuyenMai.Visible = true;
                lbSoTienGiamGia.Visible = true;
                lbSoTienGiamGia.Text = FormatCurrency(-discountValue);
            }
        }

        private bool isFormattingTienNhan = false;
        private void TxtTienNhan_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        // Khi người dùng nhập tiền nhận, tự động format thành số có phân cách
        private void TxtTienNhan_TextChanged(object? sender, EventArgs e)
        {
            if (isFormattingTienNhan) return;
            var rawDigits = new string(txtTienNhan.Text.Where(char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(rawDigits))
            {
                lbTextTienThua.Visible = false;
                lbSoTienThua.Text = "";
                return;
            }

            if (!decimal.TryParse(rawDigits, out var received)) received = 0m;
            var oldLen = txtTienNhan.Text.Length;
            var sel = txtTienNhan.SelectionStart;
            var formatted = string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0}", received);
            isFormattingTienNhan = true;
            txtTienNhan.Text = formatted;
            var newLen = txtTienNhan.Text.Length;
            txtTienNhan.SelectionStart = Math.Max(0, Math.Min(newLen, sel + (newLen - oldLen)));
            isFormattingTienNhan = false;

            // tính tiền thừa/còn thiếu
            var totalDue = ParseCurrency(lbTongCong.Text);
            var delta = received - totalDue;
            lbTextTienThua.Visible = true;
            if (delta >= 0)
            {
                lbTextTienThua.Text = "Tiền thừa";
                lbSoTienThua.Visible = true;
                lbSoTienThua.ForeColor = Color.MediumSlateBlue;
                lbSoTienThua.Text = FormatCurrency(delta);
            }
            else
            {
                lbTextTienThua.Text = "Còn thiếu";
                lbSoTienThua.Visible = true;
                lbSoTienThua.ForeColor = Color.IndianRed;
                lbSoTienThua.Text = FormatCurrency(-delta);
            }
        }
        // Chuyển đổi text thành số tiền
        private static decimal ParseCurrency(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            var digits = new string(text.Where(ch => char.IsDigit(ch)).ToArray());
            if (decimal.TryParse(digits, out var v)) return v;
            return 0;
        }
        // Chuyển đổi số tiền thành text
        private static string FormatCurrency(decimal value)
        {
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0} đ", value);
        }

        // Xử lý thanh toán
        private void BtnThanhToan_Click(object? sender, EventArgs e)
        {
            try
            {
                Form? ownerForm = FindForm();
                if (ownerForm == null) ownerForm = this;

                if (HoaDonId <= 0)
                {
                    GunaToast.Show(ownerForm, "Không tìm thấy thông tin hóa đơn!", ToastType.Error);
                    return;
                }

                // Kiểm tra hóa đơn còn tồn tại và chưa thanh toán
                var hoaDon = _hoaDonBLL.GetHoaDonById(HoaDonId);
                if (hoaDon == null)
                {
                    GunaToast.Show(ownerForm, "Hóa đơn không tồn tại!", ToastType.Error);
                    return;
                }

                if (hoaDon["trang_thai"].ToString() != "CHỜ TT")
                {
                    GunaToast.Show(ownerForm, "Hóa đơn này đã được thanh toán hoặc đã bị hủy!", ToastType.Info);
                    return;
                }

                // Tính tổng tiền cần thanh toán
                decimal totalAmount = ParseCurrency(lbTongCong.Text);
                if (totalAmount <= 0)
                {
                    GunaToast.Show(ownerForm, "Số tiền thanh toán không hợp lệ!", ToastType.Error);
                    return;
                }

                // Xác định phương thức thanh toán
                string hinhThuc = "Tiền mặt";
                if (btnThe.BorderColor == Color.Black)
                {
                    hinhThuc = "Thẻ";
                }
                else if (btnChuyenKhoan.BorderColor == Color.Black)
                {
                    hinhThuc = "Chuyển khoản";
                }

                // Kiểm tra nếu thanh toán tiền mặt: tiền khách đưa phải >= tổng tiền
                if (hinhThuc == "Tiền mặt")
                {
                    decimal receivedAmount = ParseCurrency(txtTienNhan.Text);
                    if (receivedAmount < totalAmount)
                    {
                        decimal missing = totalAmount - receivedAmount;
                        GunaToast.Show(ownerForm, $"Số tiền khách đưa chưa đủ! Còn thiếu: {FormatCurrency(missing)}", ToastType.Info);
                        return;
                    }
                }

                // Xác nhận thanh toán
                var result = MessageBox.Show(
                    $"Xác nhận thanh toán hóa đơn?\n\n" +
                    $"Tổng tiền: {FormatCurrency(totalAmount)}\n" +
                    $"Phương thức: {hinhThuc}\n" +
                    $"Khuyến mãi: {(discountValue > 0 ? FormatCurrency(discountValue) : "Không có")}",
                    "Xác nhận thanh toán",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // Xử lý thanh toán
                bool success = _hoaDonBLL.ProcessPayment(
                    HoaDonId,
                    totalAmount,
                    hinhThuc,
                    out string errorMessage,
                    Session.HoTen,
                    _kmId,
                    _voucherId,
                    discountValue > 0 ? discountValue : null
                );

                if (success)
                {
                    // Kiểm tra và cập nhật trạng thái hóa đơn nếu số tiền còn lại = 0 hoặc trạng thái đặt sảnh = "ĐÃ THANH TOÁN"
                    _hoaDonBLL.KiemTraVaCapNhatTrangThaiHoaDon(HoaDonId, out string errorTrangThai);
                    if (!string.IsNullOrEmpty(errorTrangThai))
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi cập nhật trạng thái hóa đơn: {errorTrangThai}");
                    }

                    GunaToast.Show(ownerForm, $"Thanh toán thành công! Tổng tiền: {FormatCurrency(totalAmount)}", ToastType.Success);
                    PaymentCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    string errorMsg = !string.IsNullOrEmpty(errorMessage) 
                        ? $"Thanh toán thất bại! {errorMessage}" 
                        : "Thanh toán thất bại! Vui lòng thử lại.";
                    GunaToast.Show(ownerForm, errorMsg, ToastType.Error);
                }
            }
            catch (Exception ex)
            {
                Form? ownerForm = FindForm();
                if (ownerForm == null) ownerForm = this;
                GunaToast.Show(ownerForm, $"Lỗi khi xử lý thanh toán: {ex.Message}", ToastType.Error);
            }
        }

        private void HoaDonThanhToanPanel_Load(object sender, EventArgs e)
        {

        }

        // Event handler cho button Hiển thị QR
        private void BtnHienThiQR_Click(object? sender, EventArgs e)
        {
            try
            {
                decimal soTien = ParseCurrency(lbTongCong.Text);
                if (soTien <= 0)
                {
                    GunaToast.Show(this, "Số tiền thanh toán không hợp lệ!", ToastType.Error);
                    return;
                }

                // Lấy thông tin bàn từ database
                string noiDung = "Thanh toán bàn";
                if (HoaDonId > 0)
                {
                    var hoaDon = _hoaDonBLL.GetHoaDonById(HoaDonId);
                    if (hoaDon != null && hoaDon["ban_sanh"] != DBNull.Value)
                    {
                        string banSanh = hoaDon["ban_sanh"].ToString() ?? "";
                        if (!string.IsNullOrWhiteSpace(banSanh))
                        {
                            noiDung = $"Thanh toán bàn {banSanh}";
                        }
                    }
                }

                var formQR = new Frm_QRThanhToan(soTien, noiDung)
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                formQR.ShowDialog(this);
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi hiển thị QR thanh toán: {ex.Message}", ToastType.Error);
            }
        }
    }
}
