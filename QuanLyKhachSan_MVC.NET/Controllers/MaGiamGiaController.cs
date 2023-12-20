using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class MaGiamGiaController : Controller
    {
        private readonly QuyDinhGiamGiaService quyDinhGiamGiaService;
        private readonly KhachSanService khachSanService;
        private readonly MaGiamGiaService maGiamGiaService;
        public MaGiamGiaController(QuyDinhGiamGiaService quyDinhGiamGiaServices, KhachSanService khachSanServices, MaGiamGiaService maGiamGiaServices)
        {
            quyDinhGiamGiaService = quyDinhGiamGiaServices;
            khachSanService = khachSanServices;
            maGiamGiaService = maGiamGiaServices;
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
                List<MaGiamGia> listmagiamgai = maGiamGiaService.GetAllMaGiamGia();
                Modeldata modeldata = new Modeldata
                {
                    listquyDinhGiamGia = quyDinhGiamGias,
                    listmaGiamGia = listmagiamgai
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThemMaGiamGia(MaGiamGia maGiamGia)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                ViewData["id"] = id;
                maGiamGia.ngaybatdau = DateTime.Now;
                maGiamGia.solandasudung = 0;
                maGiamGiaService.ThemMaGiamGia(maGiamGia);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}
