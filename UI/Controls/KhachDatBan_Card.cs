using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    public enum BookingStatus
    {
        Confirmed,      // Đã xác nhận
        Pending,        // Chờ xác nhận
        Cancelled       // Đã hủy
    }
    [SupportedOSPlatform("windows")]
    public partial class KhachDatBan_Card : UserControl
    {
        private Label lblName;
        private Label lblVerified;
        private Label lblPhone;
        private Label lblDateTime;
        private Label lblLocation;
        private Label lblGuests;
        private Button btnArrived;
        private Button btnCancel;
        private Button btnDetails;

        private string customerName = "Nguyễn Văn A";
        private string phoneNumber = "0901234567";
        private DateTime bookingDateTime = DateTime.Now;
        private string tableLocation = "Bàn A05 - Khu A";
        private int guestCount = 4;
        private BookingStatus status = BookingStatus.Confirmed;
        private int cornerRadius = 12;

        public KhachDatBan_Card()
        {
            InitializeComponent();
            UpdateStatusDisplay();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main container
            this.BackColor = Color.White;
            this.Size = new Size(700, 95);
            this.MinimumSize = new Size(600, 95);
            this.Padding = new Padding(12, 10, 12, 10);

            // Customer name label
            lblName = new Label
            {
                Text = customerName,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(12, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblName);

            // Verified badge
            lblVerified = new Label
            {
                Text = "✓ Đã xác nhận",
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(25, 118, 210),
                BackColor = Color.FromArgb(227, 242, 253),
                Location = new Point(110, 14),
                AutoSize = true,
                Padding = new Padding(5, 1, 5, 1)
            };
            this.Controls.Add(lblVerified);

            // Phone icon and number
            Label lblPhoneIcon = new Label
            {
                Text = "📞",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(12, 38),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblPhoneIcon);

            lblPhone = new Label
            {
                Text = phoneNumber,
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(32, 39),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblPhone);

            // Calendar icon and datetime
            Label lblCalendarIcon = new Label
            {
                Text = "📅",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(280, 38),
                AutoSize = true,
                Anchor = AnchorStyles.Top,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblCalendarIcon);

            lblDateTime = new Label
            {
                Text = bookingDateTime.ToString("yyyy-MM-dd HH:mm"),
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(300, 39),
                AutoSize = true,
                Anchor = AnchorStyles.Top,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblDateTime);

            // Location icon and address
            Label lblLocationIcon = new Label
            {
                Text = "📍",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(12, 62),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblLocationIcon);

            lblLocation = new Label
            {
                Text = tableLocation,
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(32, 63),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblLocation);

            // Guest icon and count
            Label lblGuestIcon = new Label
            {
                Text = "👥",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(280, 62),
                AutoSize = true,
                Anchor = AnchorStyles.Top,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblGuestIcon);

            lblGuests = new Label
            {
                Text = $"{guestCount} khách",
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(300, 63),
                AutoSize = true,
                Anchor = AnchorStyles.Top,
                BackColor = Color.Transparent
            };
            this.Controls.Add(lblGuests);

            // Button "Đã đến"
            btnArrived = new Button
            {
                Text = "Đã đến",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                Size = new Size(75, 30),
                Location = new Point(480, 12),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnArrived.FlatAppearance.BorderSize = 0;
            btnArrived.Click += BtnArrived_Click;
            this.Controls.Add(btnArrived);

            // Button "Hủy"
            btnCancel = new Button
            {
                Text = "✕ Hủy",
                Font = new Font("Segoe UI", 8.5F),
                Size = new Size(75, 30),
                Location = new Point(480, 50),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(211, 47, 47),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(239, 239, 239);
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);

            // Button "Chi tiết"
            btnDetails = new Button
            {
                Text = "Chi tiết",
                Font = new Font("Segoe UI", 8.5F),
                Size = new Size(75, 30),
                Location = new Point(560, 50),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(66, 66, 66),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnDetails.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            btnDetails.Click += BtnDetails_Click;
            this.Controls.Add(btnDetails);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Override Paint để bo tròn góc
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Tạo path bo tròn
            GraphicsPath path = GetRoundedRectanglePath(this.ClientRectangle, cornerRadius);

            // Vẽ nền
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillPath(brush, path);
            }

            // Vẽ viền
            using (Pen pen = new Pen(Color.FromArgb(224, 224, 224), 1))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        // Tạo GraphicsPath cho hình chữ nhật bo tròn
        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Góc trên trái
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // Góc trên phải
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // Góc dưới phải
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // Góc dưới trái
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        // Override Region để click events hoạt động đúng với góc bo tròn
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GraphicsPath path = GetRoundedRectanglePath(this.ClientRectangle, cornerRadius);
            this.Region = new Region(path);
        }

        // Cập nhật hiển thị trạng thái
        private void UpdateStatusDisplay()
        {
            switch (status)
            {
                case BookingStatus.Confirmed:
                    lblVerified.Text = "✓ Đã xác nhận";
                    lblVerified.ForeColor = Color.FromArgb(25, 118, 210);
                    lblVerified.BackColor = Color.FromArgb(227, 242, 253);
                    btnArrived.Enabled = true;
                    btnCancel.Enabled = true;
                    break;

                case BookingStatus.Pending:
                    lblVerified.Text = "⏱ Chờ xác nhận";
                    lblVerified.ForeColor = Color.FromArgb(237, 108, 2);
                    lblVerified.BackColor = Color.FromArgb(255, 243, 224);
                    btnArrived.Enabled = false;
                    btnCancel.Enabled = true;
                    break;

                case BookingStatus.Cancelled:
                    lblVerified.Text = "✕ Đã hủy";
                    lblVerified.ForeColor = Color.FromArgb(211, 47, 47);
                    lblVerified.BackColor = Color.FromArgb(255, 235, 238);
                    btnArrived.Enabled = false;
                    btnCancel.Enabled = false;
                    break;
            }
            this.Invalidate();
        }

        // Event handlers
        private void BtnArrived_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Khách đã đến!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn hủy đặt chỗ này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Status = BookingStatus.Cancelled;
                MessageBox.Show("Đã hủy đặt chỗ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hiển thị chi tiết đặt chỗ", "Chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Public Properties
        public string CustomerName
        {
            get => customerName;
            set
            {
                customerName = value;
                if (lblName != null) lblName.Text = value;
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                if (lblPhone != null) lblPhone.Text = value;
            }
        }

        public DateTime BookingDateTime
        {
            get => bookingDateTime;
            set
            {
                bookingDateTime = value;
                if (lblDateTime != null) lblDateTime.Text = value.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string TableLocation
        {
            get => tableLocation;
            set
            {
                tableLocation = value;
                if (lblLocation != null) lblLocation.Text = value;
            }
        }

        public int GuestCount
        {
            get => guestCount;
            set
            {
                guestCount = value;
                if (lblGuests != null) lblGuests.Text = $"{value} khách";
            }
        }

        public BookingStatus Status
        {
            get => status;
            set
            {
                status = value;
                UpdateStatusDisplay();
            }
        }

        public int CornerRadius
        {
            get => cornerRadius;
            set
            {
                cornerRadius = value;
                this.Invalidate();
                OnResize(EventArgs.Empty);
            }
        }
    }
}