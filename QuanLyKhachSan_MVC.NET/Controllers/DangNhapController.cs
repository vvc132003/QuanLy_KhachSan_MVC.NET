using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly NhanVienService nhanVienService;

        public DangNhapController(NhanVienService nhanVienServices)
        {
            nhanVienService = nhanVienServices;
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
            var luudangnhap = nhanVienService.CheckThongTinDangNhap(matkhau, taikhoan);
            if (luudangnhap != null)
            {
                HttpContext.Session.SetInt32("id", luudangnhap.id);
                HttpContext.Session.SetString("taikhoan", luudangnhap.taikhoan);
                HttpContext.Session.SetString("hovaten", luudangnhap.hovaten);
                HttpContext.Session.SetString("tenchucvu", luudangnhap.tenchucvu);
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                Console.WriteLine("Ten dang nhap hoac mat khau bi sai!!!");
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap", "DangNhap");
        }
    }
}