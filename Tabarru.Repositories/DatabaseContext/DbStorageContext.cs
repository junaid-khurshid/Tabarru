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

            builder.Entity<PackageDetails>()
                .Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Entity<PackageDetails>()
                .Property(p => p.FeaturesJson)
                .HasDefaultValue("[]"); // Default empty array

            builder.Entity<Template>()
                .Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Entity<Mode>()
                .HasOne(m => m.Template)
                .WithMany(t => t.Modes)
                .HasForeignKey(m => m.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Mode>()
                .HasOne(m => m.Campaign)
                .WithMany()
                .HasForeignKey(m => m.CampaignId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Charity> Charity { get; set; }
        public DbSet<CharityKycDetails> CharityKycDetails { get; set; }
        public DbSet<CharityKycDocuments> CharityKycDocuments { get; set; }
        public DbSet<EmailVerificationDetails> EmailVerificationDetails { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<PackageDetails> PackageDetails { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Mode> Modes { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; } 
        public DbSet<GiftAidDetail> GiftAidDetails { get; set; }
        public DbSet<RecurringPaymentDetail> RecurringPaymentDetails { get; set; }

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