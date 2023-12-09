using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface QuyDinhGiamGiaRepository
    {
        void ThemQuyDinhGiamGia(QuyDinhGiamGia quyDinhGiamGia);
        void CapNhatQuyDinhGiamGia(QuyDinhGiamGia quyDinhGiamGia);
        void XoaQuyDinhGiamGia(int id);
        QuyDinhGiamGia GetQuyDinhGia(float solandatphong);
        List<QuyDinhGiamGia> GetAllQuyDinhGia();
    }
}
