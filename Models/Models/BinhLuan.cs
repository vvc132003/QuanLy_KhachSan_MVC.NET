using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class BinhLuan
    {
        public int id { get; set; }
        public int idkhachhang { get; set; }
        public string noidung { get; set; }
        public DateTime thoigianbinhluan { get; set; }
        public string trangthai { get; set; }
        public int idphong { get; set; }
    }
}
