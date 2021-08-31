using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiDiem
{
    public partial class frm_Lop : Form
    {
        QLDDataContext dt = new QLDDataContext();
        public frm_Lop()
        {
            InitializeComponent();
        }

        private void frm_Lop_Load(object sender, EventArgs e)
        {
            dgv_Lop.DataSource = dt.Lop_SelectAll();
            txtMaLop.DataBindings.Clear();
            txtMaLop.DataBindings.Add("Text", dgv_Lop.DataSource, "MaLop");
            txtTenLop.DataBindings.Clear();
            txtTenLop.DataBindings.Add("Text", dgv_Lop.DataSource, "TenLop");
            txtKhoi.DataBindings.Clear();
            txtKhoi.DataBindings.Add("Text", dgv_Lop.DataSource, "Khoi");
            txtMaLop.Enabled = false;
            txtTenLop.Enabled = false;
            txtKhoi.Enabled = false;
        }
        Boolean add = false;
      
        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaLop.Enabled = true;
            txtTenLop.Enabled = true;
            txtKhoi.Enabled = true;
            txtMaLop.Text = "";
            txtTenLop.Text = "";
            txtKhoi.Text = "";
            add = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=MINH_NGUYEN\SQLEXPRESS;Initial Catalog=QuanLyDiemHocSinh;Integrated Security=True");
            conn.Open();
            string sql = "select count(*) from Lop where MaLop='" + txtMaLop.Text + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            int coutn_check = Convert.ToInt32(cmd.ExecuteScalar());
            if (txtMaLop.Text == "" || txtTenLop.Text == "" || txtKhoi.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
            }    
            else
            {
                if (add)
                {
                    if(coutn_check>0)
                    {
                        MessageBox.Show("Mã đã tồn tại");
                    }
                    else
                    {
                        dt.Lop_Insert(txtMaLop.Text, txtTenLop.Text, txtKhoi.Text);
                        frm_Lop_Load(sender, e);
                        MessageBox.Show("Thêm mới thành công!");
                        add = false;
                        txtMaLop.Enabled = false;
                        txtTenLop.Enabled = false;
                        txtKhoi.Enabled = false;
                    }

                }
                else
                {
                    dt.Lops_Update(txtMaLop.Text, txtTenLop.Text, txtKhoi.Text);
                    frm_Lop_Load(sender, e);
                    MessageBox.Show("Sửa thông tin thành công!");
                    txtMaLop.Enabled = false;
                    txtTenLop.Enabled = false;
                    txtKhoi.Enabled = false;
                }    
            }    
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtTenLop.Enabled = true;
            txtKhoi.Enabled = true;
            add = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo);
            if(result==DialogResult.Yes)
            {
                dt.Lop_Delete(txtMaLop.Text);
                frm_Lop_Load(sender, e);
                MessageBox.Show("Xóa thành công!");
            }    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dgv_Lop.DataSource = dt.Lop_Search(txtSeach.Text, txtSeach.Text, txtSeach.Text);
            txtMaLop.DataBindings.Clear();
            txtMaLop.DataBindings.Add("Text", dgv_Lop.DataSource, "MaLop");
            txtTenLop.DataBindings.Clear();
            txtTenLop.DataBindings.Add("Text", dgv_Lop.DataSource, "TenLop");
            txtKhoi.DataBindings.Clear();
            txtKhoi.DataBindings.Add("Text", dgv_Lop.DataSource, "Khoi");
        }
    }
}
