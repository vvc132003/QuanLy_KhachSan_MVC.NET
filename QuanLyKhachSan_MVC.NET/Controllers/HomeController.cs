using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly PhongService phongService;
        private readonly KhachSanService khachSanService;

        public HomeController(PhongService phongServices, KhachSanService khachSanServices)
        {
            phongService = phongServices;
            khachSanService = khachSanServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TimKiemKhachSan(string tenkhachsan, string loaiphong, int songuoi)
        {
            List<KhachSan> KhachSanlisst = khachSanService.GetAllKhachSanByname(tenkhachsan);
            foreach (var khachSan in KhachSanlisst)
            {
                List<Phong> phonglisst = phongService.GetAllPhongTrangThaiandidksloaiphongsonguoi(khachSan.id, loaiphong, songuoi);
                Modeldata modeldata = new Modeldata()
                {
                    listKhachSan = KhachSanlisst,
                    listphong = phonglisst,
                };
                return View(modeldata);
            }
            return View();
        }
        public ActionResult GetAllKhachSan()
        {
            List<KhachSan> allKhachSans = khachSanService.GetAllKhachSan();
            string html = "<ul style='list-style-type: none; margin: 0; padding: 0;'>";
            foreach (KhachSan khachSan in allKhachSans)
            {
                html += "<li style='background-color: #fff; padding: 8px; border-bottom: 1px solid #ccc; cursor: pointer;'>" + khachSan.tenkhachsan + "</li>";
            }
            html += "</ul>";
            return Content(html);
        }
    }
}