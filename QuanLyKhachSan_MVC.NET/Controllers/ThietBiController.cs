using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ThietBiController : Controller
    {
        private readonly ThietBiService thietBiService;
        public ThietBiController(ThietBiService thietBiServices)
        {
            thietBiService = thietBiServices;
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
                List<ThietBi> listthietbi = thietBiService.GetAllThietBi();
                Modeldata modeldata = new Modeldata
                {
                    listThietBi = listthietbi,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
        public IActionResult ThemThietBi(ThietBi thietbi)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                ViewData["id"] = id;
                thietbi.ngaymua = DateTime.Now;
                thietBiService.ThemThietBi(thietbi);
                return RedirectToAction("Index", "ThietBi");
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
    }
}