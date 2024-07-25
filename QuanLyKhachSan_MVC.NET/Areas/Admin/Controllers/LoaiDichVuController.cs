using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiDichVuController : Controller
    {
        private readonly LoaiDichDichVuService loaiDichDichVuService;
        private readonly KhachSanService khachSanService;
        public LoaiDichVuController(LoaiDichDichVuService loaiDichDichVuService, KhachSanService khachSanService)
        {
            this.loaiDichDichVuService = loaiDichDichVuService;
            this.khachSanService = khachSanService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {

                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                ViewData["idkhachsan"] = idkhachsan;
                List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();
                List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
                List<Modeldata> modelDataList = new List<Modeldata>();
                foreach (var loaiDichVu in loaiDichVus)
                {
                    KhachSan khachSan = khachSanService.GetKhachSanById(loaiDichVu.idkhachsan);
                    Modeldata modeldata = new Modeldata
                    {
                        loaiDichVu = loaiDichVu,
                        listKhachSan = listkhachsan,
                        khachSan = khachSan,
                    };
                    modelDataList.Add(modeldata);
                }
                return View(modelDataList);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult Themloaidichvu(LoaiDichVu loaiDichVu)
        {
            loaiDichDichVuService.ThemLoaiDichVu(loaiDichVu);
            return Redirect("~/admin/loaidichvu/");
        }
    }
}
