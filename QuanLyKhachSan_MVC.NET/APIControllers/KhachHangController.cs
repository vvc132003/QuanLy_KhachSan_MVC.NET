using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
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
        [HttpGet]
        [Route("DanhSachKhachHangDaDangKy")]
        public IActionResult DanhSachKhachHangDaDangKy(string? cccd)
        {
            List<KhachHang> listKhachHang;
            if (!string.IsNullOrEmpty(cccd))
            {
                listKhachHang = khachHangService.GetAllKhachHang().FindAll(kh => kh.cccd == cccd);
            }
            else
            {
                listKhachHang = khachHangService.GetAllKhachHang().OrderByDescending(kh => kh.id).Take(5).ToList();
            }
            Modeldata yourModel = new Modeldata
            {
                listkhachHangs = listKhachHang,
            };
            return Ok(yourModel);

        }
    }
}