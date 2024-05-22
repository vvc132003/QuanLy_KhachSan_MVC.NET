using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;

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
                huyDatPhong.lydo = "";
                huyDatPhong.ngayhuy = DateTime.Now;
                huyDatPhong.iddatphong = datphong.id;
                huyDatPhong.idnhanvien = idnv;
                huyDatPhongService.ThemHuyDatPhong(huyDatPhong);
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult DanhSachHuyDatPhong(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<HuyDatPhong> listHuyDatPhong  = huyDatPhongService.GetAllHuyDatPhongDescNgayHuy();
                int soluong = listHuyDatPhong.Count;
                int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                PagedList.IPagedList<HuyDatPhong> ipagelisHuyDatPhong = listHuyDatPhong.ToPagedList(validPageNumber, pageSize);
                Modeldata yourModel = new Modeldata
                {
                    PagedTHuyDatPhong = ipagelisHuyDatPhong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}
