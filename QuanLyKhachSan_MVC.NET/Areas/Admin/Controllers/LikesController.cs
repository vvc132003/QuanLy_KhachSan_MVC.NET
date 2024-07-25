using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;
using Service.Service;
using System.Security.Claims;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LikesController : Controller
    {
        private readonly LikesService likesService;
        private readonly KhachHangService khachHangService;
        private readonly PhongService phongService;
        public LikesController(LikesService likesService, KhachHangService khachHangService, PhongService phongService)
        {
            this.likesService = likesService;
            this.khachHangService = khachHangService;
            this.phongService = phongService;
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
                    List<Likes> likes = likesService.GetAllLikes();
                    int soluong = likes.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    IPagedList<Likes> ipagelikes = likes.ToPagedList(validPageNumber, pageSize);
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var like in ipagelikes)
                    {
                        KhachHang khachHang = khachHangService.GetKhachHangbyid(like.idkhachhang);
                        Phong phong = phongService.GetPhongID(like.idphong);
                        Modeldata modeldata = new Modeldata
                        {
                            PagedTLikes = new List<Likes> { like }.ToPagedList(1, 1),
                            khachhang = khachHang,
                            phong = phong,
                        };
                        modeldatalist.Add(modeldata);
                    }
                    return View(modeldatalist);
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

        public async Task<IActionResult> LikesPhong(int idphong, string icon)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Likes likes = new Likes();
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                // Kiểm tra xem người dùng đã like phòng này chưa
                bool isLiked = likesService.CheckIfLiked(id, idphong);
                // Nếu chưa like, thêm mới
                if (!isLiked)
                {
                    likes.idkhachhang = id;
                    likes.idphong = idphong;
                    likes.icons = icon;
                    likesService.InsertLike(likes);
                }
                else
                {
                    // Nếu đã like, cập nhật hoặc xóa
                    Likes existingLike = likesService.GetIconsLikedByIDkHACHhNAGbyidPhong(idphong, id);
                    if (existingLike != null)
                    {
                        // Nếu icon đã được chọn trước đó, xóa like
                        if (existingLike.icons.Equals(icon))
                        {
                            likesService.DeleteLike(existingLike.id);
                        }
                        else
                        {
                            // Nếu icon khác, cập nhật lại icon
                            existingLike.icons = icon;
                            likesService.CapNhatLike(existingLike);
                        }
                    }
                }
                // Trả về kết quả thành công
                return Ok();
            }
            else if (result != null && result.Succeeded)
            {
                var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                KhachHang khachHang = khachHangService.GetKhachHangbyidtaikhoangoogle(userId);
                // Kiểm tra xem người dùng đã like phòng này chưa
                bool isLiked = likesService.CheckIfLiked(khachHang.id, idphong);
                // Nếu chưa like, thêm mới
                if (!isLiked)
                {
                    likes.idkhachhang = khachHang.id;
                    likes.idphong = idphong;
                    likes.icons = icon;
                    likesService.InsertLike(likes);
                }
                else
                {
                    // Nếu đã like, cập nhật hoặc xóa
                    Likes existingLike = likesService.GetIconsLikedByIDkHACHhNAGbyidPhong(idphong, khachHang.id);
                    if (existingLike != null)
                    {
                        // Nếu icon đã được chọn trước đó, xóa like
                        if (existingLike.icons.Equals(icon))
                        {
                            likesService.DeleteLike(existingLike.id);
                        }
                        else
                        {
                            // Nếu icon khác, cập nhật lại icon
                            existingLike.icons = icon;
                            likesService.CapNhatLike(existingLike);
                        }
                    }
                }
                return Ok();
            }
            else
            {
                // Nếu không đăng nhập, trả về thông báo lỗi
                string thongbao = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return Json(new { thongbao });
            }
        }
        public async Task<IActionResult> CountLikeByIdPhong(int idphong)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            int sumlikesidphong = likesService.CountLikesByIdPhong(idphong);
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                Likes likes = likesService.GetIconsLikedByIDkHACHhNAGbyidPhong(idphong, id);
                if (likes != null)
                {
                    return Json(new { sumlikesidphong, icon = likes.icons });
                }
                else
                {
                    return Json(new { sumlikesidphong, icon = "" });
                }
            }
            else if (result != null && result.Succeeded)
            {
                var userId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                KhachHang khachHang = khachHangService.GetKhachHangbyidtaikhoangoogle(userId);
                Likes likes = likesService.GetIconsLikedByIDkHACHhNAGbyidPhong(idphong, khachHang.id);
                if (likes != null)
                {
                    return Json(new { sumlikesidphong, icon = likes.icons });
                }
                else
                {
                    return Json(new { sumlikesidphong, icon = "" });
                }
            }
            else
            {
                return Json(new { sumlikesidphong });
            }
        }
        public IActionResult Deletelikes(int idlikes)
        {
            likesService.DeleteLike(idlikes);
            return RedirectToAction("", "likes");
        }
    }
}
