using System.ComponentModel.DataAnnotations;

namespace ProfitTest.Contracts.Requests.Products
{
    /// <summary>
    /// запрос на создание товара
    /// </summary>
    public record CreateProductRequest
    {
        [Required(ErrorMessage = "Название товара обязательно")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина названия должна быть от 1 до 200 символов")]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "Цена обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal Price { get; init; }

        [Required(ErrorMessage = "Дата начала действия цены обязательна")]
        public DateTime PriceValidFrom { get; init; }

        public DateTime? PriceValidTo { get; init; }
    }
} 