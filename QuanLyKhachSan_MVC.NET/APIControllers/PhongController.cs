using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongController : ControllerBase
    {
        private readonly PhongService phongService;
        public PhongController(PhongService phongService)
        {
            this.phongService = phongService;
        }
        [HttpGet]
        [Route("ListPhong")]
        public IActionResult ListPhong()
        {
            List<Phong> phonglist = phongService.GetAllPhong();
            return Ok(phonglist);
        }
    }
}