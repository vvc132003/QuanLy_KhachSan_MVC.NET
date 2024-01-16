using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly SanPhamService sanPhamService;
        private readonly KhachSanService khachSanService;

        public SanPhamController(SanPhamService sanPhamServices, KhachSanService khachSanService)
        {
            sanPhamService = sanPhamServices;
            this.khachSanService = khachSanService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                ViewData["idkhachsan"] = idkhachsan;
                List<SanPham> sanphams = sanPhamService.GetAllSanPham();
                List<Modeldata> modeldatalist = new List<Modeldata>();
                foreach (var sanPham in sanphams)
                {
                    KhachSan khachSan = khachSanService.GetKhachSanById(sanPham.idkhachsan);
                    Modeldata modeldata = new Modeldata
                    {
                        sanPham = sanPham,
                        khachSan = khachSan,
                    };
                    modeldatalist.Add(modeldata);
                }
                return View(modeldatalist);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
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
                return RedirectToAction("dangnhap", "dangnhap");
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
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}
