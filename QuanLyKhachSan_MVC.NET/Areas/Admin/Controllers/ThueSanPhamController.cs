using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Org.BouncyCastle.Crypto;
using Service;
using Service.Service;
using System.Text.Json;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThueSanPhamController : Controller
    {
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly SanPhamService sanPhamService;
        private readonly DatPhongService datPhongService;
        private readonly PhongService phongService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ThueSanPhamController(ThueSanPhamService thueSanPhamServices, SanPhamService sanPhamServices,
            DatPhongService datPhongService, PhongService phongService,
            IHttpContextAccessor httpContextAccessor)
        {
            thueSanPhamService = thueSanPhamServices;
            sanPhamService = sanPhamServices;
            this.datPhongService = datPhongService;
            this.phongService = phongService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult ThueSanPham(ThueSanPham thueSanPham, int idsp, int iddatphong, int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                SanPham sanpham = sanPhamService.GetSanPhamByID(idsp);
                thueSanPham.idnhanvien = idnv;
                thueSanPham.soluong = 1;
                thueSanPham.idsanpham = idsp;
                thueSanPham.iddatphong = iddatphong;
                ThueSanPham thueSanPhamididdatphong = thueSanPhamService.GetThueSanPhamByDatPhongAndSanPham(iddatphong, idsp);
                if (thueSanPhamididdatphong != null)
                {
                    if (sanpham.soluongcon > 1)
                    {
                        sanpham.soluongcon -= 1;
                        sanPhamService.CapNhatSanPham(sanpham);
                        thueSanPhamididdatphong.soluong += 1;
                        thueSanPhamididdatphong.thanhtien += 1 * sanpham.giaban;
                        thueSanPhamService.CapNhatThueSanPham(thueSanPhamididdatphong);
                    }
                    else
                    {
                        Console.WriteLine("Sản phẩm này đã hết");
                    }
                }
                else
                {
                    if (sanpham.soluongcon > 1)
                    {
                        sanpham.soluongcon -= 1;
                        sanPhamService.CapNhatSanPham(sanpham);
                        thueSanPham.thanhtien = 1 * sanpham.giaban;
                        thueSanPhamService.ThueSanPham(thueSanPham);
                    }
                    else
                    {
                        Console.WriteLine("Sản phẩm này đã hết");
                    }
                }
                return Redirect($"/admin/thuephong/chitietthuephong?idphong={idphong}");

            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult XoasoluongThueSanPham(int id, int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnd = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnd;
                ViewData["hovaten"] = hovaten;
                ThueSanPham thueSanPham = thueSanPhamService.GetThueSanPhamBYID(id);
                SanPham sanpham = sanPhamService.GetSanPhamByID(thueSanPham.idsanpham);
                if (thueSanPham.soluong >= 1)
                {
                    if (thueSanPham.soluong == 1)
                    {
                        sanpham.soluongcon += 1;
                        sanPhamService.CapNhatSanPham(sanpham);
                        thueSanPhamService.XoaThueSanPham(id);
                        return Redirect($"/admin/thuephong/chitietthuephong?idphong={idphong}");
                    }
                    else if (thueSanPham.soluong > 1)
                    {
                        sanpham.soluongcon += 1;
                        sanPhamService.CapNhatSanPham(sanpham);
                        thueSanPham.soluong -= 1;
                        thueSanPham.thanhtien = thueSanPham.soluong * sanpham.giaban;
                        thueSanPhamService.CapNhatThueSanPham(thueSanPham);
                        return Redirect($"/admin/thuephong/chitietthuephong?idphong={idphong}");
                    }
                }
                return Redirect($"/admin/thuephong/chitietthuephong?idphong={idphong}");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult UpdatesoluongThueSanPham(int id, int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnd = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnd;
                ViewData["hovaten"] = hovaten;
                ThueSanPham thueSanPham = thueSanPhamService.GetThueSanPhamBYID(id);
                SanPham sanpham = sanPhamService.GetSanPhamByID(thueSanPham.idsanpham);
                if (sanpham.soluongcon > 1)
                {
                    sanpham.soluongcon -= 1;
                    sanPhamService.CapNhatSanPham(sanpham);
                    thueSanPham.soluong += 1;
                    thueSanPham.thanhtien = thueSanPham.soluong * sanpham.giaban;
                    thueSanPhamService.CapNhatThueSanPham(thueSanPham);
                }
                else
                {
                    Console.WriteLine("Sản phẩm này đã hết");
                }
                return Redirect($"/admin/thuephong/chitietthuephong?idphong={idphong}");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult Danhsachsanphamthueiddatphong(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            List<ThueSanPham> listthueSanPham = thueSanPhamService.GetAllThueSanPhamID(datPhong.id);
            return Json(listthueSanPham);
        }
        [HttpGet]
        public IActionResult GetCartItemsJson()
        {
            List<ThueSanPham> cartItems = GetCartItems();
            return Json(cartItems);
        }
        private List<ThueSanPham> GetCartItems()
        {
            var cartItemsJson = _httpContextAccessor.HttpContext.Session.GetString("examplesss");

            List<ThueSanPham> cartItemList = new List<ThueSanPham>();

            if (!string.IsNullOrEmpty(cartItemsJson))
            {
                cartItemList = JsonSerializer.Deserialize<List<ThueSanPham>>(cartItemsJson);
            }
            return cartItemList;
        }
        private void SaveCartItems(List<ThueSanPham> cartItems)
        {
            var serializedCartItems = JsonSerializer.Serialize(cartItems);
            _httpContextAccessor.HttpContext.Session.SetString("examplesss", serializedCartItems);
        }

        public IActionResult TachDon(int id, int idphong, int soluong, float thanhtien, string tensanpham, string image, int iddatphong, int idsanpham)
        {
            List<ThueSanPham> cartItems = GetCartItems();
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            Phong phong = phongService.GetPhongID(idphong);
            ThueSanPham thueSanPham = cartItems.FirstOrDefault(item => item.id == id && item.iddatphong == datPhong.id);
            SanPham sanPham = sanPhamService.GetSanPhamByID(idsanpham);
            List<ThueSanPham> checkslthuesanpham = thueSanPhamService.GetThueSanPhamByIDdatphong(iddatphong);
            /// chekc sản phẩm với số lượng cối cùng
            if (checkslthuesanpham.Count == 1 && checkslthuesanpham.First().soluong == 1)
            {
                return Json(new { success = false, messages = "Sản phẩm cuối cùng không thể tách." });
            }
            if (thueSanPham != null)
            {
                int previousQuantity = thueSanPham.soluong;
                thueSanPham.soluong += soluong;
                if (previousQuantity != thueSanPham.soluong)
                {
                    thueSanPham.thanhtien = thueSanPham.soluong * sanPham.giaban;
                    thueSanPham.ghichu = $"Được tách từ phòng: {phong.sophong}, số lượng: {soluong + 1}";
                }
            }
            else
            {
                cartItems.Add(new ThueSanPham
                {
                    id = id,
                    tensanpham = tensanpham,
                    thanhtien = soluong * sanPham.giaban,
                    soluong = soluong,
                    image = image,
                    iddatphong = iddatphong,
                    idsanpham = idsanpham,
                    ghichu = $"Được tách từ phòng: {phong.sophong}, số lượng: {soluong}",
                });
            }
            SaveCartItems(cartItems);
            ThueSanPham truSoLuongThueSanPham = thueSanPhamService.GetThueSanPhamBYID(id);
            if (truSoLuongThueSanPham != null && sanPham != null)
            {
                if (truSoLuongThueSanPham.soluong == 1)
                {
                    thueSanPhamService.XoaThueSanPham(truSoLuongThueSanPham.id);
                }
                else
                {
                    truSoLuongThueSanPham.soluong -= 1;
                    truSoLuongThueSanPham.thanhtien = truSoLuongThueSanPham.soluong * sanPham.giaban;
                    thueSanPhamService.CapNhatThueSanPham(truSoLuongThueSanPham);
                }
            }
            else
            {
                return Json(new { success = false, message = "Sản phẩm hoặc thuê sản phẩm không tồn tại." });
            }
            return Json(new { success = true, message = "Tách thành công!" });
        }
        public IActionResult HuyTach()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<ThueSanPham> cartItems = GetCartItems();
                if (cartItems != null)
                {
                    foreach (var cartItem in cartItems)
                    {
                        ThueSanPham thueSanPham = thueSanPhamService.GetThueSanPhamByDatPhongAndSanPham(cartItem.iddatphong, cartItem.idsanpham);
                        SanPham sanpham = sanPhamService.GetSanPhamByID(cartItem.idsanpham);
                        if (thueSanPham != null)
                        {
                            thueSanPham.soluong = cartItem.soluong + thueSanPham.soluong;
                            thueSanPham.thanhtien = cartItem.thanhtien + thueSanPham.thanhtien;
                            thueSanPhamService.CapNhatThueSanPham(thueSanPham);
                        }
                        else
                        {
                            ThueSanPham addthuesanpham = new ThueSanPham();
                            addthuesanpham.idnhanvien = idnv;
                            addthuesanpham.soluong = 1;
                            addthuesanpham.idsanpham = sanpham.id;
                            addthuesanpham.iddatphong = cartItem.iddatphong;
                            addthuesanpham.thanhtien = addthuesanpham.soluong * sanpham.giaban;
                            thueSanPhamService.ThueSanPham(addthuesanpham);
                        }
                    }
                    _httpContextAccessor.HttpContext.Session.Remove("examplesss");
                    return Json(new { success = true, message = "Huỷ tách thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không có dịch vụ để huỷ!" });
                }
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
    }
}