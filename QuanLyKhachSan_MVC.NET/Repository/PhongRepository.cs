using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface PhongRepository
    {
        List<Phong> GetAllPhong();
        List<Phong>  GetAllPhongTrangThai(int idkhachsan);
        List<Phong> GetAllPhongIDTang(int idtang, int idkhachsan);
        Phong GetPhongID(int id);
        void CapNhatPhong(Phong phong);
        void ThemPhong(Phong phong);
        List<Phong> GetAllPhongTrangThaiandidksloaiphongsonguoi(int idkhachsan, string loaiphong, int songuoi);

    }
}
