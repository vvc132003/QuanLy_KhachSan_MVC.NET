using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ThietBiPhongController : Controller
    {
        private readonly ThietBiPhongService thietBiPhongService;
        private readonly PhongService phongService;
        private readonly ThietBiService thetBiService;
        public ThietBiPhongController(ThietBiPhongService thietBiPhongServices, PhongService phongServices, ThietBiService thetBiServices)
        {
            thietBiPhongService = thietBiPhongServices;
            phongService = phongServices;
            thetBiService = thetBiServices;
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
                List<ThietBiPhong> listthietbiphong = thietBiPhongService.GetALLThietBiPhong();
                List<Phong> listphong = phongService.GetAllPhong();
                List<ThietBi> listthietbi = thetBiService.GetAllThietBi();
                Modeldata modeldata = new Modeldata
                {
                    listThietBiphong = listthietbiphong,
                    listphong = listphong,
                    listThietBi = listthietbi
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThemThietBiPhong(List<int> idphongs, List<int> idthietbis, int soluongduavao)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                ViewData["id"] = id;
                foreach (int idphong in idphongs)
                {
                    foreach (int idthietbi in idthietbis)
                    {
                        ThietBiPhong thietBiPhong = new ThietBiPhong();
                        thietBiPhong.idthietbi = idthietbi;
                        thietBiPhong.idphong = idphong;
                        thietBiPhong.ngayduavao = DateTime.Now;
                        thietBiPhong.soluongduavao = soluongduavao;
                        thietBiPhongService.ThemThietBiPhong(thietBiPhong);
                        ThietBi thietBi = thetBiService.GetThietBiByID(idthietbi);
                        thietBi.soluongcon -= soluongduavao;
                        thetBiService.CapNhatThietBi(thietBi);
                    }
                }
                return RedirectToAction("Index", "ThietBiPhong");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}