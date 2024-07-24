using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Collections.Generic;

using System.Drawing;
using PagedList;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanVienController : Controller
    {
        private readonly NhanVienService nhanVienService;
        private readonly ChucVuService chucVuService;

        private readonly HopDongLaoDongService hopDongLaoDongService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;
        public NhanVienController(NhanVienService nhanVienServices,
            ChucVuService chucVuServices,

            HopDongLaoDongService hopDongLaoDongServices,
            KhachSanService khachSanServices,
            KhachHangService khachHangService)
        {
            nhanVienService = nhanVienServices;
            chucVuService = chucVuServices;

            hopDongLaoDongService = hopDongLaoDongServices;
            khachSanService = khachSanServices;
            this.khachHangService = khachHangService;
        }
        public IActionResult Index(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("tenchucvu") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<NhanVien> nhanViens = nhanVienService.GetAllNhanVien();
                    int soluong = nhanViens.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    IPagedList<NhanVien> ipagenhanViens = nhanViens.ToPagedList(validPageNumber, pageSize);
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var nhanvien in ipagenhanViens)
                    {
                        KhachSan khachSan = khachSanService.GetKhachSanById(nhanvien.idkhachsan);
                        ChucVu chucVu = chucVuService.GetChucVuID(nhanvien.idchucvu);
                        Modeldata modeldata = new Modeldata
                        {
                            PagedTNhanVien = new List<NhanVien> { nhanvien }.ToPagedList(1, 1),
                            khachSan = khachSan,
                            chucVu = chucVu,
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
        /*  public IActionResult Index(int? sotrang)
          {
              if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("tenchucvu") != null)
              {
                  if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                  {
                      int id = HttpContext.Session.GetInt32("id").Value;
                      string hovaten = HttpContext.Session.GetString("hovaten");
                      string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                      ViewData["id"] = id;
                      ViewData["hovaten"] = hovaten;
                      ViewData["tenchucvu"] = tenchucvu;
                      List<NhanVien> nhanViens = nhanVienService.GetAllNhanVien();
                      int soluong = nhanViens.Count;
                      int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                      int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                      IPagedList<NhanVien> ipagenhanViens = nhanViens.ToPagedList(validPageNumber, pageSize);

                      Modeldata modeldata = new Modeldata
                      {
                          PagedTNhanVien = ipagenhanViens,

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
          }*/
        public IActionResult AddNhanVien()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    ViewData["idkhachsan"] = idkhachsan;
                    List<ChucVu> chucVus = chucVuService.GetAllChucVu();
                    List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
                    Modeldata modeldata = new Modeldata
                    {
                        listchucVu = chucVus,
                        listKhachSan = listkhachsan
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
        private async Task<string> ConvertToBase64StringAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            var base64String = Convert.ToBase64String(fileBytes);
            return $"data:{file.ContentType};base64,{base64String}";
        }
        public async Task<IActionResult> AddNhanVienn(NhanVien nhanVien, HopDongLaoDong hopDongLaoDong, IFormFile image)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                string mahoamatkhau = khachHangService.HashPassword(nhanVien.matkhau);
                nhanVien.matkhau = mahoamatkhau;
                nhanVien.solanvipham = 0;
                nhanVien.trangthai = "Đang hoạt động";
                var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return RedirectToAction("Index");
                }
                try
                {
                    // Convert image to base64 string
                    var base64String = await ConvertToBase64StringAsync(image);
                    nhanVien.image = base64String;
                    int idnhanvien = nhanVienService.ThemNhanVien(nhanVien);
                    hopDongLaoDong.idnhanvien = idnhanvien;
                    hopDongLaoDong.ngaybatdau = DateTime.Now;
                    hopDongLaoDongService.ThemHopDongLaoDong(hopDongLaoDong);
                    return Redirect("~/admin/nhanvien/");
                }
                catch (Exception ex)
                {
                    // Handle file processing exceptions
                    return RedirectToAction("Error", "Home");
                }
               
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }



        public IActionResult UpdateNhanVien(int idnhanvien)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    ViewData["idkhachsan"] = idkhachsan;
                    List<ChucVu> chucVus = chucVuService.GetAllChucVu();
                    List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
                    NhanVien nhanVien = nhanVienService.GetNhanVienID(idnhanvien);
                    Modeldata modeldata = new Modeldata
                    {
                        listchucVu = chucVus,
                        listKhachSan = listkhachsan,
                        nhanVien = nhanVien
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


        public IActionResult UpdateNhanVienn(NhanVien nhanVien)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                nhanVienService.CapNhatNhanVien(nhanVien);
                return Redirect("~/admin/nhanvien/");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }

        public IActionResult XuatEclcel()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                nhanVienService.Xuatexcel();
                return Redirect("~/admin/nhanvien/");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public ActionResult Index1()
        {
            return View();
        }
        
        public IActionResult Checkcccdnhanvien(string cccd)
        {
            NhanVien nhanVien = nhanVienService.Checkcccdnhanvien(cccd);
            return Json(new { cccdcheck = nhanVien });
        }
    }
}