using Microsoft.AspNetCore.SignalR;
using Model.Models;
using Service.Service;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly TinNhanService tinNhanService;
        private readonly CuocHoiThoaiService cuocHoiThoaiService;
        public ChatHub(TinNhanService tinNhanService, CuocHoiThoaiService cuocHoiThoaiService)
        {
            this.tinNhanService = tinNhanService;
            this.cuocHoiThoaiService = cuocHoiThoaiService;
        }
        public async Task SendMessage(int cuochoithoaiid, int nhanvienguiid, string noidung)
        {
            await tinNhanService.AddTinNhanBuh(cuochoithoaiid, nhanvienguiid, noidung);
            CuocHoiThoai cuoc_Hoi_Thoai = await cuocHoiThoaiService.GetCuocHoiThoaiByIdHub(cuochoithoaiid);
            await cuocHoiThoaiService.UpdateCuocHoiThoaiHub(cuoc_Hoi_Thoai);
            await Clients.All.SendAsync("ReceiveMessage", cuochoithoaiid, nhanvienguiid, noidung);
        }
        public async Task SendMessages(int cuochoithoaiid, int nhanvienguiid, string noidung)
        {
            noidung = "👍";
            await tinNhanService.AddTinNhanBuh(cuochoithoaiid, nhanvienguiid, noidung);
            CuocHoiThoai cuoc_Hoi_Thoai = await cuocHoiThoaiService.GetCuocHoiThoaiByIdHub(cuochoithoaiid);
            await cuocHoiThoaiService.UpdateCuocHoiThoaiHub(cuoc_Hoi_Thoai);
            await Clients.All.SendAsync("ReceiveMessages", cuochoithoaiid, nhanvienguiid, noidung);
        }
    }
}