using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using MusicPortal.Services;

var builder = WebApplication.CreateBuilder(args);
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connection));

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.Name = "Session";
});

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ICryptography, Cryptography>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession(); // Не забываем использовать сессии
app.MapControllerRoute(name: "default", pattern: "{controller=Main}/{action=Index}/{id?}");

app.Run();