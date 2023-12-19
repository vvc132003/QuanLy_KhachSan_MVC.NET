using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface ThoiGianRepository
    {
        void ThemThoiGian(ThoiGian thoiGian);
        void CapNhatThoiGian(ThoiGian thoiGian);
        List<ThoiGian> GetAllThoiGian();
        ThoiGian GetThoiGian(int idkhachsan);
        ThoiGian GetThoiGianById(int id);
    }
}
