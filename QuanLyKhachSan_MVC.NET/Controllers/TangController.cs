using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;
using System.Collections.Generic;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class TangController : Controller
    {
        private readonly TangService tangService;
        public TangController(TangService tangServices)
        {
            tangService = tangServices;
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
                List<Tang> tangs = tangService.GetAllTang();
                List<Modeldata> modellisst = new List<Modeldata>();
                foreach (var tang in tangs)
                {
                    Modeldata modeldata = new Modeldata
                    {
                        tang = tang,
                    };
                    modellisst.Add(modeldata);
                }
                return View(modellisst);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }

        public IActionResult ThemTang(Tang tang)
        {
            tangService.ThemTang(tang);
            TempData["themthanhcong"] = "";
            return RedirectToAction("Index", "Tang");
        }
        public IActionResult XoaTang(int id)
        {
            tangService.XoaTang(id);
            TempData["xoathanhcong"] = "";
            return RedirectToAction("Index", "Tang");
        }

        public IActionResult GetTangID(int id)
        {
            Tang tangs = tangService.GetTangID(id);
            return View(tangs);
        }
        public IActionResult CapNhatTang(Tang tang)
        {
            tangService.CapNhatTang(tang);
            return RedirectToAction("Index", "Tang");
        }
    }
}
