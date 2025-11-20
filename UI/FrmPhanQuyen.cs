using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
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
using Guna.UI2.WinForms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmPhanQuyen : Form
    {
        private VaiTroBLL _vaiTroBLL;
        private DatabaseHelper _dbHelper;

        public FrmPhanQuyen()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _vaiTroBLL = new VaiTroBLL(_dbHelper);
            LoadDanhSachVaiTro();
            btnDanhSachNhanVien.Click += btnDanhSachNhanVien_Click;
        }

        private void LoadDanhSachVaiTro()
        {
            try
            {
                panelDanhSachVaiTro.Controls.Clear();

                DataTable dt = _vaiTroBLL.LoadData();
                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }

                DisplayVaiTroData(dt);
                label4.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách vai trò: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayVaiTroData(DataTable dt)
        {
            const int CARD_WIDTH = 330;
            const int CARD_HEIGHT = 160;
            const int SPACING_X = 5;
            const int SPACING_Y = 5;
            const int START_X = 5;
            const int START_Y = 5;

            int panelWidth = panelDanhSachVaiTro.Width;
            int cardsPerRow = (panelWidth - START_X * 2) / (CARD_WIDTH + SPACING_X);

            if (cardsPerRow <= 0) cardsPerRow = 1;

            int currentRow = 0;
            int currentCol = 0;

            foreach (DataRow row in dt.Rows)
            {
                int vaiTroId = Convert.ToInt32(row["vai_tro_id"]);
                string ten = row["ten"].ToString();
                string ma = row["ma"].ToString();
                string moTa = row["mo_ta"] == DBNull.Value ? "" : row["mo_ta"].ToString();
                int soNguoiDung = Convert.ToInt32(row["so_nguoi_dung"]);

                int x = START_X + currentCol * (CARD_WIDTH + SPACING_X);
                int y = START_Y + currentRow * (CARD_HEIGHT + SPACING_Y);

                var card = new Controls.VaiTroCard(vaiTroId, ten, ma, moTa, soNguoiDung);
                card.Location = new Point(x, y);
                card.Size = new Size(CARD_WIDTH, CARD_HEIGHT);
                card.EditClicked += Card_EditClicked;
                card.DeleteClicked += Card_DeleteClicked;

                panelDanhSachVaiTro.Controls.Add(card);

                currentCol++;
                if (currentCol >= cardsPerRow)
                {
                    currentCol = 0;
                    currentRow++;
                }
            }
        }

        private void Card_EditClicked(object sender, EventArgs e)
        {
            var card = sender as Controls.VaiTroCard;
            if (card == null) return;

            try
            {
                DataRow row = _vaiTroBLL.GetById(card.VaiTroId);
                if (row == null)
                {
                    MessageBox.Show("Không tìm thấy vai trò!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string ten = row["ten"].ToString();
                string ma = row["ma"].ToString();
                string moTa = row["mo_ta"] == DBNull.Value ? "" : row["mo_ta"].ToString();

                        var frm = new Frm_ChinhSuaVaiTro(ten, ma, moTa);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Cập nhật vai trò
                    bool success = _vaiTroBLL.Update(card.VaiTroId, frm.MaVaiTro, frm.TenVaiTro, frm.MoTa);
                    if (success)
                    {
                        MessageBox.Show("Cập nhật vai trò thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachVaiTro();
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật vai trò!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa vai trò: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Card_DeleteClicked(object sender, EventArgs e)
        {
            var card = sender as Controls.VaiTroCard;
            if (card == null) return;

            if (card.IsAdminRole)
            {
                MessageBox.Show(
                    "Không thể xóa vai trò admin! Vai trò admin được bảo vệ và không thể xóa.",
                    "Không thể xóa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa vai trò \"{card.TenVaiTro}\"?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = _vaiTroBLL.Delete(card.VaiTroId);
                    if (success)
                    {
                        MessageBox.Show("Xóa vai trò thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachVaiTro();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa vai trò!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa vai trò: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnDanhSachNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                var existing = Application.OpenForms.OfType<Frm_CRUD_NhanVien>().FirstOrDefault();
                if (existing != null)
                {
                    existing.BringToFront();
                    existing.Focus();
                    return;
                }

                using (var frm = new Frm_CRUD_NhanVien())
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở form nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
