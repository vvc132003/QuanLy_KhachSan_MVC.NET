namespace QuanLyKhachSan_MVC.NET.Models
{
    public class ThoiGian
    {
        public int id { get; set; }
        public TimeSpan thoigiannhanphong { get; set; }
        public float phuthunhanphong { get; set; }
        public TimeSpan thoigianra { get; set; }
        public float phuthutraphong { get; set; }
        public string mota { get; set; }
        public int idkhachsan { get; set; }
    }
}
