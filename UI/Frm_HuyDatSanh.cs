using BLL;
using QLNhaHangTiecCuoi.BLL;
using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_HuyDatSanh : Form
    {
        private int _datSanhId;
        private DatSanhBLL _datSanhBLL;
        private decimal _tongCocDaThu = 0;

        public Frm_HuyDatSanh(int datSanhId)
        {
            InitializeComponent();
            _datSanhId = datSanhId;
            _datSanhBLL = new DatSanhBLL();

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;

            dtpNgayHuy.Value = DateTime.Now;
            dtpNgayHuy.MinDate = DateTime.Now.AddDays(-365);
            dtpNgayHuy.MaxDate = DateTime.Now;

            LoadThongTin();
        }

        private void LoadThongTin()
        {
            try
            {
                _tongCocDaThu = _datSanhBLL.LayTongCocDaThu(_datSanhId);
                txtPhanTram.Text = "";
                txtSoTienHoanCoc.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuayLai_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        // Tính số tiền hoàn cọc dựa trên phần trăm
        private void TxtPhanTram_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                int selectionStart = txtPhanTram.SelectionStart;
                
                string currentText = txtPhanTram.Text;
                string phanTramText = currentText.Replace("%", "").Replace(" ", "").Trim();
                
                if (string.IsNullOrWhiteSpace(phanTramText))
                {
                    txtPhanTram.Text = "";
                    txtSoTienHoanCoc.Text = "0";
                    return;
                }

                if (decimal.TryParse(phanTramText, out decimal phanTram))
                {
                    if (phanTram > 100)
                        phanTram = 100;
                    if (phanTram < 0)
                        phanTram = 0;

                    string numberText = phanTram.ToString("0.##");
                    string formattedText = numberText + " %";
                    
                    if (currentText != formattedText)
                    {
                        int oldNumberLength = phanTramText.Length;
                        int newNumberLength = numberText.Length;
                        int newPosition;
                        
                        if (selectionStart <= oldNumberLength)
                        {
                            if (oldNumberLength == newNumberLength)
                            {
                                newPosition = selectionStart;
                            }
                            else
                            {
                                newPosition = newNumberLength;
                            }
                        }
                        else
                        {
                            newPosition = newNumberLength;
                        }
                        
                        newPosition = Math.Max(0, Math.Min(newPosition, newNumberLength));
                        
                        txtPhanTram.Text = formattedText;
                        txtPhanTram.SelectionStart = newPosition;
                        txtPhanTram.SelectionLength = 0;
                    }

                    // Tính số tiền hoàn cọc
                    decimal soTienHoanCoc = (_tongCocDaThu * phanTram) / 100;
                    txtSoTienHoanCoc.Text = FormatCurrency(soTienHoanCoc);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(phanTramText))
                    {
                        txtPhanTram.Text = "";
                    }
                    txtSoTienHoanCoc.Text = "0";
                }
            }
            catch
            {
                txtSoTienHoanCoc.Text = "0";
            }
        }

        // Chỉ cho phép nhập số
        private void TxtPhanTram_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private string FormatCurrency(decimal amount)
        {
            return amount.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " ₫";
        }

        private void BtnXacNhanHuy_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtLyDoHuy.Text))
                {
                    MessageBox.Show("Vui lòng nhập lý do hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLyDoHuy.Focus();
                    return;
                }

                string phanTramText = txtPhanTram.Text.Replace("%", "").Replace(" ", "").Trim();
                if (string.IsNullOrWhiteSpace(phanTramText) || !decimal.TryParse(phanTramText, out decimal phanTram))
                {
                    MessageBox.Show("Vui lòng nhập phần trăm hoàn cọc hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhanTram.Focus();
                    return;
                }

                if (phanTram < 0 || phanTram > 100)
                {
                    MessageBox.Show("Phần trăm hoàn cọc phải từ 0% đến 100%!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhanTram.Focus();
                    return;
                }

                decimal soTienHoanCoc = (_tongCocDaThu * phanTram) / 100;

                // Xác nhận hủy
                var confirmResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn hủy đặt sảnh?\n\nThao tác này không thể hoàn tác.",
                    "Xác nhận hủy",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes)
                    return;

                // BLL để hủy đặt sảnh
                bool result = _datSanhBLL.HuyDatSanh(_datSanhId, dtpNgayHuy.Value, txtLyDoHuy.Text.Trim(), soTienHoanCoc, out string errorMessage);

                if (result)
                {
                    MessageBox.Show("Hủy đặt sảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Lỗi khi hủy đặt sảnh: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xác nhận hủy: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

