using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface TinNhanCaNhanRepository
    {
        List<TinNhanCaNhan> Gettinnhancanhantheoidnhanvien(int idnhanviengui, int idnhanviennhan);
        void ThemTinNhanCaNhan(TinNhanCaNhan tinNhanCaNhan);
    }
}
