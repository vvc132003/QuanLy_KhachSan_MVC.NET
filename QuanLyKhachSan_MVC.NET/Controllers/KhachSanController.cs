using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class KhachSanController : Controller
    {
        private readonly KhachSanService khachSanService;
        public KhachSanController(KhachSanService khachSanServices)
        {
            khachSanService = khachSanServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<KhachSan> khachsanlist = khachSanService.GetAllKhachSan();
                Modeldata modeldata = new Modeldata()
                {
                    listKhachSan = khachsanlist,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
        public IActionResult ThemKhachSan(KhachSan khachSan)
        {
            khachSan.sosao = 0;
            khachSanService.ThemKhachSan(khachSan);
            TempData["themthanhcong"] = "";
            return RedirectToAction("Index", "KhachSan");
        }
    }
}