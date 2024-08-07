using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Auth0.ManagementApi.Paging;
using Model.Models;
using Service;
using System.Globalization;
using PagedList;
using Service.Service;
using Org.BouncyCastle.Crypto;
using System.Text.Json;

namespace QuanLyKhachSan_MVC.NET.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class ThuePhongController : Controller
    {
        private readonly DatPhongService datPhongService;
        private readonly KhachHangService khachHangService;
        private readonly PhongService phongService;
        private readonly NhanPhongService nhanPhongService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly ThoiGianService thoiGianService;
        private readonly MaGiamGiaService maGiamGiaService;
        private readonly SuDungMaGiamGiaService dungMaGiamGiaService;
        private readonly SuDungMaGiamGiaService suDungMaGiamGiaService;
        private readonly GiamGiaNgayLeService giamGiaNgayLeService;
        private readonly LoaiDichDichVuService loaiDichDichVuService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GopDonDatPhongService gopDonDatPhongService;

        public ThuePhongController(DatPhongService datPhongServices,
            KhachHangService khachHangServices,
            PhongService phongServices,
            NhanPhongService nhanPhongServices,
            SanPhamService sanPhamServices,
            ThueSanPhamService thueSanPhamServices,
            ThoiGianService thoiGianServices,
            MaGiamGiaService maGiamGiaServices,
            SuDungMaGiamGiaService dungMaGiamGiaServices,
            SuDungMaGiamGiaService suDungMaGiamGiaServices,
            LoaiDichDichVuService loaiDichDichVuService,
            GiamGiaNgayLeService giamGiaNgayLeService,
            IHttpContextAccessor httpContextAccessor,
            GopDonDatPhongService gopDonDatPhongService)
        {
            datPhongService = datPhongServices;
            khachHangService = khachHangServices;
            phongService = phongServices;
            nhanPhongService = nhanPhongServices;
            sanPhamService = sanPhamServices;
            thueSanPhamService = thueSanPhamServices;
            thoiGianService = thoiGianServices;
            maGiamGiaService = maGiamGiaServices;
            dungMaGiamGiaService = dungMaGiamGiaServices;
            suDungMaGiamGiaService = suDungMaGiamGiaServices;
            this.giamGiaNgayLeService = giamGiaNgayLeService;
            this.loaiDichDichVuService = loaiDichDichVuService;
            _httpContextAccessor = httpContextAccessor;
            this.gopDonDatPhongService = gopDonDatPhongService;
        }



        public IActionResult GetDatPhongByIdPhong(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            return Json(datPhong);
        }


        public IActionResult ListPhongTrangThaiDaDat()
        {
            int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
            ViewData["idkhachsan"] = idkhachsan;
            List<Phong> phongtrangthai = phongService.GetAllPhongIDKhachSan(idkhachsan).Where(p => p.tinhtrangphong == "có khách").ToList();
            return Json(phongtrangthai);
        }

        public IActionResult GopDonDatPhong(int idphong, int idphongmoi)
        {
            Phong phong = phongService.GetPhongID(idphong);
            phong.tinhtrangphong = "chưa dọn";
            phongService.CapNhatPhong(phong);
            DatPhong datPhongcu = datPhongService.GetDatPhongByIDTrangThai(idphong);
            datPhongcu.trangthai = "đã gộp";
            datPhongService.UpdateDatPhong(datPhongcu);
            DatPhong datphongidmoi = datPhongService.GetDatPhongByIDTrangThai(idphongmoi);

            datphongidmoi.tiendatcoc = datPhongcu.tiendatcoc + datphongidmoi.tiendatcoc;
            datPhongService.UpdateDatPhong(datphongidmoi);

            GopDonDatPhong gopDonDatPhong = new GopDonDatPhong();

            if (datPhongcu.hinhthucthue == "Theo giờ")
            {
                gopDonDatPhong.tienphong = (datPhongcu.ngaydukientra.Hour - datPhongcu.ngaydat.Hour) * phong.giatientheogio;
            }
            else
            {
                gopDonDatPhong.tienphong = (datPhongcu.ngaydukientra.Day - datPhongcu.ngaydat.Day) * phong.giatientheongay;
            }

            gopDonDatPhong.iddatphongcu = datPhongcu.id;
            gopDonDatPhong.iddatphongmoi = datphongidmoi.id;
            gopDonDatPhongService.Create(gopDonDatPhong);

            List<ThueSanPham> listthueSanPham = thueSanPhamService.GetAllThueSanPhamID(datPhongcu.id);
            foreach (var thuesanpham in listthueSanPham)
            {
                ThueSanPham thueSanPhamididdatphongmoi = thueSanPhamService.GetThueSanPhamByDatPhongAndSanPham(datphongidmoi.id, thuesanpham.idsanpham);
                SanPham sanpham = sanPhamService.GetSanPhamByID(thuesanpham.idsanpham);
                if (thueSanPhamididdatphongmoi != null)
                {
                    thueSanPhamididdatphongmoi.soluong += thuesanpham.soluong;
                    thueSanPhamididdatphongmoi.ghichu = $"Gộp từ phòng {phong.sophong}, số lượng {thuesanpham.soluong}";
                    thueSanPhamididdatphongmoi.thanhtien = thueSanPhamididdatphongmoi.soluong * sanpham.giaban;
                    thueSanPhamService.CapNhatThueSanPham(thueSanPhamididdatphongmoi);
                    List<ThueSanPham> thueSanPhams = thueSanPhamService.GetThueSanPhamByIDdatphong(datPhongcu.id);
                    foreach (var xoathuesanpham in thueSanPhams)
                    {
                        thueSanPhamService.XoaThueSanPham(xoathuesanpham.id);
                    }
                }
                else
                {
                    thuesanpham.iddatphong = datphongidmoi.id;
                    thuesanpham.ghichu = $"Gộp từ phòng {phong.sophong}, số lượng {thuesanpham.soluong}";
                    thuesanpham.thanhtien = thuesanpham.soluong * sanpham.giaban;
                    thueSanPhamService.CapNhatThueSanPham(thuesanpham);
                }
            }
            TempData["gopdonthanhcong"] = "";
            return Redirect("~/staff/phong/");
        }


        public IActionResult DatPhongDon(int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                Phong phong = phongService.GetPhongID(idphong);
                List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();
                Modeldata yourModel = new Modeldata
                {
                    phong = phong,
                    loaiDichVus = loaiDichVus,
                };
                return View(yourModel);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }

        public IActionResult TachDon(int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                Phong phong = phongService.GetPhongID(idphong);
                DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
                List<Phong> phonglist = phongService.GetAllPhongTrangThai(idkhachsan);
                Modeldata yourModel = new Modeldata
                {
                    phong = phong,
                    datPhong = datPhong,
                    listphong = phonglist,
                };
                return View(yourModel);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        private List<ThueSanPham> GetCartItems()
        {
            var cartItemsJson = _httpContextAccessor.HttpContext.Session.GetString("examplesss");

            List<ThueSanPham> cartItemList = new List<ThueSanPham>();

            if (!string.IsNullOrEmpty(cartItemsJson))
            {
                cartItemList = JsonSerializer.Deserialize<List<ThueSanPham>>(cartItemsJson);
            }
            return cartItemList;
        }

        public IActionResult AddThuePhongTachDon(DatPhong datPhong, KhachHang khachHang, int idphongmoi)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                ThoiGian thoiGian = thoiGianService.GetThoiGian(HttpContext.Session.GetInt32("idkhachsan").Value);
                NhanPhong nhanPhong = new NhanPhong();
                Phong phong = phongService.GetPhongID(idphongmoi);
                KhachHang khachHangByEmail = khachHangService.GetKhachHangbyemail(khachHang.email);
                if (khachHangByEmail != null)
                {
                    /// cập nhật khách hàng
                    khachHangByEmail.cccd = khachHang.cccd;
                    khachHangByEmail.sodienthoai = khachHang.sodienthoai;
                    khachHangByEmail.tinh = khachHang.tinh;
                    khachHangByEmail.huyen = khachHang.huyen;
                    khachHangByEmail.phuong = khachHang.phuong;
                    khachHangService.CapNhatKhachHang(khachHangByEmail);
                    /// thêm đặt phòng
                    datPhong.idthoigian = thoiGian.id;
                    datPhong.idkhachhang = khachHangByEmail.id;
                    datPhong.loaidatphong = "đặt phòng đơn";
                    datPhong.trangthai = "đã đặt";
                    datPhong.ngaydat = DateTime.Now;
                    datPhong.idphong = idphongmoi;
                    TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                    if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                    {
                        datPhong.hinhthucthue = "Theo ngày";
                    }
                    else
                    {
                        datPhong.hinhthucthue = "Theo giờ";
                    }
                    int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                    List<ThueSanPham> cartItems = GetCartItems();
                    /// thêm dịch vụ vào đặt phòng
                    foreach (var cartItem in cartItems)
                    {
                        SanPham sanpham = sanPhamService.GetSanPhamByID(cartItem.idsanpham);
                        ThueSanPham addthuesanpham = new ThueSanPham();
                        addthuesanpham.idnhanvien = idnv;
                        addthuesanpham.soluong = cartItem.soluong;
                        addthuesanpham.idsanpham = sanpham.id;
                        addthuesanpham.iddatphong = idDatPhongThemVao;
                        addthuesanpham.thanhtien = cartItem.soluong * sanpham.giaban;
                        addthuesanpham.ghichu = $"Được tách từ phòng: {phong.sophong}, số lượng: {cartItem.soluong}";
                        thueSanPhamService.ThueSanPham(addthuesanpham);
                    }
                    _httpContextAccessor.HttpContext.Session.Remove("examplesss");
                    /// thêm nhận phòng
                    nhanPhong.idnhanvien = idnv;
                    nhanPhong.iddatphong = idDatPhongThemVao;
                    nhanPhong.ngaynhanphong = DateTime.Now;
                    nhanPhongService.ThemNhanPhong(nhanPhong);
                    /// cập nhật trạng thái phòng
                    phong.tinhtrangphong = "có khách";
                    phongService.CapNhatPhong(phong);
                    /// gửi mã giảm giá nếu có
                    int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangByEmail.id);
                    MaGiamGia guimamaGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                    if (guimamaGiamgia != null)
                    {
                        if (guimamaGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu && guimamaGiamgia.trangthai.Equals("còn sử dụng"))
                        {
                            maGiamGiaService.GuiEmail(khachHang, guimamaGiamgia.magiamgia);
                        }
                        else
                        {
                            /// ko có mã giảm giá
                        }
                    }
                    else
                    {
                        //// null
                    }
                    // gửi mail
                    datPhongService.GuiEmail(khachHang, datPhong, phong, thoiGian);
                }
                else
                {
                    /// thêm khách hàng
                    khachHang.trangthai = "còn hoạt động";
                    khachHangService.ThemKhachHang(khachHang);
                    KhachHang khachhangmoi = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                    /// thêm đăht phòng
                    datPhong.idkhachhang = khachhangmoi.id;
                    datPhong.idthoigian = thoiGian.id;
                    datPhong.loaidatphong = "đặt phòng đơn";
                    datPhong.trangthai = "đã đặt";
                    datPhong.ngaydat = DateTime.Now;
                    datPhong.idphong = idphongmoi;
                    TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                    if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                    {
                        datPhong.hinhthucthue = "Theo ngày";
                    }
                    else
                    {
                        datPhong.hinhthucthue = "Theo giờ";
                    }
                    int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);

                    List<ThueSanPham> cartItems = GetCartItems();
                    /// thêm dịch vụ vào đặt phòng
                    foreach (var cartItem in cartItems)
                    {
                        SanPham sanpham = sanPhamService.GetSanPhamByID(cartItem.idsanpham);
                        ThueSanPham addthuesanpham = new ThueSanPham();
                        addthuesanpham.idnhanvien = idnv;
                        addthuesanpham.soluong = cartItem.soluong;
                        addthuesanpham.idsanpham = sanpham.id;
                        addthuesanpham.iddatphong = idDatPhongThemVao;
                        addthuesanpham.thanhtien = cartItem.soluong * sanpham.giaban;
                        addthuesanpham.ghichu = $"Được tách từ phòng: {phong.sophong}, số lượng: {cartItem.soluong}";
                        thueSanPhamService.ThueSanPham(addthuesanpham);
                    }
                    _httpContextAccessor.HttpContext.Session.Remove("examplesss");

                    // thêm nhận phòng
                    nhanPhong.idnhanvien = idnv;
                    nhanPhong.iddatphong = idDatPhongThemVao;
                    nhanPhong.ngaynhanphong = DateTime.Now;
                    nhanPhongService.ThemNhanPhong(nhanPhong);
                    /// cập nhật tình trạng phòng
                    phong.tinhtrangphong = "có khách";
                    phongService.CapNhatPhong(phong);
                    /// gửi email
                    datPhongService.GuiEmail(khachHang, datPhong, phong, thoiGian);
                }
                TempData["tachdonthanhcong"] = "";
                return Redirect("~/staff/phong/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }

        public IActionResult ThemThuePhong(KhachHang khachHang, DatPhong datPhong, List<int> idsanpham, string nhanphong, string magiamgia)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                Phong checkphongtrong = phongService.GetPhongID(datPhong.idphong);
                if (checkphongtrong.tinhtrangphong == "còn trống")
                {
                    NhanPhong nhanPhong = new NhanPhong();
                    Phong phong = phongService.GetPhongID(datPhong.idphong);
                    int idnd = HttpContext.Session.GetInt32("id").Value;
                    /// kiểm tra xem khách hàng đã tồn tại hay chưa
                    KhachHang khachHangByEmail = khachHangService.GetKhachHangbyemail(khachHang.email);
                    ThoiGian thoiGian = thoiGianService.GetThoiGian(HttpContext.Session.GetInt32("idkhachsan").Value);
                    if (khachHangByEmail != null)
                    {

                        khachHangByEmail.cccd = khachHang.cccd;
                        khachHangByEmail.sodienthoai = khachHang.cccd;
                        khachHangByEmail.tinh = khachHang.tinh;
                        khachHangByEmail.huyen = khachHang.huyen;
                        khachHangByEmail.phuong = khachHang.phuong;
                        khachHangService.CapNhatKhachHang(khachHangByEmail);
                        if (nhanphong != null)
                        {
                            datPhong.idthoigian = thoiGian.id;
                            datPhong.idkhachhang = khachHangByEmail.id;
                            datPhong.loaidatphong = "đặt phòng đơn";
                            datPhong.trangthai = "đã đặt";
                            datPhong.ngaydat = DateTime.Now;
                            datPhong.idphong = datPhong.idphong;
                            TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                            if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                            {
                                datPhong.hinhthucthue = "Theo ngày";
                            }
                            else
                            {
                                datPhong.hinhthucthue = "Theo giờ";
                            }
                            int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                            if (idsanpham != null && idsanpham.Any())
                            {
                                ThueSanPham thueSanPham = new ThueSanPham();
                                foreach (int idsp in idsanpham)
                                {
                                    SanPham sanpham = sanPhamService.GetSanPhamByID(idsp);
                                    thueSanPham.idnhanvien = idnd;
                                    thueSanPham.soluong = 1;
                                    thueSanPham.idsanpham = idsp;
                                    thueSanPham.iddatphong = idDatPhongThemVao;
                                    sanpham.soluongcon -= 1;
                                    sanPhamService.CapNhatSanPham(sanpham);
                                    thueSanPham.thanhtien = 1 * sanpham.giaban;
                                    thueSanPhamService.ThueSanPham(thueSanPham);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Không thuê được sản phẩm");
                            }
                            nhanPhong.idnhanvien = idnd;
                            nhanPhong.iddatphong = idDatPhongThemVao;
                            nhanPhong.ngaynhanphong = DateTime.Now;
                            nhanPhongService.ThemNhanPhong(nhanPhong);
                            phong.tinhtrangphong = "có khách";
                            phongService.CapNhatPhong(phong);
                            int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangByEmail.id);
                            MaGiamGia guimamaGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                            if (guimamaGiamgia != null)
                            {
                                if (guimamaGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu && guimamaGiamgia.trangthai.Equals("còn sử dụng"))
                                {
                                    maGiamGiaService.GuiEmail(khachHang, guimamaGiamgia.magiamgia);
                                }
                                else
                                {
                                    /// ko có mã giảm giá
                                }
                            }
                            else
                            {
                                //// null
                            }
                            if (magiamgia != null)
                            {
                                MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                                if (maGiamGia != null)
                                {
                                    if (maGiamGia.ngayketthuc.Year > DateTime.Now.Year ||
                                       (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month > DateTime.Now.Month) ||
                                       (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month == DateTime.Now.Month &&
                                       maGiamGia.ngayketthuc.Day > DateTime.Now.Day))
                                    {
                                        if (maGiamGia.trangthai.Equals("còn sử dụng"))
                                        {
                                            if (maGiamGia.solandasudung < maGiamGia.solansudungtoida)
                                            {
                                                SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia();
                                                suDungMaGiamGia.idmagiamgia = maGiamGia.id;
                                                suDungMaGiamGia.iddatphong = idDatPhongThemVao;
                                                suDungMaGiamGia.ngaysudung = DateTime.Now;
                                                dungMaGiamGiaService.ThemSuDungMaGiamGia(suDungMaGiamGia);
                                                /// cập nhật số lần sử dụng tăng lên 1
                                                maGiamGia.solandasudung = maGiamGia.solandasudung + 1;
                                                maGiamGiaService.CapNhatMaGiamGia(maGiamGia);
                                            }
                                            else
                                            {
                                                /// 
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // nulll
                                    }
                                }
                                else
                                {
                                    // null
                                }
                            }
                            else
                            {
                                /// không áp dụng mã giảm giá
                            }
                        }
                        else
                        {
                            datPhong.idthoigian = thoiGian.id;
                            datPhong.idkhachhang = khachHangByEmail.id;
                            datPhong.loaidatphong = "đặt phòng đơn";
                            datPhong.trangthai = "đã đặt online";
                            datPhong.ngaydat = DateTime.Now;
                            datPhong.idphong = datPhong.idphong;
                            TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                            if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                            {
                                datPhong.hinhthucthue = "Theo ngày";
                            }
                            else
                            {
                                datPhong.hinhthucthue = "Theo giờ";
                            }
                            int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                            phong.tinhtrangphong = "đã đặt";
                            phongService.CapNhatPhong(phong);
                            int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangByEmail.id);
                            MaGiamGia maGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                            if (maGiamgia != null)
                            {
                                if (maGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu && maGiamgia.trangthai.Equals("còn sử dụng"))
                                {
                                    maGiamGiaService.GuiEmail(khachHang, maGiamgia.magiamgia);
                                }
                                else
                                {
                                    /// ko có mã giảm giá
                                }
                            }
                            else
                            {
                                //// null
                            }
                            if (magiamgia != null)
                            {
                                MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                                if (maGiamGia != null)
                                {
                                    if (maGiamGia.ngayketthuc.Year > DateTime.Now.Year ||
                                   (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month > DateTime.Now.Month) ||
                                   (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month == DateTime.Now.Month &&
                                   maGiamGia.ngayketthuc.Day > DateTime.Now.Day))
                                    {
                                        if (maGiamGia.trangthai.Equals("còn sử dụng"))
                                        {
                                            if (maGiamGia.solandasudung < maGiamGia.solansudungtoida)
                                            {
                                                SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia();
                                                suDungMaGiamGia.idmagiamgia = maGiamGia.id;
                                                suDungMaGiamGia.iddatphong = idDatPhongThemVao;
                                                suDungMaGiamGia.ngaysudung = DateTime.Now;
                                                dungMaGiamGiaService.ThemSuDungMaGiamGia(suDungMaGiamGia);
                                                /// cập nhật số lần sử dụng tăng lên 1
                                                maGiamGia.solandasudung = maGiamGia.solandasudung + 1;
                                                maGiamGiaService.CapNhatMaGiamGia(maGiamGia);
                                            }
                                            else
                                            {
                                                /// 
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // nulll
                                    }
                                }
                                else
                                {
                                    // null
                                }
                            }
                            else
                            {
                                /// không áp dụng mã giảm giá
                            }
                        }
                        datPhongService.GuiEmail(khachHang, datPhong, phong, thoiGian);
                    }
                    else
                    {
                        if (nhanphong != null)
                        {
                            khachHang.trangthai = "còn hoạt động";
                            khachHangService.ThemKhachHang(khachHang);
                            KhachHang khachhangmoi = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                            datPhong.idkhachhang = khachhangmoi.id;
                            datPhong.idthoigian = thoiGian.id;
                            datPhong.loaidatphong = "đặt phòng đơn";
                            datPhong.trangthai = "đã đặt";
                            datPhong.ngaydat = DateTime.Now;
                            datPhong.idphong = datPhong.idphong;
                            TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                            if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                            {
                                datPhong.hinhthucthue = "Theo ngày";
                            }
                            else
                            {
                                datPhong.hinhthucthue = "Theo giờ";
                            }
                            int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                            nhanPhong.idnhanvien = idnd;
                            nhanPhong.iddatphong = idDatPhongThemVao;
                            nhanPhong.ngaynhanphong = DateTime.Now;
                            nhanPhongService.ThemNhanPhong(nhanPhong);
                            phong.tinhtrangphong = "có khách";
                            phongService.CapNhatPhong(phong);
                            if (idsanpham != null && idsanpham.Any())
                            {
                                ThueSanPham thueSanPham = new ThueSanPham();
                                foreach (int idsp in idsanpham)
                                {
                                    SanPham sanpham = sanPhamService.GetSanPhamByID(idsp);
                                    thueSanPham.idnhanvien = idnd;
                                    thueSanPham.soluong = 1;
                                    thueSanPham.idsanpham = idsp;
                                    thueSanPham.iddatphong = idDatPhongThemVao;
                                    sanpham.soluongcon -= 1;
                                    sanPhamService.CapNhatSanPham(sanpham);
                                    thueSanPham.thanhtien = 1 * sanpham.giaban;
                                    thueSanPhamService.ThueSanPham(thueSanPham);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Không thuê được sản phẩm");
                            }
                        }
                        else
                        {
                            khachHang.trangthai = "còn hoạt động";
                            khachHangService.ThemKhachHang(khachHang);
                            KhachHang khachhangmoi = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                            datPhong.idkhachhang = khachhangmoi.id;
                            datPhong.idthoigian = thoiGian.id;
                            datPhong.loaidatphong = "đặt phòng đơn";
                            datPhong.trangthai = "đã đặt online";
                            datPhong.ngaydat = DateTime.Now;
                            datPhong.idphong = datPhong.idphong;
                            TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                            if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                            {
                                datPhong.hinhthucthue = "Theo ngày";
                            }
                            else
                            {
                                datPhong.hinhthucthue = "Theo giờ";
                            }
                            int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                            phong.tinhtrangphong = "đã đặt";
                            phongService.CapNhatPhong(phong);
                        }

                        datPhongService.GuiEmail(khachHang, datPhong, phong, thoiGian);
                    }
                    TempData["thuephongthanhcong"] = "";
                    return Redirect("~/staff/phong/");
                }
                else
                {
                    TempData["phongdaconguoithue"] = "";
                    return Redirect($"~/staff/phong?id={checkphongtrong.id}");
                }
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }

        public IActionResult Doiloaihinhthue(int iddatphong, DateTime? ngaydukientra)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDDatPhong(iddatphong);
            if (ngaydukientra.HasValue)
            {
                TimeSpan sogio = ngaydukientra.Value - datPhong.ngaydat;
                if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                {
                    datPhong.hinhthucthue = "Theo ngày";
                }
                else
                {
                    datPhong.hinhthucthue = "Theo giờ";
                }
                datPhong.ngaydukientra = ngaydukientra.Value;
                datPhongService.UpdateDatPhong(datPhong);
                TempData["doiloaihinhthue"] = "success";
                return Redirect($"/staff/thuephong/chitietthuephong?idphong={datPhong.idphong}");
            }
            else
            {
                TempData["doiloaihinhthue"] = "error";
                return Redirect($"/staff/thuephong/chitietthuephong?idphong={datPhong.idphong}");
            }
        }



        public IActionResult DatPhongDoan(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<Phong> listphong = phongService.GetAllPhongTrangThai(HttpContext.Session.GetInt32("idkhachsan").Value);
                int soluong = listphong.Count;
                int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                PagedList.IPagedList<Phong> ipagelistphong = listphong.ToPagedList(validPageNumber, pageSize);
                List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();

                Modeldata yourModel = new Modeldata
                {
                    PagedTPhong = ipagelistphong,
                };
                return View(yourModel);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult ThueNhieuPhong(KhachHang khachHang, DatPhong datPhong, NhanPhong nhanPhong, List<int> idphongs, string magiamgia)
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                if (idphongs != null && idphongs.Any())
                {
                    foreach (int idphong in idphongs)
                    {
                        KhachHang khachHangByEmail = khachHangService.GetKhachHangbyemail(khachHang.email);
                        ThoiGian thoiGian = thoiGianService.GetThoiGian(idkhachsan);
                        if (khachHangByEmail != null)
                        {
                            khachHangByEmail.cccd = khachHang.cccd;
                            khachHangByEmail.sodienthoai = khachHang.cccd;
                            khachHangByEmail.tinh = khachHang.tinh;
                            khachHangByEmail.huyen = khachHang.huyen;
                            khachHangByEmail.phuong = khachHang.phuong;
                            khachHangService.CapNhatKhachHang(khachHangByEmail);
                            datPhong.idthoigian = thoiGian.id;
                            datPhong.idkhachhang = khachHangByEmail.id;
                            datPhong.loaidatphong = "đặt phòng theo đoàn";
                            datPhong.trangthai = "đã đặt";
                            datPhong.ngaydat = DateTime.Now;
                            datPhong.idphong = idphong;
                            TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                            if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                            {
                                datPhong.hinhthucthue = "Theo ngày";
                            }
                            else
                            {
                                datPhong.hinhthucthue = "Theo giờ";
                            }
                            int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                            nhanPhong.iddatphong = idDatPhongThemVao;
                            nhanPhong.ngaynhanphong = DateTime.Now;
                            nhanPhongService.ThemNhanPhong(nhanPhong);
                            Phong phong = phongService.GetPhongID(idphong);
                            phong.tinhtrangphong = "có khách";
                            phongService.CapNhatPhong(phong);
                            int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangByEmail.id);
                            MaGiamGia guimaGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                            if (guimaGiamgia != null)
                            {
                                if (guimaGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu && guimaGiamgia.trangthai.Equals("còn sử dụng"))
                                {
                                    maGiamGiaService.GuiEmail(khachHang, guimaGiamgia.magiamgia);
                                }
                                else
                                {
                                    /// ko có mã giảm giá
                                }
                            }
                            else
                            {
                                //// null
                            }
                            if (magiamgia != null)
                            {
                                MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                                if (maGiamGia != null)
                                {
                                    if (maGiamGia.ngayketthuc.Year > DateTime.Now.Year ||
                                   (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month > DateTime.Now.Month) ||
                                   (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month == DateTime.Now.Month &&
                                   maGiamGia.ngayketthuc.Day > DateTime.Now.Day))
                                    {
                                        if (maGiamGia.trangthai.Equals("còn sử dụng"))
                                        {
                                            if (maGiamGia.solandasudung < maGiamGia.solansudungtoida)
                                            {
                                                SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia();
                                                suDungMaGiamGia.idmagiamgia = maGiamGia.id;
                                                suDungMaGiamGia.iddatphong = idDatPhongThemVao;
                                                suDungMaGiamGia.ngaysudung = DateTime.Now;
                                                dungMaGiamGiaService.ThemSuDungMaGiamGia(suDungMaGiamGia);
                                                /// cập nhật số lần sử dụng tăng lên 1
                                                maGiamGia.solandasudung = maGiamGia.solandasudung + 1;
                                                maGiamGiaService.CapNhatMaGiamGia(maGiamGia);
                                            }
                                            else
                                            {
                                                /// 
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // nulll
                                    }
                                }
                                else
                                {
                                    // null
                                }
                            }
                            else
                            {
                                /// không áp dụng mã giảm giá
                            }
                            datPhongService.GuiEmail(khachHang, datPhong, phong, thoiGian);
                        }
                        else
                        {
                            khachHang.trangthai = "còn hoạt động";
                            khachHangService.ThemKhachHang(khachHang);
                            KhachHang khachhangmoi = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                            datPhong.idthoigian = thoiGian.id;
                            datPhong.idkhachhang = khachhangmoi.id;
                            datPhong.loaidatphong = "đặt phòng theo đoàn";
                            datPhong.trangthai = "đã đặt";
                            datPhong.ngaydat = DateTime.Now;
                            datPhong.idphong = idphong;
                            TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                            if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                            {
                                datPhong.hinhthucthue = "Theo ngày";
                            }
                            else
                            {
                                datPhong.hinhthucthue = "Theo giờ";
                            }
                            int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                            nhanPhong.iddatphong = idDatPhongThemVao;
                            nhanPhong.ngaynhanphong = DateTime.Now;
                            nhanPhongService.ThemNhanPhong(nhanPhong);
                            Phong phong = phongService.GetPhongID(idphong);
                            phong.tinhtrangphong = "có khách";
                            phongService.CapNhatPhong(phong);
                            datPhongService.GuiEmail(khachHang, datPhong, phong, thoiGian);
                        }
                    }
                    TempData["thuephongthanhcong"] = "";
                    return Redirect("~/staff/phong/");
                }
                return RedirectToAction("Index", "Index1");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult ChiTietThuePhong(int idphong, int? sotrang)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<DatPhong> listdatPhongs = datPhongService.GetAllDatPhongByID(idphong);
                Phong phongs = phongService.GetPhongID(idphong);
                List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();
                List<Modeldata> listmodeldatas = new List<Modeldata>();
                if (listdatPhongs != null && listdatPhongs.Any())
                {
                    foreach (var datphong in listdatPhongs)
                    {
                        List<ThueSanPham> listthueSanPham = thueSanPhamService.GetAllThueSanPhamID(datphong.id);
                        float tongtien = 0;
                        foreach (var thueSanPham in listthueSanPham)
                        {
                            tongtien += thueSanPham.thanhtien;
                        }
                        ThoiGian thoiGian = thoiGianService.GetThoiGianById(datphong.idthoigian);
                        GiamGiaNgayLe giamGiaNgayLe = giamGiaNgayLeService.GetGiamGiaNgayLeByNgayLe(DateTime.Today);
                        SuDungMaGiamGia sudunggiamGia = suDungMaGiamGiaService.GetSuDungMaGiamGiaByIddatphong(datphong.id);
                        MaGiamGia maGiamGia = sudunggiamGia != null ? maGiamGiaService.GetMaGiamGiaById(sudunggiamGia.idmagiamgia) : null;
                        GopDonDatPhong gopDonDatPhong = gopDonDatPhongService.GetByIdDatPhongMoi(datphong.id);
                        Modeldata yourModel = new Modeldata
                        {
                            datPhong = datphong,
                            listthueSanPham = listthueSanPham,
                            tongtienhueSanPham = tongtien,
                            phong = phongs,
                            magiamGia = maGiamGia,
                            thoigian = thoiGian,
                            giamGiaNgayle = giamGiaNgayLe,
                            loaiDichVus = loaiDichVus,
                            gopDonDatPhong = gopDonDatPhong != null ? gopDonDatPhong : null // Check if gopDonDatPhong is null
                        };
                        listmodeldatas.Add(yourModel);
                    }
                }
                else
                {
                }
                return View(listmodeldatas);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }

        public IActionResult CheckInDatPhongOnline(KhachHang khachHang, DatPhong datPhong, string magiamgia, int idkhachsan)
        {
            Phong checktinhtrangphong = phongService.GetPhongID(datPhong.idphong);
            if (checktinhtrangphong.tinhtrangphong == "còn trống")
            {
                /// kiểm tra xem khách hàng đã tồn tại hay chưa
                KhachHang khachHangByEmail = khachHangService.GetKhachHangbyemail(khachHang.email);
                ThoiGian thoiGian = thoiGianService.GetThoiGian(idkhachsan);
                if (khachHangByEmail != null)
                {
                    khachHangByEmail.cccd = khachHang.cccd;
                    khachHangByEmail.sodienthoai = khachHang.cccd;

                    khachHangService.CapNhatKhachHang(khachHangByEmail);
                    datPhong.idthoigian = thoiGian.id;
                    /// id khách hàng
                    datPhong.idkhachhang = khachHangByEmail.id;
                    datPhong.loaidatphong = "đặt phòng đơn";
                    datPhong.trangthai = "đã đặt online";
                    datPhong.ngaydat = DateTime.Now;
                    datPhong.idphong = datPhong.idphong;
                    TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                    if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                    {
                        datPhong.hinhthucthue = "Theo ngày";
                    }
                    else
                    {
                        datPhong.hinhthucthue = "Theo giờ";
                    }
                    /// thực hiện thêm đặt phòng và lấy ra id đặt phòng mới tạo đó
                    int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                    /// thực hiện thêm đặt phòng và lấy ra id đặt phòng mới tạo đó
                    Phong phong = phongService.GetPhongID(datPhong.idphong);
                    phong.tinhtrangphong = "đã đặt";
                    phongService.CapNhatPhong(phong);
                    datPhongService.GuiEmail(khachHang, datPhong, phong, thoiGian);
                    int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangByEmail.id);
                    MaGiamGia guimamaGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                    if (guimamaGiamgia != null)
                    {
                        if (guimamaGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu && guimamaGiamgia.trangthai.Equals("còn sử dụng"))
                        {
                            maGiamGiaService.GuiEmail(khachHang, guimamaGiamgia.magiamgia);
                        }
                        else
                        {
                            /// ko có mã giảm giá
                        }
                    }
                    else
                    {
                        //// null
                    }
                    if (magiamgia != null)
                    {
                        MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                        if (maGiamGia != null)
                        {
                            if (maGiamGia.ngayketthuc.Year > DateTime.Now.Year ||
                           (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month > DateTime.Now.Month) ||
                           (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month == DateTime.Now.Month &&
                           maGiamGia.ngayketthuc.Day > DateTime.Now.Day))
                            {
                                if (maGiamGia.solandasudung < maGiamGia.solansudungtoida && maGiamGia.trangthai.Equals("còn sử dụng"))
                                {
                                    SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia();
                                    suDungMaGiamGia.idmagiamgia = maGiamGia.id;
                                    suDungMaGiamGia.iddatphong = idDatPhongThemVao;
                                    suDungMaGiamGia.ngaysudung = DateTime.Now;
                                    dungMaGiamGiaService.ThemSuDungMaGiamGia(suDungMaGiamGia);
                                    /// cập nhật số lần sử dụng tăng lên 1
                                    maGiamGia.solandasudung = maGiamGia.solandasudung + 1;
                                    maGiamGiaService.CapNhatMaGiamGia(maGiamGia);
                                }
                                else
                                {
                                    /// 
                                }
                            }
                            else
                            {
                                // nulll
                            }
                        }
                        else
                        {
                            // null
                        }
                    }
                    else
                    {
                        /// không áp dụng mã giảm giá
                    }
                }
                /// nếu khách hàng không tồn tại
                else
                {
                    /// khách hàng không tông tại thì thực hiện thêm khách hàng mới
                    khachHang.trangthai = "còn hoạt động";
                    khachHang.tinh = "không có";
                    khachHang.phuong = "không có";
                    khachHang.huyen = "không có";
                    khachHangService.ThemKhachHang(khachHang);
                    /// lấy id khách hnagf mới tạo từ cccd để thực hiện việc đặt phòng
                    KhachHang khachhangmoi = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                    // id khách hàng
                    datPhong.idkhachhang = khachhangmoi.id;
                    datPhong.idthoigian = thoiGian.id;
                    datPhong.loaidatphong = "đặt phòng đơn";
                    datPhong.trangthai = "đã đặt online";
                    datPhong.ngaydat = DateTime.Now;
                    datPhong.idphong = datPhong.idphong;
                    TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                    if (sogio.TotalDays >= 1 && sogio.TotalHours >= 24)
                    {
                        datPhong.hinhthucthue = "Theo ngày";
                    }
                    else
                    {
                        datPhong.hinhthucthue = "Theo giờ";
                    }
                    /// thực hiện thêm đặt phòng và lấy ra id đặt phòng mới tạo đó
                    datPhongService.ThemDatPhong(datPhong);
                    /// lấy thoong tin phòng ra từ id phòng của id đặt phòng mới tạo để thực hiện việc cập nhật trạng thái phòng đó
                    Phong phong = phongService.GetPhongID(datPhong.idphong);
                    phong.tinhtrangphong = "đã đặt";
                    phongService.CapNhatPhong(phong);
                    datPhongService.GuiEmail(khachHang, datPhong, phong, thoiGian);
                }
                return RedirectToAction("index", "home");
            }
            else
            {
                return RedirectToAction("datphongonline", "phong", new { idphong = datPhong.idphong });
            }
        }

    }
}