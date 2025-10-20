using BLL;
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
using UI.Controls;
using Windows.UI.Notifications;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmBanHang : Form
    {
        private readonly MonAnBLL _bll;
        private readonly DatabaseHelper _dbHelper;
        private const int BUTTON_WIDTH = 220;
        private const int BUTTON_HEIGHT = 150;
        private const int SPACING_X = -5;
        private const int SPACING_Y = -5;
        private RoundedButton _btnTatCaSelected;
        public FrmBanHang()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bll = new MonAnBLL(_dbHelper);
        }

        private void FrmBanHang_Load(object sender, EventArgs e)
        {
            LoadDanhSachNhomDynamic();
            LoadDanhSachMon();
        }
        private void LoadDanhSachNhomDynamic()
        {
            try
            {
                panelNhomMon.Controls.Clear();

                RoundedButton btnTatCa = CreateNhomButton("Tất cả", null);
                panelNhomMon.Controls.Add(btnTatCa);
                _btnTatCaSelected = btnTatCa;

                DataTable dt = _bll.LayDanhSachNhomMon();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string tenNhom = row["nhom"].ToString();

                        if (string.IsNullOrWhiteSpace(tenNhom))
                            continue;

                        RoundedButton btn = CreateNhomButton(tenNhom, tenNhom);
                        panelNhomMon.Controls.Add(btn);
                    }
                }
                else
                {
                    MessageBox.Show("Không có nhóm món nào trong database!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách nhóm: {ex.Message}\n\n{ex.StackTrace}", "Lỗi");
            }
        }

        private void LoadDanhSachMon()
        {
            try
            {
                panelDanhSachMon.Controls.Clear();

                DataTable dt = _bll.LayTatCaMonAn();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu món ăn!", "Thông báo");
                    return;
                }

                int panelWidth = panelDanhSachMon.Width;
                int buttonsPerRow = (panelWidth - 10) / (BUTTON_WIDTH + SPACING_X);

                if (buttonsPerRow <= 0) buttonsPerRow = 1;

                int currentRow = 0;
                int currentCol = 0;

                foreach (DataRow row in dt.Rows)
                {
                    int monId = (int)row["mon_id"];
                    string maMon = row["ma_mon"].ToString();
                    string tenMon = row["ten_mon"].ToString();
                    decimal donGia = (decimal)row["don_gia"];

                    int x = 0 + currentCol * (BUTTON_WIDTH + SPACING_X);
                    int y = 0 + currentRow * (BUTTON_HEIGHT + SPACING_Y);

                    FoodItemButton btn = new FoodItemButton
                    {
                        Name = $"btnMon_{monId}",
                        Text = tenMon,
                        Title = tenMon,
                        PriceText = $"{donGia:N0} đ",
                        Location = new System.Drawing.Point(x, y),
                        Size = new System.Drawing.Size(BUTTON_WIDTH, BUTTON_HEIGHT),
                        Tag = new MonAnInfo { MonId = monId, MaMon = maMon, TenMon = tenMon, DonGia = donGia }
                    };

                    btn.Click += (s, e) => BtnMon_Click(btn.Tag as MonAnInfo);

                    panelDanhSachMon.Controls.Add(btn);

                    currentCol++;
                    if (currentCol >= buttonsPerRow)
                    {
                        currentCol = 0;
                        currentRow++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách món: {ex.Message}", "Lỗi");
            }
        }
        private void BtnMon_Click(MonAnInfo mon)
        {
            if (mon == null) return;

            ThongBaoGoc.ShowSuccess(this, $"Bạn chọn: {mon.TenMon}\nGiá: {mon.DonGia:N0} đ", autoHide: true, durationMs: 2500);
        }
        private void btnTenMon_Click(object sender, EventArgs e)
        {
            ThongBaoGoc.ShowSuccess(this, "Đã thêm món Tôm nướng phô mai", autoHide: true, durationMs: 2500);
        }
        [SupportedOSPlatform("windows")]
        private void btnChonBan_Click(object sender, EventArgs e)
        {
            Frm_ChonBan frm = new Frm_ChonBan();
            var result = frm.ShowDialog();

            if (result == DialogResult.OK)
            {
                // xử lý sau khi chọn bàn
            }
        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {

        }
        private RoundedButton CreateNhomButton(string displayName, string nhomName)
        {
            RoundedButton btn = new RoundedButton
            {
                Text = displayName,
                Name = $"btnNhom_{displayName}",
                Width = 120,
                Height = 45,
                Margin = new Padding(5),
                Tag = nhomName,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 1, BorderColor = System.Drawing.Color.LightGray },
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular),
                Cursor = Cursors.Hand
            };

            btn.Click += (s, e) => BtnNhom_Click(btn);

            return btn;
        }
        private void BtnNhom_Click(RoundedButton btn)
        {
            if (_btnTatCaSelected != null)
            {
                _btnTatCaSelected.BackColor = System.Drawing.Color.White;
                _btnTatCaSelected.ForeColor = System.Drawing.Color.Black;
            }

            btn.BackColor = System.Drawing.Color.FromArgb(31, 111, 235); // Blue
            btn.ForeColor = System.Drawing.Color.White;
            _btnTatCaSelected = btn;

            string nhom = btn.Tag as string;
            LoadDanhSachMonTheoNhom(nhom);
        }
        private void LoadDanhSachMonTheoNhom(string nhom)
        {
            try
            {
                panelDanhSachMon.Controls.Clear();

                DataTable dt = nhom == null ?
                    _bll.LayTatCaMonAn() :
                    _bll.LayMonAnTheoNhom(nhom);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu món ăn!", "Thông báo");
                    return;
                }

                int panelWidth = panelDanhSachMon.Width;
                int buttonsPerRow = (panelWidth - 10) / (BUTTON_WIDTH + SPACING_X);
                if (buttonsPerRow <= 0) buttonsPerRow = 1;

                int currentRow = 0;
                int currentCol = 0;

                foreach (DataRow row in dt.Rows)
                {
                    int monId = (int)row["mon_id"];
                    string maMon = row["ma_mon"].ToString();
                    string tenMon = row["ten_mon"].ToString();
                    decimal donGia = (decimal)row["don_gia"];

                    int x = 0 + currentCol * (BUTTON_WIDTH + SPACING_X);
                    int y = 0 + currentRow * (BUTTON_HEIGHT + SPACING_Y);

                    FoodItemButton btn = new FoodItemButton
                    {
                        Name = $"btnMon_{monId}",
                        Text = tenMon,
                        Title = tenMon,
                        PriceText = $"{donGia:N0} đ",
                        Location = new System.Drawing.Point(x, y),
                        Size = new System.Drawing.Size(BUTTON_WIDTH, BUTTON_HEIGHT),
                        Tag = new MonAnInfo { MonId = monId, MaMon = maMon, TenMon = tenMon, DonGia = donGia }
                    };

                    btn.Click += (s, e) => BtnMon_Click(btn.Tag as MonAnInfo);
                    panelDanhSachMon.Controls.Add(btn);

                    currentCol++;
                    if (currentCol >= buttonsPerRow)
                    {
                        currentCol = 0;
                        currentRow++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách món: {ex.Message}", "Lỗi");
            }
        }
        public class MonAnInfo
        {
            public int MonId { get; set; }
            public string MaMon { get; set; }
            public string TenMon { get; set; }
            public decimal DonGia { get; set; }
        }
    }
}
