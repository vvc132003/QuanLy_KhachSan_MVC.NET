using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using Service.Service;
using System.Security.Claims;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        private readonly PhongService phongService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;
        private readonly LichSuThanhToanService lichSuThanhToanService;
        private readonly LikesService likesService;
        private readonly BinhLuanService binhLuanService;
        private readonly SanPhamService sanPhamService;
        private readonly ThueSanPhamService thueSanPhamService;
        private readonly DatPhongService datPhongService;
        private readonly HuyDatPhongService huyDatPhongService;
        private readonly ChuyenPhongService chuyenPhongService;
        private readonly MaGiamGiaService maGiamGiaService;
        public HomeController(PhongService phongServices,
            KhachSanService khachSanServices,
            KhachHangService khachHangService,
            LichSuThanhToanService lichSuThanhToanService,
            LikesService likesService,
            BinhLuanService binhLuanService,
            SanPhamService sanPhamService,
            ThueSanPhamService thueSanPhamService,
            DatPhongService datPhongService, HuyDatPhongService huyDatPhongService,
            ChuyenPhongService chuyenPhongService, MaGiamGiaService maGiamGiaService)
        {
            phongService = phongServices;
            khachSanService = khachSanServices;
            this.khachHangService = khachHangService;
            this.lichSuThanhToanService = lichSuThanhToanService;
            this.sanPhamService = sanPhamService;
            this.likesService = likesService;
            this.binhLuanService = binhLuanService;
            this.thueSanPhamService = thueSanPhamService;
            this.datPhongService = datPhongService;
            this.huyDatPhongService = huyDatPhongService;
            this.chuyenPhongService = chuyenPhongService;
            this.maGiamGiaService = maGiamGiaService;
        }

        public IActionResult ThongKe()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null &&
               HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["idkhachsan"] = idkhachsan;
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<LichSuThanhToan> lichSuThanhToans = lichSuThanhToanService.GetLichSuThanhToan();
                float tongDoanhThu = lichSuThanhToans.Sum(lichSu => lichSu.sotienthanhtoan);
                List<KhachHang> khachHangs = khachHangService.GetAllKhachHang();
                int tongsoluongkhachhang = khachHangs.Count();
                List<Likes> likes = likesService.GetAllLikes();
                int tonglikes = likes.Count();
                List<BinhLuan> binhLuans = binhLuanService.GetAllBinhLuans();
                int tongbinhluan = binhLuans.Count();
                List<SanPham> sanPhams = sanPhamService.GetAllSanPham();
                int tongsanpham = sanPhams.Count();
                List<MaGiamGia> maGiamGias = maGiamGiaService.GetAllMaGiamGia();
                int tongmagiamgia = maGiamGias.Count();
                ViewData["tongDoanhThu"] = tongDoanhThu;
                ViewData["tongSoLuongKhachHang"] = tongsoluongkhachhang;
                ViewData["tonglikes"] = tonglikes;
                ViewData["tongbinhluan"] = tongbinhluan;
                ViewData["tongsanpham"] = tongsanpham;
                ViewData["tongmagiamgia"] = tongmagiamgia;
                List<Modeldata> modeldatalist = new List<Modeldata>();
                foreach (var sanpham in sanPhams)
                {
                    List<ThueSanPham> thueSanPhams = thueSanPhamService.GetAllThueSanPhamIDSanPham(sanpham.id);
                    int totalRentedQuantity = thueSanPhams.Sum(soluong => soluong.soluong);
/*                    KhachSan khachSan = khachSanService.GetKhachSanById(sanpham.idkhachsan);
*/                    Modeldata modeldata = new Modeldata()
                    {
                        sanPham = sanpham,
/*                        khachSan = khachSan,
*/                        TotalRentedQuantity = totalRentedQuantity,
                    };
                    modeldatalist.Add(modeldata);
                }
                List<KhachSan> khachSans = khachSanService.GetAllKhachSan();
                return View((modeldatalist, khachSans));
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }


    }

}
