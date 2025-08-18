using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.DatabaseContext
{
    public class DbStorageContext : DbContext
    {
        public DbStorageContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.AddModelCreatingProfile();

            base.OnModelCreating(builder);
        }

        public DbSet<Charity> Charity { get; set; }
        public DbSet<EmailVerificationDetails> EmailVerificationDetails { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<PackageDetails> PackageDetails { get; set; }


        public override int SaveChanges()
        {
            ChangeTracker.DetectChangesAndUpdate();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChangesAndUpdate();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChangesAndUpdate();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChangesAndUpdate();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}