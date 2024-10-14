using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Racen.Backend.App.Models;
using Racen.Backend.App.Models.Car;
using Racen.Backend.App.Models.User;
using Racen.Backend.App.Models.Accessories;

namespace Racen.Backend.App.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<CarModel> Cars { get; set; }
        public DbSet<AccessoryModel> Accessories { get; set; }
        public DbSet<UserAccessoryModel> UserAccessories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the primary key for RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.Id);

            // Configure the unique constraint for accessories
            modelBuilder.Entity<AccessoryModel>()
                .HasIndex(a => new { a.CarId, a.Type })
                .IsUnique();

            // Configure the relationship between AccessoryModel and CarModel
            modelBuilder.Entity<AccessoryModel>()
                .HasOne(a => a.Car)
                .WithMany(c => c.Accessories)
                .HasForeignKey(a => a.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between UserAccessoryModel and ApplicationUser
            modelBuilder.Entity<UserAccessoryModel>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAccessories)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between UserAccessoryModel and AccessoryModel
            modelBuilder.Entity<UserAccessoryModel>()
                .HasOne(ua => ua.Accessory)
                .WithMany()
                .HasForeignKey(ua => ua.AccessoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}