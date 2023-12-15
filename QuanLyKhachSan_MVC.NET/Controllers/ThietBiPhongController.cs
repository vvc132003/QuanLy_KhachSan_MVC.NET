using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;
using PagedList;
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
        public IActionResult h(int? sotrang)
        {
            int? soluong = thietBiPhongService.SumThietBiPhong();
            int tranghientai = sotrang ?? 1;
            int soluonghienthiomoitrang = soluong ?? 10;
            List<Phong> listphong = phongService.GetAllPhong();
            List<ThietBi> listthietbi = thetBiService.GetAllThietBi();
            IPagedList<ThietBiPhong> pagedThietBiPhong = thietBiPhongService.GetALLThietBiPhong().ToPagedList(tranghientai, soluonghienthiomoitrang);
            Modeldata modeldata = new Modeldata
            {
                listphong = listphong,
                listThietBi = listthietbi,
                PagedThietBiPhong = pagedThietBiPhong,
            };
            return View(modeldata);
        }

        public IActionResult Index(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<ThietBiPhong> thietBiPhongs = thietBiPhongService.GetALLThietBiPhong();
                int soluong = 0;
                foreach (var thietbị in thietBiPhongs)
                {
                    thietbị.id = soluong;
                    soluong++;
                }
                List<Phong> listphong = phongService.GetAllPhong();
                List<ThietBi> listthietbi = thetBiService.GetAllThietBi();
                IPagedList<ThietBiPhong> pagedThietBiPhong = thietBiPhongs.ToPagedList(sotrang ?? 1, soluong);
                Modeldata modeldata = new Modeldata
                {
                    PagedThietBiPhong = pagedThietBiPhong,
                    listphong = listphong,
                    listThietBi = listthietbi
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
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
                return RedirectToAction("Index", "DangNhap");
            }
        }
    }
}