using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChatController : Controller
    {
        private readonly CuocHoiThoaiService cuocHoiThoaiService;
        private readonly NguoiThamGiaService nguoiThamGiaService;
        private readonly NhanVienService nhanVienService;
        private readonly TinNhanService tinNhanService;
        public ChatController(CuocHoiThoaiService cuocHoiThoaiService, NguoiThamGiaService nguoiThamGiaService, NhanVienService nhanVienService, TinNhanService tinNhanService)
        {
            this.cuocHoiThoaiService = cuocHoiThoaiService;
            this.nguoiThamGiaService = nguoiThamGiaService;
            this.nhanVienService = nhanVienService;
            this.tinNhanService = tinNhanService;
        }

        public IActionResult Chat()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                if (HttpContext.Session.GetString("tenchucvu").Equals("Quản lý"))
                {
                    int id = HttpContext.Session.GetInt32("id").Value;
                    int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                    string hovaten = HttpContext.Session.GetString("hovaten");
                    string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                    ViewData["idkhachsan"] = idkhachsan;
                    ViewData["id"] = id;
                    ViewData["hovaten"] = hovaten;
                    ViewData["tenchucvu"] = tenchucvu;
                    List<NhanVien> nhanViens = nhanVienService.GetAllNhanVien();
                    Modeldata modeldata = new()
                    {
                        listnhanVien = nhanViens
                    };
                    return View(modeldata);
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
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetInt32("idkhachsan") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                int idkhachsan = HttpContext.Session.GetInt32("idkhachsan").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["idkhachsan"] = idkhachsan;
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                return View();
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult DanhSachCuocTroChuyen()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<CuocHoiThoai> cuochoithoailisst = cuocHoiThoaiService.GetCuocHoiThoaiListById(id);
                string html = "";
                foreach (var cuochoithoai in cuochoithoailisst)
                {
                    string ten = "";
                    if (cuochoithoai.LoaiHoiThoai == "1-1")
                    {
                        List<NguoiThamGia> nguoiThamGialist = nguoiThamGiaService.GetNguoiThamGiaListById(cuochoithoai.Id);
                        foreach (var nguoithamgia in nguoiThamGialist)
                        {
                            NhanVien nhanVienthamgia = nhanVienService.GetNhanVienID(nguoithamgia.NhanVienThamGiaId);
                            if (nhanVienthamgia != null)
                            {
                                if (nguoithamgia.NhanVienThamGiaId != id)
                                {
                                    ten = nhanVienthamgia.hovaten;
                                    break;
                                }
                            }
                        }
                    }
                    else if (cuochoithoai.LoaiHoiThoai == "nhóm")
                    {
                        ten = cuochoithoai.Tieude;
                    }
                    TinNhan tinNhan = tinNhanService.GetTinNhanByIdHoithoaitinnhanmoinhat(cuochoithoai.Id);
                    string noidungtinnhan = "";
                    DateTime thoigiannhan;
                    if (tinNhan != null)
                    {
                        noidungtinnhan = tinNhan.NoiDung;
                        thoigiannhan = tinNhan.DuocTaoVao;
                    }
                    else
                    {
                        noidungtinnhan = "";
                        thoigiannhan = DateTime.Now;
                    }
                    html += $@"
                                <div class=""conversation"" onclick=""displayMessages({cuochoithoai.Id})"">
                                    <img src=""https://images.fpt.shop/unsafe/filters:quality(90)/fptshop.com.vn/uploads/images/tin-tuc/157765/Originals/15(1).jpg""
                                        alt="""">
                                    <div class=""details"">
                                        <h3>{ten}</h3>
                                        <p>{noidungtinnhan}</p>
                                    </div>
                                    <span class=""time"">{thoigiannhan}</span>
                                </div>
";
                }
                return Content(html);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult HienThiNhanVienNhan(int cuochoithoaiid)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                CuocHoiThoai cuochoithoai = cuocHoiThoaiService.GetCuocHoiThoaiById(cuochoithoaiid);
                string html = "";
                string ten = "";
                if (cuochoithoai.LoaiHoiThoai == "1-1")
                {
                    List<NguoiThamGia> nguoiThamGialist = nguoiThamGiaService.GetNguoiThamGiaListById(cuochoithoai.Id);
                    foreach (var nguoithamgia in nguoiThamGialist)
                    {
                        NhanVien nhanVienthamgia = nhanVienService.GetNhanVienID(nguoithamgia.NhanVienThamGiaId);
                        if (nhanVienthamgia != null)
                        {
                            if (nguoithamgia.NhanVienThamGiaId != id)
                            {
                                ten = nhanVienthamgia.hovaten;
                                break;
                            }
                        }
                    }
                }
                else if (cuochoithoai.LoaiHoiThoai == "nhóm")
                {
                    ten = cuochoithoai.Tieude;
                }
                html += $@"
                               <img src=""user-profile-image.jpg"" alt=""User Profile Image"" class=""profile-image"">
                                <h3>{ten}</h3>
";

                return Content(html);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult HienThiThongTinHoiThoai(int cuochoithoaiid)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                CuocHoiThoai cuochoithoai = cuocHoiThoaiService.GetCuocHoiThoaiById(cuochoithoaiid);
                string html = "";
                string ten = "";
                if (cuochoithoai.LoaiHoiThoai == "1-1")
                {
                    List<NguoiThamGia> nguoiThamGialist = nguoiThamGiaService.GetNguoiThamGiaListById(cuochoithoai.Id);
                    foreach (var nguoithamgia in nguoiThamGialist)
                    {
                        NhanVien nhanVienthamgia = nhanVienService.GetNhanVienID(nguoithamgia.NhanVienThamGiaId);
                        if (nhanVienthamgia != null)
                        {
                            if (nguoithamgia.NhanVienThamGiaId != id)
                            {
                                ten = nhanVienthamgia.hovaten;
                                break;
                            }
                        }
                    }
                }
                else if (cuochoithoai.LoaiHoiThoai == "nhóm")
                {
                    ten = cuochoithoai.Tieude;
                }
                html += $@"
                                <input type=""hidden"" id=""cuochoithoaiidinput"" value=""{cuochoithoaiid}"">
                                <input type=""hidden"" id=""nhanvienguiidinput"" value=""{id}"">
";
                return Content(html);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
        public IActionResult TinNhanBuycuochoithoaiid(int cuochoithoaiid)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                string hovaten = HttpContext.Session.GetString("hovaten");
                string tenchucvu = HttpContext.Session.GetString("tenchucvu");
                ViewData["id"] = id;
                ViewData["hovaten"] = hovaten;
                ViewData["tenchucvu"] = tenchucvu;
                List<TinNhan> tinNhanList = tinNhanService.GetTinNhanListByIdCuocTroChuyen(cuochoithoaiid);
                CuocHoiThoai cuochoithoai = cuocHoiThoaiService.GetCuocHoiThoaiById(cuochoithoaiid);
                string html = "";
                if (tinNhanList.Count > 0)
                {
                    foreach (var tinnhan in tinNhanList)
                    {
                        string ten = "";
                        if (cuochoithoai.LoaiHoiThoai == "1-1")
                        {
                            List<NguoiThamGia> nguoiThamGialist = nguoiThamGiaService.GetNguoiThamGiaListById(cuochoithoai.Id);
                            foreach (var nguoithamgia in nguoiThamGialist)
                            {
                                NhanVien nhanVienthamgia = nhanVienService.GetNhanVienID(nguoithamgia.NhanVienThamGiaId);
                                if (nhanVienthamgia != null)
                                {
                                    if (nguoithamgia.NhanVienThamGiaId != id)
                                    {
                                        ten = nhanVienthamgia.hovaten;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (cuochoithoai.LoaiHoiThoai == "nhóm")
                        {
                            ten = cuochoithoai.Tieude;
                        }
                        string classs = (tinnhan.NhanVienGuiId != id) ? "received" : "sent";
                        html += $@"
                                <div class=""message {classs}"">
                                    <p>{tinnhan.NoiDung}</p>
                                    <span class=""time"">{tinnhan.DuocTaoVao}</span>
                                </div>
                            ";
                    }
                }
                else
                {

                }
                return Content(html);
            }
            else
            {
                return Redirect("~/customer/dangnhap/dangnhap");
            }
        }
    }
}
