namespace QuanLyKhachSan_MVC.NET.Models
{
    public class ThietBiPhong
    {
        public int id { get; set; }
        public DateTime ngayduavao { get; set; }
        public int soluongduavao { get; set; }
        public int idphong { get; set; }
        public int idthietbi { get; set; }
        public string tenthietbi { get; set; }
        public float giathietbi { get; set; }

    }
}
