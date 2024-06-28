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

        public IActionResult LikesPhong(int idphong)
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
                    likes.icons = "❤️";
                    likesService.InsertLike(likes);
                    return Ok();
                }
                else
                {
                    return Ok();
                }
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult CountLikeByIdPhong(int idphong)
        {
            int sumlikesidphong = likesService.CountLikesByIdPhong(idphong);
            return Json(new { sumlikesidphong });
        }
    }
}
