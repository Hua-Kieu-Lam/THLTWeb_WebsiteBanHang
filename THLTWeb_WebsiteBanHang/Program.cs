using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using THLTWeb_WebsiteBanHang.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<THLTWeb_WebsiteBanHangContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("THLTWeb_WebsiteBanHangContext") ?? throw new InvalidOperationException("Connection string 'THLTWeb_WebsiteBanHangContext' not found.")));
// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
