using Microsoft.EntityFrameworkCore;
using Dz13._05._024.Models;
using Dz13._05._024;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connection));
builder.Services.AddRazorPages();
builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

app.UseStaticFiles();
app.MapRazorPages();

app.Run();