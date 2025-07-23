using ProfitTest.Domain.Models;
using ProfitTest.Persistence.Entities;

namespace ProfitTest.Persistence.Mappings
{
    public static class ProductMapping
    {
        public static ProductEntity ToEntity(this Product domain)
        {
            if (domain == null)
                throw new ArgumentNullException(nameof(domain));

            return new ProductEntity
            {
                Id = domain.Id,
                Name = domain.Name,
                Price = domain.Price,
                PriceValidFrom = domain.PriceValidFrom,
                PriceValidTo = domain.PriceValidTo,
                CreatedAt = domain.CreatedAt
            };
        }

        public static Product ToDomain(this ProductEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var result = Product.Create(
                entity.Name,
                entity.Price,
                entity.PriceValidFrom,
                entity.PriceValidTo
            );

            if (result.Product == null)
                throw new InvalidOperationException($"Невозможно преобразовать сущность в доменную модель: {result.Error}");

            return result.Product;
        }
    }
}
