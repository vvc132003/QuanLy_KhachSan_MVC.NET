using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Model.Models;
using Service;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class PhongController : Controller
    {
        private readonly PhongService phongService;
        private readonly TangService tangService;
        public PhongController(PhongService phongServices, TangService tangServices)

        {
            phongService = phongServices;
            tangService = tangServices;

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

                int slphongtrong = 0;
                int slphongdadat = 0;
                int slphongcoskhach = 0;
                int slphongsuachua = 0;
                int slphongchuadon = 0;

                foreach (var phong in soluongphongtrangthai)
                {
                    if (phong.tinhtrangphong.Equals("còn trống"))
                    {
                        slphongtrong++;
                    }
                    else if (phong.tinhtrangphong.Equals("đã đặt"))
                    {
                        slphongdadat++;
                    }
                    else if (phong.tinhtrangphong.Equals("có khách"))
                    {
                        slphongcoskhach++;
                    }
                    else if (phong.tinhtrangphong.Equals("đang sửa chữa"))
                    {
                        slphongsuachua++;
                    }
                    else if (phong.tinhtrangphong.Equals("chưa dọn"))
                    {
                        slphongchuadon++;
                    }
                }

                ViewData["slphongtrong"] = slphongtrong;
                ViewData["slphongdadat"] = slphongdadat;
                ViewData["slphongcoskhach"] = slphongcoskhach;
                ViewData["slphongsuachua"] = slphongsuachua;
                ViewData["slphongchuadon"] = slphongchuadon;

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
                return Redirect("~/customer/dangnhap/dangnhap");
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
                return Redirect("~/staff/phong/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult ThemPhong(int soluongmuonthem, Phong phong)
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
                    for (int i = 0; i < soluongmuonthem; i++)
                    {
                        phong.tinhtrangphong = "còn trống";
                        phongService.ThemPhong(phong);
                    }
                    return Redirect("~/staff/phong/");
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
    }
}