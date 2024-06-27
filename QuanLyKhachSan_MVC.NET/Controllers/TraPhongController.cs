using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class TraPhongController : Controller
    {
        private readonly DatPhongService datPhongService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly LichSuThanhToanService lichSuThanhToanService;
        private readonly PhongService phongService;
        private readonly SuDungMaGiamGiaService sugiamGiaService;
        private readonly ThoiGianService thoiGianService;
        private readonly MaGiamGiaService maGiamGiaService;
        private readonly KhachHangService khachHangService;
        private readonly GiamGiaNgayLeService giamGiaNgayLeService;



        public TraPhongController(DatPhongService datPhongServices,
                                  ThueSanPhamService thueSanPhamServices,
                                 
                                  LichSuThanhToanService lichSuThanhToanServices,
                                  PhongService phongServices,
                                  SuDungMaGiamGiaService sugiamGiaServices,
                                  ThoiGianService thoiGianServices,
                                  MaGiamGiaService maGiamGiaServices, KhachHangService khachHangServices, GiamGiaNgayLeService giamGiaNgayLeService)
        {
            datPhongService = datPhongServices;
            thueSanPhamService = thueSanPhamServices;
            
            lichSuThanhToanService = lichSuThanhToanServices;
            phongService = phongServices;
            sugiamGiaService = sugiamGiaServices;
            thoiGianService = thoiGianServices;
            maGiamGiaService = maGiamGiaServices;
            khachHangService = khachHangServices;
            this.giamGiaNgayLeService = giamGiaNgayLeService;
        }
        public IActionResult TraPhongandLSThanhToan(LichSuThanhToan lichSuThanhToan, int idphong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int idnv = HttpContext.Session.GetInt32("id").Value;
                // lấy id đặt phòng từ id phòng
                DatPhong datphong = datPhongService.GetDatPhongByIDTrangThai(idphong);
                Phong phong = phongService.GetPhongID(idphong);
                /// thực hiện lấy ds thuê sản phẩm và tổng tiền theo id đặt phòng
                List<ThueSanPham> listthueSanPham = thueSanPhamService.GetAllThueSanPhamID(datphong.id);
                SuDungMaGiamGia sudunggiamGia = sugiamGiaService.GetSuDungMaGiamGiaByIddatphong(datphong.id);
                MaGiamGia maGiamGia = null;
                float tongtienthuesanpham = 0;
                foreach (var thueSanPham in listthueSanPham)
                {
                    tongtienthuesanpham += thueSanPham.thanhtien;
                }
                /*                ThoiGian thoiGian = thoiGianService.GetThoiGianById(datphong.idthoigian);
                */
                GiamGiaNgayLe giamGiaNgayLe = giamGiaNgayLeService.GetGiamGiaNgayLeByNgayLe(DateTime.Today);
                float sotienthanhtoan = 0;
                if (sudunggiamGia != null)
                {
                    maGiamGia = maGiamGiaService.GetMaGiamGiaById(sudunggiamGia.idmagiamgia);
                }
                if (maGiamGia != null)
                {
                    if (datphong.hinhthucthue == "Theo giờ")
                    {
                        if (giamGiaNgayLe != null)
                        {
                            // Đang trong thời gian thuê theo giờ
                            /*                            sotienthanhtoan = ((((phong.giatientheogio * chinhSachGia.dieuchinhgiaphong) / 100) * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc) * maGiamGia.phantramgiamgia / 100;
                            */
                            sotienthanhtoan = ((((phong.giatientheogio * (100 + giamGiaNgayLe.dieuchinhgiaphong)) / 100) * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc) * (100 - maGiamGia.phantramgiamgia) / 100;
                        }
                        else
                        {
                            // Đang trong thời gian thuê theo giờ
                            sotienthanhtoan = ((phong.giatientheogio * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc) * maGiamGia.phantramgiamgia / 100;
                        }
                    }
                    else
                    {
                        if (giamGiaNgayLe != null)
                        {
                            // Đang thuê theo ngày
                            sotienthanhtoan = ((((phong.giatientheongay * (100 + giamGiaNgayLe.dieuchinhgiaphong)) / 100) * (datphong.ngaydukientra.Day - datphong.ngaydat.Day)) + tongtienthuesanpham - datphong.tiendatcoc) * (100 - maGiamGia.phantramgiamgia) / 100;
                            /*                            sotienthanhtoan = ((((phong.giatientheongay * chinhSachGia.dieuchinhgiaphong) / 100) * (datphong.ngaydukientra.Day - datphong.ngaydat.Day)) + tongtienthuesanpham - datphong.tiendatcoc) * maGiamGia.phantramgiamgia / 100;
                            */
                        }
                        else
                        {
                            // Đang thuê theo ngày
                            sotienthanhtoan = ((phong.giatientheongay * (datphong.ngaydukientra.Day - datphong.ngaydat.Day)) + tongtienthuesanpham - datphong.tiendatcoc) * maGiamGia.phantramgiamgia / 100;
                        }
                    }
                }
                else
                {
                    if (datphong.hinhthucthue == "Theo giờ")
                    {
                        if (giamGiaNgayLe != null)
                        {
                            // Đang trong thời gian thuê theo giờ
                            sotienthanhtoan = (((phong.giatientheogio * (100 + giamGiaNgayLe.dieuchinhgiaphong)) / 100) * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc;
                            /*                            sotienthanhtoan = (((phong.giatientheogio * chinhSachGia.dieuchinhgiaphong) / 100) * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc;
                            */
                        }
                        else
                        {
                            // Đang trong thời gian thuê theo giờ
                            sotienthanhtoan = (phong.giatientheogio * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc;
                        }
                    }
                    else
                    {
                        if (giamGiaNgayLe != null)
                        {
                            // Đang thuê theo ngày
                            sotienthanhtoan = (((phong.giatientheongay * (100 + giamGiaNgayLe.dieuchinhgiaphong)) / 100) * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc;
                            /*                            sotienthanhtoan = (((phong.giatientheongay * chinhSachGia.dieuchinhgiaphong) / 100) * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc;
                            */
                        }
                        else
                        {
                            // Đang thuê theo ngày
                            sotienthanhtoan = (phong.giatientheongay * (datphong.ngaydukientra.Day - datphong.ngaydat.Day)) + tongtienthuesanpham - datphong.tiendatcoc;
                        }
                    }
                }
                KhachHang khachHang = khachHangService.GetKhachHangbyid(datphong.idkhachhang);
                float giatienphong = 0;
                if (datphong.hinhthucthue == "Theo ngày")
                {
                    if (giamGiaNgayLe != null)
                    {
                        giatienphong = phong.giatientheongay + (phong.giatientheongay * giamGiaNgayLe.dieuchinhgiaphong) / 100;
                        /*                        giatienphong = (phong.giatientheongay * chinhSachGia.dieuchinhgiaphong) / 100;
                        */
                    }
                    else
                    {
                        giatienphong = phong.giatientheongay;
                    }
                }
                else
                {
                    if (giamGiaNgayLe != null)
                    {
                        giatienphong = phong.giatientheogio + (phong.giatientheogio * giamGiaNgayLe.dieuchinhgiaphong) / 100;
                        /*                        giatienphong = (phong.giatientheogio * chinhSachGia.dieuchinhgiaphong) / 100;
                        */
                    }
                    else
                    {
                        giatienphong = phong.giatientheogio;
                    }
                }
                lichSuThanhToanService.GuiEmailThanhToan(khachHang, giatienphong, lichSuThanhToan.phantramgiamgia, phong.sophong, datphong.ngaydat, listthueSanPham, sotienthanhtoan);
                /// thêm lịch sử thanh toán
                lichSuThanhToan.ngaythanhtoan = DateTime.Now;
                lichSuThanhToan.sotienthanhtoan = sotienthanhtoan;
                lichSuThanhToan.trangthai = "đã thanh toán";
                lichSuThanhToan.iddatphong = datphong.id;
                lichSuThanhToan.idnhanvien = idnv;
                if (lichSuThanhToan.hinhthucthanhtoan == null)
                {
                    lichSuThanhToan.hinhthucthanhtoan = "";
                    lichSuThanhToanService.ThemLichSuThanhToan(lichSuThanhToan);
                }
                else
                {
                    lichSuThanhToanService.ThemLichSuThanhToan(lichSuThanhToan);
                }
                
                /// cập nhật trạng thái của phòng 
                phong.tinhtrangphong = "chưa dọn";
                phongService.CapNhatPhong(phong);
                /// cập nhật trạng thái id đặt phòng
                datphong.trangthai = "đã trả";
                datPhongService.UpdateDatPhong(datphong);
                TempData["traphongthanhcong"] = "";
                return RedirectToAction("Index", "Phong");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }

    }
}