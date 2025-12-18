using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebHomework.Data;
using WebHomework.Models;

var builder = WebApplication.CreateBuilder(args);

// E-posta servisi (Hata almamak için)
builder.Services.AddSingleton<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, WebHomework.Data.EmailSender>();

// 1. Veritabanı Bağlantısı
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity Ayarları
// ÖNEMLİ DÜZELTME: Burayı 'IdentityUser' yaptık ki aşağıdaki kodla uyuşsun.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Mail onayı isteme
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3; // Kısa şifreye izin ver
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// 3. Middleware Ayarları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doğrulama
app.UseAuthorization();  // Yetkilendirme

// Rotaları tanımla
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// ---------------------------------------------------------
// OTOMATİK ADMİN OLUŞTURMA KODU (IdentityUser uyumlu)
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Artık yukarıda IdentityUser tanımladığımız için burası hata vermez:
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // 1. "Admin" rolü yoksa oluştur
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // 2. Kullanıcıyı bul (E-postanı kontrol et)
    var userEmail = "admin@sakarya.edu.tr";
    var user = await userManager.FindByEmailAsync(userEmail);

    if (user != null)
    {
        // 3. Kullanıcıyı Admin rolüne ekle
        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
// ---------------------------------------------------------

app.Run();
