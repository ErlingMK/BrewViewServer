using BrewView.DatabaseModels.Models;
using BrewView.DatabaseModels.Vinmonopol;
using Microsoft.EntityFrameworkCore;

namespace BrewView.DatabaseModels
{
    public class BrewContext : DbContext
    {
        public BrewContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User.User> Users { get; set; }
        public DbSet<Brew> Brews { get; set; }
        public DbSet<UserBrew> UserBrews { get; set; }
        public DbSet<AlcoholicEntity> AlcoholicEntities { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Grape> Grapes { get; set; }
        public DbSet<GrapeAlcoholicEntity> GrapeBrews { get; set; }
        public DbSet<FoodAlcoholicEntity> FoodBrews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserBrew>().HasKey(o => new {o.ProductId, Id = o.UserId});
            builder.Entity<FoodAlcoholicEntity>().HasKey(brew => new {brew.ProductId, Id = brew.FoodId});
            builder.Entity<GrapeAlcoholicEntity>().HasKey(brew => new {brew.ProductId, Id = brew.GrapeId});

            base.OnModelCreating(builder);
        }
    }
}