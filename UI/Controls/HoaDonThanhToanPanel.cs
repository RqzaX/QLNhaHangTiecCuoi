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
using static UI.Controls.GunaToast;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class HoaDonThanhToanPanel : Form
    {
        private decimal subtotalValue = 0;
        private decimal vatPercent = 0;
        private decimal discountValue = 0;

        // Thông tin hóa đơn và khuyến mãi
        public int HoaDonId { get; set; }
        private int? _kmId = null;
        private int? _voucherId = null;
        private readonly DatabaseHelper _db = new DatabaseHelper();
        private readonly HoaDonBLL _hoaDonBLL;

        // Event khi thanh toán thành công
        public event EventHandler? PaymentCompleted;

        public HoaDonThanhToanPanel()
        {
            InitializeComponent();
            _hoaDonBLL = new HoaDonBLL(_db);
            SetVoucherApplied(false);
            SelectPaymentMethodCash();

            // Events
            btnVoucher.Click += OnApplyVoucherClick;
            btnBoKhuyenMai.Click += (s, e) => SetVoucherApplied(false);

            btnTienMat.Click += (s, e) => SelectPaymentMethodCash();
            btnThe.Click += (s, e) => SelectPaymentMethodCard();
            btnChuyenKhoan.Click += (s, e) => SelectPaymentMethodBankTransfer();

            // Nhập tiền khách đưa: chỉ cho nhập số và tự format VNĐ
            txtTienNhan.KeyPress += TxtTienNhan_KeyPress;
            txtTienNhan.TextChanged += TxtTienNhan_TextChanged;
            btnThanhToan.Click += BtnThanhToan_Click;
        }

        // Thiết lập tiêu đề hiển thị khu vực thanh toán
        public void SetTitle(string title)
        {
            label1.Text = title;
        }

        // Bind số liệu từ hóa đơn thực tế
        public void BindAmounts(decimal subtotal, decimal vatPercentInput, decimal total)
        {
            subtotalValue = subtotal;
            vatPercent = vatPercentInput;
            lbTamTinh.Text = FormatCurrency(subtotalValue);
            label6.Text = $"VAT ({vatPercent:0}%)";

            var vatValue = Math.Round(subtotalValue * vatPercent / 100m, 0);
            lbVAT.Text = FormatCurrency(vatValue);
            lbTongCong.Text = FormatCurrency(total);
            btnThanhToan.Text = $"Thanh toán {FormatCurrency(total)}";
        }
        // Áp dụng voucher
        private void OnApplyVoucherClick(object? sender, EventArgs e)
        {
            // Tính tổng hiện tại để hiển thị số tiền giảm chính xác trong dialog
            ParseMoneyAndVat();
            var vatValue = Math.Round(subtotalValue * vatPercent / 100m, 0);
            var billTotal = subtotalValue + vatValue;
            using (var f = new UI.ApDungVoucher(billTotal))
            {
                if (f.ShowDialog(this) == DialogResult.OK && f.IsApplied)
                {
                    lbTenKhuyenMai.Text = f.ProgramName;
                    lbMaKM.Text = string.IsNullOrEmpty(f.ProgramCode) ? "" : f.ProgramCode;

                    ParseMoneyAndVat();

                    decimal calculatedDiscount = 0;
                    if (f.DiscountType == "PERCENT")
                    {
                        calculatedDiscount = Math.Round(subtotalValue * f.DiscountValue / 100m, 0);
                        lbGiam.Text = $"Giảm {f.DiscountValue}%";
                    }
                    else
                    {
                        calculatedDiscount = f.DiscountValue;
                        lbGiam.Text = "Giảm tiền";
                    }

                    calculatedDiscount = Math.Min(calculatedDiscount, subtotalValue);

                    discountValue = calculatedDiscount;
                    lbSoTienGiam.Text = FormatCurrency(-discountValue);
                    SetVoucherApplied(true);

                    // Lưu thông tin khuyến mãi/voucher
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
            lbKhuyenMai.Visible = applied;
            lbSoTienGiamGia.Visible = applied;

            if (!applied)
            {
                lbTenKhuyenMai.Text = "Khuyễn mãi {tên khuyến mãi}";
                lbMaKM.Text = "{mã khuyễn mãi}";
                lbGiam.Text = "";
                lbSoTienGiam.Text = "";
                lbSoTienGiamGia.Text = "";
            }
            else
            {
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
            ParseMoneyAndVat();
            var vatValue = Math.Round(subtotalValue * vatPercent / 100m, 0);
            var total = subtotalValue + vatValue - discountValue;
            if (total < 0) total = 0;

            lbVAT.Text = FormatCurrency(vatValue);
            lbTongCong.Text = FormatCurrency(total);
            lbSoTienGiamGia.Text = FormatCurrency(-discountValue);
            btnThanhToan.Text = $"Thanh toán {FormatCurrency(total)}";
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

            // format lại theo VNĐ, giữ vị trí con trỏ tương đối
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
                    _kmId,
                    _voucherId,
                    discountValue > 0 ? discountValue : null
                );

                if (success)
                {
                    GunaToast.Show(ownerForm, $"Thanh toán thành công! Tổng tiền: {FormatCurrency(totalAmount)}", ToastType.Success);
                    // Trigger event để form cha refresh danh sách
                    PaymentCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    GunaToast.Show(ownerForm, "Thanh toán thất bại! Vui lòng thử lại.", ToastType.Error);
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
    }
}
