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
    public partial class frm_NamHoc : Form
    {
        QLDDataContext dt = new QLDDataContext();
        public frm_NamHoc()
        {
            InitializeComponent();
        }

        private void frm_NamHoc_Load(object sender, EventArgs e)
        {
            dgv_NamHoc.DataSource = dt.HocKy_SelectAll();
            txtMaHocKy.Enabled = false;
            txtNamHoc.Enabled = false;
            txtTenHocKy.Enabled = false;
            txtMaHocKy.DataBindings.Clear();
            txtMaHocKy.DataBindings.Add("Text", dgv_NamHoc.DataSource, "MaHocKy");
            txtNamHoc.DataBindings.Clear();
            txtNamHoc.DataBindings.Add("Text", dgv_NamHoc.DataSource, "NamHoc");
            txtTenHocKy.DataBindings.Clear();
            txtTenHocKy.DataBindings.Add("Text", dgv_NamHoc.DataSource, "TenHocKy");
        }
        Boolean add = false;
        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaHocKy.Enabled = true;
            txtNamHoc.Enabled = true;
            txtTenHocKy.Enabled = true;
            txtTenHocKy.Text = "";
            txtNamHoc.Text = "";
            txtMaHocKy.Text = "";
            add = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtNamHoc.Enabled = true;
            txtTenHocKy.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=MINH_NGUYEN\SQLEXPRESS;Initial Catalog=QuanLyDiemHocSinh;Integrated Security=True");
            conn.Open();
            string sql = "select count(*) from HocKy where MaHocKy='" + txtMaHocKy.Text + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            int coutn_check = Convert.ToInt32(cmd.ExecuteScalar());
            if (txtMaHocKy.Text == "" || txtNamHoc.Text == "" || txtTenHocKy.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
            }    
            else
            {
                if(add)
                {
                    if(coutn_check>0)
                    {
                        MessageBox.Show("Mã học kỳ đã tồn tại");
                    }
                    else
                    {
                        dt.HocKy_Insert(txtMaHocKy.Text, txtTenHocKy.Text, txtNamHoc.Text);
                        MessageBox.Show("Thêm mới thành công!");
                        frm_NamHoc_Load(sender, e);
                        add = false;
                    }    
                }
                else
                {
                    dt.HocKy_Update(txtMaHocKy.Text, txtTenHocKy.Text, txtNamHoc.Text);
                    MessageBox.Show("Sửa thông tin thành công!");
                    frm_NamHoc_Load(sender, e);
                }    
            }    
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn cóa không?", "Thông báo", MessageBoxButtons.YesNo);
                if(result==DialogResult.Yes)
                {
                    dt.HocKy_Delete(txtMaHocKy.Text);
                    MessageBox.Show("Xóa thành công!");
                    frm_NamHoc_Load(sender, e);
                }    
            }
            catch
            {
                MessageBox.Show("Không thể xóa!");//vì có khóa ngoại
            }
        }
    }
}
