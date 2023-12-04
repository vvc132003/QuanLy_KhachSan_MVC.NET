using QuanLyKhachSan_MVC.NET.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface NhanVienRepository
    {
        void ThemNhanVien(NhanVien nhanVien);
        void CapNhatNhanVien(NhanVien nhanVien);
        void XoaNhanVien(int id);
        void Xuatexcel();
        void GuiEmail(string filePath);
        List<NhanVien> GetAllNhanVien();
        NhanVien GetNhanVienID(int id);
        NhanVien CheckThongTinDangNhap(string matkhau, string taikhoan);
        List<NhanVien> GetallNhanVientheoidbophan(int idbophan);
    }
}
