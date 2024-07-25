using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {
        private readonly SanPhamService sanPhamService;
        private readonly KhachSanService khachSanService;
        private readonly LoaiDichDichVuService loaiDichDichVuService;

        public SanPhamController(SanPhamService sanPhamServices, KhachSanService khachSanService, LoaiDichDichVuService loaiDichDichVuService)
        {
            sanPhamService = sanPhamServices;
            this.khachSanService = khachSanService;
            this.loaiDichDichVuService = loaiDichDichVuService;
        }
        public IActionResult DeleteSanPham([FromBody] List<int> idsanpham)
        {
            if (idsanpham != null && idsanpham.Any())
            {
                foreach (int id in idsanpham)
                {
                    SanPham sanPham = sanPhamService.GetSanPhamByID(id);
                    sanPham.trangthai = "hết bán";
                    sanPhamService.CapNhatSanPham(sanPham);
                };
                TempData["xoasanpham"] = "success";
                return Json(new { error = false });
            }
            else
            {
                return Json(new { error = true, message = "Vui lòng chọn sản phẩm để xoá!" });
            }
        }
        public IActionResult Khoiphucsanpham([FromBody] List<int> idsanpham)
        {
            if (idsanpham != null && idsanpham.Any())
            {
                foreach (int id in idsanpham)
                {
                    SanPham sanPham = sanPhamService.GetSanPhamByID(id);
                    sanPham.trangthai = "còn bán";
                    sanPhamService.CapNhatSanPham(sanPham);
                };
                TempData["khoiphuc"] = "success";
                return Json(new { error = false });
            }
            else
            {
                return Json(new { error = true, message = "Vui lòng chọn dịch vụ để khôi phục!" });
            }
        }
        public IActionResult DocVanBans()
        {
            return View();
        }

        public IActionResult Index(int? sotrang)
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
                    List<SanPham> sanphams = sanPhamService.GetAllSanPham();
                    int soluong = sanphams.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    PagedList.IPagedList<SanPham> ipagesanPhams = sanphams.ToPagedList(validPageNumber, pageSize);
                    List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var sanPham in ipagesanPhams)
                    {
                        if (sanPham.trangthai.Equals("còn bán"))
                        {
                            LoaiDichVu loaiDichVu = loaiDichDichVuService.GetLoaiDichVuById(sanPham.idloaidichvu);
                            KhachSan khachSan = khachSanService.GetKhachSanById(loaiDichVu.idkhachsan);
                            Modeldata modeldata = new Modeldata
                            {
                                PagedTSanPham = new List<SanPham> { sanPham }.ToPagedList(1, 1),
                                loaiDichVu = loaiDichVu,
                                khachSan = khachSan,
                            };
                            modeldatalist.Add(modeldata);
                        }
                    }
                    return View(new Tuple<List<Modeldata>, List<LoaiDichVu>>(modeldatalist, loaiDichVus));
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

        public IActionResult SanPhamdaxoa(int? sotrang)
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
                    List<SanPham> sanphams = sanPhamService.GetAllSanPham();
                    int soluong = sanphams.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    PagedList.IPagedList<SanPham> ipagesanPhams = sanphams.ToPagedList(validPageNumber, pageSize);
                    List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var sanPham in ipagesanPhams)
                    {
                        if (sanPham.trangthai.Equals("hết bán"))
                        {
                            LoaiDichVu loaiDichVu = loaiDichDichVuService.GetLoaiDichVuById(sanPham.idloaidichvu);
                            KhachSan khachSan = khachSanService.GetKhachSanById(loaiDichVu.idkhachsan);
                            Modeldata modeldata = new Modeldata
                            {
                                PagedTSanPham = new List<SanPham> { sanPham }.ToPagedList(1, 1),
                                loaiDichVu = loaiDichVu,
                                khachSan = khachSan,
                            };
                            modeldatalist.Add(modeldata);
                        }
                    }
                    return View(new Tuple<List<Modeldata>, List<LoaiDichVu>>(modeldatalist, loaiDichVus));
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
        public async Task<IActionResult> ThemSanPham(SanPham sanPham, IFormFile image)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;

                // Check file extension
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

                    sanPham.image = base64String; // Update this property name as needed
                    sanPham.trangthai = "còn bán";

                    // Add product
                    sanPhamService.ThemSanPham(sanPham);
                    return Redirect("~/admin/sanpham/");
                }
                catch (Exception ex)
                {
                    // Handle file processing exceptions
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
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
        /*        public async Task<IActionResult> ThemSanPham(SanPham sanPham, IFormFile image)
                {
                    if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
                    {
                        int id = HttpContext.Session.GetInt32("id").Value;
                        string hovaten = HttpContext.Session.GetString("hovaten");
                        ViewData["id"] = id;
                        ViewData["hovaten"] = hovaten;
                        // Check file extension
                        var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            return RedirectToAction("Index");
                        }
                        // Ensure unique file name
                        var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
                        // Create directory if it doesn't exist
                        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }
                        try
                        {
                            // Save the file
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle file saving exceptions
                            // Optionally log the error and return an appropriate view
                            return RedirectToAction("Error", "Home");
                        }
                        var relativePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");
                        sanPham.image = relativePath;
                        sanPham.trangthai = "còn bán";
                        // Add product
                        sanPhamService.ThemSanPham(sanPham);
                        return Redirect("~/admin/sanpham/");
                    }
                    else
                    {
                        return RedirectToAction("dangnhap", "dangnhap");
                    }
                }
        */       /* private bool DeleteImage(string imagePath)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);
                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            System.IO.File.Delete(path);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }
                    return false;
                }

                public async Task<IActionResult> XoaSanPham(int id)
                {
                    if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
                    {
                        SanPham sanPham = sanPhamService.GetSanPhamByID(id);
                        if (sanPham == null)
                        {
                            return NotFound();
                        }
                        if (!string.IsNullOrEmpty(sanPham.image))
                        {
                            DeleteImage(sanPham.image);
                        }
                        sanPhamService.XoaSanPham(id);

                        return Redirect("~/admin/sanpham/");
                    }
                    else
                    {
                        return RedirectToAction("dangnhap", "dangnhap");
                    }
                }*/


        public IActionResult UpdateSanPham(int? idsanpham)
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

                    return View();
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

        public IActionResult XoaSanPham(int id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnd = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnd;
                ViewData["hovaten"] = hovaten;
                sanPhamService.XoaSanPham(id);
                return Redirect("~/admin/sanpham/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
    }
}
