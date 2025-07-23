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
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IKafkaProducer<ProductCreatedMessage> createProducer,
            IKafkaProducer<ProductUpdatedMessage> updateProducer,
            IKafkaProducer<ProductDeletedMessage> deleteProducer,
            ILogger<ProductService> logger)
        {
            _createProducer = createProducer;
            _updateProducer = updateProducer;
            _deleteProducer = deleteProducer;
            _logger = logger;
        }

        // создание товара
        public async Task<(bool Success, string Error)> CreateProductAsync(string name,
            decimal price,
            DateTime validFrom,
            DateTime? validTo)
        {
            try
            {
                var message = new ProductCreatedMessage(
                    name,
                    price,
                    validFrom,
                    validTo);

                await _createProducer.ProduceAsync(message, CancellationToken.None);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке сообщения о создании товара");
                return (false, "Не удалось создать товар");
            }
        }

        // обновление товара
        public async Task<(bool Success, string Error)> UpdateProductAsync(
            Guid id,
            string name,
            decimal price,
            DateTime validFrom,
            DateTime? validTo)
        {
            try
            {
                var message = new ProductUpdatedMessage(
                    id,
                    name,
                    price,
                    validFrom,
                    validTo);

                await _updateProducer.ProduceAsync(message, CancellationToken.None);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке сообщения об обновлении товара");
                return (false, "Не удалось обновить товар");
            }
        }

        // удаление товара
        public async Task<(bool Success, string Error)> DeleteProductAsync(Guid id)
        {
            try
            {
                var message = new ProductDeletedMessage(id);
                await _deleteProducer.ProduceAsync(message, CancellationToken.None);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке сообщения об удалении товара");
                return (false, "Не удалось удалить товар");
            }
        }
    }
}
