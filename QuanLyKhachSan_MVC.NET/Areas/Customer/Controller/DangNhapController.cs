using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Security.Claims;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Identity;


namespace QuanLyKhachSan_MVC.NET.Areas.Customer.Controllers
{
    [Area("Customer")]
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
                        HttpContext.Session.SetString("image", nhanVien.image);
                        return Redirect("~/admin/home/thongke");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("id", nhanVien.id);
                        HttpContext.Session.SetString("taikhoan", nhanVien.taikhoan);
                        HttpContext.Session.SetString("hovaten", nhanVien.hovaten);
                        HttpContext.Session.SetString("tenchucvu", nhanVien.tenchucvu);
                        HttpContext.Session.SetInt32("idkhachsan", nhanVien.idkhachsan);
                        HttpContext.Session.SetString("image", nhanVien.image);
                        return Redirect("~/staff/phong/");

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
                        return Json(new { success = true });
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



        public async Task<IActionResult> DangXuat()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("dangnhap", "dangnhap");
        }

        public async Task<IActionResult> Logout()
        {
            // Đăng xuất người dùng khỏi hệ thống
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Chuyển hướng người dùng đến trang chủ hoặc trang mong muốn sau khi đăng xuất
            return RedirectToAction("dangnhap", "dangnhap");
        }
        public IActionResult Loginssss()
        {
            // Chuyển hướng người dùng đến trang đăng nhập Google
            var properties = new AuthenticationProperties { RedirectUri = "/DangNhap/GoogleResponse" };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            // Xác thực thông tin từ Google
            var authenticationResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            // Kiểm tra xem quá trình xác thực có thành công không
            if (authenticationResult.Succeeded)
            {
                // Lấy thông tin từ Google
                string idtaikhoangoogle = authenticationResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string name = authenticationResult.Principal.FindFirstValue(ClaimTypes.Name);
                string email = authenticationResult.Principal.FindFirstValue(ClaimTypes.Email);

                // Kiểm tra xem khách hàng có trong hệ thống dựa trên id Google không
                KhachHang khachHang = khachHangService.GetKhachHangbyidtaikhoangoogle(idtaikhoangoogle);
                // Kiểm tra xem khách hàng có trong hệ thống dựa trên email không
                KhachHang checkemail = khachHangService.GetKhachHangbyemail(email);

                // Nếu khách hàng chưa có trong hệ thống, tạo mới khách hàng
                if (khachHang == null)
                {
                    khachHang = new KhachHang
                    {
                        idtaikhoangoogle = idtaikhoangoogle,
                        hovaten = name,
                        email = email
                    };
                    // Thêm khách hàng vào hệ thống
                    khachHangService.ThemKhachHangGoogle(khachHang);
                    // Chuyển hướng về trang chủ
                    return RedirectToAction("index", "home");
                }
                // Nếu email của khách hàng trùng khớp, cập nhật id Google
                else if (checkemail.email == email)
                {
                    checkemail.idtaikhoangoogle = idtaikhoangoogle;
                    // Cập nhật thông tin khách hàng trong hệ thống
                    khachHangService.CapNhatKhachHang(checkemail);
                    // Chuyển hướng về trang chủ
                    return RedirectToAction("index", "home");
                }
                // Chuyển hướng về trang chủ nếu khách hàng đã tồn tại
                return RedirectToAction("index", "home");
            }
            else
            {
                // Chuyển hướng về trang đăng nhập nếu xác thực thất bại
                return RedirectToAction("DangNhap", "DangNhap");
            }
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