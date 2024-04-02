using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using THLTWeb_WebsiteBanHang.Data;
using THLTWeb_WebsiteBanHang.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<THLTWeb_WebsiteBanHangContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("THLTWeb_WebsiteBanHangContext") ?? throw new InvalidOperationException("Connection string 'THLTWeb_WebsiteBanHangContext' not found.")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
options.IdleTimeout = TimeSpan.FromMinutes(30);
options.Cookie.HttpOnly = true;
options.Cookie.IsEssential = true;
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddDefaultTokenProviders()
.AddDefaultUI()
.AddEntityFrameworkStores<THLTWeb_WebsiteBanHangContext>();
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
