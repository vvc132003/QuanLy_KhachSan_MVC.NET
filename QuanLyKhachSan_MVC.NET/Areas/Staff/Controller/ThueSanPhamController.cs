using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class ThueSanPhamController : Controller
    {
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly SanPhamService sanPhamService;


        public ThueSanPhamController(ThueSanPhamService thueSanPhamServices, SanPhamService sanPhamServices)
        {
            thueSanPhamService = thueSanPhamServices;
            sanPhamService = sanPhamServices;
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
                return Redirect($"/staff/thuephong/chitietthuephong?idphong={idphong}");

            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
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
                        return Redirect($"/staff/thuephong/chitietthuephong?idphong={idphong}");
                    }
                    else if (thueSanPham.soluong > 1)
                    {
                        sanpham.soluongcon += 1;
                        sanPhamService.CapNhatSanPham(sanpham);
                        thueSanPham.soluong -= 1;
                        thueSanPham.thanhtien = thueSanPham.soluong * sanpham.giaban;
                        thueSanPhamService.CapNhatThueSanPham(thueSanPham);
                        return Redirect($"/staff/thuephong/chitietthuephong?idphong={idphong}");
                    }
                }
                return Redirect($"/staff/thuephong/chitietthuephong?idphong={idphong}");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
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
                return Redirect($"/staff/thuephong/chitietthuephong?idphong={idphong}");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}