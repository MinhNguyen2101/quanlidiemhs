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
    public partial class frm_HocSinh : Form
    {
        public frm_HocSinh()
        {
            InitializeComponent();
        }
        QLDDataContext dt = new QLDDataContext();
        private void frm_HocSinh_Load(object sender, EventArgs e)
        {
            cboMaLop.DisplayMember = "TenLop";
            cboMaLop.ValueMember = "MaLop";
            cboMaLop.DataSource = dt.Lop_SelectAll();
           
        }
        private void cboMalop_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_Hocsinh.DataSource = dt.HocSinh_SelectMaLop(cboMaLop.SelectedValue.ToString());
            txtMaHocSinh.DataBindings.Clear();
            txtMaHocSinh.DataBindings.Add("Text", dgv_Hocsinh.DataSource, "MaHocSinh");
            txtHoTen.DataBindings.Clear();
            txtHoTen.DataBindings.Add("Text", dgv_Hocsinh.DataSource, "HoTen");
            //dateNgaySinh.DataBindings.Clear();
            //dateNgaySinh.DataBindings.Add("Text", dgv_Hocsinh.DataSource, "NgaySinh");
            txtGioiTinh.DataBindings.Clear();
            txtGioiTinh.DataBindings.Add("Text", dgv_Hocsinh.DataSource, "GioiTinh");
            txtQueQuan.DataBindings.Clear();
            txtQueQuan.DataBindings.Add("Text", dgv_Hocsinh.DataSource, "QueQuan");
            dateNgaySinh.Hide();
            lblNgaySinh.Hide();
            txtHoTen.Enabled = false;
            txtGioiTinh.Enabled = false;
            txtQueQuan.Enabled = false;
            txtMaHocSinh.Enabled = false;
        }
        Boolean add = false;
        private void btnThem_Click(object sender, EventArgs e)
        {
            /*txtMaHocSinh.Enabled = true;*/
            txtHoTen.Enabled = true;
            txtGioiTinh.Enabled = true;
            txtQueQuan.Enabled = true;
            dateNgaySinh.Enabled = true;
            txtHoTen.Text = "";
            txtGioiTinh.Text = "";
            txtQueQuan.Text = "";
            //dateNgaySinh.Value = DateTime.Now;
            dateNgaySinh.Show();
            lblNgaySinh.Show();
            add = true;
            SqlConnection conn = new SqlConnection(@"Data Source=MINH_NGUYEN\SQLEXPRESS;Initial Catalog=QuanLyDiemHocSinh;Integrated Security=True");
            conn.Open();
            string sql = "select count(*) from HocSinh ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            int k = Convert.ToInt32(cmd.ExecuteScalar());
            string g = "";
            g = cboMaLop.SelectedValue.ToString();
            k = k + 1;
            if (k < 10)
            {
                g = g + "000";
                g = g + k.ToString();
            }
            else if (k < 100 && k >= 10)
            {
                g = g + "00";
                g = g + k.ToString();
            }
            else if (k >= 100 && k < 1000)
            {
                g = g + "0";
                g = g + k.ToString();
            }
            else
            {
                g = g + k.ToString();
            }    
            txtMaHocSinh.Text = g;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtHoTen.Enabled = true;
            txtGioiTinh.Enabled = true;
            txtQueQuan.Enabled = true;
            dateNgaySinh.Enabled = true;
            dateNgaySinh.Show();
            lblNgaySinh.Show();
            dateNgaySinh.DataBindings.Clear();
            dateNgaySinh.DataBindings.Add("Text", dgv_Hocsinh.DataSource, "NgaySinh");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    dt.HocSinh_Delete(txtMaHocSinh.Text);
                    cboMalop_SelectedIndexChanged(sender, e);
                    MessageBox.Show("Xóa thành công!");
                }
            }
            catch
            {
                MessageBox.Show("Không thể xóa vì có khóa ngoại!");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=MINH_NGUYEN\SQLEXPRESS;Initial Catalog=QuanLyDiemHocSinh;Integrated Security=True");
            conn.Open();
            string sql = "select count(*) from HocSinh where MaHocSinh='" + txtMaHocSinh.Text + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            int coutn_check = Convert.ToInt32(cmd.ExecuteScalar());
            if (txtMaHocSinh.Text == "" || txtHoTen.Text == "" || txtQueQuan.Text == "" || txtGioiTinh.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }
            else
            {
                if (add)
                {
                    if (coutn_check > 0)
                    {
                        MessageBox.Show("Mã học sinh đã tồn tại!");
                    }
                    else
                    {
                        dt.HocSinh_Insert(txtMaHocSinh.Text, txtHoTen.Text, dateNgaySinh.Value, txtGioiTinh.Text, txtQueQuan.Text, cboMaLop.SelectedValue.ToString());
                        cboMalop_SelectedIndexChanged(sender, e);
                        MessageBox.Show("Thêm mới thành công!!!");
                        add = false;
                    }
                }
                else
                {
                    dt.HocSinhs_Update(txtMaHocSinh.Text, txtHoTen.Text, dateNgaySinh.Value, txtGioiTinh.Text, txtQueQuan.Text);
                    MessageBox.Show("Sửa thông tin thành công!");
                    cboMalop_SelectedIndexChanged(sender, e);

                }
            }
        }

        private void txtSeach_TextChanged(object sender, EventArgs e)
        {
            dgv_Hocsinh.DataSource = dt.HocSinh_SearchSupper(txtSeach.Text, txtSeach.Text);
        }
    }
}
