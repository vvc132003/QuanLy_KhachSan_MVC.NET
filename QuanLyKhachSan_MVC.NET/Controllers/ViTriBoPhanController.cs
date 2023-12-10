using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ViTriBoPhanController : Controller
    {
        private readonly ViTriBoPhanService viTriBoPhanService;
        private readonly BoPhanService boPhanService;

        public ViTriBoPhanController(ViTriBoPhanService viTriBoPhanServices, BoPhanService boPhanServices)
        {
            viTriBoPhanService = viTriBoPhanServices;
            boPhanService = boPhanServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<ViTriBoPhan> viTriBoPhans = viTriBoPhanService.GetAllViTriBoPhan();
                List<BoPhan> boPhans = boPhanService.GetALLBoPhan();

                if (viTriBoPhans != null && boPhans != null)
                {
                    Modeldata modeldata = new Modeldata
                    {
                        listviTriBoPhan = viTriBoPhans,
                        listbophan = boPhans,
                    };
                    return View(modeldata);
                }
                return View("Index");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }

        public IActionResult ThemViTriBoPhan(ViTriBoPhan viTriBoPhan)
        {
            viTriBoPhanService.ThemViTriBoPhan(viTriBoPhan);
            TempData["themthanhcong"] = "";
            return RedirectToAction("Index", "ViTriBoPhan");
        }
    }
}
