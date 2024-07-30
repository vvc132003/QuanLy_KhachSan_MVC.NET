using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class TinNhan
    {
        public int Id { get; set; }
        public int CuocHoiThoaiId { get; set; }
        public int NhanVienGuiId { get; set; }
        public string LoaiTinNhan { get; set; }
        public string NoiDung { get; set; }
        public DateTime DuocTaoVao { get; set; }
        public DateTime DaXoaVao { get; set; }
        public string daXem { get; set; }

    }
}