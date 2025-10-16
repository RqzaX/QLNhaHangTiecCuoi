using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class RoleCard : Control
    {
        private Label lblTitle, lblTag, lblDesc, lblUsers;
        private Button btnEdit;
        private PictureBox picShield, picUsers;

        private int _cornerRadius = 20;
        private Color _accentColor = Color.FromArgb(31, 111, 235); // Sapphire
        private string _title = "Quản trị viên";
        private string _tag = "admin";
        private string _description = "Toàn quyền quản lý hệ thống";
        private int _userCount = 2;
        private bool _hover;
        private bool _showEdit = true;

        public RoleCard()
        {
            DoubleBuffered = true;
            Size = new Size(360, 200);
            BackColor = Color.White;
            Padding = new Padding(20);

            // Shield icon góc phải
            picShield = new PictureBox
            {
                Size = new Size(28, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Image = DrawShieldIcon(Color.FromArgb(120, _accentColor)),
                SizeMode = PictureBoxSizeMode.CenterImage
            };

            lblTitle = new Label
            {
                AutoSize = false,
                Text = _title,
                Font = new Font("Segoe UI Semibold", 12f),
                ForeColor = Color.Black
            };

            lblTag = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.White,
                Padding = new Padding(10, 3, 10, 3)
            };
            PaintTag();

            lblDesc = new Label
            {
                AutoSize = false,
                Text = _description,
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(70, 70, 77)
            };

            picUsers = new PictureBox
            {
                Size = new Size(20, 20),
                Image = DrawUsersIcon(Color.FromArgb(70, 70, 77)),
                SizeMode = PictureBoxSizeMode.CenterImage
            };

            lblUsers = new Label
            {
                AutoSize = true,
                Text = $"  {_userCount} người dùng",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(70, 70, 77)
            };

            btnEdit = new Button
            {
                Text = "Chỉnh sửa",
                Size = new Size(96, 36),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9f),
                BackColor = Color.FromArgb(245, 248, 255),
                ForeColor = _accentColor
            };
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(215, 225, 245);
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 241, 255);
            btnEdit.Visible = _showEdit;

            Controls.AddRange(new Control[] { lblTitle, lblTag, lblDesc, picUsers, lblUsers, picShield, btnEdit });

            Resize += (_, __) => Reflow();
            MouseEnter += (_, __) => { _hover = true; Invalidate(); };
            MouseLeave += (_, __) => { _hover = false; Invalidate(); };

            Reflow();
            UpdateRegion();
        }

        [Category("RoleCard")]
        public int CornerRadius { get => _cornerRadius; set { _cornerRadius = Math.Max(4, value); UpdateRegion(); Invalidate(); } }

        [Category("RoleCard")]
        public Color AccentColor { get => _accentColor; set { _accentColor = value; PaintTag(); Invalidate(); } }

        [Category("RoleCard")]
        public string Title { get => _title; set { _title = value; lblTitle.Text = value; Invalidate(); } }

        [Category("RoleCard")]
        public string TagText { get => _tag; set { _tag = value; lblTag.Text = value; PaintTag(); } }

        [Category("RoleCard")]
        public string Description { get => _description; set { _description = value; lblDesc.Text = value; } }

        [Category("RoleCard")]
        public int UserCount { get => _userCount; set { _userCount = value; lblUsers.Text = $"  {value} người dùng"; } }

        [Category("RoleCard")]
        public bool ShowEditButton { get => _showEdit; set { _showEdit = value; btnEdit.Visible = value; } }

        private void Reflow()
        {
            lblTitle.Location = new Point(20, 20);
            lblTitle.Size = new Size(Width - 100, 28);

            lblTag.Location = new Point(20, lblTitle.Bottom + 6);
            lblDesc.Location = new Point(20, lblTag.Bottom + 14);
            lblDesc.Size = new Size(Width - 40, 44);

            picShield.Location = new Point(Width - picShield.Width - 16, 16);

            picUsers.Location = new Point(20, Height - 40);
            lblUsers.Location = new Point(picUsers.Right + 4, picUsers.Top - 1);

            btnEdit.Location = new Point(Width - btnEdit.Width - 20, Height - btnEdit.Height - 20);
        }

        private void PaintTag()
        {
            lblTag.BackColor = _accentColor;
            lblTag.ForeColor = Color.White;
            lblTag.Region = new Region(RoundRect(lblTag.ClientRectangle, 10));
            lblTag.Resize += (_, __) =>
            {
                lblTag.Region?.Dispose();
                lblTag.Region = new Region(RoundRect(lblTag.ClientRectangle, 10));
            };
        }

        private void UpdateRegion()
        {
            using var path = RoundRect(new Rectangle(0, 0, Width, Height), _cornerRadius);
            Region = new Region(path);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Shadow
            var shadowRect = new Rectangle(5, 5, Width - 10, Height - 10);
            using (var shadowBrush = new SolidBrush(Color.FromArgb(25, 0, 0, 0)))
                g.FillPath(shadowBrush, RoundRect(shadowRect, _cornerRadius + 1));

            // Card background
            using (var bgBrush = new SolidBrush(_hover ? Color.FromArgb(252, 254, 255) : BackColor))
            using (var borderPen = new Pen(Color.FromArgb(230, 235, 242)))
                g.FillPath(bgBrush, RoundRect(ClientRectangle, _cornerRadius));
            g.DrawPath(Pens.Transparent, RoundRect(ClientRectangle, _cornerRadius));
        }

        private static GraphicsPath RoundRect(Rectangle r, int d)
        {
            int dia = Math.Max(2, d * 2);
            var gp = new GraphicsPath();
            gp.AddArc(r.X, r.Y, dia, dia, 180, 90);
            gp.AddArc(r.Right - dia, r.Y, dia, dia, 270, 90);
            gp.AddArc(r.Right - dia, r.Bottom - dia, dia, dia, 0, 90);
            gp.AddArc(r.X, r.Bottom - dia, dia, dia, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private static Image DrawShieldIcon(Color c)
        {
            var bmp = new Bitmap(24, 24);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(c, 1.8f);
            using var br = new SolidBrush(Color.FromArgb(32, c));
            PointF[] pts = {
                new(12,3), new(20,6), new(20,13),
                new(12,22), new(4,13), new(4,6), new(12,3)
            };
            g.FillPolygon(br, pts);
            g.DrawPolygon(pen, pts);
            return bmp;
        }

        private static Image DrawUsersIcon(Color c)
        {
            var bmp = new Bitmap(20, 20);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(c, 1.6f);
            g.DrawEllipse(pen, 3, 3, 6, 6);
            g.DrawArc(pen, 1, 8, 10, 9, 30, 120);
            g.DrawEllipse(pen, 11, 5, 5, 5);
            g.DrawArc(pen, 9, 9, 10, 8, 30, 120);
            return bmp;
        }

        [Category("RoleCard")]
        public event EventHandler EditClicked
        {
            add { btnEdit.Click += value; }
            remove { btnEdit.Click -= value; }
        }
    }
}
