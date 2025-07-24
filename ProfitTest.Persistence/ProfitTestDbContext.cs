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

        // Пустой конструктор для миграций
        public ProfitTestDbContext()
        {
        }

        // Модели таблиц
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }

        // Настройка конфигурации
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

        // Конфигурация для миграций
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID=postgres;Password=Ellenoize2002;Host=localhost;Port=5432;Database=profit_db;");
            }
        }
    }
}
