using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service;

namespace QuanLyKhachSan_MVC.NET.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DangNhapController : ControllerBase
    {
        private readonly NhanVienService nhanVienService;
        private readonly KhachSanService khachSanService;
        private readonly KhachHangService khachHangService;

        public DangNhapController(NhanVienService nhanVienServices, KhachSanService khachSanServices, KhachHangService khachHangServices)
        {
            nhanVienService = nhanVienServices;
            khachSanService = khachSanServices;
            khachHangService = khachHangServices;
        }
        [HttpPost]
        [Route("DangNhapVaoHeThong")]
        public IActionResult DangNhapVaoHeThong([FromForm] string taikhoanoremail, [FromForm] string matkhau)
        {
            NhanVien nhanVien = nhanVienService.CheckThongTinDangNhaps(taikhoanoremail);
            KhachHang khachHang = khachHangService.GetKhachHangDangNhaps(taikhoanoremail);
            if (nhanVien != null)
            {
                var checkmatkhau = khachHangService.VerifyPassword(nhanVien.matkhau, matkhau);
                if (checkmatkhau == PasswordVerificationResult.Success)
                {
                    return Ok(nhanVien);
                }
                else
                {
                    return BadRequest(new { status = "error", message = "Incorrect password" });
                }
            }
            else if (khachHang != null)
            {
                var result = khachHangService.VerifyPassword(khachHang.matkhau, matkhau);
                if (result == PasswordVerificationResult.Success)
                {
                    return Ok(khachHang);
                }
                else
                {
                    return BadRequest(new { status = "error", message = "Incorrect password" });
                }
            }
            else
            {
                return BadRequest(new { status = "error", message = "User not found" });
            }
        }

    }
}
