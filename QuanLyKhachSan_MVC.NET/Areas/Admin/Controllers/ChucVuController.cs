﻿using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChucVuController : Controller
    {
        private readonly ChucVuService chucVuService;

        public ChucVuController(ChucVuService chucVuServices)
        {
            chucVuService = chucVuServices;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<ChucVu> chucVus = chucVuService.GetAllChucVu();
                    List<Modeldata> modellisst = new List<Modeldata>();
                    foreach (var chucVu in chucVus)
                    {
                        Modeldata modeldata = new Modeldata
                        {
                            chucVu = chucVu,
                        };
                        modellisst.Add(modeldata);
                    }
                    return View(modellisst);
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
        public IActionResult XoaChucVu(int id)
        {
            chucVuService.XoaChucVu(id);
            TempData["xoathanhcong"] = "";
            return Redirect("~/admin/chucvu/");
        }

        public IActionResult ThemChucVu(ChucVu chucVu)
        {
            chucVuService.ThemChucVu(chucVu);
            TempData["themthanhcong"] = "";
            return Redirect("~/admin/chucvu/");
        }
    }
}