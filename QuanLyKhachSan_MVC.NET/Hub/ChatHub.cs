using Microsoft.AspNetCore.SignalR;
using Model.Models;
using Service.Service;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly TinNhanService _tinNhanService;
        private readonly CuocHoiThoaiService _cuocHoiThoaiService;

        public ChatHub(TinNhanService tinNhanService, CuocHoiThoaiService cuocHoiThoaiService)
        {
            _tinNhanService = tinNhanService;
            _cuocHoiThoaiService = cuocHoiThoaiService;
        }

        public async Task SendMessage(int cuocHoiThoaiId, int nhanVienGuiId, string noiDung)
        {
            Console.WriteLine($"Sending message. CuocHoiThoaiId: {cuocHoiThoaiId}, NhanVienGuiId: {nhanVienGuiId}, NoiDung: {noiDung}");

            // Add the message to the database
            await _tinNhanService.AddTinNhanBuh(cuocHoiThoaiId, nhanVienGuiId, noiDung);

            // Update the conversation
            CuocHoiThoai cuocHoiThoai = await _cuocHoiThoaiService.GetCuocHoiThoaiByIdHub(cuocHoiThoaiId);
            await _cuocHoiThoaiService.UpdateCuocHoiThoaiHub(cuocHoiThoai);

            await Groups.AddToGroupAsync(Context.ConnectionId, cuocHoiThoaiId.ToString());

            // Notify all clients in the group
            await Clients.Group(cuocHoiThoaiId.ToString()).SendAsync("ReceiveMessage", cuocHoiThoaiId, nhanVienGuiId, noiDung);

            // Add logging
            Console.WriteLine($"Sent message to group {cuocHoiThoaiId}: {noiDung}");
        }



        public async Task JoinGroup(int cuocHoiThoaiId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, cuocHoiThoaiId.ToString());
        }

        public async Task LeaveGroup(int cuocHoiThoaiId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, cuocHoiThoaiId.ToString());
        }
       /* public async Task SendMessages(int cuochoithoaiid, int nhanvienguiid, string noidung)
        {
            noidung = "👍";
            await tinNhanService.AddTinNhanBuh(cuochoithoaiid, nhanvienguiid, noidung);
            CuocHoiThoai cuoc_Hoi_Thoai = await cuocHoiThoaiService.GetCuocHoiThoaiByIdHub(cuochoithoaiid);
            await cuocHoiThoaiService.UpdateCuocHoiThoaiHub(cuoc_Hoi_Thoai);
            await Clients.All.SendAsync("ReceiveMessages", cuochoithoaiid, nhanvienguiid, noidung);
        }*/
    }
}