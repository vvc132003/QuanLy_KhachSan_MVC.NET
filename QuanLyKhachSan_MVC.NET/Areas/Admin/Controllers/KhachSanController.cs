using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachSanController : Controller
    {
        private readonly KhachSanService khachSanService;
        private readonly ThoiGianService thoiGianService;
        public KhachSanController(KhachSanService khachSanServices, ThoiGianService thoiGianService)
        {
            khachSanService = khachSanServices;
            this.thoiGianService = thoiGianService;
        }
        public IActionResult Index(int? sotrang)
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
                    List<KhachSan> khachsanlist = khachSanService.GetAllKhachSan();
                    int soluong = khachsanlist.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    PagedList.IPagedList<KhachSan> ipageKhachsan = khachsanlist.ToPagedList(validPageNumber, pageSize);
                    List<Modeldata> modeldatas = new List<Modeldata>();
                    foreach (var khachsan in ipageKhachsan)
                    {
                        ThoiGian thoiGian = thoiGianService.GetThoiGian(khachsan.id);
                        Modeldata modeldata = new Modeldata()
                        {
                            PagedTKhachSan = new List<KhachSan> { khachsan }.ToPagedList(1, 1),
                            thoigian = thoiGian,
                        };
                        modeldatas.Add(modeldata);
                    }
                    return View(modeldatas);
                }
                else
                {
                    return Redirect("~/customer/dangnhap/dangnhap");
                }
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult ThemKhachSan(KhachSan khachSan, ThoiGian thoiGian)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                ViewData["id"] = id;
                ViewData["idkhachsan"] = idkhachsan;
                khachSan.sosao = 0;
                int idks = khachSanService.ThemKhachSan(khachSan);
                thoiGian.idkhachsan = idks;
                thoiGianService.ThemThoiGian(thoiGian);
                TempData["themthanhcong"] = "";
                return Redirect("~/admin/khachsan/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
    }
}