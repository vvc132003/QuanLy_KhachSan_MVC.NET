using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ThoiGianController : Controller
    {
        private readonly ThoiGianService thoiGianService;
        public ThoiGianController(ThoiGianService thoiGianServices)
        {
            thoiGianService = thoiGianServices;
        }
        public IActionResult ThemThoiGian(ThoiGian thoiGian)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                ViewData["id"] = id;
                ViewData["idkhachsan"] = idkhachsan;
                thoiGian.idkhachsan = idkhachsan;
                thoiGianService.ThemThoiGian(thoiGian);
                TempData["thoigianthanhcong"] = "";
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                               return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}