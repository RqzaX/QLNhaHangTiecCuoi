using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using UI.Common;

namespace UI
{
    public partial class Frm_ThemBan : Form
    {
        private readonly BanBLL _banBLL;
        private readonly int _chiNhanhId;
        private TextBox txtSoBan;
        private TextBox txtSucChua;
        private ComboBox cbbKhuVuc;
        private ComboBox cbbTrangThai;
        private Button btnLuu;
        private Button btnDong;

        public Frm_ThemBan(int chiNhanhId, BanBLL banBLL)
        {
            _chiNhanhId = chiNhanhId;
            _banBLL = banBLL;
            InitializeComponent();
            LoadKhuVuc();
        }

        private void InitializeComponent()
        {
            this.Text = "Thêm bàn mới";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Title
            Label lblTitle = new Label
            {
                Text = "Thêm bàn mới",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(30, 30),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "Nhập thông tin bàn mới",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(30, 70),
                AutoSize = true
            };

            // Số bàn
            Label label1 = new Label
            {
                Text = "Số bàn *",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(30, 130),
                AutoSize = true
            };

            txtSoBan = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(30, 160),
                Size = new Size(250, 30),
                TabIndex = 0
            };

            // Sức chứa
            Label label2 = new Label
            {
                Text = "Sức chứa *",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(320, 130),
                AutoSize = true
            };

            txtSucChua = new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(320, 160),
                Size = new Size(250, 30),
                TabIndex = 1
            };

            // Khu vực
            Label label3 = new Label
            {
                Text = "Khu vực",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(30, 210),
                AutoSize = true
            };

            cbbKhuVuc = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(30, 240),
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                TabIndex = 2
            };

            // Trạng thái
            Label label4 = new Label
            {
                Text = "Trạng thái *",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(320, 210),
                AutoSize = true
            };

            cbbTrangThai = new ComboBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(320, 240),
                Size = new Size(250, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                TabIndex = 3
            };
            cbbTrangThai.Items.Add("TRỐNG");
            cbbTrangThai.Items.Add("PHỤC VỤ");
            cbbTrangThai.Items.Add("ĐÃ ĐẶT");
            cbbTrangThai.Items.Add("VỆ SINH");
            cbbTrangThai.SelectedIndex = 0;

            // Buttons
            btnLuu = new Button
            {
                Text = "Lưu",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                Location = new Point(320, 300),
                Size = new Size(100, 40),
                TabIndex = 4,
                FlatStyle = FlatStyle.Flat
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += BtnLuu_Click;

            btnDong = new Button
            {
                Text = "Đóng",
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                Location = new Point(430, 300),
                Size = new Size(100, 40),
                TabIndex = 5,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnDong.FlatAppearance.BorderSize = 0;
            btnDong.Click += (s, e) => this.Close();

            // Add controls
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(label1);
            this.Controls.Add(txtSoBan);
            this.Controls.Add(label2);
            this.Controls.Add(txtSucChua);
            this.Controls.Add(label3);
            this.Controls.Add(cbbKhuVuc);
            this.Controls.Add(label4);
            this.Controls.Add(cbbTrangThai);
            this.Controls.Add(btnLuu);
            this.Controls.Add(btnDong);
        }

        private void LoadKhuVuc()
        {
            try
            {
                var dtKhuVuc = _banBLL.LayDanhSachKhuVucTheoChiNhanh(_chiNhanhId);
                cbbKhuVuc.DisplayMember = "ten_khu_vuc";
                cbbKhuVuc.ValueMember = "khu_vuc_id";
                cbbKhuVuc.DataSource = dtKhuVuc;
                cbbKhuVuc.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
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

                if (cbbTrangThai.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbTrangThai.Focus();
                    return;
                }

                // Lấy dữ liệu
                string soBan = txtSoBan.Text.Trim();
                int? khuVucId = null;
                
                if (cbbKhuVuc.SelectedIndex >= 0 && cbbKhuVuc.DataSource is DataTable dtKhuVuc)
                {
                    khuVucId = Convert.ToInt32(dtKhuVuc.Rows[cbbKhuVuc.SelectedIndex]["khu_vuc_id"]);
                }
                else if (cbbKhuVuc.SelectedItem is DataRowView drv)
                {
                    khuVucId = Convert.ToInt32(drv["khu_vuc_id"]);
                }
                else if (cbbKhuVuc.SelectedValue != null)
                {
                    khuVucId = Convert.ToInt32(cbbKhuVuc.SelectedValue);
                }

                string trangThai = cbbTrangThai.SelectedItem.ToString();

                // Thêm bàn
                int banId = _banBLL.ThemBan(_chiNhanhId, soBan, sucChua, khuVucId, trangThai);
                if (banId > 0)
                {
                    MessageBox.Show("Thêm bàn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể thêm bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

