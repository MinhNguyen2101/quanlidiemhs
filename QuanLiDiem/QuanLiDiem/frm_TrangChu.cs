using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiDiem
{
    public partial class frm_TrangChu : Form
    {
        public frm_TrangChu()
        {
            InitializeComponent();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void quảnLýLớpHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Lop frmLop = new frm_Lop();
            frmLop.ShowDialog();
        }

        private void quảnLýNămHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_NamHoc frm_namhoc = new frm_NamHoc();
            frm_namhoc.ShowDialog();
        }

        private void quảnLýMônHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_MonHoc frm_monhoc = new frm_MonHoc();
            frm_monhoc.ShowDialog();
        }

        private void quảnLýHọcSinhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_HocSinh frm_hocsinh = new frm_HocSinh();
            frm_hocsinh.ShowDialog();
        }

        private void bảngĐiểmChiTiếtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_DiemChiTiet frm_diemchitiet = new frm_DiemChiTiet();
            frm_diemchitiet.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void quảnLýĐiểmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_Diem frmDiem = new frm_Diem();
            frmDiem.ShowDialog();
        }
    }
}
