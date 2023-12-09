﻿using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ThoiGianController : Controller
    {
        private readonly ThoiGianService thoiGianService;
        public ThoiGianController(ThoiGianService thoiGianServices)
        {
            thoiGianService = thoiGianServices;
        }
        public IActionResult ThemThoiGian(ThoiGian thoiGian)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                ViewData["id"] = id;
                thoiGianService.ThemThoiGian(thoiGian);
                TempData["thoigianthanhcong"] = "";
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}