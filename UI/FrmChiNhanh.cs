using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    [SupportedOSPlatform("windows")]
    public partial class FrmChiNhanh : Form
    {
        public FrmChiNhanh()
        {
            InitializeComponent();
        }

        private void FrmChiNhanh_Load(object sender, EventArgs e)
        {
            LoadDataChiNhanh(); 
        }
        private void LoadDataChiNhanh()
        {
            dgvChiNhanh.AutoGenerateColumns = false;
            dgvChiNhanh.Rows.Clear();

            AddBranch("Chi nhánh Quận 1", "123 Nguyễn Huệ, Q1, TP.HCM", "028 3821 1234", true);
            AddBranch("Chi nhánh Quận 3", "456 Võ Văn Tần, Q3, TP.HCM", "028 3930 2345", true);
            AddBranch("Chi nhánh Thủ Đức", "789 Võ Văn Ngân, Thủ Đức, TP.HCM", "028 3897 3456", true);
        }

        private void AddBranch(string ten, string diaChi, string soDt, bool hoatDong)
        {
            int rowIndex = dgvChiNhanh.Rows.Add(ten, diaChi, soDt, hoatDong ? "Hoạt động" : "Ngừng", null);
            dgvChiNhanh.Rows[rowIndex].Cells["ThaoTac"].Value = "edit|delete";
        }



    }
}
