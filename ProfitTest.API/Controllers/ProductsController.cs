using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfitTest.Application.Interfaces;
using ProfitTest.Contracts.Common;
using ProfitTest.Contracts.Requests.Products;
using ProfitTest.Contracts.Responses.Products;

namespace ProfitTest.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// создание товара
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var priceValidFrom = DateTime.SpecifyKind(request.PriceValidFrom, DateTimeKind.Utc);
            var priceValidTo = request.PriceValidTo.HasValue ? DateTime.SpecifyKind(request.PriceValidTo.Value, DateTimeKind.Utc) : (DateTime?)null;

            var (success, error) = await _productService.CreateProductAsync(
                request.Name,
                request.Price,
                priceValidFrom,
                priceValidTo);

            if (!success)
                return BadRequest(new ErrorResponse(error));

            return Ok();
        }

        /// <summary>
        /// обновление товара
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            if (id != request.Id)
                return BadRequest(new ErrorResponse("ID в URL не соответствует ID в запросе"));

            var priceValidFrom = DateTime.SpecifyKind(request.PriceValidFrom, DateTimeKind.Utc);
            var priceValidTo = request.PriceValidTo.HasValue ? DateTime.SpecifyKind(request.PriceValidTo.Value, DateTimeKind.Utc) : (DateTime?)null;

            var (success, error) = await _productService.UpdateProductAsync(
                request.Id,
                request.Name,
                request.Price,
                priceValidFrom,
                priceValidTo);

            if (!success)
                return BadRequest(new ErrorResponse(error));

            return Ok();
        }

        /// <summary>
        /// удаление товара
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var (success, error) = await _productService.DeleteProductAsync(id);

            if (!success)
                return BadRequest(new ErrorResponse(error));

            return Ok();
        }

        /// <summary>
        /// поиск товаров по названию
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ProductListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchByName([FromQuery] string nameQuery)
        {
            var (success, products, error) = await _productService.SearchByNameAsync(nameQuery);

            if (!success)
            {
                _logger.LogError("Ошибка при поиске по названию: {Error}", error);
                return BadRequest(new ErrorResponse(error));
            }

            var response = new ProductListResponse
            {
                Items = products.Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    PriceValidFrom = p.PriceValidFrom.ToLocalTime(),
                    PriceValidTo = p.PriceValidTo?.ToLocalTime(),
                    CreatedAt = p.CreatedAt.ToLocalTime(),
                    IsPriceActive = p.IsPriceActiveAt(DateTime.UtcNow)
                }).ToList(),
                AppliedNameFilter = nameQuery
            };

            return Ok(response);
        }

        /// <summary>
        /// фильтрация товаров по периоду
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(typeof(ProductListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> FilterByPeriod(
            [FromQuery] DateTime? start,
            [FromQuery] DateTime? end)
        {
            var (success, products, error) = await _productService.FilterByPeriodAsync(
                start ?? DateTime.UtcNow,
                end);

            if (!success)
            {
                _logger.LogError("Ошибка при фильтрации по периоду: {Error}", error);
                return BadRequest(new ErrorResponse(error));
            }

            var response = new ProductListResponse
            {
                Items = products.Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    PriceValidFrom = p.PriceValidFrom.ToLocalTime(),
                    PriceValidTo = p.PriceValidTo?.ToLocalTime(),
                    CreatedAt = p.CreatedAt.ToLocalTime(),
                    IsPriceActive = p.IsPriceActiveAt(DateTime.UtcNow)
                }).ToList(),
                AppliedPeriodStart = start,
                AppliedPeriodEnd = end
            };

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProductListResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            var response = new ProductListResponse
            {
                Items = products.Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    PriceValidFrom = p.PriceValidFrom.ToLocalTime(),
                    PriceValidTo = p.PriceValidTo?.ToLocalTime(),
                    CreatedAt = p.CreatedAt.ToLocalTime(),
                    IsPriceActive = p.IsPriceActiveAt(DateTime.UtcNow)
                }).ToList()
            };
            return Ok(response);
        }
    }
}
