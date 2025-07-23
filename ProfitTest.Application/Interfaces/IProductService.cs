namespace ProfitTest.Application.Interfaces
{
    public interface IProductService
    {
        Task<(bool Success, string Error)> CreateProductAsync(string name, decimal price, DateTime validFrom, DateTime? validTo);
        Task<(bool Success, string Error)> UpdateProductAsync(Guid id, string name, decimal price, DateTime validFrom, DateTime? validTo);
        Task<(bool Success, string Error)> DeleteProductAsync(Guid id);
    }
}
