using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Service;
using Service;
using Model.Models;

namespace QuanLyKhachSan_MVC.NET.ControllersApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
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

        [HttpGet("DanhSachCuocTroChuyen")]
        public IActionResult DanhSachCuocTroChuyen()
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;

                List<CuocHoiThoai> cuochoithoailisst = cuocHoiThoaiService.GetCuocHoiThoaiListById(HttpContext.Session.GetInt32("id").Value);
                List<Modeldata> Modeldatas = new List<Modeldata>();
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

                    Modeldata modeldata = new Modeldata
                    {
                        cuocHoiThoai = cuochoithoai,
                        Ten = ten,
                        NoiDungTinNhan = noidungtinnhan,
                        ThoiGianNhan = thoigiannhan
                    };
                    Modeldatas.Add(modeldata);
                }
                return Ok(Modeldatas);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }

        [HttpGet("TinNhanBuycuochoithoaiid")]
        public IActionResult TinNhanBuycuochoithoaiid(int cuochoithoaiid)
        {
            if (HttpContext.Session.GetInt32("id") != null && HttpContext.Session.GetString("tenchucvu") != null && HttpContext.Session.GetString("hovaten") != null)
            {
                int id = HttpContext.Session.GetInt32("id").Value;
                List<TinNhan> tinNhanList = tinNhanService.GetTinNhanListByIdCuocTroChuyen(cuochoithoaiid);
                CuocHoiThoai cuochoithoai = cuocHoiThoaiService.GetCuocHoiThoaiById(cuochoithoaiid);
                List<Modeldata> Modeldatas = new List<Modeldata>();
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
                        Modeldata modeldata = new()
                        {
                            Ten = ten,
                            tinNhan = tinnhan,
                        };
                        Modeldatas.Add(modeldata);
                    }
                }
                return Ok(Modeldatas);
            }
            else
            {
                return RedirectToAction("dangnhap", "dangnhap");
            }
        }



    }
}
