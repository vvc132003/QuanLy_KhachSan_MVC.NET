using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class QRCodeRequest
    {
        public int id { get; set; }
        public string taikhoan { get; set; }
        public string tentaikhoan { get; set; }
        public string machinhanh { get; set; }
        public DateTime ngaytao { get; set; }
    }
}
