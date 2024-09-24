using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Service.Service;

namespace QuanLyKhachSan_MVC.NET.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MaQRController : Controller
    {
        private readonly QRCodeRequestService qrCodeRequestService;

        public MaQRController(QRCodeRequestService qrCodeRequestService)
        {
            this.qrCodeRequestService = qrCodeRequestService;
        }
        public IActionResult Index()
        {
            List<QRCodeRequest> QRCodeRequestList = qrCodeRequestService.GetAllQRCodeRequests();

            Modeldata modeldata = new Modeldata()
            {
                QRCodeRequests = QRCodeRequestList,
            };
            return View(modeldata);
        }
    }
}
