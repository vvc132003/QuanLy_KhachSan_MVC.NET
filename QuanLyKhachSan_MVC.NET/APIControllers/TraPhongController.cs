using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model.Models;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraPhongController : ControllerBase
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
        private readonly VietQRService _vietQRService;
        private readonly IConfiguration _configuration;
        private readonly QRCodeRequestService qRCodeRequestService;
        private readonly IHubContext<ThuePhongHub> _hubContext;




        public TraPhongController(DatPhongService datPhongServices,
                                  ThueSanPhamService thueSanPhamServices,
                                  GopDonDatPhongService gopDonDatPhongService,
                                  LichSuThanhToanService lichSuThanhToanServices,
                                  PhongService phongServices,
                                  SuDungMaGiamGiaService sugiamGiaServices,
                                  ThoiGianService thoiGianServices,
                                  MaGiamGiaService maGiamGiaServices,
                                  KhachHangService khachHangServices, GiamGiaNgayLeService giamGiaNgayLeService,
                                  VietQRService vietQRService, IConfiguration configuration, QRCodeRequestService qRCodeRequestService,
                                  IHubContext<ThuePhongHub> hubContext)
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
            _vietQRService = vietQRService;
            _configuration = configuration;
            this.qRCodeRequestService = qRCodeRequestService;
            _hubContext = hubContext;
        }
        [HttpGet]
        [Route("ListQr")]
        public IActionResult ListQr()
        {
            List<QRCodeRequest> qrCodeRequests = qRCodeRequestService.GetAllQRCodeRequests();
            return Ok(qrCodeRequests);
        }
        [HttpPost]
        [Route("TaoMaQRAsync")]
        public async Task<IActionResult> TaoMaQRAsync([FromForm] int idphong, [FromForm] int IdQRCode)
        {
            QRCodeRequest qRCodeRequest = await qRCodeRequestService.GetQRCodeByIdAsync(IdQRCode);
            /*            Console.WriteLine($"Generating QR Code with amount: AccountNo: {qRCodeRequest.AccountNo}, accountName: {qRCodeRequest.AccountName}, acqId: {qRCodeRequest.AcqId}");
            */
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
            KhachHang khachHang = khachHangService.GetKhachHangbyid(datphong.idkhachhang);
            int roundedSotienthanhtoan = (int)sotienthanhtoan;
            string qrCodeUrl = await _vietQRService.GenerateQRCodeAsync(roundedSotienthanhtoan, qRCodeRequest.taikhoan, qRCodeRequest.tentaikhoan, qRCodeRequest.machinhanh, khachHang.hovaten);
            Console.WriteLine("sô tiền thanh toán", sotienthanhtoan);
            var payload = new
            {
                QRCodeUrl = qrCodeUrl
            };
            return Ok(payload);
        }


        [HttpPost]
        [Route("TraPhongandLSThanhToan")]
        public IActionResult TraPhongandLSThanhToan([FromForm] int idphong, [FromForm] int idnhanvien, [FromForm] string? hinhthucthanhtoan)
        {
            LichSuThanhToan lichSuThanhToan = new LichSuThanhToan();
            int idnv = idnhanvien;
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
            if (hinhthucthanhtoan != null)
            {
                lichSuThanhToan.hinhthucthanhtoan = hinhthucthanhtoan;
            }
            else
            {
                lichSuThanhToan.hinhthucthanhtoan = "tiền mặt";
            }
            lichSuThanhToanService.ThemLichSuThanhToan(lichSuThanhToan);

            // Cập nhật trạng thái của phòng và đặt phòng
            phong.tinhtrangphong = "chưa dọn";
            phongService.CapNhatPhong(phong);

            datphong.trangthai = "đã trả";
            datPhongService.UpdateDatPhong(datphong);
            _hubContext.Clients.All.SendAsync("ReceiveThanhToan", phong.idkhachsan);
            return Ok();

        }
    }
}
