using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class QuyDinhGiamGiaController : Controller
    {
        private readonly QuyDinhGiamGiaService quyDinhGiamGiaService;
        public QuyDinhGiamGiaController(QuyDinhGiamGiaService quyDinhGiamGiaServices)
        {
            quyDinhGiamGiaService = quyDinhGiamGiaServices;
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
                List<QuyDinhGiamGia> quyDinhGiamGias = quyDinhGiamGiaService.GetAllQuyDinhGia();
                Modeldata modeldata = new Modeldata
                {
                    listquyDinhGiamGia = quyDinhGiamGias,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThemQuyDinhGiamGia(QuyDinhGiamGia quyDinhGiamGia)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                ViewData["id"] = id;
                quyDinhGiamGia.ngaythemquydinh = DateTime.Now;
                quyDinhGiamGiaService.ThemQuyDinhGiamGia(quyDinhGiamGia);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}
