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
        private string DetermineMessageType(string noiDung)
        {
            if (Uri.IsWellFormedUriString(noiDung, UriKind.Absolute))
            {
                return "link";
            }

            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            if (imageExtensions.Any(ext => noiDung.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                return "image";
            }
            return "chữ";
        }


        public async Task SendMessage(int cuocHoiThoaiId, int nhanVienGuiId, string noiDung)
        {
            Console.WriteLine($"Sending message. CuocHoiThoaiId: {cuocHoiThoaiId}, NhanVienGuiId: {nhanVienGuiId}, NoiDung: {noiDung}");
            string loaiTinNhan = DetermineMessageType(noiDung);
            await _tinNhanService.AddTinNhanBuh(cuocHoiThoaiId, nhanVienGuiId, noiDung, loaiTinNhan, "chuaxem");
            CuocHoiThoai cuocHoiThoai = await _cuocHoiThoaiService.GetCuocHoiThoaiByIdHub(cuocHoiThoaiId);
            await _cuocHoiThoaiService.UpdateCuocHoiThoaiHub(cuocHoiThoai);
            await Groups.AddToGroupAsync(Context.ConnectionId, cuocHoiThoaiId.ToString());
            // gửi tin nhắn đến mọi người 
            await Clients.All.SendAsync("ReceiveMessage", cuocHoiThoaiId, nhanVienGuiId, noiDung);
        }



        public async Task MarkMessageAsViewed(int idtinnhan)
        {
            TinNhan tinNhan = await _tinNhanService.GetTinNhanByIdAsync(idtinnhan);
            tinNhan.daXem = "daxem";
            await _tinNhanService.UpdateTinNhanAsync(tinNhan);
            await Clients.All.SendAsync("MessageViewed", idtinnhan);

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