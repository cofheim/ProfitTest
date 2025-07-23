using ProfitTest.Domain.Models;

namespace ProfitTest.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> SearchByNameAsync(string nameQuery);
        Task<List<Product>> FilterByPeriodAsync(DateTime start, DateTime? end);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
    }
}
