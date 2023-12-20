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
        private readonly QuyDinhGiamGiaService quyDinhGiamGiaservice;
        private readonly SuDungMaGiamGiaService dungMaGiamGiaService;
        private readonly SuDungMaGiamGiaService suDungMaGiamGiaService;

        public ThuePhongController(DatPhongService datPhongServices,
            KhachHangService khachHangServices,
            PhongService phongServices,
            NhanPhongService nhanPhongServices,
            SanPhamService sanPhamServices,
            ThueSanPhamService thueSanPhamServices,
            ThoiGianService thoiGianServices,
            MaGiamGiaService maGiamGiaServices,
            QuyDinhGiamGiaService quydinhGiamGiaServices,
            SuDungMaGiamGiaService dungMaGiamGiaServices,
            SuDungMaGiamGiaService suDungMaGiamGiaServices)
        {
            datPhongService = datPhongServices;
            khachHangService = khachHangServices;
            phongService = phongServices;
            nhanPhongService = nhanPhongServices;
            sanPhamService = sanPhamServices;
            thueSanPhamService = thueSanPhamServices;
            thoiGianService = thoiGianServices;
            maGiamGiaService = maGiamGiaServices;
            quyDinhGiamGiaservice = quydinhGiamGiaServices;
            dungMaGiamGiaService = dungMaGiamGiaServices;
            suDungMaGiamGiaService = suDungMaGiamGiaServices;
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
                return RedirectToAction("Index", "DangNhap");
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
                KhachHang khachHangTonTai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                ThoiGian thoiGian = thoiGianService.GetThoiGian(HttpContext.Session.GetInt32("idkhachsan").Value);
                if (khachHangTonTai != null)
                {
                    if (nhanphong != null)
                    {
                        datPhong.idthoigian = thoiGian.id;
                        datPhong.idkhachhang = khachHangTonTai.id;
                        datPhong.idloaidatphong = 2;
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
                        QuyDinhGiamGia quydinhgiamgia = quyDinhGiamGiaservice.GetQuyDinhGiasolandatphong(soluongdatphongtoithieu);
                        MaGiamGia magiamgias = maGiamGiaService.GetMaGiamGiaByIdQuyDinhGiamGia(quydinhgiamgia.id);
                        if (quydinhgiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu)
                        {
                            maGiamGiaService.GuiEmail(khachHang, magiamgias.magiamgia);
                            MaGiamGia capnhatsolandasudung = maGiamGiaService.GetMaGiamGiaById(magiamgias.id);
                            capnhatsolandasudung.solandasudung = capnhatsolandasudung.solandasudung + 1;
                            maGiamGiaService.CapNhatMaGiamGia(capnhatsolandasudung);
                        }
                        else
                        {
                            /// ko có mã giảm giá
                        }
                        if (magiamgia != null)
                        {
                            MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                            SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia();
                            suDungMaGiamGia.idmagiamgia = maGiamGia.id;
                            suDungMaGiamGia.iddatphong = idDatPhongThemVao;
                            suDungMaGiamGia.ngaysudung = DateTime.Now;
                            dungMaGiamGiaService.ThemSuDungMaGiamGia(suDungMaGiamGia);
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
                        int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                        phong.tinhtrangphong = "đã đặt";
                        phongService.CapNhatPhong(phong);
                        int soluongdatphongtoithieu = datPhongService.GetDatPhongCountByKhachHangId(khachHangTonTai.id);
                        QuyDinhGiamGia quydinhgiamgia = quyDinhGiamGiaservice.GetQuyDinhGiasolandatphong(soluongdatphongtoithieu);
                        MaGiamGia magiamgias = maGiamGiaService.GetMaGiamGiaByIdQuyDinhGiamGia(quydinhgiamgia.id);
                        if (quydinhgiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu)
                        {
                            maGiamGiaService.GuiEmail(khachHang, magiamgias.magiamgia);
                            MaGiamGia capnhatsolandasudung = maGiamGiaService.GetMaGiamGiaById(magiamgias.id);
                            capnhatsolandasudung.solandasudung = capnhatsolandasudung.solandasudung + 1;
                            maGiamGiaService.CapNhatMaGiamGia(capnhatsolandasudung);
                        }
                        else
                        {
                            /// ko có mã giảm giá
                        }
                        if (magiamgia != null)
                        {
                            MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                            SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia();
                            suDungMaGiamGia.idmagiamgia = maGiamGia.id;
                            suDungMaGiamGia.iddatphong = idDatPhongThemVao;
                            suDungMaGiamGia.ngaysudung = DateTime.Now;
                            dungMaGiamGiaService.ThemSuDungMaGiamGia(suDungMaGiamGia);
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
                    KhachHang khachhangmoi = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                    if (nhanphong != null)
                    {
                        khachHang.trangthai = "còn hoạt động";
                        khachHangService.ThemKhachHang(khachHang);
                        datPhong.idkhachhang = khachhangmoi.id;
                        datPhong.idthoigian = thoiGian.id;
                        datPhong.idloaidatphong = 2;
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
                        phong.tinhtrangphong = "đã đặt";
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
                        khachHangService.ThemKhachHang(khachHang);
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
                return RedirectToAction("Index", "DangNhap");
            }
        }
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
                List<Phong> listphong = phongService.GetAllPhongTrangThai(idkhachsan);
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
                return RedirectToAction("Index", "DangNhap");
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
                        KhachHang khachHangTonTai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                        ThoiGian thoiGian = thoiGianService.GetThoiGian(idkhachsan);
                        if (khachHangTonTai != null)
                        {
                            datPhong.idthoigian = thoiGian.id;
                            datPhong.idkhachhang = khachHangTonTai.id;
                            datPhong.idloaidatphong = 1;
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
                            QuyDinhGiamGia quydinhgiamgia = quyDinhGiamGiaservice.GetQuyDinhGiasolandatphong(soluongdatphongtoithieu);
                            MaGiamGia magiamgias = maGiamGiaService.GetMaGiamGiaByIdQuyDinhGiamGia(quydinhgiamgia.id);
                            if (quydinhgiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu)
                            {
                                maGiamGiaService.GuiEmail(khachHang, magiamgias.magiamgia);
                                MaGiamGia capnhatsolandasudung = maGiamGiaService.GetMaGiamGiaById(magiamgias.id);
                                capnhatsolandasudung.solandasudung = capnhatsolandasudung.solandasudung + 1;
                                maGiamGiaService.CapNhatMaGiamGia(capnhatsolandasudung);
                            }
                            else
                            {
                                /// ko có mã giảm giá
                            }
                            if (magiamgia != null)
                            {
                                MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                                SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia();
                                suDungMaGiamGia.idmagiamgia = maGiamGia.id;
                                suDungMaGiamGia.iddatphong = idDatPhongThemVao;
                                suDungMaGiamGia.ngaysudung = DateTime.Now;
                                dungMaGiamGiaService.ThemSuDungMaGiamGia(suDungMaGiamGia);
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
                            datPhong.idloaidatphong = 1;
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
                return RedirectToAction("Index", "DangNhap");
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
                        /// thời gian trễ khi trả phòng
                        /// select * from DatPhong dp INNER JOIN ThoiGian tg ON dp.idthoigian = tg.id WHERE dp.trangthai = N'đã đặt' AND DATEPART(HOUR, dp.ngaydukientra) < DATEPART(HOUR, tg.thoigianra) 
                        /// UPDATE DatPhong
                        /// SET hinhthucthue =
                        ///CASE
                        /// WHEN dp.ngaydukientra > tg.thoigianra THEN 'Theo ngày'
                        ///      ELSE 'Theo giờ'
                        ///          END
                        ///        FROM DatPhong dp
                        ///    INNER JOIN ThoiGian tg ON dp.idthoigian = tg.id
                        ///WHERE dp.trangthai = N'đã đặt';
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
                        SuDungMaGiamGia sudunggiamGia = suDungMaGiamGiaService.GetSuDungMaGiamGiaByIddatphong(datphong.id);
                        MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaById(sudunggiamGia.idmagiamgia);
                        Modeldata yourModel = new Modeldata
                        {
                            listsanPham = listsanpham,
                            datPhong = datphong,
                            listthueSanPham = listthueSanPham,
                            tongtienhueSanPham = tongtien,
                            phong = phongs,
                            magiamGia = maGiamGia,
                            thoigian = thoiGian,
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
                return RedirectToAction("Index", "DangNhap");
            }
        }
        public IActionResult DatPhongOnline(int id)
        {
            Phong phong = phongService.GetPhongID(id);
            Modeldata yourModel = new Modeldata
            {
                phong = phong,
            };
            return View(yourModel);
        }
        public IActionResult CheckInDatPhongOnline(KhachHang khachHang, DatPhong datPhong, string magiamgia)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                /// kiểm tra xem khách hàng đã tồn tại hay chưa
                KhachHang khachHangTonTai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                ThoiGian thoiGian = thoiGianService.GetThoiGian(HttpContext.Session.GetInt32("idkhachsan").Value);
                if (khachHangTonTai != null)
                {
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
                    QuyDinhGiamGia quydinhgiamgia = quyDinhGiamGiaservice.GetQuyDinhGiasolandatphong(soluongdatphongtoithieu);
                    MaGiamGia magiamgias = maGiamGiaService.GetMaGiamGiaByIdQuyDinhGiamGia(quydinhgiamgia.id);
                    if (quydinhgiamgia.soluongdatphongtoithieu == soluongdatphongtoithieu)
                    {
                        maGiamGiaService.GuiEmail(khachHang, magiamgias.magiamgia);
                        MaGiamGia capnhatsolandasudung = maGiamGiaService.GetMaGiamGiaById(magiamgias.id);
                        capnhatsolandasudung.solandasudung = capnhatsolandasudung.solandasudung + 1;
                        maGiamGiaService.CapNhatMaGiamGia(capnhatsolandasudung);
                    }
                    else
                    {
                        /// ko có mã giảm giá
                    }
                    if (magiamgia != null)
                    {
                        MaGiamGia maGiamGia = maGiamGiaService.GetMaGiamGiaByMaGiamGia(magiamgia);
                        SuDungMaGiamGia suDungMaGiamGia = new SuDungMaGiamGia();
                        suDungMaGiamGia.idmagiamgia = maGiamGia.id;
                        suDungMaGiamGia.iddatphong = idDatPhongThemVao;
                        suDungMaGiamGia.ngaysudung = DateTime.Now;
                        dungMaGiamGiaService.ThemSuDungMaGiamGia(suDungMaGiamGia);
                    }
                    else
                    {
                        /// không áp dụng mã giảm giá
                    }
                }
                /// nếu khách hàng không tồn tại
                else
                {
                    /// khách hnagf không tông tại thì thực hiện thêm khách hàng mới
                    khachHang.trangthai = "còn hoạt động";
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
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("Index", "DangNhap");
            }
        }

    }
}