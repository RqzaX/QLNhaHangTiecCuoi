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
        private string _trangThai = "";

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

            if (dgvDanhSachCoc != null)
            {
                dgvDanhSachCoc.CellClick += DgvDanhSachCoc_CellClick;
            }
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

                _trangThai = datSanhInfo["trang_thai"]?.ToString() ?? "";

                HienThiThongTinTongQuan(datSanhInfo);
                CapNhatTrangThaiChucNang();
                KiemTraVaCapNhatTrangThaiTheoTienCoc();
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

                // Lấy phí sảnh thực sự từ phi_thue_cb
                decimal? phiThueCb = null;
                if (datSanhInfo["phi_thue_cb"] != DBNull.Value && datSanhInfo["phi_thue_cb"] != null)
                {
                    if (datSanhInfo["phi_thue_cb"] is decimal decPhi)
                        phiThueCb = decPhi;
                    else if (decimal.TryParse(datSanhInfo["phi_thue_cb"].ToString(), out decimal parsedPhi))
                        phiThueCb = parsedPhi;
                }

                // Tính giá gói = số bàn * giá cơ bản/bàn
                decimal giaGoi = 0;
                if (soBan.HasValue && giaCoBan.HasValue)
                    giaGoi = soBan.Value * giaCoBan.Value;

                if (lbTextPhiSanh != null && soBan.HasValue && giaCoBan.HasValue)
                    lbTextPhiSanh.Text = $"Phí sảnh ({soBan.Value} bàn x {giaCoBan.Value:N0} đ)";

                decimal phiSanh = giaGoi;

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

                decimal phiSanhThucSu = phiThueCb ?? 0m;
                decimal tamTinh = phiSanhThucSu + giaGoi;
                if (lbSoTienTamTinh != null)
                {
                    lbSoTienTamTinh.AutoSize = false;
                    lbSoTienTamTinh.Width = 150;
                    lbSoTienTamTinh.Location = new Point(342, lbSoTienTamTinh.Location.Y);
                    lbSoTienTamTinh.Text = $"{tamTinh:N0} ₫";
                    lbSoTienTamTinh.TextAlign = ContentAlignment.MiddleRight;
                }

                // VAT cho tiệc cưới là 10%
                decimal vat = tamTinh * 0.10m;
                if (lbSoTienVAT != null)
                {
                    lbSoTienVAT.AutoSize = false;
                    lbSoTienVAT.Width = 150;
                    lbSoTienVAT.Location = new Point(342, lbSoTienVAT.Location.Y);
                    lbSoTienVAT.Text = $"{vat:N0} ₫";
                    lbSoTienVAT.TextAlign = ContentAlignment.MiddleRight;
                }

                // Phí dịch vụ 5%
                decimal phiDichVu = tamTinh * 0.05m;
                if (lbSoTienPhiDichVu != null)
                {
                    lbSoTienPhiDichVu.AutoSize = false;
                    lbSoTienPhiDichVu.Width = 150;
                    lbSoTienPhiDichVu.Location = new Point(342, lbSoTienPhiDichVu.Location.Y);
                    lbSoTienPhiDichVu.Text = $"{phiDichVu:N0} ₫";
                    lbSoTienPhiDichVu.TextAlign = ContentAlignment.MiddleRight;
                    lbSoTienPhiDichVu.Visible = true;
                }
                if (labelPhiDichVu != null)
                {
                    labelPhiDichVu.Visible = true;
                }

                // Tổng = Tạm tính + VAT + Phí dịch vụ
                decimal tongTien = tamTinh + vat + phiDichVu;
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
            catch (Exception ex) { }

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
                case "XÁC NHẬN":
                    return "Xác nhận";
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
                    case "XÁC NHẬN":
                        panel.Style = Sunny.UI.UIStyle.Blue;
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
            KiemTraVaCapNhatTrangThaiTheoTienCoc();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Load thông tin thanh toán và cọc
        private void LoadThongTinThanhToan()
        {
            try
            {
                int? hopDongId = _datSanhBLL.LayHopDongId(_datSanhId);
                if (!hopDongId.HasValue || hopDongId.Value <= 0)
                {
                    ClearThanhToanData();
                    return;
                }

                LoadDanhSachCoc(hopDongId.Value);
                TinhToanTongTien(hopDongId.Value);
                CapNhatTrangThaiChucNang();
                KiemTraVaCapNhatTrangThaiTheoTienCoc();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load danh sách cọc vào DataGridView
        private void LoadDanhSachCoc(int hopDongId)
        {
            try
            {
                DataTable dtCoc = _datSanhBLL.LayDanhSachCoc(hopDongId);

                dgvDanhSachCoc.DataSource = null;
                dgvDanhSachCoc.Columns.Clear();
                dgvDanhSachCoc.Rows.Clear();

                if (dtCoc == null || dtCoc.Rows.Count == 0)
                {
                    SetupCocColumns();
                    return;
                }

                SetupCocColumns();

                foreach (DataRow row in dtCoc.Rows)
                {
                    DateTime ngayNop = row["ngay_nop"] != DBNull.Value ? Convert.ToDateTime(row["ngay_nop"]).ToLocalTime() : DateTime.Now;
                    string hinhThuc = row["hinh_thuc"] != DBNull.Value ? row["hinh_thuc"].ToString() : "";
                    decimal soTien = row["so_tien"] != DBNull.Value ? Convert.ToDecimal(row["so_tien"]) : 0;
                    string ghiChu = row["ghi_chu"] != DBNull.Value ? row["ghi_chu"].ToString() : "";
                    int cocId = row["coc_id"] != DBNull.Value ? Convert.ToInt32(row["coc_id"]) : 0;

                    int rowIndex = dgvDanhSachCoc.Rows.Add();
                    dgvDanhSachCoc.Rows[rowIndex].Cells["NgayNop"].Value = ngayNop.ToString("d/M/yyyy");
                    dgvDanhSachCoc.Rows[rowIndex].Cells["HinhThuc"].Value = hinhThuc;
                    dgvDanhSachCoc.Rows[rowIndex].Cells["SoTien"].Value = FormatCurrency(soTien);
                    dgvDanhSachCoc.Rows[rowIndex].Cells["GhiChu"].Value = ghiChu;
                    dgvDanhSachCoc.Rows[rowIndex].Tag = cocId; // Lưu ID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách cọc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupCocColumns()
        {
            dgvDanhSachCoc.AutoGenerateColumns = false;
            dgvDanhSachCoc.AllowUserToAddRows = false;
            dgvDanhSachCoc.ColumnHeadersHeight = 35;
            dgvDanhSachCoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            Font cellFont = new Font("Segoe UI", 12F, FontStyle.Regular);
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle
            {
                Font = cellFont,
                ForeColor = Color.Black
            };

            // Ngày nộp
            if (!dgvDanhSachCoc.Columns.Contains("NgayNop"))
            {
                dgvDanhSachCoc.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "NgayNop",
                    HeaderText = "Ngày nộp",
                    Width = 120,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    DefaultCellStyle = cellStyle
                });
            }

            // Hình thức
            if (!dgvDanhSachCoc.Columns.Contains("HinhThuc"))
            {
                dgvDanhSachCoc.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "HinhThuc",
                    HeaderText = "Hình thức",
                    Width = 150,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    DefaultCellStyle = cellStyle
                });
            }

            // Số tiền
            if (!dgvDanhSachCoc.Columns.Contains("SoTien"))
            {
                dgvDanhSachCoc.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "SoTien",
                    HeaderText = "Số tiền",
                    Width = 150,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Font = cellFont,
                        ForeColor = Color.Black,
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                });
            }

            // Ghi chú
            if (!dgvDanhSachCoc.Columns.Contains("GhiChu"))
            {
                dgvDanhSachCoc.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "GhiChu",
                    HeaderText = "Ghi chú",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 200,
                    DefaultCellStyle = cellStyle
                });
            }

            // Thao tác - Edit
            if (!dgvDanhSachCoc.Columns.Contains("ColEdit"))
            {
                var colEdit = new DataGridViewButtonColumn
                {
                    Name = "ColEdit",
                    HeaderText = "Thao tác",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    Width = 80,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Font = cellFont,
                        ForeColor = Color.Black
                    }
                };
                dgvDanhSachCoc.Columns.Add(colEdit);
            }

            // Thao tác - Delete
            if (!dgvDanhSachCoc.Columns.Contains("ColDelete"))
            {
                var colDelete = new DataGridViewButtonColumn
                {
                    Name = "ColDelete",
                    HeaderText = "",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    Width = 75,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Font = cellFont,
                        ForeColor = Color.Black
                    }
                };
                dgvDanhSachCoc.Columns.Add(colDelete);
            }
        }

        // Tính toán và hiển thị tổng tiền
        private void TinhToanTongTien(int hopDongId)
        {
            try
            {
                decimal tongDuKien = _datSanhBLL.LayTongDuKien(hopDongId);
                if (lbTongDuKien != null)
                    lbTongDuKien.Text = FormatCurrency(tongDuKien);

                decimal tongCoc = 0;
                DataTable dtCoc = _datSanhBLL.LayDanhSachCoc(hopDongId);
                if (dtCoc != null)
                {
                    foreach (DataRow row in dtCoc.Rows)
                    {
                        if (row["so_tien"] != DBNull.Value)
                            tongCoc += Convert.ToDecimal(row["so_tien"]);
                    }
                }
                if (lbTongCocDaThu != null)
                    lbTongCocDaThu.Text = FormatCurrency(tongCoc);

                decimal tongConLai = tongDuKien - tongCoc;
                if (lbTongConLai != null)
                    lbTongConLai.Text = FormatCurrency(tongConLai);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tính toán tổng tiền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clear dữ liệu thanh toán khi chưa có hợp đồng
        private void ClearThanhToanData()
        {
            dgvDanhSachCoc.DataSource = null;
            dgvDanhSachCoc.Columns.Clear();
            dgvDanhSachCoc.Rows.Clear();
            SetupCocColumns();

            if (lbTongDuKien != null) lbTongDuKien.Text = "0 đ";
            if (lbTongCocDaThu != null) lbTongCocDaThu.Text = "0 đ";
            if (lbTongConLai != null) lbTongConLai.Text = "0 đ";
        }

        private string FormatCurrency(decimal amount)
        {
            return amount.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " ₫";
        }

        private void KiemTraVaCapNhatTrangThaiTheoTienCoc()
        {
            try
            {
                int? hopDongId = _datSanhBLL.LayHopDongId(_datSanhId);
                if (!hopDongId.HasValue || hopDongId.Value <= 0)
                    return;

                DataRow datSanhInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                if (datSanhInfo == null)
                    return;

                string trangThaiHienTai = datSanhInfo["trang_thai"]?.ToString() ?? "";
                
                if (trangThaiHienTai.ToUpper() == "ĐÃ HỦY" || trangThaiHienTai.ToUpper() == "HOÀN TẤT")
                {
                    return;
                }

                DataTable dtCoc = _datSanhBLL.LayDanhSachCoc(hopDongId.Value);
                bool coCoc = dtCoc != null && dtCoc.Rows.Count > 0;

                string trangThaiMoi;
                if (coCoc)
                {
                    trangThaiMoi = "ĐÃ CỌC";
                }
                else
                {
                    trangThaiMoi = "CHỜ XÁC NHẬN";
                }

                if (trangThaiHienTai.ToUpper() != trangThaiMoi.ToUpper())
                {
                    bool success = _datSanhBLL.CapNhatTrangThaiDatSanh(_datSanhId, trangThaiMoi, out string errorMessage);
                    if (success)
                    {
                        _trangThai = trangThaiMoi;
                        
                        if (trangThaiMoi.ToUpper() == "ĐÃ CỌC")
                        {
                            int hoaDonId = _datSanhBLL.TaoHoaDonKhiDaCoc(_datSanhId, out string errorHoaDon);
                            if (!string.IsNullOrEmpty(errorHoaDon) && !errorHoaDon.Contains("Đã có hóa đơn"))
                            {
                                MessageBox.Show($"Lỗi tạo hóa đơn: {errorHoaDon}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        
                        DataRow updatedInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                        if (updatedInfo != null)
                        {
                            HienThiThongTinTongQuan(updatedInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi kiểm tra và cập nhật trạng thái: {ex.Message}");
            }
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
                    // Load dữ liệu khi chuyển sang tab này
                    LoadThongTinThanhToan();
                    break;
            }
        }

        private void btnThemDotCoc_Click(object sender, EventArgs e)
        {
            try
            {
                var formThemCoc = new Frm_ThemCocMoi(_datSanhId)
                {
                    StartPosition = FormStartPosition.CenterParent
                };

                if (formThemCoc.ShowDialog(this) == DialogResult.OK)
                {
                    // Thêm cọc thành công, load lại thông tin thanh toán
                    if (panelThanhToan.Visible)
                    {
                        LoadThongTinThanhToan();
                    }
                    DataRow updatedInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                    if (updatedInfo != null)
                    {
                        HienThiThongTinTongQuan(updatedInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm đợt cọc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDoiLich_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new Frm_DoiLichDatSanh(_datSanhId)
                {
                    StartPosition = FormStartPosition.CenterParent
                };

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("Đổi lịch đặt sảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChiTietDatSanh();
                    
                    if (panelThanhToan.Visible)
                    {
                        LoadThongTinThanhToan();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đổi lịch đặt sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuyDatSanh_Click(object sender, EventArgs e)
        {
            try
            {
                var formHuy = new Frm_HuyDatSanh(_datSanhId)
                {
                    StartPosition = FormStartPosition.CenterParent
                };

                if (formHuy.ShowDialog(this) == DialogResult.OK)
                {
                    // Hủy thành công
                    MessageBox.Show("Hủy đặt sảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadChiTietDatSanh();
                    
                    if (panelThanhToan.Visible)
                    {
                        LoadThongTinThanhToan();
                    }
                    else
                    {
                        CapNhatTrangThaiChucNang();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hủy đặt sảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaDatDatVinhVien_Click(object sender, EventArgs e)
        {
            try
            {
                // Xác nhận xóa vĩnh viễn
                var confirmResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn XÓA VĨNH VIỄN đặt sảnh này?\n\n" +
                    "CẢNH BÁO: Thao tác này không thể hoàn tác!\n" +
                    "Tất cả dữ liệu liên quan (hợp đồng, cọc, thanh toán) sẽ bị xóa vĩnh viễn.",
                    "Xác nhận xóa vĩnh viễn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmResult != DialogResult.Yes)
                    return;

                // Xác nhận lần 2
                var confirmResult2 = MessageBox.Show(
                    "Bạn thực sự muốn xóa vĩnh viễn?\n\n" +
                    "Đây là lần xác nhận cuối cùng!",
                    "Xác nhận lần cuối",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Stop);

                if (confirmResult2 != DialogResult.Yes)
                    return;

                bool result = _datSanhBLL.XoaDatSanhVinhVien(_datSanhId, out string errorMessage);

                if (result)
                {
                    MessageBox.Show("Xóa đặt sảnh vĩnh viễn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Lỗi khi xóa đặt sảnh: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa đặt sảnh vĩnh viễn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Vô hiệu hóa các chức năng khi trạng thái là "ĐÃ HỦY"
        private void CapNhatTrangThaiChucNang()
        {
            bool daHuy = _trangThai.ToUpper() == "ĐÃ HỦY";

            if (btnThemDotCoc != null)
            {
                btnThemDotCoc.Enabled = !daHuy;
            }

            if (btnDoiLich != null)
            {
                btnDoiLich.Enabled = !daHuy;
            }

            if (btnHuyDatSanh != null)
            {
                btnHuyDatSanh.Enabled = !daHuy;
            }

            if (dgvDanhSachCoc != null)
            {
                if (dgvDanhSachCoc.Columns.Contains("ColEdit"))
                {
                    dgvDanhSachCoc.Columns["ColEdit"].Visible = !daHuy;
                }
                if (dgvDanhSachCoc.Columns.Contains("ColDelete"))
                {
                    dgvDanhSachCoc.Columns["ColDelete"].Visible = !daHuy;
                }
            }
        }

        private void DgvDanhSachCoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvDanhSachCoc.Rows.Count)
                return;

            DataGridViewRow row = dgvDanhSachCoc.Rows[e.RowIndex];
            int? cocId = row.Tag as int?;

            if (!cocId.HasValue || cocId.Value <= 0)
                return;

            // Xử lý nút Edit
            if (e.ColumnIndex >= 0 && dgvDanhSachCoc.Columns[e.ColumnIndex].Name == "ColEdit")
            {
                try
                {
                    var formEdit = new Frm_ThemCocMoi(_datSanhId, cocId.Value)
                    {
                        StartPosition = FormStartPosition.CenterParent
                    };

                    if (formEdit.ShowDialog(this) == DialogResult.OK)
                    {
                        int? hopDongId = _datSanhBLL.LayHopDongId(_datSanhId);
                        if (hopDongId.HasValue)
                        {
                            LoadDanhSachCoc(hopDongId.Value);
                            TinhToanTongTien(hopDongId.Value);
                            KiemTraVaCapNhatTrangThaiTheoTienCoc();
                            DataRow updatedInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                            if (updatedInfo != null)
                            {
                                HienThiThongTinTongQuan(updatedInfo);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi mở form chỉnh sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Xử lý nút Delete
            else if (e.ColumnIndex >= 0 && dgvDanhSachCoc.Columns[e.ColumnIndex].Name == "ColDelete")
            {
                try
                {
                    var confirmResult = MessageBox.Show(
                        "Bạn có chắc chắn muốn xóa đợt cọc này?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmResult == DialogResult.Yes)
                    {
                        bool success = _datSanhBLL.XoaCoc(cocId.Value, out string errorMessage);

                        if (success)
                        {
                            MessageBox.Show("Xóa đợt cọc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            int? hopDongId = _datSanhBLL.LayHopDongId(_datSanhId);
                            if (hopDongId.HasValue)
                            {
                                LoadDanhSachCoc(hopDongId.Value);
                                TinhToanTongTien(hopDongId.Value);
                                KiemTraVaCapNhatTrangThaiTheoTienCoc();
                                DataRow updatedInfo = _datSanhBLL.LayThongTinDatSanh(_datSanhId);
                                if (updatedInfo != null)
                                {
                                    HienThiThongTinTongQuan(updatedInfo);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Lỗi khi xóa đợt cọc: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa đợt cọc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
