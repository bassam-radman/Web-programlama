using Microsoft.AspNetCore.Identity;
using WebHomework.Models;

namespace WebHomework.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Kullanıcı ve Rol Yöneticilerini servisten alıyoruz
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. ROLLERİ OLUŞTUR (Admin ve Uye)
            await CreateRoleAsync(roleManager, "Admin");
            await CreateRoleAsync(roleManager, "Uye");

            // 2. ADMIN KULLANCISINI OLUŞTUR
            // İstenen Admin Bilgileri
            string adminEmail = "ogrencinumarasi@sakarya.edu.tr"; // Burayı kendi numaranla değiştirebilirsin
            string adminPassword = "sau";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                // Kullanıcıyı oluştur
                var result = await userManager.CreateAsync(newAdmin, adminPassword);

                if (result.Succeeded)
                {
                    // Kullanıcıya Admin rolünü ata
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }

        // Yardımcı metot: Rol yoksa oluşturur
        private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
