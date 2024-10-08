using Credit.Domain.Entities;
using Credit.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Credit.Infrastructure.Persistence
{
    public class MainDbContext : DbContext, IMainDbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var item in ChangeTracker.Entries<CommonEntity>().AsEnumerable())
            {
                if (item.State == EntityState.Added)
                {
                    item.Entity.CreatedAt = DateTime.Now;
                }
                else if (item.State == EntityState.Modified)
                {
                    item.Entity.UpdatedAt = DateTime.Now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        // Add DbSet for each entity
        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<CreditEntity> Credits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<PersonEntity>()
                .HasIndex(u => u.PersonalNumber)
                .IsUnique();

            modelBuilder.Entity<CreditEntity>()
                .HasOne(p => p.Person)
                .WithMany(b => b.Credits)
                .HasForeignKey(p => p.PersonId);
        }
    }
}
