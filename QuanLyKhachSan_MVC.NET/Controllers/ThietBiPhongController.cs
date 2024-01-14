using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using Auth0.ManagementApi.Paging;
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
        /*  public IActionResult h(int? sotrang)
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
          }*/

        public IActionResult Index(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                List<ThietBiPhong> thietBiPhongs = thietBiPhongService.GetALLThietBiPhong();

                if (thietBiPhongs.Any())
                {
                    int soluong = thietBiPhongs.Count;
                    int validPageNumber = sotrang ?? 1;
                    int pageSize = Math.Max(soluong, 1);

                    PagedList.IPagedList<ThietBiPhong> pagedThietBiPhong = thietBiPhongs.ToPagedList(validPageNumber, pageSize);

                    List<Phong> listphong = phongService.GetAllPhong();
                    List<ThietBi> listthietbi = thetBiService.GetAllThietBi();

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
                    return RedirectToAction("Addthietbiphongs");
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }

        public IActionResult Addthietbiphongs()
        {
            List<Phong> listphong = phongService.GetAllPhong();
            List<ThietBi> listthietbi = thetBiService.GetAllThietBi();
            Modeldata modeldata = new Modeldata
            {
                listphong = listphong,
                listThietBi = listthietbi
            };
            return View(modeldata);
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
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}