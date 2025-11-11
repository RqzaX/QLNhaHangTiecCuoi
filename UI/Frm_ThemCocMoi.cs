using BLL;
using QLNhaHangTiecCuoi.BLL;
using System;
using System.Data;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_ThemCocMoi : Form
    {
        private int _datSanhId;
        private int? _hopDongId;
        private int? _cocId;
        private int? _ttId;
        private bool _isEditMode = false;
        private bool _isThanhToan = false; 
        private DatSanhBLL _datSanhBLL;
        private decimal _tongTienDatSanh;
        private decimal _tongCocDaThu;
        private decimal _soTienConThieu = 0;
        private decimal _soTienCuCuaThanhToan = 0;

        // Constructor cho thêm cọc mới
        public Frm_ThemCocMoi(int datSanhId)
        {
            InitializeComponent();
            _datSanhId = datSanhId;
            _datSanhBLL = new DatSanhBLL();
            _isEditMode = false;
            _isThanhToan = false;

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;

            UpdateTitle();
            LoadThongTin();
            LoadHinhThuc();
        }

        public Frm_ThemCocMoi(int datSanhId, int cocId)
        {
            InitializeComponent();
            _datSanhId = datSanhId;
            _cocId = cocId;
            _datSanhBLL = new DatSanhBLL();
            _isEditMode = true;
            _isThanhToan = false;

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;

            UpdateTitle();
            LoadThongTin();
            LoadHinhThuc();
            LoadThongTinCoc();
        }

        public Frm_ThemCocMoi(int datSanhId, int ttId, bool isThanhToan)
        {
            InitializeComponent();
            _datSanhId = datSanhId;
            _ttId = ttId;
            _datSanhBLL = new DatSanhBLL();
            _isEditMode = true;
            _isThanhToan = true;

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;

            UpdateTitle();
            LoadThongTin();
            LoadHinhThuc();
            LoadThongTinThanhToan();
        }

        private void UpdateTitle()
        {
            if (lbTitle != null)
            {
                if (_isEditMode)
                {
                    if (_isThanhToan)
                    {
                        lbTitle.Text = "Cập nhật thanh toán";
                    }
                    else
                    {
                        lbTitle.Text = "Cập nhật cọc";
                    }
                }
                else
                {
                    lbTitle.Text = "Thêm đợt cọc mới";
                }
            }
        }

        private void LoadThongTin()
        {
            try
            {
                _hopDongId = _datSanhBLL.LayHopDongId(_datSanhId);
                
                if (!_hopDongId.HasValue || _hopDongId.Value <= 0)
                {
                    MessageBox.Show("Chưa có hợp đồng cho đơn đặt sảnh này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                _tongTienDatSanh = _datSanhBLL.LayTongDuKien(_hopDongId.Value);
                _tongCocDaThu = _datSanhBLL.LayTongCocDaThu(_datSanhId);
                if (!_isEditMode)
                {
                    dtpNgayNop.Value = DateTime.Now;
                }
                CapNhatLabelSoTienCocToiThieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void CapNhatLabelSoTienCocToiThieu()
        {
            try
            {
                if (lbSoTien != null && !_isThanhToan)
                {
                    decimal soTienCocToiThieu = _tongTienDatSanh * 0.20m;
                    lbSoTien.Text = $"Số tiền cọc tối thiểu (20%): {FormatCurrency(soTienCocToiThieu)}";
                }
                else if (lbSoTien != null && _isThanhToan)
                {
                    TinhSoTienConThieu();
                    lbSoTien.Text = $"Số tiền còn thiếu: {FormatCurrency(_soTienConThieu)}";
                }
            }
            catch (Exception ex)
            {
                if (lbSoTien != null)
                {
                    if (_isThanhToan)
                        lbSoTien.Text = "Số tiền còn thiếu: 0 đ";
                    else
                        lbSoTien.Text = "Số tiền (VNĐ)";
                }
            }
        }

        private void TinhSoTienConThieu()
        {
            try
            {
                if (!_hopDongId.HasValue || _hopDongId.Value <= 0)
                {
                    _soTienConThieu = 0;
                    return;
                }

                decimal tongDuKien = _datSanhBLL.LayTongDuKien(_hopDongId.Value);

                decimal tongCoc = 0;
                DataTable dtCoc = _datSanhBLL.LayDanhSachCoc(_hopDongId.Value);
                if (dtCoc != null)
                {
                    foreach (DataRow row in dtCoc.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                            tongCoc += Convert.ToDecimal(row["so_tien"]);
                    }
                }

                decimal tongThanhToan = 0;
                DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
                if (dtThanhToan != null)
                {
                    foreach (DataRow row in dtThanhToan.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                        {
                            // Trong chế độ edit: loại trừ số tiền cũ của khoản đang edit
                            if (_isEditMode && _isThanhToan && _ttId.HasValue)
                            {
                                if (row["tt_id"] != DBNull.Value && Convert.ToInt32(row["tt_id"]) == _ttId.Value)
                                {
                                    continue;
                                }
                            }
                            tongThanhToan += Convert.ToDecimal(row["so_tien"]);
                        }
                    }
                }

                // Tính tổng còn lại (không tính số tiền cũ của khoản đang edit)
                decimal tongConLai = tongDuKien - tongCoc - tongThanhToan;
                if (tongConLai < 0)
                    tongConLai = 0;
                if (_isEditMode && _isThanhToan && _soTienCuCuaThanhToan > 0)
                {
                    _soTienConThieu = tongConLai + _soTienCuCuaThanhToan;
                }
                else
                {
                    _soTienConThieu = tongConLai;
                }
            }
            catch (Exception ex)
            {
                _soTienConThieu = 0;
            }
        }

        private void LoadHinhThuc()
        {
            cbHinhThuc.Items.Clear();
            cbHinhThuc.Items.Add("Tiền mặt");
            cbHinhThuc.Items.Add("Thẻ");
            cbHinhThuc.Items.Add("Chuyển khoản");
            if (!_isEditMode)
            {
                cbHinhThuc.SelectedIndex = 0;
            }
        }

        private void LoadThongTinCoc()
        {
            if (!_cocId.HasValue || _cocId.Value <= 0)
                return;

            try
            {
                DataRow? cocInfo = _datSanhBLL.LayThongTinCoc(_cocId.Value);
                if (cocInfo != null)
                {
                    dtpNgayNop.Value = cocInfo["ngay_nop"] != DBNull.Value ? Convert.ToDateTime(cocInfo["ngay_nop"]) : DateTime.Now;
                    string hinhThuc = cocInfo["hinh_thuc"] != DBNull.Value ? cocInfo["hinh_thuc"].ToString() : "";
                    decimal soTien = cocInfo["so_tien"] != DBNull.Value ? Convert.ToDecimal(cocInfo["so_tien"]) : 0;
                    string ghiChu = cocInfo["ghi_chu"] != DBNull.Value ? cocInfo["ghi_chu"].ToString() : "";

                    for (int i = 0; i < cbHinhThuc.Items.Count; i++)
                    {
                        if (cbHinhThuc.Items[i].ToString() == hinhThuc)
                        {
                            cbHinhThuc.SelectedIndex = i;
                            break;
                        }
                    }

                    txtSoTien.Text = soTien.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                    txtGhiChu.Text = ghiChu;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin cọc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadThongTinThanhToan()
        {
            if (!_ttId.HasValue || _ttId.Value <= 0)
                return;

            try
            {
                DataRow? ttInfo = _datSanhBLL.LayThongTinThanhToan(_ttId.Value);
                if (ttInfo != null)
                {
                    _soTienCuCuaThanhToan = ttInfo["so_tien"] != DBNull.Value ? Convert.ToDecimal(ttInfo["so_tien"]) : 0;
                    
                    TinhSoTienConThieu();
                    
                    CapNhatLabelSoTienCocToiThieu();

                    dtpNgayNop.Value = ttInfo["ngay_tt"] != DBNull.Value ? Convert.ToDateTime(ttInfo["ngay_tt"]) : DateTime.Now;
                    string hinhThuc = ttInfo["hinh_thuc"] != DBNull.Value ? ttInfo["hinh_thuc"].ToString() : "";
                    decimal soTien = _soTienCuCuaThanhToan;
                    string noiDung = ttInfo["noi_dung"] != DBNull.Value ? ttInfo["noi_dung"].ToString() : "";

                    for (int i = 0; i < cbHinhThuc.Items.Count; i++)
                    {
                        if (cbHinhThuc.Items[i].ToString() == hinhThuc)
                        {
                            cbHinhThuc.SelectedIndex = i;
                            break;
                        }
                    }

                    // Đảm bảo số tiền không vượt quá số tối đa
                    if (soTien > _soTienConThieu && _soTienConThieu > 0)
                    {
                        soTien = _soTienConThieu;
                    }

                    txtSoTien.Text = soTien.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                    txtGhiChu.Text = noiDung;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnHuy_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CbHinhThuc_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbHinhThuc.SelectedItem != null)
            {
                string hinhThuc = cbHinhThuc.SelectedItem.ToString() ?? "";
                panelQR.Visible = (hinhThuc == "Chuyển khoản");
            }
        }

        private void BtnHienThiQR_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtSoTien.Text.Replace(",", "").Replace(".", ""), out decimal soTien) || soTien <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số tiền hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var formQR = new Frm_QRThanhToan(soTien, $"Cọc đặt sảnh DS{_datSanhId:D6}")
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                formQR.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị QR: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSoTien_TextChanged(object? sender, EventArgs e)
        {
            // Format số tiền
            if (string.IsNullOrWhiteSpace(txtSoTien.Text))
            {
                if (!_isThanhToan)
                {
                    txtSoTien.Text = "0";
                }
                return;
            }

            string text = txtSoTien.Text.Replace(",", "").Replace(".", "");
            if (decimal.TryParse(text, out decimal soTien))
            {
                int selectionStart = txtSoTien.SelectionStart;
                string formattedText = soTien.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                
                if (txtSoTien.Text != formattedText)
                {
                    txtSoTien.Text = formattedText;
                    
                    if (selectionStart <= txtSoTien.Text.Length)
                    {
                        txtSoTien.SelectionStart = selectionStart;
                    }
                    else
                    {
                        txtSoTien.SelectionStart = txtSoTien.Text.Length;
                    }
                }
            }
        }

        private void TxtSoTien_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void TxtSoTien_Leave(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoTien.Text))
                return;

            string text = txtSoTien.Text.Replace(",", "").Replace(".", "");
            if (decimal.TryParse(text, out decimal soTien))
            {
                if (!_isThanhToan)
                {
                    // Tính tổng thanh toán đã có
                    decimal tongThanhToan = 0;
                    if (_hopDongId.HasValue)
                    {
                        DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
                        if (dtThanhToan != null)
                        {
                            foreach (DataRow row in dtThanhToan.Rows)
                            {
                                if (row["so_tien"] != DBNull.Value)
                                {
                                    tongThanhToan += Convert.ToDecimal(row["so_tien"]);
                                }
                            }
                        }
                    }

                    // Tính số tiền còn lại sau khi trừ tổng cọc và tổng thanh toán
                    decimal soTienConLai = _tongTienDatSanh - _tongCocDaThu - tongThanhToan;
                    if (soTienConLai < 0)
                        soTienConLai = 0;

                    if (soTien > soTienConLai && soTienConLai > 0)
                    {
                        txtSoTien.Text = soTienConLai.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                        MessageBox.Show(
                            $"Số tiền đã được điều chỉnh về số tiền tối đa: {FormatCurrency(soTienConLai)}\n" +
                            $"(Tổng cọc + thanh toán không được vượt quá tổng dự kiến)",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
                else
                {
                    TinhSoTienConThieu();

                    if (soTien > _soTienConThieu && _soTienConThieu > 0)
                    {
                        txtSoTien.Text = _soTienConThieu.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                        MessageBox.Show(
                            $"Số tiền đã được điều chỉnh về số tiền tối đa: {FormatCurrency(_soTienConThieu)}",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
        }

        private void BtnXacNhan_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cbHinhThuc.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn hình thức thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtSoTien.Text.Replace(",", "").Replace(".", ""), out decimal soTien) || soTien <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số tiền hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoTien.Focus();
                    return;
                }

                if (!_hopDongId.HasValue)
                {
                    MessageBox.Show("Không tìm thấy hợp đồng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra số tiền thanh toán (chỉ áp dụng cho thanh toán)
                if (_isThanhToan)
                {
                    TinhSoTienConThieu();

                    if (soTien > _soTienConThieu && _soTienConThieu > 0)
                    {
                        MessageBox.Show(
                            $"Số tiền thanh toán không được vượt quá số tiền còn thiếu!\n" +
                            $"Số tiền còn thiếu: {FormatCurrency(_soTienConThieu)}\n" +
                            $"Số tiền đã nhập: {FormatCurrency(soTien)}",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        txtSoTien.Focus();
                        txtSoTien.SelectAll();
                        return;
                    }
                }

                // Kiểm tra số tiền cọc (chỉ áp dụng cho cọc, không áp dụng cho thanh toán)
                if (!_isThanhToan)
                {
                    decimal soTienCocCu = 0;
                    if (_isEditMode && _cocId.HasValue)
                    {
                        DataRow? cocInfo = _datSanhBLL.LayThongTinCoc(_cocId.Value);
                        if (cocInfo != null && cocInfo["so_tien"] != DBNull.Value)
                        {
                            soTienCocCu = Convert.ToDecimal(cocInfo["so_tien"]);
                        }
                    }

                    // Tính tổng thanh toán đã có
                    decimal tongThanhToan = 0;
                    if (_hopDongId.HasValue)
                    {
                        DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
                        if (dtThanhToan != null)
                        {
                            foreach (DataRow row in dtThanhToan.Rows)
                            {
                                if (row["so_tien"] != DBNull.Value)
                                {
                                    tongThanhToan += Convert.ToDecimal(row["so_tien"]);
                                }
                            }
                        }
                    }

                    decimal soTienCocToiThieu = _tongTienDatSanh * 0.20m;
                    decimal tongCocSauKhiCapNhat;

                    if (!_isEditMode)
                    {
                        tongCocSauKhiCapNhat = _tongCocDaThu + soTien;
                    }
                    else
                    {
                        tongCocSauKhiCapNhat = _tongCocDaThu - soTienCocCu + soTien;
                    }

                    // Kiểm tra tổng cọc + thanh toán không được vượt quá tổng dự kiến
                    decimal tongCocVaThanhToan = tongCocSauKhiCapNhat + tongThanhToan;
                    if (tongCocVaThanhToan > _tongTienDatSanh)
                    {
                        decimal soTienConLaiToiDa = _tongTienDatSanh - (_tongCocDaThu - soTienCocCu) - tongThanhToan;
                        if (soTienConLaiToiDa < 0)
                            soTienConLaiToiDa = 0;
                        
                        MessageBox.Show(
                            $"Tổng cọc và thanh toán không được vượt quá tổng dự kiến!\n" +
                            $"Tổng dự kiến: {FormatCurrency(_tongTienDatSanh)}\n" +
                            $"Tổng cọc hiện tại: {FormatCurrency(_tongCocDaThu - soTienCocCu)}\n" +
                            $"Tổng thanh toán: {FormatCurrency(tongThanhToan)}\n" +
                            $"Số tiền cọc tối đa có thể thêm: {FormatCurrency(soTienConLaiToiDa)}",
                            "Thông báo", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Warning
                        );
                        txtSoTien.Focus();
                        txtSoTien.SelectAll();
                        return;
                    }

                    if (tongCocSauKhiCapNhat < soTienCocToiThieu)
                    {
                        decimal soTienCanThem = soTienCocToiThieu - (_tongCocDaThu - soTienCocCu);
                        string message = !_isEditMode
                            ? $"Số tiền cọc tối thiểu phải là {FormatCurrency(soTienCocToiThieu)} (20% tổng tiền).\n" +
                              $"Tổng cọc hiện tại: {FormatCurrency(_tongCocDaThu)}\n" +
                              $"Cần thêm ít nhất: {FormatCurrency(soTienCanThem)}"
                            : $"Tổng cọc sau khi cập nhật phải đạt tối thiểu {FormatCurrency(soTienCocToiThieu)} (20% tổng tiền).\n" +
                              $"Tổng cọc sau khi cập nhật: {FormatCurrency(tongCocSauKhiCapNhat)}\n" +
                              $"Cần cập nhật để đạt tối thiểu: {FormatCurrency(soTienCanThem)}";

                        MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoTien.Focus();
                        return;
                    }

                    // Kiểm tra số tiền cọc không vượt quá số tiền còn lại (sau khi trừ tổng thanh toán)
                    decimal soTienConLai;
                    if (!_isEditMode)
                    {
                        soTienConLai = _tongTienDatSanh - _tongCocDaThu - tongThanhToan;
                    }
                    else
                    {
                        soTienConLai = _tongTienDatSanh - (_tongCocDaThu - soTienCocCu) - tongThanhToan;
                    }

                    if (soTienConLai < 0)
                        soTienConLai = 0;

                    if (soTien > soTienConLai)
                    {
                        MessageBox.Show(
                            $"Số tiền cọc không được vượt quá số tiền còn lại!\n" +
                            $"Số tiền còn lại (sau khi trừ tổng thanh toán): {FormatCurrency(soTienConLai)}\n" +
                            $"Số tiền đã nhập: {FormatCurrency(soTien)}",
                            "Thông báo", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Warning
                        );
                        txtSoTien.Focus();
                        txtSoTien.SelectAll();
                        return;
                    }
                }

                string hinhThuc = cbHinhThuc.SelectedItem.ToString() ?? "";
                DateTime ngayNop = dtpNgayNop.Value.Date;
                string ghiChu = txtGhiChu.Text.Trim();

                if (_isEditMode)
                {
                    if (_isThanhToan && _ttId.HasValue)
                    {
                        // Cập nhật thanh toán
                        bool success = _datSanhBLL.CapNhatThanhToan(_ttId.Value, soTien, ngayNop, hinhThuc,
                            string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu, out string errorMessage);

                        if (success)
                        {
                            MessageBox.Show("Cập nhật phiếu thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi khi cập nhật phiếu thanh toán: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (!_isThanhToan && _cocId.HasValue)
                    {
                        // Cập nhật cọc
                        bool success = _datSanhBLL.CapNhatCoc(_cocId.Value, soTien, ngayNop, hinhThuc,
                            string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu, out string errorMessage);

                        if (success)
                        {
                            CapNhatTrangThaiTheoTienCoc();
                            
                            MessageBox.Show("Cập nhật đợt cọc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi khi cập nhật đợt cọc: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    // Lưu cọc mới (chỉ hỗ trợ thêm cọc mới, không hỗ trợ thêm thanh toán mới ở form này)
                    int cocId = _datSanhBLL.LuuTienCoc(_hopDongId.Value, soTien, ngayNop, hinhThuc, 
                        string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu, out string errorMessage);

                    if (cocId > 0)
                    {
                        CapNhatTrangThaiTheoTienCoc();
                        
                        MessageBox.Show("Thêm đợt cọc mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi lưu đợt cọc: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xác nhận: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatCurrency(decimal amount)
        {
            return amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " ₫";
        }

        private void CapNhatTrangThaiTheoTienCoc()
        {
            try
            {
                if (!_hopDongId.HasValue || _hopDongId.Value <= 0)
                    return;

                DataRow datSanhInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                string trangThaiHienTai = datSanhInfo["trang_thai"]?.ToString() ?? "";

                if (trangThaiHienTai.ToUpper() == "ĐÃ HỦY" || trangThaiHienTai.ToUpper() == "HOÀN TẤT")
                {
                    return;
                }

                DataTable dtCoc = _datSanhBLL.LayDanhSachCoc(_hopDongId.Value);
                bool coCoc = dtCoc != null && dtCoc.Rows.Count > 0;

                string trangThaiMoi;
                if (coCoc)
                {
                    trangThaiMoi = "ĐÃ CỌC";
                }
                else
                {
                    // Nếu không có cọc: chuyển về "CHỜ XÁC NHẬN"
                    trangThaiMoi = "CHỜ XÁC NHẬN";
                }

                if (trangThaiHienTai.ToUpper() != trangThaiMoi.ToUpper())
                {
                    bool success = _datSanhBLL.CapNhatTrangThaiDatSanh(_datSanhId, trangThaiMoi, out string errorMessage);
                    if (success)
                    {
                        // Nếu chuyển sang "ĐÃ CỌC", tạo hóa đơn
                        if (trangThaiMoi.ToUpper() == "ĐÃ CỌC")
                        {
                            int hoaDonId = _datSanhBLL.TaoHoaDonKhiDaCoc(_datSanhId, out string errorHoaDon);
                            if (!string.IsNullOrEmpty(errorHoaDon))
                            {
                                MessageBox.Show($"Lỗi tạo hóa đơn: {errorHoaDon}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}

