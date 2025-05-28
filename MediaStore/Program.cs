using MediaStore.Data;
using MediaStore.Services;
using MediaStore.Services.Email;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Load config mặc định appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Load config appsettings.Development.json nếu môi trường là Development
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AimsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Aims"));
});

// Add service to VnPay - Singleton() 
// Note:
// 1. VnPayService được đăng ký Singleton => chỉ 1 instance duy nhất trong suốt vòng đời app.
// 2. Tuyệt đối không lưu trạng thái liên quan đến HttpContext, Session bên trong VnPayService.
// 3. Mọi method trong VnPayService phải đảm bảo thread-safe.
// 4. Phù hợp cho việc build URL thanh toán, validate SecureHash, lấy config VNPAY.
builder.Services.AddSingleton<IVnPayService, VnPayService>();

builder.Services.AddScoped<IShippingService, ShippingService>();
// Phan them vao
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        // options.AccessDeniedPath = "/Account/AccessDenied";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = 403; // Trả 403 thay vì redirect
                return Task.CompletedTask;
            },
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401; // Trả 401 thay vì redirect
                return Task.CompletedTask;
            }
        };
    });

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<EmailService>();


builder.Services.AddAuthorization(); // Phan them vao

builder.Services.AddControllersWithViews(); // Phan them vao

// Thiet lap de lua gio hang theo session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300);
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

app.UseSession(); // Su dung Session

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication(); // Phan them vao
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
); // Phan them vao

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}")
//     .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();
