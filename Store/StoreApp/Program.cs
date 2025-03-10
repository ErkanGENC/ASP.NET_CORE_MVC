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
using Microsoft.AspNetCore.Identity;
using StoreApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veritabanı bağlantısını yapılandırma
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RepositoryContext>(options => 
    options.UseSqlite(connectionString, 
    b => b.MigrationsAssembly("StoreApp"))); // Migration'ların StoreApp projesinde oluşturulmasını sağlar

// Identity servislerini ekle
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    // Şifre gereksinimleri
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // Kullanıcı gereksinimleri
    options.User.RequireUniqueEmail = true;
    
    // Lockout ayarları
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<RepositoryContext>()
.AddDefaultTokenProviders();

// Cookie ayarları
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = false;
    options.Cookie.Name = "StoreApp.Identity";
});

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

// Kimlik doğrulama ve yetkilendirme middleware'leri
app.UseAuthentication();
app.UseAuthorization();

// Veritabanını seed'leme
app.ConfigureAndSeedDb();

// Identity rolleri ve admin kullanıcı oluşturma
await IdentitySeed.SeedRolesAsync(app);
await IdentitySeed.SeedAdminAsync(app);

// Manuel olarak admin kullanıcısı oluşturma ve rol atama


// Area için route tanımı
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
