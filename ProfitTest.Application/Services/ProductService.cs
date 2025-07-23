using ProfitTest.Application.Interfaces;
using ProfitTest.Domain.Models;

namespace ProfitTest.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // �������� ������
        public async Task<(bool Success, string Error)> CreateProductAsync(string name, decimal price, DateTime validFrom, DateTime? validTo)
        {
            // ������� ����� ��������� �������
            var (product, error) = Product.Create(name, price, validFrom, validTo);
            if (product == null)
                return (false, error);

            await _repository.AddAsync(product);
            return (true, string.Empty);
        }

        // ���������� ������
        public async Task<(bool Success, string Error)> UpdateProductAsync(Guid id, string name, decimal price, DateTime validFrom, DateTime? validTo)
        {
            // ������� ����� �� id, ��������� ��� �������������
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return (false, $"����� � ID {id} �� ������");

            // ��������� �����
            var (success, error) = product.Update(name, price, validFrom, validTo);
            if (!success)
                return (false, error);

            await _repository.UpdateAsync(product);
            return (true, string.Empty);
        }

        // �������� ������
        public async Task<(bool Success, string Error)> DeleteProductAsync(Guid id)
        {
            // ������� ����� �� id, ��������� ��� �������������
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return (false, $"����� � ID {id} �� ������");

            // ������� ����� ���� �����
            await _repository.DeleteAsync(id);
            return (true, string.Empty);
        }
    }
}
