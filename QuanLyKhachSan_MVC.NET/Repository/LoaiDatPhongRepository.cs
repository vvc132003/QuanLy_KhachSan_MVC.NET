using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface LoaiDatPhongRepository
    {
        void ThemLoaiDatPhong(LoaiDatPhong loaiDatPhong);
        List<LoaiDatPhong> GetAllLoaiDatPhong();
    }
}
