using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Staff.Controllers
{
    [Area("Staff")]
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
                Phong phongmoi = phongService.GetPhongID(idphongmoi);
                if (phongmoi.tinhtrangphong == "còn trống")
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
                    phongmoi.tinhtrangphong = "có khách";
                    phongService.CapNhatPhong(phongmoi);
                    TempData["chuyenphongthanhcong"] = "";
                }
                else
                {
                    Console.WriteLine("Không thể chuyển phòng");
                }
                return Redirect("~/staff/phong/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult DanhSachChuyenPhong(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idnv = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    ViewData["id"] = idnv;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<ChuyenPhong> listchuyenPhong = chuyenPhongService.GetAllChuyenPhongDescNgayChuyen();
                    int soluong = listchuyenPhong.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    PagedList.IPagedList<ChuyenPhong> ipagelistchuyePhong = listchuyenPhong.ToPagedList(validPageNumber, pageSize);
                    Modeldata yourModel = new Modeldata
                    {
                        PagedTChuyenPhong = ipagelistchuyePhong,
                    };
                    return View(yourModel);
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