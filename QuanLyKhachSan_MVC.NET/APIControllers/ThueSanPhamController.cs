using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model.Models;
using Service;
using Service.Service;
using System.Runtime.InteropServices;
using System.Text.Json;


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
        private readonly TachDonService tachDonService;


        public ThueSanPhamController(ThueSanPhamService thueSanPhamServices, SanPhamService sanPhamServices,
            DatPhongService datPhongService, PhongService phongService,
            IHttpContextAccessor httpContextAccessor, IHubContext<ThuePhongHub> hubContext,
            TachDonService tachDonService)
        {
            thueSanPhamService = thueSanPhamServices;
            sanPhamService = sanPhamServices;
            this.datPhongService = datPhongService;
            this.phongService = phongService;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            this.tachDonService = tachDonService;
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
        [HttpGet]
        [Route("Danhsachsanphamthueiddatphong")]
        public IActionResult Danhsachsanphamthueiddatphong(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            List<ThueSanPham> listthueSanPham = thueSanPhamService.GetAllThueSanPhamID(datPhong.id);
            return Ok(listthueSanPham);
        }

        [HttpGet]
        [Route("GetAllTachDonByDatPhongId")]
        public IActionResult GetAllTachDonByDatPhongId(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            List<TachDon> listtachdon = tachDonService.GetAllTachDonByDatPhongId(datPhong.id);
            return Ok(listtachdon);
        }



        [HttpPost]
        [Route("TachDon")]
        public IActionResult TachDon([FromForm] int id, [FromForm] int idphong, [FromForm] int soluong, [FromForm] string tensanpham, [FromForm] string image, [FromForm] int iddatphong, [FromForm] int idsanpham)
        {
            Phong phong = phongService.GetPhongID(idphong);
            SanPham sanPham = sanPhamService.GetSanPhamByID(idsanpham);
            List<ThueSanPham> checkslthuesanpham = thueSanPhamService.GetThueSanPhamByIDdatphong(iddatphong);
            /// chekc sản phẩm với số lượng cối cùng
            if (checkslthuesanpham.Count == 1 && checkslthuesanpham.First().soluong == 1)
            {
                return Ok(new { success = false, messages = "Sản phẩm cuối cùng không thể tách." });
            }
            TachDon tachDon = tachDonService.GetTachDonByIdAndDatPhong(idsanpham, iddatphong);

            if (tachDon != null)
            {
                tachDon.soluong += 1;
                tachDon.thanhtien = tachDon.soluong * sanPham.giaban;
                tachDon.ghichu = $"Được tách từ phòng: {phong.sophong}, số lượng: {soluong + 1}";
                tachDonService.UpdateTachDon(tachDon);
            }
            else
            {
                TachDon newTachDon = new TachDon
                {
                    id = id,
                    tensanpham = tensanpham,
                    thanhtien = soluong * sanPham.giaban,
                    soluong = soluong,
                    image = image,
                    iddatphong = iddatphong,
                    idsanpham = idsanpham,
                    ghichu = $"Được tách từ phòng: {phong.sophong}, số lượng: {soluong}",
                };
                tachDonService.InsertTachDon(newTachDon);
            }

            ThueSanPham truSoLuongThueSanPham = thueSanPhamService.GetThueSanPhamBYID(id);
            if (truSoLuongThueSanPham != null && sanPham != null)
            {
                if (truSoLuongThueSanPham.soluong == 1)
                {
                    thueSanPhamService.XoaThueSanPham(truSoLuongThueSanPham.id);
                }
                else
                {
                    truSoLuongThueSanPham.soluong -= 1;
                    truSoLuongThueSanPham.thanhtien = truSoLuongThueSanPham.soluong * sanPham.giaban;
                    thueSanPhamService.CapNhatThueSanPham(truSoLuongThueSanPham);
                }
            }
            else
            {
                return Ok(new { success = false, message = "Sản phẩm hoặc thuê sản phẩm không tồn tại." });
            }
            return Ok(new { success = true, message = "Tách thành công!" });
        }

        [HttpGet]
        [Route("HuyTach")]
        public IActionResult HuyTach(int idphong, int idnhanvien)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            List<TachDon> cartItems = tachDonService.GetAllTachDonByDatPhongId(datPhong.id);
            if (cartItems != null)
            {
                foreach (var cartItem in cartItems)
                {
                    ThueSanPham thueSanPham = thueSanPhamService.GetThueSanPhamByDatPhongAndSanPham(cartItem.iddatphong, cartItem.idsanpham);
                    SanPham sanpham = sanPhamService.GetSanPhamByID(cartItem.idsanpham);
                    if (thueSanPham != null)
                    {
                        thueSanPham.soluong = cartItem.soluong + thueSanPham.soluong;
                        thueSanPham.thanhtien = cartItem.thanhtien + thueSanPham.thanhtien;
                        thueSanPhamService.CapNhatThueSanPham(thueSanPham);
                    }
                    else
                    {
                        ThueSanPham addthuesanpham = new()
                        {
                            idnhanvien = idnhanvien,
                            soluong = 1,
                            idsanpham = sanpham.id,
                            iddatphong = cartItem.iddatphong
                        };
                        addthuesanpham.thanhtien = addthuesanpham.soluong * sanpham.giaban;
                        thueSanPhamService.ThueSanPham(addthuesanpham);
                    }
                }
                tachDonService.DeleteAllTachDon();
                return Ok(new { success = true, message = "Huỷ tách thành công!" });
            }
            else
            {
                return Ok(new { success = false, message = "Không có dịch vụ để huỷ!" });
            }

        }

    }
}
