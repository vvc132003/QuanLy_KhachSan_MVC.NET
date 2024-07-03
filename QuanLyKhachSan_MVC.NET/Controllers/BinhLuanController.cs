using Emgu.CV.Face;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class BinhLuanController : Controller
    {
        private readonly BinhLuanService binhLuanService;
        private readonly KhachHangService khachHangService;
        public BinhLuanController(BinhLuanService binhLuanService, KhachHangService khachHangService)
        {
            this.binhLuanService = binhLuanService;
            this.khachHangService = khachHangService;
        }

        public IActionResult GetBinhLuanByIdPhong(int idphong)
        {
            List<BinhLuan> binhLuanList = binhLuanService.GetAllBinhLuansByIDpHONG(idphong);
            List<object> binhLuanAndKhachHangList = new List<object>();

            foreach (var binhLuan in binhLuanList)
            {
                if (binhLuan.trangthai.Equals("Đã duyệt"))
                {
                    KhachHang khachHang = khachHangService.GetKhachHangbyid(binhLuan.idkhachhang);
                    binhLuanAndKhachHangList.Add(new { binhLuan, khachHang });
                }
            }
            return Json(binhLuanAndKhachHangList);
        }
        public IActionResult AddBinhLuan(int idphong, string noidung)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                BinhLuan binhLuan = new BinhLuan();
                binhLuan.idphong = idphong;
                binhLuan.idkhachhang = (int)HttpContext.Session.GetInt32("id");
                binhLuan.noidung = noidung;
                binhLuan.trangthai = "Chưa duyệt";
                binhLuanService.InsertBinhLuan(binhLuan);
                return Json(new { success = true, message = "Bình luận của bạn đã được gửi thành công. Đợi Admin duyệt bình luận của bạn!" });
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult Index(int? sotrang)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("tenchucvu") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<BinhLuan> binhluanjlisst = binhLuanService.GetAllBinhLuans()
                                                            .OrderByDescending(b => b.id)
                                                            .ToList();
                    int soluong = binhluanjlisst.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    IPagedList<BinhLuan> ipagebinhluan = binhluanjlisst.ToPagedList(validPageNumber, pageSize);
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var binhluan in ipagebinhluan)
                    {
                        KhachHang khachHang = khachHangService.GetKhachHangbyid(binhluan.idkhachhang);
                        Modeldata modeldata = new Modeldata
                        {
                            PagedBinhLuan = new List<BinhLuan> { binhluan }.ToPagedList(1, 1),
                            khachhang = khachHang,
                        };
                        modeldatalist.Add(modeldata);
                    }
                    return View(modeldatalist);
                }
                else
                {
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult DuỵetBinhLuan(int idbinhluanduyet, int idbinhluanspam)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null && HttpContext.Session.GetString("tenchucvu") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    BinhLuan binhLuanDuyetorspam = binhLuanService.GetBinhLuanById(idbinhluanduyet > 0 ? idbinhluanduyet : idbinhluanspam);
                    if (binhLuanDuyetorspam != null)
                    {
                        binhLuanDuyetorspam.trangthai = idbinhluanduyet > 0 ? "Đã duyệt" : "Spam";
                        binhLuanService.UpdateBinhLuan(binhLuanDuyetorspam);
                    }
                    return RedirectToAction("", "binhluan");
                }
                else
                {
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
    }
}
