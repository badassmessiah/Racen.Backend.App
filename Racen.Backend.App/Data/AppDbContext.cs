using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Racen.Backend.App.Models;
using Racen.Backend.App.Models.Car;
using Racen.Backend.App.Models.User;

namespace Racen.Backend.App.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<CarModel> Cars { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the primary key for RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.Id);
        }
    }
}