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
            base.OnModelCreating(builder);
        }

        public DbSet<Charity> Charity { get; set; }
    }
}
