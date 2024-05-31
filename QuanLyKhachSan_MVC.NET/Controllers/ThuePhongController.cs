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

namespace QuanLyKhachSan_MVC.NET.Controllers
{
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
        private readonly LoaiDatPhongService loaiDatPhongService;
        private readonly GiamGiaNgayLeService giamGiaNgayLeService;
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
            LoaiDatPhongService loaiDatPhongService, GiamGiaNgayLeService giamGiaNgayLeService)
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
            this.loaiDatPhongService = loaiDatPhongService;
            this.giamGiaNgayLeService = giamGiaNgayLeService;
        }
        public IActionResult Index(int id, int? sotrang)
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
                List<SanPham> listsanpham = sanPhamService.GetAllSanPhamIDKhachSan(idkhachsan);
                const int PageSize = 10; // Số lượng phòng trên mỗi trang
                int pageNumber = sotrang ?? 1; // Trang hiện tại, mặc định là trang 1
                PagedList.IPagedList<SanPham> PagedTSanPham = listsanpham.ToPagedList(pageNumber, PageSize);
                Phong phong = phongService.GetPhongID(id);
                Modeldata yourModel = new Modeldata
                {
                    phong = phong,
                    PagedTSanPham = PagedTSanPham,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult ThemThuePhong(KhachHang khachHang, DatPhong datPhong, List<int> idsanpham, string nhanphong, string magiamgia)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                NhanPhong nhanPhong = new NhanPhong();
                Phong phong = phongService.GetPhongID(datPhong.idphong);
                int idnd = HttpContext.Session.GetInt32("id").Value;
                /// kiểm tra xem khách hàng đã tồn tại hay chưa
                KhachHang khachHangTonTai = khachHangService.GetKhachHangbyemail(khachHang.email);
                ThoiGian thoiGian = thoiGianService.GetThoiGian(HttpContext.Session.GetInt32("idkhachsan").Value);
                if (khachHangTonTai != null)
                {
                    khachHangTonTai = new KhachHang
                    {
                        cccd = khachHang.cccd,
                        sodienthoai = khachHang.cccd,
                        tinh = khachHang.tinh,
                        huyen = khachHang.huyen,
                        phuong = khachHang.phuong,
                    };
                    khachHangService.CapNhatKhachHang(khachHangTonTai);
                    if (nhanphong != null)
                    {
                        datPhong.idthoigian = thoiGian.id;
                        datPhong.idkhachhang = khachHangTonTai.id;
                        datPhong.idloaidatphong = 1;
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
                        int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangTonTai.id);
                        MaGiamGia guimamaGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                        if (guimamaGiamgia != null)
                        {
                            if (guimamaGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu)
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
                            if (maGiamGia.ngayketthuc.Year > DateTime.Now.Year ||
                               (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month > DateTime.Now.Month) ||
                               (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month == DateTime.Now.Month &&
                               maGiamGia.ngayketthuc.Day > DateTime.Now.Day))
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
                            else
                            {
                                // nulll
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
                        datPhong.idkhachhang = khachHangTonTai.id;
                        datPhong.idloaidatphong = 1;
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
                        int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangTonTai.id);
                        MaGiamGia maGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                        if (maGiamgia != null)
                        {
                            if (maGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu)
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
                            if (maGiamGia.ngayketthuc.Year > DateTime.Now.Year ||
                               (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month > DateTime.Now.Month) ||
                               (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month == DateTime.Now.Month &&
                               maGiamGia.ngayketthuc.Day > DateTime.Now.Day))
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
                            else
                            {
                                // nulll
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
                        datPhong.idloaidatphong = 1;
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
                        datPhong.idloaidatphong = 1;
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
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        /*public IActionResult Index1(int? sotrang)
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
                const int PageSize = 10; // Số lượng phòng trên mỗi trang
                int pageNumber = sotrang ?? 1; // Trang hiện tại, mặc định là trang 1
                PagedList.IPagedList<Phong> ipagelistphong = listphong.ToPagedList(pageNumber, PageSize);
                Modeldata yourModel = new Modeldata
                {
                    PagedTPhong = ipagelistphong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }*/
        public IActionResult Index1(int? sotrang)
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
                Modeldata yourModel = new Modeldata
                {
                    PagedTPhong = ipagelistphong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
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
                        KhachHang khachHangTonTai = khachHangService.GetKhachHangbyemail(khachHang.email);
                        ThoiGian thoiGian = thoiGianService.GetThoiGian(idkhachsan);
                        if (khachHangTonTai != null)
                        {
                            khachHangTonTai = new KhachHang
                            {
                                cccd = khachHang.cccd,
                                sodienthoai = khachHang.cccd,
                                tinh = khachHang.tinh,
                                huyen = khachHang.huyen,
                                phuong = khachHang.phuong,
                            };
                            khachHangService.CapNhatKhachHang(khachHangTonTai);
                            datPhong.idthoigian = thoiGian.id;
                            datPhong.idkhachhang = khachHangTonTai.id;
                            datPhong.idloaidatphong = 2;
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
                            int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangTonTai.id);
                            MaGiamGia guimaGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                            if (guimaGiamgia != null)
                            {
                                if (guimaGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu)
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
                            datPhong.idloaidatphong = 2;
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
                    return RedirectToAction("Index", "Phong");
                }
                return RedirectToAction("Index", "Index1");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult ChiTietThuePhong(int id)
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
                List<DatPhong> listdatPhongs = datPhongService.GetAllDatPhongByID(id);
                List<SanPham> listsanpham = sanPhamService.GetAllSanPhamIDKhachSan(idkhachsan);
                Phong phongs = phongService.GetPhongID(id);
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
                        DateTime ngaytramuonsom = DateTime.Now;
                        if (thoiGian != null)
                        {
                            if (datphong.hinhthucthue == "Theo giờ")
                            {
                                TimeSpan ngayTraMuonSomAsTimeSpan = ngaytramuonsom.TimeOfDay;
                                if (ngayTraMuonSomAsTimeSpan > thoiGian.thoigianra)
                                {
                                    if (ngaytramuonsom > datphong.ngaydukientra && datphong.ngaydukientra.Hour > thoiGian.thoigianra.Hours)
                                    {
                                        datphong.hinhthucthue = "Theo ngày";
                                    }
                                    else
                                    {
                                        datphong.hinhthucthue = "Theo giờ";
                                    }
                                }
                                else
                                {
                                    datphong.hinhthucthue = "Theo ngày";
                                }
                            }
                            datPhongService.UpdateDatPhong(datphong);
                        }
                        /*                        GiamGia giamGia = giamGiaService.GetGiamGiaBYIDKhachHang(datphong.id);
                        */
                        GiamGiaNgayLe giamGiaNgayLe = giamGiaNgayLeService.GetGiamGiaNgayLeByNgayLe(DateTime.Today);
                        SuDungMaGiamGia sudunggiamGia = suDungMaGiamGiaService.GetSuDungMaGiamGiaByIddatphong(datphong.id);
                        MaGiamGia maGiamGia = null;
                        if (sudunggiamGia != null)
                        {
                            maGiamGia = maGiamGiaService.GetMaGiamGiaById(sudunggiamGia.idmagiamgia);
                        }
                        if (maGiamGia != null || giamGiaNgayLe != null)
                        {
                            Modeldata yourModel = new Modeldata
                            {
                                listsanPham = listsanpham,
                                datPhong = datphong,
                                listthueSanPham = listthueSanPham,
                                tongtienhueSanPham = tongtien,
                                phong = phongs,
                                magiamGia = maGiamGia,
                                thoigian = thoiGian,
                                giamGiaNgayle = giamGiaNgayLe,
                            };
                            listmodeldatas.Add(yourModel);
                        }
                        else
                        {
                            Modeldata yourModel = new Modeldata
                            {
                                listsanPham = listsanpham,
                                datPhong = datphong,
                                listthueSanPham = listthueSanPham,
                                tongtienhueSanPham = tongtien,
                                phong = phongs,
                                thoigian = thoiGian,
                                giamGiaNgayle = giamGiaNgayLe,
                            };
                            listmodeldatas.Add(yourModel);
                        }
                    }
                }
                else
                {
                }
                return View(listmodeldatas);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }

        public IActionResult CheckInDatPhongOnline(KhachHang khachHang, DatPhong datPhong, string magiamgia, int idkhachsan)
        {
            /// kiểm tra xem khách hàng đã tồn tại hay chưa
            KhachHang khachHangTonTai = khachHangService.GetKhachHangbyemail(khachHang.email);
            ThoiGian thoiGian = thoiGianService.GetThoiGian(idkhachsan);
            if (khachHangTonTai != null)
            {
                khachHangTonTai = new KhachHang
                {
                    cccd = khachHang.cccd,
                    sodienthoai = khachHang.cccd,
                    tinh = khachHang.tinh,
                    huyen = khachHang.huyen,
                    phuong = khachHang.phuong,
                };
                khachHangService.CapNhatKhachHang(khachHangTonTai);
                datPhong.idthoigian = thoiGian.id;
                /// id khách hàng
                datPhong.idkhachhang = khachHangTonTai.id;
                datPhong.idloaidatphong = 2;
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
                int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangTonTai.id);
                MaGiamGia guimamaGiamgia = maGiamGiaService.GetMaGiamGiasolandatphong(soluongdatphongtoithieu);
                if (guimamaGiamgia != null)
                {
                    if (guimamaGiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu)
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
                    if (maGiamGia.ngayketthuc.Year > DateTime.Now.Year ||
                       (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month > DateTime.Now.Month) ||
                       (maGiamGia.ngayketthuc.Year == DateTime.Now.Year && maGiamGia.ngayketthuc.Month == DateTime.Now.Month &&
                       maGiamGia.ngayketthuc.Day > DateTime.Now.Day))
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
                    else
                    {
                        // nulll
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
                datPhong.idloaidatphong = 2;
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
            TempData["thuephongthanhcong"] = "";
            return RedirectToAction("index", "home");
        }
        public IActionResult DanhSachThuePhongTrucTiep(int? sotrang)
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
                List<DatPhong> listDatphong = datPhongService.GetAllDatPhongDescNgayDat();
                int soluong = listDatphong.Count;
                int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                PagedList.IPagedList<DatPhong> ipagelistdatphong = listDatphong.ToPagedList(validPageNumber, pageSize);
                Modeldata yourModel = new Modeldata
                {
                    PagedTDatPhong = ipagelistdatphong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult DanhSachThuePhongOnline(int? sotrang)
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
                List<DatPhong> listDatphong = datPhongService.GetAllDatPhongDatOnline();
                int soluong = listDatphong.Count;
                int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                PagedList.IPagedList<DatPhong> ipagelistdatphong = listDatphong.ToPagedList(validPageNumber, pageSize);
                Modeldata yourModel = new Modeldata
                {
                    PagedTDatPhong = ipagelistdatphong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult ChiTietThuePhongS(int id)
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
                DatPhong datPhong = datPhongService.GetDatPhongByIDDatPhong(id);
                KhachHang khachHang = khachHangService.GetKhachHangbyid(datPhong.idkhachhang);
                List<ThueSanPham> listthueSanPham = thueSanPhamService.GetAllThueSanPhamID(datPhong.id);
                LoaiDatPhong loaiDatPhong = loaiDatPhongService.GetLoaiDatPhongById(datPhong.idloaidatphong);
                Phong phong = phongService.GetPhongID(datPhong.idphong);
                List<Modeldata> listmodeldata = new List<Modeldata>();
                foreach (var sanphams in listthueSanPham)
                {
                    SanPham sanpham = sanPhamService.GetSanPhamByID(sanphams.id);
                    Modeldata modeldata = new Modeldata
                    {
                        datPhong = datPhong,
                        khachhang = khachHang,
                        listthueSanPham = listthueSanPham,
                        sanPham = sanpham,
                        loaiDatPhong = loaiDatPhong,
                        phong = phong,
                    };
                    listmodeldata.Add(modeldata);
                }
                return View(listmodeldata);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}