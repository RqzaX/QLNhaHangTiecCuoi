using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    public enum DiscountKind { Percent, Amount }
    public enum PayMethod { Cash, Card, Bank }
    [SupportedOSPlatform("windows")]
    public class ThanhToan_Panel : UserControl
    {
        // ===== Theme =====
        private static readonly Color Midnight = Color.FromArgb(15, 23, 42);   // #0F172A
        private static readonly Color Slate800 = Color.FromArgb(30, 41, 59);
        private static readonly Color Slate600 = Color.FromArgb(71, 85, 105);
        private static readonly Color Slate500 = Color.FromArgb(100, 116, 139);
        private static readonly Color Border = Color.FromArgb(226, 232, 240);
        private static readonly Color Ring = Color.FromArgb(203, 213, 225);
        private static readonly Color Surface = Color.White;
        private static readonly Font FRegular = new Font("Segoe UI", 10f, FontStyle.Regular);
        private static readonly Font FSemiBold = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
        private static readonly Font FTitle = new Font("Segoe UI Semibold", 12f, FontStyle.Bold);

        // ===== Public API =====
        private decimal _subtotal = 850_000m;
        [Category("Data")] public decimal Subtotal { get => _subtotal; set { _subtotal = Math.Max(0, value); Recalc(); } }

        private decimal _vatRate = 0.10m;
        [Category("Data")] public decimal VatRate { get => _vatRate; set { _vatRate = Math.Max(0, value); Recalc(); } }

        private DiscountKind _discountType = DiscountKind.Percent;
        [Category("Data")] public DiscountKind DiscountType { get => _discountType; set { _discountType = value; SyncDiscountUI(); Recalc(); } }

        private decimal _discountValue = 0m;
        [Category("Data")] public decimal DiscountValue { get => _discountValue; set { _discountValue = Math.Max(0, value); SyncDiscountUI(); Recalc(); } }

        private int _splitCount = 1;
        [Category("Data")] public int SplitCount { get => _splitCount; set { _splitCount = Math.Max(1, value); SyncSplitUI(); Recalc(); } }

        private PayMethod _method = PayMethod.Cash;
        [Category("Data")] public PayMethod PaymentMethod { get => _method; set { _method = value; SyncMethodUI(); PaymentMethodChanged?.Invoke(this, EventArgs.Empty); } }

        // Events
        [Category("Action")] public event EventHandler PayClicked;
        [Category("Action")] public event EventHandler PrintClicked;
        [Category("Action")] public event EventHandler EmailClicked;
        [Category("Action")] public event EventHandler PaymentMethodChanged;

        // ===== UI =====
        private CardPanel card;
        private Label lblTitle;

        // Discount
        private SegButton segPercent, segAmount;
        private NumericUpDown numDiscount;
        private PillButton btnVoucher;

        // Pay methods
        private PillButton btnCash, btnCard, btnBank;

        // Split
        private ComboHost cboSplitHost;
        private ComboBox cboSplit;

        // Summary
        private GroupPanel summary;
        private Label vSub, vVat, vTotal, vPerTitle, vPerValue;

        // Bottom actions
        private PrimaryButton btnPay;
        private GhostButton btnPrint, btnEmail;

        public ThanhToan_Panel()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Padding = new Padding(16);

            card = new CardPanel { Dock = DockStyle.Fill, Corner = 18, Shadow = true };
            Controls.Add(card);

            // Title
            lblTitle = new Label
            {
                AutoSize = true,
                Text = "Thanh toán - Bàn A01",
                Font = FTitle,
                ForeColor = Slate800,
                Location = new Point(24, 22)
            };
            card.Controls.Add(lblTitle);

            // ===== Discount row =====
            var lbDis = MakeLabel("Giảm giá", 24 + 8);
            card.Controls.Add(lbDis);

            segPercent = new SegButton { Text = "%", Checked = true, Location = new Point(24, 86), Size = new Size(54, 36) };
            segAmount = new SegButton { Text = "đ", Location = new Point(segPercent.Right - 1, 86), Size = new Size(54, 36), RightPiece = true };
            segPercent.LeftPiece = true;

            segPercent.Click += (s, e) => DiscountType = DiscountKind.Percent;
            segAmount.Click += (s, e) => DiscountType = DiscountKind.Amount;
            card.Controls.AddRange(new Control[] { segPercent, segAmount });

            numDiscount = new NumericUpDown
            {
                Location = new Point(segAmount.Right + 10, 86),
                Size = new Size(220, 36),
                BorderStyle = BorderStyle.None,
                Maximum = 100_000_000,
                ThousandsSeparator = true,
                TextAlign = HorizontalAlignment.Right,
                Font = FRegular
            };
            var numHost = new InputHost(numDiscount) { Location = new Point(segAmount.Right + 10, 86) };
            card.Controls.Add(numHost);
            numDiscount.ValueChanged += (s, e) => DiscountValue = numDiscount.Value;

            btnVoucher = new PillButton { Text = "🎁  Voucher", Location = new Point(numHost.Right + 12, 86), Size = new Size(130, 36) };
            btnVoucher.Click += (s, e) => MessageBox.Show("Mở danh sách Voucher...", "Voucher");
            card.Controls.Add(btnVoucher);

            // ===== Payment methods =====
            var lbPM = MakeLabel("Phương thức thanh toán", numHost.Bottom + 18 + 8);
            card.Controls.Add(lbPM);

            btnCash = new PillButton { Location = new Point(24, lbPM.Bottom + 8), Size = new Size(170, 44) };
            btnCard = new PillButton { Location = new Point(btnCash.Right + 12, lbPM.Bottom + 8), Size = new Size(170, 44) };
            btnBank = new PillButton { Location = new Point(btnCard.Right + 12, lbPM.Bottom + 8), Size = new Size(170, 44) };

            btnCash.SetIconAndText(DrawCash, "Tiền mặt");
            btnCard.SetIconAndText(DrawCard, "Thẻ");
            btnBank.SetIconAndText(DrawBank, "CK");

            btnCash.Click += (s, e) => PaymentMethod = PayMethod.Cash;
            btnCard.Click += (s, e) => PaymentMethod = PayMethod.Card;
            btnBank.Click += (s, e) => PaymentMethod = PayMethod.Bank;

            card.Controls.AddRange(new Control[] { btnCash, btnCard, btnBank });

            // ===== Split =====
            var lbSplit = MakeLabel("Tách hóa đơn", btnCash.Bottom + 16 + 8);
            card.Controls.Add(lbSplit);

            cboSplit = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Font = FRegular
            };
            cboSplit.Items.Add("Không tách");
            for (int i = 2; i <= 10; i++) cboSplit.Items.Add($"Tách {i}");
            cboSplit.SelectedIndex = 0;
            cboSplit.SelectedIndexChanged += (s, e) =>
            {
                SplitCount = Math.Max(1, cboSplit.SelectedIndex == 0 ? 1 : (cboSplit.SelectedIndex + 1));
            };
            cboSplitHost = new ComboHost(cboSplit) { Location = new Point(24, lbSplit.Bottom + 8), Size = new Size(360, 36) };
            card.Controls.Add(cboSplitHost);

            // ===== Summary box =====
            summary = new GroupPanel { Location = new Point(24, cboSplitHost.Bottom + 16), Size = new Size(560, 168) };
            card.Controls.Add(summary);

            MakeRow(summary, "Tạm tính", out _, out vSub, 0);
            MakeRow(summary, $"VAT ({(int)(VatRate * 100)}%)", out _, out vVat, 1);
            summary.AddDivider(2);
            MakeRow(summary, "Tổng cộng", out _, out vTotal, 3, bold: true);

            // Per person
            MakeRow(summary, "Mỗi người", out vPerTitle, out vPerValue, 4, link: true);
            vPerTitle.Visible = vPerValue.Visible = false;

            // ===== Footer buttons =====
            btnPay = new PrimaryButton { Location = new Point(24, summary.Bottom + 12), Size = new Size(560, 48) };
            btnPay.Click += (s, e) => PayClicked?.Invoke(this, EventArgs.Empty);
            card.Controls.Add(btnPay);

            btnPrint = new GhostButton { Text = "🖨️  In hóa đơn", Location = new Point(24, btnPay.Bottom + 12), Size = new Size(270, 42) };
            btnEmail = new GhostButton { Text = "📧  Gửi email", Location = new Point(btnPrint.Right + 20, btnPay.Bottom + 12), Size = new Size(270, 42) };
            btnPrint.Click += (s, e) => PrintClicked?.Invoke(this, EventArgs.Empty);
            btnEmail.Click += (s, e) => EmailClicked?.Invoke(this, EventArgs.Empty);
            card.Controls.AddRange(new Control[] { btnPrint, btnEmail });

            // Defaults
            SyncDiscountUI();
            SyncMethodUI();
            SyncSplitUI();
            Recalc();
        }

        // ========== Helpers ==========
        private Label MakeLabel(string text, int top)
        {
            return new Label
            {
                AutoSize = true,
                Text = text,
                Font = FRegular,
                ForeColor = Slate800,
                Location = new Point(24, top)
            };
        }

        private void MakeRow(Control parent, string left, out Label l, out Label r, int row, bool bold = false, bool link = false)
        {
            int y = 14 + row * 30;
            l = new Label
            {
                AutoSize = false,
                Location = new Point(16, y),
                Size = new Size(parent.Width - 32, 24),
                Font = bold ? FSemiBold : FRegular,
                ForeColor = Slate600,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = left
            };
            parent.Controls.Add(l);

            r = new Label
            {
                AutoSize = false,
                Location = new Point(parent.Width - 220, y),
                Size = new Size(200, 24),
                Font = bold ? FSemiBold : FRegular,
                ForeColor = link ? Color.FromArgb(37, 99, 235) : Slate800,
                TextAlign = ContentAlignment.MiddleRight
            };
            parent.Controls.Add(r);
        }

        private void SyncDiscountUI()
        {
            segPercent.Checked = DiscountType == DiscountKind.Percent;
            segAmount.Checked = DiscountType == DiscountKind.Amount;
            numDiscount.Maximum = (DiscountType == DiscountKind.Percent) ? 100 : 1000000000;
            numDiscount.DecimalPlaces = 0;
            var v = Math.Max(numDiscount.Minimum, Math.Min(numDiscount.Maximum, DiscountValue));
            numDiscount.Value = v;
        }

        private void SyncMethodUI()
        {
            btnCash.Checked = (PaymentMethod == PayMethod.Cash);
            btnCard.Checked = (PaymentMethod == PayMethod.Card);
            btnBank.Checked = (PaymentMethod == PayMethod.Bank);
        }

        private void SyncSplitUI()
        {
            bool show = SplitCount > 1;
            vPerTitle.Visible = vPerValue.Visible = show;
            cboSplit.SelectedIndexChanged -= null;
            cboSplit.SelectedIndex = (SplitCount <= 1) ? 0 : (SplitCount - 1);
        }

        private void Recalc()
        {
            // discount
            decimal discount = (DiscountType == DiscountKind.Percent)
                ? Math.Round(Subtotal * Math.Min(100m, DiscountValue) / 100m, 0)
                : Math.Min(Subtotal, DiscountValue);

            var baseAfter = Math.Max(0, Subtotal - discount);
            var vat = Math.Round(baseAfter * VatRate, 0);
            var total = baseAfter + vat;

            vSub.Text = Vnd(Subtotal);
            vVat.Text = Vnd(vat);
            vTotal.Text = Vnd(total);

            if (SplitCount > 1)
            {
                var per = Math.Round(total / SplitCount, 0, MidpointRounding.AwayFromZero);
                vPerValue.Text = Vnd(per);
            }

            btnPay.Text = "Thanh toán " + Vnd(total);
        }

        private static string Vnd(decimal value)
        {
            var nfi = (NumberFormatInfo)CultureInfo.GetCultureInfo("vi-VN").NumberFormat.Clone();
            nfi.CurrencySymbol = "đ";
            nfi.CurrencyGroupSeparator = ".";
            nfi.CurrencyDecimalDigits = 0;
            return string.Format(nfi, "{0:C}", value);
        }

        // ========== Custom subcontrols ==========

        private class CardPanel : Panel
        {
            public int Corner { get; set; } = 18;
            public bool Shadow { get; set; } = true;

            public CardPanel()
            {
                BackColor = Surface;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            }
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                e.Graphics.Clear(Parent?.BackColor ?? Color.WhiteSmoke);
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var r = ClientRectangle; r.Inflate(-2, -2);
                var cardRect = new Rectangle(r.X, r.Y, r.Width - 4, r.Height - 4);

                if (Shadow)
                {
                    // soft shadow
                    using var path = Round(cardRect, Corner);
                    using var sb = new SolidBrush(Color.FromArgb(20, 0, 0, 0));
                    var shadowRect = cardRect; shadowRect.Offset(0, 3);
                    using var pathShadow = Round(shadowRect, Corner + 1);
                    e.Graphics.FillPath(sb, pathShadow);
                }

                using var b = new SolidBrush(Surface);
                using var p = new Pen(Border, 1.6f);
                using var gp = Round(cardRect, Corner);
                e.Graphics.FillPath(b, gp);
                e.Graphics.DrawPath(p, gp);
            }

            private GraphicsPath Round(Rectangle rc, int radius)
            {
                int d = radius * 2;
                var gp = new GraphicsPath();
                gp.AddArc(rc.X, rc.Y, d, d, 180, 90);
                gp.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
                gp.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
                gp.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
                gp.CloseFigure();
                return gp;
            }
        }

        private class GroupPanel : Panel
        {
            public GroupPanel()
            {
                BackColor = Surface;
                Size = new Size(560, 160);
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = ClientRectangle; r.Inflate(-1, -1);

                using var gp = Round(r, 16);
                using var b = new SolidBrush(Surface);
                using var p = new Pen(Border, 1.6f);
                e.Graphics.FillPath(b, gp);
                e.Graphics.DrawPath(p, gp);
            }
            public void AddDivider(int afterRow)
            {
                using var g = CreateGraphics();
                var y = 14 + afterRow * 30 + 12;
                var pen = new Pen(Color.FromArgb(231, 235, 240), 1);
                g.DrawLine(pen, 16, y, Width - 16, y);
                pen.Dispose();
            }
            private GraphicsPath Round(Rectangle rc, int radius)
            {
                int d = radius * 2;
                var gp = new GraphicsPath();
                gp.AddArc(rc.X, rc.Y, d, d, 180, 90);
                gp.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
                gp.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
                gp.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
                gp.CloseFigure();
                return gp;
            }
        }

        private class InputHost : Panel
        {
            public InputHost(Control inner)
            {
                BackColor = Surface;
                BorderStyle = BorderStyle.None;
                Size = new Size(220, 36);
                Padding = new Padding(12, 7, 12, 7);
                Controls.Add(inner);
                inner.Dock = DockStyle.Fill;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = ClientRectangle; r.Width -= 1; r.Height -= 1;
                using var gp = Round(r, Height / 2);
                using var b = new SolidBrush(Surface);
                using var p = new Pen(Border, 1.6f);
                e.Graphics.FillPath(b, gp);
                e.Graphics.DrawPath(p, gp);
            }
            private GraphicsPath Round(Rectangle rc, int radius)
            {
                int d = radius * 2;
                var gp = new GraphicsPath();
                gp.AddArc(rc.X, rc.Y, d, d, 180, 90);
                gp.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
                gp.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
                gp.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
                gp.CloseFigure();
                return gp;
            }
        }

        private class ComboHost : InputHost
        {
            public ComboHost(ComboBox cb) : base(cb)
            {
                cb.Margin = new Padding(0);
            }
        }

        private class SegButton : Button
        {
            public bool Checked { get; set; }
            public bool LeftPiece { get; set; }
            public bool RightPiece { get; set; }

            public SegButton()
            {
                FlatStyle = FlatStyle.Flat;
                FlatAppearance.BorderSize = 0;
                Font = FSemiBold;
                BackColor = Surface;
                ForeColor = Slate800;
                Cursor = Cursors.Hand;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
                Height = 36;
            }
            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                Checked = true;
                foreach (Control c in Parent.Controls)
                    if (c is SegButton s && s != this) s.Checked = false;
                Parent.Invalidate();
                Invalidate();
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = ClientRectangle; r.Width -= 1; r.Height -= 1;

                int radius = Height / 2;
                var path = new GraphicsPath();
                if (LeftPiece || RightPiece)
                {
                    int d = radius * 2;
                    if (LeftPiece)
                    {
                        path.AddArc(r.X, r.Y, d, d, 180, 90);
                        path.AddArc(r.Right - 1, r.Y, 1, 1, 270, 90); // flat
                        path.AddArc(r.Right - 1, r.Bottom - 1, 1, 1, 0, 90);
                        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                    }
                    if (RightPiece)
                    {
                        path.AddArc(r.X - 1, r.Y, 1, 1, 180, 90);
                        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                        path.AddArc(r.X - 1, r.Bottom - 1, 1, 1, 90, 90);
                    }
                }
                if (!LeftPiece && !RightPiece) // middle or single
                {
                    int d = radius * 2;
                    path.AddArc(r.X, r.Y, d, d, 180, 90);
                    path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                    path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                    path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                }
                path.CloseFigure();

                Color back = Checked ? Color.FromArgb(248, 250, 252) : Surface;
                using var b = new SolidBrush(back);
                using var p = new Pen(Checked ? Slate800 : Border, Checked ? 2f : 1.6f);
                e.Graphics.FillPath(b, path);
                e.Graphics.DrawPath(p, path);

                TextRenderer.DrawText(e.Graphics, Text, FSemiBold, r, Slate800,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                path.Dispose();
            }
        }

        private class PillButton : Button
        {
            public bool Checked { get; set; }

            private Action<Graphics, Rectangle, Color> _iconPainter;
            private string _text = "";

            public PillButton()
            {
                FlatStyle = FlatStyle.Flat; FlatAppearance.BorderSize = 0;
                BackColor = Surface; ForeColor = Slate800; Font = FSemiBold;
                Cursor = Cursors.Hand; Height = 44;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            }
            public void SetIconAndText(Action<Graphics, Rectangle, Color> painter, string text)
            {
                _iconPainter = painter; _text = text;
                Invalidate();
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = ClientRectangle; r.Width -= 1; r.Height -= 1;
                int radius = Height / 2;

                using var gp = Round(r, radius);
                using var b = new SolidBrush(Checked ? Midnight : Surface);
                using var p = new Pen(Checked ? Midnight : Border, Checked ? 2.2f : 1.6f);

                e.Graphics.FillPath(b, gp);
                e.Graphics.DrawPath(p, gp);

                // icon
                var ic = new Rectangle(r.X + 14, r.Y + (r.Height - 20) / 2, 20, 20);
                _iconPainter?.Invoke(e.Graphics, ic, Checked ? Color.White : Slate600);

                // text
                var color = Checked ? Color.White : Slate800;
                TextRenderer.DrawText(e.Graphics, _text, FSemiBold, new Rectangle(ic.Right + 8, 0, r.Width - ic.Right - 8, r.Height),
                    color, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            }
            private GraphicsPath Round(Rectangle rc, int radius)
            {
                int d = radius * 2; var gp = new GraphicsPath();
                gp.AddArc(rc.X, rc.Y, d, d, 180, 90);
                gp.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
                gp.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
                gp.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
                gp.CloseFigure(); return gp;
            }
        }

        private class PrimaryButton : Button
        {
            public PrimaryButton()
            {
                FlatStyle = FlatStyle.Flat; FlatAppearance.BorderSize = 0;
                BackColor = Midnight; ForeColor = Color.White; Font = FSemiBold; Cursor = Cursors.Hand;
                Text = "Thanh toán"; Height = 48;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = ClientRectangle; r.Width -= 1; r.Height -= 1;
                using var gp = Round(r, Height / 2);
                using var b = new SolidBrush(BackColor);
                e.Graphics.FillPath(b, gp);
                TextRenderer.DrawText(e.Graphics, Text, FSemiBold, r, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            private GraphicsPath Round(Rectangle rc, int radius)
            {
                int d = radius * 2; var gp = new GraphicsPath();
                gp.AddArc(rc.X, rc.Y, d, d, 180, 90);
                gp.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
                gp.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
                gp.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
                gp.CloseFigure(); return gp;
            }
        }

        private class GhostButton : Button
        {
            public GhostButton()
            {
                FlatStyle = FlatStyle.Flat; FlatAppearance.BorderSize = 0;
                BackColor = Surface; ForeColor = Slate800; Font = FSemiBold; Cursor = Cursors.Hand;
                Height = 42;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = ClientRectangle; r.Width -= 1; r.Height -= 1;
                using var gp = Round(r, Height / 2);
                using var b = new SolidBrush(Surface);
                using var p = new Pen(Border, 1.6f);
                e.Graphics.FillPath(b, gp);
                e.Graphics.DrawPath(p, gp);
                TextRenderer.DrawText(e.Graphics, Text, FSemiBold, r, Slate800,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            private GraphicsPath Round(Rectangle rc, int radius)
            {
                int d = radius * 2; var gp = new GraphicsPath();
                gp.AddArc(rc.X, rc.Y, d, d, 180, 90);
                gp.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
                gp.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
                gp.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
                gp.CloseFigure(); return gp;
            }
        }

        // ====== Simple vector icons ======
        private void DrawCash(Graphics g, Rectangle r, Color c)
        {
            using var p = new Pen(c, 1.8f) { LineJoin = LineJoin.Round };
            g.DrawRectangle(p, r.X + 1, r.Y + 4, r.Width - 2, r.Height - 8);
            g.DrawEllipse(p, new Rectangle(r.X + r.Width / 2 - 3, r.Y + r.Height / 2 - 3, 6, 6));
        }
        private void DrawCard(Graphics g, Rectangle r, Color c)
        {
            using var p = new Pen(c, 1.8f) { LineJoin = LineJoin.Round };
            g.DrawRectangle(p, r.X + 1, r.Y + 3, r.Width - 2, r.Height - 6);
            g.DrawLine(p, r.X + 2, r.Y + 10, r.Right - 2, r.Y + 10);
        }
        private void DrawBank(Graphics g, Rectangle r, Color c)
        {
            using var p = new Pen(c, 1.8f) { LineJoin = LineJoin.Round };
            g.DrawLine(p, r.X + r.Width / 2, r.Y + 2, r.X + r.Width / 2, r.Bottom - 2);
            g.DrawEllipse(p, new Rectangle(r.X + r.Width / 2 - 4, r.Y + 6, 8, 8));
            g.DrawLine(p, r.X + 3, r.Bottom - 4, r.Right - 3, r.Bottom - 4);
        }
    }
}
