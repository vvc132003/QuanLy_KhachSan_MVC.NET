using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Model.Models;
using Service;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhongController : Controller
    {
        private readonly PhongService phongService;
        private readonly TangService tangService;
        private readonly ThoiGianService thoiGianService;
        private readonly DatPhongService datPhongService;
        private readonly KhachHangService khachHangService;
        private readonly LichSuThanhToanService lichSuThanhToanService;
        private readonly LikesService likesService;
        private readonly BinhLuanService binhLuanService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly KhachSanService khachSanService;
        public PhongController(PhongService phongServices,
            TangService tangServices, ThoiGianService thoiGianServices,
            DatPhongService datPhongServices, KhachHangService khachHangService,
            LichSuThanhToanService lichSuThanhToanService,
            LikesService likesService, BinhLuanService binhLuanService, SanPhamService sanPhamService,
            ThueSanPhamService thueSanPhamService,
            KhachSanService khachSanService)
        {
            phongService = phongServices;
            tangService = tangServices;
            thoiGianService = thoiGianServices;
            datPhongService = datPhongServices;
            this.khachHangService = khachHangService;
            this.lichSuThanhToanService = lichSuThanhToanService;
            this.likesService = likesService;
            this.binhLuanService = binhLuanService;
            this.sanPhamService = sanPhamService;
            this.thueSanPhamService = thueSanPhamService;
            this.khachSanService = khachSanService;
        }
        public IActionResult Index(string loaiphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["idkhachsan"] = idkhachsan;
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;


                List<Phong> soluongphongtrangthai = phongService.GetAllPhongByIdKhachSan(idkhachsan);
                foreach (var phong in soluongphongtrangthai)
                {
                    if (phong.tinhtrangphong.Equals("còn trống"))
                    {
                        int slphongtrong = soluongphongtrangthai.Count;
                        ViewData["slphongtrong"] = slphongtrong;
                    }
                    else if (phong.tinhtrangphong.Equals("đã đặt"))
                    {
                        int slphongdadat = soluongphongtrangthai.Count;
                        ViewData["slphongdadat"] = slphongdadat;
                    }
                    else if (phong.tinhtrangphong.Equals("có khách"))
                    {
                        int slphongcoskhach = soluongphongtrangthai.Count;
                        ViewData["slphongcoskhach"] = slphongcoskhach;
                    }
                    else if (phong.tinhtrangphong.Equals("đang sửa chữa"))
                    {
                        int slphongsuachua = soluongphongtrangthai.Count;
                        ViewData["slphongsuachua"] = slphongsuachua;
                    }
                    else if (phong.tinhtrangphong.Equals("chưa dọn"))
                    {
                        int slphongchuadon = soluongphongtrangthai.Count;
                        ViewData["slphongchuadon"] = slphongchuadon;
                    }
                }
                if (loaiphong == null)
                {
                    List<Tang> tanglist = tangService.GetAllTangkhachsanid(idkhachsan);
                    List<Modeldata> modeldataList = new List<Modeldata>();
                    foreach (var tang in tanglist)
                    {
                        List<Phong> phongs = phongService.GetAllPhongIDTang(tang.id, idkhachsan);
                        List<Phong> phongtrangthai = phongService.GetAllPhongTrangThai(idkhachsan);
                        Modeldata modeldata = new Modeldata
                        {
                            tang = tang,
                            listphong = phongs,
                            listphongtrangthai = phongtrangthai,
                        };
                        modeldataList.Add(modeldata);
                    }
                    return View(modeldataList);
                }
                else
                {
                    Tang tang = tangService.GetTangidkhachsan(idkhachsan);
                    List<Modeldata> modeldataList = new List<Modeldata>();
                    List<Phong> listphong = phongService.GetAllPhongSoPhong(loaiphong, idkhachsan);
                    List<Phong> phongtrangthai = phongService.GetAllPhongTrangThai(idkhachsan);
                    Modeldata modeldata = new Modeldata
                    {
                        tang = tang,
                        listphong = listphong,
                        listphongtrangthai = phongtrangthai,
                    };
                    modeldataList.Add(modeldata);
                    return View(modeldataList);
                }

            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult KhachHangbyiddatPhong(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            if (datPhong == null)
            {
                return Ok();
            }
            KhachHang khachHang = khachHangService.GetKhachHangbyid(datPhong.idkhachhang);
            if (khachHang == null)
            {
                return Ok();
            }
            return Json(new { khachHang });
        }
        public IActionResult Home()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null &&
                HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int id = HttpContext.Session.GetInt32("id").Value;
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["idkhachsan"] = idkhachsan;
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<LichSuThanhToan> lichsuthanhtoanlisst = lichSuThanhToanService.GetLichSuThanhToan();
                    List<float> doanhThuData = lichsuthanhtoanlisst.Select(item => item.sotienthanhtoan).ToList();
                    List<int> months = lichsuthanhtoanlisst.Select(item => item.ngaythanhtoan.Month).ToList();
                    ViewData["months"] = months;
                    ViewData["doanhThuData"] = doanhThuData;
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var lichSuThanhToan in lichsuthanhtoanlisst)
                    {
                        Modeldata modeldata = new Modeldata
                        {
                            lichSuThanhToan = lichSuThanhToan,
                        };
                        modeldatalist.Add(modeldata);
                    }
                    return View(modeldatalist);
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

        public IActionResult Home1()
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
                ViewData["tongDoanhThu"] = tongDoanhThu;
                ViewData["tongSoLuongKhachHang"] = tongsoluongkhachhang;
                ViewData["tonglikes"] = tonglikes;
                ViewData["tongbinhluan"] = tongbinhluan;
                ViewData["tongsanpham"] = tongsanpham;

                return View();
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }



        public IActionResult CapNhatTrangThaiPhong(int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                Phong phong = phongService.GetPhongID(idphong);
                if (phong.tinhtrangphong == "chưa dọn")
                {
                    phong.tinhtrangphong = "còn trống";
                    phongService.CapNhatPhong(phong);
                }
                else if (phong.tinhtrangphong == "còn trống")
                {
                    phong.tinhtrangphong = "đang sửa chữa";
                    phongService.CapNhatPhong(phong);
                }
                else if (phong.tinhtrangphong == "đang sửa chữa")
                {
                    phong.tinhtrangphong = "chưa dọn";
                    phongService.CapNhatPhong(phong);
                }
                else
                {
                    phong.tinhtrangphong = "đang sửa chữa";
                    phongService.CapNhatPhong(phong);
                }
                return Redirect("~/admin/phong/");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult ThemPhong(int soluongmuonthem, Phong phong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                for (int i = 0; i < soluongmuonthem; i++)
                {
                    phong.tinhtrangphong = "còn trống";
                    phongService.ThemPhong(phong);
                }
                return Redirect("~/admin/phong/");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public async Task<IActionResult> DatPhongOnline(int idphong)
        {
            Phong phong = phongService.GetPhongID(idphong);
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                KhachHang khachHang = khachHangService.GetKhachHangbyid(id);
                Modeldata modeldata = new Modeldata
                {
                    phong = phong,
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
                        phong = phong,
                        userProfileViewModel = userProfileViewModel,
                    };
                    return View(modeldata);
                }
                else
                {
                    Modeldata modeldata = new Modeldata
                    {
                        phong = phong,
                    };
                    return View(modeldata);
                }
            }
        }
    }
}