using Microsoft.AspNetCore.Mvc;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly KhachHangService khachHangService;
        public HomeController(KhachHangService khachHangService)
        {
            this.khachHangService = khachHangService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}