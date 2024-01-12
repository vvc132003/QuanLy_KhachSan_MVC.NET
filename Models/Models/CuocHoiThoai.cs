using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class CuocHoiThoai
    {
        public int Id { get; set; }
        public string Tieude { get; set; }
        public int NhanVienTaoid { get; set; }
        public string LoaiHoiThoai { get; set; }
        public DateTime DuocTaoVao { get; set; }
        public DateTime DuocCaonhatVao { get; set; }
        public DateTime DaXoaVao { get; set; }
    }
}
