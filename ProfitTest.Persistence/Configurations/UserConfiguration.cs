using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProfitTest.Persistence.Entities;

namespace ProfitTest.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.UserName).IsRequired().HasMaxLength(25);
            builder.Property(p => p.PasswordHash).IsRequired();
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.LastLoginAt).IsRequired(false);
            builder.HasIndex(p => p.UserName).IsUnique().HasDatabaseName("IX_UserName");
        }
    }
}
