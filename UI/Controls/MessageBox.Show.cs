using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UiControls
{
    public class MessageBox_Show : Form
    {
        private string message;
        private string title;
        private MessageBoxButtons buttons;
        private MessageBoxIcon icon;
        private Color? customColor;

        // Màu sắc cơ bản
        private Color backgroundColor = Color.White;
        private Color borderColor = Color.Blue;
        private Color textColor = Color.Black;

        public MessageBox_Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, Color? customColor = null)
        {
            this.message = message;
            this.title = title;
            this.buttons = buttons;
            this.icon = icon;
            this.customColor = customColor;

            // Thiết lập form đơn giản
            this.Text = title;
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None; // Không có border
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.BackColor = backgroundColor;

            // Thiết lập màu sắc dựa trên icon
            SetColors();

            // Tạo controls
            CreateControls();
        }

        private void SetColors()
        {
            switch (icon)
            {
                case MessageBoxIcon.Information:
                    borderColor = Color.Blue;
                    backgroundColor = Color.LightBlue;
                    textColor = Color.DarkBlue;
                    break;
                case MessageBoxIcon.Warning:
                    borderColor = Color.Orange;
                    backgroundColor = Color.LightYellow;
                    textColor = Color.DarkOrange;
                    break;
                case MessageBoxIcon.Error:
                    borderColor = Color.Red;
                    backgroundColor = Color.LightPink;
                    textColor = Color.DarkRed;
                    break;
                case MessageBoxIcon.Question:
                    borderColor = Color.Green;
                    backgroundColor = Color.LightGreen;
                    textColor = Color.DarkGreen;
                    break;
                default:
                    borderColor = Color.Blue;
                    backgroundColor = Color.LightBlue;
                    textColor = Color.DarkBlue;
                    break;
            }

            // Nếu có custom color thì dùng nó
            if (customColor.HasValue)
            {
                borderColor = customColor.Value;
                backgroundColor = Color.FromArgb(50, customColor.Value);
            }

            this.BackColor = backgroundColor;
        }

        private void CreateControls()
        {
            // Panel chính
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = backgroundColor;
            this.Controls.Add(mainPanel);

            // Label tiêu đề
            Label titleLabel = new Label();
            titleLabel.Text = title;
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.ForeColor = textColor;
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(20, 20);
            mainPanel.Controls.Add(titleLabel);

            // Label nội dung
            Label messageLabel = new Label();
            messageLabel.Text = message;
            messageLabel.Font = new Font("Arial", 12);
            messageLabel.ForeColor = textColor;
            messageLabel.AutoSize = true;
            messageLabel.Location = new Point(20, 60);
            messageLabel.MaximumSize = new Size(350, 0);
            mainPanel.Controls.Add(messageLabel);

            // Tạo buttons
            CreateButtons(mainPanel);
        }

        private void CreateButtons(Panel parent)
        {
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 60;
            buttonPanel.Padding = new Padding(10);
            parent.Controls.Add(buttonPanel);

            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    AddButton(buttonPanel, "OK", DialogResult.OK, true);
                    break;
                case MessageBoxButtons.OKCancel:
                    AddButton(buttonPanel, "Cancel", DialogResult.Cancel, false);
                    AddButton(buttonPanel, "OK", DialogResult.OK, true);
                    break;
                case MessageBoxButtons.YesNo:
                    AddButton(buttonPanel, "No", DialogResult.No, false);
                    AddButton(buttonPanel, "Yes", DialogResult.Yes, true);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    AddButton(buttonPanel, "Cancel", DialogResult.Cancel, false);
                    AddButton(buttonPanel, "No", DialogResult.No, false);
                    AddButton(buttonPanel, "Yes", DialogResult.Yes, true);
                    break;
            }
        }

        private void AddButton(FlowLayoutPanel panel, string text, DialogResult result, bool isPrimary)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(80, 35);
            btn.Margin = new Padding(5);
            btn.DialogResult = result;
            btn.Click += (s, e) => { this.DialogResult = result; this.Close(); };

            // Màu sắc button
            if (isPrimary)
            {
                btn.BackColor = borderColor;
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = borderColor;
            }

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = borderColor;
            btn.FlatAppearance.BorderSize = 2;

            panel.Controls.Add(btn);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Vẽ border đơn giản
            using (Pen pen = new Pen(borderColor, 3))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }
        }

        // ==================================================
        // ==== FORM ĐƠN GIẢN KHÔNG CÓ SHADOW ====
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Form đơn giản, không có shadow
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
        }
        // ==================================================
    }
}