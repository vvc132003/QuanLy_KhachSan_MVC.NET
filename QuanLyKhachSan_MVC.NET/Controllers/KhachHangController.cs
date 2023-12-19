using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly KhachHangService khachHangService;
        private readonly NhanVienService nhanVienService;

        public KhachHangController(KhachHangService khachHangServices, NhanVienService nhanVienServices)
        {
            khachHangService = khachHangServices;
            nhanVienService = nhanVienServices;
        }
        public IActionResult DangKy()
        {
            return View();
        }
        public IActionResult TaoDangKy(KhachHang khachHang)
        {
            KhachHang khachhangtontai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
            KhachHang taikhoantontai = khachHangService.GetKhachHangTaiKhoan(khachHang.taikhoan);
            NhanVien nhanVien = nhanVienService.GetNhanVienDangNhap(khachHang.matkhau, khachHang.taikhoan);
            if (taikhoantontai != null || nhanVien != null)
            {
                return RedirectToAction("DangKy", "KhachHang");
            }
            else
            {
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
                    khachHangService.KhachHangDangKy(khachHang);
                }
                return RedirectToAction("Index", "Home");
            }
        }
    }
}