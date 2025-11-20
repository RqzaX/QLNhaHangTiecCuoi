using System;
using System.Drawing;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using UI.Controls;
using UI.Common;

namespace UI
{
    public partial class Frm_ThemNguyenLieu : Form
    {
        private readonly NguyenLieuBLL _bll;
        private readonly DatabaseHelper _dbHelper;

        private Label lblMa, lblTen, lblDonVi, lblSoLuong;
        private TextBox txtMa, txtTen, txtDonVi, txtSoLuong;
        private RoundedButton btnLuu, btnHuy;

        public int? CreatedNguyenLieuId { get; private set; }

        public Frm_ThemNguyenLieu()
        {
            _dbHelper = new DatabaseHelper();
            _bll = new NguyenLieuBLL(_dbHelper);
            InitUI();
        }

        private void InitUI()
        {
            Text = "Thêm Nguyên Liệu Mới";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ClientSize = new Size(500, 320);
            BackColor = Color.White;

            BuildUI();
            WireEvents();
        }

        private void BuildUI()
        {
            // Labels
            lblMa = new Label
            {
                Text = "Mã nguyên liệu *",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            lblTen = new Label
            {
                Text = "Tên nguyên liệu *",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(20, 70),
                AutoSize = true
            };

            lblDonVi = new Label
            {
                Text = "Đơn vị tính *",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(20, 120),
                AutoSize = true
            };

            lblSoLuong = new Label
            {
                Text = "Số lượng ban đầu",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(20, 170),
                AutoSize = true
            };

            // TextBoxes
            txtMa = new TextBox
            {
                Location = new Point(150, 18),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10F)
            };

            txtTen = new TextBox
            {
                Location = new Point(150, 68),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10F)
            };

            txtDonVi = new TextBox
            {
                Location = new Point(150, 118),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "VD: kg, lít, quả..."
            };

            txtSoLuong = new TextBox
            {
                Location = new Point(150, 168),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Nhập số lượng (để trống = 0)"
            };

            // Buttons
            btnLuu = new RoundedButton
            {
                Text = "Lưu",
                Location = new Point(310, 240),
                Size = new Size(80, 40),
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F)
            };

            btnHuy = new RoundedButton
            {
                Text = "Hủy",
                Location = new Point(400, 240),
                Size = new Size(80, 40),
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI Semibold", 10F),
                DialogResult = DialogResult.Cancel
            };

            Controls.AddRange(new Control[] 
            { 
                lblMa, lblTen, lblDonVi, lblSoLuong,
                txtMa, txtTen, txtDonVi, txtSoLuong,
                btnLuu, btnHuy 
            });
        }

        private void WireEvents()
        {
            btnLuu.Click += BtnLuu_Click;
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var ma = txtMa.Text?.Trim();
                var ten = txtTen.Text?.Trim();
                var donVi = txtDonVi.Text?.Trim();

                // Validation
                if (string.IsNullOrWhiteSpace(ma))
                {
                    MessageBox.Show("Vui lòng nhập Mã nguyên liệu!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMa.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(ten))
                {
                    MessageBox.Show("Vui lòng nhập Tên nguyên liệu!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTen.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(donVi))
                {
                    MessageBox.Show("Vui lòng nhập Đơn vị tính!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonVi.Focus();
                    return;
                }

                // Kiểm tra số lượng
                decimal soLuong = 0;
                if (!string.IsNullOrWhiteSpace(txtSoLuong.Text))
                {
                    if (!decimal.TryParse(txtSoLuong.Text.Trim(), out soLuong) || soLuong < 0)
                    {
                        MessageBox.Show("Số lượng phải là số dương hoặc bằng 0!", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoLuong.Focus();
                        return;
                    }
                }

                // Thêm nguyên liệu
                int nlId = _bll.Them(ma, ten, donVi);

                if (nlId > 0)
                {
                    CreatedNguyenLieuId = nlId;

                    // Nếu có số lượng > 0, lưu vào tồn kho cho chi nhánh hiện tại
                    if (soLuong > 0)
                    {
                        int chiNhanhId = Session.ChiNhanhId > 0 ? Session.ChiNhanhId : 1; // Fallback to 1 if session not set
                        try
                        {
                            _bll.CapNhatTonKho(chiNhanhId, nlId, soLuong);
                        }
                        catch (Exception exTonKho)
                        {
                            // Log lỗi nhưng vẫn báo thành công thêm nguyên liệu
                            MessageBox.Show($"Đã thêm nguyên liệu thành công!\n\nLưu ý: Có lỗi khi cập nhật tồn kho: {exTonKho.Message}", 
                                "Thành công (có cảnh báo)", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            DialogResult = DialogResult.OK;
                            Close();
                            return;
                        }
                    }

                    MessageBox.Show("Đã thêm nguyên liệu thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm nguyên liệu!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nguyên liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

