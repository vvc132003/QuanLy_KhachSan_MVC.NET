using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class NhanTinController : Controller
    {
        private readonly TinNhanCaNhanService tinNhanCaNhanService;
        private readonly NhanVienService nhanVienService;
        public NhanTinController(TinNhanCaNhanService tinNhanCaNhanServices, NhanVienService nhanVienServices)
        {
            tinNhanCaNhanService = tinNhanCaNhanServices;
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
                List<NhanVien> listnhanvien = nhanVienService.GetAllNhanVien();
                Modeldata modeldata = new Modeldata
                {
                    listnhanVien = listnhanvien,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }

        public IActionResult NhanTin(int idnhanviennhan)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<NhanVien> listnhanvien = nhanVienService.GetAllNhanVien();
                List<TinNhanCaNhan> listtinnhancanhan = tinNhanCaNhanService.Gettinnhancanhantheoidnhanvien(id, idnhanviennhan);
                Modeldata modeldata = new Modeldata
                {
                    listTinNhanCaNhan = listtinnhancanhan,
                    listnhanVien = listnhanvien,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}