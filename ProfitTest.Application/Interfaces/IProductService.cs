using ProfitTest.Domain.Models;

namespace ProfitTest.Application.Interfaces
{
    public interface IProductService
    {
        Task<(bool Success, string Error)> CreateProductAsync(string name, decimal price, DateTime validFrom, DateTime? validTo);
        Task<(bool Success, string Error)> UpdateProductAsync(Guid id, string name, decimal price, DateTime validFrom, DateTime? validTo);
        Task<(bool Success, string Error)> DeleteProductAsync(Guid id);
        Task<(bool Success, List<Product>? Products, string Error)> SearchByNameAsync(string nameQuery);
        Task<(bool Success, List<Product>? Products, string Error)> FilterByPeriodAsync(DateTime start, DateTime? end);
        Task<List<Product>> GetAllAsync();
    }
}
