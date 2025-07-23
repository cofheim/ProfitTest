using Microsoft.Extensions.Logging;
using ProfitTest.Application.Interfaces;
using ProfitTest.Application.Interfaces.Messaging;
using ProfitTest.Contracts.Messages;
using ProfitTest.Domain.Models;

namespace ProfitTest.Application.Handlers.Products
{
    public class ProductMessageHandler : IMessageHandler<ProductCreatedMessage>,
    IMessageHandler<ProductUpdatedMessage>,
    IMessageHandler<ProductDeletedMessage>

    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductMessageHandler> _logger;

        public ProductMessageHandler(
            IProductRepository repository,
            ILogger<ProductMessageHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // создание товара
        public async Task HandleAsync(ProductCreatedMessage message, CancellationToken cancellationToken)
        {
            try
            {
                var (product, error) = Product.Create(
                    message.Name,
                    message.Price,
                    message.PriceValidFrom,
                    message.PriceValidTo
                );

                if (product == null)
                    throw new InvalidOperationException($"Не удалось создать товар: {error}");

                await _repository.AddAsync(product);
                _logger.LogInformation("Товар создан: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке сообщения о создании товара");
                throw;
            }
        }

        // обновление товара
        public async Task HandleAsync(ProductUpdatedMessage message, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _repository.GetByIdAsync(message.Id);
                if (product == null)
                    throw new InvalidOperationException($"Товар с ID {message.Id} не найден");

                var (success, error) = product.Update(
                    message.Name,
                    message.Price,
                    message.PriceValidFrom,
                    message.PriceValidTo
                );

                if (!success)
                    throw new InvalidOperationException($"Не удалось обновить товар: {error}");

                await _repository.UpdateAsync(product);
                _logger.LogInformation("Товар обновлен: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке сообщения об обновлении товара");
                throw;
            }
        }

        // удаление товара
        public async Task HandleAsync(ProductDeletedMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteAsync(message.Id);
                _logger.LogInformation("Товар удален: {ProductId}", message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке сообщения об удалении товара");
                throw;
            }
        }
    }
}
