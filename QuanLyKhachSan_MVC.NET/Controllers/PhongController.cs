using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Model.Models;
using Service;
using System.Collections.Generic;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class PhongController : Controller
    {
        private readonly PhongService phongService;
        private readonly TangService tangService;
        private readonly ThoiGianService thoiGianService;
        private readonly DatPhongService datPhongService;
        private readonly KhachHangService khachHangService;
        private readonly LichSuThanhToanService lichSuThanhToanService;
        public PhongController(PhongService phongServices, TangService tangServices, ThoiGianService thoiGianServices, DatPhongService datPhongServices, KhachHangService khachHangService, LichSuThanhToanService lichSuThanhToanService)
        {
            phongService = phongServices;
            tangService = tangServices;
            thoiGianService = thoiGianServices;
            datPhongService = datPhongServices;
            this.khachHangService = khachHangService;
            this.lichSuThanhToanService = lichSuThanhToanService;
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
        public IActionResult Home()
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
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult SoPhong(int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                KhachHang khachHang = khachHangService.GetKhachHangbyid(id);
                Phong phong = phongService.GetPhongID(idphong);
                Modeldata modeldata = new Modeldata
                {
                    phong = phong,
                    khachhang = khachHang,
                };
                return View(modeldata);
            }
            else
            {
                Phong phong = phongService.GetPhongID(idphong);
                Modeldata modeldata = new Modeldata
                {
                    phong = phong,
                };
                return View(modeldata);
            }
        }
    }
}