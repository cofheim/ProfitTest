namespace ProfitTest.Domain.Models
{
    public class Product
    {
        private Product(Guid id, string name, decimal price, DateTime priceValidFrom, DateTime? priceValidTo = null)
        {
            Id = id;
            Name = name;
            Price = price;
            PriceValidFrom = priceValidFrom;
            PriceValidTo = priceValidTo;
            CreatedAt = DateTime.UtcNow;
        }

        private Product(string name, decimal price, DateTime priceValidFrom, DateTime? priceValidTo = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            PriceValidFrom = priceValidFrom;
            PriceValidTo = priceValidTo;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public string Name { get; private set; }
        public decimal Price { get; private set; } 
        public DateTime PriceValidFrom { get; private set; }
        public DateTime? PriceValidTo { get; private set; } 
        public DateTime CreatedAt { get; }

        public static (Product? Product, string Error) Create(
            Guid? id,
            string name,
            decimal price,
            DateTime priceValidFrom,
            DateTime? priceValidTo = null)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                error = "Название товара не может быть пустым";
                return (null, error);
            }

            if (price <= 0)
            {
                error = "Цена товара должна быть больше нуля";
                return (null, error);
            }

            if (priceValidTo.HasValue && priceValidFrom >= priceValidTo.Value)
            {
                error = "Дата начала действия цены должна быть раньше даты окончания";
                return (null, error);
            }

            var product = id.HasValue
                ? new Product(id.Value, name, price, priceValidFrom, priceValidTo)
                : new Product(name, price, priceValidFrom, priceValidTo);
            return (product, error);
        }

        public (bool Success, string Error) Update(
            string name,
            decimal price,
            DateTime priceValidFrom,
            DateTime? priceValidTo = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Имя не может быть пустым");

            if (price <= 0)
                return (false, "Цена товара должна быть больше нуля");

            if (priceValidTo.HasValue && priceValidFrom >= priceValidTo.Value)
                return (false, "Дата начала действия цены должна быть раньше даты окончания");

            Name = name;
            Price = price;
            PriceValidFrom = priceValidFrom;
            PriceValidTo = priceValidTo;

            return (true, string.Empty);
        }

        public bool IsPriceActiveAt(DateTime date)
        {
            var isAfterStart = date >= PriceValidFrom;
            var isBeforeEnd = !PriceValidTo.HasValue || date <= PriceValidTo.Value;

            return isAfterStart && isBeforeEnd;
        }
    }
}
