﻿using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ThueSanPhamController : Controller
    {
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly SanPhamService sanPhamService;


        public ThueSanPhamController(ThueSanPhamService thueSanPhamServices, SanPhamService sanPhamServices)
        {
            thueSanPhamService = thueSanPhamServices;
            sanPhamService = sanPhamServices;
        }
        public IActionResult ThueSanPham(ThueSanPham thueSanPham, int id, int iddatphong, int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnd = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnd;
                ViewData["hovaten"] = hovaten;
                SanPham sanpham = sanPhamService.GetSanPhamByID(id);
                thueSanPham.idnhanvien = idnd;
                thueSanPham.soluong = 1;
                thueSanPham.idsanpham = id;
                thueSanPham.iddatphong = iddatphong;
                ThueSanPham thueSanPhamididdatphong = thueSanPhamService.GetThueSanPhamByDatPhongAndSanPham(iddatphong, id);
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
                return RedirectToAction("ChiTietThuePhong", "ThuePhong", new { id = idphong });
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
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
                        return RedirectToAction("ChiTietThuePhong", "ThuePhong", new { id = idphong });
                    }
                    else if (thueSanPham.soluong > 1)
                    {
                        sanpham.soluongcon += 1;
                        sanPhamService.CapNhatSanPham(sanpham);
                        thueSanPham.soluong -= 1;
                        thueSanPham.thanhtien = thueSanPham.soluong * sanpham.giaban;
                        thueSanPhamService.CapNhatThueSanPham(thueSanPham);
                        return RedirectToAction("ChiTietThuePhong", "ThuePhong", new { id = idphong });
                    }
                }
                return RedirectToAction("ChiTietThuePhong", "ThuePhong", new { id = idphong });
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
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
                return RedirectToAction("ChiTietThuePhong", "ThuePhong", new { id = idphong });
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}