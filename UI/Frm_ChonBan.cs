using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using QLNhaHangTiecCuoi.BLL;
using QLNhaHangTiecCuoi.Share;
using UI.Controls;
using UI.Common;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class Frm_ChonBan : Form
    {
        private Color _borderColor = Color.Black;
        private int _borderThickness = 2;
        private readonly BanBLL _banBLL;
        private readonly DatabaseHelper _dbHelper;
        private DataTable _danhSachKhuVuc;
        private int? _khuVucHienTai = null; // null = tất cả
        // Thông tin bàn đã chọn để trả về cho màn hình bán hàng
        public string SelectedSoBan { get; private set; }
        public int SelectedSucChua { get; private set; }
        public string SelectedTrangThai { get; private set; }

        public Frm_ChonBan()
        {
            try
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            UpdateRegion(18);
                
                _dbHelper = new DatabaseHelper();
                
                if (!_dbHelper.TestConnection())
                {
                    MessageBox.Show("Không thể kết nối đến database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                _banBLL = new BanBLL(_dbHelper);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form chọn bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateRegion(18);
        }

        private void UpdateRegion(int radius)
        {
            var r = new Rectangle(0, 0, Width, Height);
            using var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            Region = new Region(path);
        }

        private void trangThaiBan2_TransferClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Đã chọn bàn này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int radius = 18;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = new GraphicsPath())
            {
                int d = radius * 2;
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                using var pen = new Pen(_borderColor, _borderThickness);
                pen.Alignment = PenAlignment.Inset;
                g.DrawPath(pen, path);
            }
        }

        private void Frm_ChonBan_Load(object sender, EventArgs e)
        {
            try
            {
                if (segmentedPill1 == null)
                {
                    MessageBox.Show("Lỗi: Control segmentedPill1 không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (panelDanhSachBan == null)
                {
                    MessageBox.Show("Lỗi: Control panelDanhSachBan không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LoadDanhSachKhuVuc();
                LoadDanhSachBan();
                CapNhatThongKeBan();
                SetupSegmentedPillEvent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load form chọn bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelDanhSachBan_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void LoadDanhSachKhuVuc()
        {
            try
            {
                this.Enabled = false;
                
                if (!_dbHelper.TestConnection())
                {
                    MessageBox.Show("Không thể kết nối đến database. Vui lòng kiểm tra kết nối.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (Session.ChiNhanhId > 0)
                {
                    _danhSachKhuVuc = await Task.Run(() => _banBLL.LayDanhSachKhuVucTheoChiNhanh(Session.ChiNhanhId));
                }
                else
                {
                    _danhSachKhuVuc = await Task.Run(() => _banBLL.LayDanhSachKhuVuc());
                }
                
                UpdateSegmentedPillItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void UpdateSegmentedPillItems()
        {
            try
            {
                while (segmentedPill1.Items.Count > 1)
                {
                    segmentedPill1.Items.RemoveAt(1);
                }

                if (_danhSachKhuVuc != null && _danhSachKhuVuc.Rows.Count > 0)
                {
                    foreach (DataRow row in _danhSachKhuVuc.Rows)
                    {
                        var pillItem = new VanThuan.UI.PillItem
                        {
                            Text = row["ten_khu_vuc"].ToString()
                        };
                        segmentedPill1.Items.Add(pillItem);
                    }
                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật segmented pill: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void LoadDanhSachBan()
        {
            try
            {
                this.Enabled = false;
                
                panelDanhSachBan.Controls.Clear();

                DataTable dtBan;
                
                if (Session.ChiNhanhId > 0)
                {
                    dtBan = await Task.Run(() => _banBLL.LayDanhSachBanTheoChiNhanh(Session.ChiNhanhId, _khuVucHienTai));
                }
                else
                {
                    dtBan = await Task.Run(() => _banBLL.LayDanhSachBanTheoKhuVuc(_khuVucHienTai));
                }
                
                
                if (dtBan == null || dtBan.Rows.Count == 0)
                {
 
                    Label lblNoData = new Label
                    {
                        Text = "Không có bàn nào trong khu vực này",
                        Font = new Font("Segoe UI", 14F, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        AutoSize = true,
                        Location = new Point(20, 20)
                    };
                    panelDanhSachBan.Controls.Add(lblNoData);
                    return;
                }

                DisplayBanCards(dtBan);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load danh sách bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void DisplayBanCards(DataTable dtBan)
        {
            try
            {
                int panelWidth = 808;
                int panelHeight = 309;
                int columnCount = 3;
                
                int margin = 8;
                int padding = 15;
                
                int availableWidth = panelWidth - (padding * 2) - (margin * (columnCount - 1));
                int cardWidth = availableWidth / columnCount;
                int cardHeight = 140;
                
                int startX = padding;
                int startY = padding;

                for (int i = 0; i < dtBan.Rows.Count; i++)
                {
                    DataRow row = dtBan.Rows[i];
                    
                    TinhTrangBan banCard = CreateBanCard(row);
                    
                    int col = i % columnCount;
                    int rowIndex = i / columnCount;
                    
                    int x = startX + col * (cardWidth + margin);
                    int y = startY + rowIndex * (cardHeight + margin);
                    
                    banCard.Size = new Size(cardWidth, cardHeight);
                    banCard.Location = new Point(x, y);
                    panelDanhSachBan.Controls.Add(banCard);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị danh sách bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TinhTrangBan CreateBanCard(DataRow row)
        {
            try
            {
                TinhTrangBan banCard = new TinhTrangBan
                {
                    TableCode = row["so_ban"].ToString(),
                    Capacity = Convert.ToInt32(row["suc_chua"]),
                    Status = GetTableStateFromString(row["trang_thai"].ToString()),
                    CornerRadius = 20,
                    Font = new Font("Segoe UI", 11F),
                    ForeColor = Color.FromArgb(17, 24, 39),
                };

                banCard.Click += BanCard_Click;

                return banCard;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo card bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new TinhTrangBan();
            }
        }

        private TinhTrangBan.TableState GetTableStateFromString(string trangThai)
        {
            switch (trangThai.ToUpper())
            {
                case "TRỐNG":
                    return TinhTrangBan.TableState.Available;
                case "PHỤC VỤ":
                    return TinhTrangBan.TableState.InUse;
                case "ĐÃ ĐẶT":
                    return TinhTrangBan.TableState.Reserved;
                case "VỆ SINH":
                    return TinhTrangBan.TableState.Available; 
                default:
                    return TinhTrangBan.TableState.Available;
            }
        }

        private void BanCard_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is TinhTrangBan banCard)
                {
                    var banInfo = GetBanInfoFromTable(banCard.TableCode);
                    if (banInfo == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int banId = Convert.ToInt32(banInfo["ban_id"]);
                    string trangThai = banInfo["trang_thai"].ToString();

                    switch (trangThai.ToUpper())
                    {
                        case "TRỐNG":
                            var resultTrong = MessageBox.Show(
                                $"Bàn {banCard.TableCode} đang trống.\nBạn có muốn chọn bàn này để order món không?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (resultTrong == DialogResult.Yes)
                            {
                                bool success = _banBLL.CapNhatTrangThaiBan(banId, "PHỤC VỤ");
                                if (success)
                                {
                                    SelectedSoBan = banCard.TableCode;
                                    SelectedSucChua = banCard.Capacity;
                                    SelectedTrangThai = "Đang sử dụng";
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Không thể cập nhật trạng thái bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            break;

                        case "PHỤC VỤ":
                            var resultPhucVu = MessageBox.Show(
                                $"Bàn {banCard.TableCode} đang được sử dụng.\nBạn có muốn xem thông tin và món đã order không?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (resultPhucVu == DialogResult.Yes)
                            {
                                using (var frmThongTin = new Frm_ThongTinBan(banId, banCard.TableCode, trangThai, _banBLL))
                                {
                                    var dialogResult = frmThongTin.ShowDialog();
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        SelectedSoBan = banCard.TableCode;
                                        SelectedSucChua = banCard.Capacity;
                                        SelectedTrangThai = "Đang sử dụng";
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    }
                                }
                                
                                LoadDanhSachBan();
                                CapNhatThongKeBan();
                            }
                            break;

                        case "ĐÃ ĐẶT":
                            var resultDatTruoc = MessageBox.Show(
                                $"Bàn {banCard.TableCode} đã được đặt trước.\nBạn có muốn xem thông tin đặt bàn và tiếp nhận khách không?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (resultDatTruoc == DialogResult.Yes)
                            {
                                using (var frmThongTin = new Frm_ThongTinBan(banId, banCard.TableCode, trangThai, _banBLL))
                                {
                                    var dialogResult = frmThongTin.ShowDialog();
                                    if (dialogResult == DialogResult.OK)
                                    {
                                        SelectedSoBan = banCard.TableCode;
                                        SelectedSucChua = banCard.Capacity;
                                        SelectedTrangThai = "Đang sử dụng";
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    }
                                }
                                
                                LoadDanhSachBan();
                                CapNhatThongKeBan();
                            }
                            break;

                        case "VỆ SINH":
                            MessageBox.Show(
                                $"Bàn {banCard.TableCode} đang được vệ sinh.\nVui lòng chọn bàn khác hoặc đợi bàn được dọn dẹp xong.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            break;

                        default:
                            MessageBox.Show($"Trạng thái bàn không xác định: {trangThai}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý chọn bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataRow GetBanInfoFromTable(string soBan)
        {
            try
            {
                DataTable dtBan;
                
                if (Session.ChiNhanhId > 0)
                {
                    dtBan = _banBLL.LayDanhSachBanTheoChiNhanh(Session.ChiNhanhId, _khuVucHienTai);
                }
                else
                {
                    dtBan = _banBLL.LayDanhSachBanTheoKhuVuc(_khuVucHienTai);
                }

                if (dtBan != null && dtBan.Rows.Count > 0)
                {
                    foreach (DataRow row in dtBan.Rows)
                    {
                        if (row["so_ban"].ToString() == soBan)
                        {
                            return row;
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lấy thông tin bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void CapNhatThongKeBan()
        {
            try
            {
                if (lbTongSoBan == null || lbBanTrong == null || lbDangPhucVu == null || lbDaDatTruoc == null)
                {
                    return;
                }
                
                DataTable dtThongKe;
                
                if (Session.ChiNhanhId > 0)
                {
                    if (_khuVucHienTai.HasValue)
                    {
                        dtThongKe = _banBLL.LayThongKeBanTheoKhuVuc(_khuVucHienTai);
                    }
                    else
                    {
                        dtThongKe = _banBLL.LayThongKeBanTheoChiNhanh(Session.ChiNhanhId);
                    }
                }
                else
                {
                    dtThongKe = _banBLL.LayThongKeBanTheoKhuVuc(_khuVucHienTai);
                }
                
                if (dtThongKe != null && dtThongKe.Rows.Count > 0)
                {
                    DataRow row = dtThongKe.Rows[0];
                    
                    lbTongSoBan.Text = row["tong_ban"].ToString();
                    lbBanTrong.Text = row["ban_trong"].ToString();
                    lbDangPhucVu.Text = row["dang_su_dung"].ToString();
                    lbDaDatTruoc.Text = row["da_dat_truoc"].ToString();
                }
                else
                {
                    lbTongSoBan.Text = "0";
                    lbBanTrong.Text = "0";
                    lbDangPhucVu.Text = "0";
                    lbDaDatTruoc.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật thống kê: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupSegmentedPillEvent()
        {
            try
            {
                if (segmentedPill1 != null)
                {
                    segmentedPill1.SelectedIndexChanged += SegmentedPill1_SelectedIndexChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi segmented pill: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SegmentedPill1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (segmentedPill1 == null) return;
                
                int selectedIndex = segmentedPill1.SelectedIndex;
                
                if (selectedIndex == 0)
                {
                    _khuVucHienTai = null;
                }
                else
                {
                    if (_danhSachKhuVuc != null && selectedIndex - 1 < _danhSachKhuVuc.Rows.Count)
                    {
                        _khuVucHienTai = Convert.ToInt32(_danhSachKhuVuc.Rows[selectedIndex - 1]["khu_vuc_id"]);
                    }
                }

                CapNhatThongKeBan();
                LoadDanhSachBan();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý chọn khu vực: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
