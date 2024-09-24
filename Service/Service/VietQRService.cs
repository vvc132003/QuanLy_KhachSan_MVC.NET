using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;


namespace Service.Service
{
    public class VietQRService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public VietQRService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateQRCodeAsync(double sotienthanhtoan, string taikhoan, string tentaikhoan, string machinhanh, string tenkhachhang)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.vietqr.io/v2/generate");
            request.Headers.Add("x-client-id", _configuration["VietQR:ClientId"]);
            request.Headers.Add("x-api-key", _configuration["VietQR:ApiKey"]);

            var payload = new
            {
                accountNo = taikhoan,
                accountName = tentaikhoan,
                acqId = machinhanh,
                addInfo = $"{tenkhachhang} CHUYỂN TIỀN HÓA ĐƠN",
                amount = sotienthanhtoan,
                template = "print"
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<VietQRResponse>(responseContent);

            // Kiểm tra null trước khi truy cập thuộc tính
            if (result?.Data?.QrDataURL == null)
            {
                throw new Exception("Failed to generate QR code. The API response does not contain QR code data.");
            }

            return result.Data.QrDataURL;
        }
        public class VietQRResponse
        {
            public VietQRData Data { get; set; }
        }
        public class VietQRData
        {
            public string QrDataURL { get; set; }
        }
    }
}
