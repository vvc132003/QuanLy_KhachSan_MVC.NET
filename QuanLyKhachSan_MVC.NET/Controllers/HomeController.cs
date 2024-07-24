using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Security.Claims;
using DocumentFormat.OpenXml.Office2010.Excel;
using Service.Service;
using Newtonsoft.Json;
using System.Globalization;
using System.Transactions;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly PhongService phongService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;
        private readonly LichSuThanhToanService lichSuThanhToanService;
        private readonly LikesService likesService;
        private readonly BinhLuanService binhLuanService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly DatPhongService datPhongService;
        private readonly HuyDatPhongService huyDatPhongService;
        private readonly ChuyenPhongService chuyenPhongService;
        private readonly MaGiamGiaService maGiamGiaService;
        public HomeController(PhongService phongServices,
            KhachSanService khachSanServices,
            KhachHangService khachHangService,
            LichSuThanhToanService lichSuThanhToanService,
            LikesService likesService,
            BinhLuanService binhLuanService,
            SanPhamService sanPhamService,
            ThueSanPhamService thueSanPhamService,
            DatPhongService datPhongService, HuyDatPhongService huyDatPhongService,
            ChuyenPhongService chuyenPhongService, MaGiamGiaService maGiamGiaService)
        {
            phongService = phongServices;
            khachSanService = khachSanServices;
            this.khachHangService = khachHangService;
            this.lichSuThanhToanService = lichSuThanhToanService;
            this.sanPhamService = sanPhamService;
            this.likesService = likesService;
            this.binhLuanService = binhLuanService;
            this.thueSanPhamService = thueSanPhamService;
            this.datPhongService = datPhongService;
            this.huyDatPhongService = huyDatPhongService;
            this.chuyenPhongService = chuyenPhongService;
            this.maGiamGiaService = maGiamGiaService;
        }


        public IActionResult Upimages()
        {
            return View();
        }
       
        public IActionResult DoanhThuTheoThang()
        {
            // Lấy danh sách các giao dịch từ cơ sở dữ liệu
            List<LichSuThanhToan> lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToan();
            float tongdoanhthutungthang = 0;
            List<Modeldata> modeldatas = new List<Modeldata>();
            foreach (var lichsuthanhtoan in lichSuThanhToans)
            {
                tongdoanhthutungthang += lichsuthanhtoan.sotienthanhtoan;
                Modeldata modeldata = new Modeldata()
                {
                    lichSuThanhToan = lichsuthanhtoan,
                    Tongdoanhthutungthang = tongdoanhthutungthang,

                };
                modeldatas.Add(modeldata);
            }
            return Json(modeldatas);

        }

/*        public IActionResult ThongKe()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null &&
               HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["idkhachsan"] = idkhachsan;
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<LichSuThanhToan> lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToan();
                float tongDoanhThu = lichSuThanhToans.Sum(lichSu => lichSu.sotienthanhtoan);
                List<KhachHang> khachHangs = khachHangService.GetAllKhachHang();
                int tongsoluongkhachhang = khachHangs.Count();
                List<Likes> likes = likesService.GetAllLikes();
                int tonglikes = likes.Count();
                List<BinhLuan> binhLuans = binhLuanService.GetAllBinhLuans();
                int tongbinhluan = binhLuans.Count();
                List<SanPham> sanPhams = sanPhamService.GetAllSanPham();
                int tongsanpham = sanPhams.Count();
                List<MaGiamGia> maGiamGias = maGiamGiaService.GetAllMaGiamGia();
                int tongmagiamgia = maGiamGias.Count();
                ViewData["tongDoanhThu"] = tongDoanhThu;
                ViewData["tongSoLuongKhachHang"] = tongsoluongkhachhang;
                ViewData["tonglikes"] = tonglikes;
                ViewData["tongbinhluan"] = tongbinhluan;
                ViewData["tongsanpham"] = tongsanpham;
                ViewData["tongmagiamgia"] = tongmagiamgia;
                List<Modeldata> modeldatalist = new List<Modeldata>();
                foreach (var sanpham in sanPhams)
                {
                    List<ThueSanPham> thueSanPhams = thueSanPhamService.GetAllThueSanPhamIDSanPham(sanpham.id);
                    int totalRentedQuantity = thueSanPhams.Sum(soluong => soluong.soluong);
                    KhachSan khachSan = khachSanService.GetKhachSanById(sanpham.idkhachsan);
                    Modeldata modeldata = new Modeldata()
                    {
                        sanPham = sanpham,
                        khachSan = khachSan,
                        TotalRentedQuantity = totalRentedQuantity,
                    };
                    modeldatalist.Add(modeldata);
                }
                List<KhachSan> khachSans = khachSanService.GetAllKhachSan();
                return View((modeldatalist, khachSans));
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
*/

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