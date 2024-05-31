using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class GiamGiaNgayLeController : Controller
    {
        private readonly GiamGiaNgayLeService giamGiaNgayLeService;
        public GiamGiaNgayLeController(GiamGiaNgayLeService giamGiaNgayLeService)
        {
            this.giamGiaNgayLeService = giamGiaNgayLeService;
        }

        public IActionResult Index()
        {
            List<GiamGiaNgayLe> giamGiaNgayLes = giamGiaNgayLeService.GetAllGiamGiaNgayLe();
            List<Modeldata> modeldatas = new List<Modeldata>();
            foreach (var giamgiangayle in giamGiaNgayLes)
            {
                Modeldata modeldata = new Modeldata()
                {
                    giamGiaNgayle = giamgiangayle,
                };
                modeldatas.Add(modeldata);
            }
            return View(modeldatas);
        }
    }
}
