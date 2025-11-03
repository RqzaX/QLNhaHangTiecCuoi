using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

#region Rounded helpers
[SupportedOSPlatform("windows")]
internal sealed class RoundedPanel : Panel
{
    public int Radius { get; set; } = 12;
    public Color BorderColor { get; set; } = Color.FromArgb(220, 225, 235);
    public Color FillColor { get; set; } = Color.White;

    [SupportedOSPlatform("windows")]
    public RoundedPanel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;
        Padding = new Padding(12, 8, 12, 8);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var rect = ClientRectangle; rect.Width--; rect.Height--;
        using var path = Rounded(rect, Radius);
        using var b = new SolidBrush(FillColor);
        using var p = new Pen(BorderColor);

        g.FillPath(b, path);
        g.DrawPath(p, path);
    }

    private static System.Drawing.Drawing2D.GraphicsPath Rounded(Rectangle r, int radius)
    {
        int d = radius * 2;
        var gp = new System.Drawing.Drawing2D.GraphicsPath();
        gp.AddArc(r.X, r.Y, d, d, 180, 90);
        gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        gp.CloseFigure();
        return gp;
    }
}
#endregion
// Thêm class này trước class FrmDatSanhWizard
[SupportedOSPlatform("windows")]
public class FrmCreateContract : Form
{
    private int _datSanhId;
    private string _customerName;
    private decimal _totalAmount;

    public FrmCreateContract(int datSanhId, string customerName, decimal totalAmount)
    {
        _datSanhId = datSanhId;
        _customerName = customerName;
        _totalAmount = totalAmount;

        Text = "Tạo hợp đồng";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(650, 550);
        Font = new Font("Segoe UI", 10.5f);
        BackColor = Color.White;

        // Header
        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.White, Padding = new Padding(24) };
        var icon = new Label
        {
            Text = "✓",
            Font = new Font("Arial", 40f),
            ForeColor = Color.FromArgb(40, 167, 69),
            Dock = DockStyle.Left,
            Width = 60,
            TextAlign = ContentAlignment.MiddleCenter
        };
        var titlePanel = new Panel { Dock = DockStyle.Fill };
        var title = new Label
        {
            Text = "Tạo phiếu thành công!",
            Font = new Font("Segoe UI Semibold", 18f),
            ForeColor = Color.Black,
            Dock = DockStyle.Top,
            Height = 32
        };
        var desc = new Label
        {
            Text = "Phiếu đặt sảnh đã được tạo. Bạn có muốn tạo hợp đồng chính thức ngay không?",
            Font = new Font("Segoe UI", 10.5f),
            ForeColor = Color.Gray,
            Dock = DockStyle.Top,
            Height = 40,
            AutoSize = false
        };
        titlePanel.Controls.Add(desc);
        titlePanel.Controls.Add(title);
        pnlHeader.Controls.Add(titlePanel);
        pnlHeader.Controls.Add(icon);

        // Body
        var pnlBody = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24) };

        // Info box
        var infoBox = new RoundedPanel
        {
            Radius = 12,
            FillColor = Color.FromArgb(240, 250, 255),
            BorderColor = Color.FromArgb(200, 230, 250),
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            Padding = new Padding(16),
            Margin = new Padding(0, 0, 0, 20)
        };
        var infoText = new Label
        {
            Text = $"Tạo hợp đồng giúp bạn:\n• Quản lý điều khoản và lịch thanh toán chi tiết\n• Tạo file PDF hợp đồng chính thức\n• Theo dõi trạng thái kỳ kết và thanh toán",
            AutoSize = true,
            Font = new Font("Segoe UI", 10f)
        };
        infoBox.Controls.Add(infoText);

        // Chi tiết đơn
        var detailBox = new RoundedPanel
        {
            Radius = 12,
            FillColor = Color.White,
            BorderColor = Color.FromArgb(220, 225, 235),
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            Padding = new Padding(20),
            Margin = new Padding(0, 0, 0, 20)
        };

        var lblDetail = new Label
        {
            Text = "Chi tiết đơn đặt",
            Font = new Font("Segoe UI Semibold", 12f),
            Dock = DockStyle.Top,
            Height = 28,
            Margin = new Padding(0, 0, 0, 12)
        };

        var detailContent = new Panel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
        int y = 0;
        var details = new[]
        {
            ("ID Phiếu:", _datSanhId.ToString()),
            ("Khách hàng:", _customerName),
            ("Tổng giá trị:", $"{_totalAmount:N0} đ"),
            ("Trạng thái:", "CHỜ XÁC NHẬN")
        };

        foreach (var (label, value) in details)
        {
            var lbl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.Gray,
                Location = new Point(0, y),
                Width = 150,
                Height = 22
            };
            var val = new Label
            {
                Text = value,
                Font = new Font("Segoe UI Semibold", 10f),
                ForeColor = Color.Black,
                Location = new Point(160, y),
                AutoSize = true,
                Height = 22
            };
            detailContent.Controls.Add(lbl);
            detailContent.Controls.Add(val);
            y += 26;
        }
        detailContent.Height = y;

        detailBox.Controls.Add(detailContent);
        detailBox.Controls.Add(lblDetail);

        pnlBody.Controls.Add(detailBox);
        pnlBody.Controls.Add(infoBox);

        // Footer
        var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Color.White, Padding = new Padding(24) };

        var btnSkip = new Button
        {
            Text = "Để sau",
            Width = 100,
            Height = 44,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.Black,
            DialogResult = DialogResult.Cancel,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left
        };
        btnSkip.FlatAppearance.BorderSize = 1;
        btnSkip.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
        btnSkip.Location = new Point(24, 13);

        var btnCreate = new Button
        {
            Text = "📄 Tạo hợp đồng ngay",
            Width = 180,
            Height = 44,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(33, 33, 33),
            ForeColor = Color.White,
            DialogResult = DialogResult.OK,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };
        btnCreate.FlatAppearance.BorderSize = 0;
        btnCreate.Location = new Point(pnlFooter.Width - 180 - 24, 13);

        pnlFooter.Controls.Add(btnSkip);
        pnlFooter.Controls.Add(btnCreate);

        Controls.Add(pnlBody);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
    }
}

// Thêm class này trước class FrmDatSanhWizard
[SupportedOSPlatform("windows")]
public class FrmPackageDetail : Form
{
    public FrmPackageDetail(string packageName, decimal pricePerTable, int guestCount)
    {
        Text = packageName;
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(600, 600);
        Font = new Font("Segoe UI", 10.5f);
        BackColor = Color.White;

        // Header
        var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.White, Padding = new Padding(24, 16, 24, 0) };
        var title = new Label
        {
            Text = packageName,
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ForeColor = Color.FromArgb(120, 40, 200),
            Dock = DockStyle.Top,
            Height = 32
        };
        var desc = new Label
        {
            Text = "Gói tiệc cao cấp với thực đơn đa dạng, nguyên liệu nhập khẩu",
            Font = new Font("Segoe UI", 10f),
            ForeColor = Color.Gray,
            Dock = DockStyle.Top,
            Height = 24,
            AutoSize = false
        };
        pnlHeader.Controls.Add(desc);
        pnlHeader.Controls.Add(title);

        // Body
        var pnlBody = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24, 16, 24, 0) };

        // Giá
        var priceBox = new RoundedPanel
        {
            Radius = 12,
            Height = 90,
            FillColor = Color.FromArgb(245, 240, 255),
            BorderColor = Color.FromArgb(220, 200, 250),
            Dock = DockStyle.Top,
            Padding = new Padding(20, 12, 20, 12),
            Margin = new Padding(0, 0, 0, 20)
        };
        var lbPrice = new Label
        {
            Text = $"Giá/bàn ({guestCount} khách)\n{pricePerTable:N0} đ",
            Font = new Font("Segoe UI Semibold", 14f),
            ForeColor = Color.FromArgb(120, 40, 200),
            Dock = DockStyle.Left,
            Width = 280,
            TextAlign = ContentAlignment.MiddleLeft
        };
        var icPrice = new Label
        {
            Text = "💵",
            Font = new Font("Arial", 40f),
            ForeColor = Color.FromArgb(120, 40, 200),
            Dock = DockStyle.Right,
            Width = 60,
            TextAlign = ContentAlignment.MiddleCenter
        };
        priceBox.Controls.Add(icPrice);
        priceBox.Controls.Add(lbPrice);

        // Món khai vị
        var appetizer = CreateSection("🥘 Món khai vị", new[]
        {
            "Gỏi ngó sen tôm thịt",
            "Salad hải sản sốt chanh dây",
            "Chả giò hải sản đặc biệt",
            "Nem cuốn rau củ tôm thịt"
        });

        // Món chính
        var main = CreateSection("🍖 Món chính", new[]
        {
            "Tôm hùm nướng bơ tỏi",
            "Cá chèm hấp Hồng Kong",
            "Bộ úc nướng tiêu đen",
            "Gà ta quay mật ong"
        });

        // Canh/Súp + Trắng miệng
        var soupDessert = new Panel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Height = 240, Margin = new Padding(0, 0, 0, 16) };

        var soupBox = new RoundedPanel
        {
            Radius = 10,
            Width = 260,
            Height = 120,
            FillColor = Color.White,
            BorderColor = Color.FromArgb(220, 225, 235),
            Padding = new Padding(16, 12, 16, 12),
            Dock = DockStyle.Left,
            Margin = new Padding(0, 0, 12, 0)
        };
        var soupLbl = new Label { Text = "🥣 Canh/Súp", Font = new Font("Segoe UI Semibold", 11f), Dock = DockStyle.Top, Height = 24 };
        var soupItems = new Label
        {
            Text = "✓ Súp cua\n✓ Canh báo ngư nấm",
            Font = new Font("Segoe UI", 9.5f),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.TopLeft,
            ForeColor = Color.FromArgb(60, 140, 60)
        };
        soupBox.Controls.Add(soupItems);
        soupBox.Controls.Add(soupLbl);

        var dessertBox = new RoundedPanel
        {
            Radius = 10,
            Width = 260,
            Height = 120,
            FillColor = Color.White,
            BorderColor = Color.FromArgb(220, 225, 235),
            Padding = new Padding(16, 12, 16, 12),
            Dock = DockStyle.Right,
            Margin = new Padding(12, 0, 0, 0)
        };
        var dessertLbl = new Label { Text = "🎀 Trắng miệng", Font = new Font("Segoe UI Semibold", 11f), Dock = DockStyle.Top, Height = 24 };
        var dessertItems = new Label
        {
            Text = "✓ Chè dưỡng nhan\n✓ Trái cây theo mùa\n✓ Bánh ngọt Pháp",
            Font = new Font("Segoe UI", 9.5f),
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.TopLeft,
            ForeColor = Color.FromArgb(60, 140, 60)
        };
        dessertBox.Controls.Add(dessertItems);
        dessertBox.Controls.Add(dessertLbl);

        soupDessert.Controls.Add(dessertBox);
        soupDessert.Controls.Add(soupBox);

        // Dịch vụ kèm theo
        var services = CreateSection("✨ Dịch vụ kèm theo", new[]
        {
            "MC chuyên nghiệp",
            "Âm thanh ánh sáng hiện đại",
            "Trang trí sân khấu backdrop cao cấp",
            "Photo booth & Photobooth props",
            "Dịch vụ khăn nóng"
        });

        // Ghi chú đặc biệt
        var noteBox = new RoundedPanel
        {
            Radius = 10,
            Height = 80,
            FillColor = Color.FromArgb(230, 245, 255),
            BorderColor = Color.FromArgb(200, 230, 250),
            Dock = DockStyle.Top,
            Padding = new Padding(16),
            Margin = new Padding(0, 16, 0, 0)
        };
        var noteLabel = new Label
        {
            Text = "ℹ️ Ghi chú đặc biệt\nMiễn phí 1 phòng thay đổ VIP cho có đầu chủ rể",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f),
            ForeColor = Color.FromArgb(25, 100, 180),
            TextAlign = ContentAlignment.TopLeft
        };
        noteBox.Controls.Add(noteLabel);

        pnlBody.Controls.Add(noteBox);
        pnlBody.Controls.Add(services);
        pnlBody.Controls.Add(soupDessert);
        pnlBody.Controls.Add(main);
        pnlBody.Controls.Add(appetizer);
        pnlBody.Controls.Add(priceBox);

        // Footer
        var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Color.White, Padding = new Padding(24, 12, 24, 12) };

        var btnClose = new Button
        {
            Text = "Đóng",
            Width = 100,
            Height = 44,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.White,
            ForeColor = Color.Black,
            DialogResult = DialogResult.Cancel,
            Anchor = AnchorStyles.Left | AnchorStyles.Bottom
        };
        btnClose.FlatAppearance.BorderSize = 1;
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
        btnClose.Location = new Point(24, 12);

        var btnSelect = new Button
        {
            Text = "✓ Chọn gói này",
            Width = 160,
            Height = 44,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(36, 99, 235),
            ForeColor = Color.White,
            DialogResult = DialogResult.OK,
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom
        };
        btnSelect.FlatAppearance.BorderSize = 0;
        btnSelect.Location = new Point(pnlFooter.Width - 160 - 24, 12);

        pnlFooter.Controls.Add(btnClose);
        pnlFooter.Controls.Add(btnSelect);

        Controls.Add(pnlBody);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
    }

    private RoundedPanel CreateSection(string title, string[] items)
    {
        var box = new RoundedPanel
        {
            Radius = 10,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            FillColor = Color.White,
            BorderColor = Color.FromArgb(220, 225, 235),
            Padding = new Padding(20, 16, 20, 16),
            Dock = DockStyle.Top,
            Margin = new Padding(0, 0, 0, 16)
        };

        var titleLbl = new Label { Text = title, Font = new Font("Segoe UI Semibold", 11f), Dock = DockStyle.Top, Height = 28, Margin = new Padding(0, 0, 0, 12) };

        var itemsPanel = new Panel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
        int y = 0;
        foreach (var item in items)
        {
            var itemLbl = new Label
            {
                Text = $"✓ {item}",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(60, 140, 60),
                AutoSize = true,
                Location = new Point(0, y),
                Height = 22
            };
            itemsPanel.Controls.Add(itemLbl);
            y += 22;
        }
        itemsPanel.Height = y;

        box.Controls.Add(itemsPanel);
        box.Controls.Add(titleLbl);
        return box;
    }
}

[SupportedOSPlatform("windows")]
public class FrmDatSanhWizard : Form
{
    #region In-memory models (property để ComboBox bind được)
    class Branch { public int Id { get; set; } public string Name { get; set; } = ""; }
    class Shift { public int Id { get; set; } public string Name { get; set; } = ""; }
    class Hall
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Name { get; set; } = "";
        public int Capacity { get; set; }
        public decimal BaseFee { get; set; }
    }
    class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal PricePerTable { get; set; }
    }
    class Reservation // demo giữ lịch
    {
        public int BranchId, HallId, ShiftId, Tables;
        public DateTime Date;
        public string CustomerName = "", Phone = "";
        public int? PackageId;
        public decimal HallFee, PackagePrice, Deposit;
        public string PaymentMethod = "";
    }
    #endregion

    #region Demo data
    readonly List<Branch> _branches = new() {
        new(){ Id=1, Name="CN Trung tâm"}, new(){ Id=2, Name="CN Quận 7"}
    };
    readonly List<Shift> _shifts = new() {
        new(){ Id=1, Name="Trưa (9:00 - 14:00)"}, new(){ Id=2, Name="Tối (16:00 - 22:00)"}, new(){ Id=3, Name="Cả ngày (9:00 - 22:00)" }
    };
    readonly List<Hall> _halls = new() {
        new(){ Id=1, BranchId=1, Name="Sảnh Ruby",     Capacity=300, BaseFee=15_000_000 },
        new(){ Id=2, BranchId=1, Name="Sảnh Sapphire", Capacity=200, BaseFee=10_000_000 },
        new(){ Id=3, BranchId=2, Name="Sảnh Diamond",  Capacity=250, BaseFee=12_000_000 },
    };
    readonly List<Package> _packages = new() {
        new(){ Id=1, Name="Gói tiệc VIP",      PricePerTable=5_000_000 },
        new(){ Id=2, Name="Gói tiệc Premium",  PricePerTable=4_000_000 },
        new(){ Id=3, Name="Gói tiệc Standard", PricePerTable=3_000_000 },
    };
    static readonly List<Reservation> _reservations = new();
    #endregion

    #region State + UI fields
    int _step = 1;
    decimal _hallFee = 0m;
    decimal _pkgPrice = 0m;
    int _tables = 0;

    Panel pnlHeader = new(), pnlBody = new(), pnlFooter = new();
    Button btnBack = new(), btnNext = new(), btnCreate = new();

    Panel step1 = new(), step2 = new(), step3 = new();

    ComboBox cboCN = new(), cboSanh = new(), cboCa = new();
    DateTimePicker dtNgay = new();
    TextBox txtSoBan = new();
    Label lbAvail = new();

    TextBox txtSDT = new(), txtTenKH = new();
    FlowLayoutPanel flGoi = new();
    Label lbTamPhiSanh = new(), lbTamGoi = new(), lbTamTong = new();

    NumericUpDown numCoc1 = new();
    ComboBox cboPTTT = new();
    #endregion

    public FrmDatSanhWizard()
    {
        // Form
        Text = "Tạo đơn đặt sảnh mới (demo không DB)";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        ClientSize = new Size(900, 720);
        Font = new Font("Segoe UI", 10.5f);
        BackColor = Color.White;

        // Header
        pnlHeader = new Panel { Dock = DockStyle.Top, Height = 84, BackColor = Color.White };
        var title = new Label
        {
            Text = "Tạo đơn đặt sảnh mới",
            Font = new Font("Segoe UI Semibold", 20f),
            ForeColor = Color.Black,
            Dock = DockStyle.Top,
            Padding = new Padding(24, 10, 0, 0),
            Height = 46
        };
        var sub = new Label
        {
            Text = "Bước 1/3: Chọn sảnh & thời gian",
            Font = new Font("Segoe UI", 11f),
            ForeColor = Color.Gray,
            Dock = DockStyle.Top,
            Padding = new Padding(26, 0, 0, 12),
            Height = 32
        };
        // phụ đề trước, tiêu đề sau (Dock=Top)
        pnlHeader.Controls.Add(sub);
        pnlHeader.Controls.Add(title);

        // Footer
        pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 82, BackColor = Color.White, Padding = new Padding(24) };
        btnBack = MakePrimary("Quay lại", 124); btnBack.Enabled = false; btnBack.Click += (s, e) => GoStep(-1);
        btnNext = MakePrimary("Tiếp tục", 132); btnNext.Click += (s, e) => { if (ValidateStep()) GoStep(1); };
        btnCreate = MakePrimary("Tạo phiếu đặt sảnh", 196); btnCreate.Visible = false; btnCreate.Click += (s, e) => CreateReservation();
        pnlFooter.Controls.AddRange(new Control[] { btnBack, btnNext, btnCreate });
        btnBack.Location = new Point(24, 22);
        btnCreate.Location = new Point(pnlFooter.Width - btnCreate.Width - 24, 22);
        btnNext.Location = new Point(btnCreate.Left - btnNext.Width - 12, 22);
        pnlFooter.Resize += (s, e) =>
        {
            btnCreate.Left = pnlFooter.Width - btnCreate.Width - 24;
            btnNext.Left = btnCreate.Left - btnNext.Width - 12;
        };

        // Body
        pnlBody = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

        // Steps
        BuildStep1();
        BuildStep2();
        BuildStep3();

        step1.Dock = DockStyle.Fill;
        step2.Dock = DockStyle.Fill;
        step3.Dock = DockStyle.Fill;
        pnlBody.Controls.Add(step1);
        pnlBody.Controls.Add(step2);
        pnlBody.Controls.Add(step3);
        step2.Visible = false; step3.Visible = false; step1.BringToFront();

        // Compose
        Controls.Add(pnlBody);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);

        // Data
        LoadCombos();
        LoadPackages();
    }

    #region Build Steps
    private void BuildStep1()
    {
        step1 = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24) };

        // ===== Step indicator (1-2-3) =====
        var stepIndicator = CreateStepIndicator(1);

        // ---- Labels
        var lblCN = L("Chi nhánh");
        var lblSanh = L("Sảnh");
        var lblNgay = L("Ngày tổ chức");
        var lblCa = L("Ca");
        var lblBan = L("Số bàn dự kiến");

        // ---- Inputs
        cboCN.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSanh.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCa.DropDownStyle = ComboBoxStyle.DropDownList;
        dtNgay.Format = DateTimePickerFormat.Custom;
        dtNgay.CustomFormat = "dd/MM/yyyy";

        // ---- Bảng 4 hàng (label trái / input phải)
        var table = new TableLayoutPanel
        {
            ColumnCount = 2,
            AutoSize = true,
            Dock = DockStyle.Top,
            Width = 760,
            Margin = new Padding(0, 0, 0, 16)
        };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
        AddRow(table, lblCN, WrapInput(cboCN, 420), 0);
        AddRow(table, lblSanh, WrapInput(cboSanh, 420), 1);
        AddRow(table, lblNgay, WrapInput(dtNgay, 420), 2);
        AddRow(table, lblCa, WrapInput(cboCa, 420), 3);

        // ---- Dòng "Số bàn dự kiến"
        var soBanLine = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Margin = new Padding(0, 0, 0, 16)
        };
        soBanLine.Controls.Add(lblBan);
        soBanLine.Controls.Add(WrapInput(txtSoBan, 220));

        // ---- Thanh trạng thái khả dụng (bo tròn)
        var availHost = new RoundedPanel
        {
            Radius = 10,
            Height = 50,
            Dock = DockStyle.Top,
            FillColor = Color.FromArgb(230, 255, 236),
            BorderColor = Color.FromArgb(205, 235, 215),
            Padding = new Padding(16, 0, 16, 0),
            Margin = new Padding(0),
            Width = 760
        };
        lbAvail = new Label
        {
            Text = "—",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.FromArgb(12, 83, 35)
        };
        availHost.Controls.Add(lbAvail);

        // ---- Wrapper (container chứa form)
        var wrapper = new Panel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Width = 760 };
        wrapper.Controls.Add(availHost);
        wrapper.Controls.Add(soBanLine);
        wrapper.Controls.Add(table);

        step1.Controls.Add(wrapper);
        step1.Controls.Add(stepIndicator);

        // ---- Sự kiện
        cboCN.SelectedIndexChanged += (_, __) => LoadHalls();
        cboSanh.SelectedIndexChanged += (_, __) => UpdateAvailability();
        cboCa.SelectedIndexChanged += (_, __) => UpdateAvailability();
        dtNgay.ValueChanged += (_, __) => UpdateAvailability();
        txtSoBan.TextChanged += (_, __) => ParseTables();
    }

    private void BuildStep2()
    {
        step2 = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24) };

        // ===== Step indicator (1-2-3) =====
        var stepIndicator = CreateStepIndicator(2);

        // ---- Form nhập liệu
        var lblSdt = L("Số điện thoại");
        var lblTen = L("Tên khách hàng");
        var lblEmail = L("Email");

        var grid = new TableLayoutPanel { ColumnCount = 2, RowCount = 3, AutoSize = true, Width = 760 };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
        AddRow(grid, lblSdt, WrapInput(txtSDT, 420), 0);
        AddRow(grid, lblTen, WrapInput(txtTenKH, 420), 1);

        var txtEmail = new TextBox { Font = new Font("Segoe UI", 10.5f), PlaceholderText = "email@example.com" };
        AddRow(grid, lblEmail, WrapInput(txtEmail, 420), 2);

        grid.Dock = DockStyle.Top;
        grid.Margin = new Padding(0, 0, 0, 20);

        // ---- Tiêu đề "Chọn gói tiệc"
        var lblChonGoi = new Label { Text = "Chọn gói tiệc", Font = new Font("Segoe UI Semibold", 12.5f), Height = 32, Dock = DockStyle.Top, Margin = new Padding(0, 0, 0, 12) };

        // ---- Các gói tiệc
        flGoi = new FlowLayoutPanel { AutoScroll = false, Padding = new Padding(0), Dock = DockStyle.Top };
        flGoi.Height = 320;

        // ---- Info box tổng cộng
        var box = new RoundedPanel
        {
            Radius = 12,
            Height = 110,
            FillColor = Color.FromArgb(242, 246, 255),
            BorderColor = Color.FromArgb(220, 230, 250),
            Dock = DockStyle.Top,
            Padding = new Padding(16),
            Margin = new Padding(0, 20, 0, 0),
            Width = 760
        };
        lbTamPhiSanh = new Label { Text = "Phí sảnh: —", Dock = DockStyle.Top, Height = 28 };
        lbTamGoi = new Label { Text = "Gói tiệc (0 bàn): —", Dock = DockStyle.Top, Height = 28 };
        lbTamTong = new Label { Text = "Tổng: —", Dock = DockStyle.Top, Height = 32, Font = new Font("Segoe UI Semibold", 12.5f), ForeColor = Color.FromArgb(36, 99, 235) };
        box.Controls.Add(lbTamTong);
        box.Controls.Add(lbTamGoi);
        box.Controls.Add(lbTamPhiSanh);

        // ---- Wrapper (container chứa hết)
        var wrapper = new Panel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Width = 760 };
        wrapper.Controls.Add(box);
        wrapper.Controls.Add(flGoi);
        wrapper.Controls.Add(lblChonGoi);
        wrapper.Controls.Add(grid);

        step2.Controls.Add(wrapper);
        step2.Controls.Add(stepIndicator);
    }

    private Panel CreateStepIndicator(int currentStep)
    {
        var stepIndicator = new Panel { Dock = DockStyle.Top, Height = 80, AutoSize = false, Margin = new Padding(0, 0, 0, 24) };

        for (int i = 1; i <= 3; i++)
        {
            bool isActive = i <= currentStep;

            // Circle số bước
            var circle = new Panel
            {
                Width = 50,
                Height = 50,
                Location = new Point(i == 1 ? 80 : (i == 2 ? 250 : 420), 15),
                BackColor = isActive ? Color.FromArgb(36, 99, 235) : Color.FromArgb(220, 225, 235),
                BorderStyle = BorderStyle.FixedSingle
            };
            var lbStep = new Label
            {
                Text = i.ToString(),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18f, FontStyle.Bold)
            };
            circle.Controls.Add(lbStep);
            circle.Paint += (s, e) =>
            {
                var p = (Panel)s;
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var rect = p.ClientRectangle; rect.Width--; rect.Height--;
                using var path = RoundedRect(rect, 25);
                using var b = new SolidBrush(p.BackColor);
                g.FillPath(b, path);
            };
            stepIndicator.Controls.Add(circle);

            // Nối dây (giữa các circle)
            if (i < 3)
            {
                var lineColor = i < currentStep ? Color.FromArgb(36, 99, 235) : Color.FromArgb(220, 225, 235);
                var line = new Panel
                {
                    Width = 120,
                    Height = 3,
                    Location = new Point(130 + (i - 1) * 170, 37),
                    BackColor = lineColor
                };
                stepIndicator.Controls.Add(line);
            }
        }

        return stepIndicator;
    }

    private void BuildStep3()
    {
        step3 = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24) };

        // ===== Step indicator (1-2-3) =====
        var stepIndicator = new Panel { Dock = DockStyle.Top, Height = 80, AutoSize = false, Margin = new Padding(0, 0, 0, 24) };

        for (int i = 1; i <= 3; i++)
        {
            // Circle số bước
            var circle = new Panel
            {
                Width = 50,
                Height = 50,
                Location = new Point(i == 1 ? 50 : (i == 2 ? 220 : 390), 15),
                BackColor = i == 3 ? Color.FromArgb(36, 99, 235) : (i < 3 ? Color.FromArgb(36, 99, 235) : Color.White),
                BorderStyle = BorderStyle.FixedSingle
            };
            var lbStep = new Label { Text = i.ToString(), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.White, Font = new Font("Segoe UI", 18f, FontStyle.Bold) };
            circle.Controls.Add(lbStep);
            circle.Paint += (s, e) =>
            {
                var p = (Panel)s;
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var rect = p.ClientRectangle; rect.Width--; rect.Height--;
                using var path = RoundedRect(rect, 25);
                using var b = new SolidBrush(p.BackColor);
                g.FillPath(b, path);
            };
            stepIndicator.Controls.Add(circle);

            // Nối dây (giữa các circle)
            if (i < 3)
            {
                var line = new Panel { Width = 170, Height = 3, Location = new Point(100 + (i - 1) * 170, 37), BackColor = Color.FromArgb(36, 99, 235) };
                stepIndicator.Controls.Add(line);
            }
        }

        // ===== Điều khoản cọc =====
        var rules = new RoundedPanel
        {
            Radius = 12,
            FillColor = Color.FromArgb(240, 244, 255),
            BorderColor = Color.FromArgb(220, 230, 250),
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            Padding = new Padding(16, 12, 16, 12),
            Margin = new Padding(0, 0, 0, 20),
            Width = 760
        };
        var rulesLabel = new Label
        {
            Text = "Điều khoản cọc\n• Cọc tối thiểu: 20% tổng giá trị hợp đồng\n• Thời hạn cọc đợt 1: Trong vòng 7 ngày\n• Thời hạn cọc đợt 2: Trước sự kiện 30 ngày\n• Thời hạn thanh toán còn lại: Trước sự kiện 3 ngày",
            AutoSize = true,
            Font = new Font("Segoe UI", 10f)
        };
        rules.Controls.Add(rulesLabel);

        // ===== Form nhập liệu =====
        var lblCoc = L("Số tiền cọc đợt 1");
        numCoc1 = new NumericUpDown { Maximum = 9_000_000_000, ThousandsSeparator = true };
        var lblPT = L("Phương thức thanh toán");
        cboPTTT = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cboPTTT.Items.AddRange(new[] { "Tiền mặt", "Chuyển khoản", "QR" });

        var grid = new TableLayoutPanel { ColumnCount = 2, RowCount = 2, AutoSize = true, Width = 760 };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
        AddRow(grid, lblCoc, WrapInput(numCoc1, 280), 0);
        AddRow(grid, lblPT, WrapInput(cboPTTT, 280), 1);
        grid.Dock = DockStyle.Top;
        grid.Margin = new Padding(0, 0, 0, 20);

        // ===== Info thông báo =====
        var info = new RoundedPanel
        {
            Radius = 10,
            Height = 80,
            FillColor = Color.FromArgb(227, 255, 237),
            BorderColor = Color.FromArgb(200, 235, 215),
            Padding = new Padding(16),
            Dock = DockStyle.Top,
            Margin = new Padding(0),
            Width = 760
        };
        var infoLabel = new Label
        {
            Text = "🔒 Phiếu đặt sảnh sẽ được tạo\nSau khi xác nhận, bạn có thể tạo hợp đồng chính thức từ phiếu đặt sảnh này",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.FromArgb(21, 92, 51),
            Font = new Font("Segoe UI", 10f)
        };
        info.Controls.Add(infoLabel);

        // ===== Wrapper (container chứa hết) =====
        var wrapper = new Panel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Width = 760 };
        wrapper.Controls.Add(info);
        wrapper.Controls.Add(grid);
        wrapper.Controls.Add(rules);

        step3.Controls.Add(wrapper);
        step3.Controls.Add(stepIndicator);
    }

    // Helper để vẽ hình tròn bo góc
    private System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        int d = radius * 2;
        var gp = new System.Drawing.Drawing2D.GraphicsPath();
        gp.AddArc(r.X, r.Y, d, d, 180, 90);
        gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        gp.CloseFigure();
        return gp;
    }
    #endregion

    #region Helpers (UI + layout)
    private Button MakePrimary(string text, int width)
    {
        var b = new Button { Text = text, Width = width, Height = 44, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(36, 99, 235), ForeColor = Color.White };
        b.FlatAppearance.BorderSize = 0;
        b.Paint += (s, e) =>
        {
            var r = ((Button)s).ClientRectangle;
            using var gp = new System.Drawing.Drawing2D.GraphicsPath();
            int radius = 20;
            gp.AddArc(r.Left, r.Top, radius, radius, 180, 90);
            gp.AddArc(r.Right - radius, r.Top, radius, radius, 270, 90);
            gp.AddArc(r.Right - radius, r.Bottom - radius, radius, radius, 0, 90);
            gp.AddArc(r.Left, r.Bottom - radius, radius, radius, 90, 90);
            gp.CloseAllFigures();
            ((Button)s).Region = new Region(gp);
        };
        return b;
    }

    private Label L(string text) => new Label { Text = text, AutoSize = true, Margin = new Padding(0, 12, 8, 6) };

    // bọc input vào RoundedPanel bo tròn
    private Control WrapInput(Control inner, int width = 360, int height = 38)
    {
        var host = new RoundedPanel { Width = width, Height = height, Radius = 12 };
        if (inner is TextBox tb) { tb.BorderStyle = BorderStyle.None; tb.Font = new Font("Segoe UI", 10.5f); tb.Dock = DockStyle.Fill; }
        else if (inner is ComboBox cb) { cb.FlatStyle = FlatStyle.Flat; cb.Font = new Font("Segoe UI", 10.5f); cb.Dock = DockStyle.Fill; cb.Margin = new Padding(0); }
        else if (inner is DateTimePicker dtp) { dtp.Font = new Font("Segoe UI", 10.5f); dtp.Dock = DockStyle.Fill; }
        else if (inner is NumericUpDown nud) { nud.BorderStyle = BorderStyle.None; nud.Font = new Font("Segoe UI", 10.5f); nud.Dock = DockStyle.Fill; nud.ThousandsSeparator = true; }
        host.Controls.Add(inner);
        return host;
    }

    private void AddRow(TableLayoutPanel t, Control left, Control right, int row)
    {
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var p = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
        p.Controls.Add(right);
        t.Controls.Add(left, 0, row);
        t.Controls.Add(p, 1, row);
    }
    #endregion

    #region Navigation + validation
    private void GoStep(int delta)
    {
        _step = Math.Max(1, Math.Min(3, _step + delta));

        step1.Visible = _step == 1;
        step2.Visible = _step == 2;
        step3.Visible = _step == 3;

        if (step1.Visible) step1.BringToFront();
        if (step2.Visible) step2.BringToFront();
        if (step3.Visible) step3.BringToFront();

        btnBack.Enabled = _step > 1;
        btnNext.Visible = _step < 3;
        btnCreate.Visible = _step == 3;

        var sub = pnlHeader.Controls.OfType<Label>().First(); // phụ đề
        sub.Text = _step == 1 ? "Bước 1/3: Chọn sảnh & thời gian"
               : _step == 2 ? "Bước 2/3: Khách hàng, gói tiệc"
                            : "Bước 3/3: Điều khoản, cọc";
        if (_step == 2) UpdateSubtotal();
    }

    private bool ValidateStep()
    {
        if (_step == 1)
        {
            if (cboCN.SelectedItem == null || cboSanh.SelectedItem == null || cboCa.SelectedItem == null)
            { MessageBox.Show("Vui lòng chọn Chi nhánh, Sảnh và Ca."); return false; }
            if (_tables <= 0) { MessageBox.Show("Vui lòng nhập số bàn dự kiến."); return false; }
            if (!IsAvailable()) { MessageBox.Show("Sảnh đã bận ở thời điểm này."); return false; }
            return true;
        }
        if (_step == 2)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text)) { MessageBox.Show("Vui lòng nhập tên khách hàng."); return false; }
            if (_pkgPrice <= 0) { MessageBox.Show("Vui lòng chọn gói tiệc."); return false; }
            return true;
        }
        return true;
    }
    #endregion

    #region Data & business
    private void ParseTables()
    {
        if (int.TryParse(txtSoBan.Text.Trim(), out var n) && n >= 0) _tables = n;
        UpdateSubtotal();
    }

    private void LoadCombos()
    {
        cboCN.DataSource = null; cboCN.DisplayMember = "Name"; cboCN.ValueMember = "Id"; cboCN.DataSource = _branches;
        cboCa.DataSource = null; cboCa.DisplayMember = "Name"; cboCa.ValueMember = "Id"; cboCa.DataSource = _shifts;
        LoadHalls();
    }

    private void LoadHalls()
    {
        if (cboCN.SelectedValue == null) return;
        int cn = (int)cboCN.SelectedValue;
        var ds = _halls.Where(h => h.BranchId == cn).ToList();

        cboSanh.DataSource = null; cboSanh.DisplayMember = "Name"; cboSanh.ValueMember = "Id"; cboSanh.DataSource = ds;
        UpdateAvailability();
    }

    private void LoadPackages()
    {
        flGoi.Controls.Clear();

        // Lưu danh sách RadioButton để manage
        var radioButtons = new List<RadioButton>();

        foreach (var p in _packages)
        {
            var card = new RoundedPanel
            {
                Width = 760,
                Height = 74,
                Margin = new Padding(0, 0, 0, 12),
                Radius = 12,
                BorderColor = Color.FromArgb(230, 233, 240),
                FillColor = Color.White,
                Padding = new Padding(16, 8, 16, 8)
            };

            // RadioButton - không cần parent panel group
            var rb = new RadioButton
            {
                Text = "",  // Để trống text
                AutoSize = false,
                Width = 20,
                Height = 20,
                Location = new Point(6, 27),
                Tag = p,
                Visible = true
            };

            // Thêm vào list để manage
            radioButtons.Add(rb);

            var lbName = new Label
            {
                Text = p.Name,
                AutoSize = true,
                Location = new Point(32, 22),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular)
            };

            var lbGia = new Label
            {
                Text = $"{p.PricePerTable:N0} đ/bàn",
                AutoSize = true,
                Location = new Point(300, 22),
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 10.5f)
            };

            var btn = new Button
            {
                Text = "Xem chi tiết",
                Width = 120,
                Height = 34,
                Location = new Point(card.Width - 140, 18),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.WhiteSmoke
            };
            btn.FlatAppearance.BorderSize = 0;

            rb.CheckedChanged += (s, e) =>
            {
                if (rb.Checked)
                {
                    // Bỏ chọn tất cả các RadioButton khác
                    foreach (var otherRb in radioButtons)
                    {
                        if (otherRb != rb)
                            otherRb.Checked = false;
                    }

                    _pkgPrice = ((Package)rb.Tag).PricePerTable;
                    UpdateSubtotal();

                    // Đổi màu card khi được chọn
                    card.FillColor = Color.FromArgb(242, 246, 255);
                    card.BorderColor = Color.FromArgb(36, 99, 235);
                }
                else
                {
                    // Đổi lại màu card khi bị bỏ chọn
                    card.FillColor = Color.White;
                    card.BorderColor = Color.FromArgb(230, 233, 240);
                }
            };

            // Event cho nút "Xem chi tiết"
            btn.Click += (s, e) =>
            {
                var detailForm = new FrmPackageDetail(p.Name, p.PricePerTable, 10);
                if (detailForm.ShowDialog(this) == DialogResult.OK)
                {
                    rb.Checked = true; // Chọn gói này
                }
            };

            // Thêm tất cả control vào card
            card.Controls.Add(rb);
            card.Controls.Add(lbName);
            card.Controls.Add(lbGia);
            card.Controls.Add(btn);

            // Thêm card vào FlowLayoutPanel
            flGoi.Controls.Add(card);
        }
    }
    private bool IsAvailable()
    {
        if (cboSanh.SelectedItem == null || cboCa.SelectedItem == null) return true;
        int hallId = ((Hall)cboSanh.SelectedItem).Id;
        int shiftId = ((Shift)cboCa.SelectedItem).Id;
        DateTime day = dtNgay.Value.Date;
        return !_reservations.Any(r => r.HallId == hallId && r.ShiftId == shiftId && r.Date.Date == day);
    }

    private void UpdateAvailability()
    {
        _hallFee = (cboSanh.SelectedItem is Hall h) ? h.BaseFee : 0m;

        if (IsAvailable())
        {
            lbAvail.Text = "✓ Sảnh còn trống trong thời gian này";
            lbAvail.ForeColor = Color.FromArgb(12, 83, 35);
            (lbAvail.Parent as RoundedPanel)!.FillColor = Color.FromArgb(230, 255, 236);
        }
        else
        {
            lbAvail.Text = "⚠ Sảnh đã có đặt. Vui lòng chọn sảnh/ca/ngày khác";
            lbAvail.ForeColor = Color.FromArgb(120, 40, 0);
            (lbAvail.Parent as RoundedPanel)!.FillColor = Color.FromArgb(255, 243, 234);
        }
        UpdateSubtotal();
    }

    private void UpdateSubtotal()
    {
        lbTamPhiSanh.Text = $"Phí sảnh: {_hallFee:N0} đ";
        lbTamGoi.Text = $"Gói tiệc ({_tables} bàn): {(_pkgPrice * _tables):N0} đ";
        lbTamTong.Text = $"Tổng: {(_hallFee + _pkgPrice * _tables):N0} đ";
    }

    private void CreateReservation()
    {
        var total = _hallFee + _pkgPrice * _tables;
        var minDep = Math.Round(total * 0.20m, 0);
        if (numCoc1.Value > 0 && numCoc1.Value < minDep)
        {
            var ask = MessageBox.Show($"Cọc tối thiểu 20% = {minDep:N0} đ. Vẫn tạo?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.No) return;
        }

        _reservations.Add(new Reservation
        {
            BranchId = ((Branch)cboCN.SelectedItem).Id,
            HallId = ((Hall)cboSanh.SelectedItem).Id,
            ShiftId = ((Shift)cboCa.SelectedItem).Id,
            Date = dtNgay.Value.Date,
            Tables = _tables,
            CustomerName = txtTenKH.Text.Trim(),
            Phone = txtSDT.Text.Trim(),
            PackageId = GetSelectedPackageId(),
            HallFee = _hallFee,
            PackagePrice = _pkgPrice,
            Deposit = numCoc1.Value,
            PaymentMethod = cboPTTT.Text
        });

        int datSanhId = _reservations.Count; // Giả sử ID = số lượng phiếu

        // Mở form tạo hợp đồng
        var contractForm = new FrmCreateContract(datSanhId, txtTenKH.Text.Trim(), total);
        if (contractForm.ShowDialog(this) == DialogResult.OK)
        {
            // Người dùng chọn tạo hợp đồng
            MessageBox.Show(
                "✅ Hợp đồng đã được tạo thành công!\n\n" +
                $"Khách: {txtTenKH.Text}\n" +
                $"Sảnh: {((Hall)cboSanh.SelectedItem).Name}\n" +
                $"Tổng: {total:N0} đ\n\n" +
                "File PDF hợp đồng đã sẵn sàng để tải xuống.",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            // Người dùng chọn để sau
            MessageBox.Show(
                "✅ Phiếu đặt sảnh đã được tạo!\n\n" +
                "Bạn có thể tạo hợp đồng sau từ danh sách phiếu.",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        GoStep(-2); // quay lại bước 1
        ParseTables();
    }    private int? GetSelectedPackageId()
    {
        foreach (var rb in flGoi.Controls.OfType<RoundedPanel>()
                 .SelectMany(p => p.Controls.OfType<RadioButton>()))
            if (rb.Checked) return ((Package)rb.Tag).Id;
        return null;
    }
    #endregion
}
