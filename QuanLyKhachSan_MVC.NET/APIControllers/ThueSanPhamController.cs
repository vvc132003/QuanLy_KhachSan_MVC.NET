using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThueSanPhamController : ControllerBase
    {
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly SanPhamService sanPhamService;
        private readonly DatPhongService datPhongService;
        private readonly PhongService phongService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<ThuePhongHub> _hubContext;


        public ThueSanPhamController(ThueSanPhamService thueSanPhamServices, SanPhamService sanPhamServices,
            DatPhongService datPhongService, PhongService phongService,
            IHttpContextAccessor httpContextAccessor, IHubContext<ThuePhongHub> hubContext)
        {
            thueSanPhamService = thueSanPhamServices;
            sanPhamService = sanPhamServices;
            this.datPhongService = datPhongService;
            this.phongService = phongService;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }
        [HttpPost]
        [Route("ThueSanPham")]
        public IActionResult ThueSanPham([FromForm] int idsp, [FromForm] int iddatphong, [FromForm] int idnhanvien)
        {

            int idnv = idnhanvien;
            ThueSanPham thueSanPham = new ThueSanPham();
            SanPham sanpham = sanPhamService.GetSanPhamByID(idsp);
            thueSanPham.idnhanvien = idnv;
            thueSanPham.soluong = 1;
            thueSanPham.idsanpham = idsp;
            thueSanPham.iddatphong = iddatphong;
            ThueSanPham thueSanPhamididdatphong = thueSanPhamService.GetThueSanPhamByDatPhongAndSanPham(iddatphong, idsp);
            if (thueSanPhamididdatphong != null)
            {
                if (sanpham.soluongcon > 1)
                {
                    sanpham.soluongcon -= 1;
                    sanPhamService.CapNhatSanPham(sanpham);
                    thueSanPhamididdatphong.soluong += 1;
                    thueSanPhamididdatphong.thanhtien += 1 * sanpham.giaban;
                    thueSanPhamService.CapNhatThueSanPham(thueSanPhamididdatphong);
                }
                else
                {
                    Console.WriteLine("Sản phẩm này đã hết");
                }
            }
            else
            {
                if (sanpham.soluongcon > 1)
                {
                    sanpham.soluongcon -= 1;
                    sanPhamService.CapNhatSanPham(sanpham);
                    thueSanPham.thanhtien = 1 * sanpham.giaban;
                    thueSanPhamService.ThueSanPham(thueSanPham);
                }
                else
                {
                    Console.WriteLine("Sản phẩm này đã hết");
                }
            }
            _hubContext.Clients.All.SendAsync("ReceiveThueSanPham");
            return Ok();
        }
        [HttpPost]
        [Route("UpdatesoluongThueSanPham")]
        public IActionResult UpdatesoluongThueSanPham([FromForm] int id)
        {
            ThueSanPham thueSanPham = thueSanPhamService.GetThueSanPhamBYID(id);
            SanPham sanpham = sanPhamService.GetSanPhamByID(thueSanPham.idsanpham);
            if (sanpham.soluongcon > 1)
            {
                sanpham.soluongcon -= 1;
                sanPhamService.CapNhatSanPham(sanpham);
                thueSanPham.soluong += 1;
                thueSanPham.thanhtien = thueSanPham.soluong * sanpham.giaban;
                thueSanPhamService.CapNhatThueSanPham(thueSanPham);
                _hubContext.Clients.All.SendAsync("ReceiveThueSanPham");
            }
            else
            {
                Console.WriteLine("Sản phẩm này đã hết");
            }
            return Ok();
        }
        [HttpPost]
        [Route("XoasoluongThueSanPham")]
        public IActionResult XoasoluongThueSanPham([FromForm] int id)
        {

            ThueSanPham thueSanPham = thueSanPhamService.GetThueSanPhamBYID(id);
            SanPham sanpham = sanPhamService.GetSanPhamByID(thueSanPham.idsanpham);
            if (thueSanPham.soluong >= 1)
            {
                if (thueSanPham.soluong == 1)
                {
                    sanpham.soluongcon += 1;
                    sanPhamService.CapNhatSanPham(sanpham);
                    thueSanPhamService.XoaThueSanPham(id);
                    _hubContext.Clients.All.SendAsync("ReceiveThueSanPham");
                    return Ok();
                }
                else if (thueSanPham.soluong > 1)
                {
                    sanpham.soluongcon += 1;
                    sanPhamService.CapNhatSanPham(sanpham);
                    thueSanPham.soluong -= 1;
                    thueSanPham.thanhtien = thueSanPham.soluong * sanpham.giaban;
                    thueSanPhamService.CapNhatThueSanPham(thueSanPham);
                    _hubContext.Clients.All.SendAsync("ReceiveThueSanPham");
                    return Ok();
                }
            }
            return Ok();

        }

    }
}
