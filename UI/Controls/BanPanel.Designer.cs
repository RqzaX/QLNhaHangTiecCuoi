namespace UI.Controls
{
    partial class BanPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblKhuVuc = new Label();
            lblSoBan = new Label();
            lblSucChua = new Label();
            SuspendLayout();
            // 
            // lblKhuVuc
            // 
            lblKhuVuc.AutoSize = true;
            lblKhuVuc.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblKhuVuc.Location = new Point(65, 27);
            lblKhuVuc.Name = "lblKhuVuc";
            lblKhuVuc.Size = new Size(74, 23);
            lblKhuVuc.TabIndex = 0;
            lblKhuVuc.Text = "Khu Vực";
            // 
            // lblSoBan
            // 
            lblSoBan.AutoSize = true;
            lblSoBan.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSoBan.Location = new Point(80, 61);
            lblSoBan.Name = "lblSoBan";
            lblSoBan.Size = new Size(39, 23);
            lblSoBan.TabIndex = 1;
            lblSoBan.Text = "A01";
            // 
            // lblSucChua
            // 
            lblSucChua.AutoSize = true;
            lblSucChua.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSucChua.Location = new Point(88, 108);
            lblSucChua.Name = "lblSucChua";
            lblSucChua.Size = new Size(19, 23);
            lblSucChua.TabIndex = 2;
            lblSucChua.Text = "4";
            // 
            // BanPanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(192, 255, 192);
            ClientSize = new Size(200, 194);
            Controls.Add(lblSucChua);
            Controls.Add(lblSoBan);
            Controls.Add(lblKhuVuc);
            FormBorderStyle = FormBorderStyle.None;
            Name = "BanPanel";
            Text = "BanPanel";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblKhuVuc;
        private Label lblSoBan;
        private Label lblSucChua;
    }
}