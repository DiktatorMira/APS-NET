using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;
using MusicPortal.Services;

var builder = WebApplication.CreateBuilder(args);
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(options => options.UseLazyLoadingProxies().UseSqlServer(connection));

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.Name = "Session";
});

builder.Services.AddScoped<ICryptography, Cryptography>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ISongsRepository, SongsRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession(); // Не забываем использовать сессии
app.MapControllerRoute(name: "default", pattern: "{controller=Music}/{action=Index}/{id?}");

app.Run();