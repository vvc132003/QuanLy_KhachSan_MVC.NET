using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface ThietBiRepository
    {
        void ThemThietBi(ThietBi thietBi);
        void CapNhatThietBi(ThietBi thietBi);
        void XoaThietBi(int id);
        ThietBi GetThietBiByID(int id);
        List<ThietBi> GetAllThietBi();
    }
}
