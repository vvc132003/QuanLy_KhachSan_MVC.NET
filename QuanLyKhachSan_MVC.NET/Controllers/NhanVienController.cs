using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;
using System.Collections.Generic;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly NhanVienService nhanVienService;
        private readonly ChucVuService chucVuService;
        private readonly BoPhanService boPhanService;
        private readonly ViTriBoPhanService viTriBoPhanService;

        public NhanVienController(NhanVienService nhanVienServices, ChucVuService chucVuServices, BoPhanService boPhanServices, ViTriBoPhanService viTriBoPhanServices)
        {
            nhanVienService = nhanVienServices;
            chucVuService = chucVuServices;
            boPhanService = boPhanServices;
            viTriBoPhanService = viTriBoPhanServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                List<NhanVien> nhanViens = nhanVienService.GetAllNhanVien();
                Modeldata modeldata = new Modeldata
                {
                    listnhanVien = nhanViens,
                };
                return View(modeldata);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult AddNhanVien()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                List<ViTriBoPhan> viTriBoPhans = viTriBoPhanService.GetAllViTriBoPhan();
                List<BoPhan> boPhans = boPhanService.GetALLBoPhan();
                List<ChucVu> chucVus = chucVuService.GetAllChucVu();
                if (viTriBoPhans != null && boPhans != null && chucVus != null)
                {
                    Modeldata modeldata = new Modeldata
                    {
                        listviTriBoPhan = viTriBoPhans,
                        listbophan = boPhans,
                        listchucVu = chucVus
                    };
                    return View(modeldata);
                }
                return View("Index");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult AddNhanVienn(NhanVien nhanVien)
        {
            nhanVien.solanvipham = 0;
            nhanVien.trangthai = "Đang hoạt động";
            nhanVienService.ThemNhanVien(nhanVien);
            TempData["themthanhcong"] = "";
            return RedirectToAction("Index", "NhanVien");
        }
        public IActionResult XuatEclcel()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                nhanVienService.Xuatexcel();
                return RedirectToAction("Index", "NhanVien");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}
