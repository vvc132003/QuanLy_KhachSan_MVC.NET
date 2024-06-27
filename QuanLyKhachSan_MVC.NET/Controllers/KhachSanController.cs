using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class KhachSanController : Controller
    {
        private readonly KhachSanService khachSanService;
        public KhachSanController(KhachSanService khachSanServices)
        {
            khachSanService = khachSanServices;
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
                    Modeldata modeldata = new Modeldata()
                    {
                        PagedTKhachSan = ipageKhachsan,
                    };
                    return View(modeldata);
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
        public IActionResult ThemKhachSan(KhachSan khachSan)
        {
            khachSan.sosao = 0;
            khachSanService.ThemKhachSan(khachSan);
            TempData["themthanhcong"] = "";
            return RedirectToAction("Index", "KhachSan");
        }
    }
}