﻿using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly SanPhamService sanPhamService;


        public SanPhamController(SanPhamService sanPhamServices)
        {
            sanPhamService = sanPhamServices;
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
                List<SanPham> sanphams = sanPhamService.GetAllSanPham();
                Modeldata modeldata = new Modeldata
                {
                    listsanPham = sanphams,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
        public IActionResult ThemSanPham(SanPham sanPham)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                sanPham.idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                sanPham.trangthai = "còn bán";
                sanPhamService.ThemSanPham(sanPham);
                return RedirectToAction("Index", "SanPham");
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
        public IActionResult XoaSanPham(int id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnd = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnd;
                ViewData["hovaten"] = hovaten;
                sanPhamService.XoaSanPham(id);
                return RedirectToAction("Index", "SanPham");
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
    }
}
