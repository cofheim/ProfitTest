using Microsoft.Extensions.Logging;
using ProfitTest.Application.Interfaces;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Contracts.Messages;
using ProfitTest.Domain.Models;

namespace ProfitTest.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IKafkaProducer<ProductCreatedMessage> _createProducer;
        private readonly IKafkaProducer<ProductUpdatedMessage> _updateProducer;
        private readonly IKafkaProducer<ProductDeletedMessage> _deleteProducer;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IKafkaProducer<ProductCreatedMessage> createProducer,
            IKafkaProducer<ProductUpdatedMessage> updateProducer,
            IKafkaProducer<ProductDeletedMessage> deleteProducer,
            IProductRepository productRepository,
            ILogger<ProductService> logger)
        {
            _createProducer = createProducer ?? throw new ArgumentNullException(nameof(createProducer));
            _updateProducer = updateProducer ?? throw new ArgumentNullException(nameof(updateProducer));
            _deleteProducer = deleteProducer ?? throw new ArgumentNullException(nameof(deleteProducer));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(bool Success, string Error)> CreateProductAsync(
            string name,
            decimal price,
            DateTime validFrom,
            DateTime? validTo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return (false, "Название товара не может быть пустым");

                if (price <= 0)
                    return (false, "Цена должна быть больше нуля");

                if (validTo.HasValue && validFrom >= validTo.Value)
                    return (false, "Дата начала действия цены должна быть раньше даты окончания");

                var message = new ProductCreatedMessage(
                    name,
                    price,
                    validFrom,
                    validTo);

                await _createProducer.ProduceAsync(message, CancellationToken.None);
                _logger.LogInformation("Сообщение о создании товара отправлено: {Name}", name);

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке сообщения о создании товара");
                return (false, "Не удалось создать товар");
            }
        }

        public async Task<(bool Success, string Error)> UpdateProductAsync(
            Guid id,
            string name,
            decimal price,
            DateTime validFrom,
            DateTime? validTo)
        {
            try
            {
                if (id == Guid.Empty)
                    return (false, "Идентификатор товара не может быть пустым");

                if (string.IsNullOrWhiteSpace(name))
                    return (false, "Название товара не может быть пустым");

                if (price <= 0)
                    return (false, "Цена должна быть больше нуля");

                if (validTo.HasValue && validFrom >= validTo.Value)
                    return (false, "Дата начала действия цены должна быть раньше даты окончания");

                var message = new ProductUpdatedMessage(
                    id,
                    name,
                    price,
                    validFrom,
                    validTo);

                await _updateProducer.ProduceAsync(message, CancellationToken.None);
                _logger.LogInformation("Сообщение об обновлении товара отправлено: {Id}", id);

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке сообщения об обновлении товара");
                return (false, "Не удалось обновить товар");
            }
        }

        public async Task<(bool Success, string Error)> DeleteProductAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return (false, "Идентификатор товара не может быть пустым");

                var message = new ProductDeletedMessage(id);
                await _deleteProducer.ProduceAsync(message, CancellationToken.None);
                _logger.LogInformation("Сообщение об удалении товара отправлено: {Id}", id);

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке сообщения об удалении товара");
                return (false, "Не удалось удалить товар");
            }
        }

        public async Task<(bool Success, List<Product>? Products, string Error)> SearchByNameAsync(string nameQuery)
        {
            try
            {
                var products = await _productRepository.SearchByNameAsync(nameQuery);
                return (true, products, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске товаров по названию");
                return (false, null, "Произошла ошибка при поиске товаров");
            }
        }

        public async Task<(bool Success, List<Product>? Products, string Error)> FilterByPeriodAsync(DateTime start, DateTime? end)
        {
            try
            {
                var products = await _productRepository.FilterByPeriodAsync(start, end);
                return (true, products, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при фильтрации товаров по периоду");
                return (false, null, "Произошла ошибка при фильтрации товаров");
            }
        }
    }
}
