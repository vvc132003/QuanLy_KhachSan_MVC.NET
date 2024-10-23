using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongController : ControllerBase
    {

        private readonly PhongService phongService;
        private readonly TangService tangService;
        private readonly ThoiGianService thoiGianService;
        private readonly DatPhongService datPhongService;
        private readonly KhachHangService khachHangService;
        private readonly LichSuThanhToanService lichSuThanhToanService;
        private readonly LikesService likesService;
        private readonly BinhLuanService binhLuanService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly KhachSanService khachSanService;
        public PhongController(PhongService phongServices,
            TangService tangServices, ThoiGianService thoiGianServices,
            DatPhongService datPhongServices, KhachHangService khachHangService,
            LichSuThanhToanService lichSuThanhToanService,
            LikesService likesService, BinhLuanService binhLuanService, SanPhamService sanPhamService,
            ThueSanPhamService thueSanPhamService,
            KhachSanService khachSanService)
        {
            phongService = phongServices;
            tangService = tangServices;
            thoiGianService = thoiGianServices;
            datPhongService = datPhongServices;
            this.khachHangService = khachHangService;
            this.lichSuThanhToanService = lichSuThanhToanService;
            this.likesService = likesService;
            this.binhLuanService = binhLuanService;
            this.sanPhamService = sanPhamService;
            this.thueSanPhamService = thueSanPhamService;
            this.khachSanService = khachSanService;
        }
        [HttpGet]
        [Route("ListPhong")]
        public IActionResult ListPhong(int idkhachsan)
        {
            List<Tang> tanglist = tangService.GetAllTangkhachsanid(1);
            List<Modeldata> modeldataList = new List<Modeldata>();
            foreach (var tang in tanglist)
            {
                List<Phong> phongs = phongService.GetAllPhongIDTang(tang.id, idkhachsan);
                List<Phong> phongtrangthai = phongService.GetAllPhongTrangThai(idkhachsan);
                Modeldata modeldata = new Modeldata
                {
                    tang = tang,
                    listphong = phongs,
                    listphongtrangthai = phongtrangthai,
                };
                modeldataList.Add(modeldata);
            }
            return Ok(modeldataList);
        }

        [HttpGet]
        [Route("KhachHangbyiddatPhong")]
        public IActionResult KhachHangbyiddatPhong(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            if (datPhong == null)
            {
                return Ok();
            }
            KhachHang khachHang = khachHangService.GetKhachHangbyid(datPhong.idkhachhang);
            if (khachHang == null)
            {
                return Ok();
            }
            return Ok(new { khachHang });
        }
        [HttpGet]
        [Route("ListPhongidkhachsantrong")]
        public IActionResult ListPhongidkhachsantrong(int idkhachsan)
        {
            List<Phong> phonglist = phongService.GetAllPhongTrangThai(idkhachsan);
            return Ok(phonglist);
        }
        [HttpGet]
        [Route("ListPhongidkhachsan")]
        public IActionResult ListPhongidkhachsan(int idkhachsan)
        {
            List<Phong> phonglist = phongService.GetAllPhongByIdKhachSan(idkhachsan);
            return Ok(phonglist);
        }
    }
}