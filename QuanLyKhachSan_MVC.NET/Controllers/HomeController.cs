using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Security.Claims;
using QuanLyKhachSan_MVC.NET.Areas.Login.Modelss;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly PhongService phongService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;

        public HomeController(PhongService phongServices, KhachSanService khachSanServices, KhachHangService khachHangService)
        {
            phongService = phongServices;
            khachSanService = khachSanServices;
            this.khachHangService = khachHangService;
        }
        public IActionResult AddNhanVien()
        {
            return View();
        }
        public IActionResult DiemDanh()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                KhachHang khachHang = khachHangService.GetKhachHangbyid(id);
                Modeldata modeldata = new Modeldata
                {
                    khachhang = khachHang,
                };
                return View(modeldata);
            }
            else
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (result != null && result.Succeeded)
                {
                    var name = result.Principal.FindFirstValue(ClaimTypes.Name);
                    var email = result.Principal.FindFirstValue(ClaimTypes.Email);
                    var profileLink = result.Principal.FindFirstValue("urn:google:profile");
                    var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    var userProfileViewModel = new UserProfileViewModel
                    {
                        Name = name,
                        Email = email,
                        ProfileLink = profileLink,
                        UserId = userId
                    };
                    Modeldata modeldata = new Modeldata
                    {
                        userProfileViewModel = userProfileViewModel
                    };
                    return View(modeldata);
                }
                else
                {
                    return View();
                }
            }
        }
        public async Task<IActionResult> TimKiemKhachSan(int idkhachsan, string loaiphong, int songuoi)
        {
            if (idkhachsan != 0 && loaiphong != null && songuoi != 0)
            {
                List<Phong> phonglisst = phongService.GetAllPhongByidKhachSanndlaoiphongandsonguoi(idkhachsan, loaiphong, songuoi);
                if (phonglisst != null)
                {
                    KhachSan khachSan = khachSanService.GetKhachSanById(idkhachsan);
                    if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
                    {
                        int id = HttpContext.Session.GetInt32("id").Value;
                        string hovaten = HttpContext.Session.GetString("hovaten");
                        ViewData["id"] = id;
                        ViewData["hovaten"] = hovaten;
                        KhachHang khachHang = khachHangService.GetKhachHangbyid(id);
                        Modeldata modeldata = new Modeldata
                        {
                            khachhang = khachHang,
                            listphong = phonglisst,
                            khachSan = khachSan,
                        };
                        return View(modeldata);
                    }
                    else
                    {
                        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        if (result != null && result.Succeeded)
                        {
                            var name = result.Principal.FindFirstValue(ClaimTypes.Name);
                            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
                            var profileLink = result.Principal.FindFirstValue("urn:google:profile");
                            var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                            var userProfileViewModel = new UserProfileViewModel
                            {
                                Name = name,
                                Email = email,
                                ProfileLink = profileLink,
                                UserId = userId
                            };
                            Modeldata modeldata = new Modeldata
                            {
                                userProfileViewModel = userProfileViewModel,
                                listphong = phonglisst,
                                khachSan = khachSan,
                            };
                            return View(modeldata);
                        }
                        else
                        {
                            Modeldata modeldata = new Modeldata
                            {
                                listphong = phonglisst,
                                khachSan = khachSan,
                            };
                            return View(modeldata);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("", "home");
                }
            }
            else
            {
                return RedirectToAction("", "home");
            }
        }
        public IActionResult GetAllKhachSan()
        {
            List<KhachSan> allKhachSans = khachSanService.GetAllKhachSan();
            string html = "<ul style='list-style-type: none; margin: 0; padding: 0;'>";
            foreach (KhachSan khachSan in allKhachSans)
            {
                html += "<li style='background-color: #fff; padding: 8px; border-bottom: 1px solid #ccc; cursor: pointer;' data-id='" + khachSan.id + "'>" + khachSan.tenkhachsan + "</li>";
            }
            html += "</ul>";
            return Content(html);
        }
        public IActionResult GetAllLoaiPhongIdKhachSan(int idkhachsan)
        {
            List<string> loaiPhongs = phongService.GetAllLoaiPhongIdKhachSan(idkhachsan);
            string html = "<ul style='list-style-type: none; margin: 0; padding: 0;'>";
            foreach (string loaiPhong in loaiPhongs)
            {
                html += "<li style='background-color: #fff; padding: 8px; border-bottom: 1px solid #ccc; cursor: pointer;'>" + loaiPhong + "</li>";
            }
            html += "</ul>";
            return Content(html);
        }
        public IActionResult GetAllSoNguoiLoaiPhong(string loaiphong)
        {
            List<int> songuois = phongService.GetAllSoNguoiLoaiPhong(loaiphong);
            string html = "";
            foreach (int songuoi in songuois)
            {
                html += "<input type='text' id='songuoi' name='songuoi' class='form-control datetimepicker-input' value='" + songuoi + "' />";
            }
            html += "";
            return Content(html);
        }
    }
}