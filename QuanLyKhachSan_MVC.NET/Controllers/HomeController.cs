using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Security.Claims;
using QuanLyKhachSan_MVC.NET.Areas.Login.Modelss;
using DocumentFormat.OpenXml.Office2010.Excel;
using Service.Service;
using Newtonsoft.Json;
using System.Globalization;
using System.Transactions;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class HomeController : Controller
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



        public IActionResult SoLuongHopDong()
        {
            List<DatPhong> datPhongs = datPhongService.GetAllDatPhongDat();
            var datphong = datPhongs
                .GroupBy(dp => dp.ngaydat.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();
            List<HuyDatPhong> huyDatPhongs = huyDatPhongService.GetAllHuyDatPhong();
            var huydatphong = huyDatPhongs
                .GroupBy(hdp => hdp.ngayhuy.Month)
                .Select(h => new
                {
                    Month = h.Key,
                    Count = h.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();
            List<ChuyenPhong> chuyenPhongs = chuyenPhongService.GetAllChuyenPhong();
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
            return Json(item);
        }

        public IActionResult TongThueSanPham()
        {
            List<SanPham> sanPhams = sanPhamService.GetAllSanPham();
            Dictionary<int, int> sanPhamThueTong = new Dictionary<int, int>();

            foreach (var sanpham in sanPhams)
            {
                List<ThueSanPham> thueSanPhams = thueSanPhamService.GetAllThueSanPhamIDSanPham(sanpham.id);
                int totalRentedQuantity = thueSanPhams.Sum(soluong => soluong.soluong);
                SanPham sanPham = sanPhamService.GetSanPhamByID(sanpham.id);
                sanPhamThueTong[sanPham.id] = totalRentedQuantity;
            }

            return Json(sanPhamThueTong);
        }





        public IActionResult ThongKe()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null &&
               HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["idkhachsan"] = idkhachsan;
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<LichSuThanhToan> lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToan();
                float tongDoanhThu = lichSuThanhToans.Sum(lichSu => lichSu.sotienthanhtoan);
                List<KhachHang> khachHangs = khachHangService.GetAllKhachHang();
                int tongsoluongkhachhang = khachHangs.Count();
                List<Likes> likes = likesService.GetAllLikes();
                int tonglikes = likes.Count();
                List<BinhLuan> binhLuans = binhLuanService.GetAllBinhLuans();
                int tongbinhluan = binhLuans.Count();
                List<SanPham> sanPhams = sanPhamService.GetAllSanPham();
                int tongsanpham = sanPhams.Count();
                ViewData["tongDoanhThu"] = tongDoanhThu;
                ViewData["tongSoLuongKhachHang"] = tongsoluongkhachhang;
                ViewData["tonglikes"] = tonglikes;
                ViewData["tongbinhluan"] = tongbinhluan;
                ViewData["tongsanpham"] = tongsanpham;
                List<Modeldata> modeldatalist = new List<Modeldata>();
                foreach (var sanpham in sanPhams)
                {
                    List<ThueSanPham> thueSanPhams = thueSanPhamService.GetAllThueSanPhamIDSanPham(sanpham.id);
                    int totalRentedQuantity = thueSanPhams.Sum(soluong => soluong.soluong);
                    KhachSan khachSan = khachSanService.GetKhachSanById(sanpham.idkhachsan);
                    Modeldata modeldata = new Modeldata()
                    {
                        sanPham = sanpham,
                        khachSan = khachSan,
                        TotalRentedQuantity = totalRentedQuantity
                    };
                    modeldatalist.Add(modeldata);
                }
                return View(modeldatalist);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public class Item
        {
            public string tensanpham { get; set; }
            public decimal phantramthue { get; set; }
        }

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
            return Json(result);
        }
        public IActionResult DoanhThuTheoThang(int? idthang)
        {
            // Lấy danh sách các giao dịch từ cơ sở dữ liệu
            List<LichSuThanhToan> lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToan();

            // Tạo dictionary để lưu tổng doanh thu của từng tháng
            Dictionary<int, float> revenueByMonth = new Dictionary<int, float>();

            // Tính tổng doanh thu từng tháng
            foreach (var lichsuthanhtoan in lichSuThanhToans)
            {
                int month = lichsuthanhtoan.ngaythanhtoan.Month;
                float revenue = lichsuthanhtoan.sotienthanhtoan;

                if (idthang != null && month != idthang)
                {
                    continue; // Nếu chỉ lấy dữ liệu cho một tháng cụ thể và không phải tháng đó thì bỏ qua
                }

                if (revenueByMonth.ContainsKey(month))
                {
                    revenueByMonth[month] += revenue;
                }
                else
                {
                    revenueByMonth[month] = revenue;
                }
            }

            // Nếu idthang không được cung cấp (null), thì sắp xếp lại theo tháng để đảm bảo hiển thị đúng thứ tự thời gian
            if (idthang == null)
            {
                var sortedRevenueByMonth = revenueByMonth.OrderBy(x => x.Key).ToList();
                // Chuẩn bị dữ liệu cho biểu đồ
                var months = sortedRevenueByMonth.Select(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Key));
                var revenues = sortedRevenueByMonth.Select(x => x.Value);

                // Trả về dữ liệu dưới dạng JSON
                var jsonData = new
                {
                    Months = months,
                    Revenues = revenues
                };

                return Json(jsonData);
            }
            else // Nếu có idthang, chỉ trả về dữ liệu cho tháng đó
            {
                float totalRevenue = revenueByMonth.ContainsKey(idthang.Value) ? revenueByMonth[idthang.Value] : 0;

                // Trả về dữ liệu dưới dạng JSON
                var jsonData = new
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(idthang.Value),
                    TotalRevenue = totalRevenue
                };

                return Json(jsonData);
            }
        }


        public IActionResult AddNhanVien()
        {
            return View();
        }
        public IActionResult DiemDanh()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                KhachHang khachHang = khachHangService.GetKhachHangbyid(id);
                Modeldata modeldata = new Modeldata
                {
                    khachhang = khachHang,
                };
                return View(modeldata);
            }
            else
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (result != null && result.Succeeded)
                {
                    var name = result.Principal.FindFirstValue(ClaimTypes.Name);
                    var email = result.Principal.FindFirstValue(ClaimTypes.Email);
                    var profileLink = result.Principal.FindFirstValue("urn:google:profile");
                    var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    var userProfileViewModel = new UserProfileViewModel
                    {
                        Name = name,
                        Email = email,
                        ProfileLink = profileLink,
                        UserId = userId
                    };
                    Modeldata modeldata = new Modeldata
                    {
                        userProfileViewModel = userProfileViewModel
                    };
                    return View(modeldata);
                }
                else
                {
                    return View();
                }
            }
        }
        public async Task<IActionResult> TimKiemKhachSan(int idkhachsan, string loaiphong, int songuoi)
        {
            if (idkhachsan != 0 && loaiphong != null && songuoi != 0)
            {
                List<Phong> phonglisst = phongService.GetAllPhongByidKhachSanndlaoiphongandsonguoi(idkhachsan, loaiphong, songuoi);
                if (phonglisst != null)
                {
                    KhachSan khachSan = khachSanService.GetKhachSanById(idkhachsan);
                    if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
                    {
                        int id = HttpContext.Session.GetInt32("id").Value;
                        string hovaten = HttpContext.Session.GetString("hovaten");
                        ViewData["id"] = id;
                        ViewData["hovaten"] = hovaten;
                        KhachHang khachHang = khachHangService.GetKhachHangbyid(id);
                        Modeldata modeldata = new Modeldata
                        {
                            khachhang = khachHang,
                            listphong = phonglisst,
                            khachSan = khachSan,
                        };
                        return View(modeldata);
                    }
                    else
                    {
                        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        if (result != null && result.Succeeded)
                        {
                            var name = result.Principal.FindFirstValue(ClaimTypes.Name);
                            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
                            var profileLink = result.Principal.FindFirstValue("urn:google:profile");
                            var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                            var userProfileViewModel = new UserProfileViewModel
                            {
                                Name = name,
                                Email = email,
                                ProfileLink = profileLink,
                                UserId = userId
                            };
                            Modeldata modeldata = new Modeldata
                            {
                                userProfileViewModel = userProfileViewModel,
                                listphong = phonglisst,
                                khachSan = khachSan,
                            };
                            return View(modeldata);
                        }
                        else
                        {
                            Modeldata modeldata = new Modeldata
                            {
                                listphong = phonglisst,
                                khachSan = khachSan,
                            };
                            return View(modeldata);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("", "home");
                }
            }
            else
            {
                return RedirectToAction("", "home");
            }
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