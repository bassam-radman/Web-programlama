using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebHomework.Models;

namespace WebHomework.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // --- TEMİZLENMİŞ TABLOLAR ---

        // 1. Hizmetler
        public DbSet<GymService> GymServices { get; set; }

        // 2. Eğitmenler (Instructor'ı sildik, sadece Trainer kaldı)
        public DbSet<Trainer> Trainers { get; set; }

        // 3. Siparişler ve Randevular (Appointment'ı sildik, Order her şeyi yapıyor)
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Price için decimal ayarı
            builder.Entity<GymService>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            builder.Entity<Order>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
