using Microsoft.AspNetCore.Mvc;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
