using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface KhachHangRepository
    {
        void ThemKhachHang(KhachHang khachHang);
        KhachHang GetKhachHangCCCD(string cccd);
    }
}
