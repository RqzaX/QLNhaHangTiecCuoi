using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UI.Controls
{
    public partial class NoteDialog : Form
    {
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblIcon;
        private Panel panelContent;
        private TextBox txtNote;
        private Panel panelButtons;
        private Button btnOK;
        private Button btnCancel;
        
        public string NoteText { get; set; }
        
        public NoteDialog(string currentNote = "")
        {
            InitializeComponent();
            NoteText = currentNote;
            txtNote.Text = currentNote;
            txtNote.Focus();
            txtNote.SelectAll();
        }
        
        private void InitializeComponent()
        {
            this.panelHeader = new Panel();
            this.lblTitle = new Label();
            this.lblIcon = new Label();
            this.panelContent = new Panel();
            this.txtNote = new TextBox();
            this.panelButtons = new Panel();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();
            
            // Form properties
            this.Text = "";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Padding = new Padding(0);
            
            // Header Panel - Fixed position
            this.panelHeader.BackColor = Color.FromArgb(52, 144, 220);
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Size = new Size(450, 60);
            this.panelHeader.Dock = DockStyle.None;
            
            // Title Label
            this.lblTitle.Text = "Ghi chú món ăn";
            this.lblTitle.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(60, 20);
            this.lblTitle.Size = new Size(200, 25);
            this.lblTitle.BackColor = Color.Transparent;
            
            // Icon Label (using Unicode emoji)
            this.lblIcon.Text = "📝";
            this.lblIcon.Font = new Font("Segoe UI Emoji", 16f);
            this.lblIcon.ForeColor = Color.White;
            this.lblIcon.Location = new Point(20, 18);
            this.lblIcon.Size = new Size(30, 25);
            this.lblIcon.BackColor = Color.Transparent;
            this.lblIcon.TextAlign = ContentAlignment.MiddleCenter;
            
            // Content Panel - Fixed position below header
            this.panelContent.BackColor = Color.White;
            this.panelContent.Location = new Point(0, 60);
            this.panelContent.Size = new Size(450, 200);
            this.panelContent.Dock = DockStyle.None;
            this.panelContent.Padding = new Padding(20, 20, 20, 20);
            
            // TextBox for note input
            this.txtNote.Location = new Point(20, 20);
            this.txtNote.Size = new Size(410, 160);
            this.txtNote.Multiline = true;
            this.txtNote.ScrollBars = ScrollBars.Vertical;
            this.txtNote.Font = new Font("Segoe UI", 11f);
            this.txtNote.PlaceholderText = "Nhập ghi chú cho món ăn (ví dụ: ít cay, không hành, thêm rau...)";
            this.txtNote.BorderStyle = BorderStyle.FixedSingle;
            this.txtNote.BackColor = Color.White;
            this.txtNote.ForeColor = Color.FromArgb(51, 51, 51);
            this.txtNote.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            
            // Buttons Panel - Fixed position at bottom
            this.panelButtons.BackColor = Color.FromArgb(248, 249, 250);
            this.panelButtons.Location = new Point(0, 260);
            this.panelButtons.Size = new Size(450, 90);
            this.panelButtons.Dock = DockStyle.None;
            this.panelButtons.Padding = new Padding(25, 20, 25, 20);
            
            // OK Button
            this.btnOK.Text = "Lưu";
            this.btnOK.Size = new Size(100, 35);
            this.btnOK.Location = new Point(225, 20);
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.BackColor = Color.FromArgb(52, 144, 220);
            this.btnOK.ForeColor = Color.White;
            this.btnOK.FlatStyle = FlatStyle.Flat;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            this.btnOK.Cursor = Cursors.Hand;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnOK.Region = new Region(CreateRoundedRectanglePath(new Rectangle(0, 0, 100, 35), 15));
            
            // Cancel Button
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.Location = new Point(325, 20);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.BackColor = Color.FromArgb(108, 117, 125);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            this.btnCancel.Cursor = Cursors.Hand;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnCancel.Region = new Region(CreateRoundedRectanglePath(new Rectangle(0, 0, 100, 35), 15));
            
            // Add controls to panels
            this.panelHeader.Controls.Add(this.lblIcon);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelContent.Controls.Add(this.txtNote);
            this.panelButtons.Controls.Add(this.btnOK);
            this.panelButtons.Controls.Add(this.btnCancel);
            
            // Add panels to form
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelButtons);
            
            // Event handlers
            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;
            this.txtNote.KeyDown += TxtNote_KeyDown;
            
            // Add hover effects
            this.btnOK.MouseEnter += BtnOK_MouseEnter;
            this.btnOK.MouseLeave += BtnOK_MouseLeave;
            this.btnCancel.MouseEnter += BtnCancel_MouseEnter;
            this.btnCancel.MouseLeave += BtnCancel_MouseLeave;
            
            // Add resize event to update button regions
            this.btnOK.Resize += BtnOK_Resize;
            this.btnCancel.Resize += BtnCancel_Resize;
            
            this.ResumeLayout(false);
        }
        
        private void BtnOK_Click(object sender, EventArgs e)
        {
            NoteText = txtNote.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        private void TxtNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                BtnOK_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnCancel_Click(sender, e);
            }
        }
        
        private void BtnOK_MouseEnter(object sender, EventArgs e)
        {
            btnOK.BackColor = Color.FromArgb(41, 128, 185);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
        }
        
        private void BtnOK_MouseLeave(object sender, EventArgs e)
        {
            btnOK.BackColor = Color.FromArgb(52, 144, 220);
        }
        
        private void BtnCancel_MouseEnter(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(95, 106, 115);
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(95, 106, 115);
        }
        
        private void BtnCancel_MouseLeave(object sender, EventArgs e)
        {
            btnCancel.BackColor = Color.FromArgb(108, 117, 125);
        }
        
        private void BtnOK_Resize(object sender, EventArgs e)
        {
            btnOK.Region = new Region(CreateRoundedRectanglePath(new Rectangle(0, 0, btnOK.Width, btnOK.Height), 15));
        }
        
        private void BtnCancel_Resize(object sender, EventArgs e)
        {
            btnCancel.Region = new Region(CreateRoundedRectanglePath(new Rectangle(0, 0, btnCancel.Width, btnCancel.Height), 15));
        }
        
        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return path;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            // Draw rounded corners for form
            using (GraphicsPath path = CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 15))
            {
                this.Region = new Region(path);
            }
            
            // Draw border
            using (Pen borderPen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                e.Graphics.DrawPath(borderPen, CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 15));
            }
        }
    }
}
