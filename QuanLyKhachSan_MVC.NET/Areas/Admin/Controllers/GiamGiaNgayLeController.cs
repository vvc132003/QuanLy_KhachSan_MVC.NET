using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GiamGiaNgayLeController : Controller
    {
        private readonly GiamGiaNgayLeService giamGiaNgayLeService;
        public GiamGiaNgayLeController(GiamGiaNgayLeService giamGiaNgayLeService)
        {
            this.giamGiaNgayLeService = giamGiaNgayLeService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idnv = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    ViewData["id"] = idnv;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<GiamGiaNgayLe> giamGiaNgayLes = giamGiaNgayLeService.GetAllGiamGiaNgayLe();
                    List<Modeldata> modeldatas = new List<Modeldata>();
                    foreach (var giamgiangayle in giamGiaNgayLes)
                    {
                        Modeldata modeldata = new Modeldata()
                        {
                            giamGiaNgayle = giamgiangayle,
                        };
                        modeldatas.Add(modeldata);
                    }
                    return View(modeldatas);
                }
                else
                {
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}
