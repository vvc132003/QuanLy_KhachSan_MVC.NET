using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

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
    }
}