using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly SanPhamService sanPhamService;
        private readonly KhachSanService khachSanService;
        private readonly LoaiDichDichVuService loaiDichDichVuService;

        public SanPhamController(SanPhamService sanPhamServices, KhachSanService khachSanService, LoaiDichDichVuService loaiDichDichVuService)
        {
            sanPhamService = sanPhamServices;
            this.khachSanService = khachSanService;
            this.loaiDichDichVuService = loaiDichDichVuService;
        }
        public IActionResult Index(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
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
                    int soluong = sanphams.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    PagedList.IPagedList<SanPham> ipagesanPhams = sanphams.ToPagedList(validPageNumber, pageSize);
                    List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var sanPham in ipagesanPhams)
                    {
                        LoaiDichVu loaiDichVu = loaiDichDichVuService.GetLoaiDichVuById(sanPham.idloaidichvu);
                        KhachSan khachSan = khachSanService.GetKhachSanById(loaiDichVu.idkhachsan);
                        Modeldata modeldata = new Modeldata
                        {
                            PagedTSanPham = new List<SanPham> { sanPham }.ToPagedList(1, 1),
                            loaiDichVu = loaiDichVu,
                            khachSan = khachSan,
                        };
                        modeldatalist.Add(modeldata);
                    }
                    return View(new Tuple<List<Modeldata>, List<LoaiDichVu>>(modeldatalist, loaiDichVus));
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
        public IActionResult ThemSanPham(SanPham sanPham)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                sanPham.trangthai = "còn bán";
                sanPhamService.ThemSanPham(sanPham);
                return Redirect("~/admin/sanpham/");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }


        public IActionResult UpdateSanPham(int? idsanpham)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    ViewData["idkhachsan"] = idkhachsan;

                    return View();
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

        public IActionResult XoaSanPham(int id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnd = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnd;
                ViewData["hovaten"] = hovaten;
                sanPhamService.XoaSanPham(id);
                return Redirect("~/admin/sanpham/");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}
