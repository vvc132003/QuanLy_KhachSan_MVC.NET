using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface ThoiGianRepository
    {
        void ThemThoiGian(ThoiGian thoiGian);
        void CapNhatThoiGian(ThoiGian thoiGian);
        List<ThoiGian> GetAllThoiGian();
        ThoiGian GetThoiGian(DateTime thoigianvao);
        ThoiGian GetThoiGianById(int id);
    }
}
