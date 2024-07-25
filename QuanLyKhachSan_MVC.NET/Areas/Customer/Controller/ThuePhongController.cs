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

namespace QuanLyKhachSan_MVC.NET.Areas.Customer.Controllers
{
    [Area("Customer")]
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
            GiamGiaNgayLeService giamGiaNgayLeService)
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
                return Redirect("~/customer/home/");

            }
            else
            {
                return Redirect($"~/customer/phong/datphongonline?idphong={datPhong.idphong}");
            }
        }

    }
}