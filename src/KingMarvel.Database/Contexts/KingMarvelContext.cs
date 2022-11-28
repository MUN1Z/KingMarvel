using KingMarvel.Database.Extensions;
using KingMarvel.Database.Mappings;
using KingMarvel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KingMarvel.Database.Contexts
{
    public class KingMarvelContext : DbContext
    {
        public DbSet<Character> Character { get; set; }

        public KingMarvelContext(DbContextOptions<KingMarvelContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new CharacterMapping());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries())
                if (entry.Entity is BaseEntity baseEntity)
                    if (entry.State == EntityState.Added)
                        baseEntity.CreatedAt = DateTime.UtcNow;
                    else if (entry.State == EntityState.Modified)
                        baseEntity.UpdatedAt = DateTime.UtcNow;

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
