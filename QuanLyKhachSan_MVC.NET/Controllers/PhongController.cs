using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;
using System.Collections.Generic;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class PhongController : Controller
    {
        private readonly PhongService phongService;
        private readonly TangService tangService;

        public PhongController(PhongService phongServices, TangService tangServices)
        {
            phongService = phongServices;
            tangService = tangServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                List<Tang> tanglist = tangService.GetAllTang();
                List<Modeldata> modeldataList = new List<Modeldata>();
                foreach (var tang in tanglist)
                {
                    List<Phong> phongs = phongService.GetAllPhongIDTang(tang.id);
                    List<Phong> phongtrangthai = phongService.GetAllPhongTrangThai();
                    Modeldata modeldata = new Modeldata
                    {
                        tang = tang,
                        listphong = phongs,
                        listphongtrangthai = phongtrangthai,
                    };
                    modeldataList.Add(modeldata);
                }
                return View(modeldataList);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}
