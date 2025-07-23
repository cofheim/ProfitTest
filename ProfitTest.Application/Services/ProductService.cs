using ProfitTest.Application.Interfaces;
using ProfitTest.Domain.Models;

namespace ProfitTest.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // создание товара
        public async Task<(bool Success, string Error)> CreateProductAsync(string name, decimal price, DateTime validFrom, DateTime? validTo)
        {
            // создаем товар фабричным методом
            var (product, error) = Product.Create(name, price, validFrom, validTo);
            if (product == null)
                return (false, error);

            await _repository.AddAsync(product);
            return (true, string.Empty);
        }

        // обновление товара
        public async Task<(bool Success, string Error)> UpdateProductAsync(Guid id, string name, decimal price, DateTime validFrom, DateTime? validTo)
        {
            // находим товар по id, проверяем его существование
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return (false, $"Товар с ID {id} не найден");

            // обновляем товар
            var (success, error) = product.Update(name, price, validFrom, validTo);
            if (!success)
                return (false, error);

            await _repository.UpdateAsync(product);
            return (true, string.Empty);
        }

        // удаление товара
        public async Task<(bool Success, string Error)> DeleteProductAsync(Guid id)
        {
            // находим товар по id, проверяем его существование
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return (false, $"Товар с ID {id} не найден");

            // удаляем товар если нашли
            await _repository.DeleteAsync(id);
            return (true, string.Empty);
        }
    }
}
