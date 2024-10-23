using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class TachDon
    {
        public int id { get; set; }
        public string tensanpham { get; set; }
        public float thanhtien { get; set; }
        public string image { get; set; }
        public int soluong { get; set; }
        public int iddatphong { get; set; }
        public int idsanpham { get; set; }
        public string ghichu { get; set; }
    }
}
