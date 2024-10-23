using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model.Models;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanPhongController : ControllerBase
    {
        private readonly DatPhongService datPhongService;
        private readonly KhachHangService khachHangService;
        private readonly PhongService phongService;
        private readonly NhanPhongService nhanPhongService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly ThoiGianService thoiGianService;
        private readonly LoaiDichDichVuService loaiDichDichVuService;
        private readonly SuDungMaGiamGiaService suDungMaGiamGiaService;
        private readonly MaGiamGiaService maGiamGiaService;
        private readonly IHubContext<ThuePhongHub> _thuePhongHubContext;
        public NhanPhongController(DatPhongService datPhongServices,
            KhachHangService khachHangServices,
            PhongService phongServices,
            NhanPhongService nhanPhongServices,
            SanPhamService sanPhamServices,
            ThueSanPhamService thueSanPhamServices,
            LoaiDichDichVuService loaiDichDichVuService,
            ThoiGianService thoiGianServices,
            SuDungMaGiamGiaService suDungMaGiamGiaService,
            MaGiamGiaService maGiamGiaService,
            IHubContext<ThuePhongHub> thuePhongHubContext
            )
        {
            datPhongService = datPhongServices;
            khachHangService = khachHangServices;
            phongService = phongServices;
            nhanPhongService = nhanPhongServices;
            sanPhamService = sanPhamServices;
            thueSanPhamService = thueSanPhamServices;
            thoiGianService = thoiGianServices;
            this.loaiDichDichVuService = loaiDichDichVuService;
            this.suDungMaGiamGiaService = suDungMaGiamGiaService;
            this.maGiamGiaService = maGiamGiaService;
            _thuePhongHubContext = thuePhongHubContext;
        }
        [HttpGet]
        [Route("NhanPhongOnline")]
        public IActionResult NhanPhongOnline(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThaiOnline(idphong);
            KhachHang khachHang = khachHangService.GetKhachHangbyid(datPhong.idkhachhang);
            List<SanPham> sanPhams = sanPhamService.GetAllSanPham();
            List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();
            Phong phong = phongService.GetPhongID(idphong);
            SuDungMaGiamGia sudunggiamGia = suDungMaGiamGiaService.GetSuDungMaGiamGiaByIddatphong(datPhong.id);
            MaGiamGia maGiamGia = sudunggiamGia != null ? maGiamGiaService.GetMaGiamGiaById(sudunggiamGia.idmagiamgia) : null;
            Modeldata modeldata = new Modeldata
            {
                datPhong = datPhong,
                khachhang = khachHang,
                listsanPham = sanPhams,
                loaiDichVus = loaiDichVus,
                phong = phong,
                magiamGia = maGiamGia
            };
            return Ok(modeldata);
        }
        [HttpPost]
        [Route("CheckInNhanPhongOnline")]
        public IActionResult CheckInNhanPhongOnline([FromForm] int idnhanvien, [FromForm] int idphong, [FromForm] string? cccd, [FromForm] List<int>? idsanpham)
        {
            NhanPhong nhanPhong = new NhanPhong();
            int idnv = idnhanvien;
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThaiOnline(idphong);
            if (datPhong != null && datPhong.cccd == cccd)
            {
                nhanPhong.idnhanvien = idnv;
                nhanPhong.iddatphong = datPhong.id;
                nhanPhong.ngaynhanphong = DateTime.Now;
                nhanPhongService.ThemNhanPhong(nhanPhong);

                Phong phong = phongService.GetPhongID(idphong);
                phong.tinhtrangphong = "có khách";
                phongService.CapNhatPhong(phong);
                datPhong.trangthai = "đã đặt";
                datPhongService.UpdateDatPhong(datPhong);
                if (idsanpham != null && idsanpham.Any())
                {
                    ThueSanPham thueSanPham = new ThueSanPham();
                    foreach (int idsp in idsanpham)
                    {
                        SanPham sanpham = sanPhamService.GetSanPhamByID(idsp);
                        // thuê sản phẩm
                        thueSanPham.idnhanvien = idnv;
                        thueSanPham.soluong = 1;
                        thueSanPham.idsanpham = idsp;
                        thueSanPham.iddatphong = datPhong.id;
                        thueSanPham.thanhtien = 1 * sanpham.giaban;
                        thueSanPhamService.ThueSanPham(thueSanPham);
                        // cập nhật số lượng tồn
                        sanpham.soluongcon -= 1;
                        sanPhamService.CapNhatSanPham(sanpham);
                    }
                }
                _thuePhongHubContext.Clients.All.SendAsync("ReceiveThanhToan", phong.idkhachsan);
                return Ok();
            }
            else
            {
                return Ok();
            }
        }

    }
}
