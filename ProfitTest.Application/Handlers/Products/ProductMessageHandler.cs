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

        // �������� ������
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
                    throw new InvalidOperationException($"�� ������� ������� �����: {error}");

                await _repository.AddAsync(product);
                _logger.LogInformation("����� ������: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� ��������� ��������� � �������� ������");
                throw;
            }
        }

        // ���������� ������
        public async Task HandleAsync(ProductUpdatedMessage message, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _repository.GetByIdAsync(message.Id);
                if (product == null)
                    throw new InvalidOperationException($"����� � ID {message.Id} �� ������");

                var (success, error) = product.Update(
                    message.Name,
                    message.Price,
                    message.PriceValidFrom,
                    message.PriceValidTo
                );

                if (!success)
                    throw new InvalidOperationException($"�� ������� �������� �����: {error}");

                await _repository.UpdateAsync(product);
                _logger.LogInformation("����� ��������: {ProductId}", product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� ��������� ��������� �� ���������� ������");
                throw;
            }
        }

        // �������� ������
        public async Task HandleAsync(ProductDeletedMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteAsync(message.Id);
                _logger.LogInformation("����� ������: {ProductId}", message.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ ��� ��������� ��������� �� �������� ������");
                throw;
            }
        }
    }
}
