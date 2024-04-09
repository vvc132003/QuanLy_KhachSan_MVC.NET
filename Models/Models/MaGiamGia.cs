using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class MaGiamGia
    {
        public int id { get; set; }
        public string magiamgia { get; set; }
        public string mota { get; set; }
        public int soluongdatphongtoithieu { get; set; }
        public float tongtientoithieu { get; set; }
        public float thoigiandatphong { get; set; }
        public float phantramgiamgia { get; set; }
        public int solansudungtoida { get; set; }
        public int solandasudung { get; set; }
        public DateTime ngaybatdau { get; set; }
        public DateTime ngayketthuc { get; set; }
    }
}
