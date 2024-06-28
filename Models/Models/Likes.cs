using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Likes
    {
        public int id { get; set; }
        public int idphong { get; set; }
        public int idkhachhang { get; set; }
        public DateTime thoigianlike { get; set; }
        public string icons { get; set; }
    }
}
