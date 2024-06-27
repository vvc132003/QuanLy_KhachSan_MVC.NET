using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class VideoController : Controller
    {
       /* private readonly HttpClient _httpClient;
        private readonly string _apiKey;*/
        private static ImageModel _uploadedImage;

   /*     public VideoController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");
            _apiKey = configuration["YouTubeApiKey"];
        }

        public async Task<ActionResult> GetVideoInfo(string videoId)
        {
            string requestUrl = $"videos?id={videoId}&key={_apiKey}&part=snippet";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(responseBody);

                    Console.WriteLine("Response JSON: " + jsonResponse.ToString());  // Log the entire response

                    if (jsonResponse["items"] != null && jsonResponse["items"].HasValues)
                    {
                        var snippet = jsonResponse["items"][0]["snippet"];
                        string title = snippet["title"]?.ToString();
                        string description = snippet["description"]?.ToString();

                        ViewBag.Title = title;
                        ViewBag.Description = description;
                        ViewBag.VideoId = videoId;

                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No video found.";
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = $"Error: {response.StatusCode}";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View("Error");
            }
        }*/
        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var model = new ImageModel();
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    model.ImageData = ms.ToArray();
                    model.ContentType = file.ContentType;
                }
                _uploadedImage = model; // Lưu vào biến tĩnh
            }
            return RedirectToAction(nameof(Video));
        }

        public IActionResult ViewImage()
        {
            if (_uploadedImage == null)
            {
                return NotFound();
            }

            return File(_uploadedImage.ImageData, _uploadedImage.ContentType);
        }
        public IActionResult Video()
        {
            ViewData["ImageAvailable"] = _uploadedImage != null;
            return View();
        }
    }
}
