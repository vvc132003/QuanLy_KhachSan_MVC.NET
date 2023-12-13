namespace QuanLyKhachSan_MVC.NET.Models
{
    public class ThoiGian
    {
        public int id { get; set; }
        public DateTime thoigianvao { get; set; }
        public DateTime thoigiannhanphong { get; set; }
        public DateTime thoigianra { get; set; }
        public string mota { get; set; }
        public int idkhachsan { get; set; }

    }
}
