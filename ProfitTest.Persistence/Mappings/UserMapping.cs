using ProfitTest.Domain.Models;
using ProfitTest.Persistence.Entities;

namespace ProfitTest.Persistence.Mappings
{
    public static class UserMapping
    {
        // Из домена в сущность
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

        // Из сущности в домен
        public static User ToDomain(this UserEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var result = User.Create(
                entity.UserName,
                entity.PasswordHash,
                entity.Id,
                entity.CreatedAt,
                entity.LastLoginAt
            );

            if (result.User == null)
                throw new InvalidOperationException($"Невозможно преобразовать сущность в доменную модель: {result.Error}");

            return result.User;
        }
    }
}
