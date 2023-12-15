using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class BoPhanController : Controller
    {
        private readonly BoPhanService boPhanService;
        private readonly NhanVienService nhanVienService;


        public BoPhanController(BoPhanService boPhanServiceS, NhanVienService nhanVienServices)
        {
            boPhanService = boPhanServiceS;
            nhanVienService = nhanVienServices;
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
                List<BoPhan> boPhans = boPhanService.GetALLBoPhan();
                List<Modeldata> modeldataList = new List<Modeldata>();
                foreach (var boPhan in boPhans)
                {
                    List<NhanVien> nhanViens = nhanVienService.GetallNhanVientheoidbophan(boPhan.id);
                    Modeldata modeldata = new Modeldata
                    {
                        boPhan = boPhan,
                        listnhanVien = nhanViens,
                    };
                    modeldataList.Add(modeldata);
                }
                return View(modeldataList);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
        public IActionResult ThemBoPhan(BoPhan boPhan)
        {
            boPhanService.ThemBoPhan(boPhan);
            TempData["themthanhcong"] = "";
            return RedirectToAction("Index", "BoPhan");
        }
    }
}