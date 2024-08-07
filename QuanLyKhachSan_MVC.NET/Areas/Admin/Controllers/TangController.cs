﻿using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TangController : Controller
    {
        private readonly TangService tangService;
        private readonly KhachSanService khachSanService;
        private readonly NhanVienService nhanVienService;
        public TangController(TangService tangServices, KhachSanService khachSanServices,
            NhanVienService nhanVienService)
        {
            tangService = tangServices;
            khachSanService = khachSanServices;
            this.nhanVienService = nhanVienService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {

                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                ViewData["idkhachsan"] = idkhachsan;
                List<Tang> tangs = tangService.GetAllTang();
                List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
                List<Modeldata> modelDataList = new List<Modeldata>();
                foreach (var tang in tangs)
                {
                    KhachSan khachSan = khachSanService.GetKhachSanById(tang.idkhachsan);
                    Modeldata modeldata = new Modeldata
                    {
                        tang = tang,
                        listKhachSan = listkhachsan,
                        khachSan = khachSan,
                    };
                    modelDataList.Add(modeldata);
                }
                return View(modelDataList);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }


        public IActionResult ThemTang(Tang tang)
        {
            tangService.ThemTang(tang);
            TempData["themthanhcong"] = "";
            return Redirect("~/admin/tang/");
        }
        public IActionResult XoaTang(int id)
        {
            tangService.XoaTang(id);
            TempData["xoathanhcong"] = "";
            return Redirect("~/admin/tang/");
        }

        public IActionResult GetTangID(int id)
        {
            Tang tangs = tangService.GetTangID(id);
            return View(tangs);
        }
        public IActionResult CapNhatTang(Tang tang)
        {
            tangService.CapNhatTang(tang);
            return Redirect("~/admin/tang/");
        }
    }

}
