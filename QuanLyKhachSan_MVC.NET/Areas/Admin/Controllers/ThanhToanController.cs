using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThanhToanController : Controller
    {
        private readonly LichSuThanhToanService lichSuThanhToanService;
        public ThanhToanController(LichSuThanhToanService lichSuThanhToanService)
        {
            this.lichSuThanhToanService = lichSuThanhToanService;
        }

        public IActionResult DanhSachThanhToan(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idnv = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    ViewData["id"] = idnv;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<LichSuThanhToan> listLichSuThanhToan = lichSuThanhToanService.GetAllLichSuThanhToanDescNgayThanhToan();
                    int soluong = listLichSuThanhToan.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    PagedList.IPagedList<LichSuThanhToan> ipagelistLichSuThanhToan = listLichSuThanhToan.ToPagedList(validPageNumber, pageSize);
                    Modeldata yourModel = new Modeldata
                    {
                        PagedTLichSuThanhToan = ipagelistLichSuThanhToan,
                    };
                    return View(yourModel);
                }
                else
                {
                    return Redirect("~/customer/dangnhap/dangnhap");
                }
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
    }
}
