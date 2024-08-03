using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        private readonly GopDonDatPhongService gopDonDatPhongService;



        public TraPhongController(DatPhongService datPhongServices,
                                  ThueSanPhamService thueSanPhamServices,
                                  GopDonDatPhongService gopDonDatPhongService,
                                  LichSuThanhToanService lichSuThanhToanServices,
                                  PhongService phongServices,
                                  SuDungMaGiamGiaService sugiamGiaServices,
                                  ThoiGianService thoiGianServices,
                                  MaGiamGiaService maGiamGiaServices,
                                  KhachHangService khachHangServices, GiamGiaNgayLeService giamGiaNgayLeService)
        {
            datPhongService = datPhongServices;
            thueSanPhamService = thueSanPhamServices;
            this.gopDonDatPhongService = gopDonDatPhongService;
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
                DatPhong datphong = datPhongService.GetDatPhongByIDTrangThai(idphong);
                Phong phong = phongService.GetPhongID(idphong);
                List<ThueSanPham> listthueSanPham = thueSanPhamService.GetAllThueSanPhamID(datphong.id);
                SuDungMaGiamGia sudunggiamGia = sugiamGiaService.GetSuDungMaGiamGiaByIddatphong(datphong.id);
                MaGiamGia maGiamGia = sudunggiamGia != null ? maGiamGiaService.GetMaGiamGiaById(sudunggiamGia.idmagiamgia) : null;
                float tongtienthuesanpham = listthueSanPham.Sum(thueSanPham => thueSanPham.thanhtien);
                GiamGiaNgayLe giamGiaNgayLe = giamGiaNgayLeService.GetGiamGiaNgayLeByNgayLe(DateTime.Today);
                GopDonDatPhong gopDonDatPhong = gopDonDatPhongService.GetByIdDatPhongMoi(datphong.id);
                var ngaydukientra = datphong.ngaydukientra;
                var ngaydat = datphong.ngaydat;
                var tongngay = ngaydukientra - ngaydat;
                var soNgay = Math.Ceiling(tongngay.TotalDays);
                var soGio = Math.Ceiling(tongngay.TotalHours);
                double tongTienPhong = 0;
                double sotienthanhtoan = 0;

                if (datphong.hinhthucthue == "Theo giờ")
                {
                    float giaTienPhong = phong.giatientheogio;
                    if (giamGiaNgayLe != null)
                    {
                        giaTienPhong *= (100 + giamGiaNgayLe.dieuchinhgiaphong) / 100;
                    }
                    tongTienPhong = giaTienPhong * soGio;
                }
                else
                {
                    float giaTienPhong = phong.giatientheongay;
                    if (giamGiaNgayLe != null)
                    {
                        giaTienPhong *= (100 + giamGiaNgayLe.dieuchinhgiaphong) / 100;
                    }
                    tongTienPhong = giaTienPhong * soNgay;
                }
                if (gopDonDatPhong != null)
                {
                    tongTienPhong += gopDonDatPhong.tienphong;
                }
                sotienthanhtoan = tongTienPhong + tongtienthuesanpham - datphong.tiendatcoc;
                if (maGiamGia != null)
                {
                    sotienthanhtoan *= (100 - maGiamGia.phantramgiamgia) / 100;
                }
                sotienthanhtoan = Math.Round(sotienthanhtoan, 0); // Làm tròn đến số nguyên
                KhachHang khachHang = khachHangService.GetKhachHangbyid(datphong.idkhachhang);
                float giatienphong = datphong.hinhthucthue == "Theo ngày" ? phong.giatientheongay : phong.giatientheogio;
                if (giamGiaNgayLe != null)
                {
                    giatienphong *= (100 + giamGiaNgayLe.dieuchinhgiaphong) / 100;
                }

                lichSuThanhToanService.GuiEmailThanhToan(khachHang, giatienphong, maGiamGia?.phantramgiamgia ?? 0, phong.sophong, datphong.ngaydat, listthueSanPham, (float)sotienthanhtoan);
                // Lưu lịch sử thanh toán
                lichSuThanhToan.phantramgiamgia = maGiamGia?.phantramgiamgia ?? 0;
                lichSuThanhToan.ngaythanhtoan = DateTime.Now;
                lichSuThanhToan.sotienthanhtoan = (float)sotienthanhtoan;
                lichSuThanhToan.trangthai = "đã thanh toán";
                lichSuThanhToan.iddatphong = datphong.id;
                lichSuThanhToan.idnhanvien = idnv;
                lichSuThanhToan.hinhthucthanhtoan ??= ""; // Nếu null, gán chuỗi rỗng
                lichSuThanhToanService.ThemLichSuThanhToan(lichSuThanhToan);

                // Cập nhật trạng thái của phòng và đặt phòng
                phong.tinhtrangphong = "chưa dọn";
                phongService.CapNhatPhong(phong);

                datphong.trangthai = "đã trả";
                datPhongService.UpdateDatPhong(datphong);

                TempData["traphongthanhcong"] = "";
                return Redirect("~/admin/phong/");
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }

    }
}