using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface ThueSanPhamRepository
    {
        void ThueSanPham(ThueSanPham thueSanPham);
        void CapNhatThueSanPham(ThueSanPham thueSanPham);
        void XoaThueSanPham(int id);

        List<ThueSanPham> GetAllThueSanPhamID(int iddatphong);
        ThueSanPham GetThueSanPhamBYID(int id);
        ThueSanPham GetThueSanPhamByDatPhongAndSanPham(int iddatphong, int idsanpham);
    }
}
