using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ChuyenPhongController : Controller
    {
        private readonly ChuyenPhongService chuyenPhongService;
        private readonly DatPhongService datPhongService;
        private readonly PhongService phongService;


        public ChuyenPhongController(ChuyenPhongService chuyenPhongServices, DatPhongService datPhongServices, PhongService phongServices)
        {
            chuyenPhongService = chuyenPhongServices;
            datPhongService = datPhongServices;
            phongService = phongServices;
        }
        public IActionResult ChuyenPhong(ChuyenPhong chuyenPhong, int idphongcu, int idphongmoi)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                ///  cập nhật số phòng của id đặt phòng
                DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphongcu);
                datPhong.idphong = idphongmoi;
                datPhongService.UpdateDatPhong(datPhong);
                /// thêm chuyển phòng
                chuyenPhong.ngaychuyen = DateTime.Now;
                chuyenPhong.idnhanvien = idnv;
                chuyenPhong.idkhachhang = datPhong.idkhachhang;
                chuyenPhongService.ThemChuyenPhong(chuyenPhong);
                /// cap nhật tình trạng phòng cũ
                Phong phongcu = phongService.GetPhongID(idphongcu);
                phongcu.tinhtrangphong = "chưa dọn";
                phongService.CapNhatPhong(phongcu);
                /// cap nhật tình trạng phòng mới
                Phong phongmoi = phongService.GetPhongID(idphongmoi);
                phongmoi.tinhtrangphong = "có khách";
                phongService.CapNhatPhong(phongmoi);
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}