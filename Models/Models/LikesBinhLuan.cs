using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class LikesBinhLuan
    {
        public int id { get; set; }
        public int idbinhluan { get; set; }
        public int idkhachhang { get; set; }
        public int thich { get; set; }
        public int khongthich { get; set; }
        public DateTime thoigianlike { get; set; }
    }
}
