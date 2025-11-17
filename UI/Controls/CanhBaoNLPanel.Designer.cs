namespace UI.Controls
{
    partial class CanhBaoNLPanel
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
            uiPanel1 = new Sunny.UI.UIPanel();
            label1 = new Label();
            uiPanel2 = new Sunny.UI.UIPanel();
            label2 = new Label();
            uiPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiPanel1
            // 
            uiPanel1.Controls.Add(label2);
            uiPanel1.Controls.Add(uiPanel2);
            uiPanel1.Controls.Add(label1);
            uiPanel1.Dock = DockStyle.Fill;
            uiPanel1.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel1.Margin = new Padding(0);
            uiPanel1.MinimumSize = new Size(1, 1);
            uiPanel1.Name = "uiPanel1";
            uiPanel1.Radius = 19;
            uiPanel1.Size = new Size(867, 64);
            uiPanel1.TabIndex = 0;
            uiPanel1.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 20);
            label1.Name = "label1";
            label1.Size = new Size(167, 25);
            label1.TabIndex = 0;
            label1.Text = "Tên Nguyên Liệu ";
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // uiPanel2
            // 
            uiPanel2.FillColor = Color.FromArgb(255, 255, 128);
            uiPanel2.Font = new Font("Microsoft Sans Serif", 12F);
            uiPanel2.Location = new Point(747, 13);
            uiPanel2.Margin = new Padding(10, 0, 10, 0);
            uiPanel2.MinimumSize = new Size(100, 1);
            uiPanel2.Name = "uiPanel2";
            uiPanel2.Radius = 20;
            uiPanel2.Size = new Size(100, 38);
            uiPanel2.TabIndex = 1;
            uiPanel2.Text = "Sắp hết";
            uiPanel2.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(250, 20);
            label2.Name = "label2";
            label2.Size = new Size(97, 25);
            label2.TabIndex = 2;
            label2.Text = "Số Lượng";
            label2.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CanhBaoNLPanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Size = new Size(867, 64);
            Controls.Add(uiPanel1);
            Name = "CanhBaoNLPanel";
            uiPanel1.ResumeLayout(false);
            uiPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private Label label2;
        private Sunny.UI.UIPanel uiPanel2;
        private Label label1;
    }
}