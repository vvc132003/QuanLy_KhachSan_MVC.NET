using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Staff.Controllers
{
    [Area("Staff")]
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
        public IActionResult HuyDatPhong(HuyDatPhong huyDatPhong, int idphong)
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
                huyDatPhong.sotienphaitra = 50;
                huyDatPhong.sotienhoanlai = datphong.tiendatcoc * huyDatPhong.sotienphaitra / 100;
                huyDatPhong.lydo = "";
                huyDatPhong.ngayhuy = DateTime.Now;
                huyDatPhong.iddatphong = datphong.id;
                huyDatPhong.idnhanvien = idnv;
                huyDatPhongService.ThemHuyDatPhong(huyDatPhong);
                return Redirect("~/staff/phong/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
    }
}