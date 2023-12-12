using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly PhongKhachSanService phongkhachsanService;
        private readonly KhachSanService khachSanService;

        public HomeController(PhongKhachSanService phongkhachsanServices, KhachSanService khachSanServices)
        {
            phongkhachsanService = phongkhachsanServices;
            khachSanService = khachSanServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TimKiemKhachSan(string tenkhachsan)
        {
            List<KhachSan> KhachSanlisst = khachSanService.GetAllKhachSanByname(tenkhachsan);
            foreach (var khachSan in KhachSanlisst)
            {
                List<PhongKhachSan> phongkhachsanlisst = phongkhachsanService.GetAllPhongIDKhachSan(khachSan.id);
                Modeldata modeldata = new Modeldata()
                {
                    listKhachSan = KhachSanlisst,
                    listPhongKhachSan = phongkhachsanlisst,
                };
                return View(modeldata);
            }
            return View();
        }
    }
}