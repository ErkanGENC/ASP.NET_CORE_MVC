using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories.Contracts;
using Repositories;
using Services.Abstract;
using Services.Concrete;
using StoreApp.Models;
using Microsoft.AspNetCore.Http;
using StoreApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veritabanı bağlantısını yapılandırma
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RepositoryContext>(options => 
    options.UseSqlite(connectionString, 
    b => b.MigrationsAssembly("StoreApp"))); // Migration'ların StoreApp projesinde oluşturulmasını sağlar

// Repository ve Service kayıtları
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();

// Sepet servisi için session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

// Cart (Sepet) servis kaydı
builder.Services.AddScoped<Cart>(c => SessionCart.GetCart(c));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthorization();

// Veritabanını seed'leme
app.ConfigureAndSeedDb();

// Area için route tanımı
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
