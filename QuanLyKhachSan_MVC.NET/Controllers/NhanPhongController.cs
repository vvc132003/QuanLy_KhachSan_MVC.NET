using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class NhanPhongController : Controller
    {
        private readonly DatPhongService datPhongService;
        private readonly KhachHangService khachHangService;
        private readonly PhongService phongService;
        private readonly NhanPhongService nhanPhongService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly ThoiGianService thoiGianService;


        public NhanPhongController(DatPhongService datPhongServices,
            KhachHangService khachHangServices,
            PhongService phongServices,
            NhanPhongService nhanPhongServices,
            SanPhamService sanPhamServices,
            ThueSanPhamService thueSanPhamServices,
            ThoiGianService thoiGianServices)
        {
            datPhongService = datPhongServices;
            khachHangService = khachHangServices;
            phongService = phongServices;
            nhanPhongService = nhanPhongServices;
            sanPhamService = sanPhamServices;
            thueSanPhamService = thueSanPhamServices;
            thoiGianService = thoiGianServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NhanPhongOnline(int idphong)
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idnv = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = idnv;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThaiOnline(idphong);
                    KhachHang khachHang = khachHangService.GetKhachHangbyid(datPhong.idkhachhang);
                    List<SanPham> sanPhams = sanPhamService.GetAllSanPhamIDKhachSan(idkhachsan);
                    Modeldata modeldata = new Modeldata
                    {
                        datPhong = datPhong,
                        khachhang = khachHang,
                        listsanPham = sanPhams,
                    };
                    return View(modeldata);
                }
                else
                {
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult CheckInNhanPhongOnline(NhanPhong nhanPhong, int idphong, string cccd, List<int> idsanpham)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idnv = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = idnv;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThaiOnline(idphong);
                    if (datPhong != null && datPhong.cccd == cccd)
                    {
                        nhanPhong.idnhanvien = idnv;
                        nhanPhong.iddatphong = datPhong.id;
                        nhanPhong.ngaynhanphong = DateTime.Now;
                        nhanPhongService.ThemNhanPhong(nhanPhong);
                        Phong phong = phongService.GetPhongID(idphong);
                        phong.tinhtrangphong = "có khách";
                        phongService.CapNhatPhong(phong);
                        datPhong.trangthai = "đã đặt";
                        datPhongService.UpdateDatPhong(datPhong);
                        if (idsanpham != null && idsanpham.Any())
                        {
                            ThueSanPham thueSanPham = new ThueSanPham();
                            foreach (int idsp in idsanpham)
                            {
                                SanPham sanpham = sanPhamService.GetSanPhamByID(idsp);
                                // thuê sản phẩm
                                thueSanPham.idnhanvien = idnv;
                                thueSanPham.soluong = 1;
                                thueSanPham.idsanpham = idsp;
                                thueSanPham.iddatphong = datPhong.id;
                                thueSanPham.thanhtien = 1 * sanpham.giaban;
                                thueSanPhamService.ThueSanPham(thueSanPham);
                                // cập nhật số lượng tồn
                                sanpham.soluongcon -= 1;
                                sanPhamService.CapNhatSanPham(sanpham);
                            }
                        }
                        TempData["nhanphongthanhcong"] = "";
                        return RedirectToAction("Index", "Phong");
                    }
                    else
                    {
                        return RedirectToAction("NhanPhongOnline", "NhanPhong", new { id = idphong });
                    }
                }
                else
                {
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}
