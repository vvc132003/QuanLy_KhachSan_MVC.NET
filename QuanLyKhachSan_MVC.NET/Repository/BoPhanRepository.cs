using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface BoPhanRepository
    {
        void ThemBoPhan(BoPhan boPhan);
        void CapNhatBoPhan(BoPhan boPhan);
        void XoaBoPhan(int id);
        BoPhan BoPhanGetID(int id);
        List<BoPhan> GetALLBoPhan();
    }
}
