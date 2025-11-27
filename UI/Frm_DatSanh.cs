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
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.DAL;
using UI.Common;

namespace UI
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public partial class Frm_DatSanh : RoundedBorderForm
    {
        private int thuTuSanh = 1;
        private DatSanhBLL _datSanhBLL;
        private KhachHangBLL _khachHangBLL;
        private GoiTiecBLL _goiTiecBLL;
        
        private int? _khachHangId = null;
        private int? _selectedGoiId = null;
        private int? _soBanDuKien = null;
        private decimal _phiSanh = 0;
        private decimal _giaGoi = 0;
        private decimal _tongTien = 0;
        private int? _datSanhId = null;
        private string _phuongThucThanhToan = null;

        public Frm_DatSanh()
        {
            InitializeComponent();
            
            CornerRadius = 14;
            BorderColor = Color.Black;
            BorderThickness = 2;
            ShowDropShadow = true;
            
            _datSanhBLL = new DatSanhBLL();
            _khachHangBLL = new KhachHangBLL();
            _goiTiecBLL = new GoiTiecBLL();
            
            btnQuayLai.Visible = false;
            panelDatSanh.Visible = true;
            panelGoivaMon.Visible = false;
            panelHopDong.Visible = false;

            panelBuoc1.Style = Sunny.UI.UIStyle.Blue;
            panelBuoc2.Style = Sunny.UI.UIStyle.Gray;
            panelBuoc3.Style = Sunny.UI.UIStyle.Gray;

            panelTrangThaiSanh.Text = "Trạng thái sảnh . . .";
            panelTrangThaiSanh.ForeColor = Color.Gray;
            panelTrangThaiSanh.Style = Sunny.UI.UIStyle.Gray;

            picSuccess1.Visible = false;
            picSuccess2.Visible = false;
            picSuccess3.Visible = false;

            this.Load += Frm_DatSanh_Load;
            cbbGioToChuc.DrawItem += CbbGioToChuc_DrawItem;
            cbbChiNhanh.SelectedIndexChanged += CbbChiNhanh_SelectedIndexChanged;
            cbbSanh.SelectedIndexChanged += CbbSanh_SelectedIndexChanged;
            cbbGioToChuc.SelectedIndexChanged += CbbGioToChuc_SelectedIndexChanged;
            dateNgayToChuc.ValueChanged += DateNgayToChuc_ValueChanged;
            txtSoBanDuKien.TextChanged += TxtSoBanDuKien_TextChanged;
            btnTienMat.Click += BtnTienMat_Click;
            btnThe.Click += BtnThe_Click;
            btnChuyenKhoan.Click += BtnChuyenKhoan_Click;
            btnHienThiQR.Click += BtnHienThiQR_Click;
        }

        // Sự kiện load form - khởi tạo dữ liệu ban đầu
        private void Frm_DatSanh_Load(object sender, EventArgs e)
        {
            try
            {
                LoadChiNhanh();
                LoadCa();
                
                dateNgayToChuc.Value = DateTime.Now;
                dateNgayToChuc.MinDate = DateTime.Now;
                
                CapNhatAutoScrollPanel1();
                
                TinhToanGia();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật AutoScroll cho panel1
        private void CapNhatAutoScrollPanel1()
        {
            try
            {
                if (panel1 == null) return;
                int maxBottom = 0;
                foreach (Control ctrl in panel1.Controls)
                {
                    if (ctrl.Visible)
                    {
                        int bottom = ctrl.Top + ctrl.Height;
                        if (bottom > maxBottom)
                        {
                            maxBottom = bottom;
                        }
                    }
                }

                int padding = 20;
                int minHeight = maxBottom + padding;
                panel1.AutoScrollMinSize = new Size(0, minHeight);
            }
            catch (Exception)
            {}
        }

        // Load danh sách chi nhánh vào ComboBox
        private void LoadChiNhanh()
        {
            try
            {
                DataTable dt = _datSanhBLL.LayDanhSachChiNhanh();
                cbbChiNhanh.DataSource = dt;
                cbbChiNhanh.DisplayMember = "ten";
                cbbChiNhanh.ValueMember = "chi_nhanh_id";
                
                if (Session.ChiNhanhId > 0)
                {
                    for (int i = 0; i < cbbChiNhanh.Items.Count; i++)
                    {
                        DataRowView row = (DataRowView)cbbChiNhanh.Items[i];
                        if (Convert.ToInt32(row["chi_nhanh_id"]) == Session.ChiNhanhId)
                        {
                            cbbChiNhanh.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chi nhánh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load danh sách sảnh theo chi nhánh đã chọn
        private void LoadSanh()
        {
            try
            {
                if (cbbChiNhanh.SelectedItem == null) return;
                
                DataRowView row = (DataRowView)cbbChiNhanh.SelectedItem;
                int chiNhanhId = Convert.ToInt32(row["chi_nhanh_id"]);
                DataTable dt = _datSanhBLL.LayDanhSachSanh(chiNhanhId);
                
                if (!dt.Columns.Contains("ten_sanh_display"))
                {
                    dt.Columns.Add("ten_sanh_display", typeof(string));
                    foreach (DataRow dr in dt.Rows)
                    {
                        string tenSanh = dr["ten_sanh"].ToString();
                        int sucChua = Convert.ToInt32(dr["suc_chua"]);
                        dr["ten_sanh_display"] = $"{tenSanh} - Sức chứa {sucChua:N0} khách";
                    }
                }
                
                cbbSanh.DataSource = dt;
                cbbSanh.DisplayMember = "ten_sanh_display";
                cbbSanh.ValueMember = "sanh_id";
                
                if (dt.Rows.Count > 0)
                {
                    cbbSanh.SelectedIndex = 0;
                }
                else
                {
                    cbbSanh.DataSource = null;
                    cbbSanh.Items.Clear();
                }
                
                KiemTraTrangThaiSanh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load danh sách giờ tổ chức tiệc (2 giờ cố định: 10:30 và 17:30 - KHÔNG LẤY TỪ DATABASE)
        private void LoadCa()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ca_id", typeof(int));
                dt.Columns.Add("ten_ca", typeof(string));
                dt.Columns.Add("gio_bd", typeof(string));
                dt.Columns.Add("gio_kt", typeof(string));
                dt.Columns.Add("display_text", typeof(string));

                dt.Rows.Add(1, "Ca sáng", "10:30", "13:30", "10:30 - 13:30 | Thường dùng cho các lễ cưới truyền thống, tổ chức sớm để có thời gian nghỉ ngơi.");
                dt.Rows.Add(2, "Ca tối", "17:30", "20:30", "17:30 - 20:30 | Phong cách sang trọng, sôi động, phù hợp cho lễ cưới chính hoặc tiệc lớn.");

                cbbGioToChuc.DataSource = dt;
                cbbGioToChuc.DisplayMember = "display_text";
                cbbGioToChuc.ValueMember = "ca_id";
                
                int maxWidth = cbbGioToChuc.Width;
                using (Graphics g = cbbGioToChuc.CreateGraphics())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string text = row["display_text"].ToString();
                        SizeF textSize = g.MeasureString(text, cbbGioToChuc.Font);
                        int textWidth = (int)textSize.Width + 30;
                        if (textWidth > maxWidth)
                        {
                            maxWidth = textWidth;
                        }
                    }
                }
                cbbGioToChuc.DropDownWidth = maxWidth;
                
                if (dt.Rows.Count > 0)
                {
                    cbbGioToChuc.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách ca: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load danh sách gói tiệc từ database và hiển thị trong panelDanhSachGoiTiec
        private void LoadGoiTiec()
        {
            try
            {
                if (panelDanhSachGoiTiec == null)
                {
                    MessageBox.Show("panelDanhSachGoiTiec không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                panelDanhSachGoiTiec.Visible = true;
                panelDanhSachGoiTiec.BringToFront();
                panelDanhSachGoiTiec.Controls.Clear();
                
                DataTable dt = _goiTiecBLL.GetAllGoiTiec();
                
                if (dt == null || dt.Rows.Count == 0)
                {
                    Label lblNoData = new Label
                    {
                        Text = "Không có gói tiệc nào",
                        Font = new Font("Segoe UI", 12F),
                        ForeColor = Color.Gray,
                        Location = new Point(10, 10),
                        AutoSize = true
                    };
                    panelDanhSachGoiTiec.Controls.Add(lblNoData);
                    return;
                }

                int panelWidth = panelDanhSachGoiTiec.Width;
                int flPanelWidth = panelWidth - 20;
                
                FlowLayoutPanel flPanel = new FlowLayoutPanel
                {
                    Location = new Point(10, 10),
                    Width = flPanelWidth,
                    AutoScroll = false,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Padding = new Padding(0),
                    AutoSize = false,
                    BackColor = Color.Transparent
                };
                
                panelDanhSachGoiTiec.Controls.Add(flPanel);

                int panelCount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        int goiId = Convert.ToInt32(row["goi_id"]);
                        string tenGoi = row["ten_goi"].ToString();
                        decimal giaCoBan = Convert.ToDecimal(row["gia_co_ban"]);

                        var goiPanel = new UI.Controls.GoiTiecPanel(goiId, tenGoi, giaCoBan)
                        {
                            Width = flPanelWidth,
                            Height = 74,
                            Margin = new Padding(0, 0, 0, 10),
                            Visible = true
                        };
                        
                        goiPanel.GoiTiecSelected += (s, ev) =>
                        {
                            foreach (Control ctrl in flPanel.Controls)
                            {
                                if (ctrl is UI.Controls.GoiTiecPanel panel && panel != goiPanel)
                                {
                                    panel.SetSelected(false);
                                }
                            }

                            _selectedGoiId = ev.GoiId;
                            TinhToanGia();
                        };

                        flPanel.Controls.Add(goiPanel);
                        panelCount++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tạo GoiTiecPanel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                }
                
                if (panelCount > 0)
                {
                    int totalHeight = 0;
                    foreach (Control item in flPanel.Controls)
                    {
                        if (item.Visible)
                        {
                            totalHeight += item.Height + item.Margin.Top + item.Margin.Bottom;
                        }
                    }
                    
                    flPanel.Height = totalHeight;
                    flPanel.Visible = true;
                    flPanel.BringToFront();
                    flPanel.Refresh();
                    panelDanhSachGoiTiec.Refresh();
                }
                
                this.BeginInvoke(new Action(() => CapNhatAutoScrollPanel1()));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách gói tiệc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện thay đổi chi nhánh - load lại danh sách sảnh
        private void CbbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSanh();
        }

        // Sự kiện thay đổi sảnh - kiểm tra trạng thái và tính lại giá
        private void CbbSanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            KiemTraTrangThaiSanh();
            TinhToanGia();
        }

        // Sự kiện thay đổi giờ tổ chức tiệc - kiểm tra lại trạng thái sảnh
        private void CbbGioToChuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            KiemTraTrangThaiSanh();
        }

        // Xử lý vẽ item trong ComboBox để hiển thị toàn bộ văn bản
        private void CbbGioToChuc_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();
            
            if (cbbGioToChuc.Items[e.Index] is DataRowView rowView)
            {
                string displayText = rowView["display_text"].ToString();
                
                Brush brush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                    ? new SolidBrush(Color.White)
                    : new SolidBrush(cbbGioToChuc.ForeColor);
                
                e.Graphics.DrawString(displayText, cbbGioToChuc.Font, brush, e.Bounds);
                
                brush.Dispose();
            }
            else
            {
                string text = cbbGioToChuc.Items[e.Index].ToString();
                Brush brush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                    ? new SolidBrush(Color.White)
                    : new SolidBrush(cbbGioToChuc.ForeColor);
                
                e.Graphics.DrawString(text, cbbGioToChuc.Font, brush, e.Bounds);
                brush.Dispose();
            }
            
            e.DrawFocusRectangle();
        }

        // Sự kiện thay đổi ngày tổ chức - kiểm tra lại trạng thái sảnh
        private void DateNgayToChuc_ValueChanged(object sender, EventArgs e)
        {
            KiemTraTrangThaiSanh();
        }

        // Sự kiện thay đổi số bàn dự kiến - tính số khách và cập nhật giá
        private void TxtSoBanDuKien_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSoBanDuKien.Text))
            {
                if (int.TryParse(txtSoBanDuKien.Text.Trim(), out int soBan) && soBan > 0)
                {
                    int soKhach = soBan * 10;
                    txtSoKhachDuKien.Text = soKhach.ToString();
                    _soBanDuKien = soBan;
                }
                else
                {
                    txtSoKhachDuKien.Text = "";
                    _soBanDuKien = null;
                }
            }
            else
            {
                txtSoKhachDuKien.Text = "";
                _soBanDuKien = null;
            }
            
            TinhToanGia();
            KiemTraTrangThaiSanh();
        }

        // Helper method để lấy giá trị từ ComboBox
        private int? GetComboBoxValue(Guna.UI2.WinForms.Guna2ComboBox comboBox, string valueMember)
        {
            if (comboBox.SelectedItem == null) return null;
            
            DataRowView row = (DataRowView)comboBox.SelectedItem;
            return Convert.ToInt32(row[valueMember]);
        }
        
        // Lấy giờ tổ chức từ ComboBox (ánh xạ index sang giờ cố định)
        // Index 0 -> 10:30 (Ca sáng)
        // Index 1 -> 17:30 (Ca tối)
        private TimeSpan? GetGioToChucFromCombo()
        {
            if (cbbGioToChuc == null || cbbGioToChuc.SelectedIndex < 0) return null;
            
            if (cbbGioToChuc.SelectedIndex == 0) return new TimeSpan(10, 30, 0);
            if (cbbGioToChuc.SelectedIndex == 1) return new TimeSpan(17, 30, 0);
            
            return null;
        }



        // Kiểm tra trạng thái sảnh (trống/đã đặt) và hiển thị thông báo
        private void KiemTraTrangThaiSanh()
        {
            try
            {
                int? sanhId = GetComboBoxValue(cbbSanh, "sanh_id");
                
                if (!sanhId.HasValue)
                {
                    panelTrangThaiSanh.Text = "Vui lòng chọn sảnh";
                    panelTrangThaiSanh.ForeColor = Color.Gray;
                    panelTrangThaiSanh.Style = Sunny.UI.UIStyle.Gray;
                    return;
                }

                DateTime ngayToChuc = dateNgayToChuc.Value;

                if (cbbGioToChuc.SelectedItem == null)
                {
                    panelTrangThaiSanh.Text = "Vui lòng chọn giờ tổ chức tiệc";
                    panelTrangThaiSanh.ForeColor = Color.Gray;
                    panelTrangThaiSanh.Style = Sunny.UI.UIStyle.Gray;
                    return;
                }

                int? caId = null;
                if (cbbGioToChuc.SelectedValue != null)
                {
                    caId = Convert.ToInt32(cbbGioToChuc.SelectedValue);
                }
                else if (cbbGioToChuc.SelectedItem is DataRowView rowView)
                {
                    caId = Convert.ToInt32(rowView["ca_id"]);
                }

                string errorMessage = string.Empty;
                bool isTrong = true;

                // Kiểm tra sảnh trống dựa trên giờ tổ chức trực tiếp từ ComboBox
                TimeSpan? gioToChucFromCombo = GetGioToChucFromCombo();
                if (gioToChucFromCombo.HasValue)
                {
                    isTrong = _datSanhBLL.KiemTraSanhTrong(
                        sanhId.Value,
                        gioToChucFromCombo.Value,
                        ngayToChuc,
                        out errorMessage
                    );
                }
                else
                {
                    errorMessage = "Vui lòng chọn giờ tổ chức tiệc!";
                    isTrong = false;
                }

                DataRow sanhInfo = _datSanhBLL.LayThongTinSanh(sanhId.Value);
                string tenSanh = sanhInfo != null ? sanhInfo["ten_sanh"].ToString() : "sảnh";
                int sucChua = sanhInfo != null ? Convert.ToInt32(sanhInfo["suc_chua"]) : 0;

                int soKhachDuKien = 0;
                if (!string.IsNullOrWhiteSpace(txtSoKhachDuKien.Text))
                {
                    if (int.TryParse(txtSoKhachDuKien.Text.Trim(), out soKhachDuKien) && soKhachDuKien > 0)
                    {
                        if (soKhachDuKien > sucChua)
                        {
                            panelTrangThaiSanh.Text = $"Cảnh báo: Số khách dự kiến ({soKhachDuKien:N0}) vượt quá sức chứa của sảnh ({sucChua:N0} khách)";
                            panelTrangThaiSanh.ForeColor = Color.FromArgb(169, 68, 66);
                            panelTrangThaiSanh.Style = Sunny.UI.UIStyle.Red;
                            return;
                        }
                    }
                }

                if (isTrong)
                {
                    if (soKhachDuKien > 0)
                    {
                        panelTrangThaiSanh.Text = $"Sảnh \"{tenSanh}\" còn trống trong thời gian này ({soKhachDuKien:N0}/{sucChua:N0} khách)";
                    }
                    else
                    {
                        panelTrangThaiSanh.Text = $"Sảnh \"{tenSanh}\" còn trống trong thời gian này (Sức chứa: {sucChua:N0} khách)";
                    }
                    panelTrangThaiSanh.ForeColor = Color.FromArgb(21, 92, 51);
                    panelTrangThaiSanh.Style = Sunny.UI.UIStyle.Green;
                }
                else
                {
                    panelTrangThaiSanh.Text = !string.IsNullOrWhiteSpace(errorMessage) && errorMessage.Contains("đã được đặt")
                        ? "Sảnh đã được đặt trong thời gian này"
                        : (string.IsNullOrWhiteSpace(errorMessage) ? "Sảnh không còn trống trong thời gian này" : errorMessage);
                    panelTrangThaiSanh.ForeColor = Color.FromArgb(169, 68, 66);
                    panelTrangThaiSanh.Style = Sunny.UI.UIStyle.Red;
                }
            }
            catch (Exception ex)
            {
                panelTrangThaiSanh.Text = $"Lỗi: {ex.Message}";
                panelTrangThaiSanh.ForeColor = Color.Gray;
                panelTrangThaiSanh.Style = Sunny.UI.UIStyle.Gray;
            }
        }

        // Tính toán giá tiền dựa trên phí sảnh, gói tiệc và số bàn
        private void TinhToanGia()
        {
            try
            {
                _phiSanh = 0;
                _giaGoi = 0;
                
                int? sanhId = GetComboBoxValue(cbbSanh, "sanh_id");
                if (sanhId.HasValue)
                {
                    DataRow sanhInfo = _datSanhBLL.LayThongTinSanh(sanhId.Value);
                    if (sanhInfo != null)
                    {
                        _phiSanh = Convert.ToDecimal(sanhInfo["phi_thue_cb"]);
                    }
                }
                
                if (_selectedGoiId.HasValue)
                {
                    int soBan = 0;
                    bool hasSoBan = false;
                    
                    if (_soBanDuKien.HasValue && _soBanDuKien.Value > 0)
                    {
                        soBan = _soBanDuKien.Value;
                        hasSoBan = true;
                    }
                    else if (txtSoBanDuKien != null && !string.IsNullOrWhiteSpace(txtSoBanDuKien.Text))
                    {
                        if (int.TryParse(txtSoBanDuKien.Text.Trim(), out soBan) && soBan > 0)
                        {
                            hasSoBan = true;
                            _soBanDuKien = soBan;
                        }
                    }
                    
                    DataRow goiInfo = _goiTiecBLL.GetGoiTiecById(_selectedGoiId.Value);
                    if (goiInfo != null)
                    {
                        decimal giaCoBan = Convert.ToDecimal(goiInfo["gia_co_ban"]);
                        
                        if (hasSoBan && soBan > 0)
                        {
                            _giaGoi = giaCoBan * soBan;
                        }
                        else
                        {
                            _giaGoi = giaCoBan;
                        }
                    }
                }
                
                decimal tamTinh = _phiSanh + _giaGoi;
                // VAT cho tiệc cưới là 10%
                decimal vat = tamTinh * 0.10m;
                decimal phiDichVu = tamTinh * 0.05m;
                _tongTien = tamTinh + vat + phiDichVu;
    
                CapNhatHienThiGia(_phiSanh, _giaGoi, tamTinh, vat, phiDichVu, _tongTien);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tính toán giá: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Cập nhật hiển thị giá tiền vào các label
        private void CapNhatHienThiGia(decimal phiSanh, decimal giaGoi, decimal tamTinh, decimal vat, decimal phiDichVu, decimal tongCong)
        {
            try
            {
                Label lblPhiSanh = this.Controls.Find("lbPhiSanh", true).FirstOrDefault() as Label;
                Label lblGiaGoi = this.Controls.Find("lbGoiTiec", true).FirstOrDefault() as Label;
                Label lblTamTinh = this.Controls.Find("lbTamTinh", true).FirstOrDefault() as Label;
                Label lblVAT = this.Controls.Find("lbPhiVAT", true).FirstOrDefault() as Label;
                Label lblPhiDichVu = this.Controls.Find("lbPhiDV", true).FirstOrDefault() as Label;
                Label lblTongCong = this.Controls.Find("lbTongCong", true).FirstOrDefault() as Label;
                Label lblNameGoiTiec = this.Controls.Find("lbNameGoiTiec", true).FirstOrDefault() as Label 
                    ?? this.Controls.Find("label33", true).FirstOrDefault() as Label;
                
                if (lblPhiSanh != null)
                    lblPhiSanh.Text = FormatTien(phiSanh);
                    
                if (lblNameGoiTiec != null)
                {
                    int soBan = 0;
                    if (_soBanDuKien.HasValue && _soBanDuKien.Value > 0)
                    {
                        soBan = _soBanDuKien.Value;
                    }
                    else if (txtSoBanDuKien != null && !string.IsNullOrWhiteSpace(txtSoBanDuKien.Text))
                    {
                        if (int.TryParse(txtSoBanDuKien.Text.Trim(), out soBan) && soBan > 0)
                        {
                            // OK
                        }
                    }
                    
                    if (soBan > 0)
                    {
                        lblNameGoiTiec.Text = $"Gói tiệc ({soBan} bàn):";
                    }
                    else
                    {
                        lblNameGoiTiec.Text = "Gói tiệc:";
                    }
                }
                    
                if (lblGiaGoi != null)
                    lblGiaGoi.Text = FormatTien(giaGoi);
                    
                if (lblTamTinh != null)
                    lblTamTinh.Text = FormatTien(tamTinh);
                    
                if (lblVAT != null)
                    lblVAT.Text = FormatTien(vat);
                    
                if (lblPhiDichVu != null)
                    lblPhiDichVu.Text = FormatTien(phiDichVu);
                    
                if (lblTongCong != null)
                    lblTongCong.Text = FormatTien(tongCong);

                if (thuTuSanh == 3)
                {
                    // Tính toán số tiền cọc đợt 1 (20% tổng giá trị)
                    decimal tienCocDot1 = tongCong * 0.20m;
                    if (txtSoTienCoc_Dot1 != null)
                    {
                        txtSoTienCoc_Dot1.Text = FormatTien(tienCocDot1);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi cập nhật hiển thị giá: {ex.Message}");
            }
        }
        
        // Format số tiền với dấu phẩy ngăn cách và ký hiệu đồng
        private string FormatTien(decimal amount)
        {
            return amount.ToString("#,##0") + " ₫";
        }

        // Validation bước 1 - Kiểm tra thông tin đặt sảnh
        private bool ValidateBuoc1(out string errorMessage)
        {
            errorMessage = string.Empty;

            int? chiNhanhId = GetComboBoxValue(cbbChiNhanh, "chi_nhanh_id");
            if (!chiNhanhId.HasValue)
            {
                errorMessage = "Vui lòng chọn chi nhánh!";
                return false;
            }

            int? sanhId = GetComboBoxValue(cbbSanh, "sanh_id");
            if (!sanhId.HasValue)
            {
                errorMessage = "Vui lòng chọn sảnh!";
                return false;
            }

            if (cbbGioToChuc.SelectedItem == null)
            {
                errorMessage = "Vui lòng chọn giờ tổ chức tiệc!";
                return false;
            }

            DateTime ngayToChuc = dateNgayToChuc.Value;
            if (ngayToChuc.Date < DateTime.Now.Date)
            {
                errorMessage = "Ngày tổ chức không được ở quá khứ!";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtSoBanDuKien.Text))
            {
                if (!int.TryParse(txtSoBanDuKien.Text.Trim(), out int soBan) || soBan <= 0)
                {
                    errorMessage = "Số bàn dự kiến phải là số nguyên dương!";
                    return false;
                }
            }

            int? caId = null;
            TimeSpan? gioToChucFromCombo = GetGioToChucFromCombo();
            if (gioToChucFromCombo.HasValue)
            {
                string checkError;
                if (!_datSanhBLL.KiemTraSanhTrong(sanhId.Value, gioToChucFromCombo.Value, ngayToChuc, out checkError))
                {
                    errorMessage = checkError;
                    return false;
                }
            }
            else
            {
                errorMessage = "Vui lòng chọn giờ tổ chức tiệc!";
                return false;
            }

            return true;
        }

        // Validation bước 2 - Kiểm tra thông tin khách hàng
        private bool ValidateBuoc2(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                errorMessage = "Vui lòng nhập số điện thoại!";
                return false;
            }

            string sdt = txtSDT.Text.Trim();
            
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^[0-9]{10,11}$"))
            {
                errorMessage = "Số điện thoại không hợp lệ! Vui lòng nhập 10-11 chữ số.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                errorMessage = "Vui lòng nhập tên khách hàng!";
                return false;
            }

            string tenKH = txtTenKH.Text.Trim();
            if (tenKH.Length < 2)
            {
                errorMessage = "Tên khách hàng phải có ít nhất 2 ký tự!";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string email = txtEmail.Text.Trim();
                try
                {
                    var emailAddress = new System.Net.Mail.MailAddress(email);
                    if (emailAddress.Address != email)
                    {
                        errorMessage = "Email không hợp lệ!";
                        return false;
                    }
                }
                catch
                {
                    errorMessage = "Email không hợp lệ!";
                    return false;
                }
            }

            return true;
        }

        // Lưu thông tin khách hàng
        private int LuuThongTinKhachHang()
        {
            try
            {
                string hoTen = txtTenKH.Text.Trim();
                string sdt = txtSDT.Text.Trim();
                string email = txtEmail.Text?.Trim() ?? "";
                string ghiChu = txtGhiChuKH.Text?.Trim() ?? "";

                DataTable dt = _khachHangBLL.TimKhachHangTheoSdt(sdt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int khachHangId = Convert.ToInt32(dt.Rows[0]["khach_hang_id"]);
                    
                    bool updated = _khachHangBLL.CapNhatKhachHang(khachHangId, hoTen,sdt, email, ghiChu);
                    if (!updated)
                    {
                        throw new Exception("Không thể cập nhật thông tin khách hàng!");
                    }
                    
                    return khachHangId;
                }

                // Khách hàng chưa tồn tại, tạo mới
                int khachHangIdNew = _khachHangBLL.TaoKhachHang(hoTen, sdt, email, ghiChu);
                
                if (khachHangIdNew <= 0)
                {
                    throw new Exception("Không thể tạo khách hàng mới!");
                }

                return khachHangIdNew;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu thông tin khách hàng: {ex.Message}", ex);
            }
        }

        // Sự kiện click label24
        private void label24_Click(object sender, EventArgs e)
        {

        }

        // Sự kiện click label16
        private void label16_Click(object sender, EventArgs e)
        {

        }

        // Sự kiện click nút Thoát - đóng form
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Sự kiện click nút Tiếp tục - xử lý chuyển bước và validation
        private void btnTiepTuc_Click(object sender, EventArgs e)
        {
            if (thuTuSanh == 1)
            {
                string errorMessage;
                if (!ValidateBuoc1(out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtSoBanDuKien != null && !string.IsNullOrWhiteSpace(txtSoBanDuKien.Text))
                {
                    if (int.TryParse(txtSoBanDuKien.Text.Trim(), out int soBan) && soBan > 0)
                    {
                        _soBanDuKien = soBan;
                    }
                }
                
                panelGoivaMon.Location = panelDatSanh.Location;
                btnQuayLai.Visible = true;
                panelDatSanh.Visible = false;
                panelGoivaMon.Visible = true;
                panelHopDong.Visible = false;

                panelBuoc1.Style = Sunny.UI.UIStyle.Green;
                panelBuoc2.Style = Sunny.UI.UIStyle.Blue;
                panelBuoc3.Style = Sunny.UI.UIStyle.Gray;
                thuTuSanh = 2;
                
                lbSoBuocHienTai.Text = "Bước 2/3: Gói tiệc & Thực đơn";
                picSuccess1.Visible = true;

                panelGoivaMon.Visible = true;
                if (panelDanhSachGoiTiec != null)
                {
                    panelDanhSachGoiTiec.Visible = true;
                }
                
                LoadGoiTiec();
                TinhToanGia();
                
                this.BeginInvoke(new Action(() =>
                {
                    Application.DoEvents();
                    CapNhatAutoScrollPanel1();
                }));
            }
            else if (thuTuSanh == 2)
            {
                string errorMessage;
                if (!ValidateBuoc2(out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    _khachHangId = LuuThongTinKhachHang();
                    if (_khachHangId == null || _khachHangId <= 0)
                    {
                        MessageBox.Show("Không thể lưu thông tin khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi lưu thông tin khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                panelHopDong.Location = panelDatSanh.Location;
                btnQuayLai.Visible = true;
                panelDatSanh.Visible = false;
                panelGoivaMon.Visible = false;
                panelHopDong.Visible = true;

                panelBuoc1.Style = Sunny.UI.UIStyle.Green;
                panelBuoc2.Style = Sunny.UI.UIStyle.Green;
                panelBuoc3.Style = Sunny.UI.UIStyle.Blue;
                thuTuSanh = 3;
                
                lbSoBuocHienTai.Text = "Bước 3/3: Hợp đồng & Thanh toán";
                picSuccess2.Visible = true;

                // Tính lại giá trước khi cập nhật hợp đồng
                TinhToanGia();
                CapNhatThongTinHopDong();
                
                this.BeginInvoke(new Action(() => CapNhatAutoScrollPanel1()));
            }
            else if (thuTuSanh == 3)
            {
                string errorMessage;
                if (!ValidateBuoc3(out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LuuDonDatSanh();
            }
        }

        // Cập nhật thông tin hiển thị trong panel hợp đồng
        private void CapNhatThongTinHopDong()
        {
            try
            {
                // Tạo số hợp đồng tự động
                string soHopDong = $"HD-{DateTime.Now:yyyy}-{DateTime.Now:MMdd}{DateTime.Now:HHmmss}";
                if (txtSoHopDong != null)
                {
                    txtSoHopDong.Text = soHopDong;
                    txtSoHopDong.ReadOnly = true;
                }

                if (dateNgayKy != null)
                {
                    dateNgayKy.Value = DateTime.Now;
                    dateNgayKy.Enabled = false;
                }

                // Tính toán số tiền cọc đợt 1 (20% tổng giá trị)
                decimal tienCocDot1 = _tongTien * 0.20m;
                if (txtSoTienCoc_Dot1 != null)
                {
                    txtSoTienCoc_Dot1.Text = FormatTien(tienCocDot1);
                    txtSoTienCoc_Dot1.ReadOnly = true;
                }
                
                if (cbXacNhanKy != null)
                {
                    cbXacNhanKy.Checked = false;
                }
                
                // Reset phương thức thanh toán và ẩn các controls
                _phuongThucThanhToan = null;
                CapNhatHienThiPhuongThucThanhToan();
                
                // Ẩn phần thanh toán còn lại ban đầu
                CapNhatHienThiThanhToanConLai();

                // Load điều khoản hợp đồng (nếu chưa có)
                if (richTxt_DieuKhoanHD != null && string.IsNullOrWhiteSpace(richTxt_DieuKhoanHD.Text))
                {
                    LoadDieuKhoanHopDong();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật thông tin hợp đồng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load điều khoản hợp đồng mặc định
        private void LoadDieuKhoanHopDong()
        {
            try
            {
                string dieuKhoan = @"ĐIỀU KHOẢN HỢP ĐỒNG DỊCH VỤ TIỆC CƯỚI

1. PHẠM VI CÔNG VIỆC
- Nhà hàng cam kết cung cấp dịch vụ tiệc cưới theo đúng gói và thực đơn đã thỏa thuận
- Bố trí sảnh, trang trí, âm thanh ánh sáng theo yêu cầu
- Phục vụ ăn uống đúng số lượng khách đã đặt

2. THANH TOÁN & ĐẶT CỌC
- Cọc đợt 1: 20% tổng giá trị - trong vòng 7 ngày kể từ ngày ký hợp đồng
- Thanh toán còn lại: 70% - trước sự kiện 3 ngày
- 10% còn lại thanh toán sau khi hoàn tất sự kiện

3. CHÍNH SÁCH HỦY & HOÀN TRẢ
- Hủy trước 15 ngày: Hoàn 100% tiền cọc
- Hủy trước 7 ngày: Hoàn 50% tiền cọc
- Hủy trước 3 ngày: Không hoàn tiền cọc

4. ĐIỀU KHOẢN KHÁC
- Khách hàng có trách nhiệm bảo vệ tài sản của nhà hàng
- Mọi thay đổi về số lượng khách phải thông báo trước 7 ngày
- Nhà hàng không chịu trách nhiệm về tài sản cá nhân của khách mời";

                if (richTxt_DieuKhoanHD != null)
                {
                    richTxt_DieuKhoanHD.Text = dieuKhoan;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi load điều khoản hợp đồng: {ex.Message}");
            }
        }

        // Validation bước 3 - Xác nhận cuối cùng trước khi lưu
        private bool ValidateBuoc3(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!_khachHangId.HasValue || _khachHangId <= 0)
            {
                errorMessage = "Thông tin khách hàng không hợp lệ! Vui lòng quay lại bước 2.";
                return false;
            }

            int? chiNhanhId = GetComboBoxValue(cbbChiNhanh, "chi_nhanh_id");
            int? sanhId = GetComboBoxValue(cbbSanh, "sanh_id");
            
            if (!chiNhanhId.HasValue || !sanhId.HasValue)
            {
                errorMessage = "Thông tin đặt sảnh không đầy đủ! Vui lòng quay lại bước 1.";
                return false;
            }

            if (cbbGioToChuc.SelectedItem == null)
            {
                errorMessage = "Chưa chọn giờ tổ chức tiệc! Vui lòng quay lại bước 1.";
                return false;
            }

            if (dateNgayToChuc.Value.Date < DateTime.Now.Date)
            {
                errorMessage = "Ngày tổ chức không được ở quá khứ!";
                return false;
            }

            if (cbXacNhanKy == null || !cbXacNhanKy.Checked)
            {
                errorMessage = "Vui lòng xác nhận đã ký hợp đồng!";
                return false;
            }

            // Không bắt buộc chọn phương thức thanh toán
            // Nếu không chọn = không cọc, trạng thái sẽ là "CHỜ XÁC NHẬN"
            // Nếu có chọn = có cọc, trạng thái sẽ là "ĐÃ CỌC"
            
            return true;
        }

        // Lưu đơn đặt sảnh vào database
        private void LuuDonDatSanh()
        {
            try
            {
                int? chiNhanhId = GetComboBoxValue(cbbChiNhanh, "chi_nhanh_id");
                int? sanhId = GetComboBoxValue(cbbSanh, "sanh_id");
                
                TimeSpan? gioToChuc = GetGioToChucFromCombo();
                if (!chiNhanhId.HasValue || !sanhId.HasValue || !gioToChuc.HasValue)
                {
                    MessageBox.Show("Vui lòng kiểm tra lại thông tin đặt sảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime ngayToChuc = dateNgayToChuc.Value;
                
                string checkError;
                if (!_datSanhBLL.KiemTraSanhTrong(sanhId.Value, gioToChuc.Value, ngayToChuc, out checkError))
                {
                    MessageBox.Show(checkError, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Gán ca_id cố định: Index 0 (10:30) -> ca_id = 1, Index 1 (17:30) -> ca_id = 2
                int? caId = cbbGioToChuc.SelectedIndex == 0 ? 1 : 2;
                
                int? soBanDuKien = null;
                if (!string.IsNullOrWhiteSpace(txtSoBanDuKien.Text))
                {
                    int soBan;
                    if (int.TryParse(txtSoBanDuKien.Text.Trim(), out soBan) && soBan > 0)
                    {
                        soBanDuKien = soBan;
                    }
                }

                string ghiChu = txtGhiChu.Text?.Trim() ?? "";

                string trangThai = "CHỜ XÁC NHẬN";
                if (!string.IsNullOrEmpty(_phuongThucThanhToan))
                {
                    trangThai = "ĐÃ CỌC";
                }

                // Tạo đơn đặt sảnh
                string errorMessage;
                int datSanhId = _datSanhBLL.TaoDatSanh(
                    chiNhanhId.Value, sanhId.Value, caId.Value, ngayToChuc,
                    _khachHangId.Value, soBanDuKien, _selectedGoiId, ghiChu, 
                    gioToChuc, trangThai, out errorMessage);

                if (datSanhId <= 0)
                {
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _datSanhId = datSanhId;

                // Tạo hợp đồng
                string soHopDong = txtSoHopDong.Text?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(soHopDong))
                {
                    soHopDong = $"HD-{DateTime.Now:yyyy}-{DateTime.Now:MMdd}{DateTime.Now:HHmmss}";
                }

                DateTime ngayKy = dateNgayKy.Value;
                string dieuKhoan = richTxt_DieuKhoanHD?.Text ?? "";

                int hopDongId = _datSanhBLL.TaoHopDong(soHopDong, datSanhId, ngayKy, _tongTien, dieuKhoan, out errorMessage);
                if (hopDongId <= 0)
                {
                    MessageBox.Show($"Lỗi tạo hợp đồng: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lưu chi tiết món ăn vào hop_dong_ct_mon từ gói tiệc
                if (_selectedGoiId.HasValue)
                {
                    DataTable dtMon = _goiTiecBLL.GetMonAnByGoiId(_selectedGoiId.Value);
                    if (dtMon != null && dtMon.Rows.Count > 0)
                    {
                        foreach (DataRow monRow in dtMon.Rows)
                        {
                            try
                            {
                                int monId = Convert.ToInt32(monRow["mon_id"]);
                                decimal soLuong = Convert.ToDecimal(monRow["so_luong"]);
                                
                                // Tính số lượng theo số bàn
                                if (soBanDuKien.HasValue && soBanDuKien.Value > 0)
                                {
                                    soLuong = soLuong * soBanDuKien.Value;
                                }
                                
                                // Lấy đơn giá từ bảng mon_an
                                decimal donGia = 0;
                                if (monRow["don_gia"] != DBNull.Value)
                                {
                                    donGia = Convert.ToDecimal(monRow["don_gia"]);
                                }
                                
                                bool success = _datSanhBLL.LuuChiTietMon(hopDongId, monId, soLuong, donGia, out errorMessage);
                                if (!success)
                                {
                                    MessageBox.Show($"Lỗi lưu chi tiết món: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi xử lý chi tiết món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                // Lưu chi tiết dịch vụ vào hop_dong_ct_dv từ gói tiệc
                if (_selectedGoiId.HasValue)
                {
                    DataTable dtDichVu = _goiTiecBLL.GetDichVuByGoiId(_selectedGoiId.Value);
                    if (dtDichVu != null && dtDichVu.Rows.Count > 0)
                    {
                        foreach (DataRow dvRow in dtDichVu.Rows)
                        {
                            try
                            {
                                int dvId = Convert.ToInt32(dvRow["dv_id"]);
                                decimal soLuong = 1; // Mặc định số lượng là 1 cho dịch vụ
                                decimal donGia = 0;
                                if (dvRow["don_gia"] != DBNull.Value)
                                {
                                    donGia = Convert.ToDecimal(dvRow["don_gia"]);
                                }
                                
                                bool success = _datSanhBLL.LuuChiTietDichVu(hopDongId, dvId, soLuong, donGia, out errorMessage);
                                if (!success)
                                {
                                    MessageBox.Show($"Lỗi lưu chi tiết dịch vụ: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi xử lý chi tiết dịch vụ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                // Lưu tiền cọc vào hop_dong_coc nếu đã chọn phương thức thanh toán
                if (!string.IsNullOrEmpty(_phuongThucThanhToan))
                {
                    // Tự động lấy số tiền cọc (20% tổng giá trị)
                    decimal tienCoc = _tongTien * 0.20m;
                    
                    string tienCocText = txtSoTienCoc_Dot1.Text?.Replace(" ₫", "").Replace(".", "").Replace(",", "") ?? "0";
                    if (decimal.TryParse(tienCocText, out decimal tienCocParsed) && tienCocParsed > 0)
                    {
                        tienCoc = tienCocParsed;
                    }
                    
                    // Lưu vào bảng hop_dong_coc với phương thức thanh toán đã chọn
                    int cocId = _datSanhBLL.LuuTienCoc(hopDongId, tienCoc, DateTime.Now, _phuongThucThanhToan, "Cọc tối thiểu 20% tổng giá trị", out errorMessage);
                    if (cocId <= 0)
                    {
                        MessageBox.Show($"Lỗi lưu tiền cọc: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Tạo hóa đơn cho tiệc cưới ngay sau khi tạo hợp đồng
                int hoaDonId = _datSanhBLL.TaoHoaDonKhiDaCoc(datSanhId, out errorMessage);
                if (hoaDonId <= 0)
                {
                    MessageBox.Show($"Đã tạo đơn đặt sảnh và hợp đồng thành công!\nTuy nhiên, không thể tạo hóa đơn: {errorMessage}", 
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Lưu thanh toán còn lại vào hop_dong_tt nếu checkbox được check
                //if (cbTienThanhToanConLai != null && cbTienThanhToanConLai.Checked)
                //{
                //    decimal tienConLai = _tongTien * 0.70m;
                //    string tienConLaiText = txtSoTienConLai.Text?.Replace(" ₫", "").Replace(".", "").Replace(",", "") ?? "0";
                //    if (decimal.TryParse(tienConLaiText, out decimal tienConLaiParsed))
                //    {
                //        tienConLai = tienConLaiParsed;
                //    }
                    
                //    // Lưu vào bảng hop_dong_tt với hình thức mặc định là "Chuyển khoản"
                //    int ttId = _datSanhBLL.LuuThanhToan(hopDongId, tienConLai, DateTime.Now, "Chuyển khoản", "Thanh toán còn lại", out errorMessage);
                //    if (ttId <= 0)
                //    {
                //        MessageBox.Show($"Lỗi lưu thanh toán: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return;
                //    }
                //}

                DialogResult result = MessageBox.Show($"Đơn đặt sảnh và hợp đồng đã được tạo thành công!\nMã đơn: DS{datSanhId:D6}\nSố hợp đồng: {soHopDong}\n\nBạn có muốn in hợp đồng ngay không?", 
                    "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        UI.Reporting.rptHopDong report = new UI.Reporting.rptHopDong();
                        
                        // Cố gắng gán tham số cho báo cáo
                        if (report.Parameters["HopDongId"] != null)
                        {
                            report.Parameters["HopDongId"].Value = hopDongId;
                            report.RequestParameters = false;
                        }
                        else
                        {
                            // Nếu không tìm thấy tham số báo cáo, thử gán trực tiếp vào query của datasource
                            var sqlDataSource = report.DataSource as DevExpress.DataAccess.Sql.SqlDataSource;
                            if (sqlDataSource != null && sqlDataSource.Queries.Count > 0)
                            {
                                var query = sqlDataSource.Queries[0] as DevExpress.DataAccess.Sql.StoredProcQuery;
                                if (query != null && query.Parameters.Count > 0)
                                {
                                    query.Parameters[0].Value = hopDongId;
                                }
                            }
                        }

                        DevExpress.XtraReports.UI.ReportPrintTool tool = new DevExpress.XtraReports.UI.ReportPrintTool(report);
                        tool.ShowPreview();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi in hợp đồng: {ex.Message}", "Lỗi In ấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu đơn đặt sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện thay đổi checkbox đặt cọc
        private void CbTienCocDot1_CheckedChanged(object sender, EventArgs e)
        {
            CapNhatHienThiThanhToanConLai();
        }

        private void CbXacNhanKy_CheckedChanged(object sender, EventArgs e)
        {
            if (cbXacNhanKy != null && cbXacNhanKy.Checked)
            {
                if (dateNgayKy != null)
                {
                    dateNgayKy.Value = DateTime.Now;
                }
            }
        }


        private void CapNhatHienThiThanhToanConLai()
        {

        }

        private void CapNhatHienThiPhuongThucThanhToan()
        {
            if (btnHienThiQR != null)
                btnHienThiQR.Visible = false;

            if (_phuongThucThanhToan == "Chuyển khoản")
            {
                if (btnHienThiQR != null)
                {
                    btnHienThiQR.Visible = true;
                }
            }
        }

        private void BtnTienMat_Click(object sender, EventArgs e)
        {
            if (_phuongThucThanhToan == "Tiền mặt")
            {
                _phuongThucThanhToan = null;
                btnTienMat.BorderThickness = 1;
                btnTienMat.BorderColor = Color.FromArgb(224, 224, 224);
            }
            else
            {
                _phuongThucThanhToan = "Tiền mặt";
                btnTienMat.BorderThickness = 2;
                btnTienMat.BorderColor = Color.FromArgb(94, 148, 255);
            }
            
            btnThe.BorderThickness = 1;
            btnThe.BorderColor = Color.FromArgb(224, 224, 224);
            btnChuyenKhoan.BorderThickness = 1;
            btnChuyenKhoan.BorderColor = Color.FromArgb(224, 224, 224);
            
            CapNhatHienThiPhuongThucThanhToan();
        }

        private void BtnThe_Click(object sender, EventArgs e)
        {
            if (_phuongThucThanhToan == "Thẻ")
            {
                _phuongThucThanhToan = null;
                btnThe.BorderThickness = 1;
                btnThe.BorderColor = Color.FromArgb(224, 224, 224);
            }
            else
            {
                _phuongThucThanhToan = "Thẻ";
                btnThe.BorderThickness = 2;
                btnThe.BorderColor = Color.FromArgb(94, 148, 255);
            }
            
            btnTienMat.BorderThickness = 1;
            btnTienMat.BorderColor = Color.FromArgb(224, 224, 224);
            btnChuyenKhoan.BorderThickness = 1;
            btnChuyenKhoan.BorderColor = Color.FromArgb(224, 224, 224);
            
            CapNhatHienThiPhuongThucThanhToan();
        }

        private void BtnChuyenKhoan_Click(object sender, EventArgs e)
        {
            if (_phuongThucThanhToan == "Chuyển khoản")
            {
                _phuongThucThanhToan = null;
                btnChuyenKhoan.BorderThickness = 1;
                btnChuyenKhoan.BorderColor = Color.FromArgb(224, 224, 224);
            }
            else
            {
                _phuongThucThanhToan = "Chuyển khoản";
                btnChuyenKhoan.BorderThickness = 2;
                btnChuyenKhoan.BorderColor = Color.FromArgb(94, 148, 255);
            }
            
            btnTienMat.BorderThickness = 1;
            btnTienMat.BorderColor = Color.FromArgb(224, 224, 224);
            btnThe.BorderThickness = 1;
            btnThe.BorderColor = Color.FromArgb(224, 224, 224);
            
            CapNhatHienThiPhuongThucThanhToan();
        }

        // Event handler cho button Hiển thị QR
        private void BtnHienThiQR_Click(object sender, EventArgs e)
        {
            try
            {
                decimal tienCoc = _tongTien * 0.20m;
                
                var formQR = new Frm_QRThanhToan(tienCoc, "Cọc tối thiểu 20% tổng giá trị")
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                formQR.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị QR thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện click nút Quay lại - quay về bước trước
        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            if (thuTuSanh == 3)
            {
                panelDatSanh.Location = panelHopDong.Location;
                btnQuayLai.Visible = true;
                panelDatSanh.Visible = false;
                panelGoivaMon.Visible = true;
                panelHopDong.Visible = false;

                panelBuoc1.Style = Sunny.UI.UIStyle.Green;
                panelBuoc2.Style = Sunny.UI.UIStyle.Blue;
                panelBuoc3.Style = Sunny.UI.UIStyle.Gray;
                thuTuSanh = 2;

                lbSoBuocHienTai.Text = "Bước 2/3: Gói tiệc & Thực đơn";
                picSuccess2.Visible = false;

                this.BeginInvoke(new Action(() => CapNhatAutoScrollPanel1()));
            }
            else if (thuTuSanh == 2)
            {
                panelGoivaMon.Location = panelDatSanh.Location;
                btnQuayLai.Visible = false;
                panelDatSanh.Visible = true;
                panelGoivaMon.Visible = false;
                panelHopDong.Visible = false;

                panelBuoc1.Style = Sunny.UI.UIStyle.Blue;
                panelBuoc2.Style = Sunny.UI.UIStyle.Gray;
                panelBuoc3.Style = Sunny.UI.UIStyle.Gray;
                thuTuSanh = 1;
 
                lbSoBuocHienTai.Text = "Bước 1/3: Thông tin đặt sảnh";
                picSuccess1.Visible = false;

                this.BeginInvoke(new Action(() => CapNhatAutoScrollPanel1()));
            }
        }
    }
}
