using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class HuyDatPhongController : Controller
    {
        private readonly HuyDatPhongService huyDatPhongService;
        private readonly PhongService phongService;
        private readonly DatPhongService datPhongService;
        public HuyDatPhongController(HuyDatPhongService huyDatPhongServices, PhongService phongServices, DatPhongService datPhongServices)
        {
            huyDatPhongService = huyDatPhongServices;
            phongService = phongServices;
            datPhongService = datPhongServices;
        }
        public IActionResult HuyDatPhong(int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                DatPhong datphong = datPhongService.GetDatPhongByIDTrangThaiOnline(idphong);
                datphong.trangthai = "đã huỷ";
                datPhongService.UpdateDatPhong(datphong);
                Phong phong = phongService.GetPhongID(idphong);
                phong.tinhtrangphong = "còn trống";
                phongService.CapNhatPhong(phong);
                HuyDatPhong huyDatPhong = new HuyDatPhong();
                huyDatPhong.lydo = "null";
                huyDatPhong.ngayhuy = DateTime.Now;
                huyDatPhong.iddatphong = datphong.id;
                huyDatPhong.idnhanvien = idnv;
                huyDatPhongService.ThemHuyDatPhong(huyDatPhong);
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }
    }
}
