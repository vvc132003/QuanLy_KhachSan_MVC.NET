using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class TraPhongController : Controller
    {
        private readonly DatPhongService datPhongService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly TraPhongService traPhongService;
        private readonly LichSuThanhToanService lichSuThanhToanService;
        private readonly PhongService phongService;
        private readonly GiamGiaService giamGiaService;
        private readonly QuyDinhGiamGiaService quyDinhGiamGiaservice;
        private readonly ThoiGianService thoiGianService;


        public TraPhongController(DatPhongService datPhongServices,
                                  ThueSanPhamService thueSanPhamServices,
                                  TraPhongService traPhongServices,
                                  LichSuThanhToanService lichSuThanhToanServices,
                                  PhongService phongServices,
                                  GiamGiaService giamGiaServices,
                                  QuyDinhGiamGiaService quydinhGiamGiaServices,
                                  ThoiGianService thoiGianServices)
        {
            datPhongService = datPhongServices;
            thueSanPhamService = thueSanPhamServices;
            traPhongService = traPhongServices;
            lichSuThanhToanService = lichSuThanhToanServices;
            phongService = phongServices;
            giamGiaService = giamGiaServices;
            quyDinhGiamGiaservice = quydinhGiamGiaServices;
            thoiGianService = thoiGianServices;
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
                GiamGia giamGia = giamGiaService.GetGiamGiaBYIDKhachHang(datphong.id);
                float tongtienthuesanpham = 0;
                foreach (var thueSanPham in listthueSanPham)
                {
                    tongtienthuesanpham += thueSanPham.thanhtien;
                }
                ThoiGian thoiGian = thoiGianService.GetThoiGianById(datphong.idthoigian);
                float sotienthanhtoan = 0;
                if (giamGia != null && giamGia.solandatphong > 0)
                {
                    if (datphong.hinhthucthue == "Theo giờ")
                    {
                        if (DateTime.Now.Hour > thoiGian.thoigianra.Hours)
                        {
                            // Quá hạn trả phòng, chuyển sang thuê theo ngày
                            sotienthanhtoan = ((phong.giatientheongay * (datphong.ngaydukientra.Day - datphong.ngaydat.Day)) + tongtienthuesanpham - datphong.tiendatcoc) * giamGia.phantramgiamgia / 100;
                        }
                        else
                        {
                            // Đang trong thời gian thuê theo giờ
                            sotienthanhtoan = ((phong.giatientheogio * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc) * giamGia.phantramgiamgia / 100;
                        }
                    }
                    else
                    {
                        // Đang thuê theo ngày
                        sotienthanhtoan = ((phong.giatientheongay * (datphong.ngaydukientra.Day - datphong.ngaydat.Day)) + tongtienthuesanpham - datphong.tiendatcoc) * giamGia.phantramgiamgia / 100;
                    }
                }
                else
                {
                    if (datphong.hinhthucthue == "Theo giờ")
                    {
                        if (DateTime.Now.Hour > thoiGian.thoigianra.Hours)
                        {
                            // Quá hạn trả phòng, chuyển sang thuê theo ngày
                            sotienthanhtoan = (phong.giatientheongay * (datphong.ngaydukientra.Day - datphong.ngaydat.Day)) + tongtienthuesanpham - datphong.tiendatcoc;
                        }
                        else
                        {
                            // Đang trong thời gian thuê theo giờ
                            sotienthanhtoan = (phong.giatientheogio * (datphong.ngaydukientra.Hour - datphong.ngaydat.Hour)) + tongtienthuesanpham - datphong.tiendatcoc;
                        }
                    }
                    else
                    {
                        // Đang thuê theo ngày
                        sotienthanhtoan = (phong.giatientheongay * (datphong.ngaydukientra.Day - datphong.ngaydat.Day)) + tongtienthuesanpham - datphong.tiendatcoc;
                    }
                }

                /// thêm lịch sử thanh toán
                lichSuThanhToan.ngaythanhtoan = DateTime.Now;
                lichSuThanhToan.sotienthanhtoan = sotienthanhtoan;
                lichSuThanhToan.trangthai = "đã thanh toán";
                lichSuThanhToan.iddatphong = datphong.id;
                lichSuThanhToan.idnhanvien = idnv;
                lichSuThanhToanService.ThemLichSuThanhToan(lichSuThanhToan);
                /// thêm trả phòng
                TraPhong traPhong = new TraPhong();
                traPhong.ngaytra = DateTime.Now;
                traPhong.idnhanvien = idnv;
                traPhong.iddatphong = datphong.id;
                traPhongService.ThemTraPhong(traPhong);
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
                return RedirectToAction("DangNhap", "DangNhap");
            }
        }
    }
}