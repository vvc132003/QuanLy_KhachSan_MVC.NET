using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachSanController : ControllerBase
    {
        private readonly KhachSanService khachSanService;
        private readonly PhongService phongService;
        private readonly IHubContext<ThuePhongHub> _thuePhongHubContext;

        public KhachSanController(KhachSanService khachSanService, PhongService phongService, IHubContext<ThuePhongHub> thuePhongHubContext)
        {
            this.khachSanService = khachSanService;
            this.phongService = phongService;
            this._thuePhongHubContext = thuePhongHubContext;
        }
        [HttpGet]
        [Route("KhachSanList")]
        public IActionResult KhachSanList()
        {
            List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
            return Ok(listkhachsan);
        }
        [HttpGet]
        [Route("GetPhongbyidKhachsan")]
        public IActionResult GetPhongbyidKhachsan(int idkhachsan)
        {
            List<Phong> listPhong = phongService.GetAllPhongIDKhachSan(idkhachsan);
            return Ok(listPhong);
        }
    }
}
