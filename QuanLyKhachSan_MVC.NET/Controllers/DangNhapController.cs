using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Security.Claims;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Identity;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly NhanVienService nhanVienService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;

        public DangNhapController(NhanVienService nhanVienServices, KhachSanService khachSanServices, KhachHangService khachHangServices)
        {
            nhanVienService = nhanVienServices;
            khachSanService = khachSanServices;
            khachHangService = khachHangServices;
        }

        public IActionResult DangNhap()
        {
            NhanVien nhanVien = new NhanVien();
            Modeldata yourModel = new Modeldata
            {
                nhanVien = nhanVien,
            };
            return View(yourModel);
        }
        public IActionResult DangNhapVaoHeThong(string taikhoanoremail, string matkhau)
        {
            NhanVien nhanVien = nhanVienService.CheckThongTinDangNhaps(taikhoanoremail);
            KhachHang khachHang = khachHangService.GetKhachHangDangNhaps(taikhoanoremail);
            if (nhanVien != null)
            {
                var checkmatkhau = khachHangService.VerifyPassword(nhanVien.matkhau, matkhau);
                if (checkmatkhau == PasswordVerificationResult.Success)
                {
                    if (nhanVien.tenchucvu == "Quản lý")
                    {
                        HttpContext.Session.SetInt32("id", nhanVien.id);
                        HttpContext.Session.SetString("taikhoan", nhanVien.taikhoan);
                        HttpContext.Session.SetString("hovaten", nhanVien.hovaten);
                        HttpContext.Session.SetString("tenchucvu", nhanVien.tenchucvu);
                        HttpContext.Session.SetInt32("idkhachsan", nhanVien.idkhachsan);
                        return Redirect("~/admin/home/thongke");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("id", nhanVien.id);
                        HttpContext.Session.SetString("taikhoan", nhanVien.taikhoan);
                        HttpContext.Session.SetString("hovaten", nhanVien.hovaten);
                        HttpContext.Session.SetString("tenchucvu", nhanVien.tenchucvu);
                        HttpContext.Session.SetInt32("idkhachsan", nhanVien.idkhachsan);
                        return Redirect("~/admin/phong/");

                    }
                }
                else
                {
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else if (khachHang != null)
            {
                var result = khachHangService.VerifyPassword(khachHang.matkhau, matkhau);
                if (result == PasswordVerificationResult.Success)
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
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }

        public IActionResult DangNhapVaoHeThongs(string taikhoanoremail, string matkhau)
        {
            NhanVien nhanVien = nhanVienService.CheckThongTinDangNhaps(taikhoanoremail);
            KhachHang khachHang = khachHangService.GetKhachHangDangNhaps(taikhoanoremail);
            if (nhanVien != null)
            {
                var checkmatkhau = khachHangService.VerifyPassword(nhanVien.matkhau, matkhau);
                if (checkmatkhau == PasswordVerificationResult.Success)
                {
                    if (nhanVien.tenchucvu == "Quản lý")
                    {
                        HttpContext.Session.SetInt32("id", nhanVien.id);
                        HttpContext.Session.SetString("taikhoan", nhanVien.taikhoan);
                        HttpContext.Session.SetString("hovaten", nhanVien.hovaten);
                        HttpContext.Session.SetString("tenchucvu", nhanVien.tenchucvu);
                        HttpContext.Session.SetInt32("idkhachsan", nhanVien.idkhachsan);
                        return Json(new { success = true});
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("id", nhanVien.id);
                        HttpContext.Session.SetString("taikhoan", nhanVien.taikhoan);
                        HttpContext.Session.SetString("hovaten", nhanVien.hovaten);
                        HttpContext.Session.SetString("tenchucvu", nhanVien.tenchucvu);
                        HttpContext.Session.SetInt32("idkhachsan", nhanVien.idkhachsan);
                        return Json(new { success = true });

                    }
                }
                else
                {
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else if (khachHang != null)
            {
                var result = khachHangService.VerifyPassword(khachHang.matkhau, matkhau);
                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("id", khachHang.id);
                    HttpContext.Session.SetString("hovaten", khachHang.hovaten);
                    return Json(new { success = true });
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

        /* public IActionResult DangNhaps(string taikhoan, string matkhau)
         {
             NhanVien luudangnhap = nhanVienService.GetNhanVienDangNhap(matkhau, taikhoan);
             KhachHang khachHang = khachHangService.GetKhachHangDangNhap(taikhoan, matkhau);
             if (luudangnhap != null && (luudangnhap.tenchucvu == "Quản lý" || luudangnhap.tenchucvu == "Nhân viên"))
             {
                 return RedirectToAction("dangnhap", "dangnhap");
             }
             else if (khachHang != null)
             {
                 return RedirectToAction("Index", "Home");
             }
             else
             {
                                return RedirectToAction("dangnhap", "dangnhap");
             }
         }*/



        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap", "DangNhap");
        }
        public IActionResult Loginssss()
        {
            // Chuyển hướng người dùng đến trang đăng nhập Google
            var properties = new AuthenticationProperties { RedirectUri = "/DangNhap/GoogleResponse" };
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
                string idtaikhoangoogle = authenticationResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string name = authenticationResult.Principal.FindFirstValue(ClaimTypes.Name);
                string email = authenticationResult.Principal.FindFirstValue(ClaimTypes.Email);

                // Check if the user already exists in the database
                KhachHang khachHang = khachHangService.GetKhachHangbyidtaikhoangoogle(idtaikhoangoogle);
                KhachHang checkemail = khachHangService.GetKhachHangbyemail(email);
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
                    return RedirectToAction("index", "home");
                }
                else if (checkemail.email == email)
                {
                    checkemail.idtaikhoangoogle = idtaikhoangoogle;
                    khachHangService.CapNhatKhachHang(checkemail);
                    return RedirectToAction("index", "home");
                }
                // Redirect to the Logins action
                return RedirectToAction("index", "home");
            }
            else
            {
                // Handle the case where authentication failed
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }

        public async Task<IActionResult> Logout()
        {
            // Đăng xuất người dùng khỏi hệ thống
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Chuyển hướng người dùng đến trang chủ hoặc trang mong muốn sau khi đăng xuất
            return RedirectToAction("DangNhap", "DangNhap");
        }
        public async Task<IActionResult> Index()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return RedirectToAction(nameof(DangNhapController.Index), "DangNhap");

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
    }
}