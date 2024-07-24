using Microsoft.AspNetCore.Mvc;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<ImageController> _logger;

        public ImageController(ILogger<ImageController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                _logger.LogWarning("Invalid file type: {FileName}", file.FileName);
                TempData["Message"] = "Invalid file type";
                return RedirectToAction("Index");
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
            }
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return RedirectToAction("Index");
        }
    }
}