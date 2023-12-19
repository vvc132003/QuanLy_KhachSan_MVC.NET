using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface TangRepository
    {
        List<Tang> GetAllTang(int idkhachsan);
        Tang GetTangidkhachsan(int idkhachsan);
        void ThemTang(Tang tang);
        void CapNhatTang(Tang tang);
        void XoaTang(int id);
        Tang GetTangID(int id);
    }
}
