using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class PhongKhachSanController : Controller
    {
        private readonly PhongKhachSanService phongKhachSanService;
        private readonly KhachSanService khachSanService;
        public PhongKhachSanController(PhongKhachSanService phongKhachSanServices, KhachSanService khachSanServices)
        {
            phongKhachSanService = phongKhachSanServices;
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
                List<PhongKhachSan> phongkhachsanlist = phongKhachSanService.GetAllPhongKhachSan();
                Modeldata modeldata = new Modeldata()
                {
                    listKhachSan = khachsanlist,
                    listPhongKhachSan = phongkhachsanlist,
                };
                return View(modeldata);
            }
            else
            {
                               return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult ThemPhongKhachSan(PhongKhachSan phongKhachSan)
        {
            phongKhachSan.tinhtrangphong = "còn trống";
            phongKhachSanService.ThemPhongKhachSan(phongKhachSan);
            TempData["themthanhcong"] = "";
            return RedirectToAction("Index", "PhongKhachSan");
        }
    }
}
