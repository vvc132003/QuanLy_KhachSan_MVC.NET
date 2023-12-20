using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface QuyDinhGiamGiaRepository
    {
        void ThemQuyDinhGiamGia(QuyDinhGiamGia quyDinhGiamGia);
        void CapNhatQuyDinhGiamGia(QuyDinhGiamGia quyDinhGiamGia);
        void XoaQuyDinhGiamGia(int id);
        List<QuyDinhGiamGia> GetQuyDinhGia(int idkhachsan);
        List<QuyDinhGiamGia> GetAllQuyDinhGia();
    }
}
