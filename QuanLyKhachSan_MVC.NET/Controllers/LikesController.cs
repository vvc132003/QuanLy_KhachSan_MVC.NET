using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class LikesController : Controller
    {
        private readonly LikesService likesService;
        public LikesController(LikesService likesService)
        {
            this.likesService = likesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LikesPhong(int idphong, string icon)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                bool isLiked = likesService.CheckIfLiked(id, idphong);
                if (!isLiked)
                {
                    Likes likes = new Likes();
                    likes.idkhachhang = id;
                    likes.idphong = idphong;
                    likes.icons = icon;
                    likesService.InsertLike(likes);
                    return Ok();
                }
                else
                {
                    Likes likes = likesService.GetIconsLikedByIDkHACHhNAGbyidPhong(idphong, id);
                    if (likes != null)
                    {
                        likes.icons = icon;
                        likesService.CapNhatLike(likes);
                        return Ok();
                    }
                    return Ok();
                }
            }
            else
            {
                string thongbao = "vui lòng đăng nhập trước khi like !";
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
                Likes likes = likesService.GetIconsLikedByIDkHACHhNAG(id);
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
    }
}
