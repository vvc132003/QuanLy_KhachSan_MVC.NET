using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;
using System.Globalization;

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
        private readonly GiamGiaService giamGiaService;
        private readonly QuyDinhGiamGiaService quyDinhGiamGiaservice;


        public ThuePhongController(DatPhongService datPhongServices,
            KhachHangService khachHangServices,
            PhongService phongServices,
            NhanPhongService nhanPhongServices,
            SanPhamService sanPhamServices,
            ThueSanPhamService thueSanPhamServices,
            ThoiGianService thoiGianServices,
            GiamGiaService giamGiaServices,
            QuyDinhGiamGiaService quydinhGiamGiaServices)
        {
            datPhongService = datPhongServices;
            khachHangService = khachHangServices;
            phongService = phongServices;
            nhanPhongService = nhanPhongServices;
            sanPhamService = sanPhamServices;
            thueSanPhamService = thueSanPhamServices;
            thoiGianService = thoiGianServices;
            giamGiaService = giamGiaServices;
            quyDinhGiamGiaservice = quydinhGiamGiaServices;
        }
        public IActionResult Index(int id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                Phong phong = phongService.GetPhongID(id);
                Modeldata yourModel = new Modeldata
                {
                    phong = phong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThemThuePhong(KhachHang khachHang, DatPhong datPhong, NhanPhong nhanPhong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                /// kiểm tra xem khách hàng đã tồn tại hay chưa
                KhachHang khachHangTonTai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                ThoiGian thoiGian = thoiGianService.GetThoiGianById(DateTime.Now);
                if (khachHangTonTai != null)
                {
                    datPhong.idthoigian = thoiGian.id;
                    /// id khách hàng
                    datPhong.idkhachhang = khachHangTonTai.id;
                    datPhong.idloaidatphong = 2;
                    datPhong.trangthai = "đã đặt";
                    datPhong.ngaydat = DateTime.Now;
                    datPhong.idphong = datPhong.idphong;
                    TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                    if (sogio.TotalDays >= 1)
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
                    nhanPhong.iddatphong = idDatPhongThemVao;
                    nhanPhong.ngaynhanphong = DateTime.Now;
                    nhanPhongService.ThemNhanPhong(nhanPhong);
                    /// lấy thoong tin phòng ra từ id phòng của id đặt phòng mới tạo để thực hiện việc cập nhật trạng thái phòng đó
                    Phong phong = phongService.GetPhongID(datPhong.idphong);
                    phong.tinhtrangphong = "có khách";
                    phongService.CapNhatPhong(phong);
                    /// thêm giảm giá
                    /// lấy tổng số lần đặt phòng của khách hàng
                    int solandatphong = datPhongService.GetDatPhongCountByKhachHangId(khachHangTonTai.id);
                    QuyDinhGiamGia quyDinhGiamGia = quyDinhGiamGiaservice.GetQuyDinhGia(solandatphong);
                    if (quyDinhGiamGia != null && solandatphong == quyDinhGiamGia.solandatphong)
                    {
                        GiamGia giamGia = new GiamGia();
                        giamGia.iddatphong = idDatPhongThemVao;
                        giamGia.ngaythemgiamgia = DateTime.Now;
                        giamGia.solandatphong = solandatphong;
                        giamGia.phantramgiamgia = quyDinhGiamGia.phantramgiamgia;
                        giamGia.idkhachhang = khachHangTonTai.id;
                        giamGia.idquydinh = quyDinhGiamGia.id;
                        giamGiaService.ThemGiamGia(giamGia);
                    }
                    else
                    {
                        Console.WriteLine("Không có giảm giá!");
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
                    datPhong.trangthai = "đã đặt";
                    datPhong.ngaydat = DateTime.Now;
                    datPhong.idphong = datPhong.idphong;
                    TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                    if (sogio.TotalDays >= 1)
                    {
                        datPhong.hinhthucthue = "Theo ngày";
                    }
                    else
                    {
                        datPhong.hinhthucthue = "Theo giờ";
                    }
                    /// thực hiện thêm đặt phòng và lấy ra id đặt phòng mới tạo đó
                    int idDatPhongThemVao = datPhongService.ThemDatPhong(datPhong);
                    /// thêm nhận phòng với id đặt phòng vừa đc lấy ra
                    nhanPhong.iddatphong = idDatPhongThemVao;
                    nhanPhong.ngaynhanphong = DateTime.Now;
                    nhanPhongService.ThemNhanPhong(nhanPhong);
                    /// lấy thoong tin phòng ra từ id phòng của id đặt phòng mới tạo để thực hiện việc cập nhật trạng thái phòng đó
                    Phong phong = phongService.GetPhongID(datPhong.idphong);
                    phong.tinhtrangphong = "có khách";
                    phongService.CapNhatPhong(phong);
                }
                TempData["thuephongthanhcong"] = "";
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult Index1()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                List<Phong> phong = phongService.GetAllPhongTrangThai();
                Modeldata yourModel = new Modeldata
                {
                    listphong = phong,
                };
                return View(yourModel);
            }
            else
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
        public IActionResult ThueNhieuPhong(List<int> idphongs, KhachHang khachHang, DatPhong datPhong, NhanPhong nhanPhong)
        {
            if (idphongs != null && idphongs.Any())
            {
                foreach (int phongId in idphongs)
                {
                    KhachHang khachHangTonTai = khachHangService.GetKhachHangCCCD(khachHang.cccd);
                    ThoiGian thoiGian = thoiGianService.GetThoiGianById(DateTime.Now);
                    if (khachHangTonTai != null)
                    {
                        datPhong.idthoigian = thoiGian.id;
                        datPhong.idkhachhang = khachHangTonTai.id;
                        datPhong.idloaidatphong = 2;
                        datPhong.trangthai = "đã đặt";
                        datPhong.ngaydat = DateTime.Now;
                        datPhong.idphong = phongId;
                        TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                        if (sogio.TotalDays >= 1)
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
                        nhanPhongService.ThemNhanPhong(nhanPhong); Phong phong = phongService.GetPhongID(phongId);
                        phong.tinhtrangphong = "có khách";
                        phongService.CapNhatPhong(phong);
                        /// thêm giảm giá
                        /// lấy tổng số lần đặt phòng của khách hàng
                        int solandatphong = datPhongService.GetDatPhongCountByKhachHangId(khachHangTonTai.id);
                        QuyDinhGiamGia quyDinhGiamGia = quyDinhGiamGiaservice.GetQuyDinhGia(solandatphong);
                        if (quyDinhGiamGia != null && solandatphong == quyDinhGiamGia.solandatphong)
                        {
                            GiamGia giamGia = new GiamGia();
                            giamGia.iddatphong = idDatPhongThemVao;
                            giamGia.ngaythemgiamgia = DateTime.Now;
                            giamGia.solandatphong = solandatphong;
                            giamGia.phantramgiamgia = quyDinhGiamGia.phantramgiamgia;
                            giamGia.idkhachhang = khachHangTonTai.id;
                            giamGia.idquydinh = quyDinhGiamGia.id;
                            giamGiaService.ThemGiamGia(giamGia);
                        }
                        else
                        {
                            Console.WriteLine("Không có giảm giá!");
                        }
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
                        datPhong.idphong = phongId;
                        TimeSpan sogio = datPhong.ngaydukientra - datPhong.ngaydat;
                        if (sogio.TotalDays >= 1)
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
                        Phong phong = phongService.GetPhongID(phongId);
                        phong.tinhtrangphong = "có khách";
                        phongService.CapNhatPhong(phong);
                    }
                }
                TempData["thuephongthanhcong"] = "";
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("Index", "Index1");
            }
        }
        public IActionResult ChiTietThuePhong(int id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = idnv;
                ViewData["hovaten"] = hovaten;
                List<DatPhong> listdatPhongs = datPhongService.GetAllDatPhongByID(id);
                List<SanPham> listsanpham = sanPhamService.GetAllSanPham();
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
                        ThoiGian thoiGian = thoiGianService.GetThoiGianById(DateTime.Now);
                        DateTime ngaydukientra = datphong.ngaydukientra;
                        DateTime ngaytraphong = thoiGian.thoigianra;
                        if (ngaytraphong > ngaydukientra)
                        {
                            if (datphong.hinhthucthue == "Theo giờ" && ngaytraphong < ngaydukientra)
                            {
                                datphong.hinhthucthue = "Theo ngày";
                                datPhongService.UpdateDatPhong(datphong);
                            }
                            else if (datphong.hinhthucthue == "Theo ngày" && ngaytraphong > ngaydukientra)
                            {
                                datphong.hinhthucthue = "Theo giờ";
                                datPhongService.UpdateDatPhong(datphong);
                            }
                        }
                        datPhongService.UpdateDatPhong(datphong);
                        GiamGia giamGia = giamGiaService.GetGiamGiaBYIDKhachHang(datphong.id);
                        Modeldata yourModel = new Modeldata
                        {
                            listsanPham = listsanpham,
                            datPhong = datphong,
                            listthueSanPham = listthueSanPham,
                            tongtienhueSanPham = tongtien,
                            phong = phongs,
                            giamGia = giamGia,
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
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}