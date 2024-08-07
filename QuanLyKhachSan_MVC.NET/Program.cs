
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;
using Model.Models;
using NPOI.SS.Formula.Functions;
using Service;
using Service.Service;
using SignalRChat.Hubs;
using System.Security.Claims;
using MathNet.Numerics.LinearAlgebra.Factorization;
using NPOI.POIFS.Properties;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Net.NetworkInformation;
using System.Net;
using QuanLyKhachSan_MVC.NET.ControllersApi;
using System.Speech.Synthesis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<TangService>();
builder.Services.AddScoped<PhongService>();
builder.Services.AddScoped<ChucVuService>();
builder.Services.AddScoped<NhanVienService>();
builder.Services.AddScoped<DatPhongService>();
/*builder.Services.AddScoped<KhachHangService>();*/
builder.Services.AddScoped<NhanPhongService>();
builder.Services.AddScoped<SanPhamService>();
builder.Services.AddScoped<ThueSanPhamService>();
builder.Services.AddScoped<ChuyenPhongService>();
builder.Services.AddScoped<LichSuThanhToanService>();
builder.Services.AddScoped<ThietBiService>();
builder.Services.AddScoped<ThietBiPhongService>();
builder.Services.AddScoped<ThoiGianService>();
builder.Services.AddScoped<SuDungMaGiamGiaService>();
builder.Services.AddScoped<MaGiamGiaService>();
builder.Services.AddScoped<HopDongLaoDongService>();
builder.Services.AddScoped<KhachSanService>();
builder.Services.AddScoped<PhongKhachSanService>();
builder.Services.AddScoped<HuyDatPhongService>();
builder.Services.AddScoped<CuocHoiThoaiService>();
builder.Services.AddScoped<NguoiThamGiaService>();
builder.Services.AddScoped<TinNhanService>();
builder.Services.AddScoped<TinNhanIconService>();
builder.Services.AddScoped<IconService>();
builder.Services.AddScoped<GiamGiaNgayLeService>();
builder.Services.AddScoped<LikesService>();
builder.Services.AddScoped<BinhLuanService>();
builder.Services.AddScoped<LikesBinhLuanService>();
builder.Services.AddScoped<LoaiDichDichVuService>();
builder.Services.AddScoped<XacMinhService>();
builder.Services.AddScoped<GopDonDatPhongService>();


builder.Services.AddScoped<SpeechSynthesizer>(); // Đăng ký như Singleton nếu bạn muốn sử dụng cùng một instance cho toàn bộ ứng dụng


// mã hoá mật khẩu
builder.Services.AddScoped<IPasswordHasher<KhachHang>, PasswordHasher<KhachHang>>();
builder.Services.AddScoped(sp => new KhachHangService(new PasswordHasher<KhachHang>()));



builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
      .AddCookie()
      .AddGoogle(options =>
      {
          options.ClientId = "675174564412-1qjefhnl3fm17hsl35irv69fnecgf66b.apps.googleusercontent.com";
          options.ClientSecret = "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";
          options.CallbackPath = new PathString("/signin-google");
          options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
          options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
          options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
          options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
          options.ClaimActions.MapJsonKey("urn:google:profile", "link");
          options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
          options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userid");

      });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllowAnonymous", policy => policy.RequireAssertion(context => true));
});
builder.Services.AddHttpClient();



// Đăng ký dịch vụ xử lý tệp tĩnh
builder.Services.AddDirectoryBrowser();


builder.Services.AddSession(options =>
{
    // phiên làm việc khi không có hoạt động nào từ người dùng
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    // Thuộc tính này được đặt là true, điều này đảm bảo rằng cookie session chỉ có thể được truy cập bởi HTTP
    // và không thể được truy cập thông qua JavaScript.
    // Điều này giúp bảo vệ cookie chống lại các cuộc tấn công Cross-Site Scripting (XSS).
    options.Cookie.HttpOnly = true;
    // Thuộc tính này là true, cho biết rằng cookie session là bắt buộc cho hoạt động của ứng dụng
    // và không nên bị loại bỏ. Điều này quan trọng đặc biệt khi cấu hình GDPR(General Data Protection Regulation)
    // trong khu vực EU, vì nó sẽ xác định rằng cookie này là bắt buộc để ứng dụng hoạt động chính xác.
    options.Cookie.IsEssential = true;
    ///Tại sao cần cấu hình như vậy?
    //IdleTimeout: Việc thiết lập thời gian hết hạn giúp giảm thiểu nguy cơ bảo mật nếu một phiên làm việc không được sử dụng quá lâu.
    //Nếu không có thời gian hết hạn, một phiên có thể vẫn tồn tại trong bộ nhớ và gây nguy cơ bảo mật nếu bị chiếm đoạt.
    //Cookie.HttpOnly: Bảo vệ cookie session tránh bị tấn công XSS bằng cách không cho phép truy cập từ JavaScript.
    //Điều này giảm thiểu nguy cơ bị đánh cắp thông tin phiên bằng cách lợi dụng lỗ hổng XSS.
    //Cookie.IsEssential: Đảm bảo rằng cookie session được xem là bắt buộc, giúp đảm bảo rằng các cài đặt bảo mật và quyền riêng tư được tuân thủ đầy đủ.
    //Việc cấu hình này giúp cải thiện bảo mật và đảm bảo tính ổn định của ứng dụng ASP.NET Core trong việc quản lý phiên làm việc của người dùng.
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    new[] { "application/octet-stream" });
});
var app = builder.Build();
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
/*app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Content/static")),
    RequestPath = "/static"
});*/
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "customer_default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Customer" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.UseResponseCompression();
app.MapHub<ChatHub>("/chathub");
app.Run();