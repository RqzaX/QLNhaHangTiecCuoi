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
            roundedPanel1 = new UI.Controls.RoundedPanel();
            label3 = new Label();
            roundedPanel2 = new UI.Controls.RoundedPanel();
            roundedTextBox1 = new UI.Controls.RoundedTextBox();
            label4 = new Label();
            roundedPanel1.SuspendLayout();
            roundedPanel2.SuspendLayout();
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
            roundedButton1.FlatAppearance.BorderSize = 0;
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
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BorderThickness = 5;
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Location = new Point(10, 67);
            roundedPanel1.Margin = new Padding(3, 2, 3, 2);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Padding = new Padding(10, 9, 10, 9);
            roundedPanel1.Size = new Size(339, 469);
            roundedPanel1.TabIndex = 13;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(21, 16);
            label3.Name = "label3";
            label3.Size = new Size(89, 19);
            label3.TabIndex = 15;
            label3.Text = "Lịch đặt bàn";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.White;
            roundedPanel2.BorderThickness = 5;
            roundedPanel2.Controls.Add(roundedTextBox1);
            roundedPanel2.Controls.Add(label4);
            roundedPanel2.Location = new Point(354, 67);
            roundedPanel2.Margin = new Padding(3, 2, 3, 2);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Padding = new Padding(10, 9, 10, 9);
            roundedPanel2.Size = new Size(676, 469);
            roundedPanel2.TabIndex = 14;
            // 
            // roundedTextBox1
            // 
            roundedTextBox1.BackColor = Color.White;
            roundedTextBox1.Font = new Font("Segoe UI", 10F);
            roundedTextBox1.ForeColor = Color.Black;
            roundedTextBox1.Location = new Point(249, 9);
            roundedTextBox1.Margin = new Padding(3, 2, 3, 2);
            roundedTextBox1.Name = "roundedTextBox1";
            roundedTextBox1.Padding = new Padding(9, 6, 9, 6);
            roundedTextBox1.Size = new Size(414, 32);
            roundedTextBox1.TabIndex = 17;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(34, 16);
            label4.Name = "label4";
            label4.Size = new Size(131, 19);
            label4.TabIndex = 16;
            label4.Text = "Danh sách đặt bàn";
            // 
            // FrmDatBan
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1041, 675);
            Controls.Add(roundedPanel2);
            Controls.Add(roundedPanel1);
            Controls.Add(roundedButton1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmDatBan";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmDatBan";
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private Label label1;
        private Controls.RoundedButton roundedButton1;
        private Controls.RoundedPanel roundedPanel1;
        private Label label3;
        private Controls.RoundedPanel roundedPanel2;
        private Controls.RoundedTextBox roundedTextBox1;
        private Label label4;
    }
}