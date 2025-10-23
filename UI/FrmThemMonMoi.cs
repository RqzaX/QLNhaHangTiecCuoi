using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class FrmThemMonMoi : Form
    {
        private readonly ThucDonGoiBLL _bll;

        private TextBox txtMa, txtTen, txtNhom, txtDVT, txtGia;
        private CheckBox chkDangBan;
        private Button btnLuu, btnHuy;

        public int? CreatedMonId { get; private set; }  // khi thêm
        public int? EditMonId { get; private set; }  // khi sửa

        // === Thêm ===
        public FrmThemMonMoi()
        {
            _bll = new ThucDonGoiBLL();
            InitUI("Thêm món mới");
        }

        // === Sửa ===
        public FrmThemMonMoi(int monId, string maMon, string tenMon, string nhom, string donViTinh, decimal donGia, bool dangBan)
        {
            _bll = new ThucDonGoiBLL();
            EditMonId = monId;
            InitUI("Sửa món");
            txtMa.Text = maMon ?? "";
            txtTen.Text = tenMon ?? "";
            txtNhom.Text = nhom ?? "";
            txtDVT.Text = donViTinh ?? "";
            txtGia.Text = donGia.ToString(CultureInfo.InvariantCulture);
            chkDangBan.Checked = dangBan;
        }

        private void InitUI(string title)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ClientSize = new Size(520, 300);

            BuildUI();
        }

        private void BuildUI()
        {
            var lblMa = new Label { Text = "Mã món *", Left = 20, Top = 20, Width = 120 };
            var lblTen = new Label { Text = "Tên món *", Left = 20, Top = 55, Width = 120 };
            var lblNhom = new Label { Text = "Nhóm", Left = 20, Top = 90, Width = 120 };
            var lblDVT = new Label { Text = "Đơn vị tính *", Left = 20, Top = 125, Width = 120 };
            var lblGia = new Label { Text = "Đơn giá *", Left = 20, Top = 160, Width = 120 };

            txtMa = new TextBox { Left = 150, Top = 18, Width = 330 };
            txtTen = new TextBox { Left = 150, Top = 53, Width = 330 };
            txtNhom = new TextBox { Left = 150, Top = 88, Width = 330 };
            txtDVT = new TextBox { Left = 150, Top = 123, Width = 330 , PlaceholderText = "VD: Đĩa hoặc Tô" };
            txtGia = new TextBox { Left = 150, Top = 158, Width = 330, PlaceholderText = "VD: 45000 hoặc 45,000" };

            chkDangBan = new CheckBox { Left = 150, Top = 190, Text = "Đang bán", Checked = true };

            btnLuu = new Button { Text = "Lưu", Left = 310, Top = 230, Width = 80 };
            btnHuy = new Button { Text = "Hủy", Left = 400, Top = 230, Width = 80, DialogResult = DialogResult.Cancel };

            btnLuu.Click += btnLuu_Click;

            Controls.AddRange(new Control[] { lblMa, lblTen, lblNhom, lblDVT, lblGia, txtMa, txtTen, txtNhom, txtDVT, txtGia, chkDangBan, btnLuu, btnHuy });
        }

        private void btnLuu_Click(object? sender, EventArgs e)
        {
            try
            {
                var ma = txtMa.Text?.Trim();
                var ten = txtTen.Text?.Trim();
                var nhom = string.IsNullOrWhiteSpace(txtNhom.Text) ? null : txtNhom.Text.Trim();
                var dvt = txtDVT.Text?.Trim();

                if (string.IsNullOrWhiteSpace(ma)) { MessageBox.Show("Vui lòng nhập Mã món."); return; }
                if (string.IsNullOrWhiteSpace(ten)) { MessageBox.Show("Vui lòng nhập Tên món."); return; }
                if (string.IsNullOrWhiteSpace(dvt)) { MessageBox.Show("Vui lòng nhập Đơn vị tính."); return; }

                if (!TryParseMoney(txtGia.Text, out decimal gia) || gia < 0)
                { MessageBox.Show("Đơn giá không hợp lệ."); return; }

                if (EditMonId.HasValue)
                {
                    _bll.CapNhatMonAn(EditMonId.Value, ma!, ten!, nhom, dvt!, gia, chkDangBan.Checked);
                    MessageBox.Show("Đã cập nhật món thành công!");
                }
                else
                {
                    var id = _bll.ThemMonAn(ma!, ten!, nhom, dvt!, gia, chkDangBan.Checked);
                    CreatedMonId = id;
                    MessageBox.Show("Đã thêm món thành công!");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool TryParseMoney(string? input, out decimal value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;
            string raw = input.Replace(",", "").Replace(".", "");
            return decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }
    }
}
