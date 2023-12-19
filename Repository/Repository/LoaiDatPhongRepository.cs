using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface LoaiDatPhongRepository
    {
        void ThemLoaiDatPhong(LoaiDatPhong loaiDatPhong);
        List<LoaiDatPhong> GetAllLoaiDatPhong();
    }
}
