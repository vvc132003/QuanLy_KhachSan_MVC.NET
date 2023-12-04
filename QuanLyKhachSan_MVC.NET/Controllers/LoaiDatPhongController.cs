using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class LoaiDatPhongController : Controller
    {
        private readonly LoaiDatPhongService loaiDatPhongService;

        public LoaiDatPhongController(LoaiDatPhongService loaiDatPhongServices)
        {
            loaiDatPhongService = loaiDatPhongServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                List<LoaiDatPhong> loaiDatPhongs = loaiDatPhongService.GetAllLoaiDatPhong();
                Modeldata modeldata = new Modeldata()
                {
                    listloaiDatPhong = loaiDatPhongs,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThemLoaiDatPhong(LoaiDatPhong loaiDatPhongs)
        {
            loaiDatPhongService.ThemLoaiDatPhong(loaiDatPhongs);
            TempData["themthanhcong"] = "";
            return RedirectToAction("Index", "LoaiDatPhong");
        }
    }
}
