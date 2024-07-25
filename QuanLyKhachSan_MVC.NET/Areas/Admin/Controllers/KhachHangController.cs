using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangController : Controller
    {
        private readonly KhachHangService khachHangService;
        private readonly NhanVienService nhanVienService;
        private readonly HttpClient _httpClient;

        public KhachHangController(KhachHangService khachHangServices, NhanVienService nhanVienServices, HttpClient httpClient)
        {
            khachHangService = khachHangServices;
            nhanVienService = nhanVienServices;
            _httpClient = httpClient;
        }
        public IActionResult Index(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idnv = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    ViewData["id"] = idnv;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;

                    List<KhachHang> listKhachHang = khachHangService.GetAllKhachHang();
                    int soluong = listKhachHang.Count;
                    int validPageNumber = sotrang ?? 1; // Current page, default is page 1
                    int pageSize = Math.Max(soluong, 1); // Number of items per page
                    PagedList.IPagedList<KhachHang> ipagelistKhachHang = listKhachHang.ToPagedList(validPageNumber, pageSize);
                    Modeldata yourModel = new Modeldata
                    {
                        PagedTKhachHang = ipagelistKhachHang,
                    };
                    return View(yourModel);
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
        public IActionResult DanhSachKhachHangDaDangKy(int? sotrang, string cccd)
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {

                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;

                List<KhachHang> listKhachHang;
                if (cccd != null)
                {
                    listKhachHang = khachHangService.GetAllKhachHang().FindAll(kh => kh.cccd == cccd);
                }
                else
                {
                    listKhachHang = khachHangService.GetAllKhachHang();
                }
                int soluong = listKhachHang.Count;
                int validPageNumber = sotrang ?? 1; // Current page, default is page 1
                int pageSize = Math.Max(soluong, 1); // Number of items per page
                PagedList.IPagedList<KhachHang> ipagelistKhachHang = listKhachHang.ToPagedList(validPageNumber, pageSize);
                Modeldata yourModel = new Modeldata
                {
                    PagedTKhachHang = ipagelistKhachHang,
                };
                return Json(yourModel);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
    }
}