using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface DatPhongRepository
    {
        int ThemDatPhong(DatPhong datPhong);
        void UpdateDatPhong(DatPhong datPhong);
        int GetDatPhongCountByKhachHangId(int idkhachhang);
        List<DatPhong> GetAllDatPhongByID(int id);
        List<DatPhong> GetAllDatPhong();
        DatPhong GetDatPhongByIDTrangThai(int idphong);
        DatPhong GetDatPhongByIDTrangThaiOnline(int idphong);

    }
}
