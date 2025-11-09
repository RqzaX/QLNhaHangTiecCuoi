using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace UI
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public partial class Frm_QRThanhToan : Form
    {
        private decimal _soTien;
        private string _noiDung;

        public Frm_QRThanhToan(decimal soTien, string noiDung = "")
        {
            InitializeComponent();
            
            _soTien = soTien;
            _noiDung = noiDung;
            
            LoadThongTin();
        }

        private void LoadThongTin()
        {
            if (lbSoTien != null)
            {
                lbSoTien.Text = FormatTien(_soTien);
            }
            
            if (lbNoiDung != null && !string.IsNullOrEmpty(_noiDung))
            {
                lbNoiDung.Text = _noiDung;
            }
        }

        private string FormatTien(decimal amount)
        {
            return amount.ToString("#,##0") + " ₫";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
