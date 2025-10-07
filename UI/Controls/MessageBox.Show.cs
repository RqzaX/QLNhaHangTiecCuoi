using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UiControls
{
    [SupportedOSPlatform("windows")]
    public class ModernMessageBox : Form
    {
        // ==================== Public API ====================
        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption = "Thông báo",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            bool darkMode = false,
            DialogResult defaultButton = DialogResult.OK)
        {
            using (var f = new ModernMessageBox(text, caption, buttons, icon, darkMode, defaultButton))
            {
                return owner == null ? f.ShowDialog() : f.ShowDialog(owner);
            }
        }

        public static DialogResult Show(
            string text,
            string caption = "Thông báo",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information,
            bool darkMode = false,
            DialogResult defaultButton = DialogResult.OK)
            => Show(null, text, caption, buttons, icon, darkMode, defaultButton);

        public static DialogResult Info(string text, string caption = "Thông báo", bool darkMode = false)
            => Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information, darkMode, DialogResult.OK);

        public static DialogResult Warning(string text, string caption = "Cảnh báo", bool darkMode = false)
            => Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning, darkMode, DialogResult.OK);

        public static DialogResult Error(string text, string caption = "Lỗi", bool darkMode = false)
            => Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, darkMode, DialogResult.OK);

        public static DialogResult YesNo(string text, string caption = "Xác nhận", bool darkMode = false)
            => Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, darkMode, DialogResult.No);

        // ================== Controls & fields ==================
        private readonly Label _lblText = new Label();
        private readonly Label _lblTitle = new Label();
        private readonly PictureBox _pic = new PictureBox();
        private readonly FlowLayoutPanel _buttonsPanel = new FlowLayoutPanel();
        private readonly Panel _titleBar = new Panel();
        private readonly Panel _contentPanel = new Panel();

        private readonly Color Primary = ColorTranslator.FromHtml("#1F6FEB");
        private readonly bool _dark;

        private const int PAD = 16;
        private const int BTN_H = 42;
        private const int BTN_W = 120;
        private const int RADIUS = 14;

        // ===================== Constructor =====================
        public ModernMessageBox(
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            bool darkMode,
            DialogResult defaultButton)
        {
            _dark = darkMode;

            // Form
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 12f, FontStyle.Regular);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = MaximizeBox = false;
            TopMost = true;
            Padding = new Padding(PAD);
            DoubleBuffered = true;
            MinimumSize = new Size(480, 220);
            BackColor = _dark ? Color.FromArgb(20, 24, 32) : Color.White;
            ForeColor = _dark ? Color.FromArgb(226, 232, 240) : Color.FromArgb(15, 23, 42);

            // Title bar
            _titleBar.Height = 44;
            _titleBar.Dock = DockStyle.Top;
            _titleBar.BackColor = _dark ? Color.FromArgb(24, 28, 38) : Color.FromArgb(246, 248, 252);
            _titleBar.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    NativeMethods.ReleaseCapture();
                    NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HTCAPTION, 0);
                }
            };
            Controls.Add(_titleBar);

            var btnClose = new Button
            {
                Text = "✕",
                FlatStyle = FlatStyle.Flat,
                ForeColor = _dark ? Color.White : Color.Black,
                BackColor = Color.Transparent,
                Size = new Size(44, 44),
                Dock = DockStyle.Right,
                TabStop = false
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            _titleBar.Controls.Add(btnClose);

            _lblTitle.Text = caption;
            _lblTitle.AutoSize = false;
            _lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            _lblTitle.Dock = DockStyle.Fill;
            _lblTitle.Font = SemiBold(Font);     // dùng font "Segoe UI Semibold" nếu có, fallback Bold
            _lblTitle.Padding = new Padding(PAD, 0, 0, 0);
            _titleBar.Controls.Add(_lblTitle);

            // Content container
            _contentPanel.Dock = DockStyle.Fill;
            Controls.Add(_contentPanel);

            // Icon
            _pic.SizeMode = PictureBoxSizeMode.CenterImage;
            _pic.Size = new Size(48, 48);
            _pic.Location = new Point(PAD, PAD + 6);

            // Text
            _lblText.AutoSize = true; // để tự giãn chiều cao ngay lần đầu
            _lblText.MaximumSize = new Size(820, 0); // sẽ cập nhật lại theo chiều rộng form
            _lblText.Location = new Point(_pic.Right + PAD, PAD);
            _lblText.Font = new Font(Font.FontFamily, 13.5f, FontStyle.Regular);
            _lblText.Text = text;
            _lblText.UseMnemonic = false;
            _lblText.ForeColor = ForeColor;

            // Bottom buttons area
            _buttonsPanel.Dock = DockStyle.Bottom;
            _buttonsPanel.Height = BTN_H + PAD * 2;
            _buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
            _buttonsPanel.Padding = new Padding(PAD, PAD, PAD, PAD);
            _buttonsPanel.BackColor = _dark ? Color.FromArgb(24, 28, 38) : Color.FromArgb(246, 248, 252);

            _contentPanel.Controls.Add(_buttonsPanel);
            _contentPanel.Controls.Add(_pic);
            _contentPanel.Controls.Add(_lblText);

            // Border/rounding
            Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), RADIUS))
                using (var pen = new Pen(_dark ? Color.FromArgb(60, 70, 84) : Color.FromArgb(220, 224, 232)))
                {
                    g.DrawPath(pen, path);
                }
            };

            // Icon + layout + events
            SetIcon(icon);
            Resize += (s, e) => LayoutMessage();

            // Buttons
            BuildButtons(buttons, defaultButton);

            // Keyboard
            KeyPreview = true;
            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) CloseWith(DialogResult.Cancel);
                else if (e.KeyCode == Keys.Enter) PerformDefaultClick();
            };

            // Sound
            PlaySystemSound(icon);

            // Bo góc & layout ngay lần đầu
            Shown += (s, e) => { ApplyRoundRegion(this, RADIUS); LayoutMessage(); };
            LayoutMessage(); // <<< đảm bảo label hiện đủ nội dung ngay lần đầu
        }

        // ================= Layout & helpers =================
        private void LayoutMessage()
        {
            // Cập nhật bề ngang tối đa cho text dựa vào form width
            int textLeft = _pic.Visible ? _pic.Right + PAD : PAD;
            int textWidth = Math.Max(Width - textLeft - PAD * 2, 240);
            _lblText.MaximumSize = new Size(textWidth, 0); // AutoSize sẽ tự tính Height

            // Chiều cao mong muốn (nội dung + khu nút)
            int contentBottom = _lblText.Bottom + PAD;
            int desiredHeight = Math.Max(contentBottom + _buttonsPanel.Height + PAD, MinimumSize.Height);
            Height = desiredHeight;
            _lblText.BringToFront();
        }

        private void SetIcon(MessageBoxIcon icon)
        {
            Icon sysIcon = icon switch
            {
                MessageBoxIcon.Error => SystemIcons.Error,
                MessageBoxIcon.Warning => SystemIcons.Warning,
                MessageBoxIcon.Question => SystemIcons.Question,
                MessageBoxIcon.Information => SystemIcons.Information,
                _ => null
            };

            _pic.Image = sysIcon?.ToBitmap();
            _pic.Visible = sysIcon != null;
            _lblText.Left = _pic.Visible ? _pic.Right + PAD : PAD;
        }

        private void PlaySystemSound(MessageBoxIcon icon)
        {
            try
            {
                switch (icon)
                {
                    case MessageBoxIcon.Error: SystemSounds.Hand.Play(); break;
                    case MessageBoxIcon.Warning: SystemSounds.Exclamation.Play(); break;
                    case MessageBoxIcon.Question: SystemSounds.Question.Play(); break;
                    case MessageBoxIcon.Information: SystemSounds.Asterisk.Play(); break;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void BuildButtons(MessageBoxButtons buttons, DialogResult defaultButton)
        {
            _buttonsPanel.Controls.Clear();

            void AddBtn(string text, DialogResult dr, bool primary = false, bool isDefault = false)
            {
                var b = new Button
                {
                    Text = text,
                    DialogResult = dr,
                    Font = SemiBold(Font),
                    FlatStyle = FlatStyle.Flat,
                    Height = BTN_H,
                    Width = BTN_W,
                    Margin = new Padding(8),
                    BackColor = primary ? Primary : (_dark ? Color.FromArgb(40, 46, 60) : Color.White),
                    ForeColor = primary ? Color.White : (_dark ? Color.White : Color.FromArgb(30, 41, 59)),
                    TabStop = true
                };
                b.FlatAppearance.BorderSize = primary ? 0 : 1;
                b.FlatAppearance.BorderColor = _dark ? Color.FromArgb(70, 80, 96) : Color.FromArgb(210, 214, 220);
                b.HandleCreated += (s, e) => ApplyRoundRegion(b, 10);
                b.Click += (s, e) => CloseWith(dr);

                _buttonsPanel.Controls.Add(b);

                if (isDefault)
                {
                    AcceptButton = b; // Enter
                    b.Focus();
                }
            }

            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    AddBtn("OK", DialogResult.OK, true, defaultButton == DialogResult.OK);
                    break;

                case MessageBoxButtons.OKCancel:
                    AddBtn("Hủy", DialogResult.Cancel, false, defaultButton == DialogResult.Cancel);
                    AddBtn("OK", DialogResult.OK, true, defaultButton == DialogResult.OK);
                    CancelButton = GetButtonByResult(DialogResult.Cancel);
                    break;

                case MessageBoxButtons.YesNo:
                    AddBtn("Không", DialogResult.No, false, defaultButton == DialogResult.No);
                    AddBtn("Có", DialogResult.Yes, true, defaultButton == DialogResult.Yes);
                    break;

                case MessageBoxButtons.YesNoCancel:
                    AddBtn("Hủy", DialogResult.Cancel, false, defaultButton == DialogResult.Cancel);
                    AddBtn("Không", DialogResult.No, false, defaultButton == DialogResult.No);
                    AddBtn("Có", DialogResult.Yes, true, defaultButton == DialogResult.Yes);
                    CancelButton = GetButtonByResult(DialogResult.Cancel);
                    break;

                case MessageBoxButtons.RetryCancel:
                    AddBtn("Hủy", DialogResult.Cancel, false, defaultButton == DialogResult.Cancel);
                    AddBtn("Thử lại", DialogResult.Retry, true, defaultButton == DialogResult.Retry);
                    CancelButton = GetButtonByResult(DialogResult.Cancel);
                    break;

                case MessageBoxButtons.AbortRetryIgnore:
                    AddBtn("Bỏ qua", DialogResult.Ignore, false, defaultButton == DialogResult.Ignore);
                    AddBtn("Thử lại", DialogResult.Retry, true, defaultButton == DialogResult.Retry);
                    AddBtn("Huỷ thao tác", DialogResult.Abort, false, defaultButton == DialogResult.Abort);
                    break;
            }

            // Nếu chưa đặt default theo tham số → lấy nút đầu tiên
            if (AcceptButton == null && _buttonsPanel.Controls.Count > 0)
                AcceptButton = (IButtonControl)_buttonsPanel.Controls[_buttonsPanel.Controls.Count - 1];
        }

        private IButtonControl GetButtonByResult(DialogResult result)
        {
            foreach (Control c in _buttonsPanel.Controls)
                if (c is Button b && b.DialogResult == result) return b;
            return null;
        }

        private void PerformDefaultClick()
        {
            if (AcceptButton is IButtonControl btn) btn.PerformClick();
        }

        private void CloseWith(DialogResult dr)
        {
            DialogResult = dr;
            Close();
        }

        // ============== Font helpers ==============
        private static Font SemiBold(Font baseFont, float? size = null)
        {
            try
            {
                var fam = new FontFamily("Segoe UI Semibold"); // nếu có, dùng đúng họ semibold
                return new Font(fam, size ?? baseFont.Size, FontStyle.Regular);
            }
            catch
            {
                return new Font(baseFont, size.HasValue ? FontStyle.Bold : FontStyle.Bold);
            }
        }

        // ============== Rounded helpers ==============
        private static void ApplyRoundRegion(Control c, int radius)
        {
            if (c.Width < 1 || c.Height < 1) return;
            using (var path = RoundedRect(new Rectangle(0, 0, c.Width, c.Height), radius))
                c.Region = new Region(path);
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ============== Native (drag form) ==============
        private static class NativeMethods
        {
            public const int WM_NCLBUTTONDOWN = 0xA1;
            public const int HTCAPTION = 0x2;

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool ReleaseCapture();

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        }
    }
}
