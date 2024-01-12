using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Collections.Generic;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class TangController : Controller
    {
        private readonly TangService tangService;
        private readonly KhachSanService khachSanService;
        public TangController(TangService tangServices, KhachSanService khachSanServices)
        {
            tangService = tangServices;
            khachSanService = khachSanServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                ViewData["idkhachsan"] = idkhachsan;
                List<Tang> tangs = tangService.GetAllTang(idkhachsan);
                List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
                List<Modeldata> modelDataList = new List<Modeldata>();
                foreach (var tang in tangs)
                {
                    Modeldata modeldata = new Modeldata
                    {
                        tang = tang,
                        listKhachSan = listkhachsan
                    };
                    modelDataList.Add(modeldata);
                }
                return View(modelDataList);
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
