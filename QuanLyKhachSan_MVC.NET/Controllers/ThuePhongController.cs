using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ThuePhongController : Controller
    {
        private readonly DatPhongService datPhongService;
        private readonly KhachHangService khachHangService;
        private readonly PhongService phongService;
        private readonly NhanPhongService nhanPhongService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;


        public ThuePhongController(DatPhongService datPhongServices, KhachHangService khachHangServices, PhongService phongServices, NhanPhongService nhanPhongServices, SanPhamService sanPhamServices, ThueSanPhamService thueSanPhamServices)
        {
            datPhongService = datPhongServices;
            khachHangService = khachHangServices;
            phongService = phongServices;
            nhanPhongService = nhanPhongServices;
            sanPhamService = sanPhamServices;
            thueSanPhamService = thueSanPhamServices;
        }
        public IActionResult Index(int id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                Phong phong = phongService.GetPhongID(id);
                Modeldata yourModel = new Modeldata
                {
                    phong = phong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThemThuePhong(KhachHang khachHang, DatPhong datPhong, NhanPhong nhanPhong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                KhachHang khachHangTonTai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                if (khachHangTonTai != null)
                {
                    datPhong.idkhachhang = khachHangTonTai.id;
                    datPhong.idloaidatphong = 2;
                    datPhong.trangthai = "đã đặt";
                    datPhong.ngaydat = DateTime.Now;
                    datPhong.idphong = datPhong.idphong;
                    int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                    nhanPhong.iddatphong = idDatPhongThemVao;
                    nhanPhong.ngaynhanphong = DateTime.Now;
                    nhanPhongService.ThemNhanPhong(nhanPhong);
                    Phong phong = phongService.GetPhongID(datPhong.idphong);
                    phong.tinhtrangphong = "có khách";
                    phongService.CapNhatPhong(phong);
                    return RedirectToAction("Index", "Phong");
                }
                else
                {
                    khachHang.trangthai = "còn hoạt động";
                    khachHangService.ThemKhachHang(khachHang);
                    KhachHang khachhangmoi = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                    datPhong.idkhachhang = khachhangmoi.id;
                    datPhong.idloaidatphong = 2;
                    datPhong.trangthai = "đã đặt";
                    datPhong.ngaydat = DateTime.Now;
                    datPhong.idphong = datPhong.idphong;
                    int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                    nhanPhong.iddatphong = idDatPhongThemVao;
                    nhanPhong.ngaynhanphong = DateTime.Now;
                    nhanPhongService.ThemNhanPhong(nhanPhong);
                    Phong phong = phongService.GetPhongID(datPhong.idphong);
                    phong.tinhtrangphong = "có khách";
                    phongService.CapNhatPhong(phong);
                    return RedirectToAction("Index", "Phong");
                }
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult Index1()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                List<Phong> phong = phongService.GetAllPhongTrangThai();
                Modeldata yourModel = new Modeldata
                {
                    listphong = phong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThueNhieuPhong(List<int> idphongs, KhachHang khachHang, DatPhong datPhong, NhanPhong nhanPhong)
        {
            if (idphongs != null && idphongs.Any())
            {
                foreach (int phongId in idphongs)
                {
                    KhachHang khachHangTonTai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                    if (khachHangTonTai != null)
                    {
                        datPhong.idkhachhang = khachHangTonTai.id;
                        datPhong.idloaidatphong = 2;
                        datPhong.trangthai = "đã đặt";
                        datPhong.ngaydat = DateTime.Now;
                        datPhong.idphong = phongId;
                        int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                        nhanPhong.iddatphong = idDatPhongThemVao;
                        nhanPhong.ngaynhanphong = DateTime.Now;
                        nhanPhongService.ThemNhanPhong(nhanPhong); Phong phong = phongService.GetPhongID(phongId);
                        phong.tinhtrangphong = "có khách";
                        phongService.CapNhatPhong(phong);
                    }
                    else
                    {
                        khachHang.trangthai = "còn hoạt động";
                        khachHangService.ThemKhachHang(khachHang);
                        KhachHang khachhangmoi = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                        datPhong.idkhachhang = khachhangmoi.id;
                        datPhong.idloaidatphong = 2;
                        datPhong.trangthai = "đã đặt";
                        datPhong.ngaydat = DateTime.Now;
                        datPhong.idphong = phongId;
                        int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                        nhanPhong.iddatphong = idDatPhongThemVao;
                        nhanPhong.ngaynhanphong = DateTime.Now;
                        nhanPhongService.ThemNhanPhong(nhanPhong);
                        Phong phong = phongService.GetPhongID(phongId);
                        phong.tinhtrangphong = "có khách";
                        phongService.CapNhatPhong(phong);
                    }
                }
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("Index", "Index1");
            }
        }
        public IActionResult ChiTietThuePhong(int id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                List<DatPhong> listdatPhongs = datPhongService.GetAllDatPhongByID(id);
                List<SanPham> listsanpham = sanPhamService.GetAllSanPham();
                List<Modeldata> listmodeldatas = new List<Modeldata>();
                if (listdatPhongs != null && listdatPhongs.Any())
                {
                    foreach (var datphong in listdatPhongs)
                    {
                        List<ThueSanPham> listthueSanPham = thueSanPhamService.GetAllThueSanPhamID(datphong.id);
                        Modeldata yourModel = new Modeldata
                        {
                            listsanPham = listsanpham,
                            datPhong = datphong,
                            listthueSanPham = listthueSanPham,
                        };
                        listmodeldatas.Add(yourModel);
                    }
                }
                else
                {
                }
                return View(listmodeldatas);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}
