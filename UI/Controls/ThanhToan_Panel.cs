using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public partial class ThanhToan_Panel : UserControl
    {
        private decimal subTotal = 850000;
        private decimal vatPercent = 10;
        private string selectedPaymentMethod = "CK";

        // Controls
        private Label lblTitle;
        private Label lblDiscount;
        private ComboBox cboDiscountType;
        private RoundedTextBoxControl txtDiscountValue;
        private Button btnVoucher;
        private Label lblPaymentMethod;
        private PaymentMethodButton btnCash;
        private PaymentMethodButton btnCard;
        private PaymentMethodButton btnBankTransfer;
        private Label lblSeparateBill;
        private ComboBox cboSeparateBill;
        private Label lblTotalLabel;
        private Label lblTotalValue;
        private Button btnCheckout;
        private Button btnPrint;

        public ThanhToan_Panel()
        {
            InitializeComponent();
            SetupControls();
            UpdateTotals();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Size = new Size(400, 600);
            this.BackColor = Color.White;
            this.Padding = new Padding(20);
            this.ResumeLayout(false);
        }

        private void SetupControls()
        {
            int yPos = 15;

            // Title
            lblTitle = CreateLabel("Thanh toán - Bàn A01", new Point(20, yPos), new Size(360, 30),
                new Font("Segoe UI", 13, FontStyle.Bold));
            this.Controls.Add(lblTitle);
            yPos += 45;

            // Discount Section
            lblDiscount = CreateLabel("Giảm giá", new Point(20, yPos), new Size(360, 20),
                new Font("Segoe UI", 9, FontStyle.Bold));
            this.Controls.Add(lblDiscount);
            yPos += 25;

            // Discount Type ComboBox
            cboDiscountType = new RoundedComboBox
            {
                Location = new Point(20, yPos),
                Size = new Size(70, 32),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9)
            };
            cboDiscountType.Items.AddRange(new object[] { "%", "₫" });
            cboDiscountType.SelectedIndex = 0;
            this.Controls.Add(cboDiscountType);

            // Discount Value TextBox
            txtDiscountValue = new RoundedTextBoxControl
            {
                Location = new Point(100, yPos),
                Size = new Size(140, 32),
                Text = "0",
                TextAlign = HorizontalAlignment.Center
            };
            txtDiscountValue.TextChanged += (s, e) => UpdateTotals();
            this.Controls.Add(txtDiscountValue);

            // Voucher Button
            btnVoucher = new RoundedStandardButton
            {
                Location = new Point(250, yPos),
                Size = new Size(130, 32),
                Text = "🎫 Voucher",
                Font = new Font("Segoe UI", 9)
            };
            this.Controls.Add(btnVoucher);
            yPos += 45;

            // Payment Method Section
            lblPaymentMethod = CreateLabel("Phương thức thanh toán", new Point(20, yPos),
                new Size(360, 20), new Font("Segoe UI", 9, FontStyle.Bold));
            this.Controls.Add(lblPaymentMethod);
            yPos += 25;

            // Payment Method Buttons
            int btnWidth = 110;
            int btnSpacing = 15;
            btnCash = CreatePaymentButton("💵\nTiền mặt", new Point(20, yPos), "Cash");
            btnCard = CreatePaymentButton("💳\nThẻ", new Point(20 + btnWidth + btnSpacing, yPos), "Card");
            btnBankTransfer = CreatePaymentButton("$\nCK", new Point(20 + (btnWidth + btnSpacing) * 2, yPos), "CK");
            btnBankTransfer.IsSelected = true;

            this.Controls.AddRange(new Control[] { btnCash, btnCard, btnBankTransfer });
            yPos += 75;

            // Separate Bill Section
            lblSeparateBill = CreateLabel("Tách hóa đơn", new Point(20, yPos),
                new Size(360, 20), new Font("Segoe UI", 9, FontStyle.Bold));
            this.Controls.Add(lblSeparateBill);
            yPos += 25;

            cboSeparateBill = new RoundedComboBox
            {
                Location = new Point(20, yPos),
                Size = new Size(360, 32),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9)
            };
            cboSeparateBill.Items.AddRange(new object[] {
                "🔀  Không tách",
                "Tách 2",
                "Tách 3",
                "Tách 4"
            });
            cboSeparateBill.SelectedIndex = 0;
            this.Controls.Add(cboSeparateBill);
            yPos += 45;

            // Summary - Only show Total
            Panel pnlTotal = new Panel
            {
                Location = new Point(20, yPos),
                Size = new Size(360, 50),
                BackColor = Color.FromArgb(248, 248, 250)
            };

            lblTotalLabel = new Label
            {
                Text = "Tạm tính\nVAT (10%)\nTổng cộng",
                Location = new Point(15, 8),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 8.5f),
                BackColor = Color.Transparent
            };

            lblTotalValue = new Label
            {
                Text = "850.000 ₫\n85.000 ₫\n935.000 ₫",
                Location = new Point(200, 8),
                Size = new Size(145, 35),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.TopRight,
                BackColor = Color.Transparent
            };

            pnlTotal.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedRect(pnlTotal.ClientRectangle, 8))
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(248, 248, 250)))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            };

            pnlTotal.Controls.AddRange(new Control[] { lblTotalLabel, lblTotalValue });
            this.Controls.Add(pnlTotal);
            yPos += 60;

            // Checkout Button
            btnCheckout = new Button
            {
                Location = new Point(20, yPos),
                Size = new Size(360, 48),
                Text = "Thanh toán 935.000 ₫",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(15, 15, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedRect(btnCheckout.ClientRectangle, 10))
                {
                    using (SolidBrush brush = new SolidBrush(btnCheckout.BackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
                TextRenderer.DrawText(e.Graphics, btnCheckout.Text, btnCheckout.Font,
                    btnCheckout.ClientRectangle, btnCheckout.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            btnCheckout.MouseEnter += (s, e) => { btnCheckout.BackColor = Color.FromArgb(35, 35, 50); btnCheckout.Invalidate(); };
            btnCheckout.MouseLeave += (s, e) => { btnCheckout.BackColor = Color.FromArgb(15, 15, 30); btnCheckout.Invalidate(); };
            btnCheckout.Click += BtnCheckout_Click;
            this.Controls.Add(btnCheckout);
            yPos += 58;

            // Print Button
            btnPrint = new RoundedStandardButton
            {
                Location = new Point(20, yPos),
                Size = new Size(360, 38),
                Text = "🖨️  In hóa đơn",
                Font = new Font("Segoe UI", 9)
            };
            this.Controls.Add(btnPrint);
        }

        private Label CreateLabel(string text, Point location, Size size, Font font)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Size = size,
                Font = font,
                BackColor = Color.Transparent
            };
        }

        private PaymentMethodButton CreatePaymentButton(string text, Point location, string method)
        {
            var btn = new PaymentMethodButton();
            btn.Location = location;
            btn.Size = new Size(110, 65);
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 8.5f);
            btn.PaymentMethod = method;
            btn.Click += PaymentButton_Click;
            return btn;
        }

        private void PaymentButton_Click(object sender, EventArgs e)
        {
            var btn = sender as PaymentMethodButton;
            btnCash.IsSelected = false;
            btnCard.IsSelected = false;
            btnBankTransfer.IsSelected = false;
            btn.IsSelected = true;
            selectedPaymentMethod = btn.PaymentMethod;
        }

        private void UpdateTotals()
        {
            decimal discount = 0;
            if (decimal.TryParse(txtDiscountValue.Text, out decimal discountValue))
            {
                if (cboDiscountType.SelectedIndex == 0)
                    discount = subTotal * (discountValue / 100);
                else
                    discount = discountValue;
            }

            decimal afterDiscount = subTotal - discount;
            decimal vat = afterDiscount * (vatPercent / 100);
            decimal total = afterDiscount + vat;

            lblTotalValue.Text = $"{afterDiscount:N0} ₫\n{vat:N0} ₫\n{total:N0} ₫";
            btnCheckout.Text = $"Thanh toán {total:N0} ₫";
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Thanh toán thành công!\nPhương thức: {selectedPaymentMethod}",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        public decimal SubTotal
        {
            get => subTotal;
            set { subTotal = value; UpdateTotals(); }
        }

        public decimal VATPercent
        {
            get => vatPercent;
            set { vatPercent = value; UpdateTotals(); }
        }
    }

    // Rounded Button for Payment Methods
    public class PaymentMethodButton : Button
    {
        private bool isSelected = false;
        public string PaymentMethod { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                this.Invalidate();
            }
        }
        [SupportedOSPlatform("windows")]
        public PaymentMethodButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Cursor = Cursors.Hand;
            this.BackColor = Color.White;
            this.ForeColor = Color.FromArgb(70, 70, 70);
        }
        [SupportedOSPlatform("windows")]
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GetRoundedRectPath(this.ClientRectangle, 10))
            {
                Color bgColor = isSelected ? Color.FromArgb(15, 15, 30) : Color.White;
                using (SolidBrush brush = new SolidBrush(bgColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                Color borderColor = isSelected ? Color.FromArgb(15, 15, 30) : Color.FromArgb(230, 230, 230);
                using (Pen pen = new Pen(borderColor, 1.5f))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            Color textColor = isSelected ? Color.White : Color.FromArgb(70, 70, 70);
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle,
                textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
        [SupportedOSPlatform("windows")]
        private GraphicsPath GetRoundedRectPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // Rounded TextBox
    [SupportedOSPlatform("windows")]
    public class RoundedTextBoxControl : Panel
    {
        private TextBox innerTextBox;

        public RoundedTextBoxControl()
        {
            this.BackColor = Color.FromArgb(248, 248, 250);
            this.Padding = new Padding(8, 6, 8, 6);

            innerTextBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 248, 250),
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9)
            };
            this.Controls.Add(innerTextBox);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = GetRoundedRectPath(this.ClientRectangle, 6))
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(248, 248, 250)))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius - 1, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius - 1, bounds.Bottom - radius - 1, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius - 1, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        public override string Text
        {
            get => innerTextBox.Text;
            set => innerTextBox.Text = value;
        }

        public HorizontalAlignment TextAlign
        {
            get => innerTextBox.TextAlign;
            set => innerTextBox.TextAlign = value;
        }

        public new event EventHandler TextChanged
        {
            add => innerTextBox.TextChanged += value;
            remove => innerTextBox.TextChanged -= value;
        }
    }
    [SupportedOSPlatform("windows")]
    // Rounded ComboBox
    public class RoundedComboBox : ComboBox
    {
        public RoundedComboBox()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = Color.FromArgb(248, 248, 250);
            this.ForeColor = Color.FromArgb(70, 70, 70);
        }
    }
    [SupportedOSPlatform("windows")]
    // Rounded Standard Button
    public class RoundedStandardButton : Button
    {
        public RoundedStandardButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Cursor = Cursors.Hand;
            this.BackColor = Color.White;
            this.ForeColor = Color.FromArgb(70, 70, 70);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = GetRoundedRectPath(this.ClientRectangle, 8))
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1.5f))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle,
                this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.Right - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.Right - radius, bounds.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}