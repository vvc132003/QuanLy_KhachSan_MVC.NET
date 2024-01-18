using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ChinhSachGiaController : Controller
    {
        private readonly ChinhSachGiaService chinhSachGiaService;
        private readonly NgayLeService ngayLeService;
        public ChinhSachGiaController(ChinhSachGiaService chinhSachGiaService, NgayLeService ngayLeService)
        {
            this.chinhSachGiaService = chinhSachGiaService;
            this.ngayLeService = ngayLeService;
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
                List<ChinhSachGia> chinhSachGiaslist = chinhSachGiaService.GetAllChinhSachGia();
                List<Modeldata> modeldatas = new List<Modeldata>();
                foreach (var chinhsachgia in chinhSachGiaslist)
                {
                    NgayLe ngayLe = ngayLeService.GetNgayLeById(chinhsachgia.idngayle);
                    List<NgayLe> ngayLes = ngayLeService.GetAllNgayLes();
                    Modeldata modeldata = new Modeldata()
                    {
                        chinhSachGia = chinhsachgia,
                        ngayLe = ngayLe,
                        listngayle = ngayLes,
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
        public IActionResult AddChinhSach()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<NgayLe> ngayLes = ngayLeService.GetAllNgayLes();
                Modeldata modeldata = new Modeldata()
                {
                    listngayle = ngayLes,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult ThemChinhSach(float dieuchinhgiaphong, String tenchinhsach, DateTime ngaybatdau, DateTime ngayketthuc, int idngayle)
        {
            ChinhSachGia chinhSachGia = new ChinhSachGia();
            chinhSachGia.dieuchinhgiaphong = dieuchinhgiaphong;
            chinhSachGia.tenchinhsach = tenchinhsach;
            chinhSachGia.ngayketthuc = ngayketthuc;
            chinhSachGia.ngaybatdau = ngaybatdau;
            chinhSachGia.idngayle = idngayle;
            chinhSachGiaService.ThemChinhSachGia(chinhSachGia);
            TempData["themthanhcong"] = "";
            return RedirectToAction("index", "chinhsachgia");
        }
    }
}