using Microsoft.AspNetCore.Mvc;
using Model.Models;
using PagedList;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
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
                    return RedirectToAction("dangnhap", "dangnhap");
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }

        public IActionResult LikesPhong(int idphong, string icon)
        {
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
                    Likes likes = new Likes();
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
            else
            {
                // Nếu không đăng nhập, trả về thông báo lỗi
                string thongbao = "Bạn cần đăng nhập để thực hiện thao tác này.";
                return Json(new { thongbao });
            }
        }
        public IActionResult CountLikeByIdPhong(int idphong)
        {
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
