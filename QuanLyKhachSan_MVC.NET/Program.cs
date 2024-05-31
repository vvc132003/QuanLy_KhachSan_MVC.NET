using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;
using Model.Models;
using Service;
using Service.Service;
using SignalRChat.Hubs;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<TangService>();
builder.Services.AddScoped<PhongService>();
builder.Services.AddScoped<ChucVuService>();
builder.Services.AddScoped<BoPhanService>();
builder.Services.AddScoped<ViTriBoPhanService>();
builder.Services.AddScoped<NhanVienService>();
builder.Services.AddScoped<LoaiDatPhongService>();
builder.Services.AddScoped<DatPhongService>();
builder.Services.AddScoped<KhachHangService>();
builder.Services.AddScoped<NhanPhongService>();
builder.Services.AddScoped<SanPhamService>();
builder.Services.AddScoped<ThueSanPhamService>();
builder.Services.AddScoped<ChuyenPhongService>();
builder.Services.AddScoped<TraPhongService>();
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



// Add services to the container.
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

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
    name: "default",
    pattern: "{controller=Home}/{Action=Index}/{id?}");
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.UseResponseCompression();
app.MapHub<ChatHub>("/chathub");
app.Run();