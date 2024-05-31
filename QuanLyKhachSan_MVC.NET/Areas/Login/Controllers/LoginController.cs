using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using QuanLyKhachSan_MVC.NET.Areas.Login.Modelss;

namespace QuanLyKhachSan_MVC.NET.Areas.Login.Controllers
{
    [Area("Login")]
    public class LoginController : Controller
    {
        private readonly NhanVienService nhanVienService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;

        public LoginController(NhanVienService nhanVienServices, KhachSanService khachSanServices, KhachHangService khachHangServices)
        {
            nhanVienService = nhanVienServices;
            khachSanService = khachSanServices;
            khachHangService = khachHangServices;
        }
        public async Task<IActionResult> Logins()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return RedirectToAction(nameof(LoginController.Logins), "Login");

            // Lấy thông tin người dùng từ các Claims
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var profileLink = result.Principal.FindFirstValue("urn:google:profile");
            var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier); // Lấy ID của người dùng
            // Tạo một instance của UserProfileViewModel và truyền thông tin người dùng vào
            var userProfileViewModel = new UserProfileViewModel
            {
                Name = name,
                Email = email,
                ProfileLink = profileLink,
                UserId = userId // Thêm ID của người dùng vào model

            };

            // Trả về view UserProfile và truyền model vào view
            var modeldata = new Modeldata
            {
                userProfileViewModel = userProfileViewModel
            };
            return View(modeldata);
        }
        public IActionResult Loginssss()
        {
            // Chuyển hướng người dùng đến trang đăng nhập Google
            var properties = new AuthenticationProperties { RedirectUri = "/Login/Login/GoogleResponse" };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            // Define authentication properties with a redirect URI

            // Authenticate the user using the Google authentication scheme
            var authenticationResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (authenticationResult.Succeeded)
            {
                // Extract user information from the claims
                var idtaikhoangoogle = authenticationResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var name = authenticationResult.Principal.FindFirstValue(ClaimTypes.Name);
                var email = authenticationResult.Principal.FindFirstValue(ClaimTypes.Email);

                // Check if the user already exists in the database
                KhachHang khachHang = khachHangService.GetKhachHangbyidtaikhoangoogle(idtaikhoangoogle);

                if (khachHang == null)
                {
                    // If the user does not exist, create a new one
                    khachHang = new KhachHang
                    {
                        idtaikhoangoogle = idtaikhoangoogle,
                        hovaten = name,
                        email = email
                    };

                    khachHangService.ThemKhachHangGoogle(khachHang);
                    return RedirectToAction("Logins", "/Login/Login");

                }

                // Redirect to the Logins action
                return RedirectToAction("Logins", "/Login/Login");
            }
            else
            {
                // Handle the case where authentication failed
                return RedirectToAction("Logins", "/Login/Login");
            }
        }


        public async Task<IActionResult> Logout()
        {
            // Đăng xuất người dùng khỏi hệ thống
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Chuyển hướng người dùng đến trang chủ hoặc trang mong muốn sau khi đăng xuất
            return RedirectToAction("Logins", "Login/Login");
        }
        public IActionResult DangNhapVaoHeThong(string taikhoan, string matkhau)
        {
            NhanVien luudangnhap = nhanVienService.CheckThongTinDangNhap(matkhau, taikhoan);
            KhachHang khachHang = khachHangService.GetKhachHangDangNhap(taikhoan, matkhau);
            if (luudangnhap != null)
            {
                if (luudangnhap.tenchucvu == "Quản lý")
                {
                    HttpContext.Session.SetInt32("id", luudangnhap.id);
                    HttpContext.Session.SetString("taikhoan", luudangnhap.taikhoan);
                    HttpContext.Session.SetString("hovaten", luudangnhap.hovaten);
                    HttpContext.Session.SetString("tenchucvu", luudangnhap.tenchucvu);
                    HttpContext.Session.SetInt32("idkhachsan", luudangnhap.idkhachsan);
                    return RedirectToAction("home", "phong");
                }
                else
                {
                    HttpContext.Session.SetInt32("id", luudangnhap.id);
                    HttpContext.Session.SetString("taikhoan", luudangnhap.taikhoan);
                    HttpContext.Session.SetString("hovaten", luudangnhap.hovaten);
                    HttpContext.Session.SetString("tenchucvu", luudangnhap.tenchucvu);
                    HttpContext.Session.SetInt32("idkhachsan", luudangnhap.idkhachsan);
                    return RedirectToAction("index", "phong");
                }
            }
            else if (khachHang != null)
            {
                HttpContext.Session.SetInt32("id", khachHang.id);
                HttpContext.Session.SetString("hovaten", khachHang.hovaten);
                return RedirectToAction("index", "home");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}
