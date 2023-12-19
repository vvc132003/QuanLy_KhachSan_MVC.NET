using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface GiamGiaRepository
    {
        void ThemGiamGia(GiamGia giamGia);
        GiamGia GetGiamGiaBYIDKhachHang(int iddatphong);
    }
}
