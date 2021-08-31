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
    public partial class frm_MonHoc : Form
    {
        QLDDataContext dt = new QLDDataContext();
        public frm_MonHoc()
        {
            InitializeComponent();
        }

        private void frm_MonHoc_Load(object sender, EventArgs e)
        {
            cboNamHoc.DisplayMember = "NamHoc";
            cboNamHoc.ValueMember = "MaHocKy";
            cboNamHoc.DataSource = dt.HocKy_SelectAll();
        }

        private void cboNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_MonHoc.DataSource = dt.MonHoc_SelectMaHocKy(cboNamHoc.SelectedValue.ToString());
            txtMonHoc.Enabled = false;
            txtTenMonHoc.Enabled = false;
            txtMonHoc.DataBindings.Clear();
            txtMonHoc.DataBindings.Add("Text", dgv_MonHoc.DataSource, "MaMonHoc");
            txtTenMonHoc.DataBindings.Clear();
            txtTenMonHoc.DataBindings.Add("Text", dgv_MonHoc.DataSource, "TenMonHoc");
        }
        Boolean add = false;
        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMonHoc.Enabled = true;
            txtTenMonHoc.Enabled = true;
            txtMonHoc.Text = "";
            txtTenMonHoc.Text = "";
            add = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtTenMonHoc.Enabled = true;

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=MINH_NGUYEN\SQLEXPRESS;Initial Catalog=QuanLyDiemHocSinh;Integrated Security=True");
            conn.Open();
            string sql = "select count(*) from MonHoc where MaMonHoc='" + txtMonHoc.Text + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            int coutn_check = Convert.ToInt32(cmd.ExecuteScalar());
            if (txtMonHoc.Text == "" || txtTenMonHoc.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }    
            else
            {
                if (add)
                {
                    if (coutn_check > 0)
                    {
                        MessageBox.Show("Mã môn học đã tồn tại");
                    }
                    else
                    {
                        dt.MonHoc_Insert(txtMonHoc.Text, txtTenMonHoc.Text, cboNamHoc.SelectedValue.ToString());
                        MessageBox.Show("Thêm mới thành công!!!");
                        cboNamHoc_SelectedIndexChanged(sender, e);
                        add = false;
                    }
                }
                else
                {
                    dt.MonHoc_Update(txtMonHoc.Text, txtTenMonHoc.Text, cboNamHoc.SelectedValue.ToString());
                    MessageBox.Show("Sửa thông tin thành công!!!");
                    cboNamHoc_SelectedIndexChanged(sender, e);
                }    
            }    
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo);
                if(result==DialogResult.Yes)
                {
                    dt.MonHoc_Delete(txtMonHoc.Text);
                    MessageBox.Show("Xóa thành công!");
                    cboNamHoc_SelectedIndexChanged(sender, e);
                }   
            }
            catch
            {
                MessageBox.Show("Không thể xóa!!!");
            }
        }

        private void txtSeach_TextChanged(object sender, EventArgs e)
        {
            dgv_MonHoc.DataSource = dt.MonHoc_SearchSuper2(txtSeach.Text, txtSeach.Text);
        }
    }
}
