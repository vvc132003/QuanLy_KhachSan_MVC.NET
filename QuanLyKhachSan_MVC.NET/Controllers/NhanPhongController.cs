﻿using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class NhanPhongController : Controller
    {
        private readonly DatPhongService datPhongService;
        private readonly KhachHangService khachHangService;
        private readonly PhongService phongService;
        private readonly NhanPhongService nhanPhongService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly ThoiGianService thoiGianService;
        private readonly GiamGiaService giamGiaService;
        private readonly QuyDinhGiamGiaService quyDinhGiamGiaservice;


        public NhanPhongController(DatPhongService datPhongServices,
            KhachHangService khachHangServices,
            PhongService phongServices,
            NhanPhongService nhanPhongServices,
            SanPhamService sanPhamServices,
            ThueSanPhamService thueSanPhamServices,
            ThoiGianService thoiGianServices,
            GiamGiaService giamGiaServices,
            QuyDinhGiamGiaService quydinhGiamGiaServices)
        {
            datPhongService = datPhongServices;
            khachHangService = khachHangServices;
            phongService = phongServices;
            nhanPhongService = nhanPhongServices;
            sanPhamService = sanPhamServices;
            thueSanPhamService = thueSanPhamServices;
            thoiGianService = thoiGianServices;
            giamGiaService = giamGiaServices;
            quyDinhGiamGiaservice = quydinhGiamGiaServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NhanPhongOnline(int id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThaiOnline(id);
                // Inside your controller action
                List<Modeldata> yourModelList = new List<Modeldata>();

                if (datPhong != null)
                {
                    Modeldata yourModel = new Modeldata
                    {
                        datPhong = datPhong,
                    };
                    yourModelList.Add(yourModel);
                }
                return View(yourModelList);

            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult CheckInNhanPhongOnline(NhanPhong nhanPhong, int idphong, string cccd)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThaiOnline(idphong);
                if (datPhong != null && datPhong.cccd == cccd)
                {
                    nhanPhong.idnhanvien = idnv;
                    nhanPhong.iddatphong = datPhong.id;
                    nhanPhong.ngaynhanphong = DateTime.Now;
                    nhanPhongService.ThemNhanPhong(nhanPhong);
                    Phong phong = phongService.GetPhongID(idphong);
                    phong.tinhtrangphong = "có khách";
                    phongService.CapNhatPhong(phong);
                    datPhong.trangthai = "đã đặt";
                    datPhongService.UpdateDatPhong(datPhong);
                    return RedirectToAction("Index", "Phong");
                }
                else
                {
                    return RedirectToAction("NhanPhongOnline", "NhanPhong", new { id = idphong });
                }
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}
