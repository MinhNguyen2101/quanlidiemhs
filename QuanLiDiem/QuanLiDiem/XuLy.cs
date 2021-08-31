using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiDiem
{
    class XuLy
    {
        public String XepLoai(Double diem)
        {
            string x;
            if (diem >= 8)
            {
                x = "Giỏi";
            }
            else if (diem >= 6.5 && diem < 8)
            {
                x = "Khá";
            }
            else if (diem >= 5 && diem < 6.5)
            {
                x = "Trung bình";
            }
            else
            {
                x = "Yếu";
            }
            return x;
        }
    }
}
