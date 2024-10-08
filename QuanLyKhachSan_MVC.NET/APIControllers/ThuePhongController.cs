using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model.Models;
using PagedList;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThuePhongController : ControllerBase
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
        private readonly IHubContext<ThuePhongHub> _hubContext;
        private readonly TachDonService tachDonService;

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
            GopDonDatPhongService gopDonDatPhongService,
            IHubContext<ThuePhongHub> hubContext,
            TachDonService tachDonService)
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
            _hubContext = hubContext;
            this.tachDonService = tachDonService;
        }



        public IActionResult GetDatPhongByIdPhong(int idphong)
        {
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            return Ok(datPhong);
        }


        /*  public IActionResult ListPhongTrangThaiDaDat()
          {
              int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
              ViewData["idkhachsan"] = idkhachsan;
              List<Phong> phongtrangthai = phongService.GetAllPhongIDKhachSan(idkhachsan).Where(p => p.tinhtrangphong == "có khách").ToList();
              return Json(phongtrangthai);
          }
  */

        /*  public IActionResult GopDonDatPhong(int idphong, int idphongmoi)
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
              return Ok();
          }*/

        [HttpGet]
        [Route("DatPhongDon")]
        public IActionResult DatPhongDon(int idphong)
        {
            Phong phong = phongService.GetPhongID(idphong);
            List<LoaiDichVu> loaiDichVus = loaiDichDichVuService.LayTatCaLoaiDichVu();

            DatPhongDonResponse response = new DatPhongDonResponse
            {
                Phong = phong,
                LoaiDichVus = loaiDichVus
            };

            return Ok(response);
        }

        public class DatPhongDonResponse
        {
            public Phong Phong { get; set; }
            public List<LoaiDichVu> LoaiDichVus { get; set; }
        }

        [HttpGet]
        [Route("Dasachsanpham")]
        public IActionResult Dasachsanpham(int idloaidichvu, string? tensanpham)
        {
            List<SanPham> listsanpham;
            if (idloaidichvu > 0 && !string.IsNullOrEmpty(tensanpham))
            {
                listsanpham = sanPhamService.GetAllSanPhamIDLoaidichvuandTenSanPham(idloaidichvu, tensanpham).Where(sp => sp.trangthai.Equals("còn bán")).ToList();
            }
            else if (idloaidichvu > 0)
            {
                listsanpham = sanPhamService.GetAllSanPhamIDLoaidichvu(idloaidichvu).Where(sp => sp.trangthai.Equals("còn bán")).ToList();
            }
            else
            {
                listsanpham = sanPhamService.GetAllSanPham().Where(sp => sp.trangthai.Equals("còn bán")).ToList();
            }

            Modeldata yourModel = new Modeldata
            {
                listsanPham = listsanpham,
            };
            return Ok(yourModel);

        }


        public IActionResult TachDon(int idphong, int idkhachsan)
        {

            Phong phong = phongService.GetPhongID(idphong);
            DatPhong datPhong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            List<Phong> phonglist = phongService.GetAllPhongTrangThai(idkhachsan);
            Modeldata yourModel = new Modeldata
            {
                phong = phong,
                datPhong = datPhong,
                listphong = phonglist,
            };
            return Ok(yourModel);

        }
        [HttpPost]
        [Route("ThemThuePhong")]
        public IActionResult ThemThuePhong([FromForm] KhachHang khachHang, [FromForm] DatPhong datPhong, [FromForm] List<int>? idsanpham, [FromForm] string? nhanphong, [FromForm] string? magiamgia, [FromForm] int idnhanvien, [FromForm] int idkhachsan)
        {

            Phong checkphongtrong = phongService.GetPhongID(datPhong.idphong);
            if (checkphongtrong.tinhtrangphong == "còn trống")
            {
                NhanPhong nhanPhong = new NhanPhong();
                Phong phong = phongService.GetPhongID(datPhong.idphong);
                int idnd = idnhanvien;
                /// kiểm tra xem khách hàng đã tồn tại hay chưa
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
                _hubContext.Clients.All.SendAsync("ReceiveThuePhong", datPhong.idphong);
                return Ok();
            }
            else
            {
                return Ok();
            }

        }
        [HttpGet]
        [Route("ChiTietThuePhong")]
        public IActionResult ChiTietThuePhong(int idphong)
        {
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
            return Ok(listmodeldatas);
        }
        [HttpGet]
        [Route("ChiTietThanhToan")]
        public IActionResult ChiTietThanhToan(int idphong)
        {
            DatPhong datphong = datPhongService.GetDatPhongByIDTrangThai(idphong);
            Phong phongs = phongService.GetPhongID(idphong);
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
                tongtienhueSanPham = tongtien,
                phong = phongs,
                magiamGia = maGiamGia,
                thoigian = thoiGian,
                giamGiaNgayle = giamGiaNgayLe,
                gopDonDatPhong = gopDonDatPhong != null ? gopDonDatPhong : null
            };
            return Ok(yourModel);
        }

        [HttpPost]
        [Route("AddThuePhongTachDon")]
        public IActionResult AddThuePhongTachDon([FromForm] DatPhong datPhong, [FromForm] KhachHang khachHang, [FromForm] int idphongmoi, [FromForm] int idkhachsan, [FromForm] int idnhanvien)
        {
            int idnv = idnhanvien;
            ThoiGian thoiGian = thoiGianService.GetThoiGian(idkhachsan);
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
                List<TachDon> cartItems = tachDonService.GetAllTachDon();
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
                    addthuesanpham.ghichu = cartItem.ghichu;
                    thueSanPhamService.ThueSanPham(addthuesanpham);
                }
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

                List<TachDon> cartItems = tachDonService.GetAllTachDon();
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
                    addthuesanpham.ghichu = cartItem.ghichu;
                    thueSanPhamService.ThueSanPham(addthuesanpham);
                }

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
            return Ok();

        }
    }

}

