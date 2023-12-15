using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly KhachHangService khachHangService;
        public KhachHangController(KhachHangService khachHangServices)
        {
            khachHangService = khachHangServices;
        }
        public IActionResult DangKy(KhachHang khachHang)
        {
            return View();
        }
        public IActionResult TaoDangKy(KhachHang khachHang)
        {
            KhachHang khachhangtontai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
            if (khachhangtontai != null)
            {
                khachhangtontai.hovaten = khachHang.hovaten;
                khachhangtontai.sodienthoai = khachHang.sodienthoai;
                khachhangtontai.email = khachHang.email;
                khachhangtontai.tinh = khachHang.tinh;
                khachhangtontai.huyen = khachHang.huyen;
                khachhangtontai.phuong = khachHang.phuong;
                khachhangtontai.taikhoan = khachHang.taikhoan;
                khachhangtontai.matkhau = khachHang.matkhau;
                khachHangService.CapNhatKhachHang(khachhangtontai);
            }
            else
            {
                khachHang.trangthai = "còn hoạt động";
                khachHangService.ThemKhachHang(khachHang);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}