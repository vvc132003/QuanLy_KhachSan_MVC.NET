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
        private readonly NhanVienService nhanVienService;
        public BinhLuanController(BinhLuanService binhLuanService, KhachHangService khachHangService, NhanVienService nhanVienService)
        {
            this.binhLuanService = binhLuanService;
            this.khachHangService = khachHangService;
            this.nhanVienService = nhanVienService;
        }

        public IActionResult GetBinhLuanByIdPhong(int idphong)
        {
            List<BinhLuan> binhLuanList = binhLuanService.GetAllBinhLuansByIDpHONG(idphong);
            List<object> binhLuanAndKhachHangList = new List<object>();

            foreach (var binhLuan in binhLuanList)
            {
                KhachHang khachHang = null;
                NhanVien nhanVien = null;
                if (binhLuan.trangthai.Equals("Đã duyệt"))
                {
                    if (binhLuan.loainguoithamgia.Equals("khachhang"))
                    {
                        khachHang = khachHangService.GetKhachHangbyid(binhLuan.idnguoithamgia);
                    }
                    else
                    {
                        nhanVien = nhanVienService.GetNhanVienID(binhLuan.idnguoithamgia);
                    }
                    binhLuanAndKhachHangList.Add(new { binhLuan, khachHang, nhanVien });
                }
            }
            return Json(binhLuanAndKhachHangList);
        }

        public IActionResult BinhLuans()
        {
            return View();
        }
        public IActionResult BinhLuanss(int idphong)
        {
            List<BinhLuan> binhLuanList = binhLuanService.GetBinhLuanByPhong(idphong);
            List<Modeldata> listmodeldata = new List<Modeldata>();
            foreach (var binhLuan in binhLuanList)
            {
                if (binhLuan.trangthai.Equals("Đã duyệt"))
                {
                    List<BinhLuan> binhLuans = binhLuanService.GetAllBinhLuansByIparent_comment_id(binhLuan.id);
                    KhachHang khachHang = null;
                    NhanVien nhanVien = null;

                    if (binhLuan.loainguoithamgia.Equals("khachhang"))
                    {
                        khachHang = khachHangService.GetKhachHangbyid(binhLuan.idnguoithamgia);
                    }
                    else
                    {
                        nhanVien = nhanVienService.GetNhanVienID(binhLuan.idnguoithamgia);
                    }
                    foreach (var reply in binhLuans)
                    {
                        if (reply.loainguoithamgia.Equals("khachhang"))
                        {
                            reply.hovaten = khachHangService.GetKhachHangbyid(reply.idnguoithamgia)?.hovaten;
                        }
                        else
                        {
                            reply.hovaten = nhanVienService.GetNhanVienID(reply.idnguoithamgia)?.hovaten;
                        }
                    }
                    Modeldata modeldata = new Modeldata
                    {
                        listBinhLuan = binhLuans,
                        binhLuan = binhLuan,
                        nhanVien = nhanVien,
                        khachhang = khachHang,
                    };
                    listmodeldata.Add(modeldata);
                }
            }

            return Json(listmodeldata);
        }


        public IActionResult AddBinhLuan(int idphong, string noidung)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                BinhLuan binhLuan = new BinhLuan();
                binhLuan.idphong = idphong;
                binhLuan.idnguoithamgia = (int)HttpContext.Session.GetInt32("id");
                binhLuan.loainguoithamgia = "khachhang";
                binhLuan.noidung = noidung;
                binhLuan.trangthai = "Chưa duyệt";
                binhLuan.parent_comment_id = 0;
                binhLuan.thich = 0;
                binhLuan.khongthich = 0;
                binhLuanService.InsertBinhLuan(binhLuan);
                return Json(new { success = true, message = "Bình luận của bạn đã được gửi thành công. Đợi Admin duyệt bình luận của bạn!" });
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult AddBinhLuans(int idphong, string noidung, int parent_comment_id)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                BinhLuan binhLuan = new BinhLuan();
                binhLuan.idphong = idphong;
                binhLuan.idnguoithamgia = (int)HttpContext.Session.GetInt32("id");
                binhLuan.loainguoithamgia = "khachhang";
                binhLuan.noidung = noidung;
                binhLuan.trangthai = "Chưa duyệt";
                binhLuan.parent_comment_id = parent_comment_id;
                binhLuan.thich = 0;
                binhLuan.khongthich = 0;
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
                    foreach (var binhLuan in ipagebinhluan)
                    {
                        KhachHang khachHang = null;
                        NhanVien nhanVien = null;
                        if (binhLuan.loainguoithamgia.Equals("khachhang"))
                        {
                            khachHang = khachHangService.GetKhachHangbyid(binhLuan.idnguoithamgia);
                        }
                        else
                        {
                            nhanVien = nhanVienService.GetNhanVienID(binhLuan.idnguoithamgia);
                        }
                        Modeldata modeldata = new Modeldata
                        {
                            PagedBinhLuan = new List<BinhLuan> { binhLuan }.ToPagedList(1, 1),
                            khachhang = khachHang,
                            nhanVien = nhanVien,
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
        public IActionResult ThichBinhLuan(int idbinhluan)
        {
            BinhLuan binhLuan = binhLuanService.GetBinhLuanById(idbinhluan);
            binhLuan.thich += 1;
            binhLuanService.UpdateBinhLuan(binhLuan);
            return Ok();
        }

        public IActionResult KhongThichBinhLuan(int idbinhluan)
        {
            BinhLuan binhLuan = binhLuanService.GetBinhLuanById(idbinhluan);
            binhLuan.khongthich
                += 1;
            binhLuanService.UpdateBinhLuan(binhLuan);
            return Ok();
        }
    }
}