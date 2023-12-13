﻿using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;
using System.Collections.Generic;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class PhongController : Controller
    {
        private readonly PhongService phongService;
        private readonly TangService tangService;
        private readonly ThoiGianService thoiGianService;
        private readonly DatPhongService datPhongService;

        public PhongController(PhongService phongServices, TangService tangServices, ThoiGianService thoiGianServices, DatPhongService datPhongServices)
        {
            phongService = phongServices;
            tangService = tangServices;
            thoiGianService = thoiGianServices;
            datPhongService = datPhongServices;
        }
        public IActionResult Index()
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
                List<Tang> tanglist = tangService.GetAllTang(idkhachsan);
                ThoiGian thoiGian = thoiGianService.GetThoiGian(DateTime.Now, idkhachsan);
                List<Modeldata> modeldataList = new List<Modeldata>();
                List<DatPhong> listdatphong = datPhongService.GetAllDatPhong();
                foreach (var datphong in listdatphong)
                {
                    if (DateTime.Now.Hour > thoiGian.thoigiannhanphong.Hour)
                    {
                        datphong.trangthai = "đã huỷ";
                        datPhongService.UpdateDatPhong(datphong);
                        Phong phong = new Phong();
                        phong.id = datphong.idphong;
                        phong.tinhtrangphong = "còn trống";
                        phongService.CapNhatPhong(phong);
                    }
                    else
                    {
                        //không huỷ đặt phòng
                    }
                }
                foreach (var tang in tanglist)
                {
                    List<Phong> phongs = phongService.GetAllPhongIDTang(tang.id, idkhachsan);
                    List<Phong> phongtrangthai = phongService.GetAllPhongTrangThai(idkhachsan);
                    Modeldata modeldata = new Modeldata
                    {
                        tang = tang,
                        listphong = phongs,
                        listphongtrangthai = phongtrangthai,
                        thoigian = thoiGian,
                    };
                    modeldataList.Add(modeldata);
                }
                return View(modeldataList);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
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
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThemPhong(int soluongmuonthem, Phong phong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                for (int i = 0; i < soluongmuonthem; i++)
                {
                    phong.loaiphong = "VIP";
                    phong.tinhtrangphong = "còn trống";
                    phongService.ThemPhong(phong);
                }
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}