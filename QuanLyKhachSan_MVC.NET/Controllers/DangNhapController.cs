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
            if (luudangnhap != null && (luudangnhap.tenchucvu == "Quản lý" || luudangnhap.tenchucvu == "Nhân viên"))
            {
                HttpContext.Session.SetInt32("id", luudangnhap.id);
                HttpContext.Session.SetString("taikhoan", luudangnhap.taikhoan);
                HttpContext.Session.SetString("hovaten", luudangnhap.hovaten);
                HttpContext.Session.SetString("tenchucvu", luudangnhap.tenchucvu);
                HttpContext.Session.SetInt32("idkhachsan", luudangnhap.idkhachsan);
                return RedirectToAction("Index", "Phong");
            }
            else if (khachHang != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        /* public IActionResult DangNhaps(string taikhoan, string matkhau)
         {
             NhanVien luudangnhap = nhanVienService.GetNhanVienDangNhap(matkhau, taikhoan);
             KhachHang khachHang = khachHangService.GetKhachHangDangNhap(taikhoan, matkhau);
             if (luudangnhap != null && (luudangnhap.tenchucvu == "Quản lý" || luudangnhap.tenchucvu == "Nhân viên"))
             {
                 return RedirectToAction("DangNhap", "DangNhap");
             }
             else if (khachHang != null)
             {
                 return RedirectToAction("Index", "Home");
             }
             else
             {
                                return RedirectToAction("DangNhap", "DangNhap");
             }
         }*/
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap", "DangNhap");
        }
    }
}