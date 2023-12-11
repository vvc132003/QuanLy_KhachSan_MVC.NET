using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface HopDongLaoDongRepository
    {
        void ThemHopDongLaoDong(HopDongLaoDong hopDongLaoDong);
        void CapNhatHopDongLaoDong(HopDongLaoDong hopDongLaoDong);
        HopDongLaoDong GetHopDongLaoDongByID(int idnhanvien);
    }
}