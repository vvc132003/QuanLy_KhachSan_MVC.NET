using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Newtonsoft.Json;
using PagedList;
using Service;
using Service.Service;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
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
        public IActionResult DangKy()
        {
            return View();
        }
        public IActionResult TaoDangKy(KhachHang khachHang, string checkmatkhau)
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
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}