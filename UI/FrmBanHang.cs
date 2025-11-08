using BLL;
using QLNhaHangTiecCuoi.Share;
using QLNhaHangTiecCuoi.BLL;
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
using Guna.UI2.WinForms;
using UI.Common;
using Sunny.UI;
using Tulpep.NotificationWindow;
using Windows.UI.Notifications;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmBanHang : Form
    {
        private readonly MonAnBLL _bll;
        private readonly KOTBLL _kotBLL;
        private readonly DatabaseHelper _dbHelper;
        private const int BUTTON_WIDTH = 220;
        private const int BUTTON_HEIGHT = 150;
        private const int SPACING_X = -5;
        private const int SPACING_Y = -5;
        private Guna2Button _btnTatCaSelected;
        // Giỏ hàng
        private List<OrderItemCard> _cartItems = new List<OrderItemCard>();
        // Thông tin bàn đã chọn
        private int _selectedBanId = 0;
        private string _selectedSoBan = "";
        private int? _phieuOrderId = null;
        private readonly OrderBLL _orderBLL;
        public FrmBanHang()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bll = new MonAnBLL(_dbHelper);
            _kotBLL = new KOTBLL(_dbHelper);
            _orderBLL = new OrderBLL(_dbHelper);
        }

        public FrmBanHang(string soBan, string tenKhachHang, int soKhach)
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            _bll = new MonAnBLL(_dbHelper);
            _kotBLL = new KOTBLL(_dbHelper);
            _orderBLL = new OrderBLL(_dbHelper);
            
            // Set thông tin bàn và khách hàng
            _selectedSoBan = soBan;
            _selectedBanId = GetBanIdFromSoBan(soBan);
            
            // Cập nhật UI
            btnChonBan.Text = $"{soBan}    PHỤC VỤ\n👥 {soKhach} khách";
            if (btnChonBan is Guna2Button gb)
            {
                gb.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
                gb.TextOffset = new Point(14, 0);
                gb.Padding = new Padding(14, 8, 14, 8);
                if (gb.Height < 50)
                {
                    gb.Height = 50;
                }
            }
            ApplySelectedTableStyle("PHỤC VỤ");
        }

        private void FrmBanHang_Load(object sender, EventArgs e)
        {
            LoadDanhSachNhomDynamic();
            LoadDanhSachMon();
            SetupSearchFunctionality();
            btnXoaTatCaMon.Click += btnXoaTatCaMon_Click;
            SetupEventHandlers();
        }

        private void SetupEventHandlers()
        {
            // Tìm nút "Gửi xuống bếp"
            var btnGuiXuongBep = this.Controls.Find("btnGuiXuongBep", true).FirstOrDefault() as Guna2Button;
            if (btnGuiXuongBep != null)
            {
                btnGuiXuongBep.Click += GuiDonXuongBep_Click;
            }
        }
        private void LoadDanhSachNhomDynamic()
        {
            try
            {
                panelNhomMon.Controls.Clear();

                // Tạo button "Tất cả" 
                Guna2Button btnTatCa = CreateNhomButton("Tất cả", null);
                btnTatCa.Location = new Point(5, 10); 
                panelNhomMon.Controls.Add(btnTatCa);
                _btnTatCaSelected = btnTatCa;

                DataTable dt = _bll.LayDanhSachNhomMon();

                if (dt != null && dt.Rows.Count > 0)
                {
                    int x = 130;
                    int y = 10; 
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        string tenNhom = row["nhom"].ToString();

                        if (string.IsNullOrWhiteSpace(tenNhom))
                            continue;

                        Guna2Button btn = CreateNhomButton(tenNhom, tenNhom);
                        btn.Location = new Point(x, y);
                        panelNhomMon.Controls.Add(btn);
                        
                        x += btn.Width + 5;
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

                DisplayMonAnData(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách món: {ex.Message}", "Lỗi");
            }
        }

        private void DisplayMonAnData(DataTable dt)
        {
            int panelWidth = panelDanhSachMon.Width;
            int buttonsPerRow = (panelWidth - 10) / (BUTTON_WIDTH + SPACING_X);

            if (buttonsPerRow <= 0) buttonsPerRow = 1;

            int currentRow = 0;
            int currentCol = 0;

            foreach (DataRow row in dt.Rows)
            {
                int x = 0 + currentCol * (BUTTON_WIDTH + SPACING_X);
                int y = 0 + currentRow * (BUTTON_HEIGHT + SPACING_Y);

                CreateMonAnCard(row, x, y);

                currentCol++;
                if (currentCol >= buttonsPerRow)
                {
                    currentCol = 0;
                    currentRow++;
                }
            }
        }

        private void CreateMonAnCard(DataRow row, int x, int y)
        {
            int monId = (int)row["mon_id"];
            string maMon = row["ma_mon"].ToString();
            string tenMon = row["ten_mon"].ToString();
            decimal donGia = (decimal)row["don_gia"];

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
        }
        private void BtnMon_Click(MonAnInfo mon)
        {
            if (mon == null) return;
            
            var existingItem = _cartItems.FirstOrDefault(item => item.MonId == mon.MonId);
            
            if (existingItem != null)
            {
                existingItem.Quantity += 1;
                existingItem.UpdateDisplay();
            }
            else
            {
                var orderItem = new OrderItemCard
                {
                    MonId = mon.MonId,
                    TenMon = mon.TenMon,
                    DonGia = mon.DonGia,
                    Quantity = 1,
                    Location = new Point(0, _cartItems.Count * 130),
                    Size = new Size(348, 120)
                };
                
                orderItem.ItemRemoved += OnItemRemoved;
                orderItem.QuantityChanged += OnQuantityChanged;
                
                _cartItems.Add(orderItem);
                panelGioHang.Controls.Add(orderItem);
            }
            
            UpdateOrderCount();

            GunaToast.Show(this, $"Đã thêm: {mon.TenMon}\nGiá: {mon.DonGia:N0} đ", UI.Controls.ToastType.Success, 2600, UI.Controls.ToastPos.TopRight);
        }
        private void btnTenMon_Click(object sender, EventArgs e)
        {
            
        }
        [SupportedOSPlatform("windows")]
        private void btnChonBan_Click(object sender, EventArgs e)
        {
            Frm_ChonBan frm = new Frm_ChonBan();
            var result = frm.ShowDialog();

            if (result == DialogResult.OK)
            {
                string soBan = frm.SelectedSoBan;
                int sucChua = frm.SelectedSucChua;
                string trangThai = frm.SelectedTrangThai;

                if (btnChonBan != null && !string.IsNullOrWhiteSpace(soBan))
                {
                    // Lưu thông tin bàn đã chọn
                    _selectedSoBan = soBan;
                    _selectedBanId = GetBanIdFromSoBan(soBan);
                    btnChonBan.Text = $"{soBan}    {trangThai}\n👥 0/{sucChua} khách";
                    if (btnChonBan is Guna2Button gb)
                    {
                        gb.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
                        gb.TextOffset = new Point(14, 0);
                        gb.Padding = new Padding(14, 8, 14, 8);
                        // Tăng height
                        if (gb.Height < 50)
                        {
                            gb.Height = 50;
                        }
                    }
                    ApplySelectedTableStyle(trangThai);
                }
            }
        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaTatCaMon_Click(object sender, EventArgs e)
        {
            try
            {
                if (_cartItems.Count == 0)
                {
                    GunaToast.Show(this, "Giỏ hàng đã trống!", UI.Controls.ToastType.Info, 2000, UI.Controls.ToastPos.TopRight);
                    return;
                }

                var result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa tất cả {_cartItems.Count} món trong giỏ hàng?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    panelGioHang.Controls.Clear();
                    
                    foreach (var item in _cartItems)
                    {
                        item.Dispose();
                    }
                    
                    _cartItems.Clear();
                    
                    UpdateOrderCount();
                    
                    GunaToast.Show(this, "Đã xóa tất cả món trong giỏ hàng!", UI.Controls.ToastType.Success, 2000, UI.Controls.ToastPos.TopRight);
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi khi xóa món: {ex.Message}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
            }
        }

        private void ApplySelectedTableStyle(string trangThai)
        {
            if (btnChonBan == null) return;

            string st = (trangThai ?? "").Trim().ToUpperInvariant();

            Color back = Color.FromArgb(220, 252, 231);      // Green-100
            Color hover = Color.FromArgb(187, 247, 208);     // Green-200 (base for calc)
            Color down = Color.FromArgb(134, 239, 172);      // Green-300 (base for calc)
            Color text = Color.FromArgb(22, 101, 52);        // Green-800
            Color border = Color.FromArgb(16, 185, 129);     // Green-500

            if (st.Contains("PHỤC VỤ") || st.Contains("ĐANG DÙNG") || st.Contains("ĐANG PHỤC VỤ"))
            {
                back = Color.FromArgb(254, 226, 226);   // Red-100
                hover = Color.FromArgb(254, 202, 202);  // Red-200
                down = Color.FromArgb(252, 165, 165);   // Red-300
                text = Color.FromArgb(153, 27, 27);     // Red-800
                border = Color.FromArgb(239, 68, 68);   // Red-500
            }
            else if (st.Contains("ĐÃ ĐẶT") || st.Contains("ĐẶT TRƯỚC"))
            {
                back = Color.FromArgb(254, 243, 199);   // Amber-100
                hover = Color.FromArgb(253, 230, 138);  // Amber-200
                down = Color.FromArgb(252, 211, 77);    // Amber-300
                text = Color.FromArgb(146, 64, 14);     // Amber-800
                border = Color.FromArgb(245, 158, 11);  // Amber-500
            }

            if (btnChonBan is Guna2Button rb)
            {
                Color customHover = Blend(back, border, 0.12);
                Color customDown = Blend(back, border, 0.22);
                rb.HoverState.FillColor = customHover;
                rb.FillColor = back;
                rb.ForeColor = text;
            }
        }

        private EventHandler _btnHoverEnter;
        private EventHandler _btnHoverLeave;
        private MouseEventHandler _btnHoverDown;
        private MouseEventHandler _btnHoverUp;

        private void ApplyCustomHover(Button btn, Color baseBack, Color accent)
        {
            if (_btnHoverEnter != null) btn.MouseEnter -= _btnHoverEnter;
            if (_btnHoverLeave != null) btn.MouseLeave -= _btnHoverLeave;
            if (_btnHoverDown != null) btn.MouseDown -= _btnHoverDown;
            if (_btnHoverUp != null) btn.MouseUp -= _btnHoverUp;

            Color hoverBack = Blend(baseBack, accent, 0.12);
            Color downBack = Blend(baseBack, accent, 0.22);

            _btnHoverEnter = (s, e) => { btn.BackColor = hoverBack; };
            _btnHoverLeave = (s, e) => { btn.BackColor = baseBack; };
            _btnHoverDown = (s, e) => { btn.BackColor = downBack; };
            _btnHoverUp = (s, e) =>
            {
                var inside = btn.ClientRectangle.Contains(btn.PointToClient(Control.MousePosition));
                btn.BackColor = inside ? hoverBack : baseBack;
            };

            btn.MouseEnter += _btnHoverEnter;
            btn.MouseLeave += _btnHoverLeave;
            btn.MouseDown += _btnHoverDown;
            btn.MouseUp += _btnHoverUp;
        }

        private Color Blend(Color a, Color b, double t)
        {
            int r = (int)(a.R + (b.R - a.R) * t);
            int g = (int)(a.G + (b.G - a.G) * t);
            int bl = (int)(a.B + (b.B - a.B) * t);
            return Color.FromArgb(Clamp(r), Clamp(g), Clamp(bl));
        }
        private int Clamp(int v) => v < 0 ? 0 : (v > 255 ? 255 : v);

        private int GetBanIdFromSoBan(string soBan)
        {
            try
            {
                // Tìm ban_id từ số bàn
                string query = "SELECT ban_id FROM ban WHERE so_ban = @soBan AND chi_nhanh_id = @chiNhanhId";
                var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
                {
                    new Microsoft.Data.SqlClient.SqlParameter("@soBan", soBan),
                    new Microsoft.Data.SqlClient.SqlParameter("@chiNhanhId", Session.ChiNhanhId)
                };

                var result = _dbHelper.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch
            {
                return 0;
            }
        }

        private void GuiDonXuongBep_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedBanId == 0 || string.IsNullOrEmpty(_selectedSoBan))
                {
                    GunaToast.Show(this, "Vui lòng chọn bàn trước khi gửi đơn!", UI.Controls.ToastType.Info, 2000, UI.Controls.ToastPos.TopRight);
                    return;
                }

                if (_cartItems.Count == 0)
                {
                    GunaToast.Show(this, "Giỏ hàng trống! Vui lòng thêm món trước khi gửi đơn.", UI.Controls.ToastType.Info, 2000, UI.Controls.ToastPos.TopRight);
                    return;
                }

                var result = MessageBox.Show(
                    $"Bạn có chắc muốn gửi đơn cho {_selectedSoBan} xuống bếp?\n\nSố món: {_cartItems.Count}",
                    "Xác nhận gửi đơn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;

                var orderItems = new List<KOTBLL.OrderItem>();
                var forPersist = new List<OrderItemInput>();
                foreach (var cartItem in _cartItems)
                {
                    orderItems.Add(new KOTBLL.OrderItem
                    {
                        MonId = cartItem.MonId,
                        Quantity = cartItem.Quantity,
                        Notes = cartItem.Note
                    });

                    forPersist.Add(new OrderItemInput
                    {
                        MonId = cartItem.MonId,
                        TenMon = cartItem.TenMon,
                        DonGia = cartItem.DonGia,
                        SoLuong = cartItem.Quantity
                    });
                }

                bool success = _kotBLL.GuiDonXuongBep(_selectedBanId, Session.ChiNhanhId, Session.NguoiDungId, orderItems, "BẾP");

                if (success)
                {
                    // Lưu phiếu order vào DB và sinh hóa đơn chờ thanh toán
                    int poId = _orderBLL.SaveOrder(Session.ChiNhanhId, _selectedBanId, Session.HoTen, forPersist);
                    _phieuOrderId = poId;
                    // VAT mặc định 8%
                    int hdId = _orderBLL.CreateInvoiceFromCart(Session.ChiNhanhId, forPersist, 8m, 0m, 0m);

                    panelGioHang.Controls.Clear();
                    foreach (var item in _cartItems)
                    {
                        item.Dispose();
                    }
                    _cartItems.Clear();
                    UpdateOrderCount();

                    GunaToast.Show(this, $"Đã gửi đơn cho {_selectedSoBan} xuống bếp thành công!", UI.Controls.ToastType.Success, 2000, UI.Controls.ToastPos.TopRight);
                }
                else
                {
                    GunaToast.Show(this, "Có lỗi xảy ra khi gửi đơn xuống bếp!", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
                }
            }
            catch (Exception ex)
            {
                GunaToast.Show(this, $"Lỗi gửi đơn: {ex.Message}", UI.Controls.ToastType.Error, 3000, UI.Controls.ToastPos.TopRight);
            }
        }
        private Guna2Button CreateNhomButton(string displayName, string nhomName)
        {
            int textWidth = TextRenderer.MeasureText(displayName, new Font("Segoe UI", 11, FontStyle.Bold)).Width;
            int buttonWidth = Math.Max(120, textWidth + 20);
            
            Guna2Button btn = new Guna2Button
            {
                Text = displayName,
                Name = $"btnNhom_{displayName}",
                Width = buttonWidth,
                Height = 33,
                Margin = new Padding(5, 5, 5, 5),
                Tag = nhomName,
                FillColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderRadius = 10,
                BorderColor = Color.FromArgb(225, 229, 234),
                BorderThickness = 1,
                Animated = true
            };

            btn.Click += (s, e) => BtnNhom_Click(btn);

            return btn;
        }
        private void BtnNhom_Click(Guna2Button btn)
        {
            if (_btnTatCaSelected != null)
            {
                _btnTatCaSelected.FillColor = Color.White;
                _btnTatCaSelected.ForeColor = Color.Black;
            }

            btn.FillColor = Color.FromArgb(31, 111, 235);
            btn.ForeColor = Color.White;
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

                DisplayMonAnData(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách món: {ex.Message}", "Lỗi");
            }
        }
        private void UpdateOrderCount()
        {
            int totalItems = _cartItems.Sum(item => item.Quantity);
            labelDonHang.Text = $"Đơn hàng ({totalItems})";
            UpdateTotals();
        }
        
        private void UpdateTotals()
        {
            decimal subTotal = _cartItems.Sum(item => item.DonGia * item.Quantity);
            decimal vatAmount = subTotal * 0.08m;
            decimal total = subTotal + vatAmount;
            lbTamTinh.Text = $"{subTotal:N0} đ";
            lbVAT.Text = $"{vatAmount:N0} đ";
            lbTongCong.Text = $"{total:N0} đ";
        }
        
        private void OnItemRemoved(object sender, EventArgs e)
        {
            var item = sender as OrderItemCard;
            if (item != null)
            {
                _cartItems.Remove(item);
                panelGioHang.Controls.Remove(item);
                item.Dispose();
                
                ReorderCartItems();
                UpdateOrderCount();
            }
        }
        
        private void OnQuantityChanged(object sender, EventArgs e)
        {
            UpdateOrderCount();
        }
        
        private void ReorderCartItems()
        {
            panelGioHang.Controls.Clear();
            
            for (int i = 0; i < _cartItems.Count; i++)
            {
                _cartItems[i].Location = new Point(0, i * 130);
                panelGioHang.Controls.Add(_cartItems[i]);
            }
        }
        
        public class MonAnInfo
        {
            public int MonId { get; set; }
            public string MaMon { get; set; }
            public string TenMon { get; set; }
            public decimal DonGia { get; set; }
        }

        private void SetupSearchFunctionality()
        {
            txtTimMon.Text = "Tìm kiếm món ăn...";
            txtTimMon.ForeColor = Color.Gray;
            
            txtTimMon.Enter += TxtTimMon_Enter;
            txtTimMon.Leave += TxtTimMon_Leave;
            txtTimMon.TextChanged += TxtTimMon_TextChanged;
            txtTimMon.KeyPress += TxtTimMon_KeyPress;
        }

        private void TxtTimMon_Enter(object sender, EventArgs e)
        {
            if (txtTimMon.Text == "Tìm kiếm món ăn...")
            {
                txtTimMon.Text = "";
                txtTimMon.ForeColor = Color.Black;
            }
        }

        private void TxtTimMon_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimMon.Text))
            {
                txtTimMon.Text = "Tìm kiếm món ăn...";
                txtTimMon.ForeColor = Color.Gray;
            }
        }

        private void TxtTimMon_TextChanged(object sender, EventArgs e)
        {
            if (txtTimMon.Text != "Tìm kiếm món ăn...")
            {
                PerformSearch();
            }
        }

        private void TxtTimMon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PerformSearch();
                e.Handled = true;
            }
        }

        private void PerformSearch()
        {
            try
            {
                string searchText = txtTimMon.Text.Trim();
                
                if (string.IsNullOrWhiteSpace(searchText) || searchText == "Tìm kiếm món ăn...")
                {
                    LoadDanhSachMon(); // Load tất cả món
                    LoadDanhSachNhomDynamic(); // Load tất cả nhóm
                    return;
                }

                SearchNhomMon(searchText);

                // Tìm kiếm món ăn
                DataTable searchResults;
                
                if (_btnTatCaSelected != null && _btnTatCaSelected.Text != "Tất cả")
                {
                    string selectedNhom = _btnTatCaSelected.Text;
                    searchResults = _bll.TimKiemMonAnTheoNhom(searchText, selectedNhom);
                }
                else
                {
                    searchResults = _bll.TimKiemMonAn(searchText);
                }

                DisplaySearchResults(searchResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplaySearchResults(DataTable searchResults)
        {
            try
            {
                panelDanhSachMon.Controls.Clear();

                if (searchResults == null || searchResults.Rows.Count == 0)
                {
                    Label lblNoResults = new Label
                    {
                        Text = "Không tìm thấy món ăn nào",
                        Font = new Font("Segoe UI", 12F, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Location = new Point(20, 20)
                    };
                    panelDanhSachMon.Controls.Add(lblNoResults);
                    return;
                }

                DisplayMonAnData(searchResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị kết quả: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearSearch()
        {
            txtTimMon.Text = "Tìm kiếm món ăn...";
            txtTimMon.ForeColor = Color.Gray;
            LoadDanhSachMon(); 
            LoadDanhSachNhomDynamic();
        }

        private void SearchNhomMon(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadDanhSachNhomDynamic();
                    return;
                }

                var oldLabels = panelNhomMon.Controls.OfType<Label>().ToList();
                foreach (var label in oldLabels)
                {
                    panelNhomMon.Controls.Remove(label);
                    label.Dispose();
                }

                var allButtons = panelNhomMon.Controls.OfType<Guna2Button>().ToList();
                var matchingButtons = allButtons.Where(btn => 
                    btn.Text.ToLower().Contains(searchText.ToLower())).ToList();

                foreach (var btn in allButtons)
                {
                    btn.Visible = false;
                }

                if (matchingButtons.Any())
                {
                    int x = 5;
                    foreach (var btn in matchingButtons)
                    {
                        btn.Visible = true;
                        btn.Location = new Point(x, 10);
                        x += btn.Width + 10;
                    }
                }
                else
                {
                    Label lblNoResults = new Label
                    {
                        Text = "Không tìm thấy nhóm món nào",
                        Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Location = new Point(5, 10)
                    };
                    panelNhomMon.Controls.Add(lblNoResults);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm nhóm món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
