using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface SanPhamRepository
    {
        void ThemSanPham(SanPham sanPham);
        void CapNhatSanPham(SanPham sanPham);
        void XoaSanPham(int id);
        SanPham GetSanPhamByID(int id);
        List<SanPham> GetAllSanPham();
    }
}
