using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly NhanVienService nhanVienService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;

        public DangNhapController(NhanVienService nhanVienServices, KhachSanService khachSanServices, KhachHangService khachHangServices)
        {
            nhanVienService = nhanVienServices;
            khachSanService = khachSanServices;
            khachHangService = khachHangServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DangNhap()
        {
            NhanVien nhanVien = new NhanVien();
            Modeldata yourModel = new Modeldata
            {
                nhanVien = nhanVien,
            };
            return View(yourModel);
        }
        public IActionResult DangNhapVaoHeThong(string taikhoan, string matkhau)
        {
            NhanVien luudangnhap = nhanVienService.CheckThongTinDangNhap(matkhau, taikhoan);
            KhachHang khachHang = khachHangService.GetKhachHangDangNhap(taikhoan, matkhau);
            if (luudangnhap != null)
            {
                if (luudangnhap.tenchucvu == "Quản lý")
                {
                    HttpContext.Session.SetInt32("id", luudangnhap.id);
                    HttpContext.Session.SetString("taikhoan", luudangnhap.taikhoan);
                    HttpContext.Session.SetString("hovaten", luudangnhap.hovaten);
                    HttpContext.Session.SetString("tenchucvu", luudangnhap.tenchucvu);
                    HttpContext.Session.SetInt32("idkhachsan", luudangnhap.idkhachsan);
                    return RedirectToAction("home", "phong");
                }
                else
                {
                    HttpContext.Session.SetInt32("id", luudangnhap.id);
                    HttpContext.Session.SetString("taikhoan", luudangnhap.taikhoan);
                    HttpContext.Session.SetString("hovaten", luudangnhap.hovaten);
                    HttpContext.Session.SetString("tenchucvu", luudangnhap.tenchucvu);
                    HttpContext.Session.SetInt32("idkhachsan", luudangnhap.idkhachsan);
                    return RedirectToAction("index", "phong");
                }
            }
            else if (khachHang != null)
            {
                HttpContext.Session.SetInt32("id", khachHang.id);
                HttpContext.Session.SetString("hovaten", khachHang.hovaten);
                return RedirectToAction("index", "home");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        /* public IActionResult DangNhaps(string taikhoan, string matkhau)
         {
             NhanVien luudangnhap = nhanVienService.GetNhanVienDangNhap(matkhau, taikhoan);
             KhachHang khachHang = khachHangService.GetKhachHangDangNhap(taikhoan, matkhau);
             if (luudangnhap != null && (luudangnhap.tenchucvu == "Quản lý" || luudangnhap.tenchucvu == "Nhân viên"))
             {
                 return RedirectToAction("dangnhap", "dangnhap");
             }
             else if (khachHang != null)
             {
                 return RedirectToAction("Index", "Home");
             }
             else
             {
                                return RedirectToAction("dangnhap", "dangnhap");
             }
         }*/
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap", "DangNhap");
        }
    }
}