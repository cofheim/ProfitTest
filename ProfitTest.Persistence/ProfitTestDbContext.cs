using Microsoft.EntityFrameworkCore;
using ProfitTest.Persistence.Configurations;
using ProfitTest.Persistence.Entities;

namespace ProfitTest.Persistence
{
    public class ProfitTestDbContext : DbContext
    {
        public ProfitTestDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<ProductEntity> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
