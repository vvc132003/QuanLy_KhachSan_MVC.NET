using Microsoft.Extensions.FileProviders;
using QuanLyKhachSan_MVC.NET.Models;
using QuanLyKhachSan_MVC.NET.Service;

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
builder.Services.AddScoped<QuyDinhGiamGiaService>();
builder.Services.AddScoped<GiamGiaService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Content/static")),
    RequestPath = "/static"
});
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
