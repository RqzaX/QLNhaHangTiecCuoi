using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Frm_ChiTietHangThanhVien : Form
    {
        private string _hangCode;
        private string _title;
        private Color _bgColor1;
        private Color _bgColor2;
        private Color _textColor;
        private string _icon;
        private string[] _benefits;

        public Frm_ChiTietHangThanhVien(string hangCode, string title, Color bgColor1, Color bgColor2,
            Color textColor, string icon, string[] benefits)
        {
            _hangCode = hangCode;
            _title = title;
            _bgColor1 = bgColor1;
            _bgColor2 = bgColor2;
            _textColor = textColor;
            _icon = icon;
            _benefits = benefits;

            InitializeComponent();
            LoadChiTiet();
        }

        private void LoadChiTiet()
        {
            try
            {
                // Set background cho form
                this.BackColor = Color.White;
                this.Padding = new Padding(30);
                this.Size = new Size(700, 650);

                // Panel chính
                Guna.UI2.WinForms.Guna2GradientPanel mainPanel = new Guna.UI2.WinForms.Guna2GradientPanel
                {
                    Dock = DockStyle.Fill,
                    BorderRadius = 20,
                    FillColor = _bgColor1,
                    FillColor2 = _bgColor2,
                    BorderThickness = 2,
                    BorderColor = Color.FromArgb(180, 180, 180),
                    Padding = new Padding(40, 40, 40, 40)
                };
                this.Controls.Add(mainPanel);

                // Icon circle với icon bên trong
                Panel iconCircle = new Panel
                {
                    Size = new Size(80, 80),
                    Location = new Point(40, 40),
                    BackColor = Color.Transparent
                };
                iconCircle.Paint += (s, e) =>
                {
                    var g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    // Vẽ vòng tròn trắng
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.FillEllipse(brush, 0, 0, iconCircle.Width - 1, iconCircle.Height - 1);
                    }

                    // Vẽ viền
                    using (var pen = new Pen(_textColor, 2.5f))
                    {
                        g.DrawEllipse(pen, 1, 1, iconCircle.Width - 3, iconCircle.Height - 3);
                    }

                    // Vẽ icon bên trong
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    var rect = new RectangleF(0, 0, iconCircle.Width, iconCircle.Height);
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap
                    };

                    using (var brush = new SolidBrush(_textColor))
                    {
                        try
                        {
                            using (var emojiFont = new Font("Segoe UI Emoji", 40F, FontStyle.Bold))
                            {
                                g.DrawString(_icon, emojiFont, brush, rect, sf);
                            }
                        }
                        catch
                        {
                            using (var defaultFont = new Font("Segoe UI", 40F, FontStyle.Bold))
                            {
                                g.DrawString(_icon, defaultFont, brush, rect, sf);
                            }
                        }
                    }
                };
                mainPanel.Controls.Add(iconCircle);

                // Title
                Label lblTitle = new Label
                {
                    Text = _title,
                    Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                    ForeColor = _textColor,
                    Location = new Point(140, 55),
                    AutoSize = true,
                    BackColor = Color.Transparent
                };
                mainPanel.Controls.Add(lblTitle);

                // Benefits container
                Panel benefitsContainer = new Panel
                {
                    Location = new Point(40, 150),
                    BackColor = Color.Transparent,
                    AutoScroll = false
                };

                void UpdateBenefitsContainerSize()
                {
                    if (mainPanel.Width > 0 && mainPanel.Height > 0)
                    {
                        benefitsContainer.Left = 40;
                        benefitsContainer.Top = 150;
                        benefitsContainer.Width = mainPanel.Width - 80;
                        benefitsContainer.Height = mainPanel.Height - 250;
                    }
                }

                mainPanel.SizeChanged += (s, e) => UpdateBenefitsContainerSize();
                mainPanel.Controls.Add(benefitsContainer);

                // Benefits
                int yPos = 0;
                foreach (string benefit in _benefits)
                {
                    Panel benefitItem = new Panel
                    {
                        Location = new Point(0, yPos),
                        Height = 45,
                        Width = benefitsContainer.Width,
                        BackColor = Color.Transparent
                    };

                    // Icon sao
                    Panel starIcon = new Panel
                    {
                        Size = new Size(20, 20),
                        Location = new Point(0, 12),
                        BackColor = Color.Transparent
                    };
                    starIcon.Paint += (s, e) =>
                    {
                        var g = e.Graphics;
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        var points = new PointF[10];
                        float centerX = starIcon.Width / 2f;
                        float centerY = starIcon.Height / 2f;
                        float outerRadius = 8f;
                        float innerRadius = 4f;

                        for (int i = 0; i < 10; i++)
                        {
                            float angle = (float)(i * Math.PI / 5 - Math.PI / 2);
                            float radius = i % 2 == 0 ? outerRadius : innerRadius;
                            points[i] = new PointF(
                                centerX + radius * (float)Math.Cos(angle),
                                centerY + radius * (float)Math.Sin(angle)
                            );
                        }

                        using (var pen = new Pen(_textColor, 1.5f))
                        {
                            g.DrawPolygon(pen, points);
                        }
                    };
                    benefitItem.Controls.Add(starIcon);

                    // Label text
                    Label lblBenefit = new Label
                    {
                        Text = benefit,
                        Font = new Font("Segoe UI", 13F, FontStyle.Regular),
                        ForeColor = Color.FromArgb(70, 70, 70),
                        AutoSize = true,
                        BackColor = Color.Transparent,
                        Location = new Point(30, 12),
                        UseCompatibleTextRendering = true
                    };
                    benefitItem.Controls.Add(lblBenefit);

                    benefitsContainer.Controls.Add(benefitItem);
                    yPos += 50;
                }

                // Cập nhật width của các benefitItem khi container resize
                benefitsContainer.SizeChanged += (s, e) =>
                {
                    foreach (Control ctrl in benefitsContainer.Controls)
                    {
                        if (ctrl is Panel benefitItem)
                        {
                            benefitItem.Width = benefitsContainer.Width;
                        }
                    }
                };

                UpdateBenefitsContainerSize();

                // Nút đóng
                Guna.UI2.WinForms.Guna2Button btnClose = new Guna.UI2.WinForms.Guna2Button
                {
                    Text = "Đóng",
                    Font = new Font("Segoe UI Semibold", 12F),
                    FillColor = Color.White,
                    ForeColor = _textColor,
                    Size = new Size(120, 45),
                    Location = new Point(mainPanel.Width - 160, mainPanel.Height - 75),
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    Cursor = Cursors.Hand,
                    BorderRadius = 10,
                    BorderThickness = 2,
                    BorderColor = _textColor
                };
                btnClose.Click += (s, e) => this.Close();
                mainPanel.Controls.Add(btnClose);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load chi tiết hạng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

