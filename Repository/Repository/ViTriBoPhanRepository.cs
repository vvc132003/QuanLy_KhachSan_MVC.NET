using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface ViTriBoPhanRepository
    {
        void ThemViTriBoPhan(ViTriBoPhan viTriBoPhan);
        void CapNhatViTriBoPhan(ViTriBoPhan viTriBoPhan);
        void XoaViTriBoPhan(int id);
        ViTriBoPhan GetViTriBoPhanID(int id);
        List<ViTriBoPhan> GetAllViTriBoPhan();
    }
}
