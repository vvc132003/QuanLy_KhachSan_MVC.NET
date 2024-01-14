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
        public IActionResult TimKiemKhachSan(int idkhachsan, string loaiphong, int songuoi)
        {
            List<Phong> phonglisst = phongService.GetAllPhongTrangThaiandidksloaiphongsonguoi(idkhachsan, loaiphong, songuoi);
            Modeldata modeldata = new Modeldata()
            {
                listphong = phonglisst,
            };
            return View(modeldata);
        }
        public IActionResult GetAllKhachSan()
        {
            List<KhachSan> allKhachSans = khachSanService.GetAllKhachSan();
            string html = "<ul style='list-style-type: none; margin: 0; padding: 0;'>";
            foreach (KhachSan khachSan in allKhachSans)
            {
                html += "<li style='background-color: #fff; padding: 8px; border-bottom: 1px solid #ccc; cursor: pointer;' data-id='" + khachSan.id + "'>" + khachSan.tenkhachsan + "</li>";
            }
            html += "</ul>";
            return Content(html);
        }
        public IActionResult GetAllLoaiPhongIdKhachSan(int idkhachsan)
        {
            List<string> loaiPhongs = phongService.GetAllLoaiPhongIdKhachSan(idkhachsan);
            string html = "<ul style='list-style-type: none; margin: 0; padding: 0;'>";
            foreach (string loaiPhong in loaiPhongs)
            {
                html += "<li style='background-color: #fff; padding: 8px; border-bottom: 1px solid #ccc; cursor: pointer;'>" + loaiPhong + "</li>";
            }
            html += "</ul>";
            return Content(html);
        }
        public IActionResult GetAllSoNguoiLoaiPhong(string loaiphong)
        {
            List<int> songuois = phongService.GetAllSoNguoiLoaiPhong(loaiphong);
            string html = "";
            foreach (int songuoi in songuois)
            {
                html += "<input type='text' id='songuoi' name='songuoi' class='form-control datetimepicker-input' value='" + songuoi + "' />";
            }
            html += "";
            return Content(html);
        }
    }
}