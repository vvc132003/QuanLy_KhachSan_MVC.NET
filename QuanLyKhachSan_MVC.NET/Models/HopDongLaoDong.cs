namespace QuanLyKhachSan_MVC.NET.Models
{
    public class HopDongLaoDong
    {
        public int id { get; set; }
        public string loaihopdong { get; set; }
        public DateTime ngaybatdau { get; set; }
        public DateTime ngayketthuc { get; set; }
        public int idnhanvien { get; set; }

    }
}
