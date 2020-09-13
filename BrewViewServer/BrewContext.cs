using BrewViewServer.Models;
using BrewViewServer.Models.User;
using BrewViewServer.Models.VinmonopolModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BrewViewServer
{
    public class BrewContext : DbContext
    {
        public BrewContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Brew> Brews { get; set; }
        public DbSet<UserBrew> UserBrews { get; set; }
        public DbSet<AlcoholicEntity> AlcoholicEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserBrew>().HasKey(o => new { o.ProductId, Id = o.UserId });

            base.OnModelCreating(builder);
        }
    }
}