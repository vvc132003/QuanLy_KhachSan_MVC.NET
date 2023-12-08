using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface ThietBiPhongRepository
    {
        void ThemThietBiPhong(ThietBiPhong thietBiPhong);
        void CapNhatThietBiPhong(ThietBiPhong thietBiPhong);
        void XoaThietBiPhong(int id);
        List<ThietBiPhong> GetALLThietBiPhong();
        List<ThietBiPhong> GetALLThietBiPhongbyIDPhong(int idphong);
        ThietBiPhong GetALLThietBiPhongBYID(int id);

    }
}
