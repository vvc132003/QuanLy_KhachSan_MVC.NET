using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class NguoiThamGia
    {
        public int Id { get; set; }
        public int CuocHoiThoaiId { get; set; }
        public int NhanVienThamGiaId { get; set; }
        public DateTime DuocTaoVao { get; set; }
        public DateTime DuocCapNhatVao { get; set; }
    }
}
