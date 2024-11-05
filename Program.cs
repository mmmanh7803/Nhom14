using Microsoft.EntityFrameworkCore;
using QLcaulacbosinhvien.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies; // Add this line
using Microsoft.AspNetCore.Http.Features;
using QLcaulacbosinhvien.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");



builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connection));

builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10); // Thời gian sống của session
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        
    });



var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseStaticFiles();


 app.UseAuthorization();

app.MapControllerRoute(
            name : "areas",
            pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}");
       

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
