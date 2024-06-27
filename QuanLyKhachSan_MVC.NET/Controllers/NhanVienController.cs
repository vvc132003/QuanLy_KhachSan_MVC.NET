using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using PagedList;

namespace QuanLyKhachSan_MVC.NET.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly NhanVienService nhanVienService;
        private readonly ChucVuService chucVuService;

        private readonly HopDongLaoDongService hopDongLaoDongService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;
        public NhanVienController(NhanVienService nhanVienServices,
            ChucVuService chucVuServices,

            HopDongLaoDongService hopDongLaoDongServices,
            KhachSanService khachSanServices,
            KhachHangService khachHangService)
        {
            nhanVienService = nhanVienServices;
            chucVuService = chucVuServices;

            hopDongLaoDongService = hopDongLaoDongServices;
            khachSanService = khachSanServices;
            this.khachHangService = khachHangService;
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
                    List<NhanVien> nhanViens = nhanVienService.GetAllNhanVien();
                    int soluong = nhanViens.Count;
                    int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                    int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                    IPagedList<NhanVien> ipagenhanViens = nhanViens.ToPagedList(validPageNumber, pageSize);
                    List<Modeldata> modeldatalist = new List<Modeldata>();
                    foreach (var nhanvien in ipagenhanViens)
                    {
                        KhachSan khachSan = khachSanService.GetKhachSanById(nhanvien.idkhachsan);
                        ChucVu chucVu = chucVuService.GetChucVuID(nhanvien.idchucvu);
                        Modeldata modeldata = new Modeldata
                        {
                            PagedTNhanVien = new List<NhanVien> { nhanvien }.ToPagedList(1, 1),
                            khachSan = khachSan,
                            chucVu = chucVu,
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
        /*  public IActionResult Index(int? sotrang)
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
                      List<NhanVien> nhanViens = nhanVienService.GetAllNhanVien();
                      int soluong = nhanViens.Count;
                      int validPageNumber = sotrang ?? 1;// Trang hiện tại, mặc định là trang 1
                      int pageSize = Math.Max(soluong, 1); // Số lượng phòng trên mỗi trang
                      IPagedList<NhanVien> ipagenhanViens = nhanViens.ToPagedList(validPageNumber, pageSize);

                      Modeldata modeldata = new Modeldata
                      {
                          PagedTNhanVien = ipagenhanViens,

                      };
                      return View(modeldata);
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
          }*/
        public IActionResult AddNhanVien()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    int id = HttpContext.Session.GetInt32("id").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    ViewData["idkhachsan"] = idkhachsan;
                    List<ChucVu> chucVus = chucVuService.GetAllChucVu();
                    List<KhachSan> listkhachsan = khachSanService.GetAllKhachSan();
                    Modeldata modeldata = new Modeldata
                    {
                        listchucVu = chucVus,
                        listKhachSan = listkhachsan
                    };
                    return View(modeldata);
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
        public IActionResult AddNhanVienn(NhanVien nhanVien, HopDongLaoDong hopDongLaoDong)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                string mahoamatkhau = khachHangService.HashPassword(nhanVien.matkhau);
                nhanVien.matkhau = mahoamatkhau;
                nhanVien.solanvipham = 0;
                nhanVien.trangthai = "Đang hoạt động";
                int idnhanvien = nhanVienService.ThemNhanVien(nhanVien);
                hopDongLaoDong.idnhanvien = idnhanvien;
                hopDongLaoDong.ngaybatdau = DateTime.Now;
                hopDongLaoDongService.ThemHopDongLaoDong(hopDongLaoDong);
                TempData["themthanhcong"] = "";
                return RedirectToAction("Index", "NhanVien");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public IActionResult XuatEclcel()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                nhanVienService.Xuatexcel();
                return RedirectToAction("Index", "NhanVien");
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }
        public ActionResult Index1()
        {
            return View();
        }
        public ActionResult Upload(string imagePath)
        {
            try
            {
                string cascadePath = "haarcascade_frontalface_default.xml";
                CascadeClassifier faceCascade = new CascadeClassifier(cascadePath);

                // Đọc ảnh từ đường dẫn
                Mat image = CvInvoke.Imread(imagePath);

                // Chuyển đổi ảnh sang ảnh xám để tăng hiệu suất
                Mat grayImage = new Mat();
                CvInvoke.CvtColor(image, grayImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                // Nhận diện khuôn mặt trong ảnh xám
                Rectangle[] faces = faceCascade.DetectMultiScale(grayImage, 1.1, 5);

                // Vẽ hình chữ nhật xung quanh các khuôn mặt đã nhận diện được
                foreach (Rectangle face in faces)
                {
                    CvInvoke.Rectangle(image, face, new Bgr(System.Drawing.Color.Red).MCvScalar, 2);
                }

                // Lưu ảnh đã nhận diện khuôn mặt
                string outputImagePath = "detected_faces.jpg";
                CvInvoke.Imwrite(outputImagePath, image);

                // Trả về URL đến ảnh đã nhận diện khuôn mặt
                return Content(outputImagePath);
            }
            catch (Exception e)
            {
                // Xử lý lỗi và trả về thông điệp lỗi
                return Content($"Lỗi không nhận dạng được khuôn mặt: {e.Message}");
            }
        }
    }
}