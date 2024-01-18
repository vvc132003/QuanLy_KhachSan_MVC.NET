using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class NgayLeController : Controller
    {
        private readonly NgayLeService ngayLeService;
        public NgayLeController(NgayLeService ngayLeService)
        {
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
                List<NgayLe> ngayLelist = ngayLeService.GetAllNgayLes();
                Modeldata modeldata = new Modeldata()
                {
                    listngayle = ngayLelist,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult AddNgayLe(NgayLe ngayLe)
        {
            ngayLeService.ThemNgayLe(ngayLe);
            TempData["themthanhcong"] = "";
            return RedirectToAction("index", "ngayle");
        }
    }
}