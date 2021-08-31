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
    public partial class frm_DiemChiTiet : Form
    {
        QLDDataContext dt = new QLDDataContext();
        XuLy xl = new XuLy();
        public frm_DiemChiTiet()
        {
            InitializeComponent();
        }

        private void frm_DiemChiTiet_Load(object sender, EventArgs e)
        {
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
            cboLop.DataSource = dt.Lop_SelectAll();
            cboNamHoc.DisplayMember = "NamHoc";
            cboNamHoc.ValueMember = "MaHocKy";
            cboNamHoc.DataSource = dt.HocKy_SelectAll();
            button1.Hide();
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
                lblQueQuan.Text = r.QueQuan;
                lblNgaySinh.Text = r.NgaySinh.ToString();
                lblGioiTinh.Text = r.GioiTinh;
            }
            var table = new DataTable();
            DataColumnCollection col = table.Columns;
            if (!col.Contains("MaMonHoc"))
                table.Columns.Add("MaMonHoc", typeof(String));
            if (!col.Contains("TenMonHoc"))
                table.Columns.Add("TenMonHoc", typeof(String));
            if (!col.Contains("Diem45p"))
                table.Columns.Add("Diem45p", typeof(String));
            if (!col.Contains("Diem15p"))
                table.Columns.Add("Diem15p", typeof(String));
            if (!col.Contains("DiemThi"))
                table.Columns.Add("DiemThi", typeof(String));
            if (!col.Contains("Tongket"))
                table.Columns.Add("Tongket", typeof(String));
            if (!col.Contains("XepLoai"))
                table.Columns.Add("XepLoai", typeof(String));

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
                    r["XepLoai"] = xl.XepLoai(Convert.ToDouble(d.Tongket));
                }
            } //End for Mã Môn Học
            dgv_DiemChiTiet.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            //Khoier tạo WorkBook
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            //Khởi tạo workSheet
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            app.Visible = true;
            //Đổ dữ liệu 
            worksheet.Cells[1, 1] = "Bảng Điểm Chi Tiết Học Sinh ( "+cboNamHoc.SelectedValue.ToString()+")";
            worksheet.Cells[3, 1] = "Thông tin sinh viên:  ";
            worksheet.Cells[4, 2] = "Mã Học Sinh:";
            worksheet.Cells[4, 3] = lblMaHocSinh.Text;
            worksheet.Cells[5, 2] = "Họ Và Tên:";
            worksheet.Cells[5, 3] = lblTenHocSinh.Text;
            worksheet.Cells[6, 2] = "Ngày Sinh:";
            worksheet.Cells[6, 3] = lblNgaySinh.Text;
            worksheet.Cells[7, 2] = "Quê Quán:";
            worksheet.Cells[7, 3] = lblQueQuan.Text;

            worksheet.Cells[8, 1] = "STT";
            worksheet.Cells[8, 2] = "Mã môn học ";
            worksheet.Cells[8, 3] = "Tên môn học ";
            worksheet.Cells[8, 4] = "Điểm 15p";
            worksheet.Cells[8, 5] = "Điểm 45p";
            worksheet.Cells[8, 6] = "Điểm thi";
            worksheet.Cells[8, 7] = "Tổng kết";
            worksheet.Cells[8, 8] = "Xếp loại";
            for (int i = 0; i < dgv_DiemChiTiet.RowCount ; i++)
            {
                for (int j = 0; j < 7; j++)//j<7 số cột có trong datagrid view
                {
                    worksheet.Cells[i + 9, 1] = i + 1;// bắt đầu từ dòng thứ 9 số thứ tự 
                    worksheet.Cells[i + 9, j + 2] = dgv_DiemChiTiet.Rows[i].Cells[j].Value;
                }
            }
            int hs = dgv_DiemChiTiet.RowCount;
            //Định dạng trang 
            worksheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlPortrait;
            /*worksheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4;*/
            worksheet.PageSetup.LeftMargin = 0;
            worksheet.PageSetup.RightMargin = 0;
            worksheet.PageSetup.TopMargin = 0;
            worksheet.PageSetup.BottomMargin = 0;
            //Đinh dạng cột

            worksheet.Range["A1"].ColumnWidth = 8.25;
            //worksheet.Range["B1"].ColumnWidth = 17;
            worksheet.Range["C1"].ColumnWidth = 25;
            worksheet.Range["D1"].ColumnWidth = 19;
            worksheet.Range["E1"].ColumnWidth = 14;
            worksheet.Range["F1"].ColumnWidth = 14;
            worksheet.Range["G1"].ColumnWidth = 15;
            worksheet.Range["H1"].ColumnWidth = 15;
            /* worksheet.Range["H1"].ColumnWidth = 8.25;*/

            //Định dạng Font chữ 
            worksheet.Range["A1", "H100"].Font.Name = "Time New Roma";
            worksheet.Range["A1", "H100"].Font.Size = 14;
            worksheet.Range["A1", "H1"].MergeCells = true;
            worksheet.Range["A1", "H1"].Font.Bold = true;
            worksheet.Range["A8", "H8"].Font.Bold = true;
            //Kẻ bảng 
            worksheet.Range["A8", "H" + (hs + 10)].Borders.LineStyle = 1;


            //Định dạng các dòng text;
            worksheet.Range["A1", "H1"].HorizontalAlignment = 3;// Định dạng chữ nằm ở giữa dòng 
            worksheet.Range["A8", "A" + (hs + 10)].HorizontalAlignment = 3;
            worksheet.Range["B8", "B" + (hs + 10)].HorizontalAlignment = 3;
            worksheet.Range["C8", "C" + (hs + 10)].HorizontalAlignment = 3;
            worksheet.Range["D8", "D" + (hs + 10)].HorizontalAlignment = 3;
            worksheet.Range["E8", "E" + (hs + 10)].HorizontalAlignment = 3;
            worksheet.Range["F8", "F" + (hs + 10)].HorizontalAlignment = 3;
            worksheet.Range["G8", "G" + (hs + 10)].HorizontalAlignment = 3;
            worksheet.Range["H8", "H" + (hs + 10)].HorizontalAlignment = 3;
        }

       
    }
}
