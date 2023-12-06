namespace QuanLyKhachSan_MVC.NET.Models
{
    public class ChuyenPhong
    {
        public int id { get; set; }
        public DateTime ngaychuyen { get; set; }
        public string lydo { get; set; }
        public int idkhachhang { get; set; }
        public int idnhanvien { get; set; }
        public int idphongcu { get; set; }
        public int idphongmoi { get; set; }
    }
}
