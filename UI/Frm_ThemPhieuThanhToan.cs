using BLL;
using QLNhaHangTiecCuoi.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_ThemPhieuThanhToan : Form
    {
        private int _datSanhId;
        private int? _hopDongId;
        private int? _ttId;
        private bool _isEditMode = false;
        private DatSanhBLL _datSanhBLL;
        private decimal _soTienConThieu = 0;
        private bool _isFormatting = false;
        private decimal _soTienCuCuaThanhToan = 0;

        public Frm_ThemPhieuThanhToan(int datSanhId)
        {
            InitializeComponent();
            _datSanhId = datSanhId;
            _datSanhBLL = new DatSanhBLL();
            _isEditMode = false;

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;

            LoadThongTin();
            LoadHinhThuc();
        }

        public Frm_ThemPhieuThanhToan(int datSanhId, int ttId)
        {
            InitializeComponent();
            _datSanhId = datSanhId;
            _ttId = ttId;
            _datSanhBLL = new DatSanhBLL();
            _isEditMode = true;

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;

            LoadThongTin();
            LoadHinhThuc();
            LoadThongTinThanhToan();
            
            TinhSoTienConThieu();
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

                if (!_isEditMode)
                {
                    if (dtpNgayThanhToan != null)
                    {
                        dtpNgayThanhToan.Value = DateTime.Now;
                    }
                    TinhSoTienConThieu();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void TinhSoTienConThieu()
        {
            try
            {
                if (!_hopDongId.HasValue || _hopDongId.Value <= 0)
                {
                    if (lbSoTien != null)
                        lbSoTien.Text = "Số tiền còn thiếu: 0 đ";
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

                // Tính tổng đã thanh toán
                decimal tongThanhToan = 0;
                DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
                if (dtThanhToan != null)
                {
                    foreach (DataRow row in dtThanhToan.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                        {
                            if (_isEditMode && _ttId.HasValue)
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

                decimal tongConLai = tongDuKien - tongCoc - tongThanhToan;
                if (tongConLai < 0)
                    tongConLai = 0;

                if (_isEditMode && _soTienCuCuaThanhToan > 0)
                {
                    _soTienConThieu = tongConLai + _soTienCuCuaThanhToan;
                }
                else
                {
                    _soTienConThieu = tongConLai;
                }

                if (lbSoTien != null)
                {
                    lbSoTien.Text = $"Số tiền còn thiếu: {FormatCurrency(_soTienConThieu)}";
                }
            }
            catch (Exception ex)
            {
                _soTienConThieu = 0;
                if (lbSoTien != null)
                    lbSoTien.Text = "Số tiền còn thiếu: 0 đ";
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

                    if (dtpNgayThanhToan != null)
                    {
                        dtpNgayThanhToan.Value = ttInfo["ngay_tt"] != DBNull.Value ? Convert.ToDateTime(ttInfo["ngay_tt"]) : DateTime.Now;
                    }
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

                    if (txtSoTien != null)
                    {
                        _isFormatting = true;
                        txtSoTien.Text = soTien.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                        _isFormatting = false;
                    }
                    if (txtNoiDung != null)
                    {
                        txtNoiDung.Text = noiDung;
                    }
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

                string noiDung = txtNoiDung.Text.Trim();
                if (string.IsNullOrWhiteSpace(noiDung))
                {
                    noiDung = $"Thanh toán đặt sảnh DS{_datSanhId:D6}";
                }

                var formQR = new Frm_QRThanhToan(soTien, noiDung)
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
        // format số tiền với dấu phẩy, không validate số tối đa khi đang nhập
        private void TxtSoTien_TextChanged(object? sender, EventArgs e)
        {
            if (_isFormatting)
                return;

            if (string.IsNullOrWhiteSpace(txtSoTien.Text))
            {
                return;
            }

            string text = txtSoTien.Text.Replace(",", "").Replace(".", "");
            if (decimal.TryParse(text, out decimal soTien))
            {
                _isFormatting = true;
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
                _isFormatting = false;
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
                TinhSoTienConThieu();

                if (soTien > _soTienConThieu && _soTienConThieu > 0)
                {
                    _isFormatting = true;
                    txtSoTien.Text = _soTienConThieu.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                    _isFormatting = false;
                    MessageBox.Show(
                        $"Số tiền đã được điều chỉnh về số tiền tối đa: {FormatCurrency(_soTienConThieu)}",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
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

                TinhSoTienConThieu();

                // Kiểm tra số tiền không được vượt quá số tiền còn thiếu
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
                    _isFormatting = true;
                    txtSoTien.Text = _soTienConThieu.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                    _isFormatting = false;
                    txtSoTien.Focus();
                    txtSoTien.SelectAll();
                    return;
                }

                string hinhThuc = cbHinhThuc.SelectedItem.ToString() ?? "";
                DateTime ngayTT = dtpNgayThanhToan != null ? dtpNgayThanhToan.Value.Date : DateTime.Now.Date;
                string noiDung = txtNoiDung != null ? txtNoiDung.Text.Trim() : "";

                if (_isEditMode && _ttId.HasValue)
                {
                    // Cập nhật thanh toán
                    bool success = _datSanhBLL.CapNhatThanhToan(_ttId.Value, soTien, ngayTT, hinhThuc,
                        string.IsNullOrWhiteSpace(noiDung) ? null : noiDung, out string errorMessage);

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
                else
                {
                    // Lưu thanh toán mới
                    int ttId = _datSanhBLL.LuuThanhToan(_hopDongId.Value, soTien, ngayTT, hinhThuc, 
                        string.IsNullOrWhiteSpace(noiDung) ? null : noiDung, out string errorMessage);

                    if (ttId > 0)
                    {
                        MessageBox.Show("Thêm phiếu thanh toán mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi khi lưu phiếu thanh toán: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            return amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
        }
    }
}

