using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProfitTest.Persistence.Entities;

namespace ProfitTest.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.PriceValidFrom).IsRequired();
            builder.Property(p => p.PriceValidTo).IsRequired(false);
            builder.Property(p => p.CreatedAt).IsRequired().HasDefaultValue(DateTime.UtcNow);
            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => new { p.PriceValidFrom, p.PriceValidTo });
        }


    }
}
