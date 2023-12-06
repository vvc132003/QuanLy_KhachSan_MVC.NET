namespace QuanLyKhachSan_MVC.NET.Models
{
    public class ThueSanPham
    {
        public int id { get; set; }
        public int soluong { get; set; }
        public float thanhtien { get; set; }
        public int idsanpham { get; set; }
        public string tensanpham { get; set; }
        public string image { get; set; }
        public int idnhanvien { get; set; }
        public int iddatphong { get; set; }
    }
}