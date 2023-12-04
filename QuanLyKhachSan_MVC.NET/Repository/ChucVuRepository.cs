using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface ChucVuRepository
    {
        void ThemChucVu(ChucVu chucVu);
        void CapNhatChucVu(ChucVu chucVu);
        void XoaChucVu(int id);
        List<ChucVu> GetAllChucVu();
        ChucVu GetChucVuID(int id);
    }
}