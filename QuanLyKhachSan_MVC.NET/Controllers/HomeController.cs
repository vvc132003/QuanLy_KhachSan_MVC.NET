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
        public IActionResult GetAllSoNguoiLoaiPhong(int idphong)
        {
            List<int> songuois = phongService.GetAllSoNguoiLoaiPhong(idphong);
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