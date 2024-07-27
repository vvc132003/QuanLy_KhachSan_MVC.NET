using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                huyDatPhong.sotienhoanlai = datphong.tiendatcoc * huyDatPhong.sotienphaitra / 100;
                huyDatPhong.lydo = "";
                huyDatPhong.ngayhuy = DateTime.Now;
                huyDatPhong.iddatphong = datphong.id;
                huyDatPhong.idnhanvien = idnv;
                huyDatPhongService.ThemHuyDatPhong(huyDatPhong);
                return Redirect("~/admin/phong/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }

        public IActionResult HuyPhongbuyid(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThaiOnline(idphong);
            return Json(datPhong);
        }

        public IActionResult DanhSachHuyDatPhong(int? sotrang)
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
                    List<HuyDatPhong> listHuyDatPhong = huyDatPhongService.GetAllHuyDatPhongDescNgayHuy();
                    int soluong = listHuyDatPhong.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    PagedList.IPagedList<HuyDatPhong> ipagelisHuyDatPhong = listHuyDatPhong.ToPagedList(validPageNumber, pageSize);
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var huydatphong in ipagelisHuyDatPhong)
                    {
                        DatPhong datPhong = datPhongService.GetDatPhongByIDDatPhong(huydatphong.iddatphong);
                        Modeldata yourModel = new Modeldata
                        {
                            PagedTHuyDatPhong = new List<HuyDatPhong> { huydatphong }.ToPagedList(1, 1),
                            datPhong = datPhong,
                        };
                        modeldatalist.Add(yourModel);
                    }
                    return View(modeldatalist);
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
