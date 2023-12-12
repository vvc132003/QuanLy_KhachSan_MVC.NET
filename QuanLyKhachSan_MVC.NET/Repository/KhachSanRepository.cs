using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface KhachSanRepository
    {
        void ThemKhachSan(KhachSan khachSan);
        List<KhachSan> GetAllKhachSan();
        KhachSan GetKhachSanById(int id);
    }
}
