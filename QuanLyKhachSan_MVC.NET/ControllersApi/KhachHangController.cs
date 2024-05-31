using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly KhachHangService khachHangService;
        public KhachHangController(KhachHangService khachHangService)
        {
            this.khachHangService = khachHangService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            List<KhachHang> khachHangs = khachHangService.GetAllKhachHang();
            return Ok(khachHangs);
        }
    }
}