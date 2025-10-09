namespace UI
{
    partial class FrmDatBan
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
            label2 = new Label();
            label1 = new Label();
            roundedButton1 = new UI.Controls.RoundedButton();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            cbbTrangThaiBan = new UI.Controls.ComboBox_Border();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(10, 33);
            label2.Name = "label2";
            label2.Size = new Size(218, 19);
            label2.TabIndex = 11;
            label2.Text = "Quản lý đặt bàn của khách hàng";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(10, 7);
            label1.Name = "label1";
            label1.Size = new Size(85, 27);
            label1.TabIndex = 10;
            label1.Text = "Đặt bàn";
            // 
            // roundedButton1
            // 
            roundedButton1.BackColor = Color.FromArgb(31, 111, 235);
            roundedButton1.CornerRadius = 17;
            roundedButton1.FlatStyle = FlatStyle.Flat;
            roundedButton1.Font = new Font("Segoe UI Semibold", 10.5F);
            roundedButton1.ForeColor = Color.White;
            roundedButton1.Location = new Point(854, 9);
            roundedButton1.Margin = new Padding(3, 2, 3, 2);
            roundedButton1.Name = "roundedButton1";
            roundedButton1.Padding = new Padding(9, 4, 9, 4);
            roundedButton1.Size = new Size(151, 35);
            roundedButton1.TabIndex = 12;
            roundedButton1.Text = "+ Tạo đặt bàn mới";
            roundedButton1.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Location = new Point(-1, 66);
            panel1.Name = "panel1";
            panel1.Size = new Size(371, 855);
            panel1.TabIndex = 13;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Location = new Point(369, 66);
            panel2.Name = "panel2";
            panel2.Size = new Size(653, 141);
            panel2.TabIndex = 14;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(cbbTrangThaiBan);
            panel3.Location = new Point(369, 208);
            panel3.Name = "panel3";
            panel3.Size = new Size(653, 52);
            panel3.TabIndex = 15;
            // 
            // panel4
            // 
            panel4.Location = new Point(369, 266);
            panel4.Name = "panel4";
            panel4.Size = new Size(653, 643);
            panel4.TabIndex = 16;
            // 
            // cbbTrangThaiBan
            // 
            cbbTrangThaiBan.BackColor = Color.Transparent;
            cbbTrangThaiBan.BorderColor = Color.FromArgb(226, 232, 240);
            cbbTrangThaiBan.BorderFocusColor = Color.FromArgb(99, 102, 241);
            cbbTrangThaiBan.BorderHoverColor = Color.FromArgb(203, 213, 225);
            cbbTrangThaiBan.CardBackColor = Color.White;
            cbbTrangThaiBan.CornerRadius = 12;
            cbbTrangThaiBan.Location = new Point(420, 7);
            cbbTrangThaiBan.MinimumSize = new Size(80, 36);
            cbbTrangThaiBan.Name = "cbbTrangThaiBan";
            cbbTrangThaiBan.Padding = new Padding(10, 6, 30, 6);
            cbbTrangThaiBan.Placeholder = "";
            cbbTrangThaiBan.SelectedIndex = -1;
            cbbTrangThaiBan.SelectedItem = null;
            cbbTrangThaiBan.Size = new Size(230, 36);
            cbbTrangThaiBan.TabIndex = 0;
            // 
            // FrmDatBan
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1041, 675);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(roundedButton1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmDatBan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmDatBan";
            Load += FrmDatBan_Load;
            panel3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Controls.RoundedButton roundedButton1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Controls.ComboBox_Border cbbTrangThaiBan;
    }
}