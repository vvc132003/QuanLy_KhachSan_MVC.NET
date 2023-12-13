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
        private readonly HopDongLaoDongService hopDongLaoDongService;
        private readonly KhachSanService khachSanService;

        public NhanVienController(NhanVienService nhanVienServices, ChucVuService chucVuServices, BoPhanService boPhanServices, ViTriBoPhanService viTriBoPhanServices, HopDongLaoDongService hopDongLaoDongServices,KhachSanService khachSanServices)
        {
            nhanVienService = nhanVienServices;
            chucVuService = chucVuServices;
            boPhanService = boPhanServices;
            viTriBoPhanService = viTriBoPhanServices;
            hopDongLaoDongService = hopDongLaoDongServices;
            khachSanService = khachSanServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("tenchucvu") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
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
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetInt32("idkhachsan") != null &&  HttpContext.Session.GetString("hovaten") != null)
            {
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                ViewData["idkhachsan"] = idkhachsan;
                List<ViTriBoPhan> viTriBoPhans = viTriBoPhanService.GetAllViTriBoPhan();
                List<BoPhan> boPhans = boPhanService.GetALLBoPhan();
                List<ChucVu> chucVus = chucVuService.GetAllChucVu();
                List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
                if (viTriBoPhans != null && boPhans != null && chucVus != null)
                {
                    Modeldata modeldata = new Modeldata
                    {
                        listviTriBoPhan = viTriBoPhans,
                        listbophan = boPhans,
                        listchucVu = chucVus,
                        listKhachSan = listkhachsan
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
        public IActionResult AddNhanVienn(NhanVien nhanVien, HopDongLaoDong hopDongLaoDong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                nhanVien.solanvipham = 0;
                nhanVien.trangthai = "Đang hoạt động";
                int idnhanvien = nhanVienService.ThemNhanVien(nhanVien);
                hopDongLaoDong.idnhanvien = idnhanvien;
                hopDongLaoDong.ngaybatdau = DateTime.Now;
                hopDongLaoDongService.ThemHopDongLaoDong(hopDongLaoDong);
                TempData["themthanhcong"] = "";
                return RedirectToAction("Index", "NhanVien");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult XuatEclcel()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
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
