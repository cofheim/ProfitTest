using Microsoft.EntityFrameworkCore;
using ProfitTest.Application.Interfaces;
using ProfitTest.Domain.Models;
using ProfitTest.Persistence.Mappings;

namespace ProfitTest.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProfitTestDbContext _context;

        public ProductRepository(ProfitTestDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // получение по id 
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return entity?.ToDomain();
        }

        // получение всех
        public async Task<List<Product>> GetAllAsync()
        {
            var entities = await _context.Products.ToListAsync();
            return entities.Select(x => x.ToDomain()).ToList();
        }

        // поиск по имени
        public async Task<List<Product>> SearchByNameAsync(string nameQuery)
        {
            if (string.IsNullOrWhiteSpace(nameQuery))
                throw new ArgumentException("Поисковый запрос не может быть пустым", nameof(nameQuery));

            var entities = await _context.Products
                .Where(x => x.Name.ToLower().Contains(nameQuery.ToLower()))
                .ToListAsync();

            return entities.Select(x => x.ToDomain()).ToList();
        }

        // фильтрация по периоду
        public async Task<List<Product>> FilterByPeriodAsync(DateTime start, DateTime? end)
        {
            var query = _context.Products.AsQueryable();

            // Логика для поиска пересекающихся интервалов:
            // Продукт активен в искомом периоде, если:
            // 1. Его дата начала <= Дате конца фильтра
            // 2. Его дата конца >= Дате начала фильтра

            // Устанавливаем конец дня для конечной даты фильтра, чтобы включить весь день.
            var effectiveEnd = end?.Date.AddDays(1).AddTicks(-1);

            query = query.Where(p =>
                (!effectiveEnd.HasValue || p.PriceValidFrom <= effectiveEnd.Value) &&
                (p.PriceValidTo == null || p.PriceValidTo >= start.Date)
            );

            // материализуем запрос и сохраняем в "entities"
            var entities = await query.ToListAsync();
            return entities.Select(x => x.ToDomain()).ToList(); // преобразуем сущности в доменные модели и материализуем
        }

        // создание
        public async Task AddAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var entity = product.ToEntity();
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        //  обновление
        public async Task UpdateAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var updated = await _context.Products
                .Where(x => x.Id == product.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Name, product.Name)
                    .SetProperty(x => x.Price, product.Price)
                    .SetProperty(x => x.PriceValidFrom, product.PriceValidFrom)
                    .SetProperty(x => x.PriceValidTo, product.PriceValidTo));

            if (updated == 0)
                throw new InvalidOperationException($"Товар с ID {product.Id} не найден");
        }

        // удаление
        public async Task DeleteAsync(Guid id)
        {
            var deleted = await _context.Products
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            if (deleted == 0)
                throw new InvalidOperationException($"Товар с ID {id} не найден");
        }
    }
}
