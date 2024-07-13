using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface KhachSanRepository
    {
        int ThemKhachSan(KhachSan khachSan);
        List<KhachSan> GetAllKhachSan();
        KhachSan GetKhachSanById(int id);
    }
}
