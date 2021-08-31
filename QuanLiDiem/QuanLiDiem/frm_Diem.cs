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
    public partial class frm_Diem : Form
    {
        QLDDataContext dt = new QLDDataContext();
        public frm_Diem()
        {
            InitializeComponent();
        }

        private void frm_Diem_Load(object sender, EventArgs e)
        {
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
            cboLop.DataSource = dt.Lop_SelectAll();
            cboNamHoc.DisplayMember = "NamHoc";
            cboNamHoc.ValueMember = "MaHocKy";
            cboNamHoc.DataSource = dt.HocKy_SelectAll();
            txtMaMonHoc.Hide();
            lbMamonhoc.Hide();
            txtDiem15p.Enabled = false;
            txtDiem45p.Enabled = false;
            txtDiemThi.Enabled = false;
        }

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeviewHocSinh.Nodes.Clear();
            foreach (var r in dt.HocSinh_SelectMaLop(cboLop.SelectedValue.ToString()))
            {
                TreeNode root = new TreeNode();
                root.Name = r.MaHocSinh;
                root.Text = r.HoTen;
                treeviewHocSinh.Nodes.Add(root.Name, root.Text);
            }
        }

        private void treeviewHocSinh_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            foreach (var r in dt.HocSinh_SelectID(e.Node.Name))
            {
                lblMaHocSinh.Text = e.Node.Name;
                lblTenHocSinh.Text = e.Node.Text;
            }
            var table = new DataTable();
            DataColumnCollection col = table.Columns;
            if (!col.Contains("MaMonHoc"))  // chưa tồn tại cột thì thêm cột
                table.Columns.Add("MaMonHoc", typeof(String));
            if (!col.Contains("TenMonHoc"))
                table.Columns.Add("TenMonHoc", typeof(String));
            if (!col.Contains("Diem15p"))
                table.Columns.Add("Diem15p", typeof(String));
            if (!col.Contains("Diem45p"))
                table.Columns.Add("Diem45p", typeof(String));
            if (!col.Contains("DiemThi"))
                table.Columns.Add("DiemThi", typeof(String));
            if (!col.Contains("Tongket"))
                table.Columns.Add("Tongket", typeof(String));
            foreach (var m in dt.MonHoc_SelectMaHocKy(cboNamHoc.SelectedValue.ToString()))
            {
                DataRow r = table.NewRow();
                r["MaMonHoc"] = m.MaMonHoc;
                r["TenMonHoc"] = m.TenMonHoc;

                table.Rows.Add(r);
                foreach (var d in dt.Diem_Seach(m.MaMonHoc, e.Node.Name))
                {
                    r["Diem15p"] = d.Diem15p;
                    r["Diem45p"] = d.Diem45p;
                    r["DiemThi"] = d.DiemThi;
                    r["TongKet"] = d.Tongket;
                    
                }
            } //End for Mã Môn Học
            dgv_Diem.DataSource = table;
            txtMaMonHoc.DataBindings.Clear();
            txtMaMonHoc.DataBindings.Add("Text", dgv_Diem.DataSource, "MaMonHoc");
            lblTenMonHoc.DataBindings.Clear();
            lblTenMonHoc.DataBindings.Add("Text", dgv_Diem.DataSource, "TenMonHoc");
            txtDiem15p.DataBindings.Clear();
            txtDiem15p.DataBindings.Add("Text", dgv_Diem.DataSource, "Diem15p");
            txtDiem45p.DataBindings.Clear();
            txtDiem45p.DataBindings.Add("Text", dgv_Diem.DataSource, "Diem45p");
            txtDiemThi.DataBindings.Clear();
            txtDiemThi.DataBindings.Add("Text", dgv_Diem.DataSource, "DiemThi");
        }
        Boolean add = false;
        private void btnThem_Click(object sender, EventArgs e)
        {
            txtDiem15p.Enabled = true;
            txtDiem45p.Enabled = true;
            txtDiemThi.Enabled = true;
            add = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtDiem15p.Enabled = true;
            txtDiem45p.Enabled = true;
            txtDiemThi.Enabled = true;
            txtDiem15p.DataBindings.Clear();
            txtDiem15p.DataBindings.Add("Text",dgv_Diem.DataSource,"Diem15p");
            txtDiem45p.DataBindings.Clear();
            txtDiem45p.DataBindings.Add("Text", dgv_Diem.DataSource, "Diem45p");
            txtDiemThi.DataBindings.Clear();
            txtDiemThi.DataBindings.Add("Text", dgv_Diem.DataSource, "DiemThi");
            add = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtDiem15p.Text == "" || txtDiem45p.Text == "" || txtDiemThi.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!!!");
            }   
            else
            {
                if(add)
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=MINH_NGUYEN\SQLEXPRESS;Initial Catalog=QuanLyDiemHocSinh;Integrated Security=True");
                    conn.Open();
                    string sql = "select count(*) from Diem where MaMonHoc='" + txtMaMonHoc.Text + "' and MaHocSinh='" + lblMaHocSinh.Text + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    int coutn_check = Convert.ToInt32(cmd.ExecuteScalar());
                    if(coutn_check>0)
                    {
                        MessageBox.Show("Học sinh đã có điểm môn này.");
                    }   
                    else
                    {                                      
                            dt.Diem_Insert(lblMaHocSinh.Text, txtMaMonHoc.Text, cboLop.SelectedValue.ToString(), Convert.ToDouble(txtDiem15p.Text), Convert.ToDouble(txtDiem45p.Text), Convert.ToDouble(txtDiemThi.Text));
                            dt.Diem_Update(lblMaHocSinh.Text, txtMaMonHoc.Text, Convert.ToDouble(txtDiem15p.Text), Convert.ToDouble(txtDiem45p.Text), Convert.ToDouble(txtDiemThi.Text));
                            MessageBox.Show("Thêm mới thành công ");
                            cboLop_SelectedIndexChanged(sender, e);
                            add = false;
                            txtDiem15p.Enabled = false;
                            txtDiem45p.Enabled = false;
                            txtDiemThi.Enabled = false;
                    }    
                }   
                else
                {
                    dt.Diem_Update(lblMaHocSinh.Text, txtMaMonHoc.Text, Convert.ToDouble(txtDiem15p.Text), Convert.ToDouble(txtDiem45p.Text), Convert.ToDouble(txtDiemThi.Text));
                    MessageBox.Show("Sửa thành công ");
                    txtDiem15p.Enabled = false;
                    txtDiem45p.Enabled = false;
                    txtDiemThi.Enabled = false;
                }    
            }    
        }

        private void txtDiem15p_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch("^[0-1-1.5-2-2.5-3-3.5-4-4.5-5-5.5-6-6.5-7-7.5-8-8.5-9.5-10]", txtDiem15p.Text))
            {
                MessageBox.Show("Chỉ được nhập trong khoảng từ 0-10 và làm tròn đến 0.5!!");
                txtDiem15p.Text = string.Empty;
            }
        }

        private void txtDiem15p_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            //{
            //    e.Handled = true;
            //    MessageBox.Show("chỉ được nhập số!", "Thông báo", MessageBoxButtons.OK);

            //}
        }

        private void txtDiem45p_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            //{
            //    e.Handled = true;
            //    MessageBox.Show("chỉ được nhập số!", "Thông báo", MessageBoxButtons.OK);

            //}
        }

        private void txtDiemThi_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            //{
            //    e.Handled = true;
            //    MessageBox.Show("chỉ được nhập số!", "Thông báo", MessageBoxButtons.OK);

            //}
        }

        private void txtDiem45p_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch("^[0-1-1.5-2-2.5-3-3.5-4-4.5-5-5.5-6-6.5-7-7.5-8-8.5-9.5-10]", txtDiem45p.Text))
            {
                MessageBox.Show("Chỉ được nhập trong khoảng từ 0-10 và làm tròn đến 0.5!!");
                txtDiem15p.Text = string.Empty;
            }
        }

        private void txtDiemThi_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch("^[0-1-1.5-2-2.5-3-3.5-4-4.5-5-5.5-6-6.5-7-7.5-8-8.5-9.5-10]", txtDiemThi.Text))
            {
                MessageBox.Show("Chỉ được nhập trong khoảng từ 0-10 và làm tròn đến 0.5!!");
                txtDiem15p.Text = string.Empty;
            }
        }
    }
}
