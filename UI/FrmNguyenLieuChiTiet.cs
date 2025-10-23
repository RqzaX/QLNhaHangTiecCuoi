using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLNhaHangTiecCuoi.BLL;

namespace UI
{
    public partial class FrmNguyenLieuChiTiet : Form
    {
        // ====== fields ======
        private readonly int _nlId;
        private readonly int _chiNhanhId;
        private readonly NguyenLieuBLL _bll;

        // ====== controls ======
        private TextBox txtMa, txtTen, txtDonVi;
        private NumericUpDown nudTon;
        private Button btnSua, btnLuu, btnDong;

        private bool _isEdit = false;

        // === ctor KHỞI TẠO TỪ FrmKho ===
        public FrmNguyenLieuChiTiet(int nlId, int chiNhanhId, NguyenLieuBLL bll)
        {
            _nlId = nlId;
            _chiNhanhId = chiNhanhId;
            _bll = bll;

            BuildUI();
            LoadData();
            SetEdit(false);
        }

        // =========== UI ===========
        private void BuildUI()
        {
            Text = "Nguyên liệu - Chi tiết";
            ClientSize = new Size(540, 260);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblMa = new Label { Text = "Mã NL", AutoSize = true, Location = new Point(24, 24) };
            var lblTen = new Label { Text = "Tên NL", AutoSize = true, Location = new Point(24, 64) };
            var lblDv = new Label { Text = "Đơn vị", AutoSize = true, Location = new Point(24, 104) };
            var lblTon = new Label { Text = "SL tồn", AutoSize = true, Location = new Point(24, 144) };

            txtMa = new TextBox { Location = new Point(120, 20), Width = 380 };
            txtTen = new TextBox { Location = new Point(120, 60), Width = 380 };
            txtDonVi = new TextBox { Location = new Point(120, 100), Width = 160 };
            nudTon = new NumericUpDown
            {
                Location = new Point(120, 140),
                Width = 160,
                DecimalPlaces = 3,
                Minimum = 0,
                Maximum = 100000000,
                Increment = 1
            };

            btnSua = new Button { Text = "Sửa", Location = new Point(220, 190), Width = 80 };
            btnLuu = new Button { Text = "Lưu", Location = new Point(310, 190), Width = 80 };
            btnDong = new Button { Text = "Đóng", Location = new Point(400, 190), Width = 80 };

            btnSua.Click += (s, e) => SetEdit(true);
            btnLuu.Click += BtnLuu_Click;
            btnDong.Click += (s, e) => Close();

            Controls.AddRange(new Control[] {
                lblMa, lblTen, lblDv, lblTon,
                txtMa, txtTen, txtDonVi, nudTon,
                btnSua, btnLuu, btnDong
            });
        }

        private void SetEdit(bool enable)
        {
            _isEdit = enable;
            txtMa.ReadOnly = !enable;
            txtTen.ReadOnly = !enable;
            txtDonVi.ReadOnly = !enable;
            nudTon.Enabled = enable;

            btnSua.Enabled = !enable;
            btnLuu.Enabled = enable;
        }

        // =========== DATA ===========
        private void LoadData()
        {
            // 1) Thông tin nguyên liệu
            var r = _bll.LayNguyenLieuById(_nlId); // DataRow (đã hướng dẫn tạo BLL trước đó)
            if (r != null)
            {
                txtMa.Text = r["ma_nl"]?.ToString();
                txtTen.Text = r["ten_nl"]?.ToString();
                txtDonVi.Text = r["don_vi"]?.ToString();
            }

            // 2) Số lượng tồn theo chi nhánh hiện tại
            var slTon = _bll.LayTonKhoTaiChiNhanh(_chiNhanhId, _nlId); // decimal
            if (slTon < nudTon.Minimum) slTon = nudTon.Minimum;
            if (slTon > nudTon.Maximum) slTon = nudTon.Maximum;
            nudTon.Value = slTon;
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (!_isEdit) return;

            var ma = txtMa.Text.Trim();
            var ten = txtTen.Text.Trim();
            var dv = txtDonVi.Text.Trim();

            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten) || string.IsNullOrWhiteSpace(dv))
            {
                MessageBox.Show("Vui lòng nhập đủ Mã, Tên, Đơn vị.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1) Cập nhật thông tin nguyên liệu
                _bll.Sua(_nlId, ma, ten, dv);

                // 2) Cập nhật/Upsert tồn kho cho chi nhánh hiện tại
                _bll.CapNhatTonKho(_chiNhanhId, _nlId, nudTon.Value);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lưu thất bại:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
