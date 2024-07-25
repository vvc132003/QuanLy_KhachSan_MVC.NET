using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing; // Sử dụng System.Drawing cho Rectangle
using System.IO;
using System.Linq;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class DiemDanhController : Controller
    {

        public class User
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string ImagePath { get; set; }
        }

        private static List<User> registeredUsers = new List<User>
        {
            new User { Id = 1, UserName = "", ImagePath = "wwwroot/images/pt.jpg" },
            new User { Id = 2, UserName = " chính", ImagePath = "wwwroot/images/pchinhs.png" },
            new User { Id = 3, UserName = " chính", ImagePath = "wwwroot/images/pchinh.png" },
            new User { Id = 4, UserName = "", ImagePath = "wwwroot/images/pt1.png" },
            // Add more users if needed
        };

        public IActionResult Compare()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Compares([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                ViewBag.Message = "File not selected";
                return View("Compare");
            }

            try
            {
                using var stream = image.OpenReadStream();
                using var ms = new MemoryStream();
                stream.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();

                // Convert byte array to Mat
                using var uploadedMat = new Mat();
                CvInvoke.Imdecode(fileBytes, ImreadModes.Color, uploadedMat);

                // Detect faces in the uploaded image
                var detectedFaces = DetectFaces(uploadedMat);

                if (detectedFaces.Count == 0)
                {
                    ViewBag.Message = "No face detected in the uploaded image";
                    return View("Compare");
                }

                bool matchFound = false;
                foreach (var user in registeredUsers)
                {
                    using var userMat = CvInvoke.Imread(user.ImagePath, ImreadModes.Color);
                    var userFaces = DetectFaces(userMat);

                    if (userFaces.Count == 0)
                    {
                        continue;
                    }

                    foreach (var userFace in userFaces)
                    {
                        foreach (var detectedFace in detectedFaces)
                        {
                            var match = CompareFaces(detectedFace, userFace);
                            if (match)
                            {
                                ViewBag.Message = $"Identified as {user.UserName}";
                                matchFound = true;
                                break;
                            }
                        }
                        if (matchFound) break;
                    }
                    if (matchFound) break;
                }

                if (!matchFound)
                {
                    ViewBag.Message = "No matching user found";
                }

                return View("Compare");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                return View("Compare");
            }
        }

        private List<Mat> DetectFaces(Mat image)
        {
            var faceCascade = new CascadeClassifier("wwwroot/cascades/haarcascade_frontalface_default.xml");
            using var gray = new Mat();
            CvInvoke.CvtColor(image, gray, ColorConversion.Bgr2Gray);
            var faces = faceCascade.DetectMultiScale(
                gray,
                scaleFactor: 1.1,    // Thay đổi nếu cần
                minNeighbors: 5,     // Thay đổi nếu cần
                minSize: new Size(30, 30) // Kích thước tối thiểu của khuôn mặt để phát hiện
            );

            var faceMats = new List<Mat>();
            foreach (var face in faces)
            {
                var faceMat = new Mat(image, face);  // Crop the face from the image
                faceMats.Add(faceMat);
            }

            return faceMats;
        }




        private bool CompareFaces(Mat face1, Mat face2)
        {
            Size standardSize = new Size(200, 200); // Define a standard size

            using var resizedFace1 = new Mat();
            using var resizedFace2 = new Mat();

            CvInvoke.Resize(face1, resizedFace1, standardSize);
            CvInvoke.Resize(face2, resizedFace2, standardSize);

            using var gray1 = new Mat();
            using var gray2 = new Mat();
            CvInvoke.CvtColor(resizedFace1, gray1, ColorConversion.Bgr2Gray);
            CvInvoke.CvtColor(resizedFace2, gray2, ColorConversion.Bgr2Gray);

            var orb = new ORB();
            var keypoints1 = new VectorOfKeyPoint();
            var keypoints2 = new VectorOfKeyPoint();
            using var descriptors1 = new UMat();
            using var descriptors2 = new UMat();

            orb.DetectAndCompute(gray1, null, keypoints1, descriptors1, false);
            orb.DetectAndCompute(gray2, null, keypoints2, descriptors2, false);

            if (descriptors1.IsEmpty || descriptors2.IsEmpty)
            {
                return false;
            }       

            var bfMatcher = new BFMatcher(DistanceType.Hamming, crossCheck: true);
            var matches = new VectorOfDMatch();
            bfMatcher.Match(descriptors1, descriptors2, matches);

            var goodMatches = matches.ToArray()
                .Where(m => m.Distance < 30)
                .ToList();

            return goodMatches.Count > 10;
        }






    }
}
