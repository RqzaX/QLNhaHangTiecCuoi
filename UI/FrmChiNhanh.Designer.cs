namespace UI
{
    partial class FrmChiNhanh
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                // Dispose timer để tránh memory leak
                if (_searchTimer != null)
                {
                    _searchTimer.Stop();
                    _searchTimer.Dispose();
                    _searchTimer = null;
                }
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
            VanThuan.UI.PillItem pillItem1 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem2 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem3 = new VanThuan.UI.PillItem();
            VanThuan.UI.PillItem pillItem4 = new VanThuan.UI.PillItem();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges23 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges24 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges33 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges34 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges29 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges30 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges31 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges32 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            label2 = new Label();
            segmentedPill1 = new VanThuan.UI.SegmentedPill();
            label1 = new Label();
            PanelTimKiemChiNhanh = new Guna.UI2.WinForms.Guna2GradientPanel();
            btnThemChiNhanh = new Guna.UI2.WinForms.Guna2Button();
            cbbLocCN = new Guna.UI2.WinForms.Guna2ComboBox();
            txtTimKiemChiNhanh = new Guna.UI2.WinForms.Guna2TextBox();
            panelKhuVuc = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            label3 = new Label();
            btnThem = new Guna.UI2.WinForms.Guna2Button();
            dgvKhuVuc = new DataGridView();
            panelBan = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            PanelThaoTacBan = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            btnThemBan = new Guna.UI2.WinForms.Guna2Button();
            cbbTrangThai = new Guna.UI2.WinForms.Guna2ComboBox();
            cbbKhuVuc = new Guna.UI2.WinForms.Guna2ComboBox();
            txtTimBan = new Guna.UI2.WinForms.Guna2TextBox();
            PanelSoDoBan = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            lblPhanChiaKhuVuc = new Label();
            PanelDanhSachBan = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            panelSanh = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            panelTimKiemSanh = new Panel();
            txtTimSanh = new Guna.UI2.WinForms.Guna2TextBox();
            btnThemSanh = new Guna.UI2.WinForms.Guna2Button();
            panelChiNhanh = new Panel();
            PanelTimKiemChiNhanh.SuspendLayout();
            panelKhuVuc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKhuVuc).BeginInit();
            panelBan.SuspendLayout();
            PanelThaoTacBan.SuspendLayout();
            PanelSoDoBan.SuspendLayout();
            panelSanh.SuspendLayout();
            panelTimKiemSanh.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(312, 20);
            label2.TabIndex = 12;
            label2.Text = "Quản lý chi nhánh,Khu vực,Bàn ăn và Sảnh tiệc";
            // 
            // segmentedPill1
            // 
            segmentedPill1.BackColor = Color.Transparent;
            segmentedPill1.ContainerPadding = new Padding(3);
            segmentedPill1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pillItem1.Text = "Chi Nhánh";
            pillItem2.Text = "Khu vực";
            pillItem3.Text = "Bàn";
            pillItem4.Text = "Sảnh tiệc";
            segmentedPill1.Items.Add(pillItem1);
            segmentedPill1.Items.Add(pillItem2);
            segmentedPill1.Items.Add(pillItem3);
            segmentedPill1.Items.Add(pillItem4);
            segmentedPill1.Location = new Point(12, 83);
            segmentedPill1.Name = "segmentedPill1";
            segmentedPill1.Size = new Size(388, 46);
            segmentedPill1.TabIndex = 13;
            segmentedPill1.Text = "segmentedPill1";
            segmentedPill1.SelectedIndexChanged += segmentedPill1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(261, 35);
            label1.TabIndex = 11;
            label1.Text = "Chi Nhánh/Bàn/Sảnh";
            // 
            // PanelTimKiemChiNhanh
            // 
            PanelTimKiemChiNhanh.BackColor = Color.Transparent;
            PanelTimKiemChiNhanh.BorderRadius = 25;
            PanelTimKiemChiNhanh.Controls.Add(btnThemChiNhanh);
            PanelTimKiemChiNhanh.Controls.Add(cbbLocCN);
            PanelTimKiemChiNhanh.Controls.Add(txtTimKiemChiNhanh);
            PanelTimKiemChiNhanh.CustomizableEdges = customizableEdges7;
            PanelTimKiemChiNhanh.FillColor = Color.White;
            PanelTimKiemChiNhanh.FillColor2 = Color.White;
            PanelTimKiemChiNhanh.Location = new Point(8, 135);
            PanelTimKiemChiNhanh.Name = "PanelTimKiemChiNhanh";
            PanelTimKiemChiNhanh.ShadowDecoration.CustomizableEdges = customizableEdges8;
            PanelTimKiemChiNhanh.Size = new Size(1130, 77);
            PanelTimKiemChiNhanh.TabIndex = 14;
            // 
            // btnThemChiNhanh
            // 
            btnThemChiNhanh.BorderColor = Color.DimGray;
            btnThemChiNhanh.BorderRadius = 20;
            btnThemChiNhanh.BorderThickness = 1;
            btnThemChiNhanh.CustomizableEdges = customizableEdges1;
            btnThemChiNhanh.DisabledState.BorderColor = Color.DarkGray;
            btnThemChiNhanh.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThemChiNhanh.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThemChiNhanh.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThemChiNhanh.FillColor = Color.FromArgb(192, 255, 255);
            btnThemChiNhanh.Font = new Font("Segoe UI", 9F);
            btnThemChiNhanh.ForeColor = Color.Black;
            btnThemChiNhanh.Location = new Point(890, 7);
            btnThemChiNhanh.Name = "btnThemChiNhanh";
            btnThemChiNhanh.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnThemChiNhanh.Size = new Size(225, 56);
            btnThemChiNhanh.TabIndex = 2;
            btnThemChiNhanh.Text = "Thêm Chi Nhánh";
            // 
            // cbbLocCN
            // 
            cbbLocCN.BackColor = Color.Transparent;
            cbbLocCN.BorderRadius = 18;
            cbbLocCN.CustomizableEdges = customizableEdges3;
            cbbLocCN.DrawMode = DrawMode.OwnerDrawFixed;
            cbbLocCN.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbLocCN.FocusedColor = Color.FromArgb(94, 148, 255);
            cbbLocCN.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbbLocCN.Font = new Font("Segoe UI", 10F);
            cbbLocCN.ForeColor = Color.FromArgb(68, 88, 112);
            cbbLocCN.ItemHeight = 30;
            cbbLocCN.Location = new Point(538, 16);
            cbbLocCN.Name = "cbbLocCN";
            cbbLocCN.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cbbLocCN.Size = new Size(235, 36);
            cbbLocCN.TabIndex = 1;
            // 
            // txtTimKiemChiNhanh
            // 
            txtTimKiemChiNhanh.BorderRadius = 18;
            txtTimKiemChiNhanh.CustomizableEdges = customizableEdges5;
            txtTimKiemChiNhanh.DefaultText = "";
            txtTimKiemChiNhanh.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtTimKiemChiNhanh.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtTimKiemChiNhanh.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtTimKiemChiNhanh.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtTimKiemChiNhanh.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTimKiemChiNhanh.Font = new Font("Segoe UI", 9F);
            txtTimKiemChiNhanh.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTimKiemChiNhanh.Location = new Point(16, 16);
            txtTimKiemChiNhanh.Margin = new Padding(3, 4, 3, 4);
            txtTimKiemChiNhanh.Name = "txtTimKiemChiNhanh";
            txtTimKiemChiNhanh.PlaceholderText = "Tìm kiếm chi nhanh . . .";
            txtTimKiemChiNhanh.SelectedText = "";
            txtTimKiemChiNhanh.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtTimKiemChiNhanh.Size = new Size(506, 47);
            txtTimKiemChiNhanh.TabIndex = 0;
            // 
            // panelKhuVuc
            // 
            panelKhuVuc.AutoScroll = true;
            panelKhuVuc.Controls.Add(label3);
            panelKhuVuc.Controls.Add(btnThem);
            panelKhuVuc.Controls.Add(dgvKhuVuc);
            panelKhuVuc.CustomizableEdges = customizableEdges11;
            panelKhuVuc.Location = new Point(4, 129);
            panelKhuVuc.Name = "panelKhuVuc";
            panelKhuVuc.ShadowDecoration.CustomizableEdges = customizableEdges12;
            panelKhuVuc.Size = new Size(1127, 644);
            panelKhuVuc.TabIndex = 16;
            panelKhuVuc.Visible = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("SimSun", 13.8F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(31, 18);
            label3.Name = "label3";
            label3.Size = new Size(211, 23);
            label3.TabIndex = 2;
            label3.Text = "Danh Sách Khu Vực";
            // 
            // btnThem
            // 
            btnThem.BorderColor = Color.DarkGray;
            btnThem.BorderRadius = 18;
            btnThem.CustomizableEdges = customizableEdges9;
            btnThem.DisabledState.BorderColor = Color.DarkGray;
            btnThem.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThem.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThem.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThem.FillColor = Color.Black;
            btnThem.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(891, 31);
            btnThem.Name = "btnThem";
            btnThem.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnThem.Size = new Size(225, 56);
            btnThem.TabIndex = 1;
            btnThem.Text = "+Thêm Khu Vực";
            // 
            // dgvKhuVuc
            // 
            dgvKhuVuc.AllowUserToAddRows = false;
            dgvKhuVuc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKhuVuc.Location = new Point(12, 93);
            dgvKhuVuc.Name = "dgvKhuVuc";
            dgvKhuVuc.ReadOnly = true;
            dgvKhuVuc.RowHeadersWidth = 51;
            dgvKhuVuc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKhuVuc.Size = new Size(1104, 546);
            dgvKhuVuc.TabIndex = 0;
            dgvKhuVuc.CellDoubleClick += dgvKhuVuc_CellDoubleClick;
            // 
            // panelBan
            // 
            panelBan.BorderRadius = 10;
            panelBan.Controls.Add(PanelThaoTacBan);
            panelBan.Controls.Add(PanelSoDoBan);
            panelBan.Controls.Add(PanelDanhSachBan);
            panelBan.CustomizableEdges = customizableEdges27;
            panelBan.Location = new Point(6, 129);
            panelBan.Name = "panelBan";
            panelBan.ShadowDecoration.CustomizableEdges = customizableEdges28;
            panelBan.Size = new Size(1172, 661);
            panelBan.TabIndex = 17;
            panelBan.Visible = false;
            // 
            // PanelThaoTacBan
            // 
            PanelThaoTacBan.Controls.Add(btnThemBan);
            PanelThaoTacBan.Controls.Add(cbbTrangThai);
            PanelThaoTacBan.Controls.Add(cbbKhuVuc);
            PanelThaoTacBan.Controls.Add(txtTimBan);
            PanelThaoTacBan.CustomizableEdges = customizableEdges21;
            PanelThaoTacBan.Location = new Point(7, 10);
            PanelThaoTacBan.Name = "PanelThaoTacBan";
            PanelThaoTacBan.ShadowDecoration.CustomizableEdges = customizableEdges22;
            PanelThaoTacBan.Size = new Size(1143, 77);
            PanelThaoTacBan.TabIndex = 2;
            // 
            // btnThemBan
            // 
            btnThemBan.BorderRadius = 18;
            btnThemBan.CustomizableEdges = customizableEdges13;
            btnThemBan.DisabledState.BorderColor = Color.DarkGray;
            btnThemBan.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThemBan.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThemBan.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThemBan.FillColor = Color.Black;
            btnThemBan.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnThemBan.ForeColor = Color.White;
            btnThemBan.Location = new Point(1004, 6);
            btnThemBan.Name = "btnThemBan";
            btnThemBan.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnThemBan.Size = new Size(128, 53);
            btnThemBan.TabIndex = 3;
            btnThemBan.Text = "+Thêm Bàn";
            // 
            // cbbTrangThai
            // 
            cbbTrangThai.BackColor = Color.Transparent;
            cbbTrangThai.BorderRadius = 18;
            cbbTrangThai.CustomizableEdges = customizableEdges15;
            cbbTrangThai.DrawMode = DrawMode.OwnerDrawFixed;
            cbbTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbTrangThai.FocusedColor = Color.FromArgb(94, 148, 255);
            cbbTrangThai.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbbTrangThai.Font = new Font("Segoe UI", 10F);
            cbbTrangThai.ForeColor = Color.FromArgb(68, 88, 112);
            cbbTrangThai.ItemHeight = 30;
            cbbTrangThai.Location = new Point(775, 6);
            cbbTrangThai.Name = "cbbTrangThai";
            cbbTrangThai.ShadowDecoration.CustomizableEdges = customizableEdges16;
            cbbTrangThai.Size = new Size(197, 36);
            cbbTrangThai.TabIndex = 2;
            // 
            // cbbKhuVuc
            // 
            cbbKhuVuc.BackColor = Color.Transparent;
            cbbKhuVuc.BorderRadius = 18;
            cbbKhuVuc.CustomizableEdges = customizableEdges17;
            cbbKhuVuc.DrawMode = DrawMode.OwnerDrawFixed;
            cbbKhuVuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbKhuVuc.FocusedColor = Color.FromArgb(94, 148, 255);
            cbbKhuVuc.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbbKhuVuc.Font = new Font("Segoe UI", 10F);
            cbbKhuVuc.ForeColor = Color.FromArgb(68, 88, 112);
            cbbKhuVuc.ItemHeight = 30;
            cbbKhuVuc.Location = new Point(560, 8);
            cbbKhuVuc.Name = "cbbKhuVuc";
            cbbKhuVuc.ShadowDecoration.CustomizableEdges = customizableEdges18;
            cbbKhuVuc.Size = new Size(209, 36);
            cbbKhuVuc.TabIndex = 1;
            // 
            // txtTimBan
            // 
            txtTimBan.BorderRadius = 18;
            txtTimBan.CustomizableEdges = customizableEdges19;
            txtTimBan.DefaultText = "";
            txtTimBan.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtTimBan.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtTimBan.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtTimBan.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtTimBan.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTimBan.Font = new Font("Segoe UI", 9F);
            txtTimBan.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTimBan.Location = new Point(15, 8);
            txtTimBan.Margin = new Padding(3, 4, 3, 4);
            txtTimBan.Name = "txtTimBan";
            txtTimBan.PlaceholderText = "Tìm Số Bàn";
            txtTimBan.SelectedText = "";
            txtTimBan.ShadowDecoration.CustomizableEdges = customizableEdges20;
            txtTimBan.Size = new Size(506, 60);
            txtTimBan.TabIndex = 0;
            // 
            // PanelSoDoBan
            // 
            PanelSoDoBan.AutoScroll = true;
            PanelSoDoBan.Controls.Add(lblPhanChiaKhuVuc);
            PanelSoDoBan.CustomizableEdges = customizableEdges23;
            PanelSoDoBan.Location = new Point(553, 93);
            PanelSoDoBan.Name = "PanelSoDoBan";
            PanelSoDoBan.ShadowDecoration.CustomizableEdges = customizableEdges24;
            PanelSoDoBan.Size = new Size(575, 551);
            PanelSoDoBan.TabIndex = 1;
            // 
            // lblPhanChiaKhuVuc
            // 
            lblPhanChiaKhuVuc.AutoSize = true;
            lblPhanChiaKhuVuc.BackColor = Color.White;
            lblPhanChiaKhuVuc.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPhanChiaKhuVuc.Location = new Point(27, 49);
            lblPhanChiaKhuVuc.Name = "lblPhanChiaKhuVuc";
            lblPhanChiaKhuVuc.Size = new Size(102, 25);
            lblPhanChiaKhuVuc.TabIndex = 1;
            lblPhanChiaKhuVuc.Text = "Khu Vực A";
            lblPhanChiaKhuVuc.Click += lblPhanChiaKhuVuc_Click;
            // 
            // PanelDanhSachBan
            // 
            PanelDanhSachBan.AutoScroll = true;
            PanelDanhSachBan.CustomizableEdges = customizableEdges25;
            PanelDanhSachBan.Location = new Point(7, 93);
            PanelDanhSachBan.Name = "PanelDanhSachBan";
            PanelDanhSachBan.ShadowDecoration.CustomizableEdges = customizableEdges26;
            PanelDanhSachBan.Size = new Size(536, 548);
            PanelDanhSachBan.TabIndex = 0;
            // 
            // panelSanh
            // 
            panelSanh.AutoScroll = true;
            panelSanh.Controls.Add(panelTimKiemSanh);
            panelSanh.CustomizableEdges = customizableEdges33;
            panelSanh.Location = new Point(3, 129);
            panelSanh.Name = "panelSanh";
            panelSanh.ShadowDecoration.CustomizableEdges = customizableEdges34;
            panelSanh.Size = new Size(1150, 658);
            panelSanh.TabIndex = 0;
            panelSanh.Visible = false;
            // 
            // panelTimKiemSanh
            // 
            panelTimKiemSanh.Controls.Add(txtTimSanh);
            panelTimKiemSanh.Controls.Add(btnThemSanh);
            panelTimKiemSanh.Location = new Point(14, 12);
            panelTimKiemSanh.Name = "panelTimKiemSanh";
            panelTimKiemSanh.Size = new Size(1112, 60);
            panelTimKiemSanh.TabIndex = 0;
            // 
            // txtTimSanh
            // 
            txtTimSanh.BorderRadius = 18;
            txtTimSanh.CustomizableEdges = customizableEdges29;
            txtTimSanh.DefaultText = "";
            txtTimSanh.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtTimSanh.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtTimSanh.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtTimSanh.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtTimSanh.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTimSanh.Font = new Font("Segoe UI", 9F);
            txtTimSanh.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTimSanh.Location = new Point(24, 8);
            txtTimSanh.Margin = new Padding(3, 4, 3, 4);
            txtTimSanh.Name = "txtTimSanh";
            txtTimSanh.PlaceholderText = "Tìm Sảnh...";
            txtTimSanh.SelectedText = "";
            txtTimSanh.ShadowDecoration.CustomizableEdges = customizableEdges30;
            txtTimSanh.Size = new Size(643, 48);
            txtTimSanh.TabIndex = 5;
            // 
            // btnThemSanh
            // 
            btnThemSanh.BorderRadius = 18;
            btnThemSanh.CustomizableEdges = customizableEdges31;
            btnThemSanh.DisabledState.BorderColor = Color.DarkGray;
            btnThemSanh.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThemSanh.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThemSanh.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThemSanh.FillColor = Color.Black;
            btnThemSanh.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnThemSanh.ForeColor = Color.White;
            btnThemSanh.Location = new Point(966, 3);
            btnThemSanh.Name = "btnThemSanh";
            btnThemSanh.ShadowDecoration.CustomizableEdges = customizableEdges32;
            btnThemSanh.Size = new Size(128, 48);
            btnThemSanh.TabIndex = 4;
            btnThemSanh.Text = "Thêm Sảnh";
            // 
            // panelChiNhanh
            // 
            panelChiNhanh.Location = new Point(7, 218);
            panelChiNhanh.Name = "panelChiNhanh";
            panelChiNhanh.Size = new Size(1146, 552);
            panelChiNhanh.TabIndex = 15;
            // 
            // FrmChiNhanh
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 900);
            Controls.Add(panelSanh);
            Controls.Add(panelKhuVuc);
            Controls.Add(panelBan);
            Controls.Add(PanelTimKiemChiNhanh);
            Controls.Add(panelChiNhanh);
            Controls.Add(segmentedPill1);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmChiNhanh";
            Text = "FrmChiNhanh";
            Load += FrmChiNhanh_Load;
            PanelTimKiemChiNhanh.ResumeLayout(false);
            panelKhuVuc.ResumeLayout(false);
            panelKhuVuc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvKhuVuc).EndInit();
            panelBan.ResumeLayout(false);
            PanelThaoTacBan.ResumeLayout(false);
            PanelSoDoBan.ResumeLayout(false);
            PanelSoDoBan.PerformLayout();
            panelSanh.ResumeLayout(false);
            panelTimKiemSanh.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private VanThuan.UI.SegmentedPill segmentedPill1;
        private Label label1;
        private Guna.UI2.WinForms.Guna2GradientPanel PanelTimKiemChiNhanh;
        private Guna.UI2.WinForms.Guna2Button btnThemChiNhanh;
        private Guna.UI2.WinForms.Guna2ComboBox cbbLocCN;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiemChiNhanh;
        private Panel panelChiNhanh;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelKhuVuc;
        private Guna.UI2.WinForms.Guna2Button btnThem;
        private DataGridView dgvKhuVuc;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelBan;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelSanh;
        private Label label3;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel PanelSoDoBan;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel PanelDanhSachBan;
        private Label lblPhanChiaKhuVuc;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel PanelThaoTacBan;
        private Guna.UI2.WinForms.Guna2ComboBox cbbKhuVuc;
        private Guna.UI2.WinForms.Guna2TextBox txtTimBan;
        private Guna.UI2.WinForms.Guna2Button btnThemBan;
        private Guna.UI2.WinForms.Guna2ComboBox cbbTrangThai;
        private Panel panelTimKiemSanh;
        private Guna.UI2.WinForms.Guna2Button btnThemSanh;
        private Guna.UI2.WinForms.Guna2TextBox txtTimSanh;
    }
}