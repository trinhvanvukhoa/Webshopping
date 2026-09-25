using Microsoft.EntityFrameworkCore;
using WebsiteShopping.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = "";
if (builder.Environment.IsDevelopment())
{
    // Lấy chuỗi DevConnection trong appsettings.Development.json
    connectionString = builder.Configuration.GetConnectionString("DevConnection")
    ?? throw new InvalidOperationException("Connection string 'DevConnection' not found.");
    Console.WriteLine("Đang chạy ở môi trường: DEVELOPMENT");
}
else
{
    // Lấy chuỗi ProdConnection trong appsettings.json
    connectionString = builder.Configuration.GetConnectionString("ProdConnection")
    ?? throw new InvalidOperationException("Connection string 'ProdConnection' not found.");
    Console.WriteLine("Đang chạy ở môi trường: PRODUCTION");
}
// Đăng ký ApplicationDbContext vào hệ thống Dependency Injection (DI)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));
// Thêm các service khác (Controllers, Swagger...)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin",
    defaults: new
    {
        area = "Admin",
        controller = "Dashboard",
        action = "Index"
    });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
