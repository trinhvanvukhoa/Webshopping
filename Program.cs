using Microsoft.EntityFrameworkCore;
using WebsiteShopping.Data;
using WebsiteShopping.Models;

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

// Cấu hình Authentication Cookie cho Admin
builder.Services.AddAuthentication("AdminCookie")
    .AddCookie("AdminCookie", options =>
    {
        options.LoginPath = "/Admin/Account/Login";
        options.AccessDeniedPath = "/Admin/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// Cấu hình Session cho Giỏ hàng
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
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

app.UseSession();

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
    pattern: "{controller=Product}/{action=Index}/{id?}");

// Seed admin user if not exists
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Username = "admin",
            Password = "admin123",
            FullName = "Administrator",
            Role = "Admin"
        });
        db.SaveChanges();
    }
}

app.Run();
