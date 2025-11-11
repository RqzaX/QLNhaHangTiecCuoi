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
            if (_isEditMode)
            {
                TinhSoTienConThieu_Edit();
            }
            else
            {
                TinhSoTienConThieu_ThemMoi();
            }
        }

        // Luồng thêm thanh toán mới
        private void TinhSoTienConThieu_ThemMoi()
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
                
                // Tính tổng cọc đã thu
                decimal tongCoc = 0;
                bool coCoc = false;
                
                try
                {
                    tongCoc = _datSanhBLL.LayTongCocDaThu(_datSanhId);
                    if (tongCoc > 0)
                    {
                        coCoc = true;
                    }
                }
                catch
                {
                    tongCoc = 0;
                }
                
                if (tongCoc == 0)
                {
                    try
                    {
                        DataTable dtCoc = _datSanhBLL.LayDanhSachCoc(_hopDongId.Value);
                        if (dtCoc != null && dtCoc.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtCoc.Rows)
                            {
                                if (row["so_tien"] != DBNull.Value)
                                {
                                    decimal soTienCoc = Convert.ToDecimal(row["so_tien"]);
                                    tongCoc += soTienCoc;
                                }
                            }
                            if (tongCoc > 0)
                            {
                                coCoc = true;
                            }
                        }
                    }
                    catch
                    {
                        tongCoc = 0;
                        coCoc = false;
                    }
                }

                // Nếu chưa có cọc nào, hiển thị thông báo về cọc tối thiểu
                if (!coCoc)
                {
                    _soTienConThieu = 0;
                    if (lbSoTien != null)
                    {
                        lbSoTien.Text = "Số tiền cọc tối thiểu (20%): 0 đ";
                    }
                    return;
                }

                // Tính tổng đã thanh toán (không loại trừ gì cả vì đang thêm mới)
                decimal tongThanhToan = 0;
                DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
                if (dtThanhToan != null && dtThanhToan.Rows.Count > 0)
                {
                    foreach (DataRow row in dtThanhToan.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                        {
                            decimal soTienTT = Convert.ToDecimal(row["so_tien"]);
                            tongThanhToan += soTienTT;
                        }
                    }
                }

                decimal tongConLai = tongDuKien - tongCoc - tongThanhToan;
                if (tongConLai < 0)
                    tongConLai = 0;

                _soTienConThieu = tongConLai;

                if (_soTienConThieu > 0)
                {
                    lbSoTien.Text = $"Số tiền còn thiếu: {FormatCurrency(_soTienConThieu)}";
                }
                else
                {
                    lbSoTien.Text = $"Số tiền còn thiếu: 0 đ";
                }
            }
            catch (Exception ex)
            {
                _soTienConThieu = 0;
                if (lbSoTien != null)
                    lbSoTien.Text = "Số tiền còn thiếu: 0 đ";
            }
        }

        // Luồng edit thanh toán
        private void TinhSoTienConThieu_Edit()
        {
            try
            {
                if (!_hopDongId.HasValue || _hopDongId.Value <= 0)
                {
                    if (lbSoTien != null)
                        lbSoTien.Text = "Số tiền còn thiếu: 0 đ";
                    return;
                }

                if (!_ttId.HasValue || _ttId.Value <= 0)
                {
                    if (lbSoTien != null)
                        lbSoTien.Text = "Số tiền còn thiếu: 0 đ";
                    return;
                }

                // Đảm bảo _soTienCuCuaThanhToan đã được load
                if (_soTienCuCuaThanhToan == 0)
                {
                    DataRow? ttInfo = _datSanhBLL.LayThongTinThanhToan(_ttId.Value);
                    if (ttInfo != null && ttInfo["so_tien"] != DBNull.Value)
                    {
                        _soTienCuCuaThanhToan = Convert.ToDecimal(ttInfo["so_tien"]);
                    }
                }

                decimal tongDuKien = _datSanhBLL.LayTongDuKien(_hopDongId.Value);
                
                // Tính tổng cọc đã thu
                decimal tongCoc = 0;
                bool coCoc = false;
                
                try
                {
                    tongCoc = _datSanhBLL.LayTongCocDaThu(_datSanhId);
                    if (tongCoc > 0)
                    {
                        coCoc = true;
                    }
                }
                catch
                {
                    tongCoc = 0;
                }
                
                if (tongCoc == 0)
                {
                    try
                    {
                        DataTable dtCoc = _datSanhBLL.LayDanhSachCoc(_hopDongId.Value);
                        if (dtCoc != null && dtCoc.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtCoc.Rows)
                            {
                                if (row["so_tien"] != DBNull.Value)
                                {
                                    decimal soTienCoc = Convert.ToDecimal(row["so_tien"]);
                                    tongCoc += soTienCoc;
                                }
                            }
                            if (tongCoc > 0)
                            {
                                coCoc = true;
                            }
                        }
                    }
                    catch
                    {
                        tongCoc = 0;
                        coCoc = false;
                    }
                }

                // Nếu chưa có cọc nào, hiển thị thông báo về cọc tối thiểu
                if (!coCoc)
                {
                    _soTienConThieu = 0;
                    if (lbSoTien != null)
                    {
                        lbSoTien.Text = "Số tiền cọc tối thiểu (20%): 0 đ";
                    }
                    return;
                }

                decimal tongThanhToan = 0;
                DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
                if (dtThanhToan != null && dtThanhToan.Rows.Count > 0)
                {
                    foreach (DataRow row in dtThanhToan.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                        {
                            if (row["tt_id"] != DBNull.Value && Convert.ToInt32(row["tt_id"]) == _ttId.Value)
                            {
                                continue;
                            }
                            decimal soTienTT = Convert.ToDecimal(row["so_tien"]);
                            tongThanhToan += soTienTT;
                        }
                    }
                }

                decimal tongConLai = tongDuKien - tongCoc - tongThanhToan;
                if (tongConLai < 0)
                    tongConLai = 0;

                _soTienConThieu = tongConLai;

                if (_soTienConThieu > 0)
                {
                    lbSoTien.Text = $"Số tiền còn thiếu: {FormatCurrency(_soTienConThieu)}";
                }
                else
                {
                    lbSoTien.Text = $"Số tiền còn thiếu: 0 đ";
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
                if (!_hopDongId.HasValue || _hopDongId.Value <= 0)
                {
                    _hopDongId = _datSanhBLL.LayHopDongId(_datSanhId);
                }

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
                if (lbSoTien != null && _soTienConThieu > 0)
                {
                    lbSoTien.Text = $"Số tiền còn thiếu: {FormatCurrency(_soTienConThieu)}";
                }
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

                // Cập nhật số tiền còn thiếu theo luồng
                if (lbSoTien != null && _hopDongId.HasValue && _hopDongId.Value > 0)
                {
                    if (_isEditMode)
                    {
                        CapNhatSoTienConThieu_Edit(soTien);
                    }
                    else
                    {
                        CapNhatSoTienConThieu_ThemMoi(soTien);
                    }
                }
            }
        }

        // Cập nhật số tiền còn thiếu khi thêm mới
        private void CapNhatSoTienConThieu_ThemMoi(decimal soTien)
        {
            decimal soTienConThieuMoi = _soTienConThieu - soTien;
            if (soTienConThieuMoi < 0)
                soTienConThieuMoi = 0;

            lbSoTien.Text = $"Số tiền còn thiếu: {FormatCurrency(soTienConThieuMoi)}";
        }

        // Cập nhật số tiền còn thiếu khi edit
        private void CapNhatSoTienConThieu_Edit(decimal soTien)
        {
            decimal soTienConThieuMoi = _soTienConThieu - soTien;
            if (soTienConThieuMoi < 0)
                soTienConThieuMoi = 0;

            lbSoTien.Text = $"Số tiền còn thiếu: {FormatCurrency(soTienConThieuMoi)}";
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
                if (!_hopDongId.HasValue || _hopDongId.Value <= 0)
                    return;

                if (_isEditMode)
                {
                    ValidateSoTien_Edit(soTien);
                }
                else
                {
                    ValidateSoTien_ThemMoi(soTien);
                }
            }
        }

        // Validate số tiền khi thêm mới
        private void ValidateSoTien_ThemMoi(decimal soTien)
        {
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

            decimal tongThanhToanHienTai = 0;
            DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
            if (dtThanhToan != null)
            {
                foreach (DataRow row in dtThanhToan.Rows)
                {
                    if (row["so_tien"] != DBNull.Value)
                    {
                        tongThanhToanHienTai += Convert.ToDecimal(row["so_tien"]);
                    }
                }
            }

            decimal soTienConThieuChinhXac = tongDuKien - tongCoc - tongThanhToanHienTai;
            if (soTienConThieuChinhXac < 0)
                soTienConThieuChinhXac = 0;

            if (soTien > soTienConThieuChinhXac && soTienConThieuChinhXac > 0)
            {
                _isFormatting = true;
                txtSoTien.Text = soTienConThieuChinhXac.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                _isFormatting = false;
                MessageBox.Show(
                    $"Số tiền đã được điều chỉnh về số tiền tối đa: {FormatCurrency(soTienConThieuChinhXac)}\n" +
                    $"(Tổng cọc + thanh toán không được vượt quá tổng dự kiến)",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else if (soTienConThieuChinhXac == 0 && soTien > 0)
            {
                _isFormatting = true;
                txtSoTien.Text = "0";
                _isFormatting = false;
                MessageBox.Show(
                    $"Không thể nhập thêm số tiền vì tổng cọc và thanh toán đã đạt tổng dự kiến!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        // Validate số tiền khi edit
        private void ValidateSoTien_Edit(decimal soTien)
        {
            if (!_ttId.HasValue || _ttId.Value <= 0)
                return;

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

            // Tính tổng thanh toán (LOẠI TRỪ khoản đang edit)
            decimal tongThanhToanHienTai = 0;
            DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
            if (dtThanhToan != null)
            {
                foreach (DataRow row in dtThanhToan.Rows)
                {
                    if (row["so_tien"] != DBNull.Value)
                    {
                        // Loại trừ khoản đang edit
                        if (row["tt_id"] != DBNull.Value && Convert.ToInt32(row["tt_id"]) == _ttId.Value)
                        {
                            continue;
                        }
                        tongThanhToanHienTai += Convert.ToDecimal(row["so_tien"]);
                    }
                }
            }

            decimal soTienConThieuChinhXac = tongDuKien - tongCoc - tongThanhToanHienTai;
            if (soTienConThieuChinhXac < 0)
                soTienConThieuChinhXac = 0;

            if (soTien > soTienConThieuChinhXac && soTienConThieuChinhXac > 0)
            {
                _isFormatting = true;
                txtSoTien.Text = soTienConThieuChinhXac.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                _isFormatting = false;
                MessageBox.Show(
                    $"Số tiền đã được điều chỉnh về số tiền tối đa: {FormatCurrency(soTienConThieuChinhXac)}\n" +
                    $"(Tổng cọc + thanh toán không được vượt quá tổng dự kiến)",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else if (soTienConThieuChinhXac == 0 && soTien > 0)
            {
                _isFormatting = true;
                txtSoTien.Text = "0";
                _isFormatting = false;
                MessageBox.Show(
                    $"Không thể nhập thêm số tiền vì tổng cọc và thanh toán đã đạt tổng dự kiến!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
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

                // Chuyển hướng theo luồng
                if (_isEditMode)
                {
                    XacNhanThanhToan_Edit(soTien);
                }
                else
                {
                    XacNhanThanhToan_ThemMoi(soTien);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xác nhận: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Luồng xác nhận thêm thanh toán mới
        private void XacNhanThanhToan_ThemMoi(decimal soTien)
        {
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

            // Tính tổng thanh toán hiện tại (không loại trừ gì)
            decimal tongThanhToanHienTai = 0;
            DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
            if (dtThanhToan != null)
            {
                foreach (DataRow row in dtThanhToan.Rows)
                {
                    if (row["so_tien"] != DBNull.Value)
                    {
                        tongThanhToanHienTai += Convert.ToDecimal(row["so_tien"]);
                    }
                }
            }
            
            // Kiểm tra tổng cọc + thanh toán không vượt quá tổng dự kiến
            decimal tongThanhToanSauKhiThem = tongThanhToanHienTai + soTien;
            decimal tongCocVaThanhToan = tongCoc + tongThanhToanSauKhiThem;
            if (tongCocVaThanhToan > tongDuKien)
            {
                decimal soTienToiDa = tongDuKien - tongCoc - tongThanhToanHienTai;
                if (soTienToiDa < 0)
                    soTienToiDa = 0;
                
                MessageBox.Show(
                    $"Tổng cọc và thanh toán không được vượt quá tổng dự kiến!\n" +
                    $"Tổng dự kiến: {FormatCurrency(tongDuKien)}\n" +
                    $"Tổng cọc: {FormatCurrency(tongCoc)}\n" +
                    $"Tổng thanh toán hiện tại: {FormatCurrency(tongThanhToanHienTai)}\n" +
                    $"Số tiền thanh toán tối đa có thể nhập: {FormatCurrency(soTienToiDa)}",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                _isFormatting = true;
                if (soTienToiDa > 0)
                {
                    txtSoTien.Text = soTienToiDa.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                }
                else
                {
                    txtSoTien.Text = "0";
                }
                _isFormatting = false;
                txtSoTien.Focus();
                txtSoTien.SelectAll();
                return;
            }

            // Tính số tiền còn thiếu
            decimal soTienConThieuChinhXac = tongDuKien - tongCoc - tongThanhToanHienTai;
            if (soTienConThieuChinhXac < 0)
                soTienConThieuChinhXac = 0;

            // Kiểm tra số tiền không được vượt quá số tiền còn thiếu
            if (soTien > soTienConThieuChinhXac)
            {
                MessageBox.Show(
                    $"Số tiền thanh toán không được vượt quá số tiền còn thiếu!\n" +
                    $"Số tiền còn thiếu: {FormatCurrency(soTienConThieuChinhXac)}\n" +
                    $"Số tiền đã nhập: {FormatCurrency(soTien)}",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                _isFormatting = true;
                if (soTienConThieuChinhXac > 0)
                {
                    txtSoTien.Text = soTienConThieuChinhXac.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                }
                else
                {
                    txtSoTien.Text = "0";
                }
                _isFormatting = false;
                txtSoTien.Focus();
                txtSoTien.SelectAll();
                return;
            }

            // Lưu thanh toán mới
            string hinhThuc = cbHinhThuc.SelectedItem.ToString() ?? "";
            DateTime ngayTT = dtpNgayThanhToan != null ? dtpNgayThanhToan.Value.Date : DateTime.Now.Date;
            string noiDung = txtNoiDung != null ? txtNoiDung.Text.Trim() : "";

            int ttId = _datSanhBLL.LuuThanhToan(_hopDongId.Value, soTien, ngayTT, hinhThuc, 
                string.IsNullOrWhiteSpace(noiDung) ? null : noiDung, out string errorMessage);

            if (ttId > 0)
            {
                _datSanhBLL.KiemTraVaCapNhatTrangThaiTheoTienThanhToan(_datSanhId, out string errorTrangThai);
                MessageBox.Show("Thêm phiếu thanh toán mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Lỗi khi lưu phiếu thanh toán: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Luồng xác nhận edit thanh toán
        private void XacNhanThanhToan_Edit(decimal soTien)
        {
            if (!_ttId.HasValue || _ttId.Value <= 0)
            {
                MessageBox.Show("Không tìm thấy thông tin thanh toán cần cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Tính tổng thanh toán hiện tại (LOẠI TRỪ khoản đang edit)
            decimal tongThanhToanHienTai = 0;
            DataTable dtThanhToan = _datSanhBLL.LayDanhSachThanhToan(_hopDongId.Value);
            if (dtThanhToan != null)
            {
                foreach (DataRow row in dtThanhToan.Rows)
                {
                    if (row["so_tien"] != DBNull.Value)
                    {
                        // Loại trừ khoản đang edit
                        if (row["tt_id"] != DBNull.Value && Convert.ToInt32(row["tt_id"]) == _ttId.Value)
                        {
                            continue;
                        }
                        tongThanhToanHienTai += Convert.ToDecimal(row["so_tien"]);
                    }
                }
            }
            
            // Kiểm tra tổng cọc + thanh toán không vượt quá tổng dự kiến
            decimal tongThanhToanSauKhiCapNhat = tongThanhToanHienTai + soTien;
            decimal tongCocVaThanhToan = tongCoc + tongThanhToanSauKhiCapNhat;
            if (tongCocVaThanhToan > tongDuKien)
            {
                decimal soTienToiDa = tongDuKien - tongCoc - tongThanhToanHienTai;
                if (soTienToiDa < 0)
                    soTienToiDa = 0;
                
                MessageBox.Show(
                    $"Tổng cọc và thanh toán không được vượt quá tổng dự kiến!\n" +
                    $"Tổng dự kiến: {FormatCurrency(tongDuKien)}\n" +
                    $"Tổng cọc: {FormatCurrency(tongCoc)}\n" +
                    $"Tổng thanh toán hiện tại (không bao gồm khoản đang edit): {FormatCurrency(tongThanhToanHienTai)}\n" +
                    $"Số tiền thanh toán tối đa có thể nhập: {FormatCurrency(soTienToiDa)}",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                _isFormatting = true;
                if (soTienToiDa > 0)
                {
                    txtSoTien.Text = soTienToiDa.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                }
                else
                {
                    txtSoTien.Text = "0";
                }
                _isFormatting = false;
                txtSoTien.Focus();
                txtSoTien.SelectAll();
                return;
            }

            // Tính số tiền còn thiếu (không bao gồm số tiền cũ của khoản đang edit)
            decimal soTienConThieuChinhXac = tongDuKien - tongCoc - tongThanhToanHienTai;
            if (soTienConThieuChinhXac < 0)
                soTienConThieuChinhXac = 0;

            // Kiểm tra số tiền không được vượt quá số tiền còn thiếu
            if (soTien > soTienConThieuChinhXac)
            {
                MessageBox.Show(
                    $"Số tiền thanh toán không được vượt quá số tiền còn thiếu!\n" +
                    $"Số tiền còn thiếu: {FormatCurrency(soTienConThieuChinhXac)}\n" +
                    $"Số tiền đã nhập: {FormatCurrency(soTien)}",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                _isFormatting = true;
                if (soTienConThieuChinhXac > 0)
                {
                    txtSoTien.Text = soTienConThieuChinhXac.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                }
                else
                {
                    txtSoTien.Text = "0";
                }
                _isFormatting = false;
                txtSoTien.Focus();
                txtSoTien.SelectAll();
                return;
            }

            // Cập nhật thanh toán
            string hinhThuc = cbHinhThuc.SelectedItem.ToString() ?? "";
            DateTime ngayTT = dtpNgayThanhToan != null ? dtpNgayThanhToan.Value.Date : DateTime.Now.Date;
            string noiDung = txtNoiDung != null ? txtNoiDung.Text.Trim() : "";

            bool success = _datSanhBLL.CapNhatThanhToan(_ttId.Value, soTien, ngayTT, hinhThuc,
                string.IsNullOrWhiteSpace(noiDung) ? null : noiDung, out string errorMessage);

            if (success)
            {
                // Kiểm tra và cập nhật trạng thái nếu đã thanh toán hết
                _datSanhBLL.KiemTraVaCapNhatTrangThaiTheoTienThanhToan(_datSanhId, out string errorTrangThai);

                MessageBox.Show("Cập nhật phiếu thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Lỗi khi cập nhật phiếu thanh toán: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatCurrency(decimal amount)
        {
            return amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
        }
    }
}

