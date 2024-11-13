using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Racen.Backend.App.Models;
using Racen.Backend.App.Models.User;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            RefreshTokens = Set<RefreshToken>();
            Motorcycles = Set<Motorcycle>();
            Items = Set<Items>();
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Items> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the primary key for RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.Id);

        }
    }
}