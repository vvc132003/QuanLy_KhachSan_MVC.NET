﻿using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

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
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<LoaiDatPhong> loaiDatPhongs = loaiDatPhongService.GetAllLoaiDatPhong();
                Modeldata modeldata = new Modeldata()
                {
                    listloaiDatPhong = loaiDatPhongs,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
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
