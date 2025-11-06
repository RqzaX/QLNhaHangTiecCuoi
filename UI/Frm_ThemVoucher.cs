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

namespace UI
{
    public partial class Frm_ThemVoucher : Form
    {
        private VoucherBLL _bll;
        private ChuongTrinhKMBLL _kmBll;

        public Frm_ThemVoucher()
        {
            InitializeComponent();
            _bll = new VoucherBLL();
            _kmBll = new ChuongTrinhKMBLL();
            InitializeEvents();
            LoadChuongTrinhKM();
        }

        private void InitializeEvents()
        {
            cbbTenCTApDung.SelectedIndexChanged += CbbTenCTApDung_SelectedIndexChanged;
            btnLuu.Click += BtnLuu_Click;
            btnDong.Click += BtnDong_Click;
        }

        private void LoadChuongTrinhKM()
        {
            try
            {
                DataTable dt = _kmBll.LoadData();
                cbbTenCTApDung.Items.Clear();
                cbbTenCTApDung.DisplayMember = "TenCT";
                cbbTenCTApDung.ValueMember = "ID";
                cbbTenCTApDung.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load chương trình khuyến mãi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CbbTenCTApDung_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbTenCTApDung.SelectedItem != null)
                {
                    DataRowView selectedKm = (DataRowView)cbbTenCTApDung.SelectedItem;
                    DataRow kmRow = _kmBll.GetById(Convert.ToInt32(selectedKm["ID"]));
                    if (kmRow != null)
                    {
                        decimal giaTri = kmRow["gia_tri"] == DBNull.Value ? 0 : Convert.ToDecimal(kmRow["gia_tri"]);
                        string hinhThuc = kmRow["hinh_thuc"]?.ToString() ?? "";

                        if (hinhThuc == "PERCENT")
                        {
                            txtGiaTri.Text = giaTri.ToString("0");
                        }
                        else if (hinhThuc == "AMOUNT")
                        {
                            txtGiaTri.Text = giaTri.ToString("#,##0").Replace(",", ".");
                        }
                        else
                        {
                            txtGiaTri.Text = "0";
                        }

                        decimal donToiThieu = giaTri * 10;
                        if (donToiThieu < 1000000) donToiThieu = 1000000;
                        txtDonToiThieu.Text = donToiThieu.ToString("#,##0").Replace(",", ".");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load thông tin CTKM: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã voucher!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMa.Focus();
                    return;
                }

                if (cbbTenCTApDung.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn chương trình khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbTenCTApDung.Focus();
                    return;
                }

                DataRowView selectedKm = (DataRowView)cbbTenCTApDung.SelectedItem;
                int kmId = Convert.ToInt32(selectedKm["ID"]);

                string soLanText = txtLuotDung.Text.Trim();
                if (string.IsNullOrWhiteSpace(soLanText))
                {
                    MessageBox.Show("Vui lòng nhập số lượt dùng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLuotDung.Focus();
                    return;
                }

                if (!int.TryParse(soLanText, out int soLan))
                {
                    MessageBox.Show("Số lượt dùng phải là số nguyên!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLuotDung.Focus();
                    return;
                }

                if (soLan <= 0)
                {
                    MessageBox.Show("Số lượt dùng phải lớn hơn 0!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLuotDung.Focus();
                    return;
                }

                bool result = _bll.Add(kmId, txtMa.Text.Trim(), soLan, null);
                if (result)
                {
                    MessageBox.Show("Thêm voucher thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm voucher thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm voucher: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
