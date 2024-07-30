using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;
using Service.Service;
using System.Security.Claims;

namespace QuanLyKhachSan_MVC.NET.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class KhachHangController : Controller
    {
        private readonly KhachHangService khachHangService;
        private readonly NhanVienService nhanVienService;
        private readonly HttpClient _httpClient;
        private readonly XacMinhService xacMinhService;
        public KhachHangController(KhachHangService khachHangServices, NhanVienService nhanVienServices, HttpClient httpClient, XacMinhService xacMinhService)
        {
            khachHangService = khachHangServices;
            nhanVienService = nhanVienServices;
            _httpClient = httpClient;
            this.xacMinhService = xacMinhService;
        }


        public async Task<IActionResult> Chat()
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


        public IActionResult DangKy()
        {
            return View();
        }

        public IActionResult XacMinh()
        {
            return View();
        }


        public IActionResult DangKyNhanMaXacNhan(KhachHang khachHang, string checkmatkhau)
        {
            var maso = new Random().Next(100000, 999999).ToString();
            XacMinh xacMinh = new XacMinh();
            xacMinh.email = khachHang.email;
            xacMinh.maso = maso;
            xacMinh.thoigianhethan = DateTime.Now.AddMinutes(30);
            KhachHang khachHangByEmail = khachHangService.GetKhachHangbyemail(khachHang.email);
            KhachHang khachHangByTaiKhoan = khachHangService.GetKhachHangTaiKhoan(khachHang.taikhoan);
            NhanVien nhanVienByTaiKhoan = nhanVienService.NhanVienByTaiKhoan(khachHang.taikhoan);

            // Kiểm tra nếu tài khoản và email chưa tồn tại
            if (khachHangByTaiKhoan == null && nhanVienByTaiKhoan == null)
            {
                // Kiểm tra nếu tài khoản không trùng với tài khoản của khách hàng hiện tại hoặc tài khoản của nhân viên hiện tại
                if (khachHang.taikhoan != khachHangByEmail?.taikhoan && khachHang.taikhoan != nhanVienByTaiKhoan?.taikhoan)
                {
                    if (khachHangByEmail != null)
                    {
                        // Cập nhật thông tin khách hàng đã tồn tại
                        if (khachHang.matkhau == checkmatkhau)
                        {
                            string mahoamatkhau = khachHangService.HashPassword(khachHang.matkhau);
                            khachHangByEmail.matkhau = mahoamatkhau;
                            khachHangByEmail.hovaten = khachHang.hovaten;
                            khachHangByEmail.sodienthoai = khachHang.sodienthoai;
                            khachHangByEmail.email = khachHang.email;
                            khachHangByEmail.tinh = khachHang.tinh;
                            khachHangByEmail.huyen = khachHang.huyen;
                            khachHangByEmail.phuong = khachHang.phuong;
                            khachHangByEmail.taikhoan = khachHang.taikhoan;

                            // Lưu thông tin khách hàng đăng ký tạm thời ở Session
                            HttpContext.Session.SetString("matkhau", khachHangByEmail.matkhau);
                            HttpContext.Session.SetString("hovaten", khachHangByEmail.hovaten);
                            HttpContext.Session.SetString("sodienthoai", khachHangByEmail.sodienthoai);
                            HttpContext.Session.SetString("email", khachHangByEmail.email);
                            HttpContext.Session.SetString("tinh", khachHangByEmail.tinh);
                            HttpContext.Session.SetString("huyen", khachHangByEmail.huyen);
                            HttpContext.Session.SetString("phuong", khachHangByEmail.phuong);
                            HttpContext.Session.SetString("taikhoan", khachHangByEmail.taikhoan);

                            // Lưu mã xác nhận và gửi mã xác nhận qua email
                            xacMinhService.Create(xacMinh);
                            xacMinhService.GuiEmailXacMinh(xacMinh);

                            return Redirect("~/customer/khachhang/xacminh");

                        }
                        else
                        {
                            return Redirect("~/customer/khachhang/dangky");

                        }
                    }
                    else
                    {
                        if (khachHang.matkhau == checkmatkhau)
                        {
                            string mahoamatkhau = khachHangService.HashPassword(khachHang.matkhau);
                            khachHang.matkhau = mahoamatkhau;

                            // Lưu thông tin khách hàng đăng ký tạm thời ở Session
                            HttpContext.Session.SetString("hovaten", khachHang.hovaten);
                            HttpContext.Session.SetString("sodienthoai", khachHang.sodienthoai);
                            HttpContext.Session.SetString("email", khachHang.email);
                            HttpContext.Session.SetString("cccd", khachHang.cccd);
                            HttpContext.Session.SetString("tinh", khachHang.tinh);
                            HttpContext.Session.SetString("huyen", khachHang.huyen);
                            HttpContext.Session.SetString("phuong", khachHang.phuong);
                            HttpContext.Session.SetString("matkhau", khachHang.matkhau);
                            HttpContext.Session.SetString("taikhoan", khachHang.taikhoan);

                            // Lưu mã xác nhận và gửi mã xác nhận qua email
                            xacMinhService.Create(xacMinh);
                            xacMinhService.GuiEmailXacMinh(xacMinh);

                            return Redirect("~/customer/khachhang/xacminh");

                        }
                        else
                        {
                            return Redirect("~/customer/khachhang/dangky");
                        }
                    }
                }
            }
            return Redirect("~/customer/khachhang/dangky");
        }

        public IActionResult TaoDangKy(string maso)
        {
            XacMinh xacMinh = xacMinhService.GetBymaso(maso);
            if (xacMinh != null && xacMinh.maso == maso)
            {
                if (maso == xacMinh.maso)
                {
                    string taikhoan = HttpContext.Session.GetString("taikhoan");
                    // Kiểm tra sự tồn tại của tài khoản và email
                    KhachHang khachHangByEmail = khachHangService.GetKhachHangbyemail(xacMinh.email);
                    KhachHang khachHangByTaiKhoan = khachHangService.GetKhachHangTaiKhoan(taikhoan);
                    NhanVien nhanVienByTaiKhoan = nhanVienService.NhanVienByTaiKhoan(taikhoan);
                    // Kiểm tra nếu tài khoản và email chưa tồn tại
                    if (khachHangByTaiKhoan == null && nhanVienByTaiKhoan == null)
                    {
                        // Kiểm tra nếu tài khoản không trùng với tài khoản của khách hàng hiện tại hoặc tài khoản của nhân viên hiện tại
                        if (taikhoan != khachHangByEmail?.taikhoan && taikhoan != nhanVienByTaiKhoan?.taikhoan)
                        {
                            if (khachHangByEmail != null)
                            {
                                /// lấy thông tin khách hàng đã đăng ký được lưu ở session ra
                                khachHangByEmail.matkhau = HttpContext.Session.GetString("matkhau");
                                khachHangByEmail.hovaten = HttpContext.Session.GetString("hovaten");
                                khachHangByEmail.sodienthoai = HttpContext.Session.GetString("sodienthoai");
                                khachHangByEmail.email = HttpContext.Session.GetString("email");
                                khachHangByEmail.tinh = HttpContext.Session.GetString("tinh");
                                khachHangByEmail.huyen = HttpContext.Session.GetString("huyen");
                                khachHangByEmail.phuong = HttpContext.Session.GetString("phuong");
                                khachHangByEmail.taikhoan = HttpContext.Session.GetString("taikhoan");

                                khachHangService.CapNhatKhachHang(khachHangByEmail);
                                return Redirect("~/customer/dangnhap/dangnhap");
                            }
                            else
                            {
                                KhachHang khachHang = new KhachHang();
                                /// lấy thông tin khách hàng đã đăng ký được lưu ở session ra
                                khachHang.matkhau = HttpContext.Session.GetString("matkhau");
                                khachHang.hovaten = HttpContext.Session.GetString("hovaten");
                                khachHang.sodienthoai = HttpContext.Session.GetString("sodienthoai");
                                khachHang.email = HttpContext.Session.GetString("email");
                                khachHang.tinh = HttpContext.Session.GetString("tinh");
                                khachHang.huyen = HttpContext.Session.GetString("huyen");
                                khachHang.phuong = HttpContext.Session.GetString("phuong");
                                khachHang.taikhoan = HttpContext.Session.GetString("taikhoan");
                                khachHang.trangthai = "còn hoạt động";
                                khachHang.cccd = HttpContext.Session.GetString("cccd");

                                khachHangService.KhachHangDangKy(khachHang);
                                return Redirect("~/customer/dangnhap/dangnhap");

                            }
                        }
                    }
                }
                else
                {
                    return Redirect("~/customer/khachhang/xacminh");
                }
            }
            return Redirect("~/customer/khachhang/xacminh");
        }




        /*        public IActionResult TaoDangKy(KhachHang khachHang, string checkmatkhau)
                {
                    // Kiểm tra sự tồn tại của tài khoản và email
                    KhachHang khachHangByEmail = khachHangService.GetKhachHangbyemail(khachHang.email);
                    KhachHang khachHangByTaiKhoan = khachHangService.GetKhachHangTaiKhoan(khachHang.taikhoan);
                    NhanVien nhanVienByTaiKhoan = nhanVienService.NhanVienByTaiKhoan(khachHang.taikhoan);

                    // Kiểm tra nếu tài khoản và email chưa tồn tại
                    if (khachHangByTaiKhoan == null && nhanVienByTaiKhoan == null)
                    {
                        // Kiểm tra nếu tài khoản không trùng với tài khoản của khách hàng hiện tại hoặc tài khoản của nhân viên hiện tại
                        if (khachHang.taikhoan != khachHangByEmail?.taikhoan && khachHang.taikhoan != nhanVienByTaiKhoan?.taikhoan)
                        {
                            if (khachHangByEmail != null)
                            {
                                // Cập nhật thông tin khách hàng đã tồn tại
                                if (khachHang.matkhau == checkmatkhau)
                                {
                                    string mahoamatkhau = khachHangService.HashPassword(khachHang.matkhau);
                                    khachHangByEmail.matkhau = mahoamatkhau;
                                    khachHangByEmail.hovaten = khachHang.hovaten;
                                    khachHangByEmail.sodienthoai = khachHang.sodienthoai;
                                    khachHangByEmail.email = khachHang.email;
                                    khachHangByEmail.tinh = khachHang.tinh;
                                    khachHangByEmail.huyen = khachHang.huyen;
                                    khachHangByEmail.phuong = khachHang.phuong;
                                    khachHangByEmail.taikhoan = khachHang.taikhoan;
                                    khachHangService.CapNhatKhachHang(khachHangByEmail);
                                    return RedirectToAction("dangnhap", "dangnhap");
                                }
                                else
                                {
                                    return RedirectToAction("DangKy", "KhachHang");
                                }
                            }
                            else
                            {
                                if (khachHang.matkhau == checkmatkhau)
                                {
                                    string mahoamatkhau = khachHangService.HashPassword(khachHang.matkhau);
                                    khachHang.matkhau = mahoamatkhau;
                                    khachHang.trangthai = "còn hoạt động";
                                    khachHangService.KhachHangDangKy(khachHang);
                                    return RedirectToAction("dangnhap", "dangnhap");
                                }
                                else
                                {
                                    return RedirectToAction("DangKy", "KhachHang");
                                }
                            }
                        }
                    }

                    // Nếu tài khoản hoặc email đã tồn tại, chuyển hướng đến trang đăng ký
                    return RedirectToAction("DangKy", "KhachHang");
                }
        */
        /* public async Task<IActionResult> IndexAsync(int? sotrang)
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

                     string username = "11177515";
                     string password = "60-dayfreetrial";

                     // Credentials
                     string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

                     // Create an instance of HttpClient
                     using (HttpClient _httpClient = new HttpClient())
                     {
                         // Set the authentication header
                         _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                         // Make the GET request
                         HttpResponseMessage response = await _httpClient.GetAsync("http://chinhvovan13-001-site1.ftempurl.com/api/khachhang/index");
                         if (!response.IsSuccessStatusCode)
                         {
                             // Handle the failed API call
                             return StatusCode((int)response.StatusCode);
                         }

                         // Process the response if successful
                         string responseData = await response.Content.ReadAsStringAsync();
                         List<KhachHang> listKhachHang = JsonConvert.DeserializeObject<List<KhachHang>>(responseData);
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
         }*/
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
    }
}