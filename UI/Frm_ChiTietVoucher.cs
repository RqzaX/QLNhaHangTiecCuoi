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
    public partial class Frm_ChiTietVoucher : Form
    {
        private VoucherBLL _bll;
        private ChuongTrinhKMBLL _kmBll;
        private int _voucherId;
        private Frm_Voucher _parentForm;
        private bool _isEditMode = false;
        private int _daDung = 0;
        private decimal _originalGiaTri = 0;
        private decimal _originalDonToiThieu = 0;
        private string _currentHinhThuc = "";

        public Frm_ChiTietVoucher(int voucherId, Frm_Voucher parentForm = null)
        {
            InitializeComponent();
            _bll = new VoucherBLL();
            _kmBll = new ChuongTrinhKMBLL();
            _voucherId = voucherId;
            _parentForm = parentForm;
            InitializeEvents();
            SetEditMode(false);
            LoadChuongTrinhKM();
            LoadData();
        }

        private bool _isUpdating = false;

        private void InitializeEvents()
        {
            btnDong.Click += BtnDong_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            cbbCTApDung.SelectedIndexChanged += CbbCTApDung_SelectedIndexChanged;
        }

        private string TinhTrangThaiVoucher(DateTime tgBatDau, DateTime tgKetThuc)
        {
            DateTime now = DateTime.Now.Date;

            if (tgKetThuc < now)
            {
                return "Đã hết hạn";
            }

            else if (tgBatDau <= now && tgKetThuc >= now)
            {
                return "Đang áp dụng";
            }

            else
            {
                return "Đã hết hạn";
            }
        }

        private void CbbCTApDung_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbCTApDung.SelectedItem != null)
                {
                    DataRowView selectedKm = (DataRowView)cbbCTApDung.SelectedItem;
                    DataRow kmRow = _kmBll.GetById(Convert.ToInt32(selectedKm["ID"]));
                    if (kmRow != null)
                    {
                        string hinhThuc = kmRow["hinh_thuc"]?.ToString() ?? "";
                        _currentHinhThuc = hinhThuc;

                        DateTime tgBatDau = kmRow["tg_bat_dau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(kmRow["tg_bat_dau"]).Date;
                        DateTime tgKetThuc = kmRow["tg_ket_thuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(kmRow["tg_ket_thuc"]).Date;

                        // Tính trạng thái voucher dựa trên trạng thái CTKM
                        string trangThai = TinhTrangThaiVoucher(tgBatDau, tgKetThuc);

                        cbbTrangThai.Items.Clear();
                        cbbTrangThai.Items.Add(trangThai);
                        cbbTrangThai.SelectedIndex = 0;

                        // Không tự động cập nhật đơn tối thiểu khi đổi CTKM
                        // Người dùng có thể nhập/sửa tự do, không phụ thuộc vào giá trị
                    }
                }
            }
            catch { }
        }

        private void LoadChuongTrinhKM()
        {
            try
            {
                DataTable dt = _kmBll.LoadData();
                cbbCTApDung.Items.Clear();
                cbbCTApDung.DisplayMember = "TenCT";
                cbbCTApDung.ValueMember = "ID";
                cbbCTApDung.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách chương trình khuyến mãi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                DataRow row = _bll.GetById(_voucherId);
                if (row == null)
                {
                    MessageBox.Show("Không tìm thấy voucher!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtMa.Text = row["Code"]?.ToString() ?? "";

                int kmId = row["KmId"] == DBNull.Value ? 0 : Convert.ToInt32(row["KmId"]);
                if (kmId > 0)
                {
                    for (int i = 0; i < cbbCTApDung.Items.Count; i++)
                    {
                        DataRowView drv = (DataRowView)cbbCTApDung.Items[i];
                        if (Convert.ToInt32(drv["ID"]) == kmId)
                        {
                            cbbCTApDung.SelectedIndex = i;
                            break;
                        }
                    }
                }

                decimal giaTri = row["GiaTri"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaTri"]);
                _originalGiaTri = giaTri;
                string hinhThuc = row["HinhThuc"]?.ToString() ?? "";
                _currentHinhThuc = hinhThuc;
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

                // Lấy giá trị đơn tối thiểu từ parent form (nếu có), nếu không thì tính mặc định
                decimal donToiThieu = 0;
                if (_parentForm != null)
                {
                    donToiThieu = _parentForm.GetDonToiThieu(_voucherId);
                }

                // Nếu không có trong parent form, tính giá trị mặc định
                if (donToiThieu == 0)
                {
                    donToiThieu = giaTri * 10;
                    if (donToiThieu < 1000000) donToiThieu = 1000000;
                }

                _originalDonToiThieu = donToiThieu;
                txtDonToiThieu.Text = donToiThieu.ToString("#,##0").Replace(",", ".");

                int soLan = row["SoLan"] == DBNull.Value ? 0 : Convert.ToInt32(row["SoLan"]);
                _daDung = row["DaDung"] == DBNull.Value ? 0 : Convert.ToInt32(row["DaDung"]);
                txtDaDung.Text = _daDung.ToString();
                txtLuotDung.Text = soLan.ToString();

                DateTime tgBatDau = row["TgBatDau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgBatDau"]).Date;
                DateTime tgKetThuc = row["TgKetThuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row["TgKetThuc"]).Date;

                // Tính trạng thái voucher dựa trên trạng thái CTKM
                string trangThai = TinhTrangThaiVoucher(tgBatDau, tgKetThuc);

                cbbTrangThai.Items.Clear();
                cbbTrangThai.Items.Add(trangThai);
                cbbTrangThai.SelectedIndex = 0;

                SetEditMode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEditMode(bool isEdit)
        {
            _isEditMode = isEdit;

            txtMa.ReadOnly = !isEdit;
            cbbCTApDung.Enabled = isEdit;
            txtLuotDung.ReadOnly = !isEdit;
            txtDaDung.ReadOnly = !isEdit;
            txtGiaTri.ReadOnly = !isEdit;

            // Đơn tối thiểu luôn cho phép nhập/sửa (không phụ thuộc vào giá trị)
            txtDonToiThieu.Enabled = true;
            txtDonToiThieu.ReadOnly = !isEdit;

            cbbTrangThai.Enabled = false;

            if (isEdit)
            {
                btnSua.Text = "Lưu";
                btnSua.FillColor = Color.FromArgb(34, 197, 94);
            }
            else
            {
                btnSua.Text = "Sửa";
                btnSua.FillColor = Color.FromArgb(59, 130, 246);
            }
        }

        private void BtnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (!_isEditMode)
            {
                SetEditMode(true);
                txtDonToiThieu.Refresh();
                txtDonToiThieu.Invalidate();
                Application.DoEvents();
            }
            else
            {
                SaveData();
            }
        }

        private void SaveData()
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

                if (cbbCTApDung.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn chương trình khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbCTApDung.Focus();
                    return;
                }

                DataRowView selectedKm = (DataRowView)cbbCTApDung.SelectedItem;
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

                string daDungText = txtDaDung.Text.Trim();
                if (string.IsNullOrWhiteSpace(daDungText))
                {
                    MessageBox.Show("Vui lòng nhập số lượt đã dùng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDaDung.Focus();
                    return;
                }

                if (!int.TryParse(daDungText, out int daDung))
                {
                    MessageBox.Show("Số lượt đã dùng phải là số nguyên!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDaDung.Focus();
                    return;
                }

                if (daDung < 0)
                {
                    MessageBox.Show("Số lượt đã dùng không được nhỏ hơn 0!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDaDung.Focus();
                    return;
                }

                if (daDung > soLan)
                {
                    MessageBox.Show("Số lượt đã dùng không được lớn hơn số lượt dùng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDaDung.Focus();
                    return;
                }

                DataRow kmRow = _kmBll.GetById(kmId);
                if (kmRow == null)
                {
                    MessageBox.Show("Không tìm thấy chương trình khuyến mãi!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string hinhThuc = kmRow["hinh_thuc"]?.ToString() ?? "";
                string giaTriText = txtGiaTri.Text.Trim().Replace(".", "").Replace(",", "").Replace("đ", "").Replace("%", "").Replace(" ", "");

                if (string.IsNullOrWhiteSpace(giaTriText))
                {
                    MessageBox.Show("Vui lòng nhập giá trị!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaTri.Focus();
                    return;
                }

                if (!decimal.TryParse(giaTriText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal giaTri))
                {
                    MessageBox.Show("Giá trị không hợp lệ! Vui lòng nhập số.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaTri.Focus();
                    return;
                }

                if (giaTri < 0)
                {
                    MessageBox.Show("Giá trị không được nhỏ hơn 0!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaTri.Focus();
                    return;
                }

                if (hinhThuc == "PERCENT" && giaTri > 50)
                {
                    MessageBox.Show("Giá trị giảm theo % không được vượt quá 50%!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaTri.Focus();
                    return;
                }
                else if (hinhThuc == "AMOUNT")
                {
                    if (giaTri < 100000)
                    {
                        MessageBox.Show("Giá trị giảm theo số tiền không được nhỏ hơn 100.000 đ!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGiaTri.Focus();
                        return;
                    }
                    if (giaTri > 10000000)
                    {
                        MessageBox.Show("Giá trị giảm theo số tiền không được vượt quá 10.000.000 đ!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtGiaTri.Focus();
                        return;
                    }
                }

                // Lưu giá trị đơn tối thiểu (không lưu vào database, chỉ lưu vào Dictionary trong parent form)
                string donToiThieuText = txtDonToiThieu.Text.Trim().Replace(".", "").Replace(",", "").Replace("đ", "").Replace(" ", "");
                if (string.IsNullOrWhiteSpace(donToiThieuText))
                {
                    MessageBox.Show("Vui lòng nhập đơn tối thiểu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonToiThieu.Focus();
                    return;
                }

                if (!decimal.TryParse(donToiThieuText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal donToiThieu))
                {
                    MessageBox.Show("Đơn tối thiểu không hợp lệ! Vui lòng nhập số.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonToiThieu.Focus();
                    return;
                }

                if (donToiThieu < 0)
                {
                    MessageBox.Show("Đơn tối thiểu không được nhỏ hơn 0!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonToiThieu.Focus();
                    return;
                }

                // Cập nhật giá trị đơn tối thiểu vào parent form (không lưu vào database)
                if (_parentForm != null)
                {
                    _parentForm.UpdateDonToiThieu(_voucherId, donToiThieu);
                }

                bool resultVoucher = _bll.Update(_voucherId, kmId, txtMa.Text.Trim(), soLan, null, daDung);

                bool resultKM = _kmBll.Update(
                    kmId,
                    kmRow["ma_km"]?.ToString() ?? "",
                    kmRow["ten"]?.ToString() ?? "",
                    hinhThuc,
                    giaTri,
                    kmRow["tg_bat_dau"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(kmRow["tg_bat_dau"]),
                    kmRow["tg_ket_thuc"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(kmRow["tg_ket_thuc"]),
                    kmRow["ap_dung_loai"]?.ToString() ?? "ALL"
                );

                if (resultVoucher && resultKM)
                {
                    MessageBox.Show("Cập nhật voucher thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetEditMode(false);
                    LoadData();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật voucher. Vui lòng thử lại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa voucher này?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool success = _bll.Delete(_voucherId);
                    if (success)
                    {
                        MessageBox.Show("Xóa voucher thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa voucher. Vui lòng thử lại!", "Lỗi",
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
    }
}
