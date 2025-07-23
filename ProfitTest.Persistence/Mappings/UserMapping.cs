using ProfitTest.Domain.Models;
using ProfitTest.Persistence.Entities;

namespace ProfitTest.Persistence.Mappings
{
    public static class UserMapping
    {
        // из доменой в сущность
        public static UserEntity ToEntity(this User domain)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            return new UserEntity
            {
                Id = domain.Id,
                UserName = domain.UserName,
                PasswordHash = domain.PasswordHash,
                CreatedAt = domain.CreatedAt,
                LastLoginAt = domain.LastLoginAt
            };
        }

        // из сущности в доменную
        public static User ToDomain(this UserEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var result = User.Create(
                entity.UserName,
                entity.PasswordHash
            );

            if (result.User == null)
                throw new InvalidOperationException($"Невозможно преобразовать сущность в доменную модель: {result.Error}");

            var user = result.User;

            if (entity.LastLoginAt.HasValue)
                user.UpdateLastLogin();

            return user;
        }
    }
}
