using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;

namespace UI
{
    public partial class Frm_ThongTinBan : Form
    {
        private readonly BanBLL _banBLL;
        private readonly int _banId;
        private bool _isEditMode = false;
        private bool _allowEdit = true;

        public Frm_ThongTinBan(int banId, string soBan, string trangThai, BanBLL banBLL, bool allowEdit = true)
        {
            InitializeComponent();
            _banId = banId;
            _banBLL = banBLL;
            _allowEdit = allowEdit;
            
            this.Text = $"Chi tiết bàn";
            lblTitle.Text = "Chi tiết bàn";
            lblSubtitle.Text = "Thông tin chi tiết về bàn";
            
            LoadData();
            SetEditMode(false);
            
            // Ẩn nút Sửa và Xóa nếu không cho phép chỉnh sửa
            if (!_allowEdit)
            {
                btnSua.Visible = false;
                btnXoa.Visible = false;
                // Điều chỉnh vị trí nút Đóng
                btnDong.Location = new Point((this.Width - btnDong.Width) / 2, btnDong.Location.Y);
            }
            else
            {
                // Hiển thị nút Xóa khi cho phép chỉnh sửa
                btnXoa.Visible = true;
            }
        }

        private void LoadData()
        {
            try
            {
                // Load thông tin bàn
                var dtBan = _banBLL.LayThongTinBan(_banId);
                if (dtBan != null && dtBan.Rows.Count > 0)
                {
                    var ban = dtBan.Rows[0];
                    
                    txtSoBan.Text = ban["so_ban"]?.ToString() ?? "";
                    txtSucChua.Text = ban["suc_chua"]?.ToString() ?? "0";
                    
                    // Load khu vực
                    LoadKhuVuc();
                    int? khuVucId = ban["khu_vuc_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(ban["khu_vuc_id"]);
                    if (khuVucId.HasValue)
                    {
                        for (int i = 0; i < cbbKhuVuc.Items.Count; i++)
                        {
                            DataRowView drv = (DataRowView)cbbKhuVuc.Items[i];
                            if (Convert.ToInt32(drv["khu_vuc_id"]) == khuVucId.Value)
                            {
                                cbbKhuVuc.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    
                    // Load trạng thái
                    string trangThai = ban["trang_thai"]?.ToString() ?? "TRỐNG";
                    cbbTrangThai.Items.Clear();
                    cbbTrangThai.Items.Add("Trống");
                    cbbTrangThai.Items.Add("Phục vụ");
                    cbbTrangThai.Items.Add("Đã đặt");
                    cbbTrangThai.Items.Add("Vệ sinh");
                    
                    switch (trangThai.ToUpper())
                    {
                        case "TRỐNG":
                            cbbTrangThai.SelectedIndex = 0;
                            break;
                        case "PHỤC VỤ":
                            cbbTrangThai.SelectedIndex = 1;
                            break;
                        case "ĐÃ ĐẶT":
                            cbbTrangThai.SelectedIndex = 2;
                            break;
                        case "VỆ SINH":
                            cbbTrangThai.SelectedIndex = 3;
                            break;
                        default:
                            cbbTrangThai.SelectedIndex = 0;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadKhuVuc()
        {
            try
            {
                var dtKhuVuc = _banBLL.LayDanhSachKhuVucTheoChiNhanh(UI.Common.Session.ChiNhanhId);
                cbbKhuVuc.DisplayMember = "ten_khu_vuc";
                cbbKhuVuc.ValueMember = "khu_vuc_id";
                cbbKhuVuc.DataSource = dtKhuVuc;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEditMode(bool isEdit)
        {
            _isEditMode = isEdit;
            
            txtSoBan.ReadOnly = !isEdit;
            txtSucChua.ReadOnly = !isEdit;
            cbbKhuVuc.Enabled = isEdit;
            cbbTrangThai.Enabled = isEdit;
            
            if (isEdit)
            {
                btnSua.Text = "Lưu";
                btnSua.BackColor = Color.FromArgb(34, 197, 94);
                btnSua.HoverBackColor = Color.FromArgb(22, 163, 74);
                btnSua.PressedBackColor = Color.FromArgb(21, 128, 61);
            }
            else
            {
                btnSua.Text = "Sửa";
                btnSua.BackColor = Color.FromArgb(59, 130, 246);
                btnSua.HoverBackColor = Color.FromArgb(37, 99, 235);
                btnSua.PressedBackColor = Color.FromArgb(29, 78, 216);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (!_allowEdit)
            {
                return; // Không cho phép sửa
            }

            if (!_isEditMode)
            {
                // Chuyển sang chế độ sửa
                SetEditMode(true);
            }
            else
            {
                // Lưu dữ liệu
                SaveData();
            }
        }

        private void SaveData()
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(txtSoBan.Text))
                {
                    MessageBox.Show("Vui lòng nhập số bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoBan.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSucChua.Text) || !int.TryParse(txtSucChua.Text, out int sucChua) || sucChua <= 0)
                {
                    MessageBox.Show("Vui lòng nhập sức chứa hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSucChua.Focus();
                    return;
                }

                if (cbbKhuVuc.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn khu vực!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbKhuVuc.Focus();
                    return;
                }

                if (cbbTrangThai.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbTrangThai.Focus();
                    return;
                }

                // Lấy dữ liệu
                string soBan = txtSoBan.Text.Trim();
                int? khuVucId = null;
                
                // Lấy khu_vuc_id từ ComboBox
                if (cbbKhuVuc.SelectedIndex >= 0 && cbbKhuVuc.DataSource is DataTable dtKhuVucSource)
                {
                    khuVucId = Convert.ToInt32(dtKhuVucSource.Rows[cbbKhuVuc.SelectedIndex]["khu_vuc_id"]);
                }
                else if (cbbKhuVuc.SelectedItem is DataRowView drv)
                {
                    khuVucId = Convert.ToInt32(drv["khu_vuc_id"]);
                }
                else if (cbbKhuVuc.SelectedValue != null)
                {
                    khuVucId = Convert.ToInt32(cbbKhuVuc.SelectedValue);
                }

                string trangThai = "";
                switch (cbbTrangThai.SelectedIndex)
                {
                    case 0:
                        trangThai = "TRỐNG";
                        break;
                    case 1:
                        trangThai = "PHỤC VỤ";
                        break;
                    case 2:
                        trangThai = "ĐÃ ĐẶT";
                        break;
                    case 3:
                        trangThai = "VỆ SINH";
                        break;
                }

                // Validate khu vực có thuộc chi nhánh hiện tại không
                if (khuVucId.HasValue)
                {
                    var dtKhuVucCheck = _banBLL.LayDanhSachKhuVucTheoChiNhanh(UI.Common.Session.ChiNhanhId);
                    bool khuVucHopLe = false;
                    foreach (DataRow row in dtKhuVucCheck.Rows)
                    {
                        if (Convert.ToInt32(row["khu_vuc_id"]) == khuVucId.Value)
                        {
                            khuVucHopLe = true;
                            break;
                        }
                    }
                    
                    if (!khuVucHopLe)
                    {
                        MessageBox.Show("Khu vực được chọn không thuộc chi nhánh hiện tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Cập nhật
                bool success = _banBLL.CapNhatBan(_banId, soBan, sucChua, khuVucId, trangThai);
                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin bàn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetEditMode(false);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật thông tin bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Lỗi khi lưu dữ liệu: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nChi tiết: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Xác nhận xóa
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa bàn '{txtSoBan.Text}'?\n\nLưu ý: Tất cả các đặt bàn liên quan cũng sẽ bị xóa.",
                    "Xác nhận xóa bàn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    // Thực hiện xóa
                    bool success = _banBLL.XoaBan(_banId);
                    if (success)
                    {
                        MessageBox.Show("Xóa bàn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
