using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MaGiamGiaController : Controller
    {
        private readonly KhachSanService khachSanService;
        private readonly MaGiamGiaService maGiamGiaService;
        private readonly KhachHangService khachHangService;
        public MaGiamGiaController(KhachSanService khachSanServices, MaGiamGiaService maGiamGiaServices, KhachHangService khachHangService)
        {
            khachSanService = khachSanServices;
            maGiamGiaService = maGiamGiaServices;
            this.khachHangService = khachHangService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<MaGiamGia> listmagiamgai = maGiamGiaService.GetAllMaGiamGia();
                    Modeldata modeldata = new Modeldata
                    {
                        listmaGiamGia = listmagiamgai
                    };
                    return View(modeldata);
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
        public IActionResult ThemMaGiamGia(MaGiamGia magiamgias)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                ViewData["id"] = id;
                magiamgias.solandasudung = 0;
                maGiamGiaService.ThemMaGiamGia(magiamgias);
                return Redirect("~/admin/magiamgia/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult DuocGiamGia(string magiamgia)
        {
            if (magiamgia != null)
            {
                MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                if (maGiamGia != null)
                {
                    if (DateTime.Now.Date <= maGiamGia.ngayketthuc.Date)
                    {
                        if (maGiamGia.solandasudung < maGiamGia.solansudungtoida && maGiamGia.trangthai.Equals("còn sử dụng"))
                        {
                            return Json(new { duocgiamgia = maGiamGia });
                        }
                    }
                }
            }
            return Ok();
        }
        public IActionResult DeleteMaGiamGia([FromBody] List<int> idmagiamgia)
        {
            if (idmagiamgia != null && idmagiamgia.Any())
            {
                foreach (int id in idmagiamgia)
                {
                    maGiamGiaService.XoaMaGiamGia(id);
                }
                TempData["xoamagiamgia"] = "success";
                return Json(new { error = false });
            }
            else
            {
                return Json(new { error = true, message = "Vui lòng chọn bình luận để xoá!" });
            }
        }
        public IActionResult GetMaGiamGia(int id)
        {
            MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaById(id);
            return Json(maGiamGia);
        }
        public IActionResult UpdateMaGiamGia(MaGiamGia maGiamGias, int idmagiamgia)
        {
            if (idmagiamgia > 0)
            {
                MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaById(idmagiamgia);
                if (maGiamGia != null)
                {
                    maGiamGia.trangthai = "không sử dụng";
                    maGiamGiaService.CapNhatMaGiamGia(maGiamGia);
                }
            }
            else
            {
                maGiamGias.trangthai = "còn sử dụng";
                maGiamGiaService.CapNhatMaGiamGia(maGiamGias);
            }
            return Redirect("~/admin/magiamgia/");
        }

    }
}