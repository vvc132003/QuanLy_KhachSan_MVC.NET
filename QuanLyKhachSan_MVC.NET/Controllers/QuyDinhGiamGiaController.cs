using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class QuyDinhGiamGiaController : Controller
    {
        private readonly QuyDinhGiamGiaService quyDinhGiamGiaService;
        private readonly KhachSanService khachSanService;
        public QuyDinhGiamGiaController(QuyDinhGiamGiaService quyDinhGiamGiaServices, KhachSanService khachSanServices)
        {
            quyDinhGiamGiaService = quyDinhGiamGiaServices;
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
                List<QuyDinhGiamGia> quyDinhGiamGias = quyDinhGiamGiaService.GetAllQuyDinhGia();
                List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
                Modeldata modeldata = new Modeldata
                {
                    listquyDinhGiamGia = quyDinhGiamGias,
                    listKhachSan = listkhachsan
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
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
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}
