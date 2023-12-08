﻿using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ThietBiController : Controller
    {
        private readonly ThietBiService thietBiService;
        public ThietBiController(ThietBiService thietBiServices)
        {
            thietBiService = thietBiServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                List<ThietBi> listthietbi = thietBiService.GetAllThietBi();
                Modeldata modeldata = new Modeldata
                {
                    listThietBi = listthietbi,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThemThietBi(ThietBi thietbi)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                ViewData["id"] = id;
                thietbi.ngaymua = DateTime.Now;
                thietBiService.ThemThietBi(thietbi);
                return RedirectToAction("Index", "ThietBi");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}