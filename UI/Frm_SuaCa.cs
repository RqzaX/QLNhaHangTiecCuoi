using QLNhaHangTiecCuoi.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.Share;

namespace UI
{
    public partial class Frm_SuaCa : Form
    {
        private readonly int _caId;
        private readonly int _chiNhanhId;
        private readonly string _tenCa;
        private readonly NguoiDungBLL _bll;
        private DataGridView dgvNhanVienChuaTrongCa;
        private DataGridView dgvNhanVienTrongCa;
        private Button btnThem, btnXoa, btnLuu, btnDong;
        private bool _hasChanges = false; // Đánh dấu có thay đổi

        public Frm_SuaCa(int caId, int chiNhanhId, string tenCa, NguoiDungBLL bll)
        {
            _caId = caId;
            _chiNhanhId = chiNhanhId;
            _tenCa = tenCa;
            _bll = bll;
            InitializeComponent();
            Text = $"Chỉnh sửa ca: {_tenCa}";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Clear DataSource trước để đảm bảo refresh
                dgvNhanVienTrongCa.DataSource = null;
                dgvNhanVienChuaTrongCa.DataSource = null;

                // Load nhân viên trong ca
                DataTable dtTrongCa = _bll.LayNhanVienTrongCa(_caId, _chiNhanhId);
                if (dtTrongCa == null)
                {
                    dtTrongCa = new DataTable();
                }
                dgvNhanVienTrongCa.DataSource = dtTrongCa;
                dgvNhanVienTrongCa.Refresh();

                // Load nhân viên chưa có trong ca
                DataTable dtChuaTrongCa = _bll.LayNhanVienChuaTrongCa(_caId, _chiNhanhId);
                if (dtChuaTrongCa == null)
                {
                    dtChuaTrongCa = new DataTable();
                }
                dgvNhanVienChuaTrongCa.DataSource = dtChuaTrongCa;
                dgvNhanVienChuaTrongCa.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNhanVienChuaTrongCa.CurrentRow == null)
                {
                    MessageBox.Show("Chọn một nhân viên để thêm vào ca.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRowView row = (DataRowView)dgvNhanVienChuaTrongCa.CurrentRow.DataBoundItem;
                int nguoiDungId = Convert.ToInt32(row["nguoi_dung_id"]);
                string hoTen = row["ho_ten"].ToString();

                // Thêm nhân viên vào ca
                int nguoiDungCaId = _bll.ThemNhanVienVaoCa(nguoiDungId, _chiNhanhId, _caId);

                if (nguoiDungCaId > 0)
                {
                    // Đánh dấu có thay đổi
                    _hasChanges = true;

                    // Reload data trước khi hiển thị thông báo
                    LoadData();

                    MessageBox.Show($"Đã thêm nhân viên '{hoTen}' vào ca thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể thêm nhân viên vào ca. Vui lòng thử lại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm nhân viên vào ca: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNhanVienTrongCa.CurrentRow == null)
                {
                    MessageBox.Show("Chọn một nhân viên để xóa khỏi ca.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataRowView row = (DataRowView)dgvNhanVienTrongCa.CurrentRow.DataBoundItem;
                int nguoiDungCaId = Convert.ToInt32(row["nguoi_dung_ca_id"]);
                string hoTen = row["ho_ten"].ToString();

                if (MessageBox.Show($"Xóa nhân viên '{hoTen}' khỏi ca?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool success = _bll.XoaNhanVienKhoiCa(nguoiDungCaId);

                    if (success)
                    {
                        // Đánh dấu có thay đổi
                        _hasChanges = true;

                        // Reload data trước khi hiển thị thông báo
                        LoadData();

                        MessageBox.Show($"Đã xóa nhân viên '{hoTen}' khỏi ca thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa nhân viên khỏi ca. Vui lòng thử lại.", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa nhân viên khỏi ca: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            // Dữ liệu đã được lưu ngay khi thêm/xóa, nên chỉ cần đóng form
            // Nếu có thay đổi thì set DialogResult.OK để PhanCaPanel reload
            if (_hasChanges)
            {
                DialogResult = DialogResult.OK;
            }
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Nếu đóng form bằng nút X hoặc ESC, vẫn cần reload nếu có thay đổi
            if (_hasChanges && DialogResult != DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
            }
            base.OnFormClosing(e);
        }
    }
}

