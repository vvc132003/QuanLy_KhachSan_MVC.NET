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

        public TraPhongController(DatPhongService datPhongServices,
                                  ThueSanPhamService thueSanPhamServices,
                                  TraPhongService traPhongServices,
                                  LichSuThanhToanService lichSuThanhToanServices, PhongService phongServices, GiamGiaService giamGiaServices, QuyDinhGiamGiaService quydinhGiamGiaServices)
        {
            datPhongService = datPhongServices;
            thueSanPhamService = thueSanPhamServices;
            traPhongService = traPhongServices;
            lichSuThanhToanService = lichSuThanhToanServices;
            phongService = phongServices;
            giamGiaService = giamGiaServices;
            quyDinhGiamGiaservice = quydinhGiamGiaServices;
        }
        public IActionResult TraPhongandLSThanhToan(int idphong)
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
                float tongtien = 0;
                foreach (var thueSanPham in listthueSanPham)
                {
                    tongtien += thueSanPham.thanhtien;
                }
                float sotienthanhtoan = 0;
                if (giamGia != null && giamGia.solandatphong > 0)
                {
                    sotienthanhtoan = (((phong.giatien + tongtien) - datphong.tiendatcoc) * giamGia.phantramgiamgia) / 100;
                }
                else
                {
                    sotienthanhtoan = (phong.giatien + tongtien) - datphong.tiendatcoc;
                }
                /// thêm lịch sử thanh toán
                LichSuThanhToan lichSuThanhToan = new LichSuThanhToan();
                lichSuThanhToan.ngaythanhtoan = DateTime.Now;
                lichSuThanhToan.sotienthanhtoan = sotienthanhtoan;
                lichSuThanhToan.hinhthucthanhtoan = "chuyển khoản";
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
