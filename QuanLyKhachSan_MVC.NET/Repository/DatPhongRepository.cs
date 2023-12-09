using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface DatPhongRepository
    {
        int ThemDatPhong(DatPhong datPhong);
        void UpdateDatPhong(DatPhong datPhong);
        int GetDatPhongCountByKhachHangId(int idkhachhang);
        List<DatPhong> GetAllDatPhongByID(int id);
        DatPhong GetDatPhongByIDTrangThai(int idphong);
    }
}
