using BrewViewServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer
{
    public class BrewContext : IdentityDbContext<AppUser>
    {
        public BrewContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Brew> Brews { get; set; }
        public DbSet<AppUserBrew> AppUserBrews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUserBrew>().HasKey(o => new {o.ProductId, Id = o.AppUserId});

            base.OnModelCreating(builder);
        }
    }
}