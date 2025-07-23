using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfitTest.Persistence.Entities;

namespace ProfitTest.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.PriceValidFrom).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.HasIndex(x => x.Name).HasDatabaseName("IX_Products_Name");
            builder.HasIndex(x => new { x.PriceValidFrom, x.PriceValidTo }).HasDatabaseName("IX_Products_PriceValidPeriod");
        }
    }
}
