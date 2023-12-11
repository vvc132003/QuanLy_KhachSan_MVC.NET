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
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<Tang> tanglist = tangService.GetAllTang();
                ThoiGian thoiGian = thoiGianService.GetThoiGian(DateTime.Now);
                List<Modeldata> modeldataList = new List<Modeldata>();
                List<DatPhong> listdatphong = datPhongService.GetAllDatPhong();
                /*foreach (var datphong in listdatphong)
                {
                    if (datphong.ngaydat.Hour > thoiGian.thoigiannhanphong.Hour)
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
                    }
                }*/
                foreach (var tang in tanglist)
                {
                    List<Phong> phongs = phongService.GetAllPhongIDTang(tang.id);
                    List<Phong> phongtrangthai = phongService.GetAllPhongTrangThai();
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
    }
}