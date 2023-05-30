using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YasinFinal.DAL;
using YasinFinal.Models;
using YasinFinal.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<LayoutServices>();
var app = builder.Build();



app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller}/{action}/{id?}"
    );
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
