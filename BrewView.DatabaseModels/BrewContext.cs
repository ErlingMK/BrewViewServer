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
        public DbSet<ProductGtin> ProductGtins { get; set; }
        public DbSet<UserBrew> UserBrews { get; set; }
        public DbSet<AlcoholicEntity> AlcoholicEntities { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Grape> Grapes { get; set; }
        public DbSet<GrapeAlcoholicEntity> GrapeBrews { get; set; }
        public DbSet<FoodAlcoholicEntity> FoodBrews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AlcoholicEntity>()
                .OwnsMany(a => a.Prices)
                .HasOne(p => p.AlcoholicEntity);

            builder.Entity<UserBrew>()
                .HasKey(o => new {o.ProductId, Id = o.UserId});

            builder.Entity<UserBrew>()
                .OwnsMany(ub => ub.Notes)
                .HasOne(n => n.UserBrew);

            builder.Entity<FoodAlcoholicEntity>()
                .HasKey(brew => new {brew.ProductId, Id = brew.FoodId});

            builder.Entity<FoodAlcoholicEntity>()
                .HasOne(f => f.AlcoholicEntity)
                .WithMany(a => a.FoodAlcoholicEntities)
                .HasForeignKey(f => f.ProductId);

            builder.Entity<FoodAlcoholicEntity>()
                .HasOne(f => f.Food)
                .WithMany(f => f.FoodAlcoholicEntities)
                .HasForeignKey(f => f.FoodId);

            builder.Entity<GrapeAlcoholicEntity>()
                .HasOne(f => f.AlcoholicEntity)
                .WithMany(a => a.GrapeAlcoholicEntities)
                .HasForeignKey(f => f.ProductId);

            builder.Entity<GrapeAlcoholicEntity>()
                .HasOne(f => f.Grape)
                .WithMany(f => f.GrapeAlcoholicEntities)
                .HasForeignKey(f => f.GrapeId);

            builder.Entity<GrapeAlcoholicEntity>()
                .HasKey(brew => new {brew.ProductId, Id = brew.GrapeId});

            base.OnModelCreating(builder);
        }
    }
}