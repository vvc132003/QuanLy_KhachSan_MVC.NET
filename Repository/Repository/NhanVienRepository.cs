using Model.Models;

namespace QuanLyKhachSan_MVC.NET.Repository
{
    public interface NhanVienRepository
    {
        int ThemNhanVien(NhanVien nhanVien);
        void CapNhatNhanVien(NhanVien nhanVien);
        void XoaNhanVien(int id);
        void Xuatexcel();
        void GuiEmail(string filePath);
        List<NhanVien> GetAllNhanVien();
        NhanVien GetNhanVienID(int id);
        NhanVien GetNhanVienDangNhap(string matkhau, string taikhoan);
        NhanVien CheckThongTinDangNhap(string matkhau, string taikhoan, int idkhachsan);
        List<NhanVien> GetallNhanVientheoidbophan(int idbophan);
    }
}
