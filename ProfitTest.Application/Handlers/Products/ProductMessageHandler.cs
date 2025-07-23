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
        private readonly IProductService _productService;

        public ProductMessageHandler(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        // создание товара
        public async Task HandleAsync(ProductCreatedMessage message, CancellationToken cancellationToken)
        {
            var (success, error) = await _productService.CreateProductAsync(
                message.Name,
                message.Price,
                message.PriceValidFrom,
                message.PriceValidTo
            );

            if (!success)
                throw new InvalidOperationException($"Ќе удалось создать товар: {error}");
        }
        
        // обновление товара
        public async Task HandleAsync(ProductUpdatedMessage message, CancellationToken cancellationToken)
        {
            var (success, error) = await _productService.UpdateProductAsync(
                message.Id,
                message.Name,
                message.Price,
                message.PriceValidFrom,
                message.PriceValidTo
            );

            if (!success)
                throw new InvalidOperationException($"Ќе удалось обновить товар: {error}");
        }

        // удаление товара
        public async Task HandleAsync(ProductDeletedMessage message, CancellationToken cancellationToken)
        {
            var (success, error) = await _productService.DeleteProductAsync(message.Id);

            if (!success)
                throw new InvalidOperationException($"Ќе удалось удалить товар: {error}");
        }
    }
}
