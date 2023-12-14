using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly NhanVienService nhanVienService;
        private readonly KhachSanService khachSanService;


        public DangNhapController(NhanVienService nhanVienServices, KhachSanService khachSanServices)
        {
            nhanVienService = nhanVienServices;
            khachSanService = khachSanServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DangNhap()
        {
            NhanVien nhanVien = new NhanVien();
            List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
            Modeldata yourModel = new Modeldata
            {
                nhanVien = nhanVien,
                listKhachSan = listkhachsan,
            };
            return View(yourModel);
        }
        public IActionResult DangNhapVaoHeThong(string taikhoan, string matkhau, int idkhachsan)
        {
            var luudangnhap = nhanVienService.CheckThongTinDangNhap(matkhau, taikhoan, idkhachsan);
            if (luudangnhap != null)
            {
                HttpContext.Session.SetInt32("id", luudangnhap.id);
                HttpContext.Session.SetString("taikhoan", luudangnhap.taikhoan);
                HttpContext.Session.SetString("hovaten", luudangnhap.hovaten);
                HttpContext.Session.SetString("tenchucvu", luudangnhap.tenchucvu);
                HttpContext.Session.SetInt32("idkhachsan", luudangnhap.idkhachsan);
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                Console.WriteLine("Ten dang nhap hoac mat khau bi sai!!!");
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult DangNhaps(string taikhoan, string matkhau)
        {
            var luudangnhap = nhanVienService.index(matkhau, taikhoan);
            if (luudangnhap != null)
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "DangNhap");
        }
    }
}