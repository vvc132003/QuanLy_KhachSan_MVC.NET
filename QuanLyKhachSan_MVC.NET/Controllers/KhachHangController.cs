using Microsoft.AspNetCore.Mvc;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class KhachHangController : Controller
    {
        public IActionResult DangKy()
        {
            return View();
        }
    }
}