using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using NPOI.SS.Formula.Functions;
using PagedList;
using Service;
using Service.Service;
using System.Globalization;
using System.Security.Claims;
using System.Speech.Synthesis;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class BinhLuanController : Controller
    {
        private readonly SpeechSynthesizer speechSynthesizer;

        private readonly BinhLuanService binhLuanService;
        private readonly KhachHangService khachHangService;
        private readonly NhanVienService nhanVienService;
        private readonly PhongService phongService;
        private readonly LikesBinhLuanService likesBinhLuanService;
        public BinhLuanController(BinhLuanService binhLuanService,
            KhachHangService khachHangService, NhanVienService nhanVienService,
            LikesBinhLuanService likesBinhLuanService, SpeechSynthesizer speechSynthesizer,
            PhongService phongService)
        {
            this.binhLuanService = binhLuanService;
            this.khachHangService = khachHangService;
            this.nhanVienService = nhanVienService;
            this.phongService = phongService;
            this.likesBinhLuanService = likesBinhLuanService;
            this.speechSynthesizer = speechSynthesizer;
            speechSynthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new CultureInfo("vi-VN"));
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
                    int tonglikebinhluanthichbuyid = 0;
                    int tonglikebinhluankhongthichbuyid = 0;
                    /// bình luận cha
                    KhachHang khachHang = null;
                    NhanVien nhanVien = null;
                    /// lấy ra likes của bình luận cha
                    List<LikesBinhLuan> likesBinhLuancha = likesBinhLuanService.GetLikesBinhLuanByidbinhluan(binhLuan.id);
                    foreach (var likebinhluancha in likesBinhLuancha)
                    {
                        tonglikebinhluanthichbuyid += likebinhluancha.thich;
                        tonglikebinhluankhongthichbuyid += likebinhluancha.khongthich;
                    }
                    if (binhLuan.loainguoithamgia.Equals("khachhang"))
                    {
                        khachHang = khachHangService.GetKhachHangbyid(binhLuan.idnguoithamgia);
                    }
                    else
                    {
                        nhanVien = nhanVienService.GetNhanVienID(binhLuan.idnguoithamgia);
                    }
                    /// bình luận con
                    List<BinhLuan> binhLuans = binhLuanService.GetAllBinhLuansByIparent_comment_id(binhLuan.id);
                    foreach (var reply in binhLuans)
                    {
                        int tonglikereplythichbuyid = 0;
                        int tonglikereplykhongthichbuyid = 0;
                        List<LikesBinhLuan> likesBinhLuancon = likesBinhLuanService.GetLikesBinhLuanByidbinhluan(reply.id);
                        foreach (var likebinhluancon in likesBinhLuancon)
                        {
                            tonglikereplythichbuyid += likebinhluancon.thich;
                            tonglikereplykhongthichbuyid += likebinhluancon.khongthich;
                        }
                        reply.Tonglikebinhluanthichbuyid = tonglikereplythichbuyid;
                        reply.Tonglikebinhluankhongthichbuyid = tonglikereplykhongthichbuyid;

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
                        Tonglikebinhluanthichbuyid = tonglikebinhluanthichbuyid,
                        Tonglikebinhluankhongthichbuyid = tonglikebinhluankhongthichbuyid,
                    };
                    listmodeldata.Add(modeldata);
                }
            }

            return Json(listmodeldata);
        }


        public async Task<IActionResult> AddBinhLuan(int idphong, string noidung)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            BinhLuan binhLuan = new BinhLuan();
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                binhLuan.idphong = idphong;
                binhLuan.idnguoithamgia = (int)HttpContext.Session.GetInt32("id");
                binhLuan.loainguoithamgia = "khachhang";
                binhLuan.noidung = noidung;
                binhLuan.trangthai = "Chưa duyệt";
                binhLuan.parent_comment_id = 0;
                binhLuanService.InsertBinhLuan(binhLuan);
                return Json(new { success = true, message = "Bình luận của bạn đã được gửi thành công. Đợi Admin duyệt bình luận của bạn!" });
            }
            else if (result != null && result.Succeeded)
            {
                var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                KhachHang khachHang = khachHangService.GetKhachHangbyidtaikhoangoogle(userId);
                binhLuan.idphong = idphong;
                binhLuan.idnguoithamgia = khachHang.id;
                binhLuan.loainguoithamgia = "khachhang";
                binhLuan.noidung = noidung;
                binhLuan.trangthai = "Chưa duyệt";
                binhLuan.parent_comment_id = 0;
                binhLuanService.InsertBinhLuan(binhLuan);
                return Json(new { success = true, message = "Bình luận của bạn đã được gửi thành công. Đợi Admin duyệt bình luận của bạn!" });
            }
            else
            {
                string thongbao = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return Json(new { thongbao });
            }
        }
        public async Task<IActionResult> AddBinhLuans(int idphong, string noidung, int parent_comment_id)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            BinhLuan binhLuan = new BinhLuan();
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                binhLuan.idphong = idphong;
                binhLuan.idnguoithamgia = (int)HttpContext.Session.GetInt32("id");
                binhLuan.loainguoithamgia = "khachhang";
                binhLuan.noidung = noidung;
                binhLuan.trangthai = "Chưa duyệt";
                binhLuan.parent_comment_id = parent_comment_id;
                binhLuanService.InsertBinhLuan(binhLuan);
                return Json(new { success = true, message = "Bình luận của bạn đã được gửi thành công. Đợi Admin duyệt bình luận của bạn!" });
            }
            else if (result != null && result.Succeeded)
            {
                var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                KhachHang khachHang = khachHangService.GetKhachHangbyidtaikhoangoogle(userId);
                binhLuan.idphong = idphong;
                binhLuan.idnguoithamgia = khachHang.id;
                binhLuan.loainguoithamgia = "khachhang";
                binhLuan.noidung = noidung;
                binhLuan.trangthai = "Chưa duyệt";
                binhLuan.parent_comment_id = parent_comment_id;
                binhLuanService.InsertBinhLuan(binhLuan);
                return Json(new { success = true, message = "Bình luận của bạn đã được gửi thành công. Đợi Admin duyệt bình luận của bạn!" });
            }
            else
            {
                string thongbao = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return Json(new { thongbao });
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
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;

                LikesBinhLuan likesBinhLuan = new LikesBinhLuan();
                likesBinhLuan.idkhachhang = id;
                likesBinhLuan.idbinhluan = idbinhluan;
                likesBinhLuan.thich = 1;
                likesBinhLuan.khongthich = 0;
                likesBinhLuan.thoigianlike = DateTime.Now;
                LikesBinhLuan checklikebinhluan = likesBinhLuanService.CheckLikesBinhLuanIDbinhluanandIdKhachHang(idbinhluan, id);
                if (checklikebinhluan == null)
                {
                    likesBinhLuanService.InsertLike(likesBinhLuan);
                }
                else
                {
                    likesBinhLuanService.DeleteLike(checklikebinhluan.id);
                }
                return Json(new { success = true, message = "Thành công!" });
            }
            else
            {
                string thongbao = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return Json(new { thongbao });
            }
        }

        public IActionResult KhongThichBinhLuan(int idbinhluan)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                LikesBinhLuan likesBinhLuan = new LikesBinhLuan();
                likesBinhLuan.idkhachhang = id;
                likesBinhLuan.idbinhluan = idbinhluan;
                likesBinhLuan.thich = 0;
                likesBinhLuan.khongthich = 1;
                likesBinhLuan.thoigianlike = DateTime.Now;
                LikesBinhLuan checklikebinhluan = likesBinhLuanService.CheckLikesBinhLuanIDbinhluanandIdKhachHang(idbinhluan, id);
                if (checklikebinhluan == null)
                {
                    likesBinhLuanService.InsertLike(likesBinhLuan);
                }
                else
                {
                    likesBinhLuanService.DeleteLike(checklikebinhluan.id);
                }
                return Json(new { success = true, message = "Thành công!" });
            }
            else
            {
                string thongbao = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return Json(new { thongbao });
            }
        }

        public IActionResult DeleteBinhLuan([FromBody] List<int> idbinhluan)
        {
            if (idbinhluan != null && idbinhluan.Any())
            {
                foreach (int id in idbinhluan)
                {
                    binhLuanService.DeleteBinhLuan(id);
                }
                TempData["xoabinhluan"] = "success";
                return Json(new { error = false });
            }
            else
            {
                return Json(new { error = true, message = "Vui lòng chọn bình luận để xoá!" });
            }
        }
        public IActionResult DocVanBans()
        {
            return View();
        }


        public IActionResult DocBinhLuan(string noidung)
        {
            speechSynthesizer.Speak(noidung);
            return Ok();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                speechSynthesizer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}