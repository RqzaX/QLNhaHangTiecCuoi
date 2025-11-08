using BLL;
using QLNhaHangTiecCuoi.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using UI.Common;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_ChiTietDatSanh : RoundedBorderForm
    {
        private int _datSanhId;
        private DatSanhBLL _datSanhBLL;
        private GoiTiecBLL _goiTiecBLL;

        public Frm_ChiTietDatSanh(int datSanhId)
        {
            InitializeComponent();
            _datSanhId = datSanhId;
            _datSanhBLL = new DatSanhBLL();
            _goiTiecBLL = new GoiTiecBLL();

            CornerRadius = 15;
            BorderColor = Color.Black;
            BorderThickness = 2;
            ShowDropShadow = true;
            StartPosition = FormStartPosition.CenterParent;

            panelTongQuan.Location = new Point(7, 168);

            this.Load += Frm_ChiTietDatSanh_Load;
        }
    
        private void Frm_ChiTietDatSanh_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1036, 695);
            LoadChiTietDatSanh();
        }

        private void LoadChiTietDatSanh()
        {
            try
            {
                if (_datSanhId <= 0)
                {
                    MessageBox.Show("Mã đặt sảnh không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                DataRow datSanhInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                if (datSanhInfo == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin đặt sảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                HienThiThongTinTongQuan(datSanhInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void HienThiThongTinGoiVaThucDon(DataRow datSanhInfo)
        {
            if (datSanhInfo == null) return;

            try
            {
                int? goiId = null;
                if (datSanhInfo["goi_id"] != DBNull.Value && datSanhInfo["goi_id"] != null)
                {
                    if (datSanhInfo["goi_id"] is int intGoiId)
                        goiId = intGoiId;
                    else if (int.TryParse(datSanhInfo["goi_id"].ToString(), out int parsedGoiId))
                        goiId = parsedGoiId;
                }

                decimal? giaCoBan = null;
                if (datSanhInfo["gia_co_ban"] != DBNull.Value && datSanhInfo["gia_co_ban"] != null)
                {
                    if (datSanhInfo["gia_co_ban"] is decimal decGia)
                        giaCoBan = decGia;
                    else if (decimal.TryParse(datSanhInfo["gia_co_ban"].ToString(), out decimal parsedGia))
                        giaCoBan = parsedGia;
                }

                int? soBan = null;
                if (datSanhInfo["so_ban_du_kien"] != DBNull.Value && datSanhInfo["so_ban_du_kien"] != null)
                {
                    if (datSanhInfo["so_ban_du_kien"] is int intBan)
                        soBan = intBan;
                    else if (int.TryParse(datSanhInfo["so_ban_du_kien"].ToString(), out int parsedBan))
                        soBan = parsedBan;
                }

                // Hiển thị thông tin gói tiệc
                if (lbGiaCoBan != null && giaCoBan.HasValue)
                    lbGiaCoBan.Text = $"{giaCoBan.Value:N0} ₫";

                if (lbSoBan != null && soBan.HasValue)
                    lbSoBan.Text = $"{soBan.Value} bàn";

                if (lbTextPhiSanh != null && soBan.HasValue && giaCoBan.HasValue)
                    lbTextPhiSanh.Text = $"Phí sảnh ({soBan.Value} bàn x {giaCoBan.Value:N0} đ)";

                decimal phiSanh = 0;
                if (soBan.HasValue && giaCoBan.HasValue)
                    phiSanh = soBan.Value * giaCoBan.Value;

                if (lbSoTienPhiSanh != null)
                {
                    lbSoTienPhiSanh.AutoSize = false;
                    lbSoTienPhiSanh.Width = 150;
                    lbSoTienPhiSanh.Location = new Point(342, lbSoTienPhiSanh.Location.Y);
                    lbSoTienPhiSanh.Text = $"{phiSanh:N0} ₫";
                    lbSoTienPhiSanh.TextAlign = ContentAlignment.MiddleRight;
                }

                // Load danh sách món ăn và dịch vụ
                decimal tongMonAn = 0;
                decimal tongDichVu = 0;

                if (goiId.HasValue && goiId.Value > 0)
                {
                    // Load món ăn
                    DataTable dtMonAn = _goiTiecBLL.GetMonAnByGoiId(goiId.Value);
                    if (dtMonAn != null && dtMonAn.Rows.Count > 0 && dgvDanhSachMonAn != null)
                    {
                        if (!dtMonAn.Columns.Contains("thanh_tien"))
                            dtMonAn.Columns.Add("thanh_tien", typeof(decimal));

                        dgvDanhSachMonAn.DataSource = null;
                        dgvDanhSachMonAn.Columns.Clear();

                        dgvDanhSachMonAn.AutoGenerateColumns = false;
                        dgvDanhSachMonAn.AllowUserToAddRows = false;
                        dgvDanhSachMonAn.ColumnHeadersVisible = true;
                        dgvDanhSachMonAn.ColumnHeadersHeight = 35;
                        dgvDanhSachMonAn.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        dgvDanhSachMonAn.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        dgvDanhSachMonAn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        
                        Font cellFont = new Font("Segoe UI", 10F, FontStyle.Regular);
                        Font headerFont = new Font("Segoe UI", 10F, FontStyle.Bold);
                        dgvDanhSachMonAn.DefaultCellStyle.Font = cellFont;
                        dgvDanhSachMonAn.ColumnHeadersDefaultCellStyle.Font = headerFont;

                        dgvDanhSachMonAn.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = "ten_mon",
                            HeaderText = "Tên món",
                            DataPropertyName = "ten_mon",
                            Width = 200,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                            MinimumWidth = 150,
                            DefaultCellStyle = new DataGridViewCellStyle 
                            { 
                                Font = cellFont,
                                WrapMode = DataGridViewTriState.True
                            }
                        });

                        dgvDanhSachMonAn.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = "so_luong",
                            HeaderText = "Số lượng",
                            DataPropertyName = "so_luong",
                            Width = 100,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            DefaultCellStyle = new DataGridViewCellStyle 
                            { 
                                Font = cellFont,
                                Alignment = DataGridViewContentAlignment.MiddleRight,
                                Format = "0"
                            }
                        });

                        dgvDanhSachMonAn.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = "don_gia",
                            HeaderText = "Đơn giá",
                            DataPropertyName = "don_gia",
                            Width = 120,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            DefaultCellStyle = new DataGridViewCellStyle 
                            { 
                                Format = "N0", 
                                Font = cellFont,
                                Alignment = DataGridViewContentAlignment.MiddleRight
                            }
                        });

                        dgvDanhSachMonAn.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = "thanh_tien",
                            HeaderText = "Thành tiền",
                            DataPropertyName = "thanh_tien",
                            Width = 120,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            DefaultCellStyle = new DataGridViewCellStyle 
                            { 
                                Format = "N0", 
                                Font = cellFont,
                                Alignment = DataGridViewContentAlignment.MiddleRight
                            }
                        });

                        // Tính số lượng dựa trên số bàn (mỗi bàn = 10 khách)
                        int soLuongKhach = soBan.HasValue ? soBan.Value * 10 : 0;
                        foreach (DataRow row in dtMonAn.Rows)
                        {
                            decimal donGia = 0;
                            if (row["don_gia"] != DBNull.Value)
                                donGia = Convert.ToDecimal(row["don_gia"]);

                            decimal soLuongMon = 0;
                            if (row["so_luong"] != DBNull.Value)
                            {
                                soLuongMon = Convert.ToDecimal(row["so_luong"]);
                                row["so_luong"] = soLuongMon * soLuongKhach;
                            }

                            // Tính lại thành tiền
                            decimal thanhTien = soLuongMon * soLuongKhach * donGia;
                            row["thanh_tien"] = thanhTien;
                            tongMonAn += thanhTien;
                        }

                        dgvDanhSachMonAn.DataSource = dtMonAn;
                    }

                    // Load dịch vụ
                    DataTable dtDichVu = _goiTiecBLL.GetDichVuByGoiId(goiId.Value);
                    if (dtDichVu != null && dtDichVu.Rows.Count > 0 && dgvDanhSachDichVu != null)
                    {
                        // Thêm cột số lượng và thanh_tien nếu chưa có
                        if (!dtDichVu.Columns.Contains("so_luong"))
                            dtDichVu.Columns.Add("so_luong", typeof(decimal));
                        if (!dtDichVu.Columns.Contains("thanh_tien"))
                            dtDichVu.Columns.Add("thanh_tien", typeof(decimal));

                        dgvDanhSachDichVu.DataSource = null;
                        dgvDanhSachDichVu.Columns.Clear();

                        dgvDanhSachDichVu.AutoGenerateColumns = false;
                        dgvDanhSachDichVu.AllowUserToAddRows = false;
                        dgvDanhSachDichVu.ColumnHeadersVisible = true;
                        dgvDanhSachDichVu.ColumnHeadersHeight = 35;
                        dgvDanhSachDichVu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        dgvDanhSachDichVu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        dgvDanhSachDichVu.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        
                        Font cellFont = new Font("Segoe UI", 10F, FontStyle.Regular);
                        Font headerFont = new Font("Segoe UI", 10F, FontStyle.Bold);
                        dgvDanhSachDichVu.DefaultCellStyle.Font = cellFont;
                        dgvDanhSachDichVu.ColumnHeadersDefaultCellStyle.Font = headerFont;

                        dgvDanhSachDichVu.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = "ten_dv",
                            HeaderText = "Tên dịch vụ",
                            DataPropertyName = "ten_dv",
                            Width = 200,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                            MinimumWidth = 150,
                            DefaultCellStyle = new DataGridViewCellStyle 
                            { 
                                Font = cellFont,
                                WrapMode = DataGridViewTriState.True
                            }
                        });

                        dgvDanhSachDichVu.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = "so_luong",
                            HeaderText = "Số lượng",
                            DataPropertyName = "so_luong",
                            Width = 100,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            DefaultCellStyle = new DataGridViewCellStyle 
                            { 
                                Font = cellFont,
                                Alignment = DataGridViewContentAlignment.MiddleRight,
                                Format = "0"
                            }
                        });

                        dgvDanhSachDichVu.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = "don_gia",
                            HeaderText = "Đơn giá",
                            DataPropertyName = "don_gia",
                            Width = 120,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            DefaultCellStyle = new DataGridViewCellStyle 
                            { 
                                Format = "N0", 
                                Font = cellFont,
                                Alignment = DataGridViewContentAlignment.MiddleRight
                            }
                        });

                        dgvDanhSachDichVu.Columns.Add(new DataGridViewTextBoxColumn
                        {
                            Name = "thanh_tien",
                            HeaderText = "Thành tiền",
                            DataPropertyName = "thanh_tien",
                            Width = 120,
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                            DefaultCellStyle = new DataGridViewCellStyle 
                            { 
                                Format = "N0", 
                                Font = cellFont,
                                Alignment = DataGridViewContentAlignment.MiddleRight
                            }
                        });

                        // Thêm cột số lượng và thành tiền cho dịch vụ
                        foreach (DataRow row in dtDichVu.Rows)
                        {
                            if (row["so_luong"] == DBNull.Value)
                                row["so_luong"] = 1;
                            if (row["don_gia"] != DBNull.Value && row["so_luong"] != DBNull.Value)
                            {
                                decimal donGia = Convert.ToDecimal(row["don_gia"]);
                                decimal soLuong = Convert.ToDecimal(row["so_luong"]);
                                row["thanh_tien"] = donGia * soLuong;
                                tongDichVu += donGia * soLuong;
                            }
                        }

                        dgvDanhSachDichVu.DataSource = dtDichVu;
                    }
                }

                if (lbTongMonAn != null)
                {
                    lbTongMonAn.AutoSize = false;
                    lbTongMonAn.Width = 150;
                    lbTongMonAn.Location = new Point(342, lbTongMonAn.Location.Y);
                    lbTongMonAn.Text = $"{tongMonAn:N0} ₫";
                    lbTongMonAn.TextAlign = ContentAlignment.MiddleRight;
                }

                if (lbTongDichVu != null)
                {
                    lbTongDichVu.AutoSize = false;
                    lbTongDichVu.Width = 150;
                    lbTongDichVu.Location = new Point(342, lbTongDichVu.Location.Y);
                    lbTongDichVu.Text = $"{tongDichVu:N0} ₫";
                    lbTongDichVu.TextAlign = ContentAlignment.MiddleRight;
                }

                decimal tamTinh = phiSanh + tongMonAn + tongDichVu;
                if (lbSoTienTamTinh != null)
                {
                    lbSoTienTamTinh.AutoSize = false;
                    lbSoTienTamTinh.Width = 150;
                    lbSoTienTamTinh.Location = new Point(342, lbSoTienTamTinh.Location.Y);
                    lbSoTienTamTinh.Text = $"{tamTinh:N0} ₫";
                    lbSoTienTamTinh.TextAlign = ContentAlignment.MiddleRight;
                }

                decimal vat = tamTinh * 0.08m;
                if (lbSoTienVAT != null)
                {
                    lbSoTienVAT.AutoSize = false;
                    lbSoTienVAT.Width = 150;
                    lbSoTienVAT.Location = new Point(342, lbSoTienVAT.Location.Y);
                    lbSoTienVAT.Text = $"{vat:N0} ₫";
                    lbSoTienVAT.TextAlign = ContentAlignment.MiddleRight;
                }

                decimal tongTien = tamTinh + vat;
                if (lbTongTien != null)
                {
                    lbTongTien.AutoSize = false;
                    lbTongTien.Width = 150;
                    lbTongTien.Location = new Point(342, lbTongTien.Location.Y);
                    lbTongTien.Text = $"{tongTien:N0} ₫";
                    lbTongTien.TextAlign = ContentAlignment.MiddleRight;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin gói và thực đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiThongTinTongQuan(DataRow datSanhInfo)
        {
            if (datSanhInfo == null) return;

            try
            {
                if (lbMaDatSanh != null)
                    lbMaDatSanh.Text = $"Mã đặt sảnh: DS{_datSanhId:D6}";

                if (panelTrangThai != null)
                {
                    string trangThai = datSanhInfo["trang_thai"]?.ToString() ?? "";
                    panelTrangThai.Text = ChuyenDoiTrangThai(trangThai);
                    SetStylePanelTrangThai(panelTrangThai, trangThai);
                }

                if (lbTenKhachHang != null)
                    lbTenKhachHang.Text = datSanhInfo["ten_khach_hang"]?.ToString() ?? "";

                if (lbSDT != null)
                    lbSDT.Text = datSanhInfo["sdt"]?.ToString() ?? "";

                if (lbTenNVPhuTrach != null)
                    lbTenNVPhuTrach.Text = !string.IsNullOrEmpty(Session.HoTen) ? Session.HoTen : "Chưa xác định";

                if (lbChiNhanh != null)
                    lbChiNhanh.Text = datSanhInfo["ten_chi_nhanh"]?.ToString() ?? "";

                if (lbSanh != null)
                    lbSanh.Text = datSanhInfo["ten_sanh"]?.ToString() ?? "";
            }
            catch (Exception ex){}

            if (lbGiovaNgayToChuc != null)
            {
                string gioToChuc = "";
                string gioKt = "";
                string ngayToChuc = "";
                TimeSpan? gioToChucTimeSpan = null;

                try
                {
                    if (datSanhInfo["gio_to_chuc"] != DBNull.Value && datSanhInfo["gio_to_chuc"] != null)
                    {
                        if (datSanhInfo["gio_to_chuc"] is TimeSpan tsGio)
                        {
                            gioToChucTimeSpan = tsGio;
                            gioToChuc = $"{tsGio.Hours:D2}:{tsGio.Minutes:D2}";
                        }
                        else if (TimeSpan.TryParse(datSanhInfo["gio_to_chuc"].ToString(), out TimeSpan parsedGio))
                        {
                            gioToChucTimeSpan = parsedGio;
                            gioToChuc = $"{parsedGio.Hours:D2}:{parsedGio.Minutes:D2}";
                        }
                        else
                            gioToChuc = datSanhInfo["gio_to_chuc"].ToString();
                    }
                }
                catch { }

                // Tự động tính giờ kết thúc dựa vào giờ tổ chức (2 khung giờ: 10:30-13:30 và 17:30-20:30)
                if (gioToChucTimeSpan.HasValue)
                {
                    TimeSpan gio10_30 = new TimeSpan(10, 30, 0);
                    TimeSpan gio17_30 = new TimeSpan(17, 30, 0);
                    
                    if (Math.Abs((gioToChucTimeSpan.Value - gio10_30).TotalMinutes) <= 5)
                    {
                        gioKt = "13:30";
                    }
                    else if (Math.Abs((gioToChucTimeSpan.Value - gio17_30).TotalMinutes) <= 5)
                    {
                        gioKt = "20:30";
                    }
                    else
                    {
                        try
                        {
                            if (datSanhInfo["gio_kt"] != DBNull.Value && datSanhInfo["gio_kt"] != null)
                            {
                                if (datSanhInfo["gio_kt"] is TimeSpan tsKt)
                                    gioKt = $"{tsKt.Hours:D2}:{tsKt.Minutes:D2}";
                                else if (TimeSpan.TryParse(datSanhInfo["gio_kt"].ToString(), out TimeSpan parsedKt))
                                    gioKt = $"{parsedKt.Hours:D2}:{parsedKt.Minutes:D2}";
                                else
                                    gioKt = datSanhInfo["gio_kt"].ToString();
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    try
                    {
                        if (datSanhInfo["gio_kt"] != DBNull.Value && datSanhInfo["gio_kt"] != null)
                        {
                            if (datSanhInfo["gio_kt"] is TimeSpan tsKt)
                                gioKt = $"{tsKt.Hours:D2}:{tsKt.Minutes:D2}";
                            else if (TimeSpan.TryParse(datSanhInfo["gio_kt"].ToString(), out TimeSpan parsedKt))
                                gioKt = $"{parsedKt.Hours:D2}:{parsedKt.Minutes:D2}";
                            else
                                gioKt = datSanhInfo["gio_kt"].ToString();
                        }
                    }
                    catch { }
                }

                try
                {
                    if (datSanhInfo["ngay_to_chuc"] != DBNull.Value && datSanhInfo["ngay_to_chuc"] != null)
                    {
                        if (datSanhInfo["ngay_to_chuc"] is DateTime ngay)
                            ngayToChuc = ngay.ToString("dd/MM/yyyy");
                        else if (DateTime.TryParse(datSanhInfo["ngay_to_chuc"].ToString(), out DateTime parsedNgay))
                            ngayToChuc = parsedNgay.ToString("dd/MM/yyyy");
                        else
                            ngayToChuc = datSanhInfo["ngay_to_chuc"].ToString();
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(gioToChuc) && !string.IsNullOrEmpty(gioKt) && !string.IsNullOrEmpty(ngayToChuc))
                    lbGiovaNgayToChuc.Text = $"{gioToChuc} - {gioKt} | {ngayToChuc}";
                else if (!string.IsNullOrEmpty(gioToChuc) && !string.IsNullOrEmpty(ngayToChuc))
                    lbGiovaNgayToChuc.Text = $"{gioToChuc} | {ngayToChuc}";
                else if (!string.IsNullOrEmpty(ngayToChuc))
                    lbGiovaNgayToChuc.Text = ngayToChuc;
            }

            if (lbSoBan_SoKhach != null)
            {
                int? soBan = null;
                if (datSanhInfo["so_ban_du_kien"] != DBNull.Value && datSanhInfo["so_ban_du_kien"] != null)
                {
                    try
                    {
                        if (datSanhInfo["so_ban_du_kien"] is int intVal)
                            soBan = intVal;
                        else if (datSanhInfo["so_ban_du_kien"] is decimal decVal)
                            soBan = (int)decVal;
                        else if (int.TryParse(datSanhInfo["so_ban_du_kien"].ToString(), out int parsedVal))
                            soBan = parsedVal;
                    }
                    catch
                    {
                        soBan = null;
                    }
                }

                if (soBan.HasValue && soBan.Value > 0)
                {
                    int soKhach = soBan.Value * 10;
                    lbSoBan_SoKhach.Text = $"{soBan.Value} bàn / {soKhach:N0} khách";
                }
                else
                {
                    lbSoBan_SoKhach.Text = "Chưa xác định";
                }
            }

            if (lbGhiChuSuKien != null)
                lbGhiChuSuKien.Text = datSanhInfo["ghi_chu"]?.ToString() ?? "Không có ghi chú";
        }

        private void HienThiThongTinHopDong(DataRow datSanhInfo)
        {
            if (datSanhInfo == null) return;

            try
            {
                // Hiển thị số hợp đồng
                Label lbSoHopDongControl = this.Controls.Find("lbSoHopDong", true).FirstOrDefault() as Label;
                if (lbSoHopDongControl != null)
                {
                    string soHopDong = datSanhInfo["so_hop_dong"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(soHopDong))
                        lbSoHopDongControl.Text = "Chưa có hợp đồng";
                    else
                        lbSoHopDongControl.Text = soHopDong;
                }

                // Hiển thị ngày ký
                Label lbNgayKyControl = this.Controls.Find("lbNgayKy", true).FirstOrDefault() as Label;
                if (lbNgayKyControl != null)
                {
                    if (datSanhInfo["ngay_ky"] != DBNull.Value && datSanhInfo["ngay_ky"] != null)
                    {
                        try
                        {
                            if (datSanhInfo["ngay_ky"] is DateTime ngayKy)
                                lbNgayKyControl.Text = ngayKy.ToString("dd/MM/yyyy");
                            else if (DateTime.TryParse(datSanhInfo["ngay_ky"].ToString(), out DateTime parsedNgayKy))
                                lbNgayKyControl.Text = parsedNgayKy.ToString("dd/MM/yyyy");
                            else
                                lbNgayKyControl.Text = datSanhInfo["ngay_ky"].ToString();
                        }
                        catch
                        {
                            lbNgayKyControl.Text = "Chưa có";
                        }
                    }
                    else
                    {
                        lbNgayKyControl.Text = "Chưa có";
                    }
                }

                // Hiển thị điều khoản
                Control richDieuKhoanControl = this.Controls.Find("rich_DieuKhoan", true).FirstOrDefault();
                if (richDieuKhoanControl != null)
                {
                    string dieuKhoan = datSanhInfo["dieu_khoan"]?.ToString() ?? "";
                    if (richDieuKhoanControl is Sunny.UI.UIRichTextBox richTextBox)
                    {
                        if (string.IsNullOrEmpty(dieuKhoan))
                            richTextBox.Text = "Chưa có điều khoản hợp đồng.";
                        else
                            richTextBox.Text = dieuKhoan;
                    }
                }
                else if (rich_DieuKhoan != null)
                {
                    string dieuKhoan = datSanhInfo["dieu_khoan"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(dieuKhoan))
                        rich_DieuKhoan.Text = "Chưa có điều khoản hợp đồng.";
                    else
                        rich_DieuKhoan.Text = dieuKhoan;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin hợp đồng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ChuyenDoiTrangThai(string trangThai)
        {
            if (string.IsNullOrEmpty(trangThai))
                return "";

            switch (trangThai.ToUpper())
            {
                case "CHỜ XÁC NHẬN":
                    return "Chờ xác nhận";
                case "ĐÃ CỌC":
                    return "Đã cọc";
                case "ĐÃ HỦY":
                    return "Đã hủy";
                case "ĐÃ THANH TOÁN":
                    return "Đã thanh toán";
                case "HOÀN TẤT":
                    return "Hoàn tất";
                default:
                    // Nếu không khớp, chuyển đổi chữ đầu tiên thành chữ hoa, các chữ còn lại thành chữ thường
                    if (trangThai.Length > 0)
                    {
                        return char.ToUpper(trangThai[0]) + trangThai.Substring(1).ToLower();
                    }
                    return trangThai;
            }
        }

        // Đặt màu style
        private void SetStylePanelTrangThai(Sunny.UI.UIPanel panel, string trangThai)
        {
            if (panel == null || string.IsNullOrEmpty(trangThai))
                return;

            try
            {
                string trangThaiUpper = trangThai.ToUpper();
                switch (trangThaiUpper)
                {
                    case "CHỜ XÁC NHẬN":
                        panel.Style = Sunny.UI.UIStyle.Orange;
                        break;
                    case "ĐÃ CỌC":
                        panel.Style = Sunny.UI.UIStyle.Blue;
                        break;
                    case "ĐÃ HỦY":
                        panel.Style = Sunny.UI.UIStyle.Red;
                        break;
                    case "ĐÃ THANH TOÁN":
                        panel.Style = Sunny.UI.UIStyle.Green;
                        break;
                    case "HOÀN TẤT":
                        panel.Style = Sunny.UI.UIStyle.LayuiGreen;
                        break;
                    default:
                        panel.Style = Sunny.UI.UIStyle.Gray;
                        break;
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, giữ nguyên style hiện tại
                System.Diagnostics.Debug.WriteLine($"Lỗi khi đặt style cho panelTrangThai: {ex.Message}");
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void segmentedPill1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelTongQuan.Location = new Point(7, 168);
            panelTongQuan.Visible = true;
            panelGoivaThucDon.Visible = false;
            panelHopDong.Visible = false;
            panelThanhToan.Visible = false;

            switch (segmentedPill1.SelectedIndex)
            {
                case 0:
                    panelTongQuan.Location = new Point(7, 168);
                    panelTongQuan.Visible = true;
                    panelGoivaThucDon.Visible = false;
                    panelHopDong.Visible = false;
                    panelThanhToan.Visible = false;
                    break;
                case 1:
                    panelGoivaThucDon.Location = new Point(7, 168);
                    panelGoivaThucDon.Visible = true;
                    panelHopDong.Visible = false;
                    panelThanhToan.Visible = false;
                    panelTongQuan.Visible = false;
                    // Load dữ liệu khi chuyển sang tab này
                    DataRow datSanhInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                    if (datSanhInfo != null)
                        HienThiThongTinGoiVaThucDon(datSanhInfo);
                    break;
                case 2:
                    panelHopDong.Location = new Point(7, 168);
                    panelHopDong.Visible = true;
                    panelTongQuan.Visible = false;
                    panelGoivaThucDon.Visible = false;
                    panelThanhToan.Visible = false;
                    // Load dữ liệu khi chuyển sang tab này
                    DataRow datSanhInfoHopDong = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                    if (datSanhInfoHopDong != null)
                        HienThiThongTinHopDong(datSanhInfoHopDong);
                    break;
                case 3:
                    panelThanhToan.Location = new Point(7, 168);
                    panelThanhToan.Visible = true;
                    panelTongQuan.Visible = false;
                    panelGoivaThucDon.Visible = false;
                    panelHopDong.Visible = false;
                    break;
            }
        }
    }
}
