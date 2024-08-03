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
        public IActionResult SoLuongHopDong(int? idnam, int idkhachsan)
        {
            List<DatPhong> datPhongs = new List<DatPhong>();
            List<HuyDatPhong> huyDatPhongs = new List<HuyDatPhong>();
            List<ChuyenPhong> chuyenPhongs = new List<ChuyenPhong>();
            List<Phong> phongs = phongService.GetAllPhongIDKhachSan(idkhachsan);

            if (idnam.HasValue && idnam.Value > 0)
            {
                if (idkhachsan > 0)
                {
                    foreach (var phong in phongs)
                    {
                        datPhongs.AddRange(datPhongService.GetAllDatPhongbyidphong(phong.id)
                            .Where(dp => dp.ngaydat.Year == idnam.Value && dp.trangthai == "đã trả"));
                    }
                }
                else
                {
                    datPhongs = datPhongService.GetAllDatPhongDat()
                        .Where(dp => dp.ngaydat.Year == idnam.Value && dp.trangthai == "đã trả")
                        .ToList();
                    huyDatPhongs = huyDatPhongService.GetAllHuyDatPhong()
                        .Where(hdp => hdp.ngayhuy.Year == idnam.Value)
                        .ToList();
                    chuyenPhongs = chuyenPhongService.GetAllChuyenPhong()
                        .Where(cp => cp.ngaychuyen.Year == idnam.Value)
                        .ToList();
                }
            }
            else
            {
                datPhongs = datPhongService.GetAllDatPhongDat()
                    .Where(dp => dp.trangthai == "đã trả")
                    .ToList();
                huyDatPhongs = huyDatPhongService.GetAllHuyDatPhong();
                chuyenPhongs = chuyenPhongService.GetAllChuyenPhong();
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
            var result = new
            {
                datphong = datphong,
                huydatphong = huydatphong,
                chuyenphong = chuyenphong
            };
            return Ok(result);
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
            float tongdoangthuhuydatphong = 0;

            if (idnam.HasValue && idnam > 0)
            {
                lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToanYear(idnam.Value);
                tongdoangthuhuydatphong = huyDatPhongService.GetAllHuyDatPhong()
                    .Where(hdp => hdp.ngayhuy.Year == idnam.Value)
                    .Sum(huydatphong => huydatphong.sotienhoanlai);
            }
            else
            {
                lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToan();
                tongdoangthuhuydatphong = huyDatPhongService.GetAllHuyDatPhong()
                    .Sum(huydatphong => huydatphong.sotienhoanlai);
            }

            var doanhThuTheoThang = lichSuThanhToans
                .GroupBy(l => l.ngaythanhtoan.Month)
                .Select(g => new
                {
                    Thang = g.Key,
                    TongDoanhThu = g.Sum(l => l.sotienthanhtoan) + tongdoangthuhuydatphong
                })
                .ToList();

            var modeldatas = doanhThuTheoThang.Select(d => new Modeldatas
            {
                Thang = d.Thang,
                Tongdoanhthutungthang = d.TongDoanhThu
            }).ToList();

            return Ok(modeldatas);
        }
        public class Modeldatas
        {
            public int Thang { get; set; }
            public float Tongdoanhthutungthang { get; set; }
        }
        [HttpGet("TongPhanTram")]
        public IActionResult TongPhanTram(DateTime? startDate, DateTime? endDate, int idkhachsan)
        {
            List<ThueSanPham> thueSanPhams = new List<ThueSanPham>();
            if (startDate.HasValue && endDate.HasValue && idkhachsan > 0)
            {
                List<Phong> phongs = phongService.GetAllPhongIDKhachSan(idkhachsan);
                foreach (var phong in phongs)
                {
                    List<DatPhong> datPhongs = datPhongService.GetAllDatPhongbyidphong(phong.id);
                    foreach (var datPhong in datPhongs)
                    {
                        List<ThueSanPham> thueSanPhamsForPhong = thueSanPhamService.GetThueSanPhamByDatebyidddatphong(startDate.Value, endDate.Value, datPhong.id);
                        thueSanPhams.AddRange(thueSanPhamsForPhong);
                    }
                }
            }
            else if (startDate.HasValue && endDate.HasValue)
            {
                thueSanPhams = thueSanPhamService.GetThueSanPhamByDate(startDate.Value, endDate.Value);
            }
            else
            {
                thueSanPhams = thueSanPhamService.GetAllThueSanPham();
            }
            List<Modeldata> modeldatas = new List<Modeldata>();
            // Lọc ra danh sách duy nhất các sản phẩm được thuê
            var uniqueThueSanPhams = thueSanPhams.GroupBy(tsp => tsp.idsanpham).Select(group => new { idSanPham = group.Key, soLuong = group.Sum(tsp => tsp.soluong) }).ToList();
            // Tính tổng số lượng sản phẩm được thuê
            int tongsoluongsanphamduocthue = thueSanPhams.Sum(thueSanPham => thueSanPham.soluong);
            /*foreach (var sanphamthue in thueSanPhams)
            {
                tongsoluongsanphamduocthue += sanphamthue.soluong;
            }*/
            foreach (var item in uniqueThueSanPhams)
            {
                SanPham sanPham = sanPhamService.GetSanPhamByID(item.idSanPham);
                float phantramsanpham = ((float)item.soLuong / tongsoluongsanphamduocthue) * 100;
                Console.WriteLine($"Tên sản phẩm: {sanPham.tensanpham}, số lượng thuê: {item.soLuong}, phần trăm: {phantramsanpham} % ");
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