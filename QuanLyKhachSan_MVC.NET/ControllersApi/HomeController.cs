using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service;
using Model.Models;
using System.Globalization;

namespace QuanLyKhachSan_MVC.NET.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly PhongService phongService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;
        private readonly LichSuThanhToanService lichSuThanhToanService;
        private readonly LikesService likesService;
        private readonly BinhLuanService binhLuanService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly DatPhongService datPhongService;
        private readonly HuyDatPhongService huyDatPhongService;
        private readonly ChuyenPhongService chuyenPhongService;
        public HomeController(PhongService phongServices,
            KhachSanService khachSanServices,
            KhachHangService khachHangService,
            LichSuThanhToanService lichSuThanhToanService,
            LikesService likesService,
            BinhLuanService binhLuanService,
            SanPhamService sanPhamService,
            ThueSanPhamService thueSanPhamService,
            DatPhongService datPhongService, HuyDatPhongService huyDatPhongService, ChuyenPhongService chuyenPhongService)
        {
            phongService = phongServices;
            khachSanService = khachSanServices;
            this.khachHangService = khachHangService;
            this.lichSuThanhToanService = lichSuThanhToanService;
            this.sanPhamService = sanPhamService;
            this.likesService = likesService;
            this.binhLuanService = binhLuanService;
            this.thueSanPhamService = thueSanPhamService;
            this.datPhongService = datPhongService;
            this.huyDatPhongService = huyDatPhongService;
            this.chuyenPhongService = chuyenPhongService;
        }
        [HttpGet("SoLuongHopDong")]
        public IActionResult SoLuongHopDong(int? idnam)
        {
            List<DatPhong> datPhongs;
            List<HuyDatPhong> huyDatPhongs;
            List<ChuyenPhong> chuyenPhongs;

            if (idnam.HasValue && idnam > 0)
            {
                datPhongs = datPhongService.GetAllDatPhongDat()
                    .Where(dp => dp.ngaydat.Year == idnam && dp.trangthai == "đã trả")
                    .ToList();

                huyDatPhongs = huyDatPhongService.GetAllHuyDatPhong()
                    .Where(hdp => hdp.ngayhuy.Year == idnam)
                    .ToList();

                chuyenPhongs = chuyenPhongService.GetAllChuyenPhong()
                    .Where(cp => cp.ngaychuyen.Year == idnam)
                    .ToList();
            }
            else
            {
                datPhongs = datPhongService.GetAllDatPhongDat()
                    .Where(dp => dp.trangthai == "đã trả")
                    .ToList();

                huyDatPhongs = huyDatPhongService.GetAllHuyDatPhong()
                    .ToList();

                chuyenPhongs = chuyenPhongService.GetAllChuyenPhong()
                    .ToList();
            }

            var datphong = datPhongs
                .GroupBy(dp => dp.ngaydat.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            var huydatphong = huyDatPhongs
                .GroupBy(hdp => hdp.ngayhuy.Month)
                .Select(h => new
                {
                    Month = h.Key,
                    Count = h.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            var chuyenphong = chuyenPhongs
                .GroupBy(cp => cp.ngaychuyen.Month)
                .Select(c => new
                {
                    Month = c.Key,
                    Count = c.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            var item = new
            {
                datphong = datphong,
                huydatphong = huydatphong,
                chuyenphong = chuyenphong
            };

            return Ok(item);
        }

        public class Item
        {
            public string tensanpham { get; set; }
            public decimal phantramthue { get; set; }
        }
        [HttpGet("TongSoLuongThueSanPhamTheoPhanTram")]
        public IActionResult TongSoLuongThueSanPhamTheoPhanTram()
        {
            // Lấy danh sách tất cả các bản ghi thuê sản phẩm từ dịch vụ thueSanPhamService
            List<ThueSanPham> thueSanPhams = thueSanPhamService.GetAllThueSanPham();
            // Tính tổng số lượng thuê của tất cả các sản phẩm
            int totalRentals = thueSanPhams.Sum(ts => ts.soluong);
            // Khởi tạo danh sách kết quả để lưu thông tin sản phẩm và phần trăm thuê tương ứng
            List<Item> result = new List<Item>();
            // Sử dụng Dictionary để lưu tổng số lượng thuê của mỗi sản phẩm dựa trên idsanpham
            Dictionary<int, int> totalRentalsByProductId = new Dictionary<int, int>();
            // Duyệt qua danh sách thueSanPhams để tính tổng số lượng thuê của từng sản phẩm
            foreach (var thuesanpham in thueSanPhams)
            {
                int productId = thuesanpham.idsanpham;
                // Kiểm tra nếu chưa có key productId trong totalRentalsByProductId thì khởi tạo với giá trị 0
                if (!totalRentalsByProductId.ContainsKey(productId))
                {
                    totalRentalsByProductId[productId] = 0;
                }
                // Cập nhật tổng số lượng thuê cho sản phẩm có id là productId
                totalRentalsByProductId[productId] += thuesanpham.soluong;
            }
            // Duyệt qua từng productId trong totalRentalsByProductId để tính phần trăm thuê và thêm vào danh sách kết quả
            foreach (var productId in totalRentalsByProductId.Keys)
            {
                // Lấy thông tin sản phẩm từ dịch vụ sanPhamService dựa trên productId
                SanPham sanPham = sanPhamService.GetSanPhamByID(productId);
                // Lấy tổng số lượng thuê của sản phẩm có id là productId
                int totalProductRentals = totalRentalsByProductId[productId];
                // Tính phần trăm thuê của sản phẩm này so với tổng số lượng thuê của tất cả các sản phẩm
                decimal percentage = Math.Round((decimal)totalProductRentals * 100 / totalRentals, 2);
                // Tạo một đối tượng Item mới để lưu thông tin sản phẩm và phần trăm thuê vào danh sách kết quả
                var item = new Item
                {
                    tensanpham = sanPham.tensanpham, // Tên sản phẩm
                    phantramthue = percentage,       // Phần trăm thuê
                };
                result.Add(item); // Thêm đối tượng Item vào danh sách kết quả
            }
            // Tính tổng phần trăm thuê của tất cả các sản phẩm trong danh sách kết quả
            decimal totalPercentage = result.Sum(item => item.phantramthue);
            // Nếu tổng phần trăm thuê không đạt 100%, điều chỉnh phần trăm cuối cùng trong danh sách kết quả
            if (totalPercentage != 100)
            {
                decimal adjustment = 100 - totalPercentage;
                var lastItem = result.LastOrDefault(); // Lấy phần tử cuối cùng trong danh sách kết quả
                if (lastItem != null)
                {
                    lastItem.phantramthue += adjustment; // Điều chỉnh phần trăm thuê của phần tử cuối cùng
                }
            }
            // Trả về danh sách kết quả dưới dạng JSON
            return Ok(result);
        }
        [HttpGet("DoanhThuTheoThang")]
        public IActionResult DoanhThuTheoThang(int? idnam)
        {
            List<LichSuThanhToan> lichSuThanhToans;
            if (idnam.HasValue && idnam > 0)
            {
                lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToanYear(idnam.Value);
            }
            else
            {
                lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToan();
            }
            float tongdoanhthutungthang = 0;
            List<Modeldata> modeldatas = new List<Modeldata>();
            foreach (var lichsuthanhtoan in lichSuThanhToans)
            {
                tongdoanhthutungthang += lichsuthanhtoan.sotienthanhtoan;
                Modeldata modeldata = new Modeldata()
                {
                    lichSuThanhToan = lichsuthanhtoan,
                    Tongdoanhthutungthang = tongdoanhthutungthang,
                };
                modeldatas.Add(modeldata);
            }
            return Ok(modeldatas);
        }


        [HttpGet("TongPhanTram")]
        public IActionResult TongPhanTram(DateTime? startDate, DateTime? endDate)
        {
            List<ThueSanPham> thueSanPhams;

            if (startDate.HasValue && endDate.HasValue)
            {
                thueSanPhams = thueSanPhamService.GetThueSanPhamByDate(startDate.Value, endDate.Value);
            }
            else
            {
                thueSanPhams = thueSanPhamService.GetAllThueSanPham();
            }
            List<Modeldata> modeldatas = new List<Modeldata>();
            int tongsoluongsanphamduocthue = 0;
            foreach (var thuesanpham in thueSanPhams)
            {
                tongsoluongsanphamduocthue += thuesanpham.soluong;
            }
            foreach (var thuesanpham in thueSanPhams)
            {
                SanPham sanPham = sanPhamService.GetSanPhamByID(thuesanpham.idsanpham);
                float phantramsanpham = (float)thuesanpham.soluong / tongsoluongsanphamduocthue * 100;
                Modeldata modeldata = new Modeldata()
                {
                    sanPham = sanPham,
                    Tongdoanhthutungthang = phantramsanpham,
                };
                modeldatas.Add(modeldata);
            }
            return Ok(modeldatas);
        }
    }
}