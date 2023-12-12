using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface PhongKhachSanRepository
    {
        List<PhongKhachSan> GetAllPhongIDKhachSan(int idkhachsan);
        void ThemPhongKhachSan(PhongKhachSan phongKhachSan);
        List<PhongKhachSan> GetAllPhongKhachSan();

    }
}
